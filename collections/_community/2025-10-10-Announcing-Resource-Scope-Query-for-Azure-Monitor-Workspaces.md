---
external_url: https://techcommunity.microsoft.com/t5/azure-observability-blog/announcing-resource-scope-query-for-azure-monitor-workspaces/ba-p/4460567
title: Announcing Resource-Scope Query for Azure Monitor Workspaces
author: Tyler_Kight
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-10-10 01:32:21 +00:00
tags:
- Access Control
- Application Insights
- Azure Monitor Agent
- Azure Monitor Workspaces
- Azure Portal
- Azure RBAC
- Centralized Monitoring
- Container Insights
- Grafana
- Log Analytics Workspaces
- Metrics
- Observability
- OpenTelemetry
- Prometheus
- Query Endpoint
- RBAC
- Resource Permissions
- Resource Scope Query
- Workspace Permissions
section_names:
- azure
- devops
- security
---
Tyler_Kight announces the public preview of resource-scope query for Azure Monitor Workspaces, detailing how it streamlines observability, access controls, and aligns metric querying with Azure-native experiences.<!--excerpt_end-->

# Announcing Resource-Scope Query for Azure Monitor Workspaces

Tyler_Kight unveils the public preview of resource-scope query capability for Azure Monitor Workspaces (AMWs), marking a major advancement in Azure observability. This feature simplifies metric querying, enhances access control through Azure RBAC, and brings AMWs closer to the native Azure experience.

## What is Resource-Scope Query?

Resource-scope query lets users query metrics and logs scoped directly to specific Azure resources, resource groups, or subscriptions. This avoids needing to know which AMW stores the data, leading to:

- Simpler querying workflows directly within resource contexts.
- More granular access control (RBAC); permissions are checked at the resource level, supporting least-privileged access.

For more details, see: [Resource-scope query in Azure Monitor Workspaces](https://learn.microsoft.com/en-us/azure/azure-monitor/metrics/azure-monitor-workspace-manage-access).

## Why Use Resource-Centric Querying?

Traditionally, querying metrics in AMWs required knowledge of the specific workspace and direct workspace permissions. With resource-centric queries, users:

- Can use the Azure resource metrics blade for direct queries.
- Only require access to the resource, not the workspace.
- Benefit central monitoring teams with controlled AMWs and empowered app teams for self-monitoring.

This especially helps DevOps and on-call engineers troubleshooting alerts without needing to track down storage details.

## How It Works

- **Azure Monitor Agent** automatically adds metadata (resource ID, subscription ID, resource group) to each ingested metric at no extra cost.
- Queries are sent to region-scoped endpoints such as:
  - `https://query.<region>.prometheus.monitor.azure.com`
- Querying options:
  - Azure Portal PromQL Editor
  - Grafana dashboards ([integration guide](https://learn.microsoft.com/en-us/azure/azure-monitor/metrics/azure-monitor-workspace-manage-access))
  - Query-based metric alerts
  - Azure Monitor solutions (Container Insights, App Insights with OTel metrics)
  - Prometheus HTTP APIs
- For programmatic queries, include the `x-ms-azure-scoping` HTTP header to specify the ARM Resource ID.
- Currently, scoping is available at the level of an individual resource, resource group, or subscription. Multi-resource scoping is planned by end of 2025.

## Who Benefits?

- **Application Teams**: Metric access for their own resources without additional workspace permissions.
- **Central Monitoring Teams**: Secure AMW management plus flexible, granular access for app teams.
- **DevOps Engineers**: Easier troubleshooting of resource alerts.
- **Grafana Users**: Dynamic dashboards scoped to subscriptions/resource groups without workspace lookup.

## Access Control Modes

Azure Monitor Workspaces support two access control modes:

- **Require workspace permissions**: Users need explicit permissions to the workspace. Granular RBAC at the resource level is not allowed.
- **Use resource or workspace permissions**: Granular RBAC; users can be limited to just the data of resources they can view, using Azure read permissions.

From October 10, 2025, the resource-context mode becomes the default for new AMWs.

For configuration steps, see [Changing workspace control mode](https://learn.microsoft.com/en-us/azure/azure-monitor/metrics/azure-monitor-workspace-manage-access).

## Rollout and Availability

- Dimension stamping is already enabled for all AMWs.
- Public preview for resource-scope query endpoint starts October 10, 2025.
- All AMWs created after this date will use resource-context access by default.

## Final Thoughts

This update streamlines monitoring workflows and security for Azure professionals. Whether you're overseeing large-scale VM deployments, managing AKS clusters, or creating custom applications with OpenTelemetry, resource-scoped querying allows you to monitor workloads directly in their context. Get started via your resource's Metrics blade or update your Grafana data source to use the new endpoint after the preview date.

---

**References**

- [Resource-scope query documentation](https://learn.microsoft.com/en-us/azure/azure-monitor/metrics/azure-monitor-workspace-manage-access)
- [Azure Monitor Observability Blog](https://techcommunity.microsoft.com/t5/s/gxcuf89792/images/cmstNC05WEo0blc?image-dimensions=100x16&amp;constrain-image=true)

*Author: Tyler_Kight*

*Version 1.0, Updated October 10, 2025*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-observability-blog/announcing-resource-scope-query-for-azure-monitor-workspaces/ba-p/4460567)
