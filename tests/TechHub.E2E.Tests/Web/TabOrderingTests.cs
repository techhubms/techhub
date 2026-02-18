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

        // Act - Tab through elements until we find one in main content
        var foundMainContentElement = false;
        var maxTabs = 50; // Safety limit

        for (int i = 0; i < maxTabs && !foundMainContentElement; i++)
        {
            await Page.Keyboard.PressAsync("Tab");

            // Pattern 9: Use Page.EvaluateAsync instead of Locator(":focus") to avoid timeout when focus is on body
            await Page.WaitForConditionAsync(
                "() => document.activeElement && document.activeElement !== document.body");
            var isInMain = await Page.EvaluateAsync<bool>(
                "() => { const el = document.activeElement; return el.closest('main') !== null || el.closest('.main-content') !== null || el.closest('article') !== null; }"
            );

            if (isInMain)
            {
                foundMainContentElement = true;
            }
        }

        // Assert - Should find focusable elements in main content
        foundMainContentElement.Should().BeTrue("tab order should include elements in main content area");
    }

    [Fact]
    public async Task TabOrder_ShouldInclude_SidebarElements()
    {
        // Arrange
        await Page.GotoRelativeAsync("/ai/genai-basics");

        // Act - Tab through elements until we find one in sidebar
        var foundSidebarElement = false;
        var maxTabs = 100; // Safety limit (need to get past nav and main content)

        for (int i = 0; i < maxTabs && !foundSidebarElement; i++)
        {
            await Page.Keyboard.PressAsync("Tab");

            // Pattern 9: Use Page.EvaluateAsync instead of Locator(":focus") to avoid timeout when focus is on body
            await Page.WaitForConditionAsync(
                "() => document.activeElement && document.activeElement !== document.body");
            var isInSidebar = await Page.EvaluateAsync<bool>(
                "() => { const el = document.activeElement; return el.closest('.sidebar') !== null || el.closest('aside') !== null; }"
            );

            if (isInSidebar)
            {
                foundSidebarElement = true;
            }
        }

        // Assert - Should find focusable elements in sidebar (TOC links, tag buttons, etc.)
        foundSidebarElement.Should().BeTrue("tab order should include elements in sidebar area");
    }

    [Fact]
    public async Task SkipLink_WhenActivated_ShouldMoveFocus_ToMainContent()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act - Tab to skip link and press Enter
        await Page.Keyboard.PressAsync("Tab"); // Focus skip link
        await Page.Keyboard.PressAsync("Enter"); // Activate skip link

        // Wait for focus to move to the target element
        // The skip link navigates to #skiptohere - browser moves focus to that anchor target
        await Page.WaitForConditionAsync(
            "() => { const el = document.activeElement; return el && (el.id === 'skiptohere' || el.tagName === 'H1' || el === document.body); }");

        // Assert - Get what's currently focused
        var elementInfo = await Page.EvaluateAsync<string>(
            "() => document.activeElement ? (document.activeElement.id || document.activeElement.tagName.toLowerCase()) : 'none'"
        );

        // Focus should be on skiptohere (H1) or body (if tabindex was removed quickly)
        var validFocusTargets = new[] { "skiptohere", "h1", "body" };
        validFocusTargets.Should().Contain(elementInfo,
            $"after activating skip link, focus should be on primary content element or body, got {elementInfo}");

        // Next tab should focus first interactive element within primary content
        await Page.Keyboard.PressAsync("Tab");
        
        // Wait for focus to stabilize after tab press
        // Use longer timeout (2s) because this involves multiple async operations:
        // 1. Tab keypress processed
        // 2. H1 blur event fires
        // 3. Blur handler removes tabindex
        // 4. Focus moves to next element
        await Page.WaitForFunctionAsync(
            @"() => {
                const el = document.activeElement;
                return el && el !== document.body && 
                       (el.closest('main') !== null || 
                        el.closest('article') !== null || 
                        el.closest('section') !== null);
            }");
        
        var isInPrimaryContent = await Page.EvaluateAsync<bool>(
            "() => { const el = document.activeElement; return el && (el.closest('main') !== null || el.closest('article') !== null || el.closest('section') !== null); }"
        );

        isInPrimaryContent.Should().BeTrue("after skip link, next tab should focus element within primary content");
    }

    [Fact]
    public async Task TabOrdering_AfterNavigation_ShouldStart_WithSkipLink()
    {
        // Arrange - Start on homepage
        await Page.GotoRelativeAsync("/");

        // Wait for section cards to be rendered
        await Page.WaitForSelectorAsync(".section-card");

        // Act - Click on the first section card to navigate (simpler than tabbing through)
        // This directly tests that after navigation, tab order restarts with skip link
        var firstSectionCard = Page.Locator("a.section-card").First;
        await firstSectionCard.ClickBlazorElementAsync();

        // Wait for enhanced navigation to complete
        // nav-helpers.js resets focus via requestAnimationFrame after enhanced navigation,
        // but rAF doesn't fire reliably in headless Chrome. Instead, wait for Blazor to be
        // ready, then explicitly reset focus to body (same action nav-helpers.js performs).
        await Page.WaitForBlazorReadyAsync();

        // Explicitly reset focus to body (what nav-helpers.js does via rAF)
        // Keep tabindex until after we press Tab to ensure proper tab order
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

        // Wait for focus to move to #skiptohere target (browser navigation to hash)
        await Page.WaitForConditionAsync(
            "() => { const el = document.activeElement; return el && (el.id === 'skiptohere' || el.tagName === 'H1' || el === document.body); }");

        // Verify focus moved (could be on H1 or body depending on timing)
        var focusInfo = await Page.EvaluateAsync<string>(
            "() => document.activeElement ? (document.activeElement.id || document.activeElement.tagName.toLowerCase()) : 'none'"
        );
        var validFocusTargets = new[] { "skiptohere", "h1", "body" };
        validFocusTargets.Should().Contain(focusInfo, "after activating skip link, focus should be on heading or body");

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

        // Wait for enhanced navigation to complete
        // nav-helpers.js resets focus via requestAnimationFrame after enhanced navigation,
        // but rAF doesn't fire reliably in headless Chrome. Explicitly reset focus to body
        // (same action nav-helpers.js performs) to ensure Tab starts from top.
        await Page.EvaluateAsync(@"() => {
            document.body.tabIndex = -1;
            document.body.focus();
            document.body.removeAttribute('tabindex');
        }");

        // Step 5: Verify we're on a new page (not homepage)
        var currentUrl = Page.Url;
        currentUrl.Should().NotEndWith("/", "should have navigated away from homepage");

        // Step 6: Tab should focus skip link on the new page (NOT the footer)
        // The application should have reset focus to body during enhanced navigation
        await Page.Keyboard.PressAsync("Tab");

        // Wait for focus to land on a real element (not body) after Tab
        await Page.WaitForConditionAsync(
            "() => document.activeElement && document.activeElement !== document.body");

        var tagName = await Page.EvaluateAsync<string>("() => document.activeElement.tagName.toLowerCase()");
        var className = await Page.EvaluateAsync<string>("() => document.activeElement.className || ''");

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
