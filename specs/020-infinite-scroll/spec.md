# 020-infinite-scroll

> **Feature**: Progressive content loading using Intersection Observer API with loading indicators, error handling, and filter compatibility

## Overview

Infinite scroll replaces the initial "20 + same-day" pagination with seamless progressive loading as users scroll down content lists. When users approach the bottom of the page, the system automatically fetches and appends the next batch of items without requiring button clicks. This enhancement improves browsing experience while maintaining compatibility with existing filtering, search, and URL state features.

## Requirements

### Functional Requirements

**FR-1**: The system MUST detect when user scrolls near bottom of content list (within 300px of bottom)  
**FR-2**: The system MUST fetch next batch of items when scroll threshold reached  
**FR-3**: The system MUST append fetched items to existing content list seamlessly  
**FR-4**: The system MUST display loading indicator while fetching content  
**FR-5**: The system MUST stop fetching when all content has been loaded  
**FR-6**: The system MUST maintain active filter state during infinite scroll (date, tags, search)  
**FR-7**: The system MUST update URL parameters to reflect current page/offset  
**FR-8**: The system MUST restore scroll position when user navigates back (browser back button)  
**FR-9**: The system MUST handle network errors gracefully with retry option  
**FR-10**: The system MUST announce loaded content to screen readers  
**FR-11**: The system MUST support keyboard navigation to newly loaded content  

### Non-Functional Requirements

**NFR-1**: Intersection Observer API MUST be used for scroll detection (no scroll event listeners)  
**NFR-2**: Content loading MUST be debounced to prevent duplicate requests (minimum 500ms)  
**NFR-3**: Loading indicator MUST appear within 100ms of scroll threshold  
**NFR-4**: New content MUST render within 1 second of fetch completion  
**NFR-5**: Infinite scroll MUST be disabled on initial page load (first 20 items server-rendered)  
**NFR-6**: Browsers without Intersection Observer MUST fall back to "Load More" button  
**NFR-7**: Each fetch request MUST include current filter parameters (date, tags, search)  
**NFR-8**: Scroll position MUST be preserved in session storage for back navigation  
**NFR-9**: Memory usage MUST be monitored to prevent performance degradation with large lists  

## Use Cases

### UC-1: Automatic Content Loading

**Actor**: Site Visitor  
**Precondition**: Collection page is displayed with 20 items  
**Trigger**: Visitor scrolls down content list  

**Flow**:

1. Visitor views collection page with initial 20 items (server-rendered)
2. Visitor scrolls down to view items
3. When visitor reaches 300px from bottom, Intersection Observer triggers
4. System checks if more content available (total items > loaded items)
5. Loading spinner appears at bottom of list
6. System fetches next 20 items via API with current filters
7. System appends new items below existing content
8. Loading spinner disappears
9. Visitor continues scrolling without interruption
10. Process repeats until all content loaded

**Postcondition**: Visitor sees seamlessly loaded content

### UC-2: Infinite Scroll with Active Filters

**Actor**: Site Visitor  
**Precondition**: Visitor has applied date filter "Last 30 days"  
**Trigger**: Visitor scrolls down filtered content  

**Flow**:

1. Visitor applies "Last 30 days" filter
2. Content list updates to show 20 filtered items
3. Visitor scrolls down
4. Intersection Observer triggers at bottom
5. System fetches next batch with date filter parameter: `GET /api/items?date=30d&offset=20&limit=20`
6. API returns only items matching date filter
7. New items appended to list
8. Filter state remains active
9. URL includes filter: `/github-copilot/news/?date=30d`

**Postcondition**: Infinite scroll respects active filters

### UC-3: Network Error Handling

**Actor**: Site Visitor  
**Precondition**: Visitor scrolls to trigger content fetch  
**Trigger**: Network request fails  

**Flow**:

1. Visitor scrolls to bottom
2. System initiates fetch request
3. Loading spinner appears
4. Network request fails (timeout, 500 error, offline)
5. Loading spinner disappears
6. Error message appears: "Failed to load content"
7. Retry button displayed: "Try Again"
8. Visitor clicks "Try Again"
9. System retries fetch request
10. On success, content loads normally

**Postcondition**: Visitor can recover from network errors

### UC-4: Back Button Navigation

**Actor**: Site Visitor  
**Precondition**: Visitor has scrolled and loaded 100 items  
**Trigger**: Visitor clicks item to view detail, then clicks browser back button  

**Flow**:

1. Visitor scrolls down collection page, loading 100 total items
2. System stores scroll position in session storage: `{ url: '/ai/news/', offset: 100, scrollY: 3500 }`
3. Visitor clicks item #85 to view detail page
4. Browser navigates to item detail page
5. Visitor clicks browser back button
6. Browser returns to collection page URL: `/ai/news/`
7. System checks session storage for stored state
8. System finds stored offset: 100, scrollY: 3500
9. System fetches first 100 items in single request (or batches)
10. System renders 100 items on page
11. System scrolls window to scrollY: 3500
12. Visitor sees same position as before navigation

**Postcondition**: Scroll position and loaded content are restored

### UC-5: Screen Reader Announcements

**Actor**: Screen Reader User  
**Precondition**: User scrolls down with screen reader active  
**Trigger**: New content loads  

**Flow**:

1. Screen reader user navigates down content list
2. User reaches bottom of current content
3. Intersection Observer triggers fetch
4. Loading indicator appears with ARIA live region: "Loading more content..."
5. New items fetched and appended
6. ARIA live region updates: "Loaded 20 more items. 40 of 150 total."
7. Screen reader announces update to user
8. User continues navigating through new content

**Postcondition**: Screen reader user is informed of content changes

### UC-6: Fallback for Unsupported Browsers

**Actor**: Site Visitor (using browser without Intersection Observer support)  
**Precondition**: Visitor's browser doesn't support Intersection Observer  
**Trigger**: Page loads  

**Flow**:

1. Visitor loads collection page
2. System detects Intersection Observer not supported (feature detection)
3. System displays "Load More" button below initial 20 items
4. Visitor scrolls to bottom
5. Visitor clicks "Load More" button
6. System fetches next 20 items
7. New content appends to list
8. Button remains visible with updated text: "Load More (40 of 150 loaded)"

**Postcondition**: Visitor can load content incrementally without Intersection Observer

## Acceptance Criteria

**AC-1**: Given collection page with 100 items, when user scrolls to 300px from bottom, then next 20 items are fetched automatically  
**AC-2**: Given fetch in progress, when triggered again within 500ms, then duplicate request is prevented (debounced)  
**AC-3**: Given loading state, when waiting for response, then loading spinner is visible at bottom of list  
**AC-4**: Given successful fetch, when items returned, then items are appended below existing content without page jump  
**AC-5**: Given all content loaded, when user scrolls to bottom, then no additional fetch is triggered and "End of content" message displays  
**AC-6**: Given date filter active, when infinite scroll triggers, then API request includes date filter parameter  
**AC-7**: Given tag filter active, when infinite scroll triggers, then API request includes tag filter parameter  
**AC-8**: Given search query active, when infinite scroll triggers, then API request includes search parameter  
**AC-9**: Given network error, when fetch fails, then error message and retry button are displayed  
**AC-10**: Given retry button click, when request succeeds, then content loads and error message disappears  
**AC-11**: Given 100 items loaded, when user navigates away and back, then scroll position is restored to previous location  
**AC-12**: Given new content loaded, when screen reader active, then ARIA live region announces "Loaded N more items"  
**AC-13**: Given Intersection Observer unsupported, when page loads, then "Load More" button is displayed instead  

## Technical Implementation

### Intersection Observer Setup

**Observer Configuration**:

```typescript
const observerOptions = {
  root: null,              // Use viewport as root
  rootMargin: '300px',     // Trigger 300px before bottom
  threshold: 0.01          // Trigger when 1% visible
};

const observer = new IntersectionObserver(handleIntersect, observerOptions);
```

**Target Element** (sentinel):

```html
<div id="infinite-scroll-sentinel" aria-hidden="true"></div>
```

**Intersection Handler**:

```typescript
function handleIntersect(entries: IntersectionObserverEntry[]) {
  entries.forEach(entry => {
    if (entry.isIntersecting && !isLoading && hasMoreContent) {
      loadMoreContent();
    }
  });
}
```

### Content Loading Logic

**State Management**:

```typescript
interface InfiniteScrollState {
  offset: number;           // Current offset (e.g., 0, 20, 40...)
  limit: number;            // Items per batch (default: 20)
  totalItems: number;       // Total available items
  isLoading: boolean;       // Fetch in progress
  hasMore: boolean;         // More content available
  error: string | null;     // Error message if failed
  filters: FilterState;     // Active filters (date, tags, search)
}
```

**Load More Function**:

```typescript
async function loadMoreContent() {
  // Prevent duplicate requests
  if (isLoading || !hasMore) return;
  
  // Set loading state
  setIsLoading(true);
  setError(null);
  showLoadingIndicator();
  
  try {
    // Build API request with filters
    const params = new URLSearchParams({
      offset: state.offset.toString(),
      limit: state.limit.toString(),
      ...buildFilterParams(state.filters)  // date, tags, search
    });
    
    // Fetch next batch
    const response = await fetch(`/api/items?${params}`);
    if (!response.ok) throw new Error('Failed to fetch');
    
    const data = await response.json();
    
    // Append items to list
    appendItems(data.items);
    
    // Update state
    setState({
      offset: state.offset + data.items.length,
      hasMore: data.items.length === state.limit,
      totalItems: data.total
    });
    
    // Announce to screen readers
    announceToScreenReader(`Loaded ${data.items.length} more items. ${state.offset + data.items.length} of ${data.total} total.`);
    
    // Update URL
    updateURL({ offset: state.offset });
    
  } catch (error) {
    // Handle error
    setError('Failed to load content');
    showRetryButton();
  } finally {
    setIsLoading(false);
    hideLoadingIndicator();
  }
}
```

### Filter Integration

**Build Filter Parameters**:

```typescript
function buildFilterParams(filters: FilterState): Record<string, string> {
  const params: Record<string, string> = {};
  
  // Date filter
  if (filters.dateRange) {
    params.date = filters.dateRange;  // e.g., "30d", "90d"
  }
  
  // Tag filters
  if (filters.tags.length > 0) {
    params.tags = filters.tags.join(',');
  }
  
  // Search query
  if (filters.searchQuery) {
    params.search = filters.searchQuery;
  }
  
  return params;
}
```

**Filter Change Handler**:

```typescript
function onFilterChange(newFilters: FilterState) {
  // Reset infinite scroll state
  setState({
    offset: 0,
    hasMore: true,
    filters: newFilters
  });
  
  // Clear current items
  clearItems();
  
  // Load first batch with new filters
  loadMoreContent();
}
```

### Scroll Position Restoration

**Save State**:

```typescript
function saveScrollState() {
  const state = {
    url: window.location.pathname + window.location.search,
    offset: scrollState.offset,
    scrollY: window.scrollY,
    timestamp: Date.now()
  };
  
  sessionStorage.setItem('scrollState', JSON.stringify(state));
}

// Save on navigation
window.addEventListener('beforeunload', saveScrollState);
```

**Restore State**:

```typescript
function restoreScrollState() {
  const savedState = sessionStorage.getItem('scrollState');
  if (!savedState) return;
  
  const state = JSON.parse(savedState);
  
  // Check if same URL
  if (state.url !== window.location.pathname + window.location.search) {
    return;
  }
  
  // Check if not stale (< 5 minutes)
  if (Date.now() - state.timestamp > 300000) {
    return;
  }
  
  // Fetch all items up to saved offset
  fetchBatchItems(0, state.offset).then(items => {
    renderItems(items);
    window.scrollTo(0, state.scrollY);
  });
}

// Restore on page load
window.addEventListener('DOMContentLoaded', restoreScrollState);
```

### Accessibility

**ARIA Live Region**:

```html
<div 
  id="infinite-scroll-announcer" 
  role="status" 
  aria-live="polite" 
  aria-atomic="true"
  class="visually-hidden"
>
  <!-- Dynamically updated announcements -->
</div>
```

**Announce Function**:

```typescript
function announceToScreenReader(message: string) {
  const announcer = document.getElementById('infinite-scroll-announcer');
  if (announcer) {
    announcer.textContent = message;
    
    // Clear after 1 second to allow re-announcement
    setTimeout(() => {
      announcer.textContent = '';
    }, 1000);
  }
}
```

**Keyboard Navigation**:

```typescript
// Ensure new items are keyboard navigable
function appendItems(items: Item[]) {
  items.forEach(item => {
    const element = renderItem(item);
    
    // Ensure focusable links have proper tabindex
    const links = element.querySelectorAll('a');
    links.forEach(link => link.setAttribute('tabindex', '0'));
    
    contentContainer.appendChild(element);
  });
}
```

### Loading Indicator

**HTML Structure**:

```html
<div id="loading-indicator" class="loading-indicator" aria-hidden="true">
  <div class="spinner"></div>
  <p>Loading more content...</p>
</div>
```

**Show/Hide Functions**:

```typescript
function showLoadingIndicator() {
  const indicator = document.getElementById('loading-indicator');
  indicator?.classList.add('visible');
}

function hideLoadingIndicator() {
  const indicator = document.getElementById('loading-indicator');
  indicator?.classList.remove('visible');
}
```

**CSS**:

```css
.loading-indicator {
  display: none;
  padding: var(--spacing-4);
  text-align: center;
}

.loading-indicator.visible {
  display: block;
}

.spinner {
  border: 3px solid var(--color-border-default);
  border-top-color: var(--color-accent-primary);
  border-radius: 50%;
  width: 40px;
  height: 40px;
  animation: spin 1s linear infinite;
  margin: 0 auto var(--spacing-2);
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

/* Respect reduced motion preference */
@media (prefers-reduced-motion: reduce) {
  .spinner {
    animation: none;
    border-top-color: var(--color-accent-primary);
  }
}
```

### Error Handling

**Error Message Component**:

```html
<div id="error-message" class="error-message" role="alert">
  <p>Failed to load content. Please check your connection.</p>
  <button id="retry-button" class="btn btn-primary">
    Try Again
  </button>
</div>
```

**Error Handlers**:

```typescript
function showRetryButton() {
  const errorMessage = document.getElementById('error-message');
  errorMessage?.classList.add('visible');
  
  const retryButton = document.getElementById('retry-button');
  retryButton?.addEventListener('click', handleRetry);
}

function handleRetry() {
  const errorMessage = document.getElementById('error-message');
  errorMessage?.classList.remove('visible');
  
  loadMoreContent();
}
```

### Fallback for Unsupported Browsers

**Feature Detection**:

```typescript
function initInfiniteScroll() {
  if ('IntersectionObserver' in window) {
    // Use Intersection Observer
    setupIntersectionObserver();
  } else {
    // Fallback to Load More button
    showLoadMoreButton();
  }
}
```

**Load More Button**:

```html
<button id="load-more-button" class="btn btn-primary">
  Load More (<span id="loaded-count">20</span> of <span id="total-count">150</span> loaded)
</button>
```

**Button Handler**:

```typescript
function showLoadMoreButton() {
  const button = document.getElementById('load-more-button');
  button?.classList.add('visible');
  button?.addEventListener('click', loadMoreContent);
}

// Update button text after each load
function updateLoadMoreButton() {
  const loadedCount = document.getElementById('loaded-count');
  const totalCount = document.getElementById('total-count');
  
  if (loadedCount) loadedCount.textContent = state.offset.toString();
  if (totalCount) totalCount.textContent = state.totalItems.toString();
  
  // Hide button if all loaded
  if (!state.hasMore) {
    const button = document.getElementById('load-more-button');
    button?.classList.remove('visible');
  }
}
```

## Performance Optimization

### Debouncing

Prevent rapid-fire requests when scrolling quickly:

```typescript
let debounceTimeout: number | null = null;

function loadMoreContent() {
  if (debounceTimeout) {
    clearTimeout(debounceTimeout);
  }
  
  debounceTimeout = setTimeout(() => {
    actualLoadMoreContent();
    debounceTimeout = null;
  }, 500);  // Wait 500ms after last trigger
}
```

### Memory Management

Monitor and limit total loaded items to prevent performance degradation:

```typescript
const MAX_ITEMS_IN_DOM = 500;  // Maximum items before virtualization

function appendItems(items: Item[]) {
  const currentItemCount = document.querySelectorAll('.item-card').length;
  
  // Warn if approaching limit
  if (currentItemCount > MAX_ITEMS_IN_DOM * 0.8) {
    console.warn('Approaching maximum items in DOM. Consider implementing virtual scrolling.');
  }
  
  // Implement virtual scrolling if exceeded (future enhancement)
  if (currentItemCount > MAX_ITEMS_IN_DOM) {
    implementVirtualScrolling();
  }
  
  // Otherwise, append normally
  items.forEach(item => {
    contentContainer.appendChild(renderItem(item));
  });
}
```

### Batch Rendering

Render items in batches to avoid blocking main thread:

```typescript
function appendItems(items: Item[]) {
  const fragment = document.createDocumentFragment();
  
  items.forEach(item => {
    fragment.appendChild(renderItem(item));
  });
  
  // Append all at once (single reflow)
  contentContainer.appendChild(fragment);
}
```

## Edge Cases

**EC-1**: User scrolls very fast → Debouncing prevents multiple simultaneous requests  
**EC-2**: Network is slow → Loading indicator remains visible until response  
**EC-3**: API returns empty array → Set `hasMore = false`, display "End of content"  
**EC-4**: API returns fewer items than requested → Still append items, set `hasMore = false` if < limit  
**EC-5**: User applies filter while loading → Cancel pending request, restart with new filters  
**EC-6**: User refreshes page mid-scroll → Start from beginning (no state persisted across refresh except back navigation)  
**EC-7**: Session storage full → Gracefully fail to save state, log warning  
**EC-8**: Saved scroll state is stale (> 5 minutes) → Ignore, start from beginning  
**EC-9**: Browser doesn't support Intersection Observer → Fall back to "Load More" button  
**EC-10**: User has reduced motion preference → Disable smooth scrolling, instant content append  

## Migration Notes

**From Jekyll (MVP)**:

- MVP uses "20 + same-day" pagination with server-side rendering
- Current behavior: Show first 20 items + all items from same day as 20th item
- **POST-MVP**: Infinite scroll replaces pagination
- **MIGRATION**: Keep "20 + same-day" for initial server-side render, infinite scroll for subsequent batches
- **PRESERVE**: All filtering and search functionality must work with infinite scroll

## Testing Strategy

### Unit Tests

- Test Intersection Observer callback triggers correctly
- Test debounce logic prevents duplicate requests
- Test filter parameter building
- Test scroll state save/restore logic
- Test error handling (network failures, malformed responses)

### Integration Tests

- Test infinite scroll with active date filter
- Test infinite scroll with active tag filters
- Test infinite scroll with active search query
- Test back button navigation restores scroll position
- Test filter change resets scroll state

### Visual Tests

- Test loading indicator appears/disappears correctly
- Test error message and retry button display
- Test smooth content appending (no layout shifts)

### Accessibility Tests

- Test ARIA live region announces loaded content
- Test keyboard navigation to newly loaded items
- Test screen reader compatibility
- Test focus management (focus doesn't jump unexpectedly)

### Performance Tests

- Test memory usage with 500+ loaded items
- Test scroll smoothness during content loading
- Test debouncing prevents excessive API calls
- Test rendering performance (batch rendering)

### Cross-Browser Tests

- Test Intersection Observer in Chrome, Firefox, Safari, Edge
- Test fallback "Load More" button in IE11 (if supported)
- Test on mobile devices (touch scrolling)

## Open Questions

None - infinite scroll requirements are clearly defined

## References

- [013-api-endpoints Spec](../013-api-endpoints/spec.md) - Content API structure
- [017-page-components Spec](../017-page-components/spec.md) - Collection page integration
- [019-filtering-system Spec](../019-filtering-system/spec.md) - Filter compatibility
- [022-search Spec](../022-search/spec.md) - Search integration
- [Intersection Observer API](https://developer.mozilla.org/en-US/docs/Web/API/Intersection_Observer_API) - Browser API docs
- Root `AGENTS.md` - Performance and accessibility principles
