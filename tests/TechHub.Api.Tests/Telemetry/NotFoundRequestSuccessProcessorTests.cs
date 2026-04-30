using System.Diagnostics;
using FluentAssertions;
using TechHub.ServiceDefaults;

namespace TechHub.Api.Tests.Telemetry;

/// <summary>
/// Unit tests for <see cref="NotFoundRequestSuccessProcessor"/>.
/// Verifies that HTTP 404 server spans are marked as successful to prevent
/// false-positive failure alerts in Application Insights.
/// </summary>
public class NotFoundRequestSuccessProcessorTests
{
    private static readonly ActivitySource Source = new("TechHub.Tests.Telemetry");
    private readonly NotFoundRequestSuccessProcessor _sut = new();

    private static ActivityListener CreateListener() => new()
    {
        ShouldListenTo = _ => true,
        Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData,
    };

    [Fact]
    public void OnEnd_WhenServer404_SetsStatusToOk()
    {
        // Arrange
        using var listener = CreateListener();
        ActivitySource.AddActivityListener(listener);

        using var activity = Source.StartActivity("HTTP GET", ActivityKind.Server)!;
        activity.SetTag("http.response.status_code", 404);

        // Act
        _sut.OnEnd(activity);

        // Assert
        activity.Status.Should().Be(ActivityStatusCode.Ok,
            "404 Not Found is expected server behavior, not an application error");
    }

    [Theory]
    [InlineData(200)]
    [InlineData(201)]
    [InlineData(204)]
    [InlineData(301)]
    [InlineData(400)]
    [InlineData(401)]
    [InlineData(403)]
    [InlineData(500)]
    [InlineData(503)]
    public void OnEnd_WhenServerNon404_DoesNotChangeStatus(int statusCode)
    {
        // Arrange
        using var listener = CreateListener();
        ActivitySource.AddActivityListener(listener);

        using var activity = Source.StartActivity("HTTP GET", ActivityKind.Server)!;
        activity.SetTag("http.response.status_code", statusCode);

        // Act
        _sut.OnEnd(activity);

        // Assert
        activity.Status.Should().Be(ActivityStatusCode.Unset,
            "only 404 should be overridden; other status codes keep their existing span status");
    }

    [Theory]
    [InlineData(ActivityKind.Client)]
    [InlineData(ActivityKind.Internal)]
    [InlineData(ActivityKind.Producer)]
    [InlineData(ActivityKind.Consumer)]
    public void OnEnd_WhenNonServerKind404_DoesNotChangeStatus(ActivityKind kind)
    {
        // Arrange
        using var listener = CreateListener();
        ActivitySource.AddActivityListener(listener);

        using var activity = Source.StartActivity("HTTP GET", kind)!;
        activity.SetTag("http.response.status_code", 404);

        // Act
        _sut.OnEnd(activity);

        // Assert
        activity.Status.Should().Be(ActivityStatusCode.Unset,
            "outbound HTTP client spans and internal spans should not be affected");
    }

    [Fact]
    public void OnEnd_WhenNoStatusCodeTag_DoesNotChangeStatus()
    {
        // Arrange
        using var listener = CreateListener();
        ActivitySource.AddActivityListener(listener);

        using var activity = Source.StartActivity("HTTP GET", ActivityKind.Server)!;

        // Act
        _sut.OnEnd(activity);

        // Assert
        activity.Status.Should().Be(ActivityStatusCode.Unset,
            "activities without an HTTP status code tag should be left unchanged");
    }
}
