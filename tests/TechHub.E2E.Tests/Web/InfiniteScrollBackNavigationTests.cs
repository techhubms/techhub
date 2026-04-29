using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for infinite scroll back-navigation scroll position restoration.
/// Validates that scrolling down, loading more content, navigating to a detail page,
/// and pressing back restores the scroll position instead of scrolling to the bottom.
/// </summary>
public class InfiniteScrollBackNavigationTests : PlaywrightTestBase
{
    public InfiniteScrollBackNavigationTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task BackNavigation_AfterInfiniteScroll_RestoresScrollPosition()
    {
        // Arrange - Navigate to a browse page with enough content for infinite scroll
        await Page.GotoRelativeAsync("/all?types=videos");

        // Wait for initial content to load
        await Page.WaitForConditionAsync(
            "() => document.querySelectorAll('.card').length > 0");

        var initialCount = await Page.Locator(".card").CountAsync();
        initialCount.Should().BeGreaterThan(0, "should have initial content cards");

        // Verify scroll trigger exists (more content available)
        var hasScrollTrigger = await Page.EvaluateAsync<bool>(
            "() => document.getElementById('scroll-trigger') !== null");

        if (!hasScrollTrigger)
        {
            // Not enough content for infinite scroll — skip this test
            return;
        }

        // Act - Scroll to load at least one more batch
        var expectedAfterScroll = initialCount + 1;
        await Page.ScrollToLoadMoreAsync(expectedAfterScroll);

        var afterScrollCount = await Page.Locator(".card").CountAsync();
        afterScrollCount.Should().BeGreaterThan(initialCount,
            "should have loaded more items after scrolling");

        // Scroll to a realistic mid-page position. ScrollToLoadMoreAsync leaves us at
        // the absolute document bottom (an edge case). A real user would be browsing
        // items somewhere in the middle. This also ensures the scroll-trigger is well
        // outside the 300px viewport margin, preventing a cascade on back-navigation.
        await Page.EvaluateAsync(@"() => {
            const midY = Math.floor((document.documentElement.scrollHeight - window.innerHeight) / 2);
            window.scrollTo(0, midY);
            window.dispatchEvent(new Event('scroll'));
        }");

        // Capture the realistic mid-page scroll position
        var scrollYBeforeNav = await Page.EvaluateAsync<double>("() => window.scrollY");
        scrollYBeforeNav.Should().BeGreaterThan(0,
            "should be scrolled down after loading more content");

        // Navigate away via enhanced navigation. Use JavaScript's native .click()
        // instead of Playwright's ClickAsync because Playwright always calls
        // scrollIntoViewIfNeeded before clicking, which fires a scroll event that
        // overwrites the saved scroll position in infinite-scroll.js.
        // JS .click() dispatches the click event directly — Blazor's router intercepts it.
        var visibleCardIndex = await Page.EvaluateAsync<int>(@"() => {
            const links = document.querySelectorAll('.card-link');
            for (let i = 0; i < links.length; i++) {
                const rect = links[i].getBoundingClientRect();
                if (rect.top >= 0 && rect.top < window.innerHeight) return i;
            }
            return 0;
        }");
        await Page.EvaluateAsync(
            "(idx) => document.querySelectorAll('.card-link')[idx].click()",
            visibleCardIndex);
        await Page.WaitForConditionAsync(
            "() => !window.location.search.includes('types=videos')");
        await Page.WaitForBlazorReadyAsync();

        // Act - Press browser back button
        await Page.GoBackAsync();
        await Page.WaitForBlazorUrlContainsAsync("types=videos");

        // Wait for content to be restored from circuit cache
        await Page.WaitForConditionAsync(
            $"(expected) => document.querySelectorAll('.card').length >= expected",
            afterScrollCount);

        // Wait for scroll listener to be set up (confirms OnAfterRenderAsync completed,
        // which includes both scroll restoration and listener attachment)
        await Page.WaitForConditionAsync(
            "() => window.__scrollListenerReady?.['scroll-trigger'] === true");

        // Assert - Scroll position should be restored near where the user was
        // Wait briefly for scroll position to be applied (scrollTo is synchronous but
        // the browser may batch the update with the preceding DOM changes).
        await Page.WaitForConditionAsync(
            "() => window.scrollY > 0");
        var restoredPosition = await Page.EvaluateAsync<double>("() => window.scrollY");
        var maxScrollY = await Page.EvaluateAsync<double>(
            "() => document.documentElement.scrollHeight - window.innerHeight");

        var distanceFromOriginal = Math.Abs(restoredPosition - scrollYBeforeNav);
        distanceFromOriginal.Should().BeLessThan(200,
            $"scroll position should be restored near {scrollYBeforeNav}px, " +
            $"but was at {restoredPosition}px (max scroll: {maxScrollY}px)");
    }

    [Fact]
    public async Task BackNavigation_AfterInfiniteScroll_DoesNotTriggerCascade()
    {
        // Arrange - Navigate to a browse page with infinite scroll content
        await Page.GotoRelativeAsync("/all?types=videos");

        await Page.WaitForConditionAsync(
            "() => document.querySelectorAll('.card').length > 0");

        var initialCount = await Page.Locator(".card").CountAsync();

        var hasScrollTrigger = await Page.EvaluateAsync<bool>(
            "() => document.getElementById('scroll-trigger') !== null");

        if (!hasScrollTrigger)
        {
            return; // Not enough content for this test
        }

        // Load one more batch
        await Page.ScrollToLoadMoreAsync(initialCount + 1);
        var afterScrollCount = await Page.Locator(".card").CountAsync();

        // Scroll up from the very bottom so the saved position doesn't keep the
        // scroll-trigger within the 300px viewport margin. ScrollToLoadMoreAsync
        // leaves scrollY at document bottom (artificially), but a real user would
        // be browsing items further up. Position the trigger >300px below viewport.
        await Page.EvaluateAsync(@"() => {
            const trigger = document.getElementById('scroll-trigger');
            if (trigger) {
                const triggerTop = trigger.getBoundingClientRect().top + window.scrollY;
                window.scrollTo(0, Math.max(0, triggerTop - window.innerHeight - 400));
                window.dispatchEvent(new Event('scroll'));
            }
        }");

        // Navigate away by clicking a card link (must use enhanced navigation to
        // preserve the Blazor circuit and ContentGridStateCache — a full page load
        // via GotoRelativeAsync would destroy both, making back-navigation start fresh).
        var firstCardLink = Page.Locator(".card-link").First;
        await firstCardLink.ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page).Not.ToHaveURLAsync(
                new System.Text.RegularExpressions.Regex(@".*\?types=videos.*"),
                new() { Timeout = 2000 }));

        // Act - Go back
        await Page.GoBackAsync();
        await Page.WaitForBlazorUrlContainsAsync("types=videos");

        // Wait for cache restoration AND scroll listener setup (both happen in
        // OnAfterRenderAsync). The scroll listener being ready confirms that
        // OnAfterRenderAsync completed — scroll was restored and the listener's
        // immediate handleScroll() check ran without triggering a cascade.
        await Page.WaitForConditionAsync(
            $"(expected) => document.querySelectorAll('.card').length >= expected && window.__scrollListenerReady?.['scroll-trigger'] === true",
            afterScrollCount);

        // Wait for scroll position to be restored (restoreScrollPosition runs before
        // SetupScrollListener in OnAfterRenderAsync, but the browser may batch the
        // scrollTo update with preceding DOM changes).
        await Page.WaitForConditionAsync(
            "() => window.scrollY > 0");

        var finalCount = await Page.Locator(".card").CountAsync();

        // Allow at most one additional batch (20 items) as tolerance —
        // a cascade would load ALL remaining content
        var maxAcceptableCount = afterScrollCount + 20;
        finalCount.Should().BeLessThanOrEqualTo(maxAcceptableCount,
            $"back-navigation should NOT trigger a cascade of batch loads. " +
            $"Expected ~{afterScrollCount} items but got {finalCount}");
    }
}
