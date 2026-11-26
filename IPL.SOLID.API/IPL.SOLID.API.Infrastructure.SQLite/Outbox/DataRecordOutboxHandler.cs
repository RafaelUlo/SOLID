using IPL.SOLID.API.Application.DomainEvents;
using IPL.SOLID.API.Domain.DomainEvents.DateRecords;
using IPL.SOLID.API.Domain.Entities;
using IPL.SOLID.API.Domain.Enums;
using IPL.SOLID.API.Infrastructure.Shared.Serialization;
using IPL.SOLID.API.Infrastructure.SQLite.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IPL.SOLID.API.Infrastructure.SQLite.Outbox;

public sealed class DataRecordOutboxHandler(SolidWriteContext context) : INotificationHandler<DomainEventNotification>
{
    public async Task Handle(DomainEventNotification notification, CancellationToken token)
    {
        var domainEvent = notification.Event;

        var record = domainEvent switch
        {
            DataRecordCreatedEvent => domainEvent.NewState as DataRecord,
            DataRecordDeletedEvent => domainEvent.OldState as DataRecord,
            _ => null
        };

        if (record is null)
        {
            return;
        }

        if (context.OutboxMessages.Local.Any(message => message.Id == domainEvent.Id))
        {
            return;
        }

        var channel = await context.Channels
            .Include(channel => channel.Application)
            .SingleAsync(channel => channel.Id == record.ChannelId, token);

        var application = channel.Application
            ?? throw new InvalidOperationException("Channel has no application.");

        var channelPath = $"/api/solid/{Uri.EscapeDataString(application.ResourceName)}/{Uri.EscapeDataString(channel.ResourceName)}";

        var eventType = domainEvent is DataRecordCreatedEvent
            ? EventTypeEnum.Creation
            : EventTypeEnum.Deletion;

        var occurredAt = new DateTimeOffset(domainEvent.CreatedAt, TimeSpan.Zero);

        // Freeze recipients inside the same transaction as the resource mutation.
        // A later subscription deletion does not cancel already committed deliveries.
        var subscriptions = await context.Subscriptions
            .Where(subscription => subscription.ChannelId == channel.Id &&
                subscription.Evt == eventType)
            .ToListAsync(token);

        subscriptions = [.. subscriptions.Where(subscription =>
            !subscription.Expiration.HasValue || subscription.Expiration.Value >= occurredAt)];

        var resourcePath = channelPath + "/" + Uri.EscapeDataString(record.ResourceName);

        switch (domainEvent)
        {
            case DataRecordCreatedEvent created:
                created.Path = resourcePath;
                break;
            case DataRecordDeletedEvent deleted:
                deleted.Path = resourcePath;
                break;
        }

        var message = new OutboxMessage
        {
            Id = domainEvent.Id,
            CreatedAt = domainEvent.CreatedAt,
            Payload = DomainEventSerializer.Serialize(domainEvent),
            Topic = channelPath.TrimStart('/'),
            ProcessedAt = subscriptions.Count == 0 ? DateTime.UtcNow : null,
            Deliveries = [.. subscriptions.Select(subscription => new OutboxDelivery
            {
                Id = Guid.NewGuid(),
                SubscriptionId = subscription.Id,
                Endpoint = subscription.Endpoint,
                NextAttemptAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            })]
        };
        context.OutboxMessages.Add(message);
    }

}
