using System.Text.Json.Serialization;

namespace IPL.SOLID.API.Application.Dtos.Applications;

public sealed record ApplicationDto
{
    [JsonPropertyName("res-type")]
    public string ResourceType => "application";

    [JsonPropertyName("resource-desc")]
    public required string Description { get; init; }

    [JsonPropertyName("resource-name")]
    public string ResourceName { get; init; }

    [JsonPropertyName("dt-creation")]
    public DateTime CreatedAt { get; init; }

}
