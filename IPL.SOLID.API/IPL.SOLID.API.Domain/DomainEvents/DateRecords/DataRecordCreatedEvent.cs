using IPL.SOLID.API.Domain.Entities;

namespace IPL.SOLID.API.Domain.DomainEvents.DateRecords;

public sealed class DataRecordCreatedEvent : IDomainEvent
{
    public Guid Id { get; set; }
    public BaseEntity? OldState { get; set; }
    public BaseEntity? NewState { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Event => "creation";
    public int Evt => 1;
    public string Path { get; set; } = string.Empty;

    internal DataRecordCreatedEvent(DataRecord? oldState, DataRecord newState)
    {
        ArgumentNullException.ThrowIfNull(newState);

        Id = Guid.NewGuid();
        OldState = oldState;
        NewState = newState;
        CreatedAt = DateTime.UtcNow;
    }
}
