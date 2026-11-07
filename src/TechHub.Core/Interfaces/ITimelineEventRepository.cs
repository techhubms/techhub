using TechHub.Core.Models;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Stores the dated lifecycle events (preview, GA, deprecation, ...) that articles state.
/// </summary>
public interface ITimelineEventRepository
{
    /// <summary>Version of the extraction prompt and rules (events and products). Bump it to let the backfill job re-extract older articles.</summary>
    const int CurrentPromptVersion = 9;

    /// <summary>Events were extracted while the article was processed (the source text was available).</summary>
    const string ExtractedFromSource = "source";

    /// <summary>Events were extracted later from the stored article text by the backfill job.</summary>
    const string ExtractedFromStoredContent = "stored-content";

    /// <summary>
    /// Events that come from the catalog, not from an extraction: the dated features of the former GitHub Copilot features page.
    /// Replacing the events of an article, and editing its text, leave them alone.
    /// </summary>
    const string ExtractedFromCatalog = "catalog";

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
    /// Like <see cref="ReplaceEventsAsync"/>, but only when the article is still at <paramref name="expectedRevision"/>
    /// (<see cref="TimelineBackfillItem.Revision"/>), checked under the article's write lock. Returns <c>false</c> and
    /// changes nothing when the article was edited after it was read, so events from outdated text are never stored.
    /// When <paramref name="products"/> is set, the products of the article are replaced in the same transaction.
    /// </summary>
    Task<bool> ReplaceEventsIfUnchangedAsync(
        string collectionName,
        string slug,
        IReadOnlyList<TimelineEvent> events,
        string extractedFrom,
        int promptVersion,
        long expectedRevision,
        IReadOnlyList<string>? products = null,
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

    /// <summary>
    /// Gets the timeline of a section: the events of the articles whose primary section it is (every article for
    /// <c>all</c>, roundups excluded), newest first, with the same event stated by several articles merged into one
    /// entry. Returns nothing for an unknown section.
    /// </summary>
    Task<IReadOnlyList<TimelineEntry>> GetTimelineAsync(TimelineRequest request, CancellationToken ct = default);
}
