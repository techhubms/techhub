using Microsoft.Extensions.Logging;
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
    private readonly ILogger<RoundupRelevanceFilter> _logger;

    public RoundupRelevanceFilter(
        RoundupGeneratorOptions options,
        IOptions<AppSettings> settings,
        ILogger<RoundupRelevanceFilter> logger)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(logger);

        _options = options;
        _settings = settings.Value;
        _logger = logger;
    }

    /// <summary>
    /// Filters articles by relevance: selects high first, adds medium if high count is below
    /// <see cref="RoundupGeneratorOptions.MinHighArticlesPerSection"/>, adds low if total is below
    /// <see cref="RoundupGeneratorOptions.MinTotalArticlesPerSection"/>. Skips empty sections.
    /// </summary>
    public Dictionary<string, IReadOnlyList<RoundupArticle>> Filter(
        IReadOnlyDictionary<string, IReadOnlyList<RoundupArticle>> articlesBySection)
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

            var selected = new List<RoundupArticle>(high);

            if (selected.Count < _options.MinHighArticlesPerSection)
            {
                selected.AddRange(medium);
            }

            if (selected.Count < _options.MinTotalArticlesPerSection)
            {
                selected.AddRange(low);
            }

            if (selected.Count == 0)
            {
                _logger.LogInformation("Section {Section} has no articles after filtering, skipping", displayName);
                continue;
            }

            _logger.LogInformation(
                "Section {Section}: {HighCount} high + {MediumCount} medium + {LowCount} low => {Total} selected",
                displayName, high.Count, selected.Count - high.Count,
                selected.Count - high.Count - medium.Count, selected.Count);

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
}
