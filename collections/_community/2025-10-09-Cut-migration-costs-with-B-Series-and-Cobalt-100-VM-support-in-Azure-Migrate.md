---
layout: "post"
title: "Cut migration costs with B-Series and Cobalt 100 VM support in Azure Migrate"
description: "This article introduces Azure Migrate's support for B-Series and Cobalt 100 virtual machines, helping organizations optimize migration costs and performance on Azure. The content explains the benefits of burstable B-Series VMs for variable workloads, and highlights Cobalt 100 VMs as an ARM-native solution for ARM64 workloads. Cost comparisons and assessment tools are provided."
author: "ankitsurkar"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-migration-and/cut-migration-costs-with-b-series-and-cobalt-100-vm-support-in/ba-p/4460285"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-10-09 10:26:14 +00:00
permalink: "/community/2025-10-09-Cut-migration-costs-with-B-Series-and-Cobalt-100-VM-support-in-Azure-Migrate.html"
categories: ["Azure"]
tags: ["ARM Native Workloads", "ARM64", "Azure", "Azure Infrastructure", "Azure Migrate", "Azure Virtual Machines", "B Series VM", "Burstable VMs", "Cloud Migration", "Cobalt 100 VM", "Community", "Cost Estimation", "Energy Efficiency", "VM Cost Optimization", "Workload Assessment"]
tags_normalized: ["arm native workloads", "arm64", "azure", "azure infrastructure", "azure migrate", "azure virtual machines", "b series vm", "burstable vms", "cloud migration", "cobalt 100 vm", "community", "cost estimation", "energy efficiency", "vm cost optimization", "workload assessment"]
---

ankitsurkar explains how Azure Migrate’s new support for B-Series and Cobalt 100 VMs enables smarter, more cost-effective cloud migrations by allowing better alignment of workloads with infrastructure options.<!--excerpt_end-->

# Cut Migration Costs with B-Series and Cobalt 100 VM Support in Azure Migrate

## Introduction

Migrating to the cloud doesn't have to mean higher costs. Many workloads—such as development/test environments, small databases, and low-traffic applications—don't need continuous CPU power. Azure Migrate now supports both B-Series and Cobalt 100 VMs, providing more flexibility and cost optimization during migration planning.

## B-Series VMs: Burstable Power, Lower Costs

B-Series virtual machines allow workloads with variable CPU usage to operate cost-effectively:

- **Baseline Performance:** Pay for a steady, low baseline of CPU performance.
- **Burst Capability:** Temporarily scale up with accumulated CPU credits.
- **Cost Efficiency:** Ideal for workloads that spend much of their time idle, such as dev/test, small web servers, or batch jobs.

**Savings example:**

- Standard_B8s v2 (B-Series): 8 vCPUs, 32 GiB RAM, approx $270.10/month (Windows license included)
- D8s v4 (D-Series): 8 vCPUs, 32 GiB RAM, approx $548.96/month (Windows license included)

If your workloads spike only occasionally, B-Series VMs can cut costs by up to 50% compared to D-Series.

## Cobalt 100 VMs: ARM-Native Power

Cobalt 100 VMs are optimized for ARM64 workloads:

- **Architecture Alignment:** Supports ARM64-based servers without major re-architecture.
- **Performance Gains:** Take advantage of ARM-native optimizations for better efficiency.
- **Cost Advantages:** Pair ARM64 hardware efficiency with Azure’s pricing for more value per core.

Great for migrating modern, energy-efficient workloads running on-premises ARM servers.

## Why Use New VM Types in Azure Migrate?

- Match workload needs to infrastructure for best performance and value
- Assess and plan migrations with improved accuracy
- Fit workloads onto the most suitable cloud resources available

## Get Started

Review the [Assessment Properties in Azure Migrate](https://learn.microsoft.com/en-us/azure/migrate/vm-assessment-properties?view=migrate) for details.

## About the Author

Content by ankitsurkar, active contributor to Microsoft technical community blogs.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-migration-and/cut-migration-costs-with-b-series-and-cobalt-100-vm-support-in/ba-p/4460285)
