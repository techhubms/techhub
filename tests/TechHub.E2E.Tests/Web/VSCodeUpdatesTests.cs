using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for GitHub Copilot VS Code Updates custom page.
/// Verifies page-specific content and features.
/// 
/// Common component tests (TOC, highlighting) are in:
/// - SidebarTocTests.cs: Table of contents behavior
/// - HighlightingTests.cs: Code syntax highlighting
/// </summary>
public class VSCodeUpdatesTests : PlaywrightTestBase
{
    public VSCodeUpdatesTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    private const string PageUrl = "/github-copilot/vscode-updates";

    [Fact]
    public async Task VSCodeUpdates_ShouldLoad_Successfully()
    {
        // Act
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Page title shows the selected video title (latest video auto-selected)
        // or "VS Code Updates" if no video loaded yet — both end with "- Tech Hub"
        await Assertions.Expect(Page).ToHaveTitleAsync(new Regex("(Visual Studio Code|VS Code Updates).+Tech Hub"));
    }

    [Fact]
    public async Task VSCodeUpdates_ShouldDisplay_Content()
    {
        // Act
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Page should have main content heading (excluding banner heading)
        var mainHeading = Page.Locator(".page-h1");
        await mainHeading.AssertElementVisibleAsync();

        // Should have some content (paragraphs, lists, etc.)
        var count = await Page.GetElementCountBySelectorAsync("p");
        count.Should().BeGreaterThan(0, $"Expected at least one paragraph, but found {count}");
    }

    [Fact]
    public async Task VSCodeUpdates_ShouldNot_HaveConsoleErrors()
    {
        // Arrange - Collect console messages
        var consoleMessages = new List<IConsoleMessage>();
        Page.Console += (_, msg) => consoleMessages.Add(msg);

        // Act
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - No console errors (filter WebSocket connection errors from Blazor)
        var errors = consoleMessages
            .Where(m => m.Type == "error")
            .Where(m => !m.Text.Contains("WebSocket"))
            .Where(m => !m.Text.Contains("ERR_CONNECTION_REFUSED"))
            .ToList();

        errors.Should().BeEmpty($"Expected no console errors on {PageUrl}, but found: {string.Join(", ", errors.Select(e => e.Text))}");
    }

    [Fact]
    public async Task VSCodeUpdates_ShouldShowAllUpdates_NotFilteredByDate()
    {
        // Act
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - The sidebar should list all video updates, not just recent ones
        // The "All Updates" panel contains sidebar-list items for each video
        var sidebarItems = Page.Locator(".sidebar-list li");
        await sidebarItems.First.WaitForAsync();

        var itemCount = await sidebarItems.CountAsync();
        itemCount.Should().BeGreaterThanOrEqualTo(1,
            "VS Code updates page should show all videos regardless of date (lastDays=0 disables date filter)");
    }
}
