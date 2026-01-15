# TechHub.Api Development Guide

> **AI CONTEXT**: This is a **LEAF** context file for `src/TechHub.Api/`. It complements [src/AGENTS.md](../AGENTS.md) and [Root AGENTS.md](../../AGENTS.md).
> **RULE**: All rules from parent AGENTS.md files apply. This file adds API-specific patterns.

## Overview

This project implements the REST API backend using ASP.NET Core Minimal APIs. It exposes endpoints for sections, content, filtering, RSS feeds, and structured data.

**When to read this file**: When creating or modifying API endpoints, implementing new features, or understanding the API architecture.

**Testing this code**: See [tests/TechHub.Api.Tests/AGENTS.md](../../tests/TechHub.Api.Tests/AGENTS.md) for integration testing patterns.

## Starting & Running

**ALWAYS refer to [Root AGENTS.md](../../AGENTS.md#starting--stopping-the-website)** for complete instructions on:

- Starting the website with the `Run` function
- Running tests with `Run -OnlyTests`
- Using Playwright MCP tools for testing
- Proper terminal management

**Quick reference for API-only operations** (when you don't need the full site):

```powershell
# Build API project only
dotnet build src/TechHub.Api/TechHub.Api.csproj

# Run API project directly (no orchestration)
dotnet run --project src/TechHub.Api/TechHub.Api.csproj

# Test API endpoints
dotnet test tests/TechHub.Api.Tests/TechHub.Api.Tests.csproj
```

## Project Structure

```text
TechHub.Api/
├── Program.cs                    # Entry point, DI registration, middleware pipeline
├── Endpoints/                    # Minimal API endpoint definitions
│   ├── SectionEndpoints.cs      # Section-related endpoints
│   ├── ContentEndpoints.cs      # Content retrieval endpoints
│   ├── FilterEndpoints.cs       # Advanced filtering endpoints
│   └── RssEndpoints.cs          # RSS feed generation
├── appsettings.json             # Configuration (sections, collections, paths)
├── appsettings.Development.json # Development-specific settings
└── TechHub.Api.csproj           # Project file
```

## Minimal API Patterns

### Endpoint Organization

**Use static extension methods for endpoint mapping**:

```csharp
// Endpoints/SectionEndpoints.cs
namespace TechHub.Api.Endpoints;

public static class SectionEndpoints
{
    public static void MapSectionEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/sections")
            .WithTags("Sections")
            .WithOpenApi();
        
        group.MapGet("/", GetAllSections)
            .WithName("GetAllSections")
            .WithSummary("Get all sections")
            .WithDescription("Retrieves all configured sections with their collections");
        
        group.MapGet("/{sectionUrl}", GetSectionByUrl)
            .WithName("GetSectionByUrl")
            .WithSummary("Get a specific section by URL");
    }
    
    private static async Task<IResult> GetAllSections(
        ISectionRepository repository,
        CancellationToken ct)
    {
        var sections = await repository.GetAllAsync(ct);
        return Results.Ok(sections);
    }
    
    private static async Task<IResult> GetSectionByUrl(
        string sectionUrl,
        ISectionRepository repository,
        CancellationToken ct)
    {
        var section = await repository.GetByUrlAsync(sectionUrl, ct);
        return section is null 
            ? Results.NotFound(new { error = $"Section '{sectionUrl}' not found" })
            : Results.Ok(section);
    }
}
```

**Register endpoints in Program.cs**:

```csharp
// Program.cs
var app = builder.Build();

// Map all endpoints
app.MapSectionEndpoints();
app.MapContentEndpoints();
app.MapFilterEndpoints();
app.MapRssEndpoints();

app.Run();
```

### OpenAPI Documentation

**ALWAYS add comprehensive OpenAPI metadata**:

```csharp
group.MapGet("/{sectionUrl}/content", GetContentForSection)
    .WithName("GetContentForSection")
    .WithSummary("Get all content items for a section")
    .WithDescription("Retrieves all content items (news, videos, blogs, etc.) for the specified section, sorted by date (newest first)")
    .WithTags("Content")
    .WithOpenApi()
    .Produces<IReadOnlyList<ContentItemDto>>(StatusCodes.Status200OK)
    .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
```

**Benefits**:

- Automatic Swagger UI documentation
- Client code generation support
- API discoverability
- Clear endpoint contracts

### Endpoint Handler Patterns

**Use dependency injection directly in handler parameters**:

```csharp
private static async Task<IResult> GetFilteredContent(
    string sectionUrl,
    [FromQuery] string? tags,              // Query parameter
    [FromQuery] string? dateRange,         // Query parameter
    ISectionRepository sectionRepo,        // DI service
    IContentRepository contentRepo,        // DI service
    ILogger<ContentEndpoints> logger,      // DI logger
    CancellationToken ct)                  // Cancellation token
{
    try
    {
        // Validation
        var section = await sectionRepo.GetByUrlAsync(sectionUrl, ct);
        if (section is null)
        {
            logger.LogWarning("Section not found: {SectionUrl}", sectionUrl);
            return Results.NotFound(new { error = $"Section '{sectionUrl}' not found" });
        }
        
        // Get and filter content
        var allContent = await contentRepo.GetBySectionAsync(sectionUrl, ct);
        var filtered = ApplyFilters(allContent, tags, dateRange);
        
        logger.LogInformation("Returned {Count} content items for section {Section}", 
            filtered.Count, sectionUrl);
            
        return Results.Ok(filtered);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error retrieving content for section {SectionUrl}", sectionUrl);
        return Results.Problem("An error occurred while retrieving content");
    }
}
```

### Response Patterns

**Use Results.* methods for consistent responses**:

```csharp
// Success responses
return Results.Ok(data);                              // 200 with data
return Results.NoContent();                           // 204 no content
return Results.Created($"/api/items/{id}", item);     // 201 with location

// Error responses
return Results.NotFound(new { error = "Not found" }); // 404
return Results.BadRequest(new { error = "Invalid" }); // 400
return Results.Problem("Server error");               // 500
return Results.ValidationProblem(errors);             // 400 with validation errors
```

### Error Handling

**ALWAYS handle exceptions and log them**:

```csharp
private static async Task<IResult> GetContent(
    string id,
    IContentRepository repository,
    ILogger<ContentEndpoints> logger,
    CancellationToken ct)
{
    try
    {
        var content = await repository.GetByIdAsync(id, ct);
        return content is null
            ? Results.NotFound(new { error = $"Content '{id}' not found" })
            : Results.Ok(content);
    }
    catch (ArgumentException ex)
    {
        logger.LogWarning(ex, "Invalid content ID: {Id}", id);
        return Results.BadRequest(new { error = ex.Message });
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error retrieving content {Id}", id);
        return Results.Problem("An error occurred while retrieving content");
    }
}
```

## Dependency Injection

**Service registration in Program.cs**:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults (OpenTelemetry, health checks, resilience)
builder.AddServiceDefaults();

// Configuration binding
builder.Services.Configure<ContentOptions>(
    builder.Configuration.GetSection("Content"));

// Infrastructure services
builder.Services.AddMemoryCache();
builder.Services.AddSingleton(TimeProvider.System);

// Repositories
builder.Services.AddSingleton<ISectionRepository, ConfigurationBasedSectionRepository>();
builder.Services.AddSingleton<IContentRepository, FileBasedContentRepository>();

// Services
builder.Services.AddSingleton<IMarkdownService, MarkdownService>();
builder.Services.AddScoped<IRssService, RssService>();

// API features
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// CORS for Blazor frontend
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(builder.Configuration["AllowedOrigins"] ?? "https://localhost:5184")
              .AllowAnyMethod()
              .AllowAnyHeader());
});
```

## Configuration

**appsettings.json structure**:

```json
{
  "Content": {
    "CollectionsRootPath": "/workspaces/techhub/collections",
    "Timezone": "Europe/Brussels",
    "Sections": {
      "ai": {
        "Title": "AI",
        "Description": "Artificial Intelligence and Machine Learning",
        "Url": "/ai",
        "Image": "/images/section-backgrounds/ai.jpg",
        "Collections": [
          {
            "Title": "News",
            "Name": "news",
            "Url": "/ai/news",
            "Description": "Latest AI announcements",
            "IsCustom": false
          }
        ]
      }
    }
  }
}
```

## Testing

**See [tests/TechHub.Api.Tests/AGENTS.md](../../tests/TechHub.Api.Tests/AGENTS.md)** for comprehensive API testing patterns including:

- Integration tests with WebApplicationFactory
- Endpoint testing patterns
- Mock repository setup
- Response validation

## Common Patterns

### Filtering and Querying

```csharp
private static async Task<IResult> GetFilteredContent(
    [FromQuery] string? tags,
    [FromQuery] string? search,
    [FromQuery] int? limit,
    IContentRepository repository,
    CancellationToken ct)
{
    var allContent = await repository.GetAllAsync(ct);
    
    // Apply filters
    var filtered = allContent;
    
    if (!string.IsNullOrWhiteSpace(tags))
    {
        var tagList = tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(t => t.Trim().ToLowerInvariant())
            .ToList();
        filtered = filtered.Where(c => c.Tags.Any(t => tagList.Contains(t.ToLowerInvariant())));
    }
    
    if (!string.IsNullOrWhiteSpace(search))
    {
        var searchLower = search.ToLowerInvariant();
        filtered = filtered.Where(c => 
            c.Title.Contains(searchLower, StringComparison.OrdinalIgnoreCase) ||
            c.Excerpt.Contains(searchLower, StringComparison.OrdinalIgnoreCase));
    }
    
    // Apply limit
    var result = limit.HasValue 
        ? filtered.Take(limit.Value).ToList()
        : filtered.ToList();
    
    return Results.Ok(result);
}
```

### RSS Feed Generation

**RSS Endpoints** (Internal API - Not Publicly Accessible):

The API provides 3 RSS feed endpoints that are called by the Web frontend proxies:

```csharp
// All content across all sections
private static async Task<IResult> GetAllContentRssFeed(
    IRssService rssService,
    ISectionRepository sectionRepo,
    IContentRepository contentRepo,
    CancellationToken ct)
{
    // Get all sections and content
    var sections = await sectionRepo.GetAllAsync(ct);
    var allContent = await contentRepo.GetAllAsync(ct);
    
    // Generate feed with all content
    var rssXml = rssService.GenerateFeed(
        title: "Tech Hub - All Content",
        description: "Latest content across all Tech Hub sections",
        sections,
        allContent);
    
    return Results.Content(rssXml, "application/xml; charset=utf-8");
}

// Section-specific content
private static async Task<IResult> GetSectionRssFeed(
    string sectionName,
    ISectionRepository sectionRepo,
    IContentRepository contentRepo,
    IRssService rssService,
    CancellationToken ct)
{
    var section = await sectionRepo.GetByUrlAsync(sectionName, ct);
    if (section is null)
        return Results.NotFound(new { error = $"Section '{sectionName}' not found" });
    
    var content = await contentRepo.GetBySectionAsync(sectionName, ct);
    var rssXml = rssService.GenerateFeed(section, content);
    
    return Results.Content(rssXml, "application/xml; charset=utf-8");
}

// Collection-specific content (e.g., roundups)
private static async Task<IResult> GetCollectionRssFeed(
    string collectionName,
    IContentRepository contentRepo,
    IRssService rssService,
    CancellationToken ct)
{
    var content = await contentRepo.GetByCollectionAsync(collectionName, ct);
    
    var rssXml = rssService.GenerateFeed(
        title: $"Tech Hub - {collectionName}",
        description: $"Latest {collectionName} from Tech Hub",
        sections: null,
        content);
    
    return Results.Content(rssXml, "application/xml; charset=utf-8");
}
```

**Endpoint Mapping** (in `RssEndpoints.cs`):

```csharp
public static class RssEndpoints
{
    public static void MapRssEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/rss")
            .WithTags("RSS")
            .WithOpenApi();

        group.MapGet("/all", GetAllContentRssFeed)
            .WithName("GetAllContentRssFeed")
            .Produces<string>(200, "application/xml");

        group.MapGet("/{sectionName}", GetSectionRssFeed)
            .WithName("GetSectionRssFeed")
            .Produces<string>(200, "application/xml")
            .ProducesProblem(404);

        group.MapGet("/collection/{collectionName}", GetCollectionRssFeed)
            .WithName("GetCollectionRssFeed")
            .Produces<string>(200, "application/xml");
    }
    
    // ... handler methods above ...
}
```

**Content Type**: All RSS endpoints return `application/xml; charset=utf-8`

**Security**: These API endpoints will be secured and NOT publicly accessible. User-facing RSS feeds are served via Web frontend proxies (see [src/TechHub.Web/AGENTS.md](../TechHub.Web/AGENTS.md)).

**Frontend Proxies**: See [src/TechHub.Web/AGENTS.md](../TechHub.Web/AGENTS.md) for user-facing RSS feed URLs (`/all/feed.xml`, `/{section}/feed.xml`).

**Documentation**: See [docs/rss-feeds.md](../../docs/rss-feeds.md) for functional RSS feed documentation.

- **[src/AGENTS.md](../AGENTS.md)** - Shared .NET patterns and code quality standards
- **[src/TechHub.Core/AGENTS.md](../TechHub.Core/AGENTS.md)** - Domain models and DTOs
- **[src/TechHub.Infrastructure/AGENTS.md](../TechHub.Infrastructure/AGENTS.md)** - Repository implementations
- **[tests/TechHub.Api.Tests/AGENTS.md](../../tests/TechHub.Api.Tests/AGENTS.md)** - API testing patterns
- **[Root AGENTS.md](../../AGENTS.md)** - Complete workflow, starting/stopping website
- **[docs/api-specification.md](../../docs/api-specification.md)** - Complete API contracts

---

**Remember**: Every endpoint should have comprehensive OpenAPI documentation, proper error handling, and logging.
