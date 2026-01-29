using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for sidebar table of contents (TOC) component behavior.
/// Tests common TOC functionality on representative pages to avoid duplication.
/// 
/// Test Pages:
/// - /github-copilot/vscode-updates - Page with code syntax highlighting
/// - /ai/genai-basics - Page with mermaid diagrams and complex nested TOC
/// 
/// Coverage:
/// - TOC rendering and visibility on direct page load
/// - TOC scroll-spy works after client-side navigation
/// - TOC link navigation and scrolling
/// - Active link updates on scroll
/// - Keyboard accessibility
/// - Anchor navigation (direct URL with hash)
/// - Last section detection (scroll spy edge case)
/// - No console errors
/// </summary>
[Collection("Sidebar TOC Tests")]
public class SidebarTocTests : IAsyncLifetime
{
    private readonly PlaywrightCollectionFixture _fixture;

    public SidebarTocTests(PlaywrightCollectionFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        _fixture = fixture;
    }

    private IBrowserContext? _context;
    private IPage? _page;
    private IPage Page => _page ?? throw new InvalidOperationException("Page not initialized");

    public async Task InitializeAsync()
    {
        _context = await _fixture.CreateContextAsync();
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

    #region TOC Rendering

    [Theory]
    [InlineData("/github-copilot/vscode-updates")]
    [InlineData("/ai/genai-basics")]
    public async Task SidebarToc_ShouldRender_Successfully(string url)
    {
        // Arrange & Act
        await Page.GotoRelativeAsync(url);

        // Assert - Check sidebar TOC exists
        var toc = Page.Locator(".sidebar-toc");
        await toc.AssertElementVisibleAsync();

        // Should have TOC heading
        var tocHeading = toc.Locator("h2, h3").First;
        await tocHeading.AssertElementVisibleAsync();

        // Should have TOC links
        var tocLinks = toc.Locator("a");
        var linkCount = await tocLinks.CountAsync();
        linkCount.Should().BeGreaterThan(0, $"Expected TOC to have navigation links on {url}");
    }

    #endregion

    #region TOC Link Navigation

    [Theory]
    [InlineData("/github-copilot/vscode-updates")]
    [InlineData("/ai/genai-basics")]
    public async Task SidebarToc_ClickingLink_ShouldScrollToSection(string url)
    {
        // Arrange
        await Page.GotoRelativeAsync(url);

        // Get all TOC links
        var tocLinks = Page.Locator(".sidebar-toc a");
        var linkCount = await tocLinks.CountAsync();

        if (linkCount == 0)
        {
            Assert.Fail($"No TOC links found on {url}");
        }

        // Act - Click first TOC link
        var firstLink = tocLinks.First;
        var linkText = await firstLink.TextContentAsync();
        await firstLink.ClickAndWaitForScrollAsync();

        // Assert - URL should have hash
        var pageUrl = Page.Url;
        pageUrl.Should().Contain("#", $"Expected URL to contain anchor after clicking TOC link '{linkText}' on {url}");

        // Assert - Clicked link should have active class
        await Assertions.Expect(Page.Locator(".sidebar-toc a.active").First).ToBeVisibleAsync();
    }

    #endregion

    #region Active Link Updates on Scroll

    [Theory]
    [InlineData("/github-copilot/vscode-updates")]
    [InlineData("/ai/genai-basics")]
    public async Task SidebarToc_Scrolling_ShouldUpdateActiveLink(string url)
    {
        // Arrange
        await Page.GotoRelativeAsync(url);

        // Find a section heading (h2 or h3 with ID)
        var headings = Page.Locator("h2[id], h3[id]");
        var headingCount = await headings.CountAsync();

        if (headingCount < 2)
        {
            Assert.Fail($"Not enough headings with IDs found on {url}");
        }

        // Act - Scroll to second heading
        _ = headings.Nth(1);
        await Page.EvaluateAndWaitForScrollAsync("document.querySelectorAll('h2[id], h3[id]')[1].scrollIntoView()");

        // Assert - Active TOC link should update
        var activeTocLink = Page.Locator(".sidebar-toc a.active").First;
        await activeTocLink.AssertElementVisibleAsync();

        var activeLinkText = await activeTocLink.TextContentAsync();
        activeLinkText.Should().NotBeNullOrEmpty($"Active TOC link should have text on {url}");
    }

    [Theory]
    [InlineData("/github-copilot/vscode-updates")]
    [InlineData("/ai/genai-basics")]
    public async Task SidebarToc_LastSection_ShouldBecome_Active_WhenScrolledToBottom(string url)
    {
        // Arrange
        await Page.GotoRelativeAsync(url);

        // Get last heading with ID
        var headings = Page.Locator("h2[id], h3[id]");
        var headingCount = await headings.CountAsync();

        if (headingCount == 0)
        {
            Assert.Fail($"No headings with IDs found on {url}");
        }

        var lastHeading = headings.Last;
        var lastHeadingId = await lastHeading.GetAttributeAsync("id");

        // Act - Scroll to the absolute bottom of the page
        // This ensures the last section heading crosses the 30% detection line
        await Page.EvaluateAndWaitForScrollAsync("window.scrollTo(0, document.documentElement.scrollHeight)");

        // Get the last TOC link - use href$= to match the end of the href (hash part)
        var lastTocLink = Page.Locator($".sidebar-toc a[href$='#{lastHeadingId}']");

        // Use Playwright's auto-retrying assertion - wait for TOC link to become active
        // This handles timing variations in scroll spy detection
        await Assertions.Expect(lastTocLink).ToHaveClassAsync(new System.Text.RegularExpressions.Regex(".*active.*"),
            new() { Timeout = 3000 });
    }

    #endregion

    #region Keyboard Accessibility

    [Theory]
    [InlineData("/ai/genai-basics")]
    public async Task SidebarToc_TocLinks_ShouldBe_KeyboardAccessible(string url)
    {
        // Note: Only testing on genai-basics because vscode-updates has different tab order
        // due to highlight.js code blocks being focusable

        // Arrange
        await Page.GotoRelativeAsync(url);

        // Act - Tab through page until we hit a TOC link
        var foundTocLink = false;
        for (var i = 0; i < 50; i++)
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

    #region Anchor Navigation

    [Fact]
    public async Task SidebarToc_DirectAnchorNavigation_ShouldScrollTo_CorrectPosition()
    {
        // Arrange - Navigate to page with hash anchor
        await Page.GotoRelativeAsync("/ai/genai-basics#types-of-prompts-and-messages");

        // Wait for page to settle (allow scroll animation to complete)
        await Page.WaitForTimeoutAsync(500);

        // Get initial scroll position (should already be at anchor)
        var initialScrollY = await Page.EvaluateAsync<double>("window.scrollY");

        // Act - Click the TOC link for the same section (should already be there)
        var tocLink = Page.Locator(".sidebar-toc a[href$='#types-of-prompts-and-messages']");
        await tocLink.ClickAsync();

        // Wait for any scroll animation
        await Page.WaitForTimeoutAsync(500);

        // Get final scroll position
        var finalScrollY = await Page.EvaluateAsync<double>("window.scrollY");

        // Assert - Scroll position should be nearly identical (within 200px tolerance for browser differences)
        var scrollDifference = Math.Abs(finalScrollY - initialScrollY);
        scrollDifference.Should().BeLessThan(200,
            $"Expected minimal scroll change when clicking TOC link for current section. Initial: {initialScrollY}px, Final: {finalScrollY}px");
    }

    #endregion

    #region Console Errors

    [Theory]
    [InlineData("/github-copilot/vscode-updates")]
    [InlineData("/ai/genai-basics")]
    public async Task SidebarToc_Page_ShouldNot_HaveConsoleErrors(string url)
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

        // Wait briefly for any console errors to be logged
        await Page.WaitForTimeoutAsync(100);

        // Assert
        // Filter out expected/benign errors:
        // - SRI integrity errors for highlight.js (CDN resources that work despite errors)
        // - Ad-blocker related errors (ERR_CONNECTION_REFUSED, ERR_ADDRESS_INVALID - blocked by DNS-level ad blockers)
        var significantErrors = consoleErrors
            .Where(e => !e.Contains("integrity") || !e.Contains("highlight.js"))
            .Where(e => !e.Contains("ERR_CONNECTION_REFUSED"))
            .Where(e => !e.Contains("ERR_ADDRESS_INVALID"))
            .ToList();

        significantErrors.Should().BeEmpty($"Expected no console errors on {url}, but found: {string.Join(", ", significantErrors)}");
    }

    #endregion

    #region TOC After Client-Side Navigation

    [Fact]
    public async Task SidebarToc_AfterClientSideNavigation_ScrollSpy_ShouldWork()
    {
        // Arrange - Start from Home page
        await Page.GotoRelativeAsync("/");

        // Act - Navigate to GenAI Basics via client-side routing
        await Page.GotoRelativeAsync("/ai");
        await Page.GotoRelativeAsync("/ai/genai-basics");

        // Assert - Verify TOC exists and has active state
        var toc = Page.Locator(".sidebar-toc");
        await BlazorHelpers.AssertElementVisibleAsync(toc);

        var tocLinks = Page.Locator(".sidebar-toc a");
        var linkCount = await tocLinks.CountAsync();
        linkCount.Should().BeGreaterThan(0, "TOC should have navigation links");

        // Wait for TOC scroll spy to initialize and activate at least one link
        var activeTocLinks = Page.Locator(".sidebar-toc a.active");
        await Assertions.Expect(activeTocLinks.First).ToBeVisibleAsync(new() { Timeout = 5000 });

        // Verify at least one TOC link has active class (overview section should be active)
        var activeCount = await activeTocLinks.CountAsync();
        activeCount.Should().BeGreaterThan(0, "At least one TOC link should be active after navigation");
    }

    #endregion
}
