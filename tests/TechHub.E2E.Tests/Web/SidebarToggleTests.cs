using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

public class SidebarToggleTests : PlaywrightTestBase
{
    public SidebarToggleTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task SidebarToggle_ShouldBeVisible_OnDesktop()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert - Toggle button should be visible on desktop (1920x1080 viewport)
        await Page.AssertElementVisibleBySelectorAsync(".sidebar-toggle");
    }

    [Fact]
    public async Task SidebarToggle_ShouldBeHidden_OnMobile()
    {
        // Arrange - Set mobile viewport
        await Page.SetViewportSizeAsync(768, 1024);

        // Act
        await Page.GotoRelativeAsync("/");

        // Assert - Toggle button should be hidden on mobile
        var toggle = Page.Locator(".sidebar-toggle");
        await Assertions.Expect(toggle).ToBeHiddenAsync();
    }

    [Fact]
    public async Task SidebarToggle_Click_ShouldCollapseSidebar()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act - Click the toggle button
        await Page.Locator(".sidebar-toggle").ClickAsync();

        // Assert - <html> element should have collapsed class
        var html = Page.Locator("html");
        await Assertions.Expect(html).ToHaveClassAsync(new Regex("sidebar-collapsed"));
    }

    [Fact]
    public async Task SidebarToggle_DoubleClick_ShouldExpandSidebar()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act - Click toggle twice
        await Page.Locator(".sidebar-toggle").ClickAsync();
        await Page.Locator(".sidebar-toggle").ClickAsync();

        // Assert - <html> element should NOT have collapsed class
        var html = Page.Locator("html");
        await Assertions.Expect(html).Not.ToHaveClassAsync(new Regex("sidebar-collapsed"));
    }

    [Fact]
    public async Task SidebarToggle_ShouldHavCorrectAriaAttributes()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert
        var toggle = Page.Locator(".sidebar-toggle");
        await Assertions.Expect(toggle).ToHaveAttributeAsync("aria-expanded", "true");
        await Assertions.Expect(toggle).ToHaveAttributeAsync("aria-label", "Collapse sidebar");
    }

    [Fact]
    public async Task SidebarToggle_WhenCollapsed_ShouldUpdateAriaAttributes()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act - ClickAsync() is sufficient here: Playwright's built-in stability check
        // waits until the element is not being updated by a Blazor DOM diff before
        // dispatching the click. No URL change is expected, so ClickBlazorElementAsync
        // (which waits for URL change) is not needed.
        await Page.Locator(".sidebar-toggle").ClickAsync();

        // Assert
        var toggle = Page.Locator(".sidebar-toggle");
        await Assertions.Expect(toggle).ToHaveAttributeAsync("aria-expanded", "false");
        await Assertions.Expect(toggle).ToHaveAttributeAsync("aria-label", "Expand sidebar");
    }

    [Fact]
    public async Task SidebarToggle_CollapsedState_ShouldPersistAcrossNavigation()
    {
        // Arrange - Collapse sidebar on homepage
        await Page.GotoRelativeAsync("/");
        await Page.Locator(".sidebar-toggle").ClickAsync();

        // Verify collapsed
        var html = Page.Locator("html");
        await Assertions.Expect(html).ToHaveClassAsync(new Regex("sidebar-collapsed"));

        // Act - Navigate to a section page (full page load to test cookie persistence)
        await Page.GotoRelativeAsync("/github-copilot");

        // Assert - Sidebar should still be collapsed (server reads cookie during SSR)
        html = Page.Locator("html");
        await Assertions.Expect(html).ToHaveClassAsync(new Regex("sidebar-collapsed"));
    }

    [Fact]
    public async Task SidebarToggle_WhenCollapsed_ShouldHideSidebarContent()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act
        await Page.Locator(".sidebar-toggle").ClickAsync();

        // Assert - Sidebar sections should be hidden (CSS: html.sidebar-collapsed .sidebar > *:not(.sidebar-toggle))
        var sidebarSections = Page.Locator("html.sidebar-collapsed .sidebar .sidebar-section");
        var count = await sidebarSections.CountAsync();
        for (int i = 0; i < count; i++)
        {
            await Assertions.Expect(sidebarSections.Nth(i)).ToBeHiddenAsync();
        }
    }

    [Fact]
    public async Task SidebarToggle_WhenCollapsed_ShouldShowMoreCardsPerRow()
    {
        // Arrange - Navigate to homepage and count columns before collapse
        await Page.GotoRelativeAsync("/");

        // Get the number of grid columns before collapse
        var columnsBefore = await Page.EvaluateAsync<int>(@"() => {
            const grid = document.querySelector('.sections-grid .grid');
            if (!grid) return 0;
            const style = window.getComputedStyle(grid);
            return style.gridTemplateColumns.split(' ').length;
        }");

        // Act - Collapse sidebar
        await Page.Locator(".sidebar-toggle").ClickAsync();

        // Wait for layout to settle after CSS transition
        await Page.WaitForConditionAsync(
            $"(before) => {{ const grid = document.querySelector('.sections-grid .grid'); if (!grid) return false; const cols = window.getComputedStyle(grid).gridTemplateColumns.split(' ').length; return cols > before; }}",
            columnsBefore);

        // Get columns after collapse
        var columnsAfter = await Page.EvaluateAsync<int>(@"() => {
            const grid = document.querySelector('.sections-grid .grid');
            if (!grid) return 0;
            const style = window.getComputedStyle(grid);
            return style.gridTemplateColumns.split(' ').length;
        }");

        // Assert - Should have more columns when sidebar is collapsed
        columnsAfter.Should().BeGreaterThan(columnsBefore);
    }
}
