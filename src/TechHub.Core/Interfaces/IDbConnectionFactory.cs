using System.Data;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Factory for creating database connections.
/// Provides thread-safe connection creation for concurrent operations.
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// Creates and opens a new database connection.
    /// The caller is responsible for disposing the connection.
    /// </summary>
    /// <returns>An open database connection</returns>
    IDbConnection CreateConnection();
}
