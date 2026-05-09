using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for the "Load more" button on collection pages.
/// Validates pagination, correct appending of items, and end-of-content display.
/// </summary>
public class LoadMoreButtonTests : PlaywrightTestBase
{
    private const string LoadMoreEnabledOrEndOfContentCondition =
        "() => document.querySelector('.load-more-btn:not([disabled])') !== null || document.querySelector('.end-of-content') !== null";
    private const string HasEnabledLoadMoreCondition =
        "() => document.querySelector('.load-more-btn:not([disabled])') !== null";

    public LoadMoreButtonTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task ContentGrid_InitialLoad_ShowsItems()
    {
        await Page.GotoRelativeAsync("/github-copilot/news");

        await Page.WaitForConditionAsync(
            "() => document.querySelectorAll('.card').length > 0");

        var count = await Page.Locator(".card").CountAsync();
        count.Should().BeGreaterThan(0, "initial batch should contain content cards");
    }

    [Fact]
    public async Task ContentGrid_InitialLoad_ShowsLoadMoreOrEndOfContent()
    {
        await Page.GotoRelativeAsync("/github-copilot/news");

        await Page.WaitForConditionAsync(
            "() => document.querySelectorAll('.card').length > 0");

        // Either a Load more button is present, or End of content is shown —
        // both are valid depending on how many items the collection has.
        await Page.WaitForConditionAsync(
            "() => document.querySelector('.load-more-btn') !== null || document.querySelector('.end-of-content') !== null");

        var hasLoadMore = await Page.EvaluateAsync<bool>(
            "() => document.querySelector('.load-more-btn') !== null");
        var hasEndOfContent = await Page.EvaluateAsync<bool>(
            "() => document.querySelector('.end-of-content') !== null");

        (hasLoadMore || hasEndOfContent).Should().BeTrue(
            "page should show either a Load more button or End of content indicator");
    }

    [Fact]
    public async Task LoadMoreButton_WhenClicked_AppendsMoreItems()
    {
        // Prefer ai/blogs because it typically has more than one batch.
        await Page.GotoRelativeAsync("/ai/blogs");

        await Page.WaitForConditionAsync(
            "() => document.querySelectorAll('.card').length > 0");

        // In preview environments content can fluctuate. If the collection currently fits in
        // one batch, skip explicitly instead of passing vacuously.
        await Page.WaitForConditionAsync(
            LoadMoreEnabledOrEndOfContentCondition);

        var hasLoadMore = await Page.EvaluateAsync<bool>(
            HasEnabledLoadMoreCondition);
        Assert.SkipWhen(!hasLoadMore,
            "Collection currently has <= BatchSize items in this environment; no Load more interaction to test.");

        var initialCount = await Page.Locator(".card").CountAsync();

        // Click Load more and wait for card count to increase
        var loadMoreBtn = Page.Locator(".load-more-btn");
        await loadMoreBtn.ClickAndExpectAsync(async () =>
        {
            var newCount = await Page.Locator(".card").CountAsync();
            newCount.Should().BeGreaterThan(initialCount,
                $"clicking Load more should append items (was {initialCount})");
        });
    }

    [Fact]
    public async Task LoadMoreButton_WhenClicked_ItemsAreAppendedNotReplaced()
    {
        // Prefer ai/blogs because it typically has more than one batch.
        await Page.GotoRelativeAsync("/ai/blogs");

        await Page.WaitForConditionAsync(
            "() => document.querySelectorAll('.card').length > 0");

        // In preview environments content can fluctuate. If the collection currently fits in
        // one batch, skip explicitly instead of passing vacuously.
        await Page.WaitForConditionAsync(
            LoadMoreEnabledOrEndOfContentCondition);

        var hasLoadMore = await Page.EvaluateAsync<bool>(
            HasEnabledLoadMoreCondition);
        Assert.SkipWhen(!hasLoadMore,
            "Collection currently has <= BatchSize items in this environment; no Load more interaction to test.");

        var initialCount = await Page.Locator(".card").CountAsync();

        // Capture the title of the very first card (it must still be there after load)
        var firstCardTitle = await Page.Locator(".card").First.TextContentAsync();

        var loadMoreBtn = Page.Locator(".load-more-btn");
        await loadMoreBtn.ClickAndExpectAsync(async () =>
        {
            var newCount = await Page.Locator(".card").CountAsync();
            newCount.Should().BeGreaterThan(initialCount,
                "items should be appended, not replaced");
        });

        // Verify first card is still present (was not replaced)
        var firstCardAfterLoad = await Page.Locator(".card").First.TextContentAsync();
        firstCardAfterLoad.Should().Be(firstCardTitle,
            "first card must remain in place after loading more items");
    }

    [Fact]
    public async Task LoadMoreButton_WhenAllContentLoaded_ShowsEndOfContent()
    {
        // Use ai/blogs (47 items) — exactly one Load More click (40 SSR + 7 remainder).
        // A small last batch keeps the Blazor SignalR diff tiny, which is critical for
        // the slow3g network profile where large diffs take >60 s to transfer.
        await Page.GotoRelativeAsync("/ai/blogs");

        await Page.WaitForConditionAsync(
            "() => document.querySelectorAll('.card').length > 0");

        // Keep clicking Load more until End of content appears (or we run out of attempts)
        const int MaxClicks = 20;
        for (var i = 0; i < MaxClicks; i++)
        {
            // Wait for either the button to become enabled (not disabled/loading) or
            // end-of-content to appear. Under slow3g the button stays disabled while
            // the Blazor circuit processes the previous batch — we must NOT click it
            // while disabled or the click is silently dropped.
            await Page.WaitForConditionAsync(
                "() => document.querySelector('.load-more-btn:not([disabled])') !== null || document.querySelector('.end-of-content') !== null",
                new PageWaitForFunctionOptions { Timeout = BlazorHelpers.E2ETimeout });

            // If end-of-content appeared while the button was disabled (last batch
            // finished loading), we're done — no click needed.
            var isEnd = await Page.EvaluateAsync<bool>(
                "() => document.querySelector('.end-of-content') !== null");
            if (isEnd) break;

            var countBefore = await Page.Locator(".card").CountAsync();

            // Under slow3g the Blazor SignalR circuit may briefly disconnect, causing a single
            // ClickAsync to be silently dropped. We use a short-poll retry: click once, wait 5 s
            // for the DOM to update, then re-click if it hasn't. Each re-click is safe because
            // the Load More button is disabled while a batch is in-flight.
            var batchCompleted = false;
            var deadline = System.Diagnostics.Stopwatch.StartNew();
            while (!batchCompleted && deadline.ElapsedMilliseconds < BlazorHelpers.E2ETimeout)
            {
                // Already done? (count grew or end-of-content appeared since loop started)
                var alreadyDone = await Page.EvaluateAsync<bool>(
                    $"() => document.querySelectorAll('.card').length > {countBefore} || document.querySelector('.end-of-content') !== null");
                if (alreadyDone) { batchCompleted = true; break; }

                // Click only when the button is enabled (not mid-load, not removed yet)
                var buttonEnabled = await Page.EvaluateAsync<bool>(
                    "() => { var b = document.querySelector('.load-more-btn'); return b !== null && !b.disabled; }");
                if (buttonEnabled)
                {
                    try
                    {
                        await Page.Locator(".load-more-btn").ClickAsync(new() { Timeout = 2000 });
                    }
                    catch (PlaywrightException)
                    {
                        // Button became disabled/disappeared between JS check and click — safe to retry
                    }
                }

                // Give Blazor up to 5 s to respond; re-poll (and re-click) if it hasn't yet
                try
                {
                    await Page.WaitForConditionAsync(
                        $"() => document.querySelectorAll('.card').length > {countBefore} || document.querySelector('.end-of-content') !== null",
                        new PageWaitForFunctionOptions { Timeout = 5000 });
                    batchCompleted = true;
                }
                catch (TimeoutException)
                {
                    // No response yet — outer while-loop will re-click if the button is available
                }
            }

            batchCompleted.Should().BeTrue($"batch {i + 1} should load within {BlazorHelpers.E2ETimeout}ms");
        }

        var endOfContent = Page.Locator(".end-of-content");
        await Assertions.Expect(endOfContent).ToBeVisibleAsync(
            new() { Timeout = BlazorHelpers.E2ETimeout });
    }

    [Fact]
    public async Task LoadMoreButton_WithTagFilter_PreservesFilterAcrossLoads()
    {
        const string Tag = "copilot chat";

        await Page.GotoRelativeAsync($"/github-copilot/news?tags={Uri.EscapeDataString(Tag)}");

        await Page.WaitForConditionAsync(
            "() => document.querySelectorAll('.card').length > 0");

        // Verify URL still contains the tag filter
        Page.Url.Should().Contain("tags=copilot", "URL should preserve tag filter after navigation");

        await Page.WaitForConditionAsync(
            "() => document.querySelector('.load-more-btn') !== null || document.querySelector('.end-of-content') !== null");

        var hasLoadMore = await Page.EvaluateAsync<bool>(
            "() => document.querySelector('.load-more-btn') !== null");

        if (!hasLoadMore) return;

        var initialCount = await Page.Locator(".card").CountAsync();

        var loadMoreBtn = Page.Locator(".load-more-btn");
        await loadMoreBtn.ClickAndExpectAsync(async () =>
        {
            var newCount = await Page.Locator(".card").CountAsync();
            var endVisible = await Page.EvaluateAsync<bool>(
                "() => document.querySelector('.end-of-content') !== null");
            (newCount > initialCount || endVisible).Should().BeTrue(
                "Load more with tag filter should load more filtered items or show end of content");
        });

        // Tag filter is preserved in the URL after loading more
        Page.Url.Should().Contain("tags=copilot", "tag filter should be preserved after Load more");
    }

    [Fact]
    public async Task BackNavigation_AfterLoadMore_RestoresScrollPosition()
    {
        await Page.GotoRelativeAsync("/github-copilot/news");

        await Page.WaitForConditionAsync(
            "() => document.querySelectorAll('.card').length > 0");

        await Page.WaitForConditionAsync(
            "() => document.querySelector('.load-more-btn') !== null || document.querySelector('.end-of-content') !== null");

        var hasLoadMore = await Page.EvaluateAsync<bool>(
            "() => document.querySelector('.load-more-btn') !== null");

        if (!hasLoadMore)
        {
            return;
        }

        // Load more items so the page grows
        var loadMoreBtn = Page.Locator(".load-more-btn");
        var initialCount = await Page.Locator(".card").CountAsync();
        await loadMoreBtn.ClickAndExpectAsync(async () =>
        {
            var newCount = await Page.Locator(".card").CountAsync();
            newCount.Should().BeGreaterThan(initialCount,
                "load more should append items before scrolling");
        });

        // Scroll to a mid-page position
        await Page.ScrollToPositionAsync(500);

        // Navigate away via sub-nav to a different page (SPA-style enhanced navigation)
        // Using the "all" collection link which has a predictable URL that is NOT /github-copilot/news
        var allLink = Page.Locator(".sub-nav a[href*='/github-copilot/all']").First;
        await allLink.ClickAndExpectAsync(async () =>
        {
            await Assertions.Expect(Page).ToHaveURLAsync(
                new System.Text.RegularExpressions.Regex(@".*/github-copilot/all.*"),
                new() { Timeout = BlazorHelpers.E2ETimeout });
        });

        // Navigate back
        await Page.GoBackAsync();
        await Page.WaitForBlazorUrlContainsAsync("github-copilot/news");
        await Page.WaitForBlazorReadyAsync();

        // Scroll position should be restored (allow ±100px tolerance).
        // Poll rather than assert immediately — scroll restoration may be async
        // (ResizeObserver path) when the page hasn't reached its final height yet.
        await Page.WaitForConditionAsync(
            "(expected) => Math.abs(window.scrollY - expected) < 100",
            (object)500.0,
            onTimeout: "() => JSON.stringify({scrollY: window.scrollY, savedPositions: window.__savedScrollPositions})");
    }
}
