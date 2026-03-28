using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using TechHub.Core.Models.Admin;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Retrieves database statistics for the admin dashboard.
/// Uses PostgreSQL-specific system views for size and performance data.
/// </summary>
public sealed class DatabaseStatisticsService
{
    private readonly IDbConnection _connection;
    private readonly ILogger<DatabaseStatisticsService> _logger;

    private static readonly TimeZoneInfo BrusselsTimeZone =
        TimeZoneInfo.FindSystemTimeZoneById("Europe/Brussels");

    public DatabaseStatisticsService(
        IDbConnection connection,
        ILogger<DatabaseStatisticsService> logger)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(logger);

        _connection = connection;
        _logger = logger;
    }

    public async Task<DatabaseStatistics> GetStatisticsAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("Fetching database statistics");

        _ = ct; // CancellationToken available for future use

        // IDbConnection is not thread-safe — run queries sequentially
        var total = await GetTotalContentItemsAsync();
        var byCollection = await GetItemsByCollectionAsync();
        var bySection = await GetItemsBySectionAsync();
        var tags = await GetTagStatsAsync();
        var latest = await GetLatestItemsAsync();
        var dbSize = await GetDatabaseSizeAsync();
        var tableSizes = await GetTableSizesAsync();
        var processing = await GetProcessingStatsAsync();
        var slowQueries = await GetSlowQueriesAsync();

        return new DatabaseStatistics
        {
            TotalContentItems = total,
            ItemsByCollection = byCollection,
            ItemsBySection = bySection,
            TotalUniqueTags = tags.uniqueTags,
            TotalTagWords = tags.totalWords,
            LatestItems = latest,
            DatabaseSize = dbSize,
            TableSizes = tableSizes,
            SlowQueries = slowQueries,
            Processing = processing,
            GeneratedAt = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, BrusselsTimeZone)
        };
    }

    private async Task<int> GetTotalContentItemsAsync()
    {
        return await _connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM content_items");
    }

    private async Task<IReadOnlyList<CollectionCount>> GetItemsByCollectionAsync()
    {
        var results = await _connection.QueryAsync<CollectionCount>(
            "SELECT collection_name AS CollectionName, COUNT(*) AS Count FROM content_items GROUP BY collection_name ORDER BY Count DESC");
        return results.ToList();
    }

    private async Task<IReadOnlyList<SectionCount>> GetItemsBySectionAsync()
    {
        // Using the boolean columns for accurate per-section counts (items can be in multiple sections)
        var results = await _connection.QueryAsync<SectionCount>("""
            SELECT section_name AS SectionName, COUNT(*) AS Count FROM (
                SELECT 'AI' AS section_name FROM content_items WHERE is_ai = true
                UNION ALL SELECT 'Azure' FROM content_items WHERE is_azure = true
                UNION ALL SELECT '.NET' FROM content_items WHERE is_dotnet = true
                UNION ALL SELECT 'DevOps' FROM content_items WHERE is_devops = true
                UNION ALL SELECT 'GitHub Copilot' FROM content_items WHERE is_github_copilot = true
                UNION ALL SELECT 'ML' FROM content_items WHERE is_ml = true
                UNION ALL SELECT 'Security' FROM content_items WHERE is_security = true
            ) AS sections
            GROUP BY section_name
            ORDER BY Count DESC
            """);
        return results.ToList();
    }

    private async Task<(int uniqueTags, int totalWords)> GetTagStatsAsync()
    {
        var result = await _connection.QuerySingleAsync<(int uniqueTags, int totalWords)>("""
            SELECT
                (SELECT COUNT(DISTINCT tag_word) FROM content_tags_expanded WHERE is_full_tag = true) AS uniqueTags,
                (SELECT COUNT(*) FROM content_tags_expanded) AS totalWords
            """);
        return result;
    }

    private async Task<IReadOnlyList<RecentItem>> GetLatestItemsAsync()
    {
        var results = await _connection.QueryAsync<RecentItem>("""
            SELECT title AS Title, collection_name AS CollectionName, slug AS Slug,
                   primary_section_name AS PrimarySectionName, date_epoch AS DateEpoch,
                   created_at AS CreatedAt
            FROM content_items
            ORDER BY created_at DESC NULLS LAST, date_epoch DESC
            LIMIT 10
            """);
        return results.ToList();
    }

    private async Task<DatabaseSizeInfo> GetDatabaseSizeAsync()
    {
        var result = await _connection.QuerySingleAsync<DatabaseSizeInfo>("""
            SELECT
                current_database() AS DatabaseName,
                pg_size_pretty(pg_database_size(current_database())) AS TotalSize,
                pg_size_pretty(
                    pg_database_size(current_database()) -
                    COALESCE((SELECT SUM(pg_indexes_size(c.oid)) FROM pg_class c
                     JOIN pg_namespace n ON n.oid = c.relnamespace
                     WHERE n.nspname = 'public' AND c.relkind = 'r'), 0)
                ) AS DataSize,
                pg_size_pretty(
                    COALESCE((SELECT SUM(pg_indexes_size(c.oid)) FROM pg_class c
                     JOIN pg_namespace n ON n.oid = c.relnamespace
                     WHERE n.nspname = 'public' AND c.relkind = 'r'), 0)
                ) AS IndexSize
            """);
        return result;
    }

    private async Task<IReadOnlyList<TableSizeInfo>> GetTableSizesAsync()
    {
        var results = await _connection.QueryAsync<TableSizeInfo>("""
            SELECT
                s.relname AS TableName,
                n_live_tup::int AS RowCount,
                pg_size_pretty(pg_total_relation_size(c.oid)) AS TotalSize,
                pg_size_pretty(pg_relation_size(c.oid)) AS DataSize,
                pg_size_pretty(pg_indexes_size(c.oid)) AS IndexSize
            FROM pg_stat_user_tables s
            JOIN pg_class c ON c.relname = s.relname AND c.relkind = 'r'
            JOIN pg_namespace n ON n.oid = c.relnamespace AND n.nspname = 'public'
            WHERE s.schemaname = 'public'
            ORDER BY pg_total_relation_size(c.oid) DESC
            """);
        return results.ToList();
    }

    private async Task<ProcessingStats> GetProcessingStatsAsync()
    {
        try
        {
            var result = await _connection.QuerySingleAsync<ProcessingStats>("""
                SELECT
                    COUNT(*) AS TotalJobs,
                    COUNT(*) FILTER (WHERE status = 'completed') AS CompletedJobs,
                    COUNT(*) FILTER (WHERE status = 'failed') AS FailedJobs,
                    COALESCE(SUM(items_added), 0) AS TotalItemsAdded,
                    (SELECT COUNT(*) FROM processed_urls) AS TotalProcessedUrls,
                    MAX(started_at) AS LastRunAt
                FROM content_processing_jobs
                """);
            return result;
        }
        catch (Npgsql.PostgresException ex)
        {
            _logger.LogWarning(ex, "Could not fetch processing stats (table may not exist yet)");
            return new ProcessingStats();
        }
    }

    private async Task<IReadOnlyList<SlowQuery>?> GetSlowQueriesAsync()
    {
        try
        {
            // pg_stat_statements may not be enabled
            var results = await _connection.QueryAsync<SlowQuery>("""
                SELECT
                    LEFT(query, 200) AS Query,
                    calls AS Calls,
                    mean_exec_time AS MeanTimeMs,
                    total_exec_time AS TotalTimeMs
                FROM pg_stat_statements
                WHERE dbname = current_database()
                  AND calls > 1
                  AND query NOT LIKE '%pg_stat%'
                ORDER BY mean_exec_time DESC
                LIMIT 10
                """);
            return results.ToList();
        }
        catch (Npgsql.PostgresException ex)
        {
            _logger.LogDebug(ex, "pg_stat_statements not available (extension may not be enabled)");
            return null;
        }
    }
}
