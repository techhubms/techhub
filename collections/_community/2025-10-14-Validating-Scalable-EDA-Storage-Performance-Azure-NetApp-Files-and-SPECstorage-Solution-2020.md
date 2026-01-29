---
external_url: https://techcommunity.microsoft.com/t5/azure-architecture-blog/validating-scalable-eda-storage-performance-azure-netapp-files/ba-p/4459517
title: 'Validating Scalable EDA Storage Performance: Azure NetApp Files and SPECstorage Solution 2020'
author: GeertVanTeylingen
feed_name: Microsoft Tech Community
date: 2025-10-14 01:37:14 +00:00
tags:
- Azure Architecture
- Azure NetApp Files
- Benchmarking
- Cloud File Storage
- Cloud Storage
- Data Management
- Electronic Design Automation
- Global Collaboration
- High Performance Computing
- HPC
- Hybrid Cloud
- IaC
- Latency Optimization
- Performance Engineering
- Resilience
- Scalability
- SPECstorage Solution
- Storage Optimization
- Virtual Networks
- Azure
- DevOps
- Community
section_names:
- azure
- devops
primary_section: azure
---
GeertVanTeylingen presents a thorough analysis of how Azure NetApp Files powers electronic design automation (EDA) with scalable, high-performance, and globally distributed cloud storage, validated via SPECstorage Solution 2020 benchmarks.<!--excerpt_end-->

# Validating Scalable EDA Storage Performance: Azure NetApp Files and SPECstorage Solution 2020

Electronic Design Automation (EDA) workloads are at the heart of semiconductor innovation, requiring storage that can keep pace with massive compute demands and vast datasets. Azure NetApp Files is engineered to meet these needs, enabling engineering teams to manage complex simulations and accelerate time-to-market with a fully managed, scalable, and high-performance solution.

## Why Azure NetApp Files for EDA?

Azure NetApp Files is an Azure-native, enterprise-grade file storage service optimized for cloud-based EDA workflows. It offers:

- On-premises-level throughput and sub-millisecond latency
- Seamless global availability (over 45 Azure regions)
- Simple deployment and management through the Azure portal and automation tools such as Terraform/CLI
- End-to-end security and compliance features, including replication, on-disk encryption, snapshots, and access controls

Microsoft's commitment is demonstrated in the independent validation of Azure NetApp Files using the SPECstorage® Solution 2020 EDA_BLENDED benchmark—the industry standard for measuring storage performance in silicon design environments.

## Unmatched Performance and Scale

Azure NetApp Files enables:

- Single large volumes delivering close to 12 GiB/s throughput and hundreds of thousands of operations per second at sub-millisecond latency
- Horizontal scalability via multiple parallel volumes, supporting tens of thousands of compute cores for large simulation clusters
- Up to 2 PiB per volume in a single namespace, simplifying access to enormous datasets

## Cost and Performance Optimization

Engineering teams can tune Azure NetApp Files for their specific workloads:

- **Flexible Service Level (FSL):** Independently scale storage capacity and bandwidth, ideal for bursty or archival EDA workloads
- **Cool Access:** Automatically offloads infrequently accessed data, reducing costs for archived datasets
- **Dynamic Scalability:** Resize or switch service levels on the fly without disruption
- **Optimized Metadata Handling:** Essential for EDA’s metadata-intensive operations, ensuring frontend design tools and version control stay responsive

## Global Collaboration and 24×7 Productivity

- Azure NetApp Files supports cross-region replication and global VNet peering
- Keeps geographically distributed teams synchronized and productive
- Enables true 24/7 “follow-the-sun” workflows vital to modern chip design

## Operational Simplicity and Reliability

- Fully managed: eliminates specialized storage administration
- Unified Azure billing, monitoring, and role-based access control
- 99.99% availability SLA
- Backed by NetApp® ONTAP® technology for proven enterprise resilience

## Key Capabilities and Benefits: At a Glance

| Capability | Benefit |
| --- | --- |
| Volumes as a Service | Agile provisioning, pay for what you use |
| Extreme Performance | Support for massive, I/O-intensive EDA simulations |
| Massive Single-Volume Capacity | Easy management of huge design datasets |
| Linear Scale-Out | Unlimited aggregate performance for any HPC cluster size |
| Global Deployment | Resilience and immediate team access worldwide |
| Standard Protocols | NFS, SMB, and REST API support for frictionless migrations |
| Data Management Features | Snapshots, clones, encryption, and access controls for safety and efficiency |
| Multiple Service Levels | Align cost and performance to each project phase |
| Dynamic Scalability | Resize and re-tier storage to match workload needs |
| MetaData Optimized | Keeps tooling and version control snappy, even with millions of files |

## Validating with SPECstorage® Solution 2020: Real-World Benchmarks

- Azure NetApp Files was subjected to SPECstorage® Solution 2020 EDA_BLENDED benchmark testing, consisting of both single volume and multi-volume (scale-out) scenarios.
- Results:
  - **Single Large Volume:** 1,760 EDA_BLENDED JOBS, 12,780 MiB/s throughput, 792,046 ops/sec, 0.48 ms ORT
  - **Six Large Volumes:** 10,560 EDA_BLENDED JOBS, 76,487 MB/s throughput, 4,745,453 ops/sec, 0.64 ms ORT
- Linear scalability was observed: performance increased directly with more volumes added, and latency remained under 1 ms even at peak load.

### Architecture and Testbed

- Ten RHEL 9.5 workload clients (16 Gbps each) per volume, using Azure Virtual Networks
- Multi-region and multi-availability-zone configuration for scale testing
- Full test details and diagrams are provided in [official Microsoft documentation](https://learn.microsoft.com/azure/azure-netapp-files/solutions-benefits-azure-netapp-files-electronic-design-automation)

## The Bottom Line: What This Means for EDA Teams

- Run large chip design projects on Azure with confidence that storage won’t become the bottleneck
- Scale out as needed for large tape-out events without costly overprovisioning
- Lower TCO due to elasticity, automation, and integrated cost controls
- Leverage global collaboration features for distributed engineering teams
- Rely on published, vendor-neutral benchmarks for transparency and due diligence

## Additional Resources

- [What is Azure NetApp Files](https://learn.microsoft.com/en-us/azure/azure-netapp-files/azure-netapp-files-introduction)
- [SPECstorage® Solution 2020](https://www.spec.org/storage2020/)
- [Azure NetApp Files EDA Solution Benefits](https://learn.microsoft.com/azure/azure-netapp-files/solutions-benefits-azure-netapp-files-electronic-design-automation)
- [Published Benchmark Results](https://www.spec.org/storage2020/results/eda_blended/)
- [Azure NetApp Files: Revolutionizing silicon design for HPC](https://azure.microsoft.com/blog/azure-netapp-files-revolutionizing-silicon-design-for-high-performance-computing/)

---

*Author: GeertVanTeylingen, Azure Architecture Blog (Microsoft)*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-architecture-blog/validating-scalable-eda-storage-performance-azure-netapp-files/ba-p/4459517)
