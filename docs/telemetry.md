# Telemetry and Observability

Tech Hub uses three complementary telemetry systems: Application Insights for production monitoring, Google Analytics for user behavior analytics, and the Aspire Dashboard for local development observability.

## Telemetry Overview

| System | Scope | Environment | What It Captures |
|--------|-------|-------------|------------------|
| Application Insights (server) | API + Web backends | Staging, Production | Requests, dependencies, exceptions, traces, metrics, live metrics |
| Application Insights (browser) | Web frontend (client) | Production | Page views, browser exceptions, client-side performance |
| Google Analytics 4 | Web frontend (client) | Production | User behavior, page views, traffic analytics |
| Aspire Dashboard | API + Web backends | Development | Distributed traces, structured logs, metrics (via OTLP) |

## Application Insights

### Server-Side Telemetry

Both the API and Web services export OpenTelemetry data to Application Insights via the `Azure.Monitor.OpenTelemetry.AspNetCore` package. This is configured in `TechHub.ServiceDefaults` and runs automatically when the `APPLICATIONINSIGHTS_CONNECTION_STRING` environment variable is present.

The shared `AddServiceDefaults()` method configures:

- **Logging**: Structured logs with formatted messages and scopes
- **Metrics**: ASP.NET Core request metrics, HTTP client metrics, .NET runtime metrics
- **Tracing**: ASP.NET Core request traces, HTTP client dependency traces

When `APPLICATIONINSIGHTS_CONNECTION_STRING` is set, `UseAzureMonitor()` exports all three signals to Application Insights. When `OTEL_EXPORTER_OTLP_ENDPOINT` is set (local dev), `UseOtlpExporter()` sends the same signals to the Aspire Dashboard. Both exporters can run simultaneously.

### Cloud Role Names

Each service identifies itself in Application Insights via `OTEL_SERVICE_NAME`, which maps to the cloud role name shown in Application Map, Live Metrics, and transaction search:

| Service | Cloud Role Name | Set By |
|---------|----------------|--------|
| API | `techhub-api` | `OTEL_SERVICE_NAME` in Bicep and docker-compose |
| Web | `techhub-web` | `OTEL_SERVICE_NAME` in Bicep and docker-compose |

In local development with Aspire, the resource names `api` and `web` from the AppHost serve as service names automatically.

### Client-Side Telemetry (Browser SDK)

The Application Insights JavaScript SDK loads in the Web frontend for production environments. It captures browser-side page views, client exceptions, and performance timings. The SDK is injected conditionally in `App.razor` when both conditions are met:

1. `ASPNETCORE_ENVIRONMENT` is not `Development`
2. `APPLICATIONINSIGHTS_CONNECTION_STRING` is non-empty

Blazor enhanced navigation page views are tracked manually via `Blazor.addEventListener('enhancedload', ...)` since the SDK's automatic route tracking does not work with Blazor's client-side navigation.

### Environment Behavior

| Environment | Server Telemetry | Browser SDK |
|-------------|-----------------|-------------|
| Development | No (no connection string) | No |
| Staging | Yes | Yes |
| Production | Yes | Yes |

### Application Map

With both services reporting via `OTEL_SERVICE_NAME`, the Application Map shows:

- **techhub-web** - Blazor frontend, depends on techhub-api
- **techhub-api** - REST API, depends on PostgreSQL

The Web-to-API dependency is automatically captured through HTTP client tracing with distributed trace context propagation.

## Availability Tests

Application Insights availability tests actively probe the web frontend from five global locations every 5 minutes and alert when an outage is detected.

### What Is Tested

Each hostname in the `primaryHosts` Bicep parameter (e.g. `tech.hub.ms`, `tech.xebia.ms`) gets its own standard availability test that sends an HTTP GET to `https://<host>/` and validates:

- HTTP 200 response
- SSL certificate validity (alerts when fewer than 7 days remain)

### Probe Locations

| Location ID | Region |
|-------------|--------|
| `emea-nl-ams-azr` | West Europe (Amsterdam) |
| `us-ca-sjc-azr` | West US (San Jose) |
| `us-tx-sn1-azr` | South Central US |
| `apac-sg-sin-azr` | Southeast Asia (Singapore) |
| `emea-gb-db3-azr` | North Europe (Dublin) |

### Alert Behavior

A metric alert (severity 1) is created alongside each test. It fires when **2 or more** probe locations fail simultaneously within a 5-minute window, evaluated every minute. The threshold of 2 avoids false positives from transient single-location probe failures.

Alerts are created with an empty actions list — add an action group in the Azure Portal to receive email or webhook notifications.

### When No Tests Are Created

Tests are only created when `primaryHosts` is non-empty in the Bicep deployment. If no custom hostnames are provided, no availability tests or alerts are created.

## Google Analytics

GA4 is active in Production only (controlled by `GoogleAnalytics:MeasurementId` in `appsettings.Production.json`).

## Local Development (Aspire Dashboard)

During local development, the Aspire Dashboard at `https://localhost:18888` provides the same observability signals without requiring Azure resources.

The Aspire AppHost automatically configures `OTEL_EXPORTER_OTLP_ENDPOINT` for both services, which enables the OTLP exporter in `ServiceDefaults`. The dashboard shows:

- **Traces**: Distributed traces across API and Web with full request/dependency correlation
- **Structured Logs**: All log output with severity, scopes, and structured properties
- **Metrics**: Runtime metrics, request rates, response times, and HTTP client metrics

When running via docker-compose (`Run -Docker`), the same setup applies - an Aspire Dashboard container receives OTLP data from both services.

## Configuration Reference

### Environment Variables

| Variable | Purpose | Where Set |
|----------|---------|-----------|
| `APPLICATIONINSIGHTS_CONNECTION_STRING` | Enables Azure Monitor exporter | Bicep (staging/prod), not set locally |
| `OTEL_EXPORTER_OTLP_ENDPOINT` | Enables OTLP exporter for Aspire Dashboard | Aspire AppHost (auto), docker-compose |
| `OTEL_SERVICE_NAME` | Cloud role name in Application Insights / service name in Aspire | Bicep, docker-compose, Aspire AppHost (auto) |

### Infrastructure

| File | What It Does |
|------|-------------|
| [infra/modules/monitoring.bicep](../infra/modules/monitoring.bicep) | Creates Application Insights + Log Analytics Workspace + availability tests |
| [infra/modules/api.bicep](../infra/modules/api.bicep) | Passes connection string and service name to API container |
| [infra/modules/web.bicep](../infra/modules/web.bicep) | Passes connection string and service name to Web container |

## Implementation Reference

- [Extensions.cs](../src/TechHub.ServiceDefaults/Extensions.cs) - OpenTelemetry configuration and exporter setup
- [App.razor](../src/TechHub.Web/Components/App.razor) - Browser SDK and GA4 script injection
- [AppHost.cs](../src/TechHub.AppHost/AppHost.cs) - Aspire service orchestration
- [docker-compose.yml](../docker-compose.yml) - Docker environment variables for OTLP and service names
- [monitoring.bicep](../infra/modules/monitoring.bicep) - Application Insights Azure resource
