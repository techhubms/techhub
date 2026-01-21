---
external_url: https://techcommunity.microsoft.com/t5/azure-architecture-blog/accelerating-hpc-and-eda-with-powerful-azure-netapp-files/ba-p/4469739
title: Accelerating HPC and EDA Innovation with Azure NetApp Files Enhancements
author: GeertVanTeylingen
feed_name: Microsoft Tech Community
date: 2025-11-14 16:52:12 +00:00
tags:
- AI Integration
- Azure NetApp Files
- Backup And Restore
- Cloud Storage
- Compliance
- Cost Optimization
- Data Management
- Data Migration
- HPC
- Hybrid Cloud
- Machine Learning
- Object Storage
- Performance Optimization
- Quota Reporting
- S3 Compatible
- Volume Scalability
- Well Architected
section_names:
- ai
- azure
- security
---
GeertVanTeylingen and co-authors present a detailed overview of new Azure NetApp Files features for HPC and EDA, highlighting advancements in storage performance, hybrid mobility, security, and AI readiness that support demanding engineering workloads.<!--excerpt_end-->

# Accelerating HPC and EDA with Powerful Azure NetApp Files Enhancements

Azure NetApp Files has been updated with several new features aimed at empowering High-Performance Computing (HPC) and Electronic Design Automation (EDA) users. This article, co-authored by GeertVanTeylingen, Ranga Sankar, and Thomas Willingham, provides a comprehensive overview of the enhancements and their real-world impact on data-intensive workloads.

## Key Innovations for HPC and EDA Workloads

### Most Effective Price/Performance for Any-Size Workload

- **Large volume breakthrough mode:** Supports up to 2 PiB per volume, with dedicated capacity and up to 50 GiB/s throughput. Designed for high IOPS and large distributed teams.
- **Volumes up to 7.2 PiB with cool access:** Consolidate massive datasets efficiently; cool access automatically tiers cold data to lower-cost storage.
- **User and group quota reporting:** Real-time tracking for governance and cost management.

### Best–of–Breed Security and Data Management

- **Backup support for large volumes:** Incremental backups, centralized policies, and compliance-ready features for environments generating hundreds of terabytes.
- **Single file restore from backup:** Quickly recover individual files without rolling back entire volumes, ideal for complex design or audit workflows.

### Hybrid Cloud Data Mobility

- **Cache volumes for burst-to-cloud:** Optimize peak workloads and distributed team collaboration by caching data in cloud regions as needed.
- **Migration assistant:** Enterprise-grade lift-and-shift migration from on-prem or other NetApp solutions, with minimal downtime, full integrity, and flexibility across volume types.

### Machine Learning, Data & AI–Ready

- **Object REST API:** S3-compatible access for modern object storage, enabling seamless integration with Azure AI Search, Databricks, Azure AI Foundry, Fabric, and more. This bridges high-performance engineering storage and cloud-native analytics and AI workflows.

## Why These Advancements Matter

These improvements are tailored to help engineering and science teams:

- **Scale securely:** Consistent throughput, compliance features, and strong data governance.
- **Optimize costs:** Efficient storage tiering, on-demand resource scaling, and reduced migration overhead.
- **Drive operational excellence:** Quota visibility, simplified backups, and repeatable migration processes.
- **Boost performance:** High IOPS for simulations, fast restores, and low-latency data access.
- **Prepare for AI and analytics:** Native integration with Microsoft and third-party tools for advanced analytics, ML, and AI workflows.

## Security and Compliance Highlights

- Dedicated capacity and strong isolation reduce security risks.
- Centralized backup and quota monitoring support regulated industries.
- Single file restores aid compliance and mitigate recovery exposure.

## Further Resources

- [Validating Scalable EDA Storage Performance: Azure NetApp Files and SPECstorage Solution 2020](https://techcommunity.microsoft.com/blog/azurearchitectureblog/validating-scalable-eda-storage-performance-azure-netapp-files-and-specstorage-s/4459517)
- [What's New in Azure NetApp Files](https://learn.microsoft.com/azure/azure-netapp-files/whats-new)
- [Azure NetApp Files Documentation](https://learn.microsoft.com/azure/azure-netapp-files/)
- [Migration Assistant](https://learn.microsoft.com/azure/azure-netapp-files/migrate-volumes?tabs=portal)

---
Azure NetApp Files continues to serve as a backbone for innovation across HPC and EDA workloads, offering scalability, operational excellence, robust backup options, hybrid flexibility, and integration with Azure AI and analytics—all while keeping security and governance at the forefront.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-architecture-blog/accelerating-hpc-and-eda-with-powerful-azure-netapp-files/ba-p/4469739)
