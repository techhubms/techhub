---
external_url: https://code-maze.com/dotnet-imeterfactory-application-performance/
title: Measure Application Performance in .NET Using IMeterFactory
author: Muhammed Saleem
feed_name: Code Maze Blog
date: 2025-02-26 08:47:52 +00:00
tags:
- .NET
- .NET Counters
- ASP.NET Core
- C#
- Dependency Injection
- IMeterFactory
- MetricCollector
- Metrics
- Performance
- Performance Monitoring
- Swagger UI
- System.Diagnostics.Metrics
- Testing
- Web API
section_names:
- coding
primary_section: coding
---
Authored by Muhammed Saleem, this article explores practical techniques for integrating and utilizing IMeterFactory to measure and optimize application performance in .NET, focusing on metrics collection and monitoring within ASP.NET Core applications.<!--excerpt_end-->

# Measure Application Performance in .NET Using IMeterFactory

**Author:** Muhammed Saleem  
**Originally appeared on [Code Maze](https://code-maze.com/dotnet-imeterfactory-application-performance/)**

## Introduction

Performance monitoring is essential for ensuring applications run efficiently and reliably. .NET provides robust tools for this purpose, accessible via the `IMeterFactory` API. This article explains the use of these tools to check application health, measure performance, collect optimization data, and adhere to best practices for metric instrumentation in .NET, particularly within ASP.NET Core Web APIs.

## .NET Metric Instruments Overview

.NET provides a variety of metric instruments:

- **Counter<T>**: Tracks increasing counts (e.g., total requests or clicks).
- **Gauge<T>**: Measures fluctuating non-cumulative values (e.g., current memory consumption).
- **UpDownCounter<T>**: Captures values that can both increase and decrease (e.g., queue sizes).
- **Histogram<T>**: Visualizes data distribution across value ranges.
- **ObservableCounter<T>**, **ObservableGauge<T>**, **ObservableUpDownCounter<T>**: Report their values as observed in real-time.

These instruments support accurate, meaningful, and diverse monitoring needs.

## Configuring IMeterFactory in ASP.NET Core Web API

IMeterFactory is part of the `System.Diagnostics.Metrics` namespace, included by default in .NET 8+. Here's how to set it up:

```csharp
public class MetricsService {
    public MetricsService(IMeterFactory meterFactory) {
        var meter = meterFactory.Create("Metrics.Service");
        // Metric instrument initialization follows
    }
}
```

Using Dependency Injection (DI), `IMeterFactory` is provided to the `MetricsService`, allowing direct creation of metrics collectors.

## Defining Metric Instruments

Example code to declare and initialize common metric instruments:

```csharp
public class MetricsService {
    private readonly Counter<int> _userClicks;
    private readonly Histogram<double> _responseTime;
    private int _requests;
    private double _memoryConsumption;

    public MetricsService(IMeterFactory meterFactory) {
        var meter = meterFactory.Create("Metrics.Service");
        _userClicks = meter.CreateCounter<int>("metrics.service.user_clicks");
        _responseTime = meter.CreateHistogram<double>("metrics.service.response_time");
        meter.CreateObservableCounter("metrics.service.requests", () => _requests);
        meter.CreateObservableGauge("metrics.service.memory_consumption", () => _memoryConsumption);
    }
}
```

- **Counter** and **Histogram** are for event counts and duration/distribution, respectively.
- **ObservableCounter** and **ObservableGauge** are initialized with lambda callbacks that return current values.

## Capturing Metrics

Define an interface for the service:

```csharp
public interface IMetricsService {
    void RecordUserClick();
    void RecordResponseTime(double value);
    void RecordRequest();
    void RecordMemoryConsumption(double value);
}
```

Implement the interface:

```csharp
public class MetricsService : IMetricsService {
    // Private fields and constructor omitted for brevity
    public void RecordUserClick() {
        _userClicks.Add(1);
    }
    public void RecordResponseTime(double value) {
        _responseTime.Record(value);
    }
    public void RecordRequest() {
        Interlocked.Increment(ref _requests);
    }
    public void RecordMemoryConsumption(double value) {
        _memoryConsumption = value;
    }
}
```

### Using Metrics in Controllers

Inject `IMetricsService` into your controller and record metrics:

```csharp
[Route("api/[controller]")]
[ApiController]
public class MetricsController(IMetricsService metricsService) : ControllerBase {
    [HttpGet]
    public IActionResult Get() {
        var random = Random.Shared;
        metricsService.RecordUserClick();
        for (int i = 0; i < 100; i++) {
            metricsService.RecordResponseTime(random.NextDouble());
        }
        metricsService.RecordRequest();
        metricsService.RecordMemoryConsumption(GC.GetAllocatedBytesForCurrentThread() / (1024 * 1024));
        return Ok();
    }
}
```

### Registering the Service

Register `MetricsService` as a singleton for `IMetricsService` in your application setup:

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IMetricsService, MetricsService>();
var app = builder.Build();
app.Run();
```

Optionally, add Swagger for API exploration:

```csharp
builder.Services.AddSwaggerGen();
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

## Visualizing the Metrics

Install the [dotnet-counters](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/dotnet-counters) tool for runtime metrics monitoring:

```sh
dotnet tool update -g dotnet-counters
```

To monitor the metrics for your API:

```sh
dotnet-counters monitor -n MetricsAPI --counters Metrics.Service
```

Run your API and invoke endpoints to generate real-time metric data. Data such as user clicks, request counts, response time percentiles, and memory consumption can be observed live in the console.

## Enhancing Clarity: Units and Descriptions

Add units and descriptions when defining instruments to improve data presentation:

```csharp
_responseTime = meter.CreateHistogram<double>(
    name: "metrics.service.response_time",
    unit: "Seconds",
    description: "This metric measures the time taken for the application to respond to user requests."
);
meter.CreateObservableGauge(
    name: "metrics.service.memory_consumption",
    () => _memoryConsumption,
    unit: "Megabytes",
    description: "This metric measures the amount of memory used by the application."
);
```

Visible units aid interpretation in tools like dotnet-counters, even if the description is not displayed.

## Multi-Dimensional Metrics (Tags)

Metrics can be tagged to produce additional dimensions (e.g., region, feature):

```csharp
public void RecordUserClickDetailed(string region, string feature) {
    _userClicks.Add(1, new KeyValuePair<string, object?>("user.region", region), new KeyValuePair<string, object?>("user.feature", feature));
}
```

For resource consumption:

```csharp
private double _cpu, _memory, _threadCount;
private IEnumerable<Measurement<double>> GetResourceConsumption() {
    return [
        new Measurement<double>(_cpu, new KeyValuePair<string,object?>("resource_usage", "cpu")),
        new Measurement<double>(_memory, new KeyValuePair<string,object?>("resource_usage", "memory")),
        new Measurement<double>(_threadCount, new KeyValuePair<string,object?>("resource_usage", "thread_count")),
    ];
}
meter.CreateObservableGauge(name: "metrics.service.resource_consumption", () => GetResourceConsumption());
```

Calls to the above metric tracking enhance observability with rich context.

## Utility Class Example (CPU Usage Calculation)

```csharp
public static class Utilities {
    public static double GetCpuUsagePercentage() {
        var process = Process.GetCurrentProcess();
        var startTime = DateTime.UtcNow;
        var initialCpuTime = process.TotalProcessorTime;
        Thread.Sleep(1000);
        var endTime = DateTime.UtcNow;
        var finalCpuTime = process.TotalProcessorTime;
        var totalCpuTimeUsed = (finalCpuTime - initialCpuTime).TotalMilliseconds;
        var totalTimeElapsed = (endTime - startTime).TotalMilliseconds;
        var cpuUsage = (totalCpuTimeUsed / (Environment.ProcessorCount * totalTimeElapsed)) * 100;
        return cpuUsage;
    }
}
```

## Testing Metrics with MetricCollector

Use `MetricCollector<T>` from the `Microsoft.Extensions.Diagnostics.Testing` package to verify custom metrics:

```csharp
private static ServiceProvider CreateServiceProvider() {
    var serviceCollection = new ServiceCollection();
    serviceCollection.AddMetrics();
    serviceCollection.AddSingleton<MetricsService>();
    return serviceCollection.BuildServiceProvider();
}

public void GivenMetricsConfigured_WhenUserClickRecorded_ThenCounterCaptured() {
    using var services = CreateServiceProvider();
    var metrics = services.GetRequiredService<MetricsService>();
    var meterFactory = services.GetRequiredService<IMeterFactory>();
    var collector = new MetricCollector<int>(meterFactory, "Metrics.Service", "metrics.service.user_clicks");
    metrics.RecordUserClick();
    var measurements = collector.GetMeasurementSnapshot();
    Assert.Single(measurements);
    Assert.Equal(1, measurements[0].Value);
}
```

Call `RecordObservableInstruments()` before reading from observable counters/gauges.

## IMeterFactory Best Practices

- Prefer dependency injection for DI-aware libraries over static variable usage.
- Use unique, lower-case, dot-separated names for meters and instruments, often including assembly or namespace context (see [OpenTelemetry guidelines](https://github.com/open-telemetry/semantic-conventions/blob/main/docs/general/metrics.md#general-guidelines)).
- Match instrument type to scenario: use **Observable** instruments for high-frequency events.
- Use histograms for event timings requiring percentile distribution.
- Use **UpDownCounter** for quantities that both increment and decrement (e.g., current queue size).
- Adopt standard strings (such as [UCUM](https://ucum.org/)) for units, and prefer numeric/string types for metric tags.

## Conclusion

This article presented a practical approach to implementing performance monitoring in .NET applications using IMeterFactory. Key takeaways include:

- Instrumenting code with appropriate metrics.
- Using dotnet-counters for visualization.
- Employing best practices for naming, tagging, and unit descriptions.
- Utilizing MetricCollector for test-driven development.

These steps contribute to building more maintainable, observable, high-performing applications in the .NET ecosystem.

This post appeared first on "Code Maze Blog". [Read the entire article here](https://code-maze.com/dotnet-imeterfactory-application-performance/)
