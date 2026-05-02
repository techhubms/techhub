using FluentAssertions;
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

        // Act - Scroll to load at least one more batch.
        // Capture the scroll listener version BEFORE loading, so we can wait for a fresh
        // re-attachment after the batch load. On CI (testing against remote production),
        // the SignalR JS interop call that re-attaches the listener can arrive at the
        // browser AFTER our CDP EvaluateAsync below. Without this wait, the scroll-to-midY
        // event fires when no listener is active, so the position is never saved.
        var versionBefore = await Page.EvaluateAsync<int>(
            "() => window.__scrollListenerVersion?.['scroll-trigger'] || 0");

        var expectedAfterScroll = initialCount + 1;
        await Page.ScrollToLoadMoreAsync(expectedAfterScroll);

        var afterScrollCount = await Page.Locator(".card").CountAsync();
        afterScrollCount.Should().BeGreaterThan(initialCount,
            "should have loaded more items after scrolling");

        // Wait for scroll listener to be freshly re-attached after batch load render.
        // OnAfterRenderAsync → SetupScrollListener() → observeScrollTrigger() increments
        // the version counter. This ensures the listener is active before we scroll.
        await Page.WaitForConditionAsync(
            "(v) => (window.__scrollListenerVersion?.['scroll-trigger'] || 0) > v",
            versionBefore);

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

        // Navigate away via enhanced navigation. Use ClickVisibleCardLinkAsync which
        // uses JS .click() instead of Playwright's ClickAsync — Playwright always calls
        // scrollIntoViewIfNeeded before clicking, which fires a scroll event that
        // overwrites the saved scroll position in infinite-scroll.js.
        await Page.ClickVisibleCardLinkAsync();
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

        // Scroll up IMMEDIATELY to prevent cascade: OnAfterRenderAsync will re-attach the
        // scroll listener shortly after the batch completes, and its immediate handleScroll
        // check fires at the current scrollY. EvaluateAsync (CDP, local browser connection)
        // runs before observeScrollTrigger (SignalR, server→browser), so the trigger is
        // positioned >300px below the viewport when the immediate check runs, preventing
        // further cascade batch loads.
        await Page.EvaluateAsync(@"() => {
            const trigger = document.getElementById('scroll-trigger');
            if (trigger) {
                const triggerTop = trigger.getBoundingClientRect().top + window.scrollY;
                window.scrollTo(0, Math.max(0, triggerTop - window.innerHeight - 400));
                window.dispatchEvent(new Event('scroll'));
            }
        }");

        // Wait for the scroll listener to be re-attached at the scrolled-up position with no
        // batch load in progress. This confirms any cascade batch that was already in-flight
        // before the scroll-up has completed, and observeScrollTrigger's immediate handleScroll
        // check at the scrolled-up position did NOT trigger another cascade batch.
        await Page.WaitForConditionAsync(
            "() => window.__scrollListenerReady?.['scroll-trigger'] === true && !document.querySelector('.loading-more-indicator')");

        // If the cascade loaded all content the scroll trigger is gone — back-navigation
        // cannot cascade further, so this test scenario is no longer applicable.
        var hasScrollTriggerAfterLoad = await Page.EvaluateAsync<bool>(
            "() => document.getElementById('scroll-trigger') !== null");
        if (!hasScrollTriggerAfterLoad)
        {
            return;
        }

        var afterScrollCount = await Page.Locator(".card").CountAsync();

        // Navigate away via enhanced navigation. Use ClickVisibleCardLinkAsync which
        // uses JS .click() instead of Playwright's ClickAsync — Playwright always calls
        // scrollIntoViewIfNeeded before clicking, which fires a scroll event that
        // overwrites the saved scroll position with ~0.
        await Page.ClickVisibleCardLinkAsync();
        await Page.WaitForConditionAsync(
            "() => !window.location.search.includes('types=videos')");
        await Page.WaitForBlazorReadyAsync();

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
