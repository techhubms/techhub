---
external_url: https://techcommunity.microsoft.com/t5/azure-observability-blog/announcing-public-preview-of-query-based-metric-alerts-in-azure/ba-p/4469723
title: 'Announcing Public Preview: Query-Based Metric Alerts in Azure Monitor'
author: yairgil
feed_name: Microsoft Tech Community
date: 2025-11-23 18:55:48 +00:00
tags:
- AKS
- Alerting
- ARM Templates
- Azure Monitor
- Azure REST API
- Cloud Monitoring
- Custom Metrics
- Kubernetes Monitoring
- Managed Identity
- Metric Alerts
- OpenTelemetry
- Prometheus
- PromQL
- RBAC
- Resource Management
section_names:
- azure
primary_section: azure
---
yairgil introduces the new public preview feature of query-based metric alerts in Azure Monitor, showcasing advanced monitoring and alerting options for Azure users and developers.<!--excerpt_end-->

# Announcing Public Preview: Query-Based Metric Alerts in Azure Monitor

Azure Monitor metric alerts now offer comprehensive coverage for all metrics in your monitoring environment—including Azure platform metrics, Prometheus, and OpenTelemetry custom metrics. These improvements empower you to:

- Write advanced queries and conditions using PromQL,
- Detect complex event patterns,
- Monitor workloads at scale, and
- Customize alert notifications in ways not previously possible.

## Key Benefits

- **Full Metrics Coverage:** Query-based metric alerts now support all Azure metrics, including custom metrics ingested from Prometheus and OpenTelemetry workloads.
- **PromQL-Driven Conditions:** Apply PromQL syntax to select, aggregate, and transform metrics for robust alerting scenarios.
- **Sophisticated Event Detection:** Identify complex relationships and patterns across multiple timeseries, perform change detection, aggregation, and time-based comparisons.
- **Flexible Scoping:** Choose resource-centric (for granular RBAC) or workspace-centric (cross-resource) alert rules.
- **Scalable Alerting:** Build rules that monitor metrics across many resources, subscriptions, or entire resource groups.
- **Managed Identity Support:** Use Azure Managed Identity for secure, compliant queries without manual credential management.
- **Customizable Alerts:** Set dynamic custom properties and email subjects to streamline triage and contextualize alerts for your team.
- **Community Integration:** Import and use community PromQL alert queries for standard or custom scenarios.

## Supported Metrics

Query-based metric alerts work with any metrics available in Azure Monitor Workspaces, including:

- Metrics from [Azure Monitor Managed Service for Prometheus](https://learn.microsoft.com/en-us/azure/azure-monitor/metrics/prometheus-metrics-overview) — perfect for AKS clusters and other Prometheus sources.
- [Virtual Machine OpenTelemetry Guest OS Metrics](https://learn.microsoft.com/en-us/azure/azure-monitor/metrics/metrics-opentelemetry-guest).
- Other [OpenTelemetry custom metrics](https://learn.microsoft.com/en-us/azure/azure-monitor/app/opentelemetry) ingested into Azure Monitor.

Threshold-based metric alerts remain supported for platform metrics, with query-based alerts on platform metrics coming soon.

## Comparison: Query-Based Metric Alerts vs Prometheus Rule Groups

| Feature | Azure Prometheus Rule Groups | Query-Based Metric Alerts |
|---|---|---|
| Alert rule management | Part of a rule group resource | Independent Azure resource |
| Supported metrics | Managed Prometheus only | Managed Prometheus, OTel metrics |
| Condition logic | PromQL | PromQL |
| Aggregation & transformation | Full PromQL | Full PromQL |
| Scope | Workspace-wide | Resource-centric or workspace-wide |
| Alerting at scale | Not supported | Subscription/resource group level |
| Cross-resource conditions | Supported | Supported |
| RBAC granularity | Workspace level | Resource or workspace level |
| Managed identity support | Not supported | Supported |
| Notification customization | Label/annotation-based | Dynamic properties, emails |

This comparison demonstrates the increased power, flexibility, and native Azure integration of query-based metric alerts, making them suitable for modern monitoring needs.

## Getting Started

If you have an Azure Monitor workspace with Prometheus or OpenTelemetry metrics, you can create query-based metric alert rules now using:

- **Azure Portal**
- **ARM templates**
- **Azure REST API**

For full setup details and documentation, visit the [Azure Monitor query-based metric alerts overview](https://learn.microsoft.com/en-us/azure/azure-monitor/alerts/alerts-query-based-metric-alerts-overview).

Stay tuned for further enhancements to this feature in upcoming releases.

---

*Published by yairgil, Microsoft*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-observability-blog/announcing-public-preview-of-query-based-metric-alerts-in-azure/ba-p/4469723)
