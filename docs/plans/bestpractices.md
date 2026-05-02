# Best Practices Improvement Plan

Improvement items identified during a general repository review (May 2026).
Ordered by impact. Each item includes context, rationale, and implementation notes.

---

## 1. Migrate to `NpgsqlDataSource` for Connection Pooling

**Priority**: High
**Affected files**: `DbConnectionFactory.cs`, `Program.cs` (API), test utilities

**Current state**: `PostgresConnectionFactory` creates raw `NpgsqlConnection` instances with
synchronous `Open()`. Connections are registered as scoped via the factory.

**Target state**: Register `NpgsqlDataSource` as a singleton (it manages the pool internally).
Resolve connections with `await dataSource.OpenConnectionAsync()`.

**Benefits**:

- Proper async connection open (no thread pool blocking on scope creation)
- Built-in connection pooling with typed configuration (max pool size, timeouts)
- Multiplexing support for high-throughput scenarios
- Aligns with Npgsql 7+ recommended patterns

**Implementation notes**:

1. Replace `PostgresConnectionFactory` with `NpgsqlDataSource` singleton registration
2. Change scoped `IDbConnection` registration to resolve via `dataSource.OpenConnectionAsync()`
3. Update `IDbConnectionFactory` interface or remove in favor of direct `NpgsqlDataSource` injection
4. Update Testcontainers setup in test utilities to use the new pattern
5. Verify connection disposal still happens at scope end

---

## 2. Add Rate Limiting

**Priority**: High
**Affected files**: `Program.cs` (Web + API)

**Context**: The API is internal-only (not directly internet-facing). Public traffic
hits the Blazor Server Web layer, which then calls the API.

**Strategy**: Primary rate limiting on the **Web layer** (public surface), lightweight
defense-in-depth on the API.

### Web Layer (Primary)

Protect against:

- Excessive page loads and SignalR connection attempts
- Bot scraping of content pages
- DoS on Blazor Server circuits (expensive per-connection)

Suggested policies:

- **Fixed window** on page requests: e.g. 60 requests/minute per IP
- **Concurrency limiter** on SignalR: cap concurrent circuits per IP
- Stricter limit on RSS feed endpoints (bot-targeted)

### API Layer (Defense-in-Depth)

Protect against:

- Web layer bugs that amplify requests
- Future architecture changes that might expose the API
- Internal runaway loops

Suggested policies:

- **Sliding window** on content endpoints: generous limit (e.g. 200 req/min per client)
- **Stricter limit** on admin endpoints: even though auth-protected, rate limit
  failed auth attempts

**Implementation notes**:

1. Use `Microsoft.AspNetCore.RateLimiting` (built-in since .NET 7)
2. Define named policies (`"web-general"`, `"web-signalr"`, `"api-public"`, `"api-admin"`)
3. Apply via `.RequireRateLimiting("policy-name")` on endpoint groups
4. Return `429 Too Many Requests` with `Retry-After` header
5. Consider IP-based partitioning using `X-Forwarded-For` (already trusted from Container Apps)

---

## 3. Add Content-Security-Policy Header

**Priority**: High
**Affected files**: `SecurityHeadersMiddleware.cs`

**Current state**: Middleware adds `X-Content-Type-Options`, `X-Frame-Options`,
`Referrer-Policy`, `Permissions-Policy` — but no CSP.

**Target state**: Add a CSP header appropriate for Blazor Server + external scripts
(Google Analytics, Application Insights).

**Implementation notes**:

1. Start with report-only mode (`Content-Security-Policy-Report-Only`) to avoid breakage
2. Define allowlist for `script-src`, `style-src`, `connect-src`, `img-src`
3. Blazor Server needs `'self'` + `'unsafe-inline'` for styles (scoped CSS) and
   possibly `'unsafe-eval'` for SignalR (verify)
4. Allow Google Analytics domains (`*.google-analytics.com`, `*.googletagmanager.com`)
5. Allow Application Insights (`*.applicationinsights.azure.com`)
6. Once validated in production, switch from report-only to enforcing
7. Consider nonce-based `script-src` for inline scripts if feasible

---

## 4. Add Database Health Check

**Priority**: Medium
**Affected files**: `Program.cs` (API), potentially a new health check class

**Current state**: Only a startup completion check exists. No ongoing database
connectivity monitoring.

**Target state**: Add a health check that pings PostgreSQL on every `/health` request.

**Implementation notes**:

1. Create a `DatabaseHealthCheck` that runs a simple query (e.g. `SELECT 1`)
2. Use the `IDbConnectionFactory` or `NpgsqlDataSource` to open a connection
3. Register with short timeout (5s) to detect connection issues quickly
4. Tag with `"ready"` so readiness probes reflect DB state
5. Consider `AspNetCore.HealthChecks.Npgsql` NuGet package for a pre-built implementation

---

## 5. Add NuGet Cache in CI

**Priority**: Medium
**Affected files**: `.github/workflows/ci.yml`

**Current state**: Each CI job runs `dotnet restore` independently, downloading
packages from scratch every time.

**Target state**: Cache the NuGet global packages folder across runs.

**Implementation notes**:

1. Add `actions/cache` step before restore in each job
2. Cache path: `~/.nuget/packages`
3. Cache key: `nuget-${{ hashFiles('**/Directory.Packages.props') }}`
4. Restore key: `nuget-` (partial match for fallback)
5. Expected improvement: 20-40s saved per job on cache hit

---

## 6. Seal `ContentRepository`

**Priority**: Low
**Affected files**: `ContentRepository.cs`

**Current state**: `public class ContentRepository` with `protected` members,
but no subclasses exist in the codebase.

**Target state**: `public sealed class ContentRepository` with `private` members.

**Implementation notes**:

1. Change `protected` to `private` for `Connection`, `Dialect`, `Cache`, `MarkdownService`
2. Mark class as `sealed`
3. JIT can devirtualize calls, marginally improving performance
4. Communicates design intent (not designed for extension)

---

## 7. Docker Image Size Reduction

**Priority**: Low
**Affected files**: `src/TechHub.Api/Dockerfile`, `src/TechHub.Web/Dockerfile`

**Current state**: `apt-get install -y curl` (and `python3` in API) without
`--no-install-recommends`.

**Target state**: Add `--no-install-recommends` to minimize layer size.

**Implementation notes**:

1. Change to `apt-get install -y --no-install-recommends curl python3`
2. Expected savings: 50-100MB per image
3. Verify `yt-dlp` still works without recommended Python packages

---

## 8. Explicit HSTS Configuration

**Priority**: Low
**Affected files**: `Program.cs` (Web)

**Current state**: Uses default `UseHsts()` (30-day max-age, no includeSubDomains).

**Target state**: Explicit 1-year max-age with `includeSubDomains`.

**Implementation notes**:

```text
builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(365);
    options.IncludeSubDomains = true;
    options.Preload = true;
});
```

Consider HSTS preload submission after stable deployment.

---

## 9. Scope Forwarded Headers Trust

**Priority**: Low
**Affected files**: `Program.cs` (Web)

**Current state**: `KnownIPNetworks.Clear()` + `KnownProxies.Clear()` trusts all
sources for `X-Forwarded-For`.

**Target state**: Restrict to Azure Container Apps internal subnet range.

**Implementation notes**:

1. Determine the Container Apps Environment internal CIDR (typically `10.0.0.0/16` or similar)
2. Add only that range to `KnownNetworks` instead of clearing the list
3. Make configurable via appsettings for environment portability
4. Risk: Azure may rotate internal IPs — test thoroughly

---

## 10. Consider NSubstitute Over Moq

**Priority**: Low (non-urgent)
**Affected files**: All test projects

**Current state**: Moq is used for mocking. `TechHubApiClient` is intentionally
non-sealed to support Moq's proxy generation.

**Rationale**: Moq had SponsorLink controversy; NSubstitute has a cleaner API
and doesn't require classes to be non-sealed (works with interfaces directly).

**Implementation notes**:

1. This is a large refactor — only do if there's a natural opportunity
2. Replace `Moq` package with `NSubstitute` in Directory.Packages.props
3. Rewrite mock setups: `mock.Setup(x => x.Method()).Returns()` → `sub.Method().Returns()`
4. `TechHubApiClient` can be sealed once Moq is removed (tests mock the interface)
5. Consider doing this incrementally (new tests use NSubstitute, migrate old tests over time)

---

## 11. `GenerateDocumentationFile` Optimization

**Priority**: Low
**Affected files**: `Directory.Build.props`

**Current state**: `GenerateDocumentationFile` is enabled globally to activate IDE0005
(unused usings detection during build).

**Alternative**: Use `<EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>` (already set)
which should handle IDE0005 without generating XML doc files. Verify if removing
`GenerateDocumentationFile` still catches unused usings, then remove it to speed up builds.

---

## Execution Order

| Phase | Items | Rationale |
|-------|-------|-----------|
| 1 | 5 (CI cache) | Quick win, speeds up all subsequent development |
| 2 | 1 (NpgsqlDataSource), 4 (DB health check) | Database layer modernization |
| 3 | 2 (Rate limiting) | Security hardening |
| 4 | 3 (CSP header) | Security hardening |
| 5 | 6, 7, 8, 9 | Low-priority cleanup |
| 6 | 10, 11 | Optional/incremental |
