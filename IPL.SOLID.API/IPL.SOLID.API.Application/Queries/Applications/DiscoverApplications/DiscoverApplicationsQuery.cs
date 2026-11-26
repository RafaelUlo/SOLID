using IPL.SOLID.API.Application.Responses;
using MediatR;

namespace IPL.SOLID.API.Application.Queries.Applications.DiscoverApplications;

public sealed record DiscoverApplicationsQuery : IRequest<SemanticResponse<List<string>>>;
