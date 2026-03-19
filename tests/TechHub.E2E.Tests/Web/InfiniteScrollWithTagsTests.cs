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
        // Arrange - Navigate directly with tag filter applied in the URL.
        // This tests that infinite scroll pagination preserves the tag filter,
        // which works identically regardless of how the filter was applied.
        // Tag click interaction is covered by TagFilteringTests.
        const string Tag = "copilot chat";
        const string TagUrlPrefix = "tags=copilot";

        // Act - Navigate directly with the tag filter pre-applied
        await Page.GotoRelativeAsync($"/github-copilot/news?tags={Tag}");

        // Wait for filtered content to render (cards should appear)
        await Page.WaitForConditionAsync(
            "() => document.querySelectorAll('.card').length > 0");

        // Capture first batch count
        var firstBatchCount = await Page.Locator(".card").CountAsync();
        firstBatchCount.Should().BeGreaterThan(0, "should have items after filtering");

        // Only test infinite scroll if there's more content available.
        // Wait for the scroll trigger element to either appear (more pages) or end-of-content
        // to confirm the initial batch has fully rendered.
        await Page.WaitForConditionAsync(
            "() => document.getElementById('scroll-trigger') !== null || document.querySelector('.end-of-content') !== null",
            new PageWaitForFunctionOptions
            {
                Timeout = BlazorHelpers.IncreasedTimeout,
                PollingInterval = BlazorHelpers.DefaultPollingInterval
            });

        var scrollTriggerExists = await Page.EvaluateAsync<bool>(
            "() => document.getElementById('scroll-trigger') !== null");

        if (scrollTriggerExists)
        {
            // Scroll to trigger the next batch load. The scroll trigger is present, meaning the
            // component believes more items may exist. However, the next batch may return 0 items
            // (e.g., when total items exactly equals the batch size), which replaces the scroll
            // trigger with end-of-content. Accept either outcome: more items OR end-of-content.
            await Page.WaitForConditionAsync(
                $"() => window.__scrollListenerReady?.['scroll-trigger'] === true && document.getElementById('scroll-trigger') !== null",
                new PageWaitForFunctionOptions
                {
                    Timeout = BlazorHelpers.IncreasedTimeout,
                    PollingInterval = BlazorHelpers.DefaultPollingInterval
                });

            await Page.WaitForFunctionAsync(
                @"(firstBatch) => {
                    window.scrollTo({ top: document.documentElement.scrollHeight, behavior: 'instant' });
                    window.dispatchEvent(new Event('scroll'));
                    const cardCount = document.querySelectorAll('.card').length;
                    const endOfContent = document.querySelector('.end-of-content') !== null;
                    return cardCount > firstBatch || endOfContent;
                }",
                firstBatchCount,
                new PageWaitForFunctionOptions
                {
                    Timeout = BlazorHelpers.IncreasedTimeout,
                    PollingInterval = BlazorHelpers.DefaultPollingInterval
                });

            // Verify that something happened (more items loaded or end reached)
            var afterScrollCount = await Page.Locator(".card").CountAsync();
            var endOfContentVisible = await Page.Locator(".end-of-content").CountAsync() > 0;

            (afterScrollCount > firstBatchCount || endOfContentVisible).Should().BeTrue(
                "scrolling should either load additional items or reach end-of-content");
        }
        else
        {
            // If no scroll trigger exists, it means first batch loaded all available items
            // This is valid - just verify we got some items
            firstBatchCount.Should().BeGreaterThan(0, "should have loaded at least some filtered items");
        }

        // Verify URL still contains tag filter (URL normalizes to lowercase)
        var currentUrl = Page.Url;
        currentUrl.Should().Contain(TagUrlPrefix,
            "tag filter should be preserved in URL during infinite scroll");

        // Verify tag button has 'selected' class (component uses CSS class, not aria-pressed)
        var tagButton = Page.Locator("button.tag-cloud-item.selected").First;
        var hasSelectedClass = await tagButton.CountAsync() > 0;
        hasSelectedClass.Should().BeTrue("a tag button should be selected after navigating with tag filter");
    }
}
