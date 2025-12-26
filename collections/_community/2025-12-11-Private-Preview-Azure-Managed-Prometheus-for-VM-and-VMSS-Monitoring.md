---
layout: "post"
title: "Private Preview: Azure Managed Prometheus for VM & VMSS Monitoring"
description: "This community update introduces the private preview of Azure Managed Prometheus support for Virtual Machines (VM) and Virtual Machine Scale Sets (VMSS). Previously available for containerized workloads, Prometheus can now monitor traditional IaaS assets, including capabilities tuned for AI and high-performance computing (HPC) scenarios using GPUs and InfiniBand networking. The update details key benefits, integration with Azure Monitor, access requirements, and how to get started."
author: "Daramfon"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-high-performance-computing/private-preview-azure-managed-prometheus-on-vm-vmss/ba-p/4473472"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-12-11 22:01:17 +00:00
permalink: "/community/2025-12-11-Private-Preview-Azure-Managed-Prometheus-for-VM-and-VMSS-Monitoring.html"
categories: ["Azure", "DevOps"]
tags: ["AKS", "Alerting", "Azure", "Azure Arc", "Azure Managed Grafana", "Azure Monitor", "Community", "DevOps", "GPU Monitoring", "HPC", "IaaS", "InfiniBand", "Metric Collection", "Observability", "Private Preview", "Prometheus", "PromQL", "Virtual Machines", "VMSS"]
tags_normalized: ["aks", "alerting", "azure", "azure arc", "azure managed grafana", "azure monitor", "community", "devops", "gpu monitoring", "hpc", "iaas", "infiniband", "metric collection", "observability", "private preview", "prometheus", "promql", "virtual machines", "vmss"]
---

Daramfon presents an introduction to Azure Managed Prometheus support for VMs and VMSS, now in private preview, highlighting unified monitoring, integration with Azure Monitor, and specific benefits for HPC and AI workloads.<!--excerpt_end-->

# Private Preview: Azure Managed Prometheus for VM & VMSS Monitoring

Azure Managed Prometheus now supports monitoring for traditional Virtual Machines (VM) and Virtual Machine Scale Sets (VMSS), expanding from its original focus on containerized workloads such as AKS and Azure Arc–enabled clusters. This private preview delivers several core enhancements:

## Key Capabilities

- **Unified Metric Collection**: Integrate Prometheus-style monitoring across both containers and IaaS workloads (VMs/VMSS).
- **HPC & AI Monitoring**: Full support for GPU and InfiniBand metric collection, enabling advanced scenarios for high performance computing.
- **Comprehensive Telemetry**: Collect node-level (CPU, memory, storage, NIC, InfiniBand) and GPU-level (utilization, memory, throttling, ECC) metrics through standard Prometheus exporters.
- **Managed Backend**: Leverage a fully managed Prometheus backend with no server maintenance, auto-scaling, and zero operational storage management.
- **Azure Monitor Integration**: Store, query, and visualize metrics directly within [Azure Monitor Workspaces](https://learn.microsoft.com/en-us/azure/azure-monitor/metrics/azure-monitor-workspace-overview).
- **Visualization & Alerting**: Use [Azure Managed Grafana](https://learn.microsoft.com/en-us/azure/managed-grafana/overview) for pre-built dashboards, plus native support for PromQL and alerting rules.
- **Mixed Fleet Observability**: Monitor containerized (AKS), Arc-enabled, and IaaS resources in a single, unified environment.

## Why It Matters for HPC Workloads

- **AI/HPC Optimization**: Enables detailed cluster performance visibility for teams managing GPU-accelerated infrastructure.
- **Node & Cluster Views**: Built-in dashboards and deep linking for fast navigation between cluster-level and node-level insights.
- **Real-Time Querying**: Execute live PromQL queries against Azure Monitor data sources.

## How to Participate

- **Access**: Private preview currently requires allowlisting of your Azure subscription. [Request access here](https://forms.office.com/r/r5g9gDxayz).
- **Getting Started**: Use the [step-by-step onboarding guide](https://github.com/Azure/azhpc-guest-monitoring/blob/main/docs/azure-managed-prometheus-vms.md) on GitHub for setup instructions.
- **Feedback**: Once onboarded, provide feedback and ask questions by opening issues in the provided GitHub repository.

## Next Steps

- Begin scraping metrics, visualize with Azure Managed Grafana, and monitor both AKS and VM-based fleets.
- Leverage out-of-the-box dashboards and custom PromQL queries for observability tailored to HPC and AI environments.

> _This announcement was posted by Daramfon. As this is a private preview, user feedback is especially encouraged to shape the future direction of Azure’s managed monitoring platform._

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/private-preview-azure-managed-prometheus-on-vm-vmss/ba-p/4473472)
