using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;

namespace TechHub.Api.Services;

/// <summary>
/// Runs the weekly roundup generation pipeline every Monday at <see cref="RoundupGeneratorOptions.RunHourUtc"/> UTC.
/// When triggered, it generates a roundup for the previous Monday-to-Sunday week and writes it directly
/// to the <c>content_items</c> database table.
/// </summary>
public sealed class RoundupGeneratorBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly RoundupGeneratorOptions _options;
    private readonly StartupStateService _startupState;
    private readonly ILogger<RoundupGeneratorBackgroundService> _logger;

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

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Waiting for startup operations to complete before first roundup check…");
        await _startupState.StartupTask.WaitAsync(stoppingToken);

        if (!_options.Enabled)
        {
            _logger.LogInformation("RoundupGeneratorBackgroundService is disabled via configuration");
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

            await Task.Delay(delay, stoppingToken);

            if (stoppingToken.IsCancellationRequested)
            {
                break;
            }

            await RunOnceAsync(stoppingToken);
        }
    }

#pragma warning disable CA1031
    private async Task RunOnceAsync(CancellationToken ct)
    {
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
                "Running roundup generation for week {WeekStart}–{WeekEnd}",
                weekStart, weekEnd);

            await using var scope = _serviceProvider.CreateAsyncScope();
            var service = scope.ServiceProvider.GetRequiredService<IRoundupGeneratorService>();
            var generated = await service.GenerateAsync(weekStart, weekEnd, ct);

            if (generated)
            {
                _logger.LogInformation("Roundup generated successfully for week {WeekStart}–{WeekEnd}", weekStart, weekEnd);
            }
            else
            {
                _logger.LogInformation("Roundup already existed for week {WeekStart}–{WeekEnd}, skipped", weekStart, weekEnd);
            }
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            // Shutting down — expected
        }
        catch (Exception ex) when (ex is not OutOfMemoryException and not StackOverflowException)
        {
            _logger.LogError(ex, "Unexpected exception in RoundupGeneratorBackgroundService");
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
