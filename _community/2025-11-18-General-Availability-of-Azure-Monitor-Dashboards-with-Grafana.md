---
layout: "post"
title: "General Availability of Azure Monitor Dashboards with Grafana"
description: "This article announces the general availability of Azure Monitor dashboards with Grafana, detailing how users can create and manage cloud-native monitoring dashboards within the Azure portal. The content covers pre-built and community dashboards, integration best practices, automation using ARM/Bicep, and open-source compatibility for monitoring a wide range of Azure resources."
author: "KayodePrince"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-observability-blog/announcing-general-availability-azure-monitor-dashboards-with/ba-p/4468972"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-18 16:56:24 +00:00
permalink: "/2025-11-18-General-Availability-of-Azure-Monitor-Dashboards-with-Grafana.html"
categories: ["Azure", "DevOps"]
tags: ["AKS", "Application Insights", "ARM Templates", "Azure", "Azure Container Apps", "Azure Data Explorer", "Azure Managed Grafana", "Azure Monitor", "Azure Portal", "Azure PostgreSQL", "Bicep", "Cloud Native Monitoring", "Community", "Cosmos DB", "Dashboard Automation", "DevOps", "Grafana", "Microsoft Foundry", "Monitoring Solutions", "OpenTelemetry", "RBAC"]
tags_normalized: ["aks", "application insights", "arm templates", "azure", "azure container apps", "azure data explorer", "azure managed grafana", "azure monitor", "azure portal", "azure postgresql", "bicep", "cloud native monitoring", "community", "cosmos db", "dashboard automation", "devops", "grafana", "microsoft foundry", "monitoring solutions", "opentelemetry", "rbac"]
---

KayodePrince introduces the general availability of Azure Monitor dashboards with Grafana, a new solution for building and managing cloud-native monitoring dashboards directly in the Azure portal.<!--excerpt_end-->

# General Availability of Azure Monitor Dashboards with Grafana

Azure has announced the general availability of Azure Monitor dashboards with Grafana, offering cloud-native monitoring and visualization for users' Azure data. This release enables:

- **Direct creation and editing of Grafana dashboards within the Azure portal**, reducing administrative overhead compared to managing Grafana instances independently or paying for managed services.
- **Pre-built dashboards** for Azure services such as Kubernetes, Application Insights, Storage Accounts, Cosmos DB, Azure PostgreSQL, OpenTelemetry metrics, and more.
- **Import capabilities** from thousands of community and open-source dashboards, supporting data sources like Prometheus, Azure Monitor (metrics, logs, traces, Resource Graph), and Azure Data Explorer.

## Key Features

- **Open-source compatibility**: Dashboards are portable across all Grafana instances, native to Azure, and support Azure RBAC permissions and ARM/Bicep template automation.
- **Regional access**: Create and manage Grafana dashboards in over 30 Azure regions.
- **Language selection**: End users can select their preferred language in the Azure Portal for Grafana UI.
- **Automation**: Manage dashboard resources as ARM entities, auto-generate ARM templates for deployment and management.

## Enhanced Monitoring with Grafana Explore

- Use [Grafana Explore](https://grafana.com/docs/grafana/latest/explore/get-started-with-explore/) for ad-hoc metric queries and visualization integration.
- Out-of-the-box support for:
  - Additional Azure Kubernetes Service configurations, including AKS Automatic and AKS Arc clusters
  - Azure Container Apps dashboards
  - Microsoft Foundry and Agent Framework
  - High Performance Computing dashboards, including GPU monitoring
  - OpenTelemetry metrics
  - Application Insights monitoring

## Upgrade Path to Azure Managed Grafana

For users needing advanced enterprise Grafana capabilities or integration with third-party sources, Azure Managed Grafana is available as a fully managed hosted service for Grafana Enterprise.

- Comparison details available: [Solution Comparison](https://aka.ms/DashboardsWithGrafanaComparison)
- Get started: [Learn more](https://aka.ms/dashboardswithgrafanadocs)

## Supported Azure Resources and Integrations

- Kubernetes (AKS)
- Application Insights
- Storage Accounts
- Cosmos DB
- Azure PostgreSQL
- Azure Data Explorer
- OpenTelemetry & Prometheus Metrics

## Automation and Security

- Dashboard management with Azure Resource Manager and Bicep templates
- Azure RBAC support for controlled dashboard access

## Community and Support

- Access thousands of community dashboards ([Grafana community dashboards](https://grafana.com/grafana/dashboards/?dataSource=prometheus))
- Engage with Azure Observability Blog for updates and best practices

## Author

**KayodePrince** - Microsoft Azure Observability Blog Contributor

_Last updated: Nov 13, 2025_

_Version: 1.0_

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-observability-blog/announcing-general-availability-azure-monitor-dashboards-with/ba-p/4468972)
