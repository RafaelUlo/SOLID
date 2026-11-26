using System.Text.Json.Serialization;

namespace IPL.SOLID.SensorSimulator.SolidMiddlewareContracts;

public sealed record CreateChannelCommand
{
    [JsonPropertyName("res-type")]
    public required string ResourceType { get; init; }

    [JsonPropertyName("resource-name")]
    public required string ResourceName { get; init; }

    [JsonPropertyName("resource-desc")]
    public string Description { get; init; } = string.Empty;
}
