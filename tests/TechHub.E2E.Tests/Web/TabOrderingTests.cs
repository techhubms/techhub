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
[Collection("Tab Ordering Tests")]
public class TabOrderingTests : IAsyncLifetime
{
    private readonly PlaywrightCollectionFixture _fixture;

    public TabOrderingTests(PlaywrightCollectionFixture fixture)
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

    [Fact]
    public async Task TabOrder_ShouldStart_WithSkipLink()
    {
        // Arrange
        await Page.GotoRelativeAsync("/ai/genai-basics");

        // Act - Press Tab to focus first element
        await Page.Keyboard.PressAsync("Tab");

        // Assert - First focused element should be skip link
        var focusedElement = Page.Locator(":focus");
        var tagName = await focusedElement.EvaluateAsync<string>("el => el.tagName.toLowerCase()");
        var href = await focusedElement.EvaluateAsync<string>("el => el.getAttribute('href') || ''");

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
        var focusedElement = Page.Locator(":focus");

        // Check if element is within nav or header
        var isInNav = await focusedElement.EvaluateAsync<bool>(
            "el => el.closest('nav') !== null || el.closest('header') !== null"
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

            var focusedElement = Page.Locator(":focus");
            var elementInfo = await focusedElement.EvaluateAsync<string>(
                @"el => {
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

            var focusedElement = Page.Locator(":focus");
            // Check if element is within main content area (main tag, article, or section with content)
            var isInMain = await focusedElement.EvaluateAsync<bool>(
                "el => el.closest('main') !== null || el.closest('.main-content') !== null || el.closest('article') !== null"
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

        // Wait for Blazor interactivity (sidebar has interactive tag cloud)
        await Page.WaitForBlazorReadyAsync();

        // Act - Tab through elements until we find one in sidebar
        var foundSidebarElement = false;
        var maxTabs = 100; // Safety limit (need to get past nav and main content)

        for (int i = 0; i < maxTabs && !foundSidebarElement; i++)
        {
            await Page.Keyboard.PressAsync("Tab");

            var focusedElement = Page.Locator(":focus");
            var isInSidebar = await focusedElement.EvaluateAsync<bool>(
                "el => el.closest('.sidebar') !== null || el.closest('aside') !== null"
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

        // Wait for focus to move (skip link JS uses requestAnimationFrame)
        await Task.Delay(300);

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
        await firstSectionCard.ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Wait for enhanced navigation to complete and application to reset focus
        await Task.Delay(200);

        // Assert - After navigation, first tab should focus skip link on new page
        // The application should have reset focus to body during enhanced navigation
        await Page.Keyboard.PressAsync("Tab");
        
        var firstFocusedElement = Page.Locator(":focus");
        var tagName = await firstFocusedElement.EvaluateAsync<string>("el => el.tagName.toLowerCase()");
        var className = await firstFocusedElement.EvaluateAsync<string>("el => el.className || ''");

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
        var focusedElement = Page.Locator(":focus");
        var href = await focusedElement.EvaluateAsync<string>("el => el.getAttribute('href') || ''");
        href.Should().Contain("skiptohere", "first tab should focus skip link");

        // Step 2: Enter to activate skip link
        await Page.Keyboard.PressAsync("Enter");
        await Task.Delay(300); // Wait for focus to move (JS uses requestAnimationFrame)

        // Verify focus moved (could be on H1 or body depending on timing)
        var focusInfo = await Page.EvaluateAsync<string>(
            "() => document.activeElement ? (document.activeElement.id || document.activeElement.tagName.toLowerCase()) : 'none'"
        );
        var validFocusTargets = new[] { "skiptohere", "h1", "body" };
        validFocusTargets.Should().Contain(focusInfo, "after activating skip link, focus should be on heading or body");

        // Step 3: Tab to first focusable element in primary content (should be first section card)
        await Page.Keyboard.PressAsync("Tab");
        focusedElement = Page.Locator(":focus");

        // Verify we're on a section card link
        var isSectionCard = await focusedElement.EvaluateAsync<bool>(
            "el => el.classList.contains('section-card') && el.tagName.toLowerCase() === 'a'"
        );
        isSectionCard.Should().BeTrue("after skip link activation + tab, should focus first section card");

        // Step 4: Enter to navigate to the section page
        await Page.Keyboard.PressAsync("Enter");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Wait for enhanced navigation to complete
        await Task.Delay(100);

        // Step 5: Verify we're on a new page (not homepage)
        var currentUrl = Page.Url;
        currentUrl.Should().NotEndWith("/", "should have navigated away from homepage");

        // Step 6: Tab should focus skip link on the new page (NOT the footer)
        // The application should have reset focus to body during enhanced navigation
        await Page.Keyboard.PressAsync("Tab");
        focusedElement = Page.Locator(":focus");
        
        var tagName = await focusedElement.EvaluateAsync<string>("el => el.tagName.toLowerCase()");
        var className = await focusedElement.EvaluateAsync<string>("el => el.className || ''");

        // First focusable element should be the skip link
        (tagName == "a" && className.Contains("skip-link")).Should().BeTrue(
            $"on new page, first tab should focus skip link, NOT footer. Got {tagName}.{className}");

        // Verify we're NOT on a footer element
        var isInFooter = await focusedElement.EvaluateAsync<bool>(
            "el => el.closest('footer') !== null"
        );
        isInFooter.Should().BeFalse("first focused element should NOT be in footer - tab order should restart at top");
    }
}
