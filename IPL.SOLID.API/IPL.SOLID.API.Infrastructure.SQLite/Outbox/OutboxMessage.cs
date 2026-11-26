namespace IPL.SOLID.API.Infrastructure.SQLite.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; set; }

    public string Payload { get; set; } = string.Empty;

    public string Topic { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? ProcessedAt { get; set; }

    public List<OutboxDelivery> Deliveries { get; set; } = [];
}
