# Implementation Plan: PostgreSQL Storage & Search Architecture

**Branch**: `011-azure-search-storage` | **Date**: 2026-01-27 | **Spec**: [spec.md](spec.md)
**Input**: Feature specification from `/specs/011-azure-search-storage/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/commands/plan.md` for the execution workflow.

## Summary

Migrate Tech Hub from filesystem-based content storage to a PostgreSQL database as the primary storage and search engine. This enables blazing-fast faceted navigation with accurate real-time counts, full-text search with PostgreSQL's tsvector, tag subset matching via word tokenization, and cost-effective hosting (~$15/month vs $75-250/month for Azure AI Search). The implementation uses Dapper for optimal read-heavy performance, hash-based incremental sync for fast startup, and SQLite/Docker PostgreSQL for local development. Azure AI Search can be added as Phase 2 for semantic/vector search.

## Technical Context

**Language/Version**: .NET 10, C# 13 with nullable reference types  
**Primary Dependencies**: PostgreSQL 16+, Npgsql, Dapper, SQLite (local dev)  
**Storage**: PostgreSQL (production), SQLite with FTS5 (local dev/integration tests)  
**Testing**: xUnit (unit/integration), bUnit (components), Playwright (E2E), Pester (PowerShell)  
**Target Platform**: Linux containers (Azure Container Apps), DevContainer for development  
**Project Type**: Web application (Blazor SSR frontend + Minimal API backend)  
**Performance Goals**: <200ms tag filtering, <60s first sync, <1s subsequent sync, 100% accurate facet counts  
**Constraints**: Must support local dev without Azure, zero-downtime deployment, fail-fast sync errors  
**Scale/Scope**: 10,000+ articles, ~50MB database, 4 junction tables, 8 user stories

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

| Constitutional Rule | Compliance | Notes |
|---------------------|------------|-------|
| **1. Test-Driven Development** | ✅ PASS | Spec defines 8 user stories with acceptance criteria. Tests will be written first for all changes. |
| **2. 10-Step Development Workflow** | ✅ PASS | Plan follows workflow: gathered context from spec, creating plan now, will research in Phase 0. |
| **3. Domain-Specific AGENTS.md** | ✅ PASS | Will update [src/AGENTS.md](../../src/AGENTS.md), [tests/AGENTS.md](../../tests/AGENTS.md) with new patterns. |
| **4. Configuration-Driven** | ✅ PASS | Database connection and sync settings in appsettings.json. No hardcoded values. |
| **5. Server-Side Rendering First** | ✅ PASS | Database sync happens at startup. All content server-rendered from database. |
| **6. Accessibility (WCAG 2.1 AA)** | ✅ PASS | No UI changes in Phase 1. Existing components already meet standards. |
| **7. Documentation Updates** | ✅ PASS | Will update [docs/content-management.md](../../docs/content-management.md), [docs/api-specification.md](../../docs/api-specification.md) post-implementation. |

**Result**: ✅ ALL GATES PASSED - Proceed to Phase 0

## Project Structure

### Documentation (this feature)

```text
specs/011-azure-search-storage/
├── spec.md              # Feature specification (COMPLETE)
├── plan.md              # This file (IN PROGRESS)
├── research.md          # Phase 0 output (TO BE CREATED)
├── data-model.md        # Phase 1 output (TO BE CREATED)
├── quickstart.md        # Phase 1 output (TO BE CREATED)
├── contracts/           # Phase 1 output (TO BE CREATED)
│   └── IContentRepository.cs  # Repository interface with new search methods
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)

```text
src/
├── TechHub.Core/
│   ├── Models/
│   │   └── ContentItem.cs           # Existing domain model (NO CHANGES)
│   ├── DTOs/
│   │   ├── ContentItemDto.cs        # Maps to database schema
│   │   ├── SearchRequest.cs         # NEW: Search parameters
│   │   ├── SearchResults.cs         # NEW: Search response
│   │   ├── FacetResults.cs          # NEW: Facet counts
│   │   └── PaginationCursor.cs      # NEW: Keyset pagination
│   └── Interfaces/
│       └── IContentRepository.cs    # MODIFIED: Add SearchAsync, GetFacetsAsync, GetRelatedAsync
│
├── TechHub.Infrastructure/
│   ├── Data/
│   │   ├── Migrations/              # NEW: SQL migration scripts
│   │   │   ├── 001_initial_schema.sql
│   │   │   └── 002_fts_indexes.sql
│   │   ├── PostgresContentRepository.cs  # NEW: Dapper implementation for production
│   │   ├── SqliteContentRepository.cs    # NEW: Dapper implementation for local dev
│   ├── Services/
│   │   └── ContentSyncService.cs    # NEW: Hash-based incremental sync
│   └── FileBasedContentRepository.cs  # TO BE REMOVED after migration
│
├── TechHub.Api/
│   └── Program.cs                   # MODIFIED: Add sync on startup, configure database provider
│
└── TechHub.Web/
    └── (NO CHANGES IN PHASE 1)

tests/
├── TechHub.Core.Tests/
│   └── DTOs/                        # NEW: Unit tests for SearchRequest, PaginationCursor
├── TechHub.Infrastructure.Tests/
│   ├── PostgresContentRepositoryTests.cs   # NEW: Integration tests with Docker PostgreSQL
│   ├── SqliteContentRepositoryTests.cs     # NEW: Integration tests with in-memory SQLite
│   └── ContentSyncServiceTests.cs          # NEW: Unit tests for sync logic
├── TechHub.Api.Tests/
│   └── (MODIFIED: Update existing tests to use new repository)
├── TechHub.Web.Tests/
│   └── (NO CHANGES: Component tests still use mocks)
└── TechHub.E2E.Tests/
    └── SearchTests.cs               # NEW: E2E tests for search workflows (AFTER UI implementation)

infra/
└── main.bicep                       # MODIFIED: Add Azure PostgreSQL Flexible Server

scripts/
└── (NO CHANGES: Content processing scripts unchanged)

.devcontainer/
├── post-create.sh                   # MODIFIED: Install PostgreSQL client tools
└── devcontainer.json                # MODIFIED: Add PostgreSQL service container
```

**Structure Decision**: Web application structure (backend API + frontend Web). This is an existing .NET solution with 4 projects (Core, Infrastructure, Api, Web). We're modifying Infrastructure to add database support and replacing FileBasedContentRepository with database implementations. No new projects needed - all changes fit within existing architecture.

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

**No violations detected** - All constitutional rules are satisfied by this implementation.

---

## Phase 0: Research & Outline ✅ COMPLETE

**Status**: Research complete - all technical unknowns resolved  
**Artifact**: [research.md](research.md)

**Research Areas Completed**:

1. ✅ PostgreSQL tsvector full-text search patterns
2. ✅ Dapper query patterns for read-heavy workloads
3. ✅ SQLite FTS5 for local development parity
4. ✅ SHA256 hash-based change detection
5. ✅ Keyset pagination (cursor-based) implementation
6. ✅ SQL migration script strategy
7. ✅ Docker Compose PostgreSQL setup
8. ✅ Azure PostgreSQL tier selection (Basic $15/mo)

---

## Phase 1: Design & Contracts ✅ COMPLETE

**Status**: Design complete - data model, interfaces, and quickstart ready  
**Artifacts**: 
- [data-model.md](data-model.md) - Complete database schema
- [contracts/](contracts/) - Interface definitions (3 files)
- [quickstart.md](quickstart.md) - Developer onboarding guide

**Design Artifacts Created**:

1. ✅ **data-model.md** (Complete)
   - 7 database tables (content_items, collections, content_tags, content_tags_expanded, content_sections, content_plans, sync_metadata)
   - ERD diagram showing relationships
   - PostgreSQL + SQLite schema variants
   - Indexes for performance (GIN, B-tree, covering indexes)
   - Query patterns (faceted nav, tag AND logic, full-text search, keyset pagination)
   - Size estimates (33MB @ 4K items, 50MB @ 6K items)

2. ✅ **contracts/IContentRepository.cs** (Complete)
   - Extended existing repository interface
   - New methods: SearchAsync, GetFacetsAsync, GetRelatedAsync
   - Kept existing methods for backward compatibility

3. ✅ **contracts/SearchDTOs.cs** (Complete)
   - SearchRequest (Query, Tags, Sections, Collections, DateFrom/To, Take, OrderBy, ContinuationToken)
   - SearchResults<T> (Items, TotalCount, HasMore, NextCursor)
   - FacetRequest, FacetResults, FacetValue
   - PaginationCursor with Base64 JSON encoding

4. ✅ **contracts/IContentSyncService.cs** (Complete)
   - SyncAsync (incremental sync with hash-based diff)
   - ForceSyncAsync (full re-import)
   - IsContentChangedAsync (hash comparison for single file)
   - SyncResult record (Added/Updated/Deleted/Unchanged/Duration)

5. ✅ **quickstart.md** (Complete)
   - Local development setup (SQLite vs PostgreSQL)
   - First run vs subsequent run workflows
   - Testing workflows (all tests, specific projects, no tests)
   - Architecture overview and key concepts
   - Common tasks and troubleshooting

**Constitution Re-Check**:

| Rule | Status | Notes |
|------|--------|-------|
| **1. TDD** | ✅ PASS | Test patterns documented, ready for implementation |
| **2. 10-Step Workflow** | ✅ PASS | Following workflow: research → design → tasks → implement |
| **3. AGENTS.md** | ✅ PASS | Updates planned for src/AGENTS.md, tests/AGENTS.md |
| **4. Config-Driven** | ✅ PASS | Database provider, connection string, sync settings in appsettings.json |
| **5. SSR First** | ✅ PASS | Database sync at startup, all content server-rendered |
| **6. Accessibility** | ✅ PASS | No UI changes in Phase 1 |
| **7. Documentation** | ✅ PASS | docs/ updates planned post-implementation |

**Result**: ✅ ALL GATES PASSED - Ready for Phase 2 (tasks.md generation)

---

## Phase 2: Implementation Progress ⏳ IN PROGRESS

**Status**: SQLite implementation COMPLETE ✅ | PostgreSQL implementation NEXT  
**Artifacts**: tasks.md generated and being executed

### Completed Work (✅)

**Phase 1-3 (Setup through User Story 8 - SQLite)**: 100% COMPLETE

- ✅ All NuGet packages added (Npgsql, Dapper, Microsoft.Data.Sqlite)
- ✅ SQL dialect abstraction created (ISqlDialect, SqliteDialect, PostgresDialect)
- ✅ Database schema migrations created (SQLite + PostgreSQL variants)
- ✅ IContentRepository interface extended with SearchAsync, GetFacetsAsync
- ✅ DatabaseContentRepositoryBase created with shared Dapper logic
- ✅ ContentSyncService implemented with hash-based incremental sync
- ✅ SqliteContentRepository fully implemented with FTS5 search
- ✅ SearchDTOs created (SearchRequest, SearchResults, FacetRequest, etc.)
- ✅ BaseContentRepositoryTests created with 203 tests for feature parity
- ✅ FileBasedContentRepositoryTests + SqliteContentRepositoryTests passing (203/203 tests ✅)
- ✅ Integration tests validated with TestCollections data
- ✅ Sync-on-startup integrated into Program.cs

**Phase 4-6 (User Stories 1-3 - SQLite)**: COMPLETE ✅

- ✅ Tag filtering with accurate facet counts (<200ms)
- ✅ Tag subset matching with word boundaries ("AI" matches "Azure AI")
- ✅ Full-text search with FTS5 and BM25 ranking
- ✅ Keyset pagination for infinite scroll
- ✅ Date range filtering integrated

**Test Results**: 203 tests passing, 1 skipped (Postgres-only test)

### Current Status: Ready for PostgreSQL 🎯

**What Works**: 
- FileBasedContentRepository: ✅ All features via file system
- SqliteContentRepository: ✅ All features via SQLite + FTS5
- BaseContentRepositoryTests: ✅ Enforces feature parity across all implementations

**What's Next**: PostgresContentRepository implementation

**Next User Story**: User Story 8 (PostgreSQL path) - Tasks T032-T042

### Next Steps

1. **Implement PostgresContentRepository** (Tasks T032-T042)
   - Create PostgresContentRepository.cs with tsvector full-text search
   - Implement all IContentRepository methods using PostgreSQL-specific optimizations
   - Run BaseContentRepositoryTests against PostgreSQL (Docker required)
   - Performance validation: <200ms tag filtering, <300ms search

2. **Infrastructure Setup** (if not already done)
   - Create docker-compose.yml with PostgreSQL service
   - Update .devcontainer for PostgreSQL client tools
   - Azure Bicep for Azure PostgreSQL Flexible Server

3. **API Endpoints** (Tasks T055-T056, T074-T076)
   - GET /api/facets for tag filtering UI
   - POST /api/search for full-text search UI
   - Integration tests for endpoints

4. **Documentation Updates** (Tasks T100-T104)
   - Update docs/content-management.md
   - Update docs/api-specification.md  
   - Update src/AGENTS.md and tests/AGENTS.md

5. **Cleanup** (Tasks T105-T107)
   - Consider keeping or removing FileBasedContentRepository (spec says keep for feature parity)
