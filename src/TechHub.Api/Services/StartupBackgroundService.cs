using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;

namespace TechHub.Api.Services;

/// <summary>
/// Human-readable descriptions for each custom page key, used when seeding the database.
/// </summary>
static file class CustomPageDescriptions
{
    private static readonly Dictionary<string, string> _map = new(StringComparer.OrdinalIgnoreCase)
    {
        ["dx-space"] = "Developer Experience Space cards and content",
        ["features"] = "GitHub Copilot Features matrix",
        ["genai-advanced"] = "GenAI Advanced module content",
        ["genai-applied"] = "GenAI Applied module content",
        ["genai-basics"] = "GenAI Basics module content",
        ["getting-started"] = "GitHub Copilot Getting Started guide",
        ["handbook"] = "GitHub Copilot Handbook content",
        ["hero-banner"] = "Hero banner cards (announcements)",
        ["levels"] = "Levels of Enlightenment capability model",
        ["sdlc"] = "AI SDLC phase mapping",
        ["tool-tips"] = "GitHub Copilot Tool Tips collection",
    };

    public static string Get(string key) =>
        _map.TryGetValue(key, out var description) ? description : $"{key} custom page data";
}

/// <summary>
/// Runs database migrations and data seeding as a background service after Kestrel starts.
/// This ensures the health endpoint is reachable during startup, allowing Aspire's WaitFor
/// to properly gate dependent services (like the web frontend) until startup completes.
/// FAIL-FAST: If migrations fail, the application is stopped immediately.
/// </summary>
public class StartupBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly StartupStateService _startupState;
    private readonly IHostApplicationLifetime _hostLifetime;
    private readonly ILogger<StartupBackgroundService> _logger;

    public StartupBackgroundService(
        IServiceProvider serviceProvider,
        StartupStateService startupState,
        IHostApplicationLifetime hostLifetime,
        ILogger<StartupBackgroundService> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _startupState = startupState ?? throw new ArgumentNullException(nameof(startupState));
        _hostLifetime = hostLifetime ?? throw new ArgumentNullException(nameof(hostLifetime));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            // Run startup operations in a scope (scoped services require a scope)
            using var scope = _serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            // Run database migrations
            var migrationRunner = services.GetRequiredService<IMigrationRunner>();
            await migrationRunner.RunMigrationsAsync(stoppingToken);
            _logger.LogInformation("✅ Database migrations completed");

            // Seed processed URLs from legacy JSON files (one-time migration)
            var processedUrlRepo = services.GetRequiredService<IProcessedUrlRepository>();
            var processedUrlPaths = new[]
            {
                ResolvePath("processed-entries.json"),
                ResolvePath("skipped-entries.json")
            };
            await processedUrlRepo.SeedFromJsonAsync(processedUrlPaths, stoppingToken);
            _logger.LogInformation("✅ Processed URLs seeded (if table was empty)");

            // Abort any jobs left in 'running' state from a prior crash/restart
            var jobRepo = services.GetRequiredService<IContentProcessingJobRepository>();
            var aborted = await jobRepo.AbortRunningJobsAsync(stoppingToken);
            if (aborted > 0)
            {
                _logger.LogWarning("✅ Aborted {Count} stale running job(s) from prior server instance", aborted);
            }

            // Seed custom page data from collections/_custom/*.json (one-time migration)
            var appSettings = services.GetRequiredService<IOptions<AppSettings>>().Value;
            var customPageRepo = services.GetRequiredService<ICustomPageDataRepository>();
            if (await customPageRepo.IsEmptyAsync(stoppingToken))
            {
                var customDir = Path.Combine(appSettings.Content.CollectionsPath, "_custom");
                if (Directory.Exists(customDir))
                {
                    foreach (var file in Directory.EnumerateFiles(customDir, "*.json"))
                    {
                        var key = Path.GetFileNameWithoutExtension(file);
                        var json = await File.ReadAllTextAsync(file, stoppingToken);
                        var description = CustomPageDescriptions.Get(key);
                        await customPageRepo.UpsertAsync(key, description, json, stoppingToken);
                    }

                    _logger.LogInformation("✅ Custom page data seeded from {Dir}", customDir);
                }
                else
                {
                    _logger.LogWarning("Custom page data directory not found at {Dir}, skipping seed", customDir);
                }
            }
            else
            {
                _logger.LogInformation("✅ Custom page data already present, skipping seed");
            }

            // Mark startup complete — content is already in the database
            _startupState.MarkStartupCompleted();

            // Log database record counts for verification
            LogDatabaseRecordCounts(services);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // Application is shutting down (e.g. dotnet watch restart) — not a startup failure.
            _logger.LogWarning("Startup operations cancelled due to application shutdown");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "💥 CRITICAL: Startup operations failed. Application cannot continue. Shutting down...");

            // Fail fast: Stop the application immediately
            _hostLifetime.StopApplication();

            throw;
        }
    }

    private static string ResolvePath(string path) =>
        Path.IsPathRooted(path) ? path : Path.Join(AppContext.BaseDirectory, path);

    private void LogDatabaseRecordCounts(IServiceProvider services)
    {
        using var connection = services.GetRequiredService<IDbConnectionFactory>().CreateConnection();

        var tables = new (string TableName, string Description)[]
        {
            ("content_items", "content items"),
            ("content_tags_expanded", "expanded tags"),
            ("processed_urls", "processed URLs"),
            ("rss_feed_configs", "RSS feed configs")
        };

        _logger.LogInformation("📊 Database record counts:");

        foreach (var (tableName, description) in tables)
        {
            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT COUNT(*) FROM {tableName}";
            var count = Convert.ToInt32(command.ExecuteScalar(), System.Globalization.CultureInfo.InvariantCulture);
            _logger.LogInformation("   - {TableName}: {Count} {Description}", tableName, count, description);
        }
    }
}
