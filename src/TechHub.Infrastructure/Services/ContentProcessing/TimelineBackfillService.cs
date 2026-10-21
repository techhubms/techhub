using System.Globalization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Logging;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Services.ContentProcessing;

/// <summary>
/// Scans stored articles for dated lifecycle events (preview, GA, deprecation, ...) and stores them in the timeline table.
/// Only articles that were never scanned (or scanned with an older prompt version) are processed, so a run that is
/// cancelled or fails can simply be started again. A failing article is logged and left unscanned; the run continues.
/// </summary>
public sealed class TimelineBackfillService : ITimelineBackfillService
{
    private const int ProgressInterval = 25;

    private readonly ITimelineEventRepository _timelineRepo;
    private readonly IAiCategorizationService _aiService;
    private readonly AiCategorizationOptions _options;
    private readonly ILogger<TimelineBackfillService> _logger;

    public TimelineBackfillService(
        ITimelineEventRepository timelineRepo,
        IAiCategorizationService aiService,
        IOptions<AiCategorizationOptions> options,
        ILogger<TimelineBackfillService> logger)
    {
        ArgumentNullException.ThrowIfNull(timelineRepo);
        ArgumentNullException.ThrowIfNull(aiService);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _timelineRepo = timelineRepo;
        _aiService = aiService;
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<TimelineBackfillResult> RunAsync(TimelineBackfillRequest request, Func<string, Task> log, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(log);

        var candidates = await _timelineRepo.GetBackfillCandidatesAsync(request, ITimelineEventRepository.CurrentPromptVersion, ct);
        await log(string.Create(CultureInfo.InvariantCulture,
            $"{candidates.Count} articles to scan (force={request.Force}, published after={request.PublishedAfter?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? "any"}, collection={request.Collection ?? "all"}, limit={request.Limit?.ToString(CultureInfo.InvariantCulture) ?? "none"})"));

        var scanned = 0;
        var withEvents = 0;
        var eventsStored = 0;
        var failed = 0;
        var processed = 0;

        foreach (var candidate in candidates)
        {
            ct.ThrowIfCancellationRequested();
            processed++;

#pragma warning disable CA1031 // Best-effort: continue with other articles if one fails
            try
            {
                var item = await _timelineRepo.GetItemForExtractionAsync(candidate.CollectionName, candidate.Slug, ct);
                if (item is null)
                {
                    continue;
                }

                var raw = new RawFeedItem
                {
                    Title = item.Title,
                    ExternalUrl = item.ExternalUrl,
                    PublishedAt = DateTimeOffset.FromUnixTimeSeconds(item.DateEpoch),
                    FeedName = item.FeedName,
                    CollectionName = item.CollectionName,
                    FullContent = item.Content
                };

                var result = await _aiService.ExtractTimelineEventsAsync(raw, ct);
                if (result.IsFailure)
                {
                    failed++;
                    await log($"FAILED {candidate.CollectionName}/{candidate.Slug}: {result.Explanation}");
                    continue;
                }

                await _timelineRepo.ReplaceEventsAsync(
                    candidate.CollectionName,
                    candidate.Slug,
                    result.Events,
                    ITimelineEventRepository.ExtractedFromStoredContent,
                    ITimelineEventRepository.CurrentPromptVersion,
                    ct);

                scanned++;
                eventsStored += result.Events.Count;
                if (result.Events.Count > 0)
                {
                    withEvents++;
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                failed++;
                _logger.LogWarning(ex, "Timeline backfill failed for {Collection}/{Slug}", candidate.CollectionName.Sanitize(), candidate.Slug.Sanitize());
                await log($"FAILED {candidate.CollectionName}/{candidate.Slug}: {ex.Message}");
            }
#pragma warning restore CA1031

            if (processed % ProgressInterval == 0)
            {
                await log(string.Create(CultureInfo.InvariantCulture,
                    $"Progress {processed}/{candidates.Count}: {scanned} scanned, {eventsStored} events from {withEvents} articles, {failed} failed"));
            }

            if (_options.TimelineBackfillDelaySeconds > 0 && processed < candidates.Count)
            {
                await Task.Delay(TimeSpan.FromSeconds(_options.TimelineBackfillDelaySeconds), ct);
            }
        }

        var summary = new TimelineBackfillResult
        {
            Candidates = candidates.Count,
            Scanned = scanned,
            WithEvents = withEvents,
            EventsStored = eventsStored,
            Failed = failed
        };

        await log(string.Create(CultureInfo.InvariantCulture,
            $"Done: {summary.Scanned}/{summary.Candidates} scanned, {summary.EventsStored} events from {summary.WithEvents} articles, {summary.Failed} failed"));
        return summary;
    }
}
