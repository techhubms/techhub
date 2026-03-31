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

        // Flush accumulated log and progress counters to the database so the frontend shows live progress
        async Task FlushProgressAsync()
        {
            if (jobId > 0)
            {
                await _jobRepo.UpdateLogAsync(jobId, log.ToString(), ct);
                await _jobRepo.UpdateProgressAsync(jobId, feedsProcessed, itemsAdded, itemsSkipped, errorCount, ct);
            }
        }

        try
        {
            jobId = await _jobRepo.CreateAsync(triggerType, ct: ct);

            if (!_options.Enabled && triggerType != "manual")
            {
                Log("Content processing is disabled. Skipping run.");
                await _jobRepo.CompleteAsync(jobId, 0, 0, 0, 0, log.ToString(), ct);
                return;
            }

            Log(string.Create(CultureInfo.InvariantCulture, $"Starting content processing run (trigger: {triggerType})"));

            var feeds = await _feedRepo.GetEnabledAsync(ct);
            Log(string.Create(CultureInfo.InvariantCulture, $"Loaded {feeds.Count} feed(s) from database"));
            await FlushProgressAsync();

            foreach (var feed in feeds)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }

                Log(string.Create(CultureInfo.InvariantCulture, $"Processing feed: {feed.Name}"));
                feedsProcessed++;

                var rawItems = await _rssService.IngestAsync(feed, ct);

                // Pre-filter: check which items are already known (in content_items or processed_urls)
                var newItems = new List<RawFeedItem>();
                var alreadyInDb = 0;
                var previouslySkipped = 0;
                var previouslyFailed = 0;

                foreach (var item in rawItems)
                {
                    if (await ExistsAsync(item.ExternalUrl, ct))
                    {
                        alreadyInDb++;
                        itemsSkipped++;
                    }
                    else
                    {
                        var processedStatus = await _processedUrlRepo.GetStatusAsync(item.ExternalUrl, ct);
                        if (processedStatus != null)
                        {
                            if (processedStatus == "skipped")
                            {
                                previouslySkipped++;
                            }
                            else
                            {
                                previouslyFailed++;
                            }

                            itemsSkipped++;
                        }
                        else
                        {
                            newItems.Add(item);
                        }
                    }
                }

                var parts = new List<string>
                {
                    $"{rawItems.Count} items from feed",
                    $"{newItems.Count} new",
                    $"{alreadyInDb} already in DB"
                };
                if (previouslySkipped > 0)
                {
                    parts.Add($"{previouslySkipped} previously skipped");
                }

                if (previouslyFailed > 0)
                {
                    parts.Add($"{previouslyFailed} previously failed");
                }

                Log(string.Create(CultureInfo.InvariantCulture,
                    $"  {string.Join(", ", parts)}"));

                if (newItems.Count == 0)
                {
                    await FlushProgressAsync();
                    continue;
                }

                var limit = _options.MaxItemsPerRun > 0
                    ? Math.Min(newItems.Count, _options.MaxItemsPerRun - itemsAdded)
                    : newItems.Count;

                for (var i = 0; i < limit && !ct.IsCancellationRequested; i++)
                {
                    var raw = newItems[i];
                    var step = "tags";

                    try
                    {
                        // Fetch YouTube tags for YouTube items (merged into FeedTags for AI)
                        var ytTagCount = 0;
                        if (raw.IsYouTube && _options.MaxYouTubeTagCount > 0)
                        {
                            var ytTags = await _youtubeTagService.GetTagsAsync(raw.ExternalUrl, ct);
                            ytTagCount = ytTags.Count;
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
                            }
                        }

                        // Fetch full content (YouTube transcript or article body)
                        step = raw.IsYouTube ? "transcript" : "content";
                        var hadContentBefore = !string.IsNullOrWhiteSpace(raw.FullContent);
                        raw = await _articleService.EnrichWithContentAsync(raw, ct);
                        var hasContentAfter = !string.IsNullOrWhiteSpace(raw.FullContent);
                        var contentFetched = !hadContentBefore && hasContentAfter;

                        // Pre-AI delay to prevent rate limiting (like the original PS scripts: 15s between every call)
                        if (_options.RequestDelayMs > 0 && i > 0)
                        {
                            await Task.Delay(_options.RequestDelayMs, ct);
                        }

                        // AI categorization
                        step = "AI";
                        CategorizationResult categorizationResult;
                        try
                        {
                            categorizationResult = await _aiService.CategorizeAsync(raw, ct);
                        }
                        catch (OperationCanceledException ex) when (!ct.IsCancellationRequested)
                        {
                            // Polly timeout or internal AI timeout (not user cancellation)
                            Log(string.Create(CultureInfo.InvariantCulture, $"  ✗ Error ({step}): {raw.ExternalUrl} — {ex.Message}"));
                            await _processedUrlRepo.RecordFailureAsync(raw.ExternalUrl, ex.Message, raw.FeedName, raw.CollectionName, reason: null, ct);
                            errorCount++;
                            continue;
                        }
                        catch (Exception ex) when (ex is HttpRequestException or JsonException or InvalidOperationException or TimeoutException)
                        {
                            Log(string.Create(CultureInfo.InvariantCulture, $"  ✗ Error ({step}): {raw.ExternalUrl} — {ex.Message}"));
                            await _processedUrlRepo.RecordFailureAsync(raw.ExternalUrl, ex.Message, raw.FeedName, raw.CollectionName, reason: null, ct);
                            errorCount++;
                            continue;
                        }

                        if (categorizationResult.Item == null)
                        {
                            var skipReason = TruncateLogReason(categorizationResult.Explanation);
                            var skipMsg = raw.IsYouTube
                                ? string.Create(CultureInfo.InvariantCulture, $"  ⊘ Skipped: {raw.ExternalUrl} ({ytTagCount} tags, transcript {(contentFetched ? "fetched" : "failed")}) — {skipReason}")
                                : string.Create(CultureInfo.InvariantCulture, $"  ⊘ Skipped: {raw.ExternalUrl} — {skipReason}");
                            Log(skipMsg);
                            await _processedUrlRepo.RecordSkippedAsync(raw.ExternalUrl, feedName: raw.FeedName, collectionName: raw.CollectionName, reason: categorizationResult.Explanation, ct: ct);
                            itemsSkipped++;
                            continue;
                        }

                        var processed = categorizationResult.Item;

                        // Write to database
                        step = "db-write";
                        try
                        {
                            await WriteItemAsync(processed, ct);

                            var addMsg = raw.IsYouTube
                                ? string.Create(CultureInfo.InvariantCulture, $"  ✓ Added: {raw.ExternalUrl} ({ytTagCount} tags, transcript {(contentFetched ? "fetched" : "failed")})")
                                : string.Create(CultureInfo.InvariantCulture, $"  ✓ Added: {raw.ExternalUrl}");
                            Log(addMsg);
                            await _processedUrlRepo.RecordSuccessAsync(
                                raw.ExternalUrl,
                                raw.IsYouTube ? raw.FeedTags : null,
                                raw.FeedName,
                                raw.CollectionName,
                                categorizationResult.Explanation,
                                ct);
                            itemsAdded++;
                        }
                        catch (DbException ex)
                        {
                            Log(string.Create(CultureInfo.InvariantCulture, $"  ✗ Error writing: {processed.ExternalUrl} — {ex.Message}"));
                            await _processedUrlRepo.RecordFailureAsync(raw.ExternalUrl, ex.Message, raw.FeedName, raw.CollectionName, categorizationResult.Explanation, ct);
                            errorCount++;
                        }
                    }
                    catch (OperationCanceledException) when (ct.IsCancellationRequested)
                    {
                        throw;
                    }
                    catch (Exception ex) when (ex is not OutOfMemoryException and not StackOverflowException)
                    {
                        Log(string.Create(CultureInfo.InvariantCulture, $"  ✗ Error ({step}): {raw.ExternalUrl} — {ex.Message}"));
                        await _processedUrlRepo.RecordFailureAsync(raw.ExternalUrl, ex.Message, raw.FeedName, raw.CollectionName, reason: null, ct);
                        errorCount++;
                    }

                    // Flush log and counters to DB after each URL so the frontend shows live progress
                    await FlushProgressAsync();
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
            await TryFailJobAsync(jobId, feedsProcessed, itemsAdded, itemsSkipped, errorCount, log.ToString(), ct);
        }
        catch (Exception ex) when (ex is not OutOfMemoryException and not StackOverflowException)
        {
            Log(string.Create(CultureInfo.InvariantCulture, $"FATAL ERROR: {ex.Message}"));
            _logger.LogError(ex, "Content processing run {JobId} failed with unhandled exception", jobId);
            await TryFailJobAsync(jobId, feedsProcessed, itemsAdded, itemsSkipped, errorCount, log.ToString(), ct);
        }
    }

    private async Task TryFailJobAsync(long jobId, int feedsProcessed, int itemsAdded, int itemsSkipped, int errorCount, string logOutput, CancellationToken ct)
    {
        if (jobId <= 0)
        {
            return;
        }

        try
        {
            await _jobRepo.FailAsync(jobId, feedsProcessed, itemsAdded, itemsSkipped, errorCount, logOutput, ct);
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

    private static string TruncateLogReason(string reason)
    {
        const int MaxLength = 120;
        if (string.IsNullOrWhiteSpace(reason))
        {
            return "no reason provided";
        }

        // Take first line/sentence only
        var firstLine = reason.Split('\n', 2)[0].Trim();
        return firstLine.Length <= MaxLength
            ? firstLine
            : firstLine[..MaxLength] + "…";
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
                PrimarySection = item.PrimarySectionName,
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
            var tagRows = BuildTagWords(item.Tags, item.CollectionName, item.Slug, item.DateEpoch,
                isAi, isAzure, isDotnet, isDevops, isGhc, isMl, isSecurity, bitmask);
            foreach (var row in tagRows)
            {
                await _connection.ExecuteAsync(new CommandDefinition(
                    @"INSERT INTO content_tags_expanded
                        (collection_name, slug, tag_word, tag_display, is_full_tag,
                         date_epoch, is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
                         is_ml, is_security, sections_bitmask)
                      VALUES
                        (@CollectionName, @Slug, @TagWord, @TagDisplay, @IsFullTag,
                         @DateEpoch, @IsAi, @IsAzure, @IsDotnet, @IsDevops, @IsGhc,
                         @IsMl, @IsSecurity, @Bitmask)
                      ON CONFLICT DO NOTHING",
                    row,
                    cancellationToken: ct));
            }
        }
    }

    private static List<object> BuildTagWords(
        IReadOnlyList<string> tags, string collection, string slug, long dateEpoch,
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
                DateEpoch = dateEpoch,
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
