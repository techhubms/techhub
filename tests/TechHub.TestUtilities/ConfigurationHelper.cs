using Microsoft.Extensions.Configuration;
using TechHub.Core.Configuration;

namespace TechHub.TestUtilities;

/// <summary>
/// Helper for loading configuration in tests from actual appsettings.json files
/// </summary>
public static class ConfigurationHelper
{
    /// <summary>
    /// Default BaseUrl for tests (used when not specified in config)
    /// </summary>
    public const string DefaultTestBaseUrl = "https://localhost:5003";

    /// <summary>
    /// Load AppSettings from appsettings.json with optional overrides for test paths
    /// </summary>
    /// <param name="overrideCollectionsPath">Override the collections path (e.g., for test collections)</param>
    /// <param name="overrideBaseUrl">Override the base URL for RSS feed tests</param>
    /// <returns>AppSettings populated from appsettings.json</returns>
    public static AppSettings LoadAppSettings(string? overrideCollectionsPath = null, string? overrideBaseUrl = DefaultTestBaseUrl)
    {
        // Load from API's appsettings.json (has all the real configuration)
        var apiProjectPath = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "../../../../../src/TechHub.Api"));

        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(apiProjectPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

        // Build overrides dictionary
        var overrides = new Dictionary<string, string?>();

        // Override collections path if specified (for test collections)
        if (!string.IsNullOrWhiteSpace(overrideCollectionsPath))
        {
            overrides["AppSettings:Content:CollectionsPath"] = overrideCollectionsPath;
        }

        // Override BaseUrl (defaults to production URL for consistent test expectations)
        if (!string.IsNullOrWhiteSpace(overrideBaseUrl))
        {
            overrides["AppSettings:BaseUrl"] = overrideBaseUrl;
        }

        if (overrides.Count > 0)
        {
            configBuilder.AddInMemoryCollection(overrides);
        }

        var configuration = configBuilder.Build();

        var settings = configuration.GetSection("AppSettings").Get<AppSettings>()
            ?? throw new InvalidOperationException("Failed to load AppSettings from appsettings.json");

        return settings;
    }
}
