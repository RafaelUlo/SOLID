using IPL.SOLID.API.Application.Dtos.DataRecords;
using IPL.SOLID.API.Application.Responses;
using MediatR;

namespace IPL.SOLID.API.Application.Queries.DataRecords.GetDataRecords;

public sealed class GetDataRecordsQuery : IRequest<SemanticResponse<DataRecordDto>>
{
    public required string ApplicationName { get; init; }

    public required string ChannelName { get; init; }

    public required string ResourceName { get; init; }
}
