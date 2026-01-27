# Tasks: PostgreSQL Storage & Search Architecture

**Input**: Design documents from `/specs/011-azure-search-storage/`  
**Prerequisites**: plan.md ✅, spec.md ✅, research.md ✅, data-model.md ✅, contracts/ ✅

**Implementation Strategy**: SQLite first (lower risk, faster iteration), then PostgreSQL (production-ready expansion)

**Organization**: Tasks grouped by user story to enable independent implementation and testing of each story

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US8)
- Include exact file paths in descriptions

---

## Phase 1: Setup (Project Infrastructure)

**Purpose**: Database provider abstraction and project setup

- [ ] T001 Add Npgsql NuGet package to src/TechHub.Infrastructure/TechHub.Infrastructure.csproj
- [ ] T002 Add Dapper NuGet package to src/TechHub.Infrastructure/TechHub.Infrastructure.csproj (micro-ORM for efficient database queries)
- [ ] T003 Add Microsoft.Data.Sqlite NuGet package to src/TechHub.Infrastructure/TechHub.Infrastructure.csproj
- [ ] T004 Create ISqlDialect abstraction in src/TechHub.Core/Interfaces/ISqlDialect.cs
- [ ] T005 [P] Create SqliteDialect implementation in src/TechHub.Infrastructure/Data/SqliteDialect.cs
- [ ] T006 [P] Create PostgresDialect implementation in src/TechHub.Infrastructure/Data/PostgresDialect.cs
- [ ] T007 Create database configuration section in src/TechHub.Api/appsettings.json and src/TechHub.Web/appsettings.json

---

## Phase 2: Foundational (Database Schema & Core Infrastructure)

**Purpose**: Core infrastructure that MUST be complete before ANY user story can be implemented

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

### Schema Creation

- [ ] T008 Create SQL migration script for SQLite in src/TechHub.Infrastructure/Data/Migrations/sqlite/001_initial_schema.sql
- [ ] T009 Create SQL migration script for PostgreSQL in src/TechHub.Infrastructure/Data/Migrations/postgres/001_initial_schema.sql
- [ ] T010 Create migration runner service in src/TechHub.Infrastructure/Data/MigrationRunner.cs (executes SQL scripts automatically on startup)

### Repository Base

- [ ] T011 Update IContentRepository interface in src/TechHub.Core/Interfaces/IContentRepository.cs per contracts/IContentRepository.cs
- [ ] T012 Create ContentRepositoryBase abstract class in src/TechHub.Infrastructure/Data/ContentRepositoryBase.cs with shared Dapper logic
- [ ] T013 Create InMemoryContentRepository for tests in tests/TechHub.TestUtilities/InMemoryContentRepository.cs <<< Do NOT do this! We have a repository for testing in our testutilities. if needed update that one

### Sync Infrastructure

- [ ] T014 Create IContentSyncService interface in src/TechHub.Core/Interfaces/IContentSyncService.cs per contracts/IContentSyncService.cs
- [ ] T015 Create SyncResult DTO in src/TechHub.Core/DTOs/SyncResult.cs
- [ ] T016 Create ContentSyncService in src/TechHub.Infrastructure/Services/ContentSyncService.cs with hash-based diff logic
- [ ] T017 Add ContentSync configuration section to appsettings.Development.json and appsettings.Production.json

### Testing Infrastructure

- [ ] T018 Create DatabaseFixture<T> for integration tests in tests/TechHub.TestUtilities/DatabaseFixture.cs
- [ ] T019 Create test data seeding helpers in tests/TechHub.TestUtilities/TestDataBuilder.cs

**Checkpoint**: Foundation ready - user story implementation can now begin

---

## Phase 3: User Story 8 - Local Development Without Azure (Priority: P0) 🎯 MVP FOUNDATION

**Goal**: Developers can run full app locally with SQLite or Docker PostgreSQL, no Azure required

**Independent Test**: Run `Run` without Azure credentials, verify all tests pass

**Strategy**: Build SQLite first (simpler, faster iteration), then expand to PostgreSQL using same patterns

### SQLite Implementation (Lower Risk Path)

- [ ] T020 [P] [US8] Create SqliteContentRepository in src/TechHub.Infrastructure/Data/SqliteContentRepository.cs
- [ ] T021 [P] [US8] Create SearchDTOs in src/TechHub.Core/DTOs/SearchDTOs.cs per contracts/SearchDTOs.cs
- [ ] T022 [US8] Implement GetBySlugAsync in SqliteContentRepository using Dapper
- [ ] T023 [US8] Implement GetAllAsync in SqliteContentRepository with keyset pagination
- [ ] T024 [US8] Implement GetByCollectionAsync in SqliteContentRepository
- [ ] T025 [US8] Implement GetBySectionAsync in SqliteContentRepository
- [ ] T026 [US8] Update Program.cs in src/TechHub.Api/Program.cs to register SQLite provider when Database:Provider = "SQLite"
- [ ] T027 [US8] Update Program.cs in src/TechHub.Web/Program.cs to register SQLite provider when Database:Provider = "SQLite"
- [ ] T028 [US8] Add sync-on-startup to both Program.cs files (call ContentSyncService before app.Run())
- [ ] T029 [US8] Create integration tests for SqliteContentRepository in tests/TechHub.Infrastructure.Tests/SqliteContentRepositoryTests.cs
- [ ] T030 [US8] Verify sync skipping works when ContentSync:Enabled = false in tests/TechHub.Infrastructure.Tests/ContentSyncServiceTests.cs
- [ ] T031 [US8] Verify hash-based incremental sync in tests/TechHub.Infrastructure.Tests/ContentSyncServiceTests.cs

### PostgreSQL Implementation (Expand on SQLite Patterns)

- [ ] T032 [US8] Create PostgresContentRepository in src/TechHub.Infrastructure/Data/PostgresContentRepository.cs (reuse SqliteContentRepository patterns)
- [ ] T033 [US8] Implement GetBySlugAsync in PostgresContentRepository
- [ ] T034 [US8] Implement GetAllAsync in PostgresContentRepository with keyset pagination
- [ ] T035 [US8] Implement GetByCollectionAsync in PostgresContentRepository
- [ ] T036 [US8] Implement GetBySectionAsync in PostgresContentRepository
- [ ] T037 [US8] Update Program.cs in src/TechHub.Api/Program.cs to register PostgreSQL provider when Database:Provider = "PostgreSQL"
- [ ] T038 [US8] Update Program.cs in src/TechHub.Web/Program.cs to register PostgreSQL provider when Database:Provider = "PostgreSQL"
- [ ] T039 [US8] Create docker-compose.yml in repository root with PostgreSQL service
- [ ] T040 [US8] Update .devcontainer/post-create.sh to install PostgreSQL client tools
- [ ] T041 [US8] Create integration tests for PostgresContentRepository in tests/TechHub.Infrastructure.Tests/PostgresContentRepositoryTests.cs
- [ ] T042 [US8] Create E2E tests with Docker PostgreSQL in tests/TechHub.E2E.Tests/DatabaseTests.cs

**Checkpoint**: Local development works with both SQLite and PostgreSQL, sync tested

---

## Phase 4: User Story 1 - Fast Tag Filtering with Accurate Counts (Priority: P0) 🎯 MVP CORE

**Goal**: Users filter by tags and see accurate facet counts updated within 200ms, with AND logic

**Independent Test**: Select "AI" tag, verify counts update <200ms showing intersection with other tags

**Strategy**: Implement in SQLite first, validate performance, then add PostgreSQL-specific optimizations

### SQLite Tag Filtering

- [ ] T043 [P] [US1] Implement GetFacetsAsync in SqliteContentRepository using word-boundary tag matching
- [ ] T044 [P] [US1] Create FacetResults DTO per contracts/SearchDTOs.cs
- [ ] T045 [US1] Add GIN-equivalent indexes to SQLite migration (001_initial_schema.sql)
- [ ] T046 [US1] Implement tag subset matching (word boundaries) in SqliteContentRepository
- [ ] T047 [US1] Implement tag AND logic using INTERSECT or HAVING COUNT in SqliteContentRepository
- [ ] T048 [US1] Create integration tests for GetFacetsAsync in tests/TechHub.Infrastructure.Tests/SqliteContentRepositoryTests.cs
- [ ] T049 [US1] Create performance tests for tag filtering (<200ms target) in tests/TechHub.Infrastructure.Tests/PerformanceTests.cs
- [ ] T050 [US1] Verify tag subset matching ("AI" matches "Azure AI", "Generative AI") in tests/TechHub.Infrastructure.Tests/TagMatchingTests.cs

### PostgreSQL Tag Filtering (Optimized)

- [ ] T051 [US1] Implement GetFacetsAsync in PostgresContentRepository with GIN indexes
- [ ] T052 [US1] Add PostgreSQL-specific optimizations (parallel query hints) if needed
- [ ] T053 [US1] Create integration tests for PostgresContentRepository GetFacetsAsync in tests/TechHub.Infrastructure.Tests/PostgresContentRepositoryTests.cs
- [ ] T054 [US1] Verify performance <200ms with 1000+ content items in tests/TechHub.Infrastructure.Tests/PerformanceTests.cs

### API Endpoints for Tag Filtering

- [ ] T055 [US1] Create GET /api/facets endpoint in src/TechHub.Api/Endpoints/FacetsEndpoints.cs
- [ ] T056 [US1] Add integration tests for /api/facets in tests/TechHub.Api.Tests/FacetsEndpointTests.cs

**Checkpoint**: Tag filtering works with accurate counts, <200ms performance validated

---

## Phase 5: User Story 2 - Tag Subset Matching (Priority: P0)

**Goal**: Selecting "AI" shows all content with tags containing "AI" as complete word

**Independent Test**: Search "AI" tag, verify results include "AI", "Azure AI", "Generative AI" but NOT "AIR"

### Tag Subset Matching Implementation (Both Databases)

- [ ] T057 [P] [US2] Verify word-boundary regex in SqliteContentRepository (already implemented in US1)
- [ ] T058 [P] [US2] Verify word-boundary regex in PostgresContentRepository (already implemented in US1)
- [ ] T059 [US2] Create comprehensive tests for edge cases in tests/TechHub.Infrastructure.Tests/TagSubsetMatchingTests.cs
- [ ] T060 [US2] Test "Visual Studio" matches "Visual Studio Code" but not "VisualStudio" (no space)
- [ ] T061 [US2] Test word boundaries with special characters (e.g., "C#", ".NET")

**Checkpoint**: Tag subset matching validated with edge cases

---

## Phase 6: User Story 3 - Full-Text Search with Highlighting (Priority: P1)

**Goal**: Users search content item titles and content with highlighted results and relevance ranking

**Independent Test**: Search "agent framework tutorial", verify results ranked by relevance with highlights

### SQLite FTS5 Implementation

- [ ] T062 [US3] Create FTS5 virtual table migration in src/TechHub.Infrastructure/Data/Migrations/sqlite/002_fts_indexes.sql
- [ ] T063 [US3] Implement SearchAsync in SqliteContentRepository using FTS5
- [ ] T064 [US3] Add snippet extraction for search highlights in SqliteContentRepository
- [ ] T065 [US3] Implement relevance ranking using BM25 in SqliteContentRepository
- [ ] T066 [US3] Create integration tests for SearchAsync in tests/TechHub.Infrastructure.Tests/SqliteContentRepositoryTests.cs
- [ ] T067 [US3] Verify search performance <300ms in tests/TechHub.Infrastructure.Tests/PerformanceTests.cs

### PostgreSQL tsvector Implementation

- [ ] T068 [US3] Create tsvector migration in src/TechHub.Infrastructure/Data/Migrations/postgres/002_fts_indexes.sql
- [ ] T069 [US3] Implement SearchAsync in PostgresContentRepository using tsvector
- [ ] T070 [US3] Add ts_headline for search highlights in PostgresContentRepository
- [ ] T071 [US3] Implement relevance ranking using ts_rank in PostgresContentRepository
- [ ] T072 [US3] Create integration tests for PostgresContentRepository SearchAsync in tests/TechHub.Infrastructure.Tests/PostgresContentRepositoryTests.cs
- [ ] T073 [US3] Verify search performance <300ms in tests/TechHub.Infrastructure.Tests/PerformanceTests.cs

### API Endpoints for Search

- [ ] T074 [US3] Create POST /api/search endpoint in src/TechHub.Api/Endpoints/SearchEndpoints.cs
- [ ] T075 [US3] Add integration tests for /api/search in tests/TechHub.Api.Tests/SearchEndpointTests.cs
- [ ] T076 [US3] Create E2E tests for search workflow in tests/TechHub.E2E.Tests/SearchTests.cs

**Checkpoint**: Full-text search works with highlighting and relevance ranking

---

## Phase 7: User Story 5 - Date Range Filtering with Facets (Priority: P1)

**Goal**: Filter by date ranges with facet counts updating to reflect temporal scope

**Independent Test**: Set date range "Last 30 days", verify tag counts reflect only that period

### Implementation (Both Databases)

- [ ] T077 [P] [US5] Add date range parameters to GetFacetsAsync in SqliteContentRepository
- [ ] T078 [P] [US5] Add date range parameters to GetFacetsAsync in PostgresContentRepository
- [ ] T079 [US5] Update SearchAsync to support date filtering in both repositories
- [ ] T080 [US5] Create integration tests for date range filtering in tests/TechHub.Infrastructure.Tests/DateFilteringTests.cs
- [ ] T081 [US5] Verify facet counts update correctly with date ranges
- [ ] T082 [US5] Test edge cases (timezone boundaries, epoch 0, future dates)

### API Updates

- [ ] T083 [US5] Update GET /api/facets to accept date range parameters in src/TechHub.Api/Endpoints/FacetsEndpoints.cs
- [ ] T084 [US5] Update POST /api/search to accept date range parameters in src/TechHub.Api/Endpoints/SearchEndpoints.cs
- [ ] T085 [US5] Add integration tests for date-filtered endpoints in tests/TechHub.Api.Tests/DateFilteringEndpointTests.cs

**Checkpoint**: Date range filtering works with accurate facet counts

---

## Phase 8: User Story 6 - Related Articles Discovery (Priority: P2 - Phase 1 Implementation)

**Goal**: View content item and see related content based on tag overlap

**Independent Test**: View "Azure AI Search" content item, verify related section shows topically similar content

**Note**: Phase 1 uses tag-based similarity. Phase 2 (future) will add semantic similarity via Azure AI Search.

### Tag-Based Related Content Items

- [ ] T086 [P] [US6] Implement GetRelatedAsync in SqliteContentRepository using tag overlap query per data-model.md
- [ ] T087 [P] [US6] Implement GetRelatedAsync in PostgresContentRepository
- [ ] T088 [US6] Add fallback logic (same section/collection) when tag overlap returns <5 results
- [ ] T089 [US6] Create integration tests for GetRelatedAsync in tests/TechHub.Infrastructure.Tests/RelatedArticlesTests.cs
- [ ] T090 [US6] Verify results ranked by shared tag count descending

### API Endpoints

- [ ] T091 [US6] Create GET /api/related/{articleId} endpoint in src/TechHub.Api/Endpoints/RelatedEndpoints.cs
- [ ] T092 [US6] Add integration tests for /api/related in tests/TechHub.Api.Tests/RelatedEndpointTests.cs

**Checkpoint**: Related articles feature works based on tag overlap

---

## Phase 9: User Story 4 & 7 - Deferred to Phase 2 (Azure AI Search)

**Note**: These user stories require Azure AI Search integration:

- **User Story 4**: Semantic AI Search (vector embeddings, natural language queries)
- **User Story 7**: MCP Server for AI Agents (benefits from semantic search)

**Phase 1 Completion**: PostgreSQL provides solid foundation for Phase 2 AI features

**Future Tasks** (not in this implementation):

- Azure AI Search provisioning
- Vector embedding generation
- Semantic ranking integration
- MCP server implementation

---

## Phase 10: Polish & Cross-Cutting Concerns

**Purpose**: Production readiness and documentation

### Testing & Validation

- [ ] T093 [P] Run full test suite (PowerShell, unit, integration, component, E2E)
- [ ] T094 [P] Verify quickstart.md accuracy by following all steps
- [ ] T095 [P] Performance validation: Tag filtering <200ms, search <300ms, sync <60s first run, <1s subsequent
- [ ] T096 Verify zero-count tags are visually de-emphasized in UI

### Infrastructure

- [ ] T097 Update infra/main.bicep to add Azure PostgreSQL Flexible Server (Basic tier)
- [ ] T098 Create deployment script for database migrations in scripts/Deploy-Database.ps1
- [ ] T099 Add connection string to Azure Key Vault reference in appsettings.Production.json

### Documentation

- [ ] T100 [P] Update docs/content-management.md to document database sync workflow
- [ ] T101 [P] Update docs/api-specification.md with new endpoints (/api/search, /api/facets, /api/related)
- [ ] T102 [P] Update src/AGENTS.md with Dapper patterns and repository implementation examples
- [ ] T103 [P] Update tests/AGENTS.md with database testing patterns
- [ ] T104 Update README.md with database setup instructions

### Cleanup

- [ ] T105 Remove src/TechHub.Infrastructure/FileBasedContentRepository.cs (old filesystem implementation)
- [ ] T106 Remove references to FileBasedContentRepository from dependency injection
- [ ] T107 Verify no regression in existing API endpoints (non-database endpoints still work)

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - start immediately
- **Foundational (Phase 2)**: Depends on Setup - BLOCKS all user stories
- **User Story 8 (Phase 3)**: Depends on Foundational - LOCAL DEV FOUNDATION (must complete first)
- **User Stories 1, 2, 3, 5, 6 (Phases 4-8)**: All depend on Phase 3 (US8) completion
- **Polish (Phase 10)**: Depends on all user stories being complete

### User Story Dependencies

- **US8 (P0)**: Must complete FIRST - provides database infrastructure for all other stories
- **US1 (P0)**: Depends on US8 - can start after Phase 3
- **US2 (P0)**: Depends on US1 (tag subset logic reuses facet implementation)
- **US3 (P1)**: Depends on US8 - can start after Phase 3 (independent of US1/US2)
- **US5 (P1)**: Depends on US1 (extends facet logic with date filters)
- **US6 (P2)**: Depends on US8 - can start after Phase 3 (independent of other stories)

### Implementation Strategy: SQLite → PostgreSQL

**For each user story**:

1. ✅ Implement in SqliteContentRepository FIRST (faster iteration, simpler debugging)
2. ✅ Write and run integration tests against SQLite
3. ✅ Validate performance and correctness
4. ✅ Expand to PostgresContentRepository (reuse patterns, add optimizations)
5. ✅ Run same integration tests against PostgreSQL
6. ✅ Compare performance, optimize if needed

**Benefits**:

- Lower risk: SQLite validates core logic before PostgreSQL complexity
- Faster feedback: No Docker dependency for initial development
- Parallel work: One developer on SQLite, another on PostgreSQL (after SQLite works)
- Test reuse: Same test suite runs against both providers

### Parallel Opportunities

**Phase 1 (Setup)**: T001-T003 can run in parallel (different packages), T005-T006 can run in parallel (different dialects)

**Phase 2 (Foundational)**: 
- T008-T009 can run in parallel (different SQL dialects)
- T012-T013 can run in parallel (base class vs test implementation)
- T018-T019 can run in parallel (different test utilities)

**Phase 3 (US8)**:
- T020-T021 can run in parallel (repository vs DTOs)
- T022-T025 can run in parallel (different repository methods, same file)
- T029-T031 can run in parallel (different test files)
- T032-T036 can run in parallel (different PostgreSQL methods, same pattern as SQLite)
- T041-T042 can run in parallel (different test projects)

**Phase 4 (US1)**:
- T043-T044 can run in parallel (implementation vs DTO)
- T048-T050 can run in parallel (different test files)
- T051-T054 can run in parallel (after T043-T050 validate SQLite approach)

**Phase 6 (US3)**:
- T062-T067 (SQLite FTS5) can be developed in parallel with T068-T073 (PostgreSQL tsvector) by different developers

---

## Parallel Example: User Story 8 (Local Development)

**SQLite Track**:

```bash
# SQLite foundation
- T020: Create SqliteContentRepository
- T021: Create SearchDTOs
- T022-T025: Implement basic CRUD methods (parallel)

# Validation
- T029-T031: Integration tests (parallel)
```

**PostgreSQL Track** (starts after SQLite works):

```bash
# PostgreSQL expansion
- T032: Create PostgresContentRepository (copy SQLite patterns)
- T033-T036: Implement same methods (parallel, proven logic)

# Validation
- T041-T042: Integration tests (parallel)
```

---

## MVP Scope Recommendation

**Minimal Viable Product** = Local development + Tag filtering:

- ✅ Phase 1: Setup
- ✅ Phase 2: Foundational
- ✅ Phase 3: User Story 8 (Local dev with SQLite only - defer PostgreSQL)
- ✅ Phase 4: User Story 1 (Tag filtering with SQLite only)
- ⏸️ Defer: PostgreSQL implementations, full-text search, date filtering, related articles

**MVP Delivers**:

- Working local dev environment (SQLite)
- Database sync from markdown files
- Tag-based filtering with accurate counts
- Foundation for all future features
