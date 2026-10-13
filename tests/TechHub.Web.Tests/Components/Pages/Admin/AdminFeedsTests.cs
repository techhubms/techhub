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

    private static FeedConfig CreateFeed(long id, string name, bool enabled = true, string url = "https://example.com/feed", string outputDir = "_news") =>
        new() { Id = id, Name = name, Url = url, OutputDir = outputDir, Enabled = enabled };

    private void SetupFeeds(IReadOnlyList<FeedConfig> feeds) =>
        _api.Setup(x => x.GetFeedConfigsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(feeds);
}
