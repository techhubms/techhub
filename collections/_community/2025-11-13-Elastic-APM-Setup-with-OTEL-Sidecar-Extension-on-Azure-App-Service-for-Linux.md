---
layout: "post"
title: "Elastic APM Setup with OTEL Sidecar Extension on Azure App Service for Linux"
description: "This guide details how to instrument applications on Azure App Service for Linux using the OpenTelemetry (OTEL) sidecar extension with Elastic APM. It walks through retrieving the Elastic APM Server URL and secret token from Kibana, configuring sidecar extension deployment, setting required application environment variables, and validating telemetry data through Kibana’s observability tools. The content focuses on Azure deployment scenarios, code integration for PHP, Node.js, Python, and .NET, and includes links to sample repositories for practical reference."
author: "TulikaC"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/part-ii-otel-sidecar-extension-on-azure-app-service-for-linux/ba-p/4469576"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-13 08:44:41 +00:00
permalink: "/community/2025-11-13-Elastic-APM-Setup-with-OTEL-Sidecar-Extension-on-Azure-App-Service-for-Linux.html"
categories: ["Azure", "Coding", "DevOps"]
tags: [".NET", "APM Server", "Application Monitoring", "Azure", "Azure App Service", "Azure Monitor", "Coding", "Community", "Container Deployment", "DevOps", "Elastic APM", "Environment Variables", "Kibana", "Linux", "Node.js", "Observability", "OpenTelemetry", "OTEL Sidecar", "PHP", "Python", "Sample Repository", "Secret Token", "Telemetry"]
tags_normalized: ["dotnet", "apm server", "application monitoring", "azure", "azure app service", "azure monitor", "coding", "community", "container deployment", "devops", "elastic apm", "environment variables", "kibana", "linux", "nodedotjs", "observability", "opentelemetry", "otel sidecar", "php", "python", "sample repository", "secret token", "telemetry"]
---

TulikaC explains how to connect Azure App Service for Linux to Elastic APM using an OpenTelemetry sidecar extension—covering setup steps, app settings, and validation in Kibana.<!--excerpt_end-->

# Elastic APM Setup with OTEL Sidecar Extension on Azure App Service for Linux

This guide walks through deploying the OTEL (OpenTelemetry) Elastic sidecar extension on Azure App Service for Linux, enabling telemetry collection with Elastic APM. Most steps align with using Azure Monitor, but with Elastic-specific configuration and validation.

## Prerequisites

- Azure App Service for Linux (PHP, Node.js, Python, .NET)
- Elastic Cloud with Kibana access
- OTEL libraries added to your application
- Sample repo: [sidecar-samples/otel-sidecar](https://github.com/Azure-Samples/sidecar-samples/tree/main/otel-sidecar)

## Step 1: Obtain Elastic APM Server URL

In Kibana, navigate to **Observability → Data management → Fleet → Agent policies → Elastic Cloud agent policy → Elastic APM**. Copy the server URL from the "Server configuration" section.

## Step 2: Get or Generate the Secret Token

In the Elastic APM integration page, scroll to **Agent authorization**. Use the presented secret token or generate a new one as needed.

## Step 3: Add the OTEL Elastic Sidecar Extension

From your Web App's **Deployment Center**:

- Go to **Containers (new) → Add → Sidecar Extension**
- Select **Observability: OpenTelemetry – Elastic APM**
- Provide your APM Server URL and secret token
- Save changes

## Step 4: Verify Elastic-Specific App Settings

Default environment variables are:

```txt
ELASTIC_APM_ENDPOINT = https://<your-elastic-apm-server-url>
ELASTIC_APM_SECRET_TOKEN = <your-secret-token>
OTEL_EXPORTER = elastic
OTEL_EXPORTER_OTLP_ENDPOINT = http://127.0.0.1:4318
OTEL_SERVICE_NAME = <your-app-name>
```

- Continue using port `4318` for OTLP/HTTP to the sidecar.
- The Elastic URL is where the sidecar forwards signals.
- Other configurations (code, dependencies, autoload flag, startup command) mirror the Azure Monitor setup.

## Step 5: Validate Telemetry in Kibana

- In Kibana, navigate to **Observability → APM → Services**
- Locate your service by its OTEL_SERVICE_NAME
- Review transactions, traces, dependencies
- Additional logs and signals are available in Discover

## Reference Sample Repository

For hands-on implementation, use:
[Azure-Samples/sidecar-samples/otel-sidecar](https://github.com/Azure-Samples/sidecar-samples/tree/main/otel-sidecar) for code-based and container-based examples across PHP, Node.js, Python, and .NET.

## Next Steps

See Part III for a language cheat-sheet, app settings reference, and troubleshooting tips.

---
Author: TulikaC

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/part-ii-otel-sidecar-extension-on-azure-app-service-for-linux/ba-p/4469576)
