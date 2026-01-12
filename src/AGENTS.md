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

### Starting, Running, and Testing

**ALWAYS refer to [Root AGENTS.md](../AGENTS.md#starting--stopping-the-website)** for complete instructions on:

- Starting the website with `./run.ps1`
- Running all tests with `./run.ps1 -OnlyTests`
- Interactive debugging with `./run.ps1 -SkipTests`
- Using Playwright MCP tools for testing
- Proper terminal management and safety
- Building/testing individual projects

**Quick command reference** (see root AGENTS.md for full details):

```powershell
./run.ps1                 # Run tests, then keep servers running
./run.ps1 -OnlyTests      # Run all tests and exit (for verification)
./run.ps1 -SkipTests      # Skip tests, start servers (for debugging)
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
│   │   ├── css/                      # Global CSS (design system, components)
│   │   │   ├── design-tokens.css
│   │   │   ├── base.css
│   │   │   ├── layout.css
│   │   │   ├── components/
│   │   │   └── utilities.css
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

**Domain Terminology**:

- **`section`**: Refers to the entire Section object/concept (e.g., `var section = new Section()`)
- **`sectionName`**: Refers to the Name property of a section (the string identifier/slug, e.g., "ai", "github-copilot")
- **`collection`**: Refers to the entire Collection object/concept
- **`collectionName`**: Refers to the Name property of a collection (the string identifier, e.g., "news", "videos")
- **Consistency Rule**: Use suffixed names (`sectionName`, `collectionName`) for string identifiers in API parameters, route parameters, and method parameters

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

## Shared .NET Patterns

These patterns apply across all .NET projects. **See project-specific AGENTS.md files** for detailed examples:

- **[TechHub.Api/AGENTS.md](TechHub.Api/AGENTS.md)** - Minimal API endpoint patterns
- **[TechHub.Core/AGENTS.md](TechHub.Core/AGENTS.md)** - Domain model patterns  
- **[TechHub.Infrastructure/AGENTS.md](TechHub.Infrastructure/AGENTS.md)** - Repository and data access patterns
- **[TechHub.Web/AGENTS.md](TechHub.Web/AGENTS.md)** - Blazor component patterns

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

**Common DI Pitfalls**:

❌ **WRONG**: Singleton with scoped dependency (e.g., HttpContext)  
❌ **WRONG**: Transient for heavy objects (creates too many instances)  
❌ **WRONG**: Direct configuration access (`builder.Configuration["Key"]`)  

✅ **CORRECT**: Match lifetime to dependency requirements  
✅ **CORRECT**: Singleton for stateless services  
✅ **CORRECT**: Use Options pattern for configuration

### Markdown Frontmatter Mapping

**See [TechHub.Core/AGENTS.md](TechHub.Core/AGENTS.md#markdown-frontmatter-mapping)** for complete mapping documentation showing how markdown frontmatter fields map to domain model properties.

## Documentation Resources

**Primary Resources** (use context7 MCP tool):

When working on .NET features, ALWAYS use context7 MCP tool to fetch current documentation:

```plaintext
# .NET Runtime and Libraries
mcp_context7_resolve-library-id(libraryName: "dotnet")
mcp_context7_query-docs(libraryID: "/dotnet/docs", query: "your topic")

# ASP.NET Core
mcp_context7_resolve-library-id(libraryName: "aspnetcore")
mcp_context7_query-docs(libraryID: "/dotnet/aspnetcore", query: "minimal apis")

# Blazor
mcp_context7_query-docs(libraryID: "/dotnet/aspnetcore", query: "blazor server-side rendering")

# .NET Aspire
mcp_context7_resolve-library-id(libraryName: "aspire")
mcp_context7_query-docs(libraryID: "/dotnet/aspire", query: "service discovery")

# xUnit Testing
mcp_context7_resolve-library-id(libraryName: "xunit")
mcp_context7_query-docs(libraryID: "/xunit/xunit", query: "theories and data-driven tests")
```

**Tech Hub Documentation**:

- **[Root AGENTS.md](../AGENTS.md)** - AI Workflow, starting/stopping website, repository-wide principles
- **[TechHub.Api/AGENTS.md](TechHub.Api/AGENTS.md)** - API development patterns
- **[TechHub.Core/AGENTS.md](TechHub.Core/AGENTS.md)** - Domain model patterns
- **[TechHub.Infrastructure/AGENTS.md](TechHub.Infrastructure/AGENTS.md)** - Repository and data access patterns
- **[TechHub.Web/AGENTS.md](TechHub.Web/AGENTS.md)** - Blazor component patterns
- **[tests/AGENTS.md](../tests/AGENTS.md)** - Testing strategies and patterns
- **[docs/AGENTS.md](../docs/AGENTS.md)** - Documentation maintenance guidelines

## Related Documentation

- **[Root AGENTS.md](../AGENTS.md)** - AI workflow, starting/stopping website, principles
- **[tests/AGENTS.md](../tests/AGENTS.md)** - Testing strategies (test code is also source code!)
- **[docs/AGENTS.md](../docs/AGENTS.md)** - Documentation maintenance
- **Project-specific AGENTS.md files**: See each subdirectory for detailed patterns

---

**Remember**: Code quality is not negotiable. Every commit should build with 0 warnings, pass all tests, and follow established patterns.
