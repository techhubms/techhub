---
layout: post
title: 'Public Preview: Query-Based Metric Alerts Now in Azure Monitor'
author: yairgil
canonical_url: https://techcommunity.microsoft.com/t5/azure-observability-blog/announcing-public-preview-of-query-based-metric-alerts-in-azure/ba-p/4469290
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-12 17:43:31 +00:00
permalink: /azure/community/Public-Preview-Query-Based-Metric-Alerts-Now-in-Azure-Monitor
tags:
- AKS
- Alerting
- ARM Templates
- Azure
- Azure Managed Identity
- Azure Managed Service For Prometheus
- Azure Monitor
- Azure Portal
- Azure REST API
- Community
- Custom Metrics
- DevOps
- Kubernetes
- Metric Alerts
- Monitoring
- OpenTelemetry
- Prometheus
- PromQL
- RBAC
- Resource Management
section_names:
- azure
- devops
---
Yair Gil introduces new query-based metric alerts in Azure Monitor, enabling advanced, PromQL-driven monitoring and alerting across all Azure metrics for scalable cloud environments.<!--excerpt_end-->

# Public Preview: Query-Based Metric Alerts Now in Azure Monitor

Azure Monitor metric alerts have received a significant upgrade: they now support **query-based alert rules** using PromQL, covering all Azure metrics—including platform-provided, Prometheus, and custom metrics. This enables advanced monitoring scenarios for modern cloud workloads.

## Key Features and Benefits

- **Universal Metrics Coverage:** Define alerts using any Azure metric, including those collected from Azure Monitor managed Prometheus, OpenTelemetry, and custom sources.
- **Advanced PromQL Conditions:** Employ complex PromQL queries for sophisticated alert logic, enabling selection, aggregation, and transformation across multiple time series and resources.
- **Event Detection:** Detect intricate metric patterns based on ratio changes, aggregations, cross-metric comparisons, and behaviors over custom time windows.
- **Scoped Flexibility:** Choose resource-centric (for granular RBAC) or workspace-centric alert scoping for cross-resource insight.
- **Scale-Out Monitoring:** Monitor metrics across multiple resources or entire subscriptions with a single alert rule.
- **Managed Identity Authorization:** Securely authorize alert queries using Azure Managed Identity, simplifying credential management and ensuring compliance.
- **Configurable Notifications:** Create custom notification properties or email subjects to streamline alert triage and incident response.
- **Community Integration:** Import and adapt PromQL alerting logic from open-source Prometheus alerting communities.

## Supported Metrics and Integration

- **Prometheus Metrics:** Metrics from Azure Monitor managed Prometheus, including AKS (Azure Kubernetes Service) clusters and other Prometheus-compatible sources.
- **OpenTelemetry (OTel) Metrics:** Collect guest OS metrics and additional custom metrics using OpenTelemetry for deep observability.
- **Custom Metrics:** Support for any metrics ingested in Azure Monitor Workspace (AMW).

Currently, only metrics within an Azure Monitor workspace are supported for query-based alerts. Platform metric support is planned for future updates.

## Comparison: Query-Based Metric Alerts vs. Prometheus Rule Groups

Query-based metric alerts and Prometheus rule group alerts both use PromQL logic. However, query-based alerts are natively integrated as Azure resources and come with:

- Enhanced notification customization
- Scope control (resource or workspace level)
- Support for managed identities
- Resource-group and subscription-wide alerting possibilities

| Feature                  | Prometheus Rule Groups   | Query-Based Metric Alerts        |
|-------------------------|-------------------------|----------------------------------|
| Management              | Rule group resource     | Azure resource                   |
| Metrics Supported       | AMW Prometheus metrics  | AMW Prometheus + OTel metrics    |
| Condition Logic         | PromQL                  | PromQL                           |
| Aggregation/Transformation | Full PromQL          | Full PromQL                      |
| Scope                   | Workspace-wide          | Resource/workspace-wide          |
| Scale                   | Not supported           | Resource group, subscription     |
| Cross-Resource          | Supported               | Supported                        |
| RBAC Granularity        | Workspace               | Resource/workspace               |
| Managed Identity        | Not supported           | Supported                        |
| Notification Customization | Prometheus labels    | Dynamic properties, email subjects|

## Getting Started

To use query-based metric alerts, you need an Azure Monitor workspace with Prometheus or OpenTelemetry metrics. You can create and manage alert rules through:

- **Azure Portal**
- **ARM Templates**
- **Azure REST API**

For complete setup instructions, see the official [Azure Monitor documentation](https://learn.microsoft.com/en-us/azure/azure-monitor/alerts/alerts-query-based-metric-alerts-overview).

## Additional Resources

- [Azure Monitor Managed Service for Prometheus](https://learn.microsoft.com/en-us/azure/azure-monitor/metrics/prometheus-metrics-overview)
- [OpenTelemetry Metrics Collection](https://learn.microsoft.com/en-us/azure/azure-monitor/metrics/metrics-opentelemetry-guest)
- [Azure Managed Identities Overview](https://learn.microsoft.com/en-us/entra/identity/managed-identities-azure-resources/overview)

_Authored by Yair Gil_

_Last updated: Nov 12, 2025_

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-observability-blog/announcing-public-preview-of-query-based-metric-alerts-in-azure/ba-p/4469290)
