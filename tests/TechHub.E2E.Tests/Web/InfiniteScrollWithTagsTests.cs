using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for infinite scroll combined with tag filtering.
/// Validates the bug fix where infinite scrolling + tag filtering was broken.
/// </summary>
public class InfiniteScrollWithTagsTests : PlaywrightTestBase
{
    public InfiniteScrollWithTagsTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task InfiniteScroll_WithTagFilter_LoadsCorrectContentType()
    {
        // This test validates that when navigating directly with a tag filter,
        // the correct content (external news links) is displayed.

        const string Tag = "copilot chat";

        // Navigate directly to the page with tag filter applied
        await Page.GotoRelativeAsync($"/github-copilot/news?tags={Tag}");

        // Wait for content to load
        await Page.WaitForConditionAsync(
            "() => document.querySelectorAll('.card').length > 0");

        // Verify we have content
        var cardCount = await Page.Locator(".card").CountAsync();
        cardCount.Should().BeGreaterThan(0, "should have content cards displayed with tag filter");

        // Verify URL still has the tag filter (wasn't cleared)
        // Tags with spaces are URL-encoded as %20
        Page.Url.Should().Contain("tags=copilot", "URL should preserve tag filter");

        // The card is a div container with a .card-link inside it
        // Verify first card links to an external URL (news items are external)
        var firstCard = Page.Locator(".card").First;
        await Assertions.Expect(firstCard).ToBeVisibleAsync();

        var href = await firstCard.Locator(".card-link").GetAttributeAsync("href");
        href.Should().NotBeNullOrEmpty("card should have a link with href attribute");
        href.Should().StartWith("https://", "news items should link to external URLs");
    }

    [Fact]
    public async Task InfiniteScroll_WithTagFilter_MaintainsFilterThroughPagination()
    {
        // Arrange - tag filter uses lowercase in URL, spaces are URL-encoded as %20
        const string TagDisplay = "Copilot Chat"; // Display text on button
        const string TagUrl = "copilot%20chat"; // URL-encoded version

        // Act - Navigate and apply tag filter
        await Page.GotoRelativeAsync("/github-copilot/news");

        // Wait for initial load
        await Page.WaitForConditionAsync(
            "() => document.querySelectorAll('.card').length > 0");

        // Apply tag filter
        var tagButton = Page.Locator($"button.tag-cloud-item:has-text('{TagDisplay}')").First;
        await tagButton.ClickBlazorElementAsync(waitForUrlChange: false);

        // Wait for filter to apply and content to load
        await Page.WaitForBlazorUrlContainsAsync($"tags={TagUrl}");

        // Wait for filtered content to render (cards should appear)
        await Page.WaitForConditionAsync(
            "() => document.querySelectorAll('.card').length > 0");

        // Capture first batch count
        var firstBatchCount = await Page.Locator(".card").CountAsync();
        firstBatchCount.Should().BeGreaterThan(0, "should have items after filtering");

        // Only test infinite scroll if there's more content available
        var scrollTrigger = Page.Locator("#scroll-trigger");
        var initialScrollTriggerExists = await scrollTrigger.CountAsync() > 0;

        if (initialScrollTriggerExists)
        {
            // Scroll to load more items using JS scrollIntoView + scrollBy approach
            await Page.ScrollToLoadMoreAsync(expectedItemCount: firstBatchCount + 1);

            // Capture count after scroll
            var afterScrollCount = await Page.Locator(".card").CountAsync();
            afterScrollCount.Should().BeGreaterThan(firstBatchCount,
                "scrolling should load additional items when more are available");
        }
        else
        {
            // If no scroll trigger exists, it means first batch loaded all available items
            // This is valid - just verify we got some items
            firstBatchCount.Should().BeGreaterThan(0, "should have loaded at least some filtered items");
        }

        // Verify URL still contains tag filter (URL normalizes to lowercase)
        var currentUrl = Page.Url;
        currentUrl.Should().Contain($"tags={TagUrl}",
            "tag filter should be preserved in URL during infinite scroll");

        // Verify tag button still has 'selected' class (component uses CSS class, not aria-pressed)
        var hasSelectedClass = await tagButton.EvaluateAsync<bool>("el => el.classList.contains('selected')");
        hasSelectedClass.Should().BeTrue("tag button should remain selected after scrolling");
    }
}
