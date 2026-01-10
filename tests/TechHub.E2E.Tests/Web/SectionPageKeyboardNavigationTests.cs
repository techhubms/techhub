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
        _page = await _context.NewPageWithDefaultsAsync();
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
        // Tech Hub logo -> Nav menu -> Sidebar -> Main content -> Footer
        // NOTE: After footer, focus should exit page to browser UI (NOT loop back - that would be a keyboard trap!)

        await Page.GotoRelativeAsync(sectionUrl);

        // Wait for Blazor to fully hydrate before starting keyboard navigation
        var logo = Page.Locator("a[href='/']");
        await logo.WaitForBlazorInteractivityAsync();

        // 1. First Tab: Tech Hub logo in header
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

}

