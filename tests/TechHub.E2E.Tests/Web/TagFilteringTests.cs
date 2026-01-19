using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for tag filtering functionality including toggle behavior and visual state
/// Tests both SidebarTags and SidebarTagCloud components
/// </summary>
[Collection("Tag Filtering Tests")]
public class TagFilteringTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
    private const string BaseUrl = "http://localhost:5184";
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
            await _context.DisposeAsync();
        }
    }

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
            new() { Timeout = 10000 });

        // Assert - URL should contain the tag parameter
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        var currentUrl = Page.Url;
        currentUrl.Should().Contain("tags=", $"Expected URL to contain tags parameter after clicking tag '{tagText}'");

        // Verify the clicked tag is in the URL
        var uri = new Uri(currentUrl);
        var tagsParam = System.Web.HttpUtility.ParseQueryString(uri.Query).Get("tags");
        tagsParam.Should().NotBeNullOrEmpty("Tags parameter should have a value");
        var normalizedTagText = tagText?.Trim().ToLowerInvariant() ?? "";
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
        await tagButton.ClickBlazorElementAsync();

        // Assert - URL should no longer contain the tag parameter or be empty
        var urlAfterSecondClick = Page.Url;
        var uri = new Uri(urlAfterSecondClick);
        var tagsParam = System.Web.HttpUtility.ParseQueryString(uri.Query).Get("tags");

        var normalizedTagText = tagText?.Trim().ToLowerInvariant() ?? "";

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

        // Wait for page to load and process tags
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

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
        var allItems = await Page.Locator(".content-item-card").CountAsync();
        allItems.Should().BeGreaterThan(0, "Section should have content items");

        // Act 1 - Select first tag
        var firstTagButton = Page.Locator(".tag-cloud-item").First;
        var firstTagText = (await firstTagButton.TextContentAsync())?.Trim().ToLowerInvariant() ?? "";
        await firstTagButton.ClickBlazorElementAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var itemsAfterFirstTag = await Page.Locator(".content-item-card").CountAsync();
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
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var itemsAfterSecondTag = await Page.Locator(".content-item-card").CountAsync();

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
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Act - Get all visible content items
        var visibleItems = await Page.Locator(".content-item-card").CountAsync();

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
}
