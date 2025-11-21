---
layout: "post"
title: "Announcing Public Preview of AMLFS 20: New Azure Managed Lustre SKU for AI and HPC"
description: "This post introduces AMLFS Durable Premium 20 (AMLFS 20), a new high-performance SKU in Azure Managed Lustre. Designed for massive-scale AI and HPC workloads, AMLFS 20 enables storing up to 25 PiB in a unified namespace, features multi-MDS architecture for advanced metadata IOPS, and supports up to 20 billion files. The update streamlines deployment for large datasets and accelerates data preparation in demanding research and engineering tasks."
author: "wolfgangdesalvador"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-high-performance-computing/announcing-the-public-preview-of-amlfs-20-azure-managed-lustre/ba-p/4470665"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-18 17:00:00 +00:00
permalink: "/2025-11-18-Announcing-Public-Preview-of-AMLFS-20-New-Azure-Managed-Lustre-SKU-for-AI-and-HPC.html"
categories: ["AI", "Azure", "ML"]
tags: ["AI", "AI Workloads", "AMLFS 20", "Azure", "Azure Managed Lustre", "Cloud Storage", "Community", "Dataset Management", "Filesystem", "High Performance Computing", "HPC", "Large Scale Storage", "Machine Learning", "Metadata Performance", "Microsoft Azure", "ML", "Multi MDS Architecture", "PiB Scale", "Public Preview"]
tags_normalized: ["ai", "ai workloads", "amlfs 20", "azure", "azure managed lustre", "cloud storage", "community", "dataset management", "filesystem", "high performance computing", "hpc", "large scale storage", "machine learning", "metadata performance", "microsoft azure", "ml", "multi mds architecture", "pib scale", "public preview"]
---

Wolfgang De Salvador and the Azure Managed Lustre team announce AMLFS 20, offering massive scale, advanced metadata performance, and simplified management for AI and HPC workloads in Azure.<!--excerpt_end-->

# Announcing the Public Preview of AMLFS 20: Azure Managed Lustre New SKU for Massive AI & HPC Workloads

**Authors:** Sachin Sheth (Principal PDM Manager), Brian Barbisch (Principal Group Software Engineering Manager), Matt White (Principal Group Software Engineering Manager), Brian Lepore (Principal Product Manager), Wolfgang De Salvador (Senior Product Manager), Ron Hogue (Senior Product Manager)

## Introduction

Azure is introducing AMLFS Durable Premium 20 (AMLFS 20), a powerful new SKU in Azure Managed Lustre, tailored to the demands of modern AI and high-performance computing (HPC) workloads.

## Key Features

- **Massive Scale**: Store up to 25 PiB (Pebibytes) of data under a single namespace, eliminating the challenges of managing multiple filesystems.
- **Advanced Metadata Performance**: AMLFS 20 uses a multi-MDS (Metadata Server) architecture, yielding over 5x improvement in metadata IOPS in mdtest benchmarks. For every 5 PiB of provisioned filesystem, an additional MDS is provided, boosting metadata throughput.
- **High File Capacity**: Supports up to 20 billion inodes, enabling expansive namespace management.

## Why AMLFS 20 Matters

- **Simplified Architecture**: Previous limits required complex multi-filesystem management for large datasets. AMLFS 20 allows a single, performant file system for workloads up to 25 PiB, simplifying deployment and ongoing administration.
- **Accelerated Data Preparation**: Multi-MDT architecture increases metadata IOPS, a key requirement for rapid file access during AI training and large-scale data engineering tasks.
- **Faster Time-to-Value**: Researchers and engineers now experience reduced bottlenecks and easier management, gaining faster access to large datasets and expediting innovation cycles.

## Availability

AMLFS 20 is available in Public Preview alongside existing AMLFS SKUs. For deeper insight into throughput configurations and options, refer to the [Azure Managed Lustre documentation](https://learn.microsoft.com/en-us/azure/azure-managed-lustre/create-file-system-portal#throughput-configurations).

## How to Join the Preview

If your workloads demand large-scale AI or HPC capabilities, [fill out this form](https://aka.ms/AMLFS20PreviewForm) to describe your use case and request early access. The Azure Managed Lustre team will follow up with onboarding details for the preview.

*Updated November 18, 2025*

*Version 1.0*

---

**Learn More:**

- [Azure Managed Lustre documentation](https://learn.microsoft.com/en-us/azure/azure-managed-lustre/create-file-system-portal#throughput-configurations)
- [Join the AMLFS 20 Preview](https://aka.ms/AMLFS20PreviewForm)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/announcing-the-public-preview-of-amlfs-20-azure-managed-lustre/ba-p/4470665)
