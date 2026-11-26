using IPL.SOLID.API.Application.Commands.Subscriptions.CreateSubscription;
using IPL.SOLID.API.Application.Dtos.Subscriptions;
using IPL.SOLID.API.Application.Queries.Subscription;
using IPL.SOLID.API.Application.Responses;
using IPL.SOLID.API.Web.Contexts;
using IPL.SOLID.API.Web.CustomAttributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IPL.SOLID.API.Web.Controllers;


[ApiController]
public sealed class SubscriptionController(IMediator sender, DiscoveryContext discoveryContext) : BaseController(sender, discoveryContext)
{
    [HttpGet("{application}/{channel}/subs/{resource}")]
    public async Task<IActionResult> GetSubscription(
        [FromRoute] string application,
        [FromRoute] string channel,
        [FromRoute] string resource,
        CancellationToken token)
    {
        return await SendAsync(new GetSubscriptionsQuery
        {
            ApplicationName = application,
            ChannelName = channel,
            ResourceName = resource
        },
            token);
    }

    [ProducesResponseType(typeof(SemanticResponse<SubscriptionDto>), StatusCodes.Status201Created)]
    [ResourceType("subscription")]
    [HttpPost("{application}/{channel}")]
    public async Task<IActionResult> CreateSubscription(
        [FromRoute] string application,
        [FromRoute] string channel,
        [FromBody] CreateSubscriptionCommand command,
        CancellationToken cancellationToken)
    {
        return await SendAsync(command with { ApplicationName = application, ChannelName = channel }, cancellationToken);
    }

}
