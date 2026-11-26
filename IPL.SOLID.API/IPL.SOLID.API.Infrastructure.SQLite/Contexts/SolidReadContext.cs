using IPL.SOLID.API.Application.Contexts;
using IPL.SOLID.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

using ApplicationEntity = IPL.SOLID.API.Domain.Entities.Application;

namespace IPL.SOLID.API.Infrastructure.SQLite.Contexts;

public sealed class SolidReadContext : DbContext, ISolidReadContext
{
    public SolidReadContext(DbContextOptions<SolidReadContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationEntity> Applications { get; set; }

    public DbSet<Channel> Channels { get; set; }

    public DbSet<DataRecord> DataRecords { get; set; }

    public DbSet<Subscription> Subscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SolidReadContext).Assembly);
    }
}
