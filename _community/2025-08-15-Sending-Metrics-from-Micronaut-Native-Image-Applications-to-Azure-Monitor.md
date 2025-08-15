---
layout: "post"
title: "Sending Metrics from Micronaut Native Image Applications to Azure Monitor"
description: "This tutorial, authored by Logico_jp, demonstrates how to configure a Micronaut Java application (built with GraalVM Native Image) to collect and export custom and built-in metrics to Azure Monitor (Application Insights) using Micrometer Azure Monitor integration. It covers Maven build configuration, environment setup, instrumentation key management, application settings, and validation in Azure."
author: "Logico_jp"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-metrics-from-micronaut-native-image-applications-to-azure/ba-p/4443763"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-15 02:31:07 +00:00
permalink: "/2025-08-15-Sending-Metrics-from-Micronaut-Native-Image-Applications-to-Azure-Monitor.html"
categories: ["Azure", "DevOps"]
tags: ["Application Configuration", "Application Insights", "Azure", "Azure Monitor", "CI/CD", "Community", "Custom Metrics", "DevOps", "GraalVM", "Instrumentation Key", "Java", "JDK 21", "Maven", "Metadata Repository", "Metrics", "Micrometer", "Micrometer Azure Monitor", "Micronaut", "Native Image", "Native Image.properties", "Pom.xml", "REST API"]
tags_normalized: ["application configuration", "application insights", "azure", "azure monitor", "ci slash cd", "community", "custom metrics", "devops", "graalvm", "instrumentation key", "java", "jdk 21", "maven", "metadata repository", "metrics", "micrometer", "micrometer azure monitor", "micronaut", "native image", "native image dot properties", "pom dot xml", "rest api"]
---

Logico_jp guides readers through setting up a Micronaut native image Java application to send custom and built-in metrics to Azure Monitor, covering prerequisites, configuration, native image aspects, and validation steps.<!--excerpt_end-->

# Sending Metrics from Micronaut Native Image Applications to Azure Monitor

**Author:** Logico_jp  
**Date:** July 20, 2025

This guide details how to configure a Micronaut application (built as a native image with GraalVM) for monitoring and metric export to Azure Monitor (Application Insights) using Micrometer.

## Prerequisites

- Maven 3.9.10
- JDK 21
- Micronaut 4.9.0+

## References

- [Create a Micronaut Application to Collect Metrics and Monitor Them on Azure Monitor Metrics](https://graal.cloud/gdk/gdk-modules/metrics/micronaut-metrics-azure/)
- [Micronaut Metrics Guide](https://guides.micronaut.io/latest/micronaut-metrics-maven-java.html)

## Project Setup

### Generating the Project

Use Micronaut CLI or Micronaut Launch:

```sh
mn create-app \
  --build=maven \
  --jdk=21 \
  --lang=java \
  --test=junit \
  --features=validation,graalvm,micrometer-azure-monitor,http-client,micrometer-annotation,yaml \
  dev.logicojp.micronaut.azuremonitor-metric
```

*Features used:* validation, graalvm, micrometer-azure-monitor, http-client, micrometer-annotation, yaml.

### Download & Unpack

- Use [Micronaut Launch](https://micronaut.io/launch/) to select these features and download the archetype as a Zip file.

## Implementation Notes

### Directory Structure

Standard Micronaut archetypes combine `azure` and `lib` folders from GraalVM guides into one structure.

### Instrumentation Key

Update your `application.properties` or `application.yml` to include the Azure Monitor instrumentation key or connection string as follows:

#### application.properties Example

```
micronaut.metrics.export.azuremonitor.connectionString="InstrumentationKey=..."
```

#### application.yml Equivalent

```yaml
micronaut:
  metrics:
    enabled: true
    export:
      azuremonitor:
        enabled: true
        connectionString: InstrumentationKey=...
```

#### Environment Variables

- Use `AZURE_MONITOR_CONNECTION_STRING` or `MICRONAUT_METRICS_EXPORT_AZUREMONITOR_CONNECTIONSTRING`

#### Example using env variable in YAML

```yaml
micronaut:
  metrics:
    enabled: true
    export:
      azuremonitor:
        enabled: true
        connectionString: ${AZURE_MONITOR_CONNECTION_STRING}
```

Micrometer Azure Monitor configuration details: [Micronaut Micrometer documentation](https://micronaut-projects.github.io/micronaut-micrometer/latest/guide/#_meter_binder).

### Maven Build (pom.xml)

Add dependencies:

```xml
<dependency>
  <groupId>org.graalvm.buildtools</groupId>
  <artifactId>graalvm-reachability-metadata</artifactId>
  <version>0.11.0</version>
</dependency>
```

Add the GraalVM Maven plugin for native-image builds, setting optimization preferences with `-Ob` for quick builds.

## Native Image Configuration

- Generate config files (`jni-config.json`, `reflect-config.json`, etc.) with the Tracing Agent:

```sh
$JAVA_HOME/bin/java \
  -agentlib:native-image-agent=config-output-dir=src/main/resources/META-INF/{groupId}/{artifactId}/ \
  -jar ./target/{artifactId}-{version}.jar

# Optional: Create a trace file and generate metadata

$JAVA_HOME/bin/java \
  -agentlib:native-image-agent=trace-output=/path/to/trace-file.json \
  -jar ./target/{artifactId}-{version}.jar
native-image-configure generate \
  --trace-input=/path/to/trace-file.json \
  --output-dir=/path/to/config-dir/
```

- Place config files under `src/main/resources/META-INF/native-image/{groupId}/{artifactId}`
- Use `native-image.properties` to externalize options (initialization timing, build options)
- Enable HTTP(S) protocol support with `--enable-http` and `--enable-https` as needed
- Prevent fallback builds with `--no-fallback`

See these guides for configuration details:

- [Configure Native Image with the Tracing Agent](https://www.graalvm.org/latest/reference-manual/native-image/guides/configure-with-tracing-agent/)
- [Native Image Build Configuration](https://www.graalvm.org/latest/reference-manual/native-image/overview/BuildConfiguration/#embed-a-configuration-file)

## Metrics Testing

1. **Java Application:**
    - Build: `mvn clean package`
    - Run and verify metrics are exported to Application Insights

2. **API Endpoints:**
    - Test REST: `GET /books` and `GET /books/{isbn}`
    - List metrics at `GET /metrics`

Custom and default metrics, such as:

- `microserviceBooksNumber.checks`
- `microserviceBooksNumber.time`
- `microserviceBooksNumber.latest`
- `books.find`
- `books.index`
- JVM and HTTP server metrics

1. **Validate in Azure Monitor:**
    - Confirm custom metrics appear in Application Insights
    - Compare values such as `http.server.requests` with API usage

## Optimization Tips

- Use quick build (`-Ob`) during development to accelerate native compilation
- Refer to [Optimizations and Performance](https://www.graalvm.org/latest/reference-manual/native-image/optimizations-and-performance/#optimization-levels)
- Troubleshoot startup and GC messages as described

## Summary

By following these steps, you can build a performant Micronaut native image Java application that exports detailed metrics to Azure Monitor for visibility and operations. This includes full Maven and GraalVM integration, Azure configuration, and both built-in and custom application metrics.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-metrics-from-micronaut-native-image-applications-to-azure/ba-p/4443763)
