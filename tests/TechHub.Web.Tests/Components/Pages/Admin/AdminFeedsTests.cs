using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Web.Components.Pages.Admin;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components.Pages.Admin;

/// <summary>
/// bUnit tests for AdminFeeds.razor — feed list, add/edit modal and delete flows.
/// </summary>
public class AdminFeedsTests : AdminPageTestBase
{
    private readonly Mock<ITechHubApiClient> _api = new();

    public AdminFeedsTests()
    {
        Services.AddSingleton(_api.Object);
        SetupFeeds([]);
    }

    [Fact]
    public void Sorting_Default_PreservesFeedOrder()
    {
        // Arrange
        SetupFeeds([CreateFeed(10, "Zulu"), CreateFeed(2, "Alpha")]);

        // Act
        var cut = Render<AdminFeeds>();

        // Assert
        cut.FindAll("tbody tr td:first-child").Select(cell => cell.TextContent.Trim()).Should().Equal("10", "2");
        cut.FindAll("thead button").Should().HaveCount(8);
        cut.FindAll("th[aria-sort]").Should().BeEmpty();
    }

    [Theory]
    [InlineData("ID", "2", "10")]
    [InlineData("Name", "10", "2")]
    [InlineData("URL", "2", "10")]
    [InlineData("Output Dir", "2", "10")]
    [InlineData("Enabled", "2", "10")]
    [InlineData("Polling interval", "10", "2")]
    [InlineData("Last content", "10", "2")]
    [InlineData("Last check", "10", "2")]
    public void Sorting_ColumnClick_TogglesDirectionWithoutApiCalls(string column, string first, string second)
    {
        // Arrange
        var earlier = new DateTimeOffset(2026, 11, 1, 12, 0, 0, TimeSpan.Zero);
        var a = CreateFeed(10, "alpha", url: "https://z.example/feed", outputDir: "_news");
        a.PollingIntervalMinutes = 15;
        a.LastContentAt = earlier;
        a.LastCheckedAt = earlier;
        var b = CreateFeed(2, "Zulu", enabled: false, url: "https://a.example/feed", outputDir: "_blogs");
        b.LastContentAt = earlier.AddDays(1);
        b.LastCheckedAt = earlier.AddDays(1);
        SetupFeeds([b, a]);
        var cut = Render<AdminFeeds>();
        // Act
        cut.FindAll("thead button").Single(b => b.TextContent.Trim() == column).Click();

        // Assert
        cut.FindAll("tbody tr td:first-child").Select(cell => cell.TextContent.Trim()).Should().Equal(first, second);
        cut.Find("th[aria-sort]").GetAttribute("aria-sort").Should().Be("ascending");
        cut.FindAll("thead button").Single(b => b.TextContent.Contains(column)).Click();
        cut.FindAll("tbody tr td:first-child").Select(cell => cell.TextContent.Trim()).Should().Equal(second, first);
        cut.Find("th[aria-sort]").GetAttribute("aria-sort").Should().Be("descending");
        _api.Verify(x => x.GetFeedConfigsAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData("Last content")]
    [InlineData("Last check")]
    public void Sorting_MissingDates_SortsBeforeKnownDatesAscending(string column)
    {
        // Arrange
        var known = CreateFeed(2, "Known");
        known.LastContentAt = DateTimeOffset.UtcNow;
        known.LastCheckedAt = DateTimeOffset.UtcNow;
        SetupFeeds([known, CreateFeed(1, "Never")]);
        var cut = Render<AdminFeeds>();

        // Act
        cut.FindAll("thead button").Single(b => b.TextContent.Trim() == column).Click();

        // Assert
        cut.FindAll("tbody tr td:first-child").Select(cell => cell.TextContent.Trim()).Should().Equal("1", "2");
    }

    [Fact]
    public void Sorting_Refresh_PreservesSelectionAndUsesUpdatedData()
    {
        // Arrange
        SetupFeeds([CreateFeed(1, "Zulu"), CreateFeed(2, "Alpha")]);
        var cut = Render<AdminFeeds>();
        cut.FindAll("thead button").Single(b => b.TextContent.Trim() == "Name").Click();
        cut.FindAll("thead button").Single(b => b.TextContent.Contains("Name")).Click();
        SetupFeeds([CreateFeed(3, "Beta"), CreateFeed(4, "Gamma")]);

        // Act
        cut.Find(".admin-actions .btn-secondary").Click();

        // Assert
        cut.FindAll("tbody tr td:first-child").Select(cell => cell.TextContent.Trim()).Should().Equal("4", "3");
        cut.Find("th[aria-sort]").GetAttribute("aria-sort").Should().Be("descending");
        cut.FindAll("thead button").Single(b => b.TextContent.Trim() == "ID").Click();
        cut.Find("th[aria-sort]").GetAttribute("aria-sort").Should().Be("ascending");
    }

    [Fact]
    public void PollingInterval_Edit_PreservesAndSavesInterval()
    {
        // Arrange
        var feed = CreateFeed(5, "Aggregate");
        feed.PollingIntervalMinutes = 15;
        SetupFeeds([feed]);
        _api.Setup(x => x.UpdateFeedConfigAsync(It.IsAny<FeedConfig>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(feed);
        var cut = Render<AdminFeeds>();
        cut.Find(".feed-polling-interval").TextContent.Should().Contain("15 min");
        cut.Find("tbody .admin-link-btn").Click();
        cut.Find("#feedPollingInterval").GetAttribute("value").Should().Be("15");

        // Act
        cut.Find("#feedPollingInterval").Change("60");
        cut.Find(".admin-modal-footer .btn").Click();

        // Assert
        _api.Verify(x => x.UpdateFeedConfigAsync(
            It.Is<FeedConfig>(f => f.PollingIntervalMinutes == 60), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void PollingInterval_InvalidValue_ShowsErrorWithoutSaving()
    {
        // Arrange
        var cut = Render<AdminFeeds>();
        cut.Find(".admin-actions .btn").Click();
        cut.Find("#feedPollingInterval").GetAttribute("value").Should().Be("60");

        // Act
        cut.Find("#feedPollingInterval").Change("0");
        cut.Find(".admin-modal-footer .btn").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Polling interval must be at least 1 minute.");
        cut.FindAll(".admin-modal").Should().ContainSingle();
        _api.Verify(x => x.CreateFeedConfigAsync(It.IsAny<FeedConfig>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Render_WithFeeds_ShowsOneRowPerFeed()
    {
        // Arrange
        SetupFeeds([CreateFeed(1, "GitHub Blog"), CreateFeed(2, "Azure Blog", enabled: false)]);

        // Act
        var cut = Render<AdminFeeds>();

        // Assert
        var rows = cut.FindAll("tbody tr");
        rows.Should().HaveCount(2);
        rows[0].TextContent.Should().Contain("GitHub Blog").And.Contain("✓ Yes");
        rows[1].TextContent.Should().Contain("Azure Blog").And.Contain("✗ No");
        rows[1].QuerySelector(".admin-status")!.ClassList.Should().Contain("admin-status-failed");
    }

    [Fact]
    public void Render_FeedNeverChecked_ShowsDashesForLastContentAndCheck()
    {
        // Arrange
        SetupFeeds([CreateFeed(1, "GitHub Blog")]);

        // Act
        var cut = Render<AdminFeeds>();

        // Assert
        cut.Find("td.feed-last-content").TextContent.Trim().Should().Be("—");
        cut.Find("td.feed-last-check").TextContent.Trim().Should().Be("—");
        cut.FindAll("td.feed-last-check .admin-status").Should().BeEmpty();
    }

    [Fact]
    public void Render_FeedChecked_ShowsOkStatusAndCheckTime()
    {
        // Arrange
        var feed = CreateFeed(1, "GitHub Blog");
        feed.LastCheckedAt = new DateTimeOffset(2026, 11, 5, 8, 0, 0, TimeSpan.Zero);
        SetupFeeds([feed]);

        // Act
        var cut = Render<AdminFeeds>();

        // Assert
        var cell = cut.Find("td.feed-last-check");
        cell.QuerySelector(".admin-status")!.ClassList.Should().Contain("admin-status-completed");
        cell.TextContent.Should().Contain("✓ OK").And.Contain("2026-11-05 09:00:00");
    }

    [Fact]
    public void Render_FeedWithFailedCheck_ShowsTheErrorAsFailed()
    {
        // Arrange
        var feed = CreateFeed(1, "Broken Blog");
        feed.LastCheckedAt = new DateTimeOffset(2026, 11, 5, 8, 0, 0, TimeSpan.Zero);
        feed.LastCheckError = "Failed to download feed: HTTP 404 (NotFound)";
        SetupFeeds([feed]);

        // Act
        var cut = Render<AdminFeeds>();

        // Assert
        var status = cut.Find("td.feed-last-check .admin-status");
        status.ClassList.Should().Contain("admin-status-failed");
        status.TextContent.Trim().Should().Be("Failed to download feed: HTTP 404 (NotFound)");
        status.GetAttribute("title").Should().Be("Failed to download feed: HTTP 404 (NotFound)");
    }

    [Fact]
    public void Render_FeedWithContent_ShowsWhenContentWasLastAdded()
    {
        // Arrange
        var feed = CreateFeed(1, "GitHub Blog");
        feed.LastContentAt = DateTimeOffset.UtcNow.AddDays(-3);
        SetupFeeds([feed]);

        // Act
        var cut = Render<AdminFeeds>();

        // Assert
        var cell = cut.Find("td.feed-last-content");
        cell.TextContent.Trim().Should().Be("3 days ago");
        cell.GetAttribute("title").Should().MatchRegex(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$");
    }

    [Fact]
    public void Render_NoFeeds_ShowsEmptyMessage()
    {
        // Act
        var cut = Render<AdminFeeds>();

        // Assert
        cut.Find(".admin-empty").TextContent.Should().Contain("No RSS feeds configured yet.");
    }

    [Fact]
    public void Render_ApiFails_ShowsErrorAlert()
    {
        // Arrange
        _api.Setup(x => x.GetFeedConfigsAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new HttpRequestException("down"));

        // Act
        var cut = Render<AdminFeeds>();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to load feeds: down");
    }

    [Fact]
    public void AddFeed_Click_OpensEmptyAddFormWithEnabledChecked()
    {
        // Arrange
        var cut = Render<AdminFeeds>();

        // Act
        cut.Find(".admin-actions .btn").Click();

        // Assert
        cut.Find("#feed-editor-title").TextContent.Should().Be("Add Feed");
        cut.Find("#feedName").GetAttribute("value").Should().BeNullOrEmpty();
        cut.Find(".admin-modal input[type='checkbox']").HasAttribute("checked").Should().BeTrue();
    }

    [Fact]
    public void AddFeed_Save_CreatesFeedAndShowsSuccess()
    {
        // Arrange
        _api.Setup(x => x.CreateFeedConfigAsync(It.IsAny<FeedConfig>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateFeed(10, "New Feed"));
        var cut = Render<AdminFeeds>();
        cut.Find(".admin-actions .btn").Click();
        cut.Find("#feedName").Change("New Feed");
        cut.Find("#feedUrl").Change("https://example.com/feed");
        cut.Find("#feedOutputDir").Change("_news");

        // Act
        cut.Find(".admin-modal-footer .btn").Click();

        // Assert
        _api.Verify(
            x => x.CreateFeedConfigAsync(
                It.Is<FeedConfig>(f => f.Name == "New Feed" && f.Url == "https://example.com/feed" && f.OutputDir == "_news" && f.Enabled && f.Id == 0),
                It.IsAny<CancellationToken>()),
            Times.Once);
        _api.Verify(x => x.UpdateFeedConfigAsync(It.IsAny<FeedConfig>(), It.IsAny<CancellationToken>()), Times.Never);
        cut.Find(".admin-alert-success").TextContent.Should().Contain("Feed 'New Feed' created successfully.");
        cut.FindAll(".admin-modal").Should().BeEmpty();
        _api.Verify(x => x.GetFeedConfigsAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public void AddFeed_SaveFails_KeepsFormOpenAndShowsError()
    {
        // Arrange
        _api.Setup(x => x.CreateFeedConfigAsync(It.IsAny<FeedConfig>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("Response status code does not indicate success: 409 (Conflict)."));
        var cut = Render<AdminFeeds>();
        cut.Find(".admin-actions .btn").Click();
        cut.Find("#feedName").Change("Dup");

        // Act
        cut.Find(".admin-modal-footer .btn").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to save feed:").And.Contain("409");
        cut.FindAll(".admin-modal").Should().ContainSingle();
        cut.Find("#feedName").GetAttribute("value").Should().Be("Dup");
    }

    [Fact]
    public void EditFeed_Click_OpensFormPrefilled()
    {
        // Arrange
        SetupFeeds([CreateFeed(5, "GitHub Blog", url: "https://github.blog/feed/", outputDir: "_blogs")]);
        var cut = Render<AdminFeeds>();

        // Act
        cut.Find("tbody .admin-link-btn").Click();

        // Assert
        cut.Find("#feed-editor-title").TextContent.Should().Be("Edit Feed");
        cut.Find("#feedName").GetAttribute("value").Should().Be("GitHub Blog");
        cut.Find("#feedUrl").GetAttribute("value").Should().Be("https://github.blog/feed/");
        cut.Find("#feedOutputDir").GetAttribute("value").Should().Be("_blogs");
    }

    [Fact]
    public void EditFeed_Save_UpdatesExistingFeedById()
    {
        // Arrange
        SetupFeeds([CreateFeed(5, "GitHub Blog")]);
        _api.Setup(x => x.UpdateFeedConfigAsync(It.IsAny<FeedConfig>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateFeed(5, "Renamed"));
        var cut = Render<AdminFeeds>();
        cut.Find("tbody .admin-link-btn").Click();
        cut.Find("#feedName").Change("Renamed");
        cut.Find(".admin-modal input[type='checkbox']").Change(false);

        // Act
        cut.Find(".admin-modal-footer .btn").Click();

        // Assert
        _api.Verify(
            x => x.UpdateFeedConfigAsync(It.Is<FeedConfig>(f => f.Id == 5 && f.Name == "Renamed" && !f.Enabled), It.IsAny<CancellationToken>()),
            Times.Once);
        _api.Verify(x => x.CreateFeedConfigAsync(It.IsAny<FeedConfig>(), It.IsAny<CancellationToken>()), Times.Never);
        cut.Find(".admin-alert-success").TextContent.Should().Contain("Feed 'Renamed' updated successfully.");
    }

    [Fact]
    public void EditFeed_CancelAfterChanges_DoesNotModifyListedFeed()
    {
        // Arrange
        SetupFeeds([CreateFeed(5, "GitHub Blog")]);
        var cut = Render<AdminFeeds>();
        cut.Find("tbody .admin-link-btn").Click();
        cut.Find("#feedName").Change("Changed but cancelled");

        // Act
        cut.Find(".admin-modal-footer .btn-secondary").Click();

        // Assert
        cut.FindAll(".admin-modal").Should().BeEmpty();
        cut.Find("tbody tr").TextContent.Should().Contain("GitHub Blog");
        _api.Verify(x => x.UpdateFeedConfigAsync(It.IsAny<FeedConfig>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Form_EscapeKey_ClosesModal()
    {
        // Arrange
        var cut = Render<AdminFeeds>();
        cut.Find(".admin-actions .btn").Click();

        // Act
        cut.Find(".admin-modal-backdrop").TriggerEvent("onkeydown", new KeyboardEventArgs { Key = "Escape" });

        // Assert
        cut.FindAll(".admin-modal").Should().BeEmpty();
    }

    [Fact]
    public void Form_OtherKey_KeepsModalOpen()
    {
        // Arrange
        var cut = Render<AdminFeeds>();
        cut.Find(".admin-actions .btn").Click();

        // Act
        cut.Find(".admin-modal-backdrop").TriggerEvent("onkeydown", new KeyboardEventArgs { Key = "a" });

        // Assert
        cut.FindAll(".admin-modal").Should().ContainSingle();
    }

    [Fact]
    public void Delete_Click_DeletesFeedAndReloads()
    {
        // Arrange
        SetupFeeds([CreateFeed(5, "GitHub Blog")]);
        var cut = Render<AdminFeeds>();

        // Act
        cut.Find("tbody .admin-link-btn-danger").Click();

        // Assert
        _api.Verify(x => x.DeleteFeedConfigAsync(5, It.IsAny<CancellationToken>()), Times.Once);
        _api.Verify(x => x.GetFeedConfigsAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        cut.Find(".admin-alert-success").TextContent.Should().Contain("Feed 'GitHub Blog' deleted.");
    }

    [Fact]
    public void Delete_HiddenForFeedsWithContent()
    {
        // Arrange
        SetupFeeds([
            CreateFeed(5, "Has Content", hasContent: true),
            CreateFeed(6, "Empty Feed")]);

        // Act
        var cut = Render<AdminFeeds>();

        // Assert
        var rows = cut.FindAll("tbody tr");
        rows[0].QuerySelector(".admin-link-btn-danger").Should().BeNull();
        rows[1].QuerySelector(".admin-link-btn-danger").Should().NotBeNull();
    }

    [Fact]
    public void Delete_Fails_ShowsErrorAndDoesNotReload()
    {
        // Arrange
        SetupFeeds([CreateFeed(5, "GitHub Blog")]);
        _api.Setup(x => x.DeleteFeedConfigAsync(5, It.IsAny<CancellationToken>())).ThrowsAsync(new HttpRequestException("nope"));
        var cut = Render<AdminFeeds>();

        // Act
        cut.Find("tbody .admin-link-btn-danger").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to delete feed: nope");
        _api.Verify(x => x.GetFeedConfigsAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Refresh_Click_ReloadsFeeds()
    {
        // Arrange
        var cut = Render<AdminFeeds>();

        // Act
        cut.Find(".admin-actions .btn-secondary").Click();

        // Assert
        _api.Verify(x => x.GetFeedConfigsAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    private static FeedConfig CreateFeed(long id, string name, bool enabled = true, string url = "https://example.com/feed", string outputDir = "_news", bool hasContent = false) =>
        new() { Id = id, Name = name, Url = url, OutputDir = outputDir, Enabled = enabled, HasContent = hasContent };

    private void SetupFeeds(IReadOnlyList<FeedConfig> feeds) =>
        _api.Setup(x => x.GetFeedConfigsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(feeds);
}
