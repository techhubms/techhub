using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// Stores and retrieves RSS feed configurations from PostgreSQL.
/// </summary>
public sealed class RssFeedConfigRepository : IRssFeedConfigRepository
{
    private readonly IDbConnection _connection;
    private readonly ILogger<RssFeedConfigRepository> _logger;

    public RssFeedConfigRepository(
        IDbConnection connection,
        ILogger<RssFeedConfigRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(logger);

        _connection = connection;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<FeedConfig>> GetEnabledAsync(CancellationToken ct = default)
    {
        const string Sql = @"
SELECT id, name, url, output_dir AS OutputDir, enabled,
    polling_interval_minutes AS PollingIntervalMinutes,
    last_checked_at AS LastCheckedAt, last_check_error AS LastCheckError
FROM rss_feed_configs
WHERE enabled = TRUE
ORDER BY name";

        var result = await _connection.QueryAsync<FeedConfig>(
            new CommandDefinition(Sql, cancellationToken: ct));
        return result.ToList();
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<FeedConfig>> GetAllAsync(CancellationToken ct = default)
    {
        const string Sql = @"
SELECT f.id, f.name, f.url, f.output_dir AS OutputDir, f.enabled,
    f.polling_interval_minutes AS PollingIntervalMinutes,
    (EXISTS (SELECT 1 FROM content_items c WHERE c.feed_name = f.name)
     OR EXISTS (SELECT 1 FROM processed_urls p WHERE p.feed_name = f.name)) AS HasContent,
    f.last_checked_at AS LastCheckedAt, f.last_check_error AS LastCheckError,
    (SELECT MAX(c.created_at) FROM content_items c WHERE c.feed_name = f.name) AS LastContentAt
FROM rss_feed_configs f
ORDER BY f.enabled DESC, f.name";

        var result = await _connection.QueryAsync<FeedConfig>(
            new CommandDefinition(Sql, cancellationToken: ct));
        return result.ToList();
    }

    /// <inheritdoc/>
    public async Task<FeedConfig?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        const string Sql = @"
SELECT f.id, f.name, f.url, f.output_dir AS OutputDir, f.enabled,
    f.polling_interval_minutes AS PollingIntervalMinutes,
    (EXISTS (SELECT 1 FROM content_items c WHERE c.feed_name = f.name)
     OR EXISTS (SELECT 1 FROM processed_urls p WHERE p.feed_name = f.name)) AS HasContent,
    f.last_checked_at AS LastCheckedAt, f.last_check_error AS LastCheckError,
    (SELECT MAX(c.created_at) FROM content_items c WHERE c.feed_name = f.name) AS LastContentAt
FROM rss_feed_configs f
WHERE f.id = @Id";

        return await _connection.QuerySingleOrDefaultAsync<FeedConfig>(
            new CommandDefinition(Sql, new { Id = id }, cancellationToken: ct));
    }

    /// <inheritdoc/>
    public async Task<long> CreateAsync(FeedConfig feed, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(feed);

        const string Sql = @"
INSERT INTO rss_feed_configs (name, url, output_dir, enabled, polling_interval_minutes)
VALUES (@Name, @Url, @OutputDir, @Enabled, @PollingIntervalMinutes)
RETURNING id";

        var id = await _connection.ExecuteScalarAsync<long>(
            new CommandDefinition(Sql, new { feed.Name, feed.Url, feed.OutputDir, feed.Enabled, feed.PollingIntervalMinutes }, cancellationToken: ct));

        _logger.LogInformation("Created RSS feed config {FeedId}: {FeedName} ({Url})", id, feed.Name, feed.Url);
        return id;
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateAsync(FeedConfig feed, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(feed);

        const string Sql = @"
UPDATE rss_feed_configs SET
    name       = @Name,
    url        = @Url,
    output_dir = @OutputDir,
    enabled    = @Enabled,
    polling_interval_minutes = @PollingIntervalMinutes,
    updated_at = NOW()
WHERE id = @Id";

        var rows = await _connection.ExecuteAsync(
            new CommandDefinition(Sql, new { feed.Id, feed.Name, feed.Url, feed.OutputDir, feed.Enabled, feed.PollingIntervalMinutes }, cancellationToken: ct));

        return rows > 0;
    }

    /// <inheritdoc/>
    public async Task<bool> RecordCheckAsync(long id, string? errorMessage, CancellationToken ct = default)
    {
        const string Sql = @"
UPDATE rss_feed_configs SET
    last_checked_at  = NOW(),
    last_check_error = @ErrorMessage
WHERE id = @Id";

        var rows = await _connection.ExecuteAsync(
            new CommandDefinition(Sql, new { Id = id, ErrorMessage = errorMessage }, cancellationToken: ct));

        return rows > 0;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(long id, CancellationToken ct = default)
    {
        const string Sql = "DELETE FROM rss_feed_configs WHERE id = @Id";

        var rows = await _connection.ExecuteAsync(
            new CommandDefinition(Sql, new { Id = id }, cancellationToken: ct));

        return rows > 0;
    }
}
