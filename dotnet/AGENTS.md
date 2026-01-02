# Tech Hub .NET Development Guide

> **AI CONTEXT**: This is the **ROOT** context file for the .NET solution.
> For framework-agnostic principles, see the parent [/AGENTS.md](../AGENTS.md).

## Index

- [AI Assistant Workflow](#ai-assistant-workflow)
- [Solution Structure](#solution-structure)
- [Development Principles](#development-principles)
- [Documentation Map](#documentation-map)

## AI Assistant Workflow

Follow the 8-step workflow defined in the root AGENTS.md:

1. **Gather Context** - Read AGENTS.md files for the domain you're modifying
2. **Create a Plan** - Break down tasks into steps
3. **Research & Validate** - Use context7 MCP for .NET/Blazor docs
4. **Verify Behavior** - Use Playwright MCP for testing
5. **Implement Changes** - Follow patterns in domain AGENTS.md
6. **Test & Validate** - Run appropriate test suites
7. **Update Documentation** - Keep AGENTS.md files current
8. **Report Completion** - Summarize changes

## Solution Structure

```text
dotnet/
├── AGENTS.md                     # This file - root .NET context
├── TechHub.sln                  # Solution file
├── src/
│   ├── TechHub.Api/             # REST API (see src/TechHub.Api/AGENTS.md)
│   ├── TechHub.Web/             # Blazor frontend (see src/TechHub.Web/AGENTS.md)
│   ├── TechHub.Core/            # Domain models (see src/TechHub.Core/AGENTS.md)
│   ├── TechHub.Infrastructure/  # Data access (see src/TechHub.Infrastructure/AGENTS.md)
│   ├── TechHub.ServiceDefaults/ # Shared Aspire config
│   └── TechHub.AppHost/         # Aspire orchestration
├── tests/                       # See tests/AGENTS.md
│   ├── TechHub.Core.Tests/
│   ├── TechHub.Api.Tests/
│   ├── TechHub.Web.Tests/
│   ├── TechHub.Infrastructure.Tests/
│   └── TechHub.E2E.Tests/
├── infra/                       # See infra/AGENTS.md
│   ├── main.bicep
│   ├── modules/
│   └── parameters/
└── scripts/                     # See scripts/AGENTS.md
    ├── deploy.ps1
    └── test.ps1
```

## Development Principles

All principles from root [AGENTS.md](../AGENTS.md) apply. Additionally:

### .NET-Specific Rules

- **Use .NET 10** latest LTS features and patterns
- **Prefer records** for DTOs and immutable models
- **Use Minimal APIs** for the REST backend
- **Use file-scoped namespaces** in all C# files
- **Use nullable reference types** - enable in all projects
- **Prefer async/await** for all I/O operations
- **Follow repository pattern** for all data access
- **Use dependency injection** for all services
- **Apply SOLID principles** consistently

### Architecture Rules

- **Separate concerns**: API, Web, Core, Infrastructure layers
- **Domain models in Core**: No dependencies on infrastructure
- **API is stateless**: No session state, use JWT if auth added
- **Web calls API only**: No direct data access from frontend
- **Use DTOs for API**: Don't expose domain models directly

### Testing Requirements

- **Unit tests**: xUnit with Moq for mocking
- **Component tests**: bUnit for Blazor components
- **Integration tests**: WebApplicationFactory for API
- **E2E tests**: Playwright with page object pattern
- **Code coverage**: Minimum 80%

### Code Style

- **File-scoped namespaces**: `namespace TechHub.Api;` (not `namespace TechHub.Api { }`)
- **Nullable reference types**: Enabled in all projects
- **Target-typed new**: Use `new()` instead of `new TypeName()`
- **Primary constructors**: Use for simple classes
- **Collection expressions**: Use `[]` for empty collections
- **String interpolation**: Use `$"{value}"` over `string.Format`

## Documentation Map

| Area | AGENTS.md Location | Purpose |
| ------ | -------------------- | --------- |
| Root .NET | `/dotnet/AGENTS.md` | High-level .NET guidance |
| API | `/dotnet/src/TechHub.Api/AGENTS.md` | API development patterns |
| Web | `/dotnet/src/TechHub.Web/AGENTS.md` | Blazor component patterns |
| Core | `/dotnet/src/TechHub.Core/AGENTS.md` | Domain model patterns |
| Infrastructure | `/dotnet/src/TechHub.Infrastructure/AGENTS.md` | Data access patterns |
| Tests | `/dotnet/tests/AGENTS.md` | Testing strategy |
| Infra | `/dotnet/infra/AGENTS.md` | Bicep/Azure patterns |
| Scripts | `/dotnet/scripts/AGENTS.md` | Automation scripts |

See each domain AGENTS.md for specific patterns and rules.

## Common Commands

```powershell
# Start development server (both API + Web via Aspire)
dotnet run --project src/TechHub.AppHost

# Run all tests
dotnet test

# Run specific test project
dotnet test tests/TechHub.Core.Tests

# Run E2E tests
./scripts/run-e2e-tests.ps1

# Build release
dotnet build -c Release

# Restore dependencies
dotnet restore

# Clean build artifacts
dotnet clean
```

## Development Workflow

### Initial Setup

1. Ensure .NET 10 SDK installed: `dotnet --version`
2. Restore dependencies: `dotnet restore`
3. Build solution: `dotnet build`
4. Run tests: `dotnet test`
5. Start Aspire: `dotnet run --project src/TechHub.AppHost`

### Making Changes

1. Read domain AGENTS.md for the area you're modifying
2. Create/update tests FIRST (TDD)
3. Implement changes following patterns
4. Run tests: `dotnet test`
5. Check for errors: `dotnet build`
6. Start Aspire to verify manually
7. Update documentation if behavior changed

### Before Committing

- [ ] All tests pass: `dotnet test`
- [ ] No build warnings: `dotnet build`
- [ ] Code coverage >= 80%
- [ ] AGENTS.md files updated if needed
- [ ] No secrets in code or config

## Key Patterns

### Dependency Injection

```csharp
// Register services in Program.cs
builder.Services.AddScoped<ISectionRepository, SectionRepository>();
builder.Services.AddSingleton<IFileSystemService, FileSystemService>();

// Inject in endpoints/components
public static async Task<IResult> GetSections(
    ISectionRepository repository,
    CancellationToken ct)
{
    var sections = await repository.GetAllAsync(ct);
    return Results.Ok(sections);
}
```

### Repository Pattern

```csharp
// Interface in TechHub.Core
public interface ISectionRepository
{
    Task<IEnumerable<Section>> GetAllAsync(CancellationToken ct = default);
    Task<Section?> GetByUrlAsync(string url, CancellationToken ct = default);
}

// Implementation in TechHub.Infrastructure
public class SectionRepository : ISectionRepository
{
    private readonly IFileSystemService _fileSystem;
    
    public SectionRepository(IFileSystemService fileSystem)
    {
        _fileSystem = fileSystem;
    }
    
    public async Task<IEnumerable<Section>> GetAllAsync(CancellationToken ct = default)
    {
        // Implementation...
    }
}
```

### Minimal API Endpoints

```csharp
// Group endpoints by feature
public static class SectionEndpoints
{
    public static void MapSectionEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/sections")
            .WithTags("Sections")
            .WithOpenApi();
            
        group.MapGet("/", GetAllSections);
        group.MapGet("/{url}", GetSectionByUrl);
    }
    
    private static async Task<IResult> GetAllSections(
        ISectionRepository repo,
        CancellationToken ct) =>
        Results.Ok(await repo.GetAllAsync(ct));
        
    private static async Task<IResult> GetSectionByUrl(
        string url,
        ISectionRepository repo,
        CancellationToken ct) =>
        await repo.GetByUrlAsync(url, ct) is Section section
            ? Results.Ok(section)
            : Results.NotFound();
}
```

### Blazor Components

```razor
@* SectionCard.razor *@
@inherits ComponentBase

<div class="section-card" style="background-image: url('@Section.Image')">
    <a href="@Section.Url">
        <h3>@Section.Title</h3>
        <p>@Section.Description</p>
    </a>
</div>

@code {
    [Parameter, EditorRequired]
    public Section Section { get; set; } = null!;
}
```

## Related Documentation

- [Root AGENTS.md](../AGENTS.md) - Framework-agnostic principles
- [Migration Plan](../docs/dotnet-migration-plan.md) - Complete migration roadmap
- [Current Site Analysis](../specs/current-site-analysis.md) - Jekyll behavior reference
- [Feature Specifications](../specs/features/) - Detailed feature specs
