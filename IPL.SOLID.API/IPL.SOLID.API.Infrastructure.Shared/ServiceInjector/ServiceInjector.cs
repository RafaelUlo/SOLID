using IPL.SOLID.API.Application.DomainEvents;
using IPL.SOLID.API.Infrastructure.Shared.DomainEvents;
using Microsoft.Extensions.DependencyInjection;

namespace IPL.SOLID.API.Infrastructure.Shared.ServiceInjector;

public static class ServiceInjector
{
    public static IServiceCollection AddSolidSharedServices(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventPublisher, MediatRDomainEventPublisher>();
        return services;
    }
}
