# JavaScript Development Guidelines

This file contains all JavaScript-specific development guidelines for the Tech Hub site. For comprehensive development patterns, see the [AI Coding Standards](../ai-coding-standards.md).

## Core JavaScript Principles

### Server-Side First Architecture

**Critical Rule**: JavaScript must never render initial page content (exception: assets/js/sections.js only)

**Correct Approach**:

1. Jekyll/Liquid renders all initial content server-side
2. JavaScript adds event listeners and interactivity
3. JavaScript responds to user interactions
4. Content is immediately visible and functional

**Examples**:

```javascript
// ✅ CORRECT: Enhance existing server-rendered content
document.querySelectorAll('.tag-filter-btn').forEach(btn => {
  btn.addEventListener('click', handleFilterClick);
});

// ❌ WRONG: Create content on page load
window.addEventListener('load', () => {
  createFilterButtons(); // This should be done server-side!
});
```

**The assets/js/sections.js Exception**:

- Only `assets/js/sections.js` may modify content on initial load
- It handles section collections state based on URL parameters
- This is necessary for proper navigation highlighting
- All other JavaScript must wait for user interaction

### JavaScript Enhancement Process

1. **Data Extraction**: Read embedded data from server-rendered HTML
2. **Event Listeners**: Add click handlers to server-rendered buttons  
3. **Real-Time Filtering**: Show/hide content without page reload
4. **State Management**: Maintain filter state and URL parameters

### File Structure and Assets

**Critical File: `assets/js/sections.js`**

This is the ONLY exception to the "no initial content creation" rule:

- **Purpose**: Handles section collections activation based on URL parameters
- **Scope**: Only modifies section collections state on page load
- **Restriction**: All other JavaScript must wait for user interaction

**File organization principles:**

```text
assets/js/
├── sections.js       # Exception - can run on page load
└── filters.js     # Complete filtering functionality with mode-based logic
```

## Filtering System Implementation

For comprehensive filtering system details, see [Filtering System](filtering-system.md).

### Key JavaScript Functions

The filtering system uses a unified tag-based approach with these core functions:

- `executeFilterToggle()`: Handle all tag filter interactions (sections, collections, content tags)
- `updateDisplay()`: Update content visibility and filter counts in real-time
- `updateURL()`: Preserve filter state in browser URL for bookmarking

### Filter Interaction Flow

```javascript
User clicks tag → JavaScript filters content → DOM updated → URL updated
```

### Unified Tag Filter Implementation

**All Filter Types (Using Pre-Calculated Relationships):**

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
        // Handle inclusive tag filters (sections, collections, content tags)
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

**Date Filtering with Pre-Calculated Mappings:**

```javascript
function isWithinDateFilter(postIndex, dateFilter) {
    // Use pre-calculated date filter mappings for ultra-fast lookups
    return window.dateFilterMappings[dateFilter].has(postIndex);
}
```

### Unified Tag-Based Architecture

**Implementation**: Uses pre-calculated server-side tag relationships for consistent, fast filtering:

```javascript
// Server generates tag relationships, JavaScript uses them directly
const relatedPostIndices = window.tagRelationships[tag];
const isMatch = relatedPostIndices.includes(postIndex);
```

**Examples of tag-based filtering (all work the same way)**:

- Section tags: "ai", "github copilot"
- Collection tags: "news", "posts", "videos", "community"
- Content tags: "azure", "visual studio code", "machine learning"

**Subset matching examples**:

- Selecting "ai" shows: "ai", "generative ai", "azure ai", "ai agents"
- Selecting "visual studio" shows: "visual studio", "visual studio code", "visual studio 2022"
- Selecting "azure" shows: "azure", "azure devops", "azure ai", "azure functions"

**Unified approach benefits**:

```javascript
// All filter types use the same logic - no special cases needed
function checkFilterForCurrentMode(postIndex, filters) {
    return filters.every(filter => {
        return window.tagRelationships[filter] &&
               window.tagRelationships[filter].includes(postIndex);
    });
}
```

### Date Filter Button Logic and Error Handling

**Client-Side Date Filter Count Calculation**:

Date filter counts (e.g., "Today", "Last 3 days") are always recalculated client-side in JavaScript, using the user's local timezone. This ensures that date-based filtering reflects the user's actual day, not the server's timezone.

**Rules for Hiding/Disabling Date Filter Buttons**:

- Date filter buttons are only hidden if there are truly zero posts for that date range in the user's timezone (not just due to tag filters)
- If a date filter has zero posts due to active tag filters, the button remains visible (with a count of 0), so users can see the effect of their selections
- This logic applies to all date filter buttons, not just "Today"

**Error Handling**:

- Always handle edge cases where filter data may be missing, malformed, or incomplete
- Gracefully degrade if required data is not present (e.g., show a warning, do not throw unhandled exceptions)
- Add tests for error handling and edge cases to ensure robust behavior

## Performance and Mobile Optimization

### DOM Manipulation Best Practices

**Performance Optimization**:

- **DOM Manipulation**: Show/hide existing elements (no re-rendering)
- **Event Delegation**: Use minimal event listeners for better performance
- **Debounced Updates**: Prevent excessive recalculations during rapid user interactions
- **Minimize client-side JavaScript execution**: Move heavy processing to server-side when possible
- **Optimize JavaScript queries**: Use efficient DOM selectors and caching
- **Monitor memory usage**: Check for memory leaks in long-running sessions

**Data Embedding**:

JavaScript relies on server-embedded data:

```javascript
// Check if window.filterData is properly set
console.log(window.filterData);
```

**Critical Requirements**:

- Verify server-rendered HTML structure
- Test event delegation setup
- Ensure data attributes are properly set on content elements

### Mobile and Touch Considerations

**Touch Events**:

- **Handle both mouse and touch events appropriately**: Ensure compatibility across all devices
- **Touch-friendly interfaces**: Consider touch target sizes and interaction patterns

**Mobile Performance**:

- **Optimize JavaScript for mobile devices**: Minimize processing overhead
- **Test on mobile networks**: Ensure acceptable performance on slower connections
- **Touch optimization**: Ensure touch interactions are responsive
- **Cross-browser compatibility**: Test across different browsers and versions
- **Mobile browsers**: Verify touch interactions work properly on mobile browsers

### Performance Monitoring

**Real-time Performance Monitoring**:

```javascript
// Monitor JavaScript performance
console.time('filter-operation');
// ... filtering logic
console.timeEnd('filter-operation');

// Check memory usage
console.log('Memory usage:', performance.memory);
```

**Production Performance Monitoring**:

```javascript
// Monitor filter performance
function performanceAwareFilter(filterFunction) {
    console.time('filter-operation');
    filterFunction();
    console.timeEnd('filter-operation');
    
    // Check memory usage if available
    if (performance.memory) {
        console.log('Memory usage:', {
            used: Math.round(performance.memory.usedJSHeapSize / 1048576) + ' MB',
            total: Math.round(performance.memory.totalJSHeapSize / 1048576) + ' MB'
        });
    }
}

// Usage
performanceAwareFilter(() => executeFilterToggle('ai', 'tag'));
```

## Error Handling and Debugging

### Common JavaScript Issues

**Symptom**: Filters not working or console errors

**Common Causes**:

- Missing or incorrect data embedding  
- Event listener setup issues
- DOM manipulation errors

**Debugging Steps**:

1. Check if `window.filterData` is properly set
2. Verify server-rendered HTML structure  
3. Test event delegation
4. Check browser console for specific errors

### Browser Debugging Techniques

**Basic debugging**:

```javascript
// Check filter data
console.log(window.filterData);

// Test DOM structure
console.log(document.querySelectorAll('[data-tag]'));

// Monitor event handling
document.addEventListener('click', function(e) {
    console.log('Clicked:', e.target);
});
```

**Advanced debugging for filtering**:

```javascript
// Check embedded data
console.log(window.filterData);

// Monitor filter interactions
window.filterData.posts.forEach(post => {
    console.log(`Post: ${post.title}, Tags: ${post.tags}`);
});

// Verify data attributes
document.querySelectorAll('[data-tags]').forEach(element => {
    console.log('Element tags:', element.dataset.tags);
});
```

## Integration with Jekyll System

### Timezone Consistency

**Critical Requirement**:

- **Server-side and client-side consistency**: Server generates epoch timestamps; client uses visitor's local timezone for better UX
- **Europe/Brussels timezone**: Server-side date processing uses this timezone consistently
- **Date Filter Logic**: Ensure date calculations use epoch timestamps consistently

For comprehensive date and timezone processing guidelines, see [Date and Timezone Processing](datetime-processing.md).

**Server-side (Liquid) date handling:**

```liquid
{%- assign post_epoch = post.date | date_to_epoch -%}
{%- assign now_epoch = '' | now_epoch -%}
{%- if post_epoch >= now_epoch -%}
  <!-- Post is from today or future -->
{%- endif -%}
```

**Client-side (JavaScript) date handling:**

```javascript
// Use visitor's local timezone for better user experience
function isWithinDateFilter(postEpoch, days) {
  const now = new Date();
  const today = new Date(now);
  today.setHours(0, 0, 0, 0);
  const todayStart = Math.floor(today.getTime() / 1000);
  
  if (days === 0) {
    const todayEnd = todayStart + 86399;
    return postEpoch >= todayStart && postEpoch <= todayEnd;
  } else {
    const daysInSeconds = (days - 1) * 86400;
    const cutoffEpoch = todayStart - daysInSeconds;
    return postEpoch >= cutoffEpoch;
  }
}
```

### Data Flow Dependencies

```text
Enhanced markdown → tag_filters.rb → Dynamic tag processing
     ↓
Liquid templates → Server-rendered HTML with embedded data  
     ↓
JavaScript → Interactive filtering
```

### Critical Integration Points

**Server Rendering → Client Enhancement**:

- Server renders complete functional page
- JavaScript enhances without breaking functionality
- All filtering logic must work in both server and client contexts

**Data Embedding Patterns**:

```javascript
// Verify window.filterData structure
console.log(window.filterData); // Should contain posts, tags, etc.

// Check data attributes on content elements
document.querySelectorAll('[data-tags]').forEach(element => {
    console.log('Tags:', element.dataset.tags);
    console.log('Date:', element.dataset.date);
});
```

**Server-side data preparation for JavaScript**:

- All filter data must be embedded in HTML during server-side rendering
- Data attributes must be properly set on filterable content elements
- JavaScript should never fetch additional data - everything should be pre-embedded

## Advanced JavaScript Patterns

### Event Delegation Strategies

```javascript
// Efficient event delegation pattern
document.addEventListener('click', function(e) {
    // Handle tag filter clicks
    if (e.target.classList.contains('tag-filter-btn')) {
        executeFilterToggle(e.target.dataset.tag, e.target.dataset.filterType);
    }
    
    // Handle date filter clicks
    if (e.target.classList.contains('date-filter-btn')) {
        applyDateFilter(e.target.dataset.days);
    }
});
```

### Component-Based Development

**CSS Integration**:

- **Component-Based CSS**: Organize CSS by component for maintainability
- **JavaScript alignment**: Ensure JavaScript interactions align with CSS component structure

**Code Organization**:

- **Modular approach**: Structure JavaScript to align with Jekyll component system
- **Maintainable code**: Write clear, documented JavaScript for team collaboration

## Export/Testability Pattern for JavaScript Modules

To ensure all production logic is testable, export any function that needs to be tested directly from the module. Use ES module `export` syntax or assign functions to `module.exports` (for CommonJS) as appropriate. Never duplicate or reimplement production logic in test files—always test the real exported function.

**Example (ES Module):**

```javascript
// In assets/js/filters.js
export function calculateDateFilterCounts(posts, dateFilters, timezone) {
  // ...implementation...
}

// In spec/javascript/filters.test.js
import { calculateDateFilterCounts } from '../../assets/js/filters.js';
```

**Example (CommonJS):**

```javascript
// In assets/js/sections.js
function getSectionFromUrl(url) { /* ... */ }
module.exports = { getSectionFromUrl };

// In spec/javascript/sections.test.js
const { getSectionFromUrl } = require('../../assets/js/sections.js');
```

**Guidelines:**

- Only export functions that are part of the public API or need to be tested
- Never export functions solely for test coverage if they are not used in production
- If a function is not accessible for testing, refactor the code to make it testable or ask for help

## Unit Testing

This section covers JavaScript unit testing standards using Jest for Phase 4 testing in the Tech Hub filtering system.

### Testing Framework

**Framework**: Jest with jsdom
**Purpose**: Unit testing client-side filtering logic, state management, and data processing without browser dependencies
**Test Location**: `spec/javascript/`
**Test Execution**: Use `/workspaces/techhub/run-javascript-tests.ps1` wrapper script

### Test Structure and Organization

```text
spec/javascript/
├── package.json              # Jest configuration and dependencies
├── test-setup.js             # Global mocks and test utilities
├── filters.test.js        # Core filtering logic tests
├── sections.test.js          # Section navigation tests
└── [module-name].test.js     # Additional module tests
```

### Running JavaScript Tests

```powershell
# Run all JavaScript tests using wrapper script

pwsh /workspaces/techhub/javascript-tests.ps1

# Run specific test file

pwsh /workspaces/techhub/run-javascript-tests.ps1 -TestFile "filters.test.js"

# Run with coverage analysis

pwsh /workspaces/techhub/run-javascript-tests.ps1 -Coverage

# Run tests matching name pattern

pwsh /workspaces/techhub/run-javascript-tests.ps1 -TestName "filter logic"

# Run in watch mode for development

pwsh /workspaces/techhub/run-javascript-tests.ps1 -Watch

# Run with verbose output

pwsh /workspaces/techhub/run-javascript-tests.ps1 -Verbose
```

### Test Coverage Requirements

**JavaScript tests should cover**:

- **Filter Logic**: Tag filtering, date filtering, and filter combinations
- **State Management**: Filter state, URL synchronization, and data transformations
- **Data Processing**: Tag relationships, date calculations, and utility functions
- **Error Handling**: Invalid inputs and graceful degradation
- **Edge Cases**: Empty data, boundary conditions, and special scenarios

**JavaScript tests should NOT cover**:

- DOM manipulation (use mocks instead)
- Browser-specific APIs (focus on logic)
- Jekyll data generation (belongs in Ruby tests)
- Visual behavior (belongs in Playwright tests)
- Network requests (mock external dependencies)

### Test Environment Setup

**Jest configuration in package.json**:

```json
{
  "name": "tech-hub-js-tests",
  "scripts": {
    "test": "jest",
    "test:watch": "jest --watch",
    "test:coverage": "jest --coverage"
  },
  "jest": {
    "testEnvironment": "jsdom",
    "setupFilesAfterEnv": ["<rootDir>/test-setup.js"],
    "collectCoverageFrom": [
      "../../assets/js/**/*.js",
      "!../../assets/js/**/*.min.js"
    ],
    "testMatch": ["**/*.test.js"],
    "clearMocks": true,
    "restoreMocks": true
  }
}
```

**Global test setup in test-setup.js**:

```javascript
// Global mocks for browser environment
global.window = {
  activeFilters: new Set(),
  isUpdating: false,
  location: {
    href: 'http://localhost/',
    search: '',
    pathname: '/'
  },
  history: {
    pushState: jest.fn(),
    replaceState: jest.fn()
  }
};

global.document = {
  querySelectorAll: jest.fn(() => []),
  querySelector: jest.fn(() => null),
  addEventListener: jest.fn()
};

// Mock data factories
global.createMockFilterData = function() {
  return [
    {
      epoch: Math.floor(Date.now() / 1000),
      tags: ['ai', 'github copilot', 'visual studio code'],
      categories: ['ai'],
      collection: 'news'
    },
    {
      epoch: Math.floor(Date.now() / 1000) - 86400,
      tags: ['azure', 'dotnet', 'development'],
      categories: ['ai'],
      collection: 'posts'
    }
  ];
};

global.createMockTagRelationships = function() {
  return {
    'ai': [0, 1],
    'github copilot': [0],
    'azure': [1],
    'visual studio code': [0],
    'dotnet': [1]
  };
};

// Reset function for test isolation
global.resetGlobalFilterState = function() {
  global.window.activeFilters = new Set();
  global.window.isUpdating = false;
};
```

### Test Writing Standards

**Use descriptive Jest test structure with proper organization**:

- Structure tests with `describe` blocks for logical grouping
- Use meaningful test names that explain expected behavior
- Follow Arrange-Act-Assert pattern for clarity
- Use `beforeEach` for test setup and data reset

**Key Test Categories**:

- **Filter Logic**: Test tag filtering, date filtering, and combinations
- **State Management**: Test filter state operations without DOM dependencies
- **URL Synchronization**: Test URL parameter handling and state persistence
- **Performance**: Test efficiency with large datasets
- **Error Handling**: Test graceful degradation and edge cases

### Test Coverage Focus Areas

**Core Testing Requirements**:

- **Filter Logic**: Tag filtering, date filtering, and filter combinations
- **State Management**: Filter state, URL synchronization, and data transformations
- **Data Processing**: Tag relationships, date calculations, and utility functions
- **Error Handling**: Invalid inputs and graceful degradation
- **Edge Cases**: Empty data, boundary conditions, and special scenarios

**Assertion Guidelines**:

- Use specific Jest matchers: `toHaveLength()`, `toEqual()`, `toMatchObject()`
- Prefer boolean checks: `toBe(true)`, `toBeDefined()`, `not.toBeNull()`
- Use function call verification: `toHaveBeenCalledWith()`, `toHaveBeenCalledTimes()`
- Include type checking when appropriate: `typeof`, `Array.isArray()`
- Avoid vague assertions: `toBeTruthy()`, `toBeFalsy()`

**Test Documentation Standards**:

- Write self-documenting test names that explain exact behavior
- Use descriptive describe blocks for logical organization
- Include comments for complex test setup or assertions
- Avoid implementation-focused or vague test names

### Integration with CI/CD

**JavaScript tests in the testing pipeline**:

1. **Fast Execution**: JavaScript tests run early for quick feedback
2. **Isolated Environment**: Tests run in jsdom without browser overhead
3. **Coverage Tracking**: Monitor test coverage for client-side logic
4. **Parallel Execution**: Jest runs tests in parallel for speed

**CI execution example**:

```powershell
# CI pipeline step

pwsh /workspaces/techhub/run-javascript-tests.ps1 -Coverage -Verbose
```

This testing approach ensures client-side filtering logic is thoroughly validated in isolation, providing confidence in JavaScript functionality before integration testing with Playwright.
