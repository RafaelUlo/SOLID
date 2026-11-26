using IPL.SOLID.API.Application.Responses;
﻿using IPL.SOLID.API.Application.Contexts;
using IPL.SOLID.API.Application.Dtos.Subscriptions;
using IPL.SOLID.API.Application.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IPL.SOLID.API.Application.Queries.Subscription;

public sealed class GetSubscriptionsQueryHandler : ISemanticRequestHandler<GetSubscriptionsQuery, SubscriptionDto>
{
    private readonly ISolidReadContext _readContext;

    public GetSubscriptionsQueryHandler(ISolidReadContext solidReadContext)
    {
        _readContext = solidReadContext;
    }


    public async Task<SemanticResponse<SubscriptionDto>> Handle(
        GetSubscriptionsQuery request, ResponseFactory<SubscriptionDto> response, CancellationToken cancellationToken)
    {
        var application = await _readContext.Applications
            .Include(e => e.Channels)
                .ThenInclude(e => e.Subscriptions)
            .FirstOrDefaultAsync(a => a.ResourceName == request.ApplicationName, cancellationToken);
        if (application is null) return response.NotFound($"Application with resource name '{request.ApplicationName}' not found.");

        var channel = application
            .Channels
            .FirstOrDefault(a => a.ApplicationId == application.Id &&
                a.ResourceName == request.ChannelName);
        if (channel is null) return response.NotFound($"Channel with resource name '{request.ChannelName}' that belong to '{request.ApplicationName}' not found.");

        var subscription = channel.Subscriptions
            .FirstOrDefault(a => a.ChannelId == channel.Id &&
                a.ResourceName == request.ResourceName);
        if (subscription is null) return response.NotFound($"Subscription with resource name '{request.ResourceName}' that belong to Channel '{request.ChannelName}' and Application '{request.ApplicationName}'not found.");

        return response.Ok(ApplicationMapping.CreateSubscriptionDto(subscription));
    }
}
