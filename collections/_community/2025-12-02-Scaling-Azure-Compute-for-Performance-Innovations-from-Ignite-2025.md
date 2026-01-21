---
external_url: https://techcommunity.microsoft.com/t5/azure-compute-blog/scaling-azure-compute-for-performance/ba-p/4474662
title: 'Scaling Azure Compute for Performance: Innovations from Ignite 2025'
author: DanaCozmei
feed_name: Microsoft Tech Community
date: 2025-12-02 21:20:46 +00:00
tags:
- AI Inference
- Automation
- Azure Compute
- Capacity Optimization
- Cloud Resiliency
- Compute Gallery
- Data Analytics
- Direct Virtualization
- Global Deployment
- GPU
- Large Containers
- NVMe
- Performance Tuning
- Scale Sets
- Scheduled Actions
- Soft Delete
- VM Applications
- VMSS Instance Mix
- Zonal Redundant Storage
section_names:
- ai
- azure
- devops
- security
---
DanaCozmei summarizes Ignite 2025’s key Azure Compute breakthroughs, focusing on practical infrastructure features for scaling AI, analytics, and distributed workloads with resiliency, automation, and performance.<!--excerpt_end-->

# Scaling Azure Compute for Performance: Innovations from Ignite 2025

Ignite 2025 revealed the next wave of Azure Compute advancements to meet the real-world pressures of modern workloads—especially AI inference, data-centric analytics, and globally distributed services. This summary by DanaCozmei highlights practical improvements released and in preview:

## Direct Virtualization – Immediate Device Access for High-Performance Apps

- Enables nearly bare metal access to NVMe disks and GPUs.
- Ideal for latency-sensitive workloads such as AI inference and gaming.
- Isolation for child VMs hosting hostile workloads.
- Lowers latency and cost for demanding applications.
- Available in limited preview: [Sign up for Direct Virtualization Preview](https://aka.ms/DirectVirtualizationPreview)

## Large Container Sizes – Accelerating Compute-Intensive Applications

- High vCPU and memory configurations to supercharge AI/ML training and analytics.
- Allows rapid scaling of inference workloads.
- Simplifies container orchestration: fewer containers per workload.
- Reduces inter-container latency.
- Generally Available: [Learn more about Large Containers](https://aka.ms/bigcontainersblog)

## VM Applications – Streamlining Global Application Deployments

- Manage and deploy up to 25 applications per VM (2GB each).
- Consistent deployments across regions—automatic replication and updates.
- Automate updates without manual intervention.
- Reduces operational overhead for global distributed workloads.
- [VM Applications GA Announcement](https://aka.ms/VMApps/blogs/ignite2025)

## Scheduled Actions – Automating Operations at Scale

- Schedule power operations for up to 5,000 VMs simultaneously.
- Built-in reliability and safeguards for large-scale automation.
- Actions available: Start, Stop, Hibernate (more coming soon).

## Azure Compute Gallery – New Features for Reliability

- Soft Delete (preview): Protect VM images from accidental deletions.
- Zonal Redundant Storage (ZRS): Default for image version storage to improve uptime and resiliency.
- Enhances operational reliability and compliance for teams managing VM images and artifacts.
- [Resiliency Announcement](https://aka.ms/acgresiliencyblog)

## VMSS Instance Mix – Flexible Capacity Acquisition

- Define up to five VM SKUs in a deployment for capacity fungibility.
- Supports allocation strategies (CapacityOptimized, LowestPrice, Prioritized).
- Enables agility and cost optimization for unpredictable workloads.

## Best Practices Shared at Ignite

- Use latest SKUs for optimal performance and cost-efficiency.
- Instance mix helps acquire capacity at scale across VM sizes.
- VM Apps improve reliable global app delivery.
- Scheduled actions streamline power state management (great for Virtual Desktop scenarios).
- Build resiliency and security natively into compute architecture.

## Session On-Demand

Check out the full session: [BRK173: Scaling Azure Compute at Ignite](https://ignite.microsoft.com/en-US/sessions/BRK173?source=/speakers/80665103-5e69-4b8c-ad15-1d1f84c8dd6a)

## Conclusion: Enabling AI-Ready, Resilient Cloud Infrastructure

Azure Compute’s evolution reflects customer demand for intelligent, adaptive, and automated infrastructure. With innovations like Direct Virtualization, large containers, VM Applications, and capacity-optimized scale sets, enterprises can scale confidently for AI, analytics, and distributed workloads. These advancements empower technical teams to deliver robust, production-ready solutions that keep pace with the industry's growth toward intelligent, distributed systems.

Learn more at [Azure Compute Blog](https://techcommunity.microsoft.com/t5/s/gxcuf89792/m_assets/avatars/default/avatar-5.svg?image-dimensions=50x50).

*Author: DanaCozmei (Joined November 14, 2023)*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-compute-blog/scaling-azure-compute-for-performance/ba-p/4474662)
