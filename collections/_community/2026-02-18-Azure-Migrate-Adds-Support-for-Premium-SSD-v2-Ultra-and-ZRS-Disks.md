---
external_url: https://techcommunity.microsoft.com/t5/azure-storage-blog/azure-migrate-now-supporting-premium-ssd-v2-ultra-and-zrs-disks/ba-p/4495332
title: Azure Migrate Adds Support for Premium SSD v2, Ultra, and ZRS Disks
author: Lakshya_Jalan
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-02-18 17:22:53 +00:00
tags:
- Azure
- Azure Managed Disks
- Azure Migrate
- Cloud Migration
- Community
- Cost Optimization
- Data Resiliency
- Disk Recommendations
- Infrastructure Migration
- IOPS
- Performance Optimization
- Premium SSD V2
- Throughput
- Ultra Disk
- Workload Assessment
- Zone Redundant Storage
- ZRS Disks
section_names:
- azure
---
Lakshya_Jalan announces enhanced Azure Migrate capabilities, allowing seamless assessment and migration to Premium SSD v2, Ultra, and ZRS Disks for improved workload performance and resiliency.<!--excerpt_end-->

# Azure Migrate Adds Support for Premium SSD v2, Ultra, and ZRS Disks

Azure Migrate continues to evolve with the introduction of assessment and migration support for [Premium SSD v2](https://learn.microsoft.com/en-us/azure/virtual-machines/disks-types), [Ultra Disk](https://learn.microsoft.com/en-us/azure/virtual-machines/disks-types), and [ZRS Disks](https://learn.microsoft.com/en-us/azure/virtual-machines/disks-redundancy#zone-redundant-storage-for-managed-disks). With Premium SSD v2 and ZRS Disks now Generally Available and Ultra Disk in Public Preview, organizations have more flexibility and performance options when migrating mission-critical workloads to Azure.

## What’s New

### Expanded Assessment Targets

- Azure Migrate now includes Premium SSD v2 and Ultra Disks in its assessment process, providing tailored recommendations based on real workload metrics like size, IOPS, and throughput.
- Recommendations go beyond static sizing to ensure optimal performance, resiliency, and cost alignment for each workload.

### Benefits of Advanced Disk Options

- **Premium SSD v2**: Decouples capacity and performance, letting you fine-tune IOPS and throughput for your needs.
- **Ultra Disks**: Delivers the highest performance for demanding scenarios.
- **ZRS Disks**: Offers synchronous replication across three availability zones, boosting data availability and resiliency.

### Migration Workflow Enhancements

- Once Azure Migrate determines the best disk targets, selected disk options (Premium SSD v2, Ultra, or ZRS) are auto-populated into the migration plan.
- The streamlined process simplifies the lift-and-shift of on-premises workloads to high-performance Azure disks.

### Zone-Redundant Storage (ZRS) Migration

- You can now configure assessments in Azure Migrate to recommend ZRS Disks, providing an extra layer of redundancy and protection.
- This is set via advanced server settings in assessment configurations.
- ZRS synchronously replicates data across physically separate zones for improved resiliency.

> “Through this preview, we have Pv2 disks recommendations in place of Pv1, which is beneficial for our estate during migration in terms of both cost and performance. We are now awaiting General Availability.”  
> *– Yogesh Patil, Cloud Enterprise Architect, Tata Consultancy Services (TCS)*

### Getting Started

- Visit the official [Azure Migrate](https://azure.microsoft.com/migration) portal for documentation and resources.
- For expert migration assistance, consider [Azure Accelerate](https://azure.microsoft.com/en-us/solutions/azure-accelerate/), reach out to your Microsoft field partner, or get started with your migration project in [Azure](https://ms.portal.azure.com/#view/Microsoft_Azure_Migrate/AmhManageMigrateProjects.ReactView).

With these new capabilities, Azure Migrate helps you land your applications and data on the most suitable Azure storage platform, maximizing value and operational excellence.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-storage-blog/azure-migrate-now-supporting-premium-ssd-v2-ultra-and-zrs-disks/ba-p/4495332)
