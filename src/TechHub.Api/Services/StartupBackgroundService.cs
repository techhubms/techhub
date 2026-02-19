using TechHub.Core.Interfaces;

namespace TechHub.Api.Services;

/// <summary>
/// Runs database migrations and content sync as a background service after Kestrel starts.
/// This ensures the health endpoint is reachable during startup, allowing Aspire's WaitFor
/// to properly gate dependent services (like the web frontend) until startup completes.
/// FAIL-FAST: If migrations or content sync fail, the application is stopped immediately.
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
            _startupState.MarkMigrationsCompleted();

            // Synchronize content from markdown files to database
            var contentSyncService = services.GetRequiredService<IContentSyncService>();
            var syncResult = await contentSyncService.SyncAsync(stoppingToken);
            _logger.LogInformation("✅ Content sync: {Added} added, {Updated} updated, {Deleted} deleted, {Unchanged} unchanged ({Duration}ms)",
                syncResult.Added, syncResult.Updated, syncResult.Deleted, syncResult.Unchanged, syncResult.Duration.TotalMilliseconds);
            _startupState.MarkContentSyncCompleted();

            // Log database record counts for verification
            LogDatabaseRecordCounts(services);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "💥 CRITICAL: Startup operations failed. Application cannot continue without content. Shutting down...");
            
            // Fail fast: Stop the application immediately
            // Without content, the application is useless
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
            ("sync_metadata", "sync metadata entries")
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
