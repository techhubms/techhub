using FluentAssertions;
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
}
