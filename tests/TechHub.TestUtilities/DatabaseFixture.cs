using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Infrastructure.Data;

namespace TechHub.TestUtilities;

/// <summary>
/// Base fixture for integration tests that need a database.
/// Creates an in-memory SQLite database with migrations applied.
/// Implements IDisposable to ensure cleanup after tests.
/// </summary>
/// <typeparam name="T">The test class using this fixture</typeparam>
public class DatabaseFixture<T> : IDisposable
{
    private readonly SqliteConnection _connection;
    private bool _disposed;

    public IDbConnection Connection => _connection;

    public DatabaseFixture()
    {
        // Create in-memory SQLite database (data lives only for the connection lifetime)
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        // Run migrations to create schema
        var migrationRunner = new MigrationRunner(
            _connection,
            new SqliteDialect(),
            NullLogger<MigrationRunner>.Instance);

        migrationRunner.RunMigrationsAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Clears all data from the database, keeping the schema intact.
    /// Useful for resetting state between test methods.
    /// </summary>
    public void ClearData()
    {
        // Delete in reverse dependency order
        using var command = _connection.CreateCommand();
        command.CommandText = @"
            DELETE FROM content_plans;
            DELETE FROM content_sections;
            DELETE FROM content_tags_expanded;
            DELETE FROM content_tags;
            DELETE FROM content_items;
            DELETE FROM collections;
            DELETE FROM sync_metadata;
        ";
        command.ExecuteNonQuery();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _connection?.Dispose();
            }

            _disposed = true;
        }
    }
}
