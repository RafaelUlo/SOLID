using IPL.SOLID.API.Application.Contexts;
using IPL.SOLID.API.Application.Responses;
using Microsoft.EntityFrameworkCore;

namespace IPL.SOLID.API.Application.Queries.Subscriptions.DiscoverSubscriptions;

public sealed class DiscoverSubscriptionsQueryHandler(ISolidReadContext context)
    : ISemanticRequestHandler<DiscoverSubscriptionsQuery, List<string>>
{
    public async Task<SemanticResponse<List<string>>> Handle(
        DiscoverSubscriptionsQuery request, ResponseFactory<List<string>> response, CancellationToken token)
    {
        var resources = context.Subscriptions.Where(resource => resource.Channel.Application != null);

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
            $"/api/solid/{Uri.EscapeDataString(resource.ApplicationName)}/{Uri.EscapeDataString(resource.ChannelName)}/subs/{Uri.EscapeDataString(resource.ResourceName)}")
            .ToList();

        return response.Ok(paths);
    }
}
