using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TechHub.E2E.Tests.Api;

/// <summary>
/// Custom WebApplicationFactory for E2E tests with real file system
/// Uses workspace root for content files instead of bin/Debug/net10.0
/// Shared across all API E2E test classes for consistent test environment
/// </summary>
public class ApiTestFactory : WebApplicationFactory<Program>
{
    private readonly string _workspaceRoot;

    public ApiTestFactory()
    {
        // Find workspace root by walking up from test assembly location
        // From: /workspaces/techhub/tests/TechHub.E2E.Tests/bin/Debug/net10.0
        // To:   /workspaces/techhub (5 levels up)
        _workspaceRoot = Path.GetFullPath(
            Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "..")
        );
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Set content root to workspace for file-based content loading
        builder.UseContentRoot(_workspaceRoot);

        // Configure test-specific settings via in-memory configuration
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Keep existing configuration sources but add test overrides
            config.AddJsonFile(Path.Combine(_workspaceRoot, "src", "TechHub.Api", "appsettings.json"), optional: false);

            // Override only the paths that need adjustment for test environment
            var testOverrides = new Dictionary<string, string?>
            {
                ["AppSettings:Content:CollectionsPath"] = Path.Combine(_workspaceRoot, "collections"),
                ["AppSettings:Caching:ContentAbsoluteExpirationMinutes"] = "60",
                ["AppSettings:Caching:ApiResponseAbsoluteExpirationMinutes"] = "60"
            };

            config.AddInMemoryCollection(testOverrides);
        });

        // Suppress verbose logging during tests
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.SetMinimumLevel(LogLevel.Warning);
        });

        builder.UseEnvironment("Test");
    }
}
