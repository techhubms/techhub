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
                new Regex(@".*/github-copilot/all.*"), new() { Timeout = 2000 }));

        // Go back — triggers popstate → navigating='traverse' → restoreScrollPosition
        await Page.GoBackAsync();
        await Page.WaitForBlazorUrlContainsAsync("vscode-updates");
        await Page.WaitForBlazorReadyAsync();

        // Assert — scroll position should be restored to 500
        await Page.WaitForConditionAsync(
            "() => Math.abs(window.scrollY - 500) <= 5");

        var finalScrollY = await Page.EvaluateAsync<double>("() => window.scrollY");
        finalScrollY.Should().BeApproximately(500, 5,
            "scroll-manager should restore the saved position after back navigation");
    }
}
