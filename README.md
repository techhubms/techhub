# Tech Hub .NET Migration

> **Status**: 🚧 In Development

This directory contains the .NET/Blazor implementation of Tech Hub, migrating from the Jekyll-based static site.

## Quick Start

### Option 1: Open in DevContainer (Recommended)

1. In VS Code, open the Command Palette (`Ctrl+Shift+P` or `Cmd+Shift+P`)
2. Select **"Dev Containers: Reopen in Container"**
3. Choose the **"Tech Hub .NET"** container
4. Wait for the container to build and initialize

### Option 2: Local Development

Requirements:

- .NET 10 SDK
- Node.js LTS
- PowerShell 7+
- Azure CLI (optional)

```powershell
# Install .NET Aspire workload
dotnet workload install aspire

# Restore packages (when solution exists)
dotnet restore

# Run with Aspire (when solution exists)
dotnet run --project src/TechHub.AppHost
```

## Architecture

This is a modern .NET application with **separate frontend and backend**:

- **TechHub.Api** - REST API backend (ASP.NET Core Minimal API)
- **TechHub.Web** - Blazor frontend (SSR + WebAssembly)
- **TechHub.Core** - Domain models and interfaces
- **TechHub.Infrastructure** - Data access implementations
- **TechHub.AppHost** - .NET Aspire orchestration

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
