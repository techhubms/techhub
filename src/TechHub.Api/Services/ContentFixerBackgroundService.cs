using TechHub.Core.Interfaces;
using TechHub.Core.Models.Admin;

namespace TechHub.Api.Services;

/// <summary>
/// Manual-trigger-only background service for bulk content fixes (tags, authors, markdown).
/// No periodic schedule — runs only when triggered via admin endpoint.
/// </summary>
public sealed class ContentFixerBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly StartupStateService _startupState;
    private readonly ILogger<ContentFixerBackgroundService> _logger;

    private volatile TaskCompletionSource<bool> _manualTrigger = new(TaskCreationOptions.RunContinuationsAsynchronously);

#pragma warning disable CA2213 // Disposed in RunOnceAsync finally block
    private volatile CancellationTokenSource? _runCts;
#pragma warning restore CA2213

    public ContentFixerBackgroundService(
        IServiceProvider serviceProvider,
        StartupStateService startupState,
        ILogger<ContentFixerBackgroundService> logger)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(startupState);
        ArgumentNullException.ThrowIfNull(logger);

        _serviceProvider = serviceProvider;
        _startupState = startupState;
        _logger = logger;
    }

    /// <summary>
    /// Signals the background loop to run a content fix job immediately (admin trigger).
    /// </summary>
    public void TriggerImmediateRun()
    {
        if (_manualTrigger.TrySetResult(true))
        {
            _logger.LogInformation("Manual content fixer run triggered");
        }
        else
        {
            _logger.LogInformation("Manual trigger requested but a run is already queued");
        }
    }

    /// <summary>
    /// Cancels the currently running content fix job.
    /// </summary>
    public bool CancelCurrentRun()
    {
        var cts = _runCts;
        if (cts is null || cts.IsCancellationRequested)
        {
            return false;
        }

        cts.Cancel();
        _logger.LogInformation("Admin-triggered cancellation of content fixer run");
        return true;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Wait for startup to complete
        await _startupState.StartupTask.WaitAsync(stoppingToken);

        _logger.LogInformation("ContentFixerBackgroundService started — manual trigger only (no periodic schedule)");

        while (!stoppingToken.IsCancellationRequested)
        {
            // Wait only for manual trigger — no timer
            await _manualTrigger.Task.WaitAsync(stoppingToken);
            _manualTrigger = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            await RunOnceAsync(stoppingToken);
        }
    }

#pragma warning disable CA1031 // Catch-all intentional: background loop must not crash the host
    private async Task RunOnceAsync(CancellationToken ct)
    {
        _runCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        var runToken = _runCts.Token;
        try
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var jobRepo = scope.ServiceProvider.GetRequiredService<IContentProcessingJobRepository>();
            var jobId = await jobRepo.CreateAsync("manual", ContentProcessingJobType.ContentCleanup, runToken);

            try
            {
                var contentFixer = scope.ServiceProvider.GetRequiredService<IContentFixerService>();

                _logger.LogInformation("Content fixer run started (job {JobId})", jobId);
                await jobRepo.AppendLogAsync(jobId, "Starting content cleanup...", runToken);

                var result = await contentFixer.FixAllAsync(jobId: jobId, ct: runToken);

                _logger.LogInformation("Content fixer run completed — {TotalFixed} items fixed (job {JobId})", result.TotalFixed, jobId);
                await jobRepo.CompleteAsync(jobId,
                    feedsProcessed: 0, itemsAdded: 0, itemsSkipped: 0, errorCount: 0,
                    transcriptsSucceeded: 0, transcriptsFailed: 0,
                    logOutput: result.ToLogOutput(),
                    itemsFixed: result.TotalFixed, ct: runToken);

                // Only invalidate cache when changes were applied directly (not in review mode)
                if (result.TotalFixed > 0)
                {
                    var contentRepo = scope.ServiceProvider.GetRequiredService<IContentRepository>();
                    contentRepo.InvalidateCachedData();
                }
            }
            catch (OperationCanceledException) when (!ct.IsCancellationRequested)
            {
                // Admin-triggered cancellation — record as aborted
                _logger.LogInformation("Content fixer run aborted by admin (job {JobId})", jobId);
                await jobRepo.AbortJobAsync(jobId,
                    feedsProcessed: 0, itemsAdded: 0, itemsSkipped: 0, errorCount: 0,
                    transcriptsSucceeded: 0, transcriptsFailed: 0,
                    logOutput: "Content cleanup aborted by admin.", ct: CancellationToken.None);
            }
            catch (Exception ex) when (ex is not OperationCanceledException and not OutOfMemoryException and not StackOverflowException)
            {
                _logger.LogError(ex, "Content fixer run failed (job {JobId})", jobId);
                await jobRepo.FailAsync(jobId,
                    feedsProcessed: 0, itemsAdded: 0, itemsSkipped: 0, errorCount: 1,
                    transcriptsSucceeded: 0, transcriptsFailed: 0,
                    logOutput: $"Content cleanup failed: {ex.Message}", ct: CancellationToken.None);
            }
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            // Shutting down — expected
        }
        catch (Exception ex) when (ex is not OutOfMemoryException and not StackOverflowException)
        {
            _logger.LogError(ex, "Unexpected exception in ContentFixerBackgroundService");
        }
        finally
        {
            _runCts.Dispose();
            _runCts = null;
        }
    }
#pragma warning restore CA1031
}
