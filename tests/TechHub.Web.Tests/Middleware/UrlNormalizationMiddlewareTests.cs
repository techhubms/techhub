using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TechHub.Core.Models;
using TechHub.Web.Middleware;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Middleware;

/// <summary>
/// Tests for UrlNormalizationMiddleware — the unified URL cleanup and legacy redirect handler.
///
/// Key behaviours verified:
///   - At most ONE 301 redirect per request regardless of how many normalizations apply.
///   - .html extension stripping, YYYY-MM-DD- date prefix stripping.
///   - Legacy API lookup for single-segment paths (only when the canonical path is unknown).
///   - Graceful fallback when the API is unavailable.
///   - Known sections, known pages, and static files are passed through without an API call.
///   - Case normalization is NOT performed — casing is handled by the infrastructure layer.
/// </summary>
public class UrlNormalizationMiddlewareTests
{
    // ── HTML extension stripping ────────────────────────────────────────────

    [Theory]
    [InlineData("/ai/videos/article.html", "/ai/videos/article")]
    [InlineData("/github-copilot/features.html", "/github-copilot/features")]
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
        var cache = BuildSectionCache("ai");
        var mockApi = new Mock<ITechHubApiClient>();
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/ai.html", sectionCache: cache, mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be("/ai");
        mockApi.Verify(x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()), Times.Never);
        nextCalled().Should().BeFalse();
    }

    [Fact]
    public async Task SingleSegment_HtmlExtension_UnknownSlug_RedirectsToCleanPath()
    {
        // /article.html → strip .html → /article → API returns null → still 301 /article (URL cleaned)
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/article.html", apiResult: null);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be("/article");
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
    [InlineData("/section/collection/2000-01-01-slug", "/section/collection/slug")]
    public async Task MultiSegment_DatePrefix_RedirectsToCleanPath(string input, string expected)
    {
        var (middleware, context, nextCalled) = CreateMiddleware(path: input);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be(expected);
        nextCalled().Should().BeFalse();
    }

    [Fact]
    public async Task SingleSegment_DatePrefix_KnownSlug_RedirectsDirectlyToCanonical()
    {
        // /2026-01-12-article → strip date → /article → API returns canonical → ONE redirect
        var canonical = "/ai/videos/article";
        var (middleware, context, nextCalled) = CreateMiddleware(
            path: "/2026-01-12-article",
            apiResult: new LegacyRedirectResult(canonical));

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be(canonical);
        nextCalled().Should().BeFalse();
    }

    [Fact]
    public async Task SingleSegment_DatePrefix_NotFound_RedirectsToStrippedPath()
    {
        // /2026-01-12-article → strip date → /article → API returns null → 301 /article
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/2026-01-12-article", apiResult: null);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be("/article");
        nextCalled().Should().BeFalse();
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

    // ── Legacy lookup ───────────────────────────────────────────────────────

    [Fact]
    public async Task SingleSegment_LegacySlug_Found_RedirectsWith301()
    {
        var canonical = "/github-copilot/videos/my-article";
        var (middleware, context, nextCalled) = CreateMiddleware(
            path: "/My-Article",
            apiResult: new LegacyRedirectResult(canonical));

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be(canonical);
        nextCalled().Should().BeFalse();
    }

    [Fact]
    public async Task SingleSegment_LegacySlug_NotFound_PassesThrough()
    {
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/Unknown-Slug", apiResult: null);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue();
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status301MovedPermanently);
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
            path: "/My-Slug",
            queryString: "?section=ai",
            mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        capturedHint.Should().Be("ai");
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
            path: "/My-Slug",
            queryString: queryString,
            mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        capturedHint.Should().BeNull("invalid section hints must be discarded to avoid a 400 from the API");
    }

    [Fact]
    public async Task ApiException_PassesThrough()
    {
        var mockApi = new Mock<ITechHubApiClient>();
        mockApi
            .Setup(x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("Connection refused"));

        var (middleware, context, nextCalled) = CreateMiddleware(path: "/Some-Slug", mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue("should pass through when API throws");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status301MovedPermanently);
    }

    [Fact]
    public async Task ApiException_WithHtmlPath_StillCleansUrl()
    {
        // Even when the API is down, .html should be stripped.
        var mockApi = new Mock<ITechHubApiClient>();
        mockApi
            .Setup(x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("Connection refused"));

        var (middleware, context, nextCalled) = CreateMiddleware(path: "/article.html", mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be("/article");
        nextCalled().Should().BeFalse();
    }

    [Fact]
    public async Task ApiTimeout_WhenNotRequestAbort_PassesThrough()
    {
        // TaskCanceledException from an HTTP client timeout (not a request abort) should fall through gracefully.
        var mockApi = new Mock<ITechHubApiClient>();
        mockApi
            .Setup(x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new TaskCanceledException("API timeout"));

        var (middleware, context, nextCalled) = CreateMiddleware(path: "/Some-Slug", mockApiClient: mockApi);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue("HTTP timeout (not a request abort) should fall through gracefully");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status301MovedPermanently);
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

        var (middleware, context, _) = CreateMiddleware(path: "/Some-Slug", mockApiClient: mockApi);
        context.RequestAborted = cts.Token;

        var act = () => middleware.InvokeAsync(context);

        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task InvalidOperationException_Propagates()
    {
        // InvalidOperationException indicates a DI/config bug and must not be swallowed.
        var mockApi = new Mock<ITechHubApiClient>();
        mockApi
            .Setup(x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Service not registered"));

        var (middleware, context, _) = CreateMiddleware(path: "/Some-Slug", mockApiClient: mockApi);

        var act = () => middleware.InvokeAsync(context);

        await act.Should().ThrowAsync<InvalidOperationException>();
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
    public async Task KnownNonSectionPage_PassesThrough(string path)
    {
        var (middleware, context, nextCalled) = CreateMiddleware(path: path);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue($"known non-section page {path} should pass through");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status301MovedPermanently);
    }

    [Fact]
    public async Task KnownSectionName_PassesThrough_WithoutApiCall()
    {
        var mockApi = new Mock<ITechHubApiClient>();
        var cache = BuildSectionCache("ai");
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
            path: "/my-article",
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

    // ── Helpers ─────────────────────────────────────────────────────────────

    private static SectionCache BuildSectionCache(params string[] sectionNames)
    {
        var cache = new SectionCache();
        var collection = new Collection("videos", "Videos", "/ai/videos", "Videos", "Videos");
        var sections = sectionNames
            .Select(name => new Section(name, name.ToUpperInvariant(), "desc", $"/{name}", name, [collection]))
            .ToList();
        cache.Initialize(sections);
        return cache;
    }

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

        var cache = sectionCache ?? new SectionCache();
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
