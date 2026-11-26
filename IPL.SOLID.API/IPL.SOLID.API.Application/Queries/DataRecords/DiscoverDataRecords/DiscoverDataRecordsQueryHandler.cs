using IPL.SOLID.API.Application.Contexts;
using IPL.SOLID.API.Application.Responses;
using Microsoft.EntityFrameworkCore;

namespace IPL.SOLID.API.Application.Queries.DataRecords.DiscoverDataRecords;

public sealed class DiscoverDataRecordsQueryHandler(ISolidReadContext context)
    : ISemanticRequestHandler<DiscoverDataRecordsQuery, List<string>>
{
    public async Task<SemanticResponse<List<string>>> Handle(
        DiscoverDataRecordsQuery request, ResponseFactory<List<string>> response, CancellationToken token)
    {
        var resources = context.DataRecords.Where(resource => resource.Channel.Application != null);

        if (request.Scope.Application is not null)
            resources = resources.Where(resource =>
                resource.Channel.Application!.ResourceName == request.Scope.Application);

        if (request.Scope.Channel is not null)
            resources = resources.Where(resource => resource.Channel.ResourceName == request.Scope.Channel);

        var names = await resources
            .OrderBy(resource => resource.Channel.Application!.ResourceName)
            .ThenBy(resource => resource.Channel.ResourceName)
            .ThenBy(resource => resource.ResourceName)
            .Select(resource => new
            {
                ApplicationName = resource.Channel.Application!.ResourceName,
                ChannelName = resource.Channel.ResourceName,
                resource.ResourceName
            })
            .ToListAsync(token);

        var paths = names.Select(resource =>
            $"/api/solid/{Uri.EscapeDataString(resource.ApplicationName)}/{Uri.EscapeDataString(resource.ChannelName)}/{Uri.EscapeDataString(resource.ResourceName)}")
            .ToList();

        return response.Ok(paths);
    }
}
