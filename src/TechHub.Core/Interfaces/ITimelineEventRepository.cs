using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Stores the dated lifecycle events (preview, GA, deprecation, ...) that articles state.
/// </summary>
public interface ITimelineEventRepository
{
    /// <summary>Version of the extraction prompt and rules. Bump it to let the backfill job re-extract older articles.</summary>
    const int CurrentPromptVersion = 1;

    /// <summary>Events were extracted while the article was processed (the source text was available).</summary>
    const string ExtractedFromSource = "source";

    /// <summary>Events were extracted later from the stored article text by the backfill job.</summary>
    const string ExtractedFromStoredContent = "stored-content";

    /// <summary>
    /// Replaces all events of an article and records that it was scanned, in one transaction.
    /// Passing no events removes the previous ones. The article must exist in <c>content_items</c>.
    /// </summary>
    Task ReplaceEventsAsync(
        string collectionName,
        string slug,
        IReadOnlyList<TimelineEvent> events,
        string extractedFrom,
        int promptVersion,
        CancellationToken ct = default);

    /// <summary>
    /// Gets the articles the backfill still has to scan: never scanned, or scanned with an older prompt version
    /// (or all of them when <see cref="TimelineBackfillRequest.Force"/> is set), newest first.
    /// Roundups are never scanned.
    /// </summary>
    Task<IReadOnlyList<TimelineBackfillCandidate>> GetBackfillCandidatesAsync(
        TimelineBackfillRequest request,
        int promptVersion,
        CancellationToken ct = default);

    /// <summary>Loads the stored text of an article for extraction, or <c>null</c> when it no longer exists.</summary>
    Task<TimelineBackfillItem?> GetItemForExtractionAsync(string collectionName, string slug, CancellationToken ct = default);

    /// <summary>Gets the events stored for an article, ordered by date.</summary>
    Task<IReadOnlyList<StoredTimelineEvent>> GetEventsForItemAsync(string collectionName, string slug, CancellationToken ct = default);
}
