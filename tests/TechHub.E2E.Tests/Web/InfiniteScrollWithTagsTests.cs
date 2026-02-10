using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for infinite scroll combined with tag filtering.
/// Validates the bug fix where infinite scrolling + tag filtering was broken.
/// </summary>
[Collection("Infinite Scroll Tests")]
public class InfiniteScrollWithTagsTests : IAsyncLifetime
{
    private readonly PlaywrightCollectionFixture _fixture;
    private IBrowserContext? _context;
    private IPage? _page;

    public InfiniteScrollWithTagsTests(PlaywrightCollectionFixture fixture)
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
    public async Task InfiniteScroll_WithTagFilter_LoadsCorrectContentType()
    {
        // This test validates that when navigating directly with a tag filter,
        // the correct content (external news links) is displayed.

        const string tag = "copilot";

        // Navigate directly to the page with tag filter applied
        await Page.GotoRelativeAsync($"/github-copilot/news?tags={tag}");

        // Wait for content to load
        await Page.WaitForFunctionAsync(
            "() => document.querySelectorAll('.card').length > 0",
            new PageWaitForFunctionOptions { Timeout = 10000, PollingInterval = 100 });

        // Verify we have content
        var cardCount = await Page.Locator(".card").CountAsync();
        cardCount.Should().BeGreaterThan(0, "should have content cards displayed with tag filter");

        // Verify URL still has the tag filter (wasn't cleared)
        Page.Url.Should().Contain($"tags={tag}", "URL should preserve tag filter");

        // The card itself is an <a> tag (the entire card is a link)
        // Verify first card links to an external URL (news items are external)
        var firstCard = Page.Locator("a.card").First;
        await Assertions.Expect(firstCard).ToBeVisibleAsync(
            new LocatorAssertionsToBeVisibleOptions { Timeout = 5000 });

        var href = await firstCard.GetAttributeAsync("href");
        href.Should().NotBeNullOrEmpty("card should have an href attribute");
        href.Should().StartWith("https://", "news items should link to external URLs");
    }

    [Fact]
    public async Task InfiniteScroll_WithTagFilter_MaintainsFilterThroughPagination()
    {
        // Arrange - tag filter uses lowercase in URL
        const string tagDisplay = "Copilot"; // Display text on button
        const string tagUrl = "copilot"; // URL-normalized version

        // Act - Navigate and apply tag filter
        await Page.GotoRelativeAsync("/github-copilot/news");

        // Wait for initial load
        await Page.WaitForFunctionAsync(
            "() => document.querySelectorAll('.card').length > 0",
            new PageWaitForFunctionOptions { Timeout = 10000 });

        // Apply tag filter
        var tagButton = Page.Locator($"button.tag-cloud-item:has-text('{tagDisplay}')").First;
        await tagButton.ClickBlazorElementAsync(waitForUrlChange: false);

        // Wait for filter to apply and content to load
        await Page.WaitForBlazorUrlContainsAsync($"tags={tagUrl}");
        
        // Wait for filtered content to render (cards should appear)
        await Page.WaitForFunctionAsync(
            "() => document.querySelectorAll('.card').length > 0",
            new PageWaitForFunctionOptions { Timeout = 5000, PollingInterval = 100 });

        // Capture first batch count
        var firstBatchCount = await Page.Locator(".card").CountAsync();
        firstBatchCount.Should().BeGreaterThan(0, "should have items after filtering");

        // Only test infinite scroll if there's more content available
        var scrollTrigger = Page.Locator("#scroll-trigger");
        var initialScrollTriggerExists = await scrollTrigger.CountAsync() > 0;
        
        if (initialScrollTriggerExists)
        {
            // Use polling that scrolls on each iteration to handle IntersectionObserver timing
            // The observer is set up in OnAfterRenderAsync and might not be ready immediately
            await Page.WaitForFunctionAsync(
                @"(expectedCount) => {
                    const trigger = document.getElementById('scroll-trigger');
                    if (trigger) {
                        trigger.scrollIntoView({ behavior: 'auto', block: 'end' });
                    }
                    return document.querySelectorAll('.card').length > expectedCount;
                }",
                firstBatchCount,
                new PageWaitForFunctionOptions { Timeout = 15000, PollingInterval = 200 });
                
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
        currentUrl.Should().Contain($"tags={tagUrl}",
            "tag filter should be preserved in URL during infinite scroll");

        // Verify tag button still has 'selected' class (component uses CSS class, not aria-pressed)
        var hasSelectedClass = await tagButton.EvaluateAsync<bool>("el => el.classList.contains('selected')");
        hasSelectedClass.Should().BeTrue("tag button should remain selected after scrolling");
    }
}
