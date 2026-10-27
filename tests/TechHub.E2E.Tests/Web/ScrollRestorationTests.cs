using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for scroll position restoration on back/forward navigation.
/// These tests are intentionally separate from InfiniteScrollBackNavigationTests
/// to isolate the core scroll-restore mechanism from infinite-scroll complexity.
///
/// The pages used here (/github-copilot/vscode-updates, /ai/genai-basics) are
/// long custom content pages with NO infinite scroll (no scroll-trigger element).
/// This makes failures unambiguous: if the scroll position is not restored, the
/// fault is in the save/restore path, not in IntersectionObserver or batch loading.
/// </summary>
public class ScrollRestorationTests : PlaywrightTestBase
{
    public ScrollRestorationTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task BackNavigation_OnLongContentPage_RestoresScrollPosition()
    {
        // Arrange — navigate to a long content page that has no infinite scroll
        await Page.GotoRelativeAsync("/github-copilot/vscode-updates");

        // Confirm there is no scroll-trigger (this test must not involve infinite scroll)
        var hasScrollTrigger = await Page.EvaluateAsync<bool>(
            "() => document.getElementById('scroll-trigger') !== null");
        hasScrollTrigger.Should().BeFalse("vscode-updates is a custom content page, not an infinite-scroll grid");

        // Scroll to a mid-page position. ScrollToPositionAsync retries until the page
        // is tall enough (handles slow image loading under throttled networks).
        await Page.ScrollToPositionAsync(500);

        // Act — navigate away via enhanced (SPA-style) navigation so savedPositions is retained.
        var allLink = Page.Locator(".sub-nav a[href*='/github-copilot/all']").First;
        await allLink.ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page).ToHaveURLAsync(
                new Regex(@".*/github-copilot/all.*")));

        // Go back — triggers popstate → navigating='traverse' → restoreScrollPosition
        await Page.GoBackAsync();
        await Page.WaitForBlazorUrlContainsAsync("vscode-updates");
        await Page.WaitForBlazorReadyAsync();

        // Assert — scroll position should be restored to 500.
        // onTimeout captures diagnostics instead of a cryptic Playwright TimeoutException.
        await Page.WaitForConditionAsync(
            "(expected) => Math.abs(window.scrollY - expected) < 5",
            (object)500.0,
            onTimeout: "() => JSON.stringify({scrollY: window.scrollY, maxScroll: document.documentElement.scrollHeight - window.innerHeight, savedPositions: window.__savedScrollPositions})");
    }

    [Fact]
    public async Task ReadyBeforePushState_WhileNavigationActive_DoesNotRearmSpinnerOrReadinessLock()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot/vscode-updates");

        // Act — reproduce the race with the real markScriptsReady callback: a forward navigation is
        // active (armed by pushState), the destination signals ready (which completes the active
        // navigation synchronously via __restoreScrollPosition), and only then does the destination's
        // pushState run.
        var scriptsReadyAfterPushState = await Page.EvaluateAsync<bool?>(@"async () => {
            document.body.dispatchEvent(new MouseEvent('click', { bubbles: true }));
            history.pushState(null, '', location.pathname + '?e2e-nav=1');
            if (window.__scriptsReady !== false) throw new Error('The first pushState must arm navigation readiness');
            window.markScriptsReady();
            await new Promise(resolve => {
                const check = () => window.__scriptsReady === true ? resolve() : setTimeout(check, 10);
                check();
            });
            history.pushState(null, '', location.pathname + '?e2e-nav=2');
            return window.__scriptsReady;
        }");

        // Assert
        scriptsReadyAfterPushState.Should().BeTrue(
            "a pushState right after the ready signal that completed the active navigation must not reset __scriptsReady, " +
            "because no further markScriptsReady will follow to clear it");
        await Page.WaitForBlazorReadyAsync();
        await Assertions.Expect(Page.Locator("#nav-spinner.active")).ToHaveCountAsync(0);
    }
}
