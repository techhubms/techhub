using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Hosting;
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
    public void IsValidUnsubscribeToken_WithDifferentDecodedLength_ReturnsFalse()
    {
        var isValid = NewsletterService.IsValidUnsubscribeToken("test@example.com", ShortDecodedToken, "test-secret");

        isValid.Should().BeFalse();
    }

    [Fact]
    public async Task SendCombinedWeeklyAsync_WhenAllSlugsAlreadySent_ReturnsFalse()
    {
        const string Slug = "weekly-ai-roundup-2026-06-01-combined-skip";
        const string TargetKey = "2026-06-01";
        await SeedRoundupAsync(Slug);
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO newsletter_send_log (send_kind, target_key, recipient_count, status)
            VALUES ('weekly-roundup', @TargetKey, 0, 'sent')
            ON CONFLICT (send_kind, target_key) DO UPDATE SET status = 'sent'
            """, new { TargetKey });

        var sut = CreateService();

        var sent = await sut.SendCombinedWeeklyAsync([Slug], TargetKey, TestContext.Current.CancellationToken);

        sent.Should().BeFalse();
    }

    [Fact]
    public async Task SendCombinedWeeklyAsync_SendsOneEmailPerSubscriberAcrossMultipleSections()
    {
        const string AiSlug = "weekly-ai-roundup-2026-06-02-combined";
        const string DotnetSlug = "weekly-dotnet-roundup-2026-06-02-combined";
        const string TargetKey = "2026-06-02";

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

        var sent = await sut.SendCombinedWeeklyAsync([AiSlug, DotnetSlug], TargetKey, TestContext.Current.CancellationToken);

        sent.Should().BeTrue();

        // Exactly one email should be sent (not one per section)
        emailSender.Verify(
            s => s.SendAsync("multi-section@example.com", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
        emailSender.VerifyNoOtherCalls();

        var status = await _fixture.Connection.ExecuteScalarAsync<string?>(
            "SELECT status FROM newsletter_send_log WHERE send_kind = 'weekly-roundup' AND target_key = @TargetKey",
            new { TargetKey });
        var rowCount = await _fixture.Connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM newsletter_send_log WHERE send_kind = 'weekly-roundup' AND target_key = @TargetKey",
            new { TargetKey });

        status.Should().Be("sent");
        rowCount.Should().Be(1);
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
        var sent = await sut.SendCombinedWeeklyAsync([DotnetSlug, AiSlug], ct: TestContext.Current.CancellationToken);

        sent.Should().BeTrue();
        htmlBody.Should().NotBeNull();
        htmlBody!.IndexOf("Artificial Intelligence", StringComparison.Ordinal).Should().BeLessThan(
            htmlBody.IndexOf(".NET", StringComparison.Ordinal),
            "weekly digest sections should follow configured website order");
    }

    [Fact]
    public async Task SendDailyOverviewAsync_OrdersSectionsByWebsiteOrderInEmail()
    {
        const string RecipientEmail = "daily-ordering@example.com";

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
            .Setup(s => s.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<string, string, string, string, CancellationToken>((recipient, _, html, _, _) =>
            {
                if (string.Equals(recipient, RecipientEmail, StringComparison.Ordinal))
                {
                    htmlBody = html;
                }
            })
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

    [Fact]
    public async Task SendDailyOverviewAsync_LinksToCanonicalContentPage_NotSearchResultsPage()
    {
        const string Slug = "daily-item-newsletter-link-test-2026-05-26";
        const string RecipientEmail = "daily-link-test@example.com";
        var day = new DateOnly(2026, 5, 26);
        await CleanupDailyOverviewTestDataAsync();

        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch,
                 primary_section_name, external_url, author, feed_name, tags_csv,
                 sections_bitmask, content_hash, is_ai, created_at)
            VALUES
                (@Slug, 'blogs', 'Daily Link Test Item', 'Body', 'Excerpt', 1748044800,
                 'ai', 'https://example.com/external-article', 'TechHub', 'TechHub', ',AI,',
                 1, 'hash-daily-newsletter-link-test', TRUE, '2026-05-26T12:00:00Z')
            ON CONFLICT (collection_name, slug) DO NOTHING
            """, new { Slug });

        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO newsletter_subscribers (email, is_confirmed, confirmed_at, preferences)
            VALUES ('daily-link-test@example.com', TRUE, NOW(), '{"weeklySections":[],"dailySections":["ai"]}'::jsonb)
            """);

        string? htmlBody = null;
        string? textBody = null;
        var emailSender = new Mock<IEmailSender>(MockBehavior.Strict);
        emailSender
            .Setup(s => s.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<string, string, string, string, CancellationToken>((recipient, _, html, text, _) =>
            {
                if (string.Equals(recipient, RecipientEmail, StringComparison.Ordinal))
                {
                    htmlBody = html;
                    textBody = text;
                }
            })
            .ReturnsAsync(true);

        var contentRepository = new Mock<IContentRepository>(MockBehavior.Strict);
        contentRepository
            .Setup(x => x.GetAllSectionsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([CreateSection("ai")]);

        var sut = CreateService(contentRepository.Object, emailSender.Object);

        var sent = await sut.SendDailyOverviewAsync(day, TestContext.Current.CancellationToken);

        sent.Should().BeTrue();
        const string ExpectedUrl = "https://tech.hub.ms/ai/blogs/daily-item-newsletter-link-test-2026-05-26";
        htmlBody.Should().NotBeNull();
        htmlBody!.Should().Contain(ExpectedUrl, "newsletter items should always link to the canonical TechHub page");
        htmlBody.Should().NotContain("/all?search=", "newsletter items should not link to the filtered search results page");
        textBody.Should().NotBeNull();
        textBody!.Should().Contain(ExpectedUrl, "newsletter items should always link to the canonical TechHub page");
        textBody.Should().NotContain("/all?search=", "newsletter items should not link to the filtered search results page");
    }

    [Fact]
    public async Task SendDailyOverviewAsync_WhenNoItemsForSubscriberSections_SendsEmailWithNoItemsMessage()
    {
        const string RecipientEmail = "daily-no-items@example.com";
        var day = new DateOnly(2026, 5, 25);

        // Remove leftover subscribers from other tests (e.g. confirmed@example.com) that also
        // have "ai" in their daily sections, so this test's Strict mock only sees one recipient.
        await CleanupDailyOverviewTestDataAsync();

        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO newsletter_subscribers (email, is_confirmed, confirmed_at, preferences)
            VALUES ('daily-no-items@example.com', TRUE, NOW(), '{"weeklySections":[],"dailySections":["ai"]}'::jsonb)
            """);

        string? htmlBody = null;
        string? textBody = null;
        var emailSender = new Mock<IEmailSender>(MockBehavior.Strict);
        emailSender
            .Setup(s => s.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<string, string, string, string, CancellationToken>((recipient, _, html, text, _) =>
            {
                if (string.Equals(recipient, RecipientEmail, StringComparison.Ordinal))
                {
                    htmlBody = html;
                    textBody = text;
                }
            })
            .ReturnsAsync(true);

        var contentRepository = new Mock<IContentRepository>(MockBehavior.Strict);
        contentRepository
            .Setup(x => x.GetAllSectionsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([CreateSection("ai")]);

        var sut = CreateService(contentRepository.Object, emailSender.Object);

        var sent = await sut.SendDailyOverviewAsync(day, TestContext.Current.CancellationToken);

        sent.Should().BeTrue();
        emailSender.Verify(
            s => s.SendAsync(RecipientEmail, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
        emailSender.VerifyNoOtherCalls();
        htmlBody.Should().Contain("No new content items for today.");
        textBody.Should().Contain("No new content items for today.");
    }

    [Fact]
    public async Task SendAdminStatusReportAsync_WhenNoFailures_ReturnsFalseWithoutSendingEmail()
    {
        var day = new DateOnly(2026, 5, 28);
        const string TargetKey = "2026-05-28";
        await _fixture.Connection.ExecuteAsync("""
            DELETE FROM newsletter_send_log WHERE send_kind = 'admin-status' AND target_key = @TargetKey
            """, new { TargetKey });

        var emailSender = new Mock<IEmailSender>(MockBehavior.Strict);
        var sut = CreateService(adminReportEmailAddress: "admin@example.com", emailSender: emailSender.Object);

        var result = await sut.SendAdminStatusReportAsync(day, TestContext.Current.CancellationToken);

        result.Should().BeFalse();
        emailSender.VerifyNoOtherCalls();
    }

    // ── Token logic (static, no database) ─────────────────────────────────

    [Fact]
    public void IsValidConfirmToken_TokenBuiltForSameEmailAndSecret_ReturnsTrue()
    {
        // Arrange
        var token = NewsletterService.BuildConfirmToken("user@example.com", Secret);

        // Act
        var isValid = NewsletterService.IsValidConfirmToken("user@example.com", token, Secret);

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void BuildConfirmToken_SameInput_IsDeterministicAndUrlSafe()
    {
        // Act
        var first = NewsletterService.BuildConfirmToken("user@example.com", Secret);
        var second = NewsletterService.BuildConfirmToken("user@example.com", Secret);

        // Assert
        first.Should().Be(second);
        first.Should().HaveLength(43, "an HMAC-SHA256 digest encodes to 43 unpadded base64url characters");
        first.Should().MatchRegex("^[A-Za-z0-9_-]+$");
    }

    [Fact]
    public void IsValidConfirmToken_TamperedToken_ReturnsFalse()
    {
        // Arrange
        var token = NewsletterService.BuildConfirmToken("user@example.com", Secret);
        var tampered = (token[0] == 'A' ? 'B' : 'A') + token[1..];

        // Act
        var isValid = NewsletterService.IsValidConfirmToken("user@example.com", tampered, Secret);

        // Assert
        isValid.Should().BeFalse();
    }

    [Fact]
    public void IsValidConfirmToken_TokenForDifferentEmail_ReturnsFalse()
    {
        // Arrange
        var token = NewsletterService.BuildConfirmToken("user@example.com", Secret);

        // Act
        var isValid = NewsletterService.IsValidConfirmToken("other@example.com", token, Secret);

        // Assert
        isValid.Should().BeFalse();
    }

    [Fact]
    public void IsValidConfirmToken_TokenSignedWithDifferentSecret_ReturnsFalse()
    {
        // Arrange
        var token = NewsletterService.BuildConfirmToken("user@example.com", "another-secret");

        // Act
        var isValid = NewsletterService.IsValidConfirmToken("user@example.com", token, Secret);

        // Assert
        isValid.Should().BeFalse();
    }

    [Fact]
    public void IsValidConfirmToken_UnsubscribeTokenUsed_ReturnsFalse()
    {
        // Arrange
        var unsubscribeToken = NewsletterService.BuildUnsubscribeToken("user@example.com", Secret);

        // Act
        var isValid = NewsletterService.IsValidConfirmToken("user@example.com", unsubscribeToken, Secret);

        // Assert
        isValid.Should().BeFalse("confirm and unsubscribe tokens use separate HMAC domains");
    }

    [Fact]
    public void IsValidUnsubscribeToken_ConfirmTokenUsed_ReturnsFalse()
    {
        // Arrange
        var confirmToken = NewsletterService.BuildConfirmToken("user@example.com", Secret);

        // Act
        var isValid = NewsletterService.IsValidUnsubscribeToken("user@example.com", confirmToken, Secret);

        // Assert
        isValid.Should().BeFalse("a confirmation link must never grant unsubscribe or manage access");
    }

    [Fact]
    public void BuildConfirmToken_AndBuildUnsubscribeToken_ProduceDifferentTokensForSameInput()
    {
        // Act
        var confirmToken = NewsletterService.BuildConfirmToken("user@example.com", Secret);
        var unsubscribeToken = NewsletterService.BuildUnsubscribeToken("user@example.com", Secret);

        // Assert
        confirmToken.Should().NotBe(unsubscribeToken);
    }

    [Theory]
    [InlineData("USER@EXAMPLE.COM")]
    [InlineData("  user@example.com  ")]
    [InlineData("User@Example.com")]
    public void IsValidConfirmToken_EmailWithDifferentCaseOrWhitespace_ReturnsTrue(string email)
    {
        // Arrange
        var token = NewsletterService.BuildConfirmToken("user@example.com", Secret);

        // Act
        var isValid = NewsletterService.IsValidConfirmToken(email, token, Secret);

        // Assert
        isValid.Should().BeTrue("emails are normalised (trimmed, lower-cased) before signing");
    }

    [Theory]
    [InlineData("USER@EXAMPLE.COM")]
    [InlineData("  user@example.com  ")]
    public void IsValidUnsubscribeToken_EmailWithDifferentCaseOrWhitespace_ReturnsTrue(string email)
    {
        // Arrange
        var token = NewsletterService.BuildUnsubscribeToken("user@example.com", Secret);

        // Act
        var isValid = NewsletterService.IsValidUnsubscribeToken(email, token, Secret);

        // Assert
        isValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("not base64!")]
    [InlineData("a")]
    [InlineData("!!!!")]
    [InlineData("AAAA")]
    public void IsValidConfirmToken_MalformedOrWrongLengthToken_ReturnsFalseWithoutThrowing(string token)
    {
        // Act
        var isValid = NewsletterService.IsValidConfirmToken("user@example.com", token, Secret);

        // Assert
        isValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("not base64!")]
    [InlineData("a")]
    [InlineData("!!!!")]
    public void IsValidUnsubscribeToken_MalformedToken_ReturnsFalseWithoutThrowing(string token)
    {
        // Act
        var isValid = NewsletterService.IsValidUnsubscribeToken("user@example.com", token, Secret);

        // Assert
        isValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void IsValidConfirmToken_BlankToken_ReturnsFalse(string token)
    {
        // Act
        var isValid = NewsletterService.IsValidConfirmToken("user@example.com", token, Secret);

        // Assert
        isValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void BuildConfirmToken_BlankSecret_ThrowsArgumentException(string secret)
    {
        // Act
        var act = () => NewsletterService.BuildConfirmToken("user@example.com", secret);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("secret");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void BuildUnsubscribeToken_BlankSecret_ThrowsArgumentException(string secret)
    {
        // Act
        var act = () => NewsletterService.BuildUnsubscribeToken("user@example.com", secret);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("secret");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void IsValidConfirmToken_BlankSecret_Throws(string secret)
    {
        // Arrange
        var token = NewsletterService.BuildConfirmToken("user@example.com", Secret);

        // Act
        var act = () => NewsletterService.IsValidConfirmToken("user@example.com", token, secret);

        // Assert — callers must guard against a blank secret before validating
        act.Should().Throw<ArgumentException>();
    }

    // ── ConfirmSubscriberAsync ────────────────────────────────────────────

    [Fact]
    public async Task ConfirmSubscriberAsync_ValidToken_ConfirmsSubscriber()
    {
        // Arrange
        const string Email = "confirm-valid@example.com";
        await SeedSubscriberAsync(Email, confirmed: false);
        var sut = CreateService();
        var token = NewsletterService.BuildConfirmToken(Email, Secret);

        // Act
        var result = await sut.ConfirmSubscriberAsync(Email, token, TestContext.Current.CancellationToken);

        // Assert
        result.Should().Be(ConfirmSubscriptionResult.Confirmed);
        (await IsConfirmedAsync(Email)).Should().BeTrue();
    }

    [Fact]
    public async Task ConfirmSubscriberAsync_EmailInDifferentCase_ConfirmsSubscriber()
    {
        // Arrange
        const string Email = "confirm-case@example.com";
        await SeedSubscriberAsync(Email, confirmed: false);
        var sut = CreateService();
        var token = NewsletterService.BuildConfirmToken(Email, Secret);

        // Act
        var result = await sut.ConfirmSubscriberAsync("Confirm-CASE@Example.com", token, TestContext.Current.CancellationToken);

        // Assert
        result.Should().Be(ConfirmSubscriptionResult.Confirmed);
        (await IsConfirmedAsync(Email)).Should().BeTrue();
    }

    [Fact]
    public async Task ConfirmSubscriberAsync_AlreadyConfirmedSubscriber_ReturnsAlreadyConfirmed()
    {
        // Arrange
        const string Email = "confirm-already@example.com";
        await SeedSubscriberAsync(Email, confirmed: true);
        var sut = CreateService();
        var token = NewsletterService.BuildConfirmToken(Email, Secret);

        // Act
        var result = await sut.ConfirmSubscriberAsync(Email, token, TestContext.Current.CancellationToken);

        // Assert
        result.Should().Be(ConfirmSubscriptionResult.AlreadyConfirmed);
    }

    [Fact]
    public async Task ConfirmSubscriberAsync_TamperedToken_ReturnsInvalidTokenAndDoesNotConfirm()
    {
        // Arrange
        const string Email = "confirm-tampered@example.com";
        await SeedSubscriberAsync(Email, confirmed: false);
        var sut = CreateService();
        var token = NewsletterService.BuildConfirmToken(Email, Secret);
        var tampered = (token[0] == 'A' ? 'B' : 'A') + token[1..];

        // Act
        var result = await sut.ConfirmSubscriberAsync(Email, tampered, TestContext.Current.CancellationToken);

        // Assert
        result.Should().Be(ConfirmSubscriptionResult.InvalidToken);
        (await IsConfirmedAsync(Email)).Should().BeFalse();
    }

    [Fact]
    public async Task ConfirmSubscriberAsync_UnsubscribeTokenUsed_ReturnsInvalidTokenAndDoesNotConfirm()
    {
        // Arrange
        const string Email = "confirm-wrong-domain@example.com";
        await SeedSubscriberAsync(Email, confirmed: false);
        var sut = CreateService();
        var unsubscribeToken = NewsletterService.BuildUnsubscribeToken(Email, Secret);

        // Act
        var result = await sut.ConfirmSubscriberAsync(Email, unsubscribeToken, TestContext.Current.CancellationToken);

        // Assert
        result.Should().Be(ConfirmSubscriptionResult.InvalidToken);
        (await IsConfirmedAsync(Email)).Should().BeFalse();
    }

    [Fact]
    public async Task ConfirmSubscriberAsync_ValidTokenForUnknownEmail_ReturnsInvalidToken()
    {
        // Arrange
        const string Email = "confirm-unknown@example.com";
        await _fixture.Connection.ExecuteAsync("DELETE FROM newsletter_subscribers WHERE lower(email) = @Email", new { Email });
        var sut = CreateService();
        var token = NewsletterService.BuildConfirmToken(Email, Secret);

        // Act
        var result = await sut.ConfirmSubscriberAsync(Email, token, TestContext.Current.CancellationToken);

        // Assert
        result.Should().Be(ConfirmSubscriptionResult.InvalidToken);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ConfirmSubscriberAsync_BlankSecret_ReturnsInvalidTokenAndDoesNotConfirm(string secret)
    {
        // Arrange
        var email = secret.Length == 0 ? "confirm-empty-secret@example.com" : "confirm-whitespace-secret@example.com";
        await SeedSubscriberAsync(email, confirmed: false);
        var sut = CreateService(unsubscribeSecret: secret);
        var tokenFromAnotherSecret = NewsletterService.BuildConfirmToken(email, Secret);

        // Act
        var result = await sut.ConfirmSubscriberAsync(email, tokenFromAnotherSecret, TestContext.Current.CancellationToken);

        // Assert
        result.Should().Be(ConfirmSubscriptionResult.InvalidToken);
        (await IsConfirmedAsync(email)).Should().BeFalse();
    }

    // ── SendConfirmationEmailAsync ────────────────────────────────────────

    [Fact]
    public async Task SendConfirmationEmailAsync_WithSecret_SendsEmailWithValidConfirmLink()
    {
        // Arrange
        const string Email = "user+tag@example.com";
        var sent = new List<SentEmail>();
        var emailSender = CreateRecordingSender(sent);
        var sut = CreateService(emailSender: emailSender.Object);

        // Act
        var result = await sut.SendConfirmationEmailAsync(Email, TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeTrue();
        var message = sent.Should().ContainSingle().Subject;
        message.Recipient.Should().Be(Email);
        message.Subject.Should().Be("Confirm your TechHub newsletter subscription");
        message.Html.Should().Contain("Confirm subscription");
        message.Text.Should().Contain("https://tech.hub.ms/newsletter/confirm?email=user%2Btag%40example.com&token=");
        NewsletterService.IsValidConfirmToken(Email, ExtractToken(message.Text), Secret).Should().BeTrue();
        NewsletterService.IsValidUnsubscribeToken(Email, ExtractToken(message.Text), Secret).Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SendConfirmationEmailAsync_BlankSecret_ReturnsFalseWithoutSending(string secret)
    {
        // Arrange
        var emailSender = new Mock<IEmailSender>(MockBehavior.Strict);
        var sut = CreateService(emailSender: emailSender.Object, unsubscribeSecret: secret);

        // Act
        var result = await sut.SendConfirmationEmailAsync("user@example.com", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeFalse();
        emailSender.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task SendConfirmationEmailAsync_SenderFails_ReturnsFalse()
    {
        // Arrange
        var sent = new List<SentEmail>();
        var emailSender = CreateRecordingSender(sent, result: false);
        var sut = CreateService(emailSender: emailSender.Object);

        // Act
        var result = await sut.SendConfirmationEmailAsync("user@example.com", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeFalse();
        sent.Should().ContainSingle();
    }

    // ── SendManageLinkEmailAsync ──────────────────────────────────────────

    [Fact]
    public async Task SendManageLinkEmailAsync_ConfirmedSubscriber_SendsEmailWithValidManageLink()
    {
        // Arrange
        const string Email = "manage-link-confirmed@example.com";
        await SeedSubscriberAsync(Email, confirmed: true);
        var sent = new List<SentEmail>();
        var emailSender = CreateRecordingSender(sent);
        var sut = CreateService(emailSender: emailSender.Object);

        // Act
        var result = await sut.SendManageLinkEmailAsync(Email, TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeTrue();
        var message = sent.Should().ContainSingle().Subject;
        message.Recipient.Should().Be(Email);
        message.Subject.Should().Be("Manage your TechHub newsletter subscription");
        message.Text.Should().Contain("https://tech.hub.ms/newsletter/manage?email=manage-link-confirmed%40example.com&token=");
        NewsletterService.IsValidUnsubscribeToken(Email, ExtractToken(message.Text), Secret).Should().BeTrue();
    }

    [Fact]
    public async Task SendManageLinkEmailAsync_UnknownEmail_ReturnsTrueWithoutSending()
    {
        // Arrange
        const string Email = "manage-link-unknown@example.com";
        await _fixture.Connection.ExecuteAsync("DELETE FROM newsletter_subscribers WHERE lower(email) = @Email", new { Email });
        var emailSender = new Mock<IEmailSender>(MockBehavior.Strict);
        var sut = CreateService(emailSender: emailSender.Object);

        // Act
        var result = await sut.SendManageLinkEmailAsync(Email, TestContext.Current.CancellationToken);

        // Assert — must not reveal whether an address is subscribed
        result.Should().BeTrue();
        emailSender.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task SendManageLinkEmailAsync_UnsubscribedSubscriber_ReturnsTrueWithoutSending()
    {
        // Arrange
        const string Email = "manage-link-unsubscribed@example.com";
        await SeedSubscriberAsync(Email, confirmed: true);
        await _fixture.Connection.ExecuteAsync(
            "UPDATE newsletter_subscribers SET unsubscribed_at = NOW() WHERE lower(email) = @Email", new { Email });
        var emailSender = new Mock<IEmailSender>(MockBehavior.Strict);
        var sut = CreateService(emailSender: emailSender.Object);

        // Act
        var result = await sut.SendManageLinkEmailAsync(Email, TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeTrue();
        emailSender.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SendManageLinkEmailAsync_BlankSecret_ReturnsFalseWithoutSending(string secret)
    {
        // Arrange
        const string Email = "manage-link-blank-secret@example.com";
        await SeedSubscriberAsync(Email, confirmed: true);
        var emailSender = new Mock<IEmailSender>(MockBehavior.Strict);
        var sut = CreateService(emailSender: emailSender.Object, unsubscribeSecret: secret);

        // Act
        var result = await sut.SendManageLinkEmailAsync(Email, TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeFalse();
        emailSender.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task SendManageLinkEmailAsync_SenderFails_ReturnsFalse()
    {
        // Arrange
        const string Email = "manage-link-sender-fails@example.com";
        await SeedSubscriberAsync(Email, confirmed: true);
        var sent = new List<SentEmail>();
        var emailSender = CreateRecordingSender(sent, result: false);
        var sut = CreateService(emailSender: emailSender.Object);

        // Act
        var result = await sut.SendManageLinkEmailAsync(Email, TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeFalse();
        sent.Should().ContainSingle();
    }

    // ── GetSubscriberPreferencesAsync / UpdateSubscriberPreferencesAsync ──

    [Fact]
    public async Task GetSubscriberPreferencesAsync_ValidToken_ReturnsSubscriber()
    {
        // Arrange
        const string Email = "prefs-get-valid@example.com";
        await SeedSubscriberAsync(Email, confirmed: true, preferencesJson: """{"weeklySections":["azure"],"dailySections":[]}""");
        var sut = CreateService();
        var token = NewsletterService.BuildUnsubscribeToken(Email, Secret);

        // Act
        var subscriber = await sut.GetSubscriberPreferencesAsync(Email, token, TestContext.Current.CancellationToken);

        // Assert
        subscriber.Should().NotBeNull();
        subscriber!.Email.Should().Be(Email);
        subscriber.WeeklySections.Should().Equal("azure");
    }

    [Fact]
    public async Task GetSubscriberPreferencesAsync_ConfirmTokenUsed_ReturnsNull()
    {
        // Arrange
        const string Email = "prefs-get-confirm-token@example.com";
        await SeedSubscriberAsync(Email, confirmed: true);
        var sut = CreateService();
        var confirmToken = NewsletterService.BuildConfirmToken(Email, Secret);

        // Act
        var subscriber = await sut.GetSubscriberPreferencesAsync(Email, confirmToken, TestContext.Current.CancellationToken);

        // Assert
        subscriber.Should().BeNull();
    }

    [Fact]
    public async Task GetSubscriberPreferencesAsync_BlankSecret_ReturnsNull()
    {
        // Arrange
        const string Email = "prefs-get-blank-secret@example.com";
        await SeedSubscriberAsync(Email, confirmed: true);
        var sut = CreateService(unsubscribeSecret: "");
        var token = NewsletterService.BuildUnsubscribeToken(Email, Secret);

        // Act
        var subscriber = await sut.GetSubscriberPreferencesAsync(Email, token, TestContext.Current.CancellationToken);

        // Assert
        subscriber.Should().BeNull();
    }

    [Fact]
    public async Task UpdateSubscriberPreferencesAsync_ValidToken_PersistsPreferences()
    {
        // Arrange
        const string Email = "prefs-update-valid@example.com";
        await SeedSubscriberAsync(Email, confirmed: true);
        var sut = CreateService();
        var token = NewsletterService.BuildUnsubscribeToken(Email, Secret);

        // Act
        var updated = await sut.UpdateSubscriberPreferencesAsync(
            Email, token, "Jane", ["azure"], [], TestContext.Current.CancellationToken);

        // Assert
        updated.Should().BeTrue();
        var stored = await new NewsletterSubscriberRepository(_fixture.Connection)
            .GetSubscriberByEmailAsync(Email, TestContext.Current.CancellationToken);
        stored.Should().NotBeNull();
        stored!.DisplayName.Should().Be("Jane");
        stored.WeeklySections.Should().Equal("azure");
        stored.DailySections.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateSubscriberPreferencesAsync_InvalidToken_ReturnsFalseAndLeavesPreferencesUnchanged()
    {
        // Arrange
        const string Email = "prefs-update-invalid@example.com";
        await SeedSubscriberAsync(Email, confirmed: true, preferencesJson: """{"weeklySections":["azure"],"dailySections":[]}""");
        var sut = CreateService();
        var confirmToken = NewsletterService.BuildConfirmToken(Email, Secret);

        // Act
        var updated = await sut.UpdateSubscriberPreferencesAsync(
            Email, confirmToken, "Hacker", [], ["azure"], TestContext.Current.CancellationToken);

        // Assert
        updated.Should().BeFalse();
        var stored = await new NewsletterSubscriberRepository(_fixture.Connection)
            .GetSubscriberByEmailAsync(Email, TestContext.Current.CancellationToken);
        stored!.DisplayName.Should().BeNull();
        stored.WeeklySections.Should().Equal("azure");
        stored.DailySections.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateSubscriberPreferencesAsync_UnconfirmedSubscriber_ReturnsFalse()
    {
        // Arrange
        const string Email = "prefs-update-unconfirmed@example.com";
        await SeedSubscriberAsync(Email, confirmed: false);
        var sut = CreateService();
        var token = NewsletterService.BuildUnsubscribeToken(Email, Secret);

        // Act
        var updated = await sut.UpdateSubscriberPreferencesAsync(
            Email, token, null, ["azure"], [], TestContext.Current.CancellationToken);

        // Assert
        updated.Should().BeFalse();
    }

    // ── SendTestWeeklyAsync ───────────────────────────────────────────────

    [Fact]
    public async Task SendTestWeeklyAsync_NoSections_ReturnsFalseWithoutSending()
    {
        // Arrange
        var emailSender = new Mock<IEmailSender>(MockBehavior.Strict);
        var sut = CreateService(emailSender: emailSender.Object);

        // Act
        var result = await sut.SendTestWeeklyAsync([], "admin@example.com", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeFalse();
        emailSender.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SendTestWeeklyAsync_BlankSecret_ReturnsFalseWithoutSending(string secret)
    {
        // Arrange
        var emailSender = new Mock<IEmailSender>(MockBehavior.Strict);
        var sut = CreateService(emailSender: emailSender.Object, unsubscribeSecret: secret);

        // Act
        var result = await sut.SendTestWeeklyAsync(["ai"], "admin@example.com", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeFalse();
        emailSender.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task SendTestWeeklyAsync_NoRoundupForSection_ReturnsFalseWithoutSendingOrLogging()
    {
        // Arrange
        await ClearTestSendLogAsync();
        var emailSender = new Mock<IEmailSender>(MockBehavior.Strict);
        var sut = CreateService(emailSender: emailSender.Object);

        // Act
        var result = await sut.SendTestWeeklyAsync(["section-without-roundups"], "admin@example.com", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeFalse();
        emailSender.VerifyNoOtherCalls();
        (await CountTestSendLogRowsAsync("test-send")).Should().Be(0);
    }

    [Fact]
    public async Task SendTestWeeklyAsync_RoundupExists_SendsTestEmailAndLogsSuccess()
    {
        // Arrange
        const string Recipient = "test-weekly-admin@example.com";
        await ClearTestSendLogAsync();
        await SeedLatestTestRoundupAsync();
        var sent = new List<SentEmail>();
        var emailSender = CreateRecordingSender(sent);
        var sut = CreateService(emailSender: emailSender.Object);

        // Act
        var result = await sut.SendTestWeeklyAsync(["ai"], Recipient, TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeTrue();
        var message = sent.Should().ContainSingle().Subject;
        message.Recipient.Should().Be(Recipient);
        message.Subject.Should().Be($"[Test] {LatestTestRoundupTitle}");
        message.Html.Should().Contain("Artificial Intelligence");
        message.Text.Should().Contain($"https://tech.hub.ms/ai/roundups/{LatestTestRoundupSlug}");
        NewsletterService.IsValidUnsubscribeToken(Recipient, ExtractToken(message.Text), Secret).Should().BeTrue();

        var log = await GetSingleTestSendLogAsync("test-send");
        log.Status.Should().Be("sent");
        log.RecipientCount.Should().Be(1);
        log.FailedCount.Should().Be(0);
        log.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public async Task SendTestWeeklyAsync_SenderFails_ReturnsFalseAndLogsFailure()
    {
        // Arrange
        await ClearTestSendLogAsync();
        await SeedLatestTestRoundupAsync();
        var sent = new List<SentEmail>();
        var emailSender = CreateRecordingSender(sent, result: false);
        var sut = CreateService(emailSender: emailSender.Object);

        // Act
        var result = await sut.SendTestWeeklyAsync(["ai"], "test-weekly-fail@example.com", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeFalse();
        var log = await GetSingleTestSendLogAsync("test-send");
        await ClearTestSendLogAsync(); // failed_count feeds the admin status report; don't leak it to other tests
        log.Status.Should().Be("failed");
        log.RecipientCount.Should().Be(0);
        log.FailedCount.Should().Be(1);
        log.ErrorMessage.Should().Be("Delivery failed");
    }

    // ── SendTestDailyEmailAsync ───────────────────────────────────────────

    [Fact]
    public async Task SendTestDailyEmailAsync_NoSections_ReturnsFalseWithoutSending()
    {
        // Arrange
        var emailSender = new Mock<IEmailSender>(MockBehavior.Strict);
        var sut = CreateService(emailSender: emailSender.Object);

        // Act
        var result = await sut.SendTestDailyEmailAsync([], "admin@example.com", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeFalse();
        emailSender.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SendTestDailyEmailAsync_BlankSecret_ReturnsFalseWithoutSending(string secret)
    {
        // Arrange
        var emailSender = new Mock<IEmailSender>(MockBehavior.Strict);
        var sut = CreateService(emailSender: emailSender.Object, unsubscribeSecret: secret);

        // Act
        var result = await sut.SendTestDailyEmailAsync(["ai"], "admin@example.com", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeFalse();
        emailSender.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task SendTestDailyEmailAsync_ValidRequest_SendsTestEmailAndLogsSuccess()
    {
        // Arrange
        const string Recipient = "test-daily-admin@example.com";
        await ClearTestSendLogAsync();
        var sent = new List<SentEmail>();
        var emailSender = CreateRecordingSender(sent);
        var sut = CreateService(emailSender: emailSender.Object);

        // Act
        var result = await sut.SendTestDailyEmailAsync(["ai"], Recipient, TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeTrue();
        var message = sent.Should().ContainSingle().Subject;
        message.Recipient.Should().Be(Recipient);
        message.Subject.Should().MatchRegex(@"^\[Test\] TechHub Daily Overview — \d{4}-\d{2}-\d{2}$");
        NewsletterService.IsValidUnsubscribeToken(Recipient, ExtractToken(message.Text), Secret).Should().BeTrue();

        var log = await GetSingleTestSendLogAsync("test-daily");
        log.Status.Should().Be("sent");
        log.RecipientCount.Should().Be(1);
        log.FailedCount.Should().Be(0);
    }

    [Fact]
    public async Task SendTestDailyEmailAsync_SenderFails_ReturnsFalseAndLogsFailure()
    {
        // Arrange
        await ClearTestSendLogAsync();
        var sent = new List<SentEmail>();
        var emailSender = CreateRecordingSender(sent, result: false);
        var sut = CreateService(emailSender: emailSender.Object);

        // Act
        var result = await sut.SendTestDailyEmailAsync(["ai"], "test-daily-fail@example.com", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeFalse();
        var log = await GetSingleTestSendLogAsync("test-daily");
        await ClearTestSendLogAsync(); // failed_count feeds the admin status report; don't leak it to other tests
        log.Status.Should().Be("failed");
        log.RecipientCount.Should().Be(0);
        log.FailedCount.Should().Be(1);
        log.ErrorMessage.Should().Be("Delivery failed");
    }

    // ── Helpers for the tests above ───────────────────────────────────────

    private const string Secret = "test-secret";
    private const string LatestTestRoundupSlug = "weekly-ai-roundup-test-send-latest";
    private const string LatestTestRoundupTitle = "AI Weekly: Test Send Roundup";

    private sealed record SentEmail(string Recipient, string Subject, string Html, string Text);

    private static Mock<IEmailSender> CreateRecordingSender(List<SentEmail> sent, bool result = true)
    {
        var sender = new Mock<IEmailSender>(MockBehavior.Strict);
        sender
            .Setup(s => s.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<string, string, string, string, CancellationToken>((recipient, subject, html, text, _) =>
                sent.Add(new SentEmail(recipient, subject, html, text)))
            .ReturnsAsync(result);
        return sender;
    }

    private static string ExtractToken(string text)
    {
        const string Marker = "token=";
        var start = text.IndexOf(Marker, StringComparison.Ordinal) + Marker.Length;
        var end = text.IndexOfAny(['\r', '\n', ' ', '&'], start);
        return Uri.UnescapeDataString(end < 0 ? text[start..] : text[start..end]);
    }

    private async Task SeedSubscriberAsync(
        string email,
        bool confirmed,
        string preferencesJson = """{"weeklySections":[],"dailySections":[]}""")
    {
        await _fixture.Connection.ExecuteAsync(
            "DELETE FROM newsletter_subscribers WHERE lower(email) = lower(@Email)",
            new { Email = email });

        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO newsletter_subscribers (email, is_confirmed, confirmed_at, preferences)
            VALUES (@Email, @Confirmed, CASE WHEN @Confirmed THEN NOW() ELSE NULL END, CAST(@Preferences AS jsonb))
            """, new { Email = email, Confirmed = confirmed, Preferences = preferencesJson });
    }

    private async Task<bool?> IsConfirmedAsync(string email) =>
        await _fixture.Connection.ExecuteScalarAsync<bool?>(
            "SELECT is_confirmed FROM newsletter_subscribers WHERE lower(email) = lower(@Email) AND unsubscribed_at IS NULL",
            new { Email = email });

    private async Task ClearTestSendLogAsync() =>
        await _fixture.Connection.ExecuteAsync(
            "DELETE FROM newsletter_send_log WHERE send_kind IN ('test-send', 'test-daily')");

    private async Task<int> CountTestSendLogRowsAsync(string sendKind) =>
        await _fixture.Connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM newsletter_send_log WHERE send_kind = @SendKind",
            new { SendKind = sendKind });

    private async Task<(string Status, int RecipientCount, int FailedCount, string? ErrorMessage)> GetSingleTestSendLogAsync(string sendKind) =>
        await _fixture.Connection.QuerySingleAsync<(string Status, int RecipientCount, int FailedCount, string? ErrorMessage)>(
            """
            SELECT status, recipient_count, failed_count, error_message
            FROM newsletter_send_log
            WHERE send_kind = @SendKind
            """,
            new { SendKind = sendKind });

    // Far-future date_epoch so this roundup is always "the latest" AI roundup for the test-send lookup
    private async Task SeedLatestTestRoundupAsync() =>
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch,
                 primary_section_name, external_url, author, feed_name, tags_csv,
                 sections_bitmask, content_hash, is_ai)
            VALUES
                (@Slug, 'roundups', @Title, '## AI Highlights', 'Test send intro', 4102444800,
                 'ai', '/ai/roundups/' || @Slug, 'TechHub', 'TechHub', ',Roundups,AI,',
                 1, 'hash-newsletter-test-send-latest', TRUE)
            ON CONFLICT (collection_name, slug) DO NOTHING
            """, new { Slug = LatestTestRoundupSlug, Title = LatestTestRoundupTitle });

    private NewsletterService CreateService(
        IContentRepository? contentRepository = null,
        IEmailSender? emailSender = null,
        string unsubscribeSecret = "test-secret",
        string adminReportEmailAddress = "")
    {
        var options = Options.Create(new NewsletterOptions
        {
            Endpoint = "https://invalid.communication.azure.com/",
            SenderAddress = "DoNotReply@example.azurecomm.net",
            WebsiteBaseUrl = "https://tech.hub.ms",
            UnsubscribeSecret = unsubscribeSecret,
            AdminReportEmailAddress = adminReportEmailAddress
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

        var env = new Mock<IHostEnvironment>();
        env.Setup(e => e.EnvironmentName).Returns(Environments.Production);

        return new NewsletterService(
            _fixture.Connection,
            new NewsletterSubscriberRepository(_fixture.Connection),
            contentRepository,
            options,
            appSettings,
            emailSender,
            NullLogger<NewsletterService>.Instance,
            new NewsletterTemplateProvider(env.Object));
    }

    private static Section CreateSection(string name) =>
        CreateSection(name, name.Equals("dotnet", StringComparison.OrdinalIgnoreCase) ? ".NET" : "AI");

    private static Section CreateSection(string name, string title) =>
        new(
            name,
            title,
            $"{name} section",
            $"/{name}",
            title,
            [new Collection("blogs", "Blogs", $"/{name}/blogs", "Blogs", "Blogs")]);

    private async Task CleanupDailyOverviewTestDataAsync()
    {
        await _fixture.Connection.ExecuteAsync("""
            DELETE FROM newsletter_send_log
            WHERE send_kind = 'daily-overview'
              AND target_key IN ('2026-05-20', '2026-05-21', '2026-05-22', '2026-05-25', '2026-05-26');

            DELETE FROM newsletter_subscribers
            WHERE email IN ('confirmed@example.com', 'unconfirmed@example.com', 'confirmed-daily@example.com', 'daily-ordering@example.com', 'daily-no-items@example.com', 'daily-link-test@example.com');

            DELETE FROM content_items
            WHERE collection_name = 'blogs'
              AND slug IN ('daily-item-newsletter-test-2026-05-20', 'daily-item-newsletter-test-2026-05-21', 'daily-order-ai-2026-05-22', 'daily-order-dotnet-2026-05-22', 'daily-item-newsletter-link-test-2026-05-26');
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
