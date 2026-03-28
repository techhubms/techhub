using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using TechHub.Core.Interfaces;
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
    public async Task RecordSuccessAsync(string externalUrl, IReadOnlyList<string>? youtubeTags = null, CancellationToken ct = default)
    {
        const string Sql = @"
INSERT INTO processed_urls (external_url, status, youtube_tags)
VALUES (@ExternalUrl, 'succeeded', @YouTubeTags)
ON CONFLICT (external_url) DO UPDATE SET
    status = 'succeeded',
    error_message = NULL,
    youtube_tags = COALESCE(@YouTubeTags, processed_urls.youtube_tags),
    updated_at = NOW()";

        await _connection.ExecuteAsync(new CommandDefinition(
            Sql,
            new { ExternalUrl = externalUrl, YouTubeTags = youtubeTags as string[] ?? youtubeTags?.ToArray() },
            cancellationToken: ct));
    }

    /// <inheritdoc/>
    public async Task RecordFailureAsync(string externalUrl, string errorMessage, CancellationToken ct = default)
    {
        const string Sql = @"
INSERT INTO processed_urls (external_url, status, error_message)
VALUES (@ExternalUrl, 'failed', @ErrorMessage)
ON CONFLICT (external_url) DO UPDATE SET
    status = 'failed',
    error_message = @ErrorMessage,
    updated_at = NOW()";

        await _connection.ExecuteAsync(new CommandDefinition(
            Sql,
            new { ExternalUrl = externalUrl, ErrorMessage = errorMessage },
            cancellationToken: ct));
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

        // Check if we already have records — only seed into an empty table
        var existingCount = await _connection.ExecuteScalarAsync<int>(
            new CommandDefinition("SELECT COUNT(*) FROM processed_urls", cancellationToken: ct));

        if (existingCount > 0)
        {
            _logger.LogInformation("processed_urls table already has {Count} entries. Skipping seed.", existingCount);
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

                await _connection.ExecuteAsync(new CommandDefinition(
                    @"INSERT INTO processed_urls (external_url, status)
                      VALUES (@ExternalUrl, 'succeeded')
                      ON CONFLICT (external_url) DO NOTHING",
                    new { ExternalUrl = url },
                    cancellationToken: ct));
                seeded++;
            }
        }

        _logger.LogInformation("Seeded {Count} processed URLs from JSON files", seeded);
    }
}
