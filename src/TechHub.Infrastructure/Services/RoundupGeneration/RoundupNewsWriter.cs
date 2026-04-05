using System.Globalization;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Services.RoundupGeneration;

/// <summary>
/// Step 3: Creates news-like stories per section using AI.
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
    /// Iterates sections in config order, builds input for each, and calls AI to produce news-like content.
    /// </summary>
    public async Task<string> WriteAsync(
        Dictionary<string, IReadOnlyList<RoundupArticle>> filtered,
        string weekDescription,
        string writingGuidelines,
        CancellationToken ct)
    {
        var systemMessage = _systemPrompt.Value
            .Replace("{WeekDescription}", weekDescription, StringComparison.Ordinal)
            .Replace("{WritingStyleGuidelines}", writingGuidelines, StringComparison.Ordinal);

        var responses = new List<string>();

        foreach (var (sectionSlug, sectionConfig) in _settings.Content.Sections)
        {
            if (sectionSlug.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (!filtered.TryGetValue(sectionSlug, out var articles))
            {
                continue;
            }

            var displayName = sectionConfig.Title;

            _logger.LogInformation("Step 3: Processing section {Section} ({Count} articles)", displayName, articles.Count);

            var sectionInput = BuildSectionInput(displayName, articles);

            var userMessage = string.Create(CultureInfo.InvariantCulture,
                $"ARTICLE ANALYSIS RESULTS FOR {displayName} SECTION TO TRANSFORM INTO NEWS-STYLE CONTENT:\n\n{sectionInput}");

            var response = await _aiHelper.CallAiWithRetryAsync(systemMessage, userMessage, "Step 3 - " + displayName, ct);
            if (response is not null)
            {
                responses.Add(response);
                _logger.LogInformation("Step 3: Section {Section} complete", displayName);
            }
            else
            {
                _logger.LogWarning("Step 3: Section {Section} AI call failed, skipping section", displayName);
            }

            await Task.Delay(TimeSpan.FromSeconds(_options.RateLimitDelaySeconds), ct);
        }

        return string.Join("\n\n", responses);
    }

    internal static string BuildSectionInput(string displayName, IReadOnlyList<RoundupArticle> articles)
    {
        var sb = new StringBuilder();
        sb.AppendLine(string.Create(CultureInfo.InvariantCulture, $"## {displayName}"));
        sb.AppendLine();

        foreach (var article in articles)
        {
            sb.AppendLine(string.Create(CultureInfo.InvariantCulture, $"ARTICLE: {article.Title}"));
            sb.AppendLine(string.Create(CultureInfo.InvariantCulture, $"SUMMARY: {article.Summary}"));
            sb.AppendLine(string.Create(CultureInfo.InvariantCulture, $"RELEVANCE: {article.Relevance}"));

            if (!string.IsNullOrEmpty(article.TopicType))
            {
                sb.AppendLine(string.Create(CultureInfo.InvariantCulture, $"TYPE: {article.TopicType}"));
            }

            if (!string.IsNullOrEmpty(article.ImpactLevel))
            {
                sb.AppendLine(string.Create(CultureInfo.InvariantCulture, $"IMPACT: {article.ImpactLevel}"));
            }

            if (!string.IsNullOrEmpty(article.TimeSensitivity))
            {
                sb.AppendLine(string.Create(CultureInfo.InvariantCulture, $"TIMING: {article.TimeSensitivity}"));
            }

            if (article.KeyTopics.Count > 0)
            {
                sb.AppendLine(string.Create(CultureInfo.InvariantCulture,
                    $"KEY_TOPICS: {string.Join(", ", article.KeyTopics)}"));
            }

            var link = article.IsInternal
                ? string.Create(CultureInfo.InvariantCulture,
                    $"[{article.Title}]({{{{ \"{article.ExternalUrl}\" | relative_url }}}})")
                : string.Create(CultureInfo.InvariantCulture,
                    $"[{article.Title}]({article.ExternalUrl})");

            sb.AppendLine(string.Create(CultureInfo.InvariantCulture, $"LINK: {link}"));
            sb.AppendLine();
        }

        return sb.ToString();
    }
}
