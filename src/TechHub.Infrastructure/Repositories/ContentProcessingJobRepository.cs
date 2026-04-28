using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.Extensions.Logging;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.Admin;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// Stores and retrieves content processing job records from PostgreSQL.
/// </summary>
public sealed class ContentProcessingJobRepository : IContentProcessingJobRepository
{
    private readonly IDbConnection _connection;
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<ContentProcessingJobRepository> _logger;

    public ContentProcessingJobRepository(
        IDbConnection connection,
        IDbConnectionFactory connectionFactory,
        ILogger<ContentProcessingJobRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(connectionFactory);
        ArgumentNullException.ThrowIfNull(logger);

        _connection = connection;
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<long> CreateAsync(string triggerType, string jobType = ContentProcessingJobType.ContentProcessing, CancellationToken ct = default)
    {
        const string Sql = @"
INSERT INTO content_processing_jobs (started_at, status, trigger_type, job_type)
VALUES (NOW(), 'running', @TriggerType, @JobType)
RETURNING id";

        var id = await _connection.ExecuteScalarAsync<long>(
            new CommandDefinition(Sql, new { TriggerType = triggerType, JobType = jobType }, cancellationToken: ct));

        _logger.LogDebug("Created processing job {JobId} (trigger: {TriggerType}, type: {JobType})", id, triggerType, jobType);
        return id;
    }

    /// <inheritdoc/>
    public async Task CompleteAsync(
        long jobId, int feedsProcessed, int itemsAdded, int itemsSkipped,
        int errorCount, int transcriptsSucceeded, int transcriptsFailed, string? logOutput, int itemsFixed = 0, CancellationToken ct = default)
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
    transcripts_succeeded = @TranscriptsSucceeded,
    transcripts_failed    = @TranscriptsFailed,
    items_fixed     = @ItemsFixed,
    log_output      = CASE WHEN @LogOutput IS NOT NULL THEN @LogOutput ELSE log_output END
WHERE id = @JobId";

        await _connection.ExecuteAsync(new CommandDefinition(
            Sql,
            new
            {
                JobId = jobId,
                FeedsProcessed = feedsProcessed,
                ItemsAdded = itemsAdded,
                ItemsSkipped = itemsSkipped,
                ErrorCount = errorCount,
                TranscriptsSucceeded = transcriptsSucceeded,
                TranscriptsFailed = transcriptsFailed,
                ItemsFixed = itemsFixed,
                LogOutput = logOutput
            },
            cancellationToken: ct));
    }

    /// <inheritdoc/>
    public async Task FailAsync(long jobId, int feedsProcessed, int itemsAdded, int itemsSkipped, int errorCount, int transcriptsSucceeded, int transcriptsFailed, string? logOutput, int itemsFixed = 0, CancellationToken ct = default)
    {
        const string Sql = @"
UPDATE content_processing_jobs SET
    completed_at    = NOW(),
    duration_ms     = EXTRACT(MILLISECONDS FROM (NOW() - started_at))::BIGINT,
    status          = 'failed',
    feeds_processed = @FeedsProcessed,
    items_added     = @ItemsAdded,
    items_skipped   = @ItemsSkipped,
    error_count     = @ErrorCount,
    transcripts_succeeded = @TranscriptsSucceeded,
    transcripts_failed    = @TranscriptsFailed,
    items_fixed     = @ItemsFixed,
    log_output      = CASE WHEN @LogOutput IS NOT NULL THEN @LogOutput ELSE log_output END
WHERE id = @JobId";

        await _connection.ExecuteAsync(new CommandDefinition(
            Sql,
            new
            {
                JobId = jobId,
                FeedsProcessed = feedsProcessed,
                ItemsAdded = itemsAdded,
                ItemsSkipped = itemsSkipped,
                ErrorCount = errorCount,
                TranscriptsSucceeded = transcriptsSucceeded,
                TranscriptsFailed = transcriptsFailed,
                ItemsFixed = itemsFixed,
                LogOutput = logOutput
            },
            cancellationToken: ct));
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

            // Use an independent connection so this can safely be called concurrently
            // with other database operations on the scoped connection (e.g. Progress<T>
            // callbacks that fire on thread-pool threads while the main pipeline is running).
            using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(new CommandDefinition(
                Sql, new { JobId = jobId, Line = line }, cancellationToken: ct));
        }
        catch (DbException ex)
        {
            // Best-effort: log internally but don't surface the error
            _logger.LogDebug(ex, "Failed to append log line to job {JobId}", jobId);
        }
    }

    /// <inheritdoc/>
    public async Task UpdateLogAsync(long jobId, string logOutput, CancellationToken ct = default)
    {
        try
        {
            const string Sql = @"
UPDATE content_processing_jobs
SET log_output = @LogOutput
WHERE id = @JobId";

            await _connection.ExecuteAsync(new CommandDefinition(
                Sql, new { JobId = jobId, LogOutput = logOutput }, cancellationToken: ct));
        }
        catch (DbException ex)
        {
            _logger.LogDebug(ex, "Failed to update log for job {JobId}", jobId);
        }
    }

    /// <inheritdoc/>
    public async Task<ContentProcessingJob?> GetByIdAsync(long jobId, CancellationToken ct = default)
    {
        const string Sql = @"
SELECT id, started_at AS StartedAt, completed_at AS CompletedAt,
       duration_ms AS DurationMs, status, trigger_type AS TriggerType,
       job_type AS JobType, feeds_processed AS FeedsProcessed,
       items_added AS ItemsAdded, items_skipped AS ItemsSkipped,
       error_count AS ErrorCount,
       transcripts_succeeded AS TranscriptsSucceeded,
       transcripts_failed AS TranscriptsFailed,
       items_fixed AS ItemsFixed,
       log_output AS LogOutput
FROM content_processing_jobs
WHERE id = @JobId";

        return await _connection.QuerySingleOrDefaultAsync<ContentProcessingJob>(
            new CommandDefinition(Sql, new { JobId = jobId }, cancellationToken: ct));
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<ContentProcessingJob>> GetRecentAsync(int count = 20, CancellationToken ct = default)
    {
        const string Sql = @"
SELECT id, started_at AS StartedAt, completed_at AS CompletedAt,
       duration_ms AS DurationMs, status, trigger_type AS TriggerType,
       job_type AS JobType, feeds_processed AS FeedsProcessed,
       items_added AS ItemsAdded, items_skipped AS ItemsSkipped,
       error_count AS ErrorCount,
       transcripts_succeeded AS TranscriptsSucceeded,
       transcripts_failed AS TranscriptsFailed,
       items_fixed AS ItemsFixed
FROM content_processing_jobs
ORDER BY started_at DESC, id DESC
LIMIT @Count";

        var result = await _connection.QueryAsync<ContentProcessingJob>(
            new CommandDefinition(Sql, new { Count = count }, cancellationToken: ct));

        return result.ToList();
    }

    /// <inheritdoc/>
    public async Task UpdateProgressAsync(long jobId, int feedsProcessed, int itemsAdded, int itemsSkipped, int errorCount, int transcriptsSucceeded, int transcriptsFailed, int itemsFixed = 0, CancellationToken ct = default)
    {
        try
        {
            const string Sql = @"
UPDATE content_processing_jobs SET
    feeds_processed = @FeedsProcessed,
    items_added     = @ItemsAdded,
    items_skipped   = @ItemsSkipped,
    error_count     = @ErrorCount,
    transcripts_succeeded = @TranscriptsSucceeded,
    transcripts_failed    = @TranscriptsFailed,
    items_fixed     = @ItemsFixed
WHERE id = @JobId AND status = 'running'";

            await _connection.ExecuteAsync(new CommandDefinition(
                Sql,
                new { JobId = jobId, FeedsProcessed = feedsProcessed, ItemsAdded = itemsAdded, ItemsSkipped = itemsSkipped, ErrorCount = errorCount, TranscriptsSucceeded = transcriptsSucceeded, TranscriptsFailed = transcriptsFailed, ItemsFixed = itemsFixed },
                cancellationToken: ct));
        }
        catch (DbException ex)
        {
            _logger.LogDebug(ex, "Failed to update progress for job {JobId}", jobId);
        }
    }

    /// <inheritdoc/>
    public async Task AbortJobAsync(
        long jobId, int feedsProcessed, int itemsAdded, int itemsSkipped,
        int errorCount, int transcriptsSucceeded, int transcriptsFailed, string? logOutput, int itemsFixed = 0, CancellationToken ct = default)
    {
        const string Sql = @"
UPDATE content_processing_jobs SET
    completed_at    = NOW(),
    duration_ms     = EXTRACT(MILLISECONDS FROM (NOW() - started_at))::BIGINT,
    status          = 'aborted',
    feeds_processed = @FeedsProcessed,
    items_added     = @ItemsAdded,
    items_skipped   = @ItemsSkipped,
    error_count     = @ErrorCount,
    transcripts_succeeded = @TranscriptsSucceeded,
    transcripts_failed    = @TranscriptsFailed,
    items_fixed     = @ItemsFixed,
    log_output      = CASE WHEN @LogOutput IS NOT NULL THEN @LogOutput ELSE log_output END
WHERE id = @JobId AND status = 'running'";

        await _connection.ExecuteAsync(new CommandDefinition(
            Sql,
            new
            {
                JobId = jobId,
                FeedsProcessed = feedsProcessed,
                ItemsAdded = itemsAdded,
                ItemsSkipped = itemsSkipped,
                ErrorCount = errorCount,
                TranscriptsSucceeded = transcriptsSucceeded,
                TranscriptsFailed = transcriptsFailed,
                ItemsFixed = itemsFixed,
                LogOutput = logOutput
            },
            cancellationToken: ct));
    }

    /// <inheritdoc/>
    public async Task<int> AbortRunningJobsAsync(CancellationToken ct = default)
    {
        const string Sql = @"
UPDATE content_processing_jobs SET
    completed_at = NOW(),
    duration_ms  = EXTRACT(MILLISECONDS FROM (NOW() - started_at))::BIGINT,
    status       = 'aborted',
    log_output   = COALESCE(log_output, '') || E'\n[Startup] Job aborted — server was restarted while job was running.'
WHERE status = 'running'";

        var count = await _connection.ExecuteAsync(
            new CommandDefinition(Sql, cancellationToken: ct));

        if (count > 0)
        {
            _logger.LogWarning("Aborted {Count} stale running job(s) from prior server instance", count);
        }

        return count;
    }

    /// <inheritdoc/>
    public async Task PurgeOldJobsAsync(int keepCount, CancellationToken ct = default)
    {
        try
        {
            await _connection.ExecuteAsync(new CommandDefinition(
                @"DELETE FROM content_processing_jobs
                  WHERE id NOT IN (
                      SELECT id FROM content_processing_jobs
                      ORDER BY started_at DESC LIMIT @KeepCount
                  )",
                new { KeepCount = keepCount },
                cancellationToken: ct));
        }
        catch (DbException ex)
        {
            _logger.LogWarning(ex, "Failed to purge old processing jobs");
        }
    }
}
