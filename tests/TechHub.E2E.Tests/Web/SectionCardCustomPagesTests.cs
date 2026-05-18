using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for custom page badges on section cards.
/// Tests the "+X more" expand/collapse functionality and custom page ordering.
/// </summary>
public class SectionCardCustomPagesTests : PlaywrightTestBase
{
    public SectionCardCustomPagesTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task HomePage_SectionCard_ShowsAllRegularCollections()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act - Find first section card with collections
        var sectionCards = Page.Locator(".section-card-container");
        var firstCardWithCollections = sectionCards.First;

        // Assert - Regular collections should use .badge-purple
        var regularBadges = firstCardWithCollections.Locator(".badge-purple");
        var count = await regularBadges.CountAsync();

        // Should have at least one regular collection
        count.Should().BeGreaterThan(0, "section cards should display regular collection badges");
    }

    [Fact]
    public async Task HomePage_SectionCard_ShowsFirstCustomPage_WhenCustomPagesExist()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act - Look for section cards with custom pages (GitHub Copilot or AI sections typically have them)
        var customBadges = Page.Locator(".section-collections > .badge-custom");
        var customBadgeCount = await customBadges.CountAsync();

        // Assert - If custom pages exist, at least one should be visible
        if (customBadgeCount > 0)
        {
            var firstCustomBadge = customBadges.First;
            await Assertions.Expect(firstCustomBadge).ToBeVisibleAsync();

            // Custom badge should have specific styling (badge-custom class)
            var classes = await firstCustomBadge.GetAttributeAsync("class");
            classes.Should().Contain("badge-custom", "custom page badges should use badge-custom class");
        }
    }

    [Fact]
    public async Task HomePage_SectionCard_ShowsExpandButton_WhenMultipleCustomPagesExist()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act - Find expand buttons ("+X more") for custom pages
        var expandButtons = Page.Locator(".badge-expandable[data-expand-target]");
        var buttonCount = await expandButtons.CountAsync();

        // Assert - If there are multiple custom pages on any section, expand button should exist
        if (buttonCount > 0)
        {
            var firstButton = expandButtons.First;
            await Assertions.Expect(firstButton).ToBeVisibleAsync();

            // Button should show "+X more" text pattern
            var buttonText = await firstButton.TextContentAsync();
            buttonText.Should().MatchRegex(@"\+\d+ more", "button should show '+X more' format");

            // Button should have correct ARIA attributes
            var ariaExpanded = await firstButton.GetAttributeAsync("aria-expanded");
            ariaExpanded.Should().Be("false", "button should initially be collapsed");
        }
    }

    [Fact]
    public async Task HomePage_SectionCard_ExpandButton_ShowsHiddenCustomPages_WhenClicked()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act - Find first expand button
        var expandButtons = Page.Locator(".badge-expandable[data-expand-target]");
        var initialButtonCount = await expandButtons.CountAsync();

        if (initialButtonCount > 0)
        {
            var expandButton = expandButtons.First;

            // Get the target container ID
            var targetId = await expandButton.GetAttributeAsync("data-expand-target");
            targetId.Should().NotBeNullOrWhiteSpace("expand button should have data-expand-target attribute");

            var targetContainer = Page.Locator($"#{targetId}");

            // Verify container is initially hidden
            var isHidden = await targetContainer.IsHiddenAsync();
            isHidden.Should().BeTrue("custom pages container should be initially hidden");

            // Click to expand — retry the [click + visibility check] block to
            // survive the Blazor Server hydration race where @onclick may not
            // be attached yet.
            await expandButton.ClickAndExpectAsync(async () =>
            {
                await Assertions.Expect(targetContainer).ToBeVisibleAsync();
            });

            // Button with this specific target ID should be removed after clicking
            var specificButton = Page.Locator($".badge-expandable[data-expand-target='{targetId}']");
            var specificButtonCount = await specificButton.CountAsync();
            specificButtonCount.Should().Be(0, "the clicked expand button should be removed after clicking");

            // Total button count should decrease by 1. Use Playwright's retry assertion
            // rather than a raw CountAsync() to survive the Blazor re-render window
            // where remaining buttons momentarily disappear before being re-attached.
            await Assertions.Expect(expandButtons).ToHaveCountAsync(
                initialButtonCount - 1);

            // Hidden custom pages should be visible
            var hiddenBadges = targetContainer.Locator(".badge-custom");
            var hiddenBadgeCount = await hiddenBadges.CountAsync();
            hiddenBadgeCount.Should().BeGreaterThan(0, "expanded container should contain additional custom page badges");
        }
    }

    [Fact]
    public async Task HomePage_SectionCard_CustomPages_AreOrderedByOrderProperty()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act - Find sections with multiple custom pages (sections with expand button)
        var expandButtons = Page.Locator(".badge-expandable[data-expand-target]");
        var buttonCount = await expandButtons.CountAsync();

        if (buttonCount > 0)
        {
            var expandButton = expandButtons.First;
            var targetId = await expandButton.GetAttributeAsync("data-expand-target");

            // Get the section card container for this expand button
            var sectionCardContainer = expandButton.Locator("xpath=ancestor::div[contains(@class, 'section-card-container')]").First;

            // Get first visible custom page (should have lowest Order value)
            var firstVisibleCustom = sectionCardContainer.Locator(".section-collections > .badge-custom").First;
            var firstVisibleText = await firstVisibleCustom.TextContentAsync();

            // Expand to see all custom pages
            await expandButton.ClickAndExpectAsync(async () =>
                await Assertions.Expect(Page.Locator($"#{targetId}")).ToBeVisibleAsync());

            // Get all custom page badges (visible + hidden) in display order
            var allCustomBadges = new List<string>
            {
                // Add first visible custom page
                firstVisibleText?.Trim() ?? ""
            };

            // Add hidden custom pages in order
            var targetContainer = Page.Locator($"#{targetId}");
            var hiddenBadges = targetContainer.Locator(".badge-custom");
            var hiddenCount = await hiddenBadges.CountAsync();

            for (int i = 0; i < hiddenCount; i++)
            {
                var badgeText = await hiddenBadges.Nth(i).TextContentAsync();
                allCustomBadges.Add(badgeText?.Trim() ?? "");
            }

            // Assert - Pages should be displayed in order
            // We can't verify exact Order property values from E2E test, but we can verify:
            // 1. All custom pages are present
            // 2. Order is consistent (no duplicates, all unique)
            allCustomBadges.Should().OnlyHaveUniqueItems("custom pages should not be duplicated");
            allCustomBadges.Should().AllSatisfy(text => text.Should().NotBeNullOrWhiteSpace("all custom page badges should have text"));

            // Note: The exact ordering is tested in component tests (SectionCardTests.cs)
            // This E2E test verifies the ordering is applied and consistent in the rendered page
        }
    }

    [Fact]
    public async Task HomePage_SectionCard_ExpandButton_DoesNotPreventCardNavigation()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        var expandButtons = Page.Locator(".badge-expandable[data-expand-target]");
        var buttonCount = await expandButtons.CountAsync();

        if (buttonCount > 0)
        {
            var expandButton = expandButtons.First;

            // Get the section card container
            var sectionCard = expandButton.Locator("xpath=ancestor::div[contains(@class, 'section-card-container')]//a[contains(@class, 'section-card')]").First;

            // Act - Click expand button should not navigate
            var initialUrl = Page.Url;
            var targetId2 = await expandButton.GetAttributeAsync("data-expand-target");
            await expandButton.ClickAndExpectAsync(async () =>
                await Assertions.Expect(Page.Locator($"#{targetId2}")).ToBeVisibleAsync());

            // Assert - Should still be on homepage
            Page.Url.Should().Be(initialUrl, "clicking expand button should not navigate away from homepage");

            // Section card link should still be clickable and navigate correctly
            var sectionUrl = await sectionCard.GetAttributeAsync("href");
            sectionUrl.Should().NotBeNullOrWhiteSpace("section card should have valid href");
        }
    }

    [Fact]
    public async Task HomePage_SectionCard_CustomPageBadges_AreClickableLinks()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act - Find custom page badges
        var customBadges = Page.Locator(".badge-custom");
        var badgeCount = await customBadges.CountAsync();

        if (badgeCount > 0)
        {
            var firstBadge = customBadges.First;

            // Assert - Badge should be a link
            var tagName = await firstBadge.EvaluateAsync<string>("el => el.tagName");
            tagName.Should().Be("A", "custom page badges should be <a> elements");

            // Should have valid href
            var href = await firstBadge.GetAttributeAsync("href");
            href.Should().NotBeNullOrWhiteSpace("custom page badge should have valid href");
            href.Should().StartWith("/", "custom page badge should link to internal page");

            // Should have aria-label for accessibility
            var ariaLabel = await firstBadge.GetAttributeAsync("aria-label");
            ariaLabel.Should().NotBeNullOrWhiteSpace("custom page badge should have aria-label for accessibility");
        }
    }

    [Fact]
    public async Task HomePage_SectionCard_ExpandedCustomPages_AreClickableLinks()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        var expandButtons = Page.Locator(".badge-expandable[data-expand-target]");
        var buttonCount = await expandButtons.CountAsync();

        if (buttonCount > 0)
        {
            var expandButton = expandButtons.First;
            var targetId = await expandButton.GetAttributeAsync("data-expand-target");

            // Expand to reveal hidden custom pages
            await expandButton.ClickAndExpectAsync(async () =>
                await Assertions.Expect(Page.Locator($"#{targetId}")).ToBeVisibleAsync());

            var targetContainer = Page.Locator($"#{targetId}");
            var hiddenBadges = targetContainer.Locator(".badge-custom");
            var hiddenCount = await hiddenBadges.CountAsync();

            if (hiddenCount > 0)
            {
                var firstHiddenBadge = hiddenBadges.First;

                // Assert - Hidden badge should be a clickable link
                var tagName = await firstHiddenBadge.EvaluateAsync<string>("el => el.tagName");
                tagName.Should().Be("A", "hidden custom page badges should be <a> elements");

                var href = await firstHiddenBadge.GetAttributeAsync("href");
                href.Should().NotBeNullOrWhiteSpace("hidden custom page badge should have valid href");
                href.Should().StartWith("/", "hidden custom page badge should link to internal page");

                var ariaLabel = await firstHiddenBadge.GetAttributeAsync("aria-label");
                ariaLabel.Should().NotBeNullOrWhiteSpace("hidden custom page badge should have aria-label");
            }
        }
    }

    [Fact]
    public async Task HomePage_NoJavaScriptErrors_WhenExpandingCustomPages()
    {
        // Arrange
        var consoleErrors = new List<string>();
        Page.Console += (_, msg) =>
        {
            if (msg.Type == "error")
            {
                // Filter out infrastructure errors (WebSocket, SignalR transport, Aspire dashboard, etc.)
                var text = msg.Text;
                if (!text.Contains("WebSocket connection") &&
                    !text.Contains("WebSocket failed to connect") &&
                    !text.Contains("Failed to start the transport") &&
                    !text.Contains("ERR_CONNECTION_REFUSED") &&
                    !text.Contains("wss://"))
                {
                    consoleErrors.Add(text);
                }
            }
        };

        await Page.GotoRelativeAsync("/");

        // Assert — Blazor Server circuit must be established before we do anything.
        // GotoRelativeAsync already waits for __blazorServerReady via WaitForBlazorReadyAsync;
        // this explicit assertion makes the requirement visible and produces a clear failure
        // message when the circuit never connects (e.g. WebSocket blocked by a proxy, or
        // sticky sessions misconfigured so the negotiate/WebSocket requests hit different replicas).
        var circuitActiveBeforeInteraction = await Page.EvaluateAsync<bool>(
            "() => window.__blazorServerReady === true");
        circuitActiveBeforeInteraction.Should().BeTrue(
            "Blazor Server SignalR circuit must be established before interacting with the page");

        var expandButtons = Page.Locator(".badge-expandable[data-expand-target]");
        var buttonCount = await expandButtons.CountAsync();

        if (buttonCount > 0)
        {
            // Act - Click expand button
            var expandButton = expandButtons.First;
            var targetId = await expandButton.GetAttributeAsync("data-expand-target");
            await expandButton.ClickAndExpectAsync(async () =>
                await Assertions.Expect(Page.Locator($"#{targetId}")).ToBeVisibleAsync());

            // Assert — circuit must still be active after an interactive action.
            // If sticky sessions are broken, a user interaction can route to a different
            // replica which doesn't own the circuit, causing it to drop. A dropped circuit
            // means all @onclick handlers stop working and the page becomes a dead shell.
            var circuitActiveAfterInteraction = await Page.EvaluateAsync<bool>(
                "() => window.__blazorServerReady === true");
            circuitActiveAfterInteraction.Should().BeTrue(
                "Blazor Server circuit should remain active after interacting with the page — " +
                "if it dropped, sticky sessions may be misconfigured on the Container App");

            // Assert - No JavaScript errors
            consoleErrors.Should().BeEmpty("expanding custom pages should not cause JavaScript errors");
        }
    }
}
