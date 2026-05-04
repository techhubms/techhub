using FluentAssertions;
using Microsoft.AspNetCore.Http;
using TechHub.ServiceDefaults;

namespace TechHub.Api.Tests.Telemetry;

/// <summary>
/// Unit tests for the OpenTelemetry request filter helpers in
/// <see cref="ServiceDefaultsExtensions"/>. These methods drive <c>options.Filter</c>
/// in AddAspNetCoreInstrumentation — returning <c>false</c> prevents any Activity from
/// being created, which means zero App Insights rows and zero data cost (unlike a
/// processor that creates the Activity and marks it Ok at the end).
/// </summary>
public class ServiceDefaultsFilterTests
{
    [Theory]
    [InlineData("/health")]
    [InlineData("/Health")]
    [InlineData("/HEALTH")]
    [InlineData("/alive")]
    [InlineData("/ALIVE")]
    public void IsHealthProbeRequest_ReturnsTrue_ForHealthAndAlive(string path)
    {
        ServiceDefaultsExtensions.IsHealthProbeRequest(path)
            .Should().BeTrue("health and liveness probe paths must be filtered from telemetry");
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/ai")]
    [InlineData("/healthz")]
    [InlineData("/health-check")]
    [InlineData("/_blazor/disconnect")]
    public void IsHealthProbeRequest_ReturnsFalse_ForOtherPaths(string path)
    {
        ServiceDefaultsExtensions.IsHealthProbeRequest(path)
            .Should().BeFalse();
    }

    [Theory]
    [InlineData("/_blazor/disconnect")]
    [InlineData("/_blazor/disconnect/")]
    [InlineData("/_BLAZOR/DISCONNECT")]
    [InlineData("/_blazor/disconnect/abc123circuitid")]
    public void IsBlazorDisconnectRequest_ReturnsTrue_ForDisconnectPath(string path)
    {
        ServiceDefaultsExtensions.IsBlazorDisconnectRequest(path)
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
        ServiceDefaultsExtensions.IsBlazorDisconnectRequest(path)
            .Should().BeFalse(
                "only the /disconnect sub-path is suppressed; other /_blazor paths " +
                "(especially /negotiate) must remain visible for circuit health monitoring");
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
        ServiceDefaultsExtensions.IsBotRequest(userAgent)
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
        ServiceDefaultsExtensions.IsBotRequest(userAgent)
            .Should().BeFalse("real user agents must not be filtered from telemetry");
    }

    [Theory]
    [InlineData("/.env")]
    [InlineData("/.env/")]                              // trailing slash must still be detected
    [InlineData("/wp-admin")]
    [InlineData("/wp-login.php")]
    [InlineData("/xmlrpc.php")]
    [InlineData("/some/path/wp-content/uploads/file.jpg")]
    [InlineData("/phpmyadmin")]
    [InlineData("/random.xml")]
    [InlineData("/random.xml/")]                        // trailing slash on .xml probe
    [InlineData("/evil-sitemap.xml")]
    [InlineData("/wordpress.xml")]
    [InlineData("/backup.sql")]
    [InlineData("/server.key")]
    [InlineData("/archive.zip")]
    [InlineData("/actuator")]                           // exact actuator segment
    [InlineData("/actuator/health")]                    // actuator sub-path
    public void IsProbeRequest_ReturnsTrue_ForProbePathsAndExtensions(string path)
    {
        ServiceDefaultsExtensions.IsProbeRequest(new PathString(path))
            .Should().BeTrue("scanner probe paths must be filtered from telemetry to avoid noise");
    }

    [Theory]
    [InlineData("/feed.xml")]
    [InlineData("/all/feed.xml")]
    [InlineData("/ai/feed.xml")]
    [InlineData("/sitemap.xml")]
    [InlineData("/")]
    [InlineData("/ai")]
    [InlineData("/ai/videos")]
    [InlineData("/about")]
    [InlineData("/ai/actuator-systems-deep-dive")]      // "actuator" as slug prefix, not a probe
    public void IsProbeRequest_ReturnsFalse_ForLegitimatePathsAndFeedExceptions(string path)
    {
        ServiceDefaultsExtensions.IsProbeRequest(new PathString(path))
            .Should().BeFalse("legitimate paths and RSS/sitemap endpoints must not be suppressed from telemetry");
    }
}
