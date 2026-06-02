using FluentAssertions;
using Microsoft.AspNetCore.Http;
using TechHub.Web.Middleware;

namespace TechHub.Web.Tests.Middleware;

public class HeadRequestMiddlewareTests
{
    // ─── Non-HEAD methods pass through unchanged ──────────────────────────────

    [Theory]
    [InlineData("GET")]
    [InlineData("POST")]
    [InlineData("PUT")]
    public async Task InvokeAsync_NonHeadMethod_CallsNextUnchanged(string method)
    {
        var (middleware, context, nextCalled) = CreateMiddleware(method, "/some/path");

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue();
        context.Request.Method.Should().Be(method);
    }

    // ─── Extension-less HEAD → 200 OK, no SSR ────────────────────────────────

    [Theory]
    [InlineData("/")]
    [InlineData("/ai")]
    [InlineData("/ai/videos/my-article-slug")]
    [InlineData("/all/authors/john-doe")]
    [InlineData("/about")]
    [InlineData("/admin/login")]
    [InlineData("/news/2024/some-post")]
    public async Task InvokeAsync_HeadExtensionlessPath_Returns200WithoutCallingNext(string path)
    {
        var (middleware, context, nextCalled) = CreateMiddleware("HEAD", path);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeFalse("SSR must be bypassed for extension-less HEAD requests");
        context.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        context.Response.ContentType.Should().Be("text/html; charset=utf-8");
    }

    // ─── Known minimal API endpoints must use the HEAD→GET rewrite path ───────

    [Theory]
    [InlineData("/version")]
    [InlineData("/health")]
    [InlineData("/alive")]
    public async Task InvokeAsync_HeadMinimalApiPath_CallsNextWithGetMethod(string path)
    {
        string? methodSeenByNext = null;
        RequestDelegate next = ctx =>
        {
            methodSeenByNext = ctx.Request.Method;
            return Task.CompletedTask;
        };
        var middleware = new HeadRequestMiddleware(next);
        var context = new DefaultHttpContext();
        context.Request.Method = "HEAD";
        context.Request.Path = path;

        await middleware.InvokeAsync(context);

        methodSeenByNext.Should().Be(HttpMethods.Get, "minimal API endpoints must see GET so their MapGet handlers match");
        context.Request.Method.Should().Be("HEAD", "method must be restored to HEAD after next() returns");
    }

    [Fact]
    public async Task InvokeAsync_HeadExtensionlessPath_MethodRemainsHead()
    {
        var (middleware, context, _) = CreateMiddleware("HEAD", "/ai/videos/article");

        await middleware.InvokeAsync(context);

        // Method must not be mutated — HEAD remains HEAD so callers see the correct method.
        context.Request.Method.Should().Be("HEAD");
    }

    // ─── File-extension HEAD → GET rewrite, body suppressed ──────────────────

    [Theory]
    [InlineData("/favicon.ico")]
    [InlineData("/robots.txt")]
    [InlineData("/js/mobile-nav.abc123.js")]
    [InlineData("/all/feed.xml")]
    [InlineData("/sitemap.xml")]
    [InlineData("/css/app.min.css")]
    public async Task InvokeAsync_HeadFileExtensionPath_CallsNextWithGetMethod(string path)
    {
        string? methodSeenByNext = null;
        RequestDelegate next = ctx =>
        {
            methodSeenByNext = ctx.Request.Method;
            return Task.CompletedTask;
        };
        var middleware = new HeadRequestMiddleware(next);
        var context = new DefaultHttpContext();
        context.Request.Method = "HEAD";
        context.Request.Path = path;

        await middleware.InvokeAsync(context);

        methodSeenByNext.Should().Be(HttpMethods.Get, "next must see GET so endpoint selection matches MapStaticAssets/MapGet");
    }

    [Theory]
    [InlineData("/favicon.ico")]
    [InlineData("/robots.txt")]
    [InlineData("/js/app.abc.js")]
    [InlineData("/all/feed.xml")]
    public async Task InvokeAsync_HeadFileExtensionPath_MethodRestoredToHeadAfterNext(string path)
    {
        var (middleware, context, nextCalled) = CreateMiddleware("HEAD", path);

        await middleware.InvokeAsync(context);

        nextCalled().Should().BeTrue();
        context.Request.Method.Should().Be("HEAD", "method must be restored to HEAD after next() returns");
    }

    [Fact]
    public async Task InvokeAsync_HeadFileExtensionPath_BodyIsStreamNullDuringNextAndRestoredAfter()
    {
        using var originalBody = new MemoryStream();
        Stream? bodySeenByNext = null;

        RequestDelegate next = ctx =>
        {
            bodySeenByNext = ctx.Response.Body;
            return Task.CompletedTask;
        };
        var middleware = new HeadRequestMiddleware(next);
        var context = new DefaultHttpContext();
        context.Request.Method = "HEAD";
        context.Request.Path = "/favicon.ico";
        context.Response.Body = originalBody;

        await middleware.InvokeAsync(context);

        bodySeenByNext.Should().BeSameAs(Stream.Null, "body must be Stream.Null while next executes so no bytes are written");
        context.Response.Body.Should().BeSameAs(originalBody, "original body must be restored after next() returns");
    }

    [Fact]
    public async Task InvokeAsync_HeadFileExtensionPath_BodyRestoredEvenWhenNextThrows()
    {
        using var originalBody = new MemoryStream();

        RequestDelegate next = _ => throw new InvalidOperationException("simulated error");
        var middleware = new HeadRequestMiddleware(next);
        var context = new DefaultHttpContext();
        context.Request.Method = "HEAD";
        context.Request.Path = "/favicon.ico";
        context.Response.Body = originalBody;

        await middleware.Invoking(m => m.InvokeAsync(context)).Should().ThrowAsync<InvalidOperationException>();

        context.Response.Body.Should().BeSameAs(originalBody, "body must be restored even when next() throws");
        context.Request.Method.Should().Be("HEAD", "method must be restored even when next() throws");
    }

    // ─── Constructor guard ────────────────────────────────────────────────────

    [Fact]
    public void Constructor_NullNext_ThrowsArgumentNullException()
    {
        var act = () => new HeadRequestMiddleware(null!);

        act.Should().Throw<ArgumentNullException>().WithParameterName("next");
    }

    // ─── Helper ───────────────────────────────────────────────────────────────

    private static (HeadRequestMiddleware middleware, HttpContext context, Func<bool> nextCalled) CreateMiddleware(
        string method = "GET",
        string path = "/")
    {
        var wasCalled = false;
        RequestDelegate next = _ =>
        {
            wasCalled = true;
            return Task.CompletedTask;
        };
        var middleware = new HeadRequestMiddleware(next);
        var context = new DefaultHttpContext();
        context.Request.Method = method;
        context.Request.Path = path;
        return (middleware, context, () => wasCalled);
    }
}
