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
    private const string ShortDecodedToken = "AQ";

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
    public async Task SendRoundupNewsletterAsync_ShouldRenderResponsiveHtmlMarkers()
    {
        const string Slug = "weekly-ai-roundup-2026-06-03-responsive-markers";
        const string Recipient = "responsive-markers@example.com";

        await SeedRoundupAsync(Slug);
        await _fixture.Connection.ExecuteAsync("""
            DELETE FROM newsletter_subscribers WHERE email = @Email;
            INSERT INTO newsletter_subscribers (email, is_confirmed, confirmed_at, preferences)
            VALUES (@Email, TRUE, NOW(), '{"weeklySections":["ai"],"dailySections":[]}'::jsonb)
            """, new { Email = Recipient });

        string? htmlBody = null;
        var emailSender = new Mock<IEmailSender>(MockBehavior.Strict);
        emailSender
            .Setup(s => s.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<string, string, string, string, CancellationToken>((_, _, html, _, _) => htmlBody = html)
            .ReturnsAsync(true);

        var sut = CreateService(emailSender: emailSender.Object);

        var sent = await sut.SendRoundupNewsletterAsync(Slug, TestContext.Current.CancellationToken);

        sent.Should().BeTrue();
        htmlBody.Should().NotBeNull();
        htmlBody.Should().Contain("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"");
        htmlBody.Should().Contain("width=\"100%\"");
        htmlBody.Should().Contain("max-width:900px");
    }

    [Fact]
    public async Task SendDailyOverviewAsync_SkipsUnconfirmedSubscribers()
    {
        const string Slug = "daily-item-newsletter-test-2026-05-20";
        var day = new DateOnly(2026, 5, 20);
        await CleanupDailyOverviewTestDataAsync();

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

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task SendDailyOverviewAsync_WhenUnsubscribeSecretMissing_ReturnsFalseWithoutSending(string unsubscribeSecret)
    {
        const string Slug = "daily-item-newsletter-test-2026-05-21";
        var day = new DateOnly(2026, 5, 21);
        await CleanupDailyOverviewTestDataAsync();

        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch,
                 primary_section_name, external_url, author, feed_name, tags_csv,
                 sections_bitmask, content_hash, is_ai, created_at)
            VALUES
                (@Slug, 'blogs', 'Daily AI Item', 'Body', 'Excerpt', 1747785600,
                 'ai', '/ai/all-2', 'TechHub', 'TechHub', ',AI,',
                 1, 'hash-daily-newsletter-test-2', TRUE, '2026-05-21T12:00:00Z')
            ON CONFLICT (collection_name, slug) DO NOTHING
            """, new { Slug });

        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO newsletter_subscribers (email, is_confirmed, confirmed_at, preferences)
            VALUES ('confirmed-daily@example.com', TRUE, NOW(), '{"weeklySections":[],"dailySections":["ai"]}'::jsonb)
            """);

        var emailSender = new Mock<IEmailSender>(MockBehavior.Strict);
        var contentRepository = new Mock<IContentRepository>(MockBehavior.Strict);
        contentRepository
            .Setup(x => x.GetAllSectionsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([CreateSection("ai")]);

        var sut = CreateService(contentRepository.Object, emailSender.Object, unsubscribeSecret: unsubscribeSecret);

        var sent = await sut.SendDailyOverviewAsync(day, TestContext.Current.CancellationToken);

        sent.Should().BeFalse();
        emailSender.VerifyNoOtherCalls();
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

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task SendTestEmailAsync_WhenUnsubscribeSecretMissing_ReturnsFalseWithoutSending(string unsubscribeSecret)
    {
        await SeedRoundupAsync("weekly-ai-roundup-2026-05-26");

        var emailSender = new Mock<IEmailSender>(MockBehavior.Strict);
        var sut = CreateService(emailSender: emailSender.Object, unsubscribeSecret: unsubscribeSecret);

        var sent = await sut.SendTestEmailAsync("weekly-ai-roundup-2026-05-26", "test@example.com", TestContext.Current.CancellationToken);

        sent.Should().BeFalse();
        emailSender.VerifyNoOtherCalls();
    }

    [Fact]
    public void IsValidUnsubscribeToken_WithDifferentDecodedLength_ReturnsFalse()
    {
        var isValid = NewsletterService.IsValidUnsubscribeToken("test@example.com", ShortDecodedToken, "test-secret");

        isValid.Should().BeFalse();
    }

    [Fact]
    public async Task SendCombinedWeeklyAsync_WhenAllSlugsAlreadySent_ReturnsFalse()
    {
        const string Slug = "weekly-ai-roundup-2026-06-01-combined-skip";
        await SeedRoundupAsync(Slug);
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO newsletter_send_log (send_kind, target_key, recipient_count, status)
            VALUES ('weekly-roundup', @Slug, 0, 'sent')
            ON CONFLICT (send_kind, target_key) DO UPDATE SET status = 'sent'
            """, new { Slug });

        var sut = CreateService();

        var sent = await sut.SendCombinedWeeklyAsync([Slug], TestContext.Current.CancellationToken);

        sent.Should().BeFalse();
    }

    [Fact]
    public async Task SendCombinedWeeklyAsync_SendsOneEmailPerSubscriberAcrossMultipleSections()
    {
        const string AiSlug = "weekly-ai-roundup-2026-06-02-combined";
        const string DotnetSlug = "weekly-dotnet-roundup-2026-06-02-combined";

        // Remove any weekly AI subscribers left by other tests so VerifyNoOtherCalls is reliable
        await _fixture.Connection.ExecuteAsync("""
            DELETE FROM newsletter_subscribers
            WHERE preferences @> '{"weeklySections":["ai"]}'
               OR email = 'multi-section@example.com'
            """);

        // Seed roundups for two different sections
        await SeedRoundupAsync(AiSlug);
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch,
                 primary_section_name, external_url, author, feed_name, tags_csv,
                 sections_bitmask, content_hash, is_ai)
            VALUES
                (@Slug, 'roundups', '.NET Weekly', '## .NET Highlights', 'Dotnet intro', 1747699200,
                 'dotnet', '/dotnet/roundups/' || @Slug, 'TechHub', 'TechHub', ',Roundups,.NET,',
                 2, 'hash-newsletter-dotnet-combined', FALSE)
            ON CONFLICT (collection_name, slug) DO NOTHING
            """, new { Slug = DotnetSlug });

        // Seed a subscriber subscribed to both sections
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO newsletter_subscribers (email, is_confirmed, confirmed_at, preferences)
            VALUES ('multi-section@example.com', TRUE, NOW(), '{"weeklySections":["ai","dotnet"],"dailySections":[]}'::jsonb)
            ON CONFLICT (lower(email)) WHERE unsubscribed_at IS NULL DO NOTHING
            """);

        var emailSender = new Mock<IEmailSender>();
        emailSender.Setup(s => s.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var sut = CreateService(emailSender: emailSender.Object);

        var sent = await sut.SendCombinedWeeklyAsync([AiSlug, DotnetSlug], TestContext.Current.CancellationToken);

        sent.Should().BeTrue();

        // Exactly one email should be sent (not one per section)
        emailSender.Verify(
            s => s.SendAsync("multi-section@example.com", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
        emailSender.VerifyNoOtherCalls();

        // Both slugs should be logged as sent
        var aiStatus = await _fixture.Connection.ExecuteScalarAsync<string?>(
            "SELECT status FROM newsletter_send_log WHERE send_kind = 'weekly-roundup' AND target_key = @Slug",
            new { Slug = AiSlug });
        var dotnetStatus = await _fixture.Connection.ExecuteScalarAsync<string?>(
            "SELECT status FROM newsletter_send_log WHERE send_kind = 'weekly-roundup' AND target_key = @Slug",
            new { Slug = DotnetSlug });

        aiStatus.Should().Be("sent");
        dotnetStatus.Should().Be("sent");
    }

    [Fact]
    public async Task SendCombinedWeeklyAsync_OrdersSectionsByWebsiteOrderInEmail()
    {
        const string AiSlug = "weekly-ai-roundup-2026-06-03-ordering";
        const string DotnetSlug = "weekly-dotnet-roundup-2026-06-03-ordering";

        await _fixture.Connection.ExecuteAsync("""
            DELETE FROM newsletter_subscribers
            WHERE preferences @> '{"weeklySections":["ai"]}'
               OR preferences @> '{"weeklySections":["dotnet"]}'
               OR email = 'weekly-ordering@example.com';
            DELETE FROM content_items WHERE collection_name = 'roundups' AND slug IN (@AiSlug, @DotnetSlug);
            DELETE FROM newsletter_send_log WHERE send_kind = 'weekly-roundup' AND target_key IN (@AiSlug, @DotnetSlug);
            """, new { AiSlug, DotnetSlug });

        await SeedRoundupAsync(AiSlug);
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch,
                 primary_section_name, external_url, author, feed_name, tags_csv,
                 sections_bitmask, content_hash, is_ai)
            VALUES
                (@Slug, 'roundups', '.NET Weekly', '## .NET Highlights', 'Dotnet intro', 1748304000,
                 'dotnet', '/dotnet/roundups/' || @Slug, 'TechHub', 'TechHub', ',Roundups,.NET,',
                 2, 'hash-newsletter-dotnet-ordering', FALSE)
            ON CONFLICT (collection_name, slug) DO NOTHING
            """, new { Slug = DotnetSlug });

        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO newsletter_subscribers (email, is_confirmed, confirmed_at, preferences)
            VALUES ('weekly-ordering@example.com', TRUE, NOW(), '{"weeklySections":["dotnet","ai"],"dailySections":[]}'::jsonb)
            ON CONFLICT (lower(email)) WHERE unsubscribed_at IS NULL DO UPDATE SET preferences = EXCLUDED.preferences
            """);

        string? htmlBody = null;
        var emailSender = new Mock<IEmailSender>(MockBehavior.Strict);
        emailSender
            .Setup(s => s.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<string, string, string, string, CancellationToken>((recipient, _, html, _, _) =>
            {
                if (string.Equals(recipient, "weekly-ordering@example.com", StringComparison.OrdinalIgnoreCase))
                {
                    htmlBody = html;
                }
            })
            .ReturnsAsync(true);

        var sut = CreateService(emailSender: emailSender.Object);
        var sent = await sut.SendCombinedWeeklyAsync([DotnetSlug, AiSlug], TestContext.Current.CancellationToken);

        sent.Should().BeTrue();
        htmlBody.Should().NotBeNull();
        htmlBody!.IndexOf("Artificial Intelligence", StringComparison.Ordinal).Should().BeLessThan(
            htmlBody.IndexOf(".NET", StringComparison.Ordinal),
            "weekly digest sections should follow configured website order");
    }

    [Fact]
    public async Task SendDailyOverviewAsync_OrdersSectionsByWebsiteOrderInEmail()
    {
        await _fixture.Connection.ExecuteAsync("""
            DELETE FROM newsletter_send_log WHERE send_kind = 'daily-overview' AND target_key = '2026-05-22';
            DELETE FROM newsletter_subscribers WHERE email = 'daily-ordering@example.com';
            DELETE FROM content_items WHERE collection_name = 'blogs' AND slug IN ('daily-order-ai-2026-05-22', 'daily-order-dotnet-2026-05-22');
            """);

        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO newsletter_subscribers (email, is_confirmed, confirmed_at, preferences)
            VALUES ('daily-ordering@example.com', TRUE, NOW(), '{"weeklySections":[],"dailySections":["dotnet","ai"]}'::jsonb)
            """);

        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch,
                 primary_section_name, external_url, author, feed_name, tags_csv,
                 sections_bitmask, content_hash, is_ai, created_at)
            VALUES
                ('daily-order-ai-2026-05-22', 'blogs', 'AI Daily Item', 'Body', 'Excerpt', 1747872000,
                 'ai', '/ai/all-daily-order-2026-05-22', 'TechHub', 'TechHub', ',AI,',
                 1, 'hash-daily-order-ai', TRUE, '2026-05-22T10:00:00Z'),
                ('daily-order-dotnet-2026-05-22', 'blogs', '.NET Daily Item', 'Body', 'Excerpt', 1747872000,
                 'dotnet', '/dotnet/all-daily-order-2026-05-22', 'TechHub', 'TechHub', ',.NET,',
                 2, 'hash-daily-order-dotnet', FALSE, '2026-05-22T10:00:00Z')
            ON CONFLICT (collection_name, slug) DO NOTHING
            """);

        string? htmlBody = null;
        var emailSender = new Mock<IEmailSender>(MockBehavior.Strict);
        emailSender
            .Setup(s => s.SendAsync("daily-ordering@example.com", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<string, string, string, string, CancellationToken>((_, _, html, _, _) => htmlBody = html)
            .ReturnsAsync(true);

        var contentRepository = new Mock<IContentRepository>(MockBehavior.Strict);
        contentRepository
            .Setup(x => x.GetAllSectionsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([CreateSection("ai"), CreateSection("dotnet")]);

        var sut = CreateService(contentRepository.Object, emailSender.Object);
        var sent = await sut.SendDailyOverviewAsync(new DateOnly(2026, 5, 22), TestContext.Current.CancellationToken);

        sent.Should().BeTrue();
        htmlBody.Should().NotBeNull();
        htmlBody!.IndexOf("Artificial Intelligence", StringComparison.Ordinal).Should().BeLessThan(
            htmlBody.IndexOf(".NET", StringComparison.Ordinal),
            "daily email sections should follow configured website order");
    }

    private NewsletterService CreateService(
        IContentRepository? contentRepository = null,
        IEmailSender? emailSender = null,
        string unsubscribeSecret = "test-secret")
    {
        var options = Options.Create(new NewsletterOptions
        {
            Endpoint = "https://invalid.communication.azure.com/",
            SenderAddress = "DoNotReply@example.azurecomm.net",
            WebsiteBaseUrl = "https://tech.hub.ms",
            UnsubscribeSecret = unsubscribeSecret
        });

        contentRepository ??= new Mock<IContentRepository>(MockBehavior.Loose).Object;
        emailSender ??= new Mock<IEmailSender>().Object;

        var appSettings = Options.Create(new AppSettings
        {
            BaseUrl = "https://tech.hub.ms",
            Content = new ContentSettings
            {
                Sections = new Dictionary<string, SectionConfig>
                {
                    ["ai"] = new SectionConfig { Title = "Artificial Intelligence", Description = "", Url = "/ai", Tag = "ai", Order = 1, Collections = [] },
                    ["dotnet"] = new SectionConfig { Title = ".NET", Description = "", Url = "/dotnet", Tag = "dotnet", Order = 2, Collections = [] },
                    ["azure"] = new SectionConfig { Title = "Azure", Description = "", Url = "/azure", Tag = "azure", Order = 3, Collections = [] }
                }
            }
        });

        return new NewsletterService(
            _fixture.Connection,
            new NewsletterSubscriberRepository(_fixture.Connection),
            contentRepository,
            options,
            appSettings,
            emailSender,
            NullLogger<NewsletterService>.Instance);
    }

    private static Section CreateSection(string name) =>
        new(
            name,
            name.Equals("dotnet", StringComparison.OrdinalIgnoreCase) ? ".NET" : "AI",
            $"{name} section",
            $"/{name}",
            name.Equals("dotnet", StringComparison.OrdinalIgnoreCase) ? ".NET" : "AI",
            [new Collection("blogs", "Blogs", $"/{name}/blogs", "Blogs", "Blogs")]);

    private async Task CleanupDailyOverviewTestDataAsync()
    {
        await _fixture.Connection.ExecuteAsync("""
            DELETE FROM newsletter_send_log
            WHERE send_kind = 'daily-overview'
              AND target_key IN ('2026-05-20', '2026-05-21', '2026-05-22');

            DELETE FROM newsletter_subscribers
            WHERE email IN ('confirmed@example.com', 'unconfirmed@example.com', 'confirmed-daily@example.com', 'daily-ordering@example.com');

            DELETE FROM content_items
            WHERE collection_name = 'blogs'
              AND slug IN ('daily-item-newsletter-test-2026-05-20', 'daily-item-newsletter-test-2026-05-21', 'daily-order-ai-2026-05-22', 'daily-order-dotnet-2026-05-22');
            """);
    }

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
