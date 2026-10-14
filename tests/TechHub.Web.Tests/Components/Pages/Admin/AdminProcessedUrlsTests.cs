using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechHub.Core.Models;
using TechHub.Core.Models.Admin;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Web.Components;
using TechHub.Web.Components.Pages.Admin;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components.Pages.Admin;

/// <summary>
/// bUnit tests for AdminProcessedUrls.razor — listing, query-string restore, pagination and removal flows.
/// Debounced filter inputs are intentionally not covered here (they rely on a wall-clock delay).
/// </summary>
public class AdminProcessedUrlsTests : AdminPageTestBase
{
    private readonly Mock<ITechHubApiClient> _api = new();

    public AdminProcessedUrlsTests()
    {
        ComponentFactories.AddStub<ContentItemEditorModal>();
        Services.AddSingleton(_api.Object);
        _api.Setup(x => x.GetFeedConfigsAsync(It.IsAny<CancellationToken>())).ReturnsAsync([]);
        SetupResult([]);
    }

    // ── Rendering ───────────────────────────────────────────────────────────

    [Fact]
    public void Render_WithItems_ShowsRowsWithStatusLabelsAndFallbacks()
    {
        // Arrange
        SetupResult(
        [
            CreateItem("https://example.com/blog/2026/my-long-article-title", "succeeded", feed: "Blog", collection: "blogs", slug: "my-long-article-title"),
            CreateItem("https://example.com/skipped", "skipped", reason: "Not relevant"),
            CreateItem("https://example.com/failed", "failed", error: "Timeout"),
        ]);

        // Act
        var cut = Render<AdminProcessedUrls>();

        // Assert
        var rows = cut.FindAll("tbody tr");
        rows.Should().HaveCount(3);
        rows[0].TextContent.Should().Contain("✓ Added").And.Contain("Blog").And.Contain("blogs");
        rows[0].QuerySelector(".admin-cell-article a")!.TextContent.Should().Be("my-long-article-title");
        rows[0].QuerySelector(".admin-cell-article a")!.GetAttribute("href").Should().Be("https://example.com/blog/2026/my-long-article-title");
        rows[1].TextContent.Should().Contain("⊘ Skipped").And.Contain("Not relevant");
        rows[1].QuerySelector(".admin-cell-feed")!.TextContent.Should().Be("—");
        rows[2].TextContent.Should().Contain("✗ Failed").And.Contain("Timeout");
        rows[2].QuerySelector(".admin-status")!.ClassList.Should().Contain("admin-status-failed");
    }

    [Fact]
    public void Render_NoItems_ShowsEmptyMessage()
    {
        // Act
        var cut = Render<AdminProcessedUrls>();

        // Assert
        cut.Find(".admin-empty").TextContent.Should().Contain("No processed URLs match the current filters.");
    }

    [Fact]
    public void Render_ApiFails_ShowsErrorAlert()
    {
        // Arrange
        _api.Setup(x => x.GetProcessedUrlsAsync(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(),
                It.IsAny<string?>(), It.IsAny<long?>(), It.IsAny<string?>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("API down"));

        // Act
        var cut = Render<AdminProcessedUrls>();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to load processed URLs: API down");
    }

    [Fact]
    public void Render_LongReason_IsTruncatedWithEllipsis()
    {
        // Arrange
        SetupResult([CreateItem("https://example.com/a", "skipped", reason: new string('r', 200))]);

        // Act
        var cut = Render<AdminProcessedUrls>();

        // Assert
        cut.Find(".admin-cell-reason").TextContent.Trim().Should().HaveLength(120).And.EndWith("…");
    }

    [Fact]
    public void Render_ItemWithoutTranscript_ShowsWarningMarker()
    {
        // Arrange
        SetupResult([CreateItem("https://youtube.com/watch?v=abc", "succeeded", hasTranscript: false)]);

        // Act
        var cut = Render<AdminProcessedUrls>();

        // Assert
        cut.Find(".admin-transcript-warning").GetAttribute("title").Should().Be("Transcript not fetched");
    }

    [Fact]
    public void Render_FeedNamesFromApi_AreSortedInFeedDropdown()
    {
        // Arrange
        _api.Setup(x => x.GetFeedConfigsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([new FeedConfig { Name = "Zeta" }, new FeedConfig { Name = "Alpha" }]);

        // Act
        var cut = Render<AdminProcessedUrls>();

        // Assert
        var options = cut.FindAll("#feedFilter option").Select(o => o.TextContent).ToList();
        options.Should().Equal("All feeds", "Alpha", "Zeta");
    }

    // ── Query string restore ────────────────────────────────────────────────

    [Fact]
    public void Render_QueryStringFilters_AreForwardedToApi()
    {
        // Arrange
        Services.GetRequiredService<NavigationManager>().NavigateTo(
            "/admin/processed-urls?status=failed&q=copilot&feed=Blog&section=ai&primaryOnly=true&collection=news&jobId=26&page=2");

        // Act
        var cut = Render<AdminProcessedUrls>();

        // Assert
        _api.Verify(
            x => x.GetProcessedUrlsAsync(2, 100, "failed", "copilot", "Blog", "news", 26L, "ai", true, It.IsAny<CancellationToken>()),
            Times.Once);
        cut.Find("#searchFilter").GetAttribute("value").Should().Be("copilot");
        cut.Find("#jobIdFilter").GetAttribute("value").Should().Be("26");
        cut.Find(".admin-checkbox-label input[type='checkbox']").HasAttribute("checked").Should().BeTrue();
    }

    [Fact]
    public void Render_NoQueryString_UsesUnfilteredFirstPage()
    {
        // Act
        Render<AdminProcessedUrls>();

        // Assert
        _api.Verify(
            x => x.GetProcessedUrlsAsync(1, 100, null, null, null, null, null, null, false, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void Render_NoSectionFilter_HidesPrimarySectionCheckbox()
    {
        // Act
        var cut = Render<AdminProcessedUrls>();

        // Assert
        cut.FindAll(".admin-checkbox-label").Should().BeEmpty();
    }

    // ── Pagination ──────────────────────────────────────────────────────────

    [Fact]
    public void Render_MultiplePages_ShowsSummaryAndPagination()
    {
        // Arrange
        SetupResult([CreateItem("https://example.com/a", "succeeded")], total: 250);

        // Act
        var cut = Render<AdminProcessedUrls>();

        // Assert
        cut.Find(".admin-result-summary").TextContent.Should().Contain("Showing 1–100").And.Contain("of 250 entries");
        cut.Find(".admin-pagination-info").TextContent.Should().Be("Page 1 of 3");
        cut.FindAll(".admin-pagination .btn-secondary")[0].HasAttribute("disabled").Should().BeTrue();
        cut.FindAll(".admin-pagination .btn-secondary")[1].HasAttribute("disabled").Should().BeFalse();
    }

    [Fact]
    public void Render_SinglePage_HidesPagination()
    {
        // Arrange
        SetupResult([CreateItem("https://example.com/a", "succeeded")], total: 1);

        // Act
        var cut = Render<AdminProcessedUrls>();

        // Assert
        cut.FindAll(".admin-pagination").Should().BeEmpty();
    }

    [Fact]
    public void NextPage_Click_LoadsSecondPage()
    {
        // Arrange
        SetupResult([CreateItem("https://example.com/a", "succeeded")], total: 250);
        var cut = Render<AdminProcessedUrls>();

        // Act
        cut.FindAll(".admin-pagination .btn-secondary")[1].Click();

        // Assert
        _api.Verify(
            x => x.GetProcessedUrlsAsync(2, 100, null, null, null, null, null, null, false, It.IsAny<CancellationToken>()),
            Times.Once);
        cut.Find(".admin-pagination-info").TextContent.Should().Be("Page 2 of 3");
    }

    [Fact]
    public void PreviousPage_Click_GoesBackOnePage()
    {
        // Arrange
        SetupResult([CreateItem("https://example.com/a", "succeeded")], total: 250);
        Services.GetRequiredService<NavigationManager>().NavigateTo("/admin/processed-urls?page=3");
        var cut = Render<AdminProcessedUrls>();

        // Act
        cut.FindAll(".admin-pagination .btn-secondary")[0].Click();

        // Assert
        cut.Find(".admin-pagination-info").TextContent.Should().Be("Page 2 of 3");
        cut.FindAll(".admin-pagination .btn-secondary")[1].HasAttribute("disabled").Should().BeFalse();
    }

    // ── Remove ──────────────────────────────────────────────────────────────

    [Fact]
    public void Remove_Success_ShowsMessageAndReloads()
    {
        // Arrange
        SetupResult([CreateItem("https://example.com/a", "failed")]);
        _api.Setup(x => x.DeleteProcessedUrlAsync("https://example.com/a", It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var cut = Render<AdminProcessedUrls>();

        // Act
        cut.Find("tbody .admin-link-btn-danger").Click();

        // Assert
        cut.Find(".admin-alert-success").TextContent.Should().Contain("Removed: https://example.com/a — will be retried on next run.");
        _api.Verify(
            x => x.GetProcessedUrlsAsync(1, 100, null, null, null, null, null, null, false, It.IsAny<CancellationToken>()),
            Times.Exactly(2));
    }

    [Fact]
    public void Remove_UrlNotFound_ShowsErrorAndDoesNotReload()
    {
        // Arrange
        SetupResult([CreateItem("https://example.com/a", "failed")]);
        _api.Setup(x => x.DeleteProcessedUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var cut = Render<AdminProcessedUrls>();

        // Act
        cut.Find("tbody .admin-link-btn-danger").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("URL not found (may have already been removed).");
        _api.Verify(
            x => x.GetProcessedUrlsAsync(1, 100, null, null, null, null, null, null, false, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void Remove_Throws_ShowsFailureMessage()
    {
        // Arrange
        SetupResult([CreateItem("https://example.com/a", "failed")]);
        _api.Setup(x => x.DeleteProcessedUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ThrowsAsync(new HttpRequestException("boom"));
        var cut = Render<AdminProcessedUrls>();

        // Act
        cut.Find("tbody .admin-link-btn-danger").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to remove URL: boom");
    }

    [Fact]
    public void RemoveAllFailed_ButtonShowsFailedCountAndDeletes()
    {
        // Arrange
        SetupResult([CreateItem("https://example.com/a", "failed")], failedCount: 4);
        _api.Setup(x => x.DeleteAllFailedProcessedUrlsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(4);
        var cut = Render<AdminProcessedUrls>();
        cut.Find(".admin-actions .btn-danger").TextContent.Should().Contain("Remove All Failed (4)");

        // Act
        cut.Find(".admin-actions .btn-danger").Click();

        // Assert
        _api.Verify(x => x.DeleteAllFailedProcessedUrlsAsync(It.IsAny<CancellationToken>()), Times.Once);
        cut.Find(".admin-alert-success").TextContent.Should().Contain("Removed 4 failed entries");
    }

    [Fact]
    public void RemoveAllFailed_NoFailedUrls_ButtonIsHidden()
    {
        // Act
        var cut = Render<AdminProcessedUrls>();

        // Assert
        cut.FindAll(".admin-actions .btn-danger").Should().BeEmpty();
    }

    [Fact]
    public void RemoveAllFailed_Throws_ShowsFailureMessage()
    {
        // Arrange
        SetupResult([CreateItem("https://example.com/a", "failed")], failedCount: 2);
        _api.Setup(x => x.DeleteAllFailedProcessedUrlsAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new HttpRequestException("boom"));
        var cut = Render<AdminProcessedUrls>();

        // Act
        cut.Find(".admin-actions .btn-danger").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to remove entries: boom");
    }

    // ── Content item editor ─────────────────────────────────────────────────

    [Fact]
    public void Edit_OnlyOfferedForSucceededItemsWithCollectionAndSlug()
    {
        // Arrange
        SetupResult(
        [
            CreateItem("https://example.com/a", "succeeded", collection: "blogs", slug: "a"),
            CreateItem("https://example.com/b", "failed", collection: "blogs", slug: "b"),
            CreateItem("https://example.com/c", "succeeded", collection: "blogs", slug: null),
        ]);

        // Act
        var cut = Render<AdminProcessedUrls>();

        // Assert
        cut.FindAll("button[title='Edit content item']").Should().ContainSingle();
    }

    [Fact]
    public void Edit_ItemNotFound_ShowsErrorMessage()
    {
        // Arrange
        SetupResult([CreateItem("https://example.com/a", "succeeded", collection: "blogs", slug: "a")]);
        _api.Setup(x => x.GetContentItemEditDataAsync("blogs", "a", It.IsAny<CancellationToken>())).ReturnsAsync((ContentItemEditData?)null);
        var cut = Render<AdminProcessedUrls>();

        // Act
        cut.Find("button[title='Edit content item']").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Content item not found.");
    }

    [Fact]
    public void Edit_ApiFails_ShowsErrorMessage()
    {
        // Arrange
        SetupResult([CreateItem("https://example.com/a", "succeeded", collection: "blogs", slug: "a")]);
        _api.Setup(x => x.GetContentItemEditDataAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("boom"));
        var cut = Render<AdminProcessedUrls>();

        // Act
        cut.Find("button[title='Edit content item']").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to load content item: boom");
    }

    private static ProcessedUrlListItem CreateItem(
        string url,
        string status,
        string? feed = null,
        string? collection = null,
        string? slug = null,
        string? reason = null,
        string? error = null,
        bool? hasTranscript = null) =>
        new()
        {
            ExternalUrl = url,
            Status = status,
            FeedName = feed,
            CollectionName = collection,
            Slug = slug,
            Reason = reason,
            ErrorMessage = error,
            HasTranscript = hasTranscript,
            ProcessedAt = new DateTimeOffset(2026, 5, 6, 7, 8, 0, TimeSpan.Zero),
        };

    private void SetupResult(IReadOnlyList<ProcessedUrlListItem> items, int? total = null, int failedCount = 0)
    {
        _api.Setup(x => x.GetProcessedUrlsAsync(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(),
                It.IsAny<string?>(), It.IsAny<long?>(), It.IsAny<string?>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<ProcessedUrlListItem> { Items = items, TotalCount = total ?? items.Count });

        _api.Setup(x => x.GetProcessedUrlsAsync(
                1, 1, "failed", It.IsAny<string?>(), It.IsAny<string?>(),
                It.IsAny<string?>(), It.IsAny<long?>(), It.IsAny<string?>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<ProcessedUrlListItem> { Items = [], TotalCount = failedCount });
    }
}
