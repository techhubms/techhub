using System.Globalization;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Services.RoundupGeneration;

/// <summary>
/// Writes the topics of a section roundup in one AI call, from the key facts of the articles that get a full write-up.
/// </summary>
internal sealed class RoundupNewsWriter
{
    private static readonly Lazy<string> _systemPrompt = RoundupAiHelper.LoadResource("roundup-news-writer-system.md");

    private readonly RoundupAiHelper _aiHelper;
    private readonly AppSettings _settings;
    private readonly RoundupGeneratorOptions _options;
    private readonly ILogger<RoundupNewsWriter> _logger;

    public RoundupNewsWriter(
        RoundupAiHelper aiHelper,
        IOptions<AppSettings> settings,
        RoundupGeneratorOptions options,
        ILogger<RoundupNewsWriter> logger)
    {
        ArgumentNullException.ThrowIfNull(aiHelper);
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _aiHelper = aiHelper;
        _settings = settings.Value;
        _options = options;
        _logger = logger;
    }

    /// <summary>
    /// Writes the section content. Returns <c>null</c> when the AI call failed.
    /// </summary>
    /// <param name="sectionSlug">Section slug, e.g. "github-copilot".</param>
    /// <param name="leadArticles">Articles that get a full write-up, in the order they should be considered.</param>
    /// <param name="previousHeadings">Headings of last week's roundup for this section, or empty.</param>
    /// <param name="weekDescription">Human-readable description of the week.</param>
    /// <param name="writingGuidelines">The writing style guidelines.</param>
    /// <param name="ct">Cancellation token.</param>
    public async Task<string?> WriteAsync(
        string sectionSlug,
        IReadOnlyList<RoundupArticle> leadArticles,
        string previousHeadings,
        string weekDescription,
        string writingGuidelines,
        CancellationToken ct)
    {
        var displayName = _settings.Content.Sections.TryGetValue(sectionSlug, out var sectionConfig)
            ? sectionConfig.Title
            : sectionSlug;

        var systemMessage = _systemPrompt.Value
            .Replace("{WeekDescription}", weekDescription, StringComparison.Ordinal)
            .Replace("{SectionName}", displayName, StringComparison.Ordinal)
            .Replace("{WritingStyleGuidelines}", writingGuidelines, StringComparison.Ordinal);

        var userMessage = BuildUserMessage(displayName, leadArticles, previousHeadings);

        var response = await _aiHelper.CallAiWithRetryAsync(systemMessage, userMessage, "Writer - " + displayName, AiProfile.RoundupNewsWriter, ct);
        if (response is null)
        {
            _logger.LogWarning("Roundup writer: AI call failed for section {Section}", displayName);
        }

        await Task.Delay(TimeSpan.FromSeconds(_options.RateLimitDelaySeconds), ct);
        return response;
    }

    internal static string BuildUserMessage(string displayName, IReadOnlyList<RoundupArticle> articles, string previousHeadings)
    {
        var sb = new StringBuilder();
        sb.AppendLine(CultureInfo.InvariantCulture, $"ARTICLES FOR THE {displayName} SECTION:");
        sb.AppendLine();

        foreach (var article in articles)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"ARTICLE: {article.Title}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"SIGNIFICANCE: {article.Significance}");

            if (!string.Equals(article.Lifecycle, "none", StringComparison.Ordinal))
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"LIFECYCLE: {article.Lifecycle}");
            }

            if (article.Products.Count > 0)
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"PRODUCTS: {string.Join(", ", article.Products)}");
            }

            if (article.KeyFacts.Count > 0)
            {
                sb.AppendLine("KEY FACTS:");
                foreach (var fact in article.KeyFacts)
                {
                    sb.AppendLine(CultureInfo.InvariantCulture, $"- {fact}");
                }
            }
            else
            {
                // Articles whose metadata could not be backfilled still have the older summary.
                sb.AppendLine(CultureInfo.InvariantCulture, $"SUMMARY: {article.Summary}");
            }

            sb.AppendLine(CultureInfo.InvariantCulture, $"LINK: [{article.Title}]({article.ExternalUrl})");
            sb.AppendLine();
        }

        if (!string.IsNullOrWhiteSpace(previousHeadings))
        {
            sb.AppendLine("LAST WEEK'S HEADINGS FOR THIS SECTION (context only):");
            sb.AppendLine(previousHeadings);
        }

        return sb.ToString();
    }
}
