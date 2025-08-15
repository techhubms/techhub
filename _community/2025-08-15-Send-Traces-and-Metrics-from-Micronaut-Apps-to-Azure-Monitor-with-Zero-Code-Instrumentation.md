---
layout: "post"
title: "Send Traces and Metrics from Micronaut Apps to Azure Monitor with Zero-Code Instrumentation"
description: "This technical guide walks through using zero-code instrumentation to send traces and metrics from a Micronaut-based Java API to Azure Monitor's Application Insights. It explains setup with Maven, GraalVM native-image, and OpenTelemetry integrations, covering configuration, environment variables, and common stumbling blocks. The article includes a sample application storing movie data in PostgreSQL and details practical telemetry output and caveats for JVM/native images."
author: "Logico_jp"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-signals-from-micronaut-applications-to-azure-monitor/ba-p/4443884"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-15 04:55:03 +00:00
permalink: "/2025-08-15-Send-Traces-and-Metrics-from-Micronaut-Apps-to-Azure-Monitor-with-Zero-Code-Instrumentation.html"
categories: ["Azure"]
tags: ["Application Configuration", "Application Insights", "Azure", "Azure Monitor", "Azure Tracing", "Community", "Environment Variables", "GraalVM", "Java", "JDK 21", "Maven", "Metrics", "Micrometer", "Micronaut", "Micronaut Tracing Opentelemetry", "Native Image", "OpenTelemetry", "PostgreSQL", "Telemetry", "Tracing", "Zero Code Instrumentation"]
tags_normalized: ["application configuration", "application insights", "azure", "azure monitor", "azure tracing", "community", "environment variables", "graalvm", "java", "jdk 21", "maven", "metrics", "micrometer", "micronaut", "micronaut tracing opentelemetry", "native image", "opentelemetry", "postgresql", "telemetry", "tracing", "zero code instrumentation"]
---

Logico_jp provides a detailed walkthrough on instrumenting Micronaut Java applications to send traces and metrics to Azure Monitor, using zero-code OpenTelemetry and native-image techniques.<!--excerpt_end-->

# Send Traces and Metrics from Micronaut Apps to Azure Monitor with Zero-Code Instrumentation

Author: Logico_jp  
Original post: 13 August 2025

This article demonstrates how to use zero-code instrumentation to collect telemetry (traces and metrics) from a Micronaut-based REST API and send it directly to Azure Monitor's Application Insights. The approach focuses on minimizing code changes—leveraging configuration and dependencies for tracing HTTP and JDBC activity, and deploying as both a regular JVM application and a GraalVM native image.

## Prerequisites

- Maven 3.9.10
- JDK 21
- Micronaut 4.9.0 or later
- Azure Monitor / Application Insights provisioned
- PostgreSQL Flexible Server provisioned

## Key References

- [Series background](https://techcommunity.microsoft.com/blog/appsonazureblog/send-signals-from-micronaut-native-image-applications-to-azure-monitor/4443735)
- [Zero-Code Instrumentation Overview (OpenTelemetry)](https://opentelemetry.io/docs/concepts/instrumentation/zero-code/)
- [Sample App Code (GitHub)](https://github.com/anishi1222/micronaut-telemetry-movie)

## Project Archetype and Features

When creating the archetype (via `mn` CLI or [Micronaut Launch](https://micronaut.io/launch/)), include these features:

- graalvm
- management
- micrometer-azure-monitor
- azure-tracing
- yaml (for `application.yml` config)
- validation
- postgres
- jdbc-hikari
- data-jpa

## Core Dependencies

To instrument HTTP and JDBC:

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

For GraalVM native-image:

```xml
<dependency>
  <groupId>org.graalvm.buildtools</groupId>
  <artifactId>graalvm-reachability-metadata</artifactId>
  <version>0.11.0</version>
</dependency>
```

Maven plugin for native image:

```xml
<plugin>
  <groupId>org.graalvm.buildtools</groupId>
  <artifactId>native-maven-plugin</artifactId>
  <configuration>
    <metadataRepository>
      <enabled>true</enabled>
    </metadataRepository>
    <buildArgs combine.children="append">
      <buildArg>-Ob</buildArg>
    </buildArgs>
    <quickBuild>true</quickBuild>
  </configuration>
</plugin>
```

## Application Configuration (application.yml)

Set up database and Azure Monitor connections by environment variable:

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

otel:
  exclusions: /health, /info, /metrics, /actuator/health, /actuator/info, /actuator/metrics
```

## Building and Running

- **Java application**: Run with standard JVM to verify traces/metrics reach Application Insights.
- **Native image**: Use Maven's `native-maven-plugin` and GraalVM, collect config files with the tracing agent, and build with `mvn package -Dpackaging=native-image`.
- Place configuration files (reflect, proxy, resource, reachability-metadata) under `src/main/resources/META-INF/native-image/{groupId}/{artifactId}` for pickup by the native-image tool.

Refer to [GraalVM docs](https://www.graalvm.org/latest/reference-manual/native-image/guides/configure-with-tracing-agent/) for config file generation.

## Testing

- Initialize database data:

  ```shell
  curl -X PUT https://<app_url>/api/movies
  # Response: {"message":"Database initialized with default movies."}
  ```

- Verify movies exist:

  ```shell
  curl https://<app_url>/api/movies
  ```

- Check metrics via REST endpoint: `/metrics`

## Caveats

- JVM-related metrics (like `jvm.memory.max`) do not provide full info in native images (returns -2).
- Logs require explicit OpenTelemetry setup; zero-code only covers metrics/traces.

Sample log setup (constructor-based OpenTelemetry registration):

```java
@Inject
AzureTracingConfigurationProperties azureTracingConfigurationProperties;

private static final Logger logger = LoggerFactory.getLogger(MovieController.class);

public MovieController(AzureTracingConfigurationProperties azureTracingConfigurationProperties) {
  this.azureTracingConfigurationProperties = azureTracingConfigurationProperties;
  AutoConfiguredOpenTelemetrySdkBuilder sdkBuilder = AutoConfiguredOpenTelemetrySdk.builder();
  AzureMonitorAutoConfigure.customize(sdkBuilder, azureTracingConfigurationProperties.getConnectionString());
  OpenTelemetryAppender.install(sdkBuilder.build().getOpenTelemetrySdk());
  logger.info("OpenTelemetry configured for MovieController.");
}
```

## Links to Deeper Dives

- [Send metrics from Micronaut native image applications to Azure Monitor](https://techcommunity.microsoft.com/blog/appsonazureblog/send-metrics-from-micronaut-native-image-applications-to-azure-monitor/4443763)
- [Send logs from Micronaut native image applications to Azure Monitor](https://techcommunity.microsoft.com/blog/appsonazureblog/send-logs-from-micronaut-native-image-applications-to-azure-monitor/4443867)
- [Micronaut-telemetry-movie on GitHub](https://github.com/anishi1222/micronaut-telemetry-movie)

## Summary

Zero-code instrumentation significantly streamlines collecting performance and trace data from Micronaut applications to Azure Monitor. Explicit configuration (and sometimes explicit code for logs) is still occasionally necessary, especially for advanced scenarios or native images.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-signals-from-micronaut-applications-to-azure-monitor/ba-p/4443884)
