using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.Admin;

#pragma warning disable CA1031 // Catch-all intentional: errors must not stop pipeline processing

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// Stores and retrieves content processing job records from PostgreSQL.
/// </summary>
public sealed class ContentProcessingJobRepository : IContentProcessingJobRepository
{
    private readonly IDbConnection _connection;
    private readonly ILogger<ContentProcessingJobRepository> _logger;

    public ContentProcessingJobRepository(
        IDbConnection connection,
        ILogger<ContentProcessingJobRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(logger);

        _connection = connection;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<long> CreateAsync(string triggerType, CancellationToken ct = default)
    {
        const string Sql = @"
INSERT INTO content_processing_jobs (started_at, status, trigger_type)
VALUES (NOW(), 'running', @TriggerType)
RETURNING id";

        var id = await _connection.ExecuteScalarAsync<long>(
            new CommandDefinition(Sql, new { TriggerType = triggerType }, cancellationToken: ct));

        _logger.LogDebug("Created processing job {JobId} (trigger: {TriggerType})", id, triggerType);
        return id;
    }

    /// <inheritdoc/>
    public async Task CompleteAsync(
        long jobId, int feedsProcessed, int itemsAdded, int itemsSkipped,
        int errorCount, string logOutput, CancellationToken ct = default)
    {
        const string Sql = @"
UPDATE content_processing_jobs SET
    completed_at    = NOW(),
    duration_ms     = EXTRACT(MILLISECONDS FROM (NOW() - started_at))::BIGINT,
    status          = 'completed',
    feeds_processed = @FeedsProcessed,
    items_added     = @ItemsAdded,
    items_skipped   = @ItemsSkipped,
    error_count     = @ErrorCount,
    log_output      = @LogOutput
WHERE id = @JobId";

        await _connection.ExecuteAsync(new CommandDefinition(
            Sql,
            new { JobId = jobId, FeedsProcessed = feedsProcessed, ItemsAdded = itemsAdded,
                  ItemsSkipped = itemsSkipped, ErrorCount = errorCount, LogOutput = logOutput },
            cancellationToken: ct));
    }

    /// <inheritdoc/>
    public async Task FailAsync(long jobId, string logOutput, CancellationToken ct = default)
    {
        const string Sql = @"
UPDATE content_processing_jobs SET
    completed_at = NOW(),
    duration_ms  = EXTRACT(MILLISECONDS FROM (NOW() - started_at))::BIGINT,
    status       = 'failed',
    log_output   = @LogOutput
WHERE id = @JobId";

        await _connection.ExecuteAsync(new CommandDefinition(
            Sql, new { JobId = jobId, LogOutput = logOutput }, cancellationToken: ct));
    }

    /// <inheritdoc/>
    public async Task AppendLogAsync(long jobId, string line, CancellationToken ct = default)
    {
        try
        {
            const string Sql = @"
UPDATE content_processing_jobs
SET log_output = COALESCE(log_output, '') || @Line || E'\n'
WHERE id = @JobId";

            await _connection.ExecuteAsync(new CommandDefinition(
                Sql, new { JobId = jobId, Line = line }, cancellationToken: ct));
        }
        catch (Exception ex)
        {
            // Best-effort: log internally but don't surface the error
            _logger.LogDebug(ex, "Failed to append log line to job {JobId}", jobId);
        }
    }

    /// <inheritdoc/>
    public async Task<ContentProcessingJob?> GetByIdAsync(long jobId, CancellationToken ct = default)
    {
        const string Sql = @"
SELECT id, started_at, completed_at, duration_ms, status, trigger_type,
       feeds_processed, items_added, items_skipped, error_count, log_output
FROM content_processing_jobs
WHERE id = @JobId";

        return await _connection.QuerySingleOrDefaultAsync<ContentProcessingJob>(
            new CommandDefinition(Sql, new { JobId = jobId }, cancellationToken: ct));
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<ContentProcessingJob>> GetRecentAsync(int count = 20, CancellationToken ct = default)
    {
        const string Sql = @"
SELECT id, started_at, completed_at, duration_ms, status, trigger_type,
       feeds_processed, items_added, items_skipped, error_count, log_output
FROM content_processing_jobs
ORDER BY started_at DESC
LIMIT @Count";

        var result = await _connection.QueryAsync<ContentProcessingJob>(
            new CommandDefinition(Sql, new { Count = count }, cancellationToken: ct));

        return result.ToList();
    }
}

#pragma warning restore CA1031
