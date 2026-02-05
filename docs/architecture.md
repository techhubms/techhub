# Architecture

## .NET Aspire

Tech Hub uses **.NET Aspire** for orchestration, observability, and service discovery.

### Capabilities

- **Service Discovery**: Web frontend automatically discovers the API via `https+http://api`
- **OpenTelemetry**: Distributed tracing, metrics, and structured logging
- **Resilience**: Built-in retry policies and circuit breakers for HTTP clients
- **Health Checks**: `/health` and `/alive` endpoints on all services

### Port Configuration

**Goal**: HTTPS everywhere, fixed ports, single source of truth (no redundant configuration).

**Strategy**: Define ports in `launchSettings.json` only. Aspire reads these automatically - no explicit endpoint configuration needed in AppHost.

| Service | Port | Protocol | Configuration Location |
|---------|------|----------|------------------------|
| API | 5001 | HTTPS | [TechHub.Api/Properties/launchSettings.json](../src/TechHub.Api/Properties/launchSettings.json) |
| Web | 5003 | HTTPS | [TechHub.Web/Properties/launchSettings.json](../src/TechHub.Web/Properties/launchSettings.json) |
| Aspire Dashboard | 18888 | HTTP | Auto-configured by Aspire |

**Benefits**:

- Same ports whether running via Aspire (`Run`) or directly (`dotnet run`)
- No duplicate endpoint definitions
- Single source of truth prevents configuration drift

### Service Model

```text
┌─────────────────────────────────────────────────────────────┐
│                    Aspire AppHost                           │
│  (Orchestrates services, configures service discovery)      │
└─────────────────────────────────────────────────────────────┘
                              │
              ┌───────────────┼───────────────┐
              ▼               ▼               ▼
┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐
│   TechHub.Api   │  │   TechHub.Web   │  │ Aspire Dashboard│
│   (Port 5001)   │◄─┤   (Port 5003)   │  │  (Port 18888)   │
│                 │  │                 │  │                 │
│ - REST API      │  │ - Blazor SSR    │  │ - Traces        │
│ - Swagger UI    │  │ - Service       │  │ - Metrics       │
│ - Health checks │  │   Discovery     │  │ - Logs          │
└─────────────────┘  └─────────────────┘  └─────────────────┘
         │                    │                    ▲
         │                    │                    │
         └────────────────────┴──── OTLP ─────────┘
                        (OpenTelemetry)
```

### Shared Configuration

The `TechHub.ServiceDefaults` project provides shared Aspire configuration:

```csharp
// In API and Web Program.cs
builder.AddServiceDefaults();  // Adds OpenTelemetry, health checks, resilience
// ...
app.MapDefaultEndpoints();     // Maps /health and /alive endpoints
```

## Running with Aspire

See [running-and-testing.md](running-and-testing.md) for details on running the Aspire AppHost and accessing the dashboard.
