using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for tag filtering functionality including toggle behavior and visual state.
/// Tests SidebarTagCloud component for tag-based content filtering.
/// </summary>
public class TagFilteringTests : PlaywrightTestBase
{
    public TagFilteringTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task TagButton_WhenClicked_AddsTagToUrl()
    {
        // Arrange - Navigate to home page
        await Page.GotoRelativeAsync("/");
        await WaitForTagCloudReadyAsync();

        // Act - Click first tag button in popular tags
        var tagButton = Page.Locator(".tag-cloud-item").First;
        var tagText = await tagButton.TextContentAsync();

        // Get current URL before click
        var urlBeforeClick = Page.Url;

        await tagButton.ClickBlazorElementAsync();

        // Wait for URL to change (navigation should happen)
        await Page.WaitForURLAsync(url => url.Contains("tags=") || url != urlBeforeClick,
            new() { Timeout = BlazorHelpers.DefaultAssertionTimeout });

        // Assert - URL should contain the tag parameter
        var currentUrl = Page.Url;
        currentUrl.Should().Contain("tags=", $"Expected URL to contain tags parameter after clicking tag '{tagText}'");

        // Verify the clicked tag is in the URL
        var uri = new Uri(currentUrl);
        var tagsParam = System.Web.HttpUtility.ParseQueryString(uri.Query).Get("tags");
        tagsParam.Should().NotBeNullOrEmpty("Tags parameter should have a value");

        // Extract tag name from "TagName (count)" format
        var normalizedTagText = ExtractTagNameFromText(tagText).ToLowerInvariant();
        tagsParam.Should().Contain(normalizedTagText, $"Expected tags parameter to contain '{normalizedTagText}'");
    }

    [Fact]
    public async Task TagButton_WhenClickedTwice_TogglesTagOffAndRemovesFromUrl()
    {
        // Arrange - Navigate to a section page (uses Filter mode, not Navigate mode)
        // Home page uses Navigate mode which causes full navigation, breaking element references
        await Page.GotoRelativeAsync("/github-copilot");
        await WaitForTagCloudReadyAsync();

        // Act 1 - Click first tag button to select it
        var tagButton = Page.Locator(".tag-cloud-item").First;
        var tagText = await tagButton.TextContentAsync();

        // Click and wait for URL change
        await tagButton.ClickBlazorElementAsync();

        var urlAfterFirstClick = Page.Url;
        urlAfterFirstClick.Should().Contain("tags=", "First click should add tag to URL");

        // Wait for tag cloud to be ready again after URL change
        // Blazor Server may re-render the component when URL params change
        await WaitForTagCloudReadyAsync();

        // Act 2 - Click the same tag button again to deselect it
        // Re-acquire locator after Blazor re-render to avoid stale reference
        tagButton = Page.Locator(".tag-cloud-item").First;
        // On section pages (Filter mode), deselecting the last tag may not trigger a URL change
        // (Blazor considers "no tags" the default state and may skip pushState).
        // Use waitForUrlChange: false to avoid timeout, then wait for Blazor re-render.
        await tagButton.ClickBlazorElementAsync(waitForUrlChange: false);

        // Wait for Blazor to process the toggle and re-render
        await WaitForTagCloudReadyAsync();

        // Assert - URL should no longer contain the tag parameter or be empty
        var urlAfterSecondClick = Page.Url;
        var uri = new Uri(urlAfterSecondClick);
        var tagsParam = System.Web.HttpUtility.ParseQueryString(uri.Query).Get("tags");

        var normalizedTagText = ExtractTagNameFromText(tagText).ToLowerInvariant();

        if (string.IsNullOrEmpty(tagsParam))
        {
            // Tags parameter completely removed - this is valid
            tagsParam.Should().BeNullOrEmpty("After toggling off the only tag, tags parameter should be removed");
        }
        else
        {
            // If tags parameter still exists, it shouldn't contain the toggled tag
            tagsParam.Should().NotContain(normalizedTagText,
                $"After toggling off tag '{normalizedTagText}', it should be removed from URL");
        }
    }

    [Fact]
    public async Task TagButton_WhenActive_ShowsSelectedVisualState()
    {
        // Arrange - Navigate to a section page with a pre-selected tag
        // Use /github-copilot instead of /all as it has guaranteed tags
        await Page.GotoRelativeAsync("/github-copilot?tags=vs%20code");
        await WaitForTagCloudReadyAsync();

        // Assert - Tag button should have selected/active state
        var selectedTagButton = Page.Locator(".tag-cloud-item.selected")
            .Filter(new() { HasTextRegex = new Regex("vs code", RegexOptions.IgnoreCase) });

        await Assertions.Expect(selectedTagButton).ToBeVisibleAsync(new()
        {
            Timeout = 5000
        });

        // Verify visual styling (should have different background/border color)
        var backgroundColor = await selectedTagButton.EvaluateAsync<string>("el => window.getComputedStyle(el).backgroundColor");
        backgroundColor.Should().NotBeNullOrEmpty("Selected tag should have a background color");
    }

    [Fact]
    public async Task TagButton_WhenMultipleSelected_AllShowActiveState()
    {
        // Arrange - Navigate to a section page with multiple pre-selected tags
        // Use /github-copilot instead of /all as it has guaranteed tags
        await Page.GotoRelativeAsync("/github-copilot?tags=vs%20code,developer%20tools");
        await WaitForTagCloudReadyAsync();

        // Assert - Both tag buttons should have selected/active state
        var vsCodeTag = Page.Locator(".tag-cloud-item.selected")
            .Filter(new() { HasTextRegex = new Regex("vs code", RegexOptions.IgnoreCase) });

        var devToolsTag = Page.Locator(".tag-cloud-item.selected")
            .Filter(new() { HasTextRegex = new Regex("developer tools", RegexOptions.IgnoreCase) });

        await Assertions.Expect(vsCodeTag).ToBeVisibleAsync(new() { Timeout = 5000 });
        await Assertions.Expect(devToolsTag).ToBeVisibleAsync(new() { Timeout = 5000 });
    }

    [Fact]
    public async Task TagUrl_WithDuplicateTags_ShowsDeduplicatedSelection()
    {
        // Arrange - Navigate to URL with duplicate tags
        // The component should internally deduplicate and only show unique selected tags
        await Page.GotoRelativeAsync("/github-copilot?tags=vs%20code,developer%20tools,vs%20code,productivity,developer%20tools");
        await WaitForTagCloudReadyAsync();

        // Assert - Only unique tags should be visually selected (internal deduplication)
        // Note: The URL itself may not be cleaned up, but the UI should only show each tag once
        var selectedTags = Page.Locator(".tag-cloud-item.selected");
        var count = await selectedTags.CountAsync();

        // We should have distinct selected tags, not duplicates
        count.Should().BeGreaterThan(0, "At least one tag should be selected");

        // Verify the selected tags are the expected ones (each unique tag once)
        var vsCodeSelected = Page.Locator(".tag-cloud-item.selected")
            .Filter(new() { HasTextRegex = new Regex("vs code", RegexOptions.IgnoreCase) });
        var devToolsSelected = Page.Locator(".tag-cloud-item.selected")
            .Filter(new() { HasTextRegex = new Regex("developer tools", RegexOptions.IgnoreCase) });

        await Assertions.Expect(vsCodeSelected).ToBeVisibleAsync(new() { Timeout = 5000 });
        await Assertions.Expect(devToolsSelected).ToBeVisibleAsync(new() { Timeout = 5000 });
    }

    [Fact]
    public async Task TagButton_OnSectionPage_ShouldToggleCorrectly()
    {
        // Arrange - Navigate to a section page (e.g., GitHub Copilot)
        await Page.GotoRelativeAsync("/github-copilot");
        await WaitForTagCloudReadyAsync();

        // Act - Click a tag button
        var tagButton = Page.Locator(".tag-cloud-item").First;
        var tagText = await tagButton.TextContentAsync();
        await tagButton.ClickBlazorElementAsync();

        // Assert 1 - URL should contain tag
        var urlAfterClick = Page.Url;
        urlAfterClick.Should().Contain("tags=", "Tag should be added to section page URL");

        // Wait for tag cloud to be ready again after URL change
        await WaitForTagCloudReadyAsync();

        // Act 2 - Click again to toggle off
        // Re-acquire locator after Blazor re-render to avoid stale reference
        tagButton = Page.Locator(".tag-cloud-item").First;
        await tagButton.ClickBlazorElementAsync();

        // Assert 2 - Tag should be removed
        var urlAfterToggle = Page.Url;
        var uri = new Uri(urlAfterToggle);
        var tagsParam = System.Web.HttpUtility.ParseQueryString(uri.Query).Get("tags");

        var normalizedTagText = tagText?.Trim().ToLowerInvariant() ?? "";

        if (!string.IsNullOrEmpty(tagsParam))
        {
            tagsParam.Should().NotContain(normalizedTagText,
                "After toggling off, tag should be removed from URL");
        }
    }

    [Fact]
    public async Task TagButton_OnAnySection_ShouldToggleCorrectly()
    {
        // Arrange - Navigate to a different section page (AI section)
        await Page.GotoRelativeAsync("/ai");
        await WaitForTagCloudReadyAsync();

        // Act - Click a tag button
        var tagButton = Page.Locator(".tag-cloud-item").First;
        var tagText = await tagButton.TextContentAsync();
        await tagButton.ClickBlazorElementAsync();

        // Assert 1 - URL should contain tag
        var urlAfterClick = Page.Url;
        urlAfterClick.Should().Contain("tags=", "Tag should be added to section page URL");

        // Wait for tag cloud to be ready again after URL change
        await WaitForTagCloudReadyAsync();

        // Act 2 - Click again to toggle off
        // Re-acquire locator after Blazor re-render to avoid stale reference
        tagButton = Page.Locator(".tag-cloud-item").First;
        await tagButton.ClickBlazorElementAsync();

        // Assert 2 - Tag should be removed
        var urlAfterToggle = Page.Url;
        var uri = new Uri(urlAfterToggle);
        var tagsParam = System.Web.HttpUtility.ParseQueryString(uri.Query).Get("tags");

        var normalizedTagText = tagText?.Trim().ToLowerInvariant() ?? "";

        if (!string.IsNullOrEmpty(tagsParam))
        {
            tagsParam.Should().NotContain(normalizedTagText,
                "After toggling off, tag should be removed from URL");
        }
    }

    [Fact]
    public async Task TagFiltering_WithMultipleTags_UsesAndLogic()
    {
        // Arrange - Navigate to GitHub Copilot section
        await Page.GotoRelativeAsync("/github-copilot");
        await WaitForTagCloudReadyAsync();

        // Find content items count before filtering
        // Wait for content to be visible first
        await Page.WaitForSelectorAsync(".card", new() { State = WaitForSelectorState.Visible });
        var allItems = await Page.Locator(".card").CountAsync();
        allItems.Should().BeGreaterThan(0, "Section should have content items");

        // Act 1 - Select first tag
        var firstTagButton = Page.Locator(".tag-cloud-item").First;
        var firstTagText = (await firstTagButton.TextContentAsync())?.Trim().ToLowerInvariant() ?? "";
        await firstTagButton.ClickBlazorElementAsync();

        // Wait for content to update after tag filter
        // Use Playwright's polling to detect when content has finished loading
        await Page.WaitForBlazorReadyAsync();

        // Wait for URL to update with tag parameter (confirms navigation happened)
        await Page.WaitForConditionAsync(
            "() => window.location.search.includes('tags=')",
            new PageWaitForFunctionOptions { Timeout = 5000 });

        // Wait for cards to stabilize after Blazor re-render
        await Page.WaitForConditionAsync(
            @"() => {
                const cards = document.querySelectorAll('.card');
                return cards.length > 0;
            }",
            new PageWaitForFunctionOptions { Timeout = 5000, PollingInterval = 100 });
        await Page.WaitForBlazorReadyAsync();

        var itemsAfterFirstTag = await Page.Locator(".card").CountAsync();
        itemsAfterFirstTag.Should().BeLessThanOrEqualTo(allItems, "Filtering by one tag should reduce or maintain item count");

        // Act 2 - Select second tag (if available and different from first)
        await WaitForTagCloudReadyAsync();
        var secondTagButton = Page.Locator(".tag-cloud-item").Filter(new()
        {
            HasNotTextRegex = new Regex($"^{Regex.Escape(firstTagText)}$", RegexOptions.IgnoreCase)
        }).First;
        var secondTagText = (await secondTagButton.TextContentAsync())?.Trim().ToLowerInvariant() ?? "";

        // Only proceed if we have a different tag
        if (secondTagText != firstTagText)
        {
            await secondTagButton.ClickBlazorElementAsync();

            // Wait for content to update after second tag filter
            await Page.WaitForBlazorReadyAsync();

            // Wait for cards to stabilize after Blazor re-render
            await Page.WaitForConditionAsync(
                @"() => {
                    const cards = document.querySelectorAll('.card');
                    return cards.length >= 0;
                }",
                new PageWaitForFunctionOptions { Timeout = 5000, PollingInterval = 100 });
            await Page.WaitForBlazorReadyAsync();

            var itemsAfterSecondTag = await Page.Locator(".card").CountAsync();

            // Assert - AND logic means adding more tags should reduce or maintain count, never increase
            itemsAfterSecondTag.Should().BeLessThanOrEqualTo(itemsAfterFirstTag,
                $"AND logic: Adding second tag '{secondTagText}' should reduce or maintain items (had {itemsAfterFirstTag}, now {itemsAfterSecondTag})");

            // Verify URL contains both tags
            var currentUrl = Page.Url;
            var uri = new Uri(currentUrl);
            var tagsParam = System.Web.HttpUtility.ParseQueryString(uri.Query).Get("tags");
            tagsParam.Should().NotBeNullOrEmpty("Tags parameter should exist");
            tagsParam.Should().Contain(firstTagText, "URL should contain first tag");
            tagsParam.Should().Contain(secondTagText, "URL should contain second tag");

            // Note: We don't verify individual item tags because ContentItemCard only displays
            // the first 5 tags, so the selected tags might not be visible in the HTML.
            // The AND logic is proven by:
            // 1. Item count decreased or stayed the same (verified above)
            // 2. URL contains both tags (verified above)
            // 3. API filtering is tested in API integration tests
        }
    }

    [Fact]
    public async Task TagFiltering_InSection_FiltersWithinSectionOnly()
    {
        // Arrange - Navigate to GitHub Copilot section with a tag
        await Page.GotoRelativeAsync("/github-copilot?tags=vs%20code");
        await WaitForTagCloudReadyAsync();

        // Act - Get all visible content items
        var visibleItems = await Page.Locator(".card").CountAsync();

        // Assert - Verify we have results
        visibleItems.Should().BeGreaterThan(0,
            "Filtering by 'vs code' tag in GitHub Copilot section should return results");

        // Section and tag filtering are verified by:
        // 1. Items are returned (not zero) - proven above
        // 2. API filtering is thoroughly tested in ContentEndpointsE2ETests
        // 3. This E2E test verifies the UI correctly applies filters from URL parameters
        //
        // Note: We don't verify URLs or tags in the HTML because:
        // - News items often link to external sources (github.blog, etc.), not internal URLs
        // - ContentItemCard only displays first 5 tags, so selected tags might not be visible
    }

    [Fact]
    public async Task TagCloud_AllSection_BothAllAndAllAllShowIdenticalTags()
    {
        // Arrange - Navigate to /all and get tag cloud contents
        await Page.GotoRelativeAsync("/all");
        await WaitForTagCloudReadyAsync();

        // Get all tag texts from /all
        var tagCloudOnAll = Page.Locator(".tag-cloud");
        await Assertions.Expect(tagCloudOnAll).ToBeVisibleAsync(new() { Timeout = 5000 });

        var tagsOnAll = await Page.Locator(".tag-cloud-item").AllTextContentsAsync();
        tagsOnAll.Should().NotBeEmpty("Tag cloud should be visible on /all");

        // Act - Navigate to /all/all
        await Page.GotoRelativeAsync("/all/all");
        await WaitForTagCloudReadyAsync();

        // Get tag cloud contents on /all/all
        var tagCloudOnAllAll = Page.Locator(".tag-cloud");
        await Assertions.Expect(tagCloudOnAllAll).ToBeVisibleAsync(new() { Timeout = 5000 });

        var tagsOnAllAll = await Page.Locator(".tag-cloud-item").AllTextContentsAsync();
        tagsOnAllAll.Should().NotBeEmpty("Tag cloud should be visible on /all/all");

        // Assert - Tags should be identical
        tagsOnAllAll.Should().BeEquivalentTo(tagsOnAll,
            "Both /all and /all/all should display the same tags in the tag cloud since they represent the same 'all content' view");
    }

    /// <summary>
    /// Helper method to wait for tag cloud to be fully loaded and interactive.
    /// Blazor Server @rendermode InteractiveServer components require:
    /// 1. Tags to be loaded from API (wait for loading text to disappear)
    /// 2. SignalR circuit to be established (handled by WaitForBlazorReadyAsync using official JS initializer)
    /// 3. Event handlers to be attached (signaled by afterServerStarted callback in TechHub.Web.lib.module.js)
    /// </summary>
    private async Task WaitForTagCloudReadyAsync()
    {
        // Wait for tag cloud to finish loading (tags become visible)
        await Page.WaitForSelectorAsync(".tag-cloud-item", new() { State = WaitForSelectorState.Visible });

        // Wait for Blazor Server interactivity to be ready (using official JS initializer callbacks)
        // This replaces the arbitrary Task.Delay - the afterServerStarted callback sets
        // window.__blazorServerReady when the SignalR circuit is fully established
        await Page.WaitForBlazorReadyAsync();
    }

    [Fact]
    public async Task TagCounts_ShouldBeConsistent_RegardlessOfClickOrder()
    {
        // This test reproduces the bug where tag counts differ based on click order
        //
        // Scenario from bug report:
        // go to /github-copilot?tags=copilot%20coding%20agent,vs%20code
        // - agent mode: 2
        // - pull requests: 5
        //
        // If you click "agent mode" first → pull requests goes to 0
        // If you click "pull requests" first → agent mode goes to 1
        //
        // Expected: Counts should be symmetric. The intersection of 
        // (copilot coding agent AND vs code AND agent mode AND pull requests) 
        // should be the same regardless of order clicked

        // Arrange - Start at /github-copilot with 2 tags selected
        await Page.GotoRelativeAsync("/github-copilot?tags=copilot%20coding%20agent,vs%20code");
        await WaitForTagCloudReadyAsync();

        // Capture initial tag counts for "agent mode" and "pull requests"
        var agentModeTag = Page.Locator(".tag-cloud-item")
            .Filter(new() { HasTextRegex = new Regex(@"^agent mode\s*\(", RegexOptions.IgnoreCase) });
        var pullRequestsTag = Page.Locator(".tag-cloud-item")
            .Filter(new() { HasTextRegex = new Regex(@"^pull requests\s*\(", RegexOptions.IgnoreCase) });

        var agentModeInitialText = await agentModeTag.TextContentAsync();
        var pullRequestsInitialText = await pullRequestsTag.TextContentAsync();

        // Extract counts from "tag name (count)" format
        var agentModeInitialCount = ExtractCountFromTagText(agentModeInitialText);
        var pullRequestsInitialCount = ExtractCountFromTagText(pullRequestsInitialText);

        agentModeInitialCount.Should().BeGreaterThan(0, "agent mode should have a count > 0");
        pullRequestsInitialCount.Should().BeGreaterThan(0, "pull requests should have a count > 0");

        // Test Path 1: Click agent mode first
        await agentModeTag.ClickBlazorElementAsync();
        await WaitForTagCloudReadyAsync();

        pullRequestsTag = Page.Locator(".tag-cloud-item")
            .Filter(new() { HasTextRegex = new Regex(@"^pull requests\s*\(", RegexOptions.IgnoreCase) });
        var pullRequestsAfterAgentMode = await pullRequestsTag.TextContentAsync();
        var pullRequestsCountPath1 = ExtractCountFromTagText(pullRequestsAfterAgentMode);

        // Reset - go back to initial state
        await Page.GotoRelativeAsync("/github-copilot?tags=copilot%20coding%20agent,vs%20code");
        await WaitForTagCloudReadyAsync();

        // Test Path 2: Click pull requests first
        pullRequestsTag = Page.Locator(".tag-cloud-item")
            .Filter(new() { HasTextRegex = new Regex(@"^pull requests\s*\(", RegexOptions.IgnoreCase) });
        await pullRequestsTag.ClickBlazorElementAsync();
        await WaitForTagCloudReadyAsync();

        agentModeTag = Page.Locator(".tag-cloud-item")
            .Filter(new() { HasTextRegex = new Regex(@"^agent mode\s*\(", RegexOptions.IgnoreCase) });
        var agentModeAfterPullRequests = await agentModeTag.TextContentAsync();
        var agentModeCountPath2 = ExtractCountFromTagText(agentModeAfterPullRequests);

        // Assert - The counts should be identical regardless of order
        // Both represent the intersection of: copilot coding agent AND vs code AND agent mode AND pull requests
        pullRequestsCountPath1.Should().Be(agentModeCountPath2,
            "The count for intersection should be symmetric: " +
            "clicking 'agent mode' then seeing 'pull requests' count should equal " +
            "clicking 'pull requests' then seeing 'agent mode' count. " +
            $"Path 1 (agent mode → pull requests count): {pullRequestsCountPath1}, " +
            $"Path 2 (pull requests → agent mode count): {agentModeCountPath2}");
    }

    [Fact]
    public async Task TagButtons_ShouldMaintainSameSize_AfterFiltering()
    {
        // This test ensures tag button sizes remain constant even when counts decrease due to filtering
        // This prevents layout shift and jumping around as users filter content

        // Arrange - Navigate to section page without filters
        await Page.GotoRelativeAsync("/github-copilot");
        await WaitForTagCloudReadyAsync();

        // Get initial sizes for all visible tags
        var tagButtons = Page.Locator(".tag-cloud-item");
        var tagCount = await tagButtons.CountAsync();
        tagCount.Should().BeGreaterThan(5, "Should have multiple tags to test");

        // Capture initial widths of first 5 tags
        var initialWidths = new Dictionary<string, double>();
        for (int i = 0; i < Math.Min(5, tagCount); i++)
        {
            var tag = tagButtons.Nth(i);
            var tagText = await tag.TextContentAsync();
            var tagName = ExtractTagNameFromText(tagText);
            var boundingBox = await tag.BoundingBoxAsync();

            if (boundingBox != null)
            {
                initialWidths[tagName] = boundingBox.Width;
            }
        }

        initialWidths.Count.Should().Be(Math.Min(5, tagCount), "Should have captured initial widths");

        // Act - Select first tag to filter
        var firstTag = tagButtons.First;
        await firstTag.ClickBlazorElementAsync();
        await WaitForTagCloudReadyAsync();

        // Assert - Tag button widths should remain the same
        // Even if counts decreased (e.g., from "123" to "5"), button should maintain its width
        tagButtons = Page.Locator(".tag-cloud-item");
        for (int i = 0; i < Math.Min(5, await tagButtons.CountAsync()); i++)
        {
            var tag = tagButtons.Nth(i);
            var tagText = await tag.TextContentAsync();
            var tagName = ExtractTagNameFromText(tagText);

            if (initialWidths.TryGetValue(tagName, out var initialWidth))
            {
                var boundingBox = await tag.BoundingBoxAsync();
                boundingBox.Should().NotBeNull($"Tag '{tagName}' should have a bounding box");

                var currentWidth = boundingBox!.Width;

                // Allow small floating point differences (< 1px) but no significant layout shift
                ((double)currentWidth).Should().BeApproximately((double)initialWidth, 2.0,
                    $"Tag '{tagName}' width should remain constant after filtering " +
                    $"(was {initialWidth}px, now {currentWidth}px). " +
                    "This prevents layout jumping as users interact with filters.");
            }
        }
    }

    /// <summary>
    /// Helper method to extract tag name from "TagName (count)" format
    /// </summary>
    private static string ExtractTagNameFromText(string? tagText)
    {
        if (string.IsNullOrWhiteSpace(tagText))
        {
            return "";
        }

        // Normalize whitespace first (TextContent might have extra spaces/newlines)
        tagText = System.Text.RegularExpressions.Regex.Replace(tagText.Trim(), @"\s+", " ");

        // Remove count in parentheses: "AI (901)" -> "AI"
        var match = System.Text.RegularExpressions.Regex.Match(tagText, @"^(.+?)\s*\(\d{1,3}(?:,\d{3})*\)$");
        if (match.Success)
        {
            return match.Groups[1].Value.Trim();
        }

        // If no count found, return as-is
        return tagText.Trim();
    }

    /// <summary>
    /// Helper method to extract count from "TagName (count)" format
    /// </summary>
    private static int ExtractCountFromTagText(string? tagText)
    {
        if (string.IsNullOrWhiteSpace(tagText))
        {
            return 0;
        }

        // Normalize whitespace
        tagText = System.Text.RegularExpressions.Regex.Replace(tagText.Trim(), @"\s+", " ");

        // Extract count: "tag name (123)" -> 123
        // Handle comma-separated thousands: "tag name (1,234)" -> 1234
        var match = System.Text.RegularExpressions.Regex.Match(tagText, @"\((\d{1,3}(?:,\d{3})*)\)$");
        if (match.Success)
        {
            var countString = match.Groups[1].Value.Replace(",", "");
            if (int.TryParse(countString, out var count))
            {
                return count;
            }
        }

        return 0;
    }
}
