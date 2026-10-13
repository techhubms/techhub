using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using TechHub.Web.Middleware;

namespace TechHub.Web.Tests.Middleware;

/// <summary>
/// Tests for SecurityHeadersMiddleware — headers are applied through Response.OnStarting,
/// so they must be verified through a real (test) server pipeline rather than a bare HttpContext.
/// </summary>
public class SecurityHeadersMiddlewareTests
{
    private static readonly string[] _expectedHeaderNames =
    [
        "X-Content-Type-Options",
        "X-Frame-Options",
        "Referrer-Policy",
        "Permissions-Policy",
        "Content-Security-Policy-Report-Only",
    ];

    [Fact]
    public async Task InvokeAsync_SuccessfulResponse_SetsAllSecurityHeaders()
    {
        // Arrange
        await using var app = await StartAppAsync();
        using var client = app.GetTestClient();

        // Act
        using var response = await client.GetAsync("/ok", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        foreach (var header in _expectedHeaderNames)
        {
            response.Headers.Contains(header).Should().BeTrue($"{header} must be present on successful responses");
        }

        GetHeader(response, "X-Content-Type-Options").Should().Be("nosniff");
        GetHeader(response, "X-Frame-Options").Should().Be("DENY");
        GetHeader(response, "Referrer-Policy").Should().Be("strict-origin-when-cross-origin");
        GetHeader(response, "Permissions-Policy").Should().Be("interest-cohort=()");
    }

    [Theory]
    [InlineData("/missing", HttpStatusCode.NotFound)]
    [InlineData("/redirect", HttpStatusCode.MovedPermanently)]
    [InlineData("/error", HttpStatusCode.InternalServerError)]
    [InlineData("/empty", HttpStatusCode.NoContent)]
    public async Task InvokeAsync_NonSuccessResponses_StillSetAllSecurityHeaders(string path, HttpStatusCode expectedStatus)
    {
        // Arrange
        await using var app = await StartAppAsync();
        using var client = app.GetTestClient();

        // Act
        using var response = await client.GetAsync(path, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(expectedStatus);
        foreach (var header in _expectedHeaderNames)
        {
            response.Headers.Contains(header).Should().BeTrue($"{header} must be present on {(int)expectedStatus} responses");
        }
    }

    [Fact]
    public async Task InvokeAsync_ContentSecurityPolicy_ForbidsFramingAndRestrictsBaseAndForms()
    {
        // Arrange
        await using var app = await StartAppAsync();
        using var client = app.GetTestClient();

        // Act
        using var response = await client.GetAsync("/ok", TestContext.Current.CancellationToken);

        // Assert
        var csp = GetHeader(response, "Content-Security-Policy-Report-Only");
        var directives = csp.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        directives.Should().Contain("default-src 'self'");
        directives.Should().Contain("frame-ancestors 'none'");
        directives.Should().Contain("base-uri 'self'");
        directives.Should().Contain("form-action 'self'");
        directives.Should().Contain(d => d.StartsWith("frame-src ", StringComparison.Ordinal) && d.Contains("https://www.youtube-nocookie.com", StringComparison.Ordinal));
        directives.Should().Contain(d => d.StartsWith("script-src ", StringComparison.Ordinal) && d.Contains("https://js.monitor.azure.com", StringComparison.Ordinal));
    }

    [Fact]
    public async Task InvokeAsync_ContentSecurityPolicy_DoesNotAllowArbitraryWildcardSources()
    {
        // Arrange
        await using var app = await StartAppAsync();
        using var client = app.GetTestClient();

        // Act
        using var response = await client.GetAsync("/ok", TestContext.Current.CancellationToken);

        // Assert
        var csp = GetHeader(response, "Content-Security-Policy-Report-Only");
        var sources = csp.Split([' ', ';'], StringSplitOptions.RemoveEmptyEntries);
        sources.Should().NotContain("*", "a bare wildcard would defeat the policy");
        sources.Should().NotContain("http:", "plain-http sources must not be allowed");
    }

    [Fact]
    public async Task InvokeAsync_HandlerSetsConflictingHeaders_MiddlewareValuesWin()
    {
        // Arrange
        await using var app = await StartAppAsync();
        using var client = app.GetTestClient();

        // Act
        using var response = await client.GetAsync("/conflict", TestContext.Current.CancellationToken);

        // Assert
        GetHeader(response, "X-Frame-Options").Should().Be("DENY");
        GetHeader(response, "X-Content-Type-Options").Should().Be("nosniff");
    }

    [Fact]
    public async Task InvokeAsync_CallsNextMiddleware()
    {
        // Arrange
        await using var app = await StartAppAsync();
        using var client = app.GetTestClient();

        // Act
        using var response = await client.GetAsync("/ok", TestContext.Current.CancellationToken);
        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        // Assert
        body.Should().Be("ok");
    }

    [Fact]
    public void Constructor_NullNext_Throws()
    {
        // Act
        var act = () => new SecurityHeadersMiddleware(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    private static string GetHeader(HttpResponseMessage response, string name)
    {
        response.Headers.NonValidated.TryGetValues(name, out var values).Should().BeTrue($"{name} header must exist");
        return values.ToString();
    }

    private static async Task<WebApplication> StartAppAsync()
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        var app = builder.Build();

        app.UseSecurityHeaders();
        app.Run(async context =>
        {
            switch (context.Request.Path.Value)
            {
                case "/missing":
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    break;
                case "/redirect":
                    context.Response.StatusCode = StatusCodes.Status301MovedPermanently;
                    context.Response.Headers.Location = "/elsewhere";
                    break;
                case "/error":
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
                case "/empty":
                    context.Response.StatusCode = StatusCodes.Status204NoContent;
                    break;
                case "/conflict":
                    context.Response.Headers["X-Frame-Options"] = "ALLOWALL";
                    context.Response.Headers["X-Content-Type-Options"] = "sniff";
                    await context.Response.WriteAsync("conflict");
                    break;
                default:
                    await context.Response.WriteAsync("ok");
                    break;
            }
        });

        await app.StartAsync();
        return app;
    }
}
