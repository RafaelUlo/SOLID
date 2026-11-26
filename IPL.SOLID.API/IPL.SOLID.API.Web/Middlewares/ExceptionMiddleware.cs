using IPL.SOLID.API.Application.Responses;
using IPL.SOLID.API.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IPL.SOLID.API.Web.Middlewares;

public sealed class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context, ResponseFactory<object> response)
    {
        try
        {
            await next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            if (context.Response.HasStarted) throw;
            logger.LogError(exception, "Request failed. Trace {TraceId}", context.TraceIdentifier);
            var result = exception switch
            {
                DomainException domain => response.Conflict(domain.Message),
                DbUpdateException => response.Conflict("The resource could not be saved because of a database constraint."),
                ArgumentException argument => response.BadRequest(new ResponseError("invalid_argument", argument.Message)),
                _ => response.InternalServerError()
            };
            context.Response.Clear();
            context.Response.StatusCode = result.StatusCode;
            await context.Response.WriteAsJsonAsync(result, context.RequestAborted);
        }
    }
}
