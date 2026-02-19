using System.Data;
using Npgsql;
using TechHub.Core.Interfaces;

namespace TechHub.Infrastructure.Data;

/// <summary>
/// Factory for creating PostgreSQL database connections.
/// Each connection is independent and can be used concurrently.
/// </summary>
public class PostgresConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public PostgresConnectionFactory(string connectionString)
    {
        ArgumentNullException.ThrowIfNull(connectionString);
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}
