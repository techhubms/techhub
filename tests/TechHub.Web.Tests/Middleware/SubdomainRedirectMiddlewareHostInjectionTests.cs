using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Web.Middleware;

namespace TechHub.Web.Tests.Middleware;

/// <summary>
/// Host-header injection and open-redirect tests for SubdomainRedirectMiddleware.
/// Redirect targets must only ever use the configured primary hosts, never the attacker-supplied Host header or path.
/// </summary>
public class SubdomainRedirectMiddlewareHostInjectionTests
{
    private static readonly string[] _allowedRedirectHosts = ["tech.xebia.ms", "tech.hub.ms"];

    [Theory]
    [InlineData("evil.com")]
    [InlineData("ghc.evil.com")]
    [InlineData("ghc.xebia.ms.evil.com")]
    [InlineData("tech.xebia.ms.evil.com")]
    [InlineData("www.tech.xebia.ms.evil.com")]
    [InlineData("evil.com.xebia.ms")]
    [InlineData("a.b.xebia.ms")]
    [InlineData("xebia.ms.attacker.example")]
    public async Task InvokeAsync_HostNotUnderConfiguredDomain_PassesThroughWithoutRedirect(string host)
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware(host);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled().Should().BeTrue();
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().BeEmpty();
    }

    [Theory]
    [InlineData("evil.xebia.ms")]
    [InlineData("ghc.xebia.ms")]
    [InlineData("ghc.hub.ms")]
    [InlineData("EVIL.HUB.MS")]
    [InlineData("www.tech.hub.ms")]
    [InlineData("WWW.TECH.XEBIA.MS")]
    [InlineData("ghc.xebia.ms:8080")]
    public async Task InvokeAsync_Redirect_AlwaysTargetsConfiguredPrimaryHost(string host)
    {
        // Arrange
        var (middleware, context, _) = CreateMiddleware(host);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        AssertRedirectsToPrimaryHost(context);
    }

    [Theory]
    [InlineData("//evil.com")]
    [InlineData("//evil.com/path")]
    [InlineData("/\\evil.com")]
    [InlineData("/@evil.com")]
    [InlineData("/.evil.com")]
    [InlineData("/https://evil.com")]
    public async Task InvokeAsync_ShortcutRedirect_HostileRequestPath_StaysOnPrimaryHost(string path)
    {
        // Arrange
        var (middleware, context, _) = CreateMiddleware("ghc.xebia.ms", path);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        AssertRedirectsToPrimaryHost(context);
    }

    [Theory]
    [InlineData("//evil.com")]
    [InlineData("/@evil.com")]
    [InlineData("/\\evil.com")]
    public async Task InvokeAsync_WwwRedirect_HostileRequestPath_StaysOnPrimaryHost(string path)
    {
        // Arrange
        var (middleware, context, _) = CreateMiddleware("www.tech.hub.ms", path);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        AssertRedirectsToPrimaryHost(context);
    }

    [Theory]
    [InlineData("ghc.xebia.ms")]
    [InlineData("evil.xebia.ms")]
    [InlineData("www.tech.hub.ms")]
    public async Task InvokeAsync_Redirect_QueryStringWithExternalUrl_StaysOnPrimaryHost(string host)
    {
        // Arrange
        var (middleware, context, _) = CreateMiddleware(host, queryString: "?next=https://evil.com&host=evil.com");

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        AssertRedirectsToPrimaryHost(context);
    }

    [Fact]
    public async Task InvokeAsync_UnknownSubdomain_LocationNeverContainsAttackerHost()
    {
        // Arrange
        var (middleware, context, _) = CreateMiddleware("evil.xebia.ms", "/some/page", "?q=1");

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.Headers.Location.ToString().Should().NotContain("evil");
    }

    [Fact]
    public async Task InvokeAsync_WwwRedirect_UsesCanonicalLowercasePrimaryHost()
    {
        // Arrange
        var (middleware, context, _) = CreateMiddleware("WWW.TECH.HUB.MS", "/ai");

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.Headers.Location.ToString().Should().Be("https://tech.hub.ms/ai");
    }

    private static void AssertRedirectsToPrimaryHost(HttpContext context)
    {
        var location = context.Response.Headers.Location.ToString();
        Uri.TryCreate(location, UriKind.Absolute, out var uri).Should().BeTrue($"'{location}' must be an absolute URL");
        uri!.Scheme.Should().Be("https");
        uri.Host.Should().BeOneOf(_allowedRedirectHosts);
        uri.UserInfo.Should().BeEmpty();
        uri.IsDefaultPort.Should().BeTrue("the attacker-supplied port must not be reflected");
    }

    private static (SubdomainRedirectMiddleware middleware, HttpContext context, Func<bool> nextCalled) CreateMiddleware(
        string host,
        string path = "/",
        string? queryString = null)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["SubdomainShortcuts:ghc"] = "github-copilot",
                ["SubdomainShortcuts:ai"] = "ai",
                ["PrimaryHosts:0"] = "tech.xebia.ms",
                ["PrimaryHosts:1"] = "tech.hub.ms",
                ["PassthroughSubdomains:0"] = "staging-tech",
            })
            .Build();

        var wasCalled = false;
        RequestDelegate next = _ =>
        {
            wasCalled = true;
            return Task.CompletedTask;
        };

        var middleware = new SubdomainRedirectMiddleware(next, configuration, NullLogger<SubdomainRedirectMiddleware>.Instance);

        var context = new DefaultHttpContext();
        context.Request.Host = new HostString(host);
        context.Request.Path = path;
        if (queryString != null)
        {
            context.Request.QueryString = new QueryString(queryString);
        }

        return (middleware, context, () => wasCalled);
    }
}
