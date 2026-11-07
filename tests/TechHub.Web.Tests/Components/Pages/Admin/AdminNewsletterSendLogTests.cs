using AngleSharp.Dom;
using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechHub.Core.Models.Admin;
using TechHub.Web.Components.Pages.Admin;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components.Pages.Admin;

/// <summary>
/// bUnit tests for AdminNewsletterSendLog.razor — the send log of the newsletter.
/// </summary>
public class AdminNewsletterSendLogTests : AdminPageTestBase
{
    private readonly Mock<ITechHubApiClient> _api = new();

    public AdminNewsletterSendLogTests()
    {
        Services.AddSingleton(_api.Object);
        SetupLog([]);
    }

    [Fact]
    public void Render_RequestsTheLastHundredEntries()
    {
        // Act
        Render<AdminNewsletterSendLog>();

        // Assert
        _api.Verify(x => x.GetNewsletterSendLogAsync(100, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Render_WithSendLog_ShowsOneRowPerEntryWithFormattedValues()
    {
        // Arrange
        SetupLog(
        [
            new NewsletterSendLogEntry
            {
                Id = 1,
                SendKind = "weekly",
                TargetKey = "2026-W28",
                SentAt = new DateTimeOffset(2026, 7, 15, 15, 30, 45, TimeSpan.Zero),
                RecipientCount = 120,
                FailedCount = 3,
                Status = "partial"
            },
            new NewsletterSendLogEntry
            {
                Id = 2,
                SendKind = "daily",
                TargetKey = "2026-07-14",
                SentAt = new DateTimeOffset(2026, 7, 14, 6, 0, 0, TimeSpan.Zero),
                RecipientCount = 5,
                FailedCount = 0,
                Status = "ok"
            }
        ]);

        // Act
        var cut = Render<AdminNewsletterSendLog>();

        // Assert
        var rows = Rows(cut);
        rows.Should().HaveCount(2);
        rows[0].QuerySelectorAll("td").Select(c => c.TextContent)
            .Should().Equal("2026-07-15 17:30:45", "weekly", "2026-W28", "120", "3", "partial");
        rows[1].QuerySelectorAll("td")[4].TextContent.Should().Be("—", "no failures are shown as a dash");
    }

    [Fact]
    public void Render_EmptySendLog_ShowsEmptyMessageAndNoRows()
    {
        // Act
        var cut = Render<AdminNewsletterSendLog>();

        // Assert
        cut.Find(".admin-empty").TextContent.Should().Contain("Nothing has been sent yet.");
        Rows(cut).Should().BeEmpty();
    }

    [Fact]
    public void Render_ApiFails_ShowsGenericError()
    {
        // Arrange
        _api.Setup(x => x.GetNewsletterSendLogAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("down"));

        // Act
        var cut = Render<AdminNewsletterSendLog>();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Be("Failed to load the send log.");
        Rows(cut).Should().BeEmpty();
    }

    [Fact]
    public void Refresh_Click_ReloadsTheLogAndClearsEarlierError()
    {
        // Arrange
        _api.Setup(x => x.GetNewsletterSendLogAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("down"));
        var cut = Render<AdminNewsletterSendLog>();
        cut.FindAll(".admin-alert-error").Should().ContainSingle();
        SetupLog([new NewsletterSendLogEntry { Id = 1, SendKind = "daily", TargetKey = "2026-07-14", SentAt = DateTimeOffset.UtcNow, Status = "ok" }]);

        // Act
        cut.Find(".admin-actions button").Click();

        // Assert
        cut.FindAll(".admin-alert").Should().BeEmpty();
        Rows(cut).Should().ContainSingle();
    }

    private static List<IElement> Rows(IRenderedComponent<AdminNewsletterSendLog> cut) =>
        cut.FindAll(".admin-table tbody tr").ToList();

    private void SetupLog(IReadOnlyList<NewsletterSendLogEntry> log) =>
        _api.Setup(x => x.GetNewsletterSendLogAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(log);
}
