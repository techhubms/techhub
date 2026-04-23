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

        // Act + Assert — retry [click + class check] to cover Blazor Server
        // hydration race where @onclick may not be attached yet.
        var toggle = Page.Locator(".sidebar-toggle");
        var html = Page.Locator("html");
        await toggle.ClickAndExpectAsync(async () =>
            await Assertions.Expect(html).ToHaveClassAsync(
                new Regex("sidebar-collapsed"), new() { Timeout = 2000 }));
    }

    [Fact]
    public async Task SidebarToggle_DoubleClick_ShouldExpandSidebar()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");
        var toggle = Page.Locator(".sidebar-toggle");
        var html = Page.Locator("html");

        // First click: collapse (retry to handle hydration race)
        await toggle.ClickAndExpectAsync(async () =>
            await Assertions.Expect(html).ToHaveClassAsync(
                new Regex("sidebar-collapsed"), new() { Timeout = 2000 }));

        // Second click: expand (retry again)
        await toggle.ClickAndExpectAsync(async () =>
            await Assertions.Expect(html).Not.ToHaveClassAsync(
                new Regex("sidebar-collapsed"), new() { Timeout = 2000 }));
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

        // Act + Assert — retry the [click + attribute check] block to cover the
        // Blazor Server hydration race where @onclick may not be attached yet.
        var toggle = Page.Locator(".sidebar-toggle");
        await toggle.ClickAndExpectAsync(async () =>
        {
            await Assertions.Expect(toggle).ToHaveAttributeAsync(
                "aria-expanded", "false", new() { Timeout = 2000 });
            await Assertions.Expect(toggle).ToHaveAttributeAsync(
                "aria-label", "Expand sidebar", new() { Timeout = 2000 });
        });
    }

    [Fact]
    public async Task SidebarToggle_CollapsedState_ShouldPersistAcrossNavigation()
    {
        // Arrange - Collapse sidebar on homepage (retry click+assert for hydration race)
        await Page.GotoRelativeAsync("/");
        var toggle = Page.Locator(".sidebar-toggle");
        var html = Page.Locator("html");
        await toggle.ClickAndExpectAsync(async () =>
            await Assertions.Expect(html).ToHaveClassAsync(
                new Regex("sidebar-collapsed"), new() { Timeout = 2000 }));

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

        // Act + Assert — retry click+class check to ensure collapse actually fired
        var toggle = Page.Locator(".sidebar-toggle");
        var html = Page.Locator("html");
        await toggle.ClickAndExpectAsync(async () =>
            await Assertions.Expect(html).ToHaveClassAsync(
                new Regex("sidebar-collapsed"), new() { Timeout = 2000 }));

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

        // Act + Assert — retry click until grid columns increase
        var toggle = Page.Locator(".sidebar-toggle");
        await toggle.ClickAndExpectAsync(async () =>
        {
            var cols = await Page.EvaluateAsync<int>(@"() => {
                const grid = document.querySelector('.sections-grid .grid');
                if (!grid) return 0;
                return window.getComputedStyle(grid).gridTemplateColumns.split(' ').length;
            }");
            cols.Should().BeGreaterThan(columnsBefore,
                "sidebar collapse should increase the grid column count");
        });

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
