---
external_url: https://techcommunity.microsoft.com/t5/azure-observability-blog/azure-monitor-managed-service-for-prometheus-now-includes-native/ba-p/4454254
title: 'Azure Monitor’s Native Grafana Dashboards: Simplified Observability for Prometheus Workloads'
author: sunayanasingh
feed_name: Microsoft Tech Community
date: 2025-09-18 17:51:27 +00:00
tags:
- AKS
- Application Insights
- Azure Managed Grafana
- Azure Monitor
- Cloud Native Monitoring
- Container Insights
- Dashboards
- Grafana
- Kubernetes Monitoring
- Metrics Visualization
- Observability
- OpenTelemetry
- Prometheus
- SaaS Monitoring
- SRE
section_names:
- azure
- devops
primary_section: azure
---
sunayanasingh introduces the native integration of Grafana dashboards within Azure Monitor managed Prometheus services, now directly accessible via the Azure portal. Learn about streamlined observability and easy onboarding for Azure workloads.<!--excerpt_end-->

# Azure Monitor’s Native Grafana Dashboards: Simplified Observability for Prometheus Workloads

Azure Monitor’s managed service for Prometheus now features native Grafana dashboards directly inside the Azure portal, giving teams powerful visualization tools for cloud-native monitoring at no additional cost. This eliminates the need to deploy or maintain self-hosted Grafana servers, reducing complexity and management overhead for DevOps and SRE teams.

## Key Benefits of Azure Monitor + Grafana Integration

- **Unified Azure experience:** Use your existing Azure login and RBAC for seamless access. Grafana dashboards appear next to other Azure resources and Monitor views.
- **No operational burden:** Fully SaaS-based dashboards—no servers or extra compute to provision or manage.
- **Open-source compatibility:** Import open-source and community dashboards using Prometheus or Azure Monitor data sources without changes.
- **Integrated visualization:** Visualize Prometheus metrics, Azure resources, logs, and traces all in one place.
- **Pre-built dashboards:** Ready-to-use dashboards available for scenarios like AKS, Azure Container Apps, Container Insights, and Application Insights.

## Who Benefits?

Teams using Prometheus metrics and OpenTelemetry logs/traces to monitor modern, distributed, or containerized workloads—especially in Azure Kubernetes Service (AKS) and related environments. This is particularly valuable for:

- **DevOps** and **SRE** teams seeking consistent, industry-standard observability tools
- Developers looking to accelerate troubleshooting and diagnostics in Azure

## Getting Started

1. **Enable Managed Prometheus** for your AKS cluster: [docs](https://learn.microsoft.com/en-us/azure/azure-monitor/containers/kubernetes-monitoring-enable?tabs=cli#enable-prometheus-and-grafana)
2. Navigate to your Azure Monitor workspace or AKS resource.
3. Go to **Monitoring > Dashboards with Grafana (preview)** in the Azure portal.
4. Explore, edit, or import dashboards. Pre-built templates are provided, but you can further customize panels, variables, and visualizations to suit your needs.

With this setup, there’s no need to deploy extra Grafana servers or configure separate infrastructure—teams can quickly take advantage of robust observability tools directly within Azure.

## When to Upgrade to Azure Managed Grafana?

While the built-in dashboards handle most Azure-centric Prometheus scenarios, consider Azure Managed Grafana for:

- Integrating non-Azure or external data sources
- Advanced authentication and networking
- Multi-cloud or hybrid connectivity needs

See the detailed [comparison guide](https://aka.ms/DashboardsWithGrafanaComparison) for choosing between solutions.

## Additional Resources

- [Azure Monitor dashboards with Grafana documentation](https://aka.ms/DashboardsWithGrafanaDocs)
- [Getting started guidance](https://aka.ms/dashboardswithgrafanadocs)

Stay tuned for further updates as Microsoft continues to advance cloud-native observability.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-observability-blog/azure-monitor-managed-service-for-prometheus-now-includes-native/ba-p/4454254)
