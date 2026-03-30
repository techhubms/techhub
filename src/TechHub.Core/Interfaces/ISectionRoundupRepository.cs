using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Repository for querying articles accumulated for weekly roundup generation.
/// </summary>
public interface ISectionRoundupRepository
{
    /// <summary>
    /// Returns all articles registered in <c>section_roundup_items</c> for the given week,
    /// grouped by section name (e.g. "github-copilot", "ai", "azure").
    /// Only articles whose <c>content_item</c> exists with AI metadata are returned.
    /// </summary>
    /// <param name="weekStart">Monday of the ISO week (Europe/Brussels time).</param>
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
}
