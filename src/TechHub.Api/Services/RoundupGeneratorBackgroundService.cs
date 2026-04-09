using System.Threading.Channels;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.Admin;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Api.Services;

/// <summary>
/// Runs the weekly roundup generation pipeline every Monday at <see cref="RoundupGeneratorOptions.RunHourUtc"/> UTC.
/// When triggered, it generates a roundup for the previous Monday-to-Sunday week and writes it directly
/// to the <c>content_items</c> database table.
/// Invoked manually by the admin UI via <see cref="TriggerImmediateRun"/>.
/// </summary>
public sealed class RoundupGeneratorBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly RoundupGeneratorOptions _options;
    private readonly StartupStateService _startupState;
    private readonly ILogger<RoundupGeneratorBackgroundService> _logger;

    // Always holds a live (incomplete) TCS that the background loop awaits.
    // TriggerImmediateRun completes it; the loop then replaces it with a fresh one.
    private volatile TaskCompletionSource<bool> _manualTrigger = new(TaskCreationOptions.RunContinuationsAsynchronously);

    // Per-run CTS for admin-triggered cancellation. Linked to the host's stoppingToken.
    // Disposed in RunOnceAsync finally block; also disposed on service shutdown.
#pragma warning disable CA2213 // Disposed in RunOnceAsync finally block and Dispose override
    private volatile CancellationTokenSource? _runCts;
#pragma warning restore CA2213

    public RoundupGeneratorBackgroundService(
        IServiceProvider serviceProvider,
        IOptions<RoundupGeneratorOptions> options,
        StartupStateService startupState,
        ILogger<RoundupGeneratorBackgroundService> logger)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(startupState);
        ArgumentNullException.ThrowIfNull(logger);

        _serviceProvider = serviceProvider;
        _options = options.Value;
        _startupState = startupState;
        _logger = logger;
    }

    /// <summary>
    /// Signals the background loop to run a roundup generation job immediately (admin trigger).
    /// Returns immediately; the actual run happens asynchronously in the background.
    /// </summary>
    public void TriggerImmediateRun()
    {
        if (_manualTrigger.TrySetResult(true))
        {
            _logger.LogInformation("Manual roundup generation run triggered");
        }
        else
        {
            _logger.LogInformation("Manual roundup trigger requested but a run is already queued");
        }
    }

    /// <summary>
    /// Cancels the currently running roundup generation job (admin cancel).
    /// Returns <c>true</c> if a run was in progress and cancellation was requested.
    /// </summary>
    public bool CancelCurrentRun()
    {
        var cts = _runCts;
        if (cts is null || cts.IsCancellationRequested)
        {
            return false;
        }

        cts.Cancel();
        _logger.LogInformation("Admin-triggered cancellation of roundup generation run");
        return true;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Waiting for startup operations to complete before first roundup check…");
        await _startupState.StartupTask.WaitAsync(stoppingToken);

        var enabled = await IsEnabledAsync(stoppingToken);

        if (!enabled)
        {
            _logger.LogInformation("RoundupGeneratorBackgroundService is disabled via database setting. Manual triggers via admin UI are still accepted.");
        }
        else
        {
            _logger.LogInformation(
                "RoundupGeneratorBackgroundService started — scheduled every Monday at {RunHourUtc:00}:00 UTC",
                _options.RunHourUtc);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            var delay = ComputeDelayUntilNextRun(DateTime.UtcNow);

            _logger.LogInformation(
                "Next roundup generation scheduled in {TotalHours:F1} hours",
                delay.TotalHours);

            // Wait for either the scheduled delay or a manual trigger
            var manualTask = _manualTrigger.Task;
            var delayTask = Task.Delay(delay, stoppingToken);

            await Task.WhenAny(manualTask, delayTask);

            if (stoppingToken.IsCancellationRequested)
            {
                break;
            }

            if (manualTask.IsCompleted)
            {
                // Manual triggers always execute regardless of enabled state
                _manualTrigger = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
                await RunOnceAsync("manual", stoppingToken);
            }
            else
            {
                // Scheduled runs check the database setting each time
                if (await IsEnabledAsync(stoppingToken))
                {
                    await RunOnceAsync("scheduled", stoppingToken);
                }
            }
        }
    }

    private async Task<bool> IsEnabledAsync(CancellationToken ct)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var repo = scope.ServiceProvider.GetRequiredService<IBackgroundJobSettingRepository>();
        return await repo.IsEnabledAsync(RoundupGeneratorOptions.SectionName, ct);
    }

#pragma warning disable CA1031
    private async Task RunOnceAsync(string triggerType, CancellationToken ct)
    {
        _runCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        var runToken = _runCts.Token;
        try
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var jobRepo = scope.ServiceProvider.GetRequiredService<IContentProcessingJobRepository>();
            var jobId = await jobRepo.CreateAsync(triggerType, ContentProcessingJobType.RoundupGeneration, runToken);

            try
            {
                // Generate roundup for the previous week (Monday–Sunday).
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                var daysSinceMonday = ((int)today.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
                var thisWeekMonday = today.AddDays(-daysSinceMonday);

                // Previous week: the Monday before this week's Monday.
                var weekStart = thisWeekMonday.AddDays(-7);
                var weekEnd = weekStart.AddDays(6);

                _logger.LogInformation(
                    "Running roundup generation for week {WeekStart}–{WeekEnd} (job {JobId})",
                    weekStart, weekEnd, jobId);

                await jobRepo.AppendLogAsync(jobId, $"Generating roundup for week {weekStart}–{weekEnd}", runToken);

                // Use a channel to serialize all progress log appends in the order they were reported.
                // Progress<T> posts callbacks to the thread pool, which would cause concurrent AppendLogAsync
                // calls to race — producing out-of-order messages in the UI log.
                var logChannel = Channel.CreateUnbounded<string>(new UnboundedChannelOptions { SingleReader = true });
                var logConsumerTask = Task.Run(async () =>
                {
                    await foreach (var message in logChannel.Reader.ReadAllAsync(CancellationToken.None))
                    {
                        await jobRepo.AppendLogAsync(jobId, message, CancellationToken.None);
                    }
                }, CancellationToken.None);

                RoundupGenerationOutcome outcome;
                try
                {
                    var service = scope.ServiceProvider.GetRequiredService<IRoundupGeneratorService>();
                    var progress = new Progress<string>(message => logChannel.Writer.TryWrite(message));
                    outcome = await service.GenerateAsync(weekStart, weekEnd, progress, jobId, runToken);
                }
                finally
                {
                    // Always complete the channel so the consumer task finishes
                    // and all buffered progress messages are written before terminal entries.
                    logChannel.Writer.Complete();
                    await logConsumerTask;
                }

                if (outcome.Result == RoundupGenerationResult.Generated)
                {
                    _logger.LogInformation("Roundup generated successfully for week {WeekStart}–{WeekEnd}", weekStart, weekEnd);

                    await jobRepo.AppendLogAsync(jobId, "Roundup generated successfully.", runToken);
                    await jobRepo.CompleteAsync(jobId, feedsProcessed: 0, itemsAdded: 1, itemsSkipped: 0, errorCount: 0,
                        transcriptsSucceeded: 0, transcriptsFailed: 0,
                        logOutput: null, ct: runToken);

                    // Invalidate content cache so the new roundup is served immediately
                    var contentRepo = scope.ServiceProvider.GetRequiredService<IContentRepository>();
                    contentRepo.InvalidateCachedData();
                }
                else if (outcome.Result == RoundupGenerationResult.ContentGenerationFailed)
                {
                    _logger.LogError("Roundup content generation failed for week {WeekStart}–{WeekEnd}", weekStart, weekEnd);
                    await jobRepo.AppendLogAsync(jobId, "Content generation failed — AI produced no usable content.", runToken);
                    await jobRepo.FailAsync(jobId, feedsProcessed: 0, itemsAdded: 0, itemsSkipped: 0, errorCount: 1,
                        transcriptsSucceeded: 0, transcriptsFailed: 0,
                        logOutput: null, ct: runToken);
                }
                else
                {
                    var reason = outcome.Result switch
                    {
                        RoundupGenerationResult.AlreadyExists => "Roundup already existed, skipped.",
                        RoundupGenerationResult.NoArticles => "No articles found for this week, skipped.",
                        RoundupGenerationResult.NoArticlesAfterFiltering => "No articles remained after relevance filtering, skipped.",
                        _ => "Roundup generation skipped."
                    };

                    _logger.LogInformation("Roundup generation skipped for week {WeekStart}–{WeekEnd}: {Reason}", weekStart, weekEnd, reason);
                    await jobRepo.AppendLogAsync(jobId, reason, runToken);
                    await jobRepo.CompleteAsync(jobId, feedsProcessed: 0, itemsAdded: 0, itemsSkipped: 1, errorCount: 0,
                        transcriptsSucceeded: 0, transcriptsFailed: 0,
                        logOutput: null, ct: runToken);
                }
            }
            catch (OperationCanceledException) when (!ct.IsCancellationRequested)
            {
                // Admin-triggered cancellation — mark as aborted
                _logger.LogInformation("Roundup generation job {JobId} cancelled by admin", jobId);
                await jobRepo.AppendLogAsync(jobId, "Roundup generation cancelled by admin.", CancellationToken.None);
                await jobRepo.AbortJobAsync(jobId, feedsProcessed: 0, itemsAdded: 0, itemsSkipped: 0, errorCount: 0,
                    transcriptsSucceeded: 0, transcriptsFailed: 0,
                    logOutput: null, ct: CancellationToken.None);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                // Shutting down — expected
            }
            catch (Exception ex) when (ex is not OutOfMemoryException and not StackOverflowException)
            {
                _logger.LogError(ex, "Unexpected exception in RoundupGeneratorBackgroundService (job {JobId})", jobId);
                await jobRepo.AppendLogAsync(jobId, $"Roundup generation failed: {ex.Message}\n{ex.StackTrace}", CancellationToken.None);
                await jobRepo.FailAsync(jobId, feedsProcessed: 0, itemsAdded: 0, itemsSkipped: 0, errorCount: 1,
                    transcriptsSucceeded: 0, transcriptsFailed: 0,
                    logOutput: null, ct: CancellationToken.None);
            }
        }
        finally
        {
            _runCts.Dispose();
            _runCts = null;
        }
    }
#pragma warning restore CA1031

    /// <summary>
    /// Calculates the delay until the next Monday at <see cref="RoundupGeneratorOptions.RunHourUtc"/> UTC.
    /// </summary>
    public TimeSpan ComputeDelayUntilNextRun(DateTime nowUtc)
    {
        var targetTime = new TimeOnly(_options.RunHourUtc, 0, 0);

        // Find the next Monday at the configured hour.
        var daysUntilMonday = ((int)DayOfWeek.Monday - (int)nowUtc.DayOfWeek + 7) % 7;

        // If today is already Monday but we haven't passed the target hour yet, run today.
        if (daysUntilMonday == 0 && nowUtc.TimeOfDay < targetTime.ToTimeSpan())
        {
            daysUntilMonday = 0;
        }
        else if (daysUntilMonday == 0)
        {
            // We already passed this Monday's run hour — schedule for next Monday.
            daysUntilMonday = 7;
        }

        var nextRun = nowUtc.Date.AddDays(daysUntilMonday) + targetTime.ToTimeSpan();
        var delay = nextRun - nowUtc;

        return delay < TimeSpan.Zero ? TimeSpan.Zero : delay;
    }
}
