---
layout: "post"
title: "Sending Metrics from Micronaut Native Image Applications to Azure Monitor"
description: "This guide, authored by Logico_jp, walks through configuring a Java Micronaut application (including as a GraalVM native image) to send custom and built-in metrics to Azure Monitor (Application Insights). It covers prerequisites, detailed steps for metrics setup, instrumentation keys, environment variable usage, Maven and GraalVM build configurations, and validating metrics transmission to Azure Monitor."
author: "Logico_jp"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-metrics-from-micronaut-native-image-applications-to-azure/ba-p/4443763"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-08-15 04:57:57 +00:00
permalink: "/2025-08-15-Sending-Metrics-from-Micronaut-Native-Image-Applications-to-Azure-Monitor.html"
categories: ["Azure", "DevOps"]
tags: ["Application Configuration", "Application Insights", "Azure", "Azure Container Apps", "Azure Monitor", "Community", "Custom Metrics", "DevOps", "GraalVM", "Instrumentation Key", "Java", "Maven", "Metrics", "Micrometer", "Micronaut", "Monitoring", "Native Image", "Pom.xml", "REST API", "YAML"]
tags_normalized: ["application configuration", "application insights", "azure", "azure container apps", "azure monitor", "community", "custom metrics", "devops", "graalvm", "instrumentation key", "java", "maven", "metrics", "micrometer", "micronaut", "monitoring", "native image", "pom dot xml", "rest api", "yaml"]
---

Logico_jp provides a comprehensive walkthrough on integrating Micronaut native image applications with Azure Monitor, showing step-by-step configuration and build details for effective metrics shipping.<!--excerpt_end-->

# Sending Metrics from Micronaut Native Image Applications to Azure Monitor

*Author: Logico_jp*

This guide demonstrates how to configure a Micronaut-based Java application—packaged as a GraalVM native image—to send metrics (both standard and custom) to Azure Monitor (Application Insights).

## Prerequisites

- Maven 3.9.10
- JDK 21
- Micronaut 4.9.0 or later
- Reference tutorials for additional context

## Project Generation and Configuration

You can generate the project using Micronaut CLI or Micronaut Launch:

```bash
$ mn create-app \
  --build=maven \
  --jdk=21 \
  --lang=java \
  --test=junit \
  --features=validation,graalvm,micrometer-azure-monitor,http-client,micrometer-annotation,yaml \
  dev.logicojp.micronaut.azuremonitor-metric
```

**Project Features:**

- validation
- graalvm
- micrometer-azure-monitor
- http-client
- micrometer-annotation
- yaml

Download and extract the archetype as usual.

## Implementation Notes

### Directory Structure

The example merges code instead of separating into `azure` and `lib` folders, reflecting the standard Micronaut archetype structure.

### Azure Monitor Instrumentation Key

Configuration for the Azure Monitor registry requires a connection string that includes the Instrumentation Key:

**In `application.yml`:**

```yaml
micronaut:
  metrics:
    enabled: true
    export:
      azuremonitor:
        enabled: true
        connectionString: ${AZURE_MONITOR_CONNECTION_STRING}
```

Alternatively, set via environment variable or in `application.properties` using the same property name.

### Meter Binders Configuration

Enable desired binders:

```yaml
micronaut:
  application:
    name: azuremonitor-metric
  metrics:
    enabled: true
    binders:
      files:
        enabled: true
      jdbc:
        enabled: true
      jvm:
        enabled: true
      logback:
        enabled: true
      processor:
        enabled: true
      uptime:
        enabled: true
      web:
        enabled: true
    export:
      azuremonitor:
        enabled: true
        step: PT1M
        connectionString: ${AZURE_MONITOR_CONNECTION_STRING}
```

## Maven and Native Image Setup

**Add GraalVM Reachability Metadata:**

```xml
<dependency>
  <groupId>org.graalvm.buildtools</groupId>
  <artifactId>graalvm-reachability-metadata</artifactId>
  <version>0.11.0</version>
</dependency>
```

**GraalVM Maven Plugin Example:**

```xml
<plugin>
  <groupId>org.graalvm.buildtools</groupId>
  <artifactId>native-maven-plugin</artifactId>
  <configuration>
    <metadataRepository>
      <enabled>true</enabled>
    </metadataRepository>
    <buildArgs combine.children="append">
      <buildArg>-Ob</buildArg> <!-- Optimization level -->
    </buildArgs>
    <quickBuild>true</quickBuild>
  </configuration>
</plugin>
```

Build as a standard Java application:

```bash
$ mvn clean package
```

## Native Image Build and Instrumentation

1. **Collect configuration:**
   - Use the GraalVM tracing agent to generate reflection and proxy configs:

     ```bash
     $JAVA_HOME/bin/java \
       -agentlib:native-image-agent=config-output-dir=src/main/resources/META-INF/{groupId}/{artifactId}/ \
       -jar ./target/{artifactId}-{version}.jar
     ```

   - Optionally, generate a trace file and reachability metadata:

   ```bash
   $JAVA_HOME/bin/java \
     -agentlib:native-image-agent=trace-output=/path/to/trace-file.json \
     -jar ./target/{artifactId}-{version}.jar
   native-image-configure generate \
     --trace-input=/path/to/trace-file.json \
     --output-dir=/path/to/config-dir/
   ```

2. **Config file layout:**
   - Place your config files in `src/main/resources/META-INF/native-image/{groupId}/{artifactId}`
3. **native-image.properties:**
   - Allows specifying initialization timing and other native-image options.
   - Example key arguments:
     - `--enable-https` / `--enable-http`
     - `--initialize-at-run-time=<classname>`
     - `--no-fallback`
     - `-Ob` for quick build

## Metrics and Validation

- Use API endpoints such as `GET /metrics` and `GET /metrics/{metric name}` to inspect built-in and custom metrics, like `books.index`, `books.find`, `microserviceBooksNumber.*`, JVM stats, CPU usage, etc.
- Example metric payload:

  ```json
  {
    "name": "books.index",
    "measurements": [
      { "statistic": "COUNT", "value": 6 },
      { "statistic": "TOTAL_TIME", "value": 3.08425 },
      { "statistic": "MAX", "value": 3.02097 }
    ],
    "baseUnit": "seconds"
  }
  ```

- Verify metrics appear in Azure Monitor / Application Insights by matching e.g., `http.server.requests` to API call counts.

## Additional Resources

- [Micronaut Micrometer for Azure Monitor](https://micronaut-projects.github.io/micronaut-micrometer/5.12.0/guide/)
- [Native Image Build Configuration](https://www.graalvm.org/latest/reference-manual/native-image/overview/BuildConfiguration/#embed-a-configuration-file)
- [GraalVM Native Image Maven Plugin Docs](https://graalvm.github.io/native-build-tools/latest/maven-plugin.html#native-image-options)

## Summary

This walkthrough provides in-depth, practical steps for reporting metrics from a GraalVM native image Micronaut app to Azure Monitor—covering build, configuration, troubleshooting, and verification.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-metrics-from-micronaut-native-image-applications-to-azure/ba-p/4443763)
