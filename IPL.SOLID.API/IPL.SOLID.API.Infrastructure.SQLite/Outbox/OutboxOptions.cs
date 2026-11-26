namespace IPL.SOLID.API.Infrastructure.SQLite.Outbox;

public sealed class OutboxOptions
{
    public int PollIntervalSeconds { get; set; } = 2;
    public int DeliveryTimeoutSeconds { get; set; } = 15;
    public int LeaseSeconds { get; set; } = 60;
    public int BatchSize { get; set; } = 20;
    public int MaxRetryDelaySeconds { get; set; } = 300;
}
