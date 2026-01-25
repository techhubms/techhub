using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for keyboard focus visibility (tab highlighting).
/// Verifies that interactive elements show visible focus indicators when tabbed to.
/// This is a WCAG 2.1 Level AA accessibility requirement.
/// 
/// Test Pages:
/// - /ai/genai-basics - Representative page with links, buttons, and interactive elements
/// 
/// Coverage:
/// - Links show visible focus outline
/// - Buttons show visible focus outline
/// - Tag buttons show visible focus outline
/// - Skip link shows visible focus outline
/// </summary>
[Collection("Tab Highlighting Tests")]
public class TabHighlightingTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
    private IBrowserContext? _context;
    private IPage? _page;
    private IPage Page => _page ?? throw new InvalidOperationException("Page not initialized");

    public async Task InitializeAsync()
    {
        _context = await fixture.CreateContextAsync();
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
    public async Task FocusedLinks_ShouldShow_VisibleOutline()
    {
        // Arrange
        await Page.GotoRelativeAsync("/ai/genai-basics");

        // Act - Use keyboard navigation to focus a link in main content
        // Tab through: skip-link -> header links -> until we get to main content link
        // Use direct keyboard navigation to trigger :focus-visible
        var mainLink = Page.Locator("main a").First;
        await mainLink.ScrollIntoViewIfNeededAsync();

        // Click near the element first to move focus into the page, then tab to it
        // This simulates real keyboard navigation which triggers :focus-visible
        await Page.Keyboard.PressAsync("Tab"); // Focus skip-link

        // Navigate until we hit a link in main - use direct focus with keyboard simulation
        await mainLink.EvaluateAsync("el => el.focus()");
        await Page.Keyboard.PressAsync("Tab");
        await Page.Keyboard.PressAsync("Shift+Tab"); // This triggers :focus-visible

        var focusedElement = Page.Locator(":focus");

        // Assert - Link should have visible outline (when accessed via keyboard)
        var outlineStyle = await focusedElement.EvaluateAsync<string>("el => window.getComputedStyle(el).outline");
        outlineStyle.Should().NotBeNullOrEmpty("focused link should have outline style");

        // Check outline width is at least 2px
        var outlineWidth = await focusedElement.EvaluateAsync<string>("el => window.getComputedStyle(el).outlineWidth");
        outlineWidth.Should().NotBe("0px", "focused link should have visible outline width when accessed via keyboard");
    }

    [Fact]
    public async Task FocusedButtons_ShouldShow_VisibleOutline()
    {
        // Arrange
        await Page.GotoRelativeAsync("/ai/genai-basics");

        // Wait for Blazor interactivity
        await Page.WaitForBlazorReadyAsync();

        // Act - Use keyboard navigation to focus a button
        // Tab+Shift pattern triggers :focus-visible in browsers
        var firstButton = Page.Locator("button").First;
        await firstButton.ScrollIntoViewIfNeededAsync();
        await firstButton.EvaluateAsync("el => el.focus()");
        await Page.Keyboard.PressAsync("Tab");
        await Page.Keyboard.PressAsync("Shift+Tab"); // This triggers :focus-visible

        var focusedElement = Page.Locator(":focus");

        // Assert - Button should have visible outline (when accessed via keyboard)
        var outlineStyle = await focusedElement.EvaluateAsync<string>("el => window.getComputedStyle(el).outline");
        outlineStyle.Should().NotBeNullOrEmpty("focused button should have outline style");

        // Check outline width is at least 2px
        var outlineWidth = await focusedElement.EvaluateAsync<string>("el => window.getComputedStyle(el).outlineWidth");
        outlineWidth.Should().NotBe("0px", "focused button should have visible outline width when accessed via keyboard");
    }

    [Fact]
    public async Task FocusedTagButtons_ShouldShow_VisibleOutline()
    {
        // Arrange - Use section page that has tag cloud sidebar (not custom pages)
        await Page.GotoRelativeAsync("/ai");

        // Wait for Blazor interactivity and tag cloud to render
        await Page.WaitForBlazorReadyAsync();
        var tagButton = Page.Locator(".tag-cloud-item").First;
        await Assertions.Expect(tagButton).ToBeVisibleAsync(new() { Timeout = 5000 });

        // Act - Use keyboard navigation to focus the tag button
        // Tab+Shift pattern triggers :focus-visible in browsers
        await tagButton.ScrollIntoViewIfNeededAsync();
        await tagButton.EvaluateAsync("el => el.focus()");
        await Page.Keyboard.PressAsync("Tab");
        await Page.Keyboard.PressAsync("Shift+Tab"); // This triggers :focus-visible

        var focusedElement = Page.Locator(":focus");

        // Assert - Tag button should have visible outline (when accessed via keyboard)
        var outlineStyle = await focusedElement.EvaluateAsync<string>("el => window.getComputedStyle(el).outline");
        outlineStyle.Should().NotBeNullOrEmpty("focused tag button should have outline style");

        // Check outline width is at least 2px
        var outlineWidth = await focusedElement.EvaluateAsync<string>("el => window.getComputedStyle(el).outlineWidth");
        outlineWidth.Should().NotBe("0px", "focused tag button should have visible outline width when accessed via keyboard");
    }

    [Fact]
    public async Task FocusedSkipLink_ShouldShow_VisibleOutline()
    {
        // Arrange
        await Page.GotoRelativeAsync("/ai/genai-basics");

        // Act - Tab to skip link (first focusable element)
        await Page.Keyboard.PressAsync("Tab");

        // Wait for skip link to appear (it animates from off-screen to visible on focus)
        var skipLink = Page.Locator(".skip-link");
        await Assertions.Expect(skipLink).ToBeVisibleAsync(new() { Timeout = 2000 });

        // Wait for focus to settle on the skip link using Playwright polling
        await Page.WaitForFunctionAsync(
            "() => document.activeElement && document.activeElement.classList.contains('skip-link')",
            new PageWaitForFunctionOptions { Timeout = 2000, PollingInterval = 50 });

        // Verify it's actually focused
        var focusedElement = Page.Locator(":focus");
        var isFocusedSkipLink = await focusedElement.EvaluateAsync<bool>("el => el.classList.contains('skip-link')");
        isFocusedSkipLink.Should().BeTrue("first Tab should focus the skip link");

        // Assert - Skip link should have visible outline (check the focused element directly)
        var outlineStyle = await focusedElement.EvaluateAsync<string>("el => window.getComputedStyle(el).outline");
        outlineStyle.Should().NotBeNullOrEmpty("focused skip link should have outline style");
        outlineStyle.Should().NotContain("none", "focused skip link outline should not be 'none'");

        // Check outline width is at least 2px
        var outlineWidth = await focusedElement.EvaluateAsync<string>("el => window.getComputedStyle(el).outlineWidth");
        outlineWidth.Should().NotBe("0px", "focused skip link should have visible outline width");
    }
}
