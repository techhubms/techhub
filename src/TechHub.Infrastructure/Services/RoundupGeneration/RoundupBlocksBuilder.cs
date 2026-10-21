using System.Globalization;
using System.Text;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Services.RoundupGeneration;

/// <summary>
/// Builds the parts of a roundup that code can decide exactly: links that the writer left out, the "Also relevant this week"
/// list of one-line mentions, and the "Dates to know" block from the timeline events.
/// </summary>
internal static class RoundupBlocksBuilder
{
    internal const string AlsoRelevantHeading = "Also relevant this week";
    internal const string DatesToKnowHeading = "Dates to know";

    private const int MaxDatesPerGroup = 10;

    private static readonly HashSet<string> _deadlineKinds = new(StringComparer.Ordinal)
    {
        "deprecation", "retirement", "end-of-support", "breaking-change", "pricing-change"
    };

    private static readonly HashSet<string> _upcomingKinds = new(StringComparer.Ordinal)
    {
        "preview", "ga", "launch", "release"
    };

    /// <summary>
    /// Makes sure every lead article is linked in <paramref name="content"/>. Links the writer left out are added to the
    /// "Other ... News" section, which is created when the writer did not write one.
    /// </summary>
    public static (string Content, IReadOnlyList<RoundupArticle> Appended) EnsureLeadLinks(
        string content,
        IReadOnlyList<RoundupArticle> leadArticles,
        string sectionTitle)
    {
        ArgumentNullException.ThrowIfNull(content);
        ArgumentNullException.ThrowIfNull(leadArticles);

        var missing = leadArticles
            .Where(a => !content.Contains(a.ExternalUrl, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (missing.Count == 0)
        {
            return (content, missing);
        }

        var bullets = string.Join("\n", missing.Select(a => string.Create(CultureInfo.InvariantCulture, $"- [{a.Title}]({a.ExternalUrl})")));

        var lines = content.TrimEnd().Split('\n').ToList();
        var otherIndex = lines.FindLastIndex(l =>
            l.StartsWith("## Other ", StringComparison.OrdinalIgnoreCase)
            && l.TrimEnd().EndsWith(" News", StringComparison.OrdinalIgnoreCase));

        if (otherIndex < 0)
        {
            var heading = string.Create(CultureInfo.InvariantCulture, $"## Other {sectionTitle} News");
            return (string.Join("\n", lines) + $"\n\n{heading}\n\nMore from this week:\n\n{bullets}", missing);
        }

        var nextHeading = lines.FindIndex(otherIndex + 1, l => l.StartsWith("## ", StringComparison.Ordinal));
        var insertAt = nextHeading < 0 ? lines.Count : nextHeading;

        var block = new List<string>();
        if (insertAt > 0 && !string.IsNullOrWhiteSpace(lines[insertAt - 1]))
        {
            block.Add(string.Empty);
        }

        block.AddRange(bullets.Split('\n'));
        if (nextHeading >= 0)
        {
            block.Add(string.Empty);
        }

        lines.InsertRange(insertAt, block);
        return (string.Join("\n", lines), missing);
    }

    /// <summary>
    /// Builds the "Also relevant this week" block: one line per brief article, with its first key fact and a link to the
    /// roundup of its primary section, where it gets the full write-up. Returns an empty string when there are none.
    /// </summary>
    /// <param name="briefArticles">Articles that only get a one-line mention here.</param>
    /// <param name="publishDate">Publish date of the roundups of this week, used to link to the other section's roundup.</param>
    /// <param name="sectionTitles">Display titles by section slug.</param>
    public static string BuildAlsoRelevant(
        IReadOnlyList<RoundupArticle> briefArticles,
        DateOnly publishDate,
        IReadOnlyDictionary<string, string> sectionTitles)
    {
        ArgumentNullException.ThrowIfNull(briefArticles);
        ArgumentNullException.ThrowIfNull(sectionTitles);

        if (briefArticles.Count == 0)
        {
            return string.Empty;
        }

        var sb = new StringBuilder();
        sb.AppendLine(CultureInfo.InvariantCulture, $"## {AlsoRelevantHeading}");
        sb.AppendLine();

        foreach (var article in briefArticles)
        {
            sb.Append(CultureInfo.InvariantCulture, $"- [{article.Title}]({article.ExternalUrl})");

            var fact = article.KeyFacts.Count > 0 ? article.KeyFacts[0] : null;
            if (!string.IsNullOrWhiteSpace(fact))
            {
                sb.Append(": ").Append(EndWithPeriod(fact.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(article.PrimarySectionName))
            {
                var title = sectionTitles.TryGetValue(article.PrimarySectionName, out var t) ? t : article.PrimarySectionName;
                var slug = RoundupContentBuilder.BuildSlug(publishDate, article.PrimarySectionName);
                sb.Append(CultureInfo.InvariantCulture, $" Full story in the [{title} roundup](/{article.PrimarySectionName}/roundups/{slug}).");
            }

            sb.AppendLine();
        }

        return sb.ToString().TrimEnd();
    }

    /// <summary>
    /// Builds the "Dates to know" block from timeline events: deadlines (deprecations, retirements, ...) and upcoming
    /// releases (preview, GA, launch, release), only for dates after the roundup week. Returns an empty string when
    /// there is nothing to report.
    /// </summary>
    public static string BuildDatesToKnow(IReadOnlyList<RoundupDateEvent> events, DateOnly weekEnd)
    {
        ArgumentNullException.ThrowIfNull(events);

        var upcoming = events.Where(e => e.Date > weekEnd).ToList();
        var deadlines = Group(upcoming, _deadlineKinds);
        var coming = Group(upcoming, _upcomingKinds);

        if (deadlines.Count == 0 && coming.Count == 0)
        {
            return string.Empty;
        }

        var sb = new StringBuilder();
        sb.AppendLine(CultureInfo.InvariantCulture, $"## {DatesToKnowHeading}");

        AppendGroup(sb, "Deadlines", deadlines);
        AppendGroup(sb, "Coming up", coming);

        return sb.ToString().TrimEnd();
    }

    internal static string FormatDate(DateOnly date, string precision) => precision switch
    {
        "month" => date.ToString("MMMM yyyy", CultureInfo.InvariantCulture),
        "quarter" => string.Create(CultureInfo.InvariantCulture, $"Q{((date.Month - 1) / 3) + 1} {date.Year}"),
        _ => date.ToString("d MMMM yyyy", CultureInfo.InvariantCulture)
    };

    private static List<RoundupDateEvent> Group(IEnumerable<RoundupDateEvent> events, HashSet<string> kinds) => events
        .Where(e => kinds.Contains(e.Kind))
        .DistinctBy(e => (e.Kind, e.Product.ToUpperInvariant(), (e.Feature ?? string.Empty).ToUpperInvariant(), e.Date))
        .OrderBy(e => e.Date)
        .ThenBy(e => e.Product, StringComparer.OrdinalIgnoreCase)
        .Take(MaxDatesPerGroup)
        .ToList();

    private static void AppendGroup(StringBuilder sb, string title, List<RoundupDateEvent> events)
    {
        if (events.Count == 0)
        {
            return;
        }

        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"### {title}");
        sb.AppendLine();

        foreach (var e in events)
        {
            var subject = string.IsNullOrWhiteSpace(e.Feature) ? e.Product : $"{e.Product} {e.Feature}";
            sb.AppendLine(CultureInfo.InvariantCulture,
                $"- **{FormatDate(e.Date, e.DatePrecision)}**: {subject}: {EndWithPeriod(e.Description.Trim())} ([{e.SourceTitle}]({e.SourceUrl}))");
        }
    }

    private static string EndWithPeriod(string text) =>
        text.EndsWith('.') || text.EndsWith('!') || text.EndsWith('?') ? text : text + ".";
}
