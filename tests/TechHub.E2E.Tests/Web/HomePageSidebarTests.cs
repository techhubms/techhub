using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

[Collection("Home Page Sidebar Tests")]
public class HomePageSidebarTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
    private const string BaseUrl = "http://localhost:5184";
    private IBrowserContext? _context;
    private IPage? _page;
    private IPage Page => _page ?? throw new InvalidOperationException("Page not initialized");

    public async Task InitializeAsync()
    {
        _context = await fixture.CreateContextAsync();
        _page = await _context.NewPageWithDefaultsAsync();
    }

    public async Task DisposeAsync()
    {
        if (_page != null)
        {
            await _page.CloseAsync();
        }

        if (_context != null)
        {
            await _context.DisposeAsync();
        }
    }

    [Fact]
    public async Task HomePage_ShouldDisplay_Sidebar()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert - Use generic helper with specific selector
        await Page.AssertElementVisibleBySelectorAsync(".sidebar");
    }

    [Fact]
    public async Task HomePage_Sidebar_ShouldDisplay_LatestItemsSection()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert - Use specific heading "Latest Content" to avoid ambiguity with "Latest Roundup"
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Latest Content");
    }

    [Fact]
    public async Task HomePage_Sidebar_ShouldDisplay_UpTo10LatestItems()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert - Find latest items section
        var latestSection = Page.Locator(".latest-items, .sidebar-latest");
        _ = latestSection.Locator("a").Filter(new() { HasNotText = "Latest" });

        var count = await latestSection.GetElementCountBySelectorAsync("a");
        count.Should().BeInRange(1, 10, $"Expected 1-10 latest item links, but found {count}");
    }

    [Fact]
    public async Task HomePage_Sidebar_ShouldDisplay_PopularTagsSection()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Popular Tags");
    }

    [Fact]
    public async Task HomePage_Sidebar_ShouldDisplay_TagLinks()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert - Wait for tag cloud to render, then count tag buttons
        var tagButtons = Page.Locator(".tag-cloud-item");
        await Assertions.Expect(tagButtons.First).ToBeVisibleAsync(new() { Timeout = 5000 });
        var count = await tagButtons.CountAsync();
        count.Should().BeGreaterThan(0, $"Expected at least one tag button, but found {count}");
    }

    [Fact]
    public async Task HomePage_Sidebar_LatestItemLinks_ShouldNavigateToContent()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act - Click first latest item link (if any)
        var latestSection = Page.Locator(".latest-items, .sidebar-latest");
        var itemLinks = latestSection.Locator("a").Filter(new() { HasNotText = "Latest" });
        var count = await latestSection.GetElementCountBySelectorAsync("a");

        if (count > 0)
        {
            await itemLinks.First.ClickBlazorElementAsync();

            // Assert - Should navigate to content detail page
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await Assertions.Expect(Page).Not.ToHaveURLAsync(new Regex("^" + Regex.Escape(BaseUrl) + "/?$"));
        }
    }

    [Fact]
    public async Task HomePage_Sidebar_TagLinks_ShouldFilterByTag()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act - Click first tag link (if any)
        var tagsSection = Page.Locator(".popular-tags, .sidebar-tags");
        var tagLinks = tagsSection.Locator("a");
        var count = await tagsSection.GetElementCountBySelectorAsync("a");

        if (count > 0)
        {
            _ = await tagLinks.First.TextContentWithTimeoutAsync();
            await tagLinks.First.ClickBlazorElementAsync();

            // Assert - Should navigate to filtered view or section with tag parameter
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            var currentUrl = Page.Url;
            (currentUrl.Contains("tag=", StringComparison.OrdinalIgnoreCase) ||
                currentUrl.Contains("tags=", StringComparison.OrdinalIgnoreCase) ||
                currentUrl != BaseUrl + "/")
                .Should().BeTrue($"Expected URL to change or contain tag parameter, but got: {currentUrl}");
        }
    }
}
