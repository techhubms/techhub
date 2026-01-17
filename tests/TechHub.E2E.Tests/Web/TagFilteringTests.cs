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

        // Use helper that clicks and waits for URL change (handles SignalR URL updates)
        await tagButton.ClickBlazorElementAsync();

        var urlAfterFirstClick = Page.Url;
        urlAfterFirstClick.Should().Contain("tags=", "First click should add tag to URL");

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
        await Page.GotoRelativeAsync("/github-copilot?tags=github%20copilot");
        await WaitForTagCloudReadyAsync();

        // Assert - Tag button should have selected/active state
        var selectedTagButton = Page.Locator(".tag-cloud-item.selected")
            .Filter(new() { HasTextRegex = new Regex("github copilot", RegexOptions.IgnoreCase) });

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
        await Page.GotoRelativeAsync("/github-copilot?tags=github%20copilot,ai");
        await WaitForTagCloudReadyAsync();

        // Assert - Both tag buttons should have selected/active state
        var githubCopilotTag = Page.Locator(".tag-cloud-item.selected")
            .Filter(new() { HasTextRegex = new Regex("github copilot", RegexOptions.IgnoreCase) });

        var aiTag = Page.Locator(".tag-cloud-item.selected")
            .Filter(new() { HasTextRegex = new Regex("^ai$", RegexOptions.IgnoreCase) });

        await Assertions.Expect(githubCopilotTag).ToBeVisibleAsync(new() { Timeout = 5000 });
        await Assertions.Expect(aiTag).ToBeVisibleAsync(new() { Timeout = 5000 });
    }

    [Fact]
    public async Task TagUrl_WithDuplicateTags_ShouldDeduplicateAutomatically()
    {
        // Arrange - Navigate to URL with duplicate tags (simulating the bug)
        // Use /github-copilot instead of /all as it has guaranteed tags
        await Page.GotoRelativeAsync("/github-copilot?tags=github%20copilot,ai,github%20copilot,devops,ai");
        await WaitForTagCloudReadyAsync();

        // Wait for page to load and process tags
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert - URL should be cleaned up without duplicates
        var currentUrl = Page.Url;
        var uri = new Uri(currentUrl);
        var tagsParam = System.Web.HttpUtility.ParseQueryString(uri.Query).Get("tags");

        if (!string.IsNullOrEmpty(tagsParam))
        {
            var tags = tagsParam.Split(',').Select(t => t.Trim()).ToList();
            var distinctTags = tags.Distinct().ToList();

            tags.Should().BeEquivalentTo(distinctTags,
                "Tags in URL should be deduplicated, no tag should appear twice");
        }
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
    public async Task TagButton_OnAnyPage_ShouldToggleCorrectly()
    {
        // Arrange - Navigate to a section page (testing filter mode on section pages)
        // Note: Originally tested /all but renamed to be more accurate about what's tested
        await Page.GotoRelativeAsync("/ai");
        await WaitForTagCloudReadyAsync();

        // Act - Click a tag button
        var tagButton = Page.Locator(".tag-cloud-item").First;
        var tagText = await tagButton.TextContentAsync();
        await tagButton.ClickBlazorElementAsync();

        // Assert 1 - URL should contain tag
        var urlAfterClick = Page.Url;
        urlAfterClick.Should().Contain("tags=", "Tag should be added to section page URL");

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
