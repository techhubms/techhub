---
external_url: https://techcommunity.microsoft.com/t5/azure-storage-blog/understanding-the-standard-hdd-i-o-unit-size-update-and-what-it/ba-p/4499128
title: Understanding the Standard HDD I/O Unit Size Update in Azure
author: austin-ma
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-03-10 18:00:00 +00:00
tags:
- Azure
- Azure Disks
- Azure Pricing
- Azure Storage
- Billing Update
- Capacity Planning
- Community
- Cost Control
- Disk I/O
- Managed Disks
- Performance Optimization
- S4 Disk
- S6 Disk
- S70 Disk
- S80 Disk
- Standard HDD
- Transaction Billing
section_names:
- azure
---
austin-ma discusses the new changes to transaction measurement for Azure Standard HDD managed disks, including updated I/O unit sizes, billing methods, and advice for optimizing your disk selection.<!--excerpt_end-->

# Understanding the Standard HDD I/O Unit Size Update in Azure

Azure has updated how transactions are measured and billed for Standard HDD managed disks. This update refines the billing model to better match actual I/O patterns, giving organizations more accurate cost predictability.

## What's Changing?

- **Billable transactions** for select HDD disk sizes (S4, S6, S70, S80) are now based on 16 KiB I/O units.
  - I/O operations ≤ 16 KiB: Count as one transaction.
  - I/O operations > 16 KiB: Counted as multiple transactions (rounded up).

**Example:**

- A 64 KiB read or write = 4 billable transactions on an S4 disk.
- A 4 KiB read or write = 1 billable transaction.

[Read the official documentation on Standard HDD Managed Disks](https://learn.microsoft.com/en-us/azure/virtual-machines/disks-understand-billing#standard-hdd-transactions).

## Built-in Transaction Caps

To ensure costs are predictable, Azure enforces transaction caps per hour for each disk size:

| Disk Size | Disk GiB | Hourly Cap |
|-----------|----------|------------|
| S4        | 32 GiB   | 450,000    |
| S6        | 64 GiB   | 858,000    |
| S70       | 16 TiB   | 93,000,000 |
| S80       | 32 TiB   | 110,000,000|

If you exceed your disk's hourly cap, additional transactions in that hour are not billed.

## Large Disk High-Throughput Protection

On S70 and S80 disks, each I/O is split into 16 KiB units, **but** there is a per-I/O cap of 16 transactions. This keeps transaction costs reasonable for large, sequential operations.

**Example:**

- 256 KiB read/write on S70 → billed as 16 transactions.
- 1 MiB write on S70 → also billed as 16 transactions (not 64).

## Cost Impact Scenarios

The update may affect costs depending on your workload's I/O pattern:

- **Large I/O or sustained IOPS workloads** may see higher transaction counts.
- **Small/infrequent I/O workloads** are likely unaffected.

**Sample Calculation Table:**

| Workload Avg | Std HDD Billable Tx/hr | Std SSD Billable Tx/hr | Std HDD Total Cost | Std SSD Total Cost |
|--------------|------------------------|------------------------|--------------------|--------------------|
| 4 KiB, 10 IOPS| 36,000                 | 36,000                 | $2.85/mo           | $7.66/mo           |
| 32 KiB, 20 IOPS| 144,000               | 43,400                 | $6.80/mo           | $8.73/mo           |
| 64 KiB, 40 IOPS| 450,000 (capped)      | 43,400 (capped)        | $17.97/mo          | $8.73/mo           |

*Any storage operation (read, write, delete) is billable. Host caching may change the billable transaction count.*

## Choosing the Right Disk

- **OS Disks**: Standard HDD OS disks are being retired in September 2028. Consider migrating to Standard SSD or Premium SSD.
- **Non-OS workloads**: Premium SSD v2 or Standard SSD disks offer greater performance for demanding scenarios.

[Compare Azure disk types and their benefits](https://learn.microsoft.com/en-us/azure/virtual-machines/disks-types)

## Additional Resources

- [Standard HDD Managed Disks documentation](https://learn.microsoft.com/en-us/azure/virtual-machines/disks-understand-billing#standard-hdd)
- [Azure Disks Pricing Page](https://azure.microsoft.com/en-us/pricing/details/managed-disks/#pricing)

## Author

_austin-ma_

*Updated Mar 09, 2026. Version 1.0*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-storage-blog/understanding-the-standard-hdd-i-o-unit-size-update-and-what-it/ba-p/4499128)
