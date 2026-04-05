using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Services.ContentProcessing;

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
    private readonly IContentItemWriteRepository _writeRepo;
    private readonly IContentProcessingJobRepository _jobRepo;
    private readonly IProcessedUrlRepository _processedUrlRepo;
    private readonly IRssFeedConfigRepository _feedRepo;
    private readonly IContentFixerService _contentFixer;
    private readonly TimeProvider _timeProvider;
    private readonly ContentProcessorOptions _options;
    private readonly ILogger<ContentProcessingService> _logger;

    public ContentProcessingService(
        IRssFeedIngestionService rssService,
        IArticleContentService articleService,
        IAiCategorizationService aiService,
        IYouTubeTagService youtubeTagService,
        IContentItemWriteRepository writeRepo,
        IContentProcessingJobRepository jobRepo,
        IProcessedUrlRepository processedUrlRepo,
        IRssFeedConfigRepository feedRepo,
        IContentFixerService contentFixer,
        TimeProvider timeProvider,
        IOptions<ContentProcessorOptions> options,
        ILogger<ContentProcessingService> logger)
    {
        ArgumentNullException.ThrowIfNull(rssService);
        ArgumentNullException.ThrowIfNull(articleService);
        ArgumentNullException.ThrowIfNull(aiService);
        ArgumentNullException.ThrowIfNull(youtubeTagService);
        ArgumentNullException.ThrowIfNull(writeRepo);
        ArgumentNullException.ThrowIfNull(jobRepo);
        ArgumentNullException.ThrowIfNull(processedUrlRepo);
        ArgumentNullException.ThrowIfNull(feedRepo);
        ArgumentNullException.ThrowIfNull(contentFixer);
        ArgumentNullException.ThrowIfNull(timeProvider);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _rssService = rssService;
        _articleService = articleService;
        _aiService = aiService;
        _youtubeTagService = youtubeTagService;
        _writeRepo = writeRepo;
        _jobRepo = jobRepo;
        _processedUrlRepo = processedUrlRepo;
        _feedRepo = feedRepo;
        _contentFixer = contentFixer;
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
        var transcriptsSucceeded = 0;
        var transcriptsFailed = 0;

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
                await _jobRepo.UpdateProgressAsync(jobId, feedsProcessed, itemsAdded, itemsSkipped, errorCount, transcriptsSucceeded, transcriptsFailed, ct: ct);
            }
        }

        try
        {
            jobId = await _jobRepo.CreateAsync(triggerType, ct: ct);

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

                var ingestionResult = await _rssService.IngestAsync(feed, ct);

                if (ingestionResult.Failed)
                {
                    Log(string.Create(CultureInfo.InvariantCulture, $"  ✗ Feed error: {feed.Name} — {ingestionResult.ErrorMessage}"));
                    errorCount++;
                    await FlushProgressAsync();
                    continue;
                }

                var rawItems = ingestionResult.Items;

                // Pre-filter: check which items are already known (in content_items or processed_urls)
                var newItems = new List<RawFeedItem>();
                var alreadyInDb = 0;
                var previouslySkipped = 0;
                var previouslyFailed = 0;

                foreach (var item in rawItems)
                {
                    if (await _writeRepo.ExistsByExternalUrlAsync(item.ExternalUrl, ct))
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

                // Flush after every feed summary so the admin dashboard shows progress
                // even during long-running AI calls for individual URLs
                await FlushProgressAsync();

                if (newItems.Count == 0)
                {
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
                                    FeedItemData = raw.FeedItemData,
                                    FeedLevelAuthor = raw.FeedLevelAuthor,
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

                        // Track transcript outcome for YouTube items
                        bool? hasTranscript = raw.IsYouTube ? contentFetched : null;
                        var transcriptStatus = contentFetched
                            ? "transcript fetched"
                            : $"transcript failed: {raw.TranscriptFailureReason ?? "unknown"}";
                        if (raw.IsYouTube)
                        {
                            if (contentFetched)
                            {
                                transcriptsSucceeded++;
                            }
                            else
                            {
                                transcriptsFailed++;

                                // Enforce TranscriptMandatory — fail the item if transcript is required but absent
                                if (feed.TranscriptMandatory)
                                {
                                    Log(string.Create(CultureInfo.InvariantCulture, $"  ✗ Failed: {raw.ExternalUrl} — transcript mandatory but not available"));
                                    await _processedUrlRepo.RecordFailureAsync(raw.ExternalUrl, "Transcript mandatory but not available", raw.FeedName, raw.CollectionName, reason: null, hasTranscript: false, jobId: jobId, ct: ct);
                                    errorCount++;
                                    await FlushProgressAsync();
                                    continue;
                                }
                            }
                        }

                        // Flush before AI call so the dashboard is current during the longest operation
                        await FlushProgressAsync();

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
                            await _processedUrlRepo.RecordFailureAsync(raw.ExternalUrl, ex.Message, raw.FeedName, raw.CollectionName, reason: null, hasTranscript: hasTranscript, jobId: jobId, ct: ct);
                            errorCount++;
                            continue;
                        }
                        catch (Exception ex) when (ex is HttpRequestException or JsonException or InvalidOperationException or TimeoutException)
                        {
                            Log(string.Create(CultureInfo.InvariantCulture, $"  ✗ Error ({step}): {raw.ExternalUrl} — {ex.Message}"));
                            await _processedUrlRepo.RecordFailureAsync(raw.ExternalUrl, ex.Message, raw.FeedName, raw.CollectionName, reason: null, hasTranscript: hasTranscript, jobId: jobId, ct: ct);
                            errorCount++;
                            continue;
                        }

                        if (categorizationResult.Item == null)
                        {
                            var reason = TruncateLogReason(categorizationResult.Explanation);

                            if (categorizationResult.IsFailure)
                            {
                                // AI did not produce a valid response (empty, malformed, validation failure, etc.)
                                var failMsg = raw.IsYouTube
                                    ? string.Create(CultureInfo.InvariantCulture, $"  ✗ Failed: {raw.ExternalUrl} ({ytTagCount} tags, {transcriptStatus}) — {reason}")
                                    : string.Create(CultureInfo.InvariantCulture, $"  ✗ Failed: {raw.ExternalUrl} — {reason}");
                                Log(failMsg);
                                await _processedUrlRepo.RecordFailureAsync(raw.ExternalUrl, categorizationResult.Explanation, raw.FeedName, raw.CollectionName, reason: null, hasTranscript: hasTranscript, jobId: jobId, ct: ct);
                                errorCount++;
                            }
                            else
                            {
                                // AI intentionally excluded this item with a valid explanation
                                var skipMsg = raw.IsYouTube
                                    ? string.Create(CultureInfo.InvariantCulture, $"  ⊘ Skipped: {raw.ExternalUrl} ({ytTagCount} tags, {transcriptStatus}) — {reason}")
                                    : string.Create(CultureInfo.InvariantCulture, $"  ⊘ Skipped: {raw.ExternalUrl} — {reason}");
                                Log(skipMsg);
                                await _processedUrlRepo.RecordSkippedAsync(raw.ExternalUrl, feedName: raw.FeedName, collectionName: raw.CollectionName, reason: categorizationResult.Explanation, hasTranscript: hasTranscript, jobId: jobId, ct: ct);
                                itemsSkipped++;
                            }

                            continue;
                        }

                        var processed = categorizationResult.Item;

                        // Apply subcollection rules from configuration
                        var matchedSubcollection = MatchSubcollectionRule(raw.FeedName, raw.Title);
                        if (matchedSubcollection != null)
                        {
                            processed = processed.WithSubcollectionName(matchedSubcollection);
                            Log(string.Create(CultureInfo.InvariantCulture,
                                $"  → Subcollection rule matched: {matchedSubcollection}"));
                        }

                        // Repair markdown before writing
                        if (!string.IsNullOrWhiteSpace(processed.Content))
                        {
                            processed = processed.WithContent(_contentFixer.RepairMarkdown(processed.Content));
                        }

                        // Write to database
                        step = "db-write";
                        try
                        {
                            await _writeRepo.UpsertProcessedItemAsync(processed, ct);

                            var addMsg = raw.IsYouTube
                                ? string.Create(CultureInfo.InvariantCulture, $"  ✓ Added: {raw.ExternalUrl} ({ytTagCount} tags, {transcriptStatus})")
                                : string.Create(CultureInfo.InvariantCulture, $"  ✓ Added: {raw.ExternalUrl}");
                            Log(addMsg);
                            await _processedUrlRepo.RecordSuccessAsync(
                                raw.ExternalUrl,
                                raw.IsYouTube ? raw.FeedTags : null,
                                raw.FeedName,
                                processed.CollectionName,
                                categorizationResult.Explanation,
                                hasTranscript,
                                jobId,
                                processed.Slug,
                                ct);
                            itemsAdded++;
                        }
                        catch (DbException ex)
                        {
                            Log(string.Create(CultureInfo.InvariantCulture, $"  ✗ Error writing: {processed.ExternalUrl} — {ex.Message}"));
                            await _processedUrlRepo.RecordFailureAsync(raw.ExternalUrl, ex.Message, raw.FeedName, processed.CollectionName, categorizationResult.Explanation, hasTranscript, jobId, ct: ct);
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
                        await _processedUrlRepo.RecordFailureAsync(raw.ExternalUrl, ex.Message, raw.FeedName, raw.CollectionName, reason: null, hasTranscript: null, jobId: jobId, ct: ct);
                        errorCount++;
                    }

                    // Flush log and counters to DB after each URL so the frontend shows live progress
                    await FlushProgressAsync();
                }
            }

            // Clean up old job records
            await _jobRepo.PurgeOldJobsAsync(_options.PurgeJobKeepCount, ct);

            // Purge old failed URL records so they can be retried
            if (_options.FailedUrlRetentionDays > 0)
            {
                await _processedUrlRepo.PurgeFailedAsync(TimeSpan.FromDays(_options.FailedUrlRetentionDays), ct);
            }

            var duration = _timeProvider.GetUtcNow() - startedAt;
            Log(string.Create(CultureInfo.InvariantCulture,
                $"Run complete. Added: {itemsAdded}, Skipped: {itemsSkipped}, Errors: {errorCount}, Duration: {duration.TotalSeconds:F1}s"));

            await _jobRepo.CompleteAsync(jobId, feedsProcessed, itemsAdded, itemsSkipped, errorCount, transcriptsSucceeded, transcriptsFailed, log.ToString(), ct: ct);
        }
        catch (OperationCanceledException)
        {
            Log("Run cancelled.");
            await TryAbortJobAsync(jobId, feedsProcessed, itemsAdded, itemsSkipped, errorCount, transcriptsSucceeded, transcriptsFailed, log.ToString());
        }
        catch (Exception ex) when (ex is not OutOfMemoryException and not StackOverflowException)
        {
            Log(string.Create(CultureInfo.InvariantCulture, $"FATAL ERROR: {ex.Message}"));
            _logger.LogError(ex, "Content processing run {JobId} failed with unhandled exception", jobId);
            await TryFailJobAsync(jobId, feedsProcessed, itemsAdded, itemsSkipped, errorCount, transcriptsSucceeded, transcriptsFailed, log.ToString(), ct);
        }
    }

    private async Task TryFailJobAsync(long jobId, int feedsProcessed, int itemsAdded, int itemsSkipped, int errorCount, int transcriptsSucceeded, int transcriptsFailed, string logOutput, CancellationToken ct)
    {
        if (jobId <= 0)
        {
            return;
        }

        try
        {
            await _jobRepo.FailAsync(jobId, feedsProcessed, itemsAdded, itemsSkipped, errorCount, transcriptsSucceeded, transcriptsFailed, logOutput, ct: ct);
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to mark job {JobId} as failed", jobId);
        }
    }

    private async Task TryAbortJobAsync(long jobId, int feedsProcessed, int itemsAdded, int itemsSkipped, int errorCount, int transcriptsSucceeded, int transcriptsFailed, string logOutput)
    {
        if (jobId <= 0)
        {
            return;
        }

        try
        {
            await _jobRepo.AbortJobAsync(jobId, feedsProcessed, itemsAdded, itemsSkipped, errorCount, transcriptsSucceeded, transcriptsFailed, logOutput, ct: CancellationToken.None);
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to mark job {JobId} as aborted", jobId);
        }
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

    /// <summary>
    /// Checks whether the given feed name and title match any configured subcollection rule.
    /// Returns the subcollection name if a rule matches, or <c>null</c> otherwise.
    /// </summary>
    internal string? MatchSubcollectionRule(string feedName, string title)
    {
        foreach (var rule in _options.SubcollectionRules)
        {
            if (!rule.FeedName.Equals(feedName, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (MatchesWildcardPattern(title, rule.TitlePattern))
            {
                return rule.Subcollection;
            }
        }

        return null;
    }

    /// <summary>
    /// Matches a string against a wildcard pattern where <c>*</c> matches any sequence of characters.
    /// Case-insensitive.
    /// </summary>
    internal static bool MatchesWildcardPattern(string input, string pattern)
    {
        if (string.IsNullOrEmpty(pattern))
        {
            return false;
        }

        // Convert wildcard pattern to regex: escape everything except *, then replace * with .*
        var regexPattern = "^" + Regex.Escape(pattern).Replace("\\*", ".*", StringComparison.Ordinal) + "$";
        return Regex.IsMatch(input, regexPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }
}
