using IPL.SOLID.API.Application.Commands.Applications.CreateApplication;
using IPL.SOLID.API.Application.Dtos.Applications;
using IPL.SOLID.API.Application.Dtos.Discovery;
using IPL.SOLID.API.Application.Queries.Applications.DiscoverApplications;
using IPL.SOLID.API.Application.Queries.Applications.GetApplication;
using IPL.SOLID.API.Application.Queries.Channels.DiscoverChannels;
using IPL.SOLID.API.Application.Responses;
using IPL.SOLID.API.Web.Contexts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IPL.SOLID.API.Web.Controllers;

[ApiController]
public class ApplicationController : BaseController
{
    public ApplicationController(IMediator mediator, DiscoveryContext discoveryContext)
        : base(mediator, discoveryContext)
    {
    }

    [HttpGet]
    [ProducesResponseType(typeof(SemanticResponse<List<string>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Discover(
    [FromHeader(Name = "solid-discovery")] string? discovery, CancellationToken token)
    {
        if (IsToDiscoveryApplication)
        {
            return await SendAsync(new DiscoverApplicationsQuery(), token);
        }

        var response = new ResponseFactory<List<string>>().BadRequest(
            new ResponseError("invalid_discovery",
                "This endpoint only supports Application discovery.", "solid-discovery"));
        return StatusCode(response.StatusCode, response);
    }

    [ProducesResponseType(typeof(SemanticResponse<ApplicationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SemanticResponse<ApplicationDto>), StatusCodes.Status404NotFound)]
    [HttpGet("{application}")]
    public async Task<IActionResult> GetApplication(
        [FromRoute] string application,
        [FromHeader(Name = "solid-discovery")] string? discovery,
        CancellationToken token)
    {
        if (IsToDiscoveryChannel)
        {
            return await SendAsync(new DiscoverChannelsQuery(new DiscoveryScopeDto(application)), token);
        }

        if (IsDiscovery)
        {
            var response = new ResponseFactory<List<string>>().BadRequest(
                new ResponseError("invalid_discovery",
                    "This endpoint only supports channel discovery.", "solid-discovery"));
            return StatusCode(response.StatusCode, response);
        }

        return await SendAsync(new GetApplicationQuery { ResourceName = application }, token);
    }

    [ProducesResponseType(typeof(SemanticResponse<ApplicationDto>), StatusCodes.Status201Created)]
    [HttpPost]
    public async Task<IActionResult> CreateApplication([FromBody] CreateApplicationCommand command, CancellationToken token)
    {
        var result = await Mediator.Send(command, token);
        return StatusCode(result.StatusCode, result);
    }
}
