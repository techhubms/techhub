using System.Diagnostics;
using System.Diagnostics.Metrics;
using FluentAssertions;
using TechHub.Web.MCP;

namespace TechHub.Web.Tests.MCP;

/// <summary>
/// Tests for <see cref="McpTelemetry"/> — the MCP content activity source and request metrics.
/// The meter and source are process-wide statics, so each test uses a unique section/operation name
/// to stay isolated from other tests that emit through the same instruments.
/// </summary>
public class McpTelemetryTests
{
    private const string DurationMetricName = "techhub.mcp.content.duration";

    [Fact]
    public void Names_AreStableBecauseTheyAreRegisteredWithOpenTelemetryInProgram()
    {
        // Assert
        McpTelemetry.Name.Should().Be("TechHub.Mcp");
        McpTelemetry.RequestsMetricName.Should().Be("techhub.mcp.content.requests");
        McpTelemetry.ActivitySource.Name.Should().Be(McpTelemetry.Name);
    }

    [Fact]
    public void RecordRequest_Called_IncrementsRequestCounterByOneWithSectionAndResultTags()
    {
        // Arrange
        var section = UniqueName();
        using var capture = new MetricCapture(section);

        // Act
        McpTelemetry.RecordRequest(section, "hit", TimeSpan.FromMilliseconds(5));

        // Assert
        var counter = capture.Measurements.Should().ContainSingle(m => m.Instrument == McpTelemetry.RequestsMetricName).Subject;
        counter.Value.Should().Be(1);
        counter.Tags.Should().BeEquivalentTo(new Dictionary<string, string?>
        {
            ["techhub.section"] = section,
            ["techhub.result"] = "hit"
        });
    }

    [Fact]
    public void RecordRequest_Called_RecordsElapsedMillisecondsInDurationHistogramWithSameTags()
    {
        // Arrange
        var section = UniqueName();
        using var capture = new MetricCapture(section);

        // Act
        McpTelemetry.RecordRequest(section, "miss", TimeSpan.FromMilliseconds(12.5));

        // Assert
        var duration = capture.Measurements.Should().ContainSingle(m => m.Instrument == DurationMetricName).Subject;
        duration.Value.Should().BeApproximately(12.5, 0.0001);
        duration.Tags["techhub.section"].Should().Be(section);
        duration.Tags["techhub.result"].Should().Be("miss");
    }

    [Fact]
    public void RecordRequest_ZeroElapsed_RecordsZeroDurationAndStillCountsRequest()
    {
        // Arrange
        var section = UniqueName();
        using var capture = new MetricCapture(section);

        // Act
        McpTelemetry.RecordRequest(section, "hit", TimeSpan.Zero);

        // Assert
        capture.Measurements.Should().ContainSingle(m => m.Instrument == DurationMetricName).Which.Value.Should().Be(0);
        capture.Measurements.Should().ContainSingle(m => m.Instrument == McpTelemetry.RequestsMetricName);
    }

    [Fact]
    public void RecordRequest_MultipleCalls_EmitsOneCounterAndOneDurationMeasurementPerCall()
    {
        // Arrange
        var section = UniqueName();
        using var capture = new MetricCapture(section);

        // Act
        McpTelemetry.RecordRequest(section, "miss", TimeSpan.FromMilliseconds(30));
        McpTelemetry.RecordRequest(section, "hit", TimeSpan.FromMilliseconds(1));
        McpTelemetry.RecordRequest(section, "hit", TimeSpan.FromMilliseconds(2));

        // Assert
        var counters = capture.Measurements.Where(m => m.Instrument == McpTelemetry.RequestsMetricName).ToList();
        counters.Should().HaveCount(3);
        counters.Select(c => c.Tags["techhub.result"]).Should().Equal("miss", "hit", "hit");
        capture.Measurements.Where(m => m.Instrument == DurationMetricName).Select(d => d.Value)
            .Should().Equal(30d, 1d, 2d);
    }

    [Fact]
    public void RecordRequest_DifferentSections_AreTaggedSeparately()
    {
        // Arrange
        var sectionA = UniqueName();
        var sectionB = UniqueName();
        using var captureA = new MetricCapture(sectionA);
        using var captureB = new MetricCapture(sectionB);

        // Act
        McpTelemetry.RecordRequest(sectionA, "hit", TimeSpan.FromMilliseconds(1));
        McpTelemetry.RecordRequest(sectionB, "miss", TimeSpan.FromMilliseconds(1));

        // Assert
        captureA.Measurements.Where(m => m.Instrument == McpTelemetry.RequestsMetricName)
            .Should().ContainSingle().Which.Tags["techhub.result"].Should().Be("hit");
        captureB.Measurements.Where(m => m.Instrument == McpTelemetry.RequestsMetricName)
            .Should().ContainSingle().Which.Tags["techhub.result"].Should().Be("miss");
    }

    [Fact]
    public void Instruments_AreDeclaredWithExpectedNamesAndUnits()
    {
        // Arrange
        var published = new List<(string Name, string? Unit)>();
        using var listener = new MeterListener();
        listener.InstrumentPublished = (instrument, _) =>
        {
            if (instrument.Meter.Name == McpTelemetry.Name)
            {
                lock (published)
                {
                    published.Add((instrument.Name, instrument.Unit));
                }
            }
        };

        // Act
        McpTelemetry.RecordRequest(UniqueName(), "hit", TimeSpan.Zero);
        listener.Start();

        // Assert
        lock (published)
        {
            published.Should().Contain((McpTelemetry.RequestsMetricName, "{request}"));
            published.Should().Contain((DurationMetricName, "ms"));
        }
    }

    [Fact]
    public void ActivitySource_WithListener_CreatesActivitiesOwnedByTheMcpSource()
    {
        // Arrange
        var operation = "McpTelemetryTests." + UniqueName();
        var started = new List<Activity>();
        using var listener = new ActivityListener
        {
            ShouldListenTo = source => source.Name == McpTelemetry.Name,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData,
            ActivityStarted = activity =>
            {
                lock (started)
                {
                    started.Add(activity);
                }
            }
        };
        ActivitySource.AddActivityListener(listener);

        // Act
        using (var activity = McpTelemetry.ActivitySource.StartActivity(operation))
        {
            // Assert
            activity.Should().NotBeNull();
            activity!.Source.Name.Should().Be("TechHub.Mcp");
            activity.OperationName.Should().Be(operation);
        }

        lock (started)
        {
            started.Should().Contain(a => a.OperationName == operation);
        }
    }

    [Fact]
    public void ActivitySource_ListenerForOtherSource_DoesNotReceiveMcpActivities()
    {
        // Arrange
        var operation = "McpTelemetryTests." + UniqueName();
        var seen = new List<string>();
        using var listener = new ActivityListener
        {
            ShouldListenTo = source => source.Name == "Some.Other.Source",
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData,
            ActivityStarted = activity =>
            {
                lock (seen)
                {
                    seen.Add(activity.OperationName);
                }
            }
        };
        ActivitySource.AddActivityListener(listener);

        // Act
        using var activity = McpTelemetry.ActivitySource.StartActivity(operation);

        // Assert
        lock (seen)
        {
            seen.Should().NotContain(operation);
        }
    }

    private static string UniqueName() => "telemetry-test-" + Guid.NewGuid().ToString("N");

    private sealed record CapturedMeasurement(string Instrument, double Value, IReadOnlyDictionary<string, string?> Tags);

    /// <summary>Records measurements of the MCP meter whose <c>techhub.section</c> tag equals the given section.</summary>
    private sealed class MetricCapture : IDisposable
    {
        private readonly MeterListener _listener = new();
        private readonly List<CapturedMeasurement> _measurements = [];

        public MetricCapture(string section)
        {
            _listener.InstrumentPublished = (instrument, l) =>
            {
                if (instrument.Meter.Name == McpTelemetry.Name)
                {
                    l.EnableMeasurementEvents(instrument);
                }
            };
            _listener.SetMeasurementEventCallback<long>((instrument, value, tags, _) => Capture(section, instrument, value, tags));
            _listener.SetMeasurementEventCallback<double>((instrument, value, tags, _) => Capture(section, instrument, value, tags));
            _listener.Start();
        }

        public IReadOnlyList<CapturedMeasurement> Measurements
        {
            get
            {
                lock (_measurements)
                {
                    return [.. _measurements];
                }
            }
        }

        public void Dispose() => _listener.Dispose();

        private void Capture(string section, Instrument instrument, double value, ReadOnlySpan<KeyValuePair<string, object?>> tags)
        {
            var map = new Dictionary<string, string?>();
            foreach (var tag in tags)
            {
                map[tag.Key] = tag.Value as string;
            }

            if (map.GetValueOrDefault("techhub.section") != section)
            {
                return;
            }

            lock (_measurements)
            {
                _measurements.Add(new CapturedMeasurement(instrument.Name, value, map));
            }
        }
    }
}
