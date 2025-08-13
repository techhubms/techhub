# Frontend Development Guidelines

This document covers frontend development standards for the Tech Hub, including responsive design, accessibility, and end-to-end testing practices.

## Frontend Architecture Overview

The Tech Hub follows a **server-side first** approach with progressive enhancement:

1. **Server-Side Rendering**: Jekyll generates complete, functional pages
2. **Progressive Enhancement**: JavaScript adds interactivity and filtering
3. **Responsive Design**: Mobile-first CSS with breakpoint-based layouts
4. **Accessibility**: WCAG-compliant markup and keyboard navigation
5. **Performance**: Optimized assets and fast page load times

## CSS and Styling Standards

### CSS Organization

**CRITICAL**: Never use inline CSS styles in HTML templates or components. All styling must be defined in SCSS files within the `_sass/` directory.

**Why This Matters**:

- **Maintainability**: Centralized styling is easier to update and debug
- **Performance**: External stylesheets can be cached by browsers
- **Consistency**: SCSS variables and mixins ensure design consistency
- **Separation of Concerns**: Keeps presentation logic separate from markup

**Correct Approach**:

```scss
// In _sass/components/_footer.scss
.site-footer {
  .wrapper {
    display: flex;
    justify-content: space-between;
    align-items: center;
    
    .author, .rss-subscribe {
      margin: 0;
    }
  }
}
```

**Avoid**:

```html
<!-- Never use inline styles -->
<div style="display: flex; justify-content: space-between;">
  <p style="margin: 0;">Content</p>
</div>
```

## Responsive Design Standards

### Breakpoint Strategy

**Mobile-First Approach**: Design for mobile devices first, then enhance for larger screens.

```scss
// Breakpoint variables (from main.scss)
$content-width: 900px;
$on-mobile: 800px;
$on-laptop: 900px;

// Mobile-first media queries
.component {
  // Mobile styles (default)
  padding: 1rem;
  
  @media (min-width: $on-mobile) {
    // Tablet styles
    padding: 1.5rem;
  }
  
  @media (min-width: $on-laptop) {
    // Desktop styles
    padding: 2rem;
  }
}
```

### Grid System

**Use CSS Grid for layout with Flexbox for components**:

```scss
// Main content grid
.content-grid {
  display: grid;
  grid-template-columns: 1fr;
  gap: 1rem;
  
  @media (min-width: $tablet-min) {
    grid-template-columns: 250px 1fr;
    gap: 2rem;
  }
  
  @media (min-width: $desktop-min) {
    grid-template-columns: 300px 1fr;
    gap: 3rem;
  }
}

// Component-level flexbox
.filter-bar {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  
  @media (min-width: $tablet-min) {
    flex-direction: row;
    flex-wrap: wrap;
  }
}
```

### Component Responsiveness

**Design components to work across all device sizes**:

```scss
// Filter buttons - responsive sizing
.filter-btn {
  padding: 0.5rem 1rem;
  font-size: 0.9rem;
  min-height: 44px; // Touch target minimum
  
  @media (min-width: $tablet-min) {
    padding: 0.625rem 1.25rem;
    font-size: 1rem;
  }
  
  @media (min-width: $desktop-min) {
    padding: 0.75rem 1.5rem;
  }
}

// Card layout - responsive stacking
.post-card {
  display: flex;
  flex-direction: column;
  
  @media (min-width: $tablet-min) {
    flex-direction: row;
    align-items: center;
  }
}
```

## Accessibility Standards

### Semantic HTML

**Use proper HTML5 semantic elements**:

```html
<!-- Good: Semantic structure -->
<main>
  <section aria-labelledby="filters-heading">
    <h2 id="filters-heading">Content Filters</h2>
    <div class="filter-controls" role="group" aria-labelledby="filters-heading">
      <button type="button" aria-pressed="false" data-filter="ai">
        AI <span class="filter-count" aria-label="5 posts">(5)</span>
      </button>
    </div>
  </section>
  
  <section aria-labelledby="results-heading" aria-live="polite">
    <h2 id="results-heading">Results</h2>
    <ul class="post-list">
      <li class="post-item">
        <article>
          <h3><a href="/post-url">Post Title</a></h3>
          <p>Post excerpt...</p>
        </article>
      </li>
    </ul>
  </section>
</main>
```

### ARIA Labels and States

**Provide clear ARIA labels for interactive elements**:

```html
<!-- Filter buttons with state -->
<button
  type="button"
  class="filter-btn"
  aria-pressed="false"
  aria-describedby="ai-filter-desc"
  data-filter="ai">
  AI <span class="filter-count">(12)</span>
</button>
<div id="ai-filter-desc" class="sr-only">
  Filter content by AI topic. Currently 12 posts available.
</div>

<!-- Live region for dynamic updates -->
<div
  aria-live="polite"
  aria-atomic="true"
  class="filter-status sr-only">
  Showing 8 of 45 posts
</div>

<!-- Skip links for keyboard navigation -->
<a href="#main-content" class="skip-link">Skip to main content</a>
<a href="#filter-controls" class="skip-link">Skip to filters</a>
```

### Keyboard Navigation

**Ensure all interactive elements are keyboard accessible**:

```javascript
// Enhanced keyboard navigation for filters
function enhanceKeyboardNavigation() {
  const filterButtons = document.querySelectorAll('.filter-btn');
  
  filterButtons.forEach((button, index) => {
    button.addEventListener('keydown', (event) => {
      switch (event.key) {
        case 'ArrowRight':
        case 'ArrowDown':
          event.preventDefault();
          const nextButton = filterButtons[index + 1] || filterButtons[0];
          nextButton.focus();
          break;
          
        case 'ArrowLeft':
        case 'ArrowUp':
          event.preventDefault();
          const prevButton = filterButtons[index - 1] || filterButtons[filterButtons.length - 1];
          prevButton.focus();
          break;
          
        case 'Home':
          event.preventDefault();
          filterButtons[0].focus();
          break;
          
        case 'End':
          event.preventDefault();
          filterButtons[filterButtons.length - 1].focus();
          break;
      }
    });
  });
}
```

### Focus Management

**Manage focus appropriately during dynamic content updates**:

```javascript
// Focus management for filter updates
function updateFilterResults(activeFilters) {
  const resultsContainer = document.querySelector('[aria-live="polite"]');
  const visiblePosts = applyFilters(activeFilters);
  
  // Update results
  renderFilteredPosts(visiblePosts);
  
  // Announce changes to screen readers
  const announcement = `Showing ${visiblePosts.length} of ${totalPosts} posts`;
  announceToScreenReader(announcement);
  
  // Move focus to results if user is navigating with keyboard
  if (document.activeElement && document.activeElement.classList.contains('filter-btn')) {
    const firstResult = document.querySelector('.post-item h3 a');
    if (firstResult && visiblePosts.length > 0) {
      // Optional: move focus to first result for better UX
      // firstResult.focus();
    }
  }
}

function announceToScreenReader(message) {
  const announcer = document.querySelector('.filter-status[aria-live]');
  if (announcer) {
    announcer.textContent = message;
  }
}
```

## Performance Standards

### Page Load Performance

**Target 200ms page load time budget**:

```javascript
// Performance monitoring
function checkPageLoadTime() {
  return new Promise((resolve) => {
    if (performance.timing.loadEventEnd) {
      const loadTime = performance.timing.loadEventEnd - performance.timing.navigationStart;
      resolve({
        loadTime,
        passesRequirement: loadTime < 200
      });
    } else {
      window.addEventListener('load', () => {
        const loadTime = performance.timing.loadEventEnd - performance.timing.navigationStart;
        resolve({
          loadTime,
          passesRequirement: loadTime < 200
        });
      });
    }
  });
}
```

### Asset Optimization

**Minimize and optimize CSS/JavaScript assets**:

```scss
// Use efficient CSS selectors
.post-item {
  // Avoid expensive selectors like:
  // * + * + .post-item (complex combinators)
  // [data-filter*="ai"] (partial attribute matches)
}

// Optimize animation performance
.filter-btn {
  transition: background-color 0.2s ease;
  // Avoid animating expensive properties like:
  // transition: all 0.3s ease; (too broad)
  // transition: box-shadow 0.3s ease; (expensive)
}
```

### Lazy Loading and Code Splitting

**Load resources only when needed**:

```javascript
// Lazy load non-critical JavaScript
function loadFilterEnhancements() {
  if (!window.filterSystemLoaded) {
    import('./advanced-filters.js').then(module => {
      module.initAdvancedFilters();
      window.filterSystemLoaded = true;
    });
  }
}

// Intersection Observer for lazy loading
function setupLazyLoading() {
  const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
      if (entry.isIntersecting && !entry.target.dataset.loaded) {
        loadContentForElement(entry.target);
        entry.target.dataset.loaded = 'true';
      }
    });
  });
  
  document.querySelectorAll('[data-lazy-load]').forEach(el => {
    observer.observe(el);
  });
}
```

## E2E Testing

This section covers end-to-end testing standards using Playwright for Phase 5 testing in the Tech Hub filtering system.

### Testing Framework

**Framework**: Playwright
**Purpose**: End-to-end testing of complete user workflows, browser compatibility, and visual behavior
**Test Location**: `spec/e2e/tests/`
**Test Execution**: Use `/workspaces/techhub/run-e2e-tests.ps1` wrapper script

### Test Structure and Organization

```text
spec/e2e/tests/
├── helpers.js                      # Shared test utilities and helpers
├── basic-functionality.spec.js     # Core site functionality
├── content-display.spec.js         # Content rendering and display
├── filtering-advanced.spec.js      # Advanced filtering scenarios
├── filtering-core.spec.js          # Core filtering functionality
├── filtering-counts.spec.js        # Filter count accuracy
├── filtering-page-types.spec.js    # Page-specific filtering
├── filtering-server-side.spec.js   # Server-side filter validation
├── filtering-ui.spec.js            # Filter UI interactions
├── hamburger-menu.spec.js          # Mobile menu functionality
├── performance-accessibility.spec.js # Performance and a11y testing
├── responsive-design.spec.js       # Responsive behavior validation
└── section-navigation.spec.js      # Section navigation testing
```

### Running E2E Tests

```powershell
# Run all E2E tests using wrapper script

pwsh /workspaces/techhub/run-e2e-tests.ps1

# Run specific test file

pwsh /workspaces/techhub/run-e2e-tests.ps1 -TestFile "filtering-core.spec.js"

# Run tests matching pattern

pwsh /workspaces/techhub/run-e2e-tests.ps1 -Grep "tag filtering"

# Run in debug mode for step-by-step execution

pwsh /workspaces/techhub/run-e2e-tests.ps1 -Debug

# Run with interactive UI mode

pwsh /workspaces/techhub/run-e2e-tests.ps1 -UI

# Run with verbose output

pwsh /workspaces/techhub/run-e2e-tests.ps1 -Verbose
```

### Test Coverage Requirements

**E2E tests should cover**:

- **User Workflows**: Complete user interaction scenarios from start to finish
- **Cross-Browser Compatibility**: Ensure functionality works across different browsers
- **Responsive Behavior**: Test layouts and interactions on different screen sizes
- **Performance Validation**: Verify page load times and interaction responsiveness
- **Accessibility Testing**: Basic keyboard navigation and screen reader compatibility
- **Error Handling**: Graceful degradation when things go wrong
- **Visual Regression**: Ensure UI changes don't break existing layouts

**E2E tests should NOT cover**:

- Unit logic testing (belongs in unit tests)
- Internal implementation details
- Mock data scenarios (use real Jekyll-generated content)
- Fine-grained algorithm testing

### Test Writing Standards

**Use descriptive Playwright test structure**:

```javascript
import { test, expect } from '@playwright/test';
import {
  checkPageLoadTime,
  checkForJavaScriptErrors,
  loadSectionsConfig
} from './helpers.js';

test.describe('Core Filtering Functionality', () => {
  test.beforeEach(async ({ page }) => {
    // Ensure clean state before each test
    await page.goto('/');
    await page.waitForLoadState('networkidle');
  });

  test.describe('Tag Filtering', () => {
    test('should filter content by single tag', async ({ page }) => {
      // Arrange
      await page.goto('/ai/news.html');
      const initialPostCount = await page.locator('.post-item').count();
      
      // Act
      await page.click('[data-filter="github copilot"]');
      await page.waitForSelector('.post-item:visible');
      
      // Assert
      const filteredPostCount = await page.locator('.post-item:visible').count();
      expect(filteredPostCount).toBeLessThan(initialPostCount);
      expect(filteredPostCount).toBeGreaterThan(0);
      
      // Verify URL updated
      await expect(page).toHaveURL(/tags=github%20copilot/);
      
      // Verify button state
      await expect(page.locator('[data-filter="github copilot"]')).toHaveAttribute('aria-pressed', 'true');
    });

    test('should combine multiple tag filters with AND logic', async ({ page }) => {
      // Arrange
      await page.goto('/ai/news.html');
      
      // Act - Apply first filter
      await page.click('[data-filter="ai"]');
      await page.waitForSelector('.post-item:visible');
      const firstFilterCount = await page.locator('.post-item:visible').count();
      
      // Act - Apply second filter
      await page.click('[data-filter="github copilot"]');
      await page.waitForSelector('.post-item:visible');
      const combinedFilterCount = await page.locator('.post-item:visible').count();
      
      // Assert
      expect(combinedFilterCount).toBeLessThanOrEqual(firstFilterCount);
      
      // Verify both filters are active
      await expect(page.locator('[data-filter="ai"]')).toHaveAttribute('aria-pressed', 'true');
      await expect(page.locator('[data-filter="github copilot"]')).toHaveAttribute('aria-pressed', 'true');
      
      // Verify URL contains both filters
      await expect(page).toHaveURL(/tags=.*ai.*github%20copilot/);
    });

    test('should handle filter removal correctly', async ({ page }) => {
      // Arrange
      await page.goto('/ai/news.html');
      await page.click('[data-filter="ai"]');
      await page.waitForSelector('.post-item:visible');
      
      // Act
      await page.click('[data-filter="ai"]'); // Remove filter
      await page.waitForSelector('.post-item:visible');
      
      // Assert
      await expect(page.locator('[data-filter="ai"]')).toHaveAttribute('aria-pressed', 'false');
      await expect(page).toHaveURL(url => !url.includes('tags='));
      
      // Verify all posts are visible again
      const allPosts = await page.locator('.post-item').count();
      const visiblePosts = await page.locator('.post-item:visible').count();
      expect(visiblePosts).toBe(allPosts);
    });
  });

  test.describe('Date Filtering', () => {
    test('should apply date filters correctly', async ({ page }) => {
      // Arrange
      await page.goto('/ai/news.html');
      const initialCount = await page.locator('.post-item:visible').count();
      
      // Act
      await page.click('[data-filter-type="date"][data-filter="last 7 days"]');
      await page.waitForSelector('.post-item:visible');
      
      // Assert
      const filteredCount = await page.locator('.post-item:visible').count();
      expect(filteredCount).toBeLessThanOrEqual(initialCount);
      
      // Verify button state
      const dateButton = page.locator('[data-filter-type="date"][data-filter="last 7 days"]');
      await expect(dateButton).toHaveAttribute('aria-pressed', 'true');
      
      // Verify URL includes date filter
      await expect(page).toHaveURL(/date=last%207%20days/);
    });

    test('should disable date filters when no posts in range', async ({ page }) => {
      // Navigate to section with limited content
      await page.goto('/community/roundups.html');
      
      // Check if buttons are properly disabled based on actual content
      const last7DaysButton = page.locator('[data-filter="last 7 days"]');
      const buttonText = await last7DaysButton.textContent();
      
      if (buttonText.includes('(0)')) {
        await expect(last7DaysButton).toBeDisabled();
      }
    });
  });

  test.describe('Clear All Functionality', () => {
    test('should clear all active filters', async ({ page }) => {
      // Arrange - Apply multiple filters
      await page.goto('/ai/news.html');
      await page.click('[data-filter="ai"]');
      await page.click('[data-filter="github copilot"]');
      await page.click('[data-filter-type="date"][data-filter="last 7 days"]');
      
      // Wait for filters to be applied
      await page.waitForSelector('.post-item:visible');
      
      // Act
      await page.click('[data-action="clear-all"]');
      await page.waitForSelector('.post-item:visible');
      
      // Assert
      // All filter buttons should be inactive
      const activeFilters = await page.locator('[aria-pressed="true"]').count();
      expect(activeFilters).toBe(0);
      
      // URL should not contain filter parameters
      await expect(page).toHaveURL(url => !url.includes('tags=') && !url.includes('date='));
      
      // All posts should be visible
      const allPosts = await page.locator('.post-item').count();
      const visiblePosts = await page.locator('.post-item:visible').count();
      expect(visiblePosts).toBe(allPosts);
    });
  });
});
```

### Responsive Design Testing

**Test layouts across different screen sizes**:

```javascript
import { test, expect, devices } from '@playwright/test';

// Test across multiple device configurations
const deviceConfigs = [
  { name: 'Mobile', ...devices['iPhone 12'] },
  { name: 'Tablet', ...devices['iPad'] },
  { name: 'Desktop', viewport: { width: 1280, height: 720 } }
];

deviceConfigs.forEach(({ name, ...deviceConfig }) => {
  test.describe(`Responsive Design - ${name}`, () => {
    test.use(deviceConfig);

    test('should display hamburger menu on mobile', async ({ page }) => {
      if (name === 'Mobile') {
        await page.goto('/');
        
        // Mobile should show hamburger menu
        await expect(page.locator('.hamburger-menu')).toBeVisible();
        await expect(page.locator('.main-nav')).toBeHidden();
        
        // Test hamburger functionality
        await page.click('.hamburger-menu');
        await expect(page.locator('.main-nav')).toBeVisible();
      }
    });

    test('should adapt filter layout for screen size', async ({ page }) => {
      await page.goto('/ai/news.html');
      
      const filterContainer = page.locator('.filter-controls');
      await expect(filterContainer).toBeVisible();
      
      if (name === 'Mobile') {
        // Mobile should stack filters vertically
        const filterButtons = page.locator('.filter-btn');
        const firstButton = filterButtons.first();
        const secondButton = filterButtons.nth(1);
        
        const firstButtonBox = await firstButton.boundingBox();
        const secondButtonBox = await secondButton.boundingBox();
        
        // On mobile, second button should be below first button
        expect(secondButtonBox.y).toBeGreaterThan(firstButtonBox.y + firstButtonBox.height - 10);
      } else {
        // Tablet/Desktop should show filters in rows
        const filterButtons = page.locator('.filter-btn');
        const count = await filterButtons.count();
        
        if (count >= 2) {
          const firstButton = filterButtons.first();
          const secondButton = filterButtons.nth(1);
          
          const firstButtonBox = await firstButton.boundingBox();
          const secondButtonBox = await secondButton.boundingBox();
          
          // On larger screens, buttons should be roughly on same line
          expect(Math.abs(secondButtonBox.y - firstButtonBox.y)).toBeLessThan(20);
        }
      }
    });

    test('should maintain touch targets on mobile', async ({ page }) => {
      if (name === 'Mobile') {
        await page.goto('/ai/news.html');
        
        const filterButtons = page.locator('.filter-btn');
        const buttonCount = await filterButtons.count();
        
        for (let i = 0; i < Math.min(buttonCount, 5); i++) {
          const button = filterButtons.nth(i);
          const boundingBox = await button.boundingBox();
          
          // Touch targets should be at least 44px
          expect(boundingBox.height).toBeGreaterThanOrEqual(44);
          expect(boundingBox.width).toBeGreaterThanOrEqual(44);
        }
      }
    });
  });
});
```

### Performance Testing

**Validate page load times and performance metrics**:

```javascript
test.describe('Performance Validation', () => {
  test('should load homepage within performance budget', async ({ page }) => {
    const { loadTime, passesRequirement } = await checkPageLoadTime(page, '/');
    
    expect(passesRequirement).toBe(true);
    expect(loadTime).toBeLessThan(200); // 200ms budget
  });

  test('should load section pages efficiently', async ({ page }) => {
    const sectionsConfig = await loadSectionsConfig();
    
    for (const [sectionKey, sectionData] of Object.entries(sectionsConfig)) {
      const sectionUrl = `/${sectionKey}/`;
      const { loadTime, passesRequirement } = await checkPageLoadTime(page, sectionUrl);
      
      expect(passesRequirement).toBe(true);
      expect(loadTime).toBeLessThan(300); // Slightly higher budget for content pages
    }
  });

  test('should not have JavaScript errors', async ({ page }) => {
    const errors = await checkForJavaScriptErrors(page, '/');
    
    expect(errors).toHaveLength(0);
  });

  test('should handle rapid filter interactions', async ({ page }) => {
    await page.goto('/ai/news.html');
    
    // Rapidly click multiple filters
    const startTime = Date.now();
    await page.click('[data-filter="ai"]');
    await page.click('[data-filter="github copilot"]');
    await page.click('[data-filter="visual studio code"]');
    await page.click('[data-filter="azure"]');
    
    // Wait for all updates to complete
    await page.waitForSelector('.post-item:visible');
    const endTime = Date.now();
    
    const interactionTime = endTime - startTime;
    expect(interactionTime).toBeLessThan(1000); // Should complete within 1 second
    
    // UI should be stable (no JavaScript errors)
    const errors = await page.evaluate(() => window.jsErrors || []);
    expect(errors).toHaveLength(0);
  });
});
```

### Accessibility Testing

**Test keyboard navigation and screen reader compatibility**:

```javascript
test.describe('Accessibility Validation', () => {
  test('should support keyboard navigation', async ({ page }) => {
    await page.goto('/ai/news.html');
    
    // Focus first filter button
    await page.keyboard.press('Tab');
    const firstButton = page.locator('.filter-btn').first();
    await expect(firstButton).toBeFocused();
    
    // Navigate with arrow keys
    await page.keyboard.press('ArrowRight');
    const secondButton = page.locator('.filter-btn').nth(1);
    await expect(secondButton).toBeFocused();
    
    // Activate filter with Enter or Space
    await page.keyboard.press('Enter');
    await expect(secondButton).toHaveAttribute('aria-pressed', 'true');
  });

  test('should have proper ARIA labels', async ({ page }) => {
    await page.goto('/ai/news.html');
    
    // Check filter buttons have proper ARIA attributes
    const filterButtons = page.locator('.filter-btn');
    const buttonCount = await filterButtons.count();
    
    for (let i = 0; i < buttonCount; i++) {
      const button = filterButtons.nth(i);
      await expect(button).toHaveAttribute('aria-pressed');
      await expect(button).toHaveAttribute('type', 'button');
    }
    
    // Check live regions exist
    await expect(page.locator('[aria-live]')).toBeAttached();
  });

  test('should announce filter changes to screen readers', async ({ page }) => {
    await page.goto('/ai/news.html');
    
    // Monitor aria-live region
    const liveRegion = page.locator('[aria-live="polite"]');
    const initialText = await liveRegion.textContent();
    
    // Apply filter
    await page.click('[data-filter="ai"]');
    await page.waitForTimeout(100); // Allow time for announcement
    
    // Verify announcement updated
    const updatedText = await liveRegion.textContent();
    expect(updatedText).not.toBe(initialText);
    expect(updatedText).toMatch(/showing \d+ of \d+ posts/i);
  });

  test('should have sufficient color contrast', async ({ page }) => {
    await page.goto('/ai/news.html');
    
    // Test button contrast
    const filterButton = page.locator('.filter-btn').first();
    const styles = await filterButton.evaluate(el => {
      const computed = window.getComputedStyle(el);
      return {
        backgroundColor: computed.backgroundColor,
        color: computed.color
      };
    });
    
    // Basic check - buttons should have defined colors
    expect(styles.backgroundColor).not.toBe('rgba(0, 0, 0, 0)');
    expect(styles.color).not.toBe('rgba(0, 0, 0, 0)');
  });
});
```

### Error Handling Testing

**Test graceful degradation and error recovery**:

```javascript
test.describe('Error Handling', () => {
  test('should handle missing JavaScript gracefully', async ({ page }) => {
    // Disable JavaScript
    await page.addInitScript(() => {
      delete window.toggleFilter;
      delete window.clearAllFilters;
    });
    
    await page.goto('/ai/news.html');
    
    // Page should still be functional
    await expect(page.locator('.post-item')).toHaveCountGreaterThan(0);
    await expect(page.locator('.filter-btn')).toBeVisible();
    
    // Basic navigation should still work
    const firstPost = page.locator('.post-item h3 a').first();
    await expect(firstPost).toBeVisible();
    await firstPost.click();
    
    // Should navigate to post page
    await expect(page).toHaveURL(/\/(news|posts|community|videos)\//);
  });

  test('should handle network errors gracefully', async ({ page }) => {
    // Intercept and fail some requests
    await page.route('**/*.js', route => {
      if (route.request().url().includes('filters.js')) {
        route.abort();
      } else {
        route.continue();
      }
    });
    
    await page.goto('/ai/news.html');
    
    // Page should still load and display content
    await expect(page.locator('.post-item')).toHaveCountGreaterThan(0);
    
    // Check for error handling in console
    const errors = await page.evaluate(() => window.jsErrors || []);
    // Some errors are expected due to failed JS load
    console.log('Expected errors due to simulated network failure:', errors);
  });

  test('should handle invalid filter states in URL', async ({ page }) => {
    // Navigate with invalid filter parameters
    await page.goto('/ai/news.html?tags=nonexistent-tag&date=invalid-date');
    
    // Page should load normally
    await expect(page.locator('.post-item')).toHaveCountGreaterThan(0);
    
    // Invalid filters should be ignored
    const activeFilters = await page.locator('[aria-pressed="true"]').count();
    expect(activeFilters).toBe(0);
    
    // URL should be cleaned up
    await expect(page).toHaveURL('/ai/news.html');
  });
});
```

### Visual Regression Testing

**Test for unintended visual changes**:

```javascript
test.describe('Visual Regression', () => {
  test('should maintain consistent homepage layout', async ({ page }) => {
    await page.goto('/');
    await page.waitForLoadState('networkidle');
    
    // Take screenshot of homepage
    await expect(page).toHaveScreenshot('homepage-layout.png', {
      fullPage: true,
      threshold: 0.2 // Allow for minor differences
    });
  });

  test('should maintain consistent filter layout', async ({ page }) => {
    await page.goto('/ai/news.html');
    await page.waitForLoadState('networkidle');
    
    // Take screenshot of filter area
    await expect(page.locator('.filter-controls')).toHaveScreenshot('filter-controls.png');
  });

  test('should maintain consistent mobile layout', async ({ page }) => {
    await page.setViewportSize({ width: 375, height: 667 }); // iPhone SE
    await page.goto('/');
    await page.waitForLoadState('networkidle');
    
    await expect(page).toHaveScreenshot('mobile-layout.png', {
      fullPage: true,
      threshold: 0.2
    });
  });
});
```

### Integration with CI/CD

**E2E tests in the testing pipeline**:

1. **Final Validation**: E2E tests run last after all other test types pass
2. **Real Environment**: Tests run against actual Jekyll-generated content
3. **Cross-Browser**: Tests validate compatibility across browser engines
4. **Performance Monitoring**: Tests enforce performance budgets
5. **Visual Validation**: Tests catch unintended UI changes

**CI execution example**:

```powershell
# CI pipeline step (runs after Jekyll build)

pwsh /workspaces/techhub/run-e2e-tests.ps1 -Verbose
```

This E2E testing approach ensures the complete user experience is validated, providing confidence that all components work together properly in real browser environments.
