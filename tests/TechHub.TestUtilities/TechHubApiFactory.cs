using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Core.Interfaces;

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
/// - Uses IntegrationTest environment (appsettings.IntegrationTest.json overrides base appsettings.json)
/// - Uses production appsettings.json configuration
/// - Mocks IContentRepository to avoid file system dependencies
/// - Runs from bin directory with relative paths via appsettings.IntegrationTest.json
/// </summary>
public class TechHubIntegrationTestApiFactory : TechHubApiFactoryBase
{
    protected override void ConfigureTestSpecificServices(IWebHostBuilder builder)
    {
        // Use IntegrationTest environment for integration tests
        builder.UseEnvironment("IntegrationTest");

        // Replace real content repository with mock for integration tests
        // No file system access needed - mock provides all test data
        builder.ConfigureServices(services =>
        {
            // Remove the real ContentRepository registration
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IContentRepository));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Register stub repository with test data
            services.AddSingleton<IContentRepository, StubContentRepository>();
        });
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
