# Blazor Component Tests - Tech Hub

> **AI CONTEXT**: This is a **LEAF** context file for Blazor component tests in the `tests/TechHub.Web.Tests/` directory. It complements the [tests/AGENTS.md](../AGENTS.md) testing strategy.
> **RULE**: Follow the 10-step workflow in Root [AGENTS.md](../../AGENTS.md). Project principles are in [README.md](../../README.md). Follow **BOTH**.

## Overview

This directory contains **component tests** for Tech Hub Blazor components using **bUnit**. These tests validate component rendering, interactivity, and state management without requiring a browser.

**Implementation being tested**: See [src/TechHub.Web/AGENTS.md](../../src/TechHub.Web/AGENTS.md) for Blazor component patterns.

## What This Directory Contains

**Test Files**: bUnit test classes that validate Blazor component behavior:

- `Components/Pages/` - Tests for Blazor page components
- `Components/*Tests.cs` (Root) - Tests for shared and core components (ContentItemCard, SectionCard, etc.)
- `Components/Layout/` - Tests for layout components (future)

## Testing Strategy

**What to Test**:

- ✅ **Component rendering** (output HTML structure)
- ✅ **Parameter binding** (prop changes update UI)
- ✅ **Event handlers** (button clicks, form submissions)
- ✅ **Conditional rendering** (if/else, null checks)
- ✅ **Component lifecycle** (OnInitialized, OnParametersSet)
- ✅ **Dependency injection** (mocked services)

**What NOT to Test**:

- ❌ **Complete user workflows** (belongs in E2E tests)
- ❌ **Browser APIs** (localStorage, sessionStorage)
- ❌ **HTTP requests** (mock API clients)
- ❌ **Razor syntax** (framework responsibility)

## Test Patterns

### What to Test

**Component Rendering**:

- Verify expected HTML structure when given parameters
- Use bUnit's `RenderComponent<T>()` with parameter builder
- Find elements by CSS selectors and verify content

**Injected Services**:

- Mock `ITechHubApiClient` and other dependencies using `Services.AddSingleton()`
- Configure mock return values before rendering
- Use `WaitForState()` for components with async initialization

**Event Handlers**:

- Trigger events with `.Click()`, `.Change()`, etc.
- Assert component state changes after events
- Verify callbacks are invoked

**Conditional Rendering**:

- Test both branches of `@if` statements
- Verify loading states and error states
- Test null/empty data scenarios

### Key bUnit Patterns

- **Test class inheritance**: Inherit from `TestContext` base class
- **Service registration**: Use `Services.AddSingleton()` before rendering
- **Async initialization**: Use `WaitForState()` for `OnInitializedAsync`
- **Element selection**: Use CSS selectors with `.Find()` and `.FindAll()`

See actual tests in `Components/` and bUnit documentation for examples.

## Running Tests

**Use the Run function for all test execution** (see [README.md - Starting, Stopping and Testing](../../README.md#starting-stopping-and-testing-the-website)):

```powershell
# Run all tests (recommended)
Run

# Run only Web component tests
Run -TestProject Web.Tests

# Run specific component tests
Run -TestName SectionCard
```

## Best Practices

1. **Use bUnit's TestContext** - Inherit from TestContext base class
2. **Mock injected services** - Use Moq for API clients
3. **Test user perspective** - Find elements by CSS selectors, not implementation
4. **Use WaitForState** - For async component initialization
5. **Test accessibility** - Verify ARIA labels, semantic HTML
6. **Isolate component tests** - Each test creates its own instance
7. **Clean test names** - `ComponentName_Scenario_ExpectedResult`

## Common Pitfalls

❌ **Don't test Blazor internals** (component lifecycle is framework's job)  
❌ **Don't test CSS styling** (component tests verify structure, not appearance)  
❌ **Don't use real HTTP clients** (always mock ITechHubApiClient)  
❌ **Don't test browser APIs** (use E2E tests for localStorage, etc.)  
❌ **Don't forget async initialization** (use WaitForState for OnInitializedAsync)

## Related Documentation

- [tests/AGENTS.md](../AGENTS.md) - Complete testing strategy
- [src/TechHub.Web/AGENTS.md](../../src/TechHub.Web/AGENTS.md) - Blazor component patterns
- [Root AGENTS.md](../../AGENTS.md#step-6-write-tests-first-tdd) - When to write tests
- [bUnit Documentation](https://bunit.dev/) - Official bUnit docs
