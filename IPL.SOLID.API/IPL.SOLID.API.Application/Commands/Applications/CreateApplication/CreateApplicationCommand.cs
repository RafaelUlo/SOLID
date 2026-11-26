using IPL.SOLID.API.Application.Responses;
using System.Text.Json.Serialization;
using IPL.SOLID.API.Application.Dtos.Applications;
using MediatR;

namespace IPL.SOLID.API.Application.Commands.Applications.CreateApplication;

public sealed record CreateApplicationCommand : IRequest<SemanticResponse<ApplicationDto>>
{
    [JsonPropertyName("res-type")]
    public required string ResourceType { get; init; }

    [JsonPropertyName("resource-name")]
    public string? ResourceName { get; init; }

    [JsonPropertyName("resource-desc")]
    public string Description { get; init; } = string.Empty;
}
