using TechHub.Core.Models;
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

    /// <summary>Returns the processing status of the URL ("succeeded", "skipped", "failed"), or <c>null</c> if not found.</summary>
    Task<string?> GetStatusAsync(string externalUrl, CancellationToken ct = default);

    /// <summary>Gets the processed URL record, or <c>null</c> if the URL has never been processed.</summary>
    Task<ProcessedUrl?> GetAsync(string externalUrl, CancellationToken ct = default);

    /// <summary>Records a successfully processed URL (AI included, written to content_items), optionally with YouTube tags.</summary>
    Task RecordSuccessAsync(string externalUrl, IReadOnlyList<string>? youtubeTags = null, string? feedName = null, string? collectionName = null, string? reason = null, bool? hasTranscript = null, long? jobId = null, string? slug = null, CancellationToken ct = default);

    /// <summary>Records a URL that was skipped by AI categorization (not relevant enough to include).</summary>
    Task RecordSkippedAsync(string externalUrl, string? feedName = null, string? collectionName = null, string? reason = null, bool? hasTranscript = null, long? jobId = null, string? slug = null, CancellationToken ct = default);

    /// <summary>Records a failed processing attempt for a URL.</summary>
    Task RecordFailureAsync(string externalUrl, string errorMessage, string? feedName = null, string? collectionName = null, string? reason = null, bool? hasTranscript = null, long? jobId = null, string? slug = null, CancellationToken ct = default);

    /// <summary>
    /// Gets a paged list of processed URLs with optional filters.
    /// Feed name and collection name are stored directly in processed_urls.
    /// </summary>
    Task<PagedResult<ProcessedUrlListItem>> GetPagedAsync(
        int offset,
        int limit,
        string? status = null,
        string? search = null,
        string? feedName = null,
        string? collectionName = null,
        long? jobId = null,
        CancellationToken ct = default);

    /// <summary>Deletes a processed URL record, its associated content item, and expanded tags so it can be retried on the next run.</summary>
    Task<bool> DeleteByUrlAsync(string externalUrl, CancellationToken ct = default);

    /// <summary>Deletes all failed processed URL records.</summary>
    Task<int> DeleteAllFailedAsync(CancellationToken ct = default);

    /// <summary>
    /// Deletes old failed records so they can be retried.
    /// Records older than <paramref name="olderThan"/> are removed.
    /// </summary>
    Task PurgeFailedAsync(TimeSpan olderThan, CancellationToken ct = default);

    /// <summary>
    /// Seeds processed URLs from JSON files (one-time migration from legacy processing pipeline).
    /// Only seeds if the table is empty. Each JSON file contains an array of objects with a
    /// <c>canonical_url</c> property.
    /// </summary>
    Task SeedFromJsonAsync(IEnumerable<string> jsonPaths, CancellationToken ct = default);
}
