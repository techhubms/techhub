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
        // re-attachment after the batch load. This ensures the infinite-scroll listener
        // is active before we scroll to a mid-page position (needed for the trigger
        // check to work correctly on subsequent scrolls).
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
            versionBefore,
            onTimeout: "() => JSON.stringify({version: window.__scrollListenerVersion?.['scroll-trigger'] || 0, ready: window.__scrollListenerReady?.['scroll-trigger']})");

        // Scroll to a realistic mid-page position. ScrollToLoadMoreAsync leaves us at
        // the absolute document bottom (an edge case). A real user would be browsing
        // items somewhere in the middle. This also ensures the scroll-trigger is well
        // outside the 300px viewport margin, preventing a cascade on back-navigation.
        // Use ScrollToPositionAsync (not raw scrollTo) so __scrollSaveLock is set:
        // this guarantees saveScrollPosition() records exactly midY regardless of
        // whether the scrollend event fires before ClickVisibleCardLinkAsync.
        var midY = await Page.EvaluateAsync<int>(
            "() => Math.floor((document.documentElement.scrollHeight - window.innerHeight) / 2)");
        await Page.ScrollToPositionAsync(midY);

        // scrollYBeforeNav equals midY exactly: ScrollToPositionAsync retries until
        // window.scrollY === targetY and sets __scrollSaveLock to the same integer.
        var scrollYBeforeNav = (double)midY;
        scrollYBeforeNav.Should().BeGreaterThan(0,
            "should be scrolled down after loading more content");

        // Navigate away via enhanced navigation. Use ClickVisibleCardLinkAsync which
        // uses JS .click() instead of Playwright's ClickAsync — Playwright always calls
        // scrollIntoViewIfNeeded before clicking, which fires a scroll event that
        // overwrites the saved scroll position in scroll-manager.js with ~0.
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
            afterScrollCount,
            onTimeout: $"() => `${{document.querySelectorAll('.card').length}} cards loaded, expected {afterScrollCount}`");

        // Wait for markScriptsReady to complete. markScriptsReady now defers __scriptsReady = true
        // until __scrollListenerReady['scroll-trigger'] is true, so WaitForBlazorReadyAsync
        // implicitly covers the scroll listener check — no separate check needed.
        // This also ensures restoreScrollPosition() has been called before the scroll wait below.
        await Page.WaitForBlazorReadyAsync();

        // Guard: wait for any loading indicator to disappear before asserting scroll position.
        // The cascade-scroll bug (scroll-manager.js pendingIntersect flush) can temporarily
        // load extra content on back-navigation; waiting here ensures that work is done and
        // restoreScrollPosition() has had a chance to scroll to the saved midY value.
        await Page.WaitForConditionAsync(
            "() => !document.querySelector('.loading-more-indicator')",
            onTimeout: "() => JSON.stringify({loadingIndicator: !!document.querySelector('.loading-more-indicator'), cards: document.querySelectorAll('.card').length, scrollY: window.scrollY})");

        // Assert — scroll position should be restored to exactly where the user was.
        // Tolerance is 10px: the JS uses `maxScroll >= y - 5` before scrolling, so
        // the maximum possible clamping error is 5px; 10px gives comfortable headroom.
        // onTimeout captures diagnostics instead of a cryptic Playwright TimeoutException.
        await Page.WaitForConditionAsync(
            "(expected) => Math.abs(window.scrollY - expected) < 10",
            (object)scrollYBeforeNav,
            onTimeout: "() => JSON.stringify({scrollY: window.scrollY, maxScroll: document.documentElement.scrollHeight - window.innerHeight})");
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

        // Scroll to 3 cards from the end of the loaded content. This simulates a user
        // who has browsed through loaded items and is near the bottom — the trigger is
        // roughly 2-3 card heights (~600-900px) below the viewport, which is safely
        // outside the 300px TRIGGER_MARGIN_PX but far more realistic than scrollY=200.
        // Using card-relative positioning avoids layout-shift sensitivity: the trigger
        // is anchored to card count, not absolute pixel measurements that shift on CI.
        await Page.EvaluateAsync(@"() => {
            const cards = document.querySelectorAll('.card');
            const targetCard = cards[Math.max(0, cards.length - 3)];
            if (targetCard) {
                // block:'end' puts targetCard at the viewport bottom; the last 2 cards
                // and the scroll-trigger sit below the fold, outside the 300px margin.
                targetCard.scrollIntoView({ block: 'end', behavior: 'instant' });
            }
            window.dispatchEvent(new Event('scroll'));
        }");

        // Wait for scroll listener to be ready with no batch load in progress.
        // Cannot use WaitForBlazorReadyAsync here: __scriptsReady may still be true from before
        // the re-render cycle, causing it to pass before the fresh listener is attached.
        await Page.WaitForConditionAsync(
            "() => window.__scrollListenerReady?.['scroll-trigger'] === true && !document.querySelector('.loading-more-indicator')",
            onTimeout: "() => JSON.stringify({ready: window.__scrollListenerReady?.['scroll-trigger'], loadingIndicator: !!document.querySelector('.loading-more-indicator')})");

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
        // overwrites the saved scroll position in scroll-manager.js with ~0.
        await Page.ClickVisibleCardLinkAsync();
        await Page.WaitForConditionAsync(
            "() => !window.location.search.includes('types=videos')");
        await Page.WaitForBlazorReadyAsync();

        // Act - Go back
        await Page.GoBackAsync();
        await Page.WaitForBlazorUrlContainsAsync("types=videos");

        // Wait for cache restoration. WaitForBlazorReadyAsync (below) implicitly ensures
        // __scrollListenerReady is true via markScriptsReady's deferred polling.
        await Page.WaitForConditionAsync(
            $"(expected) => document.querySelectorAll('.card').length >= expected",
            afterScrollCount,
            onTimeout: $"() => `${{document.querySelectorAll('.card').length}} cards loaded, expected {afterScrollCount}`");

        // Wait for markScriptsReady to complete — confirms __scrollListenerReady is true
        // and the anti-cascade __scrollRestoring window has closed.
        await Page.WaitForBlazorReadyAsync();

        // Confirm no batch load is in progress (a cascade would show the loading indicator).
        await Page.WaitForConditionAsync(
            "() => !document.querySelector('.loading-more-indicator')",
            onTimeout: "() => JSON.stringify({loadingIndicator: !!document.querySelector('.loading-more-indicator'), cards: document.querySelectorAll('.card').length})");

        var finalCount = await Page.Locator(".card").CountAsync();

        // Allow at most one additional batch (20 items) as tolerance —
        // a cascade would load ALL remaining content
        var maxAcceptableCount = afterScrollCount + 20;
        finalCount.Should().BeLessThanOrEqualTo(maxAcceptableCount,
            $"back-navigation should NOT trigger a cascade of batch loads. " +
            $"Expected ~{afterScrollCount} items but got {finalCount}");
    }

    /// <summary>
    /// Regression test for the "cascade scroll" bug:
    /// When the user navigates back to a page with infinite scroll after having been
    /// near the bottom (scroll trigger just outside the viewport + 300px margin),
    /// the page should NOT automatically scroll to the very end while loading all
    /// remaining content. The scroll position should stay near where the user was.
    ///
    /// Root cause: finishNavigation() flushed the pending IO intersection by calling
    /// LoadNextBatch regardless of whether the scroll trigger was actually visible at
    /// the restored position. The IO fires at scroll-Y=0 (before scrollTo runs), so
    /// the trigger appeared near the top — but after scroll restore it may be well
    /// below the viewport. Fixed: finishNavigation() checks getBoundingClientRect()
    /// after scrollTo and only flushes if the trigger is genuinely in the extended
    /// viewport (top &lt; innerHeight + TRIGGER_MARGIN_PX AND bottom &gt; 0).
    /// </summary>
    [Fact]
    public async Task BackNavigation_CascadingLoad_ScrollDoesNotReachEnd()
    {
        // Arrange - Navigate to a browse page with enough content for infinite scroll
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

        // Load one more batch so there is a scroll trigger with content below it
        await Page.ScrollToLoadMoreAsync(initialCount + 1);

        // Scroll to 3 cards from the end — trigger is just outside the viewport + 300px
        // margin, the most likely position to trigger a cascade on back-navigation.
        await Page.EvaluateAsync(@"() => {
            const cards = document.querySelectorAll('.card');
            const targetCard = cards[Math.max(0, cards.length - 3)];
            if (targetCard) {
                targetCard.scrollIntoView({ block: 'end', behavior: 'instant' });
            }
            window.dispatchEvent(new Event('scroll'));
        }");

        // Wait for scrollend to fire so lastSettledScrollY is updated to the new position.
        // Without this, saveScrollPosition() in beforeenhancedload reads a stale value,
        // restores to the wrong Y on back-nav, and the trigger is in viewport → cascade.
        await Page.WaitForScrollEndAsync();

        // Wait for scroll listener to be ready and no load in progress
        await Page.WaitForConditionAsync(
            "() => window.__scrollListenerReady?.['scroll-trigger'] === true && !document.querySelector('.loading-more-indicator')",
            onTimeout: "() => JSON.stringify({ready: window.__scrollListenerReady?.['scroll-trigger'], loadingIndicator: !!document.querySelector('.loading-more-indicator')})");

        // If scrolling to 3-from-end triggered another cascade that loaded all content,
        // skip — no scroll trigger remains and the scenario is no longer testable.
        var hasScrollTriggerAfterPosition = await Page.EvaluateAsync<bool>(
            "() => document.getElementById('scroll-trigger') !== null");
        if (!hasScrollTriggerAfterPosition)
        {
            return;
        }

        var scrollYBeforeNav = await Page.EvaluateAsync<double>("window.scrollY");
        var afterScrollCount = await Page.Locator(".card").CountAsync();

        // Navigate away via enhanced navigation
        await Page.ClickVisibleCardLinkAsync();
        await Page.WaitForConditionAsync(
            "() => !window.location.search.includes('types=videos')");
        await Page.WaitForBlazorReadyAsync();

        // Act - Press browser back button
        await Page.GoBackAsync();
        await Page.WaitForBlazorUrlContainsAsync("types=videos");

        // Wait for content to load (circuit cache or re-fetch)
        await Page.WaitForConditionAsync(
            $"(expected) => document.querySelectorAll('.card').length >= expected",
            afterScrollCount,
            onTimeout: $"() => `${{document.querySelectorAll('.card').length}} cards loaded, expected {afterScrollCount}`");

        await Page.WaitForBlazorReadyAsync();

        // Wait for any loading to finish — a cascade would keep the loading indicator visible
        await Page.WaitForConditionAsync(
            "() => !document.querySelector('.loading-more-indicator')",
            onTimeout: "() => JSON.stringify({loadingIndicator: !!document.querySelector('.loading-more-indicator'), cards: document.querySelectorAll('.card').length, scrollY: window.scrollY})");

        // Assert — scroll must NOT be at the absolute bottom of all loaded content.
        // If the cascade bug fires, the page scrolls to the end (scrollY ≈ maxScroll).
        // A 500px margin avoids flakiness from minor layout shifts while still catching
        // the bug where hundreds of extra cards were loaded and scrolled into view.
        var scrollY = await Page.EvaluateAsync<double>("window.scrollY");
        var maxScroll = await Page.EvaluateAsync<double>(
            "document.documentElement.scrollHeight - window.innerHeight");

        if (maxScroll > 500)
        {
            scrollY.Should().BeLessThan(maxScroll - 500,
                $"back-navigation cascade bug: page scrolled to the very end. " +
                $"scrollY={scrollY}, maxScroll={maxScroll}, expected scrollY near {scrollYBeforeNav}");
        }
    }
}
