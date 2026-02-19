---
layout: "post"
title: "Public Preview: Automatic Zone Balance for Azure Virtual Machine Scale Sets"
description: "This announcement details the public preview of automatic zone balance for Azure Virtual Machine Scale Sets. The new feature introduces automatic VM redistribution across availability zones to enhance resiliency and minimize manual intervention. It explains the value for workload reliability, operational safety measures, and the steps needed by users to enable the feature in their Azure environments."
author: "HilaryWang"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-compute-blog/public-preview-automatic-zone-balance-for-virtual-machine-scale/ba-p/4494476"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-02-17 17:30:00 +00:00
permalink: "/2026-02-17-Public-Preview-Automatic-Zone-Balance-for-Azure-Virtual-Machine-Scale-Sets.html"
categories: ["Azure"]
tags: ["Application Health Extension", "Automatic Zone Balance", "Availability Zones", "Azure", "Azure Compute", "Azure Virtual Machine Scale Sets", "Cloud Infrastructure", "Community", "Fault Tolerance", "Instance Health Monitoring", "Load Balancer Health Probes", "Load Balancing", "Operational Best Practices", "Resiliency", "Scale Set Monitoring", "VM Deployment"]
tags_normalized: ["application health extension", "automatic zone balance", "availability zones", "azure", "azure compute", "azure virtual machine scale sets", "cloud infrastructure", "community", "fault tolerance", "instance health monitoring", "load balancer health probes", "load balancing", "operational best practices", "resiliency", "scale set monitoring", "vm deployment"]
---

HilaryWang introduces the public preview of automatic zone balance for Azure Virtual Machine Scale Sets, outlining how this feature enhances workload resiliency by automatically distributing VMs across availability zones.<!--excerpt_end-->

# Public Preview: Automatic Zone Balance for Azure Virtual Machine Scale Sets

Azure now offers **automatic zone balance** in public preview for Virtual Machine Scale Sets. This feature is designed to help organizations maintain zone-resilient workloads with no manual intervention by automatically distributing VM instances across availability zones.

## Key Features

- **Continuous Monitoring**: Automatically detects and corrects zonal imbalances as your scale set changes over time due to scaling operations or capacity constraints.
- **Create-Before-Delete Workflow**: New VMs are created and verified in under-provisioned zones before removing from over-provisioned zones, ensuring there is no reduction in workload capacity during rebalancing.
- **Health Checks Integration**: Works with Application Health Extension or Load Balancer Health Probes to maintain only healthy VM instances.
- **Safety Guardrails**: Honors instance protection rules and pauses during active operations to avoid conflicts. Moves are performed gradually to minimize operational churn.
- **Automatic Instance Repairs**: Enabling automatic zone balance also activates automatic instance repairs by default, giving both zone-level and instance-level resiliency.

## Benefits

- Ideal for workloads requiring high availability, as it reduces the impact radius in case of a zone failure.
- Minimizes operational overhead by automating balancing and instance repairs.
- Ensures optimal distribution even as scale sets grow, shrink, or zones face intermittent resource constraints.

## How It Works

1. **Registration**: Enable the `AutomaticZoneRebalancing` feature flag via Azure portal, CLI, or PowerShell.
2. **Prerequisites**: Your scale set must:
    - Span at least two availability zones
    - Use best-effort zone balancing mode
    - Have application health monitoring configured
3. **Enabling**: Turn on automatic zone balance through the Azure portal, CLI, PowerShell, or REST API.

> For setup details and complete instructions, consult the [automatic zone balance documentation](https://aka.ms/AutoZoneBalanceDocs).

## Operational Principles

- Balancing is performed with minimal disruption; only one VM is moved at a time, and back-off intervals are used to prevent excessive changes.
- Protection policies and in-progress scale set actions are respected for safe operation.

---
*For questions or feedback on this feature, visit the blog board or refer to the official documentation.*

_Last updated: Feb 13, 2026 by HilaryWang_

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-compute-blog/public-preview-automatic-zone-balance-for-virtual-machine-scale/ba-p/4494476)
