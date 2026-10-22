using System.Globalization;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.Admin;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Api.Services;

/// <summary>
/// Manual-trigger-only background service that scans stored articles for timeline events.
/// No periodic schedule: it runs only when triggered via the admin endpoint, with the options of that trigger.
/// </summary>
public sealed class TimelineBackfillBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly StartupStateService _startupState;
    private readonly ILogger<TimelineBackfillBackgroundService> _logger;

    private volatile TaskCompletionSource<TimelineBackfillRequest> _manualTrigger = new(TaskCreationOptions.RunContinuationsAsynchronously);

    // Guards assignment, cancellation and disposal of _runCts so a cancel can never hit a disposed source.
    private readonly object _runCtsLock = new();

#pragma warning disable CA2213 // Disposed in RunOnceAsync finally block
    private CancellationTokenSource? _runCts;
#pragma warning restore CA2213

    public TimelineBackfillBackgroundService(
        IServiceProvider serviceProvider,
        StartupStateService startupState,
        ILogger<TimelineBackfillBackgroundService> logger)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(startupState);
        ArgumentNullException.ThrowIfNull(logger);

        _serviceProvider = serviceProvider;
        _startupState = startupState;
        _logger = logger;
    }

    /// <summary>
    /// Signals the background loop to run a timeline backfill immediately (admin trigger).
    /// Returns <c>false</c> when a run is already queued; the options of the queued run stay in effect.
    /// </summary>
    public bool TriggerImmediateRun(TimelineBackfillRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (_manualTrigger.TrySetResult(request))
        {
            _logger.LogInformation("Manual timeline backfill run triggered");
            return true;
        }

        _logger.LogInformation("Manual trigger requested but a timeline backfill run is already queued");
        return false;
    }

    /// <summary>
    /// Cancels the currently running timeline backfill.
    /// </summary>
    public bool CancelCurrentRun()
    {
        lock (_runCtsLock)
        {
            if (_runCts is null || _runCts.IsCancellationRequested)
            {
                return false;
            }

            _runCts.Cancel();
        }

        _logger.LogInformation("Admin-triggered cancellation of timeline backfill run");
        return true;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _startupState.StartupTask.WaitAsync(stoppingToken);

        _logger.LogInformation("TimelineBackfillBackgroundService started — manual trigger only (no periodic schedule)");

        while (!stoppingToken.IsCancellationRequested)
        {
            var request = await _manualTrigger.Task.WaitAsync(stoppingToken);
            _manualTrigger = new TaskCompletionSource<TimelineBackfillRequest>(TaskCreationOptions.RunContinuationsAsynchronously);

            await RunOnceAsync(request, stoppingToken);
        }
    }

#pragma warning disable CA1031 // Catch-all intentional: background loop must not crash the host
    private async Task RunOnceAsync(TimelineBackfillRequest request, CancellationToken ct)
    {
        var runCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        lock (_runCtsLock)
        {
            _runCts = runCts;
        }

        var runToken = runCts.Token;
        try
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var jobRepo = scope.ServiceProvider.GetRequiredService<IContentProcessingJobRepository>();
            var jobId = await jobRepo.CreateAsync("manual", ContentProcessingJobType.TimelineBackfill, runToken);

            try
            {
                var backfill = scope.ServiceProvider.GetRequiredService<ITimelineBackfillService>();

                _logger.LogInformation("Timeline backfill started (job {JobId})", jobId);
                var result = await backfill.RunAsync(request, line => jobRepo.AppendLogAsync(jobId, line, runToken), runToken);

                _logger.LogInformation("Timeline backfill completed — {Scanned} scanned, {Events} events (job {JobId})", result.Scanned, result.EventsStored, jobId);
                await jobRepo.CompleteAsync(jobId,
                    feedsProcessed: 0, itemsAdded: result.EventsStored, itemsSkipped: result.Scanned - result.WithEvents,
                    errorCount: result.Failed, transcriptsSucceeded: 0, transcriptsFailed: 0,
                    logOutput: null, itemsFixed: result.Scanned, ct: runToken);
            }
            catch (OperationCanceledException) when (!ct.IsCancellationRequested)
            {
                _logger.LogInformation("Timeline backfill aborted by admin (job {JobId})", jobId);
                await jobRepo.AbortJobAsync(jobId,
                    feedsProcessed: 0, itemsAdded: 0, itemsSkipped: 0, errorCount: 0,
                    transcriptsSucceeded: 0, transcriptsFailed: 0,
                    logOutput: null, ct: CancellationToken.None);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Timeline backfill failed (job {JobId})", jobId);
                await jobRepo.FailAsync(jobId,
                    feedsProcessed: 0, itemsAdded: 0, itemsSkipped: 0, errorCount: 1,
                    transcriptsSucceeded: 0, transcriptsFailed: 0,
                    logOutput: string.Create(CultureInfo.InvariantCulture, $"Timeline backfill failed: {ex.Message}"), ct: CancellationToken.None);
            }
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            // Shutting down — expected
        }
        catch (OperationCanceledException)
        {
            // Admin cancellation before the job row existed (e.g. while it was being created): nothing to abort
            _logger.LogInformation("Timeline backfill cancelled by admin before the job started");
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "Unexpected exception in TimelineBackfillBackgroundService");
        }
        finally
        {
            lock (_runCtsLock)
            {
                _runCts = null;
            }

            runCts.Dispose();
        }
    }
#pragma warning restore CA1031
}
