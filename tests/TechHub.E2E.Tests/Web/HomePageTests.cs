using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

[Collection("Home Page Tests")]
public class HomePageTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
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

    // Roundup Sidebar Section Tests
    [Fact]
    public async Task HomePage_ShouldDisplay_LatestRoundupSection()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert - Look for "Latest Roundup" heading in sidebar
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Latest Roundup");
    }

    [Fact]
    public async Task HomePage_LatestRoundupSection_ShouldDisplay_FeaturedRoundupWithTitleAndDate()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert - Should display latest roundup section (should be exactly 1)
        var latestRoundupSection = Page.Locator(".latest-roundup");
        await latestRoundupSection.AssertElementVisibleAsync();

        // Should have one featured roundup link
        var roundupLink = Page.Locator(".latest-roundup a.sidebar-featured-link");
        await roundupLink.AssertElementVisibleAsync();

        // Link should have title and date
        var roundupTitle = Page.Locator(".latest-roundup .sidebar-featured-title");
        await roundupTitle.AssertElementVisibleAsync();

        var roundupDate = Page.Locator(".latest-roundup .sidebar-featured-date");
        await roundupDate.AssertElementVisibleAsync();
    }

    [Fact]
    public async Task HomePage_ShouldDisplay_NewsletterLink()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert - Look for link containing "newsletter"
        var newsletterLink = Page.Locator("a:has-text('newsletter')");
        await newsletterLink.AssertElementVisibleAsync();

        // Should link to mailchimp
        var href = await newsletterLink.GetHrefAsync();
        href.Should().Contain("mailchi.mp");
    }

    // General Sidebar Tests
    [Fact]
    public async Task HomePage_ShouldDisplay_Sidebar()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert
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

        // Wait for Blazor Server interactivity to be ready (tag cloud uses InteractiveServer)
        await Page.WaitForBlazorReadyAsync();

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

            // Assert - Should navigate away from homepage
            await Assertions.Expect(Page).Not.ToHaveURLAsync(new Regex("^https://localhost:5003/?$"));
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
            await tagLinks.First.ClickBlazorElementAsync();

            // Assert - Should navigate to filtered view or section with tag parameter
            var currentUrl = Page.Url;
            (currentUrl.Contains("tag=", StringComparison.OrdinalIgnoreCase) ||
                currentUrl.Contains("tags=", StringComparison.OrdinalIgnoreCase) ||
                currentUrl != "https://localhost:5003/")
                .Should().BeTrue($"Expected URL to change or contain tag parameter, but got: {currentUrl}");
        }
    }
}
