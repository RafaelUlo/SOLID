namespace IPL.SOLID.API.Web.Middlewares;

using System.Text.Json;

public sealed class ResourceTypeMiddleware(
    RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (HttpMethods.IsPost(context.Request.Method) &&
            context.Request.ContentType?.StartsWith(
                "application/json",
                StringComparison.OrdinalIgnoreCase) == true)
        {
            context.Request.EnableBuffering();

            try
            {
                using var document = await JsonDocument.ParseAsync(
                    context.Request.Body,
                    cancellationToken: context.RequestAborted);

                if (document.RootElement.ValueKind ==
                        JsonValueKind.Object &&
                    document.RootElement.TryGetProperty(
                        "res-type",
                        out var property) &&
                    property.ValueKind == JsonValueKind.String)
                {
                    context.Items["ResourceType"] =
                        property.GetString();
                }
            }
            catch (JsonException)
            {
                // Deixar o tratamento de JSON inválido
                // para a camada de validação apropriada.
            }
            finally
            {
                context.Request.Body.Position = 0;
            }
        }

        await next(context);
    }
}