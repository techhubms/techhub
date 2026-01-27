---
external_url: https://techcommunity.microsoft.com/t5/azure-compute-blog/announcing-general-availability-of-azure-d192-sizes-in-the-azure/ba-p/4451427
title: Azure D192 Sizes Now Available in Dsv6 and Ddsv6-Series VMs
author: sarah-zhou
feed_name: Microsoft Tech Community
date: 2025-09-08 23:49:26 +00:00
tags:
- Azure Boost
- Azure Ddsv6 Series
- Azure Dsv6 Series
- Azure Pricing
- Cloud Infrastructure
- D192 VM Size
- Data Center
- Enterprise Workloads
- in Memory Analytics
- Intel Xeon Platinum 8573C
- Large Databases
- Managed Disks
- NVMe Storage
- Regional Availability
- SAP
- SQL Server
- Total Memory Encryption
- Virtual Machines
- Virtualization
section_names:
- azure
primary_section: azure
---
Sarah Zhou introduces the availability of Azure D192 VM sizes in the Dsv6 and Ddsv6-series, outlining their technical specs, performance capabilities, and applicability for enterprise workloads.<!--excerpt_end-->

# Azure D192 Sizes Now Available in Dsv6 and Ddsv6-Series VMs

*Authored by Sarah Zhou, Product Manager*

## Overview

Azure has added the D192 size to its Dsv6 and Ddsv6-series Virtual Machine families. These new VM sizes are powered by the 5th Generation Intel® Xeon® Platinum 8573C (Emerald Rapids) processor and are available in many Azure regions.

- **Dsv6-series:** Uses Azure managed disks only.
- **Ddsv6-series:** Offers local, temporary storage disks.

## Use Cases

The D192 VM sizes are designed for general-purpose and high-performance workloads, such as:

- Enterprise applications: SAP, SQL Server, in-memory analytics platforms, and large relational databases
- Web and application servers (moderate to heavy traffic)
- Batch processing tasks
- Developer and test environments
- Other CPU/memory intensive applications

## Key Features

- **192 vCPUs and 768 GiB RAM:** Meets demanding compute and memory requirements.
- **Azure Boost:** Up to 400K IOPS, 12 GB/s remote storage throughput, and up to 82 Gbps network bandwidth. More on [Azure Boost](https://learn.microsoft.com/en-us/azure/azure-boost/overview?toc=%2Fazure%2Fvirtual-machines%2Ftoc.json&bc=%2Fazure%2Fvirtual-machines%2Fbreadcrumb%2Ftoc.json).
- **NVMe Interface:** Provides significant improvement in local storage IOPS (3x performance for low-latency access).
- **Intel® Total Memory Encryption (TME):** Hardware-level data protection for system memory.

## Specifications

| VM Family       | Size                | vCPU | Memory (GiB) | Local Disk (GiB) | Max Data Disks | Network Gbps |
|-----------------|---------------------|------|--------------|------------------|----------------|--------------|
| Dsv6-Series     | Standard_D192s_v6   | 192  | 768          | N/A              | 64             | 82           |
| Ddsv6-Series    | Standard_D192ds_v6  | 192  | 768          | 6x1760           | 64             | 82           |

For complete specs, visit the [Dsv6](https://learn.microsoft.com/en-us/azure/virtual-machines/sizes/general-purpose/dsv6-series?tabs=sizebasic) and [Ddsv6](https://learn.microsoft.com/en-us/azure/virtual-machines/sizes/general-purpose/ddsv6-series?tabs=sizebasic) documentation.

## Regional Availability

D192 VM sizes are broadly available in Azure regions including, but not limited to: Southeast Asia, Australia East, USGov Virginia, Brazil South, Canada Central, China North 3, North Europe, West Europe, France Central, Germany West Central, Central India, Israel Central, Italy North, Japan East, Korea Central, Norway East, Poland Central, South Africa North, Spain Central, Sweden Central, Switzerland North, Taiwan North, UAE North, UK South, Central US, East US, East US 2, North Central US, West US, West US 2, West US 3. For the latest availability, see the [Product Availability by Region](https://aka.ms/FXv2-series_ProductByRegion).

## Pricing

Learn more about pricing at the [Azure Virtual Machines pricing](https://azure.microsoft.com/pricing/details/virtual-machines) page. Information about disk types (Standard SSD/HDD, Premium SSD, Ultra Disk) can be found in the [Azure managed disk type](https://learn.microsoft.com/en-us/azure/virtual-machines/disks-types) documentation.

## Getting Started

The new D192 sizes are available in the Azure Portal in supported regions. Further details on Dsv6, Ddsv6, Dlsv6, Dldsv6, Esv6, and Edsv6 VMs can be found in the [Microsoft Community Hub announcement](https://techcommunity.microsoft.com/blog/azurecompute/announcing-general-availability-of-azure-dlde-v6-vms-powered-by-intel-emr-proces/4376186).

---
**Author:** Sarah Zhou

For more updates, visit the Azure Compute Blog or follow Sarah Zhou's [profile](/users/sarah-zhou/3152986).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-compute-blog/announcing-general-availability-of-azure-d192-sizes-in-the-azure/ba-p/4451427)
