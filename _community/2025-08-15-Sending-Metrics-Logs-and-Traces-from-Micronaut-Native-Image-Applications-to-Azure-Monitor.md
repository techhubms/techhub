---
layout: "post"
title: "Sending Metrics, Logs, and Traces from Micronaut Native Image Applications to Azure Monitor"
description: "This post provides a detailed guide on how to send metrics, logs, and traces from Micronaut applications, including GraalVM Native Image builds, to Azure Monitor using the latest features in Micronaut 4.9.0. It covers suitable integration methods, dependency choices, Azure Container Apps specifics, and addresses compatibility with OpenTelemetry and Azure Monitor offerings."
author: "Logico_jp"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-signals-from-micronaut-native-image-applications-to-azure/ba-p/4443735"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-08-15 04:54:15 +00:00
permalink: "/2025-08-15-Sending-Metrics-Logs-and-Traces-from-Micronaut-Native-Image-Applications-to-Azure-Monitor.html"
categories: ["Azure"]
tags: ["Application Insights", "Azure", "Azure Container Apps", "Azure Monitor", "Cloud Native", "Community", "Distributed Tracing", "Fat JAR", "GraalVM Native Image", "Java", "Java Agent", "Log Analytics Workspace", "Logging", "Metrics", "Micronaut", "Micronaut Azure Tracing", "Monitoring", "Observability", "OpenTelemetry"]
tags_normalized: ["application insights", "azure", "azure container apps", "azure monitor", "cloud native", "community", "distributed tracing", "fat jar", "graalvm native image", "java", "java agent", "log analytics workspace", "logging", "metrics", "micronaut", "micronaut azure tracing", "monitoring", "observability", "opentelemetry"]
---

Logico_jp explains how to enable metrics, log, and trace telemetry for Micronaut applications, including GraalVM Native Images, on Azure Monitor. The article discusses integration options and the recently introduced micronaut-azure-tracing dependency.<!--excerpt_end-->

# Sending Metrics, Logs, and Traces from Micronaut Native Image Applications to Azure Monitor

*Author: Logico_jp*

## Overview

This guide addresses how to collect and send metrics, logs, and traces from Micronaut-based applications running on Azure, including support for GraalVM Native Images, leveraging the latest updates in the Micronaut ecosystem.

## Customer Query

A customer using Micronaut for microservices on Azure Container Apps requested:

- How to send traces (beyond logs/metrics) to Azure Monitor.
- Whether this works with GraalVM Native Image builds.

## Context: Existing Solutions in Java Frameworks

Java frameworks like Spring Boot and Quarkus offer built-in dependencies to facilitate Azure Monitor integration:

- [Spring Boot Starter for OpenTelemetry](https://opentelemetry.io/docs/zero-code/java/spring-boot-starter/)
- [Quarkus OpenTelemetry Exporter for Azure](https://docs.quarkiverse.io/quarkus-opentelemetry-exporter/dev/quarkus-opentelemetry-exporter-azure.html)
- [Monitoring Spring Boot Native Images on Azure](https://devblogs.microsoft.com/java/monitor-your-spring-boot-native-image-application-on-azure/)
- [Monitoring Quarkus Native Apps on Azure](https://devblogs.microsoft.com/java/monitor-your-quarkus-native-application-on-azure/)

## Options to Enable Telemetry in Micronaut on Azure

### 1. Java Agent

- Easiest to use for Java applications.
- Drawbacks: Increased startup time; not compatible with Native Images as of July 2025.

### 2. Fat JAR with Micronaut Dependencies

- Use dependencies like `io.micronaut.micrometer:micronaut-micrometer-registry-azure-monitor` and `io.micronaut.azure:micronaut-azure-logging` for metrics and logs.
- Previously, no direct dependency for traces.

### 3. Azure Monitor Agent with OpenTelemetry Protocol (OTLP)

- Enable Azure Monitor Agent in Azure Container Apps environment.
- Works with any application stack.
- **Limitation**: Cannot send metrics (per [Microsoft docs](https://learn.microsoft.com/azure/container-apps/opentelemetry-agents)).

## Recent Improvements — Trace Support for Micronaut Native Images

As of Micronaut 4.9.0, the new dependency `io.micronaut.azure:micronaut-azure-tracing` enables sending traces to Application Insights. This is powered by the Azure Monitor OpenTelemetry SDK Autoconfigure Distro, not by the Azure Monitor Agent.

- **Compatibility:** Works with GraalVM Native Images.
- **No need to abandon Native Images** for trace support.
- See [Azure Monitor OpenTelemetry SDK Autoconfigure Distro documentation](https://learn.microsoft.com/java/api/overview/azure/monitor-opentelemetry-autoconfigure-readme?view=azure-java-stable)

## Key Considerations for Sending Telemetry

- Use Micronaut dependencies for logs/metrics/traces:
  - Metrics: `micronaut-micrometer-registry-azure-monitor`
  - Logs: `micronaut-azure-logging`
  - Traces: `micronaut-azure-tracing` (since 4.9.0)
- For Azure Container Apps:
  - Agent-based OTLP is simple, but does not handle metrics.
  - Fat JAR with dependencies allows full coverage, including traces in Native Images.

## Reference Links

- [Official Guide: Send Metrics from Micronaut Native Apps to Azure Monitor](https://techcommunity.microsoft.com/blog/appsonazureblog/send-metrics-from-micronaut-native-image-applications-to-azure-monitor/4443763)
- [Send Traces from Micronaut Native Image Apps to Azure Monitor](https://techcommunity.microsoft.com/blog/appsonazureblog/send-traces-from-micronaut-native-image-applications-to-azure-monitor/4443791)
- [Send Logs from Micronaut Native Image Apps to Azure Monitor](https://techcommunity.microsoft.com/blog/appsonazureblog/send-logs-from-micronaut-native-image-applications-to-azure-monitor/4443867)
- [Send Signals via Zero-Code Instrumentation](https://techcommunity.microsoft.com/blog/appsonazureblog/send-signals-from-micronaut-applications-to-azure-monitor-through-zero-code-inst/4443884)

## Conclusion

Micronaut 4.9.0 introduces direct support for sending traces to Azure Monitor from both JAR and GraalVM Native Image applications using the `micronaut-azure-tracing` dependency. Combined with logging and metrics dependencies, this enables comprehensive observability for Micronaut applications on Azure.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-signals-from-micronaut-native-image-applications-to-azure/ba-p/4443735)
