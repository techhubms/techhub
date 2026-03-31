using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.Admin;

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

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Waiting for startup operations to complete before first roundup check…");
        await _startupState.StartupTask.WaitAsync(stoppingToken);

        if (!_options.Enabled)
        {
            _logger.LogInformation("RoundupGeneratorBackgroundService is disabled via configuration. Manual triggers via admin UI are still accepted.");

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
            "RoundupGeneratorBackgroundService started — scheduled every Monday at {RunHourUtc:00}:00 UTC",
            _options.RunHourUtc);

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

            var triggerType = manualTask.IsCompleted ? "manual" : "scheduled";

            // Reset the manual trigger for future manual runs
            if (manualTask.IsCompleted)
            {
                _manualTrigger = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            }

            await RunOnceAsync(triggerType, stoppingToken);
        }
    }

#pragma warning disable CA1031
    private async Task RunOnceAsync(string triggerType, CancellationToken ct)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var jobRepo = scope.ServiceProvider.GetRequiredService<IContentProcessingJobRepository>();
        var jobId = await jobRepo.CreateAsync(triggerType, ContentProcessingJobType.RoundupGeneration, ct);

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

            await jobRepo.AppendLogAsync(jobId, $"Generating roundup for week {weekStart}–{weekEnd}", ct);

            var service = scope.ServiceProvider.GetRequiredService<IRoundupGeneratorService>();
            var generated = await service.GenerateAsync(weekStart, weekEnd, ct);

            if (generated)
            {
                _logger.LogInformation("Roundup generated successfully for week {WeekStart}–{WeekEnd}", weekStart, weekEnd);
                await jobRepo.CompleteAsync(jobId, feedsProcessed: 0, itemsAdded: 1, itemsSkipped: 0, errorCount: 0,
                    $"Generating roundup for week {weekStart}–{weekEnd}\nRoundup generated successfully.", ct);
            }
            else
            {
                _logger.LogInformation("Roundup already existed for week {WeekStart}–{WeekEnd}, skipped", weekStart, weekEnd);
                await jobRepo.CompleteAsync(jobId, feedsProcessed: 0, itemsAdded: 0, itemsSkipped: 1, errorCount: 0,
                    $"Generating roundup for week {weekStart}–{weekEnd}\nRoundup already existed, skipped.", ct);
            }
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            // Shutting down — expected
        }
        catch (Exception ex) when (ex is not OutOfMemoryException and not StackOverflowException)
        {
            _logger.LogError(ex, "Unexpected exception in RoundupGeneratorBackgroundService (job {JobId})", jobId);
            await jobRepo.FailAsync(jobId, feedsProcessed: 0, itemsAdded: 0, itemsSkipped: 0, errorCount: 1,
                $"Roundup generation failed: {ex.Message}", ct);
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
