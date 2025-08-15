---
layout: "post"
title: "Sending Logs, Metrics, and Traces from Micronaut Native Image Apps to Azure Monitor"
description: "This article explores how developers can send logs, metrics, and traces from Micronaut applications—specifically those compiled as GraalVM Native Images—to Azure Monitor, including Application Insights and Log Analytics Workspace. It compares available integration options, details recent advances in Micronaut 4.9.0 with the new azure-tracing dependency, and provides guidance for telemetry with Azure Container Apps. Key Microsoft documentation and related community resources are referenced."
author: "Logico_jp"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-signals-from-micronaut-native-image-applications-to-azure/ba-p/4443735"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-08-15 04:54:15 +00:00
permalink: "/2025-08-15-Sending-Logs-Metrics-and-Traces-from-Micronaut-Native-Image-Apps-to-Azure-Monitor.html"
categories: ["Azure", "DevOps"]
tags: ["Application Insights", "Azure", "Azure Container Apps", "Azure Monitor", "Azure Monitor Agent", "Azure SDK For Java", "Community", "DevOps", "GraalVM", "Instrumentation", "Java", "Java Agents", "Log Analytics Workspace", "Logging", "Metrics", "Micronaut", "Micronaut Azure Tracing", "Microservices", "Native Image", "OpenTelemetry", "Quarkus", "Spring Boot", "Telemetry", "Tracing"]
tags_normalized: ["application insights", "azure", "azure container apps", "azure monitor", "azure monitor agent", "azure sdk for java", "community", "devops", "graalvm", "instrumentation", "java", "java agents", "log analytics workspace", "logging", "metrics", "micronaut", "micronaut azure tracing", "microservices", "native image", "opentelemetry", "quarkus", "spring boot", "telemetry", "tracing"]
---

Logico_jp shares practical guidance on integrating Micronaut native image applications with Azure Monitor for end-to-end observability, covering logging, metrics, and traces on Azure.<!--excerpt_end-->

# Sending Logs, Metrics, and Traces from Micronaut Native Image Apps to Azure Monitor

**Author**: Logico_jp  
**Date**: July 20, 2025 (original in Japanese; this summary in English)

## Customer Query

A customer using Micronaut to develop microservices (hosted on Azure Container Apps) asked how to:

- Send logs, metrics, and especially traces from Micronaut apps to Azure Monitor
- Achieve this while compiling to GraalVM Native Image
- Determine if Micronaut, like Spring Boot and Quarkus, provides streamlined dependencies for this integration

## Available Methods for Sending Telemetry to Azure Monitor

Developers have several integration methods:

1. **Java Agent-Based Approach**
   - Java agents can be attached, but as of July 2025, native images generated via GraalVM do not support Java agents. This approach is not compatible with native images.

2. **FAT JAR with Micronaut Dependencies**
   - You can package required dependencies for metrics and logging, such as:
     - `io.micronaut.micrometer:micronaut-micrometer-registry-azure-monitor` (metrics)
     - `io.micronaut.azure:micronaut-azure-logging` (logging)
   - However, until recently, there was no supported dependency for tracing.

3. **Azure Monitor Agent with OpenTelemetry Protocol (OTLP)**
   - By enabling Azure Monitor Agent in Azure Container Apps and configuring it to listen for OpenTelemetry protocol (OTLP) data, apps of any framework can transmit logs and traces. *Note*: As per Azure docs, metrics are currently not supported through this OTLP ingestion path.
   - See: [Collect and read OpenTelemetry data in Azure Container Apps](https://learn.microsoft.com/azure/container-apps/opentelemetry-agents)

## New Development: Native Tracing Support in Micronaut 4.9.0

With Micronaut 4.9.0, a new dependency—[`io.micronaut.azure:micronaut-azure-tracing`](https://learn.microsoft.com/java/api/overview/azure/monitor-opentelemetry-autoconfigure-readme?view=azure-java-stable)—provides a way for Micronaut applications to send trace data directly to Azure Application Insights. This leverages Azure Monitor's OpenTelemetry SDK "Autoconfigure Distro," which is compatible with GraalVM Native Image, removing a key limitation for those building native binaries.

- No need to avoid native images to get trace functionality
- Reduce cold start and memory overhead compared to agent-based approaches

## Summary Table: Integration Options

| Method              | Supported Telemetry | Native Image Support | Notes                                                                   |
|---------------------|--------------------|---------------------|-------------------------------------------------------------------------|
| Java Agent          | Logs, Metrics, Traces | No                  | Not compatible with GraalVM native images (as of 07/2025)               |
| FAT JAR (Micronaut) | Logs, Metrics      | Yes                 | Now with Micronaut 4.9.0, tracing possible with micronaut-azure-tracing  |
| OTLP (Agentless)    | Logs, Traces       | Yes                 | Use Azure Monitor Agent for Azure Container Apps (no metrics yet)        |

## Best Practices and Key Considerations

- **Choose the right dependency for your needs:**
  - Logging: `micronaut-azure-logging`
  - Metrics: `micronaut-micrometer-registry-azure-monitor`
  - Tracing: `micronaut-azure-tracing` *(Micronaut 4.9.0+)*
- When deploying in Azure Container Apps, consider configuring the Azure Monitor Agent for collecting OpenTelemetry logs/traces, but note its limitation regarding metrics.
- Always review the latest Azure and Micronaut documentation for updates.

## Reference Links and Further Reading

- [Micronaut official guide (Japanese)](https://logico-jp.dev/2025/07/20/send-metrics-logs-and-traces-from-micronaut-applications-to-azure-monitor/)
- [Azure Monitor OpenTelemetry SDK Autoconfigure Distro for Application Insights (Java)](https://learn.microsoft.com/java/api/overview/azure/monitor-opentelemetry-autoconfigure-readme?view=azure-java-stable)
- [Micronaut Azure integrations](https://micronaut-projects.github.io/micronaut-azure/latest/guide/)
- [Collect and read OpenTelemetry data in Azure Container Apps](https://learn.microsoft.com/azure/container-apps/opentelemetry-agents)
- [Send traces from Micronaut native image applications to Azure Monitor](https://techcommunity.microsoft.com/blog/appsonazureblog/send-traces-from-micronaut-native-image-applications-to-azure-monitor/4443791)
- [Compare: Spring Boot and Quarkus monitoring integrations](https://opentelemetry.io/docs/zero-code/java/spring-boot-starter/), [Quarkus Opentelemetry Exporter for Microsoft Azure](https://docs.quarkiverse.io/quarkus-opentelemetry-exporter/dev/quarkus-opentelemetry-exporter-azure.html)

## Author Profile

Logico_jp is a longstanding contributor in the Microsoft Azure and Java communities, offering deep-dive guidance on cloud application observability and best practices for telemetry.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-signals-from-micronaut-native-image-applications-to-azure/ba-p/4443735)
