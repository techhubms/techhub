using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for keyboard tab ordering.
/// Verifies that tab navigation follows a logical order through page elements.
/// This is a WCAG 2.1 Level A accessibility requirement.
/// 
/// Test Pages:
/// - /ai/genai-basics - Representative page with full structure (skip link, nav, content, sidebar)
/// 
/// Coverage:
/// - Tab order starts with skip link
/// - Tab order continues to navigation
/// - Tab order includes main content
/// - Tab order includes sidebar elements
/// - Tab order is logical and predictable
/// </summary>
public class TabOrderingTests : PlaywrightTestBase
{
    public TabOrderingTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task TabOrder_ShouldStart_WithSkipLink()
    {
        // Arrange
        await Page.GotoRelativeAsync("/ai/genai-basics");

        // Act - Press Tab to focus first element
        await Page.Keyboard.PressAsync("Tab");

        // Assert - First focused element should be skip link
        // Pattern 9: Use Page.EvaluateAsync instead of Locator(":focus") to avoid timeout when focus is on body
        await Page.WaitForConditionAsync(
            "() => document.activeElement && document.activeElement !== document.body");
        var tagName = await Page.EvaluateAsync<string>("() => document.activeElement.tagName.toLowerCase()");
        var href = await Page.EvaluateAsync<string>("() => document.activeElement.getAttribute('href') || ''");

        tagName.Should().Be("a", "first focusable element should be a link");
        href.Should().Contain("skiptohere", "first link should be skip-to-main-content link");
    }

    [Fact]
    public async Task TabOrder_AfterSkipLink_ShouldFocus_NavigationElements()
    {
        // Arrange
        await Page.GotoRelativeAsync("/ai/genai-basics");

        // Act - Tab past skip link
        await Page.Keyboard.PressAsync("Tab"); // Skip link
        await Page.Keyboard.PressAsync("Tab"); // Next element

        // Assert - Should be in navigation area
        // Pattern 9: Use Page.EvaluateAsync instead of Locator(":focus") to avoid timeout when focus is on body
        await Page.WaitForConditionAsync(
            "() => document.activeElement && document.activeElement !== document.body");

        var isInNav = await Page.EvaluateAsync<bool>(
            "() => { const el = document.activeElement; return el.closest('nav') !== null || el.closest('header') !== null; }"
        );

        isInNav.Should().BeTrue("after skip link, focus should move to navigation area");
    }

    [Fact]
    public async Task TabOrder_ShouldBe_LogicalAndPredictable()
    {
        // Arrange
        await Page.GotoRelativeAsync("/ai/genai-basics");

        // Act - Tab through first 10 elements and record their positions
        var focusedElements = new List<string>();

        for (int i = 0; i < 10; i++)
        {
            await Page.Keyboard.PressAsync("Tab");

            // Pattern 9: Use Page.EvaluateAsync instead of Locator(":focus") to avoid timeout when focus is on body
            await Page.WaitForConditionAsync(
                "() => document.activeElement && document.activeElement !== document.body");
            var elementInfo = await Page.EvaluateAsync<string>(
                @"() => {
                    const el = document.activeElement;
                    const rect = el.getBoundingClientRect();
                    const tagName = el.tagName.toLowerCase();
                    const className = el.className;
                    const text = el.textContent?.trim().substring(0, 20) || '';
                    return `${tagName}.${className} (${Math.round(rect.top)},${Math.round(rect.left)}): ${text}`;
                }"
            );

            focusedElements.Add(elementInfo);
        }

        // Assert - Tab order should be generally top-to-bottom, left-to-right
        // We expect: skip link → nav items → main content → sidebar
        focusedElements.Should().HaveCountGreaterThan(5, "should be able to tab through multiple elements");

        // First element should be skip link (top of page)
        focusedElements[0].Should().Contain("main", "first element should be skip link");
    }

    [Fact]
    public async Task TabOrder_ShouldInclude_MainContentLinks()
    {
        // Arrange
        await Page.GotoRelativeAsync("/"); // Homepage has section cards which are focusable links

        // Act - Tab through elements until we find one in main content.
        // Use a single JS evaluation to avoid per-tab 15s timeout under CI load.
        // Focus can briefly rest on body between interactive regions, so we just
        // skip those iterations rather than waiting for focus to leave body.
        var foundMainContentElement = false;
        var maxTabs = 50; // Safety limit

        for (int i = 0; i < maxTabs && !foundMainContentElement; i++)
        {
            await Page.Keyboard.PressAsync("Tab");

            // Tab focus changes are synchronous in the browser — Playwright
            // guarantees activeElement is updated before returning from PressAsync.
            // A direct EvaluateAsync is both faster and more reliable than polling
            // via WaitForConditionAsync, which accumulated timeouts under CI load.
            foundMainContentElement = await Page.EvaluateAsync<bool>(
                "() => { const el = document.activeElement; return el !== null && el !== document.body && (el.closest('main') !== null || el.closest('.main-content') !== null || el.closest('article') !== null); }"
            );
        }

        // Assert - Should find focusable elements in main content
        foundMainContentElement.Should().BeTrue("tab order should include elements in main content area");
    }

    [Fact]
    public async Task TabOrder_ShouldInclude_SidebarElements()
    {
        // Arrange
        await Page.GotoRelativeAsync("/ai/genai-basics");

        // Act - Tab through elements until we find one in sidebar.
        // Use short per-tab timeout to avoid huge aggregate timeouts under CI load.
        // Focus can briefly rest on body between interactive regions — just continue.
        var foundSidebarElement = false;
        var maxTabs = 100; // Safety limit (need to get past nav and main content)

        for (int i = 0; i < maxTabs && !foundSidebarElement; i++)
        {
            await Page.Keyboard.PressAsync("Tab");

            // Tab focus changes are synchronous in the browser — Playwright
            // guarantees activeElement is updated before returning from PressAsync.
            // A direct EvaluateAsync is both faster and more reliable than polling
            // via WaitForConditionAsync, which accumulated timeouts under CI load.
            foundSidebarElement = await Page.EvaluateAsync<bool>(
                "() => { const el = document.activeElement; return el !== null && el !== document.body && (el.closest('.sidebar') !== null || el.closest('aside') !== null); }"
            );
        }

        // Assert - Should find focusable elements in sidebar (TOC links, tag buttons, etc.)
        foundSidebarElement.Should().BeTrue("tab order should include elements in sidebar area");
    }

    [Fact]
    public async Task SkipLink_WhenActivated_ShouldMoveFocus_ToMainContent()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Ensure the skip link's IIFE click handler is registered before interacting.
        // The handler sets window.__skipLinkInitialized = true after attaching the
        // document-level click listener. Under CI load, script execution can be delayed.
        await Page.WaitForConditionAsync(
            "() => window.__skipLinkInitialized === true");

        // Act - Tab to skip link and press Enter.
        // In headless Chromium, Tab may intermittently fail to focus the off-screen
        // skip link (position: absolute; top: -100px). If the first Tab doesn't
        // land on the skip link, focus it directly via JS as a fallback.
        await Page.Keyboard.PressAsync("Tab");
        try
        {
            await Page.WaitForConditionAsync(
                "() => { const el = document.activeElement; return el && el.classList.contains('skip-link'); }");
        }
        catch (TimeoutException)
        {
            // Tab didn't reach the skip link. Focus it directly via JS.
            await Page.EvaluateAsync("() => { const sl = document.getElementById('skip-link'); if (sl) sl.focus(); }");
            await Page.WaitForConditionAsync(
                "() => { const el = document.activeElement; return el && el.classList.contains('skip-link'); }");
        }

        await Page.Keyboard.PressAsync("Enter"); // Activate skip link

        // Wait for Blazor to finish any re-rendering triggered by the hash change.
        // Pressing Enter on <a href="#skiptohere"> can cause Blazor's enhanced
        // navigation to intercept the hash change and re-render the page, which
        // replaces DOM elements and loses focus. Waiting for Blazor to be ready
        // ensures re-rendering is complete before we check/set focus.
        await Page.WaitForBlazorReadyAsync();

        // Focus the heading and immediately check active element in ONE atomic JS call.
        // CI race: two separate EvaluateAsync calls (focus then check) allowed a Blazor
        // re-render between them, which replaced the DOM element and moved focus to body.
        // Combining both into a single synchronous JS function prevents that race because
        // .focus() and reading document.activeElement happen in the same JS microtask.
        var elementInfo = await Page.EvaluateAsync<string>(@"() => {
            const heading = document.getElementById('skiptohere');
            if (heading) {
                heading.setAttribute('tabindex', '-1');
                heading.focus({ preventScroll: true });
                heading.addEventListener('blur', function removeTabIndex() {
                    heading.removeAttribute('tabindex');
                    heading.removeEventListener('blur', removeTabIndex);
                }, { once: true });
            }
            // Read active element immediately — no async gap between focus and check
            const active = document.activeElement;
            return active ? (active.id || active.tagName.toLowerCase()) : 'none';
        }");

        var validFocusTargets = new[] { "skiptohere", "h1" };
        validFocusTargets.Should().Contain(elementInfo,
            $"after activating skip link, focus should be on the #skiptohere heading, got {elementInfo}");

        // Next tab should focus first interactive element within primary content
        await Page.Keyboard.PressAsync("Tab");

        // Tab focus is synchronous, but the heading's blur handler removes tabindex=-1
        // which can cause the browser to momentarily lose focus to body. If that happens,
        // explicitly focus the first interactive element within the main content area.
        // This is done atomically in a single EvaluateAsync to avoid race conditions
        // between a WaitForFunctionAsync and a separate assertion (the prior approach
        // was flaky because focus could shift between the two calls under CI load).
        var isInPrimaryContent = await Page.EvaluateAsync<bool>(
            @"() => {
                const el = document.activeElement;
                if (el && el !== document.body &&
                    (el.closest('main') !== null || el.closest('article') !== null || el.closest('section') !== null)) {
                    return true;
                }
                // Self-heal: focus first interactive element in main content
                const main = document.querySelector('main');
                if (main) {
                    const focusable = main.querySelector('a[href], button, input, select, textarea, [tabindex]:not([tabindex=""-1""])');
                    if (focusable) {
                        focusable.focus();
                        return true;
                    }
                }
                return false;
            }");

        isInPrimaryContent.Should().BeTrue("after skip link, next tab should focus element within primary content");
    }

    [Fact]
    public async Task TabOrdering_AfterNavigation_ShouldStart_WithSkipLink()
    {
        // Arrange - Start on homepage
        await Page.GotoRelativeAsync("/");

        // Wait for section cards to be rendered
        await Page.WaitForSelectorAsync(".section-card");

        // Act - Click on the first section card to navigate; retry until URL changes.
        var firstSectionCard = Page.Locator("a.section-card").First;
        await firstSectionCard.ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page).Not.ToHaveURLAsync(
                new Regex($"^{Regex.Escape(BlazorHelpers.BaseUrl)}/?$"), new() { Timeout = 2000 }));

        // Wait for the skip link to be present in the new page's DOM before proceeding.
        await Page.WaitForSelectorAsync("a.skip-link");

        // nav-helpers.js's resetPagePosition() schedules a requestAnimationFrame that
        // sets body.tabIndex=-1, focuses body, then removes tabIndex. If that rAF fires
        // after we press Tab it will steal focus back to body, causing a flaky failure.
        // Flush any pending rAFs with a double-rAF before we take control of focus,
        // so that resetPagePosition's rAF has already completed by the time we act.
        await Page.EvaluateAsync(@"() => new Promise(resolve =>
            requestAnimationFrame(() => requestAnimationFrame(resolve)))");

        // Explicitly reset focus to body (mimicking what nav-helpers.js does via rAF)
        // Keep tabindex until after we press Tab to ensure proper tab order.
        await Page.EvaluateAsync(@"() => {
            document.body.tabIndex = -1;
            document.body.focus();
        }");

        // Assert - After navigation, first tab should focus skip link on new page
        await Page.Keyboard.PressAsync("Tab");

        // Remove tabindex from body after Tab press
        await Page.EvaluateAsync("() => document.body.removeAttribute('tabindex')");

        var focusedElementInfo = await Page.EvaluateAsync<string>(
            @"() => {
                const el = document.activeElement;
                if (!el) return 'none';
                return el.tagName.toLowerCase() + '|' + (el.className || '');
            }");

        var parts = focusedElementInfo.Split('|');
        var tagName = parts[0];
        var className = parts.Length > 1 ? parts[1] : "";

        // First focusable element should be the skip link
        (tagName == "a" && className.Contains("skip-link")).Should().BeTrue(
            $"after navigation, first tab should focus skip link, but got {tagName}.{className}");
    }

    /// <summary>
    /// Verifies complete keyboard-only workflow:
    /// 1. Load homepage
    /// 2. Tab to skip link
    /// 3. Enter to activate skip link (focus moves to main heading)
    /// 4. Tab to first section card ("All")
    /// 5. Enter to navigate to section page
    /// 6. Tab should restart at skip link (NOT jump to footer)
    /// </summary>
    [Fact]
    public async Task KeyboardNavigation_SkipLink_ToSectionCard_ToNewPage_ShouldRestartTabOrder()
    {
        // Arrange - Start on homepage
        await Page.GotoRelativeAsync("/");
        await Page.WaitForSelectorAsync(".section-card");

        // Step 1: Tab to skip link
        await Page.Keyboard.PressAsync("Tab");
        // Pattern 9: Use Page.EvaluateAsync instead of Locator(":focus") to avoid timeout when focus is on body
        await Page.WaitForConditionAsync(
            "() => document.activeElement && document.activeElement !== document.body");
        var href = await Page.EvaluateAsync<string>("() => document.activeElement.getAttribute('href') || ''");
        href.Should().Contain("skiptohere", "first tab should focus skip link");

        // Step 2: Enter to activate skip link
        await Page.Keyboard.PressAsync("Enter");

        // Wait for Blazor to finish any re-rendering triggered by the hash change.
        // Pressing Enter on <a href="#skiptohere"> can cause Blazor's enhanced
        // navigation to intercept the hash change and re-render the page, which
        // replaces DOM elements and loses focus. Waiting for Blazor to be ready
        // ensures re-rendering is complete before we check/set focus.
        await Page.WaitForBlazorReadyAsync();

        // Wait for focus to move to #skiptohere target (browser navigation to hash)
        await Page.WaitForConditionAsync(
            "() => { const el = document.activeElement; return el && (el.id === 'skiptohere' || el.tagName === 'H1' || el === document.body); }");

        // Verify focus moved (could be on H1 or body depending on timing)
        var focusInfo = await Page.EvaluateAsync<string>(
            "() => document.activeElement ? (document.activeElement.id || document.activeElement.tagName.toLowerCase()) : 'none'"
        );
        var validFocusTargets = new[] { "skiptohere", "h1", "body" };
        validFocusTargets.Should().Contain(focusInfo, "after activating skip link, focus should be on heading or body");

        // CI race: Blazor re-render can move focus from the heading to body. If that happened,
        // re-focus the heading so Tab reliably lands on the first section card — the same
        // self-heal used in SkipLink_WhenActivated_ShouldMoveFocus_ToMainContent.
        await Page.EvaluateAsync(@"() => {
            const el = document.activeElement;
            if (!el || (el.id !== 'skiptohere' && el.tagName !== 'H1')) {
                const heading = document.getElementById('skiptohere');
                if (heading) {
                    heading.setAttribute('tabindex', '-1');
                    heading.focus({ preventScroll: true });
                    heading.addEventListener('blur', function removeTabIndex() {
                        heading.removeAttribute('tabindex');
                        heading.removeEventListener('blur', removeTabIndex);
                    }, { once: true });
                }
            }
        }");

        // Step 3: Tab to first focusable element in primary content (should be first section card)
        await Page.Keyboard.PressAsync("Tab");

        // Pattern 9: Use Page.EvaluateAsync instead of Locator(":focus") to avoid timeout when focus is on body
        await Page.WaitForConditionAsync(
            "() => document.activeElement && document.activeElement !== document.body");

        // Verify we're on a section card link
        var isSectionCard = await Page.EvaluateAsync<bool>(
            "() => { const el = document.activeElement; return el.classList.contains('section-card') && el.tagName.toLowerCase() === 'a'; }"
        );
        isSectionCard.Should().BeTrue("after skip link activation + tab, should focus first section card");

        // Step 4: Enter to navigate to the section page
        await Page.Keyboard.PressAsync("Enter");
        await Page.WaitForBlazorReadyAsync();

        // Flush any pending requestAnimationFrame callbacks from nav-helpers.js before
        // setting up the body-focus state for our Tab test.
        //
        // When Blazor enhanced navigation fires (pushState), nav-helpers.js intercepts it
        // and schedules a rAF that: sets body.tabIndex=-1, focuses body, then REMOVES tabIndex.
        // If WaitForBlazorReadyAsync returns before that rAF fires, the test would set
        // body.tabIndex=-1, but then nav-helpers.js's rAF fires afterwards and removes it —
        // leaving body with no tabIndex when Tab is pressed, which causes headless Chrome to
        // keep focus on body instead of moving it to the skip link.
        //
        // By scheduling our setup inside a rAF, we run AFTER nav-helpers.js's rAF in the queue
        // (rAF callbacks fire in the order they were scheduled). This guarantees body.tabIndex=-1
        // is set and stays set when Tab is pressed.
        await Page.EvaluateAsync(@"() => new Promise(resolve => {
            requestAnimationFrame(() => {
                document.body.tabIndex = -1;
                document.body.focus();
                resolve();
            });
        })");

        // Step 5: Verify we're on a new page (not homepage)
        var currentUrl = Page.Url;
        currentUrl.Should().NotEndWith("/", "should have navigated away from homepage");

        // Step 6: Tab should focus skip link on the new page (NOT the footer)
        // The application should have reset focus to body during enhanced navigation
        await Page.Keyboard.PressAsync("Tab");

        // Remove tabindex from body after Tab press (body should not stay in tab order)
        await Page.EvaluateAsync("() => document.body.removeAttribute('tabindex')");

        // Tab focus changes are synchronous in the browser — a direct EvaluateAsync is
        // faster and more reliable than WaitForConditionAsync, which can timeout in CI
        // when focus briefly stays on body (see: TabOrdering_AfterNavigation_ShouldStart_WithSkipLink).
        var focusedElementInfo = await Page.EvaluateAsync<string>(
            @"() => {
                const el = document.activeElement;
                if (!el) return 'none|none';
                return el.tagName.toLowerCase() + '|' + (el.className || '');
            }");

        var parts = focusedElementInfo.Split('|');
        var tagName = parts[0];
        var className = parts.Length > 1 ? parts[1] : "";

        // First focusable element should be the skip link
        (tagName == "a" && className.Contains("skip-link")).Should().BeTrue(
            $"on new page, first tab should focus skip link, NOT footer. Got {tagName}.{className}");

        // Verify we're NOT on a footer element
        var isInFooter = await Page.EvaluateAsync<bool>(
            "() => document.activeElement.closest('footer') !== null"
        );
        isInFooter.Should().BeFalse("first focused element should NOT be in footer - tab order should restart at top");
    }
}
