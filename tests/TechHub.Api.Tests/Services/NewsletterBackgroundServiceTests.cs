using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Api.Services;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;

namespace TechHub.Api.Tests.Services;

/// <summary>
/// Unit tests for the manual-trigger paths of <see cref="NewsletterBackgroundService"/>.
/// Not covered here (would need production changes): the scheduled path (a <see cref="PeriodicTimer"/> with a
/// one-minute minimum interval and no time abstraction) and the roundup path (queries the database through Dapper
/// on an <see cref="System.Data.IDbConnection"/>; covered by the newsletter endpoint integration tests).
/// All synchronisation uses <see cref="TaskCompletionSource"/> signals raised from mock callbacks.
/// </summary>
public class NewsletterBackgroundServiceTests
{
    [Fact]
    public async Task TriggerImmediateRun_Daily_BypassesScheduledSendSettingAndHourGate()
    {
        // Arrange — scheduled sending is off and the configured digest hour is never the current hour,
        // so only the manual path can possibly send
        var options = CreateOptions(scheduledSendEnabled: false, dailyDigestHourLocal: (DateTime.UtcNow.Hour + 12) % 24);
        var reportSent = NewSignal();
        DateOnly? overviewDay = null;
        DateOnly? reportDay = null;
        var yesterdayBefore = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));

        var newsletterService = new Mock<INewsletterService>();
        newsletterService.Setup(s => s.SendDailyOverviewAsync(It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .Callback<DateOnly, CancellationToken>((day, _) => overviewDay = day)
            .ReturnsAsync(true);
        // The admin report is sent after the overview, so it is the signal that the run is done
        newsletterService.Setup(s => s.SendAdminStatusReportAsync(It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .Callback<DateOnly, CancellationToken>((day, _) =>
            {
                reportDay = day;
                reportSent.TrySetResult();
            })
            .ReturnsAsync(true);

        var sut = CreateSut(CompletedStartupState(), newsletterService.Object, options: options);

        // Act
        await sut.StartAsync(TestContext.Current.CancellationToken);
        sut.TriggerImmediateRun("daily");
        await WaitAsync(reportSent);
        await sut.StopAsync(CancellationToken.None);

        // Assert — both sends target yesterday (in the configured time zone, UTC here)
        var yesterdayAfter = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
        overviewDay.Should().NotBeNull();
        new[] { yesterdayBefore, yesterdayAfter }.Should().Contain(overviewDay!.Value);
        reportDay.Should().Be(overviewDay);
    }

    [Fact]
    public async Task TriggerTestSend_Weekly_SendsTestWeeklyToRecipient()
    {
        // Arrange
        var weeklySent = NewSignal();
        var newsletterService = new Mock<INewsletterService>();
        newsletterService.Setup(s => s.SendTestWeeklyAsync(It.IsAny<IReadOnlyList<string>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                weeklySent.TrySetResult();
                return Task.FromResult(true);
            });

        var sut = CreateSut(CompletedStartupState(), newsletterService.Object);

        // Act
        await sut.StartAsync(TestContext.Current.CancellationToken);
        sut.TriggerTestSend("recipient@example.com", ["ai", "azure"], "weekly");
        await WaitAsync(weeklySent);
        await sut.StopAsync(CancellationToken.None);

        // Assert
        newsletterService.Verify(
            s => s.SendTestWeeklyAsync(
                It.Is<IReadOnlyList<string>>(sections => sections.SequenceEqual(new[] { "ai", "azure" })),
                "recipient@example.com",
                It.IsAny<CancellationToken>()),
            Times.Once);
        newsletterService.Verify(
            s => s.SendTestDailyEmailAsync(It.IsAny<IReadOnlyList<string>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task TriggerTestSend_Daily_SendsTestDailyEmailToRecipient()
    {
        // Arrange
        var dailySent = NewSignal();
        var newsletterService = new Mock<INewsletterService>();
        newsletterService.Setup(s => s.SendTestDailyEmailAsync(It.IsAny<IReadOnlyList<string>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                dailySent.TrySetResult();
                return Task.FromResult(true);
            });

        var sut = CreateSut(CompletedStartupState(), newsletterService.Object);

        // Act
        await sut.StartAsync(TestContext.Current.CancellationToken);
        sut.TriggerTestSend("recipient@example.com", ["dotnet"], "daily");
        await WaitAsync(dailySent);
        await sut.StopAsync(CancellationToken.None);

        // Assert
        newsletterService.Verify(
            s => s.SendTestDailyEmailAsync(
                It.Is<IReadOnlyList<string>>(sections => sections.SequenceEqual(new[] { "dotnet" })),
                "recipient@example.com",
                It.IsAny<CancellationToken>()),
            Times.Once);
        newsletterService.Verify(
            s => s.SendTestWeeklyAsync(It.IsAny<IReadOnlyList<string>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task TriggerTestSend_WithoutSections_LogsWarningAndSendsNothing()
    {
        // Arrange — the service logs a warning when it skips the test send, which is the observable "handled" signal
        var skipped = NewSignal();
        var logger = CreateLogger(LogLevel.Warning, skipped);
        var newsletterService = new Mock<INewsletterService>();

        var sut = CreateSut(CompletedStartupState(), newsletterService.Object, logger.Object);

        // Act
        await sut.StartAsync(TestContext.Current.CancellationToken);
        sut.TriggerTestSend("recipient@example.com", [], "weekly");
        await WaitAsync(skipped);
        await sut.StopAsync(CancellationToken.None);

        // Assert
        newsletterService.Verify(
            s => s.SendTestWeeklyAsync(It.IsAny<IReadOnlyList<string>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
        newsletterService.Verify(
            s => s.SendTestDailyEmailAsync(It.IsAny<IReadOnlyList<string>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task TriggerImmediateRun_AfterTriggerTestSend_RunsManualSendInsteadOfTestSend()
    {
        // Arrange — both triggers are issued before the service starts, so the loop sees only the final state:
        // TriggerImmediateRun must discard the pending test send
        var reportSent = NewSignal();
        var newsletterService = new Mock<INewsletterService>();
        newsletterService.Setup(s => s.SendDailyOverviewAsync(It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        newsletterService.Setup(s => s.SendAdminStatusReportAsync(It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                reportSent.TrySetResult();
                return Task.FromResult(true);
            });

        var sut = CreateSut(CompletedStartupState(), newsletterService.Object);
        sut.TriggerTestSend("recipient@example.com", ["ai"], "weekly");
        sut.TriggerImmediateRun("daily");

        // Act
        await sut.StartAsync(TestContext.Current.CancellationToken);
        await WaitAsync(reportSent);
        await sut.StopAsync(CancellationToken.None);

        // Assert
        newsletterService.Verify(s => s.SendDailyOverviewAsync(It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()), Times.Once);
        newsletterService.Verify(
            s => s.SendTestWeeklyAsync(It.IsAny<IReadOnlyList<string>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task TriggerImmediateRun_WhenSendThrows_LogsErrorAndKeepsProcessingTriggers()
    {
        // Arrange
        var errorLogged = NewSignal();
        var secondRunDone = NewSignal();
        var logger = CreateLogger(LogLevel.Error, errorLogged);

        var newsletterService = new Mock<INewsletterService>();
        newsletterService.SetupSequence(s => s.SendDailyOverviewAsync(It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("send failed"))
            .ReturnsAsync(true);
        newsletterService.Setup(s => s.SendAdminStatusReportAsync(It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                secondRunDone.TrySetResult();
                return Task.FromResult(true);
            });

        var sut = CreateSut(CompletedStartupState(), newsletterService.Object, logger.Object);

        // Act
        await sut.StartAsync(TestContext.Current.CancellationToken);
        sut.TriggerImmediateRun("daily");
        await WaitAsync(errorLogged);
        sut.TriggerImmediateRun("daily");
        await WaitAsync(secondRunDone);
        await sut.StopAsync(CancellationToken.None);

        // Assert — the failed first run skipped the admin report; the second run completed normally
        newsletterService.Verify(s => s.SendDailyOverviewAsync(It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        newsletterService.Verify(s => s.SendAdminStatusReportAsync(It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task TriggerImmediateRun_BeforeStartupCompletes_WaitsForStartup()
    {
        // Arrange — startup has not completed yet
        var startupState = new StartupStateService();
        var overviewStarted = NewSignal();
        var startupCompletedWhenSendStarted = false;

        var newsletterService = new Mock<INewsletterService>();
        newsletterService.Setup(s => s.SendDailyOverviewAsync(It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                startupCompletedWhenSendStarted = startupState.IsStartupCompleted;
                overviewStarted.TrySetResult();
                return Task.FromResult(true);
            });

        var sut = CreateSut(startupState, newsletterService.Object);

        // Act
        await sut.StartAsync(TestContext.Current.CancellationToken);
        sut.TriggerImmediateRun("daily");
        startupState.MarkStartupCompleted();
        await WaitAsync(overviewStarted);
        await sut.StopAsync(CancellationToken.None);

        // Assert
        startupCompletedWhenSendStarted.Should().BeTrue("newsletters must not be sent before startup (migrations) has completed");
    }

    private static StartupStateService CompletedStartupState()
    {
        var startupState = new StartupStateService();
        startupState.MarkStartupCompleted();
        return startupState;
    }

    private static TaskCompletionSource NewSignal() => new(TaskCreationOptions.RunContinuationsAsynchronously);

    private static Task WaitAsync(TaskCompletionSource signal) =>
        signal.Task.WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken);

    private static NewsletterOptions CreateOptions(bool scheduledSendEnabled = false, int dailyDigestHourLocal = 9) => new()
    {
        ScheduledSendEnabled = scheduledSendEnabled,
        CheckIntervalMinutes = 30,
        DailyDigestHourLocal = dailyDigestHourLocal,
        DailyDigestTimeZoneId = "UTC"
    };

    private static Mock<ILogger<NewsletterBackgroundService>> CreateLogger(LogLevel level, TaskCompletionSource signal)
    {
        var logger = new Mock<ILogger<NewsletterBackgroundService>>();
        logger.Setup(l => l.Log(
                level,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()))
            .Callback(new InvocationAction(_ => signal.TrySetResult()));
        return logger;
    }

    private static NewsletterBackgroundService CreateSut(
        StartupStateService startupState,
        INewsletterService newsletterService,
        ILogger<NewsletterBackgroundService>? logger = null,
        NewsletterOptions? options = null)
    {
        var services = new ServiceCollection();
        services.AddScoped(_ => newsletterService);

        return new NewsletterBackgroundService(
            services.BuildServiceProvider(),
            startupState,
            Options.Create(options ?? CreateOptions()),
            logger ?? Mock.Of<ILogger<NewsletterBackgroundService>>());
    }
}
