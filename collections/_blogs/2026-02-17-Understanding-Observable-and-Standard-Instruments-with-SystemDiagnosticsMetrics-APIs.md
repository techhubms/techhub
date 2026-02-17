---
layout: "post"
title: "Understanding Observable and Standard Instruments with System.Diagnostics.Metrics APIs"
description: "This post offers a detailed overview of the different instrument types provided by the System.Diagnostics.Metrics API, focusing on their usage within .NET libraries and ASP.NET Core. You'll learn how to create, configure, and use standard and observable metric instruments, including counters, gauges, up/down counters, and histograms, with practical examples and real-world scenarios."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/creating-standard-and-observable-instruments/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: "https://andrewlock.net/rss.xml"
date: 2026-02-17 10:00:00 +00:00
permalink: "/2026-02-17-Understanding-Observable-and-Standard-Instruments-with-SystemDiagnosticsMetrics-APIs.html"
categories: ["Coding"]
tags: [".NET", ".NET 10", ".NET 6", ".NET Core", ".NET Monitor", "ASP.NET Core", "Blogs", "Coding", "Counter", "DiagnosticSource", "Gauge", "Histogram", "Instrumentation", "Metrics", "Monitoring", "Observability", "ObservableCounter", "ObservableGauge", "OpenTelemetry", "System.Diagnostics.Metrics", "UpDownCounter"]
tags_normalized: ["dotnet", "dotnet 10", "dotnet 6", "dotnet core", "dotnet monitor", "aspdotnet core", "blogs", "coding", "counter", "diagnosticsource", "gauge", "histogram", "instrumentation", "metrics", "monitoring", "observability", "observablecounter", "observablegauge", "opentelemetry", "systemdotdiagnosticsdotmetrics", "updowncounter"]
---

Andrew Lock provides a deep dive into the various instrument types featured in the System.Diagnostics.Metrics API. The article explores practical examples from .NET libraries and ASP.NET Core to illustrate how developers can record and observe metrics in their applications.<!--excerpt_end-->

# Understanding Observable and Standard Instruments with System.Diagnostics.Metrics APIs

## Introduction

In this article, Andrew Lock details the types of metric instruments exposed by the `System.Diagnostics.Metrics` APIs. The discussion focuses on their use in .NET and ASP.NET Core, highlighting both standard and observable instruments along with real-world code examples.

## System.Diagnostics.Metrics APIs Overview

The Metrics API, introduced in .NET 6 and available to earlier runtimes via the [`System.Diagnostics.DiagnosticSource`](https://www.nuget.org/packages/System.Diagnostics.DiagnosticSource/) package, exposes two primary objects:

- **Instrument**: Records values for a specific metric (e.g., products sold, heap size).
- **Meter**: Logical grouping of multiple instruments (e.g., `System.Runtime` Meter for runtime metrics).

There are seven instrument types (as of .NET 10):

- `Counter<T>`
- `ObservableCounter<T>`
- `UpDownCounter<T>`
- `ObservableUpDownCounter<T>`
- `Gauge<T>`
- `ObservableGauge<T>`
- `Histogram<T>`

Creating a custom metric involves choosing an appropriate instrument and associating it with a Meter.

## Standard vs Observable Instruments

- **Standard instrument**: The producer emits metric values as events occur. For example, a counter is incremented when a new HTTP request is handled.
- **Observable instrument**: The consumer requests the metric value on demand, suitable for data that is expensive or unnecessary to emit continuously (e.g., total managed heap allocations).

**Example:**

- Standard (push): ASP.NET Core emits `http.server.active_requests` as requests start.
- Observable (pull): `dotnet.gc.pause.time` is reported only when requested by a tool like `dotnet-monitor`.

## Instrument Types: Details & Examples

### Counter<T>

- **Purpose**: Count events, such as exceptions caught.
- **Example**: `aspnetcore.diagnostics.exceptions` counts exceptions in middleware.
- **Code:**

  ```csharp
  _handlerExceptionCounter = _meter.CreateCounter<long>(
    "aspnetcore.diagnostics.exceptions",
    unit: "{exception}",
    description: "Number of exceptions caught by exception handling middleware.");
  _handlerExceptionCounter.Add(1, tags);
  ```

### ObservableCounter<T>

- **Purpose**: Monotonically increasing metric, updated when observed.
- **Example**: `dotnet.gc.heap.total_allocated` via `GC.GetTotalAllocatedBytes()`.
- **Code:**

  ```csharp
  s_meter.CreateObservableCounter(
    "dotnet.gc.heap.total_allocated",
    () => GC.GetTotalAllocatedBytes(),
    unit: "By",
    description: "Number of bytes allocated on the managed GC heap.");
  ```

### UpDownCounter<T>

- **Purpose**: Track values that increase or decrease (e.g., active requests).
- **Example**: `http.server.active_requests` in ASP.NET Core Hosting.
- **Code:**

  ```csharp
  _activeRequestsCounter = _meter.CreateUpDownCounter<long>(
    "http.server.active_requests",
    unit: "{request}",
    description: "Number of active HTTP server requests.");
  _activeRequestsCounter.Add(1, tags); // On request start
  _activeRequestsCounter.Add(-1, tags); // On request end
  ```

### ObservableUpDownCounter<T>

- **Purpose**: Reports absolute value of metric (positive/negative) when observed.
- **Example**: `dotnet.gc.last_collection.heap.size` returns heap size per GC generation.
- **Code:**

  ```csharp
  s_meter.CreateObservableUpDownCounter(
    "dotnet.gc.last_collection.heap.size",
    GetHeapSizes,
    unit: "By",
    description: "Managed GC heap size after collection.");
  ```

### Gauge<T>

- **Purpose**: Record current (non-additive) values; overwrites previous value.
- **Example**: (Hypothetical) Room temperature sensor metric.
- **Code:**

  ```csharp
  var instrument = _meter.CreateGauge<double>(
    name: "locations.room.temperature",
    unit: "°C",
    description: "Current room temperature"
  );
  instrument.Record(newTemperature, new KeyValuePair<string, object?>("room", "office"));
  ```

### ObservableGauge<T>

- **Purpose**: Gauge polled by consumer; used in system monitoring scenarios.
- **Example**: `process.cpu.utilization` in Microsoft.Extensions.Diagnostics.ResourceMonitoring.
- **Code:**

  ```csharp
  meter.CreateObservableGauge(
    name: "process.cpu.utilization",
    observeValue: CpuPercentage
  );
  ```

### Histogram<T>

- **Purpose**: Record distribution of arbitrary values for analytical/statistical purposes (i.e., latencies).
- **Example**: `http.server.request.duration` captures request durations with tags for status code, route, etc.
- **Code:**

  ```csharp
  _requestDuration = _meter.CreateHistogram<double>(
    "http.server.request.duration",
    unit: "s",
    description: "Duration of HTTP server requests."
  );
  _requestDuration.Record(duration.TotalSeconds, tags);
  ```

## Summary

- Introduced all instrument types from `System.Diagnostics.Metrics` APIs.
- Provided practical, real-world examples from .NET and ASP.NET Core.
- Explained scenarios for choosing observable vs. standard instruments.
- Previewed future discussion on in-process metric recording.

For further experimentation, developers can start instrumenting their own code using these APIs and monitor metrics using OpenTelemetry, `dotnet-counters`, or `dotnet-monitor`.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/creating-standard-and-observable-instruments/)
