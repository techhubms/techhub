---
external_url: https://techcommunity.microsoft.com/t5/azure-storage-blog/priority-replication-for-geo-redundant-storage-and-object/ba-p/4468607
title: Priority Replication Now Generally Available for Azure GRS and Object Replication
author: imanihall
feed_name: Microsoft Tech Community
date: 2025-11-10 20:21:17 +00:00
tags:
- Azure Portal
- Azure Storage
- Blob Storage
- Block Blob
- Data Durability
- Disaster Recovery
- Geo Priority Replication
- Geo Redundant Storage
- GRS
- GZRS
- Object Replication
- Replication Metrics
- SLA
section_names:
- azure
primary_section: azure
---
imanihall announces general availability of priority replication for Azure GRS and Object Replication, detailing SLA guarantees and new monitoring tools for accelerated and reliable cross-region replication.<!--excerpt_end-->

# Priority Replication for Geo Redundant Storage and Object Replication is Generally Available

**Author:** imanihall

## Overview

Azure Storage now offers Priority Replication for Geo Redundant Storage (GRS), Geo-Zone-Redundant Storage (GZRS), and Object Replication (OR), providing guaranteed 15-minute synchronization between regions for Block Blob data, backed by a Service Level Agreement (SLA). This advancement helps ensure better data durability and disaster recovery capabilities for applications requiring cross-region replication.

## Key Highlights

- **Geo Priority Replication** offers an SLA-backed guarantee that the Last Sync Time (LST) of Block Blob data in GRS/GZRS accounts will be 15 minutes or less 99% of each billing month.
- **Object Replication Priority Replication** ensures 99% of operations are replicated across accounts within 15 minutes on the same continent, also SLA-backed.
- **Supported SKUs:** GRS, RA-GRS, GZRS, RA-GZRS for Block Blob data.
- Enhanced **monitoring via Azure Portal**, including new metrics such as Geo Blob Lag, Pending Operations, and Pending Bytes.

## What is Geo Priority Replication?

- **Standard GRS/GZRS:** Data is asynchronously replicated from the primary to secondary region, causing potential data loss if primary fails unexpectedly.
- **Last Sync Time (LST):** Indicates the most recent time data is guaranteed to be in the secondary region—essential for recovery point objectives (RPO).
- **With Priority Replication:** Accelerated process ensures LST/RPO for Block Blob data meets the SLA, helping mitigate data loss fears and increasing resilience.
- **More Information:** [Azure Storage Geo Priority Replication Documentation](https://learn.microsoft.com/azure/storage/common/storage-redundancy-priority-replication?toc=%2Fazure%2Fstorage%2Fblobs%2Ftoc.json&bc=%2Fazure%2Fstorage%2Fblobs%2Fbreadcrumb%2Ftoc.json&tabs=portal#enable-and-disable-geo-redundant-storage-replication)

## What is Object Replication Priority Replication?

- **Object Replication:** Asynchronous copy of operations from a source to one or more destination storage accounts under a replication policy (OR policy).
- **With Priority Replication:** Completion time is SLA-guaranteed within 15 minutes (for 99% of operations in the billing month) for accounts in the same continent.
- **Eligibility details:** [See official SLA Terms](https://www.microsoft.com/licensing/docs/view/Service-Level-Agreements-SLA-for-Online-Services)

## Monitoring SLA Compliance

### Geo Priority Replication

- **Geo Blob Lag:** Metric to track seconds since last full data synchronization; staying within 900 seconds (15 minutes) indicates SLA compliance.
- Viewable in Azure Portal. For more: [Geo Blob Lag Metric](https://learn.microsoft.com/azure/storage/common/storage-redundancy-priority-replication?toc=%2Fazure%2Fstorage%2Fblobs%2Ftoc.json&bc=%2Fazure%2Fstorage%2Fblobs%2Fbreadcrumb%2Ftoc.json&tabs=portal#monitor-compliance)

### Object Replication Priority Replication

- **Pending Operations:** Tracks number of operations pending replication.
- **Pending Bytes:** Tracks total volume of data pending replication.
- Metrics are grouped into time buckets like 0-5 min, 10-15 min, and >24 hours to help users assess SLA compliance.
- Azure Portal experience helps monitor and troubleshoot delays.
- Replication status of individual blobs can be checked to confirm completeness.

## Getting Started

- **Enabling Geo Priority Replication:** [Step-by-step instructions](https://learn.microsoft.com/azure/storage/common/storage-redundancy-priority-replication?toc=%2Fazure%2Fstorage%2Fblobs%2Ftoc.json&bc=%2Fazure%2Fstorage%2Fblobs%2Fbreadcrumb%2Ftoc.json&tabs=portal#enable-and-disable-geo-redundant-storage-replication)
- **Enabling OR Priority Replication:** [Step-by-step instructions](https://learn.microsoft.com/en-us/azure/storage/blobs/object-replication-priority-replication?tabs=portal)

## Feedback & Support

Email questions or feedback: [priorityreplication@microsoft.com](mailto:priorityreplication@microsoft.com)

---

**Updated:** Nov 10, 2025  
**Version:** 1.0

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-storage-blog/priority-replication-for-geo-redundant-storage-and-object/ba-p/4468607)
