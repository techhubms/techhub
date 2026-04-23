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
SELECT id, name, url, output_dir AS OutputDir, enabled, transcript_mandatory AS TranscriptMandatory
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
SELECT id, name, url, output_dir AS OutputDir, enabled, transcript_mandatory AS TranscriptMandatory
FROM rss_feed_configs
ORDER BY enabled DESC, name";

        var result = await _connection.QueryAsync<FeedConfig>(
            new CommandDefinition(Sql, cancellationToken: ct));
        return result.ToList();
    }

    /// <inheritdoc/>
    public async Task<FeedConfig?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        const string Sql = @"
SELECT id, name, url, output_dir AS OutputDir, enabled, transcript_mandatory AS TranscriptMandatory
FROM rss_feed_configs
WHERE id = @Id";

        return await _connection.QuerySingleOrDefaultAsync<FeedConfig>(
            new CommandDefinition(Sql, new { Id = id }, cancellationToken: ct));
    }

    /// <inheritdoc/>
    public async Task<long> CreateAsync(FeedConfig feed, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(feed);

        const string Sql = @"
INSERT INTO rss_feed_configs (name, url, output_dir, enabled, transcript_mandatory)
VALUES (@Name, @Url, @OutputDir, @Enabled, @TranscriptMandatory)
RETURNING id";

        var id = await _connection.ExecuteScalarAsync<long>(
            new CommandDefinition(Sql, new { feed.Name, feed.Url, feed.OutputDir, feed.Enabled, feed.TranscriptMandatory }, cancellationToken: ct));

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
    transcript_mandatory = @TranscriptMandatory,
    updated_at = NOW()
WHERE id = @Id";

        var rows = await _connection.ExecuteAsync(
            new CommandDefinition(Sql, new { feed.Id, feed.Name, feed.Url, feed.OutputDir, feed.Enabled, feed.TranscriptMandatory }, cancellationToken: ct));

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
