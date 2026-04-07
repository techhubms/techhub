---
author: Zoran Jovanovic
date: 2026-04-01 16:11:25 +00:00
primary_section: azure
feed_name: Microsoft Tech Community
title: 'Proactive Reliability Series — Article 1: Fault Types in Azure'
external_url: https://techcommunity.microsoft.com/t5/azure-architecture-blog/proactive-reliability-series-article-1-fault-types-in-azure/ba-p/4507006
section_names:
- azure
tags:
- Availability Zones
- Azure
- Azure CLI
- Azure PowerShell
- Azure Reliability
- Azure Resource Manager
- Azure REST API
- Azure Service Health
- Azure Status History
- Azure Well Architected Framework
- Blast Radius
- Business Continuity
- Capacity Constraints
- Community
- Cross Region Failover
- Disaster Recovery
- Failure Mode Analysis
- Fault Mode Analysis
- High Availability
- Managed Identities
- Multi Region Architecture
- Partial Region Fault
- Planned Maintenance
- Post Incident Review
- Redundancy
- Regional Outage
- Reliability Pillar
- Resilience Engineering
---

Zoran Jovanovic introduces a practical “fault type” taxonomy for Azure and explains how Fault Mode Analysis helps teams design, test, and operate workloads that can survive real platform incidents like partial region faults.<!--excerpt_end-->

## Overview

This article kicks off the **Proactive Reliability Series**, focused on **designing**, **implementing**, and **operating** reliable workloads on Azure. It’s grounded in Microsoft’s guidance from the [Azure Well-Architected Framework — Reliability pillar](https://learn.microsoft.com/en-us/azure/well-architected/reliability/).

In distributed cloud systems, failures are inevitable (regional outages, AZ issues, misconfigurations, downstream degradations). The goal is to plan for failure so incidents stay small and recoveries are predictable.

> **Disclaimer:** The views expressed in this article are the author’s own and do not represent Microsoft.

## Why Fault Mode Analysis Matters

[Fault Mode Analysis](https://learn.microsoft.com/en-us/azure/well-architected/reliability/failure-mode-analysis) (FMA) is the practice of systematically identifying potential failure points in a workload and its critical flows, then planning mitigations and recovery actions.

Key points:

- In any distributed system, failures can occur even with multiple resiliency layers.
- More complexity generally means more potential failure modes.
- Skipping (or doing incomplete) FMA increases the risk of unpredictable behavior and outages caused by suboptimal design.
- Effective FMA requires understanding what types of faults can happen in **Azure infrastructure**.

## Sample “Azure Fault Type” Taxonomy

Azure faults can range from global outages down to a single VM issue. The taxonomy here is explicitly from a **customer impact perspective** (how it affects workloads and what you can do about it), not from an internal Azure engineering view.

Some “faults” are not platform breakages but still matter as failure modes, such as:

- misunderstanding of designed behaviors (for example, planned maintenance impact)
- platform design constraints (for example, capacity constraints)

> **Disclaimer:** This taxonomy is unofficial, not endorsed or maintained by Microsoft. Likelihood values are planning heuristics (not statistics, not SLAs). Refer to official [Azure documentation](https://learn.microsoft.com/en-us/azure/) and [Azure Service Health](https://azure.status.microsoft/) for authoritative guidance.

### Fault types table (customer impact view)

| Fault Type | Blast Radius | Likelihood | Mitigation redundancy level requirements |
| --- | --- | --- | --- |
| Service Fault (Global) | Worldwide or multiple regions | Very low | High |
| Service Fault (Region) | Single service in region | Medium | Region redundancy |
| Region Fault | Single region | Very low | Region redundancy |
| Partial Region Fault | Multiple services in a single region | Low | Region redundancy |
| Availability Zone Fault | Single AZ within region | Low | Availability Zone redundancy |
| Single Resource Fault | Single VM/instance | High | Resource redundancy |
| Platform Maintenance Fault | Variable (resource to region) | High | Resource redundancy, maintenance schedules |
| Region Capacity Constraint Fault | Single region | Low | Region redundancy, capacity reservations |
| Network POP Location Fault | Network hardware colocation site | Low | Site redundancy |

The rest of the article focuses on one often underestimated fault: the **Partial Region Fault**.

## Deep Dive: Partial Region Fault

A **Partial Region Fault** affects multiple Azure services in a single region at the same time, typically due to shared dependencies such as:

- regional networking issues
- regional platform incidents
- shared infrastructure dependencies
- storage subsystem degradation affecting dependent services
- control plane issues that affect service management

Key characteristics called out:

- It’s not a complete region outage—some services may be normal while others degrade or fail.
- In referenced cases, these incidents have historically been resolved within hours.

### Partial Region Fault attributes

| Attribute | Description |
| --- | --- |
| Blast radius | Multiple services within a single region |
| Likelihood | Low |
| Typical duration | Minutes to hours |
| Fault tolerance options | Multi-region architecture; cross-region failover |
| Fault tolerance cost | High |
| Impact | Severe |
| Typical cause | Regional networking failure impacting multiple services; regional storage degradation; regional control plane issues |

### Why partial faults are a planning blind spot

Teams often plan for a binary scenario (“region up” vs “region down”). That creates gaps:

- **Failover logic may not trigger**: health probes can still pass, traffic managers may still route to the degraded region, and automation may not fail over.
- **Recovery is more complex**: you may need to selectively fail over only some services, while others remain in the primary region.

## Real-world examples referenced

The article lists incidents from Azure Status History showing multi-service impacts that were not full region outages:

### Switzerland North — Network Connectivity Impact (BT6W-FX0)

- Date: September 26–27, 2025
- Region: Switzerland North
- Window: 23:54 UTC (26 Sep) – 21:59 UTC (27 Sep 2025)
- Duration: ~22 hours
- Impact: multiple network-dependent services

Link: [View PIR on Azure Status History](https://azure.status.microsoft/en-us/status/history/?trackingid=BT6W-FX0)

### East US and West US — Managed Identities and dependent services (_M5B-9RZ)

- Date: February 3, 2026
- Regions: East US, West US
- Window: 00:10 UTC – 06:05 UTC
- Duration: ~6 hours
- Impact: Managed Identities plus dependent operations (create/update/delete resources, token acquisition)

Link: [View PIR on Azure Status History](https://azure.status.microsoft/en-us/status/history/?trackingid=_M5B-9RZ)

### Azure Government — Azure Resource Manager failures (ML7_-DWG)

- Date: December 8, 2025
- Regions: Azure Government (all regions)
- Window: 11:04–14:13 EST (16:04–19:13 UTC)
- Duration: ~3 hours
- Impact: service management operations via ARM (Azure Portal, REST APIs, Azure PowerShell, Azure CLI), affecting 20+ services

Link: [View PIR on Azure Status History](https://azure.status.microsoft/en-us/status/history/?trackingid=ML7_-DWG)

## Wrapping up

The main takeaway is that resilient Azure design requires thinking beyond full-region disaster scenarios. **Partial region faults** can create severe user impact while remaining “invisible” to simplistic failover triggers.

The taxonomy is intended as a starting point for FMA; teams should revisit it as Azure and industry practices evolve.

## Authors & reviewers

- Authored by: [Zoran Jovanovic](https://www.linkedin.com/in/zoranjovanovic/)
- Peer review: [Catalina Alupoaie](https://www.linkedin.com/in/catalina-alupoaie/)
- Peer review: [Stefan Johner](https://www.linkedin.com/in/stefanjohner/)

## References

- [Azure Well-Architected Framework — Reliability Pillar](https://learn.microsoft.com/en-us/azure/well-architected/reliability/)
- [Failure Mode Analysis](https://learn.microsoft.com/en-us/azure/well-architected/reliability/failure-mode-analysis)
- [Shared Responsibility for Reliability](https://learn.microsoft.com/en-us/azure/reliability/concept-shared-responsibility)
- [Azure Availability Zones](https://learn.microsoft.com/en-us/azure/reliability/availability-zones-overview)
- [Business Continuity and Disaster Recovery](https://learn.microsoft.com/en-us/azure/reliability/concept-business-continuity-high-availability-disaster-recovery)
- [Transient Fault Handling](https://learn.microsoft.com/en-us/azure/architecture/best-practices/transient-faults)
- [Azure Service Level Agreements](https://www.microsoft.com/licensing/docs/view/Service-Level-Agreements-SLA-for-Online-Services)
- [Azure Reliability Guidance by Service](https://learn.microsoft.com/en-us/azure/reliability/overview-reliability-guidance)
- [Azure Status History](https://azure.status.microsoft/status/history/)


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-architecture-blog/proactive-reliability-series-article-1-fault-types-in-azure/ba-p/4507006)

