using System.Text.Json.Serialization;

namespace IPL.SOLID.API.Application.Dtos.Channels;

public sealed record ChannelDto()
{
    [JsonPropertyName("resource-name")]
    public required string ResourceName { get; init; }

    [JsonPropertyName("dt-creation")]
    public DateTime CreatedAt { get; init; }

    [JsonPropertyName("resource-desc")]
    public required string Description { get; init; }

    [JsonPropertyName("res-type")]
    public static string ResourceType => "channel";
}
