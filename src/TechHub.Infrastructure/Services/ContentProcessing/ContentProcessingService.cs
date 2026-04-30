using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Logging;
using TechHub.Core.Models.Admin;
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

                    try
                    {
                        // Pre-AI delay to prevent rate limiting (like the original PS scripts: 15s between every call)
                        if (_options.RequestDelayMs > 0 && i > 0)
                        {
                            await Task.Delay(_options.RequestDelayMs, ct);
                        }

                        // Flush before pipeline so the dashboard is current during the longest operation
                        await FlushProgressAsync();

                        // Shared per-item pipeline: tags → content → transcript → AI → write
                        var itemResult = await ProcessItemAsync(raw, jobId, forcedSubcollection: null, msg => Log($"  {msg}"), feed.TranscriptMandatory, ct);

                        // Emit any supplemental informational messages (date capping, subcollection match)
                        foreach (var line in itemResult.LogLines)
                        {
                            Log(line);
                        }

                        // Update transcript counters
                        if (itemResult.HasTranscript == true)
                        {
                            transcriptsSucceeded++;
                        }
                        else if (itemResult.HasTranscript == false)
                        {
                            transcriptsFailed++;
                        }

                        // Log outcome and update counters
                        switch (itemResult.Outcome)
                        {
                            case AdHocUrlProcessOutcome.Added:
                                Log(string.Create(CultureInfo.InvariantCulture, $"  ✓ Added: {itemResult.Title ?? raw.ExternalUrl}"));
                                itemsAdded++;
                                break;
                            case AdHocUrlProcessOutcome.Skipped:
                                Log(string.Create(CultureInfo.InvariantCulture, $"  ⊘ Skipped: {itemResult.Title ?? raw.ExternalUrl}"));
                                itemsSkipped++;
                                break;
                            default:
                                Log(string.Create(CultureInfo.InvariantCulture,
                                    $"  ✗ Failed: {itemResult.Title ?? raw.ExternalUrl} — {TruncateLogReason(itemResult.Message)}"));
                                errorCount++;
                                break;
                        }
                    }
                    catch (OperationCanceledException) when (ct.IsCancellationRequested)
                    {
                        throw;
                    }
                    catch (Exception ex) when (ex is not OutOfMemoryException and not StackOverflowException)
                    {
                        Log(string.Create(CultureInfo.InvariantCulture, $"  ✗ Error: {raw.ExternalUrl} — {ex.Message}"));
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

    /// <summary>
    /// Processes a single URL ad-hoc, outside the scheduled RSS pipeline.
    /// <para>
    /// The URL is checked against <see cref="IContentItemWriteRepository.ExistsByExternalUrlAsync"/>
    /// first — if already present the method returns <see langword="null"/> to signal a duplicate.
    /// </para>
    /// </summary>
    /// <param name="url">Absolute HTTP/HTTPS URL to process.</param>
    /// <param name="collectionName">Target collection (e.g. "blogs", "news", "videos").</param>
    /// <param name="feedName">Attribution name stored in processed_urls and content_items.</param>
    /// <param name="subcollectionName">Optional subcollection to force (e.g. "ghc-features").</param>
    /// <param name="titleHint">
    /// Optional hint passed to the AI about the expected title.
    /// The AI will use it as a starting point but extract the final title from fetched content.
    /// </param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>
    /// <see langword="null"/> when the URL is already in the database (HTTP 409 Conflict).
    /// Otherwise a <see cref="AdHocUrlProcessResult"/> describing the outcome.
    /// </returns>
    public async Task<AdHocUrlProcessResult?> ProcessSingleAsync(
        string url,
        string collectionName,
        string feedName,
        string? subcollectionName = null,
        string? titleHint = null,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url);
        ArgumentException.ThrowIfNullOrWhiteSpace(collectionName);
        ArgumentException.ThrowIfNullOrWhiteSpace(feedName);

        // Duplicate check — caller should map null to HTTP 409 Conflict
        if (await _writeRepo.ExistsByExternalUrlAsync(url, ct))
        {
            return null;
        }

        var startedAt = _timeProvider.GetUtcNow();
        var log = new StringBuilder();
        long jobId = 0;

        void Log(string msg)
        {
            var line = string.Create(CultureInfo.InvariantCulture,
                $"[{_timeProvider.GetUtcNow():HH:mm:ss}] {msg}");
            log.AppendLine(line);
            _logger.LogInformation("AdHocProcessing[{JobId}] {Message}", jobId, msg);
        }

        try
        {
            jobId = await _jobRepo.CreateAsync("manual", ContentProcessingJobType.AdHocProcessing, ct);
            Log(string.Create(CultureInfo.InvariantCulture, $"Ad-hoc processing: {url.Sanitize()} → {collectionName} (feed: {feedName})"));
            if (!string.IsNullOrWhiteSpace(subcollectionName))
            {
                Log(string.Create(CultureInfo.InvariantCulture, $"  Subcollection: {subcollectionName}"));
            }

            // Use the title hint for logging and subcollection rules; AI extracts the real title
            var feedItemData = !string.IsNullOrWhiteSpace(titleHint)
                ? string.Create(CultureInfo.InvariantCulture, $"TITLE_HINT: {titleHint}")
                : string.Empty;

            var raw = new RawFeedItem
            {
                Title = !string.IsNullOrWhiteSpace(titleHint) ? titleHint : url,
                ExternalUrl = url,
                PublishedAt = _timeProvider.GetUtcNow(),
                FeedName = feedName,
                CollectionName = collectionName,
                FeedItemData = feedItemData
            };

            // Shared per-item pipeline: tags → content → transcript → AI → write
            var itemResult = await ProcessItemAsync(raw, jobId, subcollectionName, Log, ct: ct);

            // Emit any supplemental informational messages (date capping, subcollection match)
            foreach (var line in itemResult.LogLines)
            {
                Log(line);
            }

            var itemsAdded = itemResult.Outcome == AdHocUrlProcessOutcome.Added ? 1 : 0;
            var itemsSkipped = itemResult.Outcome == AdHocUrlProcessOutcome.Skipped ? 1 : 0;
            var errorCount = itemResult.Outcome == AdHocUrlProcessOutcome.Failed ? 1 : 0;
            var transcriptsSucceeded = itemResult.HasTranscript == true ? 1 : 0;
            var transcriptsFailed = itemResult.HasTranscript == false ? 1 : 0;

            var statusIcon = itemResult.Outcome switch
            {
                AdHocUrlProcessOutcome.Added => "✓ Added",
                AdHocUrlProcessOutcome.Skipped => "⊘ Skipped",
                _ => "✗ Failed"
            };
            Log(string.Create(CultureInfo.InvariantCulture, $"{statusIcon}: {itemResult.Title ?? itemResult.Message}"));

            var duration = _timeProvider.GetUtcNow() - startedAt;
            Log(string.Create(CultureInfo.InvariantCulture, $"Completed in {duration.TotalSeconds:F1}s"));

            await _jobRepo.CompleteAsync(jobId, feedsProcessed: 0, itemsAdded, itemsSkipped, errorCount,
                transcriptsSucceeded, transcriptsFailed, log.ToString(), ct: ct);

            return new AdHocUrlProcessResult
            {
                Outcome = itemResult.Outcome,
                Slug = itemResult.Slug,
                Message = itemResult.Message
            };
        }
        catch (OperationCanceledException)
        {
            Log("Ad-hoc processing cancelled.");
            await TryAbortJobAsync(jobId, 0, 0, 0, 0, 0, 0, log.ToString());
            throw;
        }
        catch (Exception ex) when (ex is not OutOfMemoryException and not StackOverflowException)
        {
            Log(string.Create(CultureInfo.InvariantCulture, $"FATAL ERROR: {ex.Message}"));
            _logger.LogError(ex, "Ad-hoc processing job {JobId} failed with unhandled exception", jobId);
            await TryFailJobAsync(jobId, 0, 0, 0, 1, 0, 0, log.ToString(), ct);
            throw;
        }
    }

    /// <summary>
    /// Result returned by <see cref="CategorizeWriteAndRecordAsync"/> and <see cref="ProcessItemAsync"/>.
    /// </summary>
    private sealed record ItemPipelineResult
    {
        public required AdHocUrlProcessOutcome Outcome { get; init; }
        public string? Slug { get; init; }
        public string? Title { get; init; }
        public required string Message { get; init; }

        /// <summary>Supplemental informational lines for the caller's log (date capping, subcollection match, etc.).</summary>
        public IReadOnlyList<string> LogLines { get; init; } = [];

        /// <summary>Number of YouTube tags fetched (0 for non-YouTube items).</summary>
        public int YouTubeTagCount { get; init; }

        /// <summary>Transcript status: true = fetched, false = failed/unavailable, null = non-YouTube.</summary>
        public bool? HasTranscript { get; init; }

        /// <summary>Human-readable transcript status for log messages (e.g. "transcript fetched", "transcript failed: reason").</summary>
        public string? TranscriptStatus { get; init; }
    }

    /// <summary>
    /// Shared per-item pipeline used by both batch and ad-hoc processing.
    /// Performs: YouTube tag fetching → content enrichment → transcript tracking → AI categorization → DB write.
    /// </summary>
    /// <param name="raw">The raw feed item to process.</param>
    /// <param name="jobId">The job ID for tracking (nullable for legacy callers).</param>
    /// <param name="forcedSubcollection">If set, forces this subcollection instead of using config rules.</param>
    /// <param name="logAction">Callback to append log lines to the caller's log.</param>
    /// <param name="transcriptMandatory">When true, YouTube items without a transcript are failed immediately (skipping AI).</param>
    /// <param name="ct">Cancellation token.</param>
    private async Task<ItemPipelineResult> ProcessItemAsync(
        RawFeedItem raw,
        long? jobId,
        string? forcedSubcollection,
        Action<string> logAction,
        bool transcriptMandatory = false,
        CancellationToken ct = default)
    {
        // 1. Fetch YouTube tags if applicable
        var ytTagCount = 0;
        if (raw.IsYouTube && _options.MaxYouTubeTagCount > 0)
        {
            try
            {
                var ytTags = await _youtubeTagService.GetTagsAsync(raw.ExternalUrl, ct);
                ytTagCount = ytTags.Count;
                logAction(string.Create(CultureInfo.InvariantCulture, $"Fetched {ytTags.Count} YouTube tag(s) from API"));
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
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogWarning(ex, "Failed to fetch YouTube tags for {Url}", raw.ExternalUrl);
                logAction(string.Create(CultureInfo.InvariantCulture, $"⚠ YouTube tag fetch failed: {ex.Message}"));
            }
        }

        // 2. Enrich with content (YouTube transcript or article body)
        string fetchLabel;
        if (raw.IsYouTube)
        {
            var ye = _options.YouTubeExplodeEnabled;
            var yd = _options.YtDlpEnabled;
            var strategy = (ye, yd) switch
            {
                (false, false) => "disabled",
                (false, true) => "yt-dlp (YoutubeExplode disabled)",
                (true, false) => "YoutubeExplode only",
                _ => "YoutubeExplode → yt-dlp fallback"
            };
            fetchLabel = string.Create(CultureInfo.InvariantCulture, $"Fetching transcript (via {strategy})…");
        }
        else
        {
            fetchLabel = "Fetching article content…";
        }

        logAction(fetchLabel);
        var hadContentBefore = !string.IsNullOrWhiteSpace(raw.FullContent);
        try
        {
            raw = await _articleService.EnrichWithContentAsync(raw, ct);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogWarning(ex, "Failed to enrich content for {Url}", raw.ExternalUrl);
            logAction(string.Create(CultureInfo.InvariantCulture, $"⚠ Content enrichment failed: {ex.Message}"));
        }

        // 3. Track transcript outcome for YouTube items
        var hasContentAfter = !string.IsNullOrWhiteSpace(raw.FullContent);
        var contentFetched = !hadContentBefore && hasContentAfter;
        bool? hasTranscript = raw.IsYouTube ? contentFetched : null;
        string? transcriptStatus = null;

        if (raw.IsYouTube)
        {
            transcriptStatus = contentFetched
                ? "transcript fetched"
                : $"transcript failed: {(raw.TranscriptFailureReason ?? "unknown").Sanitize()}";

            if (contentFetched)
            {
                logAction("Transcript fetched successfully");
            }
            else
            {
                logAction(string.Create(CultureInfo.InvariantCulture,
                    $"⚠ Transcript unavailable: {(raw.TranscriptFailureReason ?? "unknown").Sanitize()}"));

                // Enforce TranscriptMandatory — fail the item early without calling AI
                if (transcriptMandatory)
                {
                    const string Reason = "Transcript mandatory but not available";
                    logAction(string.Create(CultureInfo.InvariantCulture, $"✗ {Reason}"));
                    await _processedUrlRepo.RecordFailureAsync(raw.ExternalUrl, Reason, raw.FeedName, raw.CollectionName, reason: null, hasTranscript: false, jobId, ct: ct);
                    return new ItemPipelineResult
                    {
                        Outcome = AdHocUrlProcessOutcome.Failed,
                        Title = raw.Title,
                        Message = Reason,
                        YouTubeTagCount = ytTagCount,
                        HasTranscript = false,
                        TranscriptStatus = transcriptStatus
                    };
                }
            }
        }

        // 4. AI categorization + post-processing + DB write
        logAction("Running AI categorization…");
        var itemResult = await CategorizeWriteAndRecordAsync(raw, hasTranscript, jobId, forcedSubcollection, ct);

        if (itemResult.Outcome != AdHocUrlProcessOutcome.Failed)
        {
            var include = itemResult.Outcome == AdHocUrlProcessOutcome.Added;
            logAction(string.Create(CultureInfo.InvariantCulture, $"AI categorization completed. Include: {include}. Reason: {itemResult.Message}"));
        }

        return new ItemPipelineResult
        {
            Outcome = itemResult.Outcome,
            Slug = itemResult.Slug,
            Title = itemResult.Title,
            Message = itemResult.Message,
            LogLines = itemResult.LogLines,
            YouTubeTagCount = ytTagCount,
            HasTranscript = hasTranscript,
            TranscriptStatus = transcriptStatus
        };
    }

    /// <summary>
    /// Shared pipeline: AI categorization → post-processing → DB write → processed_url recording.
    /// Called by both the scheduled batch loop and <see cref="ProcessSingleAsync"/>.
    /// </summary>
    private async Task<ItemPipelineResult> CategorizeWriteAndRecordAsync(
        RawFeedItem raw,
        bool? hasTranscript,
        long? jobId,
        string? forcedSubcollection,
        CancellationToken ct)
    {
        // AI categorization
        CategorizationResult categorizationResult;
        try
        {
            categorizationResult = await _aiService.CategorizeAsync(raw, ct);
        }
        catch (OperationCanceledException ex) when (!ct.IsCancellationRequested)
        {
            // Polly timeout or internal AI timeout (not user cancellation)
            _logger.LogError(ex, "AI categorization timed out for {Url}", raw.ExternalUrl);
            await _processedUrlRepo.RecordFailureAsync(raw.ExternalUrl, ex.Message, raw.FeedName, raw.CollectionName, reason: null, hasTranscript, jobId, ct: ct);
            return new ItemPipelineResult { Outcome = AdHocUrlProcessOutcome.Failed, Title = raw.Title, Message = ex.Message };
        }
        catch (Exception ex) when (ex is HttpRequestException or JsonException or InvalidOperationException or TimeoutException)
        {
            _logger.LogError(ex, "AI categorization failed for {Url}", raw.ExternalUrl);
            await _processedUrlRepo.RecordFailureAsync(raw.ExternalUrl, ex.Message, raw.FeedName, raw.CollectionName, reason: null, hasTranscript, jobId, ct: ct);
            return new ItemPipelineResult { Outcome = AdHocUrlProcessOutcome.Failed, Title = raw.Title, Message = ex.Message };
        }

        if (categorizationResult.Item == null)
        {
            if (categorizationResult.IsFailure)
            {
                await _processedUrlRepo.RecordFailureAsync(raw.ExternalUrl, categorizationResult.Explanation, raw.FeedName, raw.CollectionName, reason: null, hasTranscript, jobId, ct: ct);
                return new ItemPipelineResult { Outcome = AdHocUrlProcessOutcome.Failed, Title = raw.Title, Message = categorizationResult.Explanation };
            }

            await _processedUrlRepo.RecordSkippedAsync(raw.ExternalUrl, feedName: raw.FeedName, collectionName: raw.CollectionName, reason: categorizationResult.Explanation, hasTranscript, jobId, ct: ct);
            return new ItemPipelineResult { Outcome = AdHocUrlProcessOutcome.Skipped, Title = raw.Title, Message = categorizationResult.Explanation };
        }

        var processed = categorizationResult.Item;
        var logLines = new List<string>();

        // Cap future-dated items to the processing date.
        // Some feeds publish articles with dates a few days in the future.
        // Without this cap, the item would be excluded from weekly roundups
        // which filter by date range relative to the run time.
        var nowEpoch = _timeProvider.GetUtcNow().ToUnixTimeSeconds();
        if (processed.DateEpoch > nowEpoch)
        {
            logLines.Add(string.Create(CultureInfo.InvariantCulture,
                $"  → Future date capped to now: {raw.ExternalUrl.Sanitize()} (was {DateTimeOffset.FromUnixTimeSeconds(processed.DateEpoch):yyyy-MM-dd})"));
            processed = processed.WithDateEpoch(nowEpoch);
        }

        // Apply subcollection: forced takes priority, then config rules
        if (!string.IsNullOrWhiteSpace(forcedSubcollection))
        {
            processed = processed.WithSubcollectionName(forcedSubcollection);
        }
        else
        {
            var matchedSubcollection = MatchSubcollectionRule(raw.FeedName, raw.Title);
            if (matchedSubcollection != null)
            {
                processed = processed.WithSubcollectionName(matchedSubcollection);
                logLines.Add(string.Create(CultureInfo.InvariantCulture,
                    $"  → Subcollection rule matched: {matchedSubcollection}"));
            }
        }

        // Repair markdown before writing
        if (!string.IsNullOrWhiteSpace(processed.Content))
        {
            processed = processed.WithContent(_contentFixer.RepairMarkdown(processed.Content));
        }

        // Ensure section-derived tags are present (e.g., sections=["ai"] → tag "AI")
        processed = processed.WithTags(TagNormalizer.EnsureSectionTags(processed.Tags, processed.Sections));

        // Normalize tags (fix casing, remove noise, deduplicate)
        processed = processed.WithTags(TagNormalizer.NormalizeTags(processed.Tags));

        // Write to database
        try
        {
            await _writeRepo.UpsertProcessedItemAsync(processed, ct);
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to write content item for {Url}", raw.ExternalUrl);
            await _processedUrlRepo.RecordFailureAsync(raw.ExternalUrl, ex.Message, raw.FeedName, processed.CollectionName, categorizationResult.Explanation, hasTranscript, jobId, ct: ct);
            return new ItemPipelineResult { Outcome = AdHocUrlProcessOutcome.Failed, Message = $"Database write failed: {ex.Message}" };
        }

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

        return new ItemPipelineResult
        {
            Outcome = AdHocUrlProcessOutcome.Added,
            Slug = processed.Slug,
            Title = processed.Title,
            Message = categorizationResult.Explanation,
            LogLines = logLines
        };
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
