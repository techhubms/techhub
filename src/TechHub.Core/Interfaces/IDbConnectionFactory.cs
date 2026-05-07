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

    /// <summary>
    /// Creates and opens a new database connection asynchronously.
    /// The caller is responsible for disposing the connection.
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>An open database connection</returns>
    Task<IDbConnection> CreateConnectionAsync(CancellationToken ct = default);
}
