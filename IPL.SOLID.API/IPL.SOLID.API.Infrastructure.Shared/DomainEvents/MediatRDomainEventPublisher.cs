using IPL.SOLID.API.Application.DomainEvents;
using IPL.SOLID.API.Domain.DomainEvents;
using MediatR;

namespace IPL.SOLID.API.Infrastructure.Shared.DomainEvents;

public sealed class MediatRDomainEventPublisher(IPublisher publisher) : IDomainEventPublisher
{
    public Task PublishAsync(IDomainEvent domainEvent, CancellationToken token = default) =>
        publisher.Publish(new DomainEventNotification(domainEvent), token);
}
