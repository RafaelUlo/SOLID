using IPL.SOLID.API.Application.Responses;
using MediatR;

namespace IPL.SOLID.API.Application.Commands.DataRecords.DeleteDataRecord;

public sealed record DeleteDataRecordCommand(string ApplicationName, string ChannelName, string ResourceName)
    : IRequest<SemanticResponse<object>>;
