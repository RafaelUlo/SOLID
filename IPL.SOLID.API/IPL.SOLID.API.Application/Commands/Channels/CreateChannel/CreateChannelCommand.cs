using IPL.SOLID.API.Application.Responses;
using IPL.SOLID.API.Application.Dtos.Channels;
using MediatR;
using System.Text.Json.Serialization;

namespace IPL.SOLID.API.Application.Commands.Channels.CreateChannel;

public sealed record CreateChannelCommand : IRequest<SemanticResponse<ChannelDto>>
{
    [JsonPropertyName("res-type")]
    public required string ResourceType { get; init; }

    [JsonPropertyName("resource-name")]
    public required string ResourceName { get; init; }

    [JsonPropertyName("resource-desc")]
    public string Description { get; init; } = string.Empty;

    [JsonIgnore]
    public string Application { get; init; } = string.Empty;
}
