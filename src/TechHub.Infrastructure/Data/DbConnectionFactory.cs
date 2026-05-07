using System.Data;
using Npgsql;
using TechHub.Core.Interfaces;

namespace TechHub.Infrastructure.Data;

/// <summary>
/// Factory for creating PostgreSQL database connections backed by an <see cref="NpgsqlDataSource"/>.
/// Uses the data source's built-in connection pool — connections are acquired from the pool
/// rather than establishing a new TCP connection on every call.
/// </summary>
public class PostgresConnectionFactory : IDbConnectionFactory
{
    private readonly NpgsqlDataSource _dataSource;

    public PostgresConnectionFactory(NpgsqlDataSource dataSource)
    {
        ArgumentNullException.ThrowIfNull(dataSource);
        _dataSource = dataSource;
    }

    /// <summary>
    /// Creates and opens a new database connection synchronously by acquiring one from the pool.
    /// Use only when async is not possible (e.g. DI factory delegates). In all async code paths,
    /// prefer <see cref="CreateConnectionAsync"/> to avoid blocking the thread pool.
    /// </summary>
    public IDbConnection CreateConnection()
    {
        return _dataSource.OpenConnection();
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken ct = default)
    {
        return await _dataSource.OpenConnectionAsync(ct);
    }
}
