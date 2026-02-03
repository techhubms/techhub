using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Core.Logging;
using TechHub.Infrastructure.Data;

namespace TechHub.TestUtilities;

/// <summary>
/// Base fixture for integration tests that need a database.
/// Always uses in-memory SQLite for fast, isolated integration tests.
/// For E2E tests with real PostgreSQL, use docker-compose instead.
/// Implements IDisposable to ensure cleanup after tests.
/// </summary>
/// <typeparam name="T">The test class using this fixture</typeparam>
public class DatabaseFixture<T> : IDisposable
{
    private readonly ILoggerFactory _loggerFactory;
    private bool _disposed;

    public IDbConnection Connection { get; }

    public DatabaseFixture()
    {
        // Create logger factory for seeding output (file logging only)
        var logPath = Path.Combine(".tmp", "logs", "tests.log");
        var logLevels = new Dictionary<string, LogLevel> { ["Default"] = LogLevel.Information };
        _loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddProvider(new FileLoggerProvider(logPath, logLevels));
            builder.SetMinimumLevel(LogLevel.Information);
        });

        var logger = _loggerFactory.CreateLogger<DatabaseFixture<T>>();

        // SQLite: Create in-memory database (data lives only for the connection lifetime)
        var sqliteConnection = new SqliteConnection("Data Source=:memory:");
        sqliteConnection.Open();
        Connection = sqliteConnection;

        logger.LogInformation("🗄️ Created in-memory SQLite database for {TestClass}", typeof(T).Name);

        // Run migrations to create schema
        var migrationRunner = new MigrationRunner(
            Connection,
            new SqliteDialect(),
            NullLogger<MigrationRunner>.Instance);

        migrationRunner.RunMigrationsAsync().GetAwaiter().GetResult();
        logger.LogInformation("✅ Database migrations completed (SQLite)");

        // Seed database with test markdown files using production sync logic
        TestCollectionsSeeder.SeedFromFilesAsync(Connection, loggerFactory: _loggerFactory).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Clears all data from the database, keeping the schema intact.
    /// Useful for resetting state between test methods.
    /// </summary>
    public void ClearData()
    {
        // Delete in reverse dependency order (SQLite)
        using var command = Connection.CreateCommand();
        command.CommandText = @"
            DELETE FROM content_tags_expanded;
            DELETE FROM content_items;
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
                Connection?.Dispose();
                _loggerFactory?.Dispose();
            }

            _disposed = true;
        }
    }
}
