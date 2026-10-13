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
/// Open-redirect safety tests for UrlNormalizationMiddleware. A crafted request path must never produce a
/// <c>Location</c> header that browsers interpret as an off-site (protocol-relative) URL.
/// </summary>
public class UrlNormalizationMiddlewareRedirectSafetyTests
{
    [Theory]
    [InlineData("//evil.com", "/evil.com")]
    [InlineData("///evil.com", "/evil.com")]
    [InlineData("//evil.com/", "/evil.com")]
    public async Task InvokeAsync_ProtocolRelativeSingleSegment_RedirectsToSameHostPath(string path, string expectedLocation)
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware(path, A.SectionCache.Build());

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be(expectedLocation);
        AssertLocationIsSameSite(context);
        nextCalled().Should().BeFalse();
    }

    [Theory]
    [InlineData("//")]
    [InlineData("///")]
    [InlineData("////")]
    public async Task InvokeAsync_OnlySlashes_RedirectsToRoot(string path)
    {
        // Arrange
        var (middleware, context, _) = CreateMiddleware(path, A.SectionCache.Build());

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be("/");
    }

    [Theory]
    [InlineData("//evil.com/article.html")]
    [InlineData("///evil.com/article.html")]
    [InlineData("//evil.com/2026-01-12-article")]
    [InlineData("//evil.com/ai/videos/article.html")]
    public async Task InvokeAsync_ProtocolRelativeMultiSegment_UnknownSection_Returns404WithoutRedirect(string path)
    {
        // Arrange — "evil.com" is not a known section, so no redirect must be issued at all
        var (middleware, context, nextCalled) = CreateMiddleware(path, A.SectionCache.Build());

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        context.Response.Headers.Location.ToString().Should().BeEmpty();
        nextCalled().Should().BeFalse();
    }

    [Theory]
    [InlineData("//evil.com/article.html", "/evil.com/article")]
    [InlineData("///evil.com/article.html", "/evil.com/article")]
    [InlineData("//evil.com/2026-01-12-article", "/evil.com/article")]
    [InlineData("//evil.com/a/b.html", "/evil.com/a/b")]
    public async Task InvokeAsync_ProtocolRelativeMultiSegment_CacheNotReady_CollapsesLeadingSlashes(string path, string expectedLocation)
    {
        // Arrange — an empty cache disables section validation, so a redirect is issued
        var (middleware, context, _) = CreateMiddleware(path, SectionCacheBuilder.Empty());

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be(expectedLocation);
        AssertLocationIsSameSite(context);
    }

    [Theory]
    [InlineData("//ai/videos/article.html", "/ai/videos/article")]
    [InlineData("///ai/videos/2026-01-12-article", "/ai/videos/article")]
    public async Task InvokeAsync_ProtocolRelativeKnownSection_RedirectsToSingleSlashPath(string path, string expectedLocation)
    {
        // Arrange
        var (middleware, context, _) = CreateMiddleware(path, A.SectionCache.Build());

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be(expectedLocation);
        AssertLocationIsSameSite(context);
    }

    [Theory]
    [InlineData("/%2F%2Fevil.com/article.html")]
    [InlineData("/%2f%2fevil.com/article.html")]
    [InlineData("/%5Cevil.com/article.html")]
    public async Task InvokeAsync_EncodedSlashesInSegment_NeverProducesOffSiteLocation(string path)
    {
        // Arrange
        var (middleware, context, _) = CreateMiddleware(path, SectionCacheBuilder.Empty());

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        AssertLocationIsSameSite(context);
    }

    [Fact]
    public async Task InvokeAsync_QueryStringWithExternalUrl_IsAppendedToSameSitePath()
    {
        // Arrange — the query string is preserved verbatim, but the target stays on the same host
        var (middleware, context, _) = CreateMiddleware("/ai/videos/article.html", A.SectionCache.Build(), "?next=https://evil.com");

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.Headers.Location.ToString().Should().Be("/ai/videos/article?next=https://evil.com");
        AssertLocationIsSameSite(context);
    }

    [Fact]
    public async Task InvokeAsync_LegacyApiReturnsRelativeUrl_RedirectIsSameSite()
    {
        // Arrange
        var (middleware, context, _) = CreateMiddleware(
            "//my-article.html",
            A.SectionCache.Build(),
            apiResult: new LegacyRedirectResult("/ai/videos/my-article"));

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be("/ai/videos/my-article");
        AssertLocationIsSameSite(context);
    }

    private static void AssertLocationIsSameSite(HttpContext context)
    {
        var location = context.Response.Headers.Location.ToString();
        location.Should().StartWith("/");
        location.Should().NotStartWith("//", "a protocol-relative Location would redirect off-site");
        location.Should().NotStartWith("/\\", "browsers treat a leading slash-backslash as protocol-relative");
    }

    private static (UrlNormalizationMiddleware middleware, HttpContext context, Func<bool> nextCalled) CreateMiddleware(
        string path,
        SectionCache sectionCache,
        string? queryString = null,
        LegacyRedirectResult? apiResult = null)
    {
        var wasCalled = false;
        RequestDelegate next = _ =>
        {
            wasCalled = true;
            return Task.CompletedTask;
        };

        var apiClient = new Mock<ITechHubApiClient>();
        apiClient
            .Setup(x => x.GetLegacyRedirectAsync(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResult);

        var services = new ServiceCollection();
        services.AddSingleton(apiClient.Object);
        var scopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();

        var middleware = new UrlNormalizationMiddleware(
            next,
            sectionCache,
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
