using IPL.SOLID.API.Infrastructure.SQLite.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IPL.SOLID.API.Infrastructure.SQLite.Outbox;

public sealed class OutboxBackgroundService(
    IServiceScopeFactory scopeFactory,
    IOptions<OutboxOptions> options,
    ILogger<OutboxBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await using var scope = scopeFactory.CreateAsyncScope();

                var context = scope.ServiceProvider.GetRequiredService<SolidWriteContext>();

                await RemoveExpiredResources(context, stoppingToken);

                await scope.ServiceProvider
                    .GetRequiredService<OutboxProcessor>()
                    .ProcessAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Outbox cycle failed; it will be retried.");
            }

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(options.Value.PollIntervalSeconds), stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
        }
    }

    private static async Task RemoveExpiredResources(SolidWriteContext context, CancellationToken token)
    {
        // SQLite cannot translate DateTimeOffset comparisons. Filter dates in memory after loading expirable resources.
        var now = DateTimeOffset.UtcNow;

        var records = await context.DataRecords
            .Where(record => record.Expiration != null)
            .ToListAsync(token);

        var subscriptions = await context.Subscriptions
            .Where(subscription => subscription.Expiration != null)
            .ToListAsync(token);

        context.RemoveRange(records.Where(record => record.Expiration < now).Take(100));

        context.RemoveRange(subscriptions.Where(subscription => subscription.Expiration < now).Take(100));

        if (context.ChangeTracker.HasChanges())
        {
            await context.SaveChangesAsync(token);
        }

        context.ChangeTracker.Clear();
    }
}

