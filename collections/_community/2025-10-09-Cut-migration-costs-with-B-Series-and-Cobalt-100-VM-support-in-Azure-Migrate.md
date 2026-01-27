---
external_url: https://techcommunity.microsoft.com/t5/azure-migration-and/cut-migration-costs-with-b-series-and-cobalt-100-vm-support-in/ba-p/4460285
title: Cut migration costs with B-Series and Cobalt 100 VM support in Azure Migrate
author: ankitsurkar
feed_name: Microsoft Tech Community
date: 2025-10-09 10:26:14 +00:00
tags:
- ARM Native Workloads
- ARM64
- Azure Infrastructure
- Azure Migrate
- Azure Virtual Machines
- B Series VM
- Burstable VMs
- Cloud Migration
- Cobalt 100 VM
- Cost Estimation
- Energy Efficiency
- VM Cost Optimization
- Workload Assessment
section_names:
- azure
primary_section: azure
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
