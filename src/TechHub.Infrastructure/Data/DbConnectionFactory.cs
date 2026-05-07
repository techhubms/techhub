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

    public IDbConnection CreateConnection()
    {
        return _dataSource.OpenConnection();
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken ct = default)
    {
        return await _dataSource.OpenConnectionAsync(ct);
    }
}
