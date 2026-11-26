using IPL.SOLID.API.Application.Contexts;
using IPL.SOLID.API.Application.Responses;
using Microsoft.EntityFrameworkCore;

namespace IPL.SOLID.API.Application.Queries.Applications.DiscoverApplications;

public sealed class DiscoverApplicationsQueryHandler(ISolidReadContext context)
    : ISemanticRequestHandler<DiscoverApplicationsQuery, List<string>>
{
    public async Task<SemanticResponse<List<string>>> Handle(
        DiscoverApplicationsQuery request, ResponseFactory<List<string>> response, CancellationToken token)
    {
        var names = await context.Applications
            .OrderBy(application => application.ResourceName)
            .Select(application => application.ResourceName)
            .ToListAsync(token);

        var paths = names
            .Select(name => "/api/solid/" + Uri.EscapeDataString(name))
            .ToList();

        return response.Ok(paths);
    }
}
