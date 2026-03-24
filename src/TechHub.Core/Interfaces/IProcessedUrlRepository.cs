using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Tracks which URLs the content processing pipeline has already attempted,
/// preventing duplicate processing across runs.
/// </summary>
public interface IProcessedUrlRepository
{
    /// <summary>Returns <c>true</c> if the URL has already been processed (regardless of outcome).</summary>
    Task<bool> ExistsAsync(string externalUrl, CancellationToken ct = default);

    /// <summary>Gets the processed URL record, or <c>null</c> if the URL has never been processed.</summary>
    Task<ProcessedUrl?> GetAsync(string externalUrl, CancellationToken ct = default);

    /// <summary>Records a successfully processed URL, optionally with YouTube tags.</summary>
    Task RecordSuccessAsync(string externalUrl, IReadOnlyList<string>? youtubeTags = null, CancellationToken ct = default);

    /// <summary>Records a failed processing attempt for a URL.</summary>
    Task RecordFailureAsync(string externalUrl, string errorMessage, CancellationToken ct = default);

    /// <summary>
    /// Deletes old failed records so they can be retried.
    /// Records older than <paramref name="olderThan"/> are removed.
    /// </summary>
    Task PurgeFailedAsync(TimeSpan olderThan, CancellationToken ct = default);
}
