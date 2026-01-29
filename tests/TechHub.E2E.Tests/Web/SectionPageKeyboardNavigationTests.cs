using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

[Collection("Section Page Keyboard Navigation Tests")]
public class SectionPageKeyboardNavigationTests : IAsyncLifetime
{
    private readonly PlaywrightCollectionFixture _fixture;

    public SectionPageKeyboardNavigationTests(PlaywrightCollectionFixture fixture)
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

    [Theory]
    [InlineData("/all")]
    [InlineData("/github-copilot")]
    public async Task SectionPage_TabOrder_FollowsCorrectNavigationHierarchy(string sectionUrl)
    {
        // Test validates that keyboard navigation progresses through parent containers in the correct order:
        // 1. Main navigation (Primary navigation header)
        // 2. Sub-navigation (horizontal nav below banner)
        // 3. Main content area (cards, filters, etc.)
        // 4. Footer
        // This test checks PARENT CONTAINERS, not specific elements, to avoid brittleness

        await Page.GotoRelativeAsync(sectionUrl);

        // Wait for Blazor to fully hydrate before starting keyboard navigation
        var logo = Page.Locator("a[href='/']");
        await logo.WaitForBlazorInteractivityAsync();

        // CRITICAL: Focus body first so Tab starts from the beginning of the tab order
        await Page.Locator("body").FocusAsync();

        // Helper to get current focus parent container
        async Task<string> GetFocusedContainer()
        {
            return await Page.EvaluateAsync<string>(@"
                () => {
                    const el = document.activeElement;
                    if (!el) return 'none';
                    
                    // Check which major section contains focused element
                    if (el.closest('nav[aria-label=""Primary navigation""]')) return 'main-nav';
                    if (el.closest('nav.sub-nav')) return 'sub-nav';
                    if (el.closest('aside.sidebar')) return 'sidebar';
                    if (el.closest('main')) return 'main-content';
                    if (el.closest('footer')) return 'footer';
                    
                    return 'other';
                }
            ");
        }

        // Track progression through major sections
        var visitedSections = new List<string>();
        var currentSection = "";

        // Tab through page - max 50 tabs to avoid infinite loop
        for (int i = 0; i < 50; i++)
        {
            await Page.Keyboard.PressAsync("Tab");
            var newSection = await GetFocusedContainer();

            // Track section changes
            if (newSection != currentSection && newSection != "none")
            {
                currentSection = newSection;
                if (!visitedSections.Contains(currentSection))
                {
                    visitedSections.Add(currentSection);
                }
            }

            // Stop if we've reached footer (end of page)
            if (currentSection == "footer")
            {
                break;
            }
        }

        // Assert: Should visit sections in correct order
        // Main nav MUST come before sub-nav
        var mainNavIndex = visitedSections.IndexOf("main-nav");
        var subNavIndex = visitedSections.IndexOf("sub-nav");
        (mainNavIndex >= 0).Should().BeTrue("Should tab through main navigation");
        (subNavIndex >= 0).Should().BeTrue("Should tab through sub-navigation");
        (mainNavIndex < subNavIndex).Should().BeTrue("Main navigation should come before sub-navigation");

        // Main content area should come after navigation
        var mainContentIndex = visitedSections.IndexOf("main-content");
        (mainContentIndex >= 0).Should().BeTrue("Should tab through main content area");
        (subNavIndex < mainContentIndex).Should().BeTrue("Sub-navigation should come before main content");

        // Footer should come last (if present)
        var footerIndex = visitedSections.IndexOf("footer");
        if (footerIndex >= 0)
        {
            (mainContentIndex < footerIndex).Should().BeTrue("Main content should come before footer");
        }
    }
}

