using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using TechHub.Infrastructure.Data;
using TechHub.Infrastructure.Services;

namespace TechHub.TestUtilities;

/// <summary>
/// Seeds in-memory test database with markdown files from TestCollections directory.
/// Uses the actual production ContentSyncService to ensure tests run against real sync logic.
/// Provides logging for seeding progress and final record counts.
/// </summary>
public static class TestCollectionsSeeder
{
    /// <summary>
    /// Syncs all markdown files from TestCollections into the database.
    /// Logs seeding progress and final record counts.
    /// </summary>
    /// <param name="connection">Database connection to populate</param>
    /// <param name="testCollectionsPath">Optional path override. Defaults to TestCollections in TestUtilities project.</param>
    /// <param name="loggerFactory">Optional logger factory for creating loggers. Defaults to NullLoggerFactory if not provided.</param>
    public static async Task SeedFromFilesAsync(
        IDbConnection connection,
        string? testCollectionsPath = null,
        ILoggerFactory? loggerFactory = null)
    {
        loggerFactory ??= NullLoggerFactory.Instance;
        var logger = loggerFactory.CreateLogger(typeof(TestCollectionsSeeder));

        // Hardcoded to source directory - no file copying needed
        testCollectionsPath ??= "/workspaces/techhub/tests/TechHub.TestUtilities/TestCollections";

        if (!Directory.Exists(testCollectionsPath))
        {
            throw new DirectoryNotFoundException(
                $"TestCollections directory not found at: {testCollectionsPath}. " +
                "Create test markdown files in this source directory.");
        }

        logger.LogInformation("🌱 Seeding database from TestCollections: {Path}", testCollectionsPath);

        var syncOptions = Options.Create(new ContentSyncOptions
        {
            Enabled = true,
            ForceFullSync = true,
            MaxParallelFiles = 1 // Sequential for predictable test execution
        });

        var contentOptions = Options.Create(new ContentOptions
        {
            CollectionsPath = testCollectionsPath
        });

        var dialect = new PostgresDialect();

        var markdownService = new MarkdownService();
        var syncLogger = loggerFactory.CreateLogger<ContentSyncService>();
        var syncService = new ContentSyncService(connection, markdownService, syncLogger, dialect, syncOptions, contentOptions);

        // Use actual production sync logic - ensures tests validate real code path
        var syncResult = await syncService.SyncAsync(CancellationToken.None);

        logger.LogInformation(
            "✅ Content sync complete: {Added} added, {Updated} updated, {Deleted} deleted ({Duration:N0}ms)",
            syncResult.Added, syncResult.Updated, syncResult.Deleted, syncResult.Duration.TotalMilliseconds);

        // Log record counts for each table
        await LogTableRecordCountsAsync(connection, logger);
    }

    /// <summary>
    /// Logs record counts for all database tables.
    /// Useful for verifying test database state and debugging.
    /// </summary>
    /// <param name="connection">Database connection to query</param>
    /// <param name="logger">Logger for output</param>
    public static async Task LogTableRecordCountsAsync(IDbConnection connection, ILogger logger)
    {
        var tables = new[]
        {
            ("content_items", "content items"),
            ("content_tags_expanded", "expanded tags"),
            ("sync_metadata", "sync metadata entries")
        };

        logger.LogInformation("📊 Database record counts:");

        foreach (var (tableName, description) in tables)
        {
            var count = await connection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM {tableName}");
            logger.LogInformation("   - {TableName}: {Count} {Description}", tableName, count, description);
        }
    }
}
