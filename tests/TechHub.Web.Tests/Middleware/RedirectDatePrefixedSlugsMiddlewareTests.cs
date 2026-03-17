using FluentAssertions;
using Microsoft.AspNetCore.Http;
using TechHub.Web.Middleware;

namespace TechHub.Web.Tests.Middleware;

/// <summary>
/// Tests for RedirectDatePrefixedSlugsMiddleware - strips date prefix from slug URLs and 301-redirects.
/// E.g. /ai/videos/2026-01-12-my-article -> /ai/videos/my-article (301).
/// If the canonical URL doesn't exist the normal pipeline returns a 404.
/// </summary>
public class RedirectDatePrefixedSlugsMiddlewareTests
{
    [Theory]
    [InlineData("/ai/videos/2026-01-12-my-article", "/ai/videos/my-article")]
    [InlineData("/github-copilot/vscode-updates/2024-06-07-some-update", "/github-copilot/vscode-updates/some-update")]
    [InlineData("/dotnet/blogs/2023-04-24-how-copilot-helps", "/dotnet/blogs/how-copilot-helps")]
    [InlineData("/section/collection/2000-01-01-slug", "/section/collection/slug")]
    public async Task DatePrefixedPath_RedirectsToStrippedPath(string inputPath, string expectedPath)
    {
        // Arrange
        var (middleware, context, _) = CreateMiddleware(path: inputPath);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be(expectedPath);
    }

    [Fact]
    public async Task DatePrefixedPath_PreservesQueryString()
    {
        // Arrange
        var (middleware, context, _) = CreateMiddleware(
            path: "/ai/videos/2026-01-12-my-article",
            queryString: "?ref=rss");

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be("/ai/videos/my-article?ref=rss");
    }

    [Fact]
    public async Task DatePrefixedPath_DoesNotCallNext()
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/ai/videos/2026-01-12-my-article");

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled().Should().BeFalse("middleware should NOT call next when redirecting");
    }

    [Theory]
    [InlineData("/ai/videos/my-article")]
    [InlineData("/github-copilot/features")]
    [InlineData("/about")]
    [InlineData("/")]
    [InlineData("/dotnet/blogs/some-post-without-date")]
    // Date at END of slug (e.g. roundup URLs) must NOT be redirected
    [InlineData("/all/roundups/weekly-ai-and-tech-news-roundup-2026-03-16")]
    [InlineData("/all/roundups/weekly-ai-and-tech-news-roundup-2025-12-30")]
    public async Task PathWithoutDatePrefix_PassesThrough(string path)
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware(path: path);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled().Should().BeTrue("middleware should call next for non-date-prefixed paths");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status301MovedPermanently);
    }

    private static (RedirectDatePrefixedSlugsMiddleware middleware, HttpContext context, Func<bool> nextCalled) CreateMiddleware(
        string path = "/",
        string? queryString = null)
    {
        var wasCalled = false;
        RequestDelegate next = _ =>
        {
            wasCalled = true;
            return Task.CompletedTask;
        };

        var middleware = new RedirectDatePrefixedSlugsMiddleware(next);

        var context = new DefaultHttpContext();
        context.Request.Path = path;
        if (queryString != null)
        {
            context.Request.QueryString = new QueryString(queryString);
        }

        return (middleware, context, () => wasCalled);
    }
}
