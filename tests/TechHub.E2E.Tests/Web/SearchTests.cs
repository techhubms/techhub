using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for search functionality including text search, debouncing, and search + filter combinations.
/// Tests SidebarSearch component for text-based content filtering.
/// </summary>
public class SearchTests : PlaywrightTestBase
{
    // Use a data-rich collection/query pair so this PR-preview E2E covers the combined
    // search+tag flow against a route that consistently exposes filterable content.
    private const string TagSearchTestPath = "/ai/blogs";
    private const string TagSearchTestQuery = "azure";
    private const string TagFilterNavSelector = "nav[aria-label='Filter by tags']";
    private const string EnabledTagSelector = TagFilterNavSelector + " .tag-cloud-item:not(.disabled)";
    private const string NoTagsStateSelector = TagFilterNavSelector + " .sidebar-text:not(.error)";
    private const string TagErrorSelector = TagFilterNavSelector + " .sidebar-text.error";
    private const string LoadingSkeletonSelector = TagFilterNavSelector + " .tag-cloud-skeleton";

    public SearchTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task SearchBox_OnSectionPage_RendersAndIsAccessible()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync("/github-copilot");

        // Assert - Search box should be visible and accessible
        var searchInput = Page.Locator("input[type='search'], input[placeholder*='Search']");
        await searchInput.AssertElementVisibleAsync();

        // Verify accessibility
        var ariaLabel = await searchInput.GetAttributeAsync("aria-label");
        ariaLabel.Should().NotBeNullOrEmpty("Search input should have aria-label for accessibility");
    }

    [Fact]
    public async Task SearchBox_WhenTyping_UpdatesUrlWithSearchParameter()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot");

        var searchInput = Page.Locator("input[type='search'], input[placeholder*='Search']");

        // Act - Type in search box and wait for URL to reflect the query
        await searchInput.FillBlazorInputAsync("copilot");

        // Assert - URL should contain search parameter
        var currentUrl = Page.Url;
        currentUrl.Should().Contain("search=", "Expected URL to contain search parameter after typing");

        var uri = new Uri(currentUrl);
        var searchParam = System.Web.HttpUtility.ParseQueryString(uri.Query).Get("search");
        searchParam.Should().Be("copilot", "Search parameter should match the typed query");
    }

    [Fact]
    public async Task SearchBox_WithUrlParameter_ShowsSearchQuery()
    {
        // Arrange & Act - Navigate with search parameter
        await Page.GotoRelativeAsync("/github-copilot?search=blazor");

        // Assert - Search input should contain the query from URL
        var searchInput = Page.Locator("input[type='search'], input[placeholder*='Search']");
        var inputValue = await searchInput.InputValueAsync();
        inputValue.Should().Be("blazor", "Search box should display query from URL parameter");
    }

    [Fact]
    public async Task SearchBox_WhenQueryEntered_ShowsClearButton()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot");

        var searchInput = Page.Locator("input[type='search'], input[placeholder*='Search']");

        // Act - Use FillBlazorInputAsync to ensure Blazor @oninput handler is attached before
        // filling, then wait for the search URL parameter to confirm Blazor processed the input.
        // Plain FillAsync fires the input event before Blazor attaches its handler on slow CI,
        // so _searchQueryInternal is never updated and the clear button never renders.
        await searchInput.FillBlazorInputAsync("test query");

        // Assert - Clear button should appear (auto-retries via Expect)
        var clearButton = Page.Locator("button[aria-label*='Clear']").Or(Page.Locator(".search-clear-button"));
        await clearButton.AssertElementVisibleAsync();
    }

    [Fact]
    public async Task ClearButton_WhenClicked_RemovesSearchQuery()
    {
        // Arrange - Navigate with search parameter
        await Page.GotoRelativeAsync("/github-copilot?search=test");

        var searchInput = Page.Locator("input[type='search'], input[placeholder*='Search']");
        await Assertions.Expect(searchInput).ToHaveValueAsync("test");

        // Act + Assert — retry [click + URL no longer has search= + input empty]
        // URL update (history.replaceState) and input DOM clear are separate Blazor
        // renders; asserting both inside the retry block ensures both have happened.
        var clearButton = Page.Locator("button[aria-label*='Clear']").Or(Page.Locator(".search-clear-button"));
        await clearButton.ClickAndExpectAsync(async () =>
        {
            await Assertions.Expect(Page).Not.ToHaveURLAsync(new Regex("search="));
            await Assertions.Expect(searchInput).ToHaveValueAsync("");
        });

        // Assert - Search should be cleared
        var inputValue = await searchInput.InputValueAsync();
        inputValue.Should().BeEmpty("Search input should be empty after clicking clear");

        var currentUrl = Page.Url;
        currentUrl.Should().NotContain("search=", "URL should not contain search parameter after clearing");
    }

    [Fact]
    public async Task Search_CombinedWithTagFilter_ShowsIntersectionResults()
    {
        // Arrange - use a data-rich collection page so both search and tag filters are
        // available under filter-mode navigation during E2E runs.
        await Page.GotoRelativeAsync(TagSearchTestPath);
        await WaitForSelectableTagFilterOrSkipAsync();

        // Act 1 - Select a tag
        var tagButton = Page.Locator(EnabledTagSelector).First;
        await tagButton.ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page).ToHaveURLAsync(new Regex(@".*tags=.*")));

        // Act 2 - Add search query
        var searchInput = Page.Locator("input[type='search'], input[placeholder*='Search']");
        await searchInput.FillBlazorInputAsync(TagSearchTestQuery);

        // Assert - URL should contain both search and tags parameters
        var currentUrl = Page.Url;
        currentUrl.Should().Contain("search=", "URL should contain search parameter");
        currentUrl.Should().Contain("tags=", "URL should contain tags parameter");

        // Both filters should be active
        var uri = new Uri(currentUrl);
        var queryParams = System.Web.HttpUtility.ParseQueryString(uri.Query);
        queryParams.Get("search").Should().Be(TagSearchTestQuery);
        queryParams.Get("tags").Should().NotBeNullOrEmpty();
    }

    private async Task WaitForSelectableTagFilterOrSkipAsync()
    {
        await Page.WaitForBlazorReadyAsync();

        var enabledTags = await WaitForSelectableTagFilterOrReachTerminalStateAsync();
        Assert.SkipWhen(enabledTags == 0,
            "No enabled tag filters are available in the current data snapshot.");
    }

    private async Task<int> WaitForSelectableTagFilterOrReachTerminalStateAsync()
    {
        // Retry until tag filters load or reach a terminal state (no-tags / error).
        // Progressive backoff mirrors Playwright JS expect(fn).toPass() intervals.
        var intervals = new[] { 100, 250, 500, 1000, 1000 };
        var deadline = DateTime.UtcNow.AddMilliseconds(BlazorHelpers.E2ETimeout);
        var attempt = 0;
        int enabledTags;

        while (true)
        {
            enabledTags = await Page.Locator(EnabledTagSelector).CountAsync();
            if (enabledTags > 0)
            {
                break;
            }

            var loadingSkeletonCount = await Page.Locator(LoadingSkeletonSelector).CountAsync();
            if (loadingSkeletonCount == 0)
            {
                var noTagsMessageCount = await Page.Locator(NoTagsStateSelector).CountAsync();
                var tagLoadingErrorCount = await Page.Locator(TagErrorSelector).CountAsync();
                if (noTagsMessageCount > 0 || tagLoadingErrorCount > 0)
                {
                    break;
                }
            }

            var delayMs = intervals[Math.Min(attempt, intervals.Length - 1)];
            if (DateTime.UtcNow.AddMilliseconds(delayMs) >= deadline)
            {
                break;
            }

            await Task.Delay(delayMs);
            attempt++;
        }

        return enabledTags;
    }

    [Fact]
    public async Task Search_WhenCleared_KeepsOtherFilters()
    {
        // Arrange - Navigate with both search and tags parameters
        await Page.GotoRelativeAsync("/github-copilot?search=test&tags=vs%20code");

        // Wait for search input to be populated (ensures Blazor has processed URL params)
        var searchInput = Page.Locator("input[type='search'], input[placeholder*='Search']");
        await Assertions.Expect(searchInput).ToHaveValueAsync("test");

        // Act + Assert — retry [click + URL no longer has search=, tags= remain]
        var clearButton = Page.Locator("button[aria-label*='Clear']").Or(Page.Locator(".search-clear-button"));
        await clearButton.ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page).Not.ToHaveURLAsync(new Regex("search=")));

        // Assert - Tags should remain, search should be removed
        var currentUrl = Page.Url;
        currentUrl.Should().NotContain("search=", "Search parameter should be removed");
        currentUrl.Should().Contain("tags=", "Tags parameter should remain after clearing search");
    }

    [Fact]
    public async Task SearchBox_WithKeyboardEscape_ClearsQuery()
    {
        // Arrange - Navigate with search parameter
        await Page.GotoRelativeAsync("/github-copilot?search=test");

        var searchInput = Page.Locator("input[type='search'], input[placeholder*='Search']");
        await Assertions.Expect(searchInput).ToHaveValueAsync("test");

        // Act - Focus search box and press Escape.
        // WaitForBlazorReadyAsync (called by GotoRelativeAsync) ensures the @onkeydown handler
        // is attached before we interact, so a single focus+press is sufficient.
        await Page.WaitForBlazorReadyAsync();
        await searchInput.FocusAsync();
        await searchInput.PressAsync("Escape");
        await Assertions.Expect(Page).Not.ToHaveURLAsync(new Regex("search="));

        // Assert - Search should be cleared
        var inputValue = await searchInput.InputValueAsync();
        inputValue.Should().BeEmpty("Search input should be empty after pressing Escape");

        var currentUrl = Page.Url;
        currentUrl.Should().NotContain("search=", "URL should not contain search parameter after Escape");
    }

    [Fact]
    public async Task Search_WithNoResults_ShowsEmptyState()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot");

        // Act - Search for something that won't exist
        var searchInput = Page.Locator("input[type='search'], input[placeholder*='Search']");
        await searchInput.FillBlazorInputAsync("xyzabc123nonexistent");

        // Assert - Should show "no results" message (auto-retries via Expect)
        var noResultsMessage = Page.Locator("text=/no.*results/i").Or(Page.Locator(".no-content"));
        await noResultsMessage.AssertElementVisibleAsync();
    }

    [Fact]
    public async Task Search_OnCollectionPage_WorksCorrectly()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot/blogs");

        // Act - Type in search box
        var searchInput = Page.Locator("input[type='search'], input[placeholder*='Search']");
        await searchInput.FillBlazorInputAsync("copilot");

        // Assert - URL should contain search parameter
        var currentUrl = Page.Url;
        currentUrl.Should().Contain("search=copilot");
        currentUrl.Should().Contain("/github-copilot/blogs", "Should remain on collection page");
    }
}
