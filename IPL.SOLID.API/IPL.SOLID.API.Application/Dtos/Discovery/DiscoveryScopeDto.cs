namespace IPL.SOLID.API.Application.Dtos.Discovery;

public sealed record DiscoveryScopeDto(
    string? Application = null, string? Channel = null,
    string? Resource = null, bool Subscriptions = false);
