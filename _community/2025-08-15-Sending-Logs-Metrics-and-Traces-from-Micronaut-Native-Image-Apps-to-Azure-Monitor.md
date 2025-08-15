---
layout: "post"
title: "Sending Logs, Metrics, and Traces from Micronaut Native Image Apps to Azure Monitor"
description: "This technical community post explores methods for integrating Micronaut-based microservices—specifically those compiled as GraalVM Native Images and orchestrated on Azure Container Apps—with Azure Monitor (including Application Insights and Log Analytics Workspace). It summarizes options to transmit logs, metrics, and traces, evaluates recent developments such as the new Micronaut azure-tracing dependency, and points to relevant guides and Microsoft documentation for Java observability scenarios."
author: "Logico_jp"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-signals-from-micronaut-native-image-applications-to-azure/ba-p/4443735"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-08-15 04:54:15 +00:00
permalink: "/2025-08-15-Sending-Logs-Metrics-and-Traces-from-Micronaut-Native-Image-Apps-to-Azure-Monitor.html"
categories: ["Azure", "DevOps"]
tags: ["Application Insights", "Azure", "Azure Container Apps", "Azure Monitor", "Azure Monitor Agent", "Community", "DevOps", "Fat JAR", "GraalVM Native Image", "Java", "Java Agents", "Log Analytics", "Logging", "Metrics", "Micronaut", "Micronaut Azure Tracing", "Observability", "OpenTelemetry", "SDK Autoconfigure Distro", "Tracing"]
tags_normalized: ["application insights", "azure", "azure container apps", "azure monitor", "azure monitor agent", "community", "devops", "fat jar", "graalvm native image", "java", "java agents", "log analytics", "logging", "metrics", "micronaut", "micronaut azure tracing", "observability", "opentelemetry", "sdk autoconfigure distro", "tracing"]
---

Logico_jp explains the updated best practices and tooling for sending logs, metrics, and traces from Micronaut (Java) applications—including GraalVM native images—to Azure Monitor, highlighting the new Micronaut Azure tracing integration.<!--excerpt_end-->

# Sending Logs, Metrics, and Traces from Micronaut Native Image Apps to Azure Monitor

Author: Logico_jp

## Context and Query

A customer building microservices with Micronaut, orchestrated via Azure Container Apps, inquired about the best approaches for:

- Sending *logs, metrics,* and *traces* from Micronaut apps to Azure Monitor (Application Insights, Log Analytics Workspace)
- Ensuring compatibility with *GraalVM Native Image* builds

Spring Boot and Quarkus offer native integrations for observability—does Micronaut have a similar offering?

**References:**

- [Spring Boot starter (OpenTelemetry)](https://opentelemetry.io/docs/zero-code/java/spring-boot-starter/)
- [Quarkus Opentelemetry Exporter for Microsoft Azure](https://docs.quarkiverse.io/quarkus-opentelemetry-exporter/dev/quarkus-opentelemetry-exporter-azure.html)

## Available Approaches

### 1. Use Java Agents

- Simple setup for standard JVM deployment (instrumentation for logs, metrics, traces)
- **Limitations:**
  - Can increase container/application startup time
  - **Not supported** when building Micronaut apps as GraalVM Native Images

### 2. FAT JAR with Bound Dependencies

- Use dependencies such as `io.micronaut.micrometer:micronaut-micrometer-registry-azure-monitor` and `io.micronaut.azure:micronaut-azure-logging` to emit metrics and logs
- **Limitations:**
  - Until recently, tracing support/status was missing

### 3. Azure Monitor Agent in Azure Container Apps (OpenTelemetry Protocol)

- Configure the Azure Container Apps environment to use Azure Monitor Agent and collect OpenTelemetry data
- **Advantage:** Works with any language/runtime
- **Limitation:** Metrics not supported as of July 2025 (per Microsoft documentation)

#### Documentation

- [Collect and read OpenTelemetry data in Azure Container Apps](https://learn.microsoft.com/azure/container-apps/opentelemetry-agents)

## Recent Change: Micronaut 4.9.0 and Azure Tracing

Starting with **Micronaut 4.9.0**, a new dependency (`io.micronaut.azure:micronaut-azure-tracing`) enables sending traces directly to Application Insights, with:

- Backing by [Azure Monitor OpenTelemetry SDK Autoconfigure Distro](https://learn.microsoft.com/java/api/overview/azure/monitor-opentelemetry-autoconfigure-readme?view=azure-java-stable)
- Compatibility with **Native Image builds** (does not require Java agent)
- No need to avoid GraalVM Native Images for full trace functionality

## Key Points for Logging, Metrics, and Tracing

- Logs: Use `micronaut-azure-logging` dependency
- Metrics: Use `micronaut-micrometer-registry-azure-monitor` for exporting via Micrometer
- Traces: As of Micronaut 4.9.0, use `micronaut-azure-tracing` (works with native images)

### Additional Resources

- [Send metrics from Micronaut native image applications to Azure Monitor | Microsoft Community Hub](https://techcommunity.microsoft.com/blog/appsonazureblog/send-metrics-from-micronaut-native-image-applications-to-azure-monitor/4443763)
- [Send traces from Micronaut native image applications to Azure Monitor | Microsoft Community Hub](https://techcommunity.microsoft.com/blog/appsonazureblog/send-traces-from-micronaut-native-image-applications-to-azure-monitor/4443791)
- [Send logs from Micronaut native image applications to Azure Monitor | Microsoft Community Hub](https://techcommunity.microsoft.com/blog/appsonazureblog/send-logs-from-micronaut-native-image-applications-to-azure-monitor/4443867)
- [Send signals from Micronaut applications to Azure Monitor through zero-code instrumentation](https://techcommunity.microsoft.com/blog/appsonazureblog/send-signals-from-micronaut-applications-to-azure-monitor-through-zero-code-inst/4443884)

## Summary Table of Options

| Approach | Native Image Support | Metrics | Logs | Traces | Notes |
|----------|---------------------|---------|------|--------|-------|
| Java Agent | No | Yes | Yes | Yes | Simple, slow startup |
| Fat JAR + dependencies | Yes (for logs/metrics) | Yes | Yes | Partial (now full with new tracing dep) | Use recent `micronaut-azure-tracing` for traces |
| Azure Monitor Agent (OTLP) | Yes | No | Yes | Yes | Metrics not yet supported |

## Conclusion

Micronaut users targeting Azure can now achieve full observability—including traces—in Native Image deployments by using the latest Azure tracing dependency. Each observability signal (logs, metrics, traces) may have a tailored integration and set of dependencies—be sure to review the linked documentation for implementation details.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-signals-from-micronaut-native-image-applications-to-azure/ba-p/4443735)
