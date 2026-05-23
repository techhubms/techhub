using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TechHub.Core.Configuration;
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
    private readonly RoundupGeneratorOptions _options;
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
        RoundupGeneratorOptions options,
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
        ArgumentNullException.ThrowIfNull(options);
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
        _options = options;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<RoundupGenerationOutcome> GenerateAsync(DateOnly weekStart, DateOnly weekEnd, IProgress<string>? progress = null, long? jobId = null, CancellationToken ct = default)
    {
        var lp = new LoggingProgress(_logger, progress);

        var articlesBySection = await _roundupRepo.GetArticlesForWeekAsync(weekStart, weekEnd, ct);
        var totalArticles = articlesBySection.Values.Sum(a => a.Count);
        lp.Report($"Loaded {totalArticles} articles across {articlesBySection.Count} sections");

        if (articlesBySection.Count == 0)
        {
            _logger.LogWarning("No articles found for week {WeekStart}–{WeekEnd}, skipping roundup generation", weekStart, weekEnd);
            return RoundupGenerationOutcome.NoArticles;
        }

        // On-the-fly AI metadata backfill: categorize any items missing ai_metadata
        articlesBySection = await BackfillMissingAiMetadataAsync(articlesBySection, lp, ct);

        var generatedSlugs = new List<string>();
        var skippedExistsCount = 0;
        var failedGenerationCount = 0;
        var skippedAfterFilteringCount = 0;

        foreach (var sectionName in articlesBySection.Keys)
        {
            ct.ThrowIfCancellationRequested();

            var sectionResult = await GenerateSectionRoundupAsync(sectionName, articlesBySection[sectionName], weekStart, weekEnd, jobId, lp, ct);
            switch (sectionResult.Result)
            {
                case RoundupGenerationResult.Generated:
                    generatedSlugs.AddRange(sectionResult.Slugs);
                    break;
                case RoundupGenerationResult.AlreadyExists:
                    skippedExistsCount++;
                    break;
                case RoundupGenerationResult.ContentGenerationFailed:
                    failedGenerationCount++;
                    break;
                case RoundupGenerationResult.NoArticlesAfterFiltering:
                    skippedAfterFilteringCount++;
                    break;
            }
        }

        if (generatedSlugs.Count > 0)
        {
            return RoundupGenerationOutcome.Generated(generatedSlugs);
        }

        if (failedGenerationCount > 0)
        {
            return RoundupGenerationOutcome.ContentGenerationFailed;
        }

        if (skippedExistsCount > 0 && skippedExistsCount == articlesBySection.Count)
        {
            return RoundupGenerationOutcome.AlreadyExists;
        }

        if (skippedAfterFilteringCount > 0 && skippedAfterFilteringCount == articlesBySection.Count)
        {
            return RoundupGenerationOutcome.NoArticlesAfterFiltering;
        }

        return RoundupGenerationOutcome.NoArticlesAfterFiltering;
    }

    private async Task<RoundupGenerationOutcome> GenerateSectionRoundupAsync(
        string sectionName,
        IReadOnlyList<RoundupArticle> sectionArticles,
        DateOnly weekStart,
        DateOnly weekEnd,
        long? jobId,
        LoggingProgress lp,
        CancellationToken ct)
    {
        var publishDate = weekEnd.AddDays(1); // Monday after the week ends
        var slug = RoundupContentBuilder.BuildSlug(publishDate, sectionName);

        if (await _roundupRepo.RoundupExistsAsync(sectionName, slug, ct))
        {
            _logger.LogInformation("Roundup for section {SectionName} in week {WeekStart} already exists (slug={Slug}), skipping", sectionName, weekStart, slug);
            return RoundupGenerationOutcome.AlreadyExists;
        }

        var filteredSectionArticles = _relevanceFilter.FilterSection(sectionName, sectionArticles, lp);
        if (filteredSectionArticles.Count == 0)
        {
            _logger.LogInformation("No articles remain after relevance filtering for section {SectionName} in week {WeekStart}–{WeekEnd} — generating brief 'nothing notable' roundup", sectionName, weekStart, weekEnd);
        }

        lp.Report($"After relevance filtering for section '{sectionName}': {filteredSectionArticles.Count} articles");
        var filtered = new Dictionary<string, IReadOnlyList<RoundupArticle>>(StringComparer.OrdinalIgnoreCase)
        {
            [sectionName] = filteredSectionArticles
        };

        var weekDescription = string.Create(CultureInfo.InvariantCulture,
            $"the week of {weekStart.ToString("MMMM d", CultureInfo.InvariantCulture)} to {weekEnd.ToString("MMMM d, yyyy", CultureInfo.InvariantCulture)}");

        var writingGuidelines = _writingGuidelines.Value;

        lp.Report($"Step 1/5 ({sectionName}): Creating news-like stories");
        var sectionStories = await _newsWriter.WriteAsync(filtered, weekDescription, writingGuidelines, lp, ct);
        if (string.IsNullOrWhiteSpace(sectionStories))
        {
            _logger.LogError("Step 1 produced no content for section {SectionName} in week {WeekStart}–{WeekEnd}", sectionName, weekStart, weekEnd);
            return RoundupGenerationOutcome.ContentGenerationFailed;
        }

        lp.Report($"Step 2/5 ({sectionName}): Adding ongoing narrative");
        var narrativeContent = await _narrativeEnhancer.EnhanceAsync(sectionStories, sectionName, weekStart, writingGuidelines, lp, ct);
        if (string.IsNullOrWhiteSpace(narrativeContent))
        {
            _logger.LogError("Step 2 produced no content for section {SectionName} in week {WeekStart}–{WeekEnd}", sectionName, weekStart, weekEnd);
            return RoundupGenerationOutcome.ContentGenerationFailed;
        }

        string condensedContent;
        if (_options.CondensingEnabled)
        {
            lp.Report($"Step 3/5 ({sectionName}): Condensing content");
            condensedContent = await _condenser.CondenseAsync(narrativeContent, writingGuidelines, ct);
        }
        else
        {
            lp.Report($"Step 3/5 ({sectionName}): Condensing skipped (disabled)");
            condensedContent = narrativeContent;
        }

        lp.Report($"Step 4/5 ({sectionName}): Generating metadata");
        var metadata = await _metadataGenerator.GenerateAsync(condensedContent, sectionName, weekDescription, writingGuidelines, ct);

        lp.Report($"Step 5/5 ({sectionName}): Building final content");
        var tableOfContents = RoundupContentBuilder.BuildTableOfContents(condensedContent);
        var fullContent = RoundupContentBuilder.BuildFullContent(condensedContent, metadata.Introduction, tableOfContents);
        fullContent = _contentFixer.RepairMarkdown(fullContent);

        lp.Report($"Writing roundup to database for section '{sectionName}'");
        var tagsWithSections = TagNormalizer.EnsureSectionTags(metadata.Tags, filtered.Keys);
        var normalizedTags = TagNormalizer.NormalizeTags(tagsWithSections);
        await _roundupRepo.WriteRoundupAsync(sectionName, slug, publishDate, metadata.Title, fullContent, metadata.Introduction, normalizedTags, jobId: jobId, ct: ct);

        lp.Report($"Roundup for section '{sectionName}' in week {weekStart}\u2013{weekEnd} written successfully (slug={slug})");
        return RoundupGenerationOutcome.Generated(slug);
    }

    /// <summary>
    /// Finds articles missing AI metadata, categorizes them on-the-fly,
    /// updates the database, and returns an updated article dictionary with enriched metadata.
    /// Deduplicates by ExternalUrl so each item is only categorized once even if it appears in multiple sections.
    /// </summary>
    private async Task<IReadOnlyDictionary<string, IReadOnlyList<RoundupArticle>>> BackfillMissingAiMetadataAsync(
        IReadOnlyDictionary<string, IReadOnlyList<RoundupArticle>> articlesBySection,
        LoggingProgress lp,
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

        lp.Report($"AI metadata backfill: {needsBackfill.Count} items need categorization");

        // Map ExternalUrl → enriched metadata for updating articles across sections
        var enriched = new Dictionary<string, RoundupArticle>(StringComparer.OrdinalIgnoreCase);
        var errorCount = 0;
        var doneCount = 0;

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

            doneCount++;
            lp.Report($"AI metadata backfill: {doneCount}/{needsBackfill.Count} \u2014 {article.Slug}");
        }

        lp.Report($"AI metadata backfill complete: {doneCount}/{needsBackfill.Count} processed, {enriched.Count} enriched, {errorCount} errors");

        if (enriched.Count == 0)
        {
            return articlesBySection;
        }

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
