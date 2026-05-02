using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for the content type filter on Browse pages.
/// The filter shows collection-type toggle buttons (News, Blogs, Videos, Community)
/// on every section's Browse page (/section/all), not just GitHub Copilot.
/// </summary>
public class ContentTypeFilterTests : PlaywrightTestBase
{
    // Standard content type labels as constants to avoid duplication
    private const string News = "News";
    private const string Blogs = "Blogs";
    private const string Videos = "Videos";
    private const string Community = "Community";

    public ContentTypeFilterTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    // ========================================================================
    // Rendering Tests
    // ========================================================================

    [Fact]
    public async Task ContentTypeFilter_RendersOnGitHubCopilotBrowsePage()
    {
        // Arrange & Act - Navigate to GitHub Copilot browse page (the original section)
        await Page.GotoRelativeAsync("/github-copilot/all");

        // Assert - Filter container must be visible
        var filterContainer = Page.Locator(".content-type-filter");
        await Assertions.Expect(filterContainer).ToBeVisibleAsync();

        // At least one filter button must be present
        var buttons = Page.Locator(".content-type-btn");
        var count = await buttons.CountAsync();
        count.Should().BeGreaterThan(0, "GitHub Copilot browse page should have content type filter buttons");
    }

    [Fact]
    public async Task ContentTypeFilter_RendersOnAiBrowsePage()
    {
        // Arrange & Act - Navigate to AI browse page (a regular section)
        await Page.GotoRelativeAsync("/ai/all");

        // Assert - Filter container must be visible on non-HideCollectionPages sections too
        var filterContainer = Page.Locator(".content-type-filter");
        await Assertions.Expect(filterContainer).ToBeVisibleAsync();

        var buttons = Page.Locator(".content-type-btn");
        var count = await buttons.CountAsync();
        count.Should().BeGreaterThan(0, "AI browse page should have content type filter buttons");
    }

    [Fact]
    public async Task ContentTypeFilter_RendersOnAllSectionBrowsePage()
    {
        // Arrange & Act - Navigate to the top-level "all" section browse page
        await Page.GotoRelativeAsync("/all/all");

        // Assert - Filter container must be visible
        var filterContainer = Page.Locator(".content-type-filter");
        await Assertions.Expect(filterContainer).ToBeVisibleAsync();

        var buttons = Page.Locator(".content-type-btn");
        var count = await buttons.CountAsync();
        count.Should().BeGreaterThan(0, "All section browse page should have content type filter buttons");
    }

    [Fact]
    public async Task ContentTypeFilter_ShowsStandardContentTypeButtons()
    {
        // Arrange & Act - Navigate to the AI browse page
        await Page.GotoRelativeAsync("/ai/all");

        // Assert - The standard content types must all be present
        await Assertions.Expect(ContentTypeButton(News)).ToBeVisibleAsync();
        await Assertions.Expect(ContentTypeButton(Blogs)).ToBeVisibleAsync();
        await Assertions.Expect(ContentTypeButton(Videos)).ToBeVisibleAsync();
        await Assertions.Expect(ContentTypeButton(Community)).ToBeVisibleAsync();
    }

    [Fact]
    public async Task ContentTypeFilter_AllButtonsActiveByDefault()
    {
        // Arrange & Act - Navigate to AI browse page without any filter
        await Page.GotoRelativeAsync("/ai/all");

        // Assert - All buttons should be active when no filter is applied
        var buttons = Page.Locator(".content-type-btn");
        var allButtons = await buttons.AllAsync();

        allButtons.Should().NotBeEmpty("browse page should have content type filter buttons");

        foreach (var button in allButtons)
        {
            var classAttr = await button.GetAttributeAsync("class") ?? "";
            classAttr.Should().Contain("active",
                "all content type buttons should be active when no filter is applied");
        }
    }

    // ========================================================================
    // Visibility Scope Tests — filter ONLY on browse (/all), not on collections
    // ========================================================================

    [Fact]
    public async Task ContentTypeFilter_DoesNotRenderOnCollectionPage()
    {
        // Arrange & Act - Navigate to a specific collection (not the browse/all page)
        await Page.GotoRelativeAsync("/ai/news");

        // Assert - Filter container must NOT be present on specific collection pages
        await Assertions.Expect(Page.Locator(".content-type-filter")).ToHaveCountAsync(0);
    }

    // ========================================================================
    // Interaction Tests
    // ========================================================================

    [Fact]
    public async Task ContentTypeFilter_WhenButtonClicked_UpdatesUrlWithTypesParam()
    {
        // Arrange - Navigate to AI browse page
        await Page.GotoRelativeAsync("/ai/all");
        await WaitForContentTypeFilterReadyAsync();

        // Act - Click the "Blogs" button to deselect it (all active → all except blogs).
        // Assert button CLASS change (not URL) inside ClickAndExpectAsync. This is safe
        // for toggle buttons: if the click is lost (handler not attached), the class won't
        // change, so the retry correctly clicks again. Asserting URL here would be unsafe
        // because re-clicking a toggle undoes the action, causing oscillation on slow CI.
        await ContentTypeButton(Blogs).ClickAndExpectAsync(async () =>
            await Assertions.Expect(ContentTypeButton(Blogs))
                .Not.ToHaveClassAsync(new Regex("active"), new() { Timeout = 2000 }));

        // Assert - URL should contain the types parameter (arrives in same SignalR batch as class change)
        await Assertions.Expect(Page).ToHaveURLAsync(new Regex(@".*types=.*"));

        var currentUrl = Page.Url;
        currentUrl.Should().Contain("types=", "clicking a content type button should add types parameter to URL");

        var uri = new Uri(currentUrl);
        var typesParam = System.Web.HttpUtility.ParseQueryString(uri.Query).Get("types");
        typesParam.Should().NotBeNullOrEmpty("types parameter should have a value");
    }

    [Fact]
    public async Task ContentTypeFilter_WhenButtonClicked_ShowsActiveState()
    {
        // Arrange - Navigate to AI browse page
        await Page.GotoRelativeAsync("/ai/all");
        await WaitForContentTypeFilterReadyAsync();

        // Act - Click the "Videos" button to deselect it (all active → all except videos).
        // Assert button CLASS change inside ClickAndExpectAsync (safe for toggle retries).
        await ContentTypeButton(Videos).ClickAndExpectAsync(async () =>
            await Assertions.Expect(ContentTypeButton(Videos))
                .Not.ToHaveClassAsync(new Regex("active"), new() { Timeout = 2000 }));

        // Verify: URL should reflect the filter change
        await Assertions.Expect(Page).ToHaveURLAsync(new Regex(@".*types=.*"));

        // Assert - News should remain active
        var newsClass = await ContentTypeButton(News).GetAttributeAsync("class") ?? "";
        newsClass.Should().Contain("active", "non-deselected content type buttons should remain active");
    }

    [Fact]
    public async Task ContentTypeFilter_WithUrlParam_ShowsCorrectActiveState()
    {
        // Arrange & Act - Navigate with types URL parameter pre-applied
        await Page.GotoRelativeAsync("/ai/all?types=news,blogs");
        await WaitForContentTypeFilterReadyAsync();

        // Assert - Only news and blogs should be active
        var newsClass = await ContentTypeButton(News).GetAttributeAsync("class") ?? "";
        var blogsClass = await ContentTypeButton(Blogs).GetAttributeAsync("class") ?? "";
        var videosClass = await ContentTypeButton(Videos).GetAttributeAsync("class") ?? "";
        var communityClass = await ContentTypeButton(Community).GetAttributeAsync("class") ?? "";

        newsClass.Should().Contain("active", "News button should be active when types=news,blogs");
        blogsClass.Should().Contain("active", "Blogs button should be active when types=news,blogs");
        videosClass.Should().NotContain("active", "Videos button should not be active when not in types filter");
        communityClass.Should().NotContain("active", "Community button should not be active when not in types filter");
    }

    [Fact]
    public async Task ContentTypeFilter_WhenAllTypesReactivated_RemovesTypesParamFromUrl()
    {
        // Arrange - Navigate with one type deselected (videos is missing, so it's inactive)
        await Page.GotoRelativeAsync("/ai/all?types=news,blogs,community");
        await WaitForContentTypeFilterReadyAsync();

        // Act - Clicking Videos re-activates all 4 types, which should clear the URL param
        await ContentTypeButton(Videos).ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page).Not.ToHaveURLAsync(
                new Regex(@".*types=.*"), new() { Timeout = 2000 }));

        // Assert - URL should no longer contain types parameter
        Page.Url.Should().NotContain("types=",
            "when all content types are active, the types URL parameter should be removed");
    }

    private ILocator ContentTypeButton(string label) =>
        Page.Locator(".content-type-btn", new() { HasTextString = label });

    private async Task WaitForContentTypeFilterReadyAsync()
    {
        await Assertions.Expect(Page.Locator(".content-type-filter")).ToBeVisibleAsync();
        await Assertions.Expect(Page.Locator(".content-type-btn").First).ToBeVisibleAsync();
    }
}
