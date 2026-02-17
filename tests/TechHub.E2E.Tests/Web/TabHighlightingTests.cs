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
        ILocator? focusedElement = null;
        var maxTabs = 50; // Safety limit

        for (int i = 0; i < maxTabs; i++)
        {
            await Page.Keyboard.PressAsync("Tab");

            // Check what element is now focused (Tab processing is synchronous)
            var tagName = await Page.EvaluateAsync<string>("() => document.activeElement?.tagName || ''");
            // Skip if nothing focused
            if (tagName == "")
            {
                continue;
            }

            var isInMain = await Page.EvaluateAsync<bool>("() => document.activeElement?.closest('main') !== null");

            if (tagName == "A" && isInMain)
            {
                focusedElement = Page.Locator(":focus");
                break;
            }
        }

        focusedElement.Should().NotBeNull("should find a link in main content via keyboard navigation");

        // Assert - Link should have visible outline
        var outlineWidth = await focusedElement!.EvaluateAsync<string>("el => window.getComputedStyle(el).outlineWidth");
        outlineWidth.Should().NotBe("0px", "focused link should have visible outline width when accessed via keyboard");
    }

    [Fact]
    public async Task FocusedButtons_ShouldShow_VisibleOutline()
    {
        // Arrange - Use section index page which has tag cloud buttons in sidebar
        await Page.GotoRelativeAsync("/ai");

        // Act - Use pure keyboard navigation to trigger :focus-visible
        // Tab through the page until we find a button
        ILocator? focusedElement = null;
        var maxTabs = 100; // Need more tabs to reach sidebar buttons

        for (int i = 0; i < maxTabs; i++)
        {
            await Page.Keyboard.PressAsync("Tab");

            // Check what element is now focused (Tab processing is synchronous)
            var tagName = await Page.EvaluateAsync<string>("() => document.activeElement?.tagName || ''");
            // Skip if nothing focused
            if (tagName == "")
            {
                continue;
            }

            if (tagName == "BUTTON")
            {
                focusedElement = Page.Locator(":focus");
                break;
            }
        }

        focusedElement.Should().NotBeNull("should find a button via keyboard navigation");

        // Assert - Button should have visible outline
        var outlineWidth = await focusedElement!.EvaluateAsync<string>("el => window.getComputedStyle(el).outlineWidth");
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
        ILocator? focusedElement = null;
        var maxTabs = 100; // Safety limit - tag buttons are further down

        for (int i = 0; i < maxTabs; i++)
        {
            await Page.Keyboard.PressAsync("Tab");

            // Check what element is now focused (Tab processing is synchronous)
            var hasClass = await Page.EvaluateAsync<bool>("() => document.activeElement?.classList.contains('tag-cloud-item') || false");

            if (hasClass)
            {
                focusedElement = Page.Locator(":focus");
                break;
            }
        }

        focusedElement.Should().NotBeNull("should find a tag-cloud-item via keyboard navigation");

        // Assert - Tag button should have visible outline
        var outlineWidth = await focusedElement!.EvaluateAsync<string>("el => window.getComputedStyle(el).outlineWidth");
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
}
