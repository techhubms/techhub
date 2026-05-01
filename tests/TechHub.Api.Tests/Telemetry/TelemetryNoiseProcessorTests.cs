using System.Diagnostics;
using FluentAssertions;
using TechHub.ServiceDefaults;

namespace TechHub.Api.Tests.Telemetry;

/// <summary>
/// Unit tests for <see cref="TelemetryNoiseProcessor"/>.
/// Verifies that bot crawlers are marked as successful spans to prevent
/// false-positive failure alerts in Azure Monitor.
/// </summary>
public class TelemetryNoiseProcessorTests
{
    private static readonly ActivitySource _source = new("TechHub.Tests.Telemetry");
    private readonly TelemetryNoiseProcessor _sut = new();

    private static ActivityListener CreateListener() => new()
    {
        ShouldListenTo = _ => true,
        Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData,
    };

    [Theory]
    [InlineData("AhrefsBot/7.0")]
    [InlineData("Mozilla/5.0 (compatible; Googlebot/2.1)")]
    [InlineData("bingbot/2.0")]
    [InlineData("ClaudeBot/1.0")]
    [InlineData("OAI-SearchBot/1.0")]
    [InlineData("FacebookBot")]
    [InlineData("PetalBot")]
    [InlineData("SomeOtherBot/3.0")]
    public void OnEnd_WhenBotUserAgent_SetsStatusToOk(string userAgent)
    {
        // Arrange
        using var listener = CreateListener();
        ActivitySource.AddActivityListener(listener);

        using var activity = _source.StartActivity("HTTP GET", ActivityKind.Server)!;
        activity.SetTag("user_agent.original", userAgent);
        activity.SetStatus(ActivityStatusCode.Error);

        // Act
        _sut.OnEnd(activity);

        // Assert
        activity.Status.Should().Be(ActivityStatusCode.Ok,
            "bot crawlers following stale links produce expected 404s, not application errors");
    }

    [Theory]
    [InlineData("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36")]
    [InlineData("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7)")]
    [InlineData("curl/7.68.0")]
    [InlineData("")]
    public void OnEnd_WhenNonBotUserAgent_DoesNotChangeStatus(string userAgent)
    {
        // Arrange
        using var listener = CreateListener();
        ActivitySource.AddActivityListener(listener);

        using var activity = _source.StartActivity("HTTP GET", ActivityKind.Server)!;
        activity.SetTag("user_agent.original", userAgent);
        activity.SetStatus(ActivityStatusCode.Error);

        // Act
        _sut.OnEnd(activity);

        // Assert
        activity.Status.Should().Be(ActivityStatusCode.Error,
            "non-bot user agents should not have their span status changed");
    }

    [Fact]
    public void OnEnd_WhenNoUserAgentTag_DoesNotChangeStatus()
    {
        // Arrange
        using var listener = CreateListener();
        ActivitySource.AddActivityListener(listener);

        using var activity = _source.StartActivity("HTTP GET", ActivityKind.Server)!;
        activity.SetStatus(ActivityStatusCode.Error);

        // Act
        _sut.OnEnd(activity);

        // Assert
        activity.Status.Should().Be(ActivityStatusCode.Error,
            "activities without a user_agent.original tag should be left unchanged");
    }

    [Theory]
    [InlineData(ActivityKind.Client)]
    [InlineData(ActivityKind.Internal)]
    [InlineData(ActivityKind.Producer)]
    [InlineData(ActivityKind.Consumer)]
    public void OnEnd_WhenNonServerKind_DoesNotChangeStatus(ActivityKind kind)
    {
        // Arrange
        using var listener = CreateListener();
        ActivitySource.AddActivityListener(listener);

        using var activity = _source.StartActivity("HTTP GET", kind)!;
        activity.SetTag("user_agent.original", "AhrefsBot/7.0");
        activity.SetStatus(ActivityStatusCode.Error);

        // Act
        _sut.OnEnd(activity);

        // Assert
        activity.Status.Should().Be(ActivityStatusCode.Error,
            "only server spans represent inbound HTTP requests; other kinds should not be affected");
    }
}

