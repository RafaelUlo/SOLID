namespace IPL.SOLID.API.Application.Responses;

public sealed record ResponseError(string Code, string Message, string? Field = null);