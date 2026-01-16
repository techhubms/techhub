# Feature Specification: Infinite Scroll Pagination

**Feature Branch**: `003-infinite-scroll`  
**Created**: 2026-01-16  
**Status**: Draft  
**Input**: Implement progressive content loading that automatically fetches and displays more content as users scroll down, eliminating manual pagination while maintaining performance

## User Scenarios & Testing

### User Story 1 - Browse Content Continuously (Priority: P1)

Users can scroll through content lists without clicking "Next Page" or pagination buttons - more content loads automatically as they reach the bottom.

**Why this priority**: Modern UX expectation, reduces friction in content discovery.

**Independent Test**: Navigate to section page, scroll to bottom, verify next batch loads automatically.

**Acceptance Scenarios**:

1. **Given** I'm on a section page with more than 20 items, **When** I scroll to within 300px of the bottom, **Then** the next batch of 20 items loads automatically
2. **Given** I'm scrolling through content, **When** new items load, **Then** I see a loading indicator before items appear
3. **Given** I reach the end of all content, **When** I scroll to bottom, **Then** I see "End of content" message and no more loading occurs
4. **Given** new content loads, **When** I continue scrolling, **Then** my scroll position is preserved and I don't jump to top

---

### User Story 2 - Navigate Back with Preserved State (Priority: P2)

Users can use browser back button to return to their previous scroll position with loaded content preserved.

**Why this priority**: Critical for content exploration - users expect to return to where they were.

**Independent Test**: Scroll down, load multiple batches, click item, use back button, verify scroll position and loaded content preserved.

**Acceptance Scenarios**:

1. **Given** I've scrolled down and loaded 3 batches of content, **When** I click on a content item and then use browser back button, **Then** I return to my previous scroll position with all 3 batches still loaded
2. **Given** I've loaded content and return via back button, **When** the page restores, **Then** I don't see the loading indicator re-triggering
3. **Given** I navigate back, **When** filters were active, **Then** the filtered state is preserved along with scroll position

---

### User Story 3 - Combine with Filters (Priority: P2)

Users can apply filters (tags, date, search) and infinite scroll continues to work with filtered results.

**Why this priority**: Filtering and scrolling must work together seamlessly.

**Independent Test**: Apply tag filter, scroll to load more filtered results, verify only matching items load.

**Acceptance Scenarios**:

1. **Given** I have tag filters active, **When** I scroll to bottom, **Then** the next batch contains only items matching my selected tags
2. **Given** I have date range filter active, **When** I scroll to load more, **Then** new items fall within the selected date range
3. **Given** I have search query active, **When** I scroll to load more, **Then** new items match my search query
4. **Given** I apply a filter after loading multiple batches, **When** the filter applies, **Then** content resets to first batch of filtered results

---

### User Story 4 - Keyboard Navigation Through Loaded Content (Priority: P3)

Users can use keyboard (Tab, Arrow keys) to navigate through all loaded content items.

**Why this priority**: Accessibility requirement, ensures keyboard users can access all content.

**Independent Test**: Load multiple batches via scroll, use Tab key to navigate through items, verify all items are accessible.

**Acceptance Scenarios**:

1. **Given** I've loaded 3 batches of content, **When** I press Tab repeatedly, **Then** I can navigate through all loaded items in order
2. **Given** new content loads while I'm tabbing, **When** items appear, **Then** they're added to the tab order after current focus
3. **Given** I'm using keyboard navigation, **When** I reach the last loaded item and press Tab, **Then** new content loads if available

---

### Edge Cases

- What happens when network request fails during scroll? → Show error message with retry button, don't block scrolling
- What happens when user scrolls very quickly? → Debounce scroll events, prevent duplicate requests
- What happens when all content fits on one page (less than 20 items)? → No infinite scroll triggers, show all content
- What happens when user is on slow connection? → Show loading indicator longer, but don't block interaction
- What happens when filter yields fewer than 20 results? → Show all matching items, no infinite scroll needed
- What happens when content loads while user is scrolling up? → Don't add items or change scroll position, only load when scrolling down

## Requirements

### Functional Requirements

- **FR-001**: System MUST detect when user scrolls to within 300px of bottom of content list
- **FR-002**: System MUST fetch next batch of 20 items when scroll threshold is reached
- **FR-003**: System MUST display loading indicator while fetching next batch
- **FR-004**: System MUST append fetched items to existing content list seamlessly
- **FR-005**: System MUST stop fetching when all content has been loaded
- **FR-006**: System MUST display "End of content" message when no more items available
- **FR-007**: System MUST preserve scroll position when adding new items (no jump to top)
- **FR-008**: System MUST prevent duplicate requests while fetch is in progress
- **FR-009**: System MUST work with active filters (tags, date, search) - load only matching items
- **FR-010**: System MUST reset to first batch when filters change
- **FR-011**: System MUST preserve loaded content and scroll position on browser back navigation
- **FR-012**: System MUST handle network errors gracefully with retry option
- **FR-013**: System MUST announce loaded content to screen readers
- **FR-014**: System MUST maintain keyboard navigation through all loaded items
- **FR-015**: System MUST update URL parameters to reflect loaded batches (optional for restoration)

### Non-Functional Requirements

- **NFR-001**: Intersection Observer API MUST be used for scroll detection (battery-efficient, no scroll event listeners)
- **NFR-002**: Content loading MUST complete within 1 second (p95)
- **NFR-003**: Loading indicator MUST appear within 100ms of scroll threshold
- **NFR-004**: Scroll detection MUST be debounced to prevent excessive API calls
- **NFR-005**: Loaded content MUST be cached to avoid re-fetching on back navigation
- **NFR-006**: System MUST support at least 10 batches (200 items) without performance degradation

### Key Entities

- **ContentBatch**: Represents a batch of 20 content items
- **ScrollTrigger**: Intersection Observer target element that triggers loading
- **LoadingState**: Tracks current state (idle, loading, complete, error)
- **BatchOffset**: Tracks which batch to load next (batch 1, 2, 3, etc.)

## Success Criteria

### Measurable Outcomes

- **SC-001**: Users can scroll through content continuously without clicking pagination buttons
- **SC-002**: Next batch loads within 1 second of reaching scroll threshold
- **SC-003**: Scroll position is preserved when navigating back (browser back button works correctly)
- **SC-004**: All loaded content is keyboard accessible (Tab navigation through all items)
- **SC-005**: Loading indicator provides clear feedback during fetch operations
- **SC-006**: System handles errors gracefully with user-friendly retry options
- **SC-007**: Infinite scroll works correctly with tag, date, and search filters
- **SC-008**: Zero console errors during scroll operations
- **SC-009**: Performance remains consistent with 10+ loaded batches (200+ items)

## Implementation Notes

### Reference Documentation

- [docs/filtering-system.md](/docs/filtering-system.md) - How filtering works (will need infinite scroll integration docs)
- [src/TechHub.Web/AGENTS.md](/src/TechHub.Web/AGENTS.md) - Blazor component patterns

### Current Status

**API Endpoints**:

- Content endpoints support pagination via query parameters (needs verification)
- Likely `?offset=20&limit=20` or `?page=2&pageSize=20`

**Frontend Components**:

- No infinite scroll implementation exists yet
- Needs new component or enhancement to existing content list

### Intersection Observer Pattern

**Why Intersection Observer**:

- Battery-efficient (no continuous scroll event listeners)
- Better performance than scroll events
- Built-in threshold detection
- Modern browser API (widely supported)

**Implementation**:

```javascript
// Create observer for scroll trigger element
const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting && !isLoading && hasMoreContent) {
            loadNextBatch();
        }
    });
}, {
    rootMargin: '300px' // Trigger 300px before reaching element
});

// Observe trigger element at bottom of content
observer.observe(scrollTrigger);
```

**Trigger Element**:

- Place invisible div at bottom of content list
- When visible in viewport (with 300px margin), trigger load
- Remove observer when all content loaded

### Batch Loading Strategy

**Batch Size**: 20 items per batch

- Balances performance and UX
- Matches common pagination patterns
- Not too few (excessive requests) or too many (slow loads)

**Offset Tracking**:

- Track current batch number (1, 2, 3, etc.)
- Calculate API offset: `(batchNumber - 1) * 20`
- Example: Batch 3 → offset 40, limit 20 → items 41-60

**Cache Strategy**:

- Store loaded batches in memory
- On filter change, clear cache and reset to batch 1
- On back navigation, restore from cache if available

### URL State Management

**Optional**: Track loaded batches in URL

- `?batch=3` indicates user has loaded up to batch 3
- On page load, load batches 1-3 automatically
- Helps with deep linking and sharing
- May not be necessary if browser back navigation works well

### Error Handling

**Network Failures**:

- Show error message: "Failed to load more content"
- Provide retry button
- Don't block existing content
- Log error for debugging

**Empty Batches**:

- If API returns 0 items, assume end of content
- Show "End of content" message
- Remove scroll trigger observer

### Testing Strategy

**Component Tests** (bUnit):

- Scroll trigger element renders correctly
- Loading indicator appears during fetch
- Items append to existing list
- End message displays when complete

**Integration Tests** (API):

- Pagination endpoints return correct batches
- Offset/limit parameters work correctly
- Empty batch handling

**E2E Tests** (Playwright):

- Scroll to bottom, verify next batch loads
- Load multiple batches, verify all display
- Apply filter, scroll, verify filtered results load
- Navigate to item, use back button, verify scroll position restored
- Scroll quickly, verify no duplicate requests
- Keyboard navigation through loaded items
- Screen reader announces new content

**Performance Tests**:

- Load 10 batches, measure render time
- Monitor memory usage with many loaded items
- Verify Intersection Observer cleanup on unmount

## Dependencies

- **Needed**: API pagination parameter support verification
- **Needed**: Intersection Observer polyfill for older browsers (or accept modern browser requirement)
- **In Progress**: Filter state management (shared with 001-filtering-system, 002-search)

## Out of Scope

- "Load More" button as fallback (Intersection Observer only)
- Virtual scrolling (render only visible items) - future optimization if needed
- Batch size configuration (fixed at 20 items)
- Progressive image loading (handled separately)
- Analytics for scroll depth tracking (covered in 007-google-analytics spec)
