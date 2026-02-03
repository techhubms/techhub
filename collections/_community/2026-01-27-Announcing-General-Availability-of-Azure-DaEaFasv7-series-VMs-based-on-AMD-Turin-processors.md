---
layout: "post"
title: "Announcing General Availability of Azure Da/Ea/Fasv7-series VMs based on AMD ‘Turin’ processors"
description: "This article details the launch of Azure's newest AMD-based Da/Ea/Fasv7-series Virtual Machines powered by 5th Gen AMD EPYC (Turin) processors. It covers the technical improvements in CPU, memory, and network performance, workload-specific enhancements, and security advancements for these VM series, as well as guidance on use cases, region availability, and new features such as Azure Boost, enhanced storage options, and hardware-based encryption. Customer and partner testimonials illustrate real-world performance benefits across a variety of scenarios."
author: "ArpitaChatterjee"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-compute-blog/announcing-general-availability-of-azure-da-ea-fasv7-series-vms/ba-p/4488627"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-01-27 21:54:30 +00:00
permalink: "/2026-01-27-Announcing-General-Availability-of-Azure-DaEaFasv7-series-VMs-based-on-AMD-Turin-processors.html"
categories: ["Azure"]
tags: ["AMD EPYC Turin", "Azure", "Azure Boost", "Azure Storage", "Azure Virtual Machines", "Cloud Infrastructure", "Community", "Compute Performance", "Constrained Core VM", "Dalsv7", "Dasv7", "Easv7", "Fasv7", "HSM", "Linux", "MANA", "Memory Optimization", "NVMe", "Performance Optimization", "Price Performance", "VM Scalability", "Windows"]
tags_normalized: ["amd epyc turin", "azure", "azure boost", "azure storage", "azure virtual machines", "cloud infrastructure", "community", "compute performance", "constrained core vm", "dalsv7", "dasv7", "easv7", "fasv7", "hsm", "linux", "mana", "memory optimization", "nvme", "performance optimization", "price performance", "vm scalability", "windows"]
---

ArpitaChatterjee announces the general availability of Azure's new AMD 'Turin'-based VM series, highlighting technical advances and use cases for developers, enterprises, and partners.<!--excerpt_end-->

# Announcing General Availability of Azure Da/Ea/Fasv7-series VMs based on AMD ‘Turin’ processors

**Author: ArpitaChatterjee**

## Overview

Microsoft Azure now offers a new class of Virtual Machines (VMs) powered by 5th Generation AMD EPYC™ (Turin) processors, including general-purpose (Dasv7, Dalsv7), memory-optimized (Easv7), and compute-optimized (Fasv7, Falsv7, Famsv7) series. These VMs are designed for a wide range of workloads: from standard web applications and databases to specialized compute-heavy tasks such as AI/ML, in-memory analytics, and scientific simulations.

## Key Advancements

- **Performance:**
  - Up to 35% better CPU performance and price-performance versus prior v6 AMD-based VMs.
  - Workload-specific improvements: up to 25% for Java, 65% for in-memory cache, 80% for cryptography, and 130% for web server applications.
  - Maximum CPU boost up to 4.5 GHz.
- **Scalability:**
  - Dasv7, Dalsv7, Easv7 VMs scale up to 160 vCPUs (previously 96).
  - Fasv7, Falsv7, Famsv7 VMs scale up to 80 vCPUs (previously 64), with new 1-core options.
- **Memory:**
  - Up to 1280 GiB on Easv7-series.
  - Flexible memory-to-vCPU ratios: 2:1, 4:1, and 8:1 models, supporting diverse needs.
- **Storage & Networking:**
  - Up to 20% higher remote disk IOPS, 50% higher throughput, and 55% improvement in local storage throughput.
  - Network performance up by 75% versus D/E-series v6 VMs.
  - Local disk support on new VM series.
- **Platform Technologies:**
  - Azure Boost for enhanced performance and security.
  - Support for **Microsoft Azure Network Adapter** (MANA) and **NVMe** protocol for both local and remote disks.
- **Security:**
  - **AMD Infinity Guard** hardware protection, with Transparent Secure Memory Encryption (TSME).
  - [Azure Integrated HSM](https://aka.ms/AzureIntegratedHSM-preview) available in preview for secure cryptographic key management (FIPS 140-3 Level 3).

## Use Cases and Workloads

- **General purpose:** E-commerce, web front ends, virtualization, CRM, entry/mid-range databases
- **Memory optimized:** Enterprise applications, data warehousing, BI, in-memory analytics
- **Compute optimized:** Scientific simulation, financial modeling, gaming, AI/ML pipelines
- **Lower memory tiers:** Web servers, video encoding, batch processing

## Customer and Partner Testimonials

- **Elastic:** 13% better indexing throughput compared to previous gen Daldsv6 VMs for Elasticsearch.
- **FlashGrid:** Easv7 VM series offer a balanced platform for Oracle Database configurations; 80Gbps network beneficial for Oracle RAC.
- **VertiGIS:** Higher performance, especially for single-threaded, CPU-bound apps (e.g., ArcGIS Enterprise).
- **AMD:** Collaboration delivers improved performance and efficiency for demanding cloud workloads.
- **Ubuntu/SUSE:** Improved experience and stability for Linux workloads; SUSE observed 20%-40% better performance in kernel compilation tasks.

## Availability

- Launch regions: Australia East, Central US, Germany West Central, Japan East, North Europe, South Central US, Southeast Asia, UK South, West Europe, West US 2, and West US 3.
- Large Easv7/Eadsv7 (160 vCPU) sizes: select regions (North Europe, South Central US, West Europe, West US 2).
- For up-to-date info, see [Product Availability by Region](https://aka.ms/ProductAvailabilitybyRegion).

## Resources & Documentation

- [Azure VM series specifications: Dasv7](https://aka.ms/Dasv7-series), [Dadsv7](https://aka.ms/Dadsv7-series), [Dalsv7](https://aka.ms/Dalsv7-series), [Daldsv7](https://aka.ms/Daldsv7-series), [Easv7](https://aka.ms/Easv7-series), [Eadsv7](https://aka.ms/Eadsv7-series), [Fasv7](https://aka.ms/Fasv7-series), [Fadsv7](https://aka.ms/Fadsv7-series), [Falsv7](https://aka.ms/Falsv7-series), [Faldsv7](https://aka.ms/Faldsv7-series), [Famsv7](https://aka.ms/Famsv7-series), [Famdsv7](https://aka.ms/Famdsv7-series), [Constrained-core](https://aka.ms/constrained-coresizes)
- [Azure VM Pricing](https://azure.microsoft.com/pricing/details/virtual-machines)
- [Disk Types](https://learn.microsoft.com/en-us/azure/virtual-machines/disks-types)
- [Azure Boost Overview](https://learn.microsoft.com/en-us/azure/azure-boost/overview)
- [Azure Integrated HSM Preview](https://aka.ms/AzureIntegratedHSM-preview)
- [Azure Support](https://azure.microsoft.com/en-us/support/)

## Security and Hardware Innovations

- 5th Gen AMD EPYC 'Zen 5' supports AVX-512, higher memory bandwidth, improved instructions per clock.
- Azure Integrated HSM enables secure key management (sign up [here](https://aka.ms/AzureIntegratedHSM-preview-signup)).

For feedback or questions, contact [Azure Support](https://azure.microsoft.com/en-us/support/).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-compute-blog/announcing-general-availability-of-azure-da-ea-fasv7-series-vms/ba-p/4488627)
