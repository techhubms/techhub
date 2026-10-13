using FluentAssertions;
using Microsoft.AspNetCore.Http;
using TechHub.Web.Middleware;

namespace TechHub.Web.Tests.Middleware;

public class McpStreamRequestRejectionMiddlewareTests
{
    [Theory]
    [InlineData("text/event-stream")]
    [InlineData("application/json, text/event-stream")]
    [InlineData("TEXT/EVENT-STREAM")]
    public async Task InvokeAsync_GetMcpWithEventStreamAccept_Returns405WithBody(string accept)
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware(accept: accept);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled().Should().BeFalse();
        context.Response.StatusCode.Should().Be(StatusCodes.Status405MethodNotAllowed);
        context.Response.Headers.Allow.ToString().Should().Be("POST");
        context.Response.Body.Length.Should().BeGreaterThan(0, "an empty body would be replaced by the not-found page");
    }

    [Theory]
    [InlineData("text/html,application/xhtml+xml,*/*;q=0.8")]
    [InlineData("text/html, text/event-stream")]
    [InlineData("*/*")]
    [InlineData("application/json")]
    [InlineData(null)]
    public async Task InvokeAsync_GetMcpFromBrowserOrUnspecifiedClient_PassesThrough(string? accept)
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware(accept: accept);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled().Should().BeTrue();
    }

    [Theory]
    [InlineData("POST", "/mcp")]
    [InlineData("GET", "/mcp/sub")]
    [InlineData("GET", "/mcpx")]
    [InlineData("GET", "/ai")]
    public async Task InvokeAsync_OtherRequests_PassThrough(string method, string path)
    {
        // Arrange
        var (middleware, context, nextCalled) = CreateMiddleware(accept: "text/event-stream", method: method, path: path);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled().Should().BeTrue();
    }

    private static (McpStreamRequestRejectionMiddleware Middleware, DefaultHttpContext Context, Func<bool> NextCalled) CreateMiddleware(
        string? accept,
        string method = "GET",
        string path = "/mcp")
    {
        var nextCalled = false;
        var middleware = new McpStreamRequestRejectionMiddleware(_ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        });

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.Request.Method = method;
        context.Request.Path = path;
        if (accept is not null)
        {
            context.Request.Headers.Accept = accept;
        }

        return (middleware, context, () => nextCalled);
    }
}
