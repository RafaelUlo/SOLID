using IPL.SOLID.API.Application.Contexts;
using IPL.SOLID.API.Application.Responses;
using Microsoft.EntityFrameworkCore;

namespace IPL.SOLID.API.Application.Queries.Channels.DiscoverChannels;

public sealed class DiscoverChannelsQueryHandler(ISolidReadContext context)
    : ISemanticRequestHandler<DiscoverChannelsQuery, List<string>>
{
    public async Task<SemanticResponse<List<string>>> Handle(
        DiscoverChannelsQuery request, ResponseFactory<List<string>> response, CancellationToken token)
    {
        var channels = context.Channels.Where(channel => channel.Application != null);

        if (request.Scope.Application is not null)
            channels = channels.Where(channel => channel.Application!.ResourceName == request.Scope.Application);

        var names = await channels
            .OrderBy(channel => channel.Application!.ResourceName)
            .ThenBy(channel => channel.ResourceName)
            .Select(channel => new
            {
                ApplicationName = channel.Application!.ResourceName,
                ChannelName = channel.ResourceName
            })
            .ToListAsync(token);

        var paths = names.Select(channel =>
            $"/api/solid/{Uri.EscapeDataString(channel.ApplicationName)}/{Uri.EscapeDataString(channel.ChannelName)}")
            .ToList();

        return response.Ok(paths);
    }
}
