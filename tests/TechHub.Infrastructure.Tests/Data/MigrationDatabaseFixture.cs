using Dapper;
using Npgsql;
using Testcontainers.PostgreSql;

namespace TechHub.Infrastructure.Tests.Data;

/// <summary>
/// Starts a single PostgreSQL container and hands out empty, uniquely named databases.
/// Migration tests need a database that has never been migrated, which
/// <see cref="DatabaseFixture{T}"/> cannot provide because it migrates and seeds on startup.
/// </summary>
public sealed class MigrationDatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:17.5-alpine3.22")
        .WithUsername("test")
        .WithPassword("test")
        .Build();

    public async ValueTask InitializeAsync() => await _container.StartAsync();

    public async ValueTask DisposeAsync() => await _container.DisposeAsync();

    /// <summary>
    /// Creates a new empty database in the container and returns a connection string pointing at it.
    /// </summary>
    public async Task<string> CreateEmptyDatabaseAsync(CancellationToken ct)
    {
        var databaseName = $"migration_{Guid.NewGuid():N}";

        await using (var admin = new NpgsqlConnection(_container.GetConnectionString()))
        {
            await admin.OpenAsync(ct);
            await admin.ExecuteAsync(new CommandDefinition($"CREATE DATABASE \"{databaseName}\"", cancellationToken: ct));
        }

        return new NpgsqlConnectionStringBuilder(_container.GetConnectionString())
        {
            Database = databaseName,
            Pooling = false
        }.ConnectionString;
    }

    public static async Task<NpgsqlConnection> OpenAsync(string connectionString, CancellationToken ct)
    {
        var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(ct);
        return connection;
    }
}
