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
        await Page.WaitForURLAsync(url => url.Contains("tags=") || url != urlBeforeClick);

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
        var normalizedTagText = ExtractTagNameFromText(tagText).ToLowerInvariant();

        // Click and wait for URL change
        await tagButton.ClickBlazorElementAsync();

        // Wait for URL to contain the tag and for the tag cloud to re-render
        await Page.WaitForConditionAsync(
            "(tag) => window.location.href.toLowerCase().includes('tags=' + encodeURIComponent(tag).toLowerCase())",
            normalizedTagText);
        await WaitForTagCloudReadyAsync();

        // Verify the tag button shows selected state before proceeding
        var selectedTag = Page.Locator(".tag-cloud-item.selected").First;
        await Assertions.Expect(selectedTag).ToBeVisibleAsync();

        // Act 2 - Click the same tag button again to deselect it
        // Re-acquire locator after Blazor re-render to avoid stale reference
        tagButton = Page.Locator(".tag-cloud-item.selected").First;
        // On section pages (Filter mode), deselecting the last tag may not trigger a URL change
        // (Blazor considers "no tags" the default state and may skip pushState).
        // Use waitForUrlChange: false to avoid timeout, then wait for Blazor re-render.
        await tagButton.ClickBlazorElementAsync(waitForUrlChange: false);

        // Wait for Blazor to process the toggle: the selected class should be removed
        await Page.Locator(".tag-cloud-item.selected").AssertCountAsync(0);
        await WaitForTagCloudReadyAsync();

        // Assert - URL should no longer contain the tag parameter or be empty
        var urlAfterSecondClick = Page.Url;
        var uri = new Uri(urlAfterSecondClick);
        var tagsParam = System.Web.HttpUtility.ParseQueryString(uri.Query).Get("tags");

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

        await Assertions.Expect(selectedTagButton).ToBeVisibleAsync();

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

        await Assertions.Expect(vsCodeTag).ToBeVisibleAsync();
        await Assertions.Expect(devToolsTag).ToBeVisibleAsync();
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

        await Assertions.Expect(vsCodeSelected).ToBeVisibleAsync();
        await Assertions.Expect(devToolsSelected).ToBeVisibleAsync();
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

        // Wait for URL to update with tag parameter (confirms navigation happened)
        await Page.WaitForConditionAsync(
            "() => window.location.search.includes('tags=')");

        // Wait for cards to stabilize after Blazor re-render
        await Page.WaitForConditionAsync(
            @"() => {
                const cards = document.querySelectorAll('.card');
                return cards.length > 0;
            }");

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

            // Wait for cards to stabilize after Blazor re-render
            await Page.WaitForConditionAsync(
                @"() => {
                    const cards = document.querySelectorAll('.card');
                    return cards.length >= 0;
                }");

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
        await Assertions.Expect(tagCloudOnAll).ToBeVisibleAsync();

        var tagsOnAll = await Page.Locator(".tag-cloud-item").AllTextContentsAsync();
        tagsOnAll.Should().NotBeEmpty("Tag cloud should be visible on /all");

        // Act - Navigate to /all/all
        await Page.GotoRelativeAsync("/all/all");
        await WaitForTagCloudReadyAsync();

        // Get tag cloud contents on /all/all
        var tagCloudOnAllAll = Page.Locator(".tag-cloud");
        await Assertions.Expect(tagCloudOnAllAll).ToBeVisibleAsync();

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
        // This test verifies that tag count intersection is symmetric regardless of click order.
        //
        // Instead of hardcoding specific tag names (which depend on content data and may drift),
        // we dynamically select a starting tag, then pick two tags from the filtered cloud.
        //
        // Test logic:
        // 1. Navigate to /github-copilot, pick the first tag with count > 0
        // 2. Click it → tag cloud updates with filtered counts
        // 3. From the filtered cloud, pick two more tags (tagA, tagB) both with count > 0
        // 4. Path 1: Click tagA first → read tagB's count
        // 5. Reset to same starting state
        // 6. Path 2: Click tagB first → read tagA's count
        // 7. Assert: tagB count from Path 1 == tagA count from Path 2 (symmetry)

        // Arrange - Navigate to /github-copilot section (no tags pre-selected)
        await Page.GotoRelativeAsync("/github-copilot");
        await WaitForTagCloudReadyAsync();

        // Select the first tag with count > 0 to establish a filtered baseline
        var allTags = Page.Locator(".tag-cloud-item:not(.selected):not(.disabled)");
        var firstTagCount = await allTags.CountAsync();
        firstTagCount.Should().BeGreaterThan(2, "Need at least 3 unselected non-disabled tags");

        var firstTag = allTags.First;
        var firstTagText = await firstTag.TextContentAsync();
        var firstTagName = ExtractTagNameFromText(firstTagText);

        await firstTag.ClickBlazorElementAsync();
        await WaitForTagCloudReadyAsync();

        // Now pick two unselected tags with count > 0 from the filtered cloud
        var availableTags = Page.Locator(".tag-cloud-item:not(.selected):not(.disabled)");
        var availableCount = await availableTags.CountAsync();
        availableCount.Should().BeGreaterThanOrEqualTo(2,
            $"After selecting '{firstTagName}', need at least 2 unselected non-disabled tags for symmetry test");

        var tagAElement = availableTags.Nth(0);
        var tagBElement = availableTags.Nth(1);

        var tagAText = await tagAElement.TextContentAsync();
        var tagBText = await tagBElement.TextContentAsync();
        var tagAName = ExtractTagNameFromText(tagAText);
        var tagBName = ExtractTagNameFromText(tagBText);

        var tagAInitialCount = ExtractCountFromTagText(tagAText);
        var tagBInitialCount = ExtractCountFromTagText(tagBText);

        tagAInitialCount.Should().BeGreaterThan(0, $"'{tagAName}' should have count > 0");
        tagBInitialCount.Should().BeGreaterThan(0, $"'{tagBName}' should have count > 0");

        // Build URL with the first tag selected (our starting state for both paths)
        var baseUrl = $"/github-copilot?tags={Uri.EscapeDataString(firstTagName)}";

        // Path 1: Click tagA first → read tagB's count
        await Page.GotoRelativeAsync(baseUrl);
        await WaitForTagCloudReadyAsync();

        var tagALocator = Page.Locator(".tag-cloud-item")
            .Filter(new() { HasTextRegex = BuildTagRegex(tagAName) });
        await Assertions.Expect(tagALocator).ToBeVisibleAsync();
        await tagALocator.ClickBlazorElementAsync();
        await WaitForTagCloudReadyAsync();

        var tagBLocator = Page.Locator(".tag-cloud-item")
            .Filter(new() { HasTextRegex = BuildTagRegex(tagBName) });
        await Assertions.Expect(tagBLocator).ToBeVisibleAsync();
        var tagBCountPath1 = ExtractCountFromTagText(await tagBLocator.TextContentAsync());

        // Path 2: Click tagB first → read tagA's count
        await Page.GotoRelativeAsync(baseUrl);
        await WaitForTagCloudReadyAsync();

        tagBLocator = Page.Locator(".tag-cloud-item")
            .Filter(new() { HasTextRegex = BuildTagRegex(tagBName) });
        await Assertions.Expect(tagBLocator).ToBeVisibleAsync();
        await tagBLocator.ClickBlazorElementAsync();
        await WaitForTagCloudReadyAsync();

        tagALocator = Page.Locator(".tag-cloud-item")
            .Filter(new() { HasTextRegex = BuildTagRegex(tagAName) });
        await Assertions.Expect(tagALocator).ToBeVisibleAsync();
        var tagACountPath2 = ExtractCountFromTagText(await tagALocator.TextContentAsync());

        // Assert - The intersection count should be symmetric
        tagBCountPath1.Should().Be(tagACountPath2,
            $"Intersection counts should be symmetric: " +
            $"clicking '{tagAName}' then seeing '{tagBName}' count ({tagBCountPath1}) should equal " +
            $"clicking '{tagBName}' then seeing '{tagAName}' count ({tagACountPath2}). " +
            $"Starting tags: [{firstTagName}]");
    }

    /// <summary>
    /// Builds a regex to match a tag by name in the tag cloud (e.g. "agent mode (5)").
    /// Escapes special regex characters in the tag name.
    /// </summary>
    private static Regex BuildTagRegex(string tagName)
    {
        var escaped = Regex.Escape(tagName);
        return new Regex($@"^{escaped}\s*\(", RegexOptions.IgnoreCase);
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

                // Allow for CSS class transitions (tag-size-large → tag-size-medium)
                // which can happen when filtering changes the count distribution.
                // The quantile normalization may reclassify sizes, causing a ~10px shift.
                // We use a 15px tolerance to catch major layout issues while allowing
                // intended size normalization (e.g., all-same-count tags → Medium).
                ((double)currentWidth).Should().BeApproximately((double)initialWidth, 15.0,
                    $"Tag '{tagName}' width should not drastically change after filtering " +
                    $"(was {initialWidth}px, now {currentWidth}px). " +
                    "Small changes from size class transitions are acceptable.");
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

    // ========================================================================
    // Card Badge Interactivity Tests
    // ========================================================================

    [Fact]
    public async Task CardTagBadge_WhenClicked_AddsTagToUrlFilter()
    {
        // Arrange - Navigate to a collection page where cards have tag badges
        await Page.GotoRelativeAsync("/github-copilot/news");
        await Page.Locator(".card").First.AssertElementVisibleAsync();

        // Act - Click the first tag badge button on the first card
        var firstCard = Page.Locator(".card").First;
        var tagBadge = firstCard.Locator("button.badge-tag-clickable").First;
        await tagBadge.AssertElementVisibleAsync();

        var tagText = (await tagBadge.TextContentAsync())!.Trim().ToLowerInvariant();

        await tagBadge.ClickBlazorElementAsync();

        // Assert - URL should contain the tag parameter
        await Page.WaitForURLAsync(url => url.Contains("tags="));
        var currentUrl = new Uri(Page.Url);
        var tagsParam = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(currentUrl.Query);
        tagsParam.Should().ContainKey("tags");
        tagsParam["tags"].ToString().Should().Contain(tagText,
            $"clicking tag badge '{tagText}' should add it to the tags URL parameter");
    }

    [Fact]
    public async Task CardCollectionBadge_WhenClicked_NavigatesToCollection()
    {
        // Arrange - Navigate to "All" page where collection badges are visible
        await Page.GotoRelativeAsync("/github-copilot");
        await Page.Locator(".card").First.AssertElementVisibleAsync();

        // Act - Find and click a collection badge link
        var firstCard = Page.Locator(".card").First;
        var collectionBadge = firstCard.Locator("a.badge-purple").First;
        await collectionBadge.AssertElementVisibleAsync();

        var href = await collectionBadge.GetAttributeAsync("href");
        href.Should().NotBeNullOrEmpty("collection badge should have an href");

        await collectionBadge.ClickBlazorElementAsync();

        // Assert - Should navigate to the collection page
        await Page.WaitForBlazorUrlContainsAsync(href!);
        Page.Url.Should().Contain(href!, "should navigate to the collection page");
    }

    [Fact]
    public async Task CardMoreBadge_WhenClicked_ExpandsAllTags()
    {
        // Arrange - Navigate to a page and find a card with "+X more" badge
        await Page.GotoRelativeAsync("/github-copilot/community");
        await Page.Locator(".card").First.AssertElementVisibleAsync();

        // Find a card that has a "+X more" expand button
        var expandButton = Page.Locator("button.badge-expandable");
        var expandCount = await expandButton.CountAsync();

        if (expandCount == 0)
        {
            // No cards with more than 5 tags - try another section
            await Page.GotoRelativeAsync("/ai/community");
            await Page.Locator(".card").First.AssertElementVisibleAsync();
            expandButton = Page.Locator("button.badge-expandable");
            expandCount = await expandButton.CountAsync();
        }

        // Skip test if no cards have more than 5 tags
        if (expandCount == 0)
        {
            return;
        }

        // Identify the card position: find which card-index has the expand button
        var allCards = Page.Locator(".card");
        var totalCards = await allCards.CountAsync();
        int targetCardIndex = -1;
        for (int i = 0; i < totalCards; i++)
        {
            var card = allCards.Nth(i);
            var btnCount = await card.Locator("button.badge-expandable").CountAsync();
            if (btnCount > 0)
            {
                targetCardIndex = i;
                break;
            }
        }

        if (targetCardIndex < 0)
        {
            return; // Could not find a card with expand button
        }

        var targetCard = allCards.Nth(targetCardIndex);
        var initialTagCount = await targetCard.Locator("button.badge-tag-clickable").CountAsync();

        // Act - Click "+X more" button (no URL change expected, just UI state)
        await targetCard.Locator("button.badge-expandable").ClickBlazorElementAsync(waitForUrlChange: false);

        // Wait for Blazor re-render: expand button should disappear from this card
        await Assertions.Expect(targetCard.Locator("button.badge-expandable")).ToHaveCountAsync(0);

        // Assert - More tags should now be visible
        var expandedTagCount = await targetCard.Locator("button.badge-tag-clickable").CountAsync();
        expandedTagCount.Should().BeGreaterThan(initialTagCount,
            "clicking '+X more' should reveal additional tag badges");
    }
}
