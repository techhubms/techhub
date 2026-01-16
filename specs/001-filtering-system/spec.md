# Feature Specification: Sidebar Content Filtering

**Feature Branch**: `001-filtering-system`  
**Created**: 2026-01-16  
**Status**: Draft  
**Input**: Implement sidebar-based tag and date filtering for section and collection pages to help users discover relevant content quickly

## User Scenarios & Testing

### User Story 1 - Filter Content by Tags (Priority: P1)

Users can click tags in the sidebar to filter content items and see only items matching selected tags.

**Why this priority**: Primary content discovery mechanism, most requested feature from users.

**Independent Test**: Navigate to any section page, click a tag in sidebar, verify content list updates to show only matching items.

**Acceptance Scenarios**:

1. **Given** I'm on a section page, **When** I click a tag in the sidebar, **Then** the content list updates to show only items with that tag
2. **Given** I have one tag selected, **When** I click another tag, **Then** the content list shows items matching ANY of the selected tags (OR logic)
3. **Given** I have tags selected, **When** I click a selected tag again, **Then** it deselects and content list updates
4. **Given** I filter by tags, **When** I check the URL, **Then** it includes my selected tags as query parameters (e.g., `?tags=ai,azure`)
5. **Given** I share the URL with tags, **When** someone opens it, **Then** they see the same filtered view

---

### User Story 2 - Filter Content by Date Range (Priority: P1)

Users can select date ranges (Last 7/30/90 days, All Time) to filter content by publication date.

**Why this priority**: Essential for finding recent content, complements tag filtering.

**Independent Test**: Navigate to section page, click "Last 30 days" filter, verify only recent content displays.

**Acceptance Scenarios**:

1. **Given** I'm on a section page, **When** I select "Last 30 days", **Then** the content list shows only items from the past 30 days
2. **Given** I have a date filter active, **When** I select a different date range, **Then** the content updates to that range
3. **Given** I have date and tag filters active, **When** I view results, **Then** I see items matching BOTH filters (AND logic)
4. **Given** I select a date range, **When** I check the URL, **Then** it includes the date parameter (e.g., `?date=last-30-days`)

---

### User Story 3 - Clear All Filters (Priority: P2)

Users can quickly reset all active filters to see all content again.

**Why this priority**: Escape hatch when users over-filter and get zero results.

**Independent Test**: Apply multiple filters, click "Clear All" button, verify all content displays and URL resets.

**Acceptance Scenarios**:

1. **Given** I have multiple filters active, **When** I click "Clear All Filters", **Then** all filters reset and all content displays
2. **Given** I clear all filters, **When** I check the URL, **Then** filter parameters are removed
3. **Given** I have filters active, **When** I click the browser back button, **Then** filters update to previous state

---

### User Story 4 - See Active Filter Indicators (Priority: P2)

Users can see which filters are currently active with visual badges and result counts.

**Why this priority**: Provides feedback and context about current filter state.

**Independent Test**: Apply filters, verify active tags are highlighted, result count updates, badges display.

**Acceptance Scenarios**:

1. **Given** I select a tag, **When** the filter applies, **Then** the tag is visually highlighted in the sidebar
2. **Given** I have filters active, **When** I view the content area, **Then** I see a result count (e.g., "Showing 15 results")
3. **Given** I have multiple filters, **When** I view the page, **Then** I see badges or chips indicating active filters
4. **Given** no items match my filters, **When** I view results, **Then** I see a "No results found" message with suggestion to clear filters

---

### User Story 5 - Navigate Filter State with Browser (Priority: P3)

Users can use browser back/forward buttons to navigate through filter states.

**Why this priority**: Natural browser behavior expectation, enhances UX but not critical for MVP.

**Independent Test**: Apply filters, use browser back button, verify previous filter state restores.

**Acceptance Scenarios**:

1. **Given** I apply filters, **When** I click browser back button, **Then** the previous filter state restores
2. **Given** I go back through filter states, **When** I click browser forward button, **Then** I move forward through filter history
3. **Given** I navigate filter states, **When** content updates, **Then** the page does not fully reload (client-side only)

---

### Edge Cases

- What happens when user selects tags that yield zero results? → Show "No results found" message with "Clear All Filters" button
- What happens when URL has invalid date range parameter? → Fallback to "All Time" default, log error
- What happens when URL has invalid tag names? → Ignore invalid tags, apply only valid ones, log warning
- What happens when user rapidly clicks multiple tags? → Debounce UI updates, only apply final state
- What happens on slow connections? → Show loading indicator, don't block UI, allow filter changes during load
- What happens when content is already filtered by collection via SubNav? → Combine collection filter with sidebar filters (AND logic)

## Requirements

### Functional Requirements

- **FR-001**: Sidebar MUST display a "Tags" section with all available tags for the current section/collection
- **FR-002**: Tags MUST be clickable and toggle between selected/unselected states
- **FR-003**: Selected tags MUST be visually distinguished (highlighted, different color, checkmark icon)
- **FR-004**: System MUST filter content to show items matching ANY selected tag (OR logic within tags)
- **FR-005**: System MUST display a "Date Range" filter with options: Last 7 days, Last 30 days, Last 90 days, All Time
- **FR-006**: Date range filters MUST combine with tag filters using AND logic (must match date AND any tag)
- **FR-007**: System MUST update the URL query parameters when filters change (e.g., `?tags=ai,azure&date=last-30-days`)
- **FR-008**: System MUST parse URL parameters on page load and apply corresponding filters automatically
- **FR-009**: System MUST provide a "Clear All Filters" button that resets all filters and updates URL
- **FR-010**: System MUST display result count (e.g., "Showing 42 results") when filters are active
- **FR-011**: System MUST show "No results found" message when filters yield zero items
- **FR-012**: System MUST preserve filter state when navigating via browser back/forward buttons
- **FR-013**: Filter operations MUST NOT cause full page reload (client-side filtering only)
- **FR-014**: Filtered content list MUST update within 50ms of filter selection
- **FR-015**: All filter controls MUST be keyboard accessible (Tab, Space/Enter for selection)

### Key Entities

- **SidebarTags**: Component that displays available tags and manages selection state
- **TagFilter**: Represents selected tags with OR logic (match any tag)
- **DateRangeFilter**: Represents selected date range with preset options
- **FilterState**: Combined state of tag filters, date range, and URL parameters
- **ContentItem**: Item being filtered (has tags, publication date, and other metadata)

## Success Criteria

### Measurable Outcomes

- **SC-001**: Users can apply tag filters and see results update within 50ms (no perceived lag)
- **SC-002**: Users can combine tag and date filters with accurate results (AND logic between filter types)
- **SC-003**: Filter state is shareable via URL (copy URL, share, recipient sees same filters)
- **SC-004**: All filter controls are keyboard accessible (100% Tab/Space/Enter navigation)
- **SC-005**: Browser back/forward navigation works correctly with filter state
- **SC-006**: Zero console errors or warnings during filter operations
- **SC-007**: Filter UI provides clear feedback on active state (visual highlighting, badges, result counts)
- **SC-008**: "No results found" scenario provides clear message and recovery option (clear filters button)

## Implementation Notes

### Reference Documentation

- [docs/filtering-system.md](/docs/filtering-system.md) - How filtering works (NEEDS TO BE CREATED)
- [src/TechHub.Web/AGENTS.md](/src/TechHub.Web/AGENTS.md) - Blazor component patterns
- [docs/toc-component.md](/docs/toc-component.md) - Sidebar component architecture reference

### Current Status

**API Endpoints (COMPLETE)**:
- `GET /api/content/filter` - Supports tag and date filtering with query parameters
- Tested and working with 6 integration tests passing

**Frontend Components (PLACEHOLDER)**:
- `SidebarTags.razor` exists but is non-functional
- Needs complete rewrite to support tag selection, URL parameters, and state management

### Component Architecture

**SidebarTags Component**:
- Displays list of available tags for current section/collection
- Manages selection state (selected/unselected)
- Updates URL parameters when tags selected
- Reads URL parameters on load to restore state
- Emits events when filters change
- Visual states: default, selected, hover, focus (for keyboard nav)

**Date Range Component** (may be part of SidebarTags or separate):
- Radio buttons or dropdown for date range options
- Updates URL parameters when selection changes
- Reads URL parameters on load to restore state
- Emits events when date filter changes

**Content List Component** (existing, needs filter integration):
- Receives filter state from parent
- Calls API with filter parameters
- Updates displayed content based on API response
- Shows loading state during filter operations
- Shows result count and "no results" message

### URL Parameter Format

**Query Parameters**:
- `tags` - Comma-separated tag names (e.g., `tags=ai,azure,copilot`)
- `date` - Date range identifier (e.g., `date=last-30-days`)
- Example: `/azure/all?tags=security,devops&date=last-90-days`

**URL Encoding**:
- Tag names with spaces: URL encode (e.g., `GitHub%20Copilot`)
- Multiple tags: Comma separator, no spaces
- Invalid parameters: Silently ignore, log warning

### Filter Logic

**Tag Filtering (OR)**:
- User selects ["ai", "azure"]
- Show items with tag "ai" OR tag "azure"
- SQL equivalent: `WHERE tag IN ('ai', 'azure')`

**Date Range Filtering (AND)**:
- User selects "Last 30 days" AND tags ["ai"]
- Show items published in last 30 days AND has tag "ai"
- SQL equivalent: `WHERE publishedDate >= @30DaysAgo AND tag = 'ai'`

**Collection Filtering (AND)**:
- User is on `/azure/blogs` (collection = blogs)
- User selects tag "security"
- Show items in "blogs" collection AND section "azure" AND tag "security"
- SQL equivalent: `WHERE collection = 'blogs' AND section = 'azure' AND tag = 'security'`

### Testing Strategy

**Component Tests** (bUnit):
- SidebarTags renders with correct tag list
- Clicking tag updates selection state
- Selected tags are visually highlighted
- Clear All button resets state

**Integration Tests** (API already tested):
- Filter endpoint returns correct results for tag filtering
- Filter endpoint returns correct results for date filtering
- Filter endpoint combines filters correctly

**E2E Tests** (Playwright):
- Navigate to section page, click tag, verify content updates
- Select multiple tags, verify OR logic
- Select date range + tags, verify AND logic
- Verify URL updates with filter parameters
- Copy filtered URL, open in new tab, verify same filter state
- Browser back/forward navigation with filters
- Keyboard navigation through filter controls
- "No results" scenario with clear filters recovery

**Accessibility Tests**:
- Keyboard-only navigation (Tab through tags, Space/Enter to select)
- Screen reader announces tag selection state
- ARIA labels for filter controls
- Focus states clearly visible

## Dependencies

- **Completed**: API filtering endpoints (`GET /api/content/filter`)
- **Completed**: Content repository with tag and date filtering logic
- **In Progress**: Blazor component architecture for state management
- **Needed**: URL state management library or custom implementation
- **Needed**: Visual design for active/selected tag states

## Out of Scope

- Text search filtering (covered in 002-search spec)
- Infinite scroll pagination (covered in 003-infinite-scroll spec)
- Advanced filtering (multi-select with AND logic, exclude filters, etc.)
- Filter analytics tracking (covered in 006-google-analytics spec)
- Saved filter presets or user preferences
- Tag popularity indicators or sorting

