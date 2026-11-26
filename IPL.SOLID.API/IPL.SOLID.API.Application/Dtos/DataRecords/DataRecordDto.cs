using System.Text.Json.Serialization;

namespace IPL.SOLID.API.Application.Dtos.DataRecords;

public sealed record DataRecordDto()
{
    [JsonPropertyName("resource-name")]
    public required string ResourceName { get; init; }

    [JsonPropertyName("dt-creation")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("content-type")]
    public required string ContentType { get; set; }

    [JsonPropertyName("content")]
    public required string Content { get; set; }

    [JsonPropertyName("dt-expiration")]
    public DateTimeOffset? Expiration { get; set; }

    [JsonPropertyName("res-type")]
    public static string ResourceType => "data-record";
}
