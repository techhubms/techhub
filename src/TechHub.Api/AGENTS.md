# TechHub.Api Development Guide

> **AI CONTEXT**: This is a **LEAF** context file for `src/TechHub.Api/`. It complements [src/AGENTS.md](../AGENTS.md) and [Root AGENTS.md](../../AGENTS.md).
> **RULE**: All rules from parent AGENTS.md files apply. This file adds API-specific patterns.

## Overview

This project implements the REST API backend using ASP.NET Core Minimal APIs. It exposes endpoints for sections, content, filtering, RSS feeds, and structured data.

**When to read this file**: When creating or modifying API endpoints, implementing new features, or understanding the API architecture.

**Testing this code**: See [tests/TechHub.Api.Tests/AGENTS.md](../../tests/TechHub.Api.Tests/AGENTS.md) for integration testing patterns.

## Starting & Running

**ALWAYS refer to [Root AGENTS.md - Starting, Stopping and Testing the Website](../../AGENTS.md#starting-stopping-and-testing-the-website)** for complete instructions on:

- Starting the website with the `Run` function
- Running tests with `Run`
- Using Playwright MCP tools for testing
- Proper terminal management

**Quick reference for API-only operations** (when you don't need the full site):

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

**Key Pattern**: Static extension methods on `WebApplication` with endpoint groups.

**Implementation**: Separate endpoint files in `Endpoints/` directory

**Example Structure**:

```csharp
public static class SectionEndpoints
{
    public static void MapSectionEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/sections")
            .WithTags("Sections")
            .WithOpenApi();
        
        group.MapGet("/", GetAllSections).WithName("GetAllSections");
        group.MapGet("/{sectionUrl}", GetSectionByUrl).WithName("GetSectionByUrl");
    }
    
    private static async Task<IResult> GetAllSections(
        ISectionRepository repository, CancellationToken ct)
    {
        var sections = await repository.GetAllAsync(ct);
        return Results.Ok(sections);
    }
}
```

**Register in Program.cs**: `app.MapSectionEndpoints();`

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

**Use dependency injection directly in handler parameters**: DI services, loggers, and `CancellationToken` automatically resolved.

**Important Details**:

- Query parameters: Use `[FromQuery]` attribute
- Route parameters: No attribute needed
- Services: Injected automatically
- Always include `CancellationToken` parameter
- Validate inputs before processing
- Return specific error messages with context

### Response Patterns

**Use Results.* methods**:

- Success: `Results.Ok(data)`, `Results.NoContent()`, `Results.Created(location, item)`
- Errors: `Results.NotFound()`, `Results.BadRequest()`, `Results.Problem()`

### Error Handling

**ALWAYS handle exceptions and log them** with full context (exception, parameters, operation).

## Dependency Injection

**Service registration in Program.cs**:

**Important Details**:

- Use `builder.AddServiceDefaults()` for Aspire defaults
- Configure options via `builder.Services.Configure<T>()`
- Register repositories as Singleton (stateless, cached)
- Register scoped services for per-request operations
- Configure CORS for Blazor frontend
- Add OpenAPI/Swagger with `AddEndpointsApiExplorer()` and `AddOpenApi()`

See actual `Program.cs` for complete registration.

## Configuration

**appsettings.json** contains section definitions, paths, and service settings.

See actual `appsettings.json` for complete structure.

## Testing

**See [tests/TechHub.Api.Tests/AGENTS.md](../../tests/TechHub.Api.Tests/AGENTS.md)** for comprehensive API testing patterns including:

- Integration tests with WebApplicationFactory
- Endpoint testing patterns
- Mock repository setup
- Response validation

## Common Patterns

### Filtering and Querying

**Content filtering endpoints support**:

- `tags` parameter: Comma-separated list, matched case-insensitively
- `search` parameter: Searches title and excerpt (case-insensitive)
- `limit` parameter: Limits result count

See `ContentEndpoints.cs` for implementation.

### RSS Feed Generation

**RSS Endpoints** (Internal API - called by Web frontend proxies):

| Endpoint                                   | Description                         |
| ------------------------------------------ | ----------------------------------- |
| `GET /api/rss/all`                         | All content across all sections     |
| `GET /api/rss/{sectionName}`               | Content for a specific section      |
| `GET /api/rss/collection/{collectionName}` | Content for a specific collection   |

**Implementation Notes**:

- All RSS endpoints return `application/xml; charset=utf-8`
- These API endpoints are internal (not publicly accessible)
- User-facing RSS feeds served via Web frontend proxies

**Documentation**: See [docs/rss-feeds.md](../../docs/rss-feeds.md) for functional RSS feed documentation.

**Frontend Proxies**: See [src/TechHub.Web/AGENTS.md](../TechHub.Web/AGENTS.md) for user-facing RSS feed URLs.

## Related Documentation

---

**Remember**: Every endpoint should have comprehensive OpenAPI documentation, proper error handling, and logging.
