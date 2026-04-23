using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for mobile navigation: hamburger menu, collapsible sidebar, and compact header.
/// Tests run at mobile viewport (375x812) to trigger responsive behavior at ≤1024px breakpoint.
/// </summary>
public class MobileNavigationTests : PlaywrightTestBase
{
    private const int MobileWidth = 375;
    private const int MobileHeight = 812;

    public MobileNavigationTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync();
        // Set mobile viewport to trigger ≤1024px breakpoint
        await Page.SetViewportSizeAsync(MobileWidth, MobileHeight);
    }

    // ============================================================================
    // Hamburger Menu Tests
    // ============================================================================

    [Fact]
    public async Task MobileViewport_ShowsHamburgerButton()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert - Hamburger button should be visible on mobile
        var hamburger = Page.Locator(".hamburger-btn");
        await Assertions.Expect(hamburger).ToBeVisibleAsync();
    }

    [Fact]
    public async Task MobileViewport_HidesDesktopNavLinks()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert - Desktop nav links container should be hidden via CSS
        var navLinks = Page.Locator(".nav-links");
        await Assertions.Expect(navLinks).ToBeHiddenAsync();
    }

    [Fact]
    public async Task HamburgerClick_OpensMobileMenu()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act + Assert — retry [click + visibility] to cover hydration race
        var hamburger = Page.Locator(".hamburger-btn");
        var mobileMenu = Page.Locator(".mobile-menu.open");
        await hamburger.ClickAndExpectAsync(async () =>
            await Assertions.Expect(mobileMenu).ToBeVisibleAsync(
                new() { Timeout = 2000 }));
    }

    [Fact]
    public async Task HamburgerClick_ShowsOverlay()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act + Assert — retry [click + overlay visible]
        var hamburger = Page.Locator(".hamburger-btn");
        var overlay = Page.Locator(".mobile-menu-overlay");
        await hamburger.ClickAndExpectAsync(async () =>
            await Assertions.Expect(overlay).ToBeVisibleAsync(
                new() { Timeout = 2000 }));
    }

    [Fact]
    public async Task MobileMenu_ContainsSectionHeaders()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");
        await Page.Locator(".hamburger-btn").ClickAsync();

        // Assert - Should have section headers for each section
        var sectionHeaders = Page.Locator(".mobile-menu-section-header");
        var count = await sectionHeaders.CountAsync();
        count.Should().BeGreaterThanOrEqualTo(2, "should have at least 2 sections");

        // Verify some known sections exist
        await Assertions.Expect(Page.Locator(".mobile-menu-section-header", new() { HasTextString = "GitHub Copilot" })).ToBeVisibleAsync();
        await Assertions.Expect(Page.Locator(".mobile-menu-section-header", new() { HasTextString = "Artificial Intelligence" })).ToBeVisibleAsync();
    }

    [Fact]
    public async Task MobileMenu_ContainsAboutLink()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");
        await Page.Locator(".hamburger-btn").ClickAsync();

        // Assert - About is now a section header (same style as other sections)
        var aboutLink = Page.Locator(".mobile-menu-section-header", new() { HasTextString = "About" });
        await Assertions.Expect(aboutLink).ToBeVisibleAsync();
        var href = await aboutLink.GetAttributeAsync("href");
        href.Should().Be("/about");
    }

    [Fact]
    public async Task MobileMenu_SectionClick_ExpandsSubItems()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Open the menu with retry — Blazor @onclick may not be attached yet on slow networks
        await Page.Locator(".hamburger-btn").ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page.Locator(".mobile-menu.open")).ToBeVisibleAsync(
                new() { Timeout = 2000 }));

        // Act - Click on a section header to expand.
        // The mobile menu can be taller than the viewport; scroll the target
        // section header into view before clicking. Use Force=true (via ClickAndExpectAsync)
        // to bypass Playwright's viewport-boundary check for fixed-overlay menus.
        var sectionHeader = Page.Locator(".mobile-menu-section-header", new() { HasTextString = "GitHub Copilot" });
        await sectionHeader.ScrollIntoViewIfNeededAsync();
        await sectionHeader.ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page.Locator(".mobile-menu-sub-items a").First).ToBeVisibleAsync(
                new() { Timeout = 2000 }));

        // Assert - Sub-items are now visible (confirmed above, read count)
        var subItems = Page.Locator(".mobile-menu-sub-items a");
        await Assertions.Expect(subItems.First).ToBeVisibleAsync();
        var count = await subItems.CountAsync();
        count.Should().BeGreaterThan(0, "should show sub-items after expanding section");

        // Should contain "Browse" link
        await Assertions.Expect(Page.Locator(".mobile-menu-sub-items a", new() { HasTextString = "Browse" })).ToBeVisibleAsync();
    }

    [Fact]
    public async Task MobileMenu_OverlayClick_ClosesMenu()
    {
        // Arrange — open via retry click+assert
        await Page.GotoRelativeAsync("/");
        var mobileMenuOpen = Page.Locator(".mobile-menu.open");
        await Page.Locator(".hamburger-btn").ClickAndExpectAsync(async () =>
            await Assertions.Expect(mobileMenuOpen).ToBeVisibleAsync(
                new() { Timeout = 2000 }));

        // Act + Assert — retry overlay click until menu closes
        var overlay = Page.Locator(".mobile-menu-overlay");
        await BlazorHelpers.RetryUntilPassAsync(async () =>
        {
            await overlay.ClickAsync(new() { Position = new() { X = 10, Y = 200 }, Timeout = 2000 });
            await Assertions.Expect(mobileMenuOpen).ToHaveCountAsync(0, new() { Timeout = 2000 });
        });
    }

    [Fact]
    public async Task MobileMenu_SubItemClick_NavigatesAndClosesMenu()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Open the menu with retry — Blazor @onclick may not be attached yet on slow networks
        await Page.Locator(".hamburger-btn").ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page.Locator(".mobile-menu.open")).ToBeVisibleAsync(
                new() { Timeout = 2000 }));

        // Expand GitHub Copilot section with retry (section header is inside fixed-overlay)
        var sectionHeader = Page.Locator(".mobile-menu-section-header", new() { HasTextString = "GitHub Copilot" });
        await sectionHeader.ScrollIntoViewIfNeededAsync();
        await sectionHeader.ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page.Locator(".mobile-menu-sub-items a").First).ToBeVisibleAsync(
                new() { Timeout = 2000 }));

        // Act - Click "Browse" sub-item
        var allLink = Page.Locator(".mobile-menu-sub-items a", new() { HasTextString = "Browse" });
        await allLink.ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page).ToHaveURLAsync(
                new Regex(@".*/github-copilot.*"), new() { Timeout = 2000 }));

        // Assert - Should navigate and close menu
        await Assertions.Expect(Page.Locator(".mobile-menu.open")).ToHaveCountAsync(0);
    }

    [Fact]
    public async Task MobileMenu_HamburgerToggle_ClosesOpenMenu()
    {
        // Arrange — open menu with retry
        await Page.GotoRelativeAsync("/");
        var hamburger = Page.Locator(".hamburger-btn");
        var mobileMenuOpen = Page.Locator(".mobile-menu.open");
        await hamburger.ClickAndExpectAsync(async () =>
            await Assertions.Expect(mobileMenuOpen).ToBeVisibleAsync(new() { Timeout = 2000 }));

        // Act + Assert — toggle closed with retry
        await hamburger.ClickAndExpectAsync(async () =>
            await Assertions.Expect(mobileMenuOpen).ToHaveCountAsync(0, new() { Timeout = 2000 }));
    }

    // ============================================================================
    // Sidebar Collapse Tests
    // ============================================================================

    [Fact]
    public async Task MobileViewport_SidebarToolbar_ShowsButtons()
    {
        // Act - Navigate to a page with sidebar
        await Page.GotoRelativeAsync("/github-copilot");

        // Assert - Sidebar toolbar buttons should be visible on mobile (auto-retries via Expect)
        var buttons = Page.Locator(".sidebar-toolbar-btn");
        await Assertions.Expect(buttons.First).ToBeVisibleAsync();
    }

    [Fact]
    public async Task MobileViewport_SidebarToolbar_ContentHiddenByDefault()
    {
        // Act
        await Page.GotoRelativeAsync("/github-copilot");

        // Assert - No sidebar panel should be active by default
        var activePanel = Page.Locator(".sidebar-panel-active");
        await Assertions.Expect(activePanel).ToHaveCountAsync(0);
    }

    [Fact]
    public async Task MobileViewport_SidebarToolbar_ClickOpensPanel()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot");

        // Act - Click first toolbar button
        var firstBtn = Page.Locator(".sidebar-toolbar-btn").First;
        await firstBtn.ClickAsync();

        // Assert - Button should be active and panel visible
        await Assertions.Expect(firstBtn).ToHaveClassAsync(new Regex("active"));
        var activePanel = Page.Locator(".sidebar-panel-active");
        await Assertions.Expect(activePanel).ToHaveCountAsync(1);
    }

    [Fact]
    public async Task MobileViewport_SidebarToolbar_LabelMatchesContext()
    {
        // Act - Navigate to section/collection page
        await Page.GotoRelativeAsync("/github-copilot");

        // Assert - Toolbar should have Search button
        var searchBtn = Page.Locator(".sidebar-toolbar-btn", new() { HasTextString = "Search" });
        await Assertions.Expect(searchBtn).ToBeVisibleAsync();
    }

    [Fact]
    public async Task MobileViewport_SidebarToolbar_TocPage_ShowsTableOfContentsButton()
    {
        // Act - Navigate to a TOC-only page
        await Page.GotoRelativeAsync("/github-copilot/handbook");

        // Assert - Toolbar should have "Table of Contents" button
        var tocBtn = Page.Locator(".sidebar-toolbar-btn", new() { HasTextString = "Table of Contents" });
        await Assertions.Expect(tocBtn).ToBeVisibleAsync();
    }

    // ============================================================================
    // Filter Indicator Tests (dot + highlight on toolbar buttons)
    // ============================================================================

    [Fact]
    public async Task MobileViewport_FilterIndicator_NoIndicatorByDefault()
    {
        // Arrange - Navigate to a section page without any filters
        await Page.GotoRelativeAsync("/github-copilot");

        // Assert - No toolbar button should have the has-active-filter class
        var activeFilterButtons = Page.Locator(".sidebar-toolbar-btn.has-active-filter");
        await Assertions.Expect(activeFilterButtons).ToHaveCountAsync(0);

        // Assert - No filter dots should be visible
        var filterDots = Page.Locator(".sidebar-toolbar-filter-dot");
        await Assertions.Expect(filterDots).ToHaveCountAsync(0);
    }

    [Fact]
    public async Task MobileViewport_FilterIndicator_TagFilterShowsDot()
    {
        // Arrange - Navigate to a section page with a tag filter active via URL
        await Page.GotoRelativeAsync("/github-copilot?tags=vs%20code");
        await Page.WaitForBlazorReadyAsync();

        // Assert - Tags toolbar button should have has-active-filter class
        var tagsBtn = Page.Locator(".sidebar-toolbar-btn", new() { HasTextString = "Tags" });
        await Assertions.Expect(tagsBtn).ToHaveClassAsync(new Regex("has-active-filter"));

        // Assert - Filter dot should be visible inside the Tags button
        var filterDot = tagsBtn.Locator(".sidebar-toolbar-filter-dot");
        await Assertions.Expect(filterDot).ToBeVisibleAsync();
        await Assertions.Expect(filterDot).ToHaveAttributeAsync("aria-hidden", "true");
    }

    [Fact]
    public async Task MobileViewport_FilterIndicator_SearchFilterShowsDot()
    {
        // Arrange - Navigate to a section page with a search query active via URL
        await Page.GotoRelativeAsync("/github-copilot?search=copilot");
        await Page.WaitForBlazorReadyAsync();

        // Assert - Search toolbar button should have has-active-filter class
        var searchBtn = Page.Locator(".sidebar-toolbar-btn", new() { HasTextString = "Search" });
        await Assertions.Expect(searchBtn).ToHaveClassAsync(new Regex("has-active-filter"));

        // Assert - Filter dot should be visible inside the Search button
        var filterDot = searchBtn.Locator(".sidebar-toolbar-filter-dot");
        await Assertions.Expect(filterDot).ToBeVisibleAsync();
    }

    [Fact]
    public async Task MobileViewport_FilterIndicator_OnlyAffectedButtonShowsDot()
    {
        // Arrange - Navigate with only a tag filter (no search filter)
        await Page.GotoRelativeAsync("/github-copilot?tags=vs%20code");
        await Page.WaitForBlazorReadyAsync();

        // Assert - Tags button has indicator
        var tagsBtn = Page.Locator(".sidebar-toolbar-btn", new() { HasTextString = "Tags" });
        await Assertions.Expect(tagsBtn).ToHaveClassAsync(new Regex("has-active-filter"));

        // Assert - Search button does NOT have indicator
        var searchBtn = Page.Locator(".sidebar-toolbar-btn", new() { HasTextString = "Search" });
        await Assertions.Expect(searchBtn).Not.ToHaveClassAsync(new Regex("has-active-filter"));
    }

    // ============================================================================
    // Desktop Viewport (Regression) Tests
    // ============================================================================

    [Fact]
    public async Task DesktopViewport_HamburgerIsHidden()
    {
        // Arrange - Set desktop viewport
        await Page.SetViewportSizeAsync(1920, 1080);

        // Act
        await Page.GotoRelativeAsync("/");

        // Assert - Hamburger should be hidden, nav links visible
        await Assertions.Expect(Page.Locator(".hamburger-btn")).ToBeHiddenAsync();
        await Assertions.Expect(Page.Locator(".nav-links")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task DesktopViewport_SidebarToolbarButtonsHidden()
    {
        // Arrange - Set desktop viewport
        await Page.SetViewportSizeAsync(1920, 1080);

        // Act
        await Page.GotoRelativeAsync("/github-copilot");

        // Assert - Sidebar toolbar buttons should be hidden on desktop
        await Assertions.Expect(Page.Locator(".sidebar-toolbar-buttons")).ToBeHiddenAsync();
    }

    [Fact]
    public async Task DesktopViewport_SidebarContentVisibleWithoutCollapse()
    {
        // Arrange - Set desktop viewport
        await Page.SetViewportSizeAsync(1920, 1080);

        // Act
        await Page.GotoRelativeAsync("/github-copilot");

        // Assert - Sidebar content should be visible on desktop (display: contents makes wrapper transparent)
        // Check for actual sidebar component content rather than the collapse wrapper
        var sidebar = Page.Locator(".sidebar");
        await Assertions.Expect(sidebar).ToBeVisibleAsync();
    }

    // ============================================================================
    // SubNav Collapsed on Mobile (content hidden, element remains for layout)
    // ============================================================================

    [Fact]
    public async Task MobileViewport_SubNav_IsCollapsed()
    {
        // Act
        await Page.GotoRelativeAsync("/github-copilot");

        // Assert - SubNav element is present but wrapper content is hidden
        var subNav = Page.Locator(".sub-nav");
        await Assertions.Expect(subNav).ToBeVisibleAsync();

        var wrapper = Page.Locator(".sub-nav-wrapper");
        await Assertions.Expect(wrapper).ToBeHiddenAsync();
    }
}
