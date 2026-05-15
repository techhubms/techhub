using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Moq;
using TechHub.Web.Middleware;

namespace TechHub.Web.Tests.Middleware;

/// <summary>
/// Tests for HttpMethodFilterMiddleware.
///
/// Key behaviours verified:
///   - GET and HEAD are always allowed, regardless of path.
///   - POST is allowed only for /_blazor/*, /MicrosoftIdentity/*, /signin-oidc, /admin/logout.
///   - POST to any other path returns 405.
///   - All other methods (OPTIONS, PUT, DELETE, PATCH, etc.) always return 405.
/// </summary>
public class HttpMethodFilterMiddlewareTests
{
    // ── GET and HEAD are always allowed ─────────────────────────────────────

    [Theory]
    [InlineData("GET", "/")]
    [InlineData("GET", "/ai")]
    [InlineData("GET", "/ai/videos/my-article")]
    [InlineData("GET", "/bla")]
    [InlineData("GET", "/wp-admin")]
    [InlineData("HEAD", "/")]
    [InlineData("HEAD", "/ai/videos/my-article")]
    public async Task Get_And_Head_AlwaysPassThrough(string method, string path)
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware(method: method, path: path);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled().Should().BeTrue($"{method} {path} must always pass through");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status405MethodNotAllowed);
    }

    // ── POST is allowed only for known endpoints ─────────────────────────────

    [Theory]
    [InlineData("/_blazor")]
    [InlineData("/_blazor/negotiate")]
    [InlineData("/_blazor/anything/else")]
    [InlineData("/MicrosoftIdentity")]
    [InlineData("/MicrosoftIdentity/Account/SignIn")]
    [InlineData("/microsoftidentity/account/signin")]  // case-insensitive
    [InlineData("/signin-oidc")]
    [InlineData("/SIGNIN-OIDC")]                       // case-insensitive
    [InlineData("/admin/logout")]
    [InlineData("/Admin/Logout")]                      // case-insensitive
    public async Task Post_AllowedPaths_PassThrough(string path)
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware(method: "POST", path: path);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled().Should().BeTrue($"POST {path} must pass through to downstream middleware");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status405MethodNotAllowed);
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/ai")]
    [InlineData("/ai/videos/my-article")]
    [InlineData("/admin")]
    [InlineData("/admin/login")]
    [InlineData("/admin/content")]
    [InlineData("/about")]
    [InlineData("/api/something")]
    public async Task Post_DisallowedPaths_Returns405(string path)
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware(method: "POST", path: path);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status405MethodNotAllowed,
            $"POST {path} must be rejected — only specific paths accept POST");
        nextCalled().Should().BeFalse();
    }

    // ── WebSocket upgrades always pass regardless of method ──────────────────

    [Fact]
    public async Task WebSocketUpgrade_Http2Connect_PassesThrough()
    {
        // HTTP/2 WebSocket (RFC 8441) uses CONNECT + :protocol=websocket.
        // This is what browsers use for wss:// connections over HTTP/2 (e.g. the Blazor
        // SignalR circuit). Blocking CONNECT would break all Blazor Server pages.
        var (middleware, context, nextCalled) = CreateMiddleware(method: "CONNECT", path: "/_blazor");
        SetupHttp2WebSocketFeatures(context);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue("HTTP/2 WebSocket upgrades must pass through to the SignalR hub");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status405MethodNotAllowed);
    }

    [Fact]
    public async Task Connect_WithoutWebSocketUpgrade_Returns405()
    {
        // Plain CONNECT (not a WebSocket upgrade) is still blocked — it has no legitimate
        // use on this site and is commonly used by proxy tunnelling probes.
        var (middleware, context, nextCalled) = CreateMiddleware(method: "CONNECT", path: "/_blazor");
        // No WebSocket features set → IsWebSocketRequest = false

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status405MethodNotAllowed,
            "CONNECT without WebSocket upgrade headers must be rejected");
        nextCalled().Should().BeFalse();
    }

    // ── All other methods are always blocked ─────────────────────────────────

    [Theory]
    [InlineData("OPTIONS")]
    [InlineData("PUT")]
    [InlineData("DELETE")]
    [InlineData("PATCH")]
    [InlineData("CONNECT")]
    [InlineData("TRACE")]
    public async Task UnsupportedMethods_AlwaysReturn405(string method)
    {
        // Arrange — even a well-known path should be blocked for disallowed methods
        var (middleware, context, nextCalled) = CreateMiddleware(method: method, path: "/ai");

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status405MethodNotAllowed,
            $"{method} is never valid and must always be rejected");
        nextCalled().Should().BeFalse();
    }

    [Theory]
    [InlineData("OPTIONS", "/_blazor")]
    [InlineData("PUT", "/admin/logout")]
    [InlineData("DELETE", "/signin-oidc")]
    [InlineData("PATCH", "/MicrosoftIdentity/Account/SignIn")]
    public async Task UnsupportedMethod_EvenOnPostAllowedPath_Returns405(string method, string path)
    {
        // POST-allowed paths must not become a backdoor for other methods.
        var (middleware, context, nextCalled) = CreateMiddleware(method: method, path: path);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status405MethodNotAllowed,
            $"{method} {path} must be blocked — only POST (and GET/HEAD) are allowed on this path");
        nextCalled().Should().BeFalse();
    }

    // ── IsPostAllowed helper ─────────────────────────────────────────────────

    [Theory]
    [InlineData("/_blazor", true)]
    [InlineData("/_blazor/negotiate", true)]
    [InlineData("/MicrosoftIdentity", true)]
    [InlineData("/MicrosoftIdentity/Account/SignOut", true)]
    [InlineData("/signin-oidc", true)]
    [InlineData("/admin/logout", true)]
    [InlineData("/admin", false)]
    [InlineData("/admin/login", false)]
    [InlineData("/ai", false)]
    [InlineData("/", false)]
    public void IsPostAllowed_ReturnsExpected(string path, bool expected)
    {
        HttpMethodFilterMiddleware.IsPostAllowed(path).Should().Be(expected);
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static (HttpMethodFilterMiddleware middleware, HttpContext context, Func<bool> nextCalled) CreateMiddleware(
        string method = "GET",
        string path = "/")
    {
        var wasCalled = false;
        RequestDelegate next = _ =>
        {
            wasCalled = true;
            return Task.CompletedTask;
        };

        var middleware = new HttpMethodFilterMiddleware(next);

        var context = new DefaultHttpContext();
        context.Request.Method = method;
        context.Request.Path = path;

        return (middleware, context, () => wasCalled);
    }

    /// <summary>
    /// Configures a <see cref="DefaultHttpContext"/> to look like an HTTP/2 WebSocket upgrade
    /// (CONNECT + :protocol=websocket, RFC 8441). This makes
    /// <c>context.WebSockets.IsWebSocketRequest</c> return <c>true</c>.
    /// </summary>
    private static void SetupHttp2WebSocketFeatures(HttpContext context)
    {
        // IHttpExtendedConnectFeature signals HTTP/2 extended CONNECT (RFC 8441).
        var connectFeature = new Mock<IHttpExtendedConnectFeature>();
        connectFeature.Setup(f => f.IsExtendedConnect).Returns(true);
        connectFeature.Setup(f => f.Protocol).Returns("websocket");
        context.Features.Set<IHttpExtendedConnectFeature>(connectFeature.Object);
    }
}
