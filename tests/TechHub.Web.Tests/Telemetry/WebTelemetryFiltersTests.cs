using System.Diagnostics;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using TechHub.Web.Telemetry;

namespace TechHub.Web.Tests.Telemetry;

/// <summary>
/// Tests for <see cref="WebTelemetryFilters"/> — Blazor and bot-specific trace suppression
/// that applies only to the Web frontend. These concepts do not exist in TechHub.Api.
/// </summary>
public class WebTelemetryFiltersTests
{
    [Theory]
    [InlineData("/_blazor/disconnect")]
    [InlineData("/_blazor/disconnect/")]
    [InlineData("/_BLAZOR/DISCONNECT")]
    [InlineData("/_blazor/disconnect/abc123circuitid")]
    public void IsBlazorDisconnectRequest_ReturnsTrue_ForDisconnectPath(string path)
    {
        WebTelemetryFilters.IsBlazorDisconnectRequest(path)
            .Should().BeTrue(
                "/_blazor/disconnect produces structurally expected 499s (client closed) and " +
                "400s (circuit already gone) during page unload; no diagnostic value");
    }

    [Theory]
    [InlineData("/_blazor/negotiate")]
    [InlineData("/_blazor")]
    [InlineData("/_blazor/info")]
    [InlineData("/ai")]
    [InlineData("/")]
    public void IsBlazorDisconnectRequest_ReturnsFalse_ForOtherBlazorPaths(string path)
    {
        WebTelemetryFilters.IsBlazorDisconnectRequest(path)
            .Should().BeFalse(
                "only the /disconnect sub-path is suppressed; other /_blazor paths " +
                "(especially /negotiate) must remain visible for circuit health monitoring");
    }

    [Theory]
    [InlineData("/api")]
    [InlineData("/api/")]
    [InlineData("/api/users")]
    [InlineData("/api/v1/health")]
    [InlineData("/API/sections")]
    public void IsApiProbeRequest_ReturnsTrue_ForApiPaths(string path)
    {
        WebTelemetryFilters.IsApiProbeRequest(path)
            .Should().BeTrue(
                "the web frontend never serves /api routes — these are scanners probing " +
                "for a REST API on the same host and have no diagnostic value");
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/ai")]
    [InlineData("/about")]
    [InlineData("/ai/videos/config-as-code-is-the-best")]
    [InlineData("/sitemap.xml")]
    public void IsApiProbeRequest_ReturnsFalse_ForNonApiPaths(string path)
    {
        WebTelemetryFilters.IsApiProbeRequest(path)
            .Should().BeFalse("legitimate web routes must not be suppressed from telemetry");
    }

    [Theory]
    [InlineData("/_blazor", "id", "abc123circuitid")]
    [InlineData("/_blazor", "id", "")]
    [InlineData("/_BLAZOR", "id", "xyz")]
    public void IsBlazorCircuitReconnectRequest_ReturnsTrue_ForCircuitReconnects(string path, string key, string value)
    {
        var query = new QueryCollection(new Dictionary<string, StringValues> { [key] = value });

        WebTelemetryFilters.IsBlazorCircuitReconnectRequest(path, query)
            .Should().BeTrue(
                "/_blazor?id=... is a client trying to resume an expired Blazor circuit after " +
                "a container restart; these always 404 and have no diagnostic value");
    }

    [Theory]
    [InlineData("/_blazor/disconnect", "id", "abc")]
    [InlineData("/_blazor/negotiate", "id", "abc")]
    [InlineData("/_blazor", "", "")]
    [InlineData("/_blazor", "other", "val")]
    [InlineData("/ai", "id", "abc")]
    public void IsBlazorCircuitReconnectRequest_ReturnsFalse_ForOtherRequests(string path, string key, string value)
    {
        var pairs = string.IsNullOrEmpty(key)
            ? new Dictionary<string, StringValues>()
            : new Dictionary<string, StringValues> { [key] = value };
        var query = new QueryCollection(pairs);

        WebTelemetryFilters.IsBlazorCircuitReconnectRequest(path, query)
            .Should().BeFalse(
                "only the exact /_blazor path with an 'id' query param is suppressed; " +
                "sub-paths and other paths must remain visible");
    }

    [Theory]
    [InlineData("AhrefsBot/7.0")]
    [InlineData("Mozilla/5.0 (compatible; Googlebot/2.1)")]
    [InlineData("bingbot/2.0")]
    [InlineData("ClaudeBot/1.0")]
    [InlineData("OAI-SearchBot/1.0")]
    [InlineData("FacebookBot")]
    [InlineData("PetalBot")]
    [InlineData("SomeOtherBot/3.0")]
    public void IsBotRequest_ReturnsTrue_ForBotUserAgents(string userAgent)
    {
        WebTelemetryFilters.IsBotRequest(userAgent)
            .Should().BeTrue("bot crawlers following stale links generate expected 404s with no diagnostic value");
    }

    [Theory]
    [InlineData("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36")]
    [InlineData("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7)")]
    [InlineData("curl/7.68.0")]
    [InlineData("HeadlessChrome/147.0")]
    [InlineData("")]
    public void IsBotRequest_ReturnsFalse_ForRealUsers(string userAgent)
    {
        WebTelemetryFilters.IsBotRequest(userAgent)
            .Should().BeFalse("real user agents must not be filtered from telemetry");
    }

    // ── SuppressIfClientError ────────────────────────────────────────────────

    [Theory]
    [InlineData(404)]
    [InlineData(405)]
    public void SuppressIfClientError_ClearsRecordedFlag_ForStructuralNoise(int statusCode)
    {
        // 404 = bots/scanners following dead links; 405 = scanner probing disallowed methods.
        // Both are generated exclusively by external noise, never by real application errors.
        var activity = new Activity("Microsoft.AspNetCore.Hosting.HttpRequestIn");
        activity.Start();
        activity.ActivityTraceFlags = ActivityTraceFlags.Recorded;

        var context = new DefaultHttpContext();
        context.Response.StatusCode = statusCode;

        WebTelemetryFilters.SuppressIfClientError(activity, context.Response);

        activity.ActivityTraceFlags.Should().NotHaveFlag(ActivityTraceFlags.Recorded,
            $"HTTP {statusCode} is structural noise that must not inflate requests/failed in App Insights");
        activity.Stop();
    }

    [Theory]
    [InlineData(200)]
    [InlineData(201)]
    [InlineData(204)]
    [InlineData(301)]
    [InlineData(302)]
    [InlineData(400)]
    [InlineData(401)]
    [InlineData(403)]
    [InlineData(429)]
    [InlineData(499)]
    [InlineData(500)]
    [InlineData(502)]
    [InlineData(503)]
    public void SuppressIfClientError_RetainsRecordedFlag_ForActionableResponses(int statusCode)
    {
        // 401/403 can reveal auth bugs; 429 signals a scraping/DDoS attack;
        // 5xx are genuine server errors. All must remain visible in App Insights.
        var activity = new Activity("Microsoft.AspNetCore.Hosting.HttpRequestIn");
        activity.Start();
        activity.ActivityTraceFlags = ActivityTraceFlags.Recorded;

        var context = new DefaultHttpContext();
        context.Response.StatusCode = statusCode;

        WebTelemetryFilters.SuppressIfClientError(activity, context.Response);

        activity.ActivityTraceFlags.Should().HaveFlag(ActivityTraceFlags.Recorded,
            $"HTTP {statusCode} must remain in the trace pipeline — " +
            (statusCode >= 500 ? "server errors must trigger alerts" :
             statusCode == 429 ? "rate-limit surges indicate attack traffic" :
             statusCode is 401 or 403 ? "auth failures may reveal bugs" :
             "success/redirect responses must be visible"));
        activity.Stop();
    }
}
