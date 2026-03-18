using System.Text.Json;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests verifying SEO meta tags are correctly rendered in the HTML head for all page types.
/// These tests validate FR-2, FR-3, FR-4, FR-5, FR-8, FR-10 from the SEO spec.
/// </summary>
public class SeoMetaTagsTests : PlaywrightTestBase
{
    public SeoMetaTagsTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    // ────────────────────────────────────────────────────────────────────────
    // Home page
    // ────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task HomePage_HasMetaDescription()
    {
        // Act
        await Page.GotoRelativeAsync("/");
        await WaitForSeoMetaTagsAsync("/");

        // Assert
        var description = await GetMetaContentAsync("name", "description");
        description.Should().NotBeNullOrEmpty();
        description!.Length.Should().BeLessThanOrEqualTo(160);
    }

    [Fact]
    public async Task HomePage_HasOpenGraphTags()
    {
        // Act
        await Page.GotoRelativeAsync("/");
        await WaitForSeoMetaTagsAsync("/");

        // Assert
        var ogType = await GetMetaContentAsync("property", "og:type");
        ogType.Should().Be("website");

        var ogTitle = await GetMetaContentAsync("property", "og:title");
        ogTitle.Should().NotBeNullOrEmpty();

        var ogDescription = await GetMetaContentAsync("property", "og:description");
        ogDescription.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task HomePage_HasTwitterCardTags()
    {
        // Act
        await Page.GotoRelativeAsync("/");
        await WaitForSeoMetaTagsAsync("/");

        // Assert
        var twitterCard = await GetMetaContentAsync("name", "twitter:card");
        twitterCard.Should().Be("summary_large_image");

        var twitterTitle = await GetMetaContentAsync("name", "twitter:title");
        twitterTitle.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task HomePage_HasWebSiteJsonLd()
    {
        // Act
        await Page.GotoRelativeAsync("/");
        await WaitForSeoMetaTagsAsync("/");

        // Assert
        var jsonLd = await GetJsonLdContentAsync("WebSite");
        jsonLd.Should().NotBeNullOrEmpty("WebSite JSON-LD schema should be present on home page");
    }

    // ────────────────────────────────────────────────────────────────────────
    // Section/collection page
    // ────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task SectionPage_HasMetaDescription()
    {
        // Act
        await Page.GotoRelativeAsync("/github-copilot");
        await WaitForSeoMetaTagsAsync("/github-copilot");

        // Assert
        var description = await GetMetaContentAsync("name", "description");
        description.Should().NotBeNullOrEmpty();
        description!.Length.Should().BeLessThanOrEqualTo(160);
    }

    [Fact]
    public async Task SectionPage_HasOpenGraphWebsiteType()
    {
        // Act
        await Page.GotoRelativeAsync("/github-copilot");
        await WaitForSeoMetaTagsAsync("/github-copilot");

        // Assert
        var ogType = await GetMetaContentAsync("property", "og:type");
        ogType.Should().Be("website");
    }

    [Fact]
    public async Task SectionPage_HasCollectionPageJsonLd()
    {
        // Act
        await Page.GotoRelativeAsync("/github-copilot");
        await WaitForSeoMetaTagsAsync("/github-copilot");

        // Assert
        var jsonLd = await GetJsonLdContentAsync("CollectionPage");
        jsonLd.Should().NotBeNullOrEmpty("CollectionPage JSON-LD schema should be present on section pages");
    }

    [Fact]
    public async Task SectionPage_HasBreadcrumbJsonLd()
    {
        // Act
        await Page.GotoRelativeAsync("/github-copilot");
        await WaitForSeoMetaTagsAsync("/github-copilot");

        // Assert
        var jsonLd = await GetJsonLdContentAsync("BreadcrumbList");
        jsonLd.Should().NotBeNullOrEmpty("BreadcrumbList JSON-LD schema should be present on section pages");
    }

    // ────────────────────────────────────────────────────────────────────────
    // Content detail page (roundup — these are internal pages)
    // ────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task ContentDetailPage_HasMetaDescription()
    {
        // Act
        var detailUrl = await NavigateToFirstInternalContentAsync("/all/roundups");

        // Assert
        var description = await GetMetaContentAsync("name", "description");
        description.Should().NotBeNullOrEmpty($"content page at {detailUrl} should have meta description");
        description!.Length.Should().BeLessThanOrEqualTo(160);
    }

    [Fact]
    public async Task ContentDetailPage_HasArticleOpenGraphType()
    {
        // Act
        await NavigateToFirstInternalContentAsync("/all/roundups");

        // Assert
        var ogType = await GetMetaContentAsync("property", "og:type");
        ogType.Should().Be("article");
    }

    [Fact]
    public async Task ContentDetailPage_HasOpenGraphTitleMatchingPageTitle()
    {
        // Act
        await NavigateToFirstInternalContentAsync("/all/roundups");

        // Assert - Read og:title and document.title atomically in a single JS evaluation.
        // Reading them in two separate calls (GetMetaContentAsync then TitleAsync) creates a
        // window where Blazor HeadContent rehydration can temporarily clear og:title between
        // the two reads, making the second read return null even though the first succeeded.
        // WaitForFunctionAsync retries until og:title is non-null, then returns both values
        // atomically from the same JS execution context — guaranteed consistent.
        var handle = await Page.WaitForFunctionAsync(
            @"() => {
                const ogTitleEl = document.head.querySelector(""meta[property='og:title']"");
                const ogTitle = ogTitleEl?.content ?? null;
                if (!ogTitle) return null;
                return { OgTitle: ogTitle, PageTitle: document.title };
            }",
            null,
            new Microsoft.Playwright.PageWaitForFunctionOptions
            {
                Timeout = BlazorHelpers.DefaultTimeout,
                PollingInterval = BlazorHelpers.DefaultPollingInterval
            });
        var result = await handle.JsonValueAsync<JsonElement>();

        var ogTitle = result.GetProperty("OgTitle").GetString();
        var pageTitle = result.GetProperty("PageTitle").GetString();
        ogTitle.Should().NotBeNullOrEmpty();
        pageTitle.Should().Contain(ogTitle!, "og:title should match the page title");
    }

    [Fact]
    public async Task ContentDetailPage_HasArticleJsonLd()
    {
        // Act
        await NavigateToFirstInternalContentAsync("/all/roundups");

        // Assert
        var jsonLd = await GetJsonLdContentAsync("Article");
        jsonLd.Should().NotBeNullOrEmpty("Article JSON-LD schema should be present on content detail pages");
    }

    [Fact]
    public async Task ContentDetailPage_HasBreadcrumbJsonLd()
    {
        // Act
        await NavigateToFirstInternalContentAsync("/all/roundups");

        // Assert
        var jsonLd = await GetJsonLdContentAsync("BreadcrumbList");
        jsonLd.Should().NotBeNullOrEmpty("BreadcrumbList JSON-LD schema should be present on content detail pages");
    }

    [Fact]
    public async Task ContentDetailPage_HasTwitterCardSummary()
    {
        // Act
        await NavigateToFirstInternalContentAsync("/all/roundups");

        // Assert
        var twitterCard = await GetMetaContentAsync("name", "twitter:card");
        twitterCard.Should().Be("summary_large_image");
    }

    [Fact]
    public async Task ContentDetailPage_ArticlePublishedTimePresent()
    {
        // Act
        await NavigateToFirstInternalContentAsync("/all/roundups");

        // Assert
        var publishedTime = await GetMetaContentAsync("property", "article:published_time");
        publishedTime.Should().NotBeNullOrEmpty("article:published_time should be present for article pages");
    }

    // ────────────────────────────────────────────────────────────────────────
    // Helpers
    // ────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Waits until ALL essential SEO tags are simultaneously present for the given page path.
    ///
    /// Checks page-path, description, og:title, and a JSON-LD script together in a single
    /// JavaScript evaluation. This prevents the method from returning during a transient
    /// state caused by Blazor HeadContent rehydration: after the Blazor circuit connects,
    /// HeadContent is briefly cleared and then re-rendered. Checking only page-path could
    /// succeed during the initial SSR phase, allowing subsequent reads to catch the
    /// rehydration gap. Requiring all essential tags to be simultaneously present ensures
    /// the method only returns once HeadContent is fully and stably rendered.
    /// </summary>
    private Task WaitForSeoMetaTagsAsync(string expectedPath) =>
        Page.WaitForConditionAsync(
            @"(path) => {
                const h = document.head;
                const pagePath = h.querySelector(""meta[name='page-path']"");
                const description = h.querySelector(""meta[name='description']"");
                const ogTitle = h.querySelector(""meta[property='og:title']"");
                const jsonLd = h.querySelector(""script[type='application/ld+json']"");
                return pagePath?.content === path &&
                       description?.content?.length > 0 &&
                       ogTitle?.content?.length > 0 &&
                       jsonLd !== null;
            }",
            expectedPath);

    /// <summary>
    /// Reads a meta tag's content attribute, retrying through Blazor HeadContent rehydration gaps.
    /// Returns null only if the tag is genuinely absent after the full timeout period.
    /// </summary>
    private async Task<string?> GetMetaContentAsync(string attributeName, string attributeValue)
    {
        try
        {
            var handle = await Page.WaitForFunctionAsync(
                @"([attrName, attrValue]) => {
                    const el = document.head.querySelector(`meta[${attrName}='${attrValue}']`);
                    return el ? el.getAttribute('content') : null;
                }",
                new[] { attributeName, attributeValue },
                new PageWaitForFunctionOptions { Timeout = BlazorHelpers.DefaultTimeout, PollingInterval = BlazorHelpers.DefaultPollingInterval });
            return await handle.JsonValueAsync<string?>();
        }
        catch (TimeoutException)
        {
            return null;
        }
    }

    /// <summary>
    /// Reads a JSON-LD script's content by @type, retrying through Blazor HeadContent rehydration gaps.
    /// Returns null only if the schema type is genuinely absent after the full timeout period.
    /// </summary>
    private async Task<string?> GetJsonLdContentAsync(string schemaType)
    {
        try
        {
            var handle = await Page.WaitForFunctionAsync(
                @"(type) => {
                    const scripts = Array.from(document.head.querySelectorAll('script[type=""application/ld+json""]'));
                    const match = scripts.find(s => {
                        try { return JSON.parse(s.textContent)['@type'] === type; }
                        catch { return false; }
                    });
                    return match ? match.textContent : null;
                }",
                schemaType,
                new PageWaitForFunctionOptions { Timeout = BlazorHelpers.DefaultTimeout, PollingInterval = BlazorHelpers.DefaultPollingInterval });
            return await handle.JsonValueAsync<string?>();
        }
        catch (TimeoutException)
        {
            return null;
        }
    }

    /// <summary>
    /// Navigates to a list page, finds the first card with an internal href, and navigates to that detail page.
    /// Returns the URL that was navigated to.
    /// </summary>
    private async Task<string> NavigateToFirstInternalContentAsync(string listUrl)
    {
        await Page.GotoRelativeAsync(listUrl);

        // Wait for cards to appear
        await Page.Locator(".card").First.AssertElementVisibleAsync();

        var cards = Page.Locator(".card");
        var cardCount = await cards.CountAsync();
        string? firstCardHref = null;

        for (var i = 0; i < cardCount; i++)
        {
            var href = await cards.Nth(i).Locator(".card-link").GetHrefAsync();
            if (href?.StartsWith("/") == true)
            {
                firstCardHref = href;
                break;
            }
        }

        firstCardHref.Should().NotBeNullOrEmpty("should find at least one card with an internal href");

        await Page.GotoAndWaitForBlazorAsync($"{BlazorHelpers.BaseUrl}{firstCardHref}");

        // Wait for the DETAIL page's SeoMetaTags to render by checking page-path.
        // The page-path meta is the first element in the HeadContent block — when it
        // matches the detail page's path, all other SEO tags are guaranteed present.
        await WaitForSeoMetaTagsAsync(firstCardHref!);

        return firstCardHref!;
    }
}
