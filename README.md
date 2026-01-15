# Tech Hub .NET Migration

> **Status**: 🚧 In Development

This directory contains the .NET/Blazor implementation of Tech Hub, migrating from the Jekyll-based static site.

## Quick Start

**Easiest Way - F5 in VS Code**:

1. Open the project in VS Code
2. Press **F5** (or click **Run > Start Debugging**)
3. Select **"Tech Hub (API + Web)"** from the dropdown
4. Web UI opens automatically at <http://localhost:5184>
5. API available at <http://localhost:5029> (Swagger: <http://localhost:5029/swagger>)

**Alternative - PowerShell Function**:

```powershell
# Build and run both API and Web
Run

# Clean build and test first, then start both API and Web if tests pass
Run -Clean -Test
```

**Stop the Application**: Press `Ctrl+C` in the terminal where the function is running.

## .NET Aspire

Tech Hub uses **.NET Aspire** for orchestration, observability, and service discovery. Aspire provides:

- **Service Discovery**: Web frontend automatically discovers the API via `https+http://api`
- **OpenTelemetry**: Distributed tracing, metrics, and structured logging
- **Resilience**: Built-in retry policies and circuit breakers for HTTP clients
- **Health Checks**: `/health` and `/alive` endpoints on all services

### Running with Aspire

#### Via Run Function (Recommended)

```powershell
# Default mode - uses Aspire AppHost to orchestrate API + Web
Run

# With Aspire Dashboard for telemetry visualization
Run -Dashboard
```

#### Direct AppHost

```powershell
dotnet run --project src/TechHub.AppHost/TechHub.AppHost.csproj
```

#### VS Code Task

Press `Ctrl+Shift+P` → "Tasks: Run Task" → "run-apphost"

### Aspire Dashboard

The Aspire Dashboard provides real-time visualization of:

- **Traces**: Distributed request tracing across services
- **Metrics**: Performance counters, request rates, error rates
- **Logs**: Structured logs from all services
- **Resources**: Service health and status

To start the dashboard:

```powershell
# Start with dashboard (runs as Docker container)
Run -Dashboard

# Dashboard URL: http://localhost:18888
# Note: Copy the login token from the terminal output
```

Manual dashboard start (if needed separately):

```bash
docker run --rm -it -d \
    -p 18888:18888 \
    -p 4317:18889 \
    --name aspire-dashboard \
    mcr.microsoft.com/dotnet/aspire-dashboard:9.5
```

### Architecture

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
│   (Port 5029)   │◄─┤   (Port 5184)   │  │  (Port 18888)   │
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

### ServiceDefaults

The `TechHub.ServiceDefaults` project provides shared Aspire configuration:

```csharp
// In API and Web Program.cs
builder.AddServiceDefaults();  // Adds OpenTelemetry, health checks, resilience
// ...
app.MapDefaultEndpoints();     // Maps /health and /alive endpoints
```

## Migration Status

This project is currently migrating from Jekyll to .NET/Blazor using [spec-kit](https://github.com/github/spec-kit) methodology. We're in **Phase 3** of the migration with 245/245 unit/integration tests and 60/69 E2E tests passing.

**Quick Status**: API ✅ Complete | Frontend 🔄 In Progress (95%)

For detailed progress, implementation status, and what's working now, see [MIGRATIONSTATUS.md](MIGRATIONSTATUS.md).

## Documentation

For complete development guidelines, architecture, and coding standards, see [AGENTS.md](AGENTS.md).

**Quick Links**:

- [AI Assistant Workflow](AGENTS.md#ai-assistant-workflow) - Required 10-step development process
- [.NET Tech Stack & Commands](AGENTS.md#net-tech-stack) - Runtime, frameworks, and CLI commands
- [Documentation Architecture](AGENTS.md#documentation-architecture) - Complete documentation map

## DevContainer Setup

1. In VS Code, open Command Palette (`Ctrl+Shift+P`)
2. Select **"Dev Containers: Reopen in Container"**
3. Wait for container to build and initialize
4. Use F5 or run script as described above

## Contributing

All development must follow the guidelines in [AGENTS.md](AGENTS.md):

- **[AI Assistant Workflow](AGENTS.md#ai-assistant-workflow)** - Required 10-step process
- **[Core Rules & Boundaries](AGENTS.md#1-core-rules--boundaries)** - Non-negotiable rules
- **[Feature Specifications](specs/)** - Use spec-kit methodology for all features
