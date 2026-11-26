using System.Text.Json.Serialization;

namespace IPL.SOLID.API.Application.Dtos.Subscriptions;

public sealed record SubscriptionDto()
{
    [JsonPropertyName("resource-name")]
    public required string ResourceName { get; init; }

    [JsonPropertyName("dt-creation")]
    public DateTime CreatedAt { get; init; }

    [JsonPropertyName("evt")]
    public int Evt { get; init; }

    [JsonPropertyName("endpoint")]
    public required string Endpoint { get; init; }

    [JsonPropertyName("dt-expiration")]
    public DateTimeOffset? Expiration { get; init; }

    [JsonPropertyName("res-type")]
    public static string ResourceType => "subscription";
}
