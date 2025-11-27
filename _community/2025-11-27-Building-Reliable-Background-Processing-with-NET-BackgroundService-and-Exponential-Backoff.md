---
layout: "post"
title: "Building Reliable Background Processing with .NET BackgroundService and Exponential Backoff"
description: "This article, authored by riturajjana, provides an in-depth guide to implementing background processing in .NET applications using BackgroundService and exponential backoff. It addresses the challenge of unreliable downstream APIs by architecting a non-blocking, resilient system. Complete with use case, architecture, reasoning for backoff, and robust example code, it is ideal for developers building enterprise-grade backend solutions."
author: "riturajjana"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/microsoft-developer-community/background-tasks-in-net/ba-p/4472341"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-27 08:00:00 +00:00
permalink: "/2025-11-27-Building-Reliable-Background-Processing-with-NET-BackgroundService-and-Exponential-Backoff.html"
categories: ["Azure", "Coding"]
tags: [".NET", "API Reliability", "ASP.NET Core", "Azure", "Backend Development", "BackgroundService", "C#", "Cloud Architecture", "Coding", "Community", "Dependency Resilience", "Exponential Backoff", "Hosted Services", "in Memory Store", "Microservices", "REST API", "Retry Logic"]
tags_normalized: ["dotnet", "api reliability", "aspdotnet core", "azure", "backend development", "backgroundservice", "csharp", "cloud architecture", "coding", "community", "dependency resilience", "exponential backoff", "hosted services", "in memory store", "microservices", "rest api", "retry logic"]
---

riturajjana explores .NET background processing with BackgroundService and exponential backoff, guiding developers through a resilient architecture for handling unreliable APIs, and providing complete, production-ready code samples.<!--excerpt_end-->

# Building Reliable Background Processing in .NET

#### What is a Background Task?

A background task (or service) executes work independently in the background, allowing your main application or UI to remain responsive even while waiting on external systems or APIs.

#### The Problem

How do you keep your UI and main API responsive when a downstream API is flaky or often unavailable? Synchronous calls to unreliable services slow down your systems and can lead to failed requests, unhappy users, or data loss.

#### The Solution: Asynchronous Processing with BackgroundService

Instead of invoking the unstable API synchronously, store the intended call for later execution. Then, use a BackgroundService (from .NET) to continuously poll for pending jobs and process them as soon as possible, using exponential backoff to smartly space out retries during outages.

**Key Steps:**

- API 1: Receives high-frequency requests from UI, but only stores the intent (payload), returning immediately.
- API 2: Flaky backend system, called by the background job runner.
- BackgroundService: Pulls pending jobs, attempts to call API 2.
- Exponential Backoff: Retries failed jobs with increasing delays to prevent overload.

---

## Example Architecture & Implementation

### 1. Model for Pending Jobs

```csharp
public class PendingJob {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Payload { get; set; } = string.Empty;
    public int RetryCount { get; set; } = 0;
    public DateTime NextRetryTime { get; set; } = DateTime.UtcNow;
    public bool Completed { get; set; } = false;
}
```

### 2. In-Memory Job Store

```csharp
public interface IPendingJobStore {
    Task AddJobAsync(string payload);
    Task<List<PendingJob>> GetExecutableJobsAsync();
    Task MarkJobAsCompletedAsync(Guid jobId);
    Task UpdateJobAsync(PendingJob job);
}

public class InMemoryPendingJobStore : IPendingJobStore {
    private readonly List<PendingJob> _jobs = new();
    private readonly object _lock = new();

    public Task AddJobAsync(string payload) {
        lock (_lock) {
            _jobs.Add(new PendingJob { Payload = payload });
        }
        return Task.CompletedTask;
    }

    public Task<List<PendingJob>> GetExecutableJobsAsync() {
        lock (_lock) {
            return Task.FromResult(_jobs.Where(j => !j.Completed && j.NextRetryTime <= DateTime.UtcNow).ToList());
        }
    }

    public Task MarkJobAsCompletedAsync(Guid jobId) {
        lock (_lock) {
            var job = _jobs.FirstOrDefault(j => j.Id == jobId);
            if (job != null) job.Completed = true;
        }
        return Task.CompletedTask;
    }

    public Task UpdateJobAsync(PendingJob job) => Task.CompletedTask;
}
```

### 3. BackgroundService with Exponential Backoff

```csharp
public class Api2RetryService : BackgroundService {
    private readonly IHttpClientFactory _clientFactory;
    private readonly IPendingJobStore _store;
    private readonly ILogger<Api2RetryService> _logger;

    public Api2RetryService(IHttpClientFactory clientFactory, IPendingJobStore store, ILogger<Api2RetryService> logger) {
        _clientFactory = clientFactory;
        _store = store;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        _logger.LogInformation("Background retry service started.");
        while (!stoppingToken.IsCancellationRequested) {
            var jobs = await _store.GetExecutableJobsAsync();
            foreach (var job in jobs) {
                var client = _clientFactory.CreateClient("api2");
                try {
                    var response = await client.PostAsync("/simulate", new StringContent(job.Payload, Encoding.UTF8, "application/json"), stoppingToken);
                    if (response.IsSuccessStatusCode) {
                        _logger.LogInformation("Job {JobId} processed successfully.", job.Id);
                        await _store.MarkJobAsCompletedAsync(job.Id);
                    } else {
                        await HandleFailure(job);
                    }
                } catch (Exception ex) {
                    _logger.LogError(ex, "Error calling API 2.");
                    await HandleFailure(job);
                }
            }
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private async Task HandleFailure(PendingJob job) {
        job.RetryCount++;
        var delay = CalculateBackoff(job.RetryCount);
        job.NextRetryTime = DateTime.UtcNow.Add(delay);
        await _store.UpdateJobAsync(job);
        _logger.LogWarning("Retrying job {JobId} in {Delay}. RetryCount={RetryCount}", job.Id, delay, job.RetryCount);
    }

    private TimeSpan CalculateBackoff(int retryCount) {
        var seconds = Math.Pow(2, retryCount);
        var maxSeconds = TimeSpan.FromMinutes(5).TotalSeconds;
        return TimeSpan.FromSeconds(Math.Min(seconds, maxSeconds));
    }
}
```

### 4. API 1 (Public Endpoint)

```csharp
[ApiController]
[Route("api1")]
public class Api1Controller : ControllerBase {
    private readonly IPendingJobStore _store;
    private readonly ILogger<Api1Controller> _logger;

    public Api1Controller(IPendingJobStore store, ILogger<Api1Controller> logger) {
        _store = store;
        _logger = logger;
    }

    [HttpPost("process")]
    public async Task<IActionResult> Process([FromBody] object data) {
        var payload = JsonSerializer.Serialize(data);
        await _store.AddJobAsync(payload);
        _logger.LogInformation("Stored job for background processing.");
        return Ok("Request received. Will process when API 2 recovers.");
    }
}
```

### 5. API 2 (Simulated Downtime)

```csharp
[ApiController]
[Route("api2")]
public class Api2Controller : ControllerBase {
    private static bool shouldFail = true;

    [HttpPost("simulate")]
    public IActionResult Simulate([FromBody] object payload) {
        if (shouldFail) return StatusCode(503, "API 2 is down");
        return Ok("API 2 processed payload");
    }

    [HttpPost("toggle")]
    public IActionResult Toggle() {
        shouldFail = !shouldFail;
        return Ok($"API 2 failure mode = {shouldFail}");
    }
}
```

### 6. Program Setup

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSingleton<IPendingJobStore, InMemoryPendingJobStore>();
builder.Services.AddHttpClient("api2", c => { c.BaseAddress = new Uri("http://localhost:5000/api2"); });
builder.Services.AddHostedService<Api2RetryService>();
var app = builder.Build();
app.MapControllers();
app.Run();
```

---

## Testing and Use Cases

- Start API 2 in failure mode; all attempts will fail, invoking exponential backoff on retries.
- POST a request to API 1 (e.g., `{ "name": "hello" }`). Watch as logs display increasing delays between retries.
- Toggle API 2 back online; the next retry will succeed.

**Applications:**

- Payment systems
- ERP or CRM integrations
- Third-party APIs with outages
- Internal microservice resilience

**Reference:**

- [Background tasks with hosted services in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-10.0)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/background-tasks-in-net/ba-p/4472341)
