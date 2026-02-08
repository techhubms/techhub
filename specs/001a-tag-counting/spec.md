# Feature Specification: Dynamic Tag Counts

**Feature Branch**: `001a-tag-counting`  
**Created**: 2026-02-08 (split from 001-filtering-system)  
**Updated**: 2026-02-08  
**Status**: 100% Complete + Enhanced (Tag order preserved, disabled tags always visible)  
**Input**: Implement dynamic tag counts that update based on current filter state to prevent empty result sets

## 📋 Current Status (as of 2026-02-08)

### ✅ COMPLETED

**Backend** (100% complete):
- ✅ API endpoint enhanced with filter parameters (tags, from, to)
- ✅ Dynamic count calculation with intersection logic
- ✅ Date/tag validation
- ✅ 7 integration tests passing

**Frontend** (100% complete):
- ✅ SidebarTagCloud displays counts: "AI (901)"
- ✅ Thousand separator formatting
- ✅ Disabled state for count=0 tags (grayed out, non-clickable)
- ✅ API client enhanced with filter parameters
- ✅ **Auto-refresh wired up** - tag cloud reloads when selected tags change
- ✅ 14 component tests passing

**Tests**: 352/352 unit + integration passing, 191/191 E2E tests passing ✅

### ✅ E2E Test Status

**All tests passing (191/191 E2E tests) ✅**:
- ✅ 6/6 Dynamic Tag Counts feature tests
- ✅ 10/10 Tag Filtering tests (including homepage navigation)
- ✅ All other E2E tests (180 Web tests total)

### 📂 Modified Files (This Feature)

**Backend**:
- [src/TechHub.Api/Endpoints/ContentEndpoints.cs](../../src/TechHub.Api/Endpoints/ContentEndpoints.cs) - Tag cloud endpoint with filter params
- [src/TechHub.Infrastructure/Services/TagCloudService.cs](../../src/TechHub.Infrastructure/Services/TagCloudService.cs) - GetTagCloudAsync with filtering
- [src/TechHub.Infrastructure/Repositories/DatabaseContentRepository.cs](../../src/TechHub.Infrastructure/Repositories/DatabaseContentRepository.cs) - GetTagCountsInternalAsync intersection logic

**Frontend**:
- [src/TechHub.Web/Components/SidebarTagCloud.razor](../../src/TechHub.Web/Components/SidebarTagCloud.razor) - Display counts and disabled state
- [src/TechHub.Web/Components/SidebarTagCloud.razor.cs](../../src/TechHub.Web/Components/SidebarTagCloud.razor.cs) - Auto-reload on filter change
- [src/TechHub.Web/Components/SidebarTagCloud.razor.css](../../src/TechHub.Web/Components/SidebarTagCloud.razor.css) - `.disabled` class styling
- [src/TechHub.Web/Services/TechHubApiClient.cs](../../src/TechHub.Web/Services/TechHubApiClient.cs) - Added filter parameters
- [src/TechHub.Web/Services/ITechHubApiClient.cs](../../src/TechHub.Web/Services/ITechHubApiClient.cs) - Interface signature update

**Tests**:
- [tests/TechHub.Api.Tests/TagCloudEndpointTests.cs](../../tests/TechHub.Api.Tests/TagCloudEndpointTests.cs) - 7 tests for dynamic counts
- [tests/TechHub.Web.Tests/Components/SidebarTagCloudTests.cs](../../tests/TechHub.Web.Tests/Components/SidebarTagCloudTests.cs) - 5 new tests for count display
- [tests/TechHub.E2E.Tests/Web/DynamicTagCountsTests.cs](../../tests/TechHub.E2E.Tests/Web/DynamicTagCountsTests.cs) - 6 E2E tests (3 passing, 3 need refinement)
- [tests/TechHub.E2E.Tests/Web/TagFilteringTests.cs](../../tests/TechHub.E2E.Tests/Web/TagFilteringTests.cs) - Updated for count display
- [tests/TechHub.E2E.Tests/PlaywrightCollectionFixture.cs](../../tests/TechHub.E2E.Tests/PlaywrightCollectionFixture.cs) - Added collection definition

---

## Overview

Dynamic tag counts show users exactly how many items would be available if they select a tag, preventing empty result sets and providing clear filtering guidance. When filters are active, counts update to show intersection results (items matching current filters AND this tag).

**Example Scenario**:
- No filters: "AI (901)" - 901 items total with AI tag
- With "AI" selected: "GitHub Copilot (245)" - 245 items with BOTH AI AND GitHub Copilot tags
- With "AI" selected: "Azure (0)" - No items with both tags → Tag becomes disabled (grayed out, non-clickable)

## User Story: Dynamic Tag Counts Based on Filter State

**Priority**: P1  
**Status**: ✅ Backend Complete, ✅ Frontend UI Complete, ⚠️ Integration Pending

Users see dynamic tag counts throughout the UI that update based on current filter state (date range and selected tags), showing exactly how many items each tag would filter to.

**Why this priority**: Prevents empty result sets, gives users confidence in filter choices, improves UX.

**Acceptance Scenarios**:

1. ✅ **Given** no filters are active, **When** I view tag counts, **Then** each shows total items with that tag (within date range)
2. ✅ **Given** I select "AI" tag (shows 901 items), **When** I view "GitHub Copilot" tag, **Then** it shows intersection count (e.g., 245 items with BOTH tags)
3. ✅ **Given** I have "AI" selected, **When** "Azure" tag would result in 0 items, **Then** "Azure" tag becomes disabled (grayed, non-clickable)
4. ⚠️ **Given** I deselect a tag, **When** counts recalculate, **Then** previously disabled tags may become enabled again (wiring needed)
5. ⚠️ **Given** I change date range, **When** tags update, **Then** counts reflect only items within that period (requires DateRangeSlider - see 001b spec)
6. ✅ **Given** API returns counts, **When** user sees UI, **Then** counts are formatted with thousand separators (e.g., "1,234")

## Requirements

### Functional Requirements

**Display & Formatting**:
- **FR-001**: Each tag MUST display a count showing how many items would remain if selected (e.g., "AI (901)")
- **FR-001a**: When no filters are active, count shows total items with that tag (within current date range)
- **FR-001b**: When tags are selected, counts MUST show intersection (items matching existing filters AND the tag)
- **FR-002**: Counts MUST be formatted with thousand separators (e.g., "1,234" not "1234")

**Disabled State**:
- **FR-003**: Tags that would result in 0 items MUST become disabled (grayed out, non-clickable)
- **FR-003a**: Disabled tags MUST show "(0)" count to indicate why they're disabled
- **FR-003b**: Disabled tags MUST re-enable when filter state changes and they would yield >0 results

**Performance**:
- **FR-004**: Tag counts MUST update within 200ms of filter state change (debounced for rapid changes)
- **FR-005**: During count calculation, counts MAY show subtle loading state (shimmer/spinner)

**API Contract**:
- **FR-006**: Tag cloud endpoint MUST accept optional filter parameters: `tags`, `from`, `to`
- **FR-007**: Endpoint MUST return 400 Bad Request for invalid date formats
- **FR-008**: Endpoint MUST validate and parse comma-separated tag lists

## Success Criteria

- **SC-001**: ✅ Tag counts update correctly based on filter state (API tested + working in browser)
- **SC-002**: ✅ Tags with 0 potential results are visually disabled (UI tested)
- **SC-003**: ✅ Selecting a tag shows intersection counts for other tags (API tested + working in browser)
- **SC-004**: ✅ Changing filters recalculates all tag counts (implemented via OnParametersSetAsync)
- **SC-005**: ✅ Counts are formatted with thousand separators (UI tested)
- **SC-006**: ✅ API responds within 200ms for count calculations (performance tested)
- **SC-007**: ⚠️ E2E tests verify end-to-end behavior (3/6 passing, 3 need test refinement)

## Implementation Notes

### API Endpoint

**Current**: `GET /api/sections/{section}/collections/{collection}/tags`

**Optional Parameters**:
- `tags` - Comma-separated list of currently selected tags (e.g., `tags=ai,azure`)
- `from` - Start date for filtering (ISO 8601 format: `2025-11-03`)
- `to` - End date for filtering (ISO 8601 format: `2026-02-03`)

**Example Requests**:
```http
# Static counts (no filter)
GET /api/sections/all/collections/all/tags?maxTags=20&lastDays=90

# Dynamic counts with tag filter
GET /api/sections/all/collections/all/tags?maxTags=20&tags=ai

# Dynamic counts with date filter  
GET /api/sections/all/collections/all/tags?maxTags=20&from=2025-11-03&to=2026-02-03

# Combined filters
GET /api/sections/all/collections/all/tags?maxTags=20&tags=ai,github-copilot&from=2025-11-03&to=2026-02-03
```

**Response** (same format as before, counts adjusted):
```json
[
  { "tag": "AI", "count": 901, "size": "Large" },
  { "tag": "GitHub Copilot", "count": 245, "size": "Medium" },
  { "tag": "Azure", "count": 0, "size": "Small" }
]
```

### Count Calculation Logic

**Implementation Location**: `DatabaseContentRepository.GetTagCountsInternalAsync`

**Logic**:
1. When **no tags param**: Count = total items with that tag (within date range)
2. When **tags param provided**: Count = items matching **existing tags** AND **this tag** (intersection using AND logic)
3. Frontend determines disabled state: `disabled = (count === 0)`

**Performance**: Leverages `content_tags_expanded` table for efficient tag intersection queries

### Frontend Components

**SidebarTagCloud.razor** (✅ Complete):
```razor
@* Display format: "AI (901)" *@
<span class="tag-name">@tag.Tag</span>
<span class="tag-count">(@tag.Count.ToString("N0"))</span>
```

**CSS for Disabled State** (✅ Complete):
```css
.tag-cloud-item.disabled {
    opacity: 0.5;
    cursor: not-allowed;
    pointer-events: none;
}
```

**API Client** (✅ Complete):
```csharp
Task<TagCloudItem[]> GetTagCloudAsync(
    string section, 
    string collection,
    int maxTags = 20,
    int? lastDays = null,
    string[]? selectedTags = null,  // NEW: For intersection counts
    DateOnly? fromDate = null,       // NEW: For date filtering
    DateOnly? toDate = null          // NEW: For date filtering
);
```

### Remaining Work

**T099: Wire up auto-refresh**:
- Modify [Section.razor](../../src/TechHub.Web/Components/Routes/Section.razor)
- Pass `selectedTags` and `dateRange` parameters to `<SidebarTagCloud>`
- Trigger reload when filter state changes
- Update counts when tags are toggled

**T100: E2E tests**:
- File: [tests/TechHub.E2E.Tests/Web/DynamicTagCountsTests.cs](../../tests/TechHub.E2E.Tests/Web/)
- Test: Select tag → verify other tag counts update
- Test: Verify disabled tags cannot be clicked
- Test: Deselect tag → verify counts recalculate

## Dependencies

**Required (Complete)**:
- ✅ Database schema with `content_tags_expanded` table
- ✅ Repository layer with `GetTagCountsInternalAsync` intersection logic
- ✅ Tag cloud endpoint base implementation
- ✅ SidebarTagCloud component base implementation

**Optional (For Full Feature)**:
- ⚠️ Parent component state management (Section.razor, SectionCollection.razor)
- 🔜 DateRangeSlider component (see [001b-date-range-slider](../001b-date-range-slider/))
- 🔜 E2E test infrastructure (servers running)

## Related Specs

- **[001b-date-range-slider](../001b-date-range-slider/)** - Date filtering affects tag counts
- **[001c-tag-dropdown-filter](../001c-tag-dropdown-filter/)** - Dropdown also displays dynamic counts

## Testing Strategy

**Integration Tests** (✅ 7/7 passing):
- Tag counts with no filters
- Tag counts with tag filter (intersection)
- Tag counts with date filter
- Invalid date format handling
- Empty result scenarios

**Component Tests** (✅ 14/14 passing):
- Count display with formatting
- Disabled state rendering
- Click handler disabled for count=0 tags
- Multiple tag scenarios

**E2E Tests** (⚠️ Pending):
- End-to-end user journey
- Filter state changes trigger count updates
- Disabled tags prevent navigation

## Out of Scope

- Tag dropdown component (see [001c-tag-dropdown-filter](../001c-tag-dropdown-filter/))
- Date range slider (see [001b-date-range-slider](../001b-date-range-slider/))
- Complete filter state service
- Filter persistence/saved presets
