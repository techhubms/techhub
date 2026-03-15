# Health Checks

This document describes the health check mechanism used in Tech Hub for monitoring and orchestration.

## Overview

Health checks are critical for ensuring the application is running correctly and ready to serve traffic. They are used by:

- **Azure Container Apps**: Startup, liveness, and readiness probes configured in Bicep
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

**Use Case**: Startup and liveness probes — is the process alive and responsive?

## Container Apps Probe Configuration

Both API and Web container apps configure three probe types in their Bicep modules:

| Probe | Endpoint | Purpose | Failure behavior |
|-------|----------|---------|-----------------|
| **Startup** | `GET /alive` | Gives the app time to initialize without being killed | Container is killed and restarted after max startup time |
| **Liveness** | `GET /alive` | Detects hung processes or critical memory pressure | Container is restarted |
| **Readiness** | `GET /health` | Holds traffic until migrations + content sync complete | Traffic is not routed to the container |

### API Probe Timings

The API has longer startup times due to database migrations and content synchronization:

- **Startup**: 5s initial delay, 10s period, 30 failures max (305s total)
- **Liveness**: 30s period, 3 failures before restart
- **Readiness**: 10s period, 3 failures before removing from load balancer

### Web Probe Timings

The Web frontend starts faster as it only needs to initialize the Blazor runtime:

- **Startup**: 3s initial delay, 5s period, 12 failures max (63s total)
- **Liveness**: 30s period, 3 failures before restart
- **Readiness**: 10s period, 3 failures before removing from load balancer

## Implementation Details

Health checks are implemented using standard ASP.NET Core Health Checks middleware and the `Aspire.ServiceDefaults` project.

- **Liveness Check** (`self`): Registered in `ServiceDefaults/Extensions.cs`, tagged with `"live"`. Checks GC memory pressure — returns unhealthy if memory usage exceeds 95% of the high memory load threshold.
- **Startup Health Check**: A custom health check tagged with `"ready"` (API only) that waits for database migrations and content synchronization to complete. Included in `/health` but not in `/alive`.
- **Aspire Service Defaults**: Maps both `/health` and `/alive` endpoints via `app.MapDefaultEndpoints()`.

### Why Liveness Doesn't Check the Database

Liveness probes should only verify the process itself is healthy. If liveness checked the database:

1. A DB outage would fail all liveness probes
2. Container Apps would restart all containers simultaneously
3. Containers restart, try to connect to the still-down DB, fail liveness again
4. Restart storm — all containers cycling indefinitely

The readiness probe (`/health`) handles external dependency awareness by removing containers from the load balancer without restarting them.

## Implementation Reference

- [ServiceDefaults/Extensions.cs](../src/TechHub.ServiceDefaults/Extensions.cs) — Liveness check and endpoint mapping
- [StartupHealthCheck.cs](../src/TechHub.Api/HealthChecks/StartupHealthCheck.cs) — API startup health check
- [api.bicep](../infra/modules/api.bicep) — API Container Apps probe configuration
- [web.bicep](../infra/modules/web.bicep) — Web Container Apps probe configuration
