using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for the GitHub Copilot features page (/github-copilot/features): the features of the catalog that have content,
/// shown as cards, with the subscriptions in the sidebar. The features come from the database of the environment, so these tests
/// need a database with features that have content (a restored production database has them) and fail without.
/// </summary>
public class GitHubCopilotFeaturesTests : PlaywrightTestBase
{
    public GitHubCopilotFeaturesTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    private const string PageUrl = "/github-copilot/features";

    [Fact]
    public async Task Features_ShouldLoad_WithTitleAndSidebarLayout()
    {
        await Page.GotoRelativeAsync(PageUrl);

        await Assertions.Expect(Page).ToHaveTitleAsync(new Regex("Features - Tech Hub"));
        await Page.Locator("main.page-with-sidebar").AssertElementVisibleAsync();
        await Assertions.Expect(Page.Locator("h1.page-h1")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task Features_ShouldShowTheSubscriptionsInTheSidebar()
    {
        await Page.GotoRelativeAsync(PageUrl);

        var tiers = Page.Locator("aside.sidebar .features-tier-card");

        await Assertions.Expect(Page.Locator("aside.sidebar .features-tiers-sidebar h2")).ToHaveTextAsync("Subscriptions");
        (await tiers.CountAsync()).Should().BeGreaterThan(1, "the sidebar lists every subscription tier");
    }

    [Fact]
    public async Task Features_SubscriptionTitles_ShouldLinkToTheOfficialPlansPage()
    {
        await Page.GotoRelativeAsync(PageUrl);

        var links = Page.Locator("aside.sidebar .features-tier-card h3 a");
        await Assertions.Expect(links.First).ToBeVisibleAsync();

        var hrefs = await links.EvaluateAllAsync<string[]>("els => els.map(a => a.href)");
        hrefs.Should().OnlyContain(h => h == "https://github.com/features/copilot/plans");
    }

    [Fact]
    public async Task Features_ShouldShowCardsInAGrid_NotATimeline()
    {
        await Page.GotoRelativeAsync(PageUrl);

        var cards = Page.Locator(".features-cards-grid .feature-card");

        await Assertions.Expect(cards.First).ToBeVisibleAsync();
        await Assertions.Expect(cards.First.Locator("h3")).ToBeVisibleAsync();
        await Assertions.Expect(cards.First.Locator("h3 a")).ToHaveCountAsync(0);
        await Assertions.Expect(Page.Locator(".timeline-entry")).ToHaveCountAsync(0);
    }

    [Fact]
    public async Task Features_WithVideosFilter_ShouldOnlyKeepCardsWithAVideo()
    {
        await Page.GotoRelativeAsync(PageUrl);
        await Page.WaitForBlazorReadyAsync();

        var cards = Page.Locator(".feature-card");
        await Assertions.Expect(cards.First).ToBeVisibleAsync();

        var filter = Page.Locator("button[data-filter='videos']");
        await filter.ClickAndExpectAsync(async () =>
            await Assertions.Expect(filter).ToHaveAttributeAsync("aria-pressed", "true"));

        var withoutVideo = Page.Locator(".feature-card:not(:has(.feature-badge-video))");
        await Assertions.Expect(withoutVideo).ToHaveCountAsync(0);
    }

    [Fact]
    public async Task Features_TextFilter_ShouldHideCardsWithoutTheTypedText_AndKeepTheFilterAfterTheButtons()
    {
        await Page.GotoRelativeAsync(PageUrl);
        await Page.WaitForBlazorReadyAsync();

        var cards = Page.Locator(".feature-card");
        await Assertions.Expect(cards.First).ToBeVisibleAsync();
        var total = await cards.CountAsync();
        var firstName = (await cards.First.Locator("h3").TextContentAsync())!.Trim();
        var input = Page.Locator(".features-filter-input");

        await input.FillAsync(firstName);
        var matching = await cards.Filter(new LocatorFilterOptions { HasText = firstName }).CountAsync();
        await Assertions.Expect(Page.Locator(".feature-card:visible")).ToHaveCountAsync(matching);
        matching.Should().BeLessThanOrEqualTo(total).And.BeGreaterThan(0);

        // A card list that Blazor renders again (the buttons) is filtered as well.
        var withVideos = Page.Locator("button[data-filter='videos']");
        await withVideos.ClickAndExpectAsync(async () =>
            await Assertions.Expect(withVideos).ToHaveAttributeAsync("aria-pressed", "true"));
        (await Page.Locator(".feature-card:visible").AllTextContentsAsync())
            .Should().OnlyContain(t => t.Contains(firstName, StringComparison.OrdinalIgnoreCase));

        await input.FillAsync("zzzqqq-no-such-feature");
        await Assertions.Expect(Page.Locator(".feature-card:visible")).ToHaveCountAsync(0);
        await Assertions.Expect(Page.Locator("#features-text-empty")).ToBeVisibleAsync();

        await input.FillAsync("");
        await Assertions.Expect(Page.Locator("#features-text-empty")).ToBeHiddenAsync();
        await Assertions.Expect(Page.Locator(".feature-card:visible").First).ToBeVisibleAsync();
    }

    [Fact]
    public async Task Features_ShouldBeLinked_FromTheSectionNavigation()
    {
        await Page.GotoRelativeAsync("/github-copilot");
        await Page.WaitForBlazorReadyAsync();

        (await Page.Locator($"a[href='{PageUrl}']").CountAsync()).Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Features_ShouldShowTwoCardsPerRow()
    {
        await Page.GotoRelativeAsync(PageUrl);

        var columns = await Page.Locator(".features-cards-grid").EvaluateAsync<string>("el => getComputedStyle(el).gridTemplateColumns");

        columns.Split(' ', StringSplitOptions.RemoveEmptyEntries).Should().HaveCount(2);
    }

    [Fact]
    public async Task Features_ShouldNotShowTheBillingNotice()
    {
        await Page.GotoRelativeAsync(PageUrl);

        await Assertions.Expect(Page.Locator(".custom-page-billing-notice")).ToHaveCountAsync(0);
        (await Page.ContentAsync()).Should().NotContain("usage-based (token-based) billing");
    }

    [Fact]
    public async Task Features_Subscriptions_ShouldShowAtFullHeightWithoutScrolling()
    {
        await Page.GotoRelativeAsync(PageUrl);

        var tiers = Page.Locator(".features-tiers");
        await Assertions.Expect(tiers).ToBeVisibleAsync();

        var overflowsOrScrolls = await tiers.EvaluateAsync<bool>(
            "el => ['auto', 'scroll'].includes(getComputedStyle(el).overflowY) || el.scrollHeight > el.clientHeight + 1");
        overflowsOrScrolls.Should().BeFalse("the subscription cards flow with the sidebar instead of scrolling in an area of their own");
    }

    [Fact]
    public async Task Features_Cards_ShouldNotBeEmpty()
    {
        await Page.GotoRelativeAsync(PageUrl);
        await Assertions.Expect(Page.Locator(".feature-card").First).ToBeVisibleAsync();

        var names = await Page.Locator(".feature-card:not(:has(.card-description))").Locator("h3").AllTextContentsAsync();

        names.Should().BeEmpty("every feature shows a description, with its event or first article as fallback");
    }

    [Fact]
    public async Task Features_Cards_ShouldAllShowAtLeastOneLinkToContent()
    {
        await Page.GotoRelativeAsync(PageUrl);
        await Assertions.Expect(Page.Locator(".feature-card").First).ToBeVisibleAsync();

        // A card with a video shows it as the thumbnail; every other card lists its content.
        var cardsWithoutContent = await Page.Locator(".feature-card:not(:has(.feature-card-thumbnail)):not(:has(.feature-card-related li a))")
            .Locator("h3").AllTextContentsAsync();

        cardsWithoutContent.Should().BeEmpty("a feature is only shown when it has content, so the content must be visible on its card");
    }

    [Fact]
    public async Task Features_CardWithAVideo_ShouldShowTheTitleAboveThePreview()
    {
        await Page.GotoRelativeAsync(PageUrl);
        await Assertions.Expect(Page.Locator(".feature-card-thumbnail").First).ToBeVisibleAsync();

        var card = Page.Locator(".feature-card:has(.feature-card-thumbnail)").First;
        var title = await card.Locator(".card-title").BoundingBoxAsync();
        var preview = await card.Locator(".feature-card-thumbnail").BoundingBoxAsync();

        title.Should().NotBeNull();
        preview.Should().NotBeNull();
        title!.Y.Should().BeLessThan(preview!.Y, "the title of a card is always at the top, above the video preview");
    }

    [Fact]
    public async Task Features_ContentLinks_OnADesktop_ShouldOpenInANewTab()
    {
        await Page.GotoRelativeAsync(PageUrl);
        await Page.WaitForBlazorReadyAsync();

        // A feature with a video: its thumbnail links to the internal video page of this site.
        var link = Page.Locator(".feature-card-thumbnail").First;
        await Assertions.Expect(link).ToBeVisibleAsync();
        (await link.GetAttributeAsync("data-content-link")).Should().NotBeNull();

        var newTab = await Page.RunAndWaitForPopupAsync(async () => await link.ClickAsync());
        await newTab.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

        newTab.Url.Should().Contain("/videos/");
        Page.Url.Should().EndWith(PageUrl, "closing the new tab brings the reader back to the features page");
    }

    [Fact]
    public async Task Features_ContentLinks_OnATouchDevice_ShouldKeepTheSameTab()
    {
        var phone = await Page.Context.Browser!.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 390, Height = 844 },
            IsMobile = true,
            HasTouch = true,
            IgnoreHTTPSErrors = true,
            Locale = "en-US",
            TimezoneId = "Europe/Brussels"
        });
        await RateLimitGuard.ApplyAsync(phone);

        try
        {
            var phonePage = await phone.NewPageWithDefaultsAsync();
            await phonePage.GotoRelativeAsync(PageUrl);

            (await phonePage.EvaluateAsync<bool>("() => window.matchMedia('(hover: hover) and (pointer: fine)').matches"))
                .Should().BeFalse("a touch device has no mouse or trackpad");

            var link = phonePage.Locator(".feature-card-thumbnail").First;
            await Assertions.Expect(link).ToBeVisibleAsync();
            await link.ClickAsync();

            await Assertions.Expect(phonePage).ToHaveURLAsync(new Regex("/videos/"));
            phone.Pages.Should().HaveCount(1, "no new tab was opened");
        }
        finally
        {
            await phone.CloseAsync();
        }
    }

    [Fact]
    public async Task Features_Cards_ShouldOnlyShowTheHandCursorOnClickableElements()
    {
        await Page.GotoRelativeAsync(PageUrl);
        var card = Page.Locator(".feature-card").First;
        await Assertions.Expect(card).ToBeVisibleAsync();

        (await card.EvaluateAsync<string>("el => getComputedStyle(el).cursor")).Should().Be("default");
        (await card.Locator(".feature-badge").First.EvaluateAsync<string>("el => getComputedStyle(el).cursor")).Should().Be("default");
        (await card.Locator("h3").EvaluateAsync<string>("el => getComputedStyle(el).cursor")).Should().Be("default");
        (await Page.Locator(".feature-card-related a").First.EvaluateAsync<string>("el => getComputedStyle(el).cursor")).Should().Be("pointer");
    }

    [Fact]
    public async Task Features_Description_ShouldNotStretchWhenACardIsTallerBecauseOfItsNeighbour()
    {
        await Page.GotoRelativeAsync(PageUrl);
        await Page.WaitForBlazorReadyAsync();

        // Expanding a card makes the card next to it in the grid taller; its content must not stretch with it.
        var button = Page.Locator(".feature-card-more-btn").First;
        await Assertions.Expect(button).ToBeVisibleAsync();
        await button.ClickAndExpectAsync(async () =>
            await Assertions.Expect(button).ToHaveAttributeAsync("aria-expanded", "true"));

        var largestStretch = await Page.EvaluateAsync<double>(
            """
            () => Math.max(...[...document.querySelectorAll('.feature-card-description')].map(d => {
                const text = document.createRange();
                text.selectNodeContents(d);
                return d.getBoundingClientRect().height - text.getBoundingClientRect().height;
            }))
            """);

        largestStretch.Should().BeLessThan(12, "a description is as high as its text, so the links follow it directly and only the badges sit at the bottom");
    }

    [Fact]
    public async Task Features_MoreButton_ShouldShowTheRestOfTheLinks()
    {
        await Page.GotoRelativeAsync(PageUrl);
        await Page.WaitForBlazorReadyAsync();

        var card = Page.Locator(".feature-card:has(.feature-card-more-btn)").First;
        await Assertions.Expect(card).ToBeVisibleAsync();
        var linksBefore = await card.Locator(".feature-card-related li a").CountAsync();

        var button = card.Locator(".feature-card-more-btn");
        await button.ClickAndExpectAsync(async () =>
            await Assertions.Expect(button).ToHaveAttributeAsync("aria-expanded", "true"));

        (await card.Locator(".feature-card-related li a").CountAsync()).Should().BeGreaterThan(linksBefore);
        (await button.EvaluateAsync<string>("el => getComputedStyle(el).cursor")).Should().Be("pointer");
    }
}
