Performance monitoring is essential for ensuring that our applications run efficiently and reliably. .NET offers a set of tools to help with this, accessible via **IMeterFactory**. In this article, we’ll learn how to use these tools to check the health of our applications, measure performance, and collect data for optimization.

To download the source code for this article, you can visit our [GitHub repository](https://github.com/CodeMazeBlog/CodeMazeGuides/tree/main/aspnetcore-features/PerformanceMonitoringWithIMeterFactory).

So let’s get going.

## What Are .NET Metric Instruments? 

In .NET, we have various instruments available to capture an application’s performance data, such as:

- **Counter**: Tracks increasing counts, such as total requests or clicks**

**

- **Gauge**: Measures fluctuating non-cumulative values, like current memory consumption

- **UpDownCounter**: Captures values that can increase and decrease, such as queue sizes

- **Histogram**: Visualizes how data is distributed across ranges of values

In addition to these, there are observable instruments like **ObservableCounter**, **ObservableGauge**, and **ObservableUpDownCounter** that report their values as they are observed. 

These instruments are carefully designed for different monitoring needs, allowing accurate and meaningful performance tracking.

## Configure IMeterFactory in ASP.NET Core Web API

Let’s create an ASP.NET Core Web API project and configure it to use **IMeterFactory. **We need this before we can create and use the metric instruments described above.

**IMeterFactory** is part of the `System.Diagnostics.Metrics` NuGet package, which is included by default in .NET 8+. This means we can directly inject `IMeterFactory` into our classes. Let’s do that now by creating a `MetricsService` class:

```
public class MetricsService
{
 public MetricsService(IMeterFactory meterFactory)
 {
 var meter = meterFactory.Create("Metrics.Service"); 
 }
}
```

We define a `MetricsService` class and inject `IMeterFactory` into it to initialize a `Meter` instance. Now, we can use this `Meter` instance to define and capture metrics.

## Define the IMeterFactory Instruments

Next, let’s see how to capture various metrics.

Let’s declare a counter for holding the number of user clicks, a histogram for reporting response times, and a couple of variables for storing requests and memory consumption: 

```
public class MetricsService
{
 private readonly Counter* _userClicks;
 private readonly Histogram _responseTime;

 private int _requests;
 private double _memoryConsumption;

 public MetricsService(IMeterFactory meterFactory)
 {
 var meter = meterFactory.Create("Metrics.Service");

 _userClicks = meter.CreateCounter("metrics.service.user_clicks");

 _responseTime = meter.CreateHistogram("metrics.service.response_time");

 meter.CreateObservableCounter("metrics.service.requests", () => _requests);

 meter.CreateObservableGauge("metrics.service.memory_consumption", 
 _memoryConsumption);
 }
}
```

First, we initialize the counter using the `CreateCounter()` method. Next, we initialize the histogram metric with the `CreateHistogram()` method.

After that, we set up an observable counter using the `CreateObservableCounter()` method, which returns the value of `_requests` through a callback function. Similarly, we set up an observable gauge with the `CreateObservableGauge()` method, returning the `_memoryConsumption` value via its own callback function.

Since the only difference between a `Counter` and `UpDownCounter` is that the former can only increase in value, while the latter can increase and *decrease, we will not look at `UpDownCounter` here.

Is this material useful to you? Consider subscribing and get **ASP.NET Core Web API Best Practices** eBook for [FREE!](https://code-maze.com/free-ebook-aspnetcore-webapi-best-practices/)

## Capture the Metrics

Now, let’s add a few methods for recording these metric values in both an interface and in the class.

First, let’s create an `IMetricsService` interface and add a few method contracts:

```
public interface IMetricsService
{
 void RecordUserClick();
 void RecordResponseTime(double value);
 void RecordRequest();
 void RecordMemoryConsumption(double value);
}
```

Then, let’s implement this new interface in the `MetricsService` class:

```
public class MetricsService : IMetricsService
{
 // private fields and constructor omitted for brevity
 public void RecordUserClick()
 {
 _userClicks.Add(1);
 }

 public void RecordResponseTime(double value)
 {
 _responseTime.Record(value);
 }

 public void RecordRequest()
 {
 Interlocked.Increment(ref _requests);
 }

 public void RecordMemoryConsumption(double value)
 {
 _memoryConsumption = value;
 }
}
```

Here, the `RecordUserClick()` method increments the `_userClicks` counter by one to track the number of user clicks. The `RecordResponseTime()` method records the provided application’s response time using a histogram metric. The `RecordRequest()` method safely increments the `_requests` counter by one in a multi-threaded environment each time we call it, and the `RecordMemoryConsumption()` method updates the `_memoryConsumption` field with the provided value.

After that, let’s create a controller class and inject `IMetricsService` into it. In the controller, let’s add a GET method to generate some metrics data and record those using the `MetricsService` methods:

```
[Route("api/[controller]")]
[ApiController]
public class MetricsController(IMetricsService metricsService) : ControllerBase
{
 [HttpGet]
 public IActionResult Get()
 {
 var random = Random.Shared;

 metricsService.RecordUserClick();

 for (int i = 0; i 

Is this material useful to you? Consider subscribing and get **ASP.NET Core Web API Best Practices** eBook for [FREE!](https://code-maze.com/free-ebook-aspnetcore-webapi-best-practices/)

```
var builder = WebApplication.CreateBuilder(args);
...
builder.Services.AddSingleton();

var app = builder.Build();
...
app.Run();
```

This registers `MetricsService` as a singleton service for the `IMetricsService` interface throughout the application’s lifetime.

Also, make sure to add the `Swashbuckle.AspNetCore` NuGet package and configure Swagger UI in the `Program` class:

```
var builder = WebApplication.CreateBuilder(args);
...
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
 app.UseSwagger();
 app.UseSwaggerUI();
}
...
app.Run();
```

This will enable the Swagger UI and make it easier to visualize and run the API endpoints.

## Visualize the Metrics

Let’s run the API application, which should display the Swagger UI. To view the metrics, we’ll use the [dotnet-counters](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/dotnet-counters) tool.

First, we need to install the `dotnet-counters` tool using the `dotnet tool update` command:

`dotnet tool update -g dotnet-counters`

Once the tool is installed, we get a success message:

```
> dotnet tool update -g dotnet-counters

You can invoke the tool using the following command: dotnet-counters
Tool 'dotnet-counters' (version '9.0.553101') was successfully installed.
```

While the app is still running, let’s use `dotnet-counters` to monitor all the metrics from our application:

Is this material useful to you? Consider subscribing and get **ASP.NET Core Web API Best Practices** eBook for [FREE!](https://code-maze.com/free-ebook-aspnetcore-webapi-best-practices/)

`dotnet-counters monitor -n MetricsAPI --counters Metrics.Service`

Here, we specify the `dotnet-counters` tool to monitor all metrics in the **MetricsAPI** application coming from the **MetricsService** meter. Remember that the meter name is case-sensitive.

This will bring up the metrics screen, which will be empty as we haven’t yet run the endpoint to generate those:

```
Press p to pause, r to resume, q to quit.
 Status: Waiting for initial payload...

Name Current Value

```

Now, let’s invoke the `\metrics` endpoint from the Swagger UI to create the metrics and observe the output:

```
Press p to pause, r to resume, q to quit.
 Status: Running

Name Current Value
[Metrics.Service]
 metrics.service.memory_consumption 0.003
 metrics.service.requests (Count) 1 
 metrics.service.response_time
 Percentile
 50 0.567
 95 0.938
 99 0.988
 metrics.service.user_clicks (Count) 1 
```

As expected, we can see the output with all metrics collected. As we run the endpoint multiple times, notice how the `user_clicks` and `request` values increment by one. On the other hand, `response_time` shows the percentile of a large set of random samples. **Although** **the** **dotnet-counters tool renders Histogram instruments as three percentile statistics (50th, 95th, and 99th), other tools might summarize the distribution differently or offer more configuration options**. Similarly, `memory_consumption` shows a different value every time as it represents a Gauge.

## Add Clarity With Unit and Description

When we define instruments, we can specify an optional unit and description. These details do not change any calculations, but they can help us understand the data in the collection tool’s interface. Currently**, the dotnet-counters tool does not show the description text, but it does display the unit if provided**.

Let’s modify the `MetricsService` constructor, to specify the unit as Seconds and add a description while creating the histogram for capturing the response time:

```
_responseTime = meter.CreateHistogram(name: "metrics.service.response_time",
 unit: "Seconds",
 description: "This metric measures the time taken for the application to respond to user requests.");
```

The option is available on observable instruments as well.

Let’s modify the constructor further, to specify the unit as Megabytes and add a description while creating the observable gauge metric for memory consumption:

```
meter.CreateObservableGauge(name: "metrics.service.memory_consumption",
 () => _memoryConsumption,
 unit: "Megabytes",
 description: "This metric measures the amount of memory used by the application.");
```

Now, when we run the application, hit the endpoint, and observe the metrics, we can see that the response time shows the Seconds as units, while the memory consumption shows units as Megabytes:

```
Press p to pause, r to resume, q to quit.
 Status: Running
Name Current Value
[Metrics.Service]
 metrics.service.memory_consumption (Megabytes) 0.003
 metrics.service.requests (Count) 1
 metrics.service.response_time (Seconds)
 Percentile
 50 0.532
 95 0.952
 99 0.976
 metrics.service.user_clicks (Count) 1 
```

Leveraging this, we can add units to metrics to make the data more meaningful.

## Define Multi-Dimensional Metrics

Measurements can have tags that link them to key-value pairs, which helps organize data for analysis. We can use specific tags for `Counter` and `Histogram` measurements in the overloaded `Add()` and `Record()` methods, which accept one or more `KeyValuePair` arguments. For `ObservableCounter` and `ObservableGauge`, we can add tagged measurements in the callback provided to the constructor.

For example, to improve the metrics for user clicks by adding details like the user’s region and the feature clicked, we can create a method called `RecordUserClickDetailed()` in `MetricsService`. This method allows us to send these extra details to the overloaded `Counter.Add()` method:

```
public void RecordUserClickDetailed(string region, string feature)
{
 _userClicks.Add(1,
 new KeyValuePair("user.region", region),
 new KeyValuePair("user.feature", feature));
}
```

Similarly, let’s create a multi-dimensional gauge that reports detailed resource consumption, such as CPU, memory, and thread count. First, let’s add these additional fields to our `MetricService` class:

```
private double _cpu;
private double _memory;
private double _threadCount;
```

Next, let’s create a `GetResourceConsumption()` method that returns `IEnumerable>`:

```
private IEnumerable> GetResourceConsumption()
{
 return
 [
 new Measurement(_cpu, new KeyValuePair
 ("resource_usage", "cpu")),
 new Measuremcent(_memory, new KeyValuePair
 ("resource_usage", "memory")),
 new Measurement(_threadCount, new KeyValuePair
 ("resource_usage", "thread_count")),
 ];
}
```

Next, in our constructor, we need to update the callback in our observable Gauge creation to call our new `GetResourceConsumption()` method:

Is this material useful to you? Consider subscribing and get **ASP.NET Core Web API Best Practices** eBook for [FREE!](https://code-maze.com/free-ebook-aspnetcore-webapi-best-practices/)

```
public MetricsService(IMeterFactory meterFactory)
{
 // code omitted for brevity

 meter.CreateObservableGauge(name: "metrics.service.resource_consumption",
 () => GetResourceConsumption());
}
```

Additionally, let’s create a `RecordResourceUsage()` method for capturing the resource usage:

```
public void RecordResourceUsage(double currentCpuUsage, double currentMemoryUsage, double currentThreadCount)
{
 _cpu = currentCpuUsage;
 _memory = currentMemoryUsage;
 _threadCount = currentThreadCount;
}
```

Also, let’s create a Utility class and method to calculate the CPU usage:

```
public static class Utilities
{
 public static double GetCpuUsagePercentage()
 {
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

This method measures the CPU usage of the current process by capturing the CPU time used over a one-second period and calculating the percentage of CPU utilization.

Lastly, we need to be sure to update our `IMetricsService` interface with our two new recording methods:

```
void RecordUserClickDetailed(string region, string feature);
void RecordResourceUsage(double currentCpuUsage, double currentMemoryUsage, double currentThreadCount);
```

Now, let’s call these methods by adding them to the end of the GET endpoint in the controller, immediately before the final `return Ok()` line:

```
metricsService.RecordUserClickDetailed("US", "checkout");

metricsService.RecordResourceUsage(
 Utilities.GetCpuUsagePercentage(),
 GC.GetTotalAllocatedBytes() / (1024 * 1024),
 Process.GetCurrentProcess().Threads.Count);
```

While calling the `RecordUserClickDetailed()` method, we pass the region and feature.

When calling the method, we pass the values for CPU usage, total memory, and thread count. We utilize the `GetCpuUsagePercentage()` utility method to obtain CPU usage, while the `GC.GetTotalAllocatedBytes()` method can supply the total memory allocated to the current process and `Process.GetCurrentProcess().Threads.Count` report the number of threads running in that process. 

Now, when we run the application again, exercise the endpoint, and observe the metrics, we can see it displays these details as multi-dimensional metrics:

Is this material useful to you? Consider subscribing and get **ASP.NET Core Web API Best Practices** eBook for [FREE!](https://code-maze.com/free-ebook-aspnetcore-webapi-best-practices/)

```
Press p to pause, r to resume, q to quit.
 Status: Running

Name Current Value
[Metrics.Service]
 metrics.service.memory_consumption (Megabytes) 0.016
 metrics.service.requests (Count) 1 
 metrics.service.resource_consumption
 resource_usage
 cpu 0.482
 memory 6
 thread_count 50
 metrics.service.response_time (Seconds)
 Percentile
 50 0.419
 95 0.958
 99 0.983
 metrics.service.user_clicks (Count) 1
 user.feature user.region
 checkout US 1 

```

This is an excellent way to display metrics that have multiple dimensions.

## Test IMeterFactory Metrics Using MetricCollector

We can test any custom `IMeterFactory` metrics that we add to our application using the **MetricCollector** class. This class simplifies the process of recording measurements from specific instruments and helps us verify their accuracy. Let’s see how to do this.

First, we need to add the `Microsoft.Extensions.DependencyInjection` and `Microsoft.Extensions.Diagnostics.Testing` NuGet packages. Next, we need to define a `CreateServiceProvider()` to use in our test methods:

```
private static ServiceProvider CreateServiceProvider()
{
 var serviceCollection = new ServiceCollection();
 serviceCollection.AddMetrics();
 serviceCollection.AddSingleton();

 return serviceCollection.BuildServiceProvider();
}
```

The `CreateServiceProvider()` method sets up a dependency injection container. It creates a new `ServiceCollection`, adds metric services, and a singleton instance of `MetricsService`, and then builds and returns a service provider that can be used to resolve these services.

Let’s write a test for user click metrics using `MetricCollector`:

```
public void GivenMetricsConfigured_WhenUserClickRecorded_ThenCounterCaptured()
{
 // Arrange
 using var services = CreateServiceProvider();
 var metrics = services.GetRequiredService();
 var meterFactory = services.GetRequiredService();
 var collector = new MetricCollector(meterFactory, "Metrics.Service", "metrics.service.user_clicks");

 // Act
 metrics.RecordUserClick();

 // Assert
 var measurements = collector.GetMeasurementSnapshot();
 Assert.Single(measurements);
 Assert.Equal(1, measurements[0].Value);
}
```

This test verifies that `MetricsService` accurately records a user’s click. It sets up the required services and metrics collector and then calls the `RecordUserClick()` method in `MetricsService`. Afterward, it checks that the metrics collector has captured exactly one user click. Here, the metric collector will collect the specified metrics and return a snapshot of the measurements collected.

Similarly, let’s write a test for request metrics which uses `ObservableCounter`:

```
public void GivenMetricsConfigured_WhenRequestRecorded_ThenObservableCounterCaptured()
{
 // Arrange
 using var services = CreateServiceProvider();
 var metrics = services.GetRequiredService();
 var meterFactory = services.GetRequiredService();
 var collector = new MetricCollector(meterFactory, "Metrics.Service", "metrics.service.requests");

 // Act
 metrics.RecordRequest();

 // Assert
 collector.RecordObservableInstruments();
 var measurements = collector.GetMeasurementSnapshot();
 Assert.Single(measurements);
 Assert.Equal(1, measurements[0].Value);
}
```

This test verifies that the `MetricsService` accurately records a request. It sets up the necessary components, including a `MetricsService` and a `MetricCollector`, to capture metrics. The test then calls the `RecordRequest()` method on the `MetricsService` and checks that the observable counter for requests is incremented by one.

Is this material useful to you? Consider subscribing and get **ASP.NET Core Web API Best Practices** eBook for [FREE!](https://code-maze.com/free-ebook-aspnetcore-webapi-best-practices/)

When collecting observable metrics (`ObservableCounter`, `ObservableGauge`, etc), we need to call the `RecordObservableInstruments()` method on the `MetricsCollector` to scan all the observable metrics.

The `MetricCollector` simplifies the process of writing tests for the various metric collections in our application.

## IMeterFactory Best Practices 

Let’s explore best practices for choosing and implementing **IMeterFactory** instruments.** For DI-aware libraries, avoid static variables and opt for dependency injection (DI) instead**.

When creating a Meter, it’s important to choose a unique name. As discussed earlier, follow [OpenTelemetry naming guidelines](https://github.com/open-telemetry/semantic-conventions/blob/main/docs/general/metrics.md#general-guidelines) using a lowercase, dotted hierarchical structure and underscores to separate words for naming all constructs. Ensure the instrument name is unique across the system, often incorporating assembly or namespace names.

We should always choose the appropriate instrument based on need; however, keep in mind that the Observable equivalents may perform better in performance-intensive scenarios, such as when there are more than one million calls per second per thread.

If we need to understand the distribution’s tail, such as the 90th, 95th, and 99th percentiles, instead of just averages, use a histogram to measure event timings. For measuring cache, queue, and file sizes, opt for an **UpDownCounter** or **ObservableUpDownCounter** based on ease of integration into existing code, either through API calls for increments and decrements or a callback for current values from a maintained variable.

.NET APIs allow any string as a unit, but utilizing [UCUM](https://ucum.org/), the international standard for unit names is advisable. For multi-dimensional metrics, the API accepts any object as the tag value. However, collection tools typically expect numeric types and strings, making it crucial to provide these formats. Additionally, it’s recommended to follow the naming guidelines for tag names.

## Conclusion

In this article, we discussed how to set up IMeterFactory in an ASP.NET Core Web API to effectively track various metrics. We looked at how to display these metrics using the dotnet-counters tool and shared tips for choosing and using the different metrics. Finally, we wrapped up with a discussion of best practices and testing metric collection in our code.