using IPL.SOLID.API.Application.Responses;
﻿using IPL.SOLID.API.Application.Dtos.Channels;
using MediatR;

namespace IPL.SOLID.API.Application.Queries.Channels.GetChannels;

public sealed class GetChannelQuery : IRequest<SemanticResponse<ChannelDto>>
{
    public required string ApplicationName { get; init; }

    public required string ResourceName { get; init; }
}
