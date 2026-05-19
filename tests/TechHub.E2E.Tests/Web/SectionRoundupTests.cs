using System.Text.RegularExpressions;
using System.Xml.Linq;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for the per-section roundup feature.
/// Verifies that roundups are accessible at /{section}/roundups/... and that
/// /all/roundups shows the full multi-section aggregate.
/// </summary>
public class SectionRoundupTests : PlaywrightTestBase
{
    public SectionRoundupTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    private static string BaseUrl => BlazorHelpers.BaseUrl;

    // ── Section roundup pages ────────────────────────────────────────────────

    [Theory]
    [InlineData("github-copilot")]
    [InlineData("ai")]
    public async Task SectionRoundupsPage_IsAccessibleAndShowsItems(string sectionName)
    {
        // Act
        await Page.GotoAndWaitForBlazorAsync($"{BaseUrl}/{sectionName}/roundups");

        // Assert — page loaded without being redirected to /not-found
        Page.Url.Should().NotContain("/not-found", $"/{sectionName}/roundups must be a valid page");

        // Should show at least one content card
        await Page.Locator(".card").First.AssertElementVisibleAsync();
    }

    [Theory]
    [InlineData("github-copilot")]
    [InlineData("ai")]
    public async Task SectionRoundupsPage_CardLinks_PointToSectionRoundupPath(string sectionName)
    {
        // Arrange
        await Page.GotoAndWaitForBlazorAsync($"{BaseUrl}/{sectionName}/roundups");
        await Page.Locator(".card").First.AssertElementVisibleAsync();

        // Act — find the first card with an internal href
        var cards = Page.Locator(".card");
        var cardCount = await cards.CountAsync();
        string? firstCardHref = null;

        for (int i = 0; i < cardCount; i++)
        {
            var href = await cards.Nth(i).Locator(".card-link").GetHrefAsync();
            if (href?.StartsWith("/") == true)
            {
                firstCardHref = href;
                break;
            }
        }

        // Assert — link should include the section and roundups in path
        firstCardHref.Should().NotBeNullOrEmpty("should find at least one roundup card");
        firstCardHref.Should().MatchRegex(
            @"^/[a-z\-]+/roundups/",
            $"roundup card href should be /{sectionName}/roundups/{{slug}}");
    }

    [Theory]
    [InlineData("github-copilot")]
    [InlineData("ai")]
    public async Task SectionRoundupDetailPage_URL_ContainsSectionAndRoundups(string sectionName)
    {
        // Arrange — navigate to section roundups list and find first roundup card
        await Page.GotoAndWaitForBlazorAsync($"{BaseUrl}/{sectionName}/roundups");
        await Page.Locator(".card").First.AssertElementVisibleAsync();

        var cards = Page.Locator(".card");
        var cardCount = await cards.CountAsync();
        string? firstCardHref = null;

        for (int i = 0; i < cardCount; i++)
        {
            var href = await cards.Nth(i).Locator(".card-link").GetHrefAsync();
            if (href?.StartsWith("/") == true)
            {
                firstCardHref = href;
                break;
            }
        }

        firstCardHref.Should().NotBeNullOrEmpty();

        // Act — navigate to the roundup detail page
        await Page.GotoAndWaitForBlazorAsync($"{BaseUrl}{firstCardHref}");

        // Assert — URL must include the section (not /all/) and roundups
        Page.Url.Should().Contain("/roundups/", "detail page URL should include 'roundups'");
        Page.Url.Should().NotContain("/all/roundups/", "roundup detail pages should no longer live under /all/roundups");

        // Should render actual article content
        await Page.AssertElementVisibleBySelectorAsync("main article");
    }

    // ── RSS feeds for section roundups ───────────────────────────────────────

    [Theory]
    [InlineData("github-copilot")]
    [InlineData("ai")]
    public async Task SectionRoundupFeed_ReturnsValidRss(string sectionName)
    {
        // Act
        var response = await Page.APIGetAsync($"{BaseUrl}/{sectionName}/roundups/feed.xml");

        // Assert
        response.Status.Should().Be(200, $"/{sectionName}/roundups/feed.xml must return 200");
        response.Headers["content-type"].Should().Contain("application/rss+xml");

        var xmlContent = await response.TextAsync();
        var doc = XDocument.Parse(xmlContent);

        var rss = doc.Element("rss");
        rss.Should().NotBeNull();
        rss!.Attribute("version")?.Value.Should().Be("2.0");

        var channel = rss.Element("channel");
        channel.Should().NotBeNull();
        channel!.Element("title").Should().NotBeNull();
        channel.Element("link").Should().NotBeNull();
        channel.Element("description").Should().NotBeNull();
    }

    // ── /all/roundups is the aggregate page for all roundups ──────────────────

    [Fact]
    public async Task AllRoundupsPage_IsAccessibleAndShowsItems()
    {
        // Act — /all/roundups is the aggregate collection showing roundups from all sections.
        await Page.GotoAndWaitForBlazorAsync($"{BaseUrl}/all/roundups");

        // Assert — page loaded without being redirected to /not-found
        Page.Url.Should().NotContain("/not-found", "/all/roundups must be a valid page");

        // Should show at least one content card
        await Page.Locator(".card").First.AssertElementVisibleAsync();
    }

    // ── Sitemap contains section roundup URLs ────────────────────────────────

    [Fact]
    public async Task Sitemap_RoundupUrls_UsesSectionPaths()
    {
        // Arrange — fetch the live sitemap
        var sitemapResponse = await Context.APIRequest.GetAsync($"{BaseUrl}/sitemap.xml");
        sitemapResponse.Status.Should().Be(200);

        var xml = await sitemapResponse.TextAsync();
        var doc = XDocument.Parse(xml);
        XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";

        var roundupUrls = doc.Descendants(ns + "loc")
            .Select(e => e.Value)
            .Where(url => url.Contains("/roundups/"))
            .ToList();

        // Assert — every roundup URL in sitemap must match either /all/roundups/{slug}
        // (the full multi-section digest) or /{section}/roundups/{slug} (section-specific).
        roundupUrls.Should().NotBeEmpty("sitemap should contain at least one roundup URL");

        foreach (var url in roundupUrls)
        {
            var path = new Uri(url).AbsolutePath;
            path.Should().MatchRegex(
                @"^/[a-z\-]+/roundups/[^/]+$",
                $"sitemap roundup URL {url} must match /{{section}}/roundups/{{slug}}");
        }
    }
}
