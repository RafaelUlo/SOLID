using IPL.SOLID.API.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace IPL.SOLID.API.Infrastructure.SQLite.Contexts;

public sealed class SolidWriteContextFactory : IDesignTimeDbContextFactory<SolidWriteContext>
{
    public SolidWriteContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connectionString = SqliteDatabase.GetConnectionString(configuration, Directory.GetCurrentDirectory());

        var optionsBuilder = new DbContextOptionsBuilder<SolidWriteContext>()
            .UseSqlite(connectionString);

        return new SolidWriteContext(optionsBuilder.Options);
    }
}