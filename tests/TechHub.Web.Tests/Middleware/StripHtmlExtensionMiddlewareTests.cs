using FluentAssertions;
using Microsoft.AspNetCore.Http;
using TechHub.Web.Middleware;

namespace TechHub.Web.Tests.Middleware;

/// <summary>
/// Tests for StripHtmlExtensionMiddleware - strips .html from URL paths and redirects.
/// Ensures legacy .html URLs (e.g. /github-copilot/features.html) redirect to the
/// canonical URL (/github-copilot/features) instead of matching the generic
/// /{sectionName}/{collectionName} route and causing invalid tag cloud API calls.
/// </summary>
public class StripHtmlExtensionMiddlewareTests
{
    [Theory]
    [InlineData("/github-copilot/features.html", "/github-copilot/features")]
    [InlineData("/github-copilot/getting-started.html", "/github-copilot/getting-started")]
    [InlineData("/ai/news.html", "/ai/news")]
    [InlineData("/about.html", "/about")]
    [InlineData("/index.html", "/index")]
    public async Task PathWithHtmlExtension_RedirectsToCanonicalUrl(string inputPath, string expectedPath)
    {
        // Arrange
        var (middleware, context, _) = CreateMiddleware(path: inputPath);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be(expectedPath);
    }

    [Theory]
    [InlineData("/github-copilot/features.HTML", "/github-copilot/features")]
    [InlineData("/ai/news.Html", "/ai/news")]
    [InlineData("/about.HTML", "/about")]
    public async Task PathWithHtmlExtension_CaseInsensitive(string inputPath, string expectedPath)
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
    public async Task PathWithHtmlExtension_PreservesQueryString()
    {
        // Arrange
        var (middleware, context, _) = CreateMiddleware(path: "/github-copilot/features.html", queryString: "?tags=copilot");

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be("/github-copilot/features?tags=copilot");
    }

    [Fact]
    public async Task PathWithHtmlExtension_DoesNotCallNext()
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware(path: "/features.html");

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled().Should().BeFalse("middleware should NOT call next when redirecting");
    }

    [Theory]
    [InlineData("/github-copilot/features")]
    [InlineData("/github-copilot/features/some-article")]
    [InlineData("/about")]
    [InlineData("/")]
    [InlineData("/file.css")]
    [InlineData("/file.js")]
    [InlineData("/image.png")]
    public async Task PathWithoutHtmlExtension_PassesThrough(string path)
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware(path: path);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled().Should().BeTrue("middleware should call next for non-.html paths");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status301MovedPermanently);
    }

    [Fact]
    public async Task RootHtmlPath_RedirectsToRoot()
    {
        // Arrange - edge case: path is exactly ".html" becomes empty after stripping
        var (middleware, context, _) = CreateMiddleware(path: "/.html");

        // Act
        await middleware.InvokeAsync(context);

        // Assert - stripping ".html" from "/.html" leaves "/" 
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be("/");
    }

    private static (StripHtmlExtensionMiddleware middleware, HttpContext context, Func<bool> nextCalled) CreateMiddleware(
        string path = "/",
        string? queryString = null)
    {
        var wasCalled = false;
        RequestDelegate next = _ =>
        {
            wasCalled = true;
            return Task.CompletedTask;
        };

        var middleware = new StripHtmlExtensionMiddleware(next);

        var context = new DefaultHttpContext();
        context.Request.Path = path;
        if (queryString != null)
        {
            context.Request.QueryString = new QueryString(queryString);
        }

        return (middleware, context, () => wasCalled);
    }
}
