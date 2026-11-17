using TechHub.Core.Models;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Corrects the typos in a search query. The search matches whole words and word beginnings, so a mistyped word (for
/// example "harnass" for "harness") finds nothing.
/// </summary>
public interface ISearchQueryCorrector
{
    /// <summary>
    /// Replaces every word of the query that no article of the <paramref name="scope"/> contains, not even as the beginning of a
    /// word, with the closest word that articles of the scope do contain. Words that articles of the scope contain are never
    /// touched, so a query that finds something is unchanged. The scope has to be the one of the search itself: a word that
    /// only occurs in an older article, outside the date window of the search, finds nothing and so is a typo too.
    /// Returns <c>null</c> when no word was replaced.
    /// </summary>
    /// <param name="query">The query as typed.</param>
    /// <param name="scope">The sections, collections and dates the search looks at; <c>null</c> is every article.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<string?> CorrectAsync(string query, SearchScope? scope = null, CancellationToken ct = default);
}
