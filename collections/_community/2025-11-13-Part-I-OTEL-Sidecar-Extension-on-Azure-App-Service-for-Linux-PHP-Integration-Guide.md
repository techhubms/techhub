---
layout: "post"
title: "Part I: OTEL Sidecar Extension on Azure App Service for Linux – PHP Integration Guide"
description: "This guide provides a step-by-step walkthrough on adding observability to PHP Linux web apps using the OpenTelemetry – Azure Monitor sidecar extension on Azure App Service. It covers Application Insights setup, environment variables, PHP dependencies, deployment, and using Application Insights for telemetry. The tutorial helps developers instrument traces, metrics, and logs in their apps without modifying core code."
author: "TulikaC"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/part-i-otel-sidecar-extension-on-azure-app-service-for-linux/ba-p/4469514"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-13 06:28:27 +00:00
permalink: "/community/2025-11-13-Part-I-OTEL-Sidecar-Extension-on-Azure-App-Service-for-Linux-PHP-Integration-Guide.html"
categories: ["Azure", "Coding", "DevOps"]
tags: ["Application Insights", "Azure", "Azure App Service", "Azure Monitor", "Coding", "Community", "Composer", "Containerization", "Deployment", "DevOps", "Environment Variables", "Linux", "Monitoring", "Observability", "OpenTelemetry", "PHP", "Portal Configuration", "Sidecar Extension", "Startup Script", "Telemetry"]
tags_normalized: ["application insights", "azure", "azure app service", "azure monitor", "coding", "community", "composer", "containerization", "deployment", "devops", "environment variables", "linux", "monitoring", "observability", "opentelemetry", "php", "portal configuration", "sidecar extension", "startup script", "telemetry"]
---

TulikaC explains how developers can add observability to PHP Linux web apps using Azure App Service sidecar extensions and OpenTelemetry – Azure Monitor. This detailed guide covers setup, deployment, and verification in Application Insights.<!--excerpt_end-->

# Part I: OTEL Sidecar Extension on Azure App Service for Linux – PHP Integration Guide

## Introduction

Sidecar extensions let you attach a companion container to your Azure App Service for Linux app, enabling additional capabilities like observability without altering your main app or image. This walkthrough demonstrates how to use the OpenTelemetry – Azure Monitor sidecar to collect traces, metrics, and logs from a PHP web application.

If you're new to sidecars, start with the [official documentation](https://learn.microsoft.com/en-us/azure/app-service/overview-sidecar).

## What is OpenTelemetry (OTEL)?

OpenTelemetry is a vendor-neutral standard for collecting distributed tracing, metrics, and logs. It provides auto/manual instrumentation for several popular programming languages and integrates with various backends. Read more at [OpenTelemetry Docs](https://opentelemetry.io/docs/).

## Prerequisites

- Azure subscription
- Familiarity with Azure portal and App Service deployments
- Sample PHP application (provided in this guide)

## Walkthrough: Add OpenTelemetry – Azure Monitor Sidecar to a PHP Web App

### 1. Create Application Insights Resource

- In the Azure portal, set up Application Insights (or reuse an existing instance)
- Copy the **Connection string** from the Overview blade

### 2. Create the PHP Web App (Linux)

- Deploy a new **App Service (Linux)**
- Select a supported PHP version (e.g., PHP 8.4)

### 3. Configure Environment Variables

In Application Settings, add:

- `OTEL_PHP_AUTOLOAD_ENABLED = true`
- (Optional) `SCM_DO_BUILD_DURING_DEPLOYMENT = true`

When adding the sidecar extension, ensure these variables are set:

- `APPLICATIONINSIGHTS_CONNECTION_STRING=<your-connection-string>`
- `OTEL_EXPORTER=azuremonitor`
- `OTEL_EXPORTER_OTLP_ENDPOINT=http://127.0.0.1:4318`
- `OTEL_SERVICE_NAME=php-blessed-otel` (or another meaningful name)

### 4. Clone the App Code

```shell
git clone https://github.com/Azure-Samples/sidecar-samples.git
cd otel-sidecar/php/php-blessed-app
```

### 5. Confirm PHP Dependencies

Dependencies are declared in `composer.json`:

```json
"require": {
  "open-telemetry/sdk": "^1.7",
  "open-telemetry/exporter-otlp": "^1.3",
  "open-telemetry/opentelemetry-auto-slim": "^1.2",
  "open-telemetry/opentelemetry-auto-psr18": "^1.1",
  "monolog/monolog": "^3.0",
  "open-telemetry/opentelemetry-logger-monolog": "^1.0"
},
"config": {
  "allow-plugins": {
    "open-telemetry/opentelemetry-auto-slim": true,
    "open-telemetry/opentelemetry-auto-psr18": true
  }
}
```

### 6. Minimal bootstrap in index.php

```php
use OpenTelemetry\API\Globals;
require __DIR__ . '/vendor/autoload.php';
```

### 7. Startup Script

Include a `startup.sh` like:

```bash
# !/bin/bash

if ! php -m | grep -q opentelemetry; then
  echo "Installing OpenTelemetry extension..."
  pecl install opentelemetry
  echo "extension=opentelemetry.so" > /usr/local/etc/php/conf.d/99-opentelemetry.ini
  echo "OpenTelemetry extension installed successfully"
fi

echo "Starting PHP-FPM..."
php-fpm
```

### 8. Deploy the App

Deploy via GitHub Actions, ZIP deploy, local Git, etc.

### 9. Add Sidecar Extension

- Go to **Deployment Center → Containers (new) → Add → Sidecar Extension**
- Select **Observability: OpenTelemetry – Azure Monitor**
- Paste your Application Insights Connection String

### 10. Map the Autoload Flag

- Edit the sidecar container
- Map variable `OTEL_PHP_AUTOLOAD_ENABLED` from the main app to the sidecar

### 11. Set Startup Command for PHP

In Configuration (preview) → Stack settings, set:

```bash
cp /home/site/wwwroot/default /etc/nginx/sites-enabled/default && nginx -s reload && bash /home/site/wwwroot/startup.sh
```

### 12. Verify Application Insights Telemetry

- After restarting, open Application Insights
- Use Application Map, Live Metrics, or Search for `service.name = php-blessed-otel`

## Conclusion

Sidecar extensions make adding observability to Azure PHP web apps simple and modular. Using OpenTelemetry with Azure Monitor, you can generate portable traces, metrics, and logs for querying and dashboards — all without changing your main codebase.

For connecting to other backends like Elastic APM, see the next part of this series.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/part-i-otel-sidecar-extension-on-azure-app-service-for-linux/ba-p/4469514)
