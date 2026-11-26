using IPL.SOLID.API.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IPL.SOLID.API.Web.Filters;

public sealed class SemanticResponseFilter(ResponseFactory<object> response) : IAlwaysRunResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is ObjectResult result)
        {
            if (result.Value is ISemanticResponse semantic)
            {
                result.StatusCode = semantic.StatusCode;
                return;
            }

            int status = result.StatusCode ?? 200;
            var body = result.Value is ValidationProblemDetails validation
                ? response.BadRequest(validation.Errors.SelectMany(pair => pair.Value.Select(message =>
                    new ResponseError("validation_error", message, pair.Key))).ToArray())
                : status >= 400
                    ? response.Failure(status, "request_error", result.Value?.ToString() ?? "Request failed.")
                    : new SemanticResponse<object>(result.Value, [], status);
            context.Result = new ObjectResult(body) { StatusCode = body.StatusCode };
        }
        else if (context.Result is StatusCodeResult status)
        {
            var body = response.Failure(status.StatusCode, "request_error", "Request failed.");
            context.Result = new ObjectResult(body) { StatusCode = body.StatusCode };
        }
    }

    public void OnResultExecuted(ResultExecutedContext context) { }
}
