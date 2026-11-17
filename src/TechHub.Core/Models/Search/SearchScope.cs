namespace TechHub.Core.Models;

/// <summary>
/// The part of the content a search looks at: the sections, the collections and the date window of the request. A word that
/// only occurs outside of it finds nothing, so the typo correction has to judge the words by the same scope as the search.
/// </summary>
/// <param name="Sections">Section names; empty or <c>all</c> means every section.</param>
/// <param name="Collections">Collection names; empty or <c>all</c> means every collection.</param>
/// <param name="DateFrom">Only articles from this moment on, or <c>null</c> for no lower bound.</param>
/// <param name="DateTo">Only articles up to this moment, or <c>null</c> for no upper bound.</param>
public sealed record SearchScope(
    IReadOnlyList<string> Sections,
    IReadOnlyList<string> Collections,
    DateTimeOffset? DateFrom = null,
    DateTimeOffset? DateTo = null)
{
    /// <summary>Every section, collection and date.</summary>
    public static SearchScope Everything { get; } = new([], []);
}
