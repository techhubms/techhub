using System.Data;
using System.Text.Json;
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
SELECT id, name, url, output_dir AS OutputDir, enabled
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
SELECT id, name, url, output_dir AS OutputDir, enabled
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
SELECT id, name, url, output_dir AS OutputDir, enabled
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
INSERT INTO rss_feed_configs (name, url, output_dir, enabled)
VALUES (@Name, @Url, @OutputDir, @Enabled)
RETURNING id";

        var id = await _connection.ExecuteScalarAsync<long>(
            new CommandDefinition(Sql, new { feed.Name, feed.Url, feed.OutputDir, feed.Enabled }, cancellationToken: ct));

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
    updated_at = NOW()
WHERE id = @Id";

        var rows = await _connection.ExecuteAsync(
            new CommandDefinition(Sql, new { feed.Id, feed.Name, feed.Url, feed.OutputDir, feed.Enabled }, cancellationToken: ct));

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

    /// <inheritdoc/>
    public async Task SeedFromJsonAsync(string jsonPath, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(jsonPath);

        if (!File.Exists(jsonPath))
        {
            _logger.LogWarning("RSS feeds JSON file not found at {Path}. Skipping seed.", jsonPath);
            return;
        }

        // Check if we already have feeds in the database
        var existingCount = await _connection.ExecuteScalarAsync<int>(
            new CommandDefinition("SELECT COUNT(*) FROM rss_feed_configs", cancellationToken: ct));

        if (existingCount > 0)
        {
            _logger.LogInformation("RSS feed configs table already has {Count} entries. Skipping seed.", existingCount);
            return;
        }

        await using var stream = File.OpenRead(jsonPath);
        using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);

        var feedsArray = doc.RootElement.ValueKind == JsonValueKind.Array
            ? doc.RootElement
            : doc.RootElement.TryGetProperty("feeds", out var fa) ? fa : default;

        if (feedsArray.ValueKind != JsonValueKind.Array)
        {
            return;
        }

        var seeded = 0;
        foreach (var feedEl in feedsArray.EnumerateArray())
        {
            var name = feedEl.TryGetProperty("name", out var n) ? n.GetString() ?? string.Empty : string.Empty;
            var url = feedEl.TryGetProperty("url", out var u) ? u.GetString() ?? string.Empty : string.Empty;
            var outputDir = feedEl.TryGetProperty("outputDir", out var od) || feedEl.TryGetProperty("output_dir", out od)
                ? od.GetString() ?? string.Empty : string.Empty;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(outputDir))
            {
                continue;
            }

            await _connection.ExecuteAsync(new CommandDefinition(
                @"INSERT INTO rss_feed_configs (name, url, output_dir, enabled)
                  VALUES (@Name, @Url, @OutputDir, TRUE)
                  ON CONFLICT (url) DO NOTHING",
                new { Name = name, Url = url, OutputDir = outputDir },
                cancellationToken: ct));
            seeded++;
        }

        _logger.LogInformation("Seeded {Count} RSS feed configs from {Path}", seeded, jsonPath);
    }
}
