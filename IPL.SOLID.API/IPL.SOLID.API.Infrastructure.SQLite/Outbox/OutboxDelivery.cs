namespace IPL.SOLID.API.Infrastructure.SQLite.Outbox;

public sealed class OutboxDelivery
{
    public Guid Id { get; set; }

    public Guid MessageId { get; set; }

    public OutboxMessage Message { get; set; } = null!;

    public Guid SubscriptionId { get; set; }

    public string Endpoint { get; set; } = string.Empty;

    public int Attempts { get; set; }

    public long NextAttemptAt { get; set; }

    public Guid? LeaseId { get; set; }

    public long? LeaseUntil { get; set; }

    public DateTime? DeliveredAt { get; set; }

    public string? LastError { get; set; }
}
