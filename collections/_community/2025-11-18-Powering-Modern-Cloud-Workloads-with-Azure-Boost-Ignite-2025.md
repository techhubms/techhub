---
layout: post
title: 'Powering Modern Cloud Workloads with Azure Boost: Ignite 2025'
author: Max_Uritsky
canonical_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/powering-modern-cloud-workloads-with-azure-boost-ignite-2025/ba-p/4470793
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-18 16:00:00 +00:00
permalink: /azure/community/Powering-Modern-Cloud-Workloads-with-Azure-Boost-Ignite-2025
tags:
- ABCD
- Active Active Resiliency
- Attestation
- Azure Boost
- Azure Infrastructure
- Azure VM
- Cloud Infrastructure
- Confidential Computing
- Enterprise Workloads
- High Throughput
- Ignite
- IOPS
- MANA Drivers
- Networking
- PCIe Encryption
- Performance
- RDMA
- TDISP
- Virtualization
section_names:
- azure
- security
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
