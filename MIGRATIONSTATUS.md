# .NET Migration Status

> **Current Phase**: Phase 3 - User Story 1 MVP (API Implementation) ✅ Partially Complete

This document tracks the progress of migrating Tech Hub from Jekyll to .NET/Blazor architecture.

## What's Working

✅ **RESTful API** with nested routes (16 endpoints, all tested and working):

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

**Custom Pages Endpoints**:

- `GET /api/custom-pages` - Get all custom pages (9 pages)
- `GET /api/custom-pages/{slug}` - Get specific custom page by slug

**Examples**:

```bash
# Get AI section with collections
curl http://localhost:5029/api/sections/ai

# Get all AI news items
curl http://localhost:5029/api/sections/ai/collections/news/items

# Complex filter: Copilot-tagged items in AI/ML news/blogs
curl "http://localhost:5029/api/content/filter?sections=ai,ml&collections=news,blogs&tags=copilot"
```

See [docs/api-specification.md](docs/api-specification.md) for complete API reference.

## Implementation Progress

Following the migration plan phases defined in [specs/dotnet-migration/](specs/dotnet-migration/):

- [x] **Phase 1: Foundation** (36/36 tasks) ✅ Complete
  - All projects, domain models, DTOs, interfaces, extensions
- [x] **Phase 2: Data Access** (10/17 tasks) 🔄 In Progress
  - ✅ FrontMatterParser (11 tests passing)
  - ✅ MarkdownService (19 tests passing)
  - ✅ FileBasedSectionRepository (7 tests passing)
  - ✅ FileBasedContentRepository (15 tests passing)
  - ✅ FileBasedCustomPageRepository (9 custom pages loaded)
  - ⏳ RssService, Caching, Entity tests (not started)
- [x] **Phase 3: API Endpoints** (20/70 tasks) 🔄 In Progress
  - ✅ All section endpoints (6 endpoints, 8 tests)
  - ✅ Advanced filtering (2 endpoints, 6 tests)
  - ✅ Custom pages endpoints (2 endpoints, 8 tests)
  - ✅ Blazor home page with section grid
  - ✅ ContentItemCard and SectionCard components
  - ✅ TechHubApiClient with resilience policies
  - ✅ PrimarySection URL routing logic
  - ✅ ViewingMode (internal/external) content handling
  - ✅ Custom page components (9 pages: AISDLC, DXSpace, GenAI series, GitHub Copilot series)
  - ⏳ Content detail pages (partially implemented)

**Test Results**: 242/242 unit/integration tests passing (100%), 111/111 E2E tests passing (100%)

**Performance**: Sections ~25ms, Content first load 5-9s (2266 markdown files)

## What's Working Now

✅ **Frontend** (User Story 1 Complete):

- Home page displaying 8 sections in responsive grid (<http://localhost:5184>)
- Section pages with collection navigation and content display
- Custom pages with dynamic loading (9 pages)
  - AI: SDLC, GenAI Basics, GenAI Applied, GenAI Advanced
  - GitHub Copilot: Features, Handbook, Levels of Enlightenment, VS Code Updates
  - DevOps: Developer Experience Space
- SectionCard and ContentItemCard components with Tech Hub styling
- TechHubApiClient with resilience policies (retry, circuit breaker, timeout)
- PrimarySection URL routing (e.g., `/github-copilot/videos/item-id`)
- ViewingMode support (internal content navigates to detail pages, external opens in new tab)
- Complete Tech Hub design system (colors from Jekyll _sass)
- All 8 section background images

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

## Next Steps

1. Complete Phase 2: RssService, Caching, Entity tests (T045-T051)
2. Continue Phase 3: Section/content detail pages, filtering, accessibility (T062-T087)
3. Begin Phase 4: Features implementation (filtering, search, infinite scroll)

See [specs/dotnet-migration/tasks.md](specs/dotnet-migration/tasks.md) for complete task breakdown.

## Related Documentation

- **[Migration Plan](specs/dotnet-migration/spec.md)** - Complete migration specification (⚠️ may be outdated, verify against code)
- **[Task Breakdown](specs/dotnet-migration/tasks.md)** - Detailed task list with dependencies (⚠️ may be outdated, verify against code)
- **[Data Model](specs/dotnet-migration/data-model.md)** - Domain model design (⚠️ may be outdated, verify against code)
- **[README.md](README.md)** - Quick start guide
- **[AGENTS.md](AGENTS.md)** - Development workflow and standards

**⚠️ Note on Specifications**: The `specs/` directory contains planning documents created during migration. As implementation progresses, these specs may not reflect current reality. Always verify against actual code and prefer documentation in `docs/`, domain-specific AGENTS.md files, and root AGENTS.md. See [AGENTS.md Project Overview section](AGENTS.md#project-overview) for complete guidance on when to use specs.
