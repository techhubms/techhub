using FluentAssertions;
using TechHub.Core.Models;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

public class NewsletterSubscriberRepositoryTests : IClassFixture<DatabaseFixture<NewsletterSubscriberRepositoryTests>>
{
    private readonly NewsletterSubscriberRepository _sut;

    public NewsletterSubscriberRepositoryTests(DatabaseFixture<NewsletterSubscriberRepositoryTests> fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        _sut = new NewsletterSubscriberRepository(fixture.Connection);
    }

    [Fact]
    public async Task UpsertSubscriberAsync_SavesPreferences_AndCanBeQueried()
    {
        await _sut.UpsertSubscriberAsync(
            "subscriber@example.com",
            "Subscriber",
            ["ai", "azure"],
            ["ai"],
            TestContext.Current.CancellationToken);

        // New subscribers require email confirmation before appearing in active queries
        var result = await _sut.ConfirmSubscriberAsync("subscriber@example.com", TestContext.Current.CancellationToken);
        result.Should().Be(ConfirmSubscriptionResult.Confirmed);

        var weekly = await _sut.GetActiveSubscribersAsync("ai", weekly: true, TestContext.Current.CancellationToken);
        var daily = await _sut.GetActiveSubscribersAsync("ai", weekly: false, TestContext.Current.CancellationToken);

        weekly.Should().ContainSingle(s => s.Email == "subscriber@example.com");
        daily.Should().ContainSingle(s => s.Email == "subscriber@example.com");
    }

    [Fact]
    public async Task ConfirmSubscriberAsync_WhenAlreadyConfirmed_ReturnsAlreadyConfirmed()
    {
        await _sut.UpsertSubscriberAsync(
            "already-confirmed@example.com",
            null,
            ["ai"],
            [],
            TestContext.Current.CancellationToken);

        await _sut.ConfirmSubscriberAsync("already-confirmed@example.com", TestContext.Current.CancellationToken);

        var result = await _sut.ConfirmSubscriberAsync("already-confirmed@example.com", TestContext.Current.CancellationToken);

        result.Should().Be(ConfirmSubscriptionResult.AlreadyConfirmed);
    }

    [Fact]
    public async Task ConfirmSubscriberAsync_WhenEmailNotFound_ReturnsInvalidToken()
    {
        var result = await _sut.ConfirmSubscriberAsync("not-subscribed@example.com", TestContext.Current.CancellationToken);

        result.Should().Be(ConfirmSubscriptionResult.InvalidToken);
    }

    [Fact]
    public async Task UpsertSubscriberAsync_WhenSubscriberAlreadyConfirmed_DoesNotOverwritePreferences()
    {
        await _sut.UpsertSubscriberAsync(
            "confirmed-no-overwrite@example.com",
            "Original Name",
            ["ai"],
            [],
            TestContext.Current.CancellationToken);

        await _sut.ConfirmSubscriberAsync("confirmed-no-overwrite@example.com", TestContext.Current.CancellationToken);

        // Re-upsert with different preferences (e.g. from attacker or accidental re-signup)
        var (_, needsConfirmation) = await _sut.UpsertSubscriberAsync(
            "confirmed-no-overwrite@example.com",
            "Attacker Name",
            ["dotnet"],
            ["azure"],
            TestContext.Current.CancellationToken);

        needsConfirmation.Should().BeFalse("confirmed subscriber does not need re-confirmation");

        var subscriber = await _sut.GetSubscriberByEmailAsync("confirmed-no-overwrite@example.com", TestContext.Current.CancellationToken);
        subscriber.Should().NotBeNull();
        subscriber!.WeeklySections.Should().Contain("ai", "preferences must not be overwritten for confirmed subscribers");
        subscriber.WeeklySections.Should().NotContain("dotnet");
        subscriber.DisplayName.Should().Be("Original Name", "display name must not be overwritten for confirmed subscribers");
    }

    [Fact]
    public async Task UnsubscribeAsync_RemovesSubscriberFromActiveQueries()
    {
        await _sut.UpsertSubscriberAsync(
            "unsubscribe-repo@example.com",
            null,
            ["dotnet"],
            [],
            TestContext.Current.CancellationToken);

        var unsubscribed = await _sut.UnsubscribeAsync("unsubscribe-repo@example.com", TestContext.Current.CancellationToken);
        unsubscribed.Should().BeTrue();

        var active = await _sut.GetActiveSubscribersAsync("dotnet", weekly: true, TestContext.Current.CancellationToken);
        active.Should().NotContain(s => s.Email == "unsubscribe-repo@example.com");
    }

    [Fact]
    public async Task LogSendAsync_AndHasBeenSentAsync_WorkAsExpected()
    {
        await _sut.LogSendAsync("weekly-roundup", "weekly-ai-roundup-2026-05-18", 12, 0, "sent", null, TestContext.Current.CancellationToken);

        var hasBeenSent = await _sut.HasBeenSentAsync("weekly-roundup", "weekly-ai-roundup-2026-05-18", TestContext.Current.CancellationToken);
        hasBeenSent.Should().BeTrue();

        var logs = await _sut.GetSendLogAsync(10, TestContext.Current.CancellationToken);
        logs.Should().Contain(l => l.SendKind == "weekly-roundup" && l.TargetKey == "weekly-ai-roundup-2026-05-18");
    }

    [Fact]
    public async Task GetDailyReportStatsAsync_ReturnsNonNegativeCounts()
    {
        var stats = await _sut.GetDailyReportStatsAsync(TestContext.Current.CancellationToken);

        stats.NewContentItemsLast24Hours.Should().BeGreaterThanOrEqualTo(0);
        stats.FailedProcessedUrlsLast24Hours.Should().BeGreaterThanOrEqualTo(0);
        stats.FailedJobsLast24Hours.Should().BeGreaterThanOrEqualTo(0);
        stats.FailedNewsletterSendsLast24Hours.Should().BeGreaterThanOrEqualTo(0);
        stats.NewSubscribersLast24Hours.Should().BeGreaterThanOrEqualTo(0);
        stats.ActiveSubscribers.Should().BeGreaterThanOrEqualTo(0);
        stats.UnconfirmedSubscribers.Should().BeGreaterThanOrEqualTo(0);
    }
}
