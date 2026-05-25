using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Infrastructure.Repositories;
using TechHub.Infrastructure.Services.Newsletter;
using Azure.Communication.Email;

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

        var sut = CreateService();

        var sent = await sut.SendRoundupNewsletterAsync("weekly-ai-roundup-2026-05-18", TestContext.Current.CancellationToken);

        sent.Should().BeFalse();
        var status = await _fixture.Connection.ExecuteScalarAsync<string?>(
            "SELECT status FROM newsletter_send_log WHERE send_kind = 'weekly-roundup' AND target_key = 'weekly-ai-roundup-2026-05-18'");
        status.Should().Be("failed");
    }

    private NewsletterService CreateService()
    {
        var options = Options.Create(new NewsletterOptions
        {
            ConnectionString = "endpoint=https://invalid.communication.azure.com/;accesskey=invalid",
            SenderAddress = "DoNotReply@example.azurecomm.net",
            WebsiteBaseUrl = "https://tech.hub.ms",
            UnsubscribeSecret = "test-secret"
        });

        var contentRepository = new Mock<IContentRepository>(MockBehavior.Loose).Object;
        return new NewsletterService(
            _fixture.Connection,
            new NewsletterSubscriberRepository(_fixture.Connection),
            contentRepository,
            options,
            new EmailClient(options.Value.ConnectionString),
            NullLogger<NewsletterService>.Instance);
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
