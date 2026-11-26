using IPL.SOLID.API.Application.Responses;
using IPL.SOLID.API.Application.Dtos.Applications;
using MediatR;
using System.Text.Json.Serialization;

namespace IPL.SOLID.API.Application.Queries.Applications.GetApplication;

public sealed class GetApplicationQuery : IRequest<SemanticResponse<ApplicationDto>>
{
    [JsonPropertyName("resource-name")]
    public required string ResourceName { get; init; }
}
