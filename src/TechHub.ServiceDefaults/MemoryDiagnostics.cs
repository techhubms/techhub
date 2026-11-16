using System.Diagnostics.Metrics;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TechHub.ServiceDefaults;

/// <summary>
/// A named app-level number (for example open circuits or cache entries) that is exported as the
/// metric <c>techhub.{Name}</c> and included in memory snapshot logs, so a heap size can be
/// related to what the app was holding at the time.
/// </summary>
public sealed record MemoryDiagnosticsProbe(string Name, Func<long> Read);

/// <summary>
/// Explains where memory goes. It exports the registered probes as metrics and, when a GC heap hard limit
/// is configured, logs one warning with a detailed snapshot (generation sizes, fragmentation, pinned objects,
/// threads and every probe) once the live heap after the last full GC reaches 80% of the limit.
/// </summary>
public sealed class MemoryDiagnosticsService : BackgroundService
{
    public const string MeterName = "TechHub.Diagnostics";

    internal const double WarnFraction = 0.8;
    private static readonly TimeSpan _sampleInterval = TimeSpan.FromSeconds(15);
    private static readonly TimeSpan _minimumLogInterval = TimeSpan.FromMinutes(5);

    private static readonly string[] _generationNames = ["gen0", "gen1", "gen2", "loh", "poh"];

    private readonly IReadOnlyList<MemoryDiagnosticsProbe> _probes;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<MemoryDiagnosticsService> _logger;
    private readonly Meter _meter = new(MeterName);

    public MemoryDiagnosticsService(
        IEnumerable<MemoryDiagnosticsProbe> probes,
        TimeProvider timeProvider,
        ILogger<MemoryDiagnosticsService> logger)
    {
        ArgumentNullException.ThrowIfNull(probes);
        ArgumentNullException.ThrowIfNull(timeProvider);
        ArgumentNullException.ThrowIfNull(logger);

        _probes = probes.ToList();
        _timeProvider = timeProvider;
        _logger = logger;

        foreach (var probe in _probes)
        {
            _meter.CreateObservableGauge($"techhub.{probe.Name}", probe.Read);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var limit = TryParseHeapHardLimit(Environment.GetEnvironmentVariable("DOTNET_GCHeapHardLimit"));
        if (limit is null)
        {
            _logger.LogInformation("No GC heap hard limit configured; memory snapshot warnings are disabled");
            return;
        }

        using var timer = new PeriodicTimer(_sampleInterval, _timeProvider);
        DateTimeOffset? lastLoggedAt = null;
        long lastLoggedGcIndex = 0;

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            var now = _timeProvider.GetUtcNow();

            // The snapshot only changes after a full GC, so the same one is never logged twice.
            if (ReadHeap() is { } snapshot
                && snapshot.GcIndex != lastLoggedGcIndex
                && ShouldLog(snapshot.RetainedBytes, limit.Value, lastLoggedAt, now))
            {
                lastLoggedAt = now;
                lastLoggedGcIndex = snapshot.GcIndex;
                LogSnapshot(snapshot, limit.Value);
            }
        }
    }

    public override void Dispose()
    {
        _meter.Dispose();
        base.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>Parses the hexadecimal value of <c>DOTNET_GCHeapHardLimit</c> (with or without a 0x prefix).</summary>
    internal static long? TryParseHeapHardLimit(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var hex = value.Trim();
        if (hex.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            hex = hex[2..];
        }

        return long.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var bytes) && bytes > 0
            ? bytes
            : null;
    }

    internal static bool ShouldLog(long retainedBytes, long limitBytes, DateTimeOffset? lastLoggedAt, DateTimeOffset now)
    {
        return limitBytes > 0
            && retainedBytes >= (long)(limitBytes * WarnFraction)
            && (lastLoggedAt is null || now - lastLoggedAt.Value >= _minimumLogInterval);
    }

    /// <summary>
    /// Reads the heap as of the most recent full (gen2) GC. An ephemeral GC leaves the old generations untouched, so
    /// their sizes would still include garbage that only a full GC removes and would overstate what the app retains.
    /// Returns <see langword="null"/> until a full GC has happened.
    /// </summary>
    internal static HeapSnapshot? ReadHeap()
    {
        var blocking = GC.GetGCMemoryInfo(GCKind.FullBlocking);
        var background = GC.GetGCMemoryInfo(GCKind.Background);
        var info = background.Index > blocking.Index ? background : blocking;
        if (info.Index == 0)
        {
            return null;
        }

        var generations = info.GenerationInfo;

        // A generation's size includes its free space, which is reported separately as fragmentation.
        long retained = 0;
        var text = new StringBuilder();
        for (var i = 0; i < generations.Length; i++)
        {
            var live = Math.Max(0, generations[i].SizeAfterBytes - generations[i].FragmentationAfterBytes);
            retained += live;
            if (i > 0)
            {
                text.Append(", ");
            }

            var name = i < _generationNames.Length ? _generationNames[i] : $"gen{i}";
            text.Append(CultureInfo.InvariantCulture, $"{name}={ToMb(live)} MB");
        }

        return new HeapSnapshot(
            info.Index,
            retained,
            info.TotalCommittedBytes,
            info.FragmentedBytes,
            info.PinnedObjectsCount,
            text.ToString());
    }

    /// <summary>Formats the probes as <c>name=value</c> pairs; a probe that throws is reported as <c>n/a</c>.</summary>
    internal static string FormatProbes(IEnumerable<MemoryDiagnosticsProbe> probes)
    {
        return string.Join(", ", probes.Select(probe =>
        {
            try
            {
                return $"{probe.Name}={probe.Read().ToString(CultureInfo.InvariantCulture)}";
            }
#pragma warning disable CA1031 // A failing probe must not stop the snapshot that explains a memory problem
            catch (Exception ex) when (ex is not (OutOfMemoryException or StackOverflowException or AccessViolationException))
#pragma warning restore CA1031
            {
                return $"{probe.Name}=n/a";
            }
        }));
    }

    private static long ToMb(long bytes) => bytes / (1024 * 1024);

    private void LogSnapshot(HeapSnapshot snapshot, long limitBytes)
    {
        _logger.LogWarning(
            "Live managed heap after full GC #{GcIndex} is {RetainedMb} MB, {PercentOfLimit}% of the {LimitMb} MB limit. " +
            "Live per generation: {Generations}. Committed {CommittedMb} MB, fragmentation {FragmentationMb} MB, " +
            "pinned objects {PinnedObjects}, working set {WorkingSetMb} MB, GCs gen0/gen1/gen2 {Gen0Collections}/{Gen1Collections}/{Gen2Collections}, " +
            "threads {ThreadCount}, queued work items {PendingWorkItems}. App state: {Probes}",
            snapshot.GcIndex,
            ToMb(snapshot.RetainedBytes),
            snapshot.RetainedBytes * 100 / limitBytes,
            ToMb(limitBytes),
            snapshot.Generations,
            ToMb(snapshot.CommittedBytes),
            ToMb(snapshot.FragmentationBytes),
            snapshot.PinnedObjects,
            ToMb(Environment.WorkingSet),
            GC.CollectionCount(0),
            GC.CollectionCount(1),
            GC.CollectionCount(2),
            ThreadPool.ThreadCount,
            ThreadPool.PendingWorkItemCount,
            FormatProbes(_probes));
    }

    internal readonly record struct HeapSnapshot(
        long GcIndex,
        long RetainedBytes,
        long CommittedBytes,
        long FragmentationBytes,
        long PinnedObjects,
        string Generations);
}

/// <summary>
/// Registration helpers for <see cref="MemoryDiagnosticsService"/>.
/// </summary>
public static class MemoryDiagnosticsExtensions
{
    /// <summary>
    /// Adds the memory diagnostics service plus the shared probe for the entry count of the in-memory cache.
    /// </summary>
    public static IServiceCollection AddMemoryDiagnostics(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Entry counts are only tracked when statistics are enabled; the counters are cheap.
        services.TryAddSingleton(TimeProvider.System);
        services.Configure<MemoryCacheOptions>(options => options.TrackStatistics = true);
        services.AddMemoryDiagnosticsProbe("cache.entries", sp =>
        {
            var cache = sp.GetService<IMemoryCache>() as MemoryCache;
            return () => cache?.GetCurrentStatistics()?.CurrentEntryCount ?? -1;
        });
        services.AddHostedService<MemoryDiagnosticsService>();

        return services;
    }

    /// <summary>
    /// Registers an app-level number that is exported as the metric <c>techhub.{name}</c> and added to memory snapshot logs.
    /// </summary>
    public static IServiceCollection AddMemoryDiagnosticsProbe(
        this IServiceCollection services,
        string name,
        Func<IServiceProvider, Func<long>> createRead)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(createRead);

        services.AddSingleton(sp => new MemoryDiagnosticsProbe(name, createRead(sp)));
        return services;
    }
}
