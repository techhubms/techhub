using System.Diagnostics.Metrics;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using TechHub.ServiceDefaults;

namespace TechHub.Api.Tests.Telemetry;

public class MemoryDiagnosticsTests
{
    private static readonly DateTimeOffset _now = new(2026, 11, 15, 12, 0, 0, TimeSpan.Zero);

    [Theory]
    [InlineData(79, false)]
    [InlineData(80, true)]
    [InlineData(100, true)]
    public void ShouldLog_ComparesRetainedHeapWithTheLimit(long retainedPercent, bool expected)
    {
        // Act
        var result = MemoryDiagnosticsService.ShouldLog(retainedPercent, 100, lastLoggedAt: null, _now);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(4, false)]
    [InlineData(5, true)]
    public void ShouldLog_RateLimitsRepeatedWarnings(int minutesSinceLastLog, bool expected)
    {
        // Act
        var result = MemoryDiagnosticsService.ShouldLog(95, 100, _now.AddMinutes(-minutesSinceLastLog), _now);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("25800000", 629145600L)]
    [InlineData("0x25800000", 629145600L)]
    [InlineData(" 32000000 ", 838860800L)]
    public void TryParseHeapHardLimit_ParsesHexadecimalBytes(string value, long expected)
    {
        MemoryDiagnosticsService.TryParseHeapHardLimit(value).Should().Be(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("not hex")]
    [InlineData("0")]
    public void TryParseHeapHardLimit_ReturnsNullWhenNoUsableLimit(string? value)
    {
        MemoryDiagnosticsService.TryParseHeapHardLimit(value).Should().BeNull();
    }

    [Fact]
    public void FormatProbes_ReportsEveryProbeAndSurvivesAFailingOne()
    {
        // Arrange
        MemoryDiagnosticsProbe[] probes =
        [
            new("circuits.open", () => 12),
            new("broken", () => throw new InvalidOperationException("boom")),
            new("cache.entries", () => 340)
        ];

        // Act
        var text = MemoryDiagnosticsService.FormatProbes(probes);

        // Assert
        text.Should().Be("circuits.open=12, broken=n/a, cache.entries=340");
    }

    [Fact]
    public void ReadHeap_DescribesEveryGenerationAsOfTheLastFullGc()
    {
        // Arrange — large allocations make the runtime run a full GC on its own, so there is a snapshot to read
        var fullCollectionsAtStart = GC.CollectionCount(2);
        for (var i = 0; i < 4000 && GC.CollectionCount(2) == fullCollectionsAtStart; i++)
        {
            GC.KeepAlive(new byte[1024 * 1024]);
        }

        // Act
        var snapshot = MemoryDiagnosticsService.ReadHeap();

        // Assert
        snapshot.Should().NotBeNull();
        snapshot!.Value.GcIndex.Should().BeGreaterThan(0);
        snapshot.Value.RetainedBytes.Should().BeGreaterThan(0);
        snapshot.Value.Generations.Should().Contain("gen0=").And.Contain("gen2=").And.Contain("loh=").And.Contain("poh=");
    }

    [Fact]
    public void Probes_AreExportedAsMetrics()
    {
        // Arrange
        using var sut = new MemoryDiagnosticsService(
            [new MemoryDiagnosticsProbe("test.probe", () => 7)],
            TimeProvider.System,
            Mock.Of<ILogger<MemoryDiagnosticsService>>());
        long? observed = null;
        using var listener = new MeterListener();
        listener.InstrumentPublished = (instrument, meterListener) =>
        {
            if (instrument.Meter.Name == MemoryDiagnosticsService.MeterName && instrument.Name == "techhub.test.probe")
            {
                meterListener.EnableMeasurementEvents(instrument);
            }
        };
        listener.SetMeasurementEventCallback<long>((_, measurement, _, _) => observed = measurement);
        listener.Start();

        // Act
        listener.RecordObservableInstruments();

        // Assert
        observed.Should().Be(7);
    }

    [Fact]
    public void RuntimeMetricAllowlist_OnlyContainsMetricsTheRuntimePublishes()
    {
        // Arrange — a misspelled name would silently drop the metric we want to keep
        var published = new HashSet<string>(StringComparer.Ordinal);
        using var listener = new MeterListener();
        listener.InstrumentPublished = (instrument, _) =>
        {
            if (instrument.Meter.Name == "System.Runtime")
            {
                published.Add(instrument.Name);
            }
        };

        // Act
        listener.Start();

        // Assert
        published.Should().IntersectWith(ServiceDefaultsExtensions.MemoryRuntimeMetrics);
        ServiceDefaultsExtensions.MemoryRuntimeMetrics.Should().BeSubsetOf(published);
    }

    [Fact]
    public async Task AddMemoryDiagnostics_ExposesTheCacheEntryCount()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddMemoryCache();
        services.AddMemoryDiagnostics();
        await using var provider = services.BuildServiceProvider();
        var cache = provider.GetRequiredService<IMemoryCache>();
        cache.Set("one", 1);
        cache.Set("two", 2);

        // Act
        var probe = provider.GetServices<MemoryDiagnosticsProbe>().Single(p => p.Name == "cache.entries");

        // Assert
        probe.Read().Should().Be(2);
        provider.GetServices<IHostedService>().Should().ContainSingle(s => s is MemoryDiagnosticsService);
    }

    [Fact]
    public void OpenTelemetry_ExportsUsefulMetricsButDropsHighVolumeNoise()
    {
        using var meter = new Meter("TechHub.MetricFilterTest");
        var noise = meter.CreateCounter<long>("aspnetcore.routing.match_attempts");
        var dns = meter.CreateCounter<long>("dns.lookup.duration");
        var pool = meter.CreateCounter<long>("aspnetcore.memory_pool.pooled");
        var requests = meter.CreateCounter<long>("http.server.request.duration");
        var circuits = meter.CreateCounter<long>("techhub.circuits.open");
        var exporter = new RecordingMetricExporter();
        using var reader = new PeriodicExportingMetricReader(exporter);
        var builder = Host.CreateApplicationBuilder();
        builder.ConfigureOpenTelemetry();
        builder.Services.AddOpenTelemetry().WithMetrics(metrics =>
            metrics.AddMeter(meter.Name).AddReader(reader));
        using var host = builder.Build();
        _ = host.Services.GetRequiredService<MeterProvider>();

        noise.Add(1);
        dns.Add(1);
        pool.Add(1);
        requests.Add(1);
        circuits.Add(1);
        reader.Collect().Should().BeTrue();

        exporter.Names.Should().Contain("http.server.request.duration").And.Contain("techhub.circuits.open");
        exporter.Names.Should().NotContain("aspnetcore.routing.match_attempts")
            .And.NotContain("dns.lookup.duration")
            .And.NotContain("aspnetcore.memory_pool.pooled");
    }

    private sealed class RecordingMetricExporter : BaseExporter<Metric>
    {
        public HashSet<string> Names { get; } = new(StringComparer.Ordinal);

        public override ExportResult Export(in Batch<Metric> batch)
        {
            foreach (var metric in batch)
            {
                Names.Add(metric.Name);
            }

            return ExportResult.Success;
        }
    }
}
