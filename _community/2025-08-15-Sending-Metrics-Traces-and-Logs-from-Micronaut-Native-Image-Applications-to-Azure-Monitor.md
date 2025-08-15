---
layout: "post"
title: "Sending Metrics, Traces, and Logs from Micronaut Native Image Applications to Azure Monitor"
description: "This guide addresses how to send metrics, traces, and logs from Java Micronaut microservices—especially GraalVM Native Image builds—to Azure Monitor services like Application Insights and Log Analytics. It compares available methods, discusses the new Micronaut tracing library released in version 4.9.0, and provides links to relevant resources and documentation."
author: "Logico_jp"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-metrics-traces-logs-from-micronaut-native-image/ba-p/4443735"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-15 02:27:28 +00:00
permalink: "/2025-08-15-Sending-Metrics-Traces-and-Logs-from-Micronaut-Native-Image-Applications-to-Azure-Monitor.html"
categories: ["Azure", "DevOps"]
tags: ["Azure", "Azure Application Insights", "Azure Container Apps", "Azure Log Analytics", "Azure Monitor", "Azure Monitor Agent", "Azure SDK For Java", "Community", "DevOps", "GraalVM Native Image", "Java", "Logs", "Metrics", "Micrometer", "Micronaut", "Micronaut Azure Tracing", "Micronaut Micrometer Registry Azure Monitor", "Observability", "OpenTelemetry", "Traces"]
tags_normalized: ["azure", "azure application insights", "azure container apps", "azure log analytics", "azure monitor", "azure monitor agent", "azure sdk for java", "community", "devops", "graalvm native image", "java", "logs", "metrics", "micrometer", "micronaut", "micronaut azure tracing", "micronaut micrometer registry azure monitor", "observability", "opentelemetry", "traces"]
---

Logico_jp explains how to send metrics, logs, and especially traces from Micronaut applications—including native images—to Azure Monitor, highlighting new advances in Micronaut 4.9.0.<!--excerpt_end-->

# Sending Metrics, Traces, and Logs from Micronaut Native Image Applications to Azure Monitor

_Authored by Logico_jp_

This article explores strategies for exporting metrics, traces, and logs from Micronaut-based applications (including GraalVM Native Image builds) to Azure Monitor, with a focus on recent advancements that make full observability possible. The information is correct as of August 2025.

## Summary of Query

A customer asked how to achieve tracing (in addition to metrics and logs) for Micronaut microservices running on Azure Container Apps and whether this works with GraalVM Native Image builds. They noted that Spring Boot and Quarkus have dedicated dependencies for Microsoft Azure integration and wondered whether Micronaut offers something similar.

## Options for Sending Telemetry to Azure Monitor

Below are the possible approaches:

### 1. Java Agents

- Simple to use but **do not work with GraalVM Native Image** as of mid-2025.
- Result: Not suitable for native images.

### 2. FAT JAR with Azure Dependencies

- Use dependencies such as:
    - `io.micronaut.micrometer:micronaut-micrometer-registry-azure-monitor` (for metrics)
    - `io.micronaut.azure:micronaut-azure-logging` (for logs)
- **Until recently, no tracing support was available** in Micronaut libraries for Azure.

### 3. Azure Monitor Agent with OpenTelemetry Protocol (OTLP)

- Enable Azure Monitor Agent in Azure Container Apps environment.
- Allows sending traces/logs using OTLP from any application type.
- **Metrics not supported** with this approach, according to [Microsoft’s documentation](https://learn.microsoft.com/azure/container-apps/opentelemetry-agents).

## Recent Changes: Native Image-Compatible Tracing in Micronaut 4.9.0

As of Micronaut 4.9.0 (July 2025), tracing is now natively supported for Azure Monitor:

- New dependency: `io.micronaut.azure:micronaut-azure-tracing`
- This library is built on the Azure Monitor OpenTelemetry SDK Autoconfigure Distro, not the agent, so it supports GraalVM Native Images.
- You can now safely generate native images **and** enjoy full tracing to Application Insights.

#### Reference Documentation

- [Azure Monitor OpenTelemetry SDK Autoconfigure Distro (official docs)](https://learn.microsoft.com/java/api/overview/azure/monitor-opentelemetry-autoconfigure-readme?view=azure-java-stable)
- [Announcement: Send metrics from Micronaut native image apps to Azure Monitor](https://techcommunity.microsoft.com/blog/appsonazureblog/send-metrics-from-micronaut-native-image-applications-to-azure-monitor/4443763)

## Key Takeaways for Developers

- **Metrics and Logs**: Use Micrometer and Azure logging dependencies.
- **Traces**: Add `io.micronaut.azure:micronaut-azure-tracing` for Application Insights support.
- **Native Image Support**: With Micronaut 4.9.0+, tracing works with GraalVM native builds without fallback to JVM-only features.
- **Container Apps**: OTLP/Agent approach is available but **not for metrics**.

## Related Resources

- [Micronaut documentation](https://micronaut.io/documentation/)
- [Spring Boot OpenTelemetry Starter](https://opentelemetry.io/docs/zero-code/java/spring-boot-starter/)
- [Quarkus OpenTelemetry Exporter for Azure](https://docs.quarkiverse.io/quarkus-opentelemetry-exporter/dev/quarkus-opentelemetry-exporter-azure.html)
- [Monitor Micronaut native images on Azure (Microsoft Community Hub)](https://techcommunity.microsoft.com/blog/appsonazureblog/send-metrics-from-micronaut-native-image-applications-to-azure-monitor/4443763)

## Quick Example

To send traces from Micronaut apps (native or not):

1. Add the dependency:

    ```groovy
    implementation 'io.micronaut.azure:micronaut-azure-tracing:4.9.0'
    ```

2. Configure Application Insights integration as per documentation.

## Additional Notes

- Always validate support for the latest Micronaut, GraalVM, and Azure Monitor versions.
- For full observability in the cloud, ensure both your app and Azure environment are configured for OpenTelemetry.

---

**Author:** Logico_jp  
Microsoft Community Contributor

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-metrics-traces-logs-from-micronaut-native-image/ba-p/4443735)
