using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Scans already stored articles for timeline events. Resumable: scanned articles are skipped by the next run.
/// </summary>
public interface ITimelineBackfillService
{
    /// <summary>
    /// Runs the backfill. Every message that should show up in the job log is passed to <paramref name="log"/>.
    /// Cancellation stops the run after the current article.
    /// </summary>
    Task<TimelineBackfillResult> RunAsync(TimelineBackfillRequest request, Func<string, Task> log, CancellationToken ct = default);
}
