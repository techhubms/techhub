using Bunit;
using Bunit.TestDoubles;
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
/// bUnit tests for AdminContentItems.razor — listing, query-string restore, pagination, delete, regenerate and edit flows.
/// Debounced filter inputs are intentionally not covered here (they rely on a wall-clock delay).
/// </summary>
public class AdminContentItemsTests : AdminPageTestBase
{
    private readonly Mock<ITechHubApiClient> _api = new();

    public AdminContentItemsTests()
    {
        ComponentFactories.AddStub<ContentItemEditorModal>();
        ComponentFactories.AddStub<AddContentModal>();
        Services.AddSingleton(_api.Object);
        _api.Setup(x => x.GetFeedConfigsAsync(It.IsAny<CancellationToken>())).ReturnsAsync([]);
        SetupItems([]);
    }

    // ── Rendering ───────────────────────────────────────────────────────────

    [Fact]
    public void Render_WithItems_ShowsRowsWithLinksAndProcessedUrlState()
    {
        // Arrange
        SetupItems(
        [
            CreateItem("blogs", "post-1", "Post One", hasProcessedUrl: true, externalUrl: "https://example.com/post-1"),
            CreateItem("roundups", "weekly-1", "Weekly One", hasProcessedUrl: false),
        ]);

        // Act
        var cut = Render<AdminContentItems>();

        // Assert
        var rows = cut.FindAll("tbody tr");
        rows.Should().HaveCount(2);
        rows[0].QuerySelector(".admin-cell-article a")!.GetAttribute("href").Should().Be("https://example.com/post-1");
        rows[0].QuerySelector(".admin-status")!.TextContent.Trim().Should().Be("✓ Yes");
        rows[1].QuerySelector(".admin-cell-article a")!.GetAttribute("href").Should().Be("/ai/roundups/weekly-1");
        rows[1].QuerySelector(".admin-status")!.TextContent.Trim().Should().Be("✗ No");
        rows[1].QuerySelector(".admin-status")!.ClassList.Should().Contain("admin-status-failed");
    }

    [Fact]
    public void Render_NoItems_ShowsEmptyMessage()
    {
        // Act
        var cut = Render<AdminContentItems>();

        // Assert
        cut.Find(".admin-empty").TextContent.Should().Contain("No content items match the current filters.");
    }

    [Fact]
    public void Render_ApiFails_ShowsErrorAlert()
    {
        // Arrange
        _api.Setup(x => x.GetContentItemsAsync(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<string?>(),
                It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("API down"));

        // Act
        var cut = Render<AdminContentItems>();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to load content items: API down");
    }

    [Fact]
    public void Render_NoQueryString_UsesUnfilteredFirstPage()
    {
        // Act
        Render<AdminContentItems>();

        // Assert
        _api.Verify(
            x => x.GetContentItemsAsync(1, 100, null, null, null, null, false, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void Render_QueryStringFilters_AreForwardedToApi()
    {
        // Arrange
        Services.GetRequiredService<NavigationManager>().NavigateTo(
            "/admin/content-items?q=copilot&feed=Blog&section=ai&primaryOnly=true&collection=news&page=3");

        // Act
        var cut = Render<AdminContentItems>();

        // Assert
        _api.Verify(
            x => x.GetContentItemsAsync(3, 100, "copilot", "news", "Blog", "ai", true, It.IsAny<CancellationToken>()),
            Times.Once);
        cut.Find("#searchFilter").GetAttribute("value").Should().Be("copilot");
        cut.Find(".admin-checkbox-label input[type='checkbox']").HasAttribute("checked").Should().BeTrue();
    }

    [Theory]
    [InlineData("?page=abc")]
    [InlineData("?page=0")]
    [InlineData("?page=-4")]
    public void Render_InvalidPageInQueryString_FallsBackToFirstPage(string query)
    {
        // Arrange
        Services.GetRequiredService<NavigationManager>().NavigateTo("/admin/content-items" + query);

        // Act
        Render<AdminContentItems>();

        // Assert
        _api.Verify(
            x => x.GetContentItemsAsync(1, 100, null, null, null, null, false, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void Render_FeedNamesFromApi_AreSortedInFeedDropdown()
    {
        // Arrange
        _api.Setup(x => x.GetFeedConfigsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([new FeedConfig { Name = "Zeta" }, new FeedConfig { Name = "Alpha" }]);

        // Act
        var cut = Render<AdminContentItems>();

        // Assert
        cut.FindAll("#feedFilter option").Select(o => o.TextContent).Should().Equal("All feeds", "Alpha", "Zeta");
    }

    // ── Pagination ──────────────────────────────────────────────────────────

    [Fact]
    public void Render_MultiplePages_ShowsSummaryAndPagination()
    {
        // Arrange
        SetupItems([CreateItem("blogs", "a", "A")], total: 1234);

        // Act
        var cut = Render<AdminContentItems>();

        // Assert
        cut.Find(".admin-result-summary").TextContent.Should().Contain("Showing 1–100");
        cut.Find(".admin-pagination-info").TextContent.Should().Be("Page 1 of 13");
        cut.FindAll(".admin-pagination .btn-secondary")[0].HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public void NextPage_Click_LoadsSecondPage()
    {
        // Arrange
        SetupItems([CreateItem("blogs", "a", "A")], total: 250);
        var cut = Render<AdminContentItems>();

        // Act
        cut.FindAll(".admin-pagination .btn-secondary")[1].Click();

        // Assert
        _api.Verify(
            x => x.GetContentItemsAsync(2, 100, null, null, null, null, false, It.IsAny<CancellationToken>()),
            Times.Once);
        cut.Find(".admin-pagination-info").TextContent.Should().Be("Page 2 of 3");
    }

    [Fact]
    public void NextPage_OnLastPage_IsDisabled()
    {
        // Arrange
        SetupItems([CreateItem("blogs", "a", "A")], total: 250);
        Services.GetRequiredService<NavigationManager>().NavigateTo("/admin/content-items?page=3");

        // Act
        var cut = Render<AdminContentItems>();

        // Assert
        cut.FindAll(".admin-pagination .btn-secondary")[1].HasAttribute("disabled").Should().BeTrue();
    }

    // ── Delete ──────────────────────────────────────────────────────────────

    [Fact]
    public void Delete_Success_ShowsMessageAndReloads()
    {
        // Arrange
        SetupItems([CreateItem("blogs", "post-1", "Post One")]);
        _api.Setup(x => x.DeleteContentItemAsync("blogs", "post-1", It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var cut = Render<AdminContentItems>();

        // Act
        cut.Find("tbody .admin-link-btn-danger").Click();

        // Assert
        cut.Find(".admin-alert-success").TextContent.Should().Contain("Deleted: blogs/post-1");
        _api.Verify(
            x => x.GetContentItemsAsync(1, 100, null, null, null, null, false, It.IsAny<CancellationToken>()),
            Times.Exactly(2));
    }

    [Fact]
    public void Delete_NotFound_ShowsErrorAndDoesNotReload()
    {
        // Arrange
        SetupItems([CreateItem("blogs", "post-1", "Post One")]);
        _api.Setup(x => x.DeleteContentItemAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var cut = Render<AdminContentItems>();

        // Act
        cut.Find("tbody .admin-link-btn-danger").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Content item not found (may have already been deleted).");
        _api.Verify(
            x => x.GetContentItemsAsync(1, 100, null, null, null, null, false, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void Delete_Throws_ShowsFailureMessage()
    {
        // Arrange
        SetupItems([CreateItem("blogs", "post-1", "Post One")]);
        _api.Setup(x => x.DeleteContentItemAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("boom"));
        var cut = Render<AdminContentItems>();

        // Act
        cut.Find("tbody .admin-link-btn-danger").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to delete content item: boom");
    }

    // ── Regenerate roundup ──────────────────────────────────────────────────

    [Fact]
    public void Regenerate_OnlyOfferedForRoundups()
    {
        // Arrange
        SetupItems([CreateItem("blogs", "post-1", "Post"), CreateItem("roundups", "weekly-1", "Weekly")]);

        // Act
        var cut = Render<AdminContentItems>();

        // Assert
        cut.FindAll("button[title='Delete and regenerate this roundup via AI']").Should().ContainSingle();
        cut.FindAll("tbody tr")[0].QuerySelector("button[title='Delete and regenerate this roundup via AI']").Should().BeNull();
    }

    [Fact]
    public void Regenerate_Click_TriggersRegenerationAndShowsMessage()
    {
        // Arrange
        SetupItems([CreateItem("roundups", "weekly-1", "Weekly")]);
        var cut = Render<AdminContentItems>();

        // Act
        cut.Find("button[title='Delete and regenerate this roundup via AI']").Click();

        // Assert
        _api.Verify(x => x.RegenerateRoundupAsync("roundups", "weekly-1", It.IsAny<CancellationToken>()), Times.Once);
        cut.Find(".admin-alert-success").TextContent.Should().Contain("Roundup regeneration triggered for weekly-1");
    }

    [Fact]
    public void Regenerate_Throws_ShowsFailureMessage()
    {
        // Arrange
        SetupItems([CreateItem("roundups", "weekly-1", "Weekly")]);
        _api.Setup(x => x.RegenerateRoundupAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("boom"));
        var cut = Render<AdminContentItems>();

        // Act
        cut.Find("button[title='Delete and regenerate this roundup via AI']").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to trigger roundup regeneration: boom");
    }

    // ── Editor ──────────────────────────────────────────────────────────────

    [Fact]
    public void Edit_ItemNotFound_ShowsErrorMessage()
    {
        // Arrange
        SetupItems([CreateItem("blogs", "post-1", "Post")]);
        _api.Setup(x => x.GetContentItemEditDataAsync("blogs", "post-1", It.IsAny<CancellationToken>())).ReturnsAsync((ContentItemEditData?)null);
        var cut = Render<AdminContentItems>();

        // Act
        cut.Find("button[title='Edit content item']").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Content item not found.");
    }

    [Fact]
    public void Edit_ApiFails_ShowsErrorMessage()
    {
        // Arrange
        SetupItems([CreateItem("blogs", "post-1", "Post")]);
        _api.Setup(x => x.GetContentItemEditDataAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("boom"));
        var cut = Render<AdminContentItems>();

        // Act
        cut.Find("button[title='Edit content item']").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to load content item: boom");
    }

    [Fact]
    public void Edit_Success_OpensEditorWithTitleAndData()
    {
        // Arrange
        SetupItems([CreateItem("blogs", "post-1", "Post")]);
        _api.Setup(x => x.GetContentItemEditDataAsync("blogs", "post-1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ContentItemEditData
            {
                CollectionName = "blogs",
                Slug = "post-1",
                Title = "Post",
                Author = "A",
                Excerpt = "E",
                Content = "C",
                PrimarySectionName = "ai",
                Tags = [],
                Sections = [],
            });
        var cut = Render<AdminContentItems>();

        // Act
        cut.Find("button[title='Edit content item']").Click();

        // Assert
        var modal = cut.FindComponent<Stub<ContentItemEditorModal>>().Instance.Parameters;
        modal.Get(p => p.IsOpen).Should().BeTrue();
        modal.Get(p => p.Title).Should().Be("Edit — blogs/post-1");
        modal.Get(p => p.EditData)!.Slug.Should().Be("post-1");
        cut.FindAll(".admin-alert").Should().BeEmpty();
    }

    // ── Add content modal ───────────────────────────────────────────────────

    [Fact]
    public void AddContent_Click_OpensModal()
    {
        // Arrange
        var cut = Render<AdminContentItems>();
        cut.FindComponent<Stub<AddContentModal>>().Instance.Parameters.Get(p => p.IsOpen).Should().BeFalse();

        // Act
        cut.Find(".admin-actions .btn").Click();

        // Assert
        cut.FindComponent<Stub<AddContentModal>>().Instance.Parameters.Get(p => p.IsOpen).Should().BeTrue();
    }

    [Fact]
    public async Task AddContent_ItemAdded_ShowsSuccessAndReloads()
    {
        // Arrange
        var cut = Render<AdminContentItems>();
        var onItemAdded = cut.FindComponent<Stub<AddContentModal>>().Instance.Parameters.Get(p => p.OnItemAdded);

        // Act
        await cut.InvokeAsync(() => onItemAdded.InvokeAsync());

        // Assert
        cut.Find(".admin-alert-success").TextContent.Should().Contain("Content item added successfully.");
        _api.Verify(
            x => x.GetContentItemsAsync(1, 100, null, null, null, null, false, It.IsAny<CancellationToken>()),
            Times.Exactly(2));
    }

    private static ContentItemListItem CreateItem(
        string collection,
        string slug,
        string title,
        bool hasProcessedUrl = true,
        string externalUrl = "https://example.com/item") =>
        new()
        {
            Slug = slug,
            CollectionName = collection,
            Title = title,
            Author = "Author",
            FeedName = "Feed",
            ExternalUrl = externalUrl,
            PrimarySectionName = "ai",
            CreatedAt = new DateTimeOffset(2026, 2, 3, 4, 5, 0, TimeSpan.Zero),
            HasProcessedUrl = hasProcessedUrl,
        };

    private void SetupItems(IReadOnlyList<ContentItemListItem> items, int? total = null) =>
        _api.Setup(x => x.GetContentItemsAsync(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<string?>(),
                It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<ContentItemListItem> { Items = items, TotalCount = total ?? items.Count });
}
