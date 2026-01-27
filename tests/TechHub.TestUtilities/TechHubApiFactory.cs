using System.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Core.Interfaces;
using TechHub.Infrastructure.Data;
using TechHub.Infrastructure.Repositories;

namespace TechHub.TestUtilities;

/// <summary>
/// Abstract base WebApplicationFactory for all API tests.
/// Provides shared configuration and delegates test-specific setup to subclasses.
/// </summary>
public abstract class TechHubApiFactoryBase : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Delegate test-specific configuration to subclasses
        ConfigureTestSpecificServices(builder);
    }

    /// <summary>
    /// Configure test-specific services, configuration, and dependencies.
    /// Implemented by subclasses to provide integration test vs E2E test setup.
    /// </summary>
    /// <param name="builder">The web host builder to configure</param>
    protected abstract void ConfigureTestSpecificServices(IWebHostBuilder builder);
}

/// <summary>
/// Factory for integration tests.
/// - Uses IntegrationTest environment
/// - Uses in-memory SQLite database seeded with TestCollections data
/// - Provides isolated test database for each test run
/// 
/// SQLite Thread Safety:
/// - One master connection is kept open to preserve in-memory database
/// - IDbConnectionFactory creates new connections pointing to same in-memory DB
/// - Repository is transient to ensure thread safety
/// </summary>
public class TechHubIntegrationTestApiFactory : TechHubApiFactoryBase, IDisposable
{
    private SqliteConnection? _masterConnection;
    private ILoggerFactory? _loggerFactory;
    private string? _connectionString;
    private bool _disposed;

    protected override void ConfigureTestSpecificServices(IWebHostBuilder builder)
    {
        // Use IntegrationTest environment for integration tests
        builder.UseEnvironment("IntegrationTest");

        // Create logger for seeding output
        _loggerFactory = LoggerFactory.Create(b =>
        {
            b.AddConsole();
            b.SetMinimumLevel(LogLevel.Information);
        });
        var logger = _loggerFactory.CreateLogger<TechHubIntegrationTestApiFactory>();

        // Create shared in-memory database with a name so multiple connections can access it
        // Using Mode=Memory and Cache=Shared allows multiple connections to the same in-memory DB
        _connectionString = $"Data Source=IntegrationTest_{Guid.NewGuid():N};Mode=Memory;Cache=Shared";
        
        logger.LogInformation("🗄️ Creating in-memory SQLite database: {ConnectionString}", _connectionString);
        
        // Master connection keeps the in-memory database alive
        _masterConnection = new SqliteConnection(_connectionString);
        _masterConnection.Open();

        // Run migrations on the master connection
        var migrationRunner = new MigrationRunner(
            _masterConnection,
            new SqliteDialect(),
            NullLogger<MigrationRunner>.Instance);
        migrationRunner.RunMigrationsAsync().GetAwaiter().GetResult();
        logger.LogInformation("✅ Database migrations completed");

        // Seed with TestCollections data
        TestCollectionsSeeder.SeedFromFilesAsync(_masterConnection, logger: logger).GetAwaiter().GetResult();

        // Capture connection string for factory
        var connString = _connectionString;

        // Replace database services with in-memory SQLite database
        // Use ConfigureTestServices to ensure our registrations override the original ones
        builder.ConfigureTestServices(services =>
        {
            // Remove ALL existing database registrations
            RemoveAllServices<IDbConnectionFactory>(services);
            RemoveAllServices<IDbConnection>(services);
            RemoveAllServices<IContentRepository>(services);
            RemoveAllServices<ISqlDialect>(services);

            // Register SQLite dialect as singleton (stateless)
            services.AddSingleton<ISqlDialect, SqliteDialect>();

            // Register connection factory that creates new connections to the shared in-memory DB
            services.AddSingleton<IDbConnectionFactory>(new SharedMemoryConnectionFactory(connString));

            // Register transient connection - each request gets a new connection for thread safety
            services.AddTransient<IDbConnection>(sp =>
            {
                var factory = sp.GetRequiredService<IDbConnectionFactory>();
                return factory.CreateConnection();
            });

            // Register transient repository - SQLite is not thread-safe
            services.AddTransient<IContentRepository, SqliteContentRepository>();
        });
    }

    private static void RemoveAllServices<T>(IServiceCollection services)
    {
        var descriptors = services.Where(d => d.ServiceType == typeof(T)).ToList();
        foreach (var descriptor in descriptors)
        {
            services.Remove(descriptor);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _masterConnection?.Dispose();
                _loggerFactory?.Dispose();
            }
            _disposed = true;
        }
        base.Dispose(disposing);
    }
}

/// <summary>
/// Connection factory that creates new connections to a shared in-memory SQLite database.
/// Each connection is opened and ready to use.
/// </summary>
internal class SharedMemoryConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SharedMemoryConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }
}

/// <summary>
/// Factory for E2E tests.
/// - Uses Development environment (production-like configuration)
/// - Uses workspace root for content files
/// - Uses real IContentRepository with actual markdown files
/// - Provides full integration with file system
/// </summary>
public class TechHubE2ETestApiFactory : TechHubApiFactoryBase
{
    protected override void ConfigureTestSpecificServices(IWebHostBuilder builder)
    {
        // Use Development environment for E2E tests (closest to production)
        // Default content root (API project directory) is correct - no override needed
        // ASP.NET Core automatically loads appsettings.json and appsettings.Development.json
        // Collections path (../../collections) in appsettings.json resolves correctly
        builder.UseEnvironment("Development");

        // E2E tests use real ContentRepository (no mocking)
    }
}
