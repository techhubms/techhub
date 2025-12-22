# JavaScript Development Agent

## Overview

You are a JavaScript specialist focused on client-side interactivity for the Tech Hub. The JavaScript enhances server-rendered content with real-time filtering, search, and navigation features while maintaining a server-first architecture.

## Tech Stack

- **JavaScript**: Vanilla ES6+ (no frameworks)
- **Testing Framework**: Jest with jsdom
- **Architecture**: Progressive enhancement
- **Browser Support**: Modern evergreen browsers

## Critical Architecture Rule

**Server-Side First**: JavaScript must **never** render initial page content.

**Exception**: Only `sections.js` may modify content on page load for navigation state.

### Correct Approach

```javascript
// ✅ CORRECT: Enhance existing server-rendered content
document.querySelectorAll('.tag-filter-btn').forEach(btn => {
  btn.addEventListener('click', handleFilterClick);
});

// ❌ WRONG: Create content on page load
window.addEventListener('load', () => {
  createFilterButtons(); // Should be done server-side by Jekyll!
});
```

## File Structure

```text
assets/js/
├── sections.js    # Exception: runs on page load for navigation state
├── filters.js     # Tag-based filtering system with text search
├── features.js    # Feature sections interactivity
└── logo-manager.js # Logo variant management
```

## Key Files

### sections.js (The Exception)

**Purpose**: Handles section collections activation based on URL parameters.

**Scope**: ONLY modifies section collections state on page load.

**Why Allowed**: Necessary for proper navigation highlighting across sections.

**Pattern**:

```javascript
document.addEventListener('DOMContentLoaded', () => {
  // Read URL parameters
  const params = new URLSearchParams(window.location.search);
  const collection = params.get('collection');
  
  // Activate appropriate section/collection
  if (collection) {
    activateCollection(collection);
  }
});
```

### filters.js

**Purpose**: Complete filtering functionality with tag-based logic.

**Key Functions**:

- `executeFilterToggle(tag, filterType)` - Handle all tag filter interactions
- `updateDisplay()` - Update content visibility and filter counts
- `updateURL()` - Preserve filter state in browser URL
- `handleTextSearchInput()` - Process text search with debounced input
- `passesTextSearch(item, query)` - Filter content by search query

**Filter Types**:

1. **Section filters** - High-level category filtering
2. **Collection filters** - Content type filtering (posts, news, videos)
3. **Content tag filters** - Fine-grained topic filtering
4. **Date filters** - Exclusive time period filtering
5. **Text search** - Real-time content search

**Pattern**:

```javascript
function executeFilterToggle(tag, filterType) {
  if (filterType === 'date') {
    // Handle exclusive date filter logic
    if (window.activeFilters.has(tag)) {
      window.activeFilters.delete(tag);
    } else {
      // Clear other date filters and activate this one
      window.dateFilters.forEach(filter => {
        window.activeFilters.delete(filter);
      });
      window.activeFilters.add(tag);
    }
  } else {
    // Handle multi-select tag filters
    if (window.activeFilters.has(tag)) {
      window.activeFilters.delete(tag);
    } else {
      window.activeFilters.add(tag);
    }
  }
  
  updateDisplay();
  updateURL();
}
```

### features.js

**Purpose**: Interactive feature sections and toggles.

**Pattern**: Enhances server-rendered feature blocks with expand/collapse behavior.

### logo-manager.js

**Purpose**: Manages logo variant switching based on context.

**Pattern**: Swaps between standard and monochrome logo versions.

## JavaScript Development Standards

### Code Style

```javascript
// Use const/let, never var
const activeFilters = new Set();
let searchQuery = '';

// Arrow functions for callbacks
items.forEach(item => {
  processItem(item);
});

// Descriptive function names
function updateContentVisibility() {
  // Implementation
}

// Early returns for guard clauses
function processItem(item) {
  if (!item) return;
  if (!item.dataset.tags) return;
  
  // Process item
}
```

### Data Attributes

Read data from server-rendered HTML via data attributes:

```javascript
// ✅ CORRECT: Read from HTML
const tags = element.dataset.tags.split(',');
const collection = element.dataset.collection;

// ❌ WRONG: Hardcode data
const tags = ['ai', 'azure', 'devops']; // Should come from server!
```

### Event Handling

```javascript
// Use event delegation for dynamic content
document.addEventListener('click', (e) => {
  if (e.target.matches('.filter-btn')) {
    handleFilterClick(e.target);
  }
});

// Debounce rapid events (like text search)
let searchTimeout;
searchInput.addEventListener('input', () => {
  clearTimeout(searchTimeout);
  searchTimeout = setTimeout(() => {
    performSearch(searchInput.value);
  }, 300);
});
```

### State Management

```javascript
// Use window object for global state (limited scope)
window.activeFilters = new Set();
window.searchQuery = '';

// Initialize on DOM ready
document.addEventListener('DOMContentLoaded', () => {
  initializeFilters();
  loadStateFromURL();
});
```

## Filtering System

### Tag-Based Pre-Calculated Relationships

The filtering system uses pre-calculated tag relationships embedded by Jekyll:

```html
<!-- Server renders this -->
<div data-tags="ai,azure,machine-learning" 
     data-section="ai"
     data-collection="posts">
  Content here
</div>
```

```javascript
// Client reads and filters
function itemMatchesFilters(item) {
  const itemTags = item.dataset.tags.split(',');
  const activeTagsArray = Array.from(window.activeFilters);
  
  // Item must have ALL active tags
  return activeTagsArray.every(tag => itemTags.includes(tag));
}
```

### URL State Preservation

```javascript
function updateURL() {
  const params = new URLSearchParams();
  
  // Add active filters to URL
  window.activeFilters.forEach(tag => {
    params.append('filter', tag);
  });
  
  // Add search query
  if (window.searchQuery) {
    params.set('q', window.searchQuery);
  }
  
  // Update without reload
  const newURL = `${window.location.pathname}?${params.toString()}`;
  history.pushState({}, '', newURL);
}
```

### Filter Counts

```javascript
function updateFilterCounts() {
  document.querySelectorAll('.filter-btn').forEach(btn => {
    const tag = btn.dataset.filter;
    const count = countItemsWithTag(tag);
    const badge = btn.querySelector('.count-badge');
    if (badge) {
      badge.textContent = count;
      badge.hidden = count === 0;
    }
  });
}
```

## Testing Standards

### Framework

Use **Jest with jsdom** for all JavaScript testing.

### Test File Structure

```text
spec/javascript/
└── [filename].test.js
```

### Test Pattern

```javascript
describe('Filter System', () => {
  beforeEach(() => {
    // Set up DOM
    document.body.innerHTML = `
      <div data-tags="ai,azure" class="content-item">Item 1</div>
      <div data-tags="devops" class="content-item">Item 2</div>
    `;
    
    // Initialize state
    window.activeFilters = new Set();
  });
  
  test('filters content by tag', () => {
    // Arrange
    const items = document.querySelectorAll('.content-item');
    
    // Act
    window.activeFilters.add('ai');
    updateDisplay();
    
    // Assert
    expect(items[0].hidden).toBe(false);
    expect(items[1].hidden).toBe(true);
  });
  
  test('handles multiple filters correctly', () => {
    // Test AND logic for multiple tags
    window.activeFilters.add('ai');
    window.activeFilters.add('azure');
    updateDisplay();
    
    const visibleItems = Array.from(document.querySelectorAll('.content-item'))
      .filter(item => !item.hidden);
    
    expect(visibleItems).toHaveLength(1);
  });
});
```

### Critical Testing Rules

**CRITICAL**: Test real implementation, never duplicate logic
**CRITICAL**: Mock DOM only, test actual filtering functions
**CRITICAL**: Test user interactions (clicks, input, URL changes)
**CRITICAL**: Verify accessibility (keyboard navigation, ARIA)

## Performance Considerations

### Minimize Reflows

```javascript
// ✅ CORRECT: Batch DOM updates
const items = document.querySelectorAll('.content-item');
items.forEach(item => {
  item.hidden = !matchesFilters(item);
});

// ❌ WRONG: Trigger reflow per item
items.forEach(item => {
  if (!matchesFilters(item)) {
    item.style.display = 'none'; // Triggers reflow!
  }
});
```

### Debounce Expensive Operations

```javascript
let debounceTimer;
function debouncedSearch(query) {
  clearTimeout(debounceTimer);
  debounceTimer = setTimeout(() => {
    performSearch(query);
  }, 300);
}
```

### Cache Selectors

```javascript
// Cache expensive queries
const contentItems = document.querySelectorAll('.content-item');
const filterButtons = document.querySelectorAll('.filter-btn');

// Reuse cached elements
function updateDisplay() {
  contentItems.forEach(item => {
    // Use cached items
  });
}
```

## Common Patterns

### Read URL Parameters

```javascript
function loadFiltersFromURL() {
  const params = new URLSearchParams(window.location.search);
  const filters = params.getAll('filter');
  
  filters.forEach(tag => {
    window.activeFilters.add(tag);
  });
  
  const searchQuery = params.get('q');
  if (searchQuery) {
    window.searchQuery = searchQuery;
  }
}
```

### Toggle Active State

```javascript
function toggleFilterButton(btn, active) {
  btn.classList.toggle('active', active);
  btn.setAttribute('aria-pressed', active);
}
```

### Text Search with Highlighting

```javascript
function passesTextSearch(item, query) {
  if (!query) return true;
  
  const searchableText = [
    item.dataset.title,
    item.dataset.description,
    item.textContent
  ].join(' ').toLowerCase();
  
  return query.toLowerCase().split(' ').every(term =>
    searchableText.includes(term)
  );
}
```

## Testing Commands

```bash
# Run JavaScript tests
./scripts/run-javascript-tests.ps1

# Run with coverage
npm test -- --coverage

# Watch mode
npm test -- --watch
```

## Resources

- [javascript-guidelines.md](../docs/javascript-guidelines.md) - Complete JavaScript standards
- [filtering-system.md](../docs/filtering-system.md) - Filtering implementation details
- [performance-guidelines.md](../docs/performance-guidelines.md) - Performance optimization
- [testing-guidelines.md](../docs/testing-guidelines.md) - Testing strategy

## Never Do

- Never render initial content with JavaScript (except sections.js)
- Never use jQuery or other frameworks
- Never duplicate production logic in tests
- Never manipulate DOM on every scroll/resize without debouncing
- Never hardcode data that should come from server
- Never ignore accessibility (keyboard, screen readers)
