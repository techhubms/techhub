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
    private const string BaseUrl = "https://localhost:5003";
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
        await firstLink.ClickAsync();

        // Wait for scroll to complete
        await Page.WaitForTimeoutAsync(500);

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

        // Wait for TOC scroll spy module to initialize (ES module loading)
        await Page.WaitForTimeoutAsync(1000);

        // Expected heading IDs in the order they appear on the page
        var expectedHeadingIds = new[] { "about-book", "learnings", "audience", "key-features", "toc", "authors" };
        var activatedHeadings = new List<string>();

        // Start at the top
        await Page.EvaluateAsync("window.scrollTo(0, 0)");
        await Page.WaitForTimeoutAsync(500);

        // Get page dimensions
        var pageHeight = await Page.EvaluateAsync<int>("document.documentElement.scrollHeight");
        var viewportHeight = await Page.EvaluateAsync<int>("window.innerHeight");
        var scrollIncrement = 200; // Scroll 200px at a time

        // Act - Scroll down gradually and track which headings activate
        for (var scrollPosition = 0; scrollPosition < pageHeight - viewportHeight; scrollPosition += scrollIncrement)
        {
            await Page.EvaluateAsync($"window.scrollTo(0, {scrollPosition})");
            await Page.WaitForTimeoutAsync(300); // Wait for scroll spy to detect changes

            // Check if any TOC link is active
            var activeLinks = Page.Locator(".sidebar-toc a.active");
            var activeCount = await activeLinks.CountAsync();

            if (activeCount > 0)
            {
                var activeHref = await activeLinks.First.GetAttributeAsync("href");
                if (activeHref != null && activeHref.Contains('#'))
                {
                    var headingId = activeHref.Split('#')[1];
                    if (!activatedHeadings.Contains(headingId))
                    {
                        activatedHeadings.Add(headingId);
                    }
                }
            }
        }

        // Assert - At least some headings should have been activated during scrolling
        activatedHeadings.Should().NotBeEmpty("Expected at least one heading to be activated during scrolling");

        // Assert - Activated headings should match expected headings (in any order)
        foreach (var activatedId in activatedHeadings)
        {
            expectedHeadingIds.Should().Contain(activatedId,
                $"Activated heading '{activatedId}' should be in the expected list of heading IDs");
        }
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
        await Page.WaitForTimeoutAsync(500);

        // Assert - URL should have changed (anchor added)
        var urlAfter = Page.Url;
        urlAfter.Should().NotBe(urlBefore, "Expected URL to change after pressing Enter on TOC link");
    }
}
