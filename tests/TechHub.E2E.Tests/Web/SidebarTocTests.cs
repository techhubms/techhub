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
public class SidebarTocTests : PlaywrightTestBase
{
    public SidebarTocTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

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

        // Wait for TOC scroll spy JS to finish initialization before clicking.
        // Without this, the scroll-spy active-class update may not fire on CI
        // where JS initialization lags behind Blazor's ready signal. The same
        // wait is used in SidebarToc_Scrolling_ShouldUpdateActiveLink.
        await Page.WaitForTocInitializedAsync();

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

        // Wait for TOC scroll spy JS to finish initialization.
        // Under full Run load (unit + integration tests running in parallel),
        // the scroll spy setup can take longer than page load signals.
        await Page.WaitForTocInitializedAsync();

        // Find a section heading (h2 or h3 with ID)
        var headings = Page.Locator("h2[id], h3[id]");
        var headingCount = await headings.CountAsync();

        if (headingCount < 2)
        {
            Assert.Fail($"Not enough headings with IDs found on {url}");
        }

        // Act - Scroll to second heading using JS window.scrollTo with behavior: 'instant'.
        // This fires a synchronous scroll event that the scroll spy listens for.
        // Avoids Mouse.WheelAsync which can hang under parallel Chrome load.
        var secondHeadingY = await Page.EvaluateAsync<int>(
            "document.querySelectorAll('h2[id], h3[id]')[1].getBoundingClientRect().top + window.scrollY - 150"
        );
        await Page.EvaluateAsync($"window.scrollTo({{ top: {secondHeadingY}, behavior: 'instant' }})");

        // Wait for scroll to complete (position stabilizes)
        await Page.WaitForConditionAsync(
            @$"() => Math.abs(window.scrollY - {secondHeadingY}) < 100");

        // Assert - Active TOC link should update via the scroll spy's own
        // scroll event → rAF → updateActiveHeading chain. No manual nudge:
        // we're testing that the mechanism works end-to-end.
        // Uses centralized E2ETimeout (60s safety net)
        // to accommodate rAF throttling + scrollend delays under CI load.
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

        // Wait for TOC scroll spy JS to finish initialization.
        // Under full Run load, the scroll spy setup can take longer than page load signals.
        await Page.WaitForTocInitializedAsync();

        // Get last heading with ID
        var headings = Page.Locator("h2[id], h3[id]");
        var headingCount = await headings.CountAsync();

        if (headingCount == 0)
        {
            Assert.Fail($"No headings with IDs found on {url}");
        }

        var lastHeading = headings.Last;
        var lastHeadingId = await lastHeading.GetAttributeAsync("id");

        // Act - Scroll to bottom using JS window.scrollTo with behavior: 'instant'.
        // This fires a synchronous scroll event that the scroll spy listens for.
        // Avoids Mouse.WheelAsync which can hang under parallel Chrome load.
        var scrollHeight = await Page.EvaluateAsync<int>("document.documentElement.scrollHeight - window.innerHeight");
        await Page.EvaluateAsync($"window.scrollTo({{ top: {scrollHeight}, behavior: 'instant' }})");

        // Wait for scroll to reach bottom (use navigation timeout for scroll operations)
        await Page.WaitForConditionAsync(
            @"() => Math.abs((window.innerHeight + window.scrollY) - document.documentElement.scrollHeight) < 50");

        // Assert - Last TOC link should become active via the scroll spy's own
        // scroll event → rAF → updateActiveHeading → bottom-of-page detection chain.
        // No manual nudge: we're testing that the mechanism works end-to-end.
        var lastTocLink = Page.Locator($".sidebar-toc a[href$='#{lastHeadingId}']");

        // Use Playwright's auto-retrying assertion with centralized E2ETimeout
        // (60s safety net) to accommodate rAF throttling
        // + scrollend delays under CI load.
        await lastTocLink.AssertHasClassAsync(
            new System.Text.RegularExpressions.Regex(".*active.*"));
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

        // Wait for Mermaid diagrams to finish rendering. This page has multiple diagrams
        // that render asynchronously after page load. Until they complete, layout shifts
        // above the target heading cause the scroll position to change — making the
        // "no scroll change" assertion unreliable.
        await Page.WaitForBlazorReadyAsync();

        // Also wait for scroll position to fully settle after Mermaid layout shifts.
        // Browser scroll anchoring adjusts scrollY asynchronously to compensate for
        // content height changes above the viewport.
        await Page.WaitForConditionAsync(
            @"() => {
                if (!window.__scrollCheck) window.__scrollCheck = { lastY: -1, stableFrames: 0 };
                const c = window.__scrollCheck;
                if (Math.abs(window.scrollY - c.lastY) < 1) {
                    c.stableFrames++;
                } else {
                    c.stableFrames = 0;
                }
                c.lastY = window.scrollY;
                return c.stableFrames >= 5;
            }");

        // Get initial scroll position (fully settled after mermaid rendering + scroll anchoring)
        var initialScrollY = await Page.EvaluateAsync<double>("window.scrollY");

        // Act - Click the TOC link for the same section
        var tocLink = Page.Locator(".sidebar-toc a[href$='#types-of-prompts-and-messages']");
        await tocLink.ClickAndWaitForScrollAsync();

        // Assert - The heading should be visible in the viewport after the TOC link click.
        // Under slow network, images and Mermaid diagrams can load after scrollend fires,
        // shifting the heading's viewport position without triggering another scrollend.
        // Wait for the page layout to fully settle (no DOM mutations / resize events for
        // 150 ms) so we assert the position the user actually sees.
        await Page.WaitForLayoutSettledAsync();

        var headingTop = await Page.EvaluateAsync<double>(@"
            () => {
                const el = document.getElementById('types-of-prompts-and-messages');
                return el ? el.getBoundingClientRect().top : -1;
            }");

        // Heading must be visible in the 1920×1080 viewport.  After scroll+layout settle,
        // lazy-loaded images above the heading may have pushed it down, but it must still
        // be on screen.  The upper bound is the viewport height minus a small margin.
        headingTop.Should().BeGreaterThanOrEqualTo(-5,
            $"Heading should not be above the viewport. ScrollY before click: {initialScrollY}px");
        headingTop.Should().BeLessThanOrEqualTo(1020,
            $"Heading should be visible in the viewport after TOC link click. Heading top: {headingTop}px, initial scroll: {initialScrollY}px");
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

        // Assert
        // Filter out expected/benign errors:
        // - SRI integrity errors for highlight.js (CDN resources that work despite errors)
        // - Ad-blocker related errors (ERR_CONNECTION_REFUSED, ERR_ADDRESS_INVALID - blocked by DNS-level ad blockers)
        // - Permissions policy violations for features not enabled (e.g. compute-pressure used by third-party scripts)
        var significantErrors = consoleErrors
            .Where(e => !e.Contains("integrity") || !e.Contains("highlight.js"))
            .Where(e => !e.Contains("ERR_CONNECTION_REFUSED"))
            .Where(e => !e.Contains("ERR_ADDRESS_INVALID"))
            .Where(e => !e.Contains("ERR_NAME_NOT_RESOLVED"))
            .Where(e => !e.Contains("Permissions policy violation"))
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

        // Wait for TOC scroll spy JS to finish initialization after client-side navigation.
        // Under full Run load (unit + integration tests running), the scroll spy
        // setup and initial heading scan needs a generous timeout.
        await Page.WaitForTocInitializedAsync();

        // Scroll down so a heading passes above the detection line (30% from top).
        // The scroll spy intentionally leaves no heading active when at scroll-top=0,
        // so we must scroll to trigger activation.
        await Page.Mouse.WheelAsync(0, 400);
        await Page.WaitForConditionAsync(
            "() => window.scrollY > 100");

        // Wait for at least one TOC link to become active after scrolling.
        // Uses E2ETimeout because the scroll spy uses rAF throttling
        // and scrollend events, which under load can be delayed.
        var activeTocLinks = Page.Locator(".sidebar-toc a.active");
        await BlazorHelpers.AssertElementVisibleAsync(activeTocLinks.First);

        // Verify at least one TOC link has active class (overview section should be active)
        var activeCount = await activeTocLinks.CountAsync();
        activeCount.Should().BeGreaterThan(0, "At least one TOC link should be active after navigation");
    }

    #endregion
}
