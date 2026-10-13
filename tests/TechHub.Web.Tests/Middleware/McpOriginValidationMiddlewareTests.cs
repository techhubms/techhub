using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Web.MCP;
using TechHub.Web.Middleware;

namespace TechHub.Web.Tests.Middleware;

public class McpOriginValidationMiddlewareTests
{
    [Theory]
    [InlineData("https://tech.hub.ms")]
    [InlineData("https://tech.xebia.ms")]
    [InlineData("https://TECH.HUB.MS")]
    [InlineData("https://tech.hub.ms:443")]
    [InlineData("https://mcp-base.test")]
    public async Task InvokeAsync_TrustedOrigin_PassesThrough(string origin)
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware(origin: origin);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled().Should().BeTrue();
        context.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Theory]
    [InlineData("https://evil.example")]
    [InlineData("http://tech.hub.ms")]
    [InlineData("https://tech.hub.ms:8443")]
    [InlineData("https://ai.tech.hub.ms")]
    [InlineData("http://localhost:6274")]
    [InlineData("null")]
    [InlineData("")]
    public async Task InvokeAsync_UntrustedOrigin_Returns403(string origin)
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware(origin: origin);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled().Should().BeFalse();
        context.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    }

    [Fact]
    public async Task InvokeAsync_NoOriginHeader_PassesThrough()
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware(origin: null);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled().Should().BeTrue();
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/ai")]
    [InlineData("/mcpx")]
    public async Task InvokeAsync_NonMcpPath_IgnoresOrigin(string path)
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware(origin: "https://evil.example", path: path);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled().Should().BeTrue();
    }

    [Theory]
    [InlineData("/mcp/sub")]
    [InlineData("/MCP")]
    public async Task InvokeAsync_McpSubPathOrCasing_ValidatesOrigin(string path)
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware(origin: "https://evil.example", path: path);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled().Should().BeFalse();
        context.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    }

    [Theory]
    [InlineData("http://localhost:6274")]
    [InlineData("https://localhost:5003")]
    [InlineData("http://127.0.0.1:8080")]
    [InlineData("http://[::1]:3000")]
    public async Task InvokeAsync_LoopbackOriginInDevelopment_PassesThrough(string origin)
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware(origin: origin, environment: Environments.Development);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled().Should().BeTrue();
    }

    [Fact]
    public async Task InvokeAsync_UntrustedOriginInDevelopment_Returns403()
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware(origin: "https://evil.example", environment: Environments.Development);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled().Should().BeFalse();
        context.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    }

    private static (McpOriginValidationMiddleware Middleware, DefaultHttpContext Context, Func<bool> NextCalled) CreateMiddleware(
        string? origin,
        string path = "/mcp",
        string environment = "Production")
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["PrimaryHosts:0"] = "tech.xebia.ms",
                ["PrimaryHosts:1"] = "tech.hub.ms",
            })
            .Build();

        var hostEnvironment = new Mock<IHostEnvironment>();
        hostEnvironment.SetupGet(e => e.EnvironmentName).Returns(environment);

        var nextWasCalled = false;
        RequestDelegate next = _ =>
        {
            nextWasCalled = true;
            return Task.CompletedTask;
        };

        var middleware = new McpOriginValidationMiddleware(
            next,
            configuration,
            Options.Create(new McpOptions { BaseUrl = "https://mcp-base.test/" }),
            hostEnvironment.Object,
            NullLogger<McpOriginValidationMiddleware>.Instance);

        var context = new DefaultHttpContext();
        context.Request.Method = "POST";
        context.Request.Path = path;
        context.Response.Body = new MemoryStream();
        if (origin is not null)
        {
            context.Request.Headers.Origin = origin;
        }

        return (middleware, context, () => nextWasCalled);
    }
}
