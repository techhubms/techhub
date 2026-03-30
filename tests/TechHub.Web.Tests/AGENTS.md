# Blazor Component Tests

> **RULE**: Follow [tests/AGENTS.md](../AGENTS.md) for shared testing rules and [Root AGENTS.md](../../AGENTS.md) for workflow.

Component tests using bUnit. Validates rendering, interactivity, and state management without a browser.

## What to Test

- ✅ Component rendering (HTML structure for given parameters)
- ✅ Parameter binding and conditional rendering
- ✅ Event handlers (clicks, form submissions)
- ✅ Component lifecycle (`OnInitialized`, `OnParametersSet`)

## What NOT to Test Here

- ❌ Complete user workflows (use E2E tests)
- ❌ Browser APIs (localStorage, sessionStorage)
- ❌ Razor syntax (framework responsibility)

## Key Patterns

- Inherit from `TestContext` base class
- Mock `ITechHubApiClient` via `Services.AddSingleton()` before rendering
- Use `WaitForState()` for components with `OnInitializedAsync`
- Find elements by CSS selectors with `.Find()` / `.FindAll()`
- Don't test CSS styling — verify structure, not appearance
- Don't forget `WaitForState` for async initialization
