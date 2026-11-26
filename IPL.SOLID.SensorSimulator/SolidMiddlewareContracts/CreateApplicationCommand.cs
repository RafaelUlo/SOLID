using System.Text.Json.Serialization;

namespace IPL.SOLID.SensorSimulator.SolidMiddlewareContracts;

public sealed record CreateApplicationCommand
{
    [JsonPropertyName("res-type")]
    public required string ResourceType { get; init; }

    [JsonPropertyName("resource-name")]
    public string? ResourceName { get; init; }

    [JsonPropertyName("resource-desc")]
    public string Description { get; init; } = string.Empty;
}
