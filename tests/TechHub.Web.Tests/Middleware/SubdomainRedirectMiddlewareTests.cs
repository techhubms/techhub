using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Web.Middleware;

namespace TechHub.Web.Tests.Middleware;

/// <summary>
/// Tests for SubdomainRedirectMiddleware - subdomain shortcut redirect logic.
/// Verifies that requests to shortcut subdomains (e.g., ghc.xebia.ms) are redirected
/// to the configured section path (e.g., /github-copilot).
/// </summary>
public class SubdomainRedirectMiddlewareTests
{
    [Theory]
    [InlineData("ghc.xebia.ms", "https://tech.xebia.ms/github-copilot")]
    [InlineData("ai.xebia.ms", "https://tech.xebia.ms/ai")]
    [InlineData("github-copilot.xebia.ms", "https://tech.xebia.ms/github-copilot")]
    [InlineData("copilot.xebia.ms", "https://tech.xebia.ms/github-copilot")]
    [InlineData("dotnet.xebia.ms", "https://tech.xebia.ms/dotnet")]
    [InlineData("coding.xebia.ms", "https://tech.xebia.ms/dotnet")]
    [InlineData("csharp.xebia.ms", "https://tech.xebia.ms/dotnet")]
    [InlineData("sharp.xebia.ms", "https://tech.xebia.ms/dotnet")]
    [InlineData("ml.xebia.ms", "https://tech.xebia.ms/ml")]
    [InlineData("data.xebia.ms", "https://tech.xebia.ms/ml")]
    [InlineData("machine-learning.xebia.ms", "https://tech.xebia.ms/ml")]
    [InlineData("devops.xebia.ms", "https://tech.xebia.ms/devops")]
    [InlineData("dx.xebia.ms", "https://tech.xebia.ms/devops")]
    [InlineData("azure.xebia.ms", "https://tech.xebia.ms/azure")]
    [InlineData("cloud.xebia.ms", "https://tech.xebia.ms/azure")]
    [InlineData("security.xebia.ms", "https://tech.xebia.ms/security")]
    [InlineData("sec.xebia.ms", "https://tech.xebia.ms/security")]
    public async Task SubdomainShortcut_RedirectsToConfiguredSection(string host, string expectedPath)
    {
        // Arrange
        var (middleware, context, _) = CreateMiddleware(host);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be(expectedPath);
    }

    [Theory]
    [InlineData("ghc.hub.ms", "https://tech.hub.ms/github-copilot")]
    [InlineData("ai.hub.ms", "https://tech.hub.ms/ai")]
    [InlineData("coding.hub.ms", "https://tech.hub.ms/dotnet")]
    public async Task SubdomainShortcut_WorksForAnyDomain(string host, string expectedPath)
    {
        // Arrange
        var (middleware, context, _) = CreateMiddleware(host);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be(expectedPath);
    }

    [Theory]
    [InlineData("GHC.XEBIA.MS", "https://tech.xebia.ms/github-copilot")]
    [InlineData("AI.Hub.MS", "https://tech.hub.ms/ai")]
    public async Task SubdomainShortcut_CaseInsensitive(string host, string expectedPath)
    {
        // Arrange
        var (middleware, context, _) = CreateMiddleware(host);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be(expectedPath);
    }

    [Theory]
    [InlineData("tech.xebia.ms")]
    [InlineData("tech.hub.ms")]
    [InlineData("localhost")]
    [InlineData("localhost:5003")]
    public async Task PrimaryHost_PassesThrough(string host)
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware(host);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled().Should().BeTrue("middleware should call next for primary hosts");
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status301MovedPermanently);
    }

    [Theory]
    [InlineData("unknown.xebia.ms", "https://tech.xebia.ms")]
    [InlineData("random.hub.ms", "https://tech.hub.ms")]
    [InlineData("test.xebia.ms", "https://tech.xebia.ms")]
    public async Task UnknownSubdomain_RedirectsToPrimaryHost(string host, string expectedRedirectBase)
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware(host);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled().Should().BeFalse("middleware should redirect, not pass through");
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be(expectedRedirectBase);
    }

    [Fact]
    public async Task UnknownSubdomain_PreservesPathAndQueryString()
    {
        // Arrange
        var (middleware, context, _) = CreateMiddleware("unknown.xebia.ms", path: "/some/page", queryString: "?q=test");

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be("https://tech.xebia.ms/some/page?q=test");
    }

    [Fact]
    public async Task UnknownSubdomain_OnUnknownDomain_PassesThrough()
    {
        // Arrange - a domain not in PrimaryHosts
        var (middleware, context, nextCalled) = CreateMiddleware("random.example.com");

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled().Should().BeTrue("middleware should pass through for completely unknown domains");
    }

    [Fact]
    public async Task SubdomainShortcut_PreservesQueryString()
    {
        // Arrange
        var (middleware, context, _) = CreateMiddleware("ai.xebia.ms", queryString: "?tag=copilot");

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be("https://tech.xebia.ms/ai?tag=copilot");
    }

    [Fact]
    public async Task SubdomainShortcut_PreservesPath()
    {
        // Arrange
        var (middleware, context, _) = CreateMiddleware("ai.xebia.ms", path: "/news");

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be("https://tech.xebia.ms/ai/news");
    }

    [Fact]
    public async Task SubdomainShortcut_PreservesPathAndQueryString()
    {
        // Arrange
        var (middleware, context, _) = CreateMiddleware("ghc.xebia.ms", path: "/blogs", queryString: "?tag=vscode");

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status301MovedPermanently);
        context.Response.Headers.Location.ToString().Should().Be("https://tech.xebia.ms/github-copilot/blogs?tag=vscode");
    }

    [Fact]
    public async Task NoShortcutsConfigured_PassesThrough()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>())
            .Build();

        var nextCalled = false;
        RequestDelegate next = _ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };

        var middleware = new SubdomainRedirectMiddleware(next, configuration, NullLogger<SubdomainRedirectMiddleware>.Instance);
        var context = new DefaultHttpContext();
        context.Request.Host = new HostString("ghc.xebia.ms");

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled.Should().BeTrue("middleware should pass through when no shortcuts configured");
    }

    [Fact]
    public async Task SubdomainShortcut_DoesNotCallNext()
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware("ghc.xebia.ms");

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled().Should().BeFalse("middleware should NOT call next when redirecting");
    }

    private static (SubdomainRedirectMiddleware middleware, HttpContext context, Func<bool> nextCalled) CreateMiddleware(
        string host,
        string path = "/",
        string? queryString = null)
    {
        var shortcuts = new Dictionary<string, string?>
        {
            ["SubdomainShortcuts:ghc"] = "github-copilot",
            ["SubdomainShortcuts:github-copilot"] = "github-copilot",
            ["SubdomainShortcuts:copilot"] = "github-copilot",
            ["SubdomainShortcuts:ai"] = "ai",
            ["SubdomainShortcuts:ml"] = "ml",
            ["SubdomainShortcuts:data"] = "ml",
            ["SubdomainShortcuts:machine-learning"] = "ml",
            ["SubdomainShortcuts:devops"] = "devops",
            ["SubdomainShortcuts:dx"] = "devops",
            ["SubdomainShortcuts:azure"] = "azure",
            ["SubdomainShortcuts:cloud"] = "azure",
            ["SubdomainShortcuts:coding"] = "dotnet",
            ["SubdomainShortcuts:dotnet"] = "dotnet",
            ["SubdomainShortcuts:csharp"] = "dotnet",
            ["SubdomainShortcuts:sharp"] = "dotnet",
            ["SubdomainShortcuts:security"] = "security",
            ["SubdomainShortcuts:sec"] = "security",
            ["PrimaryHosts:0"] = "tech.xebia.ms",
            ["PrimaryHosts:1"] = "tech.hub.ms"
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(shortcuts)
            .Build();

        var nextWasCalled = false;
        RequestDelegate next = _ =>
        {
            nextWasCalled = true;
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

        return (middleware, context, () => nextWasCalled);
    }
}
