---
external_url: https://techcommunity.microsoft.com/t5/reliability-and-resiliency-in/modern-azure-resilience-with-mark-russinovich/ba-p/4508967
title: Choosing the Right Multi-Region Resilience Pattern on Azure
section_names:
- azure
primary_section: azure
author: molina_sharma
date: 2026-04-06 22:38:00 +00:00
tags:
- Active Active
- Active Passive
- ASR
- Availability Zones
- Azure
- Azure Backup
- Azure Blob Storage
- Azure Cosmos DB
- Azure Resilience
- Azure Service Reliability Guides
- Azure Site Recovery
- Azure SQL
- Azure Well Architected Framework
- BCDR
- Business Continuity
- Cloud Adoption Framework
- Community
- Data Residency
- Disaster Recovery
- Geo Redundant Storage
- GRS
- Landing Zones
- Latency Tolerance
- Multi Region Architecture
- Non Paired Regions
- Object Replication
- Paired Regions
- Recoverability
- Reliability
- Resiliency
- RPO
- RTO
- Safe Deployment Practices
- Zonal Resources
- Zone Redundant
feed_name: Microsoft Tech Community
---

molina_sharma outlines practical Azure architecture choices for resilience, comparing in-region availability-zone designs with multi-region disaster recovery patterns (paired and non-paired) and active/active setups, and explains how to pick based on RTO/RPO, latency, compliance, and operational trade-offs.<!--excerpt_end-->

## Overview

Resiliency in cloud workloads spans three related goals:

- **Reliability**: consistent operation and performance
- **Resiliency**: withstanding failures
- **Recoverability**: predictable recovery after an incident

This article focuses on **multi-region design decisions on Azure**, including when to use **Availability Zones**, **paired regions**, and **non-paired regions** to meet business continuity goals.

## Four common Azure resilience patterns

1. **In-region High Availability (HA) with Availability Zones (AZ)**
   - Deploy across multiple [Availability Zones](https://learn.microsoft.com/en-us/azure/reliability/availability-zones-overview) in a single region to maximize availability.

2. **Regional Business Continuity and Disaster Recovery (BCDR)**
   - Primary/secondary across separate regions.
   - Region choice driven by geographic risk boundaries, regulatory requirements, and service availability.
   - Recovery sequencing and failover depend on workload dependencies and organizational requirements.

3. **Non-paired region BCDR**
   - Primary/secondary, but the secondary is selected based on capacity, service availability, data residency, and latency.
   - Also positioned as a long-term scale option because regions have physical footprint and latency-bound practical limits.
   - Reference: [multi-region solutions in non-paired regions](https://learn.microsoft.com/en-us/azure/reliability/regions-multi-region-nonpaired).

4. **Multi-region active/active**
   - Multiple regions serve production traffic simultaneously.
   - Can improve global performance and resilience, but increases architectural complexity and operational overhead.

## Why Azure launched with paired regions

Azure regions originally launched in pairs to align with enterprise BCDR practices:

- Familiar primary/secondary failover model
- Data residency and regulatory alignment within a geographic boundary
- Turnkey replication capabilities (example: [Geo-Redundant Storage (GRS)](https://learn.microsoft.com/azure/storage/common/storage-redundancy#geo-redundant-storage))
- Platform-level sequencing of updates to reduce simultaneous regional impact
- A recovery prioritization model for geography-wide incidents

The article emphasizes that **regional parity is not guaranteed even for paired regions**. Architects must account for differences in service availability, SKUs, scale limits, capacity, and cost. Practical validation link: [products by region table](https://azure.microsoft.com/en-us/explore/global-infrastructure/products-by-region/table).

## How Azure resilience evolved: Availability Zones

[Availability Zones](https://learn.microsoft.com/en-us/azure/reliability/availability-zones-overview?tabs=azure-cli) (introduced in 2018) are physically isolated datacenter groups inside a region, each with independent power, cooling, and networking.

Many services (e.g., App Service, Storage, Azure SQL) can use zones for platform-managed resilience.

Since ~2020, new regions have often been designed with multiple zones without relying on a paired region, enabling:

- High availability within a single region
- Platform-managed resilience for many failure scenarios
- Reduced need for multi-region deployments for standard HA targets

## How to choose: start from non-functional requirements

Define expectations that drive design choices:

- Uptime targets
- **RTO** (Recovery Time Objective)
- **RPO** (Recovery Point Objective)
- Latency tolerance
- Data residency and compliance requirements

A key framing in the post:

- **HA vs DR is differentiated by recovery objectives more than geography**.
  - HA aims for near-zero downtime and minimal data loss.
  - DR allows defined recovery time and acceptable data loss.

## HA within a region: zone-redundant vs zonal

Azure supports two common approaches:

- **Zone-redundant resources**
  - Replicated across multiple zones.
  - Some services provide built-in zone redundancy; others require configuration.
  - Microsoft typically chooses zones, though some services allow selection.

- **Zonal resources**
  - Deployed into a single zone.
  - No automatic resiliency to a zone outage.
  - To be resilient, you deploy separate resources across zones and handle failover yourself.

The post’s core guidance: **designing for resiliency across Availability Zones is generally preferred when supported**, but you still need to weigh cost, capacity constraints, and potential latency impacts.

## DR across regions: paired vs non-paired is a choice

The article argues region pairs are no longer a default rule. As Azure [Safe Deployment Practices (SDP)](https://azure.microsoft.com/en-us/blog/advancing-safe-deployment-practices/?msockid=2f9e0a1921a66bf21a0b1ee8201c6a6d) matured, pairs became one tool among several.

Using non-paired regions (or mixing paired and non-paired) can help optimize for:

- Data residency and regulatory boundaries
- Latency to specific user populations
- Different recovery objectives per workload
- Reduced coupling to tightly linked regional behaviors

Non-paired designs often require more explicit workload strategy, such as:

- Application-level replication
- Asynchronous data synchronization
- Failover orchestration

The post stresses: **regional failover is typically customer-orchestrated**; teams should design, test, and operate failover/failback rather than assuming platform-initiated regional failover.

## Workload mobility vs data protection: ASR and Azure Backup

The article distinguishes two complementary capabilities:

- **Azure Site Recovery (ASR)**
  - Near-continuous replication and orchestrated failover for VM-based workloads to a region of choice (not limited to paired regions).
  - Used when you need low RPO, controlled failover, and workload restart in a secondary region.

- **Azure Backup**
  - Durable, policy-based data protection independent of compute availability.
  - Not an HA or failover solution; it’s used when restore is the recovery mechanism.

They’re often used together: **ASR for VM continuity**, **Azure Backup for data protection/restore**, including to non-paired regions.

## If you use paired regions today

The post’s position:

- If paired regions meet compliance/data residency/DR objectives, the architecture remains valid and supported.
- Paired regions are not the only way to achieve enterprise-grade resilience anymore.
- For workloads that used paired regions mainly to protect against local datacenter failure, **Availability Zones plus geo-redundant services** may provide equal or better protection with less complexity and cost.

## What’s next: resiliency features in Azure Copilot

Two roadmap items mentioned:

- [Resiliency agent in Azure Copilot (preview)](https://learn.microsoft.com/en-us/azure/copilot/resiliency-agent)
  - Helps identify missing resiliency coverage and provides guidance (including scripts) for remediation (e.g., Azure Backup, ASR, recovery drills).

- [Resiliency in Azure](https://learn.microsoft.com/en-us/azure/resiliency/resiliency-overview)
  - Unified experience for zone resiliency, HA, backup, DR, and ransomware protection, integrated into Azure Copilot and Azure portal.

## Recommended starting points

- [CAF – Landing zone design area (BCDR)](https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/ready/landing-zone/design-area/management-business-continuity-disaster-recovery)
- [WAF – Disaster recovery strategies](https://learn.microsoft.com/en-us/azure/well-architected/reliability/disaster-recovery)
- [WAF design guide – Regions & Availability Zones](https://learn.microsoft.com/en-us/azure/well-architected/design-guides/regions-availability-zones)
- [Azure service reliability guides](https://learn.microsoft.com/en-us/azure/reliability/?product=popular)
- [Non-paired multi-region configurations](https://learn.microsoft.com/en-us/azure/reliability/regions-multi-region-nonpaired)
- [Validate feasibility before you design](https://azure.microsoft.com/en-us/explore/global-infrastructure/products-by-region/)

## Next step

Explore [Azure Essentials](https://azure.microsoft.com/en-us/solutions/azure-essentials/) and related Azure Blog guidance:

- [Resiliency in the cloud—empowered by shared responsibility and Azure Essentials](https://azure.microsoft.com/en-us/blog/resiliency-in-the-cloud-empowered-by-shared-responsibility-and-azure-essentials/)
- [How to design reliable, resilient, and recoverable workloads on Azure](https://azure.microsoft.com/en-us/blog/azure-reliability-resiliency-and-recoverability-build-continuity-by-design/)

## Related resources (as listed)

- [Architecture strategies for using Availability Zones and Regions](https://learn.microsoft.com/azure/well-architected/design-guides/regions-availability-zones)
- [Architecture strategies for highly available multi-region design](https://learn.microsoft.com/azure/well-architected/reliability/highly-available-multi-region-design)
- [Disaster Recovery](https://learn.microsoft.com/azure/resiliency/tutorial-manage-data-using-copilot)
- [Architecture strategies for designing a Disaster Recovery strategy](https://learn.microsoft.com/en-us/azure/well-architected/reliability/disaster-recovery)
- [Multi-Region solutions in nonpaired Regions](https://learn.microsoft.com/azure/reliability/regions-multi-region-nonpaired)
- [Develop a disaster recovery plan for multi-region deployments](https://learn.microsoft.com/azure/well-architected/design-guides/disaster-recovery)
- [Azure region pairs and nonpaired regions](https://learn.microsoft.com/azure/reliability/regions-paired)
- [Reliability guides for Azure services](https://learn.microsoft.com/azure/reliability/overview-reliability-guidance)


[Read the entire article](https://techcommunity.microsoft.com/t5/reliability-and-resiliency-in/modern-azure-resilience-with-mark-russinovich/ba-p/4508967)

