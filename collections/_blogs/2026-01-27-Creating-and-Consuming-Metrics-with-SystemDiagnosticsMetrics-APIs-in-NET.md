---
external_url: https://andrewlock.net/creating-and-consuming-metrics-with-system-diagnostics-metrics-apis/
title: Creating and Consuming Metrics with System.Diagnostics.Metrics APIs in .NET
author: Andrew Lock
primary_section: coding
feed_name: Andrew Lock's Blog
date: 2026-01-27 10:00:00 +00:00
tags:
- .NET
- .NET Core
- .NET Counters
- ASP.NET Core
- Blogs
- C#
- Coding
- Custom Metrics
- Datadog
- Dependency Injection
- DevOps
- Diagnostics
- Instrumentation
- Metrics API
- Monitoring
- Observability
- OpenTelemetry
- Performance Monitoring
- System.Diagnostics.Metrics
- Web API
section_names:
- coding
- devops
---
Andrew Lock provides a comprehensive walkthrough on leveraging System.Diagnostics.Metrics APIs in .NET, guiding developers through instrumentation concepts, monitoring with dotnet-counters, and building custom metrics for production-grade observability.<!--excerpt_end-->

# Creating and Consuming Metrics with System.Diagnostics.Metrics APIs in .NET

## Introduction

Andrew Lock explores the System.Diagnostics.Metrics API introduced in .NET 6 and earlier via NuGet, explaining how to create and consume metrics in .NET applications for observability and monitoring.

## System.Diagnostics.Metrics API Overview

- **Purpose**: Enables developers to record and report application metrics (counters, gauges, histograms) programmatically.
- **Interoperability**: Designed to work with OpenTelemetry and third-party monitoring tools. Accessible via .NET SDK tools like `dotnet-counters` and third-party systems like Datadog.

### Key Concepts

- **Instrument**: Represents a specific metric (e.g., products sold, GC heap size).
- **Meter**: Logical grouping of instruments (e.g., built-in .NET runtime meters).
- **Types of Instruments**:
  - `Counter<T>`, `ObservableCounter<T>`: For event counting (e.g., requests received).
  - `UpDownCounter<T>`, `ObservableUpDownCounter<T>`: For metrics that can increase/decrease (e.g., active requests).
  - `Gauge<T>`, `ObservableGauge<T>`: For tracking current values (e.g., memory used).
  - `Histogram<T>`: For statistical distributions (e.g., request duration).
- **Observability**: Instruments’ observable variants pull values on demand, improving performance and flexibility.

## Collecting Metrics with dotnet-counters

- `dotnet-counters` is a Microsoft tool for live metric monitoring and investigation.
- **Installation**:

  ```bash
  dotnet tool install -g dotnet-counters
  ```

- **Usage**:

  ```bash
  dotnet-counters monitor -n MyApp
  # or
  dotnet-counters monitor -p 123
  dotnet-counters monitor -- dotnet MyApp.dll
  ```

- **Features**: Allows filtering by Meter, displays detailed metric values, supports additional export formats for production use.
- **Tagging**: Metrics support tags for grouping (e.g., request method, route, product_id). Managing metric tag cardinality is crucial for storage and performance.

## Creating Custom Metrics

- Developers can define application-specific metrics for business logic or operational insight.
- **Example Application**: Tracking the number of times a product pricing endpoint is accessed.
- **Setup**:

  ```bash
  dotnet new web
  ```

  Application snippet:

  ```csharp
  var builder = WebApplication.CreateBuilder(args);
  var app = builder.Build();
  app.MapGet("/product/{id}", (int id) => {
      // TODO: add metrics
      return $"Pricing for product {id}";
  });
  app.Run();
  ```

### Instrumentation with Meter/Instrument

- Encapsulate metric logic in a helper class:

  ```csharp
  public class ProductMetrics
  {
      private readonly Counter<long> _pricingDetailsViewed;
      public ProductMetrics(IMeterFactory meterFactory)
      {
          var meter = meterFactory.Create("MyApp.Products");
          _pricingDetailsViewed = meter.CreateCounter<long>("myapp.products.pricing_page_requests");
      }
      public void PricingPageViewed(int id)
      {
          _pricingDetailsViewed.Add(1, new KeyValuePair<string, object?>("product_id", id));
      }
  }
  ```

- Follows naming conventions for clarity and OpenTelemetry compatibility.

### Integrating with DI and Usage

- Register in DI:

  ```csharp
  builder.Services.AddSingleton<ProductMetrics>();
  ```

- Use in endpoint handler:

  ```csharp
  app.MapGet("/product/{id}", (int id, ProductMetrics metrics) => {
      metrics.PricingPageViewed(id);
      return $"Details for product {id}";
  });
  ```

## Testing and Extending Metrics

- Launch the app and monitor with:

  ```bash
  dotnet run
  dotnet-counters monitor -n MyApp --counters MyApp.Products
  ```

- Hit the endpoint to view dynamic metrics reflecting usage.

### Enhancing Metrics with Metadata

- Add units and descriptions for better consumer interoperability:

  ```csharp
  _pricingDetailsViewed = meter.CreateCounter<int>(
      "myapp.products.pricing_page_requests",
      unit: "requests",
      description: "The number of requests to the pricing details page for the product with the given product_id");
  ```

## Conclusion

- The System.Diagnostics.Metrics APIs simplify custom and built-in metric creation in .NET applications.
- dotnet-counters provides easy, flexible local metric inspection.
- Application-specific metrics can be implemented in a scalable, maintainable way, providing compatibility with observability tools like OpenTelemetry and Datadog.

---

*Original author: Andrew Lock*

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/creating-and-consuming-metrics-with-system-diagnostics-metrics-apis/)
