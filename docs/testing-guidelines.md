# Testing Guidelines

This document provides the comprehensive testing strategy for the Tech Hub, covering when to use which type of tests and how they fit together in the testing pyramid.

## Testing Strategy Overview

The Tech Hub uses a comprehensive four-tier testing approach aligned with the 5-phase filtering system, following proper testing pyramid principles:

### Critical test writing instruction

**CRITICAL**: ALWAYS test the real implementation—meaning the actual production code as it is used in the application. Never write tests that duplicate or reimplement production logic in the test files.
**CRITICAL**: ONLY mock, stub, or simulate input data and external dependencies. All logic under test must be the real, production implementation.
**CRITICAL**: NEVER replicate or copy production code into test files. If you cannot access a production function for testing, do NOT duplicate the logic, do NOT add comments or empty tests—ASK for help so the code can be made testable.
**CRITICAL**: NEVER add or keep functions in production code solely because a test calls them. ALWAYS verify that every tested function is still in use by current production code. If a function is no longer used, remove both the function and its tests.
**CRITICAL**: If you encounter a situation where you cannot test a function without violating these rules, escalate immediately for review. The goal is to ensure all tests are meaningful, maintainable, and reflect real application behavior.

### Testing Pyramid Structure

```text
        /\
       /  \     E2E Tests (Phase 5)
      /____\    ← Slow, Few Tests, High Value
     /      \   
    /        \  Integration Tests (Phase 3)  
   /__________\ ← Medium Speed, Some Tests
  /            \
 /              \ Unit Tests (Phases 2 & 4)
/________________\ ← Fast, Many Tests, Quick Feedback
```

### Test Framework Mapping

- **PowerShell/Pester Tests** (Phase 2): Unit tests for PowerShell preprocessing scripts
- **JavaScript/Jest Tests** (Phase 4): Unit tests for client-side logic  
- **Ruby/RSpec Tests** (Phase 3): Integration tests for Jekyll plugins
- **Playwright Tests** (Phase 5): End-to-end browser tests for user workflows

## When to Use Which Test Type

### PowerShell Unit Tests (Phase 2)

**Use For**: Testing PowerShell preprocessing logic in isolation

- Tag normalization and filtering algorithms
- Content cleaning and sanitization functions  
- File processing and frontmatter extraction
- String manipulation and validation logic

**Avoid**: Testing actual file I/O, Jekyll integration, browser behavior

**Framework**: Pester v5 | **Speed**: Fast (milliseconds)
**Documentation**: See [PowerShell Guidelines](powershell-guidelines.md#testing)

### JavaScript Unit Tests (Phase 4)  

**Use For**: Testing client-side logic without browser dependencies

- Filter state management and calculations
- Tag relationship processing
- Date filtering algorithms
- URL parameter handling

**Avoid**: DOM manipulation, browser APIs, visual testing

**Framework**: Jest with jsdom | **Speed**: Fast (milliseconds)
**Documentation**: See [JavaScript Guidelines](javascript-guidelines.md#unit-testing)

### Ruby Integration Tests (Phase 3)

**Use For**: Testing Jekyll plugins and server-side data generation

- Jekyll plugin functionality
- Liquid filter behavior
- Data file generation and processing
- Server-side content transformation

**Avoid**: Client-side logic, browser rendering, external APIs

**Framework**: RSpec | **Speed**: Medium (seconds)
**Documentation**: See [Ruby Guidelines](ruby-guidelines.md)

### Playwright End-to-End Tests (Phase 5)

**Use For**: Testing complete user workflows in real browsers

- Full user interaction scenarios
- Cross-browser compatibility
- Performance and accessibility validation
- Visual regression testing

**Avoid**: Unit logic, implementation details, isolated functions

**Framework**: Playwright | **Speed**: Slow (minutes)
**Documentation**: See [Frontend Guidelines](frontend-guidelines.md#e2e-testing)

## Running Tests

The Tech Hub provides automated test runner scripts for all test frameworks:

### Test Runner Scripts

```powershell
# Run PowerShell unit tests (Phase 2)

pwsh /workspaces/techhub/run-powershell-tests.ps1

# Run JavaScript unit tests (Phase 4)  

pwsh /workspaces/techhub/run-javascript-tests.ps1

# Run Ruby integration tests (Phase 3)

pwsh /workspaces/techhub/run-plugin-tests.ps1

# Run Playwright E2E tests (Phase 5)

pwsh /workspaces/techhub/run-e2e-tests.ps1

# Run all tests in logical order (recommended)

pwsh /workspaces/techhub/run-all-tests.ps1
```

### Test Execution Workflow

**Recommended Development Sequence:**

1. **PowerShell Tests** (fastest feedback)
2. **JavaScript Tests** (fast unit validation)  
3. **Ruby Tests** (integration validation)
4. **Playwright Tests** (complete system validation)

**All Tests Script:**
The `/workspaces/techhub/run-all-tests.ps1` script executes all test suites in the optimal order with proper error handling and reporting.

**Jekyll Startup Time**: E2E tests automatically start Jekyll if needed, which can take 2-3 minutes for initial startup. Be patient during the first run as Jekyll builds the complete site.

**CRITICAL**: When you need to fix many failed tests, first analyze and see what the biggest common problem is and focus on fixing that first. If you can, make a generic fix to cover multiple tests at once. And especially with the e2e tests, use the `-MaxFailures` option to quickly see if your fixes worked.

## Test Organization and Structure

### Directory Structure

```text
spec/
├── powershell/                    # Phase 2: PowerShell unit tests
│   ├── Get-FilteredTags.Tests.ps1
│   └── Format-FrontMatterValue.Tests.ps1
├── javascript/                    # Phase 4: JavaScript unit tests  
│   ├── package.json
│   ├── filters.test.js
│   └── sections.test.js
├── plugins/                       # Phase 3: Ruby integration tests
│   ├── spec_helper.rb
│   ├── date_filters_spec.rb
│   └── section_pages_generator_spec.rb
└── e2e/                          # Phase 5: Playwright E2E tests
    ├── playwright.config.js
    └── tests/
        ├── filtering-core.spec.js
        └── [other test files]
```

### Test Data Management

- **PowerShell**: Use mock data in tests, avoid actual file I/O
- **JavaScript**: Mock factories for consistent test data
- **Ruby**: Minimal fixtures, focus on plugin behavior  
- **Playwright**: Use real Jekyll-generated content when possible

## Debugging Tests

### General Debugging Strategy

1. **Isolate the Problem**: Run only the failing test
2. **Use Appropriate Tools**: Each framework has specific debugging approaches
3. **Check Dependencies**: Ensure all required dependencies are installed
4. **Validate Environment**: Confirm proper setup and configuration

## Performance Requirements

### Test Execution Speed Targets

- **PowerShell Unit Tests**: < 10 seconds total
- **JavaScript Unit Tests**: < 15 seconds total
- **Ruby Integration Tests**: < 30 seconds total  
- **Playwright E2E Tests**: < 300 seconds total

### Performance Guidelines

- Keep unit tests fast and focused
- Avoid unnecessary browser testing
- Use proper mocking to eliminate external dependencies
- Run E2E tests only for user-facing scenarios

## Integration with CI/CD

### CI Test Execution Order

1. **PowerShell Unit Tests**: Fast validation of preprocessing logic
2. **JavaScript Unit Tests**: Fast validation of client-side logic
3. **Ruby Integration Tests**: Validation of Jekyll plugin integration
4. **Jekyll Build**: Ensure site builds successfully
5. **Playwright E2E Tests**: Full browser validation

This sequence provides early feedback while reserving expensive testing for final validation.

## Writing New Tests

### Before Writing Tests

1. **Check Existing Coverage**: Avoid duplicating tests across frameworks
2. **Choose Appropriate Level**: Select the right testing framework for your scenario
3. **Consider Performance**: Keep tests fast and focused  
4. **Plan Test Data**: Use appropriate data sources for each framework

### Test Guidelines by Framework

For detailed test writing guidelines, see the framework-specific documentation:

- **PowerShell Tests**: [PowerShell Guidelines](powershell-guidelines.md#testing)
- **JavaScript Tests**: [JavaScript Guidelines](javascript-guidelines.md#unit-testing)
- **Ruby Tests**: [Ruby Guidelines](ruby-guidelines.md)
- **Playwright Tests**: [Frontend Guidelines](frontend-guidelines.md#e2e-testing)

## Integration with Documentation

This testing documentation integrates with the broader documentation ecosystem:

- **Cross-References**: Links to framework-specific guidelines
- **Consistency**: Aligns with project development standards
- **Completeness**: Covers both development and operational aspects

For implementation details, see:

- **Plugin Development**: [plugins.md](plugins.md)  
- **Performance Optimization**: [performance-guidelines.md](performance-guidelines.md)
- **JavaScript Standards**: [javascript-guidelines.md](javascript-guidelines.md)
- **PowerShell Standards**: [powershell-guidelines.md](powershell-guidelines.md)
