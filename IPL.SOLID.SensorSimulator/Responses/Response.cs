namespace IPL.SOLID.SensorSimulator.Responses;

public sealed record Response<T>(T? Content, IReadOnlyList<ResponseError> Errors)
{
}
