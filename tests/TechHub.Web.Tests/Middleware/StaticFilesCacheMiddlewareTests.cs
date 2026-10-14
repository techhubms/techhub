using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using TechHub.Web.Middleware;

namespace TechHub.Web.Tests.Middleware;

/// <summary>
/// Tests for StaticFilesCacheMiddleware — Cache-Control policy per asset type.
/// Cache headers are applied through Response.OnStarting, so a real (test) server pipeline is used.
/// </summary>
public class StaticFilesCacheMiddlewareTests
{
    private const string ImmutableCacheControl = "public, max-age=31536000, immutable";
    private const string ShortCacheControl = "public, max-age=3600, must-revalidate";

    [Theory]
    [InlineData("/css/base.r2lq8zdogi.css")]
    [InlineData("/_framework/blazor.web.2i0r6wwb69.js")]
    [InlineData("/TechHub.Web.r2lq8zdogi.styles.css")]
    [InlineData("/data/export.abc1234567.txt")]
    public async Task InvokeAsync_FingerprintedAsset_GetsImmutableCache(string path)
    {
        // Arrange
        await using var app = await StartAppAsync();
        using var client = app.GetTestClient();

        // Act
        using var response = await client.GetAsync(path, TestContext.Current.CancellationToken);

        // Assert
        GetCacheControl(response).Should().Be(ImmutableCacheControl);
    }

    [Theory]
    [InlineData("/images/logo.png")]
    [InlineData("/images/photo.JPG")]
    [InlineData("/images/hero.webp")]
    [InlineData("/images/hero.jxl")]
    [InlineData("/images/icon.svg")]
    [InlineData("/favicon.ico")]
    [InlineData("/fonts/inter.woff2")]
    [InlineData("/fonts/inter.ttf")]
    public async Task InvokeAsync_ImageOrFont_GetsImmutableCache(string path)
    {
        // Arrange
        await using var app = await StartAppAsync();
        using var client = app.GetTestClient();

        // Act
        using var response = await client.GetAsync(path, TestContext.Current.CancellationToken);

        // Assert
        GetCacheControl(response).Should().Be(ImmutableCacheControl);
    }

    [Theory]
    [InlineData("/css/site.css")]
    [InlineData("/js/app.js")]
    [InlineData("/js/app.min.js")]
    [InlineData("/manifest.json")]
    [InlineData("/sitemap.xml")]
    [InlineData("/static/page.html")]
    public async Task InvokeAsync_NonFingerprintedTextAsset_GetsShortCacheWithRevalidation(string path)
    {
        // Arrange
        await using var app = await StartAppAsync();
        using var client = app.GetTestClient();

        // Act
        using var response = await client.GetAsync(path, TestContext.Current.CancellationToken);

        // Assert
        GetCacheControl(response).Should().Be(ShortCacheControl);
    }

    [Theory]
    [InlineData("/js/app.abc123.js", "segment shorter than 10 characters")]
    [InlineData("/js/app.ABCDEFGHIJ.js", "uppercase characters are not a fingerprint")]
    [InlineData("/js/app.abc-defghij.js", "hyphen is not a fingerprint character")]
    public async Task InvokeAsync_NotAFingerprint_FallsBackToShortCache(string path, string reason)
    {
        // Arrange
        await using var app = await StartAppAsync();
        using var client = app.GetTestClient();

        // Act
        using var response = await client.GetAsync(path, TestContext.Current.CancellationToken);

        // Assert
        GetCacheControl(response).Should().Be(ShortCacheControl, reason);
    }

    [Theory]
    [InlineData("/data/readme.txt")]
    [InlineData("/downloads/archive.zip")]
    public async Task InvokeAsync_OtherExtension_LeavesCacheControlUntouched(string path)
    {
        // Arrange
        await using var app = await StartAppAsync();
        using var client = app.GetTestClient();

        // Act
        using var response = await client.GetAsync(path, TestContext.Current.CancellationToken);

        // Assert
        GetCacheControl(response).Should().BeNull();
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/ai")]
    [InlineData("/ai/videos/my-article")]
    public async Task InvokeAsync_PathWithoutExtension_LeavesCacheControlUntouched(string path)
    {
        // Arrange
        await using var app = await StartAppAsync();
        using var client = app.GetTestClient();

        // Act
        using var response = await client.GetAsync(path, TestContext.Current.CancellationToken);

        // Assert
        GetCacheControl(response).Should().BeNull();
    }

    [Theory]
    [InlineData("/css/base.r2lq8zdogi.css?status=404", 404)]
    [InlineData("/images/logo.png?status=404", 404)]
    [InlineData("/js/app.js?status=500", 500)]
    public async Task InvokeAsync_NonOkResponse_LeavesCacheControlUntouched(string pathAndQuery, int expectedStatus)
    {
        // Arrange
        await using var app = await StartAppAsync();
        using var client = app.GetTestClient();

        // Act
        using var response = await client.GetAsync(pathAndQuery, TestContext.Current.CancellationToken);

        // Assert
        ((int)response.StatusCode).Should().Be(expectedStatus);
        GetCacheControl(response).Should().Be("no-cache", "error responses must keep the handler's own cache policy");
    }

    [Fact]
    public async Task InvokeAsync_ImmutableAsset_OverridesCacheControlSetByDownstream()
    {
        // Arrange
        await using var app = await StartAppAsync();
        using var client = app.GetTestClient();

        // Act
        using var response = await client.GetAsync("/images/logo.png?downstreamCache=1", TestContext.Current.CancellationToken);

        // Assert
        GetCacheControl(response).Should().Be(ImmutableCacheControl);
    }

    [Theory]
    [InlineData("/css/base.r2lq8zdogi.css")]
    [InlineData("/images/logo.png")]
    public async Task InvokeAsync_ImmutableAsset_RemovesVaryHeader(string path)
    {
        // Arrange
        await using var app = await StartAppAsync();
        using var client = app.GetTestClient();

        // Act
        using var response = await client.GetAsync(path + "?vary=1", TestContext.Current.CancellationToken);

        // Assert
        response.Headers.Contains("Vary").Should().BeFalse("a Vary header would prevent long-term caching");
    }

    [Fact]
    public async Task InvokeAsync_ShortCacheAsset_KeepsVaryHeader()
    {
        // Arrange
        await using var app = await StartAppAsync();
        using var client = app.GetTestClient();

        // Act
        using var response = await client.GetAsync("/js/app.js?vary=1", TestContext.Current.CancellationToken);

        // Assert
        response.Headers.Contains("Vary").Should().BeTrue();
    }

    [Fact]
    public void Constructor_NullNext_Throws()
    {
        // Act
        var act = () => new StaticFilesCacheMiddleware(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    private static string? GetCacheControl(HttpResponseMessage response)
    {
        // NonValidated keeps the raw header text; the typed CacheControl parser would reorder directives.
        return response.Headers.NonValidated.TryGetValues("Cache-Control", out var values) ? values.ToString() : null;
    }

    private static async Task<WebApplication> StartAppAsync()
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        var app = builder.Build();

        app.UseStaticFilesCaching();
        app.Run(async context =>
        {
            var query = context.Request.Query;
            if (int.TryParse(query["status"], out var status))
            {
                context.Response.StatusCode = status;
                context.Response.Headers.CacheControl = "no-cache";
            }

            if (query.ContainsKey("downstreamCache"))
            {
                context.Response.Headers.CacheControl = "public, max-age=60";
            }

            if (query.ContainsKey("vary"))
            {
                context.Response.Headers.Vary = "Accept-Encoding";
            }

            await context.Response.WriteAsync("body");
        });

        await app.StartAsync();
        return app;
    }
}
