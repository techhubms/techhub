using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Services.RoundupGeneration;

/// <summary>
/// Filters roundup article candidates by relevance level per section.
/// </summary>
internal sealed class RoundupRelevanceFilter
{
    private readonly RoundupGeneratorOptions _options;
    private readonly AppSettings _settings;

    public RoundupRelevanceFilter(
        RoundupGeneratorOptions options,
        IOptions<AppSettings> settings)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(settings);

        _options = options;
        _settings = settings.Value;
    }

    /// <summary>
    /// Filters articles by relevance: always includes all high-relevance articles.
    /// If the total is below <see cref="RoundupGeneratorOptions.MinArticlesPerSection"/>,
    /// adds medium articles (ranked by impact and time sensitivity) then low until the minimum is reached.
    /// Skips empty sections.
    /// </summary>
    public Dictionary<string, IReadOnlyList<RoundupArticle>> Filter(
        IReadOnlyDictionary<string, IReadOnlyList<RoundupArticle>> articlesBySection,
        LoggingProgress lp)
    {
        var result = new Dictionary<string, IReadOnlyList<RoundupArticle>>(StringComparer.OrdinalIgnoreCase);

        foreach (var (sectionSlug, articles) in articlesBySection)
        {
            var displayName = GetDisplayName(sectionSlug);

            var high = articles
                .Where(a => a.Relevance.Equals("high", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var medium = articles
                .Where(a => a.Relevance.Equals("medium", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var low = articles
                .Where(a => a.Relevance.Equals("low", StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Always include all high-relevance articles
            var selected = new List<RoundupArticle>(high);

            // Fill up to minimum with medium (ranked by importance), then low
            var remaining = _options.MinArticlesPerSection - selected.Count;

            if (remaining > 0 && medium.Count > 0)
            {
                var rankedMedium = RankByImportance(medium);
                selected.AddRange(rankedMedium.Take(remaining));
                remaining = _options.MinArticlesPerSection - selected.Count;
            }

            if (remaining > 0 && low.Count > 0)
            {
                var rankedLow = RankByImportance(low);
                selected.AddRange(rankedLow.Take(remaining));
            }

            if (selected.Count == 0)
            {
                lp.Report($"Relevance filter — {displayName}: no articles after filtering, skipping section");
                continue;
            }

            var selectedMediumCount = selected.Count(a => a.Relevance.Equals("medium", StringComparison.OrdinalIgnoreCase));
            var selectedLowCount = selected.Count(a => a.Relevance.Equals("low", StringComparison.OrdinalIgnoreCase));
            var skippedMediumCount = medium.Count - selectedMediumCount;
            var skippedLowCount = low.Count - selectedLowCount;

            lp.Report(
                $"Relevance filter — {displayName}: " +
                $"{high.Count} high, {medium.Count} medium, {low.Count} low available " +
                $"=> {selected.Count} selected ({skippedMediumCount} medium + {skippedLowCount} low skipped)");

            result[sectionSlug] = selected;
        }

        return result;
    }

    private string GetDisplayName(string sectionSlug)
    {
        if (_settings.Content.Sections.TryGetValue(sectionSlug, out var config))
        {
            return config.Title;
        }

        return sectionSlug;
    }

    /// <summary>
    /// Ranks articles within a relevance tier by impact level and time sensitivity
    /// so the most important ones are selected first when filling remaining slots.
    /// </summary>
    internal static IOrderedEnumerable<RoundupArticle> RankByImportance(List<RoundupArticle> articles) =>
        articles
            .OrderByDescending(a => ScoreImpactLevel(a.ImpactLevel))
            .ThenByDescending(a => ScoreTimeSensitivity(a.TimeSensitivity))
            .ThenByDescending(a => a.DateEpoch);

    private static int ScoreImpactLevel(string impactLevel) =>
        impactLevel.ToUpperInvariant() switch
        {
            "HIGH" => 3,
            "MEDIUM" => 2,
            "LOW" => 1,
            _ => 0
        };

    private static int ScoreTimeSensitivity(string timeSensitivity) =>
        timeSensitivity.ToUpperInvariant() switch
        {
            "THIS-WEEK" => 3,
            "THIS-MONTH" => 2,
            "EVERGREEN" => 1,
            _ => 0
        };
}
