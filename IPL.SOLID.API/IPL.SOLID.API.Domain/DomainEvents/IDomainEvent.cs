using IPL.SOLID.API.Domain.Entities;

namespace IPL.SOLID.API.Domain.DomainEvents;

public interface IDomainEvent
{
    public Guid Id { get; set; }

    public BaseEntity? OldState { get; set; }

    public BaseEntity? NewState { get; set; }

    public DateTime CreatedAt { get; set; }
}
