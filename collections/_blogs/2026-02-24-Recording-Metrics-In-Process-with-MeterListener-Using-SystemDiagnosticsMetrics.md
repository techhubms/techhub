---
layout: "post"
title: "Recording Metrics In-Process with MeterListener Using System.Diagnostics.Metrics"
description: "This article by Andrew Lock demonstrates how to use MeterListener from the System.Diagnostics.Metrics API to collect, aggregate, and display metrics in a .NET ASP.NET Core application. It walks through implementing a custom MetricManager, setting up metrics collection and aggregation, handling both standard and observable instruments, and visualizing metrics with Spectre.Console. Readers will learn key practices for in-process metrics and performance considerations when building observability features directly in .NET applications."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/recording-metrics-in-process-using-meterlistener/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: "https://andrewlock.net/rss.xml"
date: 2026-02-24 10:00:00 +00:00
permalink: "/2026-02-24-Recording-Metrics-In-Process-with-MeterListener-Using-SystemDiagnosticsMetrics.html"
categories: ["Coding", "DevOps"]
tags: [".NET", ".NET 6", ".NET Core", "Aggregation", "ASP.NET Core", "BackgroundService", "Blogs", "Coding", "DevOps", "HttpClient", "Instrument", "Interlocked", "Measurement", "MeterListener", "Metrics", "Monitoring", "Observability", "ObservableInstrument", "OpenTelemetry", "Performance", "Resource Monitoring", "Spectre.Console", "System.Diagnostics.Metrics"]
tags_normalized: ["dotnet", "dotnet 6", "dotnet core", "aggregation", "aspdotnet core", "backgroundservice", "blogs", "coding", "devops", "httpclient", "instrument", "interlocked", "measurement", "meterlistener", "metrics", "monitoring", "observability", "observableinstrument", "opentelemetry", "performance", "resource monitoring", "spectredotconsole", "systemdotdiagnosticsdotmetrics"]
---

Andrew Lock explores how to collect and process application metrics in .NET using MeterListener from System.Diagnostics.Metrics. He explains the MetricManager pattern, in-process aggregation, and visualizing metrics with Spectre.Console.<!--excerpt_end-->

# Recording Metrics In-Process with MeterListener Using System.Diagnostics.Metrics

_Andrew Lock_

## Introduction

This guide covers how to use the `MeterListener` API from `System.Diagnostics.Metrics` in a .NET application to listen to and process metrics in-process, rather than relying exclusively on external tools such as OpenTelemetry or Datadog. You'll learn how to collect, aggregate, and report application metrics—and see a practical example using a simple ASP.NET Core app and a custom `MetricManager`.

## Creating a Test ASP.NET Core App

The example application is a minimal ASP.NET Core API (created with `dotnet new web`) that continuously sends requests to itself. This load generation provides meaningful data for recorded metrics:

```csharp
// Very basic hello-world app
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

var task = app.RunAsync();

// Obtain the app's active address
var address = app.Services.GetRequiredService<IServer>()
    .Features.Get<IServerAddressesFeature>()
    .Addresses.First();

try {
    // Four parallel loops send HTTP GET requests continuously until the app is shut down
    await Parallel.ForAsync(0, 4, app.Lifetime.ApplicationStopping, async (i, ct) => {
        var httpClient = new HttpClient() { BaseAddress = new Uri(address) };
        while (!ct.IsCancellationRequested) {
            string _ = await httpClient.GetStringAsync("/");
        }
    });
} catch (OperationCanceledException) {
    // Expected on shutdown
}
await task;
```

## Displaying Metrics with Spectre.Console

Metrics are displayed in a live-updating Spectre.Console table. Core metrics monitored include ASP.NET Core routing attempts, GC heap allocations, active requests, heap sizes by generation, CPU utilization, and request durations.

## Building the MetricManager Helper

The `MetricManager` class encapsulates collecting and aggregating values from the `System.Diagnostics.Metrics` API. Its API includes methods for disposal and metric value retrieval:

```csharp
public class MetricManager : IDisposable {
    public void Dispose();
    public MetricValues GetMetrics();
}

public readonly record struct MetricValues(
    long TotalMatchAttempts,
    long TotalHeapAllocated,
    long ActiveRequests,
    long HeapSizeGen0,
    long HeapSizeGen1,
    long HeapSizeGen2,
    long HeapSizeLoh,
    long HeapSizePoh,
    double CpuUtilization,
    double AverageDuration,
    long TotalRequests
);
```

## Configuring MeterListener and Callbacks

A `MeterListener` is created within `MetricManager`, with callbacks assigned for published instruments and for measurements of various value types (in the example, `long` and `double`). Allocation-aware APIs are used to avoid unnecessary performance overhead.

```csharp
_listener = new() { InstrumentPublished = OnInstrumentPublished };
_listener.SetMeasurementEventCallback<long>(OnMeasurementRecordedLong);
_listener.SetMeasurementEventCallback<double>(OnMeasurementRecordedDouble);
_listener.Start();
```

## Selecting Interesting Instruments

The `OnInstrumentPublished` callback filters for specific `Meter` and `Instrument` combinations relevant to the application's metrics. Only matching instruments are enabled with `EnableMeasurementEvents`.

```csharp
private void OnInstrumentPublished(Instrument instrument, MeterListener listener) {
    string meterName = instrument.Meter.Name;
    string instrumentName = instrument.Name;
    var enable = meterName switch {
        "Microsoft.AspNetCore.Routing" => instrumentName == "aspnetcore.routing.match_attempts",
        "System.Runtime" => instrumentName is "dotnet.gc.heap.total_allocated" or "dotnet.gc.last_collection.heap.size",
        "Microsoft.AspNetCore.Hosting" => instrumentName is "http.server.active_requests" or "http.server.request.duration",
        "Microsoft.Extensions.Diagnostics.ResourceMonitoring" => instrumentName == "process.cpu.utilization",
        _ => false
    };
    if (enable) {
        listener.EnableMeasurementEvents(instrument, state: this);
    }
}
```

## Handling Measurements

For each supported value type, a callback aggregates metric values. For counters, values are added; for observable values, the latest measurement is retained. For histogram types, logic is provided to compute averages.

```csharp
private static void OnMeasurementRecordedLong(Instrument instrument, long measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state) { /* ... */ }
private static void OnMeasurementRecordedDouble(Instrument instrument, double measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state) { /* ... */ }
```

Concurrency is handled using `Interlocked` methods and locks where necessary.

## Triggering Observable Instruments

Calling `RecordObservableInstruments()` in `GetMetrics()` ensures that all observable metrics are polled and reported before reading their values.

## Deploying the Metric Display Service

A `BackgroundService` displays metrics in a periodically refreshed Spectre.Console table. It uses the API of `MetricManager` and updates the output every second.

```csharp
protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
    using var manager = new MetricManager();
    var table = /* ... Table setup ... */;
    await AnsiConsole.Live(table).StartAsync(async ctx => {
        while (!stoppingToken.IsCancellationRequested) {
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            RenderMetricValues(table, ctx, manager.GetMetrics());
        }
    });
}
```

Finally, the `MetricDisplayService` is registered as a hosted service in the ASP.NET Core app.

## Conclusion

This article demonstrates how to use the .NET `MeterListener` API to collect, aggregate, and report in-process metrics without external collection tools. It's a practical foundation for building custom observability solutions or prototyping before moving to production-grade solutions like OpenTelemetry.

> **Note:** For advanced production scenarios, consider using OpenTelemetry for broader observability features and integration.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/recording-metrics-in-process-using-meterlistener/)
