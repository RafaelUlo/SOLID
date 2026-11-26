using IPL.SOLID.API.Application.Responses;
﻿using IPL.SOLID.API.Application.Dtos.Subscriptions;
using MediatR;

namespace IPL.SOLID.API.Application.Queries.Subscription;

public sealed class GetSubscriptionsQuery : IRequest<SemanticResponse<SubscriptionDto>>
{
    public required string ApplicationName { get; init; }

    public required string ChannelName { get; init; }

    public required string ResourceName { get; init; }
}
