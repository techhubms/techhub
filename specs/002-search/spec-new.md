# Feature Specification: Sidebar Search

**Feature Branch**: `002-search`  
**Created**: 2026-01-16  
**Status**: Draft  
**Input**: Implement sidebar search box for section and collection pages to allow users to search content by text query across titles, descriptions, and tags

## User Scenarios & Testing

### User Story 1 - Search Content by Text (Priority: P1)

Users can type search queries in the sidebar to filter content items and see only items matching their search terms.

**Why this priority**: Essential content discovery mechanism, complements tag and date filtering.

**Independent Test**: Navigate to any section page, type text in sidebar search box, verify content list updates to show only matching items.

**Acceptance Scenarios**:

1. **Given** I'm on a section page, **When** I type "copilot" in the search box, **Then** the content list updates to show only items with "copilot" in title, description, or tags
2. **Given** I type a search query, **When** I wait 300ms, **Then** the search executes and results display (debounced input)
3. **Given** I have a search query active, **When** I modify the query, **Then** results update automatically after debounce delay
4. **Given** I search for content, **When** I check the URL, **Then** it includes my search query as a parameter (e.g., `?search=copilot`)
5. **Given** I share a URL with search parameter, **When** someone opens it, **Then** they see the same filtered results

---

### User Story 2 - Combine Search with Filters (Priority: P1)

Users can combine text search with tag and date filters to refine results further.

**Why this priority**: Power users need multi-criteria filtering for precise content discovery.

**Independent Test**: Apply tag filter, then add search query, verify both filters apply together.

**Acceptance Scenarios**:

1. **Given** I have tags selected, **When** I add a search query, **Then** results match BOTH the search query AND selected tags (AND logic)
2. **Given** I have a date range selected, **When** I add a search query, **Then** results match the search query AND fall within the date range
3. **Given** I have search + filters active, **When** I check the URL, **Then** it includes both search and filter parameters (e.g., `?search=copilot&tags=ai&date=last-30-days`)

---

### User Story 3 - Clear Search Query (Priority: P2)

Users can quickly clear the search box to remove the search filter.

**Why this priority**: Provides escape hatch when search query yields unwanted results.

**Independent Test**: Type search query, click clear button (X icon), verify search clears and all content displays.

**Acceptance Scenarios**:

1. **Given** I have a search query active, **When** I click the clear button (X icon) in search box, **Then** the search query clears and all content displays
2. **Given** I clear the search, **When** other filters are active, **Then** those filters remain active (only search clears)
3. **Given** I clear the search, **When** I check the URL, **Then** the search parameter is removed

---

### User Story 4 - Search Feedback and Empty States (Priority: P2)

Users see clear feedback about search state, result counts, and empty result scenarios.

**Why this priority**: Provides context and helps users understand why they see certain results.

**Independent Test**: Search for term with no matches, verify "No results found" message displays.

**Acceptance Scenarios**:

1. **Given** I search for a term, **When** results display, **Then** I see a result count (e.g., "Showing 15 results for 'copilot'")
2. **Given** I search for a term with no matches, **When** results update, **Then** I see "No results found for 'xyz'" message with suggestion to clear search
3. **Given** I'm typing in the search box, **When** debounce is active, **Then** I see a subtle loading indicator
4. **Given** I have a search query, **When** I view the search box, **Then** the query text is visible and editable

---

### Edge Cases

- What happens when search query is very short (1-2 characters)? → Still search, but may return many results
- What happens when search query has special characters? → Escape/sanitize input, search as literal text
- What happens when URL has invalid search parameter? → Ignore invalid parameter, log warning
- What happens when user types very long search query? → Limit input length (e.g., 100 characters max)
- What happens on slow connections? → Show loading indicator, don't block UI, allow query changes during search
- What happens when search combines with collection filter? → Combine all filters (search + collection + tags + date) with AND logic

## Requirements

### Functional Requirements

- **FR-001**: Sidebar MUST display a search box (text input field) for entering search queries
- **FR-002**: Search box MUST have placeholder text (e.g., "Search content...")
- **FR-003**: Search box MUST show clear button (X icon) when query is entered
- **FR-004**: Search input MUST be debounced with 300ms delay before executing search
- **FR-005**: System MUST search across content item titles, descriptions, and tags (case-insensitive)
- **FR-006**: System MUST filter content to show only items matching the search query
- **FR-007**: Search MUST combine with other filters using AND logic (must match search AND tags AND date range)
- **FR-008**: System MUST update URL query parameter when search query changes (e.g., `?search=copilot`)
- **FR-009**: System MUST parse URL search parameter on page load and apply search automatically
- **FR-010**: System MUST display result count when search is active (e.g., "Showing 15 results for 'copilot'")
- **FR-011**: System MUST show "No results found" message when search yields zero items
- **FR-012**: System MUST provide clear button to remove search query
- **FR-013**: Search operations MUST NOT cause full page reload (client-side search only)
- **FR-014**: Search results MUST update within 50ms after debounce completes
- **FR-015**: Search box MUST be keyboard accessible (focus, type, clear with Escape key)

### Key Entities

- **SidebarSearch**: Component that displays search input box and manages search state
- **SearchQuery**: Represents user's text search query
- **SearchFilter**: Combined state of search query with other filters (tags, date range)
- **ContentItem**: Item being searched (has title, description, tags, and other metadata)

## Success Criteria

### Measurable Outcomes

- **SC-001**: Users can type search query and see results update within 350ms (300ms debounce + 50ms execution)
- **SC-002**: Search combines correctly with tag and date filters (AND logic across all filter types)
- **SC-003**: Search state is shareable via URL (copy URL, share, recipient sees same search)
- **SC-004**: Search box is keyboard accessible (100% Tab/focus/Escape navigation)
- **SC-005**: Browser back/forward navigation works correctly with search state
- **SC-006**: Zero console errors or warnings during search operations
- **SC-007**: Search provides clear feedback (result counts, loading states, empty states)
- **SC-008**: Clear button removes search query and updates results immediately

## Implementation Notes

### Reference Documentation

- [docs/filtering-system.md](/docs/filtering-system.md) - How filtering works (NEEDS TO BE UPDATED with search)
- [src/TechHub.Web/AGENTS.md](/src/TechHub.Web/AGENTS.md) - Blazor component patterns

### Current Status

**API Endpoints**:

- Search capability likely exists via `/api/content/filter` endpoint with query parameter
- Needs verification of search query parameter support

**Frontend Components**:

- `SidebarSearch` component needs to be created
- May be part of `SidebarTags` component or separate component

### Component Architecture

**SidebarSearch Component**:

- Text input field with placeholder
- Clear button (X icon) when query has text
- Debounce input changes (300ms)
- Updates URL parameters when search query changes
- Reads URL parameters on load to restore state
- Emits events when search query changes
- Visual states: default, focused, typing, has-query

**Search Integration with Filters**:

- Search query combines with tag filters (AND logic)
- Search query combines with date range filter (AND logic)
- Search query combines with collection filter (AND logic)
- All filters share same URL parameter space

**Content List Component** (existing, needs search integration):

- Receives search query from parent
- Calls API with search parameter
- Updates displayed content based on API response
- Shows loading state during search operations
- Shows result count and "no results" message

### URL Parameter Format

**Query Parameters**:

- `search` - URL-encoded search query (e.g., `search=github%20copilot`)
- Combined with other filters: `?search=copilot&tags=ai,azure&date=last-30-days`

**URL Encoding**:

- Search query with spaces: URL encode (e.g., `github%20copilot`)
- Special characters: URL encode
- Empty search: Parameter omitted from URL

### Search Logic

**Text Matching**:

- Case-insensitive search
- Match against title, description, and tags
- Simple substring matching (OR across fields)
- Example: Search "copilot" matches items with "copilot" OR "Copilot" OR "COPILOT" in any field

**Combining with Filters**:

- Search "copilot" + tag "ai" → Items with "copilot" in text AND tag "ai"
- Search "copilot" + date "last-30-days" → Items with "copilot" in text AND published in last 30 days
- SQL equivalent: `WHERE (title LIKE '%copilot%' OR description LIKE '%copilot%' OR tags LIKE '%copilot%') AND tag = 'ai' AND publishedDate >= @30DaysAgo`

### Debouncing Strategy

**Why 300ms**:

- Standard debounce delay for search-as-you-type
- Balances responsiveness with reducing API calls
- Allows users to finish typing before search executes

**Implementation**:

- Use JavaScript `setTimeout` or Blazor equivalent
- Cancel previous timer on each keystroke
- Execute search after 300ms of no input

### Testing Strategy

**Component Tests** (bUnit):

- SidebarSearch renders with search input
- Typing in search box updates state
- Clear button appears when query has text
- Debounce delays search execution

**Integration Tests** (API):

- Filter endpoint supports search query parameter
- Search parameter filters content correctly
- Search combines with other filters correctly

**E2E Tests** (Playwright):

- Navigate to section page, type in search box, verify content updates
- Type search query, verify URL updates with search parameter
- Combine search with tags, verify AND logic
- Click clear button, verify search clears
- Copy search URL, open in new tab, verify same search results
- Browser back/forward navigation with search
- Keyboard navigation (Tab to search box, Escape to clear)

**Accessibility Tests**:

- Keyboard-only navigation (Tab to search, type, Escape to clear)
- Screen reader announces search box and result updates
- ARIA labels for search input and clear button
- Focus states clearly visible

## Dependencies

- **Needed**: API search parameter support in filter endpoint
- **Needed**: Debounce implementation (JavaScript or Blazor)
- **Needed**: SidebarSearch component creation
- **In Progress**: URL state management (shared with filtering system)

## Out of Scope

- Advanced search features (Boolean operators, phrase matching, fuzzy search)
- Autocomplete or search suggestions
- Search history or saved searches
- Full-text search engine (Elasticsearch, Azure Search) - using simple text matching
- Search analytics or popular queries tracking (covered in 006-google-analytics spec)
- Highlighted search terms in results (future enhancement)
