# Tech Hub .NET Migration

> **Status**: 🚧 In Development

This directory contains the .NET/Blazor implementation of Tech Hub, migrating from the Jekyll-based static site.

## Quick Start

### Option 1: F5 Debugging in VS Code (Recommended)

1. Open the project in VS Code
2. Press **F5** (or click **Run > Start Debugging**)
3. Select **"Tech Hub (API + Web)"** from the dropdown
4. Both API and Web servers will start with debugger attached
5. Web UI opens automatically at <http://localhost:5184>
6. API available at <http://localhost:5029> (Swagger: <http://localhost:5029/swagger>)

**Debug Individual Projects**:

- **API only**: Select "API (TechHub.Api)" from debug dropdown
- **Web only**: Select "Web (TechHub.Web)" from debug dropdown

### Option 2: PowerShell Run Script

```powershell
# Basic usage - build and run both projects
./run.ps1

# Clean build and run tests first
./run.ps1 -Clean -Test

# Only build (no run)
./run.ps1 -Build

# Run API only on custom port
./run.ps1 -ApiOnly -ApiPort 8080

# Run Web only without opening browser
./run.ps1 -WebOnly -NoBrowser

# Skip build (use existing binaries)
./run.ps1 -SkipBuild

# Release build
./run.ps1 -Release

# Verbose output
./run.ps1 -VerboseOutput

# See all options
./run.ps1 -?
```

**Script Parameters**:

- `-Clean` - Clean all build artifacts before building
- `-Build` - Only build without running
- `-Test` - Run all tests before starting
- `-SkipBuild` - Skip build, use existing binaries
- `-ApiOnly` - Only run the API project
- `-WebOnly` - Only run the Web project
- `-ApiPort <port>` - Custom API port (default: 5029)
- `-WebPort <port>` - Custom Web port (default: 5184)
- `-NoBrowser` - Don't open browser automatically
- `-Release` - Build in Release mode
- `-VerboseOutput` - Show verbose output

**Built-in Features**:

- **Port Cleanup**: Automatically kills processes using required ports before starting
- **Ctrl+C Handling**: Properly stops all processes and cleans up ports when interrupted
- **Conflict Prevention**: Safe to run even if ports are already in use

### Option 3: Manual dotnet Commands

```powershell
# Terminal 1: API Server
cd src/TechHub.Api
dotnet run --urls http://localhost:5029

# Terminal 2: Web Server
cd src/TechHub.Web
dotnet run
```

### Option 4: Open in DevContainer

1. In VS Code, open the Command Palette (`Ctrl+Shift+P` or `Cmd+Shift+P`)
2. Select **"Dev Containers: Reopen in Container"**
3. Choose the **"Tech Hub .NET"** container
4. Wait for the container to build and initialize
5. Use F5 debugging or run script as described above

## Architecture

This is a modern .NET application with **separate frontend and backend**:

- **TechHub.Api** - REST API backend (ASP.NET Core Minimal API)
- **TechHub.Web** - Blazor frontend (SSR + WebAssembly)
- **TechHub.Core** - Domain models and interfaces
- **TechHub.Infrastructure** - Data access implementations
- **TechHub.AppHost** - .NET Aspire orchestration

**Resilience & Reliability**:

- **HTTP Resilience Policies** - Built-in retry (3 attempts with exponential backoff), circuit breaker (50% failure ratio), and timeout (60s)
- **Graceful Error Handling** - User-friendly error messages with functional retry buttons
- **Automatic State Management** - UI automatically updates during loading and retry operations

See [/specs/](../specs/) for detailed feature specifications.

## Development Workflow

Follow the [8-step workflow](../AGENTS.md#ai-assistant-workflow) defined in the root AGENTS.md:

1. **Gather Context** - Read AGENTS.md files for the domain you're modifying
2. **Create a Plan** - Break down tasks into steps
3. **Research & Validate** - Use context7 MCP for .NET/Blazor docs
4. **Verify Behavior** - Use Playwright MCP for testing
5. **Implement Changes** - Follow patterns in domain AGENTS.md
6. **Test & Validate** - Run appropriate test suites
7. **Update Documentation** - Keep AGENTS.md files current
8. **Report Completion** - Summarize changes

## Documentation

- **[Feature Specifications](specs/)** - Complete feature requirements and specifications
- **[Root AGENTS.md](AGENTS.md)** - Framework-agnostic principles and architecture
- **[Documentation Guidelines](docs/AGENTS.md)** - How to maintain and structure documentation
- **[.NET Development Guide](.github/agents/dotnet.md)** - .NET-specific development patterns
- **[API Specification](docs/api-specification.md)** - REST API contracts and endpoints
- **[Filtering System](docs/filtering-system.md)** - Tag and date filtering behavior
- **[Content Management](docs/content-management.md)** - Content workflows and RSS processing

## Current Status

**Phase 3: User Story 1 MVP - API Implementation** ✅ (Partially Complete)

### What's Working

✅ **RESTful API** with nested routes (14 endpoints, all tested and working):

**Section Endpoints**:

- `GET /api/sections` - Get all sections (8 sections)
- `GET /api/sections/{sectionName}` - Get specific section
- `GET /api/sections/{sectionName}/items` - All items in section (e.g., 1378 AI items)
- `GET /api/sections/{sectionName}/collections` - Collections in section
- `GET /api/sections/{sectionName}/collections/{collectionName}` - Collection details
- `GET /api/sections/{sectionName}/collections/{collectionName}/items` - Items in collection

**Content Endpoints**:

- `GET /api/content/filter?sections={s}&collections={c}&tags={t}&q={query}` - Advanced filtering
- `GET /api/content/tags` - All unique tags (12,524 tags)

**Examples**:

```bash
# Get AI section with collections
curl http://localhost:5029/api/sections/ai

# Get all AI news items
curl http://localhost:5029/api/sections/ai/collections/news/items

# Complex filter: Copilot-tagged items in AI/ML news/blogs
curl "http://localhost:5029/api/content/filter?sections=ai,ml&collections=news,blogs&tags=copilot"
```

See [API Specification](docs/api-specification.md) for complete reference.

### Implementation Progress

Following the migration plan phases:

- [x] Phase 1: Foundation (36/36 tasks) ✅
  - All projects, domain models, DTOs, interfaces, extensions
- [x] Phase 2: Data Access (8/17 tasks) 🔄
  - ✅ FrontMatterParser (11 tests passing)
  - ✅ MarkdownService (19 tests passing)
  - ✅ FileBasedSectionRepository (7 tests passing)
  - ✅ FileBasedContentRepository (15 tests passing)
  - ⏳ RssService, Caching, Entity tests (not started)
- [x] Phase 3: API Endpoints (5/70 tasks) 🔄
  - ✅ All section endpoints (6 endpoints, 8 tests)
  - ✅ Advanced filtering (2 endpoints, 6 tests)
  - ⏳ Blazor components, pages, client (not started)

**Test Results**: 52/52 tests passing (100% pass rate)

**Performance**: Sections ~25ms, Content first load 5-9s (2251 markdown files)

### What's Working Now

✅ **Frontend** (User Story 1 ~90% Complete):

- Home page displaying 8 sections in responsive grid (<http://localhost:5184>)
- SectionCard component with background images
- ContentItemCard component ready for section pages
- TechHubApiClient with typed HTTP methods
- Complete Tech Hub design system (colors from Jekyll _sass)
- All 8 section background images (ai.jpg, azure.jpg, coding.jpg, devops.jpg, github-copilot.jpg, ml.jpg, security.jpg, all.jpg)

**Running the Application**:

```bash
# Terminal 1: API Server
cd src/TechHub.Api
dotnet run --urls http://localhost:5029

# Terminal 2: Web Server  
cd src/TechHub.Web
dotnet run
```

**Access**: Web UI at <http://localhost:5184>, API at <http://localhost:5029/api/sections>

### Next Steps

1. Complete Phase 2: RssService, Caching, Entity tests (T045-T051)
2. Continue Phase 3: Section/content detail pages, filtering, accessibility (T062-T087)
3. Begin Phase 4: Features implementation (filtering, search, infinite scroll)

See [specs/dotnet-migration/tasks.md](specs/dotnet-migration/tasks.md) for complete task breakdown.

## Contributing

This is a migration project. All changes should:

1. Follow the [feature specifications](../specs/)
2. Use spec-driven development methodology
3. Maintain feature parity with Jekyll site
4. Include tests for all code changes

## Related Resources

- [Current Jekyll Site](../) - Source implementation
- [Filtering System Docs](../docs/filtering-system.md) - Current filtering behavior
- [Content Management Docs](../docs/content-management.md) - Content workflows
- [spec-kit](https://github.com/github/spec-kit) - Development methodology
