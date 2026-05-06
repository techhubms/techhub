using FluentAssertions;
using TechHub.ServiceDefaults;

namespace TechHub.Api.Tests.Telemetry;

/// <summary>
/// Tests for the shared OpenTelemetry filter helpers in <see cref="ServiceDefaultsExtensions"/>
/// that apply to both the API and the Web frontend.
///
/// Web-specific filters (Blazor disconnect, bot crawlers) live in
/// TechHub.Web.Tests.Telemetry.WebTelemetryFiltersTests.
/// Probe-detection logic lives in TechHub.Core.Tests.Security.ProbeDetectorTests.
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
}

