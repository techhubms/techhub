using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Services.RoundupGeneration;

/// <summary>
/// Orchestrates the AI weekly roundup generation pipeline.
/// Delegates each step to a dedicated class while coordinating the overall flow.
/// </summary>
internal sealed class RoundupGeneratorService : IRoundupGeneratorService
{
    private static readonly Lazy<string> _writingGuidelines = RoundupAiHelper.LoadResource("roundup-writing-guidelines.md");

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    private readonly ISectionRoundupRepository _roundupRepo;
    private readonly IContentFixerService _contentFixer;
    private readonly IAiCategorizationService _aiService;
    private readonly IContentItemWriteRepository _writeRepo;
    private readonly RoundupRelevanceFilter _relevanceFilter;
    private readonly RoundupNewsWriter _newsWriter;
    private readonly RoundupNarrativeEnhancer _narrativeEnhancer;
    private readonly RoundupCondenser _condenser;
    private readonly RoundupMetadataGenerator _metadataGenerator;
    private readonly ILogger<RoundupGeneratorService> _logger;

    public RoundupGeneratorService(
        ISectionRoundupRepository roundupRepo,
        IContentFixerService contentFixer,
        IAiCategorizationService aiService,
        IContentItemWriteRepository writeRepo,
        RoundupRelevanceFilter relevanceFilter,
        RoundupNewsWriter newsWriter,
        RoundupNarrativeEnhancer narrativeEnhancer,
        RoundupCondenser condenser,
        RoundupMetadataGenerator metadataGenerator,
        ILogger<RoundupGeneratorService> logger)
    {
        ArgumentNullException.ThrowIfNull(roundupRepo);
        ArgumentNullException.ThrowIfNull(contentFixer);
        ArgumentNullException.ThrowIfNull(aiService);
        ArgumentNullException.ThrowIfNull(writeRepo);
        ArgumentNullException.ThrowIfNull(relevanceFilter);
        ArgumentNullException.ThrowIfNull(newsWriter);
        ArgumentNullException.ThrowIfNull(narrativeEnhancer);
        ArgumentNullException.ThrowIfNull(condenser);
        ArgumentNullException.ThrowIfNull(metadataGenerator);
        ArgumentNullException.ThrowIfNull(logger);

        _roundupRepo = roundupRepo;
        _contentFixer = contentFixer;
        _aiService = aiService;
        _writeRepo = writeRepo;
        _relevanceFilter = relevanceFilter;
        _newsWriter = newsWriter;
        _narrativeEnhancer = narrativeEnhancer;
        _condenser = condenser;
        _metadataGenerator = metadataGenerator;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<RoundupGenerationOutcome> GenerateAsync(DateOnly weekStart, DateOnly weekEnd, IProgress<string>? progress = null, CancellationToken ct = default)
    {
        var publishDate = weekEnd.AddDays(1); // Monday after the week ends
        var slug = RoundupContentBuilder.BuildSlug(publishDate);

        // Deduplication: skip if roundup already exists for this week.
        if (await _roundupRepo.RoundupExistsAsync(slug, ct))
        {
            _logger.LogInformation("Roundup for week {WeekStart} already exists (slug={Slug}), skipping", weekStart, slug);
            return RoundupGenerationOutcome.AlreadyExists;
        }

        _logger.LogInformation("Generating roundup for week {WeekStart}–{WeekEnd}", weekStart, weekEnd);

        var articlesBySection = await _roundupRepo.GetArticlesForWeekAsync(weekStart, weekEnd, ct);
        var totalArticles = articlesBySection.Values.Sum(a => a.Count);
        progress?.Report($"Loaded {totalArticles} articles across {articlesBySection.Count} sections");

        if (articlesBySection.Count == 0)
        {
            _logger.LogWarning("No articles found for week {WeekStart}–{WeekEnd}, skipping roundup generation", weekStart, weekEnd);
            return RoundupGenerationOutcome.NoArticles;
        }

        // On-the-fly AI metadata backfill: categorize any items missing ai_metadata
        articlesBySection = await BackfillMissingAiMetadataAsync(articlesBySection, progress, ct);

        var filtered = _relevanceFilter.Filter(articlesBySection);

        if (filtered.Count == 0)
        {
            _logger.LogWarning("No articles remain after relevance filtering for week {WeekStart}–{WeekEnd}", weekStart, weekEnd);
            progress?.Report("No articles remain after relevance filtering, skipping");
            return RoundupGenerationOutcome.NoArticlesAfterFiltering;
        }

        var filteredTotal = filtered.Values.Sum(a => a.Count);
        progress?.Report($"After relevance filtering: {filteredTotal} articles across {filtered.Count} sections");

        var weekDescription = string.Create(CultureInfo.InvariantCulture,
            $"the week of {weekStart.ToString("MMMM d", CultureInfo.InvariantCulture)} to {weekEnd.ToString("MMMM d, yyyy", CultureInfo.InvariantCulture)}");

        var writingGuidelines = _writingGuidelines.Value;

        _logger.LogInformation("Step 1/5: Creating news-like stories per section");
        progress?.Report("Step 1/5: Creating news-like stories per section");
        var sectionStories = await _newsWriter.WriteAsync(filtered, weekDescription, writingGuidelines, ct);

        _logger.LogInformation("Step 2/5: Adding ongoing narrative");
        progress?.Report("Step 2/5: Adding ongoing narrative");
        var narrativeContent = await _narrativeEnhancer.EnhanceAsync(sectionStories, weekStart, writingGuidelines, ct);

        _logger.LogInformation("Step 3/5: Condensing content");
        progress?.Report("Step 3/5: Condensing content");
        var condensedContent = await _condenser.CondenseAsync(narrativeContent, writingGuidelines, ct);

        _logger.LogInformation("Step 4/5: Generating metadata");
        progress?.Report("Step 4/5: Generating metadata");
        var metadata = await _metadataGenerator.GenerateAsync(condensedContent, weekDescription, writingGuidelines, ct);

        _logger.LogInformation("Step 5/5: Building final content");
        progress?.Report("Step 5/5: Building final content");
        var tableOfContents = RoundupContentBuilder.BuildTableOfContents(condensedContent);
        var fullContent = RoundupContentBuilder.BuildFullContent(condensedContent, metadata.Introduction, tableOfContents);

        // Repair markdown formatting before writing
        fullContent = _contentFixer.RepairMarkdown(fullContent);

        _logger.LogInformation("Writing roundup to database");
        progress?.Report("Writing roundup to database");
        await _roundupRepo.WriteRoundupAsync(slug, publishDate, metadata.Title, metadata.Description, fullContent, metadata.Introduction, metadata.Tags, ct);

        _logger.LogInformation("Roundup for week {WeekStart}–{WeekEnd} written successfully (slug={Slug})", weekStart, weekEnd, slug);
        progress?.Report($"Roundup written successfully (slug={slug})");
        return RoundupGenerationOutcome.Generated(slug);
    }

    /// <summary>
    /// Finds articles missing AI metadata, categorizes them on-the-fly,
    /// updates the database, and returns an updated article dictionary with enriched metadata.
    /// Deduplicates by ExternalUrl so each item is only categorized once even if it appears in multiple sections.
    /// </summary>
    private async Task<IReadOnlyDictionary<string, IReadOnlyList<RoundupArticle>>> BackfillMissingAiMetadataAsync(
        IReadOnlyDictionary<string, IReadOnlyList<RoundupArticle>> articlesBySection,
        IProgress<string>? progress,
        CancellationToken ct)
    {
        // Collect unique articles needing backfill (same article appears in multiple sections)
        var needsBackfill = articlesBySection.Values
            .SelectMany(articles => articles)
            .Where(a => a.NeedsAiMetadata)
            .DistinctBy(a => a.ExternalUrl, StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (needsBackfill.Count == 0)
        {
            return articlesBySection;
        }

        _logger.LogInformation("On-the-fly AI metadata backfill: {Count} items missing ai_metadata", needsBackfill.Count);
        progress?.Report($"AI metadata backfill: {needsBackfill.Count} items missing ai_metadata");

        // Map ExternalUrl → enriched metadata for updating articles across sections
        var enriched = new Dictionary<string, RoundupArticle>(StringComparer.OrdinalIgnoreCase);
        var errorCount = 0;

        foreach (var article in needsBackfill)
        {
            ct.ThrowIfCancellationRequested();

            try
            {
                var rawItem = new RawFeedItem
                {
                    Title = article.Title,
                    ExternalUrl = article.ExternalUrl,
                    PublishedAt = DateTimeOffset.FromUnixTimeSeconds(article.DateEpoch),
                    FeedName = article.FeedName,
                    CollectionName = article.CollectionName,
                    FullContent = article.Content
                };

                var result = await _aiService.CategorizeAsync(rawItem, ct);
                if (result.Item?.RoundupMetadata is { } meta)
                {
                    // Update only ai_metadata on the existing row (avoids PK/unique-constraint conflicts)
                    var aiMetadataJson = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        roundup_summary = meta.Summary,
                        key_topics = meta.KeyTopics,
                        roundup_relevance = meta.Relevance,
                        topic_type = meta.TopicType,
                        impact_level = meta.ImpactLevel,
                        time_sensitivity = meta.TimeSensitivity
                    }, _jsonOptions);

                    await _writeRepo.UpdateAiMetadataAsync(article.CollectionName, article.Slug, aiMetadataJson, ct);

                    enriched[article.ExternalUrl] = new RoundupArticle
                    {
                        SectionName = article.SectionName,
                        Title = article.Title,
                        ExternalUrl = article.ExternalUrl,
                        Slug = article.Slug,
                        CollectionName = article.CollectionName,
                        IsInternal = article.IsInternal,
                        Content = article.Content,
                        FeedName = article.FeedName,
                        DateEpoch = article.DateEpoch,
                        NeedsAiMetadata = false,
                        Summary = meta.Summary,
                        KeyTopics = meta.KeyTopics,
                        Relevance = meta.Relevance,
                        TopicType = meta.TopicType,
                        ImpactLevel = meta.ImpactLevel,
                        TimeSensitivity = meta.TimeSensitivity
                    };

                    _logger.LogDebug("AI metadata backfilled for {Slug} in {Collection}", article.Slug, article.CollectionName);
                }
                else
                {
                    _logger.LogDebug("AI categorization returned no metadata for {Slug} — using defaults", article.Slug);
                }
            }
#pragma warning disable CA1031 // Best-effort: continue with other articles if one fails
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                errorCount++;
                _logger.LogWarning(ex, "Failed to backfill AI metadata for {Slug} in {Collection}", article.Slug, article.CollectionName);
            }
#pragma warning restore CA1031
        }

        if (enriched.Count == 0 && errorCount == 0)
        {
            return articlesBySection;
        }

        _logger.LogInformation("AI metadata backfill completed: {Enriched}/{Total} items enriched, {Errors} errors",
            enriched.Count, needsBackfill.Count, errorCount);
        progress?.Report($"AI metadata backfill: {enriched.Count}/{needsBackfill.Count} enriched, {errorCount} errors");

        // Rebuild the dictionary with enriched articles replacing the originals
        var updated = new Dictionary<string, IReadOnlyList<RoundupArticle>>(articlesBySection.Count, StringComparer.OrdinalIgnoreCase);
        foreach (var (sectionData, articles) in articlesBySection)
        {
            updated[sectionData] = articles
                .Select(a => enriched.TryGetValue(a.ExternalUrl, out var e)
                    ? new RoundupArticle
                    {
                        SectionName = a.SectionName,
                        Title = e.Title,
                        ExternalUrl = e.ExternalUrl,
                        Slug = e.Slug,
                        CollectionName = e.CollectionName,
                        IsInternal = e.IsInternal,
                        Content = e.Content,
                        FeedName = e.FeedName,
                        DateEpoch = e.DateEpoch,
                        NeedsAiMetadata = false,
                        Summary = e.Summary,
                        KeyTopics = e.KeyTopics,
                        Relevance = e.Relevance,
                        TopicType = e.TopicType,
                        ImpactLevel = e.ImpactLevel,
                        TimeSensitivity = e.TimeSensitivity
                    }
                    : a)
                .ToList();
        }

        return updated;
    }
}
