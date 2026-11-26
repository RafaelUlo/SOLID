namespace IPL.SOLID.API.Domain.Policies;

public interface ICreateApplicationPolicy
{
    Task<bool> ApplicationHaveUniqueName(string resourceName, CancellationToken cancellationToken);
}
