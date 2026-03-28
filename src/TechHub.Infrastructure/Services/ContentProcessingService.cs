using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Text.Json;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Orchestrates the full content processing pipeline:
/// <list type="number">
///   <item>Load RSS feed configuration from the database.</item>
///   <item>Ingest RSS feeds into raw items.</item>
///   <item>Fetch full article content for non-YouTube items.</item>
///   <item>Categorize each item with Azure OpenAI.</item>
///   <item>Write new items directly to the database.</item>
///   <item>Record the run in <see cref="IContentProcessingJobRepository"/>.</item>
/// </list>
/// </summary>
public sealed class ContentProcessingService
{
    private readonly IRssFeedIngestionService _rssService;
    private readonly IArticleContentService _articleService;
    private readonly IAiCategorizationService _aiService;
    private readonly IYouTubeTagService _youtubeTagService;
    private readonly IDbConnection _connection;
    private readonly IContentProcessingJobRepository _jobRepo;
    private readonly IProcessedUrlRepository _processedUrlRepo;
    private readonly IRssFeedConfigRepository _feedRepo;
    private readonly TimeProvider _timeProvider;
    private readonly ContentProcessorOptions _options;
    private readonly ILogger<ContentProcessingService> _logger;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public ContentProcessingService(
        IRssFeedIngestionService rssService,
        IArticleContentService articleService,
        IAiCategorizationService aiService,
        IYouTubeTagService youtubeTagService,
        IDbConnection connection,
        IContentProcessingJobRepository jobRepo,
        IProcessedUrlRepository processedUrlRepo,
        IRssFeedConfigRepository feedRepo,
        TimeProvider timeProvider,
        IOptions<ContentProcessorOptions> options,
        ILogger<ContentProcessingService> logger)
    {
        ArgumentNullException.ThrowIfNull(rssService);
        ArgumentNullException.ThrowIfNull(articleService);
        ArgumentNullException.ThrowIfNull(aiService);
        ArgumentNullException.ThrowIfNull(youtubeTagService);
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(jobRepo);
        ArgumentNullException.ThrowIfNull(processedUrlRepo);
        ArgumentNullException.ThrowIfNull(feedRepo);
        ArgumentNullException.ThrowIfNull(timeProvider);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _rssService = rssService;
        _articleService = articleService;
        _aiService = aiService;
        _youtubeTagService = youtubeTagService;
        _connection = connection;
        _jobRepo = jobRepo;
        _processedUrlRepo = processedUrlRepo;
        _feedRepo = feedRepo;
        _timeProvider = timeProvider;
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// Runs a complete content processing pipeline run.
    /// All errors are caught and recorded in the job log — the method never throws.
    /// </summary>
    public async Task RunAsync(string triggerType = "scheduled", CancellationToken ct = default)
    {
        long jobId = 0;
        var log = new StringBuilder();
        var startedAt = _timeProvider.GetUtcNow();
        var feedsProcessed = 0;
        var itemsAdded = 0;
        var itemsSkipped = 0;
        var errorCount = 0;

        void Log(string msg)
        {
            var line = string.Create(CultureInfo.InvariantCulture,
                $"[{_timeProvider.GetUtcNow():HH:mm:ss}] {msg}");
            log.AppendLine(line);
            _logger.LogInformation("ContentProcessing[{JobId}] {Message}", jobId, msg);
        }

        try
        {
            jobId = await _jobRepo.CreateAsync(triggerType, ct);

            if (!_options.Enabled)
            {
                Log("Content processing is disabled. Skipping run.");
                await _jobRepo.CompleteAsync(jobId, 0, 0, 0, 0, log.ToString(), ct);
                return;
            }

            Log(string.Create(CultureInfo.InvariantCulture, $"Starting content processing run (trigger: {triggerType})"));

            var feeds = await _feedRepo.GetEnabledAsync(ct);
            Log(string.Create(CultureInfo.InvariantCulture, $"Loaded {feeds.Count} feed(s) from database"));

            foreach (var feed in feeds)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }

                Log(string.Create(CultureInfo.InvariantCulture, $"Processing feed: {feed.Name}"));
                feedsProcessed++;

                var rawItems = await _rssService.IngestAsync(feed, ct);
                Log(string.Create(CultureInfo.InvariantCulture, $"  {rawItems.Count} items from feed"));

                var limit = _options.MaxItemsPerRun > 0
                    ? Math.Min(rawItems.Count, _options.MaxItemsPerRun - itemsAdded)
                    : rawItems.Count;

                for (var i = 0; i < limit && !ct.IsCancellationRequested; i++)
                {
                    var raw = rawItems[i];

                    // Check if already in content_items (existing duplicate check)
                    if (await ExistsAsync(raw.ExternalUrl, ct))
                    {
                        itemsSkipped++;
                        continue;
                    }

                    // Check if we already attempted this URL (success or failure)
                    if (await _processedUrlRepo.ExistsAsync(raw.ExternalUrl, ct))
                    {
                        itemsSkipped++;
                        continue;
                    }

                    // Fetch YouTube tags for YouTube items (merged into FeedTags for AI)
                    if (raw.IsYouTube && _options.MaxYouTubeTagCount > 0)
                    {
                        var ytTags = await _youtubeTagService.GetTagsAsync(raw.ExternalUrl, ct);
                        if (ytTags.Count > 0)
                        {
                            var mergedTags = raw.FeedTags.Concat(ytTags)
                                .Select(t => t.Trim())
                                .Where(t => t.Length > 0)
                                .Distinct(StringComparer.OrdinalIgnoreCase)
                                .OrderBy(t => t, StringComparer.OrdinalIgnoreCase)
                                .ToList();
                            raw = new RawFeedItem
                            {
                                Title = raw.Title,
                                ExternalUrl = raw.ExternalUrl,
                                PublishedAt = raw.PublishedAt,
                                Description = raw.Description,
                                Author = raw.Author,
                                FeedTags = mergedTags,
                                FeedName = raw.FeedName,
                                CollectionName = raw.CollectionName,
                                FullContent = raw.FullContent
                            };
                            Log(string.Create(CultureInfo.InvariantCulture, $"  YouTube tags: {ytTags.Count} fetched, {mergedTags.Count} total after merge"));
                        }
                    }

                    // Fetch full content (YouTube transcript or article body)
                    raw = await _articleService.EnrichWithContentAsync(raw, ct);

                    // Delay between external requests
                    if (_options.RequestDelayMs > 0 && i > 0)
                    {
                        await Task.Delay(_options.RequestDelayMs, ct);
                    }

                    // AI categorization
                    ProcessedContentItem? processed;
                    try
                    {
                        processed = await _aiService.CategorizeAsync(raw, ct);
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch (Exception ex) when (ex is HttpRequestException or JsonException or InvalidOperationException or TimeoutException)
                    {
                        Log(string.Create(CultureInfo.InvariantCulture, $"  ERROR categorizing {raw.ExternalUrl}: {ex.Message}"));
                        await _processedUrlRepo.RecordFailureAsync(raw.ExternalUrl, ex.Message, ct);
                        errorCount++;
                        continue;
                    }

                    if (processed == null)
                    {
                        Log(string.Create(CultureInfo.InvariantCulture, $"  SKIPPED (AI): {raw.Title}"));
                        await _processedUrlRepo.RecordSuccessAsync(raw.ExternalUrl, ct: ct);
                        itemsSkipped++;
                        continue;
                    }

                    // Write to database
                    try
                    {
                        await WriteItemAsync(processed, ct);

                        // Register high/medium relevance items in weekly per-section draft accumulators
                        if (processed.RoundupMetadata?.Relevance is "high" or "medium")
                        {
                            await RegisterRoundupItemAsync(processed, ct);
                        }

                        Log(string.Create(CultureInfo.InvariantCulture, $"  ADDED: {processed.Title}"));
                        await _processedUrlRepo.RecordSuccessAsync(
                            raw.ExternalUrl,
                            raw.IsYouTube ? raw.FeedTags : null,
                            ct);
                        itemsAdded++;
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch (DbException ex)
                    {
                        Log(string.Create(CultureInfo.InvariantCulture, $"  ERROR writing {processed.ExternalUrl}: {ex.Message}"));
                        await _processedUrlRepo.RecordFailureAsync(raw.ExternalUrl, ex.Message, ct);
                        errorCount++;
                    }
                }
            }

            // Clean up old job records (keep last 500)
            await PurgeOldJobsAsync(ct);

            // Purge old failed URL records so they can be retried
            if (_options.FailedUrlRetentionDays > 0)
            {
                await _processedUrlRepo.PurgeFailedAsync(TimeSpan.FromDays(_options.FailedUrlRetentionDays), ct);
            }

            var duration = _timeProvider.GetUtcNow() - startedAt;
            Log(string.Create(CultureInfo.InvariantCulture,
                $"Run complete. Added: {itemsAdded}, Skipped: {itemsSkipped}, Errors: {errorCount}, Duration: {duration.TotalSeconds:F1}s"));

            await _jobRepo.CompleteAsync(jobId, feedsProcessed, itemsAdded, itemsSkipped, errorCount, log.ToString(), ct);
        }
        catch (OperationCanceledException)
        {
            Log("Run cancelled.");
            await TryFailJobAsync(jobId, log.ToString(), ct);
        }
        catch (Exception ex) when (ex is not OutOfMemoryException and not StackOverflowException)
        {
            Log(string.Create(CultureInfo.InvariantCulture, $"FATAL ERROR: {ex.Message}"));
            _logger.LogError(ex, "Content processing run {JobId} failed with unhandled exception", jobId);
            await TryFailJobAsync(jobId, log.ToString(), ct);
        }
    }

    private async Task TryFailJobAsync(long jobId, string logOutput, CancellationToken ct)
    {
        if (jobId <= 0)
        {
            return;
        }

        try
        {
            await _jobRepo.FailAsync(jobId, logOutput, ct);
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to mark job {JobId} as failed", jobId);
        }
    }

    private async Task PurgeOldJobsAsync(CancellationToken ct)
    {
        try
        {
            await _connection.ExecuteAsync(new CommandDefinition(
                @"DELETE FROM content_processing_jobs
                  WHERE id NOT IN (
                      SELECT id FROM content_processing_jobs
                      ORDER BY started_at DESC LIMIT 500
                  )",
                cancellationToken: ct));
        }
        catch (DbException ex)
        {
            _logger.LogWarning(ex, "Failed to purge old processing jobs");
        }
    }

    private async Task<bool> ExistsAsync(string externalUrl, CancellationToken ct)
    {
        var exists = await _connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                "SELECT EXISTS(SELECT 1 FROM content_items WHERE external_url = @ExternalUrl)",
                new { ExternalUrl = externalUrl },
                cancellationToken: ct));
        return exists;
    }

    /// <summary>
    /// Registers a newly processed item in the <c>section_roundup_items</c> accumulation table
    /// for each section the item belongs to.
    /// The week is identified by the Monday of the current ISO week in Europe/Brussels time.
    /// </summary>
    private async Task RegisterRoundupItemAsync(ProcessedContentItem item, CancellationToken ct)
    {
        var weekStart = GetCurrentIsoWeekMonday(_timeProvider);
        var sections = GetSectionsToRegister(item);

        foreach (var section in sections)
        {
            await _connection.ExecuteAsync(new CommandDefinition(
                @"INSERT INTO section_roundup_items
                      (section_name, week_start_date, collection_name, slug)
                  VALUES (@Section, @WeekStart, @Collection, @Slug)
                  ON CONFLICT DO NOTHING",
                new
                {
                    Section = section.ToLowerInvariant(),
                    WeekStart = weekStart,
                    Collection = item.CollectionName,
                    Slug = item.Slug
                },
                cancellationToken: ct));
        }
    }

    /// <summary>
    /// Returns the date of the Monday of the current ISO week, expressed in Europe/Brussels time.
    /// </summary>
    private static DateOnly GetCurrentIsoWeekMonday(TimeProvider timeProvider)
    {
        var brusselsZone = TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows() ? "Romance Standard Time" : "Europe/Brussels");
        var utcNow = timeProvider.GetUtcNow().UtcDateTime;
        var now = TimeZoneInfo.ConvertTimeFromUtc(utcNow, brusselsZone);
        var weekYear = System.Globalization.ISOWeek.GetYear(now);
        var weekNumber = System.Globalization.ISOWeek.GetWeekOfYear(now);
        var monday = System.Globalization.ISOWeek.ToDateTime(weekYear, weekNumber, DayOfWeek.Monday);
        return DateOnly.FromDateTime(monday);
    }

    /// <summary>
    /// Returns the list of sections an item should be registered in for roundup drafts.
    /// Uses the item's known sections first; falls back to the primary section if no sections are set.
    /// </summary>
    private static IEnumerable<string> GetSectionsToRegister(ProcessedContentItem item)
    {
        if (item.Sections.Count > 0)
        {
            return item.Sections;
        }

        if (!string.IsNullOrEmpty(item.PrimarySectionName) && item.PrimarySectionName != "all")
        {
            return [item.PrimarySectionName];
        }

        return [];
    }

    private async Task WriteItemAsync(ProcessedContentItem item, CancellationToken ct)
    {
        // Compute section booleans and bitmask from section names
        var sectionSet = new HashSet<string>(item.Sections, StringComparer.OrdinalIgnoreCase);
        var isAi = sectionSet.Contains("ai");
        var isAzure = sectionSet.Contains("azure");
        var isDotnet = sectionSet.Contains("dotnet");
        var isDevops = sectionSet.Contains("devops");
        var isGhc = sectionSet.Contains("github-copilot");
        var isMl = sectionSet.Contains("ml");
        var isSecurity = sectionSet.Contains("security");

        var bitmask = 0;
        if (isAi)
        {
            bitmask |= 1;
        }

        if (isAzure)
        {
            bitmask |= 2;
        }

        if (isDotnet)
        {
            bitmask |= 4;
        }

        if (isDevops)
        {
            bitmask |= 8;
        }

        if (isGhc)
        {
            bitmask |= 16;
        }

        if (isMl)
        {
            bitmask |= 32;
        }

        if (isSecurity)
        {
            bitmask |= 64;
        }

        var tagsCsv = item.Tags.Count > 0 ? $",{string.Join(",", item.Tags)}," : string.Empty;
        var primarySection = item.PrimarySectionName ?? (item.Sections.Count > 0 ? item.Sections[0] : "all");

        var aiMetadataJson = item.RoundupMetadata != null
            ? JsonSerializer.Serialize(new
            {
                roundup_summary = item.RoundupMetadata.Summary,
                key_topics = item.RoundupMetadata.KeyTopics,
                roundup_relevance = item.RoundupMetadata.Relevance,
                topic_type = item.RoundupMetadata.TopicType,
                impact_level = item.RoundupMetadata.ImpactLevel,
                time_sensitivity = item.RoundupMetadata.TimeSensitivity
            }, _jsonOptions)
            : null;

        const string Upsert = @"
INSERT INTO content_items
    (slug, collection_name, title, content, excerpt, date_epoch,
     primary_section_name, external_url, author, feed_name,
     tags_csv, is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
     is_ml, is_security, sections_bitmask, content_hash, ai_metadata)
VALUES
    (@Slug, @Collection, @Title, @Content, @Excerpt, @DateEpoch,
     @PrimarySection, @ExternalUrl, @Author, @FeedName,
     @TagsCsv, @IsAi, @IsAzure, @IsDotnet, @IsDevops, @IsGhc,
     @IsMl, @IsSecurity, @Bitmask, @ContentHash, @AiMetadata::jsonb)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    tags_csv             = EXCLUDED.tags_csv,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    content_hash         = EXCLUDED.content_hash,
    ai_metadata          = EXCLUDED.ai_metadata,
    updated_at           = NOW()";

        await _connection.ExecuteAsync(new CommandDefinition(
            Upsert,
            new
            {
                Slug = item.Slug,
                Collection = item.CollectionName,
                item.Title,
                Content = item.Content ?? string.Empty,
                item.Excerpt,
                item.DateEpoch,
                PrimarySection = primarySection,
                item.ExternalUrl,
                Author = item.Author ?? string.Empty,
                FeedName = item.FeedName,
                TagsCsv = tagsCsv,
                IsAi = isAi,
                IsAzure = isAzure,
                IsDotnet = isDotnet,
                IsDevops = isDevops,
                IsGhc = isGhc,
                IsMl = isMl,
                IsSecurity = isSecurity,
                Bitmask = bitmask,
                ContentHash = item.ContentHash,
                AiMetadata = aiMetadataJson
            },
            cancellationToken: ct));

        // Rebuild expanded tags
        await _connection.ExecuteAsync(new CommandDefinition(
            "DELETE FROM content_tags_expanded WHERE collection_name = @Collection AND slug = @Slug",
            new { Collection = item.CollectionName, Slug = item.Slug },
            cancellationToken: ct));

        if (item.Tags.Count > 0)
        {
            var tagRows = BuildTagWords(item.Tags, item.CollectionName, item.Slug,
                isAi, isAzure, isDotnet, isDevops, isGhc, isMl, isSecurity, bitmask);
            foreach (var row in tagRows)
            {
                await _connection.ExecuteAsync(new CommandDefinition(
                    @"INSERT INTO content_tags_expanded
                        (collection_name, slug, tag_word, tag_display, is_full_tag,
                         is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
                         is_ml, is_security, sections_bitmask)
                      VALUES
                        (@CollectionName, @Slug, @TagWord, @TagDisplay, @IsFullTag,
                         @IsAi, @IsAzure, @IsDotnet, @IsDevops, @IsGhc,
                         @IsMl, @IsSecurity, @Bitmask)
                      ON CONFLICT DO NOTHING",
                    row,
                    cancellationToken: ct));
            }
        }
    }

    private static List<object> BuildTagWords(
        IReadOnlyList<string> tags, string collection, string slug,
        bool isAi, bool isAzure, bool isDotnet, bool isDevops, bool isGhc,
        bool isMl, bool isSecurity, int bitmask)
    {
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var rows = new List<object>();

        void Add(string word, string display, bool full)
        {
            if (!seen.Add(word.ToLowerInvariant()))
            {
                return;
            }

            rows.Add(new
            {
                CollectionName = collection,
                Slug = slug,
                TagWord = word.ToLowerInvariant(),
                TagDisplay = display,
                IsFullTag = full,
                IsAi = isAi,
                IsAzure = isAzure,
                IsDotnet = isDotnet,
                IsDevops = isDevops,
                IsGhc = isGhc,
                IsMl = isMl,
                IsSecurity = isSecurity,
                Bitmask = bitmask
            });
        }

        foreach (var tag in tags)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                continue;
            }

            Add(tag, tag, true);

            // Word expansions for multi-word tags
            var words = tag.Split(new char[] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length > 1)
            {
                foreach (var word in words)
                {
                    Add(word, word, false);
                }
            }
        }

        return rows;
    }
}
