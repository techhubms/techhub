---
layout: post
title: Comprehensive VM Monitoring with OpenTelemetry in Azure Monitor
author: viviandiec
canonical_url: https://techcommunity.microsoft.com/t5/azure-observability-blog/comprehensive-vm-monitoring-with-opentelemetry-performance/ba-p/4470122
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-17 23:17:57 +00:00
permalink: /azure/community/Comprehensive-VM-Monitoring-with-OpenTelemetry-in-Azure-Monitor
tags:
- Arc Servers
- Azure Monitor
- Azure Monitor Workspace
- Data Collection Rules
- Grafana
- Guest OS
- Linux
- Log Analytics
- Metrics
- Monitoring
- Observability
- OpenTelemetry
- Performance Counters
- Process Metrics
- PromQL
- Virtual Machines
- Windows
section_names:
- azure
- devops
---
viviandiec details how Azure Monitor integrates OpenTelemetry for VM and Arc server monitoring. The post covers onboarding, advanced metrics, enhanced dashboards, and new alerting capabilities.<!--excerpt_end-->

# Comprehensive VM Monitoring with OpenTelemetry in Azure Monitor

Monitoring virtual machines has traditionally meant using multiple tools and manual investigation to diagnose issues such as high CPU or memory usage. Azure Monitor's latest update introduces a preview for OpenTelemetry (OTel) Guest OS metrics for both Azure VMs and Arc servers, addressing these pain points.

## Introduction to OpenTelemetry Guest OS Metrics

OpenTelemetry Guest OS metrics provide:

- **System and process-level counters directly from inside a VM** (CPU, memory, disk IO, network, per-process CPU and memory, uptime, thread count)
- **No manual VM logins required for diagnosis**

These counters allow troubleshooting of performance hotspots or abnormal behavior at a granular, process level.

## How Azure Monitor Integrates OpenTelemetry

- **Log Analytics (LA) metrics** remain supported for customers needing custom performance counters, extended retention, and advanced KQL analytics.
- **OpenTelemetry-based metrics** introduce a standards-based, unified schema with:
  - Simplified onboarding for both Windows and Linux
  - Consistent metric names
  - Expanded process/system counters
  - Integration with open-source/OpenTelemetry-based observability tools
  - **Cost-efficient storage** in Azure Monitor Workspace (AMW), reducing costs and improving query performance over Log Analytics

## Key Benefits

| Benefit              | Description                                                                                                            |
|---------------------|------------------------------------------------------------------------------------------------------------------------|
| Unified Data Model  | Consistent metrics schema for Windows and Linux for easier queries and dashboards                                        |
| Richer Counters     | Expanded, less ambiguous process/system metrics (CPU, memory, disk, network)                                             |
| Easy Onboarding     | Minimal setup through streamlined Azure onboarding UI                                                                   |
| Flexible Visualization | Out-of-the-box Azure dashboards, Metrics Explorer, or custom Grafana dashboards                                      |
| Cost Efficient      | Storage in AMW is more cost-effective and offers faster queries                                                         |

## Selecting the Right Metrics Pipeline

- **LA-based metrics (general availability):**
  - Use for custom performance counters, extended retention, advanced KQL, and when deep log-to-metric correlation is needed.
  - Fully mature and supported.
- **OTel-based metrics (preview):**
  - Use for standards-based telemetry, quick and simple onboarding, deeper and broader system/process coverage, and lower-cost storage.

## Onboarding and Scaling

- **Single-VM onboarding:**
  - Enable OTel counters via a dedicated screen; Azure configures Data Collection Rules (DCRs) automatically.
- **Bulk onboarding:**
  - Use the Monitoring Coverage experience or Essential Machine Management for at-scale enablement.
- **Customization:**
  - Edit DCRs to collect more metrics/logs and propagate across all associated VMs.

## Enhanced Monitoring and Dashboards

A new preview monitoring experience for Azure VMs and Arc servers is available under Azure Portal → Virtual Machine → Monitoring → Insights. It offers:

- **Basic View:** Host OS-level metrics (available by default)
- **Detailed View:** Guest OS-level metrics using OpenTelemetry (requires onboarding)

### Custom Dashboards with Grafana

- Create detailed visualizations and dashboards on top of OTel Guest OS metrics (CPU, memory, disk, network, per-process insights)
- Identify abnormal resource behavior without manual VM access
- [Documentation link for Grafana integration](https://learn.microsoft.com/en-us/azure/azure-monitor/vm/vminsights-opentelemetry#additional-metrics)

### Query-Based Metric Alerts (Preview)

- PromQL-based alerts: Customize and trigger alerts on OTel metrics stored in Azure Monitor Workspace
- Example: Set alerts on CPU/memory usage spikes by specific processes

## Getting Started

- [Get started with VM Monitoring (Preview)](https://learn.microsoft.com/en-us/azure/azure-monitor/vm/vminsights-opentelemetry)
- [Azure Monitor Dashboards with Grafana](https://learn.microsoft.com/en-us/azure/azure-monitor/visualize/visualize-use-grafana-dashboards)
- [Query Based Metric Alerts Overview (Preview)](https://learn.microsoft.com/en-us/azure/azure-monitor/alerts/alerts-query-based-metric-alerts-overview)
- [Application Monitoring Preview](https://techcommunity.microsoft.com/t5/aka.ms/AzureMonitorOTelPreview)

These enhancements make VM performance monitoring more open, standards-driven, and actionable—supporting both consistent day-to-day oversight and advanced troubleshooting for large VM fleets.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-observability-blog/comprehensive-vm-monitoring-with-opentelemetry-performance/ba-p/4470122)
