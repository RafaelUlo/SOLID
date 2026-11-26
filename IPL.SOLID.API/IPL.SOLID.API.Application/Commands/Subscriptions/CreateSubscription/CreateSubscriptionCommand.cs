using IPL.SOLID.API.Application.Dtos.Subscriptions;
using IPL.SOLID.API.Application.Responses;
using IPL.SOLID.API.Application.Serialization;
using MediatR;
using System.Text.Json.Serialization;

namespace IPL.SOLID.API.Application.Commands.Subscriptions.CreateSubscription;

public sealed record CreateSubscriptionCommand : IRequest<SemanticResponse<SubscriptionDto>>
{
    [JsonPropertyName("res-type")]
    public required string ResourceType { get; init; }

    [JsonPropertyName("resource-name")]
    public required string ResourceName { get; init; }

    [JsonPropertyName("evt")]
    public int Evt { get; init; }

    [JsonPropertyName("endpoint")]
    public required string Endpoint { get; init; }

    [JsonPropertyName("dt-expiration")]
    [JsonConverter(typeof(OptionalExpirationConverter))]
    public DateTimeOffset? Expiration { get; init; }

    [JsonIgnore]
    public string ApplicationName { get; init; } = string.Empty;

    [JsonIgnore]
    public string ChannelName { get; init; } = string.Empty;
}
