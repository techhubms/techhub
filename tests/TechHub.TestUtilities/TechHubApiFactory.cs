using System.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TechHub.Api.Services;
using TechHub.Core.Interfaces;
using TechHub.Infrastructure.Data;
using Testcontainers.PostgreSql;
using Xunit;

namespace TechHub.TestUtilities;

/// <summary>
/// Abstract base WebApplicationFactory for all API tests.
/// Provides shared configuration and delegates test-specific setup to subclasses.
/// </summary>
public abstract class TechHubApiFactoryBase : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Use default logging from Program.cs, just suppress console Info logs during tests
        builder.ConfigureLogging(logging =>
        {
            logging.AddFilter<Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider>(
                level => level >= LogLevel.Error);
        });

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
/// Factory for integration tests using PostgreSQL via Testcontainers.
/// - Uses IntegrationTest environment
/// - Spins up an isolated PostgreSQL container per test class
/// - Uses real PostgreSQL — matching production database engine
/// - Container starts eagerly in constructor (sync) since WebApplicationFactory
///   doesn't support async initialization before ConfigureWebHost
/// </summary>
public class TechHubIntegrationTestApiFactory : TechHubApiFactoryBase, IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;

    public TechHubIntegrationTestApiFactory()
    {
        _container = new PostgreSqlBuilder("postgres:17-alpine")
            .WithDatabase("techhub_test")
            .WithUsername("test")
            .WithPassword("test")
            .Build();
    }

    public async ValueTask InitializeAsync()
    {
        // Start the container before any test runs.
        // xUnit calls this before the first test that uses this factory.
        await _container.StartAsync();

        // Force the server to start and wait for background startup (migrations + content sync)
        // to complete before tests run. Without this, tests could fire requests before data is ready.
        using var _ = CreateClient();
        var startupState = Services.GetRequiredService<StartupStateService>();
        await startupState.StartupTask.WaitAsync(TimeSpan.FromSeconds(120));
    }

    public override async ValueTask DisposeAsync()
    {
        // Dispose the base WebApplicationFactory first (stops the test server)
        await base.DisposeAsync();
        // Then dispose the container
        await _container.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    protected override void ConfigureTestSpecificServices(IWebHostBuilder builder)
    {
        // Use IntegrationTest environment for integration tests
        builder.UseEnvironment("IntegrationTest");

        // Resolve TestCollections path dynamically from assembly output directory
        // (avoids hardcoded /workspaces/techhub path that fails in CI)
        var assemblyDir = Path.GetDirectoryName(typeof(TechHubIntegrationTestApiFactory).Assembly.Location)!;
        var testCollectionsPath = Path.Combine(assemblyDir, "TestCollections");
        builder.UseSetting("AppSettings:Content:CollectionsPath", testCollectionsPath);

        // Override database configuration to use the Testcontainers PostgreSQL instance
        builder.UseSetting("Database:Provider", "PostgreSQL");
        builder.UseSetting("Database:ConnectionString", _container.GetConnectionString());

        builder.ConfigureTestServices(services =>
        {
            // Replace connection factory and dialect with PostgreSQL implementations
            RemoveAllServices<IDbConnectionFactory>(services);
            RemoveAllServices<ISqlDialect>(services);

            services.AddSingleton<ISqlDialect, PostgresDialect>();
            services.AddSingleton<IDbConnectionFactory>(
                new PostgresConnectionFactory(_container.GetConnectionString()));
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
