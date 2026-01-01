---
layout: "post"
title: "Powering Modern Cloud Workloads with Azure Boost: Ignite 2025"
description: "This post details the next generation of Azure Boost, unveiled at Ignite 2025. Azure Boost introduces hardware/software offloading for virtualization workloads, offering major performance gains, improved networking, and advanced security via confidential computing. The update covers features like ABCD (Azure Boost Confidential Device), support for up to 1M IOPS and 400Gbps networking, integration with attested hardware using PCIe encryption, and recommendations for optimal VM driver configurations to maximize reliability and scale for demanding workloads including AI, analytics, and enterprise apps."
author: "Max_Uritsky"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/powering-modern-cloud-workloads-with-azure-boost-ignite-2025/ba-p/4470793"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-18 16:00:00 +00:00
permalink: "/2025-11-18-Powering-Modern-Cloud-Workloads-with-Azure-Boost-Ignite-2025.html"
categories: ["Azure", "Security"]
tags: ["ABCD", "Active Active Resiliency", "Attestation", "Azure", "Azure Boost", "Azure Infrastructure", "Azure VM", "Cloud Infrastructure", "Community", "Confidential Computing", "Enterprise Workloads", "High Throughput", "Ignite", "IOPS", "MANA Drivers", "Networking", "PCIe Encryption", "Performance", "RDMA", "Security", "TDISP", "Virtualization"]
tags_normalized: ["abcd", "active active resiliency", "attestation", "azure", "azure boost", "azure infrastructure", "azure vm", "cloud infrastructure", "community", "confidential computing", "enterprise workloads", "high throughput", "ignite", "iops", "mana drivers", "networking", "pcie encryption", "performance", "rdma", "security", "tdisp", "virtualization"]
---

Max_Uritsky introduces Azure Boost at Ignite 2025—exploring new hardware-driven virtualization, enhanced networking, and advanced security with confidential computing for Azure VMs.<!--excerpt_end-->

# Powering Modern Cloud Workloads with Azure Boost: Ignite 2025

Azure Boost takes virtualization in Microsoft Azure to a new level by offloading key processes from the host to dedicated hardware and software, resulting in higher performance, stronger workload isolation, and increased operational flexibility. Announced at Ignite 2025, this update is now available in preview on v7‑series VMs, with broader VM SKU coverage planned into 2026.

## Key Performance Enhancements

- **1M IOPS and 20 GB/s Remote Storage Throughput:** Supports intensive workloads requiring rapid data access and reliability for Azure migrations.
- **400 Gbps Networking:** Available for both general purpose and AI/HPC VMs, doubling last generation’s networking capabilities.
- **Dual TOR Architecture:** Delivers active/active resiliency for sub-second network failover and maintenance.
- **400,000 Connections per Second:** For network-optimized Dln-, Dn-, and Ensv6-series VM SKUs, enabling scalable deployment in high-demand environments.
- **Recommended Microsoft Azure Network Adapter (MANA):** Ensures optimal driver performance for Boost-enabled hardware ([MANA driver download](https://aka.ms/mana)).

## Advanced Networking for AI and HPC Workloads

- **Remote Direct Memory Access (RDMA):** Moves data with low latency for distributed and global AI training.
- **Cross-region RDMA:** Secures multi-path connections and erasure-coded RDMA for error recovery and robust AI training over global networks.

## Security and Confidential Computing with ABCD

- **Azure Boost Confidential Device (ABCD):** Securely offloads file I/O operations in confidential VMs; leverages hardware-accelerated encryption and PCIe link encryption.
- **Direct Secure I/O:** Eliminates bounce-buffer copies, reducing CPU usage and latency, increasing throughput without sacrificing security.
- **TDISP Standard Integration:** Uses IDE-encrypted PCIe connections and attested hardware to maintain confidentiality and integrity of VM data.
- **Benchmarks:** Show near general-purpose VM performance while maintaining strict security boundaries within the Trusted Compute Base (TCB).

## Implementation Details

- Traditional confidential computing requires bounce buffers to transfer encrypted data. ABCD integrates attested hardware allowing direct secure links via TDISP, minimizing encryption/decryption cycles. This unlocks higher VM throughput for networking and storage while giving more vCPU headroom for application workloads.
- Strong recommendation: Keep VM drivers (MANA) up to date to leverage latest Boost improvements.

## Getting Started & Resources

- [Azure Boost overview](https://aka.ms/azureboost)
- [MANA drivers](https://aka.ms/mana)
- [Ignite session: Powering modern cloud workloads with Azure Compute](https://ignite.microsoft.com/en-US/sessions/BRK172?source=sessions)
- [Ebsv6-Series VMs preview](https://aka.ms/Ebsv6-preview-blog)

## Conclusion

Azure’s latest advances with Boost provide unmatched scalability, performance, and security for demanding cloud workloads—from databases, analytics, and AI to confidential enterprise applications.

*Updated Nov 18, 2025 | Author: Max_Uritsky*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/powering-modern-cloud-workloads-with-azure-boost-ignite-2025/ba-p/4470793)
