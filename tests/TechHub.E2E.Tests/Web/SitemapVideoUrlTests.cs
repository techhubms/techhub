using System.Text.RegularExpressions;
using System.Xml.Linq;
using FluentAssertions;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E test that pulls a real video URL from the live <c>/sitemap.xml</c> and verifies
/// it is reachable (returns 200 and is not the <c>/not-found</c> page).
///
/// This guards against a class of regressions where a URL ends up in the sitemap
/// (and therefore in Google) but actually 404s on the site — see slug-and-content-layout-cleanup.
/// </summary>
public class SitemapVideoUrlTests : PlaywrightTestBase
{
    public SitemapVideoUrlTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    private static readonly XNamespace _sitemapNs = "http://www.sitemaps.org/schemas/sitemap/0.9";
    private static readonly Regex _videoUrlPattern = new(@"/[^/]+/videos/[^/?#]+$", RegexOptions.Compiled);

    [Fact]
    public async Task Sitemap_VideoUrls_AreAccessibleAndNotNotFound()
    {
        // Arrange — fetch the live sitemap via the same browser context (handles HTTPS / cookies).
        var sitemapResponse = await Context.APIRequest.GetAsync($"{BlazorHelpers.BaseUrl}/sitemap.xml");
        sitemapResponse.Status.Should().Be(200, "/sitemap.xml must be reachable");

        var xml = await sitemapResponse.TextAsync();
        var doc = XDocument.Parse(xml);
        var videoUrls = doc.Descendants(_sitemapNs + "loc")
            .Select(e => e.Value)
            .Where(url => _videoUrlPattern.IsMatch(new Uri(url).AbsolutePath))
            .ToList();

        videoUrls.Should().NotBeEmpty("sitemap should advertise at least one video detail URL");

        // Pick a deterministic sample (first + middle + last) so the test stays fast but covers
        // a few different slugs across the dataset.
        var sample = new[]
        {
            videoUrls[0],
            videoUrls[videoUrls.Count / 2],
            videoUrls[^1],
        }.Distinct().ToList();

        // Act + Assert — each sampled URL must load successfully and not redirect to /not-found.
        foreach (var videoUrl in sample)
        {
            var path = new Uri(videoUrl).PathAndQuery;
            var response = await Page.GotoAsync($"{BlazorHelpers.BaseUrl}{path}");

            response.Should().NotBeNull($"{path} should produce an HTTP response");
            response!.Status.Should().Be(200, $"{path} is in sitemap.xml so it must return 200");

            Page.Url.Should().NotContain("/not-found", $"{path} must not redirect to the not-found page");

            // The video detail page renders a <main article> — same marker existing tests use.
            await Page.AssertElementVisibleBySelectorAsync("main article");
        }
    }
}
