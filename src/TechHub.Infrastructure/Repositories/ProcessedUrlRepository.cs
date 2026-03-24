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
  AND processed_at < NOW() - @Interval::INTERVAL";

        var deleted = await _connection.ExecuteAsync(new CommandDefinition(
            Sql,
            new { Interval = olderThan.ToString() },
            cancellationToken: ct));

        if (deleted > 0)
        {
            _logger.LogInformation("Purged {Count} old failed URL records", deleted);
        }
    }
}
