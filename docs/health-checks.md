# Health Checks

This document describes the health check mechanism used in Tech Hub for monitoring and orchestration.

## Overview

Health checks are critical for ensuring the application is running correctly and ready to serve traffic. They are used by:

- **Azure App Service**: A single `/health` health-check path configured per site in Bicep (`healthCheckPath`), monitored continuously alongside `alwaysOn`
- **Load Balancers**: To route traffic only to healthy instances
- **Monitoring Systems**: To alert on downtime or degraded performance
- **Aspire Dashboard**: To monitor service health during development

Tech Hub includes standard health checks for the API and Web components, plus a custom startup probe that ensures content synchronization is complete before the API declares itself ready.

## Endpoints

### GET /health

Comprehensive health check endpoint that verifies all health checks pass, including the startup health check.

**Response**: `200 OK` with `text/plain` when healthy

```text
Healthy
```

**Behavior**:

- Returns `200 OK` only after ALL health checks pass, including the startup health check
- Returns `503 Service Unavailable` if any health check fails (e.g., database migrations not complete, content sync in progress)

**Use Case**: Readiness probes — is the app ready to serve traffic?

### GET /alive

Liveness check endpoint that verifies only health checks tagged with "live" pass.

**Response**: `200 OK` with `text/plain` when the process is alive

```text
Healthy
```

**Behavior**:

- Returns `200 OK` if the app runtime is responsive and not under critical memory pressure
- Checks GC memory info (`MemoryLoadBytes` vs `HighMemoryLoadThresholdBytes`) — reports unhealthy above 95% threshold
- Does **NOT** check external dependencies (database, APIs) — a DB outage should not trigger container restarts

**Use Case**: Application-level liveness signal — is the process alive and responsive? Not currently wired into Azure App Service configuration (see below), but still exposed for local/manual checks and potential future use.

## App Service Health Check Configuration

Both API and Web App Service sites configure a single health-check path in their Bicep modules:

| Setting | Value | Purpose |
|---------|-------|---------|
| `alwaysOn` | `true` | Keeps the site warm — no cold start / idle unload on Basic tier |
| `healthCheckPath` | `/health` | Azure pings this path on the running instance every minute |

**Actual App Service behavior** (see [Monitor the health of App Service instances](https://learn.microsoft.com/azure/app-service/monitor-instances-health-check)) differs from Container Apps' restart-on-failure model and is more lenient:

- Both `api` and `web` App Service Plans run **Basic B1 with a single instance**. Per Microsoft's own docs, a single-instance app is **never removed from the load balancer** while unhealthy — that would take down the entire site — so a failing `/health` never causes an outage by itself.
- After **one continuous hour** of unhealthy pings, App Service replaces the instance (a cold restart), capped at one replacement per hour and three per day per plan.
- With 2+ instances, an unhealthy instance is pulled from rotation after `WEBSITE_HEALTHCHECK_MAXPINGFAILURES` consecutive failures (default 10, i.e. ~10 minutes), then replaced after the same one-hour threshold if it doesn't recover.

Both API and Web point `healthCheckPath` at `/health` — the same endpoint that waits for database
migrations, content sync, and (for Web) API reachability — rather than the DB-agnostic `/alive`
endpoint. Microsoft's own guidance recommends this: the health-check path should check the
app's critical dependencies and return a failure code when they're unavailable.

**Trade-off vs the previous Container Apps model**: pointing the platform health check at `/health`
means a prolonged (1+ hour) outage of a critical dependency can eventually cause App Service to
replace the instance, whereas the old Container Apps setup deliberately kept its liveness probe
dependency-free (`/alive`) to avoid a faster restart-storm loop. App Service's much longer, rate-limited
replacement threshold makes this an acceptable trade-off — a single unhealthy instance is never
pulled from traffic, and replacement is both slow and capped, so it functions more as a
self-healing safety net than a restart storm risk.

## Implementation Details

Health checks are implemented using standard ASP.NET Core Health Checks middleware and the `Aspire.ServiceDefaults` project.

- **Liveness Check** (`self`): Registered in `ServiceDefaults/Extensions.cs`, tagged with `"live"`. Checks GC memory pressure — returns unhealthy if memory usage exceeds 95% of the high memory load threshold.
- **Startup Health Check**: A custom health check tagged with `"ready"` (API only) that waits for database migrations and content synchronization to complete. Included in `/health` but not in `/alive`.
- **SectionCache Health Check** (Web only): Reports unhealthy until `SectionCache` has been populated from the API at least once. Included in `/health` but not in `/alive`.
- **API Connectivity Health Check** (`api-connectivity`, Web only): Calls the API's `/alive` endpoint to verify the Web instance can reach the API over the network. Included in `/health` but not in `/alive` — see rationale below.
- **Aspire Service Defaults**: Maps both `/health` and `/alive` endpoints via `app.MapDefaultEndpoints()`.

### Why Web Checks API Connectivity (and Only `/alive`, Not `/health`)

Web's `SectionCache`/`HeroBannerCache` are populated once at startup and refreshed every 5 minutes,
but a refresh failure after startup only logs a warning — it never flips Web back to unhealthy
(`SectionCache.IsReady` is `Sections.Count > 0`, which stays `true` forever once set). Without an
explicit connectivity check, a broken Web→API network path (VNet integration misconfigured, DNS
failure, firewall rule change) would never surface in `/health`, and Web would keep serving
increasingly stale cached data indefinitely with no signal.

`ApiHealthCheck` closes that gap by calling the API's `/alive` endpoint — deliberately **not**
`/health` — with a short (5s) timeout, independent of `TechHubApiClient`'s 3-minute timeout for
slow admin operations. Checking `/alive` instead of `/health` keeps the two failure domains
separate: an API-side database outage is the API's own health concern (already reflected in the
API's `/health`), and shouldn't also flip Web unhealthy — Web's caches are specifically designed to
keep serving fine through a brief API/DB blip. `ApiHealthCheck` only fails when Web genuinely
cannot reach the API process at all.

This is a low-risk addition given App Service's actual health-check semantics (see above): on a
single-instance Basic B1 plan, `/health` failures never remove the instance from traffic and only
result in a replacement after a full continuous hour of failures — far longer than any routine API
deploy or transient blip.

### Why Liveness Doesn't Check the Database

The `/alive` endpoint intentionally does not check external dependencies. This design predates the
App Service migration, from when Container Apps used `/alive` as a DB-agnostic liveness probe to
avoid a restart storm during DB outages:

1. A DB outage would fail all liveness probes
2. Every instance would restart simultaneously
3. Instances restart, try to connect to the still-down DB, fail liveness again
4. Restart storm — all instances cycling indefinitely

App Service's `healthCheckPath` is now pointed at `/health` instead (see above), so this specific
restart-storm protection no longer fully applies at the infrastructure level — it's documented here
as historical context and because `/alive` remains available as a lighter-weight, DB-agnostic
endpoint for other uses (e.g. manual checks, future load balancer configuration).

## Implementation Reference

- [ServiceDefaults/Extensions.cs](../src/TechHub.ServiceDefaults/Extensions.cs) — Liveness check and endpoint mapping
- [StartupHealthCheck.cs](../src/TechHub.Api/HealthChecks/StartupHealthCheck.cs) — API startup health check
- [SectionCacheHealthCheck.cs](../src/TechHub.Web/Services/SectionCacheHealthCheck.cs) — Web readiness health check
- [ApiHealthCheck.cs](../src/TechHub.Web/Services/ApiHealthCheck.cs) — Web→API connectivity health check
- [api.bicep](../infra/modules/api.bicep) — API App Service health-check configuration
- [web.bicep](../infra/modules/web.bicep) — Web App Service health-check configuration
