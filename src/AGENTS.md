# Source Code Development Guide

> **AI CONTEXT**: This is a **LEAF** context file for the `src/` directory. It complements the [Root AGENTS.md](/AGENTS.md).
> **RULE**: Global rules (Timezone, Performance, AI Assistant Workflow) in Root AGENTS.md apply **IN ADDITION** to local rules. Follow **BOTH**.

## Overview

You are a .NET development specialist for the Tech Hub source code. This directory contains all production code for the .NET migration: REST API, Blazor frontend, domain models, and infrastructure services.

**CRITICAL**: Test code also has development guidance. See [tests/AGENTS.md](/tests/AGENTS.md) for comprehensive testing patterns and rules.

## Tech Stack

- **.NET 10**: Latest LTS runtime
- **C# 13**: File-scoped namespaces, nullable reference types
- **ASP.NET Core**: Minimal APIs, dependency injection
- **Blazor**: Server-Side Rendering (SSR) + WebAssembly
- **.NET Aspire**: Orchestration and telemetry

## Critical Development Rules

### Starting/Stopping the Application

**CRITICAL**: ALWAYS use the root `./run.ps1` script to start/stop/build/test the ENTIRE website:

```powershell
# Start all services (API + Web + Aspire orchestration)
./run.ps1

# The script handles:
# - Building all projects
# - Starting services in correct order
# - Health checks and readiness
# - Graceful shutdown on Ctrl+C
```

**For individual project operations only** (not full site):

```powershell
# Build single project
dotnet build src/TechHub.Api/TechHub.Api.csproj

# Test single project
dotnet test tests/TechHub.Api.Tests/TechHub.Api.Tests.csproj
```

### ✅ Always Do

- **Run `dotnet build` after changes** to check for errors
- **Fix all build warnings** (0 warnings policy enforced)
- **Follow EditorConfig rules** (auto-format code)
- **Use file-scoped namespaces** in all C# files
- **Enable nullable reference types** (already global)
- **Write tests BEFORE or DURING implementation** (TDD)
- **Run tests after code changes**: `dotnet test`
- **Maintain 80%+ code coverage** for unit tests
- **Use context7 MCP tool** for latest .NET/Blazor documentation
- **Check ALL occurrences before renaming** (use `grep_search` to find all, then update each)

### ⚠️ Ask First

- **Database schema changes** or data model modifications
- **Adding new dependencies** to project files
- **Breaking API changes** that affect existing endpoints
- **Significant refactoring** across multiple projects

### 🚫 Never Do

- **Never commit code with build warnings or errors**
- **Never skip tests after code changes**
- **Never suppress warnings without documenting rationale**
- **Never use top-level statements** (prefer explicit Program class)
- **Never assume UTC** (use `Europe/Brussels` timezone)
- **Never make parameters nullable without good reason**
- **Never duplicate production logic in tests**
- **Never add wrapper methods just for tests**
- **Never rename identifiers without checking ALL occurrences**
- **Never use `List<T>` in public APIs** (use `IReadOnlyList<T>`)
- **Never catch exceptions without logging them**

## Directory Structure

```text
src/
├── AGENTS.md                          # This file
├── TechHub.Api/                       # REST API Backend
│   ├── Program.cs                     # API entry point
│   ├── Endpoints/                     # Minimal API endpoints
│   ├── appsettings.json
│   └── TechHub.Api.csproj
├── TechHub.Web/                       # Blazor Frontend
│   ├── AGENTS.md                      # Blazor component patterns (READ THIS for Web development)
│   ├── Program.cs                     # Web entry point
│   ├── Components/                    # Blazor components
│   │   ├── Layout/
│   │   ├── Pages/
│   │   └── Shared/
│   ├── Services/                      # Frontend services (TechHubApiClient)
│   ├── wwwroot/                       # Static assets
│   │   ├── styles.css                # Global CSS with Tech Hub design system
│   │   └── images/                   # Images (/images/ convention, NOT /assets/)
│   └── TechHub.Web.csproj
├── TechHub.Core/                      # Domain Models & Interfaces
│   ├── Models/                        # Domain entities
│   ├── Interfaces/                    # Repository interfaces
│   ├── DTOs/                          # Data transfer objects
│   └── TechHub.Core.csproj
├── TechHub.Infrastructure/            # Data Access Implementation
│   ├── Repositories/                  # Repository implementations
│   ├── Services/                      # Infrastructure services
│   └── TechHub.Infrastructure.csproj
├── TechHub.ServiceDefaults/           # Shared Aspire Configuration
│   ├── Extensions.cs
│   └── TechHub.ServiceDefaults.csproj
└── TechHub.AppHost/                   # Aspire Orchestration
    ├── Program.cs
    └── TechHub.AppHost.csproj
```

## Code Quality Standards

### Code Analysis Configuration

The Tech Hub .NET project uses comprehensive code quality tooling to maintain consistent code style, catch potential issues early, and enforce best practices across the entire codebase.

**Key Configuration Files**:

- **`.editorconfig`** (root): Coding styles and formatting rules enforced by IDEs and build tools
- **`Directory.Build.props`** (root): Central configuration for all projects (analysis level, code analyzers, global settings)
- **`tests/Directory.Build.props`**: Additional suppressions for test projects

### EditorConfig Standards

**File Encodings**:

- UTF-8 for all files
- Consistent line endings (LF on Linux)

**Indentation**:

- **C# files**: 4 spaces
- **JSON/YAML/XML**: 2 spaces
- **PowerShell**: 4 spaces

**Formatting Rules**:

- **Trailing whitespace**: Automatically trimmed
- **Final newline**: Required in all files
- **Brace style**: Allman style (opening braces on new line)

**C# Conventions**:

- **File-scoped namespaces**: Required (`namespace TechHub.Api;` not `namespace TechHub.Api { }`)
- **Expression-bodied members**: Preferred where appropriate (`int Prop => value;`)
- **Pattern matching**: Preferred over type checks (`if (obj is string s)`)
- **Null-checking**: Use modern patterns (`ArgumentNullException.ThrowIfNull()`)

**Naming Conventions**:

- **Interfaces**: `IName` (PascalCase with I prefix)
- **Types/Classes**: `PascalCase`
- **Methods/Properties**: `PascalCase`
- **Private fields**: `_camelCase` (underscore prefix)
- **Parameters/Locals**: `camelCase`
- **Constants**: `PascalCase`

### Code Analysis Settings

**Analysis Level**: `latest-all` - Enables all available code analyzers

**Enforce Code Style**: Style violations reported during build (`EnforceCodeStyleInBuild=true`)

**Analysis Mode**: `All` - Maximum strictness for code analysis

**Code Analyzers**: Microsoft.CodeAnalysis.NetAnalyzers (v10.0.100)

**Global Settings**:

- **Nullable Reference Types**: Enabled globally
- **Implicit Usings**: Enabled to reduce boilerplate
- **Deterministic Builds**: Enabled for reproducible builds

### Strategic Warning Suppressions

The following warnings are intentionally suppressed because they represent deliberate design decisions or patterns appropriate for this application:

**API Design Suppressions**:

- **CA1002**: List vs Collection - Internal API uses `List<T>` for better LINQ integration and performance
- **CA1054/CA1055/CA1056**: URI parameters/properties/returns - Project uses strings for URLs for JSON serialization simplicity and relative path support
- **CA1720**: Identifier contains type name - 'Guid' in RSS feeds is the RSS standard field name, not `System.Guid`

**Error Handling Suppressions**:

- **CA1031**: Catch-all exceptions - Intentional in middleware and error handling where we need to prevent unhandled exceptions

**String/Culture Suppressions**:

- **CA1308**: ToLowerInvariant - Intentional for tag/search normalization (lowercase is conventional for web URLs)

**Code Structure Suppressions**:

- **CA1805**: Explicit initialization - Sometimes improves clarity over implicit defaults
- **CA1812**: Internal class never instantiated - DTOs are instantiated by JSON deserializer
- **CA1848**: LoggerMessage performance - Traditional logging is clearer, performance optimization not critical
- **CA1852**: Seal internal types - JSON DTOs need to remain non-sealed for proper deserialization

**ASP.NET Core Suppressions**:

- **CA2007**: ConfigureAwait - Not necessary for ASP.NET Core applications (no SynchronizationContext)

**IDE Suppressions**:

- **IDE0011**: Add braces - Enforced by EditorConfig, auto-fixes when code is formatted
- **IDE0060**: Unused parameter - Sometimes needed for interface/signature compatibility
- **IDE0211**: Top-level statements - Project prefers explicit Program class for clarity

See [Directory.Build.props](/Directory.Build.props) for complete list with detailed XML documentation.

### Code Quality Results

**Current Status** (After Configuration):

- **Build Status**: 0 warnings, 0 errors ✅
- **Tests**: 92/92 passing (100% pass rate) ✅
- **Code Analysis**: All issues either fixed or intentionally suppressed with documented rationale

**Key Learnings**:

1. **Clean builds are essential** - Incremental builds can mask warnings due to MSBuild result caching
2. **Not all warnings are actionable** - Some analyzer rules are library-focused and don't apply to applications
3. **Strategic suppressions > forced fixes** - Better to suppress with clear rationale than blindly fix warnings
4. **Document decisions** - All suppressions include XML comments explaining why they're intentional
5. **EditorConfig handles style** - Many style warnings (like IDE0011 braces) are better handled by formatting tools

### Usage in Development

**In Visual Studio / VS Code**:

- EditorConfig rules are automatically applied as you type
- Code analysis warnings appear in the Problems panel
- Quick fixes and refactorings respect the configured rules

**Command Line**:

```bash
# Build with code analysis

dotnet build

# Build treating warnings as errors (for CI)

dotnet build /p:TreatWarningsAsErrors=true

# Clean build (reveals all warnings)

dotnet clean && dotnet build
```

**Customizing Suppressions**:

To suppress additional warnings globally:

1. Edit `/Directory.Build.props`
2. Add to `<NoWarn>` property:

   ```xml
   <NoWarn>$(NoWarn);CA1234</NoWarn>
   ```

To suppress warnings for specific projects:

1. Add to the project's `.csproj` file:

   ```xml
   <PropertyGroup>
     <NoWarn>$(NoWarn);CA1234</NoWarn>
   </PropertyGroup>
   ```

## Patterns & Best Practices

### Minimal API Endpoints

Use static methods for endpoint handlers following clean architecture:

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
            .WithSummary("Get all sections");
    }
    
    private static async Task<IResult> GetAllSections(
        ISectionRepository repository,
        CancellationToken ct)
    {
        var sections = await repository.GetAllSectionsAsync(ct);
        return Results.Ok(sections);
    }
}
```

### Domain Models with Records

Use records for immutable domain models:

```csharp
namespace TechHub.Core.Models;

/// <summary>
/// Represents a content item (news, blog, video, etc.)
/// </summary>
public class ContentItem
{
    /// <summary>
    /// URL-friendly slug derived from filename
    /// </summary>
    public required string Slug { get; init; }
    
    public required string Title { get; init; }
    public required long DateEpoch { get; init; }
    public required string Collection { get; init; }
    public required IReadOnlyList<string> Categories { get; init; }
    public required IReadOnlyList<string> Tags { get; init; }
    
    // ... other properties
}
```

### Repository Pattern

Configuration-based repository (sections loaded from appsettings.json):

```csharp
namespace TechHub.Infrastructure.Repositories;

public sealed class ConfigurationBasedSectionRepository : ISectionRepository
{
    private readonly IReadOnlyList<Section> _sections;
    
    public ConfigurationBasedSectionRepository(IOptions<AppSettings> settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        _sections = settings.Value.Content.Sections
            .Select(kvp => ConvertToSection(kvp.Key, kvp.Value))
            .ToList()
            .AsReadOnly();
    }
    
    public Task<IReadOnlyList<Section>> InitializeAsync(CancellationToken ct = default)
    {
        return Task.FromResult(_sections);
    }
    
    public Task<IReadOnlyList<Section>> GetAllAsync(CancellationToken ct = default)
    {
        return Task.FromResult(_sections);
    }
    
    private static Section ConvertToSection(string key, SectionConfig config)
    {
        return new Section
        {
            Id = key,
            Title = config.Title,
            Description = config.Description,
            Url = config.Url,
            Category = config.Category,
            BackgroundImage = config.Image,
            Collections = config.Collections.Select(c => new Collection
            {
                Title = c.Title,
                Collection = c.Collection,
                Url = c.Url,
                Description = c.Description,
                IsCustom = c.IsCustom
            }).ToList()
        };
    }
}
```

**CRITICAL**: All `IContentRepository` methods **MUST** return content sorted by `DateEpoch` in **descending order** (newest first). This sorting is applied at the repository layer before caching.

### Dependency Injection Service Lifetimes

**Singleton** - Service has no state or state is shared across all requests:

- `ISectionRepository` (ConfigurationBasedSectionRepository - loads from appsettings.json)
- `IContentRepository` (FileBasedContentRepository with caching)
- `IMarkdownService` (stateless)
- `IMemoryCache`, `TimeProvider` (built-in)

**Scoped** - Service lifetime matches HTTP request:

- `IRssGenerator` (generates per-request)
- `IStructuredDataService` (generates per-request)
- `ITechHubApiClient` (typed HttpClient)

**Transient** - Lightweight, stateless, new instance each time:

- Rarely needed (most services fit Singleton or Scoped)

**Options Pattern for Configuration**:

```csharp
// Configuration class
public class ContentOptions
{
    public required string SectionsJsonPath { get; init; }
    public required string CollectionsRootPath { get; init; }
    public string Timezone { get; init; } = "Europe/Brussels";
}

// Registration in Program.cs
builder.Services.Configure<ContentOptions>(
    builder.Configuration.GetSection("Content"));

// Injection in service
public class FileSectionRepository : ISectionRepository
{
    private readonly ContentOptions _options;
    
    public FileSectionRepository(IOptions<ContentOptions> options)
    {
        _options = options.Value;
    }
}
```

**Typed HttpClient Pattern**:

```csharp
// Registration with resilience
builder.Services.AddHttpClient<ITechHubApiClient, TechHubApiClient>(client =>
{
    client.BaseAddress = new Uri("https+http://api"); // Aspire service discovery
})
.AddStandardResilienceHandler(); // Retry + Circuit Breaker
```

## Development Commands

**Building**:

```bash
# Debug build (default)

dotnet build

# Release build

dotnet build -c Release

# Restore packages

dotnet restore

# Clean build artifacts

dotnet clean
```

**Running**:

```bash
# Start with Aspire orchestration

dotnet run --project src/TechHub.AppHost

# Aspire dashboard: https://localhost:15888

# API: https://localhost:5001

# Web: https://localhost:5173
```

**Testing**:

```bash
# Run all tests

dotnet test

# Run specific project

dotnet test tests/TechHub.Api.Tests

# With coverage

dotnet test --collect:"XPlat Code Coverage"
```

See [tests/AGENTS.md](/tests/AGENTS.md) for comprehensive testing guidance.

## Project-Specific Patterns

### TechHub.Core (Domain Models)

**Purpose**: Domain models, DTOs, and interfaces (no dependencies)

**Patterns**:

- Use `record` for immutable data
- Use `required` for mandatory properties
- Use `init` accessors for immutability
- Document public APIs with XML comments
- No framework dependencies (pure .NET)

**Example**:

```csharp
/// <summary>
/// Represents a section (thematic grouping of content)
/// </summary>
public class Section
{
    /// <summary>
    /// URL-friendly name (lowercase with hyphens, e.g., "ai", "github-copilot")
    /// </summary>
    public required string Name { get; init; }
    
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Url { get; init; }
    public required string Category { get; init; }
    public required string BackgroundImage { get; init; }
    public required IReadOnlyList<CollectionReference> Collections { get; init; }
    
    /// <summary>
    /// Validates that all required properties are correctly formatted
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentException("Section name cannot be empty", nameof(Name));
        if (!Name.All(c => char.IsLower(c) || c == '-'))
            throw new ArgumentException("Section name must be lowercase with hyphens only", nameof(Name));
        if (!Url.StartsWith('/'))
            throw new ArgumentException("Section URL must start with '/'", nameof(Url));
        if (Collections.Count == 0)
            throw new ArgumentException("Section must have at least one collection", nameof(Collections));
    }
}
```

### TechHub.Infrastructure (Data Access)

**Purpose**: Repository implementations, services, file I/O

**Patterns**:

- Implement interfaces from TechHub.Core
- Use `IOptions<T>` for configuration
- Use `IMemoryCache` for caching
- Use `ILogger<T>` for logging
- Handle errors gracefully with logging

**Example**:

```csharp
public class FileContentRepository : IContentRepository
{
    private readonly ContentOptions _options;
    private readonly IMemoryCache _cache;
    private readonly ILogger<FileContentRepository> _logger;
    
    public FileContentRepository(
        IOptions<ContentOptions> options,
        IMemoryCache cache,
        ILogger<FileContentRepository> logger)
    {
        _options = options.Value;
        _cache = cache;
        _logger = logger;
    }
    
    // Implementation with caching, error handling, logging
}
```

### TechHub.Api (REST API)

**Purpose**: HTTP endpoints, minimal APIs, Swagger documentation

**Patterns**:

- Use static endpoint mapper methods
- Use endpoint groups for organization
- Add OpenAPI documentation (`.WithOpenApi()`)
- Use descriptive endpoint names (`.WithName()`)
- Use tag grouping (`.WithTags()`)
- Return `IResult` for consistent responses
- Use dependency injection for repositories

**Example**:

```csharp
public static class SectionEndpoints
{
    public static void MapSectionEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/sections")
            .WithTags("Sections")
            .WithOpenApi();
        
        group.MapGet("/", GetAllSections)
            .WithName("GetAllSections")
            .WithSummary("Get all sections");
    }
}
```

### TechHub.Web (Blazor Frontend)

**Purpose**: Blazor components, pages, client-side interactions

**CRITICAL**: Read [TechHub.Web/AGENTS.md](TechHub.Web/AGENTS.md) for complete Blazor component patterns, design system colors, and styling conventions.

**Key Patterns**:

- **Design System**: Use Tech Hub colors from `jekyll/_sass/_colors.scss` (primary: #1f6feb, purple: #bd93f9, navy: #1a1a2e)
- **Image Paths**: Use `/images/` convention (NOT `/assets/`) - e.g., `/images/section-backgrounds/ai.jpg`
- **TechHubApiClient**: Use typed HTTP client for all API calls
- **Server-Side Rendering**: Initial content must be SSR for SEO
- **Progressive Enhancement**: Core functionality works without JavaScript

**Example**:

```razor
@page "/sections/{id}"
@inject TechHubApiClient ApiClient
@inject ILogger<SectionPage> Logger

@if (section == null)
{
    <p>Loading...</p>
}
else if (errorMessage != null)
{
    <div class="error">@errorMessage</div>
}
else
{
    <SectionCard Section="@section" />
}

@code {
    [Parameter]
    public string Id { get; set; } = string.Empty;
    
    private SectionDto? section;
    private string? errorMessage;
    
    protected override async Task OnInitializedAsync()
    {
        try
        {
            section = await ApiClient.GetSectionAsync(Id);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load section {SectionId}", Id);
            errorMessage = "Unable to load section. Please try again.";
        }
    }
}
```

## Documentation Resources

**Primary Resources** (use context7 MCP tool):

When working on .NET features, ALWAYS use context7 MCP tool to fetch current documentation:

```plaintext
# .NET Runtime

mcp_context7_query-docs(libraryID: "/dotnet/docs", query: "your topic")

# ASP.NET Core

mcp_context7_query-docs(libraryID: "/dotnet/aspnetcore", query: "minimal apis")

# Blazor

mcp_context7_query-docs(libraryID: "/dotnet/aspnetcore", query: "blazor ssr")

# .NET Aspire

mcp_context7_query-docs(libraryID: "/dotnet/aspire", query: "service discovery")
```

**Tech Hub Documentation**:

- **[Root AGENTS.md](/AGENTS.md)** - Framework-agnostic principles, .NET Tech Stack, Commands, Patterns & Examples
- **[tests/AGENTS.md](/tests/AGENTS.md)** - Testing strategies and patterns
- **[Feature Specifications](/specs/)** - Complete feature requirements
- **[API Specification](/docs/api-specification.md)** - REST API documentation

## Related Documentation

- **[tests/AGENTS.md](/tests/AGENTS.md)** - Testing strategies (test code is also source code!)
- **[Root AGENTS.md](/AGENTS.md)** - Global principles, AI workflow, .NET Tech Stack, Commands, Patterns
- **[docs/AGENTS.md](/docs/AGENTS.md)** - Documentation maintenance guidelines

---

**Remember**: Code quality is not negotiable. Every commit should build with 0 warnings, pass all tests, and follow established patterns.
