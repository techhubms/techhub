# Input Validation and Sanitization

This document defines the input validation and sanitization strategy for Tech Hub. It describes what protections exist at each layer, what each layer is responsible for, and what gaps remain to be addressed.

**Related Documentation**:

- [Content API](content-api.md) - REST API endpoints and route structure
- [Content Filtering](filtering.md) - Search, tag, and date filtering behavior

## Design Principles

1. **Validate at the boundary, for the threat at that boundary** - Each layer handles its own concern. Route validation rejects malformed paths; log sanitization prevents log forging; the framework handles SQL injection and XSS.
2. **Don't conflate concerns** - "Is this safe to log?" and "Is this a valid value?" are different questions answered at different layers.
3. **Leverage framework guarantees** - Blazor HTML-encodes all rendered output (no XSS). Dapper uses parameterized queries (no SQL injection). `HttpClient` URL-encodes query params (no HTTP header injection).
4. **Fail fast** - Invalid input should be rejected as early as possible, before it reaches business logic or the database.

## User Input Surface

All user-controlled input in Tech Hub:

| Input | Source | Example values |
|---|---|---|
| Section name | URL route segment | `github-copilot`, `ai`, `all` |
| Collection name | URL route segment | `blogs`, `videos`, `community`, `all` |
| Content slug | URL route segment | `how-copilot-helps`, `weekly-ai-roundup-2026-03-16` |
| Author name | URL route segment | `Reinier van Maanen`, `O'Brien` |
| Search query | Query string (`?search=`) | `C#`, `ASP.NET`, `blazor component` |
| Tag filter | Query string (`?tags=`) | `github-copilot,ai`, `blazor` |
| Date range | Query string (`?from=`, `?to=`) | `2024-01-15`, `2026-03-19` |
| Video slug | URL route segment | `vscode-updates-march-2026` |
| Pagination | Query string (`?take=`, `?skip=`) | `20`, `0` |

## Validation and Sanitization by Layer

### Layer 1: HTTP Middleware (`InvalidRouteSegmentMiddleware`)

**Boundary**: Incoming HTTP request, before Blazor routing.

**Purpose**: Short-circuit obviously invalid URL paths (scanner probes, `.php` requests, encoded garbage) before they reach the Blazor pipeline.

**What it does**:

- Rejects scanner/attacker probe paths via `ProbeDetector.IsProbeRequest()` (e.g. `/wp-admin`, `/.env`, `/xmlrpc.php`)
- Validates the first path segment against `^[a-zA-Z][a-zA-Z-]*$` (letters and hyphens, case-insensitive)
- Passes through static files (paths with file extensions) and framework paths (`_blazor`, `_framework`, `MicrosoftIdentity`)
- Returns 404 for invalid segments — no Blazor components render, no API calls fire

**What it does NOT do**: Validate deeper segments, query strings, or request bodies.

### Layer 2: Route Parameter Validation (`RouteParameterValidator`)

**Boundary**: API endpoint filters and Blazor page `OnInitializedAsync`/`OnParametersSetAsync`.

**Purpose**: Reject structurally invalid route parameters before they reach business logic.

**Rules**:

| Parameter type | Regex | Allowed characters |
|---|---|---|
| Section/collection name | `^[a-z][a-z-]*$` | Lowercase letters and hyphens, must start with letter |
| Content slug | `^[a-z0-9][a-z0-9-]*$` | Lowercase letters, digits, and hyphens |
| Author name | No regex — rejects `/`, `\`, `\0`, length > 200 | Any printable characters except path separators |

**Where enforced**:

- **API**: `ValidateRouteParameters` endpoint filter on `/api/sections` group → returns 400 Bad Request
- **API**: `IsValidAuthorName` in `AuthorEndpoints` → returns 400 Bad Request
- **Blazor pages**: `ContentItem.razor` and `GitHubCopilotVSCodeUpdates.razor` validate in `OnInitializedAsync` → redirect to `/not-found`
- **Blazor pages**: `SectionCollection.razor` relies on `MainLayout` validation (section must exist in `SectionCache`)

### Layer 3: API Query Parameter Validation

**Boundary**: API endpoint handlers in `ContentEndpoints`.

**Purpose**: Validate and constrain query parameters that control filtering and pagination.

**Current validation**:

| Parameter | Validation | Behavior on invalid input |
|---|---|---|
| `take` | Clamped to `1..MaxPageSize` (configured) | Silently clamped |
| `skip` | `Math.Max(skip, 0)` | Silently clamped to 0 |
| `from`, `to` | `DateTimeOffset.TryParse` with ISO 8601 | Returns 400 with descriptive message |
| `from` > `to` | Swapped defensively | Silently corrected |
| `q` (search) | Max 200 characters | Returns 400 if exceeded |
| `tags` | Max 20 tags, each max 100 characters | Returns 400 if exceeded |
| `lastDays` | Range 0..3650 (0 disables date filtering) | Returns 400 if out of range |

All query parameter validation is now implemented. See `ContentEndpoints.cs` for the validation constants and logic.

### Layer 4: Frontend Input Constraints (Blazor Components)

**Boundary**: User-facing UI components.

**Purpose**: Provide first-line constraints on what the user can type or select.

**Current constraints**:

| Component | Constraint | Implementation |
|---|---|---|
| `SidebarSearch` | `maxlength="200"` on input | HTML attribute |
| `DateRangeSlider` | Slider with fixed min/max range | Cannot enter arbitrary dates |
| Tag cloud | Click-to-toggle from known tag set | Cannot enter arbitrary tag values |
| `SectionCollection` URL parsing | Max 20 tags, max 100 chars each, search max 200 chars | Code enforcement in `ParseTagsFromUrl`/`ParseSearchFromUrl` |

Frontend constraints provide immediate UX feedback so users see limits before hitting the server. However, they are not a security boundary — all limits must also be enforced server-side (API layer) because users can bypass any frontend constraint by crafting URLs manually or using browser dev tools.

### Layer 5: Log Sanitization (`InputSanitizer`)

**Boundary**: Any code that writes user-controlled values to structured logs.

**Purpose**: Prevent log forging — an attacker injecting fake log entries by embedding `\r\n` in input values.

**What it does**: Strips carriage return (`\r`) and newline (`\n`) characters.

**How to use**: Call `.Sanitize()` on every user-controlled argument in structured log statements. This is a string extension method (defined on `InputSanitizer`) that makes intent explicit and keeps structured logging intact:

```csharp
// Correct — .Sanitize() on each user-controlled argument
_logger.LogError(ex, "Failed for {Section}/{Collection}", sectionName.Sanitize(), collectionName.Sanitize());

// Wrong — string interpolation defeats structured logging
_logger.LogError(ex, $"Failed for {sectionName.Sanitize()}");

// Wrong — sanitizing the whole message template
_logger.LogError(ex, InputSanitizer.Sanitize($"Failed for {sectionName}"));
```

Apply `.Sanitize()` consistently everywhere: API endpoints, `TechHubApiClient`, Blazor components, infrastructure services. No separate patterns per layer — one approach everywhere.

**What `InputSanitizer` is NOT for**: Input validation. It does not reject bad input — it only makes values safe to log.

### Layer 6: CodeQL (Static Analysis)

**Purpose**: CodeQL's built-in `cs/log-forging` query detects user-controlled values that reach log statements without sanitization. This catches missing `.Sanitize()` calls in CI.

**Note**: The C# CodeQL pack (`codeql/csharp-all`) does not support custom `sanitizerModel` extensions, so CodeQL may still flag values that pass through `.Sanitize()`. These alerts should be reviewed and dismissed when the sanitization is confirmed present.

### Framework-Provided Protections

These are handled automatically — no application code needed:

| Threat | Protection | Provided by |
|---|---|---|
| XSS (Cross-Site Scripting) | All rendered output is HTML-encoded | Blazor rendering engine |
| SQL Injection | All queries use parameterized statements | Dapper |
| URL Injection | Query parameters are URL-encoded | `HttpClient` / `Uri.EscapeDataString` |
| CSRF | SignalR circuit (no form submissions) | Blazor InteractiveServer |

## Summary: Who Validates What

```text
HTTP Request
│
├─ Middleware ──────────── Rejects invalid first path segment (scanner probes)
│
├─ API Endpoint Filter ── Rejects invalid route params (section, collection, slug)
│
├─ API Endpoint Handler ─ Validates query params (dates, pagination)
│                         ✅ search query length, tag count/length, lastDays range
│
├─ Database ───────────── Parameterized queries (SQL injection impossible)
│
├─ Blazor Rendering ───── HTML encodes all output (XSS impossible)
│
├─ Log Statements ─────── .Sanitize() on arguments strips CR/LF (log forging impossible)
│
└─ CodeQL ─────────────── Flags missing sanitization in CI (may have false positives)
```

## Implementation Reference

- Probe detection: `src/TechHub.Core/Security/ProbeDetector.cs` — shared by middleware and telemetry filters to detect scanner/attacker probe requests
- Middleware: `src/TechHub.Web/Middleware/InvalidRouteSegmentMiddleware.cs`
- Route validation: `src/TechHub.Core/Validation/RouteParameterValidator.cs`
- API endpoint filter: `src/TechHub.Api/Endpoints/ContentEndpoints.cs` → `ValidateRouteParameters`
- Author validation: `src/TechHub.Api/Endpoints/AuthorEndpoints.cs` → `IsValidAuthorName`
- Log sanitization: `src/TechHub.Core/Logging/InputSanitizer.cs` (static method + `.Sanitize()` extension)
- Search input: `src/TechHub.Web/Components/SidebarSearch.razor`
- Date input: `src/TechHub.Web/Components/DateRangeSlider.razor`
- Tests: `tests/TechHub.Core.Tests/Validation/RouteParameterValidatorTests.cs`, `tests/TechHub.Core.Tests/Logging/InputSanitizerTests.cs`
