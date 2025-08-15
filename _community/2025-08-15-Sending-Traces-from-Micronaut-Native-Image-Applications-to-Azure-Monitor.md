---
layout: "post"
title: "Sending Traces from Micronaut Native Image Applications to Azure Monitor"
description: "This guide provides a detailed walkthrough for Java developers on configuring Micronaut applications (including native images) to send trace data to Azure Monitor’s Application Insights. It covers prerequisites, project setup with Maven and the Micronaut CLI, dependency management for Azure tracing, handling GraalVM-specific requirements, and techniques for validating trace transmission in an Azure cloud context."
author: "Logico_jp"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-traces-from-micronaut-native-image-applications-to-azure/ba-p/4443791"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-15 03:06:30 +00:00
permalink: "/2025-08-15-Sending-Traces-from-Micronaut-Native-Image-Applications-to-Azure-Monitor.html"
categories: ["Azure", "Coding"]
tags: ["Application Insights", "Application Map", "Application Monitoring", "Azure", "Azure Monitor", "Azure Monitor Opentelemetry Autoconfigure", "Cloud Observability", "Coding", "Community", "Dependency Management", "GraalVM", "Java", "Maven", "Micronaut", "Micronaut Azure Tracing", "Native Image", "OpenTelemetry", "Tracing"]
tags_normalized: ["application insights", "application map", "application monitoring", "azure", "azure monitor", "azure monitor opentelemetry autoconfigure", "cloud observability", "coding", "community", "dependency management", "graalvm", "java", "maven", "micronaut", "micronaut azure tracing", "native image", "opentelemetry", "tracing"]
---

Logico_jp presents a practical tutorial on integrating Micronaut (Java) applications—including native images—with Azure Monitor, explaining every step needed to send traces to Application Insights.<!--excerpt_end-->

# Sending Traces from Micronaut Native Image Applications to Azure Monitor

Author: Logico_jp

## Overview

This post demonstrates how to enable the collection and transmission of trace telemetry from Java Micronaut applications (including native images) to Azure Monitor's Application Insights, using current tools and Microsoft’s recommended approach.

## Prerequisites

- Maven 3.9.10 or newer
- JDK 21
- Micronaut 4.9.0+
- For native image builds: GraalVM, Graal Dev Kit (GDK)

### Reference Links

- [Original Japanese article](https://logico-jp.dev/2025/07/24/send-traces-from-micronaut-applications-to-azure-monitor/)
- [Micronaut to Azure Monitor (English)](https://techcommunity.microsoft.com/blog/appsonazureblog/send-metricstraceslogs-from-micronaut-native-image-applications-to-azure-monitor/4443735)

## Project Setup

You can generate a project using either the Micronaut CLI or [Micronaut Launch](https://micronaut.io/launch/):

### With CLI

```
mn create-app \
  --build=maven \
  --jdk=21 \
  --lang=java \
  --test=junit \
  --features=tracing-opentelemetry-http,validation,graalvm,azure-tracing,http-client,yaml \
  dev.logicojp.micronaut.azuremonitor-metric
```

**Note:** Select features: `tracing-opentelemetry-http`, `validation`, `graalvm`, `azure-tracing`, `http-client`, and `yaml`.

Configuration can use `application.yml` instead of `.properties` for better structure.

## Dependency Management

Add these dependencies in your `pom.xml`:

```xml
<dependency>
  <groupId>io.micronaut.tracing</groupId>
  <artifactId>micronaut-tracing-opentelemetry-http</artifactId>
  <scope>compile</scope>
</dependency>
<dependency>
  <groupId>io.micronaut.azure</groupId>
  <artifactId>micronaut-azure-tracing</artifactId>
</dependency>
<dependency>
  <groupId>com.azure</groupId>
  <artifactId>azure-monitor-opentelemetry-autoconfigure</artifactId>
</dependency>
<dependency>
  <groupId>org.graalvm.buildtools</groupId>
  <artifactId>graalvm-reachability-metadata</artifactId>
  <version>0.11.0</version>
</dependency>
```

### Plugin for Native Images

For GraalVM native image support, add:

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

## Application Configuration

Specify the Application Insights connection string in `application.yml`:

```yaml
azure:
  tracing:
    connection-string: InstrumentationKey=...
```

Or via environment variable:

```
azure.tracing.connection-string="InstrumentationKey=..."
```

## Version Conflicts

- To avoid Netty or Jackson version conflicts when building native images, consider switching to alternatives such as Undertow or JSON-P/BSON.

## Building and Testing

- **Standard JAR**
  - `mvn clean package`
- **Native Image**
  - `mvn package -Dpackaging=native-image`
  - Use GraalVM tracing agent to generate configuration files like `reflect-config.json`, `resource-config.json`, etc.

## Validating Trace Transmission

- Run your application and confirm traces appear in Azure Monitor > Application Insights > Application Map.
- Use `curl` to exercise endpoints and observe changes in the map and trace explorer.

## Key Points

- Always match dependencies and plugins as demonstrated to ensure seamless tracing.
- For non-standard configurations or troubleshooting, validate settings between `micronaut-azure-tracing` and `micrometer-azure-monitor`.
- Native images require additional metadata and configuration steps, especially for reflection and dependency management.

## Further Reading

- [OpenTelemetry Tracing with Micronaut](https://guides.micronaut.io/latest/micronaut-cloud-trace-oci-maven-java.html)
- [GraalVM Native Image Manual](https://www.graalvm.org/latest/reference-manual/native-image/)
- [Configure Native Image with Tracing Agent](https://www.graalvm.org/latest/reference-manual/native-image/guides/configure-with-tracing-agent/)

---

For detailed explanations or troubleshooting on metrics, see the linked articles in the overview.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-traces-from-micronaut-native-image-applications-to-azure/ba-p/4443791)
