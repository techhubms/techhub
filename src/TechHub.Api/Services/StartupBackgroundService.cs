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

            // Abort any jobs left in 'running' state from a prior crash/restart
            var jobRepo = services.GetRequiredService<IContentProcessingJobRepository>();
            var aborted = await jobRepo.AbortRunningJobsAsync(stoppingToken);
            if (aborted > 0)
            {
                _logger.LogWarning("✅ Aborted {Count} stale running job(s) from prior server instance", aborted);
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
