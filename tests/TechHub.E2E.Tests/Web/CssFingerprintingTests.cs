using System.Text.RegularExpressions;
using FluentAssertions;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests verifying CSS files use fingerprinted URLs for reliable cache busting.
/// Fingerprinted URLs contain a content hash in the filename (e.g., base.r2lq8zdogi.css),
/// ensuring browsers automatically fetch new versions after deployments.
/// </summary>
public partial class CssFingerprintingTests : PlaywrightTestBase
{
    public CssFingerprintingTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task CssLinks_ShouldUse_FingerprintedUrls()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync("/");

        // Get all CSS stylesheet links from the page
        var cssLinks = await Page.Locator("link[rel='stylesheet']").AllAsync();
        var cssHrefs = new List<string>();
        foreach (var link in cssLinks)
        {
            var href = await link.GetAttributeAsync("href");
            if (href != null)
            {
                cssHrefs.Add(href);
            }
        }

        // Assert - Should have CSS links
        cssHrefs.Should().NotBeEmpty("page should have CSS stylesheet links");

        // All CSS links should use fingerprinted URLs (content hash in filename)
        // Fingerprinted pattern: filename.{10+ char hash}.css (e.g., base.r2lq8zdogi.css)
        // The component-scoped styles file also counts (TechHub.Web.styles.css → TechHub.Web.r2lq8zdogi.styles.css)
        var fingerprintPattern = FingerprintRegex();

        foreach (var href in cssHrefs)
        {
            href.Should().MatchRegex(fingerprintPattern,
                $"CSS link '{href}' should use a fingerprinted URL for cache busting. " +
                "Non-fingerprinted CSS causes stale cache issues after deployments.");
        }
    }

    [Fact]
    public async Task CssLinks_ShouldNotUse_QueryStringCacheBusting()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync("/");

        // Get all CSS stylesheet links
        var cssLinks = await Page.Locator("link[rel='stylesheet']").AllAsync();

        // Assert - No CSS links should use query string cache busting (?v=xxx)
        // Query string cache busting is unreliable (CDNs/proxies may ignore query strings)
        foreach (var link in cssLinks)
        {
            var href = await link.GetAttributeAsync("href");
            href?.Should().NotContain("?v=",
                    $"CSS link '{href}' should not use query string cache busting. " +
                    "Use filename-based fingerprinting instead (more reliable).");
        }
    }

    [Fact]
    public async Task CssLinks_ShouldNotReference_BundleCss()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync("/");

        // Get all CSS stylesheet links
        var cssLinks = await Page.Locator("link[rel='stylesheet']").AllAsync();

        // Assert - No CSS link should reference bundle.css (we use individual fingerprinted files)
        foreach (var link in cssLinks)
        {
            var href = await link.GetAttributeAsync("href");
            href?.Should().NotContain("bundle.css",
                    "Should not use a CSS bundle. Individual fingerprinted files provide " +
                    "reliable cache busting and HTTP/2 makes bundling unnecessary.");
        }
    }

    /// <summary>
    /// Regex pattern matching fingerprinted CSS filenames.
    /// Matches patterns like: base.r2lq8zdogi.css, TechHub.Web.r2lq8zdogi.styles.css
    /// The fingerprint is a segment of 10+ lowercase alphanumeric characters.
    /// </summary>
    [GeneratedRegex(@"\.[a-z0-9]{10,}\.")]
    private static partial Regex FingerprintRegex();
}
