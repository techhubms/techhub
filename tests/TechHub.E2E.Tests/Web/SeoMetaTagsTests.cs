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
        await WaitForSeoMetaTagsAsync();

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
        await WaitForSeoMetaTagsAsync();

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
        await WaitForSeoMetaTagsAsync();

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
        await WaitForSeoMetaTagsAsync();

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
        await WaitForSeoMetaTagsAsync();

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
        await WaitForSeoMetaTagsAsync();

        // Assert
        var ogType = await GetMetaContentAsync("property", "og:type");
        ogType.Should().Be("website");
    }

    [Fact]
    public async Task SectionPage_HasCollectionPageJsonLd()
    {
        // Act
        await Page.GotoRelativeAsync("/github-copilot");
        await WaitForSeoMetaTagsAsync();

        // Assert
        var jsonLd = await GetJsonLdContentAsync("CollectionPage");
        jsonLd.Should().NotBeNullOrEmpty("CollectionPage JSON-LD schema should be present on section pages");
    }

    [Fact]
    public async Task SectionPage_HasBreadcrumbJsonLd()
    {
        // Act
        await Page.GotoRelativeAsync("/github-copilot");
        await WaitForSeoMetaTagsAsync();

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
    /// Waits for SeoMetaTags HeadContent to be fully rendered in the document head (including JSON-LD).
    /// After Blazor rehydration, HeadContent may briefly be absent; polling avoids flaky failures on slow CI runners.
    /// </summary>
    private Task WaitForSeoMetaTagsAsync() =>
        Page.WaitForConditionAsync(
            "() => document.head.querySelector(\"meta[name='description']\") !== null && document.head.querySelector(\"script[type='application/ld+json']\") !== null");

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

        // Wait for the article content to be visible before asserting head content
        await Page.AssertElementVisibleBySelectorAsync("main article");

        // Wait for SeoMetaTags HeadContent to fully render (including JSON-LD scripts at the end).
        // After Blazor rehydration the HeadContent may briefly be absent; polling avoids flaky failures on slow CI.
        await Page.WaitForConditionAsync(
            "() => document.head.querySelector(\"meta[name='description']\") !== null && document.head.querySelector(\"script[type='application/ld+json']\") !== null");

        // Wait for PageTitle to render (separate from HeadContent, may update at different times)
        await Page.WaitForConditionAsync(
            "() => document.title !== ''");

        return firstCardHref!;
    }
}
