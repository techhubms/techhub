using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Xunit;

namespace TechHub.E2E.Tests.Web;

[Collection("Home Page Sidebar Tests")]
public class HomePageSidebarTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
    private const string BaseUrl = "http://localhost:5184";
    private IBrowserContext? _context;
    private IPage Page => _page ?? throw new InvalidOperationException("Page not initialized");
    private IPage? _page;

    public async Task InitializeAsync()
    {
        _context = await fixture.CreateContextAsync();
        _page = await _context.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        if (_page != null)
            await _page.CloseAsync();
        
        if (_context != null)
            await _context.CloseAsync();
    }
    [Fact]
    public async Task HomePage_ShouldDisplay_Sidebar()
    {
        // Act
        await Page.GotoAsync(BaseUrl);
        
        // Assert
        var sidebar = Page.Locator(".sidebar, .home-sidebar, aside");
        await Assertions.Expect(sidebar).ToBeVisibleAsync();
    }

    [Fact]
    public async Task HomePage_Sidebar_ShouldDisplay_LatestItemsSection()
    {
        // Act
        await Page.GotoAsync(BaseUrl);
        
        // Assert - Use specific heading "Latest Content" to avoid ambiguity with "Latest Roundup"
        await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Latest Content" })).ToBeVisibleAsync();
    }

    [Fact]
    public async Task HomePage_Sidebar_ShouldDisplay_UpTo10LatestItems()
    {
        // Act
        await Page.GotoAsync(BaseUrl);
        
        // Assert - Find latest items section
        var latestSection = Page.Locator(".latest-items, .sidebar-latest");
        var itemLinks = latestSection.Locator("a").Filter(new() { HasNotText = "Latest" });
        
        var count = await itemLinks.CountAsync();
        Assert.True(count > 0 && count <= 10, $"Expected 1-10 latest item links, but found {count}");
    }

    [Fact]
    public async Task HomePage_Sidebar_ShouldDisplay_PopularTagsSection()
    {
        // Act
        await Page.GotoAsync(BaseUrl);
        
        // Assert
        await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() { NameRegex = new Regex("Popular Tags", RegexOptions.IgnoreCase) })).ToBeVisibleAsync();
    }

    [Fact]
    public async Task HomePage_Sidebar_ShouldDisplay_TagLinks()
    {
        // Act
        await Page.GotoAsync(BaseUrl);
        
        // Assert - Find popular tags section
        var tagsSection = Page.Locator(".popular-tags, .sidebar-tags");
        var tagLinks = tagsSection.Locator("a, .tag");
        
        var count = await tagLinks.CountAsync();
        Assert.True(count > 0, $"Expected at least one tag link, but found {count}");
    }

    [Fact]
    public async Task HomePage_Sidebar_LatestItemLinks_ShouldNavigateToContent()
    {
        // Arrange
        await Page.GotoAsync(BaseUrl);
        
        // Act - Click first latest item link (if any)
        var latestSection = Page.Locator(".latest-items, .sidebar-latest");
        var itemLinks = latestSection.Locator("a").Filter(new() { HasNotText = "Latest" });
        var count = await itemLinks.CountAsync();
        
        if (count > 0)
        {
            await itemLinks.First.ClickAsync();
            
            // Assert - Should navigate to content detail page
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await Assertions.Expect(Page).Not.ToHaveURLAsync(new Regex("^" + Regex.Escape(BaseUrl) + "/?$"));
        }
    }

    [Fact]
    public async Task HomePage_Sidebar_TagLinks_ShouldFilterByTag()
    {
        // Arrange
        await Page.GotoAsync(BaseUrl);
        
        // Act - Click first tag link (if any)
        var tagsSection = Page.Locator(".popular-tags, .sidebar-tags");
        var tagLinks = tagsSection.Locator("a");
        var count = await tagLinks.CountAsync();
        
        if (count > 0)
        {
            var firstTag = await tagLinks.First.TextContentAsync();
            await tagLinks.First.ClickAsync();
            
            // Assert - Should navigate to filtered view or section with tag parameter
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            var currentUrl = Page.Url;
            Assert.True(
                currentUrl.Contains("tag=", StringComparison.OrdinalIgnoreCase) || 
                currentUrl.Contains("tags=", StringComparison.OrdinalIgnoreCase) ||
                currentUrl != BaseUrl + "/",
                $"Expected URL to change or contain tag parameter, but got: {currentUrl}");
        }
    }
}
