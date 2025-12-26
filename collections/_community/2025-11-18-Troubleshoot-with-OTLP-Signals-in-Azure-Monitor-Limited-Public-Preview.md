---
layout: "post"
title: "Troubleshoot with OTLP Signals in Azure Monitor (Limited Public Preview)"
description: "This community post explains Azure Monitor's expanded support for OpenTelemetry (OTel) standards, enabling developers and operations teams to channel OTLP logs, metrics, and traces directly from distributed applications to Azure Monitor. It covers how to collect telemetry using Azure Monitor Agent, AKS add-ons, and the OpenTelemetry Collector, storing metrics in Prometheus-compatible workspaces and leveraging Application Insights for distributed tracing and troubleshooting. The preview extends auto-instrumentation for .NET and Python, with unified monitoring across AKS, VMs, and more."
author: "SoubhagyaDash"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-observability-blog/troubleshoot-with-otlp-signals-in-azure-monitor-limited-public/ba-p/4469668"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-18 16:09:25 +00:00
permalink: "/community/2025-11-18-Troubleshoot-with-OTLP-Signals-in-Azure-Monitor-Limited-Public-Preview.html"
categories: ["Azure", "Coding", "DevOps"]
tags: [".NET", "AKS", "Application Insights", "Auto Instrumentation", "Azure", "Azure Arc", "Azure Monitor", "Azure Monitor Agent", "Coding", "Community", "Container Insights", "DevOps", "Distributed Tracing", "Grafana", "Java", "Log Analytics", "Node.js", "OpenTelemetry", "OTLP", "Prometheus", "Python", "Telemetry", "VMSS"]
tags_normalized: ["dotnet", "aks", "application insights", "auto instrumentation", "azure", "azure arc", "azure monitor", "azure monitor agent", "coding", "community", "container insights", "devops", "distributed tracing", "grafana", "java", "log analytics", "nodedotjs", "opentelemetry", "otlp", "prometheus", "python", "telemetry", "vmss"]
---

SoubhagyaDash discusses Azure Monitor's new support for OpenTelemetry standards and OTLP signal ingestion, guiding developers and operations teams on unified observability and troubleshooting techniques.<!--excerpt_end-->

# Troubleshoot with OTLP Signals in Azure Monitor (Limited Public Preview)

Azure Monitor now offers expanded support for the OpenTelemetry (OTel) standard, making unified observability significantly easier for distributed cloud-native applications. This preview empowers both developers and operations teams to collect, analyze, and act on telemetry data—logs, metrics, and traces—via the OpenTelemetry Protocol (OTLP), no matter where their applications are running.

## Key Features

- **Direct OTLP Signal Ingestion:** Transmit logs, metrics, and traces from your OTel SDK-instrumented applications straight to Azure Monitor.
- **Unified Collection on Azure:**
  - **VMs/VMSS/Azure Arc:** Use the familiar Azure Monitor Agent (AMA) to collect both infrastructure and application telemetry.
  - **AKS (Azure Kubernetes Service):** Azure Monitor add-ons orchestrate Container Insights and managed Prometheus, auto-configuring OTLP signal collection and supporting auto-instrumentation for .NET and Python.

## Flexible Telemetry Collection

- Use Azure compute orchestration or your own OpenTelemetry Collector to channel OTLP signals to Azure Monitor cloud ingestion endpoints.
- Easily collect OTLP signals via agents or add-ons, or leverage custom OTel Collector setups as needed.

## Data Storage and Troubleshooting

- **Metrics:** Stored in Azure Monitor Workspace, leveraging Prometheus as a metrics store.
- **Logs and Traces:** Saved in Azure Monitor Log Analytics Workspace under OTel semantic conventions.
- **Application Insights:** Lights up for distributed tracing and rich troubleshooting across applications and infrastructure.
- **Dashboards:** Get out-of-the-box visualization with Grafana, integrating community dashboards for rapid analysis.

## Auto-Instrumentation Expansion

- The preview extends support for auto-instrumentation to .NET and Python applications on AKS.
- OTLP metrics collection is now available for all auto-instrumented applications, including Java, Node.js, .NET, and Python.

## How to Get Started

- Sign up for the limited public preview: [https://aka.ms/azuremonitorotelpreview](https://aka.ms/azuremonitorotelpreview)
- Access documentation and resources for configuring agents, add-ons, or OTel Collector.

## Summary

Azure Monitor’s OTel preview brings standards-based telemetry collection, simplified setup across VMs/VMSS/AKS, and seamless integration with analytics and visualization tools. This unlocks comprehensive troubleshooting, distributed tracing, and performance optimization with minimal friction.

---

*Authored by SoubhagyaDash, this guide outlines Azure Monitor's technical enhancements for OpenTelemetry observability and offers practical instructions for developers and ops teams looking to unify their monitoring workflows.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-observability-blog/troubleshoot-with-otlp-signals-in-azure-monitor-limited-public/ba-p/4469668)
