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
        twitterCard.Should().Be("summary");

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

        // Assert
        var pageTitle = await Page.TitleAsync();
        var ogTitle = await GetMetaContentAsync("property", "og:title");

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
        twitterCard.Should().Be("summary");
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
    /// Waits for SeoMetaTags HeadContent to be fully rendered for a specific page path.
    /// The page-path meta tag is the FIRST element in the HeadContent block — when it
    /// contains the expected path, all other SEO tags in that same block are guaranteed
    /// to be present and belong to the correct page. This eliminates races with stale
    /// head content from a previous page during Blazor enhanced navigation.
    /// </summary>
    private Task WaitForSeoMetaTagsAsync(string expectedPath) =>
        Page.WaitForConditionAsync(
            "(path) => document.head.querySelector(\"meta[name='page-path']\")?.content === path",
            expectedPath);

    private async Task<string?> GetMetaContentAsync(string attributeName, string attributeValue)
    {
        var content = await Page.EvaluateAsync<string?>(
            @"([attrName, attrValue]) => {
                const el = document.head.querySelector(`meta[${attrName}='${attrValue}']`);
                return el ? el.getAttribute('content') : null;
            }",
            new[] { attributeName, attributeValue });
        return content;
    }

    private async Task<string?> GetJsonLdContentAsync(string schemaType)
    {
        var content = await Page.EvaluateAsync<string?>(
            @"(type) => {
                const scripts = Array.from(document.head.querySelectorAll('script[type=""application/ld+json""]'));
                const match = scripts.find(s => {
                    try { return JSON.parse(s.textContent)['@type'] === type; }
                    catch { return false; }
                });
                return match ? match.textContent : null;
            }",
            schemaType);
        return content;
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
