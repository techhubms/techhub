using System.Data;
using Dapper;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;

namespace TechHub.Api.Services;

public sealed class NewsletterBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly StartupStateService _startupState;
    private readonly NewsletterOptions _options;
    private readonly ILogger<NewsletterBackgroundService> _logger;

    private volatile TaskCompletionSource<bool> _manualTrigger = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private string? _pendingTestSendEmail;
    private IReadOnlyList<string>? _pendingTestSections;
    private string? _pendingTestSendKind;

    public NewsletterBackgroundService(
        IServiceProvider serviceProvider,
        StartupStateService startupState,
        IOptions<NewsletterOptions> options,
        ILogger<NewsletterBackgroundService> logger)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(startupState);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _serviceProvider = serviceProvider;
        _startupState = startupState;
        _options = options.Value;
        _logger = logger;
    }

    public void TriggerImmediateRun()
    {
        _pendingTestSendEmail = null;
        _pendingTestSections = null;
        _pendingTestSendKind = null;
        _manualTrigger.TrySetResult(true);
    }

    public void TriggerTestSend(string recipientEmail, IReadOnlyList<string> sections, string kind)
    {
        _pendingTestSendEmail = recipientEmail;
        _pendingTestSections = sections;
        _pendingTestSendKind = kind;
        _manualTrigger.TrySetResult(true);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _startupState.StartupTask.WaitAsync(stoppingToken);

        var checkInterval = TimeSpan.FromMinutes(Math.Max(1, _options.CheckIntervalMinutes));
        _logger.LogInformation("NewsletterBackgroundService started with interval {CheckIntervalMinutes} minute(s)", checkInterval.TotalMinutes);

        using var timer = new PeriodicTimer(checkInterval);
        var timerTask = timer.WaitForNextTickAsync(stoppingToken).AsTask();

        while (!stoppingToken.IsCancellationRequested)
        {
            var manualTask = _manualTrigger.Task;
            var completed = await Task.WhenAny(manualTask, timerTask);
            if (stoppingToken.IsCancellationRequested)
            {
                break;
            }

            if (completed == manualTask)
            {
                _manualTrigger = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
                await RunManualAsync(stoppingToken);
                continue;
            }

            timerTask = timer.WaitForNextTickAsync(stoppingToken).AsTask();

            if (!await IsEnabledAsync(stoppingToken))
            {
                continue;
            }

            await SendLatestRoundupsAsync(stoppingToken);
            await SendScheduledDailyEmailsAsync(stoppingToken);
        }
    }

    private async Task RunManualAsync(CancellationToken ct)
    {
        var testEmail = _pendingTestSendEmail;
        var testSections = _pendingTestSections;
        var testKind = _pendingTestSendKind;
        _pendingTestSendEmail = null;
        _pendingTestSections = null;
        _pendingTestSendKind = null;

        if (!string.IsNullOrWhiteSpace(testEmail))
        {
            await RunTestSendAsync(testEmail, testSections ?? [], testKind ?? "weekly", ct);
            return;
        }

        await SendLatestRoundupsAsync(ct);
    }

    private async Task SendLatestRoundupsAsync(CancellationToken ct)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        var newsletterService = scope.ServiceProvider.GetRequiredService<INewsletterService>();

        const string Sql = """
            SELECT DISTINCT ON (primary_section_name)
                slug
            FROM content_items
            WHERE collection_name = 'roundups'
              AND primary_section_name IS NOT NULL
              AND primary_section_name <> 'all'
            ORDER BY primary_section_name, date_epoch DESC
            """;

        var slugs = (await connection.QueryAsync<string>(new CommandDefinition(Sql, cancellationToken: ct))).AsList();
        if (slugs.Count > 0)
        {
            await newsletterService.SendCombinedWeeklyAsync(slugs, ct);
        }
    }

    private async Task SendScheduledDailyEmailsAsync(CancellationToken ct)
    {
        var timeZone = ResolveTimeZone(_options.DailyDigestTimeZoneId);
        var localNow = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, timeZone);
        if (localNow.Hour != _options.DailyDigestHourLocal)
        {
            return;
        }

        var day = DateOnly.FromDateTime(localNow.DateTime.Date.AddDays(-1));
        await using var scope = _serviceProvider.CreateAsyncScope();
        var newsletterService = scope.ServiceProvider.GetRequiredService<INewsletterService>();
        await newsletterService.SendDailyOverviewAsync(day, ct);
        await newsletterService.SendAdminStatusReportAsync(day, ct);
    }

    private async Task<bool> IsEnabledAsync(CancellationToken ct)
    {
        // If disabled via environment variable (Newsletter__ScheduledSendEnabled=false), skip the
        // database round-trip. This is the mechanism used in PR preview environments to prevent
        // scheduled runs on a PITR-restored database where the setting is enabled=true.
        // Manual admin-triggered runs bypass this method entirely and always execute.
        if (!_options.ScheduledSendEnabled)
        {
            return false;
        }

        await using var scope = _serviceProvider.CreateAsyncScope();
        var repo = scope.ServiceProvider.GetRequiredService<IBackgroundJobSettingRepository>();
        return await repo.IsEnabledAsync(NewsletterOptions.SectionName, ct);
    }

    private async Task RunTestSendAsync(string recipientEmail, IReadOnlyList<string> sections, string kind, CancellationToken ct)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var newsletterService = scope.ServiceProvider.GetRequiredService<INewsletterService>();

        if (sections.Count == 0)
        {
            _logger.LogWarning("Newsletter test send requested but no sections were selected");
            return;
        }

        bool sent;
        if (string.Equals(kind, "daily", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogInformation("Running newsletter test daily send for sections: {Sections}", string.Join(", ", sections));
            sent = await newsletterService.SendTestDailyEmailAsync(sections, recipientEmail, ct);
        }
        else
        {
            _logger.LogInformation("Running newsletter test weekly send for sections: {Sections}", string.Join(", ", sections));
            sent = await newsletterService.SendTestWeeklyAsync(sections, recipientEmail, ct);
        }

        if (sent)
        {
            _logger.LogInformation("Newsletter test {Kind} send succeeded", kind);
        }
        else
        {
            _logger.LogWarning("Newsletter test {Kind} send failed — check ACS configuration and that roundups exist for the selected sections", kind);
        }
    }

    private TimeZoneInfo ResolveTimeZone(string configuredId)
    {
        if (!string.IsNullOrWhiteSpace(configuredId))
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(configuredId);
            }
            catch (TimeZoneNotFoundException ex)
            {
                _logger.LogWarning(ex, "Invalid DailyDigestTimeZoneId '{TimeZoneId}'. Falling back to UTC.", configuredId);
            }
            catch (InvalidTimeZoneException ex)
            {
                _logger.LogWarning(ex, "Invalid DailyDigestTimeZoneId '{TimeZoneId}'. Falling back to UTC.", configuredId);
            }
        }

        return TimeZoneInfo.Utc;
    }
}
