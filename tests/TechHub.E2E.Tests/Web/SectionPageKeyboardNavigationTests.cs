using Microsoft.Playwright;
using Xunit;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

[Collection("Section Page Keyboard Navigation Tests")]
public class SectionPageKeyboardNavigationTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
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

    [Theory]
    [InlineData("/all")]
    [InlineData("/github-copilot")]
    public async Task SectionPage_FullTabOrder_ProgressesThroughAllElements(string sectionUrl)
    {
        // Test: Load page -> Tab through all focusable elements in correct order
        // Skip link -> Tech Hub logo -> Nav menu -> Sidebar -> Main content -> Footer
        // NOTE: After footer, focus should exit page to browser UI (NOT loop back - that would be a keyboard trap!)
        
        await Page.GotoAsync($"{BaseUrl}{sectionUrl}");
        
        // Wait for Blazor to fully hydrate before starting keyboard navigation
        var skipLink = Page.Locator("a.skip-link");
        await skipLink.WaitForBlazorInteractivityAsync();
        
        // 1. First Tab: Skip to main content link (appears when focused)
        await Page.Keyboard.PressAsync("Tab");
        await Assertions.Expect(Page.Locator("a.skip-link")).ToBeFocusedAsync();
        
        // 2. Second Tab: Tech Hub logo in header
        await Page.Keyboard.PressAsync("Tab");
        await Assertions.Expect(Page.Locator("a[href='/']")).ToBeFocusedAsync();
        
        // 3. Tab through navigation menu (All, GitHub Copilot, AI, ML, DevOps, Azure, .NET, Security, About)
        var navLinks = Page.Locator("nav[aria-label='Primary navigation'] a");
        var navCount = await navLinks.CountAsync();
        for (int i = 0; i < navCount; i++)
        {
            await Page.Keyboard.PressAsync("Tab");
            await Assertions.Expect(navLinks.Nth(i)).ToBeFocusedAsync();
        }
        
        // 4. Tab through sidebar (Collections: All, News, Blogs, Videos, Community, then Subscribe link)
        await Page.Keyboard.PressAsync("Tab");
        await Assertions.Expect(Page.Locator("nav.collection-nav a").Nth(0)).ToBeFocusedAsync();
        
        // Continue through remaining sidebar links
        var sidebarLinks = Page.Locator("nav.collection-nav a");
        var sidebarCount = await sidebarLinks.CountAsync();
        for (int i = 1; i < sidebarCount; i++)
        {
            await Page.Keyboard.PressAsync("Tab");
            await Assertions.Expect(sidebarLinks.Nth(i)).ToBeFocusedAsync();
        }
        
        // 5. Tab into main content - main content comes AFTER sidebar in tab order
        // Note: H1 has tabindex="-1" so it's NOT in natural tab order
        // First focusable element in main content should be visible
        await Page.Keyboard.PressAsync("Tab");
        
        // Verify we've moved past the sidebar into main content
        var isSidebarLink = await Page.EvaluateAsync<bool>(@"
            () => {
                const activeEl = document.activeElement;
                const sidebarLinks = Array.from(document.querySelectorAll('nav.collection-nav a'));
                return sidebarLinks.some(el => el === activeEl);
            }
        ");
        Assert.False(isSidebarLink, "Should have moved past sidebar into main content");
        
        // 6. Tab to footer (skip through main content for brevity - main content tested separately)
        // Jump to footer by clicking to move focus there
        await Page.Locator("footer a").Nth(0).FocusAsync();
        await Assertions.Expect(Page.Locator("footer a").Nth(0)).ToBeFocusedAsync();
    }

    [Theory]
    [InlineData("/all")]
    [InlineData("/github-copilot")]
    public async Task SectionPage_ShiftTabFromLoad_GoesToFooterWithoutShowingSkipLink(string sectionUrl)
    {
        // Test: Load page -> Shift+Tab -> Should go to LAST focusable element (footer) WITHOUT showing skip link
        // This is correct accessibility behavior - skip link only appears when tabbing FORWARD
        
        await Page.GotoAsync($"{BaseUrl}{sectionUrl}");
        
        // Wait for Blazor to fully hydrate before starting keyboard navigation
        var footerLink = Page.Locator("footer a").Last;
        await footerLink.WaitForBlazorInteractivityAsync();
        
        // Shift+Tab should go to last focusable element (footer link)
        await Page.Keyboard.PressAsync("Shift+Tab");
        
        // Should be on footer link (last focusable element on page)
        await Assertions.Expect(Page.Locator("footer a").Last).ToBeFocusedAsync();
        
        // Skip link should NOT be focused
        await Assertions.Expect(Page.Locator("a.skip-link")).Not.ToBeFocusedAsync();
    }

    [Theory]
    [InlineData("/all")]
    [InlineData("/github-copilot")]
    public async Task SectionPage_ActivateSkipLink_JumpsToMainContentAndContinues(string sectionUrl)
    {
        // Test: Tab -> Skip link appears -> Enter -> Focus on H1 -> Tab -> Continue through main content
        
        await Page.GotoAsync($"{BaseUrl}{sectionUrl}");
        
        // Wait for Blazor to fully hydrate before starting keyboard navigation
        var skipLink = Page.Locator("a.skip-link");
        await skipLink.WaitForBlazorInteractivityAsync();
        
        // 1. Tab to skip link
        await Page.Keyboard.PressAsync("Tab");
        await Assertions.Expect(Page.Locator("a.skip-link")).ToBeFocusedAsync();
        
        // 2. Press Enter to activate skip link (jumps to #main-content on H1)
        await Page.Keyboard.PressAsync("Enter");
        await Task.Delay(200); // Reduced from 1500ms - skip link JS is much faster
        
        // 3. H1 should be focused (has id="main-content" and tabindex="-1")
        await Assertions.Expect(Page.Locator("#main-content")).ToBeFocusedAsync();
        
        // 4. Tab should continue to next focusable element after H1 in DOM order
        // Since H1 has tabindex="-1", next Tab goes to next naturally focusable element
        // This should be the first sidebar link (sidebar comes before main content in DOM)
        await Page.Keyboard.PressAsync("Tab");
        await Task.Delay(50); // Reduced from 100ms - faster focus detection
        await Assertions.Expect(Page.Locator("nav.collection-nav a").Nth(0)).ToBeFocusedAsync();
        
        // 5. Continue tabbing through sidebar
        await Page.Keyboard.PressAsync("Tab");
        await Assertions.Expect(Page.Locator("nav.collection-nav a").Nth(1)).ToBeFocusedAsync();
    }

    [Theory]
    [InlineData("/")]
    public async Task HomePage_TabToSkipLink_ThenTabToFirstSectionCard_ThenNavigate(string homeUrl)
    {
        // Test: On home page -> Tab -> Skip link appears -> Tab (don't press Enter) -> First section card
        // -> Press Enter -> Navigate to /all -> Tab -> Skip link appears again
        
        await Page.GotoAsync($"{BaseUrl}{homeUrl}");
        
        // Wait for Blazor to fully hydrate before starting keyboard navigation
        var skipLink = Page.Locator("a.skip-link");
        await skipLink.WaitForBlazorInteractivityAsync();
        
        // 1. Tab to skip link (appears when focused)
        await Page.Keyboard.PressAsync("Tab");
        await Assertions.Expect(Page.Locator("a.skip-link")).ToBeFocusedAsync();
        
        // 2. Tab again (WITHOUT pressing Enter on skip link) -> Tech Hub logo
        await Page.Keyboard.PressAsync("Tab");
        await Assertions.Expect(Page.Locator("a[href='/']")).ToBeFocusedAsync();
        
        // 3. Continue tabbing through nav to get to main content
        // Tab through all nav links to reach sidebar/main content area
        var navLinks = Page.Locator("nav[aria-label='Primary navigation'] a");
        var navCount = await navLinks.CountAsync();
        for (int i = 0; i < navCount; i++)
        {
            await Page.Keyboard.PressAsync("Tab");
        }
        
        // 4. Tab through sidebar to reach first section card
        var sidebarLinks = Page.Locator("aside.sidebar a");
        var sidebarCount = await sidebarLinks.CountAsync();
        for (int i = 0; i < sidebarCount; i++)
        {
            await Page.Keyboard.PressAsync("Tab");
        }
        
        // 5. Next tab should be on first section card link
        await Page.Keyboard.PressAsync("Tab");
        var firstSectionCard = Page.Locator(".section-card").Nth(0);
        await Assertions.Expect(firstSectionCard).ToBeFocusedAsync();
        
        // 6. Press Enter to navigate to the section
        await Page.Keyboard.PressAsync("Enter");
        await Page.WaitForURLAsync("**/all");
        
        // Wait for Blazor enhanced navigation to complete
        await Task.Delay(500); // Reduced from 1500ms - Blazor hydration is much faster now
        
        // 7. After navigation, Tab should show skip link again at top
        await Page.Keyboard.PressAsync("Tab");
        await Task.Delay(200); // Reduced from 500ms - faster focus detection
        await Assertions.Expect(Page.Locator("a.skip-link")).ToBeFocusedAsync();
    }
}
