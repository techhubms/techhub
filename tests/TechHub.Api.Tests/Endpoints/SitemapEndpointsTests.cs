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

    private static readonly XNamespace _sitemapNs = "http://www.sitemaps.org/schemas/sitemap/0.9";

    public SitemapEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetSitemap_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/sitemap", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetSitemap_ReturnsXmlContentType()
    {
        // Act
        var response = await _client.GetAsync("/api/sitemap", TestContext.Current.CancellationToken);

        // Assert
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/xml");
    }

    [Fact]
    public async Task GetSitemap_ReturnsValidSitemapXml()
    {
        // Act
        var response = await _client.GetAsync("/api/sitemap", TestContext.Current.CancellationToken);
        var xml = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        // Assert
        var doc = XDocument.Parse(xml);
        doc.Root.Should().NotBeNull();
        doc.Root!.Name.Should().Be(_sitemapNs + "urlset");
    }

    [Fact]
    public async Task GetSitemap_ContainsHomepage()
    {
        // Act
        var response = await _client.GetAsync("/api/sitemap", TestContext.Current.CancellationToken);
        var xml = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var locs = XDocument.Parse(xml).Descendants(_sitemapNs + "loc").Select(e => e.Value).ToList();

        // Assert
        locs.Should().Contain(l => l.EndsWith("/"), "homepage should be in the sitemap");
    }

    [Fact]
    public async Task GetSitemap_ContainsSectionPages()
    {
        // Act
        var response = await _client.GetAsync("/api/sitemap", TestContext.Current.CancellationToken);
        var xml = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var locs = XDocument.Parse(xml).Descendants(_sitemapNs + "loc").Select(e => e.Value).ToList();

        // Assert
        locs.Should().Contain(l => l.Contains("/ai"), "AI section should be in the sitemap");
        locs.Should().Contain(l => l.Contains("/github-copilot"), "GitHub Copilot section should be in the sitemap");
    }

    [Fact]
    public async Task GetSitemap_UrlElementsHaveRequiredChildren()
    {
        // Act
        var response = await _client.GetAsync("/api/sitemap", TestContext.Current.CancellationToken);
        var xml = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var urls = XDocument.Parse(xml).Descendants(_sitemapNs + "url").ToList();

        // Assert
        urls.Should().NotBeEmpty();
        foreach (var url in urls)
        {
            url.Element(_sitemapNs + "loc").Should().NotBeNull("every <url> must have a <loc>");
        }
    }

    [Fact]
    public async Task GetSitemap_DoesNotContainExternalOnlyCollectionPages()
    {
        // news/blogs/community items only link externally — they have no detail page on our site.

        // Act
        var response = await _client.GetAsync("/api/sitemap", TestContext.Current.CancellationToken);
        var xml = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var locs = XDocument.Parse(xml).Descendants(_sitemapNs + "loc").Select(e => e.Value).ToList();

        // Assert
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

        // Act
        var response = await _client.GetAsync("/api/sitemap", TestContext.Current.CancellationToken);
        var xml = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var locs = XDocument.Parse(xml).Descendants(_sitemapNs + "loc").Select(e => e.Value).ToList();

        // Assert
        locs.Should().Contain(
            l => System.Text.RegularExpressions.Regex.IsMatch(l, @"/[^/]+/videos/[^/]+$"),
            "video items have real detail pages and should appear in the sitemap");
    }
}
