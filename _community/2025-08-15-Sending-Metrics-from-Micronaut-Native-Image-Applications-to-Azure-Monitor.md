---
layout: "post"
title: "Sending Metrics from Micronaut Native Image Applications to Azure Monitor"
description: "This in-depth guide explains how to instrument Micronaut applications, including those compiled to native images with GraalVM, to send metrics to Azure Monitor (Application Insights). It covers prerequisites, project setup with Maven and Micronaut Launch, detailed configuration steps, environment variables, custom metric collection, Maven and GraalVM plugin usage, and verification methods for both Java and native-image runtimes. Best practices for native image configuration, initialization, and troubleshooting are included, with references to related documentation and Microsoft Community Hub background."
author: "Logico_jp"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-metrics-from-micronaut-native-image-applications-to-azure/ba-p/4443763"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-15 02:31:07 +00:00
permalink: "/2025-08-15-Sending-Metrics-from-Micronaut-Native-Image-Applications-to-Azure-Monitor.html"
categories: ["Azure", "Coding"]
tags: ["Application Insights", "Application Telemetry", "Azure", "Azure Monitor", "Cloud Integration", "Cloud Monitoring", "Coding", "Community", "Custom Metrics", "GraalVM", "Instrumentation Key", "Java", "JDK 21", "Maven", "Metrics", "Micrometer", "Micronaut", "Native Image", "REST API"]
tags_normalized: ["application insights", "application telemetry", "azure", "azure monitor", "cloud integration", "cloud monitoring", "coding", "community", "custom metrics", "graalvm", "instrumentation key", "java", "jdk 21", "maven", "metrics", "micrometer", "micronaut", "native image", "rest api"]
---

Logico_jp walks through sending metrics from a Micronaut native image application to Azure Monitor, detailing project setup, metric configuration, GraalVM native image specifics, and how to validate telemetry reaches Application Insights.<!--excerpt_end-->

# Sending Metrics from Micronaut Native Image Applications to Azure Monitor

**Author:** Logico_jp  
**Original Date:** July 20, 2025  

This guide demonstrates how to send custom and built-in metrics from a Micronaut application (including native images built with GraalVM) to Azure Monitor (Application Insights). It’s based on referenced tutorials, community blog posts, and the latest best practices for JVM and native-image monitoring.

## Prerequisites

- **Maven:** 3.9.10
- **JDK:** 21
- **Micronaut:** 4.9.0 or later

References:

- [Micronaut Metrics & Azure Monitor Tutorial](https://graal.cloud/gdk/gdk-modules/metrics/micronaut-metrics-azure/)
- [Micronaut Metrics Guide](https://guides.micronaut.io/latest/micronaut-metrics-maven-java.html)

## Project Setup

You can create your project either via Micronaut CLI or [Micronaut Launch](https://micronaut.io/launch/). Use `application.yml` for configuration by including the `yaml` feature. Example CLI:

```bash
mn create-app \
  --build=maven \
  --jdk=21 \
  --lang=java \
  --test=junit \
  --features=validation,graalvm,micrometer-azure-monitor,http-client,micrometer-annotation,yaml \
  dev.logicojp.micronaut.azuremonitor-metric
```

### Required Features

- validation
- graalvm
- micrometer-azure-monitor
- http-client
- micrometer-annotation
- yaml

Download the generated archetype as a ZIP from Launch and extract.

## Implementation Steps

### 1. Directory Structure

Unlike GDK examples, combine `azure` and `lib` code into the standard Micronaut structure. No custom subfolders needed.

### 2. Instrumentation Key/Connection String

Update `application.properties` or `application.yml` to set the Azure Monitor connection string. Using connection strings instead of only the Instrumentation Key is currently recommended.

Example (`application.yml`):

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

You can also set this as an environment variable for easier management in CI/CD pipelines:

- `AZURE_MONITOR_CONNECTION_STRING`

Micrometer (used by Micronaut) already supports this configuration, as seen in [AzureMonitorConfig.java](https://github.com/micrometer-metrics/micrometer/blob/main/implementations/micrometer-registry-azure-monitor/src/main/java/io/micrometer/azuremonitor/AzureMonitorConfig.java).

### 3. Maven Configuration for Native Image

Add dependencies for the [GraalVM Reachability Metadata Repository](https://search.maven.org/artifact/org.graalvm.buildtools/graalvm-reachability-metadata).

```xml
<dependency>
  <groupId>org.graalvm.buildtools</groupId>
  <artifactId>graalvm-reachability-metadata</artifactId>
  <version>0.11.0</version>
</dependency>
```

Configure the Maven native-image plugin:

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

Run a clean build via:

```bash
mvn clean package
```

## Validating Java Application

Ensure the app starts and sends metrics to Application Insights. Use the Tracing Agent to collect native image metadata:

```bash
# Collect config files

$JAVA_HOME/bin/java -agentlib:native-image-agent=config-output-dir=src/main/resources/META-INF/{groupId}/{artifactId}/ -jar ./target/{artifactId}-{version}.jar
```

See GraalVM docs for [Automatic Metadata Collection](https://www.graalvm.org/latest/reference-manual/native-image/metadata/AutomaticMetadataCollection/).

Resulting files:

- `jni-config.json`, `reflect-config.json`, `proxy-config.json`, `resource-config.json`, `reachability-metadata.json`

Store these under `src/main/resources/META-INF/native-image/{groupId}/{artifactId}` for the native-image tool to detect.

## Native Image Build and Configuration

- Build with:

```bash
mvn package -Dpackaging=native-image
```

- Use `native-image.properties` for advanced flags (property file location, HTTP(S) enablement):
  - `--enable-http`, `--enable-https`
  - `--initialize-at-run-time=...` or `--initialize-at-build-time=...`
  - `--no-fallback` to prevent JVM fallback images
  - Additional build flags as needed (see [GraalVM Options](https://www.graalvm.org/latest/reference-manual/native-image/overview/Options/#build-options))

Read more in [Native Image Build Configuration](https://www.graalvm.org/latest/reference-manual/native-image/overview/BuildConfiguration/#embed-a-configuration-file).

## Testing the Application

- Start as Java for initial validation.
- Once native image is built, test core endpoints:
  - `GET /books` and `GET /books/{isbn}`: Sample REST APIs
  - `GET /metrics`: Lists all available metrics

### Example Metrics

- Custom: `microserviceBooksNumber.checks`, `microserviceBooksNumber.time`, `microserviceBooksNumber.latest`
- Controller: `books.find`, `books.index`

Example response (snippet):

```json
{
  "names": ["books.find", "books.index", ... ,"system.load.average.1m"]
}
```

Metric result (e.g. `books.index`):

```json
{
  "name": "books.index",
  "measurements": [
    {"statistic": "COUNT", "value": 6},
    {"statistic": "TOTAL_TIME", "value": 3.08425},
    {"statistic": "MAX", "value": 3.02097}
  ],
  "availableTags": [{"tag": "exception", "values": ["none"]}],
  "baseUnit": "seconds"
}
```

## Verifying Metrics in Azure Monitor

After running the app, open Application Insights to view custom metrics (e.g., `microserviceBooksNumber.*`, `books.*`) and ensure the telemetry matches.

## Additional Best Practices

- Place native image metadata in subfolders by `groupId`/`artifactId`.
- Use environment variables for secrets/configuration in CI/CD.
- Regularly review GraalVM and Micronaut documentation for updates in native-image compatibility and plugin options.

## Resources

- [Micronaut Documentation](https://micronaut.io/documentation/)
- [Micrometer Azure Monitor Integration](https://micronaut-projects.github.io/micronaut-micrometer/5.12.0/guide/#metricsAndReportersAzureMonitor)
- [GraalVM Native Image Docs](https://www.graalvm.org/latest/reference-manual/native-image/)
- [Azure Monitor](https://learn.microsoft.com/azure/azure-monitor/)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-metrics-from-micronaut-native-image-applications-to-azure/ba-p/4443763)
