# Dependency Injection Configuration

## Overview

Defines dependency injection (DI) container setup for all Tech Hub projects. This specification ensures proper service lifetimes, configuration patterns, and testability.

## Constitution Alignment

- **Clean Architecture**: Services registered by layer (domain, infrastructure, application)
- **Configuration-Driven**: Settings loaded from `appsettings.json` and environment variables
- **.NET 10 Stack**: Uses built-in `Microsoft.Extensions.DependencyInjection`

## Service Lifetimes

### Singleton

**Use When**: Service has no state, or state is shared across all requests

**TechHub.Api Services**:
- `ISectionRepository` (FileSectionRepository with caching)
- `IContentRepository` (FileContentRepository with caching)
- `IMarkdownProcessor` (MarkdownProcessor - stateless)
- `IMemoryCache` (built-in)
- `TimeProvider` (built-in .NET 9+)

**TechHub.Web Services**:
- `TimeProvider`

**Rationale**: File-based repositories load data once and cache it. Markdown processor is stateless.

---

### Scoped

**Use When**: Service lifetime should match HTTP request lifetime

**TechHub.Api Services**:
- `IRssGenerator` (RssGenerator - generates per-request)
- `IStructuredDataService` (StructuredDataService - generates per-request)

**TechHub.Web Services**:
- `ITechHubApiClient` (Typed HttpClient)

**Rationale**: RSS and structured data generation are request-specific. HttpClient should be scoped to request.

---

### Transient

**Use When**: Service is lightweight and stateless, new instance needed each time

**TechHub Services**: None currently

**Rationale**: Most services fit singleton (stateless + cached) or scoped (request-bound) patterns.

---

## TechHub.Api DI Configuration

**File**: `/dotnet/src/TechHub.Api/Program.cs`

```csharp
using TechHub.Core.Interfaces;
using TechHub.Infrastructure.Repositories;
using TechHub.Infrastructure.Services;
using TechHub.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

// === Aspire Service Defaults ===
// Adds: OpenTelemetry, Health Checks, Service Discovery, Resilience
builder.AddServiceDefaults();

// === Configuration ===
builder.Services.Configure<ContentOptions>(
    builder.Configuration.GetSection("Content"));

// === Infrastructure Services ===
builder.Services.AddMemoryCache();
builder.Services.AddSingleton(TimeProvider.System);

// === Repositories (Singleton - File-based with caching) ===
builder.Services.AddSingleton<ISectionRepository, FileSectionRepository>();
builder.Services.AddSingleton<IContentRepository, FileContentRepository>();

// === Services ===
builder.Services.AddSingleton<IMarkdownProcessor, MarkdownProcessor>();
builder.Services.AddScoped<IRssGenerator, RssGenerator>();
builder.Services.AddScoped<IStructuredDataService, StructuredDataService>();

// === API Documentation ===
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// === CORS for Blazor Frontend ===
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(builder.Configuration["AllowedOrigins"] ?? "https://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
});

// === Response Compression ===
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

// === Output Caching ===
builder.Services.AddOutputCache(options =>
{
    // RSS feeds: 15 minutes
    options.AddPolicy("rss", builder => builder
        .Expire(TimeSpan.FromMinutes(15))
        .SetVaryByQuery("*"));
    
    // API endpoints: 5 minutes
    options.AddPolicy("api", builder => builder
        .Expire(TimeSpan.FromMinutes(5))
        .SetVaryByQuery("*"));
});

var app = builder.Build();

// === Middleware Pipeline ===
app.UseResponseCompression();
app.UseOutputCache();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors();

// === Map Endpoints ===
app.MapSectionEndpoints();
app.MapContentEndpoints();
app.MapRssEndpoints();

app.Run();
```

---

## TechHub.Web DI Configuration

**File**: `/dotnet/src/TechHub.Web/Program.cs`

```csharp
using TechHub.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// === Aspire Service Defaults ===
builder.AddServiceDefaults();

// === Blazor ===
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// === Typed HttpClient for API ===
builder.Services.AddHttpClient<ITechHubApiClient, TechHubApiClient>(client =>
{
    // Service discovery via Aspire
    client.BaseAddress = new Uri("https+http://api");
})
.AddStandardResilienceHandler(); // Retry + Circuit Breaker

// === Time Provider ===
builder.Services.AddSingleton(TimeProvider.System);

var app = builder.Build();

// === Middleware Pipeline ===
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// === Razor Components ===
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();

app.Run();
```

---

## Configuration Options Pattern

**TechHub.Infrastructure/Configuration/ContentOptions.cs**:

```csharp
namespace TechHub.Infrastructure.Configuration;

/// <summary>
/// Configuration for content file paths
/// </summary>
public class ContentOptions
{
    public const string SectionName = "Content";
    
    /// <summary>
    /// Path to sections.json file (single source of truth)
    /// </summary>
    public required string SectionsJsonPath { get; init; }
    
    /// <summary>
    /// Root directory for content collections (_news, _videos, etc.)
    /// </summary>
    public required string CollectionsRootPath { get; init; }
    
    /// <summary>
    /// Timezone for date operations (default: Europe/Brussels)
    /// </summary>
    public string Timezone { get; init; } = "Europe/Brussels";
    
    /// <summary>
    /// Enable memory caching for repository data
    /// </summary>
    public bool EnableCaching { get; init; } = true;
    
    /// <summary>
    /// Cache expiration in minutes
    /// </summary>
    public int CacheExpirationMinutes { get; init; } = 60;
}
```

**TechHub.Api/appsettings.json**:

```json
{
  "Content": {
    "SectionsJsonPath": "/workspaces/techhub/_data/sections.json",
    "CollectionsRootPath": "/workspaces/techhub/collections",
    "Timezone": "Europe/Brussels",
    "EnableCaching": true,
    "CacheExpirationMinutes": 60
  },
  "AllowedOrigins": "https://localhost:5173",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**Usage in Service**:

```csharp
using Microsoft.Extensions.Options;

namespace TechHub.Infrastructure.Repositories;

public class FileSectionRepository : ISectionRepository
{
    private readonly ContentOptions _options;
    
    public FileSectionRepository(
        IOptions<ContentOptions> options,
        IMemoryCache cache,
        ILogger<FileSectionRepository> logger)
    {
        _options = options.Value;
        // ...
    }
}
```

---

## Typed HttpClient Pattern

**TechHub.Core/Interfaces/ITechHubApiClient.cs**:

```csharp
namespace TechHub.Core.Interfaces;

/// <summary>
/// Client for communicating with TechHub.Api
/// </summary>
public interface ITechHubApiClient
{
    Task<IReadOnlyList<SectionDto>> GetAllSectionsAsync(CancellationToken ct = default);
    Task<SectionDto?> GetSectionAsync(string url, CancellationToken ct = default);
    Task<IReadOnlyList<ContentItemDto>> GetContentAsync(
        string sectionUrl, 
        string? collectionUrl = null, 
        CancellationToken ct = default);
    Task<ContentItemDto?> GetContentItemAsync(
        string sectionUrl, 
        string collectionUrl, 
        string itemId, 
        CancellationToken ct = default);
}
```

**TechHub.Web/Services/TechHubApiClient.cs**:

```csharp
namespace TechHub.Web.Services;

public class TechHubApiClient : ITechHubApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TechHubApiClient> _logger;
    
    public TechHubApiClient(
        HttpClient httpClient,
        ILogger<TechHubApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    
    public async Task<IReadOnlyList<SectionDto>> GetAllSectionsAsync(
        CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync("/api/sections", ct);
        response.EnsureSuccessStatusCode();
        
        var sections = await response.Content
            .ReadFromJsonAsync<List<SectionDto>>(ct);
        
        return sections ?? [];
    }
    
    public async Task<SectionDto?> GetSectionAsync(
        string url, 
        CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync($"/api/sections/{url}", ct);
        
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
        
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<SectionDto>(ct);
    }
    
    public async Task<IReadOnlyList<ContentItemDto>> GetContentAsync(
        string sectionUrl, 
        string? collectionUrl = null, 
        CancellationToken ct = default)
    {
        var url = collectionUrl is not null
            ? $"/api/content/{sectionUrl}/{collectionUrl}"
            : $"/api/content/{sectionUrl}";
        
        var response = await _httpClient.GetAsync(url, ct);
        response.EnsureSuccessStatusCode();
        
        var items = await response.Content
            .ReadFromJsonAsync<List<ContentItemDto>>(ct);
        
        return items ?? [];
    }
    
    public async Task<ContentItemDto?> GetContentItemAsync(
        string sectionUrl, 
        string collectionUrl, 
        string itemId, 
        CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync(
            $"/api/content/{sectionUrl}/{collectionUrl}/{itemId}", ct);
        
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
        
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<ContentItemDto>(ct);
    }
}
```

**Registration** (from above):

```csharp
builder.Services.AddHttpClient<ITechHubApiClient, TechHubApiClient>(client =>
{
    client.BaseAddress = new Uri("https+http://api"); // Aspire service discovery
})
.AddStandardResilienceHandler(); // Retry + Circuit Breaker from Microsoft.Extensions.Http.Resilience
```

---

## Testing with DI

**Unit Test Example** (TechHub.Core.Tests):

```csharp
namespace TechHub.Core.Tests.Models;

public class DateUtilsTests
{
    private readonly TimeProvider _timeProvider;
    
    public DateUtilsTests()
    {
        // Use FakeTimeProvider for deterministic testing
        _timeProvider = new FakeTimeProvider(
            new DateTimeOffset(2025, 1, 1, 12, 0, 0, TimeSpan.Zero));
    }
    
    [Fact]
    public void ToEpoch_ConvertsDateCorrectly()
    {
        // Arrange
        var date = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        // Act
        var epoch = DateUtils.ToEpoch(date, _timeProvider);
        
        // Assert
        Assert.Equal(1735689600, epoch);
    }
}
```

**Integration Test Example** (TechHub.Api.Tests):

```csharp
namespace TechHub.Api.Tests.Endpoints;

public class SectionEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    
    public SectionEndpointsTests(WebApplicationFactory<Program> factory)
    {
        // WebApplicationFactory provides DI container
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Override services for testing
                services.RemoveAll<ISectionRepository>();
                services.AddSingleton<ISectionRepository, MockSectionRepository>();
            });
        }).CreateClient();
    }
    
    [Fact]
    public async Task GetAllSections_Returns200()
    {
        // Act
        var response = await _client.GetAsync("/api/sections");
        
        // Assert
        response.EnsureSuccessStatusCode();
    }
}
```

**Component Test Example** (TechHub.Web.Tests):

```csharp
namespace TechHub.Web.Tests.Components;

public class ItemCardTests : TestContext
{
    [Fact]
    public void ItemCard_RendersTitle()
    {
        // Arrange
        var mockApiClient = new Mock<ITechHubApiClient>();
        Services.AddSingleton(mockApiClient.Object);
        
        var item = new ContentItemDto { Title = "Test", /* ... */ };
        
        // Act
        var cut = RenderComponent<ItemCard>(parameters =>
            parameters.Add(p => p.Item, item));
        
        // Assert
        cut.Find("h2").TextContent.Should().Be("Test");
    }
}
```

---

## Environment-Specific Configuration

**appsettings.Development.json**:

```json
{
  "Content": {
    "EnableCaching": false
  },
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information"
    }
  }
}
```

**appsettings.Production.json**:

```json
{
  "AllowedOrigins": "https://tech.hub.ms",
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Error"
    }
  }
}
```

**Environment Variables** (Azure Container Apps):

```bash
ASPNETCORE_ENVIRONMENT=Production
Content__SectionsJsonPath=/app/data/sections.json
Content__CollectionsRootPath=/app/data/collections
AllowedOrigins=https://tech.hub.ms
```

---

## Aspire Service Discovery

**TechHub.AppHost/Program.cs**:

```csharp
var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.TechHub_Api>("api")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");

var web = builder.AddProject<Projects.TechHub_Web>("web")
    .WithReference(api) // Injects service discovery for "api"
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");

builder.Build().Run();
```

**How It Works**:
- `WithReference(api)` injects `https+http://api` as base URL
- Blazor frontend resolves `api` to actual endpoint automatically
- Works locally (https://localhost:5001) and in Azure (service-to-service communication)

---

## Service Registration Extensions

**TechHub.Api/Extensions/ServiceCollectionExtensions.cs**:

```csharp
namespace TechHub.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTechHubRepositories(
        this IServiceCollection services)
    {
        services.AddSingleton<ISectionRepository, FileSectionRepository>();
        services.AddSingleton<IContentRepository, FileContentRepository>();
        return services;
    }
    
    public static IServiceCollection AddTechHubServices(
        this IServiceCollection services)
    {
        services.AddSingleton<IMarkdownProcessor, MarkdownProcessor>();
        services.AddScoped<IRssGenerator, RssGenerator>();
        services.AddScoped<IStructuredDataService, StructuredDataService>();
        return services;
    }
    
    public static IServiceCollection AddTechHubCaching(
        this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddOutputCache(options =>
        {
            options.AddPolicy("rss", builder => builder
                .Expire(TimeSpan.FromMinutes(15))
                .SetVaryByQuery("*"));
            
            options.AddPolicy("api", builder => builder
                .Expire(TimeSpan.FromMinutes(5))
                .SetVaryByQuery("*"));
        });
        return services;
    }
}
```

**Usage**:

```csharp
// Simplified Program.cs
builder.AddServiceDefaults();
builder.Services.Configure<ContentOptions>(builder.Configuration.GetSection("Content"));
builder.Services.AddTechHubRepositories();
builder.Services.AddTechHubServices();
builder.Services.AddTechHubCaching();
```

---

## Common Pitfalls

### ❌ WRONG: Singleton with Scoped Dependency

```csharp
// BAD: Singleton depends on scoped IHttpContextAccessor
builder.Services.AddSingleton<IMyService, MyService>(); // Depends on HttpContext
```

### ✅ CORRECT: Scoped Service

```csharp
builder.Services.AddScoped<IMyService, MyService>();
```

---

### ❌ WRONG: Transient for Heavy Object

```csharp
// BAD: Creates new MarkdownProcessor per injection
builder.Services.AddTransient<IMarkdownProcessor, MarkdownProcessor>();
```

### ✅ CORRECT: Singleton for Stateless Service

```csharp
builder.Services.AddSingleton<IMarkdownProcessor, MarkdownProcessor>();
```

---

### ❌ WRONG: Direct Configuration Access

```csharp
var sectionsPath = builder.Configuration["Content:SectionsJsonPath"];
```

### ✅ CORRECT: Options Pattern

```csharp
builder.Services.Configure<ContentOptions>(
    builder.Configuration.GetSection("Content"));

// Inject IOptions<ContentOptions>
```

---

## References

- [.NET Dependency Injection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)
- [Options Pattern](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options)
- [Typed HttpClient](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests)
- [.NET Aspire Service Discovery](https://learn.microsoft.com/en-us/dotnet/aspire/service-discovery/overview)
- `/specs/infrastructure/solution-structure.md` - Project organization
- `/specs/features/repository-pattern.md` - Repository lifetime justification
