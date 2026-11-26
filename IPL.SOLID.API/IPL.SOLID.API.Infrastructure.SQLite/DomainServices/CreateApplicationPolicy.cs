using IPL.SOLID.API.Domain.Policies;
using IPL.SOLID.API.Infrastructure.SQLite.Contexts;
using Microsoft.EntityFrameworkCore;

namespace IPL.SOLID.API.Infrastructure.SQLite.DomainServices;

public sealed class CreateApplicationPolicy : ICreateApplicationPolicy
{
    private readonly SolidReadContext _context;

    public CreateApplicationPolicy(SolidReadContext context)
    {
        _context = context;
    }

    public async Task<bool> ApplicationHaveUniqueName(string resourceName, CancellationToken cancellationToken)
    {
        return !await _context.Applications
            .AnyAsync(e => e.ResourceName == resourceName, cancellationToken);
    }
}
