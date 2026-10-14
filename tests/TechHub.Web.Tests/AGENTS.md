# Web Tests

> **RULE**: Follow [tests/AGENTS.md](../AGENTS.md) for shared testing rules and [Root AGENTS.md](../../AGENTS.md) for workflow.

Tests for the Web project: bUnit component tests (the bulk), plus service, middleware, MCP, telemetry, endpoint and convention tests. Validates rendering, interactivity, and state management without a browser.

## Structure

| Folder | Contents |
|---|---|
| `Components/` (and `Components/Pages/`) | bUnit component and page tests |
| `Services/` | `TechHubApiClient`, `ContentGridStateCache`, `DateHelper`, health check and handler tests |
| `Helpers/` | Pure helper tests (`DiffHelper`) |
| `Middleware/` | URL normalization, HTTP method filter, HEAD handling, subdomain redirect, MCP request guards |
| `MCP/` | MCP tool catalog and content provider tests |
| `Telemetry/` | Web telemetry filters and Blazor hub noise suppression |
| `Endpoints/` | RSS proxy endpoint tests, plus tests that boot the real `Program.cs` in memory (`TechHubWebFixture.cs`): `/version`, `/admin/logout`, rate limiting |
| `Conventions/` | `PageMarkScriptsReadyConventionTests` (every page must inherit `PageComponentBase`) |

## What to Test

- ✅ Component rendering (HTML structure for given parameters)
- ✅ Parameter binding and conditional rendering
- ✅ Event handlers (clicks, form submissions)
- ✅ Component lifecycle (`OnInitialized`, `OnParametersSet`)
- ✅ Middleware, services and conventions listed above

## What NOT to Test Here

- ❌ Complete user workflows (use E2E tests)
- ❌ Browser APIs (cookies set by JS, sessionStorage)
- ❌ Razor syntax (framework responsibility)

## Key Patterns

- Inherit from `BunitContext` (bUnit 2.x; `TestContext` no longer exists)
- Mock `ITechHubApiClient` via `Services.AddSingleton()` before rendering
- Use `this.AddBunitPersistentComponentState()` for components that use `PersistentComponentState`
- Use `WaitForState()` for components with `OnInitializedAsync`
- Find elements by CSS selectors with `.Find()` / `.FindAll()`
- Don't test CSS styling — verify structure, not appearance
- Don't forget `WaitForState` for async initialization
- Tests that need the full Web pipeline use `DevelopmentWebFixture` / `ProductionWebFixture` (`IClassFixture`). The API is stubbed (`StubApiHandler`), the OIDC scheme is faked, and each test picks its own client IP through `CreateClient(forwardedFor)` so rate-limit partitions never overlap
