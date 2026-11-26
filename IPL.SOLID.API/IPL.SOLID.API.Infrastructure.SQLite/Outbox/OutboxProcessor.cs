using IPL.SOLID.API.Application.Notifications;
using IPL.SOLID.API.Infrastructure.SQLite.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;

namespace IPL.SOLID.API.Infrastructure.SQLite.Outbox;

public sealed class OutboxProcessor(
    SolidWriteContext context,
    IHttpClientFactory httpClients,
    IMqttNotificationSender mqtt,
    IOptions<OutboxOptions> options,
    ILogger<OutboxProcessor> logger)
{
    public async Task ProcessAsync(CancellationToken token)
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        var ids = await context.OutboxDeliveries
            .Where(delivery => delivery.DeliveredAt == null && delivery.NextAttemptAt <= now &&
                (delivery.LeaseUntil == null || delivery.LeaseUntil <= now))
            .OrderBy(delivery => delivery.NextAttemptAt)
            .Select(delivery => delivery.Id)
            .Take(options.Value.BatchSize)
            .ToListAsync(token);

        foreach (var id in ids)
        {
            token.ThrowIfCancellationRequested();

            await DeliverAsync(id, token);

            context.ChangeTracker.Clear();
        }
    }

    private async Task DeliverAsync(Guid id, CancellationToken token)
    {
        var leaseId = Guid.NewGuid();

        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        var attempt = new OutboxDeliveryAttempt { DeliveryId = id, StartedAt = DateTime.UtcNow };

        // The conditional update prevents another worker from claiming this delivery.
        await using (var transaction = await context.Database.BeginTransactionAsync(token))
        {
            var claimed = await context.OutboxDeliveries
                .Where(delivery => delivery.Id == id && delivery.DeliveredAt == null &&
                    delivery.NextAttemptAt <= now && (delivery.LeaseUntil == null || delivery.LeaseUntil <= now))
                .ExecuteUpdateAsync(update => update
                    .SetProperty(delivery => delivery.LeaseId, leaseId)
                    .SetProperty(delivery => delivery.LeaseUntil, now + options.Value.LeaseSeconds * 1000L)
                    .SetProperty(delivery => delivery.Attempts, delivery => delivery.Attempts + 1), token);

            if (claimed == 0)
            {
                return;
            }


            context.OutboxDeliveryAttempts.Add(attempt);

            await context.SaveChangesAsync(token);

            await transaction.CommitAsync(token);
        }

        var delivery = await context.OutboxDeliveries
            .Include(delivery => delivery.Message)
            .SingleAsync(delivery => delivery.Id == id, token);

        string? error = null;

        int? httpStatus = null;

        try
        {
            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(token);

            timeout.CancelAfter(TimeSpan.FromSeconds(options.Value.DeliveryTimeoutSeconds));

            var endpoint = new Uri(delivery.Endpoint, UriKind.Absolute);

            if (endpoint.Scheme is "http" or "https")
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
                {
                    Content = new StringContent(delivery.Message.Payload, Encoding.UTF8, "application/json")
                };

                request.Headers.Add("X-Solid-Event-Id", delivery.MessageId.ToString());
                request.Headers.Add("X-Solid-Delivery-Id", delivery.Id.ToString());

                using var result = await httpClients.CreateClient("SolidNotifications")
                    .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, timeout.Token);

                httpStatus = (int)result.StatusCode;

                if (!result.IsSuccessStatusCode)
                    throw new HttpRequestException($"HTTP notification returned {httpStatus}.");
            }
            else
            {
                await mqtt.SendAsync(delivery.Endpoint, delivery.Message.Topic, delivery.Message.Payload, timeout.Token);
            }
        }
        catch (OperationCanceledException) when (token.IsCancellationRequested)
        {
            // A new worker can recover this attempt after the lease expires.
            throw;
        }
        catch (Exception exception)
        {
            error = exception is OperationCanceledException
                ? "Delivery timed out."
                : exception.GetType().Name + ": " + exception.Message;

            if (error.Length > 2000)
            {
                error = error[..2000];
            }

            logger.LogWarning("Delivery {DeliveryId} failed on attempt {Attempt}.", id, delivery.Attempts);
        }

        var completedAt = DateTime.UtcNow;

        var retrySeconds = Math.Min(options.Value.MaxRetryDelaySeconds,
            Math.Pow(2, Math.Min(delivery.Attempts, 20)));

        var retryAt = DateTimeOffset.UtcNow
            .AddSeconds(retrySeconds)
            .ToUnixTimeMilliseconds();

        await using var completion = await context.Database.BeginTransactionAsync(token);

        var updated = await context.OutboxDeliveries
            .Where(item => item.Id == id && item.LeaseId == leaseId)
            .ExecuteUpdateAsync(update => update
                .SetProperty(item => item.DeliveredAt, error == null ? completedAt : (DateTime?)null)
                .SetProperty(item => item.LastError, error)
                .SetProperty(item => item.NextAttemptAt, retryAt)
                .SetProperty(item => item.LeaseId, (Guid?)null)
                .SetProperty(item => item.LeaseUntil, (long?)null), token);

        if (updated == 0)
        {
            return;

        }

        attempt.CompletedAt = completedAt;
        attempt.Succeeded = error == null;
        attempt.HttpStatusCode = httpStatus;
        attempt.Error = error;

        await context.SaveChangesAsync(token);

        await context.OutboxMessages
            .Where(message => message.Id == delivery.MessageId &&
                message.ProcessedAt == null &&
                !message.Deliveries.Any(item => item.DeliveredAt == null))
            .ExecuteUpdateAsync(update => update.SetProperty(message => message.ProcessedAt, completedAt), token);

        await completion.CommitAsync(token);
    }
}
