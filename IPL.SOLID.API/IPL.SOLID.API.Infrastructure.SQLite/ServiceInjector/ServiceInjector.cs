using IPL.SOLID.API.Domain.Policies;
using IPL.SOLID.API.Application.Contexts;
using IPL.SOLID.API.Infrastructure.SQLite.Contexts;
using IPL.SOLID.API.Infrastructure.SQLite.DomainServices;
using IPL.SOLID.API.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IPL.SOLID.API.Infrastructure.SQLite.ServiceInjector;

public static class ServiceInjector
{
    public static IServiceCollection AddSolidSQLiteServices(this IServiceCollection services, IConfiguration config, string contentRootPath)
    {
        var databaseConnectionString = SqliteDatabase.GetConnectionString(config, contentRootPath);
        services.AddDbContext<SolidWriteContext>(options =>
        {
            options.UseSqlite(databaseConnectionString);
        });
        services.AddDbContext<SolidReadContext>(options =>
        {
            options.UseSqlite(databaseConnectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddScoped<ISolidReadContext>(provider => provider.GetRequiredService<SolidReadContext>());
        services.AddScoped<ISolidWriteContext>(provider => provider.GetRequiredService<SolidWriteContext>());
        services.AddScoped<ICreateApplicationPolicy, CreateApplicationPolicy>();

        services.AddScoped<MediatR.INotificationHandler<IPL.SOLID.API.Application.DomainEvents.DomainEventNotification>,
            IPL.SOLID.API.Infrastructure.SQLite.Outbox.DataRecordOutboxHandler>();
        services.AddOptions<IPL.SOLID.API.Infrastructure.SQLite.Outbox.OutboxOptions>()
            .Bind(config.GetSection("Outbox"))
            .Validate(options => options.PollIntervalSeconds > 0 && options.BatchSize > 0 &&
                options.DeliveryTimeoutSeconds > 0 && options.LeaseSeconds > options.DeliveryTimeoutSeconds + 5 &&
                options.MaxRetryDelaySeconds > 0, "Invalid Outbox timing configuration.")
            .ValidateOnStart();
        services.AddHttpClient("SolidNotifications", client => client.Timeout = Timeout.InfiniteTimeSpan)
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { AllowAutoRedirect = false });
        services.AddScoped<IPL.SOLID.API.Infrastructure.SQLite.Outbox.OutboxProcessor>();
        services.AddHostedService<IPL.SOLID.API.Infrastructure.SQLite.Outbox.OutboxBackgroundService>();
        return services;
    }
}
