---
layout: post
title: Announcing Preview of New Azure Dasv7, Easv7, and Fasv7-Series VMs Based on 5th Gen AMD EPYC™ ‘Turin’ Processors
author: ArpitaChatterjee
canonical_url: https://techcommunity.microsoft.com/t5/azure-compute-blog/announcing-preview-of-new-azure-dasv7-easv7-fasv7-series-vms/ba-p/4448360
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-09-09 13:36:43 +00:00
permalink: /azure/community/Announcing-Preview-of-New-Azure-Dasv7-Easv7-and-Fasv7-Series-VMs-Based-on-5th-Gen-AMD-EPYC-Turin-Processors
tags:
- AMD EPYC
- Azure Boost
- Azure Disk Storage
- Azure HSM
- Azure Virtual Machines
- Cloud Infrastructure
- Compute Optimization
- Dasv7
- Easv7
- Fasv7
- MANA
- Memory Optimization
- NVMe
- Turin
- Virtualization
- VM Performance
section_names:
- azure
---
ArpitaChatterjee details Microsoft's preview launch of high-performance Azure Virtual Machines using 5th Gen AMD EPYC™ processors, highlighting advanced scaling, storage, and security features for demanding workloads.<!--excerpt_end-->

# Announcing Preview of New Azure Dasv7, Easv7, and Fasv7-Series VMs Based on 5th Gen AMD EPYC™ ‘Turin’ Processors

Microsoft has announced the preview of new Azure AMD-based Virtual Machines (VMs), leveraging the 5th Generation AMD EPYC™ (Turin) processors. These new VMs cater to a wide range of workloads and provide significant advancements in performance, scalability, and security.

## Key Highlights

- **VM Series in Preview:**
  - General Purpose: Dasv7, Dalsv7
  - Memory-Optimized: Easv7
  - Compute-Optimized: Fasv7, Falsv7, Famsv7
  - New Families: Fadsv7, Faldsv7, Famdsv7 (with local disk support)

- **Available Azure Regions:**
  - East US 2, North Europe, West US 3

- **Technical Improvements:**
  - Up to 35% CPU performance improvement versus v6 AMD-based VMs
  - Up to 130% performance boost for web server applications
  - Boost CPU frequency up to 4.5 GHz
  - Up to 160 vCPUs (Dasv7/Dalsv7/Easv7); Fasv7 supports up to 80 vCPUs, including a new 1-core option
  - Dasv7 supports up to 640 GiB memory, Easv7 up to 1280 GiB
  - Enhanced remote storage: up to 20% more IOPS, 50% higher throughput
  - Support for NVMe protocol on local and remote disks

- **Optimized for Specific Workloads:**
  - Memory-intensive computing, in-memory analytics, web servers, databases, scientific simulations, financial analytics, and more
  - Choice of memory-to-vCPU ratios: 2:1, 4:1, 8:1 depending on series
  - No SMT on Fasv7, Falsv7, Famsv7: 1 vCPU = 1 core for peak CPU workloads
  - Constrained-core offerings for optimized licensing costs

- **Security and Advanced Features:**
  - [Azure Boost](https://learn.microsoft.com/en-us/azure/azure-boost/overview) for performance and security enhancements
  - Microsoft Azure Network Adapter ([MANA](https://aka.ms/manadocs)) for improved networking and future compatibility
  - Support for [NVMe](https://aka.ms/NVMeFAQ) storage
  - AMD Infinity Guard with Transparent Secure Memory Encryption (TSME)
  - Integration with upcoming [Azure Integrated HSM](https://aka.ms/AzureIntegratedHSM) for secure key management (preview soon)

- **Preview Sign-Up and Pricing:**
  - To access the preview, fill out the [Preview-Signup form](https://aka.ms/AMDv7_PublicPreview_Signup)
  - These VMs will incur charges during preview; pricing will be shared upon access
  - New customers can register for a [free Azure account](https://azure.microsoft.com/free/)

## Workload Suitability

- **Dasv7-Series:** General compute workloads: web applications, e-commerce, virtual desktops, and mid-range databases.
- **Easv7-Series:** For memory-intensive tasks: data warehouses, business intelligence, in-memory analytics.
- **Fasv7, Falsv7, Famsv7-Series:** For compute-heavy tasks: simulations, modeling, gaming, and advanced analytics.
- **Constrained-Core Sizing:** Flexible configurations available for optimized licensing and workload fit.

## Getting Started

- Learn more on: [Dasv7-series](https://aka.ms/Dasv7-series), [Easv7-series](https://aka.ms/Easv7-series), [Fasv7-series](https://aka.ms/Fasv7-series), and other linked specification pages.
- Full disk type info: [Azure managed disk type](https://learn.microsoft.com/en-us/azure/virtual-machines/disks-types)
- Questions? Refer to [Azure Support](https://azure.microsoft.com/en-us/support/).

## Security and Compliance

- Azure Integrated HSM (preview soon) offers FIPS 140-3 Level 3 compliance for secure cryptographic key management within VMs.
- Continued support for TSME and hardware-based security with AMD Infinity Guard.

---
These new Azure AMD-based VMs present an advanced platform for customers who need both performance and flexibility, supporting everything from general web workloads to high-end analytics and simulations. For preview access and more details, use the provided signup form links.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-compute-blog/announcing-preview-of-new-azure-dasv7-easv7-fasv7-series-vms/ba-p/4448360)
