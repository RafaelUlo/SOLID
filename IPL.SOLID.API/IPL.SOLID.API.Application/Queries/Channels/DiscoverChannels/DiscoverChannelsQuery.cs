using IPL.SOLID.API.Application.Dtos.Discovery;
using IPL.SOLID.API.Application.Responses;
using MediatR;

namespace IPL.SOLID.API.Application.Queries.Channels.DiscoverChannels;

public sealed record DiscoverChannelsQuery(DiscoveryScopeDto Scope) : IRequest<SemanticResponse<List<string>>>;
