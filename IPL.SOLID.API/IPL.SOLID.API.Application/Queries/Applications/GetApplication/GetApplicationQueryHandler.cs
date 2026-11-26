using IPL.SOLID.API.Application.Contexts;
using IPL.SOLID.API.Application.Dtos.Applications;
using IPL.SOLID.API.Application.Mappings;
using IPL.SOLID.API.Application.Responses;
using Microsoft.EntityFrameworkCore;

namespace IPL.SOLID.API.Application.Queries.Applications.GetApplication;

public sealed class GetApplicationQueryHandler : ISemanticRequestHandler<GetApplicationQuery, ApplicationDto>
{
    private readonly ISolidReadContext _readContext;

    public GetApplicationQueryHandler(ISolidReadContext readContext)
    {
        _readContext = readContext;
    }

    public async Task<SemanticResponse<ApplicationDto>> Handle(
        GetApplicationQuery request, ResponseFactory<ApplicationDto> response, CancellationToken cancellationToken)
    {
        var application = await _readContext.Applications
            .FirstOrDefaultAsync(a => a.ResourceName == request.ResourceName, cancellationToken);

        if (application is null)
        {
            return response.NotFound($"Application with resource name '{request.ResourceName}' not found.");
        }

        return response.Ok(ApplicationMapping.CreateApplicationDto(application));
    }
}
