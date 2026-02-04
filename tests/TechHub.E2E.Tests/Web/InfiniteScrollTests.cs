using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for infinite scroll pagination.
/// Tests validate continuous content loading as users scroll.
/// </summary>
[Collection("Infinite Scroll Tests")]
public class InfiniteScrollTests : IAsyncLifetime
{
    private readonly PlaywrightCollectionFixture _fixture;
    private IBrowserContext? _context;
    private IPage? _page;

    public InfiniteScrollTests(PlaywrightCollectionFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        _fixture = fixture;
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
    }

    [Fact]
    public async Task ContentGrid_InitialLoad_Shows20Items()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync("/all");

        // Wait for content to load - using .card selector (actual CSS class)
        await Page.WaitForFunctionAsync(
            "() => document.querySelectorAll('.card').length > 0",
            new PageWaitForFunctionOptions { Timeout = 10000, PollingInterval = 100 });

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
            new PageWaitForFunctionOptions { Timeout = 10000, PollingInterval = 100 });

        var initialCount = await Page.Locator(".card").CountAsync();
        initialCount.Should().Be(20);

        // Act - Scroll to the bottom to trigger next batch load
        var scrollTrigger = Page.Locator("#scroll-trigger");
        await scrollTrigger.ScrollIntoViewIfNeededAsync();

        // Wait for new items to load (should have 40 items now)
        await Page.WaitForFunctionAsync(
            "() => document.querySelectorAll('.card').length >= 40",
            new PageWaitForFunctionOptions { Timeout = 10000, PollingInterval = 100 });

        // Assert
        var newCount = await Page.Locator(".card").CountAsync();
        newCount.Should().BeGreaterThanOrEqualTo(40, "second batch should load, showing at least 40 items");
    }

    [Fact]
    public async Task ContentGrid_ScrollToEnd_ShowsEndMessage()
    {
        // Arrange - Use a smaller collection that will reach end faster
        // Roundups collection exists only in the 'all' section
        await Page.GotoRelativeAsync("/all/roundups");

        // Wait for initial content
        await Page.WaitForFunctionAsync(
            "() => document.querySelectorAll('.card').length > 0",
            new PageWaitForFunctionOptions { Timeout = 10000, PollingInterval = 100 });

        // Act - Keep scrolling until we see the end message or scroll trigger disappears
        var endMessage = Page.Locator(".end-of-content");
        var maxScrollAttempts = 20;

        for (var i = 0; i < maxScrollAttempts; i++)
        {
            if (await endMessage.IsVisibleAsync())
            {
                break;
            }

            var scrollTrigger = Page.Locator("#scroll-trigger");
            if (await scrollTrigger.CountAsync() == 0)
            {
                break;
            }

            await scrollTrigger.ScrollIntoViewIfNeededAsync();
            await Page.WaitForTimeoutAsync(500);
        }

        // Assert - End message should be visible
        await Assertions.Expect(endMessage).ToBeVisibleAsync(
            new LocatorAssertionsToBeVisibleOptions { Timeout = 5000 });

        var endText = await endMessage.TextContentAsync();
        endText.Should().Contain("End of content");

        // Scroll trigger should not exist anymore
        var scrollTriggerCount = await Page.Locator("#scroll-trigger").CountAsync();
        scrollTriggerCount.Should().Be(0, "scroll trigger should be removed when content ends");
    }

    [Fact]
    public async Task ContentGrid_ItemsAppend_DoNotReplace()
    {
        // Arrange
        await Page.GotoRelativeAsync("/all");

        // Wait for initial content
        await Page.WaitForFunctionAsync(
            "() => document.querySelectorAll('.card').length >= 20",
            new PageWaitForFunctionOptions { Timeout = 10000, PollingInterval = 100 });

        // Get first item's title to verify it stays
        var firstItemTitle = await Page.Locator(".card").First.Locator("h3").TextContentAsync();

        // Act - Load second batch
        var scrollTrigger = Page.Locator("#scroll-trigger");
        await scrollTrigger.ScrollIntoViewIfNeededAsync();

        await Page.WaitForFunctionAsync(
            "() => document.querySelectorAll('.card').length >= 40",
            new PageWaitForFunctionOptions { Timeout = 10000, PollingInterval = 100 });

        // Assert - First item should still be there with same title
        var firstItemAfterLoad = await Page.Locator(".card").First.Locator("h3").TextContentAsync();
        firstItemAfterLoad.Should().Be(firstItemTitle, "original items should not be replaced");
    }
}
