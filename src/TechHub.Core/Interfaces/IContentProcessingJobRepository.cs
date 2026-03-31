using TechHub.Core.Models.Admin;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Persistence for content processing job history.
/// </summary>
public interface IContentProcessingJobRepository
{
    /// <summary>Creates a new job record and returns the assigned ID.</summary>
    Task<long> CreateAsync(string triggerType, string jobType = ContentProcessingJobType.ContentProcessing, CancellationToken ct = default);

    /// <summary>Marks a job as completed with final statistics.</summary>
    Task CompleteAsync(long jobId, int feedsProcessed, int itemsAdded, int itemsSkipped, int errorCount, string logOutput, CancellationToken ct = default);

    /// <summary>Marks a job as failed, preserving any intermediate statistics.</summary>
    Task FailAsync(long jobId, int feedsProcessed, int itemsAdded, int itemsSkipped, int errorCount, string logOutput, CancellationToken ct = default);

    /// <summary>Appends a log line to a running job (best-effort, no throw on failure).</summary>
    Task AppendLogAsync(long jobId, string line, CancellationToken ct = default);

    /// <summary>Replaces the full log output for a running job (best-effort, for live progress).</summary>
    Task UpdateLogAsync(long jobId, string logOutput, CancellationToken ct = default);

    /// <summary>Gets a specific job by ID, or null if not found.</summary>
    Task<ContentProcessingJob?> GetByIdAsync(long jobId, CancellationToken ct = default);

    /// <summary>Updates live progress counters for a running job (best-effort, no throw on failure).</summary>
    Task UpdateProgressAsync(long jobId, int feedsProcessed, int itemsAdded, int itemsSkipped, int errorCount, CancellationToken ct = default);

    /// <summary>Marks all jobs with status 'running' as 'aborted' (called on startup to clean up stale jobs from prior crashes).</summary>
    Task<int> AbortRunningJobsAsync(CancellationToken ct = default);

    /// <summary>Gets the most recent <paramref name="count"/> jobs, newest first.</summary>
    Task<IReadOnlyList<ContentProcessingJob>> GetRecentAsync(int count = 20, CancellationToken ct = default);
}
