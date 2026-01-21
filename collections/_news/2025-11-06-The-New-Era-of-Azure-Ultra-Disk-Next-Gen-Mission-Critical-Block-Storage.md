---
external_url: https://azure.microsoft.com/en-us/blog/the-new-era-of-azure-ultra-disk-experience-the-next-generation-of-mission-critical-block-storage/
title: 'The New Era of Azure Ultra Disk: Next-Gen Mission-Critical Block Storage'
author: Aung Oo
feed_name: The Azure Blog
date: 2025-11-06 17:00:00 +00:00
tags:
- Azure Boost
- Azure Ultra Disk
- Backup And Recovery
- Block Storage
- Capacity Management
- Cloud Storage
- Cost Efficiency
- Encryption
- IOPS
- Latency
- Managed Disks
- Mission Critical Workloads
- Performance Optimization
- Snapshot Recovery
- Storage
- Virtual Machines
section_names:
- azure
---
Aung Oo reviews the latest advancements in Azure Ultra Disk, focusing on performance, flexibility, instant access snapshots, and customer success stories for mission-critical business applications.<!--excerpt_end-->

# The New Era of Azure Ultra Disk: Next-Gen Mission-Critical Block Storage

Azure Ultra Disk, introduced at Microsoft Ignite 2019, has become a foundational block storage solution for businesses with demanding operational needs. From high-frequency financial trading and electronic health records to gaming and AI/ML workloads, Ultra Disk stands out for its flexible configuration and high reliability.

## Real-World Impact: BlackRock's Success with Ultra Disk

Global asset manager BlackRock relies on Azure Ultra Disk, paired with M-series Virtual Machines, to support sensitive investment platforms. Ultra-low latency and reliable performance allow them to adapt rapidly in dynamic financial markets.

> "Now that we’re on Azure, we have a springboard to unlock adoption of cloud-managed services to be able to engineer and operate at greater scale and adopt innovative technologies."
>
> — Randall Fradin, Head of Cloud Managed Services and Platform Engineering, BlackRock

[Read the full BlackRock customer story](https://www.microsoft.com/en/customers/story/25275-blackrock-financial-management-azure-ultra-disk-storage#customers-share-modal-dialog)

## Key Innovations in Azure Ultra Disk

### Enhanced Performance

- **80% reduction in P99.9 and outlier latency** and **30% improvement in average latency**.
- Ideal for I/O-intensive and latency-sensitive workloads—such as transaction logs for mission-critical applications.
- Recommended as a better alternative to local SSD or Write Accelerator due to enhanced flexibility and data persistency.

### Cost Efficiency and Flexibility

- New provisioning model allows granular control of both **capacity and IOPS**, optimizing costs for a wide range of workloads.
- **GiB-based billing**, higher maximum IOPS per GiB, and improved minimums.
- Example: A financial app running a core database on Ultra Disk realized 22% cost savings using the new model.

| Capacity      | Previous Cost | Improved Flexibility | Savings   |
|--------------|--------------|---------------------|-----------|
| 12,500 GiB   | $1,594       | $1,497              | -6%       |
| 5,000 IOPS   | $661         | $248                | -62%      |
| 200 MB/s     | $70          | $70                 | No change |
| **Total**    | $2,324       | $1,815              | -22%      |

### High-Performance Workloads with Azure Boost

- **Memory Optimized Mbv3 VM (Standard_M416bs_v3):** Up to 550,000 IOPS and 10GB/s throughput.
- **Azure Boost Ebdsv5 VM:** Up to 400,000 IOPS and 10GB/s throughput.

Future Ignite announcements will introduce even more powerful VM configurations.

### Instant Access Snapshot

- **Instant Access Snapshot** for Ultra and Premium SSD v2 disks, now in public preview.
- Snapshots are immediately usable for new disks—no lengthy background data copy phase.
- Disks hydrate up to 10x faster, enabling rapid recovery and minimal downtime.

## Additional Features for Business Continuity

- **Live Resize:** Expand Ultra Disk storage live without downtime.
- **Dynamic Performance Adjustment:** Tune disk performance to avoid overprovisioning.
- **Encryption at Host:** End-to-end data encryption starting at the VM host.
- **Backup and Restore:** Integrations with Azure Site Recovery, VM Backup, and Disk Backup.
- **Third-party Disaster Recovery Support:** Compatibility with leading backup providers.
- **Shared Disk Capability:** Migrate clusters using SCSI Persistent Reservations for cost-effective high availability.

## Get Started

- [Comprehensive documentation](https://learn.microsoft.com/en-us/azure/virtual-machines/disks-types#ultra-disks)
- [How to deploy Azure Ultra Disk](https://learn.microsoft.com/en-us/azure/virtual-machines/disks-enable-ultra-ssd?tabs=azure-portal)
- For queries, contact [AzureDisks@microsoft.com](mailto:AzureDisks@microsoft.com)

Experience the next generation of Azure Ultra Disk for your most mission-critical workloads.

[Start using Azure Ultra Disk today](https://learn.microsoft.com/en-us/azure/virtual-machines/disks-enable-ultra-ssd?tabs=azure-portal)

This post appeared first on "The Azure Blog". [Read the entire article here](https://azure.microsoft.com/en-us/blog/the-new-era-of-azure-ultra-disk-experience-the-next-generation-of-mission-critical-block-storage/)
