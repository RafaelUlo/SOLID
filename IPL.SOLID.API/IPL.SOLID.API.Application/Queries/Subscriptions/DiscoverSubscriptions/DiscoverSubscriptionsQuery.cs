using IPL.SOLID.API.Application.Dtos.Discovery;
using IPL.SOLID.API.Application.Responses;
using MediatR;

namespace IPL.SOLID.API.Application.Queries.Subscriptions.DiscoverSubscriptions;

public sealed record DiscoverSubscriptionsQuery(DiscoveryScopeDto Scope) : IRequest<SemanticResponse<List<string>>>;
