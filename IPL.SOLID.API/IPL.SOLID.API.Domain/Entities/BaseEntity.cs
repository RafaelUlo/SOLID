using IPL.SOLID.API.Domain.DomainEvents;
using Krafted.Guards;

namespace IPL.SOLID.API.Domain.Entities;
public class BaseEntity
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public Guid Id { get; set; }
    public string ResourceName { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public BaseEntity(Guid id, string resourceName)
    {
        Guard.Against.EmptyOrWhiteSpace(resourceName);

        Id = id;
        ResourceName = resourceName;
    }

    public BaseEntity()
    {

    }

    public void ChangeResourceName(string resourceName)
    {
        Guard.Against.EmptyOrWhiteSpace(resourceName);

        ResourceName = resourceName;
    }

    protected void AddDomainEvent(IDomainEvent @event)
    {
        _domainEvents.Add(@event);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
