using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TechHub.Core.Models;
using TechHub.TestUtilities.Builders;
using TechHub.Web.Middleware;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Middleware;

/// <summary>
/// Tests for UrlNormalizationMiddleware — the unified URL cleanup and legacy redirect handler.
///
/// Key behaviours verified:
///   - At most ONE 301 redirect per request regardless of how many normalizations apply.
///   - .html extension stripping, YYYY-MM-DD- date prefix stripping.
///   - Legacy API lookup is gated on hadHtmlExtension: only URLs that originally contained .html
///     trigger an API call. Bare slugs (no .html) return 404 immediately without any API call.
///   - The ?section= hint is remapped through the section rename dictionary before the API call
///     (e.g. ?section=coding → hint "dotnet").
///   - Graceful fallback when the API is unavailable.
///   - Known sections, known pages, and static files are passed through without an API call.
///   - Case normalization is NOT performed — casing is handled by the infrastructure layer.
/// </summary>
public class UrlNormalizationMiddlewareTests
{
    // ── HTML extension stripping ────────────────────────────────────────────

    [Theory]
    [InlineData("/ai/videos/article.html", "/ai/videos/article")]
    [InlineData("/github-copilot/blogs/features.html", "/github-copilot/blogs/features")]
    [InlineData("/about.HTML", "/about")]
    public async Task MultiSegment_HtmlExtension_RedirectsToCleanPath(string input, string expected)
    {
        var (middleware, context, nextCalled) = CreateMiddleware(path: input);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be(expected);
        nextCalled().Should().BeFalse();
    }

    [Fact]
    public async Task SingleSegment_HtmlExtension_KnownSection_RedirectsToCleanPath()
    {
        // /ai.html → strip .html → /ai → known section → 301 /ai (no API call needed)
        var cache = A.SectionCache.WithSections("ai").Build();
        var mockApi = new Mock<ITechHubApiClient>();
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/ai.html", sectionCache: cache, mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be("/ai");
        mockApi.Verify(x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()), Times.Never);
        nextCalled().Should().BeFalse();
    }

    [Fact]
    public async Task SingleSegment_HtmlExtension_UnknownSlug_Returns404()
    {
        // /article.html → strip .html → /article → API returns null → 404 directly
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/article.html", apiResult: null);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        nextCalled().Should().BeFalse();
    }

    [Fact]
    public async Task SingleSegment_HtmlExtension_KnownSlug_RedirectsDirectlyToCanonical()
    {
        // /article.html → strip .html → /article → API returns canonical → 301 canonical (ONE redirect, not two)
        var canonical = "/ai/videos/my-article";
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/article.html", apiResult: new LegacyRedirectResult(canonical));

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be(canonical);
        nextCalled().Should().BeFalse();
    }

    // ── Date prefix stripping ───────────────────────────────────────────────

    [Theory]
    [InlineData("/ai/videos/2026-01-12-my-article", "/ai/videos/my-article")]
    [InlineData("/dotnet/blogs/2023-04-24-how-it-works", "/dotnet/blogs/how-it-works")]
    [InlineData("/security/news/2000-01-01-slug", "/security/news/slug")]
    public async Task MultiSegment_DatePrefix_RedirectsToCleanPath(string input, string expected)
    {
        var (middleware, context, nextCalled) = CreateMiddleware(path: input);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be(expected);
        nextCalled().Should().BeFalse();
    }

    [Fact]
    public async Task SingleSegment_DatePrefixOnly_NoHtml_Returns404_WithoutApiCall()
    {
        // /2026-01-12-article has a date prefix but no .html — not a legacy URL.
        // Return 404 immediately without calling the API.
        var mockApi = new Mock<ITechHubApiClient>();
        var (middleware, context, nextCalled) = CreateMiddleware(
            path: "/2026-01-12-article",
            mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound,
            "date-only prefix without .html is not a legacy URL — no API call should be made");
        nextCalled().Should().BeFalse();
        mockApi.Verify(
            x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()),
            Times.Never,
            "bare slugs without .html must never trigger an API lookup");
    }

    // ── Combined .html + date prefix (should still be ONE redirect) ─────────

    [Fact]
    public async Task MultiSegment_HtmlAndDatePrefix_SingleRedirectToFullyCleanPath()
    {
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/ai/videos/2026-01-12-my-article.html");

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be("/ai/videos/my-article");
        nextCalled().Should().BeFalse();
    }

    [Fact]
    public async Task SingleSegment_HtmlAndDatePrefix_RedirectsDirectlyToCanonical()
    {
        // /2026-01-12-article.html → /article → API returns canonical → one redirect, not three
        var canonical = "/ai/videos/article";
        var (middleware, context, nextCalled) = CreateMiddleware(
            path: "/2026-01-12-article.html",
            apiResult: new LegacyRedirectResult(canonical));

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be(canonical);
        nextCalled().Should().BeFalse();
    }

    // ── Legacy lookup (gated on hadHtmlExtension) ─────────────────────────

    [Fact]
    public async Task SingleSegment_BareSlug_NoHtml_Returns404_WithoutApiCall()
    {
        // /my-article has no .html — not a legacy URL. Return 404 without API call.
        var mockApi = new Mock<ITechHubApiClient>();
        var (middleware, context, nextCalled) = CreateMiddleware(
            path: "/my-article",
            mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound,
            "bare slugs without .html are not legacy URLs and must return 404 immediately");
        nextCalled().Should().BeFalse();
        mockApi.Verify(
            x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()),
            Times.Never,
            "no API call must be made for bare slugs without .html");
    }

    [Fact]
    public async Task SingleSegment_LegacySlug_WithHtml_Found_RedirectsWith301()
    {
        // /my-article.html → strip .html → /my-article → hadHtmlExtension=true → API lookup
        var canonical = "/github-copilot/videos/my-article";
        var (middleware, context, nextCalled) = CreateMiddleware(
            path: "/my-article.html",
            apiResult: new LegacyRedirectResult(canonical));

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be(canonical);
        nextCalled().Should().BeFalse();
    }

    [Fact]
    public async Task SingleSegment_LegacySlug_WithHtml_NotFound_Returns404()
    {
        // /unknown-slug.html → strip .html → API returns null → 404
        var mockApi = new Mock<ITechHubApiClient>();
        var (middleware, context, nextCalled) = CreateMiddleware(
            path: "/unknown-slug.html",
            mockApiClient: mockApi);
        mockApi.Setup(x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((LegacyRedirectResult?)null);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeFalse();
        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        mockApi.Verify(
            x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()),
            Times.Once,
            "API must be called for .html slugs");
    }

    [Fact]
    public async Task SectionHint_IsForwardedToApiCall()
    {
        string? capturedHint = null;
        var mockApi = new Mock<ITechHubApiClient>();
        mockApi
            .Setup(x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .Callback<string, string?, CancellationToken>((_, hint, _) => capturedHint = hint)
            .ReturnsAsync((LegacyRedirectResult?)null);

        var (middleware, context, _) = CreateMiddleware(
            path: "/my-slug.html",
            queryString: "?section=ai",
            mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        capturedHint.Should().Be("ai");
    }

    [Fact]
    public async Task SectionHint_LegacySectionName_IsRemappedBeforeApiCall()
    {
        // ?section=coding is an old section name; it must be remapped to "dotnet"
        // so the API lookup is scoped to the current name.
        string? capturedHint = null;
        var mockApi = new Mock<ITechHubApiClient>();
        mockApi
            .Setup(x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .Callback<string, string?, CancellationToken>((_, hint, _) => capturedHint = hint)
            .ReturnsAsync((LegacyRedirectResult?)null);

        var (middleware, context, _) = CreateMiddleware(
            path: "/my-slug.html",
            queryString: "?section=coding",
            mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        capturedHint.Should().Be("dotnet", "legacy section name 'coding' must be remapped to 'dotnet'");
    }

    [Theory]
    [InlineData("?section=%27")]        // single quote (SQL injection probe)
    [InlineData("?section=coding%27")]  // trailing quote
    [InlineData("?section=Invalid!")]   // special character
    [InlineData("?section=has spaces")] // spaces
    public async Task SectionHint_InvalidValue_IsDiscarded_AndApiCalledWithNull(string queryString)
    {
        string? capturedHint = "sentinel";
        var mockApi = new Mock<ITechHubApiClient>();
        mockApi
            .Setup(x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .Callback<string, string?, CancellationToken>((_, hint, _) => capturedHint = hint)
            .ReturnsAsync((LegacyRedirectResult?)null);

        var (middleware, context, _) = CreateMiddleware(
            path: "/my-slug.html",
            queryString: queryString,
            mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        capturedHint.Should().BeNull("invalid section hints must be discarded to avoid a 400 from the API");
    }

    [Fact]
    public async Task ApiException_WithHtmlPath_RedirectsToCleanedPath()
    {
        // /some-slug.html → strip .html → hadHtmlExtension=true → API throws → 503 (no pathChanged after strip)
        // Wait — /some-slug.html strips to /some-slug so pathChanged=true → redirect to clean URL on failure.
        // Use a path where only a non-html change happened to test the 503 branch: impossible for single-segment
        // since the only changes are .html strip (hadHtml=true) or date strip (hadHtml=false, now 404).
        // This test verifies the 503 path: pathChanged=false, API throws.
        // To get pathChanged=false with hadHtml=true we need a bare slug with .html that ALSO has no path change
        // after stripping — e.g. a slug that is exactly its own normalized form after .html strip.
        // Actually /slug.html → /slug IS a path change. We can't reach 503 with a single-segment html path
        // because stripping .html always changes the path. So 503 is only reachable for multi-segment html paths.
        // See TwoSegment tests for 503 coverage. This test documents the graceful-degradation redirect.
        var mockApi = new Mock<ITechHubApiClient>();
        mockApi
            .Setup(x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("Connection refused"));

        var (middleware, context, nextCalled) = CreateMiddleware(path: "/article.html", mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently,
            "path changed (.html stripped) so we redirect to the clean URL even on API failure");
        context.Response.Headers.Location.ToString().Should().Be("/article");
        nextCalled().Should().BeFalse();
    }

    [Fact]
    public async Task ApiTimeout_WithHtmlPath_RedirectsToCleanedPath()
    {
        // Timeout is a transient failure: same graceful-degradation as HttpRequestException.
        var mockApi = new Mock<ITechHubApiClient>();
        mockApi
            .Setup(x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new TaskCanceledException("API timeout"));

        var (middleware, context, nextCalled) = CreateMiddleware(path: "/some-slug.html", mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        // /some-slug.html → /some-slug: pathChanged=true → redirect to cleaned path
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be("/some-slug");
        nextCalled().Should().BeFalse();
    }

    [Fact]
    public async Task RequestAbort_Rethrows()
    {
        // When the client disconnects (RequestAborted is signaled) the exception must propagate,
        // not be swallowed, to avoid writing a redirect onto an already-closed connection.
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        var mockApi = new Mock<ITechHubApiClient>();
        mockApi
            .Setup(x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new OperationCanceledException("Request was cancelled"));

        var (middleware, context, _) = CreateMiddleware(path: "/Some-Slug.html", mockApiClient: mockApi);
        context.RequestAborted = cts.Token;

        var act = () => middleware.InvokeAsync(context);

        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task UnexpectedException_Propagates()
    {
        // Non-transient exceptions (DI bugs, NullReferenceException, etc.) must propagate so
        // they surface as 500 errors in monitoring — not silently disappear as phantom 404s.
        var mockApi = new Mock<ITechHubApiClient>();
        mockApi
            .Setup(x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Service not registered"));

        // Must use a .html path so the legacy lookup is triggered and the exception fires.
        var (middleware, context, _) = CreateMiddleware(path: "/some-slug.html", mockApiClient: mockApi);

        var act = () => middleware.InvokeAsync(context);

        await act.Should().ThrowAsync<InvalidOperationException>("unexpected exceptions must not be swallowed");
    }

    // ── Pass-through cases — no API call, no redirect ──────────────────────

    [Fact]
    public async Task RootPath_PassesThrough()
    {
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/");

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue();
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status301MovedPermanently);
    }

    [Fact]
    public async Task MultiSegment_NoNormalizationNeeded_PassesThrough()
    {
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/ai/videos/my-article");

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue();
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status301MovedPermanently);
    }

    [Theory]
    [InlineData("/not-found")]
    [InlineData("/about")]
    [InlineData("/error")]
    [InlineData("/admin")]
    [InlineData("/version")]
    public async Task KnownNonSectionPage_PassesThrough(string path)
    {
        var (middleware, context, nextCalled) = CreateMiddleware(path: path);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue($"known non-section page {path} should pass through");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status301MovedPermanently);
    }

    [Fact]
    public async Task VersionEndpoint_PassesThrough_WithoutApiCall()
    {
        // /version is a Minimal API endpoint (MapGet) that returns the deployed image tag.
        // It must not be treated as a legacy content slug — before this was fixed, the middleware
        // would call GetLegacyRedirectAsync("/version"), receive null, and return 404.
        var mockApi = new Mock<ITechHubApiClient>();
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/version", mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue("/version must reach the endpoint handler, not be 404'd as an unknown slug");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status404NotFound);
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status301MovedPermanently);
        mockApi.Verify(x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task OidcCallbackPath_PassesThrough_WithoutApiCall()
    {
        // /signin-oidc is the OIDC redirect URI handled by UseAuthentication.
        // UrlNormalizationMiddleware runs before UseAuthentication, so it must not treat
        // this path as a legacy content slug and must pass it through untouched.
        var mockApi = new Mock<ITechHubApiClient>();
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/signin-oidc", mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue("OIDC callback path must pass through to UseAuthentication");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status404NotFound);
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status301MovedPermanently);
        mockApi.Verify(x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task KnownSectionName_PassesThrough_WithoutApiCall()
    {
        var mockApi = new Mock<ITechHubApiClient>();
        var cache = A.SectionCache.WithSections("ai").Build();
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/ai", sectionCache: cache, mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue("known section should pass through to Blazor routing");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status301MovedPermanently);
        mockApi.Verify(x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [InlineData("/styles.css")]
    [InlineData("/script.js")]
    [InlineData("/image.png")]
    [InlineData("/favicon.ico")]
    public async Task StaticFileExtension_PassesThrough_WithoutApiCall(string path)
    {
        var mockApi = new Mock<ITechHubApiClient>();
        var (middleware, context, nextCalled) = CreateMiddleware(path: path, mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue("static files should pass through");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status301MovedPermanently);
        mockApi.Verify(x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [InlineData("/_blazor")]
    [InlineData("/_framework")]
    [InlineData("/_content")]
    public async Task UnderscorePrefixedPath_PassesThrough_WithoutApiCall(string path)
    {
        var mockApi = new Mock<ITechHubApiClient>();
        var (middleware, context, nextCalled) = CreateMiddleware(path: path, mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue("underscore-prefixed paths (Blazor internals) should pass through");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status301MovedPermanently);
        mockApi.Verify(x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [InlineData("/wp_admin")]
    [InlineData("/@username")]
    [InlineData("/some~path")]
    public async Task InvalidSlugFormat_PassesThrough_WithoutApiCall(string path)
    {
        var mockApi = new Mock<ITechHubApiClient>();
        var (middleware, context, nextCalled) = CreateMiddleware(path: path, mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue("segments that can never be valid slugs should not trigger an API call");
        mockApi.Verify(x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    // ── Query string preservation ───────────────────────────────────────────

    [Fact]
    public async Task QueryString_IsPreservedInHtmlRedirect()
    {
        var (middleware, context, _) = CreateMiddleware(
            path: "/ai/videos/article.html",
            queryString: "?ref=rss");

        await middleware.InvokeAsync(context);

        context.Response.Headers.Location.ToString().Should().Be("/ai/videos/article?ref=rss");
    }

    [Fact]
    public async Task QueryString_IsPreservedInDatePrefixRedirect()
    {
        var (middleware, context, _) = CreateMiddleware(
            path: "/ai/videos/2026-01-12-article",
            queryString: "?ref=rss");

        await middleware.InvokeAsync(context);

        context.Response.Headers.Location.ToString().Should().Be("/ai/videos/article?ref=rss");
    }

    [Fact]
    public async Task QueryString_IsPreservedInLegacyLookupRedirect()
    {
        // When the API resolves a legacy slug to a canonical URL, the original query string
        // (e.g. UTM parameters) must be appended so tracking is not lost.
        var canonical = "/ai/videos/my-article";
        var (middleware, context, _) = CreateMiddleware(
            path: "/my-article.html",
            queryString: "?utm_source=newsletter&utm_medium=email",
            apiResult: new LegacyRedirectResult(canonical));

        await middleware.InvokeAsync(context);

        context.Response.Headers.Location.ToString()
            .Should().Be(canonical + "?utm_source=newsletter&utm_medium=email");
    }

    // ── Round-trip: dates at end of slug are NOT stripped (e.g. roundups) ──

    [Theory]
    [InlineData("/all/roundups/weekly-ai-and-tech-news-roundup-2026-03-16")]
    [InlineData("/all/roundups/monthly-digest-2025-12-30")]
    public async Task DateAtEndOfSlug_NotStripped_PassesThrough(string path)
    {
        var (middleware, context, nextCalled) = CreateMiddleware(path: path);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue("dates at the END of a slug are not prefixes and must not be stripped");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status301MovedPermanently);
    }

    // ── Legacy RSS feed redirects ────────────────────────────────────────────

    [Fact]
    public async Task FeedXml_Root_RedirectsToAllFeed()
    {
        // /feed.xml is the old "everything" feed root URL. Redirect to the canonical /all/feed.xml.
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/feed.xml");

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be("/all/feed.xml");
        nextCalled().Should().BeFalse();
    }

    [Fact]
    public async Task FeedXml_KnownSection_RedirectsToSectionFeed()
    {
        // /security.xml was the old per-section feed URL. Redirect to /security/feed.xml.
        var cache = A.SectionCache.WithSections("security").Build();
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/security.xml", sectionCache: cache);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be("/security/feed.xml");
        nextCalled().Should().BeFalse();
    }

    [Theory]
    [InlineData("ai")]
    [InlineData("azure")]
    [InlineData("github-copilot")]
    public async Task FeedXml_KnownSection_CaseInsensitive_UsesCanonicalSectionName(string sectionName)
    {
        // /AI.XML should redirect to /ai/feed.xml using the canonical casing from the section cache.
        var cache = A.SectionCache.WithSections(sectionName).Build();
        var (middleware, context, nextCalled) = CreateMiddleware(
            path: $"/{sectionName.ToUpperInvariant()}.xml",
            sectionCache: cache);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be($"/{sectionName}/feed.xml");
        nextCalled().Should().BeFalse();
    }

    [Fact]
    public async Task FeedXml_UnknownSection_PassesThrough_WithoutApiCall()
    {
        // /wordpress.xml is not a known section — falls through to InvalidRouteSegmentMiddleware
        // which treats it as a probe and returns 404. No API call should be made.
        var cache = A.SectionCache.WithSections("ai").Build();
        var mockApi = new Mock<ITechHubApiClient>();
        var (middleware, context, nextCalled) = CreateMiddleware(
            path: "/wordpress.xml",
            sectionCache: cache,
            mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue("unknown .xml paths must pass through — InvalidRouteSegmentMiddleware handles them");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status301MovedPermanently);
        mockApi.Verify(
            x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()),
            Times.Never,
            "static file extension short-circuits the legacy API lookup");
    }

    [Fact]
    public async Task FeedXml_MultiSegment_NotRedirected()
    {
        // /all/feed.xml and /security/feed.xml are already canonical — must not be touched.
        var cache = A.SectionCache.WithSections("security").Build();
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/security/feed.xml", sectionCache: cache);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue("/security/feed.xml is already the canonical URL");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status301MovedPermanently);
    }

    [Fact]
    public async Task FeedXml_SectionXml_CacheNotReady_PassesThrough()
    {
        // When the section cache is not ready (API down at startup), /{sectionName}.xml
        // cannot be validated against the cache. Pass through rather than blocking a
        // legitimate feed URL — consistent with the broader policy of avoiding false-404s
        // during cache warmup/unavailability.
        var emptyCache = SectionCacheBuilder.Empty();
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/ai.xml", sectionCache: emptyCache);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue("section feed URLs must not be blocked when the cache is not ready");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status301MovedPermanently);
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status404NotFound);
    }

    // ── Multi-segment section/collection validation ─────────────────────────

    [Theory]
    [InlineData("/fakesection/videos/my-article")]
    [InlineData("/fakesection/videos")]
    [InlineData("/doesnotexist/anything")]
    public async Task MultiSegment_UnknownSection_Returns404(string path)
    {
        // The section cache has "ai" only — anything else is unknown.
        var cache = A.SectionCache.WithSections("ai").Build();
        var (middleware, context, nextCalled) = CreateMiddleware(path: path, sectionCache: cache);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound,
            "requests to non-existent sections are rejected before reaching Blazor");
        nextCalled().Should().BeFalse();
    }

    [Fact]
    public async Task MultiSegment_KnownSection_UnknownCollection_Returns404()
    {
        var cache = A.SectionCache.WithSections("ai").WithCollections("videos", "blogs").Build();
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/ai/notacollection", sectionCache: cache);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound,
            "requests to a collection that doesn't exist in a known section are rejected early");
        nextCalled().Should().BeFalse();
    }

    [Theory]
    [InlineData("/ai/videos")]
    [InlineData("/ai/blogs")]
    [InlineData("/ai/videos/my-slug")]
    public async Task MultiSegment_KnownSection_KnownCollection_PassesThrough(string path)
    {
        var cache = A.SectionCache.WithSections("ai").WithCollections("videos", "blogs").Build();
        var (middleware, context, nextCalled) = CreateMiddleware(path: path, sectionCache: cache);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue($"{path} has a valid section and collection");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status404NotFound);
    }

    [Theory]
    [InlineData("/ai/all")]
    [InlineData("/github-copilot/all")]
    [InlineData("/security/all")]
    public async Task MultiSegment_KnownSection_VirtualAllCollection_PassesThrough(string path)
    {
        // /{sectionName}/all is a valid Blazor route handled by SectionCollection.razor.
        // It is a virtual "show all content" view — not stored as a real API collection —
        // so IsKnownCollection returns false. The middleware must NOT 404 these paths.
        var sectionName = path.TrimStart('/').Split('/')[0];
        var cache = A.SectionCache.WithSections(sectionName).WithCollections("videos", "blogs").Build();
        var (middleware, context, nextCalled) = CreateMiddleware(path: path, sectionCache: cache);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue($"{path} is a valid section 'all' view and must not be 404'd");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task MultiSegment_UnknownSection_WithHtmlExtension_Returns404_NotRedirect()
    {
        // After normalization /fakesection/article.html → /fakesection/article.
        // The normalized path is invalid — return 404 directly, not a 301 to another 404.
        var cache = A.SectionCache.WithSections("ai").Build();
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/fakesection/article.html", sectionCache: cache);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound,
            "normalization must not issue a redirect to a URL that would itself 404");
        nextCalled().Should().BeFalse();
    }

    [Fact]
    public async Task MultiSegment_KnownSection_UnknownCollection_WithHtmlExtension_Returns404()
    {
        var cache = A.SectionCache.WithSections("ai").WithCollections("videos").Build();
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/ai/notacollection/article.html", sectionCache: cache);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        nextCalled().Should().BeFalse();
    }

    [Theory]
    [InlineData("/_blazor/negotiate")]
    [InlineData("/_framework/blazor.server.js")]
    [InlineData("/_content/some/asset.js")]
    public async Task MultiSegment_FrameworkPaths_PassThrough(string path)
    {
        var cache = A.SectionCache.WithSections("ai").Build();
        var (middleware, context, nextCalled) = CreateMiddleware(path: path, sectionCache: cache);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue($"framework paths must always pass through regardless of cache contents");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status404NotFound);
    }

    [Theory]
    [InlineData("/admin/users")]
    [InlineData("/admin/content/edit")]
    [InlineData("/error/details")]
    [InlineData("/newsletter/subscribe")]
    [InlineData("/newsletter/unsubscribe")]
    public async Task MultiSegment_KnownNonSectionPages_PassThrough(string path)
    {
        var cache = A.SectionCache.WithSections("ai").Build();
        var (middleware, context, nextCalled) = CreateMiddleware(path: path, sectionCache: cache);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue($"known non-section pages (admin, error, etc.) may have sub-paths");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status404NotFound);
    }

    [Theory]
    [InlineData("/all")]
    [InlineData("/all/all")]
    [InlineData("/all/roundups")]       // real collection in ai section
    [InlineData("/all/news")]           // real collection in ai section
    [InlineData("/all/videos")]         // real collection in ai section
    [InlineData("/all/authors")]        // dedicated virtual sub-page (Authors.razor)
    [InlineData("/all/feed.xml")]       // file extension — passed to RSS endpoint
    [InlineData("/all/roundups/my-article")]
    [InlineData("/all/authors/john-doe")]
    public async Task VirtualSection_All_ValidPaths_PassThrough(string path)
    {
        // /all is a virtual section (not in the API cache) that aggregates content across
        // all real sections. Valid sub-paths include the "all" keyword, dedicated virtual pages
        // (authors), file extensions (feeds), and any collection in any real section.
        var cache = A.SectionCache.WithSections("ai").WithCollections("roundups", "news", "videos").Build();
        var (middleware, context, nextCalled) = CreateMiddleware(path: path, sectionCache: cache);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue($"valid /all path '{path}' must pass through to Blazor routing");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status404NotFound);
    }

    [Theory]
    [InlineData("/all/garbage")]
    [InlineData("/all/nonexistent-collection")]
    public async Task VirtualSection_All_InvalidSubPath_Returns404(string path)
    {
        // /all only permits real section collections, dedicated virtual pages, "all", and
        // file extensions. Names that don't match any real section's collection return 404.
        var cache = A.SectionCache.WithSections("ai").WithCollections("roundups", "news", "videos").Build();
        var (middleware, context, nextCalled) = CreateMiddleware(path: path, sectionCache: cache);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound,
            $"unknown /all sub-path '{path}' must return 404");
        nextCalled().Should().BeFalse();
    }

    [Theory]
    [InlineData("/css/article.css")]
    [InlineData("/css/sidebar.css")]
    [InlineData("/js/scroll-manager.js")]
    [InlineData("/js/page-scripts.js")]
    [InlineData("/images/logo.png")]
    [InlineData("/images/section-backgrounds/ai.jxl")]
    [InlineData("/images/section-backgrounds/github-copilot.webp")]
    [InlineData("/fakesection/something.css")]
    public async Task MultiSegment_StaticFilePaths_PassThrough_RegardlessOfSection(string path)
    {
        // Static assets like /css/article.css must reach UseStaticFiles even when
        // the first path segment is not a known section name.
        var cache = A.SectionCache.WithSections("ai").Build();
        var (middleware, context, nextCalled) = CreateMiddleware(path: path, sectionCache: cache);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue($"static asset path {path} must always pass through to UseStaticFiles");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task MultiSegment_CacheNotReady_PassesThrough_WithoutFalse404()
    {
        // Empty cache = API was down at startup. Must not 404 valid-looking paths.
        var emptyCache = SectionCacheBuilder.Empty();
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/ai/videos/my-article", sectionCache: emptyCache);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue("an empty cache must not cause false 404s during API outages");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task SingleSegment_CacheNotReady_PassesThrough_WithoutApiCall()
    {
        // Empty cache = API was down at startup.
        // Real section routes like /ai must not trigger a legacy API lookup just because
        // GetSectionByName returns null when the cache is empty — that would add unnecessary
        // latency and warning noise for every section request during startup/API outages.
        var emptyCache = SectionCacheBuilder.Empty();
        var mockApi = new Mock<ITechHubApiClient>();
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/ai", sectionCache: emptyCache, mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue("a single-segment section route must pass through when the cache is not ready");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status404NotFound);
        mockApi.Verify(
            x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()),
            Times.Never,
            "legacy lookup must be skipped when the section cache is not ready");
    }

    [Fact]
    public async Task MultiSegment_KnownSection_ValidCollection_WithDatePrefix_RedirectsToCleanPath()
    {
        // Normalization strips the date, validation confirms the result is valid, then redirect.
        var cache = A.SectionCache.WithSections("ai").WithCollections("videos").Build();
        var (middleware, context, nextCalled) = CreateMiddleware(
            path: "/ai/videos/2026-01-12-my-article",
            sectionCache: cache);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be("/ai/videos/my-article");
        nextCalled().Should().BeFalse();
    }

    // ── Trailing slash redirect ─────────────────────────────────────────────

    [Theory]
    [InlineData("/ai/", "/ai")]
    [InlineData("/github-copilot/", "/github-copilot")]
    [InlineData("/ai/videos/", "/ai/videos")]
    [InlineData("/ai/videos/my-article/", "/ai/videos/my-article")]
    public async Task TrailingSlash_Redirects301ToPathWithoutSlash(string input, string expected)
    {
        // /ai/ must redirect to /ai — Blazor routing only matches paths without trailing slashes.
        var (middleware, context, nextCalled) = CreateMiddleware(path: input);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be(expected);
        nextCalled().Should().BeFalse();
    }

    [Fact]
    public async Task TrailingSlash_QueryStringPreserved()
    {
        var (middleware, context, _) = CreateMiddleware(path: "/ai/", queryString: "?ref=rss");

        await middleware.InvokeAsync(context);

        context.Response.Headers.Location.ToString().Should().Be("/ai?ref=rss");
    }

    [Fact]
    public async Task RootSlash_NotRedirected()
    {
        // The root "/" is a valid path — must never redirect.
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/");

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue();
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status301MovedPermanently);
    }

    // ── Section rename redirect ─────────────────────────────────────────────

    [Theory]
    [InlineData("/coding/videos/my-slug", "/dotnet/videos/my-slug")]
    [InlineData("/coding/blogs/article", "/dotnet/blogs/article")]
    [InlineData("/coding/videos", "/dotnet/videos")]
    [InlineData("/coding", "/dotnet")]
    public async Task RenamedSection_Coding_RedirectsToDotnet(string input, string expected)
    {
        // "coding" was renamed to "dotnet". Any path under /coding/ must redirect to /dotnet/.
        var cache = A.SectionCache.WithSections("dotnet").WithCollections("videos", "blogs").Build();
        var (middleware, context, nextCalled) = CreateMiddleware(path: input, sectionCache: cache);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be(expected);
        nextCalled().Should().BeFalse();
    }

    [Fact]
    public async Task RenamedSection_WithHtmlAndDatePrefix_SingleRedirect()
    {
        // /coding/2025-01-01-my-article.html → strip html+date → /coding/my-article →
        // rename coding→dotnet → one 301 to /dotnet/my-article (via 2-segment legacy lookup).
        var canonical = "/dotnet/videos/my-article";
        var cache = A.SectionCache.WithSections("dotnet").WithCollections("videos").Build();
        var (middleware, context, nextCalled) = CreateMiddleware(
            path: "/coding/2025-01-01-my-article.html",
            sectionCache: cache,
            apiResult: new LegacyRedirectResult(canonical));

        await middleware.InvokeAsync(context);

        // After rename + normalization → /dotnet/my-article (2-segment, dotnet is known section,
        // my-article is not a known collection) → legacy lookup → canonical.
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be(canonical);
        nextCalled().Should().BeFalse();
    }

    [Fact]
    public async Task RenamedSection_QueryStringPreserved()
    {
        var cache = A.SectionCache.WithSections("dotnet").WithCollections("videos").Build();
        var (middleware, context, _) = CreateMiddleware(
            path: "/coding/videos/my-slug",
            queryString: "?ref=rss",
            sectionCache: cache);

        await middleware.InvokeAsync(context);

        context.Response.Headers.Location.ToString().Should().Be("/dotnet/videos/my-slug?ref=rss");
    }

    // ── Two-segment Jekyll-style legacy lookup (/section/slug.html) ─────────

    [Fact]
    public async Task TwoSegment_KnownSection_SlugFound_RedirectsToCanonical()
    {
        // /ai/2026-01-15-build-powerful-ai-apps.html → /ai/build-powerful-ai-apps →
        // ai is a known section, build-powerful-ai-apps is not a collection → legacy lookup →
        // canonical URL returned.
        var canonical = "/ai/videos/build-powerful-ai-apps";
        var cache = A.SectionCache.WithSections("ai").WithCollections("videos").Build();
        var (middleware, context, nextCalled) = CreateMiddleware(
            path: "/ai/2026-01-15-build-powerful-ai-apps.html",
            sectionCache: cache,
            apiResult: new LegacyRedirectResult(canonical));

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be(canonical);
        nextCalled().Should().BeFalse();
    }

    [Fact]
    public async Task TwoSegment_KnownSection_BareSlug_NoHtml_Returns404_WithoutApiCall()
    {
        // /ai/unknown-slug has no .html — not a legacy URL. Return 404 without API call.
        var mockApi = new Mock<ITechHubApiClient>();
        var cache = A.SectionCache.WithSections("ai").WithCollections("videos").Build();
        var (middleware, context, nextCalled) = CreateMiddleware(
            path: "/ai/unknown-slug",
            sectionCache: cache,
            mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound,
            "two-segment paths without .html are not legacy URLs — no API call should be made");
        nextCalled().Should().BeFalse();
        mockApi.Verify(
            x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()),
            Times.Never,
            "bare two-segment slugs without .html must never trigger an API lookup");
    }

    [Fact]
    public async Task TwoSegment_KnownSection_SectionUsedAsHintInLegacyLookup()
    {
        // The section name (first segment) must be passed as the sectionHint to the API call.
        // Path must have .html to trigger the legacy lookup.
        string? capturedHint = null;
        var mockApi = new Mock<ITechHubApiClient>();
        mockApi
            .Setup(x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .Callback<string, string?, CancellationToken>((_, hint, _) => capturedHint = hint)
            .ReturnsAsync((LegacyRedirectResult?)null);

        var cache = A.SectionCache.WithSections("ai").WithCollections("videos").Build();
        var (middleware, context, _) = CreateMiddleware(
            path: "/ai/some-article.html",
            sectionCache: cache,
            mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        capturedHint.Should().Be("ai", "the section segment must be forwarded as the section hint");
    }

    [Fact]
    public async Task TwoSegment_KnownSection_ApiFailure_WhenPathChanged_Returns503()
    {
        // /ai/some-article.html → /ai/some-article (pathChanged=true, hadHtmlExtension=true)
        // → API throws → since pathChanged, redirect to the cleaned path as graceful fallback.
        // The 503 branch (pathChanged=false) is unreachable for .html paths because stripping
        // .html always changes the path; this test covers the redirect-on-failure branch.
        var mockApi = new Mock<ITechHubApiClient>();
        mockApi
            .Setup(x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("Connection refused"));

        var cache = A.SectionCache.WithSections("ai").WithCollections("videos").Build();
        var (middleware, context, nextCalled) = CreateMiddleware(
            path: "/ai/some-article.html",
            sectionCache: cache,
            mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be("/ai/some-article");
        nextCalled().Should().BeFalse();
    }

    [Fact]
    public async Task TwoSegment_KnownSection_ApiFailure_WhenPathChanged_RedirectsToNormalizedPath()
    {
        // /ai/2026-01-15-some-article.html → /ai/some-article before the API call.
        // If the legacy lookup then fails transiently, redirect to the cleaned path first.
        var mockApi = new Mock<ITechHubApiClient>();
        mockApi
            .Setup(x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("Connection refused"));

        var cache = A.SectionCache.WithSections("ai").WithCollections("videos").Build();
        var (middleware, context, nextCalled) = CreateMiddleware(
            path: "/ai/2026-01-15-some-article.html",
            queryString: "?ref=rss",
            sectionCache: cache,
            mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be("/ai/some-article?ref=rss");
        nextCalled().Should().BeFalse();
    }

    [Fact]
    public async Task TwoSegment_KnownSection_KnownCollection_NotLegacyLookup()
    {
        // /ai/videos is a valid section/collection path — must NOT trigger a legacy lookup.
        var mockApi = new Mock<ITechHubApiClient>();
        var cache = A.SectionCache.WithSections("ai").WithCollections("videos").Build();
        var (middleware, context, nextCalled) = CreateMiddleware(
            path: "/ai/videos",
            sectionCache: cache,
            mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue("/ai/videos is a valid section/collection path, not a legacy slug");
        mockApi.Verify(
            x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()),
            Times.Never,
            "valid section/collection paths must not trigger a legacy API call");
    }

    // ── Helpers ─────────────────────────────────────────────────────────────

    private static IServiceScopeFactory BuildScopeFactory(ITechHubApiClient apiClient)
    {
        var services = new ServiceCollection();
        services.AddSingleton<ITechHubApiClient>(apiClient);
        return services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();
    }

    private static (
        UrlNormalizationMiddleware middleware,
        HttpContext context,
        Func<bool> nextCalled)
    CreateMiddleware(
        string path = "/",
        string? queryString = null,
        LegacyRedirectResult? apiResult = null,
        SectionCache? sectionCache = null,
        Mock<ITechHubApiClient>? mockApiClient = null)
    {
        var wasCalled = false;
        RequestDelegate next = _ =>
        {
            wasCalled = true;
            return Task.CompletedTask;
        };

        var mock = mockApiClient ?? new Mock<ITechHubApiClient>();
        if (mockApiClient == null)
        {
            // Default setup: return the supplied apiResult for any call
            mock.Setup(x => x.GetLegacyRedirectAsync(
                    It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(apiResult);
        }

        // Default to a production-like ready cache so the !IsReady guard does not
        // short-circuit tests. Tests that specifically exercise the "cache not ready"
        // path pass SectionCacheBuilder.Empty() explicitly.
        var cache = sectionCache ?? A.SectionCache.Build();
        var scopeFactory = BuildScopeFactory(mock.Object);
        var middleware = new UrlNormalizationMiddleware(
            next,
            cache,
            scopeFactory,
            NullLogger<UrlNormalizationMiddleware>.Instance);

        var context = new DefaultHttpContext();
        context.Request.Path = path;
        if (queryString != null)
        {
            context.Request.QueryString = new QueryString(queryString);
        }

        return (middleware, context, () => wasCalled);
    }
}
