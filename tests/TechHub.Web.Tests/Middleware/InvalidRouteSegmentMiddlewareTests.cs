using FluentAssertions;
using Microsoft.AspNetCore.Http;
using TechHub.Web.Middleware;

namespace TechHub.Web.Tests.Middleware;

/// <summary>
/// Tests for InvalidRouteSegmentMiddleware - rejects requests whose first path segment
/// is structurally invalid (contains digits at start, percent-encoding, underscores, etc.)
/// before Blazor routing runs. Returns a bare 404 directly.
/// Paths whose final segment contains a dot (static file requests) always pass through.
/// Section/collection name validation is case-insensitive, so "/AI" and "/GitHub-Copilot"
/// pass through just like their lowercase equivalents.
/// </summary>
public class InvalidRouteSegmentMiddlewareTests
{
    [Theory]
    // Digits at start of segment
    [InlineData("/123abc")]
    [InlineData("/2024-something")]
    // Underscores or special chars
    [InlineData("/bad_segment")]
    [InlineData("/bad%20segment")]
    public async Task InvalidFirstSegment_Returns404(string path)
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware(path: path);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        nextCalled().Should().BeFalse("middleware should not call next for invalid segments");
    }

    [Theory]
    [InlineData("/github-copilot")]
    [InlineData("/GitHub-Copilot")]
    [InlineData("/AI")]
    [InlineData("/github-copilot/features")]
    [InlineData("/github-copilot/features/some-article")]
    [InlineData("/all")]
    [InlineData("/all/roundups/weekly-ai-and-tech-news-roundup-2026-03-16")]
    [InlineData("/not-found")]
    [InlineData("/about")]
    [InlineData("/ai")]
    [InlineData("/")]
    // Blazor internals: _-prefixed paths must always pass through
    [InlineData("/_blazor")]
    [InlineData("/_blazor/negotiate")]
    [InlineData("/_framework/blazor.server.js")]
    [InlineData("/_content/some-lib/styles.css")]
    // Structurally valid but unknown to section cache — caught downstream by KnownSectionFilter
    [InlineData("/wp-admin")]
    [InlineData("/wp-admin/admin.php")]
    [InlineData("/fake-section")]
    // Static file requests: final segment has a dot — always pass through regardless of
    // what the first segment looks like (MapStaticAssets handles these downstream)
    [InlineData("/.env")]
    [InlineData("/config.json")]
    [InlineData("/TechHub.Web.fwv5rmn5un.styles.css")]
    [InlineData("/css/cards.kh78ex0xwt.css")]
    [InlineData("/js/nav-helpers.fmmqw6nflr.js")]
    [InlineData("/github-copilot/feed.xml")]
    public async Task ValidSegment_Or_StaticFile_PassesThrough(string path)
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware(path: path);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled().Should().BeTrue("middleware should call next for valid segments");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task ValidPath_PreservesQueryString()
    {
        // Arrange — query string must not affect pass-through behaviour
        var (middleware, context, nextCalled) = CreateMiddleware(
            path: "/github-copilot/features",
            queryString: "?tags=copilot");

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled().Should().BeTrue();
    }

    private static (InvalidRouteSegmentMiddleware middleware, HttpContext context, Func<bool> nextCalled) CreateMiddleware(
        string path = "/",
        string? queryString = null)
    {
        var wasCalled = false;
        RequestDelegate next = _ =>
        {
            wasCalled = true;
            return Task.CompletedTask;
        };

        var middleware = new InvalidRouteSegmentMiddleware(next);

        var context = new DefaultHttpContext();
        context.Request.Path = path;
        if (queryString != null)
        {
            context.Request.QueryString = new QueryString(queryString);
        }

        return (middleware, context, () => wasCalled);
    }
}
