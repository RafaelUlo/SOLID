using IPL.SOLID.API.Infrastructure.Shared.ServiceInjector;
using IPL.SOLID.API.Application.Responses;
using IPL.SOLID.API.Application.ServiceInjector;
using IPL.SOLID.API.Infrastructure.Shared;
using IPL.SOLID.API.Infrastructure.SQLite.Contexts;
using IPL.SOLID.API.Infrastructure.SQLite.ServiceInjector;
using IPL.SOLID.API.Web.Contexts;
using IPL.SOLID.API.Web.Documentation;
using IPL.SOLID.API.Web.Middlewares;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the channel.

builder.Services.AddSolidSharedServices();

builder.Services.AddSolidSQLiteServices(builder.Configuration, builder.Environment.ContentRootPath);

builder.Services.AddScoped<DiscoveryContext>();
builder.Services.AddSingleton<IPL.SOLID.API.Application.Notifications.IMqttNotificationSender,
    IPL.SOLID.API.Infrastructure.MQTT.MqttNotificationSender>();

builder.Services.AddSolidApplicationServices();

builder.Services.AddLogging();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(ApiConstants.CORS_POLICY_NAME,
        policyBuilder =>
        {
            policyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.ResolveConflictingActions(descriptions =>
    {
        var actions = descriptions.ToArray();
        if (actions.All(action => action.HttpMethod == "POST" &&
            action.RelativePath?.TrimEnd('/') == "api/solid/{application}/{channel}"))
            return actions.OrderBy(action => action.ActionDescriptor.DisplayName).First();

        throw new InvalidOperationException("Unexpected conflicting API routes: " +
            string.Join(", ", actions.Select(action => action.ActionDescriptor.DisplayName)));
    });
    options.OperationFilter<ChannelResourceOperationFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ResourceTypeMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePages(async statusContext =>
{
    var http = statusContext.HttpContext;
    var factory = http.RequestServices.GetRequiredService<ResponseFactory<object>>();
    var result = factory.Failure(http.Response.StatusCode, "http_error",
      ReasonPhrases.GetReasonPhrase(http.Response.StatusCode));
    await http.Response.WriteAsJsonAsync(result);
});
app.UseMiddleware<DiscoveryMiddleware>();

app.UseCors(ApiConstants.CORS_POLICY_NAME);

using (var scope = app.Services.CreateScope())
{
    var solidContext = scope.ServiceProvider.GetRequiredService<SolidWriteContext>();
    await solidContext.Database.MigrateAsync();
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
