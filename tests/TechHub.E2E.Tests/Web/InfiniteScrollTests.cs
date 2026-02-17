using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for infinite scroll pagination.
/// Tests validate continuous content loading as users scroll.
/// </summary>
public class InfiniteScrollTests : PlaywrightTestBase
{
    public InfiniteScrollTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task ContentGrid_InitialLoad_Shows20Items()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync("/all");

        // Wait for content to load - using .card selector (actual CSS class)
        await Page.WaitForConditionAsync(
            "() => document.querySelectorAll('.card').length > 0");

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
        await Page.WaitForConditionAsync(
            "() => document.querySelectorAll('.card').length >= 20");

        var initialCount = await Page.Locator(".card").CountAsync();
        initialCount.Should().Be(20);

        // Act - Scroll to the bottom to trigger next batch load
        await Page.ScrollToLoadMoreAsync(expectedItemCount: 40);

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
        await Page.WaitForConditionAsync(
            "() => document.querySelectorAll('.card').length > 0");

        // Act - Keep scrolling until we see the end message or all content is loaded
        await Page.ScrollToEndOfContentAsync();

        // Assert - End message should be visible
        var endMessage = Page.Locator(".end-of-content");
        await Assertions.Expect(endMessage).ToBeVisibleAsync();

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
        await Page.WaitForConditionAsync(
            "() => document.querySelectorAll('.card').length >= 20");

        // Get first item's title to verify it stays
        var firstItemTitle = await Page.Locator(".card").First.Locator("h3").TextContentAsync();

        // Act - Load second batch by scrolling to trigger
        await Page.ScrollToLoadMoreAsync(expectedItemCount: 40);

        // Assert - First item should still be there with same title
        var firstItemAfterLoad = await Page.Locator(".card").First.Locator("h3").TextContentAsync();
        firstItemAfterLoad.Should().Be(firstItemTitle, "original items should not be replaced");
    }
}
