using System.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Npgsql;
using Testcontainers.PostgreSql;
using TechHub.Core.Logging;
using TechHub.Infrastructure.Data;
using Xunit;

namespace TechHub.TestUtilities;

/// <summary>
/// Base fixture for integration tests that need a database.
/// Uses PostgreSQL via Testcontainers — matching production database engine.
/// Implements IAsyncLifetime for async container startup/teardown with xUnit.
/// Each test class gets its own isolated PostgreSQL container instance.
/// </summary>
/// <typeparam name="T">The test class using this fixture</typeparam>
public class DatabaseFixture<T> : IAsyncLifetime
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly PostgreSqlContainer _container;

    public IDbConnection Connection { get; private set; } = null!;

    /// <summary>
    /// The connection string for creating additional connections to the same database.
    /// Useful for WebApplicationFactory or multi-connection scenarios.
    /// </summary>
    public string ConnectionString => _container.GetConnectionString();

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

        _container = new PostgreSqlBuilder("postgres:17-alpine")
            .WithDatabase("techhub_test")
            .WithUsername("test")
            .WithPassword("test")
            .Build();
    }

    public async ValueTask InitializeAsync()
    {
        var logger = _loggerFactory.CreateLogger<DatabaseFixture<T>>();

        // Start the PostgreSQL container
        await _container.StartAsync();
        logger.LogInformation("🐘 Started PostgreSQL container for {TestClass}", typeof(T).Name);

        // Open a persistent connection for this fixture
        var npgsqlConnection = new NpgsqlConnection(_container.GetConnectionString());
        await npgsqlConnection.OpenAsync();
        Connection = npgsqlConnection;

        // Run migrations to create schema
        var migrationRunner = new MigrationRunner(
            Connection,
            new PostgresDialect(),
            NullLogger<MigrationRunner>.Instance);

        await migrationRunner.RunMigrationsAsync();
        logger.LogInformation("✅ Database migrations completed (PostgreSQL)");

        // Seed database with test markdown files using production sync logic
        await TestCollectionsSeeder.SeedFromFilesAsync(Connection, loggerFactory: _loggerFactory);
    }

    /// <summary>
    /// Clears all data from the database, keeping the schema intact.
    /// Useful for resetting state between test methods.
    /// </summary>
    public void ClearData()
    {
        using var command = Connection.CreateCommand();
        command.CommandText = @"
            DELETE FROM content_tags_expanded;
            DELETE FROM content_items;
            DELETE FROM sync_metadata;
        ";
        command.ExecuteNonQuery();
    }

    public async ValueTask DisposeAsync()
    {
        Connection?.Dispose();
        _loggerFactory?.Dispose();
        await _container.DisposeAsync();
    }
}
