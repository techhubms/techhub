---
layout: "post"
title: "Send Traces from Micronaut Native Image Applications to Azure Monitor"
description: "This comprehensive guide by Logico_jp explains how to instrument Micronaut applications compiled to native images for sending distributed trace data to Azure Monitor (Application Insights). It covers prerequisites, project setup using Maven and Micronaut Launch, required dependencies, configuration details, version conflict mitigation, native image build processes, and practical testing steps to verify trace delivery and analysis within Azure Monitor's Application Map."
author: "Logico_jp"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-traces-from-micronaut-native-image-applications-to-azure/ba-p/4443791"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-08-15 04:56:31 +00:00
permalink: "/2025-08-15-Send-Traces-from-Micronaut-Native-Image-Applications-to-Azure-Monitor.html"
categories: ["Azure", "DevOps"]
tags: ["Application Insights", "Application Map", "Azure", "Azure Monitor", "Azure Monitor Opentelemetry Autoconfigure", "Azure Tracing", "Community", "Dependency Conflicts", "DevOps", "GraalVM", "HTTP Client", "Instrumentation", "Java", "Maven", "Micronaut", "Micronaut Azure Tracing", "Micronaut Native Image", "Micronaut Tracing Opentelemetry HTTP", "Native Maven Plugin", "Observability", "OpenTelemetry", "Reachability Metadata", "Tracing"]
tags_normalized: ["application insights", "application map", "azure", "azure monitor", "azure monitor opentelemetry autoconfigure", "azure tracing", "community", "dependency conflicts", "devops", "graalvm", "http client", "instrumentation", "java", "maven", "micronaut", "micronaut azure tracing", "micronaut native image", "micronaut tracing opentelemetry http", "native maven plugin", "observability", "opentelemetry", "reachability metadata", "tracing"]
---

Logico_jp explains how to send distributed traces from Micronaut native image Java applications to Azure Monitor, covering dependencies, build configuration, and trace verification, including practical troubleshooting tips.<!--excerpt_end-->

# Send Traces from Micronaut Native Image Applications to Azure Monitor

Author: Logico_jp

## Overview

This guide walks through instrumenting Micronaut applications (compiled as GraalVM native images) so that they can export distributed trace data to Azure Monitor (Application Insights). It includes prerequisites, dependency setup, configuration, build, and verification using both Java and native image artifacts.

## Prerequisites

- **Maven:** 3.9.10
- **JDK:** version 21
- **Micronaut:** 4.9.0 or later

Reference guide:

- [OpenTelemetry Tracing with Oracle Cloud and the Micronaut Framework](https://guides.micronaut.io/latest/micronaut-cloud-trace-oci-maven-java.html)
- [Create and Trace a Micronaut Application Using Azure Monitor](https://graal.cloud/gdk/gdk-modules/tracing/micronaut-trace-azure/?buildTool=maven&system=linux)

## Project Setup

You can use Micronaut CLI or [Micronaut Launch](https://micronaut.io/launch/). For YAML configuration, add the "yaml" feature.

Example CLI:

```bash
mn create-app \
  --build=maven \
  --jdk=21 \
  --lang=java \
  --test=junit \
  --features=tracing-opentelemetry-http,validation,graalvm,azure-tracing,http-client,yaml \
  dev.logicojp.micronaut.azuremonitor-metric
```

Micronaut Launch: Select features -- tracing-opentelemetry-http, validation, graalvm, azure-tracing, http-client, yaml.

## Dependencies

Core for tracing to Azure Monitor:

- `io.micronaut.azure:micronaut-azure-tracing`
- `com.azure:azure-monitor-opentelemetry-autoconfigure`
- `io.micronaut:micronaut-inject`
- `io.micronaut.tracing:micronaut-tracing-opentelemetry`
- `io.micronaut.tracing:micronaut-tracing-opentelemetry-http`

Add GraalVM reachability metadata support:

```xml
<dependency>
  <groupId>org.graalvm.buildtools</groupId>
  <artifactId>graalvm-reachability-metadata</artifactId>
  <version>0.11.0</version>
</dependency>
```

GraalVM Maven plugin:

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

## Configuration

Set the Application Insights connection string (different from metric settings):

- In `application.yml`:  

  ```yaml
  azure:
    tracing:
      connection-string: InstrumentationKey=...
  ```

- Or as an environment variable:
  `azure.tracing.connection-string="InstrumentationKey=..."`

## Handling Dependency Conflicts

Micronaut lets you substitute dependencies (e.g., use Undertow instead of Netty, JSON-P/JSON-B instead of Jackson) to resolve conflicts commonly encountered with Azure SDK and GraalVM native image compilation.

## Build and Test (Java Application)

- Build: `mvn clean package`
- Start app and verify traces are sent to Azure Monitor (Application Insights)

Generate config for native image:

```bash
# Collect reflect-config, etc.

$JAVA_HOME/bin/java \ \
  -agentlib:native-image-agent=config-output-dir=src/main/resources/META-INF/{groupId}/{artifactId}/ \
  -jar ./target/{artifactId}-{version}.jar

# Generate trace file

$JAVA_HOME/bin/java \ \
  -agentlib:native-image-agent=trace-output=/path/to/trace-file.json \
  -jar ./target/{artifactId}-{version}.jar

# Generate reachability metadata

native-image-configure generate \
  --trace-input=/path/to/trace-file.json \
  --output-dir=/path/to/config-dir/
```

See [Configure Native Image with the Tracing Agent](https://www.graalvm.org/latest/reference-manual/native-image/guides/configure-with-tracing-agent/)

Generated files go in `src/main/resources/META-INF/native-image/{groupId}/{artifactId}`.

## Native Image Build

- Build native image: `mvn package -Dpackaging=native-image`
- For faster local development, enable Quick Build and use `-Ob` optimization
  ([Maven plugin docs](https://graalvm.github.io/native-build-tools/latest/maven-plugin.html#native-image-options))

## Running and Verifying

- Deploy the application, hit endpoints (e.g., inventory check with curl), and verify responses:

  ```bash
  curl https://<app-url>/store/inventory/desktop
  # Response: {"warehouse":7,"item":"desktop","store":2}
  ```

- Place an order:

  ```bash
  curl -X POST "https://<app-url>/store/order" \
    -H 'Content-Type: application/json; charset=utf-8' \
    -d '{"item":"desktop", "count":5}'
  curl https://<app-url>/store/inventory/desktop
  ```

- View traces and application map in Azure Monitor to confirm correct telemetry.

## Troubleshooting

- Use provided configuration/metadata best practices for GraalVM
- Watch for dependency conflicts (especially Netty, Jackson)
- For more metrics guidance, refer to [Send metrics from Micronaut native image applications to Azure Monitor](https://techcommunity.microsoft.com/blog/appsonazureblog/send-metrics-from-micronaut-native-image-applications-to-azure-monitor/4443763)

## References

- [MicronautからAzure Monitorにtraceを送信したい – Logico Inside](https://logico-jp.dev/2025/07/24/send-traces-from-micronaut-applications-to-azure-monitor/)
- [Send signals from Micronaut native image applications to Azure Monitor | Microsoft Community Hub](https://techcommunity.microsoft.com/blog/appsonazureblog/send-signals-from-micronaut-native-image-applications-to-azure-monitor/4443735)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-traces-from-micronaut-native-image-applications-to-azure/ba-p/4443791)
