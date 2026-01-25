using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for GitHub Copilot Handbook custom page.
/// Verifies book information display, table of contents, and accessibility.
/// </summary>
[Collection("Custom Pages TOC Tests")]
public class HandbookTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
    private const string PageUrl = "/github-copilot/handbook";
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
            await _context.CloseAsync();
        }
    }

    [Fact]
    public async Task Handbook_ShouldRender_WithSidebarToc()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Check sidebar TOC exists
        var toc = Page.Locator(".sidebar-toc");
        await toc.AssertElementVisibleAsync();

        // Should have TOC heading
        var tocHeading = toc.Locator("h2, h3").First;
        await tocHeading.AssertElementVisibleAsync();

        // Should have TOC links
        var tocLinks = toc.Locator("a");
        var linkCount = await tocLinks.CountAsync();
        linkCount.Should().BeGreaterThan(0, "Expected TOC to have navigation links");
    }

    [Fact]
    public async Task Handbook_HeroSection_ShouldDisplay()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Check hero section exists
        var hero = Page.Locator(".handbook-hero");
        await hero.AssertElementVisibleAsync();

        // Check book cover image
        var bookCover = hero.Locator("img");
        await bookCover.AssertElementVisibleAsync();

        // Check title
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "The GitHub Copilot Handbook", level: 1);

        // Check CTA button
        var ctaButton = hero.Locator("a.handbook-cta");
        await ctaButton.AssertElementVisibleAsync();
    }

    [Fact]
    public async Task Handbook_TocLinks_ShouldScrollToSections()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Get all TOC links
        var tocLinks = Page.Locator(".sidebar-toc a");
        var linkCount = await tocLinks.CountAsync();

        if (linkCount == 0)
        {
            Assert.Fail("No TOC links found on handbook page");
        }

        // Act - Click first TOC link
        var firstLink = tocLinks.First;
        await firstLink.ClickAndWaitForScrollAsync();

        // Assert - URL should have hash
        var url = Page.Url;
        url.Should().Contain("#", "Expected URL to contain anchor after clicking TOC link");

        // Assert - Clicked link should have active class
        var activeLinks = await Page.Locator(".sidebar-toc a.active").CountAsync();
        activeLinks.Should().BeGreaterThan(0, "Expected at least one TOC link to be active");
    }

    [Fact]
    public async Task Handbook_Scrolling_ShouldUpdateActiveTocLink()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Get all TOC links
        var tocLinks = Page.Locator(".sidebar-toc a");
        var linkCount = await tocLinks.CountAsync();

        if (linkCount < 2)
        {
            // Skip if not enough TOC links
            return;
        }

        // Act - Click second TOC link to scroll to that section
        var secondLink = tocLinks.Nth(1);
        var linkText = await secondLink.TextContentAsync();
        await secondLink.ClickAndWaitForScrollAsync();

        // Assert - URL should have hash
        var url = Page.Url;
        url.Should().Contain("#", $"Expected URL to contain anchor after clicking TOC link '{linkText}'");

        // Assert - Clicked link should have active class
        // Use Playwright's auto-waiting expect assertion
        await Assertions.Expect(secondLink).ToHaveClassAsync(new System.Text.RegularExpressions.Regex(".*active.*"),
            new() { Timeout = BlazorHelpers.DefaultAssertionTimeout });
    }

    [Fact]
    public async Task Handbook_ShouldBe_KeyboardAccessible()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Act - Tab to first TOC link
        await Page.Keyboard.PressAsync("Tab");

        // Keep tabbing until we reach a TOC link (max 30 tabs to account for page complexity)
        var foundTocLink = false;
        for (var i = 0; i < 30; i++)
        {
            // Check if current element or its parent is within sidebar-toc
            var isInToc = await Page.EvaluateAsync<bool>(@"
                (function() {
                    const elem = document.activeElement;
                    if (!elem) return false;
                    // Check if element itself or any parent has sidebar-toc class
                    return elem.closest('.sidebar-toc') !== null;
                })()
            ");

            if (isInToc)
            {
                foundTocLink = true;
                break;
            }

            await Page.Keyboard.PressAsync("Tab");
        }

        // Assert
        foundTocLink.Should().BeTrue("Should be able to reach TOC links via keyboard navigation");

        // Act - Press Enter on the focused TOC link
        var urlBefore = Page.Url;
        await Page.Keyboard.PressAsync("Enter");
        await Page.WaitForScrollEndAsync(300);

        // Assert - URL should have changed (anchor added)
        var urlAfter = Page.Url;
        urlAfter.Should().NotBe(urlBefore, "Expected URL to change after pressing Enter on TOC link");
    }
}
