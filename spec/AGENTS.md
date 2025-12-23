# Testing Agent

## Overview

You are a testing specialist for the Tech Hub project. This directory contains all automated tests across multiple frameworks, implementing a comprehensive testing pyramid strategy.

## Tech Stack

- **PowerShell**: Pester v5
- **JavaScript**: Jest with jsdom
- **Ruby**: RSpec
- **E2E**: Playwright
- **CI**: GitHub Actions

## Directory Structure

```text
spec/
├── e2e/                    # Playwright end-to-end tests
│   ├── tests/             # E2E test files
│   ├── playwright.config.js
│   └── package.json
├── javascript/             # Jest unit tests
│   └── *.test.js
├── powershell/             # Pester unit tests
│   ├── *.Tests.ps1
│   ├── Initialize-BeforeAll.ps1
│   ├── Initialize-BeforeEach.ps1
│   └── test-data/
└── _plugins/               # RSpec integration tests
    └── *_spec.rb
```

## Testing Strategy

### Testing Pyramid

```text
        /\
       /  \     E2E Tests (Phase 5) - Playwright
      /____\    ← Slow, Few Tests, High Value
     /      \   
    /        \  Integration Tests (Phase 3) - RSpec
   /__________\ ← Medium Speed, Some Tests
  /            \
 /              \ Unit Tests (Phases 2 & 4) - Pester, Jest
/________________\ ← Fast, Many Tests, Quick Feedback
```

### Test Framework Mapping

- **Pester (Phase 2)**: PowerShell preprocessing scripts
- **Jest (Phase 4)**: Client-side JavaScript logic
- **RSpec (Phase 3)**: Jekyll plugins and server-side
- **Playwright (Phase 5)**: Complete user workflows

## Critical Testing Rules

**CRITICAL**: ALWAYS test the real implementation—never duplicate production logic in tests
**CRITICAL**: ONLY mock/stub external dependencies and input data
**CRITICAL**: NEVER copy production code into test files
**CRITICAL**: Remove tests if production function is no longer used

## PowerShell Testing (Pester)

### File Organization

```text
spec/powershell/
├── [ScriptName].Tests.ps1
├── Initialize-BeforeAll.ps1    # Standard setup
├── Initialize-BeforeEach.ps1   # Standard cleanup
└── test-data/                  # Test data files
```

### Test Pattern

```powershell
Describe "Function-Name" {
    BeforeAll {
        . "$PSScriptRoot/Initialize-BeforeAll.ps1"
        # Custom setup
    }

    BeforeEach {
        . "$PSScriptRoot/Initialize-BeforeEach.ps1"
        # Custom reset
    }
    
    Context "Parameter Validation" {
        It "Should throw when required parameter is null" {
            { Function-Name -Parameter $null } | Should -Throw
        }
    }
    
    Context "Core Functionality" {
        It "Should process valid input correctly" {
            # Arrange
            $input = @("test")
            
            # Act
            $result = Function-Name -Parameter $input
            
            # Assert
            $result | Should -Contain "expected"
        }
    }
}
```

### Running Pester Tests

```bash
# All PowerShell tests
./scripts/run-powershell-tests.ps1

# Specific test file
./scripts/run-powershell-tests.ps1 -TestPath "spec/powershell/Convert-RssToMarkdown.Tests.ps1"

# With coverage
./scripts/run-powershell-tests.ps1 -Coverage

# Detailed output
./scripts/run-powershell-tests.ps1 -Detailed
```

## JavaScript Testing (Jest)

### File Organization

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
      <div data-tags="ai,azure" class="item">Item 1</div>
    `;
    
    // Initialize state
    window.activeFilters = new Set();
  });
  
  afterEach(() => {
    // Cleanup
    document.body.innerHTML = '';
  });
  
  test('filters content by tag', () => {
    // Arrange
    const items = document.querySelectorAll('.item');
    
    // Act
    window.activeFilters.add('ai');
    updateDisplay();
    
    // Assert
    expect(items[0].hidden).toBe(false);
  });
  
  test('handles multiple filters', () => {
    // Test AND logic
    window.activeFilters.add('ai');
    window.activeFilters.add('azure');
    updateDisplay();
    
    const visible = Array.from(document.querySelectorAll('.item'))
      .filter(item => !item.hidden);
    
    expect(visible).toHaveLength(1);
  });
});
```

### Running Jest Tests

```bash
# All JavaScript tests
./scripts/run-javascript-tests.ps1

# Watch mode
npm test -- --watch

# Coverage
npm test -- --coverage

# Specific file
npm test -- filters.test.js
```

## Ruby Testing (RSpec)

### File Organization

```text
spec/_plugins/
└── [plugin_name]_spec.rb
```

### Test Pattern

```ruby
require 'spec_helper'

RSpec.describe Jekyll::FilterName do
  include Jekyll::FilterName
  
  describe '#filter_method' do
    context 'with valid input' do
      it 'processes input correctly' do
        result = filter_method('test-input')
        expect(result).to eq('expected-output')
      end
    end
    
    context 'with invalid input' do
      it 'handles errors gracefully' do
        expect { filter_method(nil) }.to raise_error(ArgumentError)
      end
    end
  end
end
```

### Running RSpec Tests

```bash
# All plugin tests
./scripts/run-plugin-tests.ps1

# Specific file
bundle exec rspec spec/_plugins/date_filters_spec.rb

# With documentation format
bundle exec rspec --format documentation
```

## E2E Testing (Playwright)

### File Organization

```text
spec/e2e/
├── tests/
│   ├── navigation.spec.js
│   ├── filtering.spec.js
│   └── search.spec.js
├── playwright.config.js
└── package.json
```

### Test Pattern

```javascript
import { test, expect } from '@playwright/test';

test.describe('Filter System', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/');
  });
  
  test('filters content by tag', async ({ page }) => {
    // Arrange - locate filter button
    const filterBtn = page.locator('[data-filter="ai"]');
    
    // Act - click filter
    await filterBtn.click();
    
    // Assert - verify filtering
    const visibleItems = await page.locator('.content-item:visible');
    await expect(visibleItems).toHaveCount(10);
    
    // Verify URL updated
    expect(page.url()).toContain('filter=ai');
  });
  
  test('search filters content', async ({ page }) => {
    // Type in search
    await page.fill('[data-testid="search-input"]', 'azure');
    
    // Wait for debounce
    await page.waitForTimeout(500);
    
    // Verify results
    const results = await page.locator('.content-item:visible');
    await expect(results).toHaveCountGreaterThan(0);
  });
});
```

### Running Playwright Tests

```bash
# All E2E tests
./scripts/run-e2e-tests.ps1

# Specific test file
npx playwright test tests/filtering.spec.js

# UI mode
npx playwright test --ui

# Debug mode
npx playwright test --debug

# Specific browser
npx playwright test --project=chromium
```

## Test Runners

### run-all-tests.ps1

Orchestrates all test suites:

```bash
./scripts/run-all-tests.ps1
```

Executes in order:
1. PowerShell tests (Pester)
2. JavaScript tests (Jest)
3. Ruby tests (RSpec)
4. E2E tests (Playwright)

### Individual Runners

- `run-powershell-tests.ps1` - Pester tests
- `run-javascript-tests.ps1` - Jest tests
- `run-plugin-tests.ps1` - RSpec tests
- `run-e2e-tests.ps1` - Playwright tests

## When to Use Which Test Type

### PowerShell Unit Tests

**Use For**:
- Tag normalization algorithms
- Content cleaning functions
- File processing logic
- String manipulation

**Avoid**:
- File I/O (mock instead)
- Jekyll integration
- Browser behavior

### JavaScript Unit Tests

**Use For**:
- Filter state management
- Tag relationship processing
- Date filtering algorithms
- URL parameter handling

**Avoid**:
- DOM manipulation (use Playwright)
- Full browser APIs
- Visual testing

### Ruby Integration Tests

**Use For**:
- Jekyll plugin functionality
- Liquid filter behavior
- Data file generation
- Server-side transformation

**Avoid**:
- Client-side logic
- Browser rendering
- External APIs

### Playwright E2E Tests

**Use For**:
- Complete user workflows
- Cross-browser compatibility
- Performance validation
- Visual regression

**Avoid**:
- Unit logic
- Implementation details
- Isolated functions

## Test Data Management

### PowerShell Test Data

```text
spec/powershell/test-data/
├── sample-feed.json
├── sample-post.md
└── test-config.json
```

### JavaScript Test Data

Embedded in test files via `beforeEach`:

```javascript
beforeEach(() => {
  document.body.innerHTML = `
    <!-- Test HTML -->
  `;
});
```

### Ruby Test Data

Use RSpec `let` blocks:

```ruby
let(:sample_post) do
  {
    'title' => 'Test Post',
    'date' => Time.now,
    'tags' => ['ai', 'azure']
  }
end
```

## Coverage Requirements

### Target Coverage

- **PowerShell**: 80%+ line coverage
- **JavaScript**: 80%+ line coverage
- **Ruby**: 70%+ line coverage
- **E2E**: Critical user paths

### Coverage Reports

```bash
# PowerShell coverage
./scripts/run-powershell-tests.ps1 -Coverage

# JavaScript coverage
npm test -- --coverage

# View coverage reports
# PowerShell: coverage.xml
# JavaScript: coverage/index.html
```

## CI/CD Integration

Tests run automatically in GitHub Actions:

- **PR Validation**: All tests on pull requests
- **Main Branch**: Full test suite
- **Performance**: Track test execution time

## Best Practices

### Test Naming

```powershell
# ✅ CORRECT: Descriptive test names
It "Should filter tags containing hyphens correctly" { }

# ❌ WRONG: Vague test names
It "Works" { }
```

### Arrange-Act-Assert

```javascript
test('example test', () => {
  // Arrange - set up test data
  const input = 'test';
  
  // Act - execute function
  const result = process(input);
  
  // Assert - verify outcome
  expect(result).toBe('expected');
});
```

### Test Independence

```powershell
# ✅ CORRECT: Clean state each test
BeforeEach {
    $variable = @()  # Reset for each test
}

# ❌ WRONG: Tests depend on order
$variable = @()  # Shared state!
It "First test" { $variable += 1 }
It "Second test" { $variable += 2 }  # Depends on first!
```

## Debugging Tests

### PowerShell

```powershell
# Add debug output
Write-Host "Variable value: $variable"

# Set breakpoint (VS Code)
# Click left of line number

# Step through
# F10: Step over, F11: Step into
```

### JavaScript

```javascript
// Use console.log
console.log('Debug:', value);

// Use debugger statement
debugger; // Pauses in browser DevTools

// Run single test
test.only('debug this test', () => {
  // ...
});
```

### Playwright

```bash
# Debug mode (pause on failures)
npx playwright test --debug

# Generate trace
npx playwright test --trace on

# View trace
npx playwright show-trace trace.zip
```

## Resources

- [scripts/AGENTS.md](../scripts/AGENTS.md) - PowerShell development
- [assets/js/AGENTS.md](../assets/js/AGENTS.md) - JavaScript development
- [_plugins/AGENTS.md](../_plugins/AGENTS.md) - Ruby plugin development

## Never Do

- Never duplicate production logic in tests
- Never test implementation details
- Never skip test cleanup
- Never commit failing tests
- Never mock what you're testing
- Never write tests without assertions
- Never ignore flaky tests
