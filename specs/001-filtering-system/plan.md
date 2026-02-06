# Implementation Plan: Sidebar Content Filtering

**Branch**: `dotnet-migration` | **Date**: 2026-01-16 | **Updated**: 2026-02-03 | **Spec**: [spec.md](spec.md)  
**Input**: Feature specification from `/specs/001-filtering-system/spec.md`

## Summary

Implement client-side tag and date filtering for Tech Hub content discovery. Users can filter content via sidebar tag cloud (top 20 scoped tags with **dynamic counts**), Excel-style tag dropdown (all tags with search and **dynamic counts**), and date range slider (defaults to last 90 days).

**Key Feature - Dynamic Tag Counts**:

- Each tag displays how many items would remain if selected (e.g., "AI (901)")
- When tags are selected, other tags show **intersection counts** (items matching existing filters AND this tag)
- Tags that would result in 0 items become **disabled** (grayed out, non-clickable)
- Date range changes trigger **tag count recalculation**

Filters use OR logic within tags, AND logic between filter types, with URL state preservation and sub-50ms response time.

## Implementation Status

| Component | Status | Notes |
|-----------|--------|----------|
| Core Models (Filter/Tags) | ✅ Complete | In `src/TechHub.Core/Models/` (not DTOs/) |
| FacetRequest, FacetResults | ✅ Complete | In `src/TechHub.Core/Models/Facets/` |
| ITagCloudService | ✅ Complete | - |
| TagCloudService | ✅ Complete | Quantile sizing, scoping logic |
| IContentRepository.GetTagCountsAsync | ✅ Complete | With date/section/collection filtering |
| IContentRepository.GetFacetsAsync | ✅ Complete | Fully implemented with filtering |
| SidebarTagCloud | ⚠️ Partial | Basic filtering ✅, dynamic counts ❌ |
| Tag cloud endpoint | ⚠️ Partial | Exists, needs filter params (tags, from, to) |
| E2E tests (basic filtering) | ✅ Complete | TagFilteringTests.cs |
| **Dynamic Counts Feature** | | |
| Enhanced tag cloud endpoint | ❌ Not started | Add filter params to existing endpoint |
| Dynamic count frontend | ❌ Not started | Use enhanced API in SidebarTagCloud |
| Disabled state (UI) | ❌ Not started | Frontend logic (count === 0) |
| Date range affects counts | ❌ Not started | Integration layer |
| **Other Components** | | |
| DateRangeSlider | ❌ Not started | - |
| TagDropdownFilter | ❌ Not started | - |
| FilterStateService | ❌ Not started | - |

## Technical Context

**Language/Version**: .NET 10, C# 13  
**Primary Framework**: Blazor Server-Side Rendering + WebAssembly  
**Primary Dependencies**: ASP.NET Core Minimal APIs, System.Text.Json, Markdig (markdown)  
**Storage**: File-based markdown with YAML frontmatter (collections/ directory)  
**Testing**: xUnit (unit/integration), bUnit (component), Playwright (E2E)  
**Target Platform**: Azure Container Apps, Linux containers  
**Project Type**: Web application (API backend + Blazor frontend)  
**Performance Goals**: <50ms client-side filtering, <200ms API response, virtual scrolling at 50+ tags  
**Constraints**: Client-side filtering only (no server-side search), SEO-friendly (server-rendered), accessibility (WCAG 2.1 AA)  
**Scale/Scope**: ~500 content items, 8 sections, 5 collections, 100-150 unique tags

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

### Rule #1: Test-Driven Development (TDD) ✅

**Status**: COMPLIANT

- Component tests will be written first (bUnit for Blazor components)
- E2E tests MANDATORY for all UI changes (Playwright)
- Integration tests for API endpoints (existing pattern established)
- Clean slate principle enforced (fix all tests before new work)

**Evidence**: See spec.md - Testing Strategy section defines unit, integration, component, and E2E test requirements.

### Rule #2: 10-Step Development Workflow ✅

**Status**: COMPLIANT - Followed during specification phase

1. Core Rules reviewed ✅
2. Context gathered (read AGENTS.md, existing filtering docs) ✅
3. Plan created (this document) ✅ (in progress)
4. Research validated (clarification session completed) ✅
5. Verify behavior (will use Playwright MCP for testing)
6. Tests first (TDD enforced)
7. Implementation (after tests)
8. Validate & fix (Run)
9. Update documentation (docs/filtering-system.md rewrite required)
10. Report completion

**Evidence**: Clarification session completed (5 Q&A), plan creation in progress.

### Rule #3: Domain-Specific AGENTS.md Files ✅

**Status**: COMPLIANT

**Required Reading**:

- [src/AGENTS.md](../../src/AGENTS.md) - .NET development patterns
- [src/TechHub.Web/AGENTS.md](../../src/TechHub.Web/AGENTS.md) - Blazor component patterns, Tech Hub design system
- [src/TechHub.Api/AGENTS.md](../../src/TechHub.Api/AGENTS.md) - Minimal API endpoint patterns
- [src/TechHub.Infrastructure/AGENTS.md](../../src/TechHub.Infrastructure/AGENTS.md) - Repository patterns
- [tests/TechHub.Web.Tests/AGENTS.md](../../tests/TechHub.Web.Tests/AGENTS.md) - bUnit component testing
- [tests/TechHub.E2E.Tests/AGENTS.md](../../tests/TechHub.E2E.Tests/AGENTS.md) - Playwright E2E testing

**Evidence**: Plan references domain-specific patterns (Blazor SSR, Minimal APIs, bUnit, Playwright).

### Rule #4: Configuration-Driven Development ✅

**Status**: COMPLIANT

- Section and collection configuration in `appsettings.json` (existing)
- No hardcoded section/collection data
- Filter settings configurable (date range defaults, tag thresholds)

**Evidence**: Spec references configuration-driven design (FR-001 scoping logic uses appsettings.json).

### Rule #5: Server-Side First, Client-Side Performance ✅

**Status**: COMPLIANT

- Initial page load shows complete content (SSR)
- Tag cloud pre-rendered server-side
- JavaScript enhances (filtering), doesn't create initial content
- Exception: `sections.js` allowed to modify state on load (URL parameters)

**Evidence**: Spec requires server-side rendering (FR-013: no full page reload, client-side only).

### Rule #6: Accessibility Standards ✅

**Status**: COMPLIANT

- Keyboard navigation (Tab, Space, Enter) - FR-015
- Screen reader support (ARIA labels, semantic HTML)
- Color contrast (Tech Hub design system - WCAG AA)
- Focus states (visible indicators)

**Evidence**: Spec has dedicated Accessibility Tests section, FR-015 requires keyboard accessibility.

### Rule #7: Documentation Updates ✅

**Status**: COMPLIANT - Rewrite Required

**Documentation Plan**:

1. **Functional Documentation** (docs/):
   - Rewrite `docs/filtering-system.md` (currently deleted, needs complete rewrite)
   - Document: Tag cloud scoping logic, subset matching rules, date slider behavior
   - Focus on WHAT the system does (behavior, contracts, rules)

2. **Technical Documentation** (AGENTS.md):
   - Update `src/TechHub.Web/AGENTS.md` with filtering component patterns
   - Add: SidebarTagCloud, TagDropdownFilter, DateRangeSlider component examples
   - Focus on HOW to implement (code patterns, component architecture)

3. **No Content Guidelines** (collections/):
   - Content guidelines unaffected (no markdown changes)

**Evidence**: Spec includes Documentation Rewrite Requirements section, acknowledges docs/filtering-system.md needs complete rewrite.

### ✅ CONSTITUTION CHECK PASSED

All 7 rules compliant. No violations to justify. Proceed to Phase 0 (Research & Design).

## Project Structure

### Documentation (this feature)

```text
specs/001-filtering-system/
├── spec.md              # Feature specification (complete)
├── plan.md              # This file (in progress)
├── research.md          # Phase 0 output (next step)
├── data-model.md        # Phase 1 output
├── quickstart.md        # Phase 1 output
├── contracts/           # Phase 1 output (API contracts)
│   ├── filter-request.json
│   ├── filter-response.json
│   └── tag-cloud-response.json
└── tasks.md             # Phase 2 output (/speckit.tasks command)
```

### Source Code (repository root)

```text
Tech Hub Repository Structure (Actual Paths - Updated 2026-02-03):

src/
├── TechHub.Api/                       # REST API Backend
│   ├── Endpoints/
│   │   ├── ContentEndpoints.cs       # ✅ COMPLETE: Section + tag cloud endpoints
│   │   ├── CustomPagesEndpoints.cs   # Existing
│   │   └── RssEndpoints.cs           # Existing
│   └── appsettings.json              # ✅ COMPLETE: Filter configuration
│
├── TechHub.Web/                       # Blazor Frontend
│   ├── Components/                    # Note: No Shared/ subfolder
│   │   ├── SidebarTagCloud.razor          # ✅ COMPLETE: Contextual tag cloud
│   │   ├── SidebarTagCloud.razor.cs       # ✅ COMPLETE: Code-behind
│   │   ├── SidebarTagCloud.razor.css      # ✅ COMPLETE: Scoped styles
│   │   ├── TagDropdownFilter.razor        # ❌ TODO: Excel-style dropdown
│   │   ├── DateRangeSlider.razor          # ❌ TODO: Date range selector
│   │   ├── ContentItemTagBadges.razor     # ❌ TODO: Tag badges on items
│   │   └── Pages/
│   │       ├── Section.razor                  # ✅ PARTIAL: Has tag cloud
│   │       ├── SectionCollection.razor        # TODO: Add filtering sidebar
│   │       └── ContentItem.razor              # TODO: Add tag badges
│   ├── Services/
│   │   ├── TechHubApiClient.cs       # ✅ COMPLETE: GetTagCloudAsync
│   │   ├── ITechHubApiClient.cs      # ✅ COMPLETE: Interface
│   │   └── FilterStateService.cs     # ❌ TODO: URL state management
│   └── wwwroot/
│       └── js/
│           └── filtering.js          # ❌ TODO: Client-side filter logic
│
├── TechHub.Core/                      # Domain Models & Interfaces
│   ├── Models/                        # Note: Uses Models/, not DTOs/
│   │   ├── Filter/
│   │   │   ├── FilterRequest.cs          # ✅ COMPLETE
│   │   │   ├── FilterResponse.cs         # ✅ COMPLETE
│   │   │   └── FilterSummary.cs          # ✅ COMPLETE
│   │   └── Tags/
│   │       ├── TagCloudItem.cs           # ✅ COMPLETE
│   │       ├── TagCloudRequest.cs        # ✅ COMPLETE
│   │       ├── AllTagsResponse.cs        # ✅ COMPLETE
│   │       ├── TagWithCount.cs           # ✅ COMPLETE
│   │       └── TagCountsResponse.cs      # ❌ TODO: Dynamic counts response
│   └── Interfaces/
│       ├── ITagCloudService.cs       # ✅ COMPLETE
│       ├── ITagMatchingService.cs    # ❌ TODO: Subset matching
│       └── IContentRepository.cs     # TODO: Add GetTagCountsAsync
│
└── TechHub.Infrastructure/            # Data Access Implementation
    └── Services/
        ├── TagCloudService.cs        # ✅ COMPLETE: Quantile sizing
        └── TagMatchingService.cs     # ❌ TODO: Subset matching logic

tests/
├── TechHub.Web.Tests/                # bUnit Component Tests
│   └── Components/
│       ├── SidebarTagCloudTests.cs   # ✅ COMPLETE
│       ├── TagDropdownFilterTests.cs # ❌ TODO
│       └── DateRangeSliderTests.cs   # ❌ TODO
│
├── TechHub.Api.Tests/                # Integration Tests
│   └── Endpoints/
│       ├── ContentEndpointsTests.cs  # ✅ COMPLETE: Tag cloud tests
│       └── TagCountsEndpointTests.cs # ❌ TODO: Dynamic counts
│
├── TechHub.Infrastructure.Tests/     # Unit Tests
│   └── Services/
│       ├── TagCloudServiceTests.cs   # ✅ COMPLETE
│       └── TagMatchingServiceTests.cs # ❌ TODO
│
└── TechHub.E2E.Tests/                # Playwright E2E Tests
    └── Web/
        ├── TagFilteringTests.cs      # ✅ COMPLETE: Basic tag filtering
        ├── DynamicTagCountsTests.cs  # ❌ TODO: Dynamic count tests
        └── TagCloudScopingTests.cs   # ❌ TODO: Scoping scenarios

docs/
└── filtering-system.md               # ❌ TODO: Functional documentation
```

**Structure Decision**: Tech Hub uses a clean .NET architecture with API backend, Blazor frontend, Core domain, and Infrastructure layers. Filtering components will be added to existing structure following established patterns (Minimal APIs, Blazor SSR, file-based repositories).

## Complexity Tracking

**No violations requiring justification** - Constitution check passed all 7 rules.

---

## Phase 0: Research & Outline

**Objective**: Resolve all NEEDS CLARIFICATION items and research best practices.

**Status**: ✅ COMPLETE - See [research.md](research.md) for all findings

### Research Tasks Completed

All 7 research tasks resolved with documented decisions, code samples, and rationale:

#### R1: Tag Cloud Visualization Research

**Status**: ✅ RESOLVED via Clarification Session

**Question**: How should tag sizes reflect popularity? What sizing algorithm creates clear visual hierarchy without overwhelming UI?

**Decision**: Quantile-based sizing with 3 discrete tiers:

- Large (top 25%): 5 tags - Most popular
- Medium (middle 50%): 10 tags - Popular
- Small (bottom 25%): 5 tags - Less popular

**Rationale**: Prevents excessively large tags (linear scaling issue), maintains clear visual hierarchy, simplifies implementation (discrete CSS classes vs continuous sizing).

**Alternatives Considered**:

- Linear scaling (rejected: creates large tags with similar sizes)
- Logarithmic scaling (rejected: complexity vs benefit)
- Fixed size with color gradient (rejected: less intuitive than size)

#### R2: Tag Cloud Quantity Threshold

**Status**: ✅ RESOLVED via Clarification Session

**Question**: Should tag cloud always show exactly 20 tags, or adjust based on content volume?

**Decision**: Dynamic quantity with minimum threshold - Show top 20 OR all tags with ≥5 uses, whichever is fewer.

**Rationale**: Prevents displaying rarely-used tags in small sections (better UX), caps maximum display size (prevents overwhelming UI), balances discovery vs noise.

**Alternatives Considered**:

- Fixed top 20 (rejected: shows single-use tags in small sections)
- Dynamic only (rejected: could exceed 20 tags in large sections)
- Adaptive by content count (rejected: added complexity without clear benefit)

#### R3: Virtual Scrolling Implementation

**Status**: ✅ RESOLVED via Clarification Session

**Question**: When should tag dropdown activate virtual scrolling for performance?

**Decision**: Threshold at 50 tags - Show all tags below 50, activate virtual scrolling at 50+ tags.

**Rationale**: Earlier activation (50 vs 100) provides better performance for moderately large lists, round number simplifies testing/maintenance, users won't notice virtual scrolling at this threshold (50 items fit comfortably in dropdown).

**Alternatives Considered**:

- 100 tag threshold (spec default - rejected: may cause lag with 75-99 tags)
- 25 tag threshold (rejected: too aggressive, adds complexity for small lists)
- Always virtual scroll (rejected: overkill for small tag counts)

**Implementation**: Research Blazor virtual scrolling component options (Virtualize component built-in vs third-party libraries).

#### R4: Tag Subset Matching Strategy

**Status**: ✅ RESOLVED via Clarification Session

**Question**: How should subset matching handle casing, punctuation, and special characters?

**Decision**: Case-insensitive word boundary matching with normalization (trim, lowercase, punctuation-agnostic).

**Examples**:

- "ai" matches "AI", "Generative AI", "Azure-AI", "Azure AI"
- "ai" does NOT match "AIR" (word boundary rule)

**Rationale**: Most user-friendly approach, prevents casing/punctuation issues (common UX friction), aligns with modern search interfaces (Google, Stack Overflow), simplifies implementation (normalize once, compare normalized).

**Alternatives Considered**:

- Case-sensitive exact (rejected: poor UX, "ai" won't match "AI")
- Exact match only (rejected: requires character-perfect match)
- Fuzzy matching (rejected: unpredictable results, performance cost)

**Implementation**: .NET normalization: `tag.Trim().ToLowerInvariant()`, regex for word boundaries `\b`, punctuation removal before comparison.

#### R5: Blazor State Management for Filters

**Question**: How to manage filter state between URL parameters, component state, and API calls?

**Research Required**:

- Blazor NavigationManager for URL parameter management
- Cascading parameters vs dependency injection for filter state
- Query string serialization (tags array, date range)
- Browser history integration (back/forward navigation)

**Expected Outcome**: Architecture decision for FilterStateService implementation.

#### R6: Date Range Slider Component

**Question**: Should we build custom slider or use existing Blazor component library?

**Research Required**:

- Blazor native input[type=range] capabilities (dual handles?)
- Third-party slider libraries (Radzen, MudBlazor, etc.)
- Accessibility requirements for custom controls
- Touch/mobile support for range selection

**Expected Outcome**: Component selection with accessibility assessment.

#### R7: Client-Side Filtering Performance

**Question**: How to achieve <50ms filtering with potentially 500+ items?

**Research Required**:

- JavaScript vs Blazor WebAssembly performance for filtering
- Memoization strategies for tag calculations
- Virtual scrolling integration with filtering
- Debouncing UI updates during rapid filter changes

**Expected Outcome**: Performance optimization strategy.

### Research Consolidation

**Output**: `research.md` with all decisions, rationales, and alternatives considered.

---

## Phase 1: Design & Contracts

**Status**: ✅ COMPLETE  
**Objective**: Define data models, API contracts, component architecture, and quickstart guide.

**Prerequisites**: `research.md` complete ✅

**Completed Artifacts**:

- ✅ [data-model.md](data-model.md) - All DTOs, entities, service interfaces, component hierarchy, validation rules
- ✅ [contracts/openapi.json](contracts/openapi.json) - Complete OpenAPI 3.0.3 specification with 3 endpoints
- ✅ [quickstart.md](quickstart.md) - User guide, developer guide, tester guide with examples
- ✅ Agent context updated - [.github/agents/copilot-instructions.md](/.github/agents/copilot-instructions.md) with filtering components and patterns

### D1: Data Model Design ✅

**Output**: `data-model.md`

**Entities to Define**:

1. **FilterRequest** (DTO)
   - Properties: `SelectedTags` (string[]), `DateRange` (DateRangeDto), `SectionName` (string?), `CollectionName` (string?)
   - Validation: Tag format, date range validity

2. **FilterResponse** (DTO)
   - Properties: `Items` (ContentItemDto[]), `TotalCount` (int), `AppliedFilters` (FilterSummaryDto)

3. **TagCloudItem** (DTO)
   - Properties: `Tag` (string), `Count` (int), `Size` (TagSize enum: Large/Medium/Small)

4. **DateRangeDto** (DTO)
   - Properties: `From` (DateTimeOffset?), `To` (DateTimeOffset?), `Preset` (DateRangePreset enum?)

5. **FilterState** (Client-side model)
   - Properties: Combines DTO + UI state (IsLoading, ValidationErrors)

**Validation Rules**:

- Tag names: Non-empty, max 50 chars, word boundary compatible
- Date ranges: From ≤ To, not in future
- Scoping: SectionName + CollectionName validation

### D2: API Contracts

**Output**: `contracts/` directory with JSON schemas

**Endpoints to Define**:

1. **GET /api/content/filter**
   - Query params: tags (comma-separated), dateFrom, dateTo, section, collection
   - Response: FilterResponse JSON
   - Example: `/api/content/filter?tags=ai,azure&dateFrom=2025-10-01&section=ai`

2. **GET /api/tags/cloud**
   - Query params: scope (homepage/section/collection), section?, collection?
   - Response: TagCloudItem[] JSON
   - Example: `/api/tags/cloud?scope=section&section=ai`

3. **GET /api/tags/all**
   - Query params: section?, collection?
   - Response: { tags: string[], counts: { [tag]: number } }

**OpenAPI Schema**: Update `/swagger` with filter endpoints.

### D3: Component Architecture

**Output**: Documented in `data-model.md` under "Component Hierarchy"

**Component Tree**:

```text
Section.razor / SectionCollection.razor
└── FilterSidebar.razor (NEW - container)
    ├── SidebarTagCloud.razor (NEW)
    │   ├── @onclick -> Toggle tag selection
    │   └── @emit -> TagSelected event
    ├── DateRangeSlider.razor (NEW)
    │   ├── @onchange -> Update date range
    │   └── @emit -> DateRangeChanged event
    ├── TagDropdownFilter.razor (NEW)
    │   ├── SearchBox (filter tag list)
    │   ├── Checkbox list (multi-select)
    │   └── @emit -> TagsSelected event
    └── ClearFiltersButton.razor (NEW)
        └── @onclick -> Reset all filters

ContentItemCard.razor (MODIFIED)
└── ContentItemTagBadges.razor (NEW)
    └── @onclick -> Navigate to filtered view
```

**State Flow**:

1. User interacts with filter component
2. Component emits event to FilterSidebar
3. FilterSidebar updates FilterStateService
4. FilterStateService updates URL parameters
5. URL change triggers API call (via TechHubApiClient)
6. ContentItemsGrid receives filtered results
7. UI updates with new content

### D4: Quickstart Guide

**Output**: `quickstart.md`

**Content**:

1. **For Users**: How to use tag cloud, dropdown, date slider
2. **For Developers**: How to add new filterable properties
3. **For Testers**: Key scenarios to verify (smoke tests)

### D5: Agent Context Update

**Action**: Run `.specify/scripts/powershell/update-agent-context.ps1 -AgentType copilot`

**Purpose**: Update GitHub Copilot agent context with:

- New filtering components (SidebarTagCloud, TagDropdownFilter, DateRangeSlider)
- API filter endpoints
- Tag cloud calculation logic
- Client-side filtering patterns

---

## Phase 2: Implementation Tasks

**Status**: ✅ READY - Tasks generated via `/speckit.tasks` command  
**Objective**: Execute implementation following TDD workflow with dependency-ordered tasks

**Task Breakdown**: See [tasks.md](tasks.md) for complete actionable task list

**Task Summary**:

- **Total Tasks**: 88 tasks across 12 phases
- **Distribution**: Setup (3), Foundational (17), User Stories (56), Enhancements (7), Polish (11)
- **Parallel Opportunities**: 28 tasks marked [P] can run in parallel
- **MVP Scope**: Phases 1-6 (Setup + Foundational + US1/US2/US6/US8) = Core filtering functionality

**Implementation Approach**:

1. **TDD Mandatory**: Write tests FIRST for each user story, ensure they FAIL
2. **User Story Organization**: Each story independently implementable and testable
3. **Dependency Order**: Foundational phase BLOCKS all user stories, then stories can proceed in priority order (P1 → P2 → P3)
4. **Incremental Delivery**: Each completed user story adds value without breaking previous stories
5. **Constitution Compliance**: All 7 rules verified, documented, and enforced

**Next Steps**:

1. ✅ Phase 0: Research complete → [research.md](research.md)
2. ✅ Phase 1: Design complete → [data-model.md](data-model.md), [contracts/openapi.json](contracts/openapi.json), [quickstart.md](quickstart.md)
3. ✅ Phase 2: Tasks generated → [tasks.md](tasks.md)
4. ⏭️ Begin implementation: Execute tasks following TDD workflow

---

## Phase 2 Preview: Task Organization

**Phase 1 - Setup** (3 tasks):

- Configuration updates (appsettings.json)
- CSS and JavaScript file creation

**Phase 2 - Foundational** (17 tasks) - BLOCKS ALL USER STORIES:

- DTOs (FilterRequest, FilterResponse, TagCloudItem, etc.)
- Service interfaces (ITagCloudService, ITagMatchingService)
- Service implementations (TagCloudService, TagMatchingService)
- Repository modifications (FilterAsync method)

**Phase 3 - User Story 1** (13 tasks) - Sidebar Tag Cloud 🎯 MVP:

- Tests: Unit (2), Component (1), Integration (1), E2E (2)
- Implementation: API endpoints (1), Blazor components (2), API client (1), Page integration (2)

**Phase 4 - User Story 2** (8 tasks) - Date Range Filtering:

- Tests: Component (1), Integration (1), E2E (1)
- Implementation: Blazor components (2), Page integration (2)

**Phase 5 - User Story 6** (3 tasks) - Subset Matching Validation:

- Tests: Unit verification (1), E2E (1), Validation (1)

**Phase 6 - User Story 8** (7 tasks) - Excel Dropdown:

- Tests: Component (1), E2E (1)
- Implementation: Blazor components (2), Page integration (2)

**Phase 7-10** - Additional User Stories (P2/P3):

- US7: Interactive Slider (3 tasks)
- US3: Clear Filters (5 tasks)
- US4: Active Indicators (7 tasks)
- US5: Browser Navigation (4 tasks)

**Phase 11** - Content Item Tag Badges (7 tasks):

- Tests: Component (1), E2E (1)
- Implementation: Components (2), Page integration (2)

**Phase 12** - Polish & Cross-Cutting (11 tasks):

- Documentation (1), Performance (3), Accessibility (1), Code Quality (3), Validation (3)

---

## Task Generation Workflow (COMPLETED)

**Executed by `/speckit.tasks` command**:

- ✅ Extracted all NEW, MODIFY annotations from source tree
- ✅ Created dependency-ordered tasks (Core → Infrastructure → API → Web → Tests)
- ✅ Mapped each component/endpoint to specific test requirements
- ✅ Defined acceptance criteria from spec.md scenarios
- ✅ Organized tasks by user story for independent implementation
- ✅ Identified 28 parallel execution opportunities
- ✅ Validated TDD workflow (tests before implementation)
- ✅ Estimated complexity: 88 tasks total across 12 phases

---

## Re-evaluation: Constitution Check (Post-Design)

**Status**: ✅ COMPLETE - All 7 constitutional rules verified against final Phase 1 design

### Rule #1: 10-Step Workflow ✅

**Verification**: Phase 1 artifacts follow all 10 steps:

1. ✅ Core Rules reviewed (TDD, MCP tools, documentation)
2. ✅ Context gathered (spec.md, AGENTS.md, existing codebase patterns)
3. ✅ Plan created (this document with constitution check)
4. ✅ Research completed (research.md with 7 technology decisions)
5. ⏭️ Verify current behavior (will be done during implementation)
6. ⏭️ Write tests first (TDD - tasks will include test-first approach)
7. ⏭️ Implement changes (tasks generated in Phase 2)
8. ⏭️ Validate & fix (tasks include test execution)
9. ⏭️ Update documentation (quickstart.md, functional docs)
10. ⏭️ Report completion (after implementation)

**Design Compliance**: Data models, contracts, and quickstart guide follow 10-step workflow structure.

### Rule #2: Test-Driven Development ✅

**Verification**: Comprehensive test coverage defined in data-model.md:

- **Unit Tests**: TagCloudService, TagMatchingService (Infrastructure.Tests)
- **Integration Tests**: Filter endpoints, tag cloud endpoints (Api.Tests)
- **Component Tests**: FilterSidebar, SidebarTagCloud, TagDropdownFilter, DateRangeSelector (Web.Tests)
- **E2E Tests**: Complete user workflows - tag filtering, date filtering, combined filtering, clear filters (E2E.Tests)

**Design Compliance**: quickstart.md includes test-first examples for developers, all components have corresponding test specifications.

### Rule #3: MCP Tools Over CLI ✅

**Verification**: Implementation design uses appropriate tool hierarchy:

- **Phase 0 Research**: Used context7 MCP for Blazor/ASP.NET documentation
- **Implementation Plan**: Uses built-in Blazor components (NavigationManager, Virtualize) - no CLI tools needed
- **Testing**: Playwright E2E tests will use browser automation (high-level)

**Design Compliance**: No unnecessary CLI dependencies, built-in .NET/Blazor tools preferred.

### Rule #4: Configuration-Driven Design ✅

**Verification**: Filtering system integrates with existing configuration:

- **Tag Cloud Defaults**: Configurable in `appsettings.json` (max tags, date range days)
- **Virtual Scrolling Threshold**: Configurable (default 50)
- **Quantile Percentiles**: Configurable (default 25/75 for Large/Medium/Small)
- **Date Presets**: Configurable list (7/30/90 days, all time)

**Design Compliance**: data-model.md includes configuration properties, no hardcoded filter values.

### Rule #5: Server-Side First, Client Performance ✅

**Verification**: Architecture follows SSR-first principle:

- **Initial Render**: All content server-side rendered (Blazor SSR)
- **Filter State**: URL-driven state via [SupplyParameterFromQuery]
- **JavaScript Enhancement**: Only for performance (filtering large datasets client-side)
- **Performance Budget**: <50ms client-side filtering, <200ms API response

**Design Compliance**: Component hierarchy in data-model.md shows SSR components with progressive enhancement.

### Rule #6: Accessibility (WCAG 2.1 AA) ✅

**Verification**: Comprehensive accessibility requirements:

- **Keyboard Navigation**: All tags focusable, Space/Enter to select, Arrow keys in dropdown
- **Screen Reader**: ARIA labels for all controls, state announcements (selected/expanded)
- **Focus Indicators**: Visible focus states defined for all interactive elements
- **Semantic HTML**: Button elements for tags, native select semantics for dropdown
- **Color Contrast**: Tag sizes use text size AND color, not color alone

**Design Compliance**: quickstart.md includes accessibility testing section, E2E tests will verify keyboard navigation and screen reader compatibility.

### Rule #7: Documentation Updates (Post-Design) ✅

**Verification**: All documentation artifacts created:

- ✅ **Functional Documentation**: quickstart.md with user/developer/tester guides
- ✅ **API Documentation**: contracts/openapi.json with complete endpoint specifications
- ✅ **Technical Documentation**: data-model.md with DTOs, interfaces, component hierarchy
- ✅ **Research Documentation**: research.md with all technology decisions and rationales
- ✅ **Agent Context**: .github/agents/copilot-instructions.md updated with filtering components

**Design Compliance**: Documentation rewrite ensures filtering system is fully documented before implementation begins.

---

**Constitution Compliance Summary**: ✅ ALL 7 RULES VERIFIED

**Final Assessment**: Phase 1 design artifacts comply with all constitutional requirements. Ready to proceed to Phase 2 task generation via `/speckit.tasks` command.

---

## Dependencies

### Completed (Existing)

- ✅ API filtering endpoints (`GET /api/content/filter`) - Basic implementation exists
- ✅ Content repository with tag and date filtering logic - File-based repository established
- ✅ Blazor component architecture for state management - SSR + WebAssembly foundation

### In Progress (Current .NET Migration)

- 🔄 URL state management - NavigationManager available, need FilterStateService
- 🔄 Tech Hub design system - Colors/typography defined, need filter component styles

### Needed (This Feature)

- 📋 Tag cloud visual design (3-tier sizing, scoped display)
- 📋 Excel-style tag dropdown design (search, checkboxes, virtual scrolling)
- 📋 Date range slider design (dual handles, accessibility)
- 📋 Tag cloud calculation logic (most-used tags in last 3 months, scoping)
- 📋 Virtual scrolling component (Blazor Virtualize or third-party)
- 📋 Tag subset matching implementation (normalized word boundaries)
- 📋 Tag badges component design (content item tag display)

### Future Integration

- 🔮 Infinite scroll integration (003-infinite-scroll spec) - Filter must work with lazy loading
- 🔮 Text search filtering (002-search spec) - Combined with tag/date filters
- 🔮 Analytics tracking (006-google-analytics spec) - Track filter usage

---

## Out of Scope

- Text search filtering (covered in 002-search spec)
- Infinite scroll pagination (covered in 003-infinite-scroll spec)
- Advanced filtering (AND logic within tags, exclude filters, etc.)
- Filter analytics tracking (covered in 006-google-analytics spec)
- Saved filter presets or user preferences
- Tag popularity indicators beyond tag cloud sizing
- Server-side search (client-side filtering only)

---

## Success Metrics

**From spec.md Success Criteria section**:

- **SC-001**: Users can apply tag filters and see results update within 50ms ✅ Measurable via Playwright timing
- **SC-002**: Users can combine tag and date filters with accurate results ✅ E2E test coverage
- **SC-003**: Filter state is shareable via URL ✅ E2E test: copy URL, open in new tab, verify state
- **SC-004**: All filter controls are keyboard accessible (100% Tab/Space/Enter navigation) ✅ Accessibility tests
- **SC-005**: Browser back/forward navigation works correctly with filter state ✅ E2E test coverage
- **SC-006**: Zero console errors or warnings during filter operations ✅ E2E test validation

**Performance Targets**:

- Client-side filtering: <50ms from selection to UI update
- API filter endpoint: <200ms p95 response time
- Tag cloud calculation: <100ms for 500 items
- Virtual scrolling: Smooth 60fps scroll with 100+ tags

**Quality Targets**:

- Unit test coverage: 80%+ for tag services
- Integration test coverage: 100% for filter endpoints
- Component test coverage: 100% for filter components
- E2E test coverage: 100% for user scenarios (all 10 stories)

---

## Notes

**Key Design Decisions**:

1. **Quantile-based tag sizing**: Balances visual hierarchy with usability
2. **Dynamic tag quantity**: Prevents rare tags, caps display size
3. **Virtual scrolling at 50 tags**: Earlier activation for better performance
4. **Normalized subset matching**: User-friendly, handles casing/punctuation
5. **Client-side filtering only**: Fast response, SEO via server-rendered initial content

**Risk Mitigation**:

- **Performance**: Virtual scrolling, memoization, debouncing
- **Accessibility**: Keyboard nav, ARIA labels, focus management
- **Browser Compatibility**: Blazor WebAssembly fallback, progressive enhancement
- **Data Consistency**: URL as single source of truth for filter state

**Next Actions**:

1. Complete Phase 0 research (virtual scrolling options, state management architecture)
2. Generate data-model.md (entities, validation, component hierarchy)
3. Generate API contracts (OpenAPI schemas for filter endpoints)
4. Generate quickstart.md (user guide, developer guide, test guide)
5. Re-evaluate Constitution Check with final design
6. Run `/speckit.tasks` to generate implementation tasks
