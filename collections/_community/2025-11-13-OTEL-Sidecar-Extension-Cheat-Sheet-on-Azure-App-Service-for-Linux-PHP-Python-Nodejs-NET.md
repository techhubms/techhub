---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/part-iii-otel-sidecar-extension-on-azure-app-service-for-linux/ba-p/4469589
title: 'OTEL Sidecar Extension Cheat-Sheet on Azure App Service for Linux: PHP, Python, Node.js, .NET'
author: TulikaC
feed_name: Microsoft Tech Community
date: 2025-11-13 10:17:50 +00:00
tags:
- .NET
- Application Insights
- Azure App Service
- Azure Monitor
- Elastic APM
- Environment Variables
- Linux
- Logging
- Metrics
- Node.js
- Observability
- OpenTelemetry
- OTEL EXPORTER
- OTEL Sidecar
- OTLP
- PHP
- Python
- Sample Repositories
- SCM DO BUILD DURING DEPLOYMENT
- Startup Commands
- Tracing
- Azure
- Coding
- DevOps
- Security
- Community
section_names:
- azure
- coding
- devops
- security
primary_section: coding
---
TulikaC shares a practical cheat-sheet for adding OpenTelemetry sidecar extensions to PHP, Python, Node.js, and .NET apps running on Azure App Service for Linux, including setup steps, default settings, debugging tips, and sample repos.<!--excerpt_end-->

# OTEL Sidecar Extension Cheat-Sheet on Azure App Service for Linux

**Author:** TulikaC

This cheat-sheet summarizes core setup details for running OpenTelemetry (OTEL) sidecar extensions on Azure App Service for Linux. Learn the essential steps for PHP, Python, Node.js, and .NET, including library choices, configuration, troubleshooting, and links to sample code.

## Quick Matrix by Language

| Language | OTEL Libraries | Key App Settings | Startup Command Example | Sample Repo |
|----------|----------------|------------------|------------------------|-------------|
| **PHP** | Composer autoloader (`vendor/autoload.php`), PECL extension `opentelemetry.so`, Composer plugins (Slim, PSR-18) | `OTEL_PHP_AUTOLOAD_ENABLED=true`, `SCM_DO_BUILD_DURING_DEPLOYMENT=true`, `OTEL_EXPORTER_OTLP_ENDPOINT=http://127.0.0.1:4318`, Sidecar: map `OTEL_PHP_AUTOLOAD_ENABLED` | `cp /home/site/wwwroot/default /etc/nginx/sites-enabled/default && nginx -s reload && bash /home/site/wwwroot/startup.sh` (installs opentelemetry, sets up extension, starts `php-fpm`) | [PHP Sample](https://github.com/Azure-Samples/sidecar-samples/tree/main/otel-sidecar/php) |
| **Python** | `opentelemetry-instrument` wrapper, auto-patches Flask/requests, installed via pip | `OTEL_TRACES_EXPORTER=otlp`, `OTEL_METRICS_EXPORTER=otlp`, `OTEL_LOGS_EXPORTER=otlp`, `OTEL_PYTHON_LOGGING_AUTO_INSTRUMENTATION_ENABLED=true`, `SCM_DO_BUILD_DURING_DEPLOYMENT=true`, Sidecar: map `OTEL_PYTHON_LOGGING_AUTO_INSTRUMENTATION_ENABLED` | `bash startup.sh` (includes `pip install -r requirements.txt` and runs with `opentelemetry-instrument`) | [Python Sample](https://github.com/Azure-Samples/sidecar-samples/tree/main/otel-sidecar/python) |
| **Node.js** | npm packages: `@opentelemetry/sdk-node`, `@opentelemetry/auto-instrumentations-node`, `@opentelemetry/exporter-trace-otlp-http`; init in `tracing.js` and preload via `--require` or `NODE_OPTIONS` | `OTEL_TRACES_EXPORTER=otlp`, `OTEL_METRICS_EXPORTER=otlp`, `OTEL_LOGS_EXPORTER=otlp`, `SCM_DO_BUILD_DURING_DEPLOYMENT=true` | `node --require ./tracing.js <your-app>.js` (or set `NODE_OPTIONS=--require ./tracing.js`) | [Node Sample](https://github.com/Azure-Samples/sidecar-samples/tree/main/otel-sidecar/nodejs) |
| **.NET** | NuGet: `OpenTelemetry.Extensions.Hosting`, `OpenTelemetry.Instrumentation.AspNetCore`, `OpenTelemetry.Instrumentation.Http`, `OpenTelemetry.Exporter.OpenTelemetryProtocol`; configured in `Program.cs` via `AddOpenTelemetry()` | `OTEL_DOTNET_AUTO_TRACES_ENABLED=true`, `OTEL_DOTNET_AUTO_METRICS_ENABLED=true`, `OTEL_DOTNET_AUTO_LOGS_ENABLED=true` | No custom startup command needed | [.NET Sample](https://github.com/Azure-Samples/sidecar-samples/tree/main/otel-sidecar/dotnet) |

## Default App Settings for Backends

**Azure Monitor (Application Insights):**

```text
APPLICATIONINSIGHTS_CONNECTION_STRING=<your-application-insights-connection-string>
OTEL_EXPORTER=azuremonitor
OTEL_EXPORTER_OTLP_ENDPOINT=http://127.0.0.1:4318
OTEL_SERVICE_NAME=<your-app-name>
```

**Elastic APM:**

```text
ELASTIC_APM_ENDPOINT=https://<your-elastic-endpoint>
ELASTIC_APM_SECRET_TOKEN=<your-elastic-token>
OTEL_EXPORTER=elastic
OTEL_EXPORTER_OTLP_ENDPOINT=http://127.0.0.1:4318
OTEL_SERVICE_NAME=<your-app-name>
```

**Notes:**

- These settings should be added to the main app; the sidecar reads and forwards relevant data.
- Port `:4318` used for OTLP/HTTP by default. Use `:4317` for OTLP/gRPC if needed.
- Assign a clear `OTEL_SERVICE_NAME` (e.g., `myapp-api`) for easier trace management.

## Sample Repository

Complete code and container samples for all supported languages:
[https://github.com/Azure-Samples/sidecar-samples/tree/main/otel-sidecar](https://github.com/Azure-Samples/sidecar-samples/tree/main/otel-sidecar)

## Troubleshooting Checklist

- **No telemetry appears:**
  - Check sidecar container status: Deployment Center → Containers → Running
  - Verify environment variables like `OTEL_EXPORTER_OTLP_ENDPOINT` and backend credentials
  - Inspect logs via Monitoring → Log stream (look for errors or success messages)
  - For extra debugging: set `OTEL_TELEMETRY_LOG_LEVEL=debug` in App Settings and retest

- **Sidecar fails to start:**
  - Ensure resource plan meets requirements (e.g., P0v3 tier or higher)
  - Required settings present for your backend (Application Insights or Elastic)
  - Review sidecar startup logs for configuration errors

- **High memory usage:**
  - Enable sampling, filter unnecessary endpoints, reduce log verbosity
  - Scale up app service plan if needed

## Resources

- [App Service Sidecar Overview](https://learn.microsoft.com/azure/app-service/overview-sidecar)
- [Sidecar Extensions Introduction](https://azure.github.io/AppService/2025/03/19/Sidecar-extensions.html)
- [OpenTelemetry Documentation](https://opentelemetry.io/docs/)
- [Elastic Observability APM Docs](https://www.elastic.co/guide/en/observability/current/apm.html)

## Key Takeaways

With OTEL sidecar support, Azure App Service for Linux enables robust observability for code and container apps. Standard app settings, sample code, and troubleshooting practices simplify getting started with distributed tracing and metrics collection for multiple languages.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/part-iii-otel-sidecar-extension-on-azure-app-service-for-linux/ba-p/4469589)
