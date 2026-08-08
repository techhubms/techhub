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

| Setting | Value | Purpose | Failure behavior |
|---------|-------|---------|-----------------|
| `alwaysOn` | `true` | Keeps the site warm — no cold start / idle unload on Basic tier | N/A |
| `healthCheckPath` | `/health` | Azure pings this path on the running instance | Instance is taken out of load-balancer rotation, then restarted, after repeated consecutive failures |

Unlike Container Apps' three separate probe types (startup/liveness/readiness), App Service has
**one** health-check mechanism: it periodically requests `healthCheckPath` and, after enough
consecutive failures, marks the instance unhealthy (removed from routing) and eventually restarts
it. Both API and Web point this at `/health` — the same endpoint that waits for database
migrations and content sync to complete — rather than the DB-agnostic `/alive` endpoint.

**Trade-off vs the previous Container Apps model**: pointing the platform health check at `/health`
(which depends on the database) means a prolonged database outage could eventually cause App
Service to restart the site, whereas the old Container Apps setup deliberately kept its liveness
probe DB-agnostic (`/alive`) to avoid a restart storm during DB outages. This is an accepted
trade-off of the App Service migration — Basic B1 runs a single instance per site, so there is no
"remove one bad instance from the load balancer while others keep serving" benefit to gain from
splitting readiness/liveness, and `alwaysOn` combined with the simpler single-probe model was
judged an acceptable simplification for this app's traffic profile.

## Implementation Details

Health checks are implemented using standard ASP.NET Core Health Checks middleware and the `Aspire.ServiceDefaults` project.

- **Liveness Check** (`self`): Registered in `ServiceDefaults/Extensions.cs`, tagged with `"live"`. Checks GC memory pressure — returns unhealthy if memory usage exceeds 95% of the high memory load threshold.
- **Startup Health Check**: A custom health check tagged with `"ready"` (API only) that waits for database migrations and content synchronization to complete. Included in `/health` but not in `/alive`.
- **Aspire Service Defaults**: Maps both `/health` and `/alive` endpoints via `app.MapDefaultEndpoints()`.

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
- [api.bicep](../infra/modules/api.bicep) — API App Service health-check configuration
- [web.bicep](../infra/modules/web.bicep) — Web App Service health-check configuration
