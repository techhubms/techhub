using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// Tests for custom pages with SidebarToc component.
/// Verifies table of contents generation, scroll spy, and accessibility.
/// </summary>
[Collection("Custom Pages TOC Tests")]
public class CustomPagesTocTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
    private const string BaseUrl = "https://localhost:5003";
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

    #region GitHub Copilot Handbook Tests

    [Fact]
    public async Task GitHubCopilotHandbook_ShouldRender_WithSidebarToc()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot/handbook");

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
    public async Task GitHubCopilotHandbook_HeroSection_ShouldDisplay()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot/handbook");

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
    public async Task GitHubCopilotHandbook_TocLinks_ShouldScrollToSections()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot/handbook");

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
    public async Task GitHubCopilotHandbook_Scrolling_ShouldUpdateActiveTocLink()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot/handbook");

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

                    // Track this heading if we haven't seen it activated before
                    if (!activatedHeadings.Contains(headingId))
                    {
                        activatedHeadings.Add(headingId);
                    }
                }
            }
        }

        // Assert - Verify all expected headings were activated in the correct order
        activatedHeadings.Should().HaveCount(expectedHeadingIds.Length,
            $"All {expectedHeadingIds.Length} headings should have been activated during scroll. Activated: [{string.Join(", ", activatedHeadings)}]");

        for (var i = 0; i < expectedHeadingIds.Length; i++)
        {
            activatedHeadings[i].Should().Be(expectedHeadingIds[i],
                $"Heading at position {i} should be '{expectedHeadingIds[i]}' but was '{activatedHeadings[i]}'");
        }
    }

    [Fact]
    public async Task GitHubCopilotHandbook_ShouldBe_KeyboardAccessible()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot/handbook");

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

        foundTocLink.Should().BeTrue("Should be able to reach TOC links via keyboard navigation");

        // Act - Press Enter to activate link
        var urlBefore = Page.Url;
        await Page.Keyboard.PressAsync("Enter");
        await Page.WaitForTimeoutAsync(300);

        // Assert - URL should have changed (anchor added)
        var urlAfter = Page.Url;
        urlAfter.Should().NotBe(urlBefore, "Expected URL to change after pressing Enter on TOC link");
    }

    #endregion

    #region Levels of Enlightenment Tests

    [Fact]
    public async Task LevelsOfEnlightenment_ShouldRender_WithSidebarToc()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot/levels-of-enlightenment");

        // Assert - Check page title (includes "GitHub Copilot:" prefix)
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "GitHub Copilot: Levels of Enlightenment", level: 1);

        // Check sidebar TOC exists
        var toc = Page.Locator(".sidebar-toc");
        await toc.AssertElementVisibleAsync();

        // Should have TOC links
        var tocLinks = toc.Locator("a");
        var linkCount = await tocLinks.CountAsync();
        linkCount.Should().BeGreaterThan(0, "Expected TOC to have navigation links");
    }

    [Fact]
    public async Task LevelsOfEnlightenment_TocLinks_ShouldScrollToLevels()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot/levels-of-enlightenment");

        // Get all TOC links
        var tocLinks = Page.Locator(".sidebar-toc a");
        var linkCount = await tocLinks.CountAsync();

        if (linkCount == 0)
        {
            Assert.Fail("No TOC links found on levels page");
        }

        // Act - Click first TOC link (should be "Overview" or first level)
        var firstLink = tocLinks.First;
        var linkText = await firstLink.TextContentAsync();
        await firstLink.ClickAsync();

        // Wait for scroll to complete
        await Page.WaitForTimeoutAsync(500);

        // Assert - URL should have hash
        var url = Page.Url;
        url.Should().Contain("#", $"Expected URL to contain anchor after clicking TOC link '{linkText}'");

        // Assert - Clicked link should have active class
        var activeLinks = await Page.Locator(".sidebar-toc a.active").CountAsync();
        activeLinks.Should().BeGreaterThan(0, "Expected at least one TOC link to be active");
    }

    [Fact]
    public async Task LevelsOfEnlightenment_LastSection_ShouldScroll_ToDetectionPoint()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot/levels-of-enlightenment");

        // Get last heading with ID
        var headings = Page.Locator("h2[id], h3[id]");
        var headingCount = await headings.CountAsync();

        if (headingCount == 0)
        {
            Assert.Fail("No headings with IDs found on page");
        }

        var lastHeading = headings.Last;

        // Act - Scroll to last heading
        await lastHeading.ScrollIntoViewIfNeededAsync();
        await Page.WaitForTimeoutAsync(500);

        // Scroll down more to trigger detection (50vh spacer should allow this)
        await Page.EvaluateAsync("window.scrollBy(0, 500)");
        await Page.WaitForTimeoutAsync(300);

        // Assert - Last heading's TOC link should be active
        var lastHeadingId = await lastHeading.GetAttributeAsync("id");
        var expectedActiveLink = Page.Locator($".sidebar-toc a[href$='#{lastHeadingId}']");

        var hasActiveClass = await expectedActiveLink.EvaluateAsync<bool>("el => el.classList.contains('active')");
        hasActiveClass.Should().BeTrue($"Expected TOC link for #{lastHeadingId} to be active when scrolled to bottom");
    }

    [Fact]
    public async Task LevelsOfEnlightenment_Scrolling_ShouldUpdateActiveTocLink()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot/levels-of-enlightenment");

        // Get all section headings with IDs
        var headings = Page.Locator("h2[id], h3[id]");
        var headingCount = await headings.CountAsync();

        if (headingCount < 2)
        {
            // Skip test if not enough sections to test scroll spy
            return;
        }

        // Wait for scroll spy to initialize and activate at least one link
        await Page.WaitForSelectorAsync(".sidebar-toc a.active", new() { Timeout = 5000 });

        // Record initial active link
        var initialActiveHref = await Page.Locator(".sidebar-toc a.active").First.GetAttributeAsync("href");

        // Act - Scroll to third heading (if exists) or second
        var targetHeading = headingCount >= 3 ? headings.Nth(2) : headings.Nth(1);
        await targetHeading.ScrollIntoViewIfNeededAsync();
        await Page.WaitForTimeoutAsync(300); // Wait for scroll spy to update

        // Assert - Active TOC link should have changed
        var newActiveHref = await Page.Locator(".sidebar-toc a.active").First.GetAttributeAsync("href");
        newActiveHref.Should().NotBe(initialActiveHref, "Expected active TOC link to change after scrolling to different section");
    }

    [Fact]
    public async Task LevelsOfEnlightenment_Overview_ShouldBe_Highlighted()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync("/github-copilot/levels-of-enlightenment");

        // Scroll to top to ensure first section is in view
        await Page.EvaluateAsync("window.scrollTo(0, 0)");

        // Wait for scroll spy to initialize and activate at least one link
        await Page.WaitForSelectorAsync(".sidebar-toc a.active", new() { Timeout = 5000 });

        // Assert - At least one TOC link should be active after initialization
        var activeLinks = await Page.Locator(".sidebar-toc a.active").CountAsync();
        activeLinks.Should().BeGreaterThan(0, "Expected at least one TOC link to be active after scroll spy initializes");
    }

    #endregion

    #region VS Code Updates Tests (Reference Implementation)

    [Fact]
    public async Task VSCodeUpdates_ShouldRender_WithSidebarToc()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot/vscode-updates");

        // Assert - Check sidebar TOC exists
        var toc = Page.Locator(".sidebar-toc");
        await toc.AssertElementVisibleAsync();

        // Should have TOC links
        var tocLinks = toc.Locator("a");
        var linkCount = await tocLinks.CountAsync();
        linkCount.Should().BeGreaterThan(0, "Expected TOC to have navigation links");
    }

    [Fact]
    public async Task VSCodeUpdates_TocLinks_ShouldScrollToSections()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot/vscode-updates");

        // Get all TOC links
        var tocLinks = Page.Locator(".sidebar-toc a");
        var linkCount = await tocLinks.CountAsync();

        if (linkCount < 2)
        {
            // Skip if not enough TOC links
            return;
        }

        // Act - Click second TOC link
        var secondLink = tocLinks.Nth(1);
        var linkText = await secondLink.TextContentAsync();
        await secondLink.ClickAsync();

        // Wait for scroll to complete
        await Page.WaitForTimeoutAsync(500);

        // Assert - URL should have hash
        var url = Page.Url;
        url.Should().Contain("#", $"Expected URL to contain anchor after clicking TOC link '{linkText}'");

        // Assert - Clicked link should have active class
        var hasActiveClass = await secondLink.EvaluateAsync<bool>("el => el.classList.contains('active')");
        hasActiveClass.Should().BeTrue($"Expected clicked TOC link '{linkText}' to have active class");
    }

    #endregion

    #region General TOC Behavior Tests

    [Theory]
    [InlineData("/github-copilot/handbook")]
    [InlineData("/github-copilot/levels-of-enlightenment")]
    [InlineData("/github-copilot/vscode-updates")]
    public async Task CustomPageWithToc_Should_Not_HaveConsoleErrors(string url)
    {
        // Arrange - Collect console errors
        var consoleErrors = new List<string>();
        Page.Console += (_, msg) =>
        {
            if (msg.Type == "error")
            {
                consoleErrors.Add(msg.Text);
            }
        };

        // Act
        await Page.GotoRelativeAsync(url);
        await Page.WaitForTimeoutAsync(1000); // Wait for JS to execute

        // Assert
        consoleErrors.Should().BeEmpty($"Expected no console errors on {url}, but found: {string.Join(", ", consoleErrors)}");
    }

    [Theory]
    [InlineData("/github-copilot/handbook")]
    [InlineData("/github-copilot/levels-of-enlightenment")]
    [InlineData("/github-copilot/vscode-updates")]
    public async Task CustomPageWithToc_TocLinks_ShouldBe_KeyboardAccessible(string url)
    {
        // Arrange
        await Page.GotoRelativeAsync(url);

        // Act - Tab through page until we hit a TOC link
        var foundTocLink = false;
        for (var i = 0; i < 30; i++)
        {
            await Page.Keyboard.PressAsync("Tab");
            var isTocLink = await Page.EvaluateAsync<bool>(
                "document.activeElement && (document.activeElement.closest('.sidebar-toc') !== null)"
            );
            if (isTocLink)
            {
                foundTocLink = true;
                break;
            }
        }

        // Assert
        foundTocLink.Should().BeTrue($"Should be able to reach TOC links via keyboard on {url}");
    }

    #endregion
}
