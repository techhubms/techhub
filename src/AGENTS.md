# Source Code Development Guide

> **AI CONTEXT**: This is a **LEAF** context file for the `src/` directory. It complements the [Root AGENTS.md](../AGENTS.md).
> **RULE**: Follow the 8-step workflow in Root [AGENTS.md](../AGENTS.md).

## Critical Development Rules

### ✅ Always Do

- **Always run tests after code changes**: `Run -Clean` or `Run -Clean -TestProject <name>`
- **Always use -Clean to expose build warnings** so you can fix them
- **Always fix all build warnings** (0 warnings policy enforced)
- **Always use file-scoped namespaces** in all C# files
- **Always simplify collection initialization**: Only when its a simple collection, don't wrap larger statements in `[]`
- **Always write tests BEFORE or DURING implementation** (TDD)
- **Always maintain 80%+ code coverage** for unit tests
- **Always use context7 MCP tool** for latest .NET/Blazor documentation
- **Always check ALL occurrences before renaming** (use `grep_search` to find all, then update each)
- **Always add the latest stable version when adding NuGet packages**
- **Always sanitize user-controlled strings for logging**: Use `.Sanitize()` extension method (from `TechHub.Core.Logging`). See [docs/input-validation-and-sanitization.md](../docs/input-validation-and-sanitization.md).

### ⚠️ Ask First

- **Ask first before database schema changes** or data model modifications
- **Ask first before adding new dependencies** to project files
- **Ask first before breaking API changes** that affect existing endpoints
- **Ask first before significant refactoring** across multiple projects

### 🚫 Never Do

- **Never commit code with build warnings or errors**
- **Never suppress warnings without documenting rationale** in `Directory.Build.props`
- **Never assume UTC** (use `Europe/Brussels` timezone)
- **Never make parameters nullable without good reason**
- **Never use `List<T>` in public APIs** (use `IReadOnlyList<T>`)
- **Never hide errors or swallow exceptions** — See [Error Handling Philosophy](#error-handling-philosophy)
- **Never use primary constructors** (use explicit constructors)
- **Never rename identifiers without checking ALL occurrences**

## Error Handling Philosophy

- **Be defensive for expected failures** — User input, external services, transient network issues: use retry policies (Polly), validation, or other appropriate strategies
- **Let unexpected errors surface** — After retries exhaust, let exceptions propagate. Never silently catch-and-continue; that hides faults and bad behavior
- **End users get friendly error messages** — But logs and Application Insights must contain full exception details, parameters, and context
- **Log then rethrow or let it crash** — Prefer logging with context and rethrowing over catching and returning a default value

## Project Structure

```text
src/
├── TechHub.Api/           # REST API Backend (Minimal APIs)
├── TechHub.Web/           # Blazor Frontend (InteractiveServer)
├── TechHub.Core/          # Domain Models & Interfaces (zero dependencies)
├── TechHub.Infrastructure/ # Data Access (PostgreSQL, Markdown, RSS)
├── TechHub.ServiceDefaults/ # Shared Aspire Configuration
└── TechHub.AppHost/       # Aspire Orchestration
```

Each project has its own AGENTS.md with detailed patterns.

## Code Quality

- **Code analysis**: `latest-all` level, all analyzers enabled, style enforced in build
- **EditorConfig**: Auto-formatting (Allman braces, file-scoped namespaces, pattern matching)
- **Warning suppressions**: Documented in `Directory.Build.props` with XML comments
- **Naming**: `PascalCase` for types/methods/properties, `_camelCase` for private fields, `camelCase` for parameters

## Domain Terminology

- **`section`** / **`sectionName`**: Section object vs its Name string identifier (e.g., "ai", "github-copilot")
- **`collection`** / **`collectionName`**: Collection object vs its Name string identifier (e.g., "news", "videos")
- Use suffixed names (`sectionName`, `collectionName`) for string identifiers in API/route/method parameters

## Dependency Injection Service Lifetimes

- **Singleton**: `ISectionRepository`, `ISqlDialect`, `IDbConnectionFactory`, `IMemoryCache`
- **Scoped**: `IDbConnection`, `ITechHubApiClient`
- **Transient**: `IContentRepository`, `IMarkdownService`, `IRssService`, `ITagCloudService`, `MigrationRunner`
- Always use Options pattern for configuration (never `builder.Configuration["Key"]`)

## Port Configuration

Fixed ports defined in `launchSettings.json` (single source of truth):

- API: 5001 (HTTPS)
- Web: 5003 (HTTPS)
- Aspire Dashboard: 18888 (HTTP)
