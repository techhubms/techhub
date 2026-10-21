using System.Globalization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Services.RoundupGeneration;

/// <summary>
/// Orchestrates the AI weekly roundup generation pipeline.
/// Delegates each step to a dedicated class while coordinating the overall flow.
/// </summary>
internal sealed class RoundupGeneratorService : IRoundupGeneratorService
{
    private static readonly Lazy<string> _writingGuidelines = RoundupAiHelper.LoadResource("roundup-writing-guidelines.md");

    private readonly ISectionRoundupRepository _roundupRepo;
    private readonly IContentFixerService _contentFixer;
    private readonly IAiCategorizationService _aiService;
    private readonly IContentItemWriteRepository _writeRepo;
    private readonly ITimelineEventRepository _timelineRepo;
    private readonly RoundupNewsWriter _newsWriter;
    private readonly RoundupMetadataGenerator _metadataGenerator;
    private readonly AppSettings _settings;
    private readonly ILogger<RoundupGeneratorService> _logger;

    public RoundupGeneratorService(
        ISectionRoundupRepository roundupRepo,
        IContentFixerService contentFixer,
        IAiCategorizationService aiService,
        IContentItemWriteRepository writeRepo,
        ITimelineEventRepository timelineRepo,
        RoundupNewsWriter newsWriter,
        RoundupMetadataGenerator metadataGenerator,
        IOptions<AppSettings> settings,
        ILogger<RoundupGeneratorService> logger)
    {
        ArgumentNullException.ThrowIfNull(roundupRepo);
        ArgumentNullException.ThrowIfNull(contentFixer);
        ArgumentNullException.ThrowIfNull(aiService);
        ArgumentNullException.ThrowIfNull(writeRepo);
        ArgumentNullException.ThrowIfNull(timelineRepo);
        ArgumentNullException.ThrowIfNull(newsWriter);
        ArgumentNullException.ThrowIfNull(metadataGenerator);
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(logger);

        _roundupRepo = roundupRepo;
        _contentFixer = contentFixer;
        _aiService = aiService;
        _writeRepo = writeRepo;
        _timelineRepo = timelineRepo;
        _newsWriter = newsWriter;
        _metadataGenerator = metadataGenerator;
        _settings = settings.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<RoundupGenerationOutcome> GenerateAsync(DateOnly weekStart, DateOnly weekEnd, IProgress<string>? progress = null, long? jobId = null, CancellationToken ct = default)
    {
        var lp = new LoggingProgress(_logger, progress);

        var generatedSlugs = new List<string>();
        var failedGenerationCount = 0;
        var sectionsWithArticlesCount = 0;
        var totalArticles = 0;

        var sectionSlugs = _settings.Content.Sections
            .OrderBy(kvp => kvp.Value.Order)
            .Select(kvp => kvp.Key)
            .Where(k => !string.Equals(k, "all", StringComparison.OrdinalIgnoreCase));

        foreach (var sectionName in sectionSlugs)
        {
            ct.ThrowIfCancellationRequested();

            var sectionArticles = await _roundupRepo.GetArticlesForSectionForWeekAsync(sectionName, weekStart, weekEnd, ct);
            lp.Report($"Loaded {sectionArticles.Count} articles for section '{sectionName}'");

            if (sectionArticles.Count == 0)
            {
                continue;
            }

            sectionsWithArticlesCount++;
            totalArticles += sectionArticles.Count;

            // On-the-fly AI metadata backfill: categorize any items missing ai_metadata.
            var sectionArticlesBySection = new Dictionary<string, IReadOnlyList<RoundupArticle>>(StringComparer.OrdinalIgnoreCase)
            {
                [sectionName] = sectionArticles
            };

            var enrichedSectionArticlesBySection = await BackfillMissingAiMetadataAsync(sectionArticlesBySection, lp, ct);
            var enrichedSectionArticles = enrichedSectionArticlesBySection.TryGetValue(sectionName, out var enriched)
                ? enriched
                : sectionArticles;

            var sectionResult = await GenerateSectionRoundupAsync(sectionName, enrichedSectionArticles, weekStart, weekEnd, jobId, lp, ct);
            switch (sectionResult.Result)
            {
                case RoundupGenerationResult.Generated:
                    generatedSlugs.AddRange(sectionResult.Slugs);
                    break;
                case RoundupGenerationResult.AlreadyExists:
                    break;
                case RoundupGenerationResult.ContentGenerationFailed:
                    failedGenerationCount++;
                    break;
            }
        }

        lp.Report($"Loaded {totalArticles} articles across {sectionsWithArticlesCount} sections");

        if (sectionsWithArticlesCount == 0)
        {
            _logger.LogWarning("No articles found for week {WeekStart}–{WeekEnd}, skipping roundup generation", weekStart, weekEnd);
            return RoundupGenerationOutcome.NoArticles;
        }

        if (generatedSlugs.Count > 0)
        {
            return RoundupGenerationOutcome.Generated(generatedSlugs);
        }

        if (failedGenerationCount > 0)
        {
            return RoundupGenerationOutcome.ContentGenerationFailed;
        }

        return RoundupGenerationOutcome.AlreadyExists;
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

        lp.Report($"Using {sectionArticles.Count} articles for section '{sectionName}'");
        var weekDescription = string.Create(CultureInfo.InvariantCulture,
            $"the week of {weekStart.ToString("MMMM d", CultureInfo.InvariantCulture)} to {weekEnd.ToString("MMMM d, yyyy", CultureInfo.InvariantCulture)}");

        var writingGuidelines = _writingGuidelines.Value;

        var (lead, brief) = RoundupArticleSelector.Split(sectionArticles, sectionName);

        lp.Report($"Writer ({sectionName}): creating the topics from {lead.Count} lead articles ({brief.Count} one-line mentions)");

        var sectionTitle = _settings.Content.Sections.TryGetValue(sectionName, out var sectionConfig) ? sectionConfig.Title : sectionName;
        var sectionTitles = _settings.Content.Sections.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Title, StringComparer.OrdinalIgnoreCase);

        var writerContent = string.Empty;
        if (lead.Count > 0)
        {
            var previousHeadings = RoundupHeadings.Extract(await _roundupRepo.GetPreviousRoundupContentAsync(sectionName, weekStart, ct));
            var written = await _newsWriter.WriteAsync(sectionName, lead, previousHeadings, weekDescription, writingGuidelines, ct);
            if (string.IsNullOrWhiteSpace(written))
            {
                _logger.LogError("The writer produced no content for section {SectionName} in week {WeekStart}–{WeekEnd}", sectionName, weekStart, weekEnd);
                return RoundupGenerationOutcome.ContentGenerationFailed;
            }

            writerContent = written;
        }

        var (completeContent, appended) = RoundupBlocksBuilder.EnsureLeadLinks(writerContent, lead, sectionTitle);
        if (appended.Count > 0)
        {
            _logger.LogWarning("The writer left out {Count} lead articles for section {SectionName}; appended them to the other news: {Titles}",
                appended.Count, sectionName, string.Join("; ", appended.Select(a => a.Title)));
        }

        var alsoRelevant = RoundupBlocksBuilder.BuildAlsoRelevant(brief, publishDate, sectionTitles);
        var datesToKnow = RoundupBlocksBuilder.BuildDatesToKnow(
            await _roundupRepo.GetUpcomingDateEventsAsync(sectionName, weekStart, weekEnd, ct) ?? [], weekEnd);

        // A section without lead articles has no written topics; the metadata then describes the one-line mentions.
        var metadataInput = string.IsNullOrWhiteSpace(completeContent) ? alsoRelevant : completeContent;

        lp.Report($"Metadata ({sectionName}): generating title, tags and introduction");
        var metadata = await _metadataGenerator.GenerateAsync(metadataInput, sectionName, weekDescription, writingGuidelines, ct);

        lp.Report($"Building the final content for section {sectionName}");
        var sectionContent = string.Join("\n\n", new[] { completeContent.Trim(), alsoRelevant, datesToKnow }.Where(b => !string.IsNullOrWhiteSpace(b)));
        var tableOfContents = RoundupContentBuilder.BuildTableOfContents(sectionContent);
        var fullContent = RoundupContentBuilder.BuildFullContent(sectionContent, metadata.Introduction, tableOfContents);
        fullContent = _contentFixer.RepairMarkdown(fullContent);

        lp.Report($"Writing roundup to database for section '{sectionName}'");
        var tagsWithSections = TagNormalizer.EnsureSectionTags(metadata.Tags, [sectionName]);
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

                var result = await _aiService.ClassifyAsync(rawItem, ct);
                if (result.Item?.RoundupMetadata is { } meta)
                {
                    // Update only ai_metadata on the existing row (avoids PK/unique-constraint conflicts).
                    // Classification only: the stored article text is never rewritten by the backfill.
                    await _writeRepo.UpdateAiMetadataAsync(
                        article.CollectionName, article.Slug, ContentItemWriteRepository.SerializeAiMetadata(meta), ct);

                    // The classification already extracted the timeline events of the article: keep them, so the
                    // "Dates to know" block of this roundup does not depend on the timeline backfill job having run.
                    await _timelineRepo.ReplaceEventsAsync(
                        article.CollectionName,
                        article.Slug,
                        result.Item.TimelineEvents,
                        ITimelineEventRepository.ExtractedFromStoredContent,
                        ITimelineEventRepository.CurrentPromptVersion,
                        ct);

                    enriched[article.ExternalUrl] = article.WithMetadata(meta);

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
                .Select(a => enriched.TryGetValue(a.ExternalUrl, out var e) ? e with { SectionName = a.SectionName } : a)
                .ToList();
        }

        return updated;
    }
}
