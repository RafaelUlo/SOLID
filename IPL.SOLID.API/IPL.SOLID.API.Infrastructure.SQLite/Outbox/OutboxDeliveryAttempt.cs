namespace IPL.SOLID.API.Infrastructure.SQLite.Outbox;

public sealed class OutboxDeliveryAttempt
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid DeliveryId { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public bool? Succeeded { get; set; }

    public int? HttpStatusCode { get; set; }

    public string? Error { get; set; }
}
