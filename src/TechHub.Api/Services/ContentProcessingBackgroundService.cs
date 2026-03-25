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

    // Used to signal an immediate manual run; reset to null after use
    private volatile TaskCompletionSource<bool>? _manualTrigger;

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
        var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        var existing = Interlocked.CompareExchange(ref _manualTrigger, tcs, null);
        if (existing != null)
        {
            _logger.LogInformation("Manual trigger requested but a run is already queued");
            return;
        }

        tcs.SetResult(true);
        _logger.LogInformation("Manual content processing run triggered");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_options.Enabled)
        {
            _logger.LogInformation("ContentProcessingBackgroundService is disabled via configuration");
            return;
        }

        _logger.LogInformation(
            "ContentProcessingBackgroundService started — interval: {Interval} minutes",
            _options.IntervalMinutes);

        // Wait for database migrations and content sync to complete before processing
        _logger.LogInformation("Waiting for startup operations to complete before first processing run…");
        await _startupState.StartupTask.WaitAsync(stoppingToken);
        _logger.LogInformation("Startup complete — starting first processing run");

        // Run once immediately on startup
        await RunOnceAsync("scheduled", stoppingToken);

        using var timer = new PeriodicTimer(_options.Interval);

        while (!stoppingToken.IsCancellationRequested)
        {
            // Wait for either the periodic timer or a manual trigger
            var manualTask = _manualTrigger?.Task ?? Task.Delay(Timeout.Infinite, stoppingToken);
            var timerTask = timer.WaitForNextTickAsync(stoppingToken).AsTask();

            var completed = await Task.WhenAny(manualTask, timerTask);

            if (stoppingToken.IsCancellationRequested)
            {
                break;
            }

            var trigger = completed == manualTask ? "manual" : "scheduled";
            Interlocked.Exchange(ref _manualTrigger, null);

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
