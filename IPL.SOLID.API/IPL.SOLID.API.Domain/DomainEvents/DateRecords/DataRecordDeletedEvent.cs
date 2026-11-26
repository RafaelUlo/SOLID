using IPL.SOLID.API.Domain.Entities;

namespace IPL.SOLID.API.Domain.DomainEvents.DateRecords;

public sealed class DataRecordDeletedEvent : IDomainEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public BaseEntity? OldState { get; set; }
    public BaseEntity? NewState { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Event => "deletion";
    public int Evt => 2;
    public string Path { get; set; } = string.Empty;

    internal DataRecordDeletedEvent(DataRecord oldState)
    {
        OldState = oldState.Snapshot();
    }
}
