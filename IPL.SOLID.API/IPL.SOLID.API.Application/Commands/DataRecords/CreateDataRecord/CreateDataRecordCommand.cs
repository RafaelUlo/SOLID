using IPL.SOLID.API.Application.Dtos.DataRecords;
using IPL.SOLID.API.Application.Responses;
using IPL.SOLID.API.Application.Serialization;
using MediatR;
using System.Text.Json.Serialization;

namespace IPL.SOLID.API.Application.Commands.DataRecords.CreateDataRecord;

public sealed record CreateDataRecordCommand : IRequest<SemanticResponse<DataRecordDto>>
{
    [JsonPropertyName("res-type")]
    public required string ResourceType { get; init; }

    [JsonPropertyName("resource-name")]
    public required string ResourceName { get; init; }

    [JsonPropertyName("content-type")]
    public required string ContentType { get; init; }

    [JsonPropertyName("content")]
    public required string Content { get; init; }

    [JsonPropertyName("dt-expiration")]
    [JsonConverter(typeof(OptionalExpirationConverter))]
    public DateTimeOffset? Expiration { get; init; }

    [JsonIgnore]
    public string ApplicationName { get; init; } = string.Empty;

    [JsonIgnore]
    public string ChannelName { get; init; } = string.Empty;
}
