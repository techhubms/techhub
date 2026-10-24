using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Services.RoundupGeneration;

/// <summary>
/// Splits the articles of a section into stories that get a full write-up and articles that only get a one-line mention,
/// and orders them so the writer sees the most important and related articles together.
/// </summary>
internal static class RoundupArticleSelector
{
    /// <summary>
    /// Splits <paramref name="articles"/> by their role in <paramref name="sectionName"/>.
    /// Articles without a role for the section (metadata that could not be backfilled) are leads:
    /// the worst case is a full write-up of a minor article, never a missing story.
    /// </summary>
    public static (IReadOnlyList<RoundupArticle> Lead, IReadOnlyList<RoundupArticle> Brief) Split(
        IReadOnlyList<RoundupArticle> articles,
        string sectionName)
    {
        ArgumentNullException.ThrowIfNull(articles);
        ArgumentException.ThrowIfNullOrWhiteSpace(sectionName);

        var lead = new List<RoundupArticle>();
        var brief = new List<RoundupArticle>();

        foreach (var article in articles)
        {
            var isBrief = article.SectionRoles.TryGetValue(sectionName, out var role)
                && string.Equals(role, RoundupMetadata.RoleBrief, StringComparison.Ordinal)
                && !string.Equals(article.PrimarySectionName, sectionName, StringComparison.OrdinalIgnoreCase);

            (isBrief ? brief : lead).Add(article);
        }

        return (Order(lead), Order(brief));
    }

    /// <summary>
    /// Orders by significance (high first), then by first product so articles about the same product are adjacent,
    /// then by title for a stable result.
    /// </summary>
    public static IReadOnlyList<RoundupArticle> Order(IEnumerable<RoundupArticle> articles) => articles
        .OrderBy(a => SignificanceRank(a.Significance))
        .ThenBy(a => NormalizeProduct(a.Products.Count > 0 ? a.Products[0] : null), StringComparer.Ordinal)
        .ThenBy(a => a.Title, StringComparer.OrdinalIgnoreCase)
        .ToList();

    internal static string NormalizeProduct(string? product) =>
        string.IsNullOrWhiteSpace(product) ? "\uFFFF" : product.Trim().ToLowerInvariant();

    private static int SignificanceRank(string significance) => significance switch
    {
        "high" => 0,
        "medium" => 1,
        "low" => 2,
        _ => 1
    };
}
