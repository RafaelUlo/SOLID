using IPL.SOLID.API.Infrastructure.Shared;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace IPL.SOLID.API.Infrastructure.SQLite.Contexts;

internal static class SqliteDatabase
{
    public static string GetConnectionString(IConfiguration configuration, string basePath)
    {
        var configured = configuration.GetConnectionString(ApiConstants.SQL_CONNECTIONSRING_NAME)
            ?? throw new InvalidOperationException("The SOLID database connection string is missing.");
        var connection = new SqliteConnectionStringBuilder(configured);
        var source = connection.DataSource;

        if (source.Length > 0 && source != ":memory:" && connection.Mode != SqliteOpenMode.Memory
            && !source.StartsWith("file:", StringComparison.OrdinalIgnoreCase))
        {
            connection.DataSource = Path.GetFullPath(source, basePath);
            if (connection.Mode == SqliteOpenMode.ReadWriteCreate)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(connection.DataSource)!);
            }
        }

        return connection.ToString();
    }
}
