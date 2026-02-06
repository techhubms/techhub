using FluentAssertions;
using Microsoft.Playwright;
using System.Net.Http.Json;
using TechHub.Core.Models;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for infinite scroll combined with tag filtering.
/// Validates the bug fix where infinite scrolling + tag filtering was broken.
/// </summary>
[Collection("Infinite Scroll Tests")]
public class InfiniteScrollWithTagsTests : IAsyncLifetime
{
    private readonly PlaywrightCollectionFixture _fixture;
    private IBrowserContext? _context;
    private IPage? _page;
    private readonly HttpClient _httpClient;

    public InfiniteScrollWithTagsTests(PlaywrightCollectionFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        _fixture = fixture;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "TechHub-E2E-Tests");
    }

    private IPage Page => _page ?? throw new InvalidOperationException("Page not initialized");

    public async Task InitializeAsync()
    {
        _context = await _fixture.CreateContextAsync();
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

        _httpClient.Dispose();
    }

    [Fact]
    public async Task InfiniteScroll_WithTagFilter_LoadsAllItemsAndShowsEndMessage()
    {
        // Arrange - First, query API to get expected count for "Copilot" tag in github-copilot/news
        const string tag = "Copilot";
        const string apiUrl = "https://localhost:5001/api/sections/github-copilot/collections/news/items";
        
        // Query API for total count (use high take value to get all items)
        var apiResponse = await _httpClient.GetAsync($"{apiUrl}?tags={tag}&take=200");
        apiResponse.EnsureSuccessStatusCode();
        var expectedItems = await apiResponse.Content.ReadFromJsonAsync<List<ContentItem>>();
        var expectedCount = expectedItems!.Count;

        // Verify we have enough items for meaningful test (>20 to require scrolling)
        expectedCount.Should().BeGreaterThan(20, "Copilot tag should have enough items to test infinite scroll");

        // Act - Navigate to github-copilot/news page
        await Page.GotoRelativeAsync("/github-copilot/news");

        // Wait for initial content load
        await Page.WaitForFunctionAsync(
            "() => document.querySelectorAll('.card').length > 0",
            new PageWaitForFunctionOptions { Timeout = 10000, PollingInterval = 100 });

        // Click on "Copilot" tag to filter
        var tagButton = Page.Locator($"button.tag-cloud-item:has-text('{tag}')").First;
        await Assertions.Expect(tagButton).ToBeVisibleAsync(
            new LocatorAssertionsToBeVisibleOptions { Timeout = 5000 });
        await tagButton.ClickAsync();

        // Wait for tag filter to apply and URL to update
        await Page.WaitForBlazorUrlContainsAsync("tags=copilot");
        
        // Wait for filtered content to load
        await Page.WaitForTimeoutAsync(1000);

        // Keep scrolling until we see the end message or scroll trigger disappears
        var endMessage = Page.Locator(".end-of-content");
        var maxScrollAttempts = 20;
        var scrollAttempt = 0;

        while (scrollAttempt < maxScrollAttempts)
        {
            // Check if end message is visible
            if (await endMessage.IsVisibleAsync())
            {
                break;
            }

            // Check if scroll trigger still exists
            var scrollTrigger = Page.Locator("#scroll-trigger");
            var triggerCount = await scrollTrigger.CountAsync();
            
            if (triggerCount == 0)
            {
                // No scroll trigger means we've loaded everything
                break;
            }

            // Scroll to trigger next batch
            await scrollTrigger.ScrollIntoViewIfNeededAsync();
            await Page.WaitForTimeoutAsync(500);
            scrollAttempt++;
        }

        // Assert - End message should be visible
        await Assertions.Expect(endMessage).ToBeVisibleAsync(
            new LocatorAssertionsToBeVisibleOptions { Timeout = 5000 });

        var endText = await endMessage.TextContentAsync();
        endText.Should().Contain("End of content", "end message should indicate no more items to load");

        // Scroll trigger should not exist anymore
        var finalTriggerCount = await Page.Locator("#scroll-trigger").CountAsync();
        finalTriggerCount.Should().Be(0, "scroll trigger should be removed when all content is loaded");

        // Count total items displayed on page
        var displayedItemCount = await Page.Locator(".card").CountAsync();
        
        // Assert - Displayed count should match API count
        displayedItemCount.Should().Be(expectedCount,
            $"page should display exactly {expectedCount} items matching the '{tag}' tag filter");

        // Verify all displayed items are news items (external links)
        var cards = Page.Locator(".card");
        var cardCount = await cards.CountAsync();
        
        for (int i = 0; i < cardCount; i++)
        {
            var card = cards.Nth(i);
            var link = card.Locator("a").First;
            var href = await link.GetAttributeAsync("href");
            
            // News items should have external URLs (https://)
            href.Should().StartWith("https://", 
                $"news item {i+1} should link externally, but got href: {href}");
        }
    }

    [Fact]
    public async Task InfiniteScroll_WithTagFilter_MaintainsFilterThroughPagination()
    {
        // Arrange
        const string tag = "Copilot";

        // Act - Navigate and apply tag filter
        await Page.GotoRelativeAsync("/github-copilot/news");
        
        // Wait for initial load
        await Page.WaitForFunctionAsync(
            "() => document.querySelectorAll('.card').length > 0",
            new PageWaitForFunctionOptions { Timeout = 10000 });

        // Apply tag filter
        var tagButton = Page.Locator($"button.tag-cloud-item:has-text('{tag}')").First;
        await tagButton.ClickAsync();
        
        // Wait for filter to apply
        await Page.WaitForBlazorUrlContainsAsync("tags=copilot");
        await Page.WaitForTimeoutAsync(1000);

        // Capture first batch count
        var firstBatchCount = await Page.Locator(".card").CountAsync();
        firstBatchCount.Should().BeGreaterThan(0, "should have items after filtering");

        // Scroll to load second batch
        var scrollTrigger = Page.Locator("#scroll-trigger");
        if (await scrollTrigger.CountAsync() > 0)
        {
            await scrollTrigger.ScrollIntoViewIfNeededAsync();
            await Page.WaitForTimeoutAsync(1000);
        }

        // Capture count after scroll
        var afterScrollCount = await Page.Locator(".card").CountAsync();

        // Assert - More items should be loaded
        if (await scrollTrigger.CountAsync() > 0)
        {
            afterScrollCount.Should().BeGreaterThan(firstBatchCount,
                "scrolling should load additional items when more are available");
        }

        // Verify URL still contains tag filter
        var currentUrl = Page.Url;
        currentUrl.Should().Contain($"tags={tag}", 
            "tag filter should be preserved in URL during infinite scroll");

        // Verify tag button is still in active state
        var isActive = await tagButton.GetAttributeAsync("aria-pressed");
        isActive.Should().Be("true", "tag button should remain active after scrolling");
    }
}
