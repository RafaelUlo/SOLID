using IPL.SOLID.API.Application.Contexts;
using IPL.SOLID.API.Application.Dtos.Channels;
using IPL.SOLID.API.Application.Mappings;
using IPL.SOLID.API.Application.Responses;
using Microsoft.EntityFrameworkCore;

namespace IPL.SOLID.API.Application.Queries.Channels.GetChannels;

public sealed class GetChannelQueryHandler : ISemanticRequestHandler<GetChannelQuery, ChannelDto>
{
    private readonly ISolidReadContext _readContext;

    public GetChannelQueryHandler(ISolidReadContext solidReadContext)
    {
        _readContext = solidReadContext;
    }

    public async Task<SemanticResponse<ChannelDto>> Handle(
        GetChannelQuery request, ResponseFactory<ChannelDto> response, CancellationToken cancellationToken)
    {
        var application = await _readContext.Applications
            .Include(e => e.Channels)
            .FirstOrDefaultAsync(a => a.ResourceName == request.ApplicationName, cancellationToken);
        if (application is null) return response.NotFound($"Application with resource name '{request.ApplicationName}' not found.");

        var channel = application
            .Channels
            .FirstOrDefault(a => a.ApplicationId == application.Id &&
                a.ResourceName == request.ResourceName);
        if (channel is null) return response.NotFound($"Channel with resource name '{request.ResourceName}' that belong to '{request.ApplicationName}' not found.");

        return response.Ok(ApplicationMapping.CreateChannelDto(channel));
    }
}