# Health Checks

This document describes the health check mechanism used in Tech Hub for monitoring and orchestration.

## Overview

Health checks are critical for ensuring the application is running correctly and ready to serve traffic. They are used by:

- **Orchestration (Aspire/Kubernetes)**: To determine if a container is alive (liveness) and ready to accept traffic (readiness).
- **Load Balancers**: To route traffic only to healthy instances.
- **Monitoring Systems**: To alert on downtime or degraded performance.

Tech Hub includes standard health checks for the API and Web components, plus a custom `startup` probe that ensures content synchronization is complete before the API declares itself ready.

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

**Use Case**: Readiness probes (is the app ready to serve traffic?). Orchestrators should wait for this before routing traffic to the instance.

### GET /alive

Liveness check endpoint that verifies only health checks tagged with "live" pass.

**Response**: `200 OK` with `text/plain` when the process is alive

```text
Healthy
```

**Behavior**:

- Returns `200 OK` if the ASP.NET Core host is running
- Does not check startup completion or database connectivity

**Use Case**: Liveness probes (is the process running?). Orchestrators can restart containers that fail this check.

## Implementation Details

Health checks are implemented using standard ASP.NET Core Health Checks middleware and the `Aspire.ServiceDefaults` project.

- **Startup Health Check**: A custom health check tagged with "ready" that waits for database migrations and content synchronization to complete. This check is included in `/health` but not in `/alive`.
- **Aspire Service Defaults**: Maps both `/health` and `/alive` endpoints via `app.MapDefaultEndpoints()`.

See [src/TechHub.Api/HealthChecks/StartupHealthCheck.cs](../src/TechHub.Api/HealthChecks/StartupHealthCheck.cs) for the startup health check implementation.
