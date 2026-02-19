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

        // Act - Type in search box
        await searchInput.FillAsync("copilot");

        // Wait for debounce + URL update
        await Page.WaitForUrlConditionAsync(
            "() => window.location.href.includes('search=')");

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

        // Act - Type in search box
        await searchInput.FillAsync("test query");

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

        // Act - Click clear button
        var clearButton = Page.Locator("button[aria-label*='Clear']").Or(Page.Locator(".search-clear-button"));
        await clearButton.ClickAsync();

        // Wait for URL to update (search parameter removed via Blazor pushState)
        await Page.WaitForUrlConditionAsync(
            "() => !window.location.href.includes('search=')");

        // Assert - Search should be cleared
        var inputValue = await searchInput.InputValueAsync();
        inputValue.Should().BeEmpty("Search input should be empty after clicking clear");

        var currentUrl = Page.Url;
        currentUrl.Should().NotContain("search=", "URL should not contain search parameter after clearing");
    }

    [Fact]
    public async Task Search_CombinedWithTagFilter_ShowsIntersectionResults()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot");

        // Act 1 - Select a tag
        var tagButton = Page.Locator(".tag-cloud-item").First;
        await tagButton.ClickBlazorElementAsync();

        // Wait for tags parameter to appear in URL after tag click
        await Page.WaitForUrlConditionAsync(
            "() => window.location.href.includes('tags=')");

        // Act 2 - Add search query
        var searchInput = Page.Locator("input[type='search'], input[placeholder*='Search']");
        await searchInput.FillAsync("copilot");

        // Wait for debounce + URL update (replaces unreliable Task.Delay)
        await Page.WaitForUrlConditionAsync(
            "() => window.location.href.includes('search=')");

        // Assert - URL should contain both search and tags parameters
        var currentUrl = Page.Url;
        currentUrl.Should().Contain("search=", "URL should contain search parameter");
        currentUrl.Should().Contain("tags=", "URL should contain tags parameter");

        // Both filters should be active
        var uri = new Uri(currentUrl);
        var queryParams = System.Web.HttpUtility.ParseQueryString(uri.Query);
        queryParams.Get("search").Should().Be("copilot");
        queryParams.Get("tags").Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Search_WhenCleared_KeepsOtherFilters()
    {
        // Arrange - Navigate with both search and tags parameters
        await Page.GotoRelativeAsync("/github-copilot?search=test&tags=vs%20code");

        // Wait for search input to be populated (ensures Blazor has processed URL params)
        var searchInput = Page.Locator("input[type='search'], input[placeholder*='Search']");
        await Assertions.Expect(searchInput).ToHaveValueAsync("test");

        // Act - Clear only the search
        var clearButton = Page.Locator("button[aria-label*='Clear']").Or(Page.Locator(".search-clear-button"));
        await clearButton.ClickAsync();

        // Wait for URL to update (search parameter removed via Blazor)
        await Page.WaitForUrlConditionAsync(
            "() => !window.location.href.includes('search=')");

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

        // Act - Focus search box and press Escape
        await searchInput.FocusAsync();
        await searchInput.PressAsync("Escape");

        // Wait for URL to update (search parameter removed via Blazor)
        await Page.WaitForUrlConditionAsync(
            "() => !window.location.href.includes('search=')");

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
        await searchInput.FillAsync("xyzabc123nonexistent");

        // Wait for debounce + URL update before checking results
        await Page.WaitForUrlConditionAsync(
            "() => window.location.href.includes('search=')");

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
        await searchInput.FillAsync("copilot");

        // Wait for debounce + URL update
        await Page.WaitForUrlConditionAsync(
            "() => window.location.href.includes('search=')");

        // Assert - URL should contain search parameter
        var currentUrl = Page.Url;
        currentUrl.Should().Contain("search=copilot");
        currentUrl.Should().Contain("/github-copilot/blogs", "Should remain on collection page");
    }
}
