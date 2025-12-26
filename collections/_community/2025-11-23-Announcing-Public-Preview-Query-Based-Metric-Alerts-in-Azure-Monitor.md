---
layout: "post"
title: "Announcing Public Preview: Query-Based Metric Alerts in Azure Monitor"
description: "This post details the public preview of query-based metric alerts in Azure Monitor. It covers how to leverage PromQL across all Azure-supported metrics, including platform, Prometheus, and custom OpenTelemetry metrics. The article compares this new approach with existing Prometheus rule groups, outlining feature enhancements such as RBAC options, managed identity, scalable alerting, and custom notifications. Practical guidance for getting started is included."
author: "yairgil"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-observability-blog/announcing-public-preview-of-query-based-metric-alerts-in-azure/ba-p/4469723"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-23 18:55:48 +00:00
permalink: "/community/2025-11-23-Announcing-Public-Preview-Query-Based-Metric-Alerts-in-Azure-Monitor.html"
categories: ["Azure"]
tags: ["AKS", "Alerting", "ARM Templates", "Azure", "Azure Monitor", "Azure REST API", "Cloud Monitoring", "Community", "Custom Metrics", "Kubernetes Monitoring", "Managed Identity", "Metric Alerts", "OpenTelemetry", "Prometheus", "PromQL", "RBAC", "Resource Management"]
tags_normalized: ["aks", "alerting", "arm templates", "azure", "azure monitor", "azure rest api", "cloud monitoring", "community", "custom metrics", "kubernetes monitoring", "managed identity", "metric alerts", "opentelemetry", "prometheus", "promql", "rbac", "resource management"]
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
