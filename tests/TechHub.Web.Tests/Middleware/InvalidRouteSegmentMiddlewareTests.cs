using FluentAssertions;
using Microsoft.AspNetCore.Http;
using TechHub.Web.Middleware;

namespace TechHub.Web.Tests.Middleware;

/// <summary>
/// Tests for InvalidRouteSegmentMiddleware.
///
/// The middleware enforces two layers of rejection:
///   1. Scanner/attacker probes (wp-admin, xmlrpc.php, .env, etc.) → 404, next not called.
///   2. Structurally invalid first segments (digits, percent-encoding, underscores, etc.) → 404.
///
/// Paths whose final segment contains a dot pass through after the probe filter, so
/// legitimate static files and RSS/sitemap .xml endpoints are served downstream.
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
    // WordPress probes
    [InlineData("/wp-admin")]
    [InlineData("/wp-admin/admin.php")]
    [InlineData("/wp-admin/setup-config.php")]
    [InlineData("/wp-content/themes/twenty/style.css")]
    [InlineData("/wp-includes/functions.php")]
    [InlineData("/wp-login.php")]
    // WordPress XML-RPC
    [InlineData("/xmlrpc.php")]
    // PHP admin panels
    [InlineData("/phpmyadmin")]
    [InlineData("/phpmyadmin/index.php")]
    // CGI / Spring Boot
    [InlineData("/cgi-bin/test.cgi")]
    [InlineData("/actuator")]
    [InlineData("/actuator/health")]
    // Script extensions
    [InlineData("/setup.php")]
    [InlineData("/config.asp")]
    [InlineData("/install.aspx")]
    // Config / credential files
    [InlineData("/.env")]
    [InlineData("/.htaccess")]
    [InlineData("/server/.htpasswd")]
    // Backup files
    [InlineData("/backup.sql")]
    [InlineData("/database.bak")]
    // Archives
    [InlineData("/site.zip")]
    [InlineData("/export.tar.gz")]
    // Keys / certs
    [InlineData("/private.key")]
    [InlineData("/cert.pem")]
    // .xml that is NOT a feed or sitemap
    [InlineData("/wp-content/plugins/foo.xml")]
    [InlineData("/random.xml")]
    [InlineData("/some/evil.xml")]
    public async Task ProbeRequest_Returns404_WithoutCallingNext(string path)
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware(path: path);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        nextCalled().Should().BeFalse("probe requests must be rejected without touching downstream middleware");
    }

    [Theory]
    [InlineData("/wp-admin")]
    [InlineData("/setup.php")]
    [InlineData("/.env")]
    [InlineData("/xmlrpc.php")]
    [InlineData("/random.xml")]
    [InlineData("/all/feed.xml", false)]  // feed.xml is NOT a probe
    [InlineData("/sitemap.xml", false)]   // sitemap.xml is NOT a probe
    [InlineData("/github-copilot/feed.xml", false)]
    [InlineData("/all/roundups/feed.xml", false)]
    [InlineData("/config.json", false)]   // .json is not a probe extension
    [InlineData("/", false)]
    [InlineData("/ai", false)]
    public void IsProbeRequest_IdentifiesProbesCorrectly(string path, bool expected = true)
    {
        InvalidRouteSegmentMiddleware.IsProbeRequest(path).Should().Be(expected);
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
    // Unknown section — structurally valid, caught downstream
    [InlineData("/fake-section")]
    // Static file requests: final segment has a dot — pass through after probe filter
    [InlineData("/config.json")]
    [InlineData("/TechHub.Web.fwv5rmn5un.styles.css")]
    [InlineData("/css/cards.kh78ex0xwt.css")]
    [InlineData("/js/nav-helpers.fmmqw6nflr.js")]
    // RSS and sitemap .xml endpoints (allowed through the .xml probe exception)
    [InlineData("/github-copilot/feed.xml")]
    [InlineData("/all/feed.xml")]
    [InlineData("/all/roundups/feed.xml")]
    [InlineData("/sitemap.xml")]
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
