using IPL.SOLID.API.Application.Contexts;
using IPL.SOLID.API.Application.Dtos.DataRecords;
using IPL.SOLID.API.Application.Mappings;
using IPL.SOLID.API.Application.Responses;
using Microsoft.EntityFrameworkCore;

namespace IPL.SOLID.API.Application.Queries.DataRecords.GetDataRecords;

public sealed class GetDataRecordsQueryHandler : ISemanticRequestHandler<GetDataRecordsQuery, DataRecordDto>
{
    private readonly ISolidReadContext _readContext;

    public GetDataRecordsQueryHandler(ISolidReadContext solidReadContext)
    {
        _readContext = solidReadContext;
    }

    public async Task<SemanticResponse<DataRecordDto>> Handle(
        GetDataRecordsQuery request, ResponseFactory<DataRecordDto> response, CancellationToken cancellationToken)
    {
        var application = await _readContext.Applications
            .Include(e => e.Channels)
                .ThenInclude(e => e.DataRecords)
            .FirstOrDefaultAsync(a => a.ResourceName == request.ApplicationName, cancellationToken);
        if (application is null) return response.NotFound($"Application with resource name '{request.ApplicationName}' not found.");

        var channel = application
            .Channels
            .FirstOrDefault(a => a.ApplicationId == application.Id &&
                a.ResourceName == request.ChannelName);
        if (channel is null) return response.NotFound($"Channel with resource name '{request.ChannelName}' that belong to '{request.ApplicationName}' not found.");

        var dataRecord = channel.DataRecords
            .FirstOrDefault(a => a.ChannelId == channel.Id &&
                a.ResourceName == request.ResourceName);
        if (dataRecord is null) return response.NotFound($"Subscription with resource name '{request.ResourceName}' that belong to Channel '{request.ChannelName}' and Application '{request.ApplicationName}'not found.");

        return response.Ok(ApplicationMapping.CreateDateRecordDto(dataRecord));
    }
}
