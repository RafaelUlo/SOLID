namespace IPL.SOLID.API.Application.Responses;

public sealed class ResponseFactory<T>
{
    public SemanticResponse<T> Ok(T content) => new(content, [], 200);
    public SemanticResponse<T> Created(T content) => new(content, [], 201);
    public SemanticResponse<T> NoContent() => new(default, [], 204);
    public SemanticResponse<T> Accepted(T content) => new(content, [], 202);
    public SemanticResponse<T> NotFound(string message = "Resource not found.") => Failure(404, "not_found", message);
    public SemanticResponse<T> Forbidden(string message = "Access forbidden.") => Failure(403, "forbidden", message);
    public SemanticResponse<T> Unauthorized(string message = "Authentication required.") => Failure(401, "unauthorized", message);
    public SemanticResponse<T> Conflict(string message) => Failure(409, "conflict", message);
    public SemanticResponse<T> BadRequest(params ResponseError[] errors) => new(default, errors, 400);
    public SemanticResponse<T> InternalServerError() => Failure(500, "internal_error", "An unexpected error occurred.");
    public SemanticResponse<T> Failure(int statusCode, string code, string message) =>
        new(default, [new ResponseError(code, message)], statusCode);
}
