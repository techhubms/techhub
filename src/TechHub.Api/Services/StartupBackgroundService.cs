using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;

namespace TechHub.Api.Services;

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
    private readonly ContentProcessorOptions _options;
    private readonly ILogger<StartupBackgroundService> _logger;

    public StartupBackgroundService(
        IServiceProvider serviceProvider,
        StartupStateService startupState,
        IHostApplicationLifetime hostLifetime,
        IOptions<ContentProcessorOptions> options,
        ILogger<StartupBackgroundService> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _startupState = startupState ?? throw new ArgumentNullException(nameof(startupState));
        _hostLifetime = hostLifetime ?? throw new ArgumentNullException(nameof(hostLifetime));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
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

            // Seed RSS feed configs from JSON if the table is empty (first run)
            var feedRepo = services.GetRequiredService<IRssFeedConfigRepository>();
            var rssFeedsPath = ResolvePath(_options.RssFeedsConfigPath);
            await feedRepo.SeedFromJsonAsync(rssFeedsPath, stoppingToken);
            _logger.LogInformation("✅ RSS feed configs seeded (if table was empty)");

            // Seed processed URLs from legacy JSON files (one-time migration)
            var processedUrlRepo = services.GetRequiredService<IProcessedUrlRepository>();
            var processedUrlPaths = new[]
            {
                ResolvePath("processed-entries.json"),
                ResolvePath("skipped-entries.json")
            };
            await processedUrlRepo.SeedFromJsonAsync(processedUrlPaths, stoppingToken);
            _logger.LogInformation("✅ Processed URLs seeded (if table was empty)");

            // Mark startup complete — content is already in the database
            _startupState.MarkStartupCompleted();

            // Log database record counts for verification
            LogDatabaseRecordCounts(services);
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
