namespace TechHub.Core.Interfaces;

/// <summary>
/// Corrects the typos in a search query. The search matches whole words and word beginnings, so a mistyped word (for
/// example "harnass" for "harness") finds nothing.
/// </summary>
public interface ISearchQueryCorrector
{
    /// <summary>
    /// Replaces every word of the query that no article contains, not even as the beginning of a word, with the closest word
    /// that articles do contain. Words that articles contain are never touched, so a query that finds something is unchanged.
    /// Returns <c>null</c> when no word was replaced.
    /// </summary>
    Task<string?> CorrectAsync(string query, CancellationToken ct = default);
}
