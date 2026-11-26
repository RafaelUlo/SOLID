using IPL.SOLID.API.Web.Contexts;

namespace IPL.SOLID.API.Web.Middlewares;

public sealed class DiscoveryMiddleware
{
    private readonly RequestDelegate _next;

    public DiscoveryMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, DiscoveryContext discoveryContext)
    {
        if (context.Request.Headers.TryGetValue("solid-discovery", out var headerValue))
        {
            discoveryContext.ResourceType = headerValue.ToString();
        }

        await _next(context);
    }
}
