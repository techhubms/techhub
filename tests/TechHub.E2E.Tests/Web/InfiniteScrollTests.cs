using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for infinite scroll pagination
/// Tests for user story requirements: continuous browsing, back button preservation, filter integration
/// </summary>
[Collection("Infinite Scroll Tests")]
public class InfiniteScrollTests : IAsyncLifetime
{
    private readonly PlaywrightCollectionFixture _fixture;

    public InfiniteScrollTests(PlaywrightCollectionFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        _fixture = fixture;
    }

    private IBrowserContext? _context;
    private IPage? _page;
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
    }

    [Fact]
    public async Task ContentGrid_InitialLoad_Shows20Items()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync("/all");

        // Wait for content to load
        await Page.WaitForFunctionAsync(
            "() => document.querySelectorAll('.card').length > 0",
            new PageWaitForFunctionOptions { Timeout = 5000, PollingInterval = 100 });

        // Assert - Initial batch should load 20 items
        var itemCount = await Page.Locator(".card").CountAsync();
        itemCount.Should().Be(20, "initial load should show exactly 20 items");
    }

    [Fact]
    public async Task ContentGrid_ScrollToBottom_LoadsNextBatch()
    {
        // Arrange
        await Page.GotoRelativeAsync("/all");

        // Wait for initial content
        await Page.WaitForFunctionAsync(
            "() => document.querySelectorAll('.card').length >= 20",
            new PageWaitForFunctionOptions { Timeout = 5000, PollingInterval = 100 });

        var initialCount = await Page.Locator(".card").CountAsync();
        initialCount.Should().Be(20);

        // Act - Scroll to the bottom to trigger next batch load
        // Look for the scroll trigger element (should be near bottom)
        var scrollTrigger = Page.Locator("#scroll-trigger");
        await scrollTrigger.ScrollIntoViewIfNeededAsync();

        // Wait for loading indicator to appear
        var loadingIndicator = Page.Locator(".loading-more-indicator");
        await Assertions.Expect(loadingIndicator).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 2000 });

        // Wait for new items to load (should have 40 items now)
        await Page.WaitForFunctionAsync(
            "() => document.querySelectorAll('.card').length >= 40",
            new PageWaitForFunctionOptions { Timeout = 5000, PollingInterval = 100 });

        // Assert
        var newCount = await Page.Locator(".card").CountAsync();
        newCount.Should().BeGreaterThanOrEqualTo(40, "second batch should load, showing at least 40 items");

        // Loading indicator should disappear after items load
        await Assertions.Expect(loadingIndicator).Not.ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 2000 });
    }

    [Fact]
    public async Task ContentGrid_ScrollToEnd_ShowsEndMessage()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot/news");

        // Wait for initial content
        await Page.WaitForFunctionAsync(
            "() => document.querySelectorAll('.card').length > 0",
            new PageWaitForFunctionOptions { Timeout = 5000, PollingInterval = 100 });

        // Act - Scroll multiple times to reach the end
        // Keep scrolling until we see the end message
        var endMessage = Page.Locator(".end-of-content");
        var maxScrollAttempts = 20; // Prevent infinite loop
        var attempts = 0;

        while (attempts < maxScrollAttempts)
        {
            var scrollTrigger = Page.Locator("#scroll-trigger");
            
            // Check if end message is already visible
            var isEndVisible = await endMessage.IsVisibleAsync();
            if (isEndVisible)
            {
                break;
            }

            // Try to scroll to trigger
            var triggerExists = await scrollTrigger.CountAsync() > 0;
            if (!triggerExists)
            {
                // No more trigger means we reached the end
                break;
            }

            await scrollTrigger.ScrollIntoViewIfNeededAsync();
            
            // Wait a bit for content to load
            await Page.WaitForFunctionAsync(
                "() => true", // Small delay to allow batch loading
                new PageWaitForFunctionOptions { Timeout = 1000, PollingInterval = 100 });

            attempts++;
        }

        // Assert - End message should be visible
        await Assertions.Expect(endMessage).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 2000 });
        
        var endText = await endMessage.TextContentAsync();
        endText.Should().Contain("End of content", "end message should indicate no more items");

        // Scroll trigger should not exist anymore
        var scrollTriggerCount = await Page.Locator("#scroll-trigger").CountAsync();
        scrollTriggerCount.Should().Be(0, "scroll trigger should be removed when content ends");
    }

    [Fact]
    public async Task ContentGrid_BackButton_PreservesScrollPosition()
    {
        // Arrange
        await Page.GotoRelativeAsync("/all");

        // Wait for content to load
        await Page.WaitForFunctionAsync(
            "() => document.querySelectorAll('.card').length >= 20",
            new PageWaitForFunctionOptions { Timeout = 5000, PollingInterval = 100 });

        // Scroll and load second batch
        var scrollTrigger = Page.Locator("#scroll-trigger");
        await scrollTrigger.ScrollIntoViewIfNeededAsync();

        // Wait for 40 items
        await Page.WaitForFunctionAsync(
            "() => document.querySelectorAll('.card').length >= 40",
            new PageWaitForFunctionOptions { Timeout = 5000, PollingInterval = 100 });

        // Remember scroll position
        var scrollYBeforeClick = await Page.EvaluateAsync<int>("() => window.scrollY");

        // Act - Click on a content item
        var firstItem = Page.Locator(".card").First;
        await firstItem.ClickBlazorElementAsync();

        // Wait for navigation
        await Page.WaitForBlazorUrlContainsAsync("/github-copilot");

        // Go back
        await Page.GoBackAsync();

        // Wait for /all page to restore
        await Page.WaitForBlazorUrlContainsAsync("/all");

        // Wait for content to re-render
        await Page.WaitForFunctionAsync(
            "() => document.querySelectorAll('.card').length >= 40",
            new PageWaitForFunctionOptions { Timeout = 5000, PollingInterval = 100 });

        // Assert - Scroll position should be approximately the same
        var scrollYAfterBack = await Page.EvaluateAsync<int>("() => window.scrollY");
        
        // Allow for some variance (within 100px) due to rendering differences
        Math.Abs(scrollYAfterBack - scrollYBeforeClick).Should().BeLessThan(100,
            "scroll position should be preserved when using back button");
    }

    [Fact]
    public async Task ContentGrid_InfiniteScroll_WorksWithFilters()
    {
        // Arrange
        await Page.GotoRelativeAsync("/all?tags=ai");

        // Wait for filtered content to load
        await Page.WaitForFunctionAsync(
            "() => document.querySelectorAll('.card').length > 0",
            new PageWaitForFunctionOptions { Timeout = 5000, PollingInterval = 100 });

        var initialCount = await Page.Locator(".card").CountAsync();

        // If there's enough filtered content for a second batch
        if (initialCount >= 20)
        {
            // Act - Scroll to load more filtered content
            var scrollTrigger = Page.Locator("#scroll-trigger");
            await scrollTrigger.ScrollIntoViewIfNeededAsync();

            // Wait for loading
            await Page.WaitForFunctionAsync(
                $"() => document.querySelectorAll('.card').length > {initialCount}",
                new PageWaitForFunctionOptions { Timeout = 5000, PollingInterval = 100 });

            // Assert
            var newCount = await Page.Locator(".card").CountAsync();
            newCount.Should().BeGreaterThan(initialCount, "more filtered items should load");
        }
        else
        {
            // Assert - With few results, should see end message
            var endMessage = Page.Locator(".end-of-content");
            await Assertions.Expect(endMessage).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 2000 });
        }
    }

    [Fact]
    public async Task ContentGrid_LoadingIndicator_ShowsWhileLoading()
    {
        // Arrange
        await Page.GotoRelativeAsync("/all");

        // Wait for initial content
        await Page.WaitForFunctionAsync(
            "() => document.querySelectorAll('.card').length >= 20",
            new PageWaitForFunctionOptions { Timeout = 5000, PollingInterval = 100 });

        // Act - Scroll to trigger
        var scrollTrigger = Page.Locator("#scroll-trigger");
        await scrollTrigger.ScrollIntoViewIfNeededAsync();

        // Assert - Loading indicator should appear immediately
        var loadingIndicator = Page.Locator(".loading-more-indicator");
        await Assertions.Expect(loadingIndicator).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 1000 });

        // And should contain appropriate text
        var loadingText = await loadingIndicator.TextContentAsync();
        loadingText.Should().Contain("Loading", "loading indicator should show loading message");

        // Wait for content to finish loading
        await Page.WaitForFunctionAsync(
            "() => document.querySelectorAll('.card').length >= 40",
            new PageWaitForFunctionOptions { Timeout = 5000, PollingInterval = 100 });

        // Loading indicator should disappear
        await Assertions.Expect(loadingIndicator).Not.ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 2000 });
    }

    [Fact]
    public async Task ContentGrid_ItemsAppend_DoNotReplace()
    {
        // Arrange
        await Page.GotoRelativeAsync("/all");

        // Wait for initial content
        await Page.WaitForFunctionAsync(
            "() => document.querySelectorAll('.card').length >= 20",
            new PageWaitForFunctionOptions { Timeout = 5000, PollingInterval = 100 });

        // Get first item's title to verify it stays
        var firstItemTitle = await Page.Locator(".card").First.Locator("h2").TextContentAsync();

        // Act - Load second batch
        var scrollTrigger = Page.Locator("#scroll-trigger");
        await scrollTrigger.ScrollIntoViewIfNeededAsync();

        await Page.WaitForFunctionAsync(
            "() => document.querySelectorAll('.card').length >= 40",
            new PageWaitForFunctionOptions { Timeout = 5000, PollingInterval = 100 });

        // Assert - First item should still be there with same title
        var firstItemAfterLoad = await Page.Locator(".card").First.Locator("h2").TextContentAsync();
        firstItemAfterLoad.Should().Be(firstItemTitle, "original items should not be replaced");
    }
}
