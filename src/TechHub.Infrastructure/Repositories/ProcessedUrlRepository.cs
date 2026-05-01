using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// PostgreSQL implementation of <see cref="IProcessedUrlRepository"/>.
/// Tracks which URLs have been processed to prevent duplicate work.
/// </summary>
public sealed class ProcessedUrlRepository : IProcessedUrlRepository
{
    private readonly IDbConnection _connection;
    private readonly ILogger<ProcessedUrlRepository> _logger;

    public ProcessedUrlRepository(
        IDbConnection connection,
        ILogger<ProcessedUrlRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(logger);

        _connection = connection;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(string externalUrl, CancellationToken ct = default)
    {
        const string Sql = "SELECT EXISTS(SELECT 1 FROM processed_urls WHERE external_url = @ExternalUrl)";

        return await _connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(Sql, new { ExternalUrl = externalUrl }, cancellationToken: ct));
    }

    /// <inheritdoc/>
    public async Task<string?> GetStatusAsync(string externalUrl, CancellationToken ct = default)
    {
        const string Sql = "SELECT status FROM processed_urls WHERE external_url = @ExternalUrl";

        return await _connection.ExecuteScalarAsync<string?>(
            new CommandDefinition(Sql, new { ExternalUrl = externalUrl }, cancellationToken: ct));
    }

    /// <inheritdoc/>
    public async Task<ProcessedUrl?> GetAsync(string externalUrl, CancellationToken ct = default)
    {
        const string Sql = @"
SELECT external_url AS ExternalUrl,
       status AS Status,
       error_message AS ErrorMessage,
       youtube_tags AS YouTubeTags,
       processed_at AS ProcessedAt,
       updated_at AS UpdatedAt
FROM processed_urls
WHERE external_url = @ExternalUrl";

        return await _connection.QuerySingleOrDefaultAsync<ProcessedUrl>(
            new CommandDefinition(Sql, new { ExternalUrl = externalUrl }, cancellationToken: ct));
    }

    /// <inheritdoc/>
    public async Task RecordSuccessAsync(string externalUrl, IReadOnlyList<string>? youtubeTags = null, string? feedName = null, string? collectionName = null, string? reason = null, bool? hasTranscript = null, long? jobId = null, string? slug = null, CancellationToken ct = default)
    {
        const string Sql = @"
INSERT INTO processed_urls (external_url, status, youtube_tags, feed_name, collection_name, reason, has_transcript, job_id, slug)
VALUES (@ExternalUrl, 'succeeded', @YouTubeTags, @FeedName, @CollectionName, @Reason, @HasTranscript, @JobId, @Slug)
ON CONFLICT (external_url) DO UPDATE SET
    status = 'succeeded',
    error_message = NULL,
    youtube_tags = COALESCE(@YouTubeTags, processed_urls.youtube_tags),
    feed_name = COALESCE(@FeedName, processed_urls.feed_name),
    collection_name = COALESCE(@CollectionName, processed_urls.collection_name),
    reason = COALESCE(@Reason, processed_urls.reason),
    has_transcript = COALESCE(@HasTranscript, processed_urls.has_transcript),
    job_id = COALESCE(@JobId, processed_urls.job_id),
    slug = COALESCE(@Slug, processed_urls.slug),
    updated_at = NOW()";

        await _connection.ExecuteAsync(new CommandDefinition(
            Sql,
            new
            {
                ExternalUrl = externalUrl,
                YouTubeTags = youtubeTags as string[] ?? youtubeTags?.ToArray(),
                FeedName = feedName,
                CollectionName = collectionName,
                Reason = reason,
                HasTranscript = hasTranscript,
                JobId = jobId,
                Slug = slug
            },
            cancellationToken: ct));
    }

    /// <inheritdoc/>
    public async Task RecordSkippedAsync(string externalUrl, string? feedName = null, string? collectionName = null, string? reason = null, bool? hasTranscript = null, long? jobId = null, string? slug = null, CancellationToken ct = default)
    {
        const string Sql = @"
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, reason, has_transcript, job_id, slug)
VALUES (@ExternalUrl, 'skipped', @FeedName, @CollectionName, @Reason, @HasTranscript, @JobId, @Slug)
ON CONFLICT (external_url) DO UPDATE SET
    status = 'skipped',
    error_message = NULL,
    feed_name = COALESCE(@FeedName, processed_urls.feed_name),
    collection_name = COALESCE(@CollectionName, processed_urls.collection_name),
    reason = COALESCE(@Reason, processed_urls.reason),
    has_transcript = COALESCE(@HasTranscript, processed_urls.has_transcript),
    job_id = COALESCE(@JobId, processed_urls.job_id),
    slug = COALESCE(@Slug, processed_urls.slug),
    updated_at = NOW()";

        await _connection.ExecuteAsync(new CommandDefinition(
            Sql,
            new
            {
                ExternalUrl = externalUrl,
                FeedName = feedName,
                CollectionName = collectionName,
                Reason = reason,
                HasTranscript = hasTranscript,
                JobId = jobId,
                Slug = slug
            },
            cancellationToken: ct));
    }

    /// <inheritdoc/>
    public async Task RecordFailureAsync(string externalUrl, string errorMessage, string? feedName = null, string? collectionName = null, string? reason = null, bool? hasTranscript = null, long? jobId = null, string? slug = null, CancellationToken ct = default)
    {
        const string Sql = @"
INSERT INTO processed_urls (external_url, status, error_message, feed_name, collection_name, reason, has_transcript, job_id, slug)
VALUES (@ExternalUrl, 'failed', @ErrorMessage, @FeedName, @CollectionName, @Reason, @HasTranscript, @JobId, @Slug)
ON CONFLICT (external_url) DO UPDATE SET
    status = 'failed',
    error_message = @ErrorMessage,
    feed_name = COALESCE(@FeedName, processed_urls.feed_name),
    collection_name = COALESCE(@CollectionName, processed_urls.collection_name),
    reason = COALESCE(@Reason, processed_urls.reason),
    has_transcript = COALESCE(@HasTranscript, processed_urls.has_transcript),
    job_id = COALESCE(@JobId, processed_urls.job_id),
    slug = COALESCE(@Slug, processed_urls.slug),
    updated_at = NOW()";

        await _connection.ExecuteAsync(new CommandDefinition(
            Sql,
            new
            {
                ExternalUrl = externalUrl,
                ErrorMessage = errorMessage,
                FeedName = feedName,
                CollectionName = collectionName,
                Reason = reason,
                HasTranscript = hasTranscript,
                JobId = jobId,
                Slug = slug
            },
            cancellationToken: ct));
    }

    /// <inheritdoc/>
    public async Task<PagedResult<ProcessedUrlListItem>> GetPagedAsync(
        int offset,
        int limit,
        string? status = null,
        string? search = null,
        string? feedName = null,
        string? collectionName = null,
        long? jobId = null,
        string? subcollectionName = null,
        CancellationToken ct = default)
    {
        var needsJoin = !string.IsNullOrEmpty(subcollectionName);
        var where = BuildWhereClause(status, search, feedName, collectionName, jobId, subcollectionName);
        var parameters = BuildParameters(status, search, feedName, collectionName, jobId, subcollectionName);
        var joinClause = needsJoin
            ? "LEFT JOIN content_items ci ON ci.collection_name = p.collection_name AND ci.slug = p.slug"
            : string.Empty;

        var countSql = $@"
SELECT COUNT(*)
FROM processed_urls p
{joinClause}
{where}";

        var totalCount = await _connection.ExecuteScalarAsync<int>(
            new CommandDefinition(countSql, parameters, cancellationToken: ct));

        var dataSql = $@"
SELECT p.external_url AS ExternalUrl,
       p.status AS Status,
       p.error_message AS ErrorMessage,
       p.feed_name AS FeedName,
       p.collection_name AS CollectionName,
       p.slug AS Slug,
       p.reason AS Reason,
       p.has_transcript AS HasTranscript,
       p.job_id AS JobId,
       p.processed_at AS ProcessedAt,
       p.updated_at AS UpdatedAt
FROM processed_urls p
{joinClause}
{where}
ORDER BY p.processed_at DESC
LIMIT @Limit OFFSET @Offset";

        parameters.Add("Limit", limit);
        parameters.Add("Offset", offset);

        var items = await _connection.QueryAsync<ProcessedUrlListItem>(
            new CommandDefinition(dataSql, parameters, cancellationToken: ct));

        return new PagedResult<ProcessedUrlListItem>
        {
            Items = items.AsList(),
            TotalCount = totalCount
        };
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteByUrlAsync(string externalUrl, CancellationToken ct = default)
    {
        // Delete the content item first — FK cascade (fk_processed_urls_content_item)
        // automatically removes the linked processed_urls record.
        // The explicit processed_urls DELETE handles orphan records that have no
        // matching content item (e.g. failed items with NULL slug).
        const string Sql = """
            DELETE FROM content_items WHERE external_url = @ExternalUrl;
            DELETE FROM processed_urls WHERE external_url = @ExternalUrl;
            """;

        var deleted = await _connection.ExecuteAsync(
            new CommandDefinition(Sql, new { ExternalUrl = externalUrl }, cancellationToken: ct));

        return deleted > 0;
    }

    /// <inheritdoc/>
    public async Task<int> DeleteAllFailedAsync(CancellationToken ct = default)
    {
        const string Sql = "DELETE FROM processed_urls WHERE status = 'failed'";

        return await _connection.ExecuteAsync(
            new CommandDefinition(Sql, cancellationToken: ct));
    }

    /// <inheritdoc/>
    public async Task PurgeFailedAsync(TimeSpan olderThan, CancellationToken ct = default)
    {
        const string Sql = @"
DELETE FROM processed_urls
WHERE status = 'failed'
  AND processed_at < NOW() - make_interval(secs => @TotalSeconds)";

        var deleted = await _connection.ExecuteAsync(new CommandDefinition(
            Sql,
            new { TotalSeconds = olderThan.TotalSeconds },
            cancellationToken: ct));

        if (deleted > 0)
        {
            _logger.LogInformation("Purged {Count} old failed URL records", deleted);
        }
    }

    /// <inheritdoc/>
    public async Task SeedFromJsonAsync(IEnumerable<string> jsonPaths, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(jsonPaths);

        // Check if we already have records — only seed into an empty table,
        // but always backfill missing collection_name/reason on existing entries.
        var existingCount = await _connection.ExecuteScalarAsync<int>(
            new CommandDefinition("SELECT COUNT(*) FROM processed_urls", cancellationToken: ct));

        if (existingCount > 0)
        {
            _logger.LogInformation("processed_urls table already has {Count} entries. Backfilling missing data.", existingCount);
            await BackfillFromJsonAsync(jsonPaths, ct);
            return;
        }

        var seeded = 0;

        foreach (var jsonPath in jsonPaths)
        {
            if (!File.Exists(jsonPath))
            {
                _logger.LogWarning("Processed URLs JSON file not found at {Path}. Skipping.", jsonPath);
                continue;
            }

            // Determine status from filename: skipped-entries → skipped, processed-entries → succeeded
            var fileName = Path.GetFileNameWithoutExtension(jsonPath);
            var status = fileName.Contains("skipped", StringComparison.OrdinalIgnoreCase) ? "skipped" : "succeeded";

            await using var stream = File.OpenRead(jsonPath);
            using var doc = await System.Text.Json.JsonDocument.ParseAsync(stream, cancellationToken: ct);

            if (doc.RootElement.ValueKind != System.Text.Json.JsonValueKind.Array)
            {
                continue;
            }

            foreach (var entry in doc.RootElement.EnumerateArray())
            {
                var url = entry.TryGetProperty("canonical_url", out var u) ? u.GetString() : null;
                if (string.IsNullOrWhiteSpace(url))
                {
                    continue;
                }

                var reason = entry.TryGetProperty("reason", out var r) ? r.GetString() : null;
                var collection = entry.TryGetProperty("collection", out var c) ? c.GetString() : null;
                DateTimeOffset? timestamp = entry.TryGetProperty("timestamp", out var t)
                    ? DateTimeOffset.TryParse(t.GetString(), out var parsed) ? parsed : null
                    : null;

                await _connection.ExecuteAsync(new CommandDefinition(
                    @"INSERT INTO processed_urls (external_url, status, reason, collection_name, processed_at, updated_at)
                      VALUES (@ExternalUrl, @Status, @Reason, @CollectionName, COALESCE(@ProcessedAt, NOW()), COALESCE(@ProcessedAt, NOW()))
                      ON CONFLICT (external_url) DO NOTHING",
                    new { ExternalUrl = url, Status = status, Reason = reason, CollectionName = collection, ProcessedAt = timestamp },
                    cancellationToken: ct));
                seeded++;
            }

            _logger.LogInformation("Seeded {Count} entries from {FileName}", seeded, fileName);
        }

        _logger.LogInformation("Seeded {Count} processed URLs from JSON files", seeded);
    }

    private async Task BackfillFromJsonAsync(IEnumerable<string> jsonPaths, CancellationToken ct)
    {
        var updated = 0;

        foreach (var jsonPath in jsonPaths)
        {
            if (!File.Exists(jsonPath))
            {
                continue;
            }

            await using var stream = File.OpenRead(jsonPath);
            using var doc = await System.Text.Json.JsonDocument.ParseAsync(stream, cancellationToken: ct);

            if (doc.RootElement.ValueKind != System.Text.Json.JsonValueKind.Array)
            {
                continue;
            }

            // Batch updates using a temporary table for efficiency
            // (avoids 7000+ individual UPDATE round-trips)
            await _connection.ExecuteAsync(new CommandDefinition(
                @"CREATE TEMP TABLE IF NOT EXISTS _backfill_urls (
                    external_url TEXT PRIMARY KEY,
                    collection_name TEXT,
                    reason TEXT
                )",
                cancellationToken: ct));

            foreach (var entry in doc.RootElement.EnumerateArray())
            {
                var url = entry.TryGetProperty("canonical_url", out var u) ? u.GetString() : null;
                if (string.IsNullOrWhiteSpace(url))
                {
                    continue;
                }

                var reason = entry.TryGetProperty("reason", out var r) ? r.GetString() : null;
                var collection = entry.TryGetProperty("collection", out var c) ? c.GetString() : null;

                await _connection.ExecuteAsync(new CommandDefinition(
                    @"INSERT INTO _backfill_urls (external_url, collection_name, reason)
                      VALUES (@Url, @Collection, @Reason)
                      ON CONFLICT (external_url) DO NOTHING",
                    new { Url = url, Collection = collection, Reason = reason },
                    cancellationToken: ct));
            }

            var rows = await _connection.ExecuteAsync(new CommandDefinition(
                @"UPDATE processed_urls p
                  SET collection_name = COALESCE(p.collection_name, b.collection_name),
                      reason = COALESCE(p.reason, b.reason)
                  FROM _backfill_urls b
                  WHERE p.external_url = b.external_url
                    AND (p.collection_name IS NULL OR p.reason IS NULL)",
                cancellationToken: ct));
            updated += rows;

            await _connection.ExecuteAsync(new CommandDefinition(
                "DROP TABLE IF EXISTS _backfill_urls",
                cancellationToken: ct));
        }

        if (updated > 0)
        {
            _logger.LogInformation("Backfilled {Count} processed URL entries with collection/reason from JSON", updated);
        }
    }

    private static string BuildWhereClause(string? status, string? search, string? feedName, string? collectionName, long? jobId = null, string? subcollectionName = null)
    {
        var conditions = new List<string>();

        if (!string.IsNullOrEmpty(status))
        {
            conditions.Add("p.status = @Status");
        }

        if (!string.IsNullOrEmpty(search))
        {
            conditions.Add("p.external_url ILIKE @Search");
        }

        if (!string.IsNullOrEmpty(feedName))
        {
            conditions.Add("p.feed_name = @FeedName");
        }

        if (!string.IsNullOrEmpty(collectionName))
        {
            conditions.Add("p.collection_name = @CollectionName");
        }

        if (jobId.HasValue)
        {
            conditions.Add("p.job_id = @JobId");
        }

        if (!string.IsNullOrEmpty(subcollectionName))
        {
            conditions.Add("ci.subcollection_name ILIKE @SubcollectionName");
        }

        return conditions.Count > 0
            ? "WHERE " + string.Join(" AND ", conditions)
            : string.Empty;
    }

    private static DynamicParameters BuildParameters(string? status, string? search, string? feedName, string? collectionName, long? jobId = null, string? subcollectionName = null)
    {
        var parameters = new DynamicParameters();

        if (!string.IsNullOrEmpty(status))
        {
            parameters.Add("Status", status);
        }

        if (!string.IsNullOrEmpty(search))
        {
            parameters.Add("Search", $"%{search}%");
        }

        if (!string.IsNullOrEmpty(feedName))
        {
            parameters.Add("FeedName", feedName);
        }

        if (!string.IsNullOrEmpty(collectionName))
        {
            parameters.Add("CollectionName", collectionName);
        }

        if (jobId.HasValue)
        {
            parameters.Add("JobId", jobId.Value);
        }

        if (!string.IsNullOrEmpty(subcollectionName))
        {
            parameters.Add("SubcollectionName", subcollectionName);
        }

        return parameters;
    }
}
