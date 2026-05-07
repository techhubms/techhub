using System.Data;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Factory for creating database connections.
/// Provides thread-safe connection creation for concurrent operations.
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// Creates and opens a new database connection synchronously.
    /// Use this only when an async call is not possible (e.g. DI factory delegates
    /// that cannot be awaited). Prefer <see cref="CreateConnectionAsync"/> in all
    /// async code paths to avoid blocking the thread pool.
    /// The caller is responsible for disposing the connection.
    /// </summary>
    /// <returns>An open database connection</returns>
    IDbConnection CreateConnection();

    /// <summary>
    /// Creates and opens a new database connection asynchronously.
    /// Prefer this over <see cref="CreateConnection"/> in async code paths.
    /// The caller is responsible for disposing the connection.
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>An open database connection</returns>
    Task<IDbConnection> CreateConnectionAsync(CancellationToken ct = default);
}
