using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Repository for querying articles accumulated for weekly roundup generation
/// and persisting generated roundups.
/// </summary>
public interface ISectionRoundupRepository
{
    /// <summary>
    /// Returns all articles from <c>content_items</c> created during the given week,
    /// grouped by section name (e.g. "github-copilot", "ai", "azure").
    /// Items missing AI metadata are included with <see cref="RoundupArticle.NeedsAiMetadata"/> set to <c>true</c>.
    /// Roundups are excluded to prevent circular inclusion.
    /// </summary>
    /// <param name="weekStart">Monday of the ISO week.</param>
    /// <param name="weekEnd">Sunday of the ISO week (inclusive).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>
    /// Dictionary keyed by section name, where each value is the ordered list of articles.
    /// Articles are ordered by relevance (high first) then by time sensitivity.
    /// </returns>
    Task<IReadOnlyDictionary<string, IReadOnlyList<RoundupArticle>>> GetArticlesForWeekAsync(
        DateOnly weekStart,
        DateOnly weekEnd,
        CancellationToken ct = default);

    /// <summary>
    /// Checks whether a roundup with the given slug already exists in the database.
    /// </summary>
    Task<bool> RoundupExistsAsync(string slug, CancellationToken ct = default);

    /// <summary>
    /// Loads the content of the most recent roundup published before the given week start date.
    /// Returns <c>null</c> if no previous roundup exists.
    /// </summary>
    Task<string?> GetPreviousRoundupContentAsync(DateOnly weekStart, CancellationToken ct = default);

    /// <summary>
    /// Writes a generated roundup to <c>content_items</c>, <c>content_tags_expanded</c>,
    /// and <c>processed_urls</c> atomically in a single transaction.
    /// Uses an upsert so re-running for the same week updates the existing record.
    /// </summary>
    Task WriteRoundupAsync(
        string slug,
        DateOnly publishDate,
        string title,
        string description,
        string content,
        string introduction,
        IReadOnlyList<string> tags,
        long? jobId = null,
        CancellationToken ct = default);
}
