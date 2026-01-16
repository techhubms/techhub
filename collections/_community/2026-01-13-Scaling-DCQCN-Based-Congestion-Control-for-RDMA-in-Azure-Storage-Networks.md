---
layout: post
title: Scaling DCQCN-Based Congestion Control for RDMA in Azure Storage Networks
author: VamsiVadlamuri
canonical_url: https://techcommunity.microsoft.com/t5/azure-networking-blog/data-center-quantized-congestion-notification-scaling-congestion/ba-p/4468417
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2026-01-13 22:35:21 +00:00
permalink: /azure/community/Scaling-DCQCN-Based-Congestion-Control-for-RDMA-in-Azure-Storage-Networks
tags:
- Azure
- Cloud Networking
- Cloud Storage
- Community
- Congestion Control
- Data Center
- DCQCN
- ECN
- Latency
- Network Interface Card
- Network Performance
- Priority Flow Control
- RDMA
- Real Time Tuning
- RoCE
- Scalability
- Storage Infrastructure
- Throughput
section_names:
- azure
---
VamsiVadlamuri analyzes how Azure employs DCQCN for congestion control in large-scale RDMA networking, addressing hardware interoperability, protocol tuning, and resulting performance advantages.<!--excerpt_end-->

# Scaling DCQCN-Based Congestion Control for RDMA in Azure Storage Networks

As cloud storage demands accelerate, Microsoft Azure must ensure its networks can scale with ultra-fast, dependable connectivity. Azure’s implementation of RDMA (Remote Direct Memory Access) has been transformative but also surfaced unique challenges—especially in maintaining stable performance across clusters and hardware generations. This article details Azure's hands-on journey deploying, tuning, and scaling DCQCN (Data Center Quantized Congestion Notification) for practical results.

## RDMA and the Problem of Congestion

- **RDMA** offloads network stack operations to the NIC hardware, freeing the CPU and enabling near line-rate speeds.
- As Azure's storage networks expanded, congestion challenges grew:
  - **Heterogeneous hardware**: Multiple generations of NICs and switches, each with distinct behaviors
  - **Variable latency**: Inter-datacenter links have widely varying RTTs
  - **Incast risks**: High-throughput patterns can overwhelm buffers, leading to packet loss and degraded service

## Azure’s Congestion Control Protocol Choice

Traditional TCP congestion-control methods aren’t effective at these performance tiers. Azure relies on **DCQCN + Priority Flow Control (PFC)** to:

- Maintain high throughput and low latency
- Minimize packet loss

## How DCQCN Coordinates RDMA Traffic

DCQCN implements a feedback loop among:

- **Reaction Point (RP)**: The NIC sender adapts its sending rate
- **Congestion Point (CP)**: Switches mark packets with ECN as queues fill
- **Notification Point (NP)**: The receiving NIC detects ECN marks and sends CNPs (Congestion Notification Packets) to the sender

![DCQCN mechanism illustration omitted for brevity]

- When the switch observes congestion, packets are ECN-marked
- Receiver NICs send CNPs to the sender
- Sender NICs cut their transmission rate appropriately and ramp up again slowly if congestion subsides

## Interoperability: Azure’s Multi-Gen RDMA Hardware

Azure’s data centers mix three RDMA NIC generations:

- **Gen1**: Firmware-based DCQCN, NP-side CNP coalescing, burst-based rate limiting
- **Gen2/Gen3**: Hardware-based DCQCN, RP-side CNP coalescing, per-packet rate limiting

**Interoperability Issues:**

- Gen2/Gen3 sending to Gen1 increases cache misses for Gen1’s receiver pipeline
- Gen1 sending to newer NICs can cause them to reduce rates unnecessarily due to more frequent CNPs

**Azure’s Solutions:**

- Unified NP-side CNP coalescing for Gen2/Gen3 (aligning with Gen1)
- Per-queue pair rate limiting to synchronize with Gen1’s timing
- Per-burst rate limiting on newer NICs to diminish cache pressure

## DCQCN Tuning in Azure: Achieving Fairness and Resilience

- **RTT-fair:** DCQCN’s rate adjustment is independent of link RTT, ideal for Azure’s mixed-latency regional mesh
- **Key tuning strategies:**
  - Sparse ECN marking (large marking thresholds and low marking probability on high-RTT flows)
  - Joint optimization of buffer thresholds and DCQCN parameters
  - Global DCQCN parameter sets, as Azure’s hardware enforces universal settings

## Measured Impact

- **Performance:**
  - RDMA traffic routinely runs at line rate with nearly zero packet loss
  - CPU utilization for storage frontend traffic is up to 34.5% lower than TCP-based networks
  - Up to 23.8% latency reduction for large read I/Os (1 MB), 15.6% for writes
- **Scalability:**
  - ~85% of Azure’s traffic is RDMA as of late 2025, with coverage in all public regions

## Conclusion: DCQCN as Azure’s Foundation for High-Performance Storage

DCQCN’s feedback-driven, adaptive congestion control is fundamental to Azure’s ability to deliver high-throughput, low-latency storage at scale. Its practical fixes for hardware heterogeneity and aggressive tuning are central to the platform’s storage performance, reliability, and future scalability.

---

*Author: VamsiVadlamuri*

*Version: 1.0 (Updated Jan 13, 2026)*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-networking-blog/data-center-quantized-congestion-notification-scaling-congestion/ba-p/4468417)
