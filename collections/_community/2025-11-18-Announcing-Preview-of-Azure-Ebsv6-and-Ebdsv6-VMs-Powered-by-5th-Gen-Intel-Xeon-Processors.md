---
external_url: https://techcommunity.microsoft.com/t5/azure-compute-blog/announcing-the-preview-of-the-new-azure-ebsv6-vms-based-on-the/ba-p/4470139
title: Announcing Preview of Azure Ebsv6 and Ebdsv6 VMs Powered by 5th Gen Intel Xeon Processors
author: misha-bansal
feed_name: Microsoft Tech Community
date: 2025-11-18 17:35:49 +00:00
tags:
- Azure Boost
- Azure Virtual Machines
- Cloud Infrastructure
- Data Warehousing
- Ebdsv6
- Ebsv6
- Emerald Rapids
- High Performance Computing
- Intel Xeon
- Memory Intensive Workloads
- NVMe
- OLAP
- OLTP
- Performance Optimization
- Premium SSD V2
- Public Preview
- Ultra Disk
- Virtualization
section_names:
- azure
---
Misha Bansal introduces Azure Ebsv6 and Ebdsv6 VMs, now in public preview, highlighting their enhanced storage, processing, and security features for intensive enterprise workloads.<!--excerpt_end-->

# Announcing Preview of Azure Ebsv6 and Ebdsv6 VMs Powered by 5th Gen Intel Xeon Processors

**Author:** Misha Bansal, Product Manager, Azure Compute

Azure has launched the public preview of the Ebsv6 and Ebdsv6 Virtual Machines, built on the 5th Generation Intel® Xeon® Platinum 8573C (Emerald Rapids) processors. These VMs are engineered for high remote storage throughput and are ideal for organizations dealing with intensive data workloads such as relational databases, large-scale data warehousing, and analytics.

## Key Features

- **Up to 2× higher remote storage performance** compared to Ebsv5 VMs
- **Intel® Xeon® Platinum 8573C CPU**: Delivers 15–30% better CPU performance
- **Memory configurations:** Up to 192 vCPU and 1832 GiB RAM
- **Azure Boost**: Provides up to 800,000 IOPS, 14 GB/s remote disk throughput, and increased IOPS/vCPU
- **Storage:** NVMe interface support for both local and remote disks, flexible configurations, and options for local SSD
- **Networking:** Up to 200 Gbps bandwidth
- **Security:** Enhanced using Intel® Total Memory Encryption (TME) technology

## Technical Specifications (Ebsv6 & Ebdsv6 Series)

| VM Series | vCPU | Memory (GiB) | Uncached Ultra Disk & Premium SSD v2 IOPS | Throughput (MB/s) |
|-----------|------|--------------|------------------------------------------|-------------------|
| Ebsv6/Ebdsv6 | 2–192 | 16–1832 | 13,200–800,000 | 330–14,000 |

Full specifications, including local temp disk options, are available on the [Ebdsv6 documentation page](https://learn.microsoft.com/en-us/azure/virtual-machines/sizes/memory-optimized/ebdsv6-series?tabs=sizebasic).

## Supported Workloads

- Real-time OLTP/OLAP workloads
- Relational database servers
- Data warehousing
- Advanced analytics
- High-performance enterprise workloads requiring low-latency and high-throughput storage

## How to Get Started

- Azure Ebsv6 and Ebdsv6 VMs are available for Public Preview in the US East region.
- Request access using [this survey](https://forms.office.com/pages/responsepage.aspx?id=v4j5cvGGr0GRqy180BHbR2vQUheGFPhKmch5Uj5LYy5UQlhSTjdFM0lCRVBEQjNWMVgzOTNTWFBSTC4u&route=shorturl).
- Learn more with these resources:
  - [Ebsv6-series documentation](https://learn.microsoft.com/en-us/azure/virtual-machines/sizes/memory-optimized/ebsv6-series?tabs=sizebasic)
  - [Ebdsv6-series documentation](https://learn.microsoft.com/en-us/azure/virtual-machines/sizes/memory-optimized/ebdsv6-series?tabs=sizebasic)
  - [Azure Boost overview](https://learn.microsoft.com/en-us/azure/azure-boost/overview)
  - [NVMe FAQ](https://learn.microsoft.com/en-us/azure/virtual-machines/enable-nvme-faqs)
  - [Premium SSD v2](https://learn.microsoft.com/en-us/azure/virtual-machines/disks-types#premium-ssd-v2)
  - [Ultra Disk](https://learn.microsoft.com/en-us/azure/virtual-machines/disks-types#ultra-disk-limitations)

---

For support, contact [Azure Support](https://azure.microsoft.com/en-us/support/).

*Updated Nov 17, 2025 – Version 1.0*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-compute-blog/announcing-the-preview-of-the-new-azure-ebsv6-vms-based-on-the/ba-p/4470139)
