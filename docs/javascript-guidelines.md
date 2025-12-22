# JavaScript Development Guidelines

This document provides comprehensive reference material for JavaScript development in the Tech Hub site. For action-oriented guidance and critical rules, see [assets/js/AGENTS.md](../assets/js/AGENTS.md).

## Overview

The Tech Hub uses vanilla ES6+ JavaScript for client-side functionality. JavaScript enhances server-rendered content with real-time filtering, search, and navigation features while maintaining a server-first architecture.

For detailed implementation patterns and code examples, refer to [assets/js/AGENTS.md](../assets/js/AGENTS.md).

## Architecture Principles

### Server-Side First

**Critical Rule**: JavaScript must never render initial page content (exception: `assets/js/sections.js` only for navigation state).

Jekyll/Liquid renders all initial content server-side. JavaScript adds:
1. Event listeners and interactivity
2. Real-time filtering without page reload
3. State management in URL parameters
4. Search functionality

### File Organization

```text
assets/js/
├── sections.js    # Navigation state (runs on page load)
├── filters.js     # Tag-based filtering system
├── features.js    # Feature sections interactivity
└── logo-manager.js # Logo variant management
```

## Filtering System Architecture

For complete implementation details, see [filtering-system.md](filtering-system.md) and [assets/js/AGENTS.md](../assets/js/AGENTS.md).

### Pre-Calculated Relationships

The filtering system uses server-side pre-calculated tag relationships for performance:

- **Tag Relationships**: Server maps which posts match each tag
- **Date Mappings**: Server calculates date ranges at build time
- **Client Lookups**: JavaScript performs fast array lookups without recalculation

This architecture ensures filtering remains performant even with hundreds of posts.

### Text Search with Debouncing

Text search provides real-time content filtering with a 300ms debounce delay to prevent excessive updates during typing. Content is pre-indexed during page load for fast matching.

## Performance Considerations

### DOM Manipulation

- Show/hide existing elements (no re-rendering)
- Use event delegation for minimal listeners
- Cache DOM queries
- Minimize reflows and repaints

### Mobile Optimization

- Touch-friendly target sizes
- Handle both mouse and touch events
- Optimize for mobile performance
- Test on various devices and browsers

## Testing

For JavaScript testing patterns, see [spec/AGENTS.md](../spec/AGENTS.md).

Tests use Jest with jsdom for unit testing client-side logic without browser dependencies:

```bash
# Run JavaScript tests
./scripts/run-javascript-tests.ps1

# With coverage
npm test -- --coverage
```

## Resources

- [assets/js/AGENTS.md](../assets/js/AGENTS.md) - Action-oriented JavaScript development guidance
- [filtering-system.md](filtering-system.md) - Complete filtering system architecture
- [spec/AGENTS.md](../spec/AGENTS.md) - Testing strategy and patterns
- [performance-guidelines.md](performance-guidelines.md) - Performance optimization strategies

**Symptom**: Filters not working or console errors

**Common Causes**:

- Missing or incorrect data embedding  
- Event listener setup issues
- DOM manipulation errors

**Debugging Steps**:

1. Check if `window.currentFilterData` is properly set
2. Verify server-rendered HTML structure  
3. Test event delegation
4. Check browser console for specific errors

### Browser Debugging Techniques

**Basic debugging**:

```javascript
// Check filter data
console.log(window.currentFilterData);

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
console.log(window.currentFilterData);

// Monitor filter interactions
window.currentFilterData.posts.forEach(post => {
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
// Verify window.currentFilterData structure
console.log(window.currentFilterData); // Should contain posts, tags, etc.

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

- **Filter Logic**: Tag filtering, date filtering, text search, and filter combinations
- **State Management**: Filter state, URL synchronization, and data transformations
- **Data Processing**: Tag relationships, date calculations, text search indexing, and utility functions
- **Text Search**: Content indexing, search query processing, and debounced input handling
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

- **Filter Logic**: Test tag filtering, date filtering, text search, and combinations
- **State Management**: Test filter state operations without DOM dependencies
- **URL Synchronization**: Test URL parameter handling and state persistence
- **Text Search**: Test content indexing, search logic, and debounced input
- **Performance**: Test efficiency with large datasets
- **Error Handling**: Test graceful degradation and edge cases

### Test Coverage Focus Areas

**Core Testing Requirements**:

- **Filter Logic**: Tag filtering, date filtering, text search, and filter combinations
- **State Management**: Filter state, URL synchronization, and data transformations
- **Data Processing**: Tag relationships, date calculations, text search indexing, and utility functions
- **Text Search**: Content indexing, query processing, and performance optimization
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
