using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;

namespace TechHub.Web.Tests.Middleware;

/// <summary>
/// Verifies that <c>/_blazor/*</c> paths are excluded from
/// <see cref="StatusCodePagesExtensions.UseStatusCodePagesWithReExecute"/> so that a 404
/// from a gone Blazor circuit is returned as-is rather than being re-executed through
/// <c>/not-found</c>.
///
/// Background: When a browser tab or Playwright closes and the circuit is already torn
/// down, Blazor fires <c>POST /_blazor/disconnect</c>. ASP.NET Core returns 404 (circuit
/// gone). Without the <see cref="UseWhenExtensions.UseWhen"/> guard,
/// <c>UseStatusCodePagesWithReExecute</c> re-executes the request through <c>/not-found</c>,
/// changing the OTel span name to "POST /not-found" and making the failure appear as an
/// application error in Azure Monitor dashboards.
/// </summary>
public class BlazorPathStatusCodePageExclusionTests : IAsyncLifetime
{
    private WebApplication _app = null!;
    private HttpClient _client = null!;

    public async ValueTask InitializeAsync()
    {
        // Build a minimal test server replicating the pipeline slice in TechHub.Web/Program.cs:
        //   UseWhen(!/_blazor, branch => branch.UseStatusCodePagesWithReExecute("/not-found"))
        //
        // WebApplication implements both IApplicationBuilder (middleware) and
        // IEndpointRouteBuilder (endpoints), so UseWhen and MapPost can be called on it directly.
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        _app = builder.Build();

        // This mirrors the exact code in TechHub.Web/Program.cs
        _app.UseWhen(
            ctx => !ctx.Request.Path.StartsWithSegments("/_blazor", StringComparison.OrdinalIgnoreCase),
            branch => branch.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true));

        // UseRouting must come AFTER UseWhen so that re-execution from StatusCodePagesMiddleware
        // re-runs route matching with the updated path (e.g., /not-found).
        _app.UseRouting();

        // Simulate /_blazor/disconnect returning 404 (circuit gone).
        // Use a plain status-code response with no Content-Type so UseStatusCodePagesWithReExecute
        // would intercept it — but the UseWhen guard prevents that for /_blazor paths.
        _app.MapPost("/_blazor/{*path}", (HttpContext ctx) =>
        {
            ctx.Response.StatusCode = StatusCodes.Status404NotFound;
            return Task.CompletedTask;
        });

        // The /not-found handler — simulates NotFound.razor which explicitly sets StatusCode = 404
        // and writes HTML. UseStatusCodePagesWithReExecute does NOT auto-restore the original
        // status code; the handler itself is responsible, just like NotFound.razor in production.
        _app.MapGet("/not-found", async (HttpContext ctx) =>
        {
            ctx.Response.StatusCode = StatusCodes.Status404NotFound;
            ctx.Response.ContentType = "text/html";
            await ctx.Response.WriteAsync("not-found-page");
        });

        // Simulate any other unknown path returning 404 with no body/content-type.
        // UseStatusCodePagesWithReExecute only intercepts responses with no Content-Type set,
        // so we must use a bare status code (not Results.NotFound() which adds a Problem Details body).
        _app.MapFallback((HttpContext ctx) =>
        {
            ctx.Response.StatusCode = StatusCodes.Status404NotFound;
            return Task.CompletedTask;
        });

        await _app.StartAsync();
        _client = _app.GetTestClient();
    }

    public async ValueTask DisposeAsync()
    {
        _client.Dispose();
        await _app.StopAsync();
        await _app.DisposeAsync();
    }

    [Fact]
    public async Task BlazorDisconnect_404_IsNotReExecutedThroughNotFoundPage()
    {
        // Act — simulates Playwright/browser closing while circuit is already gone
        var response = await _client.PostAsync("/_blazor/disconnect", content: null,
            TestContext.Current.CancellationToken);

        // Assert — raw 404, NOT re-executed through /not-found
        response.StatusCode.Should().Be(HttpStatusCode.NotFound,
            "/_blazor paths must bypass UseStatusCodePagesWithReExecute — a gone circuit returning 404 is expected, not an application error");
        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        body.Should().NotContain("not-found-page",
            "the response body must not come from the /not-found handler");
    }

    [Theory]
    [InlineData("/_blazor/negotiate")]
    [InlineData("/_blazor/disconnect")]
    [InlineData("/_blazor/hub")]
    public async Task BlazorPath_NotFound_NeverGoesToNotFoundPage(string path)
    {
        // Act
        var response = await _client.PostAsync(path, content: null,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        body.Should().NotContain("not-found-page");
    }

    [Fact]
    public async Task NonBlazorPath_404_IsReExecutedThroughNotFoundPage()
    {
        // Act — a regular unknown path should still get the nice 404 page
        var response = await _client.GetAsync("/some-unknown-path",
            TestContext.Current.CancellationToken);

        // Assert — re-executed through /not-found, which sets 404 (just like NotFound.razor in production).
        // UseStatusCodePagesWithReExecute does NOT auto-restore the status code; the handler sets it.
        // If this were UseStatusCodePagesWithRedirects instead, the client would follow a 302 and
        // the final status would still be 404 BUT the URL would change. Here we verify:
        //   1. Status is 404  — the handler correctly preserves the error status
        //   2. Body contains "not-found-page" — re-execution reached /not-found, not a bare empty 404
        response.StatusCode.Should().Be(HttpStatusCode.NotFound,
            "UseStatusCodePagesWithReExecute must preserve 404 status — the user sees a 404 with HTML body, not a redirect");
        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        body.Should().Contain("not-found-page",
            "the response body must come from the /not-found handler, proving re-execution happened");
    }
}
