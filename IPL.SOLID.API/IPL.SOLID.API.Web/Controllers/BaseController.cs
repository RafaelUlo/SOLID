using IPL.SOLID.API.Application.Responses;
using IPL.SOLID.API.Web.Contexts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IPL.SOLID.API.Web.Controllers;

[Route("api/solid")]
[ApiController]

public class BaseController : ControllerBase
{
    protected bool IsDiscovery { get; }

    protected IMediator Mediator { get; }

    public readonly bool IsToDiscoveryApplication;
    public readonly bool IsToDiscoveryChannel;
    public readonly bool IsToDiscoveryDataRecord;
    public readonly bool IsToDiscoverySubscription;

    protected BaseController(IMediator mediator, DiscoveryContext discoveryContext)
    {
        Mediator = mediator;
        IsDiscovery = discoveryContext.ResourceType is not null;

        IsToDiscoveryApplication = discoveryContext.ResourceType == "application";
        IsToDiscoveryChannel = discoveryContext.ResourceType == "channel";
        IsToDiscoveryDataRecord = discoveryContext.ResourceType == "data-record";
        IsToDiscoverySubscription = discoveryContext.ResourceType == "subscription";
    }

    protected async Task<IActionResult> SendAsync<T>(IRequest<SemanticResponse<T>> request, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(request, cancellationToken);

        return StatusCode(result.StatusCode, result);
    }
}
