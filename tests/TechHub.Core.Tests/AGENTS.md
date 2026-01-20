# Core Unit Tests - Tech Hub

> **AI CONTEXT**: This is a **LEAF** context file for Core unit tests in the `tests/TechHub.Core.Tests/` directory. It complements the [tests/AGENTS.md](../AGENTS.md) testing strategy.
> **RULE**: Global rules (Timezone, Performance) in Root AGENTS.md apply **IN ADDITION** to local rules. Follow **BOTH**.

## Overview

This directory contains **unit tests** for the Tech Hub Core domain layer using **xUnit**. These tests validate domain models, business logic, and pure functions in complete isolation with no external dependencies.

**Implementation being tested**: See [src/TechHub.Core/AGENTS.md](../../src/TechHub.Core/AGENTS.md) for domain model patterns.

## What This Directory Contains

**Test Files**: xUnit test classes that validate domain model behavior:

- `Models/ContentItemTests.cs` - Tests for ContentItem domain model
- `Models/SectionTests.cs` - Tests for Section domain model (future)
- `Extensions/` - Tests for extension methods (future)

## Testing Strategy

**What to Test**:

- ✅ **Domain model behavior** (GetUrlInSection, property initialization)
- ✅ **Business rules** (validation, constraints)
- ✅ **Value calculations** (derived properties)
- ✅ **Extension methods** (pure functions)
- ✅ **Edge cases** (null handling, boundary conditions)

**What NOT to Test**:

- ❌ **Framework features** (C# record equality, property getters)
- ❌ **Simple property assignments** (no logic to test)
- ❌ **External dependencies** (file I/O, HTTP calls - use mocks)

## Test Patterns

### What to Test in Domain Models

**Methods and Behavior**:

- Test methods like `GetUrlInSection()` with various inputs
- Use `[Theory]` with `[InlineData]` for multiple scenarios
- Verify correct URL generation, property calculations, etc.

**Record Equality**:

- Test that records with identical values are equal
- Important for caching, comparison, and deduplication logic

**Edge Cases**:

- Null/empty string inputs
- Boundary values
- Invalid section/collection names
- Missing required properties

### Test Data Factories

Use factory methods to create test objects consistently:

- Create private helper methods like `CreateContentItem(string slug)`
- Set all required properties with sensible defaults
- Allow parameters for the properties being tested

See actual tests in `Models/` for implementation examples.

## Running Tests

**Use the Run function for all test execution** (see [Root AGENTS.md - Using the Run Function](../../AGENTS.md#using-the-run-function)):

```powershell
# Run all tests (recommended)
Run

# Run only Core unit tests, then exit
Run -OnlyTests -TestProject Core.Tests

# Run specific test class
Run -OnlyTests -TestName ContentItem
```

## Best Practices

1. **Test behavior, not implementation** - Focus on what the method does, not how
2. **Use [Theory] for multiple cases** - DRY principle for similar scenarios
3. **Use FluentAssertions** - More readable than Assert.Equal
4. **Test edge cases** - Null, empty, boundary values
5. **Fast tests** - No I/O, no external dependencies
6. **Clear test names** - `MethodName_Scenario_ExpectedResult`
7. **Use helper methods** - Create test data factories for complex objects

## Common Pitfalls

❌ **Don't test C# language features** (record equality already works)  
❌ **Don't test simple getters/setters** (no logic = no test needed)  
❌ **Don't use real file paths** (use test data factories)  
❌ **Don't mock what you're testing** (only mock external dependencies)  
❌ **Don't share state** between tests (each test should be isolated)

## Related Documentation

- [tests/AGENTS.md](../AGENTS.md) - Complete testing strategy
- [src/TechHub.Core/AGENTS.md](../../src/TechHub.Core/AGENTS.md) - Domain model patterns
- [Root AGENTS.md](/AGENTS.md#6-test--validate) - When to write tests
