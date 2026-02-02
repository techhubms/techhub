using System.Data;
using Microsoft.Data.Sqlite;
using Npgsql;
using TechHub.Core.Interfaces;

namespace TechHub.Infrastructure.Data;

/// <summary>
/// Factory for creating SQLite database connections.
/// Each connection is independent and can be used concurrently.
/// </summary>
public class SqliteConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqliteConnectionFactory(string connectionString)
    {
        ArgumentNullException.ThrowIfNull(connectionString);
        _connectionString = connectionString;
        
        // Ensure database directory exists
        EnsureDatabaseDirectoryExists(_connectionString);
    }

    public IDbConnection CreateConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();

        // Enable Write-Ahead Logging (WAL) mode for better write performance
        // WAL allows concurrent reads while writing and prevents journal bloat
        using var cmd = connection.CreateCommand();
        cmd.CommandText = "PRAGMA journal_mode = WAL; PRAGMA synchronous = NORMAL;";
        cmd.ExecuteNonQuery();

        return connection;
    }

    private static void EnsureDatabaseDirectoryExists(string connectionString)
    {
        // Extract database file path from connection string
        var builder = new SqliteConnectionStringBuilder(connectionString);
        var dataSource = builder.DataSource;

        // Skip for in-memory databases
        if (string.IsNullOrEmpty(dataSource) || 
            dataSource.Equals(":memory:", StringComparison.OrdinalIgnoreCase) ||
            dataSource.Contains("Mode=Memory", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        // Get directory path and create if it doesn't exist
        var directory = Path.GetDirectoryName(dataSource);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            // On Unix, set permissions to 755 (rwxr-xr-x) for the directory
            if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
            {
                Directory.CreateDirectory(directory, UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute |
                                                     UnixFileMode.GroupRead | UnixFileMode.GroupExecute |
                                                     UnixFileMode.OtherRead | UnixFileMode.OtherExecute);
            }
            else
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}

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
