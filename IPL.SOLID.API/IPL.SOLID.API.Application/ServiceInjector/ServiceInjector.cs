using FluentValidation;
using IPL.SOLID.API.Application.Responses;
using Microsoft.Extensions.DependencyInjection;

namespace IPL.SOLID.API.Application.ServiceInjector;

public static class ServiceInjector
{
    public static IServiceCollection AddSolidApplicationServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(ResponseFactory<>));

        var assembly = typeof(ServiceInjector).Assembly;

        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);

        services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));

        return services;
    }
}
