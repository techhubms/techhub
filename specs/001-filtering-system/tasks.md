# Tasks: Sidebar Content Filtering

**Input**: Design documents from `/specs/001-filtering-system/`  
**Prerequisites**: plan.md ✅, spec.md ✅, research.md ✅, data-model.md ✅, contracts/ ✅, quickstart.md ✅

**Tests**: Tests included for all user stories (TDD required per constitution)

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and configuration updates

- [ ] T001 [P] Add filter configuration to src/TechHub.Api/appsettings.json (tag cloud defaults, virtual scroll threshold, quantile percentiles, date presets)
- [X] T002 [P] Create component-scoped CSS for filtering components using Blazor CSS isolation (.razor.css files co-located with components: SidebarTagCloud.razor.css, TagDropdownFilter.razor.css, DateRangeSlider.razor.css)
- [ ] T003 [P] Create JavaScript file for client-side filtering in src/TechHub.Web/wwwroot/js/filtering.js

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core infrastructure that MUST be complete before ANY user story can be implemented

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

### DTOs and Core Models

- [ ] T004 [P] Create FilterRequest DTO in src/TechHub.Core/DTOs/FilterRequest.cs
- [ ] T005 [P] Create FilterResponse DTO in src/TechHub.Core/DTOs/FilterResponse.cs
- [ ] T006 [P] Create FilterSummaryDto in src/TechHub.Core/DTOs/FilterSummaryDto.cs
- [X] T007 [P] Create TagCloudItem DTO in src/TechHub.Core/DTOs/TagCloudItem.cs
- [X] T008 [P] Create TagCloudRequest DTO in src/TechHub.Core/DTOs/TagCloudRequest.cs
- [ ] T009 [P] Create AllTagsResponse DTO in src/TechHub.Core/DTOs/AllTagsResponse.cs
- [ ] T010 [P] Create TagWithCount DTO in src/TechHub.Core/DTOs/TagWithCount.cs
- [ ] T011 [P] Create DateRangePreset enum in src/TechHub.Core/Models/DateRangePreset.cs

### Service Interfaces

- [X] T012 Create ITagCloudService interface in src/TechHub.Core/Interfaces/ITagCloudService.cs
- [ ] T013 Create ITagMatchingService interface in src/TechHub.Core/Interfaces/ITagMatchingService.cs
- [ ] T014 Modify IContentRepository interface to add FilterAsync method in src/TechHub.Core/Interfaces/IContentRepository.cs

### Service Implementations

- [X] T015 Implement TagCloudService in src/TechHub.Infrastructure/Services/TagCloudService.cs (quantile-based sizing, dynamic quantity, scoping logic)
- [ ] T016 Implement TagMatchingService in src/TechHub.Infrastructure/Services/TagMatchingService.cs (normalized subset matching, word boundaries)
- [ ] T017 Modify FileBasedContentRepository to implement FilterAsync method in src/TechHub.Infrastructure/Repositories/FileBasedContentRepository.cs

### Service Registration

- [ ] T018 Register ITagCloudService and ITagMatchingService in src/TechHub.Api/Program.cs
- [ ] T019 Create FilterStateService in src/TechHub.Web/Services/FilterStateService.cs (client state management, URL parameters)
- [ ] T020 Register FilterStateService in src/TechHub.Web/Program.cs

**Checkpoint**: Foundation ready - user story implementation can now begin in parallel

---

## Phase 3: User Story 1 - Filter via Sidebar Tag Cloud (Priority: P1) 🎯 MVP

**Goal**: Users can click tags in the sidebar tag cloud (top 20 scoped tags, last 3 months) to filter content with OR logic

**Independent Test**: Navigate to section page, verify tag cloud shows top 20 tags for that section, click tag, verify content filters

### Tests for User Story 1 (TDD - Write FIRST) ⚠️

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

**Unit Tests**:

- [ ] T021 [P] [US1] Create TagCloudServiceTests in tests/TechHub.Infrastructure.Tests/Services/TagCloudServiceTests.cs
  - Test quantile sizing (Large/Medium/Small tiers)
  - Test dynamic quantity (top 20 OR all ≥5 uses)
  - Test scoping (homepage/section/collection)
  - Test last 3 months filtering

- [ ] T022 [P] [US1] Create TagMatchingServiceTests in tests/TechHub.Infrastructure.Tests/Services/TagMatchingServiceTests.cs
  - Test normalized subset matching
  - Test word boundary detection
  - Test case-insensitive matching
  - Test punctuation handling

**Component Tests**:

- [X] T023 [P] [US1] Create SidebarTagCloudTests in tests/TechHub.Web.Tests/Components/SidebarTagCloudTests.cs
  - Test tag cloud renders with correct tags
  - Test tag selection toggles state
  - Test selected tags are highlighted
  - Test tag click updates URL parameters
  - Test URL parameters restore tag selection

**Integration Tests**:

- [ ] T024 [P] [US1] Create FilterEndpointsTests in tests/TechHub.Api.Tests/Endpoints/FilterEndpointsTests.cs
  - Test GET /api/content/filter with single tag
  - Test GET /api/content/filter with multiple tags (OR logic)
  - Test GET /api/tags/cloud with different scopes
  - Test tag subset matching via API

**E2E Tests**:

- [ ] T025 [P] [US1] Create FilteringTests in tests/TechHub.E2E.Tests/FilteringTests.cs
  - Test navigate to section page, click tag, verify content filters
  - Test select multiple tags, verify OR logic
  - Test deselect tag, verify content updates
  - Test URL includes tags parameter
  - Test share URL with tags, recipient sees same view

- [ ] T026 [P] [US1] Create TagCloudScopingTests in tests/TechHub.E2E.Tests/TagCloudScopingTests.cs
  - Test homepage shows top 20 global tags
  - Test section page shows top 20 section tags
  - Test collection page shows top 20 collection tags
  - Test content item shows only article tags

### Implementation for User Story 1

**API Endpoints**:

- [ ] T027 [US1] Create FilterEndpoints.cs in src/TechHub.Api/Endpoints/FilterEndpoints.cs
  - Implement GET /api/content/filter endpoint
  - Implement GET /api/tags/cloud endpoint
  - Implement GET /api/tags/all endpoint
  - Map endpoints in Program.cs

**Blazor Components**:

- [X] T028 [P] [US1] Create SidebarTagCloud.razor in src/TechHub.Web/Components/Shared/SidebarTagCloud.razor (contextual tag cloud with selection)
- [X] T029 [P] [US1] Create SidebarTagCloud.razor.cs code-behind in src/TechHub.Web/Components/Shared/SidebarTagCloud.razor.cs

**API Client**:

- [X] T030 [US1] Modify TechHubApiClient.cs to add FilterContentAsync, GetTagCloudAsync, GetAllTagsAsync methods in src/TechHub.Web/Services/TechHubApiClient.cs

**Page Integration**:

- [X] T031 [US1] Modify Section.razor to add SidebarTagCloud component in src/TechHub.Web/Components/Pages/Section.razor
- [ ] T032 [US1] Modify Section.razor.cs to handle filter state and URL parameters in src/TechHub.Web/Components/Pages/Section.razor.cs

**Validation**:

- [ ] T033 [US1] Run all User Story 1 tests and verify they pass (unit, component, integration, E2E)

**Checkpoint**: At this point, User Story 1 should be fully functional - users can filter content via sidebar tag cloud with URL state preservation

---

## Phase 4: User Story 2 - Filter Content by Date Range (Priority: P1)

**Goal**: Users can select date ranges (Last 7/30/90 days, All Time, custom range) to filter content by publication date, defaults to last 3 months

**Independent Test**: Navigate to section page, verify slider defaults to last 3 months, click "Last 30 days" filter, verify only recent content displays

### Tests for User Story 2 (TDD - Write FIRST) ⚠️

**Component Tests**:

- [ ] T034 [P] [US2] Create DateRangeSliderTests in tests/TechHub.Web.Tests/Components/DateRangeSliderTests.cs
  - Test slider defaults to last 3 months
  - Test preset buttons update date range
  - Test custom range selection
  - Test date range updates URL parameters
  - Test URL parameters restore slider position
  - Test invalid date ranges show error

**Integration Tests**:

- [ ] T035 [P] [US2] Add date filtering tests to FilterEndpointsTests in tests/TechHub.Api.Tests/Endpoints/FilterEndpointsTests.cs
  - Test GET /api/content/filter with date range
  - Test GET /api/content/filter with date range + tags (AND logic)

**E2E Tests**:

- [ ] T036 [P] [US2] Add date filtering tests to FilteringTests in tests/TechHub.E2E.Tests/FilteringTests.cs
  - Test navigate to page, verify slider defaults to last 3 months
  - Test click "Last 30 days" preset, verify content updates
  - Test drag slider handles, verify custom range
  - Test extend range backward to older content
  - Test combine date range + tags, verify AND logic
  - Test URL includes from/to parameters
  - Test share URL with custom date range, recipient sees same slider position

### Implementation for User Story 2

**Blazor Components**:

- [ ] T037 [P] [US2] Create DateRangeSlider.razor in src/TechHub.Web/Components/Shared/DateRangeSlider.razor (preset buttons + custom slider)
- [ ] T038 [P] [US2] Create DateRangeSlider.razor.cs code-behind in src/TechHub.Web/Components/Shared/DateRangeSlider.razor.cs

**Page Integration**:

- [ ] T039 [US2] Modify Section.razor to add DateRangeSlider component in src/TechHub.Web/Components/Pages/Section.razor
- [ ] T040 [US2] Modify Section.razor.cs to handle date range state and URL parameters in src/TechHub.Web/Components/Pages/Section.razor.cs

**Validation**:

- [ ] T041 [US2] Run all User Story 2 tests and verify they pass (component, integration, E2E)

**Checkpoint**: At this point, User Stories 1 AND 2 should both work independently - users can filter by tags OR date range OR both

---

## Phase 5: User Story 6 - Filter with Tag Subset Matching (Priority: P1)

**Goal**: Intelligent tag matching where selecting a tag shows all content with that tag OR tags that contain it as complete words

**Independent Test**: Navigate to section page, click "AI" tag, verify results include items tagged with "AI", "Generative AI", "Azure AI", etc.

**NOTE**: Implementation already included in Phase 2 (TagMatchingService) and Phase 3 (FilterEndpoints), this phase focuses on testing and validation

### Tests for User Story 6 (TDD - Verify PASS) ⚠️

**Unit Tests** (Already created in T022):

- [ ] T042 [US6] Verify TagMatchingServiceTests cover all subset matching scenarios
  - "AI" matches "AI", "Generative AI", "Azure AI" (but NOT "AIR")
  - "Visual Studio" matches "Visual Studio", "Visual Studio Code"
  - "Azure" matches "Azure", "Azure DevOps", "Azure Functions"

**E2E Tests**:

- [ ] T043 [P] [US6] Add subset matching tests to FilteringTests in tests/TechHub.E2E.Tests/FilteringTests.cs
  - Test select "AI" tag, verify results include "Generative AI" items
  - Test select "Azure" tag, verify results include "Azure DevOps" items
  - Test subset matching uses word boundaries (NOT partial string match)
  - Test combine subset-matched tags with date filters (AND logic)

**Validation**:

- [ ] T044 [US6] Run all User Story 6 tests and verify subset matching works correctly

**Checkpoint**: Subset matching is fully functional and tested

---

## Phase 6: User Story 8 - Excel-Style Tag Dropdown Filter (Priority: P1)

**Goal**: Users can filter by tags using Excel-style dropdown with search, checkboxes, virtual scrolling at 50+ tags

**Independent Test**: Navigate to section page, open tag dropdown below date slider, search for tag, select checkbox, verify content filters

### Tests for User Story 8 (TDD - Write FIRST) ⚠️

**Component Tests**:

- [ ] T045 [P] [US8] Create TagDropdownFilterTests in tests/TechHub.Web.Tests/Components/TagDropdownFilterTests.cs
  - Test dropdown renders with button and list
  - Test search box filters tag list
  - Test checkbox selection updates state
  - Test tag counts display correctly
  - Test virtual scrolling activates at 50+ tags
  - Test tag selection synchronizes with sidebar tag cloud
  - Test "Select All" / "Clear All" buttons

**E2E Tests**:

- [ ] T046 [P] [US8] Add Excel dropdown tests to FilteringTests in tests/TechHub.E2E.Tests/FilteringTests.cs
  - Test open dropdown, verify all tags display with counts
  - Test search in dropdown, verify filtering works
  - Test select tags via checkboxes, verify content updates (OR logic)
  - Test dropdown selections sync with sidebar tag cloud
  - Test keyboard navigation (Tab, Space, Arrow keys)
  - Test touch interaction on mobile

### Implementation for User Story 8

**Blazor Components**:

- [ ] T047 [P] [US8] Create TagDropdownFilter.razor in src/TechHub.Web/Components/Shared/TagDropdownFilter.razor (searchable dropdown with checkboxes)
- [ ] T048 [P] [US8] Create TagDropdownFilter.razor.cs code-behind in src/TechHub.Web/Components/Shared/TagDropdownFilter.razor.cs

**Page Integration**:

- [ ] T049 [US8] Modify Section.razor to add TagDropdownFilter component below DateRangeSlider in src/TechHub.Web/Components/Pages/Section.razor
- [ ] T050 [US8] Modify Section.razor.cs to synchronize dropdown with tag cloud state in src/TechHub.Web/Components/Pages/Section.razor.cs

**Validation**:

- [ ] T051 [US8] Run all User Story 8 tests and verify they pass (component, E2E)

**Checkpoint**: Excel-style tag dropdown is fully functional and synchronized with sidebar tag cloud

---

## Phase 7: User Story 7 - Interactive Date Range Slider (Priority: P2)

**Goal**: Users can select custom date ranges using interactive slider with from-to controls, defaults to last 3 months

**Independent Test**: Navigate to section page, verify slider defaults to last 3 months, drag handles to adjust range, verify content filters

**NOTE**: Core slider implementation already included in Phase 4 (User Story 2), this phase enhances with interactive handles and custom range UI

### Tests for User Story 7 (TDD - Enhance Existing) ⚠️

**Component Tests** (Already created in T034):

- [ ] T052 [US7] Verify DateRangeSliderTests cover all interactive slider scenarios
  - Test draggable handles (from and to dates)
  - Test extend range backward (no limit)
  - Test date range display (e.g., "Oct 16, 2024 - Jan 16, 2026")
  - Test debounced URL updates during drag

**E2E Tests** (Already created in T036):

- [ ] T053 [US7] Verify FilteringTests cover all slider interaction scenarios
  - Test drag "from" handle backward to extend range
  - Test drag "to" handle to adjust end date
  - Test smooth interaction and visual feedback
  - Test combine slider with infinite scroll (content loads within range)

**Validation**:

- [ ] T054 [US7] Run all User Story 7 tests and verify interactive slider works correctly

**Checkpoint**: Interactive date range slider is fully functional with draggable handles

---

## Phase 8: User Story 3 - Clear All Filters (Priority: P2)

**Goal**: Users can quickly reset all active filters to see all content again

**Independent Test**: Apply multiple filters, click "Clear All" button, verify all content displays and URL resets

### Tests for User Story 3 (TDD - Write FIRST) ⚠️

**Component Tests**:

- [ ] T055 [P] [US3] Add clear filters tests to existing component tests
  - Test SidebarTagCloud clear button resets tag selection
  - Test DateRangeSlider clear resets to default (last 3 months)
  - Test TagDropdownFilter clear unchecks all checkboxes

**E2E Tests**:

- [ ] T056 [P] [US3] Add clear filters tests to FilteringTests in tests/TechHub.E2E.Tests/FilteringTests.cs
  - Test apply multiple filters (tags + date range)
  - Test click "Clear All Filters" button
  - Test verify all filters reset and all content displays
  - Test verify URL parameters removed

### Implementation for User Story 3

**Blazor Components**:

- [ ] T057 [US3] Add "Clear All Filters" button to filtering sidebar
- [ ] T058 [US3] Implement clear filters logic in FilterStateService

**Validation**:

- [ ] T059 [US3] Run all User Story 3 tests and verify they pass (component, E2E)

**Checkpoint**: Clear All Filters button is functional

---

## Phase 9: User Story 4 - See Active Filter Indicators (Priority: P2)

**Goal**: Users can see which filters are currently active with visual badges and result counts

**Independent Test**: Apply filters, verify active tags are highlighted, result count updates, badges display

### Tests for User Story 4 (TDD - Write FIRST) ⚠️

**Component Tests**:

- [ ] T060 [P] [US4] Add active filter indicator tests to existing component tests
  - Test selected tags are visually highlighted
  - Test result count displays (e.g., "Showing 15 results")
  - Test active filter badges/chips display
  - Test "No results found" message when zero items match

**E2E Tests**:

- [ ] T061 [P] [US4] Add active filter tests to FilteringTests in tests/TechHub.E2E.Tests/FilteringTests.cs
  - Test select tag, verify highlighted in sidebar
  - Test verify result count updates
  - Test verify active filter badges display
  - Test apply filters with zero results, verify "No results found" message and "Clear Filters" suggestion

### Implementation for User Story 4

**Blazor Components**:

- [ ] T062 [US4] Add visual highlighting to selected tags in SidebarTagCloud.razor
- [ ] T063 [US4] Add result count display to Section.razor
- [ ] T064 [US4] Add active filter badges to filtering sidebar
- [ ] T065 [US4] Add "No results found" message with clear filters button

**Validation**:

- [ ] T066 [US4] Run all User Story 4 tests and verify they pass (component, E2E)

**Checkpoint**: Active filter indicators are fully functional

---

## Phase 10: User Story 5 - Navigate Filter State with Browser (Priority: P3)

**Goal**: Users can use browser back/forward buttons to navigate through filter states

**Independent Test**: Apply filters, use browser back button, verify previous filter state restores

### Tests for User Story 5 (TDD - Write FIRST) ⚠️

**E2E Tests**:

- [ ] T067 [P] [US5] Add browser navigation tests to FilteringTests in tests/TechHub.E2E.Tests/FilteringTests.cs
  - Test apply filters, click browser back, verify previous state restores
  - Test navigate through filter history with back/forward buttons
  - Test verify page does not fully reload (client-side only)

### Implementation for User Story 5

**Blazor Components**:

- [ ] T068 [US5] Implement browser history integration in FilterStateService
- [ ] T069 [US5] Add NavigationManager event handlers for history navigation

**Validation**:

- [ ] T070 [US5] Run all User Story 5 tests and verify they pass (E2E)

**Checkpoint**: Browser back/forward navigation works correctly with filter state

---

## Phase 11: Content Item Tag Badges (Enhancement)

**Goal**: Display tag badges on content item cards in lists, enable navigation to filtered views

**Independent Test**: View section page with content items, verify tag badges display, click tag, verify navigation to filtered view

### Tests for Content Item Tag Badges (TDD - Write FIRST) ⚠️

**Component Tests**:

- [ ] T071 [P] [US9] Create ContentItemTagBadgesTests in tests/TechHub.Web.Tests/Components/ContentItemTagBadgesTests.cs
  - Test renders subset of item tags (first 3-5)
  - Test shows "+N more" indicator if many tags
  - Test tag click navigates to filtered view
  - Test clicked tag sets active in sidebar cloud and dropdown

**E2E Tests**:

- [ ] T072 [P] [US9] Add content item tag tests to FilteringTests in tests/TechHub.E2E.Tests/FilteringTests.cs
  - Test click tag on content item card
  - Test verify navigation to filtered view with that tag
  - Test verify tag is highlighted in sidebar and dropdown

### Implementation for Content Item Tag Badges

**Blazor Components**:

- [ ] T073 [P] [US9] Create ContentItemTagBadges.razor in src/TechHub.Web/Components/Shared/ContentItemTagBadges.razor
- [ ] T074 [P] [US9] Create ContentItemTagBadges.razor.cs code-behind in src/TechHub.Web/Components/Shared/ContentItemTagBadges.razor.cs

**Page Integration**:

- [ ] T075 [US9] Modify ContentItem.razor to add tag badges in src/TechHub.Web/Components/Pages/ContentItem.razor
- [ ] T076 [US9] Modify SectionCollection.razor to add tag badges on item cards in src/TechHub.Web/Components/Pages/SectionCollection.razor

**Validation**:

- [ ] T077 [US9] Run all Content Item Tag Badge tests and verify they pass (component, E2E)

**Checkpoint**: Content item tag badges are functional and enable navigation to filtered views

---

## Phase 12: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories

### Documentation

- [ ] T078 [P] Create NEW functional documentation in docs/filtering-system.md
  - Document filter behavior (OR/AND logic)
  - Document subset matching logic
  - Document date range slider interaction
  - Document URL state management
  - Document performance characteristics
  - Document edge cases and error handling
  - Document visual feedback patterns

### Performance Optimization

- [ ] T079 [P] Optimize client-side filtering performance (<50ms target)
- [ ] T080 [P] Implement debouncing for search and slider interactions
- [ ] T081 [P] Verify virtual scrolling performance at 50+ tags

### Accessibility

- [ ] T082 [P] Run accessibility audit (WCAG 2.1 AA compliance)
  - Verify keyboard navigation works (Tab, Space, Enter, Arrow keys)
  - Verify screen reader support (ARIA labels)
  - Verify focus indicators are visible
  - Verify color contrast meets standards

### Code Quality

- [ ] T083 Code cleanup and refactoring across all filtering components
- [ ] T084 [P] Add comprehensive code comments for complex logic (quantile sizing, subset matching)
- [ ] T085 Security review (input validation, XSS prevention in tags)

### Validation

- [ ] T086 Run quickstart.md validation (all user/developer/tester scenarios)
- [ ] T087 Run full test suite (unit, integration, component, E2E) and verify 100% pass rate
- [ ] T088 Validate constitution compliance (all 7 rules followed)

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories
- **User Stories (Phase 3+)**: All depend on Foundational phase completion
  - US1 (Phase 3) → US2 (Phase 4) → US6 (Phase 5) → US8 (Phase 6) → US7 (Phase 7) → US3 (Phase 8) → US4 (Phase 9) → US5 (Phase 10)
  - Priority order: P1 stories first (US1, US2, US6, US8), then P2 (US7, US3, US4), then P3 (US5)
- **Enhancements (Phase 11)**: Can start after US1 (Phase 3) is complete
- **Polish (Phase 12)**: Depends on all desired user stories being complete

### User Story Dependencies

- **User Story 1 (P1)** - Filter via Sidebar Tag Cloud: Can start after Foundational (Phase 2) - No dependencies on other stories
- **User Story 2 (P1)** - Filter by Date Range: Can start after Foundational (Phase 2) - Integrates with US1 but independently testable
- **User Story 6 (P1)** - Tag Subset Matching: Implementation in Phase 2, testing in Phase 5 after US1
- **User Story 8 (P1)** - Excel Dropdown: Can start after US1 (Phase 3) for state synchronization
- **User Story 7 (P2)** - Interactive Slider: Enhances US2 (Phase 4)
- **User Story 3 (P2)** - Clear Filters: Can start after US1, US2 (requires both tag and date filters)
- **User Story 4 (P2)** - Active Indicators: Can start after US1 (requires filtering to be functional)
- **User Story 5 (P3)** - Browser Navigation: Can start after US1, US2 (requires filter state management)

### Within Each User Story

- Tests MUST be written and FAIL before implementation (TDD)
- DTOs before services
- Service interfaces before implementations
- Services before endpoints
- Endpoints before components
- Components before page integration
- Story complete before moving to next priority

### Parallel Opportunities

- **Phase 1**: All Setup tasks (T001-T003) can run in parallel
- **Phase 2**: All DTOs (T004-T011) can run in parallel, service tests can run in parallel with implementations
- **Within US1 Tests**: T021-T026 can run in parallel (different test files)
- **Within US1 Implementation**: T028-T029 can run in parallel (component and code-behind)
- **Within US2 Tests**: T034-T036 can run in parallel
- **Within US2 Implementation**: T037-T038 can run in parallel
- **Phase 12 Polish**: Most tasks (T078-T085) can run in parallel

---

## Parallel Example: User Story 1

```bash
# Launch all tests for User Story 1 together (TDD - write first):
Task: "Create TagCloudServiceTests in tests/TechHub.Infrastructure.Tests/Services/TagCloudServiceTests.cs"
Task: "Create TagMatchingServiceTests in tests/TechHub.Infrastructure.Tests/Services/TagMatchingServiceTests.cs"
Task: "Create SidebarTagCloudTests in tests/TechHub.Web.Tests/Components/SidebarTagCloudTests.cs"
Task: "Create FilterEndpointsTests in tests/TechHub.Api.Tests/Endpoints/FilterEndpointsTests.cs"
Task: "Create FilteringTests in tests/TechHub.E2E.Tests/FilteringTests.cs"
Task: "Create TagCloudScopingTests in tests/TechHub.E2E.Tests/TagCloudScopingTests.cs"

# Verify ALL tests fail (proves they test the right thing)

# Launch component and code-behind together:
Task: "Create SidebarTagCloud.razor in src/TechHub.Web/Components/Shared/SidebarTagCloud.razor"
Task: "Create SidebarTagCloud.razor.cs in src/TechHub.Web/Components/Shared/SidebarTagCloud.razor.cs"
```

---

## Implementation Strategy

### MVP First (User Stories 1, 2, 6, 8 - All P1)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational (CRITICAL - blocks all stories)
3. Complete Phase 3: User Story 1 (Sidebar Tag Cloud)
4. Complete Phase 4: User Story 2 (Date Range Filtering)
5. Complete Phase 5: User Story 6 (Subset Matching - validation)
6. Complete Phase 6: User Story 8 (Excel Dropdown)
7. **STOP and VALIDATE**: Test all P1 stories together, verify filtering is fully functional
8. Deploy/demo if ready

### Incremental Delivery

1. Complete Setup + Foundational → Foundation ready
2. Add User Story 1 → Test independently → MVP with tag filtering! 🎯
3. Add User Story 2 → Test independently → MVP + date filtering
4. Add User Story 6 → Validate subset matching → Enhanced search
5. Add User Story 8 → Test independently → Complete filtering UX
6. Add User Story 7 (P2) → Interactive slider → Enhanced UX
7. Add User Story 3 (P2) → Clear filters → Better UX
8. Add User Story 4 (P2) → Visual feedback → Polish
9. Add User Story 5 (P3) → Browser nav → Final polish
10. Each story adds value without breaking previous stories

### Parallel Team Strategy

With multiple developers:

1. Team completes Setup + Foundational together
2. Once Foundational is done:
   - **Developer A**: User Story 1 (Sidebar Tag Cloud)
   - **Developer B**: User Story 2 (Date Range Filtering)
   - **Developer C**: Infrastructure tests (TagCloudService, TagMatchingService)
3. After US1 + US2 complete:
   - **Developer A**: User Story 8 (Excel Dropdown)
   - **Developer B**: User Story 3 (Clear Filters) + US4 (Active Indicators)
   - **Developer C**: User Story 5 (Browser Navigation) + Content Item Tag Badges
4. Stories complete and integrate independently

---

## Task Summary

**Total Tasks**: 88

**Task Distribution**:

- Phase 1 (Setup): 3 tasks
- Phase 2 (Foundational): 17 tasks
- Phase 3 (US1): 13 tasks
- Phase 4 (US2): 8 tasks
- Phase 5 (US6): 3 tasks
- Phase 6 (US8): 7 tasks
- Phase 7 (US7): 3 tasks
- Phase 8 (US3): 5 tasks
- Phase 9 (US4): 7 tasks
- Phase 10 (US5): 4 tasks
- Phase 11 (Enhancements): 7 tasks
- Phase 12 (Polish): 11 tasks

**Parallel Opportunities**: 28 tasks marked [P] can run in parallel within their phases

**Independent Test Criteria**: Each user story has clear acceptance criteria from spec.md and independent test scenarios

**Suggested MVP Scope**: Phase 1 (Setup) + Phase 2 (Foundational) + Phase 3 (US1) + Phase 4 (US2) + Phase 5 (US6) + Phase 6 (US8) = Core filtering functionality with tag cloud, date range, subset matching, and Excel dropdown

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to specific user story for traceability
- Each user story should be independently completable and testable
- TDD enforced - write tests FIRST, ensure they FAIL before implementation
- Commit after each task or logical group
- Stop at any checkpoint to validate story independently
- E2E tests are MANDATORY for ALL UI changes (no exceptions)
- All tasks follow 10-step AI Assistant Workflow from root AGENTS.md
- Constitution check passed - all 7 rules compliant
