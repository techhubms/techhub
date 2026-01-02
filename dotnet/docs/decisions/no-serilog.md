# Decision: No Serilog

**Date**: 2026-01-02  
**Status**: Decided  
**Decision Made By**: Product Owner

## Context

During Phase 1 Foundation implementation, Serilog was initially added for structured logging. However, this was not aligned with project requirements.

## Decision

**We will NOT use Serilog for this project.**

Instead, we rely on:

- **ASP.NET Core default logging**: Built-in `ILogger<T>` with console and debug providers
- **OpenTelemetry**: For distributed tracing and telemetry
- **Application Insights**: For production monitoring (Azure deployment)

## Rationale

1. **Simplicity**: ASP.NET Core's built-in logging is sufficient for our needs
2. **Less dependencies**: Fewer NuGet packages to manage and update
3. **Standard approach**: Using framework defaults reduces learning curve
4. **OpenTelemetry coverage**: Tracing and metrics already handled by OpenTelemetry
5. **Cloud-native**: Application Insights provides production-grade observability

## Consequences

### Positive

- Simpler dependency graph
- Framework-standard logging approach
- Less configuration overhead
- Better integration with OpenTelemetry

### Negative

- Less advanced structured logging features
- No file-based logging (rely on container/cloud logging instead)

## Implementation

- Removed `Serilog.AspNetCore` package from TechHub.Api
- Use standard `ILogger<T>` injection throughout codebase
- Configure logging levels via appsettings.json
- OpenTelemetry handles distributed tracing
