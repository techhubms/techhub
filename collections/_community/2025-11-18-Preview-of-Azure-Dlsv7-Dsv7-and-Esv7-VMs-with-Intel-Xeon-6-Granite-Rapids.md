---
external_url: https://techcommunity.microsoft.com/t5/azure-compute-blog/announcing-preview-of-new-azure-dlsv7-dsv7-and-esv7-vms-based-on/ba-p/4467928
title: Preview of Azure Dlsv7, Dsv7, and Esv7 VMs with Intel Xeon 6 Granite Rapids
author: RishiGomatam
feed_name: Microsoft Tech Community
date: 2025-11-18 16:00:00 +00:00
tags:
- Azure Boost
- Azure VMs
- Business Intelligence
- Data Warehousing
- Dlsv7
- Dsv7
- Esv7
- Granite Rapids
- Intel Xeon 6
- Memory Optimized
- Networking
- NoSQL
- NVMe
- Premium Disk
- Redis
- SAP
- SQL Server
- Ultra Disk
- Vcpu
- VM Preview
section_names:
- azure
primary_section: azure
---
RishiGomatam from Microsoft introduces the preview of Azure's Dlsv7, Dsv7, and Esv7 VM families, highlighting their technical specifications and ideal workload scenarios.<!--excerpt_end-->

# Announcing Preview of Azure Dlsv7, Dsv7, and Esv7 VMs

**Author:** RishiGomatam (Microsoft)

Azure has introduced the next generation of virtual machines: Dlsv7/Dsv7 General Purpose and Esv7 Memory Optimized VMs, powered by Intel® Xeon® 6 (Granite Rapids) processors.

## Key Features & Improvements

- **Performance:** Up to 15% improvement over previous v6 VMs, with Xeon® 6 CPUs (up to 4.2 GHz turbo, 2x memory bandwidth)
- **Scalability:** Dsv7/Esv7 VMs offer up to 372 vCPUs and up to 2.8TiB of memory
- **Networking/Storage:**
  - Up to 400 Gbps networking bandwidth
  - Up to 800,000 IOPS and 20 GBps storage throughput (Premium v2, Ultra Disk)
  - Azure Boost for enhanced remote storage
- **Configurable Memory-to-vCPU Ratios:** Choose from 2:1 (Dlsv7, Dldsv7), 4:1 (Dsv7, Ddsv7), and 8:1 (Esv7, Edsv7)
- **Disk Options:** Both local NVMe temp disk and remote persistent disk configurations (Premium Disk v1/v2, Ultra Disk)

## Workload Suitability

### General Purpose (Dlsv7, Dsv7)

- E-commerce platforms
- Web applications
- Application servers
- Desktop virtualization solutions

### Memory-Optimized (Esv7)

- SQL/NoSQL database servers
- Data warehousing workloads
- Business intelligence apps
- In-memory databases (SAP, Redis)
- In-memory analytics

## Availability

Preview available in **East US 2** region. To request access, fill out the [survey form](https://forms.office.com/r/WKppu6FNDu).

> The new Azure VM families are aimed at users needing scalable, high-performance compute for enterprise applications and demanding analytics workloads.

---
**Author Profile:**

- RishiGomatam – Microsoft
- Joined June 05, 2025

---
**Resource:** [Azure Compute Blog](/category/azure/blog/azurecompute)

---
**Version:** 1.0 (Updated Nov 17, 2025)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-compute-blog/announcing-preview-of-new-azure-dlsv7-dsv7-and-esv7-vms-based-on/ba-p/4467928)
