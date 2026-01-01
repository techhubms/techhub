---
layout: "post"
title: "Public Preview: VM Customization in Azure Enables Disabling Multithreading and Constrained Cores"
description: "This announcement introduces the public preview of Azure VM Customization, featuring the ability to disable simultaneous multithreading (SMT/HT) and set constrained vCPU counts. These capabilities provide developers and IT pros with advanced control over CPU configurations, optimizing performance and software licensing costs. The features support high-performance, database, and analytics workloads and are accessible via the Azure portal, ARM templates, Azure CLI, and PowerShell in select regions."
author: "eehindero"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-compute-blog/announcing-public-preview-of-vm-customization-in-azure-disable/ba-p/4462417"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-10-17 21:52:05 +00:00
permalink: "/2025-10-17-Public-Preview-VM-Customization-in-Azure-Enables-Disabling-Multithreading-and-Constrained-Cores.html"
categories: ["Azure"]
tags: ["ARM Templates", "Azure", "Azure CLI", "Bring Your Own License", "Community", "Compliance", "Constrained Cores", "HPC", "Hyperthreading", "Performance Optimization", "PowerShell", "Simultaneous Multithreading", "SMT", "SQL Server", "Vcpu", "Virtual Machines", "VM Customization"]
tags_normalized: ["arm templates", "azure", "azure cli", "bring your own license", "community", "compliance", "constrained cores", "hpc", "hyperthreading", "performance optimization", "powershell", "simultaneous multithreading", "smt", "sql server", "vcpu", "virtual machines", "vm customization"]
---

eehindero announces the public preview of Azure VM Customization, highlighting new support for disabling SMT/hyperthreading and setting constrained core counts, which empowers users to optimize performance and licensing costs.<!--excerpt_end-->

# Public Preview: VM Customization in Azure Enables Disabling Multithreading and Constrained Cores

**Author:** eehindero  
**Published:** Oct 17, 2025

Azure has introduced a new public preview feature called **VM Customization**, which adds two powerful options:

- **Disable Simultaneous Multi-Threading (SMT/HT):** Allows supported VMs to run with one thread per core, granting workloads exclusive access to physical cores. This is ideal for latency-sensitive or performance-critical workloads.
- **Constrained Cores:** Lets users select custom vCPU counts for each VM size, tailored to licensing needs or workload demands, without altering memory, storage, or I/O resources.

## Key Benefits

- **Performance Optimization:** Disable hyperthreading for full-core isolation, which can improve performance consistency for workloads like high-performance computing (HPC), database servers, analytics, and more.
- **Software Licensing Cost Control:** Reduce licensing expenses by deploying high-memory or high-bandwidth VMs with fewer active cores. Especially beneficial for SQL Server, Oracle, SAP, and other BYOL/per-core licensing models.
- **Flexibility:** Match VM resources to specific workload profiles or licensing requirements while maintaining all other VM capabilities.
- **Compliance:** Simplifies adherence to software compliance models that require precise core counts.

## Availability

- Currently in **public preview** in regions: West Central US, North Europe, East Asia, and UK South.
- Accessible via:
  - Azure Portal
  - ARM Templates
  - Azure CLI
  - PowerShell
- Only first-party OS images are supported; marketplace images with third-party licensing are not supported.

## How to Join the Preview

To participate, fill out the survey form [here](https://forms.office.com/r/JjMivfn8bS).

## Example Scenarios

- **Database Optimization:** Deploy a SQL Server VM with high memory and I/O bandwidth but only the exact number of vCPUs matching license entitlements.
- **High-Performance Computing:** Assign physical cores (with SMT/HT off) to improve consistency and reduce latency.
- **Cost Optimization for Enterprises:** Utilize constrained core counts to avoid over-provisioning and save on licensing costs.

## Additional Notes

- These features deliver deeper customization for various enterprise workloads in Azure, empowering technical teams to optimize infrastructure and budgets without compromising on capability.
- More information can be found in the [Azure Compute Blog](/category/azure/blog/azurecompute).

---

*For detailed setup instructions and supported VM series, visit the official Azure documentation or follow the Azure Compute Blog for updates.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-compute-blog/announcing-public-preview-of-vm-customization-in-azure-disable/ba-p/4462417)
