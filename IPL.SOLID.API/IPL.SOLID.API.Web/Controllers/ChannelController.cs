using IPL.SOLID.API.Application.Commands.Channels.CreateChannel;
using IPL.SOLID.API.Application.Dtos.Channels;
using IPL.SOLID.API.Application.Dtos.Discovery;
using IPL.SOLID.API.Application.Queries.Channels.GetChannels;
using IPL.SOLID.API.Application.Queries.DataRecords.DiscoverDataRecords;
using IPL.SOLID.API.Application.Queries.Subscriptions.DiscoverSubscriptions;
using IPL.SOLID.API.Application.Responses;
using IPL.SOLID.API.Web.Contexts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IPL.SOLID.API.Web.Controllers;

[ApiController]
[Route("api/solid")]
public sealed class ChannelController(IMediator sender, DiscoveryContext discoveryContext)
    : BaseController(sender, discoveryContext)
{
    [HttpGet("{application}/{channel}")]
    public async Task<IActionResult> GetChannel(
        [FromRoute] string application,
        [FromRoute] string channel,
        [FromHeader(Name = "solid-discovery")] string? discovery,
        CancellationToken cancellationToken)
    {
        if (IsToDiscoverySubscription)
        {
            return await SendAsync(new DiscoverSubscriptionsQuery(new DiscoveryScopeDto(application, channel)), cancellationToken);
        }

        if (IsToDiscoveryDataRecord)
            return await SendAsync(new DiscoverDataRecordsQuery(new DiscoveryScopeDto(application, channel)), cancellationToken);

        if (IsDiscovery)
        {
            var response = new ResponseFactory<List<string>>().BadRequest(
                new ResponseError("invalid_discovery",
                    "This endpoint only supports channel discovery.", "solid-discovery"));
            return StatusCode(response.StatusCode, response);
        }

        return await SendAsync(new GetChannelQuery { ApplicationName = application, ResourceName = channel }, cancellationToken);
    }

    [ProducesResponseType(typeof(SemanticResponse<ChannelDto>), StatusCodes.Status201Created)]
    [HttpPost("{application}")]
    public async Task<IActionResult> CreateChannel(
        [FromRoute] string application,
        [FromBody] CreateChannelCommand command,
        CancellationToken cancellationToken)
    {
        return await SendAsync(command with { Application = application }, cancellationToken);
    }
}
