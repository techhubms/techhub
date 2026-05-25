using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;
using TechHub.Infrastructure.Repositories;
using TechHub.Infrastructure.Services.Newsletter;

namespace TechHub.Infrastructure.Tests.Services;

public class NewsletterServiceTests : IClassFixture<DatabaseFixture<NewsletterServiceTests>>
{
    private readonly DatabaseFixture<NewsletterServiceTests> _fixture;

    public NewsletterServiceTests(DatabaseFixture<NewsletterServiceTests> fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        _fixture = fixture;
    }

    [Fact]
    public async Task SendRoundupNewsletterAsync_WhenRoundupMissing_ReturnsFalse()
    {
        var sut = CreateService();

        var sent = await sut.SendRoundupNewsletterAsync("missing-roundup-slug", TestContext.Current.CancellationToken);

        sent.Should().BeFalse();
    }

    [Fact]
    public async Task SendRoundupNewsletterAsync_WhenSendFails_LogsFailedStatus()
    {
        await SeedRoundupAsync("weekly-ai-roundup-2026-05-18");
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO newsletter_subscribers (email, is_confirmed, confirmed_at, preferences)
            VALUES ('not-a-valid-email-address', TRUE, NOW(), '{"weeklySections":["ai"],"dailySections":[]}'::jsonb)
            """);

        var emailSender = new Mock<IEmailSender>();
        emailSender.Setup(s => s.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var sut = CreateService(emailSender: emailSender.Object);

        var sent = await sut.SendRoundupNewsletterAsync("weekly-ai-roundup-2026-05-18", TestContext.Current.CancellationToken);

        sent.Should().BeFalse();
        var status = await _fixture.Connection.ExecuteScalarAsync<string?>(
            "SELECT status FROM newsletter_send_log WHERE send_kind = 'weekly-roundup' AND target_key = 'weekly-ai-roundup-2026-05-18'");
        status.Should().Be("failed");
    }

    [Fact]
    public async Task SendDailyOverviewAsync_SkipsUnconfirmedSubscribers()
    {
        const string Slug = "daily-item-newsletter-test-2026-05-20";
        var day = new DateOnly(2026, 5, 20);

        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch,
                 primary_section_name, external_url, author, feed_name, tags_csv,
                 sections_bitmask, content_hash, is_ai, created_at)
            VALUES
                (@Slug, 'blogs', 'Daily AI Item', 'Body', 'Excerpt', 1747699200,
                 'ai', '/ai/all', 'TechHub', 'TechHub', ',AI,',
                 1, 'hash-daily-newsletter-test', TRUE, '2026-05-20T12:00:00Z')
            ON CONFLICT (collection_name, slug) DO NOTHING
            """, new { Slug });

        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO newsletter_subscribers (email, is_confirmed, confirmed_at, preferences)
            VALUES
                ('confirmed@example.com', TRUE, NOW(), '{"weeklySections":[],"dailySections":["ai"]}'::jsonb),
                ('unconfirmed@example.com', FALSE, NULL, '{"weeklySections":[],"dailySections":["ai"]}'::jsonb)
            """);

        var emailSender = new Mock<IEmailSender>(MockBehavior.Strict);
        emailSender
            .Setup(s => s.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var contentRepository = new Mock<IContentRepository>(MockBehavior.Strict);
        contentRepository
            .Setup(x => x.GetAllSectionsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([CreateSection("ai")]);

        var sut = CreateService(contentRepository.Object, emailSender.Object);

        var sent = await sut.SendDailyOverviewAsync(day, TestContext.Current.CancellationToken);

        sent.Should().BeTrue();
        emailSender.Verify(
            s => s.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
        var recipientCount = await _fixture.Connection.ExecuteScalarAsync<int>(
            "SELECT recipient_count FROM newsletter_send_log WHERE send_kind = 'daily-overview' AND target_key = '2026-05-20'");
        recipientCount.Should().Be(1);
    }

    [Fact]
    public async Task SendRoundupNewsletterAsync_WhenUnsubscribeSecretMissing_LogsFailedStatus()
    {
        await SeedRoundupAsync("weekly-ai-roundup-2026-05-25");
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO newsletter_subscribers (email, is_confirmed, confirmed_at, preferences)
            VALUES ('configured@example.com', TRUE, NOW(), '{"weeklySections":["ai"],"dailySections":[]}'::jsonb)
            """);

        var emailSender = new Mock<IEmailSender>(MockBehavior.Strict);
        var sut = CreateService(emailSender: emailSender.Object, unsubscribeSecret: " ");

        var sent = await sut.SendRoundupNewsletterAsync("weekly-ai-roundup-2026-05-25", TestContext.Current.CancellationToken);

        sent.Should().BeFalse();
        emailSender.VerifyNoOtherCalls();
        var status = await _fixture.Connection.ExecuteScalarAsync<string?>(
            "SELECT status FROM newsletter_send_log WHERE send_kind = 'weekly-roundup' AND target_key = 'weekly-ai-roundup-2026-05-25'");
        status.Should().Be("failed");
    }

    private NewsletterService CreateService(
        IContentRepository? contentRepository = null,
        IEmailSender? emailSender = null,
        string unsubscribeSecret = "test-secret")
    {
        var options = Options.Create(new NewsletterOptions
        {
            ConnectionString = "endpoint=https://invalid.communication.azure.com/;accesskey=invalid",
            SenderAddress = "DoNotReply@example.azurecomm.net",
            WebsiteBaseUrl = "https://tech.hub.ms",
            UnsubscribeSecret = unsubscribeSecret
        });

        contentRepository ??= new Mock<IContentRepository>(MockBehavior.Loose).Object;
        emailSender ??= new Mock<IEmailSender>().Object;
        return new NewsletterService(
            _fixture.Connection,
            new NewsletterSubscriberRepository(_fixture.Connection),
            contentRepository,
            options,
            emailSender,
            NullLogger<NewsletterService>.Instance);
    }

    private static Section CreateSection(string name) =>
        new(
            name,
            "AI",
            "AI section",
            "/ai",
            "AI",
            [new Collection("blogs", "Blogs", "/ai/blogs", "Blogs", "Blogs")]);

    private async Task SeedRoundupAsync(string slug)
    {
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch,
                 primary_section_name, external_url, author, feed_name, tags_csv,
                 sections_bitmask, content_hash, is_ai)
            VALUES
                (@Slug, 'roundups', 'AI Weekly', '## AI Highlights', 'Intro', 1747699200,
                 'ai', '/ai/roundups/' || @Slug, 'TechHub', 'TechHub', ',Roundups,AI,',
                 1, 'hash-newsletter-test', TRUE)
            ON CONFLICT (collection_name, slug) DO NOTHING
            """, new { Slug = slug });
    }
}
