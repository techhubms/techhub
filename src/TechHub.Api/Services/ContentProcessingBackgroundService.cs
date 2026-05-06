using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Infrastructure.Services.ContentProcessing;

namespace TechHub.Api.Services;

/// <summary>
/// Runs the content processing pipeline on a <see cref="PeriodicTimer"/> schedule.
/// The interval is configured via <see cref="ContentProcessorOptions.IntervalMinutes"/> (default: 60).
/// Also runs immediately on startup.
/// Invoked manually by the admin UI via <see cref="TriggerImmediateRun"/>.
/// </summary>
public sealed class ContentProcessingBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ContentProcessorOptions _options;
    private readonly StartupStateService _startupState;
    private readonly ILogger<ContentProcessingBackgroundService> _logger;

    // Always holds a live (incomplete) TCS that the background loop awaits.
    // TriggerImmediateRun completes it; the loop then replaces it with a fresh one.
    private volatile TaskCompletionSource<bool> _manualTrigger = new(TaskCreationOptions.RunContinuationsAsynchronously);

    // Per-run CTS for admin-triggered cancellation. Linked to the host's stoppingToken.
    // Disposed in RunOnceAsync finally block; also disposed on service shutdown.
#pragma warning disable CA2213 // Disposed in RunOnceAsync finally block and Dispose override
    private volatile CancellationTokenSource? _runCts;
#pragma warning restore CA2213

    public ContentProcessingBackgroundService(
        IServiceProvider serviceProvider,
        IOptions<ContentProcessorOptions> options,
        StartupStateService startupState,
        ILogger<ContentProcessingBackgroundService> logger)
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
    /// Signals the background loop to run a processing job immediately (admin trigger).
    /// Returns immediately; the actual run happens asynchronously in the background.
    /// </summary>
    public void TriggerImmediateRun()
    {
        if (_manualTrigger.TrySetResult(true))
        {
            _logger.LogInformation("Manual content processing run triggered");
        }
        else
        {
            _logger.LogInformation("Manual trigger requested but a run is already queued");
        }
    }

    /// <summary>
    /// Cancels the currently running processing job (admin cancel).
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
        _logger.LogInformation("Admin-triggered cancellation of content processing run");
        return true;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Wait for database migrations and content sync to complete before processing
        _logger.LogInformation("Waiting for startup operations to complete before first processing run…");
        await _startupState.StartupTask.WaitAsync(stoppingToken);
        _logger.LogInformation("Startup complete");

        var enabled = await IsEnabledAsync(stoppingToken);

        if (!enabled)
        {
            _logger.LogInformation(
                "Scheduled content processing is disabled via database setting. Manual triggers via admin UI are still accepted.");
        }
        else
        {
            _logger.LogInformation(
                "ContentProcessingBackgroundService started — interval: {Interval} minutes",
                _options.IntervalMinutes);

            // Run once immediately on startup when enabled
            await RunOnceAsync("scheduled", stoppingToken);
        }

        using var timer = new PeriodicTimer(_options.Interval);
        var timerTask = timer.WaitForNextTickAsync(stoppingToken).AsTask();

        while (!stoppingToken.IsCancellationRequested)
        {
            // Wait for either the periodic timer or a manual trigger
            var manualTask = _manualTrigger.Task;

            var completed = await Task.WhenAny(manualTask, timerTask);

            if (stoppingToken.IsCancellationRequested)
            {
                break;
            }

            if (completed == timerTask)
            {
                // Timer ticked — start a new wait immediately so it's ready next iteration
                timerTask = timer.WaitForNextTickAsync(stoppingToken).AsTask();

                // Scheduled runs check the database setting each time
                _manualTrigger = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
                if (await IsEnabledAsync(stoppingToken))
                {
                    await RunOnceAsync("scheduled", stoppingToken);
                }
            }
            else
            {
                // Manual triggers always execute regardless of enabled state
                _manualTrigger = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
                await RunOnceAsync("manual", stoppingToken);
            }
        }
    }

    private async Task<bool> IsEnabledAsync(CancellationToken ct)
    {
        // If disabled via environment variable (ContentProcessor__Enabled=false), skip the
        // database round-trip. This is the mechanism used in PR preview environments to prevent
        // scheduled runs on a PITR-restored database where the setting is enabled=true.
        // Manual admin-triggered runs bypass this method entirely and always execute.
        if (!_options.Enabled)
        {
            return false;
        }

        await using var scope = _serviceProvider.CreateAsyncScope();
        var repo = scope.ServiceProvider.GetRequiredService<IBackgroundJobSettingRepository>();
        return await repo.IsEnabledAsync(ContentProcessorOptions.SectionName, ct);
    }

#pragma warning disable CA1031 // Catch-all intentional: unexpected exceptions in the background loop must not crash the host
    private async Task RunOnceAsync(string triggerType, CancellationToken ct)
    {
        _runCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        try
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var service = scope.ServiceProvider.GetRequiredService<ContentProcessingService>();
            await service.RunAsync(triggerType, _runCts.Token);

            // Invalidate content cache so new/updated items are served immediately
            var contentRepo = scope.ServiceProvider.GetRequiredService<IContentRepository>();
            contentRepo.InvalidateCachedData();
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            // Shutting down — expected
        }
        catch (Exception ex) when (ex is not OutOfMemoryException and not StackOverflowException)
        {
            // Errors are already recorded by ContentProcessingService; log defensively here
            _logger.LogError(ex, "Unexpected exception in ContentProcessingBackgroundService");
        }
        finally
        {
            _runCts.Dispose();
            _runCts = null;
        }
    }
#pragma warning restore CA1031
}
