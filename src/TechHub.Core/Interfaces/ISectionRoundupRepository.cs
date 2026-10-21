using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Repository for querying articles accumulated for weekly roundup generation
/// and persisting generated roundups.
/// </summary>
public interface ISectionRoundupRepository
{
    /// <summary>
    /// Returns all articles from <c>content_items</c> created during the given week that belong to a specific section flag
    /// (for example <c>is_devops</c> for "devops"), ordered by creation time.
    /// </summary>
    /// <param name="sectionName">Section slug (for example "devops").</param>
    /// <param name="weekStart">Monday of the ISO week.</param>
    /// <param name="weekEnd">Sunday of the ISO week (inclusive).</param>
    /// <param name="ct">Cancellation token.</param>
    Task<IReadOnlyList<RoundupArticle>> GetArticlesForSectionForWeekAsync(
        string sectionName,
        DateOnly weekStart,
        DateOnly weekEnd,
        CancellationToken ct = default);

    /// <summary>
    /// Returns the dated lifecycle events that articles of the given section, created during the week, state for
    /// dates after the week (deadlines and upcoming releases). Ordered by event date.
    /// </summary>
    Task<IReadOnlyList<RoundupDateEvent>> GetUpcomingDateEventsAsync(
        string sectionName,
        DateOnly weekStart,
        DateOnly weekEnd,
        CancellationToken ct = default);

    /// <summary>
    /// Checks whether a roundup with the given slug already exists in the database.
    /// </summary>
    Task<bool> RoundupExistsAsync(string sectionName, string slug, CancellationToken ct = default);

    /// <summary>
    /// Loads the content of the most recent roundup published before the given week start date.
    /// Returns <c>null</c> if no previous roundup exists.
    /// </summary>
    Task<string?> GetPreviousRoundupContentAsync(string sectionName, DateOnly weekStart, CancellationToken ct = default);

    /// <summary>
    /// Writes a generated roundup to <c>content_items</c>, <c>content_tags_expanded</c>,
    /// and <c>processed_urls</c> atomically in a single transaction.
    /// Uses an upsert so re-running for the same week updates the existing record.
    /// </summary>
    Task WriteRoundupAsync(
        string sectionName,
        string slug,
        DateOnly publishDate,
        string title,
        string content,
        string introduction,
        IReadOnlyList<string> tags,
        long? jobId = null,
        CancellationToken ct = default);
}
