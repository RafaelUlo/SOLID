using Microsoft.EntityFrameworkCore;
using ApplicationEntity = IPL.SOLID.API.Domain.Entities.Application;

namespace IPL.SOLID.API.Application.Contexts;

public interface ISolidWriteContext
{
    DbSet<ApplicationEntity> Applications { get; }

    DbSet<Domain.Entities.Channel> Channels { get; }

    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
