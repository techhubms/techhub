using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Repository for querying articles accumulated for weekly roundup generation.
/// </summary>
public interface ISectionRoundupRepository
{
    /// <summary>
    /// Returns all articles from <c>content_items</c> created during the given week,
    /// grouped by section name (e.g. "github-copilot", "ai", "azure").
    /// Only articles with AI metadata are returned; roundups are excluded.
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
}
