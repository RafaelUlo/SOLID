using IPL.SOLID.API.Application.Dtos.Discovery;
using IPL.SOLID.API.Application.Responses;
using MediatR;

namespace IPL.SOLID.API.Application.Queries.DataRecords.DiscoverDataRecords;

public sealed record DiscoverDataRecordsQuery(DiscoveryScopeDto Scope) : IRequest<SemanticResponse<List<string>>>;
