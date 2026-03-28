using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Infrastructure.Services;

namespace TechHub.Api.Services;

/// <summary>
/// Runs the content processing pipeline on a <see cref="PeriodicTimer"/> schedule.
/// The interval is configured via <see cref="ContentProcessorOptions.IntervalMinutes"/> (default: 15).
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

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Wait for database migrations and content sync to complete before processing
        _logger.LogInformation("Waiting for startup operations to complete before first processing run…");
        await _startupState.StartupTask.WaitAsync(stoppingToken);
        _logger.LogInformation("Startup complete");

        if (!_options.Enabled)
        {
            _logger.LogInformation(
                "Scheduled content processing is disabled via configuration. Manual triggers via admin UI are still accepted.");

            // Keep listening for manual triggers even when scheduled processing is off
            while (!stoppingToken.IsCancellationRequested)
            {
                await _manualTrigger.Task.WaitAsync(stoppingToken);

                if (stoppingToken.IsCancellationRequested)
                {
                    break;
                }

                _manualTrigger = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
                await RunOnceAsync("manual", stoppingToken);
            }

            return;
        }

        _logger.LogInformation(
            "ContentProcessingBackgroundService started — interval: {Interval} minutes",
            _options.IntervalMinutes);

        // Run once immediately on startup
        await RunOnceAsync("scheduled", stoppingToken);

        using var timer = new PeriodicTimer(_options.Interval);

        while (!stoppingToken.IsCancellationRequested)
        {
            // Wait for either the periodic timer or a manual trigger
            var manualTask = _manualTrigger.Task;
            var timerTask = timer.WaitForNextTickAsync(stoppingToken).AsTask();

            var completed = await Task.WhenAny(manualTask, timerTask);

            if (stoppingToken.IsCancellationRequested)
            {
                break;
            }

            var trigger = completed == manualTask ? "manual" : "scheduled";
            _manualTrigger = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            await RunOnceAsync(trigger, stoppingToken);
        }
    }

#pragma warning disable CA1031 // Catch-all intentional: unexpected exceptions in the background loop must not crash the host
    private async Task RunOnceAsync(string triggerType, CancellationToken ct)
    {
        try
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var service = scope.ServiceProvider.GetRequiredService<ContentProcessingService>();
            await service.RunAsync(triggerType, ct);
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
    }
#pragma warning restore CA1031
}
