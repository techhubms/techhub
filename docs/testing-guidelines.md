# Testing Guidelines

This document provides the comprehensive testing strategy for the Tech Hub. For framework-specific testing guidance, see the respective AGENTS.md files:

- PowerShell testing: [scripts/AGENTS.md](../scripts/AGENTS.md) and [spec/AGENTS.md](../spec/AGENTS.md)
- JavaScript testing: [assets/js/AGENTS.md](../assets/js/AGENTS.md) and [spec/AGENTS.md](../spec/AGENTS.md)
- Ruby testing: [_plugins/AGENTS.md](../_plugins/AGENTS.md) and [spec/AGENTS.md](../spec/AGENTS.md)
- E2E testing: [spec/AGENTS.md](../spec/AGENTS.md)

## Testing Strategy Overview

The Tech Hub uses a comprehensive four-tier testing approach aligned with the testing pyramid:

### Critical Testing Rules

**CRITICAL**: ALWAYS test the real implementation—never duplicate production logic in tests
**CRITICAL**: ONLY mock external dependencies and input data
**CRITICAL**: NEVER copy production code into test files
**CRITICAL**: Remove tests if production function is no longer used

### Testing Pyramid

```text
        /\
       /  \     E2E Tests (Playwright)
      /____\    ← Slow, Few Tests, High Value
     /      \   
    /        \  Integration Tests (RSpec)  
   /__________\ ← Medium Speed, Some Tests
  /            \
 /              \ Unit Tests (Pester, Jest)
/________________\ ← Fast, Many Tests, Quick Feedback
```

### Test Framework Mapping

- **PowerShell/Pester**: Unit tests for PowerShell scripts
- **JavaScript/Jest**: Unit tests for client-side logic  
- **Ruby/RSpec**: Integration tests for Jekyll plugins
- **Playwright**: End-to-end browser tests

## When to Use Which Test Type

### PowerShell Unit Tests

**Use For**: PowerShell preprocessing logic
- Tag normalization and filtering algorithms
- Content cleaning functions  
- File processing logic
- String manipulation

**Framework**: Pester v5  
**Documentation**: [scripts/AGENTS.md](../scripts/AGENTS.md)

### JavaScript Unit Tests

**Use For**: Client-side logic without browser dependencies
- Filter state management
- Tag relationship processing
- Date filtering algorithms
- URL parameter handling

**Framework**: Jest with jsdom  
**Documentation**: [assets/js/AGENTS.md](../assets/js/AGENTS.md)

### Ruby Integration Tests

**Use For**: Jekyll plugins and server-side processing
- Jekyll plugin functionality
- Liquid filter behavior
- Data file generation

**Framework**: RSpec  
**Documentation**: [_plugins/AGENTS.md](../_plugins/AGENTS.md)

### Playwright End-to-End Tests

**Use For**: Complete user workflows in real browsers
- Full user interaction scenarios
- Cross-browser compatibility
- Performance validation

**Framework**: Playwright  
**Documentation**: [spec/AGENTS.md](../spec/AGENTS.md)

## Running Tests

```bash
# All tests
./scripts/run-all-tests.ps1

# Specific test suites
./scripts/run-powershell-tests.ps1
./scripts/run-javascript-tests.ps1
./scripts/run-plugin-tests.ps1
./scripts/run-e2e-tests.ps1
```

## Test Organization

```text
spec/
├── e2e/           # Playwright E2E tests
├── javascript/    # Jest unit tests
├── powershell/    # Pester unit tests
└── _plugins/      # RSpec integration tests
```

For detailed testing patterns, examples, and best practices, see [spec/AGENTS.md](../spec/AGENTS.md) and the framework-specific AGENTS.md files.
