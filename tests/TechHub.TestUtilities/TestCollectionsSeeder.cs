using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Infrastructure.Data;
using TechHub.Infrastructure.Services;

namespace TechHub.TestUtilities;

/// <summary>
/// Seeds test database with markdown files from TestCollections directory.
/// Uses ContentSyncService (test-only) to parse markdown and insert into the database.
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

        // Resolve from assembly output directory (TestCollections copied via CopyToOutputDirectory)
        testCollectionsPath ??= Path.Combine(
            Path.GetDirectoryName(typeof(TestCollectionsSeeder).Assembly.Location)!,
            "TestCollections");

        if (!Directory.Exists(testCollectionsPath))
        {
            throw new DirectoryNotFoundException(
                $"TestCollections directory not found at: {testCollectionsPath}. " +
                "Create test markdown files in this source directory.");
        }

        logger.LogInformation("🌱 Seeding database from TestCollections: {Path}", testCollectionsPath);

        // Insert test-only feed configs before syncing content.
        // Required because FK constraints on content_items.feed_name and processed_urls.feed_name
        // reference rss_feed_configs.name. Test data uses feed names not present in production seeds.
        await SeedTestFeedConfigsAsync(connection);

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

        // Use ContentSyncService to parse markdown files and insert into database
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

    /// <summary>
    /// Inserts test-only feed configs into rss_feed_configs.
    /// Required because migration 017 adds FK constraints from content_items.feed_name
    /// and processed_urls.feed_name to rss_feed_configs.name.
    /// Test markdown files and test code use feed names not present in production seed data.
    /// </summary>
    private static async Task SeedTestFeedConfigsAsync(IDbConnection connection)
    {
        const string sql = """
            INSERT INTO rss_feed_configs (name, url, output_dir, enabled)
            VALUES (@Name, @Url, @OutputDir, FALSE)
            ON CONFLICT (name) DO NOTHING;
            """;

        var testFeeds = new[]
        {
            // Used in test markdown frontmatter
            new { Name = "Test Feed", Url = "https://test/test-feed", OutputDir = "_blogs" },
            new { Name = "Test Data", Url = "https://test/test-data", OutputDir = "_blogs" },
            new { Name = "Randy Pagels", Url = "https://test/randy-pagels", OutputDir = "_blogs" },
            new { Name = "Thomas Maurer", Url = "https://test/thomas-maurer", OutputDir = "_blogs" },
            // Used in ContentProcessingServiceTests and ProcessedUrlRepositoryTests
            new { Name = "Test", Url = "https://test/test", OutputDir = "_blogs" },
            new { Name = "Broken Feed", Url = "https://test/broken-feed", OutputDir = "_blogs" },
            new { Name = "Broken", Url = "https://test/broken", OutputDir = "_blogs" },
            new { Name = "Good", Url = "https://test/good", OutputDir = "_news" },
            new { Name = "Feed A", Url = "https://test/feed-a", OutputDir = "_blogs" },
            new { Name = "Feed B", Url = "https://test/feed-b", OutputDir = "_news" },
            new { Name = "YT", Url = "https://test/yt", OutputDir = "_videos" },
            new { Name = "YT Mandatory", Url = "https://test/yt-mandatory", OutputDir = "_videos" },
            new { Name = "Blog", Url = "https://test/blog", OutputDir = "_blogs" },
            new { Name = "Some Feed", Url = "https://test/some-feed", OutputDir = "_news" },
            new { Name = "Feed", Url = "https://test/feed", OutputDir = "_blogs" }
        };

        foreach (var feed in testFeeds)
        {
            await connection.ExecuteAsync(sql, feed);
        }
    }
}
