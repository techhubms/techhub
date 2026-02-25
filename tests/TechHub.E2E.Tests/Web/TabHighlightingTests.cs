using System.Text.Json;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for keyboard focus visibility (tab highlighting).
/// Verifies that interactive elements show visible focus indicators when tabbed to,
/// and that click/tap interactions do NOT show focus outlines (keyboard-only outlines).
/// Also verifies that pointer interactions blur focused elements and that text inputs
/// do not show box-shadow focus rings in pointer mode.
/// This is a WCAG 2.1 Level AA accessibility requirement.
/// 
/// Test Pages:
/// - /ai/genai-basics - Representative page with links, buttons, and interactive elements
/// - /ai - Section index page with tag cloud buttons and search input
/// 
/// Coverage:
/// - Links show visible focus outline when tabbed to
/// - Buttons show visible focus outline when tabbed to
/// - Tag buttons show visible focus outline when tabbed to
/// - Skip link shows visible focus outline when tabbed to
/// - Clicked elements do NOT show focus outline (pointer/touch suppression)
/// - Search input does NOT show box-shadow ring in pointer mode
/// - Pointer click blurs previously focused non-input elements
/// - Pointer after keyboard removes keyboard-nav class
/// </summary>
public class TabHighlightingTests : PlaywrightTestBase
{
    public TabHighlightingTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task FocusedLinks_ShouldShow_VisibleOutline()
    {
        // Arrange
        await Page.GotoRelativeAsync("/ai/genai-basics");

        // Act - Use pure keyboard navigation to trigger :focus-visible
        // Tab through the page until we find a link in main content
        var foundLink = false;
        var maxTabs = 50; // Safety limit

        for (int i = 0; i < maxTabs; i++)
        {
            await Page.Keyboard.PressAsync("Tab");

            // Check what element is now focused (Tab processing is synchronous)
            var info = await Page.EvaluateAsync<JsonElement>(
                "() => ({ tag: document.activeElement?.tagName || '', inMain: document.activeElement?.closest('main') !== null })");
            var tagName = info.GetProperty("tag").GetString();
            var isInMain = info.GetProperty("inMain").GetBoolean();

            if (tagName == "A" && isInMain)
            {
                foundLink = true;
                break;
            }
        }

        foundLink.Should().BeTrue("should find a link in main content via keyboard navigation");

        // Assert - Link should have visible outline
        // Evaluate directly on document.activeElement to avoid stale :focus locator race condition
        var outlineWidth = await Page.EvaluateAsync<string>(
            "() => window.getComputedStyle(document.activeElement).outlineWidth");
        outlineWidth.Should().NotBe("0px", "focused link should have visible outline width when accessed via keyboard");
    }

    [Fact]
    public async Task FocusedButtons_ShouldShow_VisibleOutline()
    {
        // Arrange - Use section index page which has tag cloud buttons in sidebar
        await Page.GotoRelativeAsync("/ai");

        // Act - Use pure keyboard navigation to trigger :focus-visible
        // Tab through the page until we find a button
        var foundButton = false;
        var maxTabs = 100; // Need more tabs to reach sidebar buttons

        for (int i = 0; i < maxTabs; i++)
        {
            await Page.Keyboard.PressAsync("Tab");

            // Check what element is now focused (Tab processing is synchronous)
            var tagName = await Page.EvaluateAsync<string>("() => document.activeElement?.tagName || ''");

            if (tagName == "BUTTON")
            {
                foundButton = true;
                break;
            }
        }

        foundButton.Should().BeTrue("should find a button via keyboard navigation");

        // Assert - Button should have visible outline
        // Evaluate directly on document.activeElement to avoid stale :focus locator race condition
        var outlineWidth = await Page.EvaluateAsync<string>(
            "() => window.getComputedStyle(document.activeElement).outlineWidth");
        outlineWidth.Should().NotBe("0px", "focused button should have visible outline width when accessed via keyboard");
    }

    [Fact]
    public async Task FocusedTagButtons_ShouldShow_VisibleOutline()
    {
        // Arrange - Use section page that has tag cloud sidebar (not custom pages)
        await Page.GotoRelativeAsync("/ai");

        var tagButton = Page.Locator(".tag-cloud-item").First;
        await Assertions.Expect(tagButton).ToBeVisibleAsync();

        // Act - Use pure keyboard navigation to trigger :focus-visible
        // Tab through the page until we find a tag cloud item
        var foundTagButton = false;
        var maxTabs = 100; // Safety limit - tag buttons are further down

        for (int i = 0; i < maxTabs; i++)
        {
            await Page.Keyboard.PressAsync("Tab");

            // Check what element is now focused (Tab processing is synchronous)
            var hasClass = await Page.EvaluateAsync<bool>("() => document.activeElement?.classList.contains('tag-cloud-item') || false");

            if (hasClass)
            {
                foundTagButton = true;
                break;
            }
        }

        foundTagButton.Should().BeTrue("should find a tag-cloud-item via keyboard navigation");

        // Assert - Tag button should have visible outline
        // Evaluate directly on document.activeElement to avoid stale :focus locator race condition
        var outlineWidth = await Page.EvaluateAsync<string>(
            "() => window.getComputedStyle(document.activeElement).outlineWidth");
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
        await Assertions.Expect(skipLink).ToBeVisibleAsync();

        // Wait for focus to settle on the skip link using Playwright polling
        await Page.WaitForConditionAsync(
            "() => document.activeElement && document.activeElement.classList.contains('skip-link')");

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

    [Fact]
    public async Task ClickedElements_ShouldNotShow_FocusOutline()
    {
        // Arrange - Use section index page which has clickable links and buttons
        await Page.GotoRelativeAsync("/ai");

        // Verify keyboard-nav is NOT set by default (fresh page load, no keyboard input)
        var hasKeyboardNav = await Page.EvaluateAsync<bool>(
            "() => document.documentElement.classList.contains('keyboard-nav')");
        hasKeyboardNav.Should().BeFalse("keyboard-nav class should not be set on fresh page load");

        // Act - Click a link/button using pointer to trigger focus
        var link = Page.Locator("main a[href]").First;
        await Assertions.Expect(link).ToBeVisibleAsync();

        // Use FocusAsync to focus without navigating
        await link.FocusAsync();

        // Assert - Element should NOT have a visible outline (no keyboard-nav class active)
        var outlineWidth = await link.EvaluateAsync<string>(
            "el => window.getComputedStyle(el).outlineWidth");
        outlineWidth.Should().Be("0px",
            "focused element should not show outline when keyboard-nav class is not active (pointer mode)");
    }

    [Fact]
    public async Task FocusedSearchInput_ShouldNotShow_BoxShadowRing()
    {
        // Arrange - Navigate to a page with a search input
        await Page.GotoRelativeAsync("/ai");

        // Verify keyboard-nav is NOT set by default
        var hasKeyboardNav = await Page.EvaluateAsync<bool>(
            "() => document.documentElement.classList.contains('keyboard-nav')");
        hasKeyboardNav.Should().BeFalse("keyboard-nav class should not be set on fresh page load");

        // Act - Focus the search input as a pointer user would (without keyboard-nav)
        var searchInput = Page.Locator("input[type='search'], .search-input").First;
        await Assertions.Expect(searchInput).ToBeVisibleAsync();
        await searchInput.FocusAsync();

        // Assert - Should NOT have a box-shadow focus ring (only border-color change)
        var boxShadow = await searchInput.EvaluateAsync<string>(
            "el => window.getComputedStyle(el).boxShadow");
        boxShadow.Should().BeOneOf("none", "",
            "search input should not show box-shadow ring when focused via pointer");

        // Outline should also be suppressed
        var outlineWidth = await searchInput.EvaluateAsync<string>(
            "el => window.getComputedStyle(el).outlineWidth");
        outlineWidth.Should().Be("0px",
            "search input should not show outline when focused via pointer");
    }

    [Fact]
    public async Task PointerBlur_ShouldRemove_FocusFromButtons()
    {
        // Arrange - Navigate to a page with interactive buttons
        await Page.GotoRelativeAsync("/ai");

        // First use keyboard to focus a button
        await Page.Keyboard.PressAsync("Tab");

        var hasKeyboardNav = await Page.EvaluateAsync<bool>(
            "() => document.documentElement.classList.contains('keyboard-nav')");
        hasKeyboardNav.Should().BeTrue("keyboard-nav should be set after Tab press");

        // Verify something is focused (auto-retry because focus may take a frame to settle
        // in headless Chrome, especially under CI load)
        await Page.WaitForConditionAsync(
            "() => document.activeElement !== null && document.activeElement !== document.body");

        // Act - Use pointer click to simulate switching to pointer mode
        await Page.Mouse.ClickAsync(100, 100);

        // Assert - Active element should be blurred (back to body)
        var activeTagAfterClick = await Page.EvaluateAsync<string>(
            "() => document.activeElement?.tagName || ''");
        activeTagAfterClick.Should().Be("BODY",
            "pointer click should blur previously focused non-input elements");
    }

    [Fact]
    public async Task PointerAfterKeyboard_ShouldRemove_FocusOutline()
    {
        // Arrange - Navigate to page
        await Page.GotoRelativeAsync("/ai/genai-basics");

        // Act - First use keyboard to set keyboard-nav mode
        await Page.Keyboard.PressAsync("Tab");

        // Verify keyboard-nav is active
        var hasKeyboardNavBefore = await Page.EvaluateAsync<bool>(
            "() => document.documentElement.classList.contains('keyboard-nav')");
        hasKeyboardNavBefore.Should().BeTrue("keyboard-nav should be set after Tab press");

        // Now use pointer (click) to switch back to pointer mode
        await Page.Mouse.ClickAsync(100, 100);

        // Assert - keyboard-nav class should be removed after pointer interaction
        var hasKeyboardNavAfter = await Page.EvaluateAsync<bool>(
            "() => document.documentElement.classList.contains('keyboard-nav')");
        hasKeyboardNavAfter.Should().BeFalse(
            "keyboard-nav class should be removed after pointer click, disabling focus outlines");
    }
}
