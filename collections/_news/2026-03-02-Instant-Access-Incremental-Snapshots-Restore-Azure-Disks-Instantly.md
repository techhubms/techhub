---
layout: "post"
title: "Instant Access Incremental Snapshots: Restore Azure Disks Instantly"
description: "Aung Oo describes Azure's introduction of instant access support for incremental snapshots with Premium SSD v2 and Ultra Disk. This update enables instant creation, restore, and production-ready performance for mission-critical workloads, streamlining disk recovery and scaling in Azure environments."
author: "Aung Oo"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://azure.microsoft.com/en-us/blog/instant-access-incremental-snapshots-restore-without-waiting/"
viewing_mode: "external"
feed_name: "The Azure Blog"
feed_url: "https://azure.microsoft.com/en-us/blog/feed/"
date: 2026-03-02 17:00:00 +00:00
permalink: "/2026-03-02-Instant-Access-Incremental-Snapshots-Restore-Azure-Disks-Instantly.html"
categories: ["Azure"]
tags: ["Azure", "Azure CLI", "Azure Managed Disks", "Azure Storage", "Cloud Scalability", "Data Protection", "Disk Restore", "High Availability", "Incremental Snapshots", "Instant Access", "Managed Disks", "Mission Critical Workloads", "News", "Performance Optimization", "Premium SSD V2", "Snapshot API", "Storage", "Ultra Disk", "Zone Redundant Storage"]
tags_normalized: ["azure", "azure cli", "azure managed disks", "azure storage", "cloud scalability", "data protection", "disk restore", "high availability", "incremental snapshots", "instant access", "managed disks", "mission critical workloads", "news", "performance optimization", "premium ssd v2", "snapshot api", "storage", "ultra disk", "zone redundant storage"]
---

Aung Oo covers how instant access incremental snapshots transform disk restore and scaling in Azure, allowing developers and IT professionals to restore production-ready disks instantly with Premium SSD v2 and Ultra Disk.<!--excerpt_end-->

# Instant Access Incremental Snapshots: Restore Azure Disks Instantly

Azure introduces instant access support for incremental snapshots of Premium SSD v2 (Pv2) and Ultra Disk. This feature lets users create, restore, and operate production-ready disks instantly, delivering new levels of speed and efficiency for mission-critical workloads.

## Key Highlights

- **Immediate Usability**: Instant access snapshots let you restore disks as soon as they're created—no need to wait for full data hydration.
- **Top Performance**: Restored disks offer near-full performance from the start, with single-digit millisecond read latencies and sub-millisecond write latencies.
- **Incremental and Cost-Efficient**: Snapshots store only the changes made after creation, optimizing storage costs.
- **Cross-Zonal Restore**: Restore disks into different availability zones for added resiliency and flexibility.

## Benefits for Developers and Operations

- **Fast Rollback**: Create instant recovery points before risky software upgrades, allowing for quick rollbacks if needed.
- **Uninterrupted Maintenance**: Reduce downtime by performing maintenance operations immediately after snapshot creation.
- **Rapid Scale-Out**: Scale applications by quickly duplicating disks, such as adding new SQL Server replicas across zones in seconds.
- **Dev/Test Environment Refresh**: Duplicate production environments for development or testing with immediate readiness.

## Usage Example: Azure CLI

To create a standard incremental snapshot:

```
az snapshot create --resource-group <rg-name> --name <snapshot-name> --source <disk-id> --incremental true
```

To enable instant access, add the `InstantAccessDurationMins` parameter:

```
az snapshot create --resource-group <rg-name> --name <snapshot-name> --source <disk-id> --incremental true --InstantAccessDurationMins 300
```

## Architecture and Operation

- **Standard vs. Instant Access**: Traditional snapshots require background data copying and offer full performance only after hydration. Instant access snapshots remain in high-performance storage temporarily, allowing immediate disk creation and rapid performance.
- **Automatic Transition**: When the instant access period expires, snapshots are automatically transitioned to Standard Zone-Redundant Storage for resilience and long-term retention.
- **No New APIs Required**: Instant access uses existing Azure Snapshot API endpoints—the key difference is the additional instant access duration parameter.

## Typical Scenarios

- **Backup and Recovery**: Mission-critical workloads can minimize downtime by restoring disks instantly.
- **Application Scaling**: Services like Azure Database for PostgreSQL can add replicas by spinning up new disks on-demand.
- **Testimonial**:

> "Instead of waiting on long background copy processes for snapshot creation and disk hydration, our teams can create fully usable disks immediately for the day’s training and support workflows."  
> — *Chris Calas, Senior Principal Service Engineer, Providence*

## Availability and Pricing

- Supported in all regions offering Premium SSD v2 and Ultra Disk.
- Usage-based pricing charges for additional storage and restore operations. See [Managed Disk Pricing](https://azure.microsoft.com/en-us/pricing/details/managed-disks/?msockid=2e2832779aae6ae900b824af9bc56b5a) for details.

## Learn More and Documentation

- [Instant access documentation](https://learn.microsoft.com/en-us/azure/virtual-machines/disks-instant-access-snapshots?tabs=azure-cli%2Cazure-cli-snapshot-state#snapshots-of-ultra-disks-and-premium-ssd-v2)
- [Azure Snapshot API](https://learn.microsoft.com/en-us/rest/api/compute/snapshots?view=rest-compute-2025-04-01)
- [Azure CLI documentation](https://learn.microsoft.com/en-us/cli/azure/?view=azure-cli-latest)

## Conclusion

Instant access incremental snapshots on Azure Managed Disks offer developers and IT teams a streamlined process for restoring and scaling disk-based workloads, reducing downtime and increasing business agility.

This post appeared first on "The Azure Blog". [Read the entire article here](https://azure.microsoft.com/en-us/blog/instant-access-incremental-snapshots-restore-without-waiting/)
