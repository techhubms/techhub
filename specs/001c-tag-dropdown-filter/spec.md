# Feature Specification: Excel-Style Tag Dropdown Filter

**Feature Branch**: `001c-tag-dropdown-filter`  
**Created**: 2026-02-08 (split from 001-filtering-system)  
**Updated**: 2026-02-08  
**Status**: Not Started  
**Input**: Implement Excel-style tag dropdown filter with search, multi-select, and dynamic counts

## 📋 Current Status (as of 2026-02-08)

### ❌ NOT STARTED

All work for this feature is pending:
- ❌ TagDropdownFilter component
- ❌ Search/filter functionality
- ❌ Multi-select with checkboxes
- ❌ Dynamic count display (requires [001a-tag-counting](../001a-tag-counting/))
- ❌ Virtual scrolling for large tag lists (50+ tags)
- ❌ Synchronization with sidebar tag cloud
- ❌ Component tests
- ❌ E2E tests

### 📂 Dependencies

**Required (Backend - Complete)**:
- ✅ API endpoint to fetch all tags: `GET /api/tags`
- ✅ Repository layer supports tag listing

**Required (Frontend)**:
- ⚠️ Tag counting feature ([001a-tag-counting](../001a-tag-counting/)) for dynamic counts
- 🔜 SidebarTagCloud for state synchronization

**Optional**:
- 🔜 Date range slider ([001b-date-range-slider](../001b-date-range-slider/)) to filter available tags by date

---

## Overview

The Excel-style tag dropdown provides users with a searchable, scrollable interface to find and select tags beyond the top 20 displayed in the sidebar tag cloud. It features checkboxes for multi-select, dynamic counts showing intersection results, and virtual scrolling for performance with large tag lists.

**Key Features**:
- Dropdown button shows "Filter by Tags" with count of selected tags
- Opens to reveal scrollable, searchable tag list with checkboxes
- Each tag displays dynamic count (e.g., "AI (901)")
- Tags with 0 potential results are disabled (grayed row, disabled checkbox)
- Search box filters available tags in real-time
- Virtual scrolling activates at ≥50 tags for performance
- Synchronizes with sidebar tag cloud selections
- "Select All" / "Clear All" quick actions

## User Story

### User Story 8: Excel-Style Tag Dropdown Filter

**Priority**: P1  
**Status**: ❌ Not Started

Users can filter content by tags using a modern dropdown interface similar to Excel column filtering, with scrollable list, search, and dynamic counts that update based on current filter state.

**Why this priority**: Provides access to ALL tags, intuitive Excel-like filtering, dynamic counts prevent empty results.

**Acceptance Scenarios**:

1. ❌ **Given** I'm on a section/collection page, **When** I view the sidebar, **Then** I see a "Filter by Tags" dropdown positioned below the tag cloud
2. ❌ **Given** I click the tag dropdown, **When** it opens, **Then** I see a scrollable, searchable list of ALL tags with checkboxes and counts
3. ❌ **Given** I view tags in the dropdown, **When** I examine any tag, **Then** it displays the dynamic count (e.g., "AI (901)") (requires [001a-tag-counting](../001a-tag-counting/))
4. ❌ **Given** I select "AI" checkbox (901 items), **When** other tags update, **Then** their counts show the intersection (e.g., "GitHub Copilot (245)") (requires [001a-tag-counting](../001a-tag-counting/))
5. ❌ **Given** I have tags selected, **When** a tag would result in 0 items, **Then** that tag row becomes disabled (grayed out, checkbox disabled) (requires [001a-tag-counting](../001a-tag-counting/))
6. ❌ **Given** the dropdown is open, **When** I type in the search box, **Then** the tag list filters to show only matching tags with their counts
7. ❌ **Given** I select multiple tags, **When** filters apply, **Then** content shows items matching ALL selected tags (AND logic) and counts update
8. ❌ **Given** I scroll through the dropdown, **When** there are 50+ tags, **Then** virtual scrolling provides smooth performance
9. ❌ **Given** the date range changes, **When** I view the dropdown, **Then** all tag counts reflect the new date range (requires [001b-date-range-slider](../001b-date-range-slider/))
10. ❌ **Given** the dropdown is open, **When** I click outside or press Escape, **Then** the dropdown closes and filters persist

## Requirements

### Functional Requirements

**Dropdown Structure**:
- **FR-001**: System MUST provide Excel-style tag dropdown filter positioned below tag cloud in sidebar
- **FR-001a**: Dropdown button MUST show "Filter by Tags" text
- **FR-001b**: Button MUST display count of currently selected tags (e.g., "Filter by Tags (2)")
- **FR-002**: Clicking dropdown button MUST open scrollable tag list panel
- **FR-003**: Panel MUST include search box at top for filtering tags by name

**Tag List**:
- **FR-004**: Dropdown MUST display ALL available tags (not just top 20)
- **FR-004a**: Each tag MUST show checkbox for selection
- **FR-004b**: Each tag MUST display dynamic count (requires [001a-tag-counting](../001a-tag-counting/))
- **FR-005**: Tags MUST be sorted alphabetically by default
- **FR-006**: Tag list MUST support virtual scrolling when ≥50 tags present

**Search Functionality**:
- **FR-007**: Search box MUST filter tag list in real-time as user types
- **FR-007a**: Search MUST be case-insensitive
- **FR-007b**: Search MUST match partial tag names
- **FR-008**: When search yields no results, MUST show "No tags found" message

**Dynamic Counts** (requires [001a-tag-counting](../001a-tag-counting/)):
- **FR-009**: Each tag MUST display count (e.g., "AI (901)")
- **FR-009a**: Counts MUST update when tags are selected/deselected (intersection logic)
- **FR-009b**: Tags with count=0 MUST be disabled (grayed row, disabled checkbox)
- **FR-009c**: Counts MUST update when date range changes (requires [001b-date-range-slider](../001b-date-range-slider/))

**Selection & Actions**:
- **FR-010**: Users MUST be able to select/deselect tags via checkboxes
- **FR-011**: Dropdown MUST provide "Select All" and "Clear All" actions
- **FR-011a**: "Select All" MUST select only visible/enabled tags (respects search and disabled state)
- **FR-012**: Selected tags MUST synchronize with sidebar tag cloud
- **FR-013**: Multiple selected tags MUST apply AND logic (items match ALL tags)

**UI/UX**:
- **FR-014**: Clicking outside dropdown MUST close it and persist filter state
- **FR-015**: Pressing Escape MUST close dropdown
- **FR-016**: Dropdown MUST be keyboard accessible (Tab, Space, Enter, Escape)

### Performance Requirements

- **PR-001**: Virtual scrolling MUST maintain 60fps with 100+ tags
- **PR-002**: Search filtering MUST respond within 50ms
- **PR-003**: Count updates MUST complete within 200ms (debounced for rapid changes)

## Success Criteria

- **SC-001**: ❌ Dropdown displays all tags with checkboxes
- **SC-002**: ❌ Search filters tag list in real-time
- **SC-003**: ❌ Tag counts display with thousand separators (requires [001a](../001a-tag-counting/))
- **SC-004**: ❌ Counts update when selection changes (requires [001a](../001a-tag-counting/))
- **SC-005**: ❌ Tags with count=0 are disabled (requires [001a](../001a-tag-counting/))
- **SC-006**: ❌ Virtual scrolling works smoothly with 50+ tags
- **SC-007**: ❌ Selections synchronize with sidebar tag cloud
- **SC-008**: ❌ "Select All" / "Clear All" work correctly
- **SC-009**: ❌ Keyboard navigation works (Tab, Space, Escape)
- **SC-010**: ❌ Clicking outside closes dropdown

## Implementation Notes

### Component Structure

**File**: `src/TechHub.Web/Components/TagDropdownFilter.razor`

**Parameters**:
```csharp
[Parameter] public string[] SelectedTags { get; set; } = Array.Empty<string>();
[Parameter] public EventCallback<string[]> OnSelectionChanged { get; set; }
[Parameter] public DateOnly? FromDate { get; set; }  // For count filtering
[Parameter] public DateOnly? ToDate { get; set; }    // For count filtering
```

**State**:
```csharp
private bool isOpen;
private string searchQuery = "";
private TagWithCount[] allTags = Array.Empty<TagWithCount>();
private TagWithCount[] filteredTags = Array.Empty<TagWithCount>();
```

### API Integration

**Fetch All Tags** (Backend already supports this):
```http
GET /api/tags
```

**With Dynamic Counts** (requires [001a-tag-counting](../001a-tag-counting/)):
```http
GET /api/tags?tags=ai,azure&from=2025-01-01&to=2026-02-08
```

**Response**:
```json
[
  { "tag": "AI", "count": 901 },
  { "tag": "GitHub Copilot", "count": 245 },
  { "tag": "Azure", "count": 0 }
]
```

### Virtual Scrolling

**Strategy**:
- Render only visible items (viewport + buffer)
- Use CSS transforms for positioning
- Re-render on scroll with debouncing

**Trigger**: Activate when `allTags.Length >= 50`

**Libraries** (optional):
- Consider using Blazor Virtualize component
- Or custom implementation with IntersectionObserver

### Synchronization with Sidebar Tag Cloud

**Mechanism**:
- Dropdown and sidebar share same filter state (via parent component or service)
- Selecting tag in dropdown highlights it in sidebar
- Selecting tag in sidebar checks it in dropdown
- Both trigger same `OnSelectionChanged` event

**Implementation**:
```csharp
// Parent component (Section.razor)
<TagDropdownFilter SelectedTags="@selectedTags" OnSelectionChanged="HandleTagSelectionChanged" />
<SidebarTagCloud SelectedTags="@selectedTags" OnTagClicked="HandleTagSelectionChanged" />

@code {
    private string[] selectedTags = Array.Empty<string>();

    private async Task HandleTagSelectionChanged(string[] tags)
    {
        selectedTags = tags;
        await LoadContent();  // Reload with new filters
        await RefreshTagCounts();  // Update counts (requires 001a)
    }
}
```

### UI Design

**Dropdown Button**:
```
┌─────────────────────────────┐
│ Filter by Tags (2)       ▼ │
└─────────────────────────────┘
```

**Opened Dropdown**:
```
┌─────────────────────────────┐
│ [Search tags...]          × │
├─────────────────────────────┤
│ □ Select All  ✓ Clear All  │
├─────────────────────────────┤
│ ✓ AI (901)                  │
│ □ Azure (0)        [grayed] │
│ ✓ GitHub Copilot (245)      │
│ □ Kubernetes (87)           │
│ □ Visual Studio (156)       │
│ ... (virtual scrolling)     │
└─────────────────────────────┘
```

### Testing Strategy

**Component Tests** (bUnit):
- Dropdown opens/closes correctly
- Search filters tag list
- Checkboxes toggle selection
- "Select All" / "Clear All" work
- Virtual scrolling renders correctly
- Disabled state for count=0 tags
- Keyboard navigation
- Click outside to close

**Integration Tests**:
- Fetch all tags from API
- Synchronization with sidebar tag cloud

**E2E Tests** (Playwright):
- Open dropdown, verify all tags listed
- Search for tag, verify filtering
- Select tags, verify content updates
- Verify counts update (with [001a](../001a-tag-counting/))
- Verify disabled tags (with [001a](../001a-tag-counting/))
- Virtual scrolling with 50+ tags
- Keyboard navigation
- Click outside to close

## Dependencies

**Required (Backend - Complete)**:
- ✅ API endpoint to fetch all tags
- ✅ Repository layer tag listing

**Required (Frontend)**:
- ⚠️ [001a-tag-counting](../001a-tag-counting/) - Dynamic counts feature
- ✅ SidebarTagCloud - For state synchronization

**Optional**:
- 🔜 [001b-date-range-slider](../001b-date-range-slider/) - Date filtering affects available tags

**Libraries**:
- Virtual scrolling: Blazor Virtualize component or custom
- Search filtering: Built-in JavaScript or Blazor

## Related Specs

- **[001a-tag-counting](../001a-tag-counting/)** - Dynamic counts in dropdown
- **[001b-date-range-slider](../001b-date-range-slider/)** - Date range affects dropdown tags


## Integration Points

### With SidebarTagCloud
- Share same `SelectedTags` state
- Both components emit `OnSelectionChanged`
- Visual synchronization (selected tags highlighted/checked)

### With DateRangeSlider (requires [001b](../001b-date-range-slider/))
- Pass `FromDate` and `ToDate` to dropdown
- Reload dropdown when date range changes
- Filter available tags by date

### With Tag Counting (requires [001a](../001a-tag-counting/))
- Display dynamic counts for each tag
- Disable tags with count=0
- Update counts when selection changes

### With Content List
- Selecting tags triggers content reload
- AND logic: Items must match ALL selected tags
- URL updates with tag parameters

## Out of Scope

- Tag creation/editing (read-only filter interface)
- Tag favoriting or pinning
- Tag categories or hierarchies
- Advanced tag operations (exclude, OR logic)
- Tag popularity visualization
- Saved filter presets
