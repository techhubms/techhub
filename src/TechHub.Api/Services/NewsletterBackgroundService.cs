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
    private string? _pendingTestRoundupSlug;

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
        _pendingTestRoundupSlug = null;
        _manualTrigger.TrySetResult(true);
    }

    public void TriggerTestSend(string recipientEmail, string? roundupSlug)
    {
        _pendingTestSendEmail = recipientEmail;
        _pendingTestRoundupSlug = roundupSlug;
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

            if (!_options.ScheduledSendEnabled)
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
        var testSlug = _pendingTestRoundupSlug;
        _pendingTestSendEmail = null;
        _pendingTestRoundupSlug = null;

        if (!string.IsNullOrWhiteSpace(testEmail))
        {
            await RunTestSendAsync(testEmail, testSlug, ct);
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

        var slugs = await connection.QueryAsync<string>(new CommandDefinition(Sql, cancellationToken: ct));
        foreach (var slug in slugs)
        {
            await newsletterService.SendRoundupNewsletterAsync(slug, ct);
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

    private async Task RunTestSendAsync(string recipientEmail, string? roundupSlug, CancellationToken ct)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        var newsletterService = scope.ServiceProvider.GetRequiredService<INewsletterService>();

        var slug = roundupSlug;
        if (string.IsNullOrWhiteSpace(slug))
        {
            const string Sql = """
                SELECT slug
                FROM content_items
                WHERE collection_name = 'roundups'
                ORDER BY date_epoch DESC
                LIMIT 1
                """;
            slug = await connection.ExecuteScalarAsync<string?>(new CommandDefinition(Sql, cancellationToken: ct));
        }

        if (string.IsNullOrWhiteSpace(slug))
        {
            _logger.LogWarning("Newsletter test send requested but no roundup exists");
            return;
        }

        await newsletterService.SendTestEmailAsync(slug, recipientEmail, ct);
    }

    private static TimeZoneInfo ResolveTimeZone(string configuredId)
    {
        if (!string.IsNullOrWhiteSpace(configuredId))
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(configuredId);
            }
            catch (TimeZoneNotFoundException)
            {
            }
            catch (InvalidTimeZoneException)
            {
            }
        }

        return TimeZoneInfo.Utc;
    }
}
