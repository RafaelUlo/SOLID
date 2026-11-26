namespace IPL.SOLID.SensorSimulator.Responses;

public sealed record ResponseError(string Code, string Message, string? Field = null);