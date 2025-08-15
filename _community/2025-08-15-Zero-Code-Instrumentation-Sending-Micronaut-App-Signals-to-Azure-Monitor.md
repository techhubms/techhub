---
layout: "post"
title: "Zero-Code Instrumentation: Sending Micronaut App Signals to Azure Monitor"
description: "This community guide by Logico_jp demonstrates how to use zero-code instrumentation to send traces and metrics from Micronaut REST API applications to Azure Monitor (Application Insights). It covers prerequisite setup, dependencies, Maven and GraalVM configuration, environment variables, and testing, providing a hands-on walkthrough for Java developers integrating observability with Microsoft Azure."
author: "Logico_jp"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-signals-from-micronaut-applications-to-azure-monitor/ba-p/4443884"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-08-15 04:55:03 +00:00
permalink: "/2025-08-15-Zero-Code-Instrumentation-Sending-Micronaut-App-Signals-to-Azure-Monitor.html"
categories: ["Azure", "Coding", "DevOps"]
tags: ["Application Insights", "Azure", "Azure Monitor", "Azure Tracing", "Cloud Observability", "Coding", "Community", "Data JPA", "DevOps", "GraalVM", "Java", "Maven", "Metrics", "Micrometer Azure Monitor", "Micronaut", "Native Image", "Native Image.properties", "OpenTelemetry", "PostgreSQL", "REST API", "Telemetry", "Tracing", "Zero Code Instrumentation"]
tags_normalized: ["application insights", "azure", "azure monitor", "azure tracing", "cloud observability", "coding", "community", "data jpa", "devops", "graalvm", "java", "maven", "metrics", "micrometer azure monitor", "micronaut", "native image", "native image dot properties", "opentelemetry", "postgresql", "rest api", "telemetry", "tracing", "zero code instrumentation"]
---

Logico_jp explains how developers can enable zero-code instrumentation with Micronaut applications to automatically send telemetry (traces and metrics) to Azure Monitor. This guide walks through concrete steps, from setup to testing, offering practical insights for Java and Micronaut practitioners working with Azure.<!--excerpt_end-->

# Zero-Code Instrumentation: Sending Micronaut App Signals to Azure Monitor

**Author:** Logico_jp | *Posted August 13, 2025*

This guide shows how to configure a Micronaut REST API backend to automatically send traces and metrics to Azure Monitor (Application Insights) using zero-code instrumentation—no manual tracing code needed. The approach covers both JVM and GraalVM native image deployments.

## Background & Prerequisites

- This solution uses Micronaut (v4.9.0+), Java 21, Maven 3.9.10, Azure Monitor (Application Insights), and PostgreSQL Flexible Server.
- The application is a REST API for managing movie data, using PostgreSQL for persistence.

## Related Reading

- [Original Japanese post](https://logico-jp.dev/2025/08/13/use-zero-code-instrumentation-to-send-traces-and-metrics-from-a-micronaut-application-to-azure-monitor/)
- [Send signals from Micronaut native image applications to Azure Monitor](https://techcommunity.microsoft.com/blog/appsonazureblog/send-signals-from-micronaut-native-image-applications-to-azure-monitor/4443735)
- [Micronaut Launch](https://micronaut.io/launch/)
- [Zero-code | OpenTelemetry](https://opentelemetry.io/docs/concepts/instrumentation/zero-code/)

## Application Setup Overview

### 1. Project Archetype and Features

You can bootstrap your application using either the Micronaut CLI (`mn`) or [Micronaut Launch](https://micronaut.io/launch/). Choose the following features:

- `graalvm`, `management`, `micrometer-azure-monitor`, `azure-tracing`, `yaml`, `validation`, `postgres`, `jdbc-hikari`, `data-jpa`

### 2. Key Dependencies

To instrument HTTP and JDBC tracing with OpenTelemetry:

```xml
<dependency>
  <groupId>io.micronaut.tracing</groupId>
  <artifactId>micronaut-tracing-opentelemetry-http</artifactId>
</dependency>
<dependency>
  <groupId>io.micronaut.tracing</groupId>
  <artifactId>micronaut-tracing-opentelemetry-jdbc</artifactId>
</dependency>
```

For native image compatibility:

```xml
<dependency>
  <groupId>org.graalvm.buildtools</groupId>
  <artifactId>graalvm-reachability-metadata</artifactId>
  <version>0.11.0</version>
</dependency>
```

Add the GraalVM Maven plugin for native image configuration and optimization.

### 3. Environment Configuration

Configuration relies on `application.yml` for:

- Database credentials (`JDBC_URL`, `JDBC_USERNAME`, `JDBC_PASSWORD`)
- Azure Monitor Application Insights connection string (`AZURE_MONITOR_CONNECTION_STRING`)

Separate metric and trace configuration locations are needed—recommended via environment variables.

Sample snippet:

```yaml
micronaut:
  application:
    name: micronaut-telemetry-movie
  metrics:
    enabled: true
    export:
      azuremonitor:
        enabled: true
        step: PT1M
        connectionString: ${AZURE_MONITOR_CONNECTION_STRING}
datasources:
  default:
    driver-class-name: org.postgresql.Driver
    db-type: postgres
    url: ${JDBC_URL}
    username: ${JDBC_USERNAME}
    password: ${JDBC_PASSWORD}

azure:
  tracing:
    connection-string: ${AZURE_MONITOR_CONNECTION_STRING}
```

### 4. Native Image Configuration

**Native image** builds require generated metadata and plugin options:

- Run app with native-image agent to collect `config` and `trace` files.
- Create configuration files (`reflect-config.json`, `resource-config.json`, etc.) and place them in `src/main/resources/META-INF/native-image/{groupId}/{artifactId}`.
- Set up `native-image.properties` to define initialization timing and CLI options (`-Ob` for quicker builds).

Build the image with Maven:

```shell
mvn package -Dpackaging=native-image
```

## Testing and Observability

1. **Run as Java, then Native Image:** Confirm that traces and metrics are sent to Application Insights.
2. **Populate test data**:

```shell
curl -X PUT https://<app-url>/api/movies

# Should respond: { "message":"Database initialized with default movies." }
```

1. **Verify data and metrics**:

```shell
curl https://<app-url>/api/movies
curl https://<app-url>/metrics
```

JVM-specific metrics (e.g., `jvm.memory.max`) may not be available or return placeholder values (e.g., -2) in native images.

1. **Logs to Traces Table Limitation:**
- Zero-code cannot currently send application logs to Azure Monitor ‘traces’. Manual OpenTelemetry SDK configuration is required (custom `Appender` injection in controller constructor).

## Code Example

[GitHub: anishi1222/micronaut-telemetry-movie](https://github.com/anishi1222/micronaut-telemetry-movie)

## Further Reading and References

- [Configure Native Image with the Tracing Agent](https://www.graalvm.org/latest/reference-manual/native-image/guides/configure-with-tracing-agent/)
- [Collect Metadata with the Tracing Agent](https://www.graalvm.org/latest/reference-manual/native-image/metadata/AutomaticMetadataCollection/)
- [Send metrics from Micronaut native image applications to Azure Monitor](https://techcommunity.microsoft.com/blog/appsonazureblog/send-metrics-from-micronaut-native-image-applications-to-azure-monitor/4443763)
- [Send logs from Micronaut native image applications to Azure Monitor](https://techcommunity.microsoft.com/blog/appsonazureblog/send-logs-from-micronaut-native-image-applications-to-azure-monitor/4443867)

## Key Takeaways

- **Zero-code instrumentation** using Micronaut and OpenTelemetry simplifies exporting traces and metrics to Azure Monitor—no manual code required.
- **Native images** demand special configuration for accurate monitoring.
- **Application logs** require explicit OpenTelemetry SDK use for integration into Azure Monitor ‘traces’.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-signals-from-micronaut-applications-to-azure-monitor/ba-p/4443884)
