using IPL.SOLID.API.Domain.Entities;
using IPL.SOLID.API.Domain.Exceptions;
using IPL.SOLID.API.Domain.Policies;

namespace IPL.SOLID.API.Domain.Factories;

public static class ApplicationFactory
{
    public static async Task<Application> CreateApplicationAsync(string resourceName, ICreateApplicationPolicy createApplicationPolicy, CancellationToken cancellationToken = default)
    {
        var applicationHaveUniqueName = await createApplicationPolicy.ApplicationHaveUniqueName(resourceName, cancellationToken);

        if (!applicationHaveUniqueName)
        {
            throw new DomainException($"An application with the resource name '{resourceName}' already exists.");
        }

        return new Application(Guid.NewGuid(), resourceName);
    }
}
