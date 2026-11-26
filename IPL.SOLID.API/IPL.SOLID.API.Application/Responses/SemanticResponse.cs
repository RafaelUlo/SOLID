namespace IPL.SOLID.API.Application.Responses;

public sealed record SemanticResponse<T>(T? Content, IReadOnlyList<ResponseError> Errors, int StatusCode)
    : ISemanticResponse
{
    public bool Success => StatusCode is >= 200 and < 300;
}
