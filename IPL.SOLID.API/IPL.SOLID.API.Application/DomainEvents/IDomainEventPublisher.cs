using IPL.SOLID.API.Domain.DomainEvents;

namespace IPL.SOLID.API.Application.DomainEvents;

public interface IDomainEventPublisher
{
    Task PublishAsync(IDomainEvent domainEvent, CancellationToken token = default);
}
