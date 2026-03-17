using System.Net;
using System.Xml.Linq;
using FluentAssertions;
using TechHub.TestUtilities;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for the sitemap endpoint.
/// </summary>
public class SitemapEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;

    private static readonly XNamespace SitemapNs = "http://www.sitemaps.org/schemas/sitemap/0.9";

    public SitemapEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetSitemap_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/sitemap", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetSitemap_ReturnsXmlContentType()
    {
        var response = await _client.GetAsync("/api/sitemap", TestContext.Current.CancellationToken);

        response.Content.Headers.ContentType?.MediaType.Should().Be("application/xml");
    }

    [Fact]
    public async Task GetSitemap_ReturnsValidSitemapXml()
    {
        var response = await _client.GetAsync("/api/sitemap", TestContext.Current.CancellationToken);
        var xml = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        var doc = XDocument.Parse(xml);
        doc.Root.Should().NotBeNull();
        doc.Root!.Name.Should().Be(SitemapNs + "urlset");
    }

    [Fact]
    public async Task GetSitemap_ContainsHomepage()
    {
        var response = await _client.GetAsync("/api/sitemap", TestContext.Current.CancellationToken);
        var xml = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        var doc = XDocument.Parse(xml);
        var locs = doc.Descendants(SitemapNs + "loc").Select(e => e.Value).ToList();

        locs.Should().Contain(l => l.EndsWith("/"), "homepage should be in the sitemap");
    }

    [Fact]
    public async Task GetSitemap_ContainsSectionPages()
    {
        var response = await _client.GetAsync("/api/sitemap", TestContext.Current.CancellationToken);
        var xml = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        var doc = XDocument.Parse(xml);
        var locs = doc.Descendants(SitemapNs + "loc").Select(e => e.Value).ToList();

        locs.Should().Contain(l => l.Contains("/ai"), "AI section should be in the sitemap");
        locs.Should().Contain(l => l.Contains("/github-copilot"), "GitHub Copilot section should be in the sitemap");
    }

    [Fact]
    public async Task GetSitemap_UrlElementsHaveRequiredChildren()
    {
        var response = await _client.GetAsync("/api/sitemap", TestContext.Current.CancellationToken);
        var xml = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        var doc = XDocument.Parse(xml);
        var urls = doc.Descendants(SitemapNs + "url").ToList();

        urls.Should().NotBeEmpty();

        foreach (var url in urls)
        {
            url.Element(SitemapNs + "loc").Should().NotBeNull("every <url> must have a <loc>");
        }
    }

    [Fact]
    public async Task GetSitemap_DoesNotContainExternalOnlyCollectionPages()
    {
        // news/blogs/community items only link externally — they have no detail page on our site.
        var response = await _client.GetAsync("/api/sitemap", TestContext.Current.CancellationToken);
        var xml = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        var doc = XDocument.Parse(xml);
        var locs = doc.Descendants(SitemapNs + "loc").Select(e => e.Value).ToList();

        // Individual content URLs from "news" collection should not appear (they're all external links)
        locs.Should().NotContain(
            l => System.Text.RegularExpressions.Regex.IsMatch(l, @"/[^/]+/news/[^/]+$"),
            "news items link to external sources and have no detail page on this site");

        locs.Should().NotContain(
            l => System.Text.RegularExpressions.Regex.IsMatch(l, @"/[^/]+/blogs/[^/]+$"),
            "blog items link to external sources and have no detail page on this site");

        locs.Should().NotContain(
            l => System.Text.RegularExpressions.Regex.IsMatch(l, @"/[^/]+/community/[^/]+$"),
            "community items link to external sources and have no detail page on this site");
    }

    [Fact]
    public async Task GetSitemap_ContainsInternalContentPages()
    {
        // Videos and roundups have actual pages on our site.
        var response = await _client.GetAsync("/api/sitemap", TestContext.Current.CancellationToken);
        var xml = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        var doc = XDocument.Parse(xml);
        var locs = doc.Descendants(SitemapNs + "loc").Select(e => e.Value).ToList();

        locs.Should().Contain(
            l => System.Text.RegularExpressions.Regex.IsMatch(l, @"/[^/]+/videos/[^/]+$"),
            "video items have real detail pages and should appear in the sitemap");
    }
}
