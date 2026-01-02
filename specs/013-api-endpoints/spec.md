# API Endpoints Specification

> **Feature**: REST API endpoints for sections, content, and RSS feeds

## Overview

The API provides RESTful endpoints for accessing sections, content items, and RSS feeds. Built with ASP.NET Core Minimal APIs with OpenAPI documentation. Supports multi-location content access where the same content can be retrieved from different section contexts.

## Requirements

### Functional Requirements

**FR-1**: The system MUST provide endpoints for listing all sections  
**FR-2**: The system MUST provide endpoints for getting section details  
**FR-3**: The system MUST provide endpoints for listing content by section  
**FR-4**: The system MUST provide endpoints for getting specific content items  
**FR-5**: The system MUST provide endpoints for RSS feed generation  
**FR-6**: The system MUST validate section/collection/item relationships  
**FR-7**: The system MUST return 404 for invalid combinations  
**FR-8**: The system MUST support query parameters for filtering  
**FR-9**: The system MUST include OpenAPI/Swagger documentation  
**FR-10**: The system MUST use proper HTTP status codes  

### Non-Functional Requirements

**NFR-1**: Endpoints MUST respond in < 50ms (p95, cached)  
**NFR-2**: API MUST support CORS for Blazor frontend  
**NFR-3**: API MUST include proper Content-Type headers  
**NFR-4**: API MUST support cancellation tokens  
**NFR-5**: API MUST log all requests  
**NFR-6**: API MUST use output caching where appropriate  

## Endpoint Definitions

### Section Endpoints

**GET /api/sections** - List all sections
```http
GET /api/sections HTTP/1.1
Accept: application/json

Response 200 OK:
[
  {
    "id": "github-copilot",
    "title": "GitHub Copilot",
    "description": "AI pair programmer for developers",
    "url": "github-copilot",
    "category": "github-copilot",
    "image": "/assets/section-backgrounds/github-copilot.jpg",
    "collections": [...]
  }
]
```

**GET /api/sections/{url}** - Get section by URL
```http
GET /api/sections/github-copilot HTTP/1.1

Response 200 OK:
{
  "id": "github-copilot",
  "title": "GitHub Copilot",
  ...
}

Response 404 Not Found (if section doesn't exist)
```

### Content Endpoints

**GET /api/content/section/{sectionUrl}** - List content for section
```http
GET /api/content/section/ai?collection=videos&limit=20 HTTP/1.1

Query Parameters:
- collection (optional): Filter to specific collection (news, blogs, videos)
- limit (optional): Limit number of results

Response 200 OK:
[
  {
    "id": "vs-code-107",
    "title": "VS Code 107",
    "url": "/ai/videos/vs-code-107.html",
    "canonicalUrl": "/ai/videos/vs-code-107.html",
    "date": "2025-01-15",
    ...
  }
]
```

**GET /api/content/{sectionUrl}/{collection}/{itemId}** - Get specific item
```http
GET /api/content/ai/videos/vs-code-107 HTTP/1.1

Response 200 OK:
{
  "id": "vs-code-107",
  "title": "VS Code 107",
  "content": "<p>Rendered HTML content...</p>",
  "excerpt": "<p>Excerpt HTML...</p>",
  ...
}

Response 404 Not Found:
- If item doesn't exist
- If item doesn't have the section's category
- If collection doesn't match
```

**GET /api/content/roundups/latest** - Get latest roundups
```http
GET /api/content/roundups/latest?count=4 HTTP/1.1

Response 200 OK:
[
  {
    "id": "2025-01-10-weekly-roundup",
    "title": "Weekly Roundup - January 10, 2025",
    ...
  }
]
```

### RSS Endpoints

**GET /api/rss/{sectionUrl}** - Section RSS feed
```http
GET /api/rss/ai HTTP/1.1
Accept: application/rss+xml

Response 200 OK:
Content-Type: application/rss+xml

<?xml version="1.0" encoding="UTF-8"?>
<rss version="2.0">...</rss>
```

**GET /api/rss** - Everything RSS feed
```http
GET /api/rss HTTP/1.1

Response 200 OK:
Content-Type: application/rss+xml
```

## Implementation

```csharp
// TechHub.Api/Endpoints/SectionEndpoints.cs
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
            .CacheOutput();
        
        group.MapGet("/{url}", GetSectionByUrl)
            .WithName("GetSectionByUrl")
            .WithSummary("Get section by URL slug")
            .CacheOutput();
    }
    
    private static async Task<IResult> GetAllSections(
        ISectionRepository repository,
        CancellationToken ct)
    {
        var sections = await repository.GetAllSectionsAsync(ct);
        return Results.Ok(sections);
    }
    
    private static async Task<IResult> GetSectionByUrl(
        string url,
        ISectionRepository repository,
        CancellationToken ct)
    {
        var section = await repository.GetSectionByUrlAsync(url, ct);
        return section is not null
            ? Results.Ok(section)
            : Results.NotFound();
    }
}

// TechHub.Api/Endpoints/ContentEndpoints.cs
public static class ContentEndpoints
{
    public static void MapContentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/content")
            .WithTags("Content")
            .WithOpenApi();
        
        group.MapGet("/section/{sectionUrl}", GetContentForSection)
            .WithName("GetContentForSection")
            .WithSummary("Get content items for a section");
        
        group.MapGet("/{sectionUrl}/{collection}/{itemId}", GetContentItem)
            .WithName("GetContentItem")
            .WithSummary("Get specific content item in section context");
        
        group.MapGet("/roundups/latest", GetLatestRoundups)
            .WithName("GetLatestRoundups")
            .WithSummary("Get latest roundup items");
    }
    
    private static async Task<IResult> GetContentForSection(
        string sectionUrl,
        [FromQuery] string? collection,
        [FromQuery] int? limit,
        ISectionRepository sectionRepo,
        IContentRepository contentRepo,
        CancellationToken ct)
    {
        var section = await sectionRepo.GetSectionByUrlAsync(sectionUrl, ct);
        if (section is null)
        {
            return Results.NotFound(new { error = "Section not found" });
        }
        
        var items = collection is not null
            ? await contentRepo.GetItemsByCollectionInSectionAsync(
                collection, section.Category, ct)
            : await contentRepo.GetItemsByCategoryAsync(section.Category, ct);
        
        if (limit.HasValue)
        {
            items = items.Take(limit.Value).ToList();
        }
        
        var dtos = items.Select(i => i.ToDto(sectionUrl)).ToList();
        return Results.Ok(dtos);
    }
    
    private static async Task<IResult> GetContentItem(
        string sectionUrl,
        string collection,
        string itemId,
        ISectionRepository sectionRepo,
        IContentRepository contentRepo,
        CancellationToken ct)
    {
        var section = await sectionRepo.GetSectionByUrlAsync(sectionUrl, ct);
        if (section is null)
        {
            return Results.NotFound(new { error = "Section not found" });
        }
        
        var item = await contentRepo.GetItemByIdAsync(itemId, section.Category, ct);
        if (item is null || !item.Collection.Equals(collection, StringComparison.OrdinalIgnoreCase))
        {
            return Results.NotFound(new { error = "Content not found" });
        }
        
        var dto = item.ToDetailDto(sectionUrl);
        return Results.Ok(dto);
    }
    
    private static async Task<IResult> GetLatestRoundups(
        [FromQuery] int count,
        IContentRepository repository,
        CancellationToken ct)
    {
        var items = await repository.GetItemsByCollectionAsync("roundups", ct);
        var latest = items.Take(count);
        var dtos = latest.Select(i => i.ToDto("roundups")).ToList();
        return Results.Ok(dtos);
    }
}

// TechHub.Api/Endpoints/RssEndpoints.cs
public static class RssEndpoints
{
    public static void MapRssEndpoints(this WebApplication app)
    {
        app.MapGet("/api/rss/{sectionUrl}", GenerateSectionFeed)
            .WithTags("RSS")
            .WithName("GenerateSectionFeed")
            .CacheOutput(policy => policy.Expire(TimeSpan.FromMinutes(15)));
        
        app.MapGet("/api/rss", GenerateEverythingFeed)
            .WithTags("RSS")
            .WithName("GenerateEverythingFeed")
            .CacheOutput(policy => policy.Expire(TimeSpan.FromMinutes(15)));
    }
    
    private static async Task<IResult> GenerateSectionFeed(
        string sectionUrl,
        IRssGenerator rssGenerator,
        CancellationToken ct)
    {
        var rss = await rssGenerator.GenerateSectionFeedAsync(sectionUrl, ct);
        return Results.Content(rss, "application/rss+xml");
    }
    
    private static async Task<IResult> GenerateEverythingFeed(
        IRssGenerator rssGenerator,
        CancellationToken ct)
    {
        var rss = await rssGenerator.GenerateAllFeedAsync(ct);
        return Results.Content(rss, "application/rss+xml");
    }
}
```

## Program.cs Configuration

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Output caching
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder => builder.Expire(TimeSpan.FromMinutes(10)));
});

// CORS for Blazor frontend
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(
            builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? [])
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

app.UseCors();
app.UseOutputCache();

// Map endpoints
app.MapSectionEndpoints();
app.MapContentEndpoints();
app.MapRssEndpoints();

// OpenAPI in development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Run();
```

## Testing

```csharp
public class SectionEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    
    public SectionEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetAllSections_Returns200_WithSections()
    {
        var response = await _client.GetAsync("/api/sections");
        
        response.EnsureSuccessStatusCode();
        var sections = await response.Content.ReadFromJsonAsync<List<SectionDto>>();
        Assert.NotNull(sections);
        Assert.NotEmpty(sections);
    }
    
    [Theory]
    [InlineData("ai")]
    [InlineData("github-copilot")]
    public async Task GetSectionByUrl_ValidUrl_ReturnsSection(string url)
    {
        var response = await _client.GetAsync($"/api/sections/{url}");
        
        response.EnsureSuccessStatusCode();
        var section = await response.Content.ReadFromJsonAsync<SectionDto>();
        Assert.NotNull(section);
        Assert.Equal(url, section.Url);
    }
    
    [Fact]
    public async Task GetContentItem_ValidMultiLocationUrl_ReturnsContent()
    {
        // Same content accessible from multiple sections
        var response1 = await _client.GetAsync("/api/content/ai/videos/vs-code-107");
        var response2 = await _client.GetAsync("/api/content/github-copilot/videos/vs-code-107");
        
        Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
        Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
        
        var item1 = await response1.Content.ReadFromJsonAsync<ContentItemDetailDto>();
        var item2 = await response2.Content.ReadFromJsonAsync<ContentItemDetailDto>();
        
        Assert.Equal(item1.Id, item2.Id);
        Assert.Equal(item1.Title, item2.Title);
        Assert.NotEqual(item1.Url, item2.Url); // Different section context URLs
        Assert.Equal(item1.CanonicalUrl, item2.CanonicalUrl); // Same canonical
    }
}
```

## References

- `/specs/011-domain-models/spec.md` - DTOs and models
- `/specs/012-repository-pattern/spec.md` - Data access
- `/specs/021-rss-feeds/spec.md` - RSS generation

