---
author: pbeegala
section_names:
- azure
- security
title: 'Resiliency Patterns for Azure Front Door: Field Lessons'
feed_name: Microsoft Tech Community
tags:
- Active Active Architecture
- Akamai
- Azure
- Azure Application Gateway
- Azure Front Door
- Azure Traffic Manager
- Blast Radius Reduction
- Business Continuity
- Community
- Control Plane
- Data Plane
- Disaster Recovery
- DNS Failover
- Edge Networking
- High Availability
- Incident Response
- Multi CDN
- Routing Strategy
- Security
- Tenant Isolation
- WAF
- Web Application Firewall
date: 2026-03-17 07:19:43 +00:00
external_url: https://techcommunity.microsoft.com/t5/azure-architecture-blog/resiliency-patterns-for-azure-front-door-field-lessons/ba-p/4501252
primary_section: azure
---

pbeegala summarizes lessons learned from Azure Front Door incidents in October 2025 and lays out practical resiliency patterns—DNS failover, multi-CDN, and alternate ingress paths—aimed at keeping mission-critical internet-facing workloads available when global routing services have an outage.<!--excerpt_end-->

## Abstract

Azure Front Door (AFD) sits at the edge of Microsoft’s global cloud, delivering secure, performant, and highly available applications to users worldwide. As adoption has grown—especially for mission‑critical workloads—the need for resilient application architectures that can tolerate rare but impactful platform incidents has become essential.

This article summarizes key lessons from Azure Front Door incidents in October 2025, outlines how Microsoft is hardening the platform, and describes architectural patterns customers can adopt to maintain business continuity when global load‑balancing services are unavailable.

## Who this is for

This article is intended for:

- Cloud and solution architects designing **mission‑critical internet‑facing workloads**
- Platform and SRE teams responsible for **high availability and disaster recovery**
- Security architects evaluating **WAF placement and failover trade‑offs**
- Customers running **revenue‑impacting workloads on Azure Front Door**

## Introduction

Azure Front Door (AFD) operates at massive global scale, serving secure, low‑latency traffic for Microsoft first‑party services and thousands of customer applications. Microsoft is investing in:

- **Tenant isolation**
- **Independent infrastructure resiliency**
- **Active‑active service architectures**

Even so, global distributed systems can still have outages. If you run **mission‑critical workloads** on AFD, you should assume global routing can become temporarily unavailable and design **alternative routing paths**.

## Resiliency options for mission‑critical workloads

Each pattern below trades off cost, complexity, operational maturity, and availability.

## 1. No CDN with Application Gateway

Figure 1: Azure Front Door primary routing with DNS failover

**When to use:** Workloads without CDN caching requirements that prioritize predictable failover.

**Architecture summary**

- Azure Traffic Manager (ATM) runs in **Always Serve** mode to provide DNS‑level failover.
- Web Application Firewall (WAF) is implemented **regionally** using Azure Application Gateway.
- App Gateway can be private (when using AFD Premium) and is the default path; DNS failover is available when AFD is not reachable.
- When failover is triggered, one step is switching AppGW IP to public (ATM can route to public endpoints only).
- Switch back to the AFD route once AFD resumes service.

**Pros**

- DNS‑based failover away from the global load balancer
- Consistent WAF enforcement at the regional layer
- Application Gateways can remain private during normal operations

**Cons**

- Additional cost and reduced composite SLA from extra components
- Application Gateway must be made public during failover
- Active‑passive pattern requires **regular testing** to maintain confidence

## 2. Multi‑CDN for mission‑critical applications

Figure 2: Multi‑CDN architecture using Azure Front Door and Akamai with DNS‑based traffic steering

**When to use:** Mission‑critical applications with strict availability requirements and heavy CDN usage.

**Architecture summary**

- Dual CDN setup (for example, Azure Front Door + Akamai)
- Azure Traffic Manager in **Always Serve** mode
- Traffic split (for example, 90/10) to keep both CDN caches warm
- During failover, 100% of traffic is shifted to the secondary CDN
- Ensure origin servers can handle the extra hits (cache misses)

**Pros**

- Highest resilience against CDN‑specific or control‑plane outages
- Maintains cache readiness on both providers

**Cons**

- Expensive and operationally complex
- Requires origin capacity planning for cache‑miss surges
- Not suitable if applications rely on CDN‑specific advanced features

## 3. Multi‑layered CDN (Sequential CDN architecture)

Figure 3: Sequential CDN architecture with Akamai as caching layer in front of Azure Front Door

**When to use:** Rare, niche scenarios where a layered CDN approach is acceptable.

This is not common: Akamai can become a single entry point of failure. However, if AFD isn't available, you can update Akamai properties to directly route to origin servers.

**Architecture summary**

- Akamai used as the front caching layer
- Azure Front Door used as the L7 gateway and WAF
- During failover, Akamai routes traffic directly to origin services

**Pros**

- Direct fallback path to origins if AFD becomes unavailable
- Single caching layer in normal operation

**Cons**

- Fronting CDN remains a single point of failure
- Not generally recommended due to complexity
- Requires a well‑tested operational playbook

## 4. No CDN – Traffic Manager redirect to origin (with Application Gateway)

Figure 4: DNS‑based failover directly to origin via Application Gateway when Azure Front Door is unavailable

**When to use:** Applications that require L7 routing but no CDN caching.

**Architecture summary**

- Azure Front Door provides L7 routing and WAF
- Azure Traffic Manager enables DNS failover
- During an AFD outage, Traffic Manager routes directly to Application Gateway‑protected origins

**Pros**

- Alternative ingress path to origin services
- Consistent regional WAF enforcement

**Cons**

- Additional infrastructure cost
- Operational dependency on Traffic Manager configuration accuracy

## 5. No CDN – Traffic Manager redirect to origin (no Application Gateway)

Figure 5: Direct DNS failover to origin services without Application Gateway

**When to use:** Cost‑sensitive scenarios with clearly accepted security trade‑offs.

**Architecture summary**

- WAF implemented directly in Azure Front Door
- Traffic Manager provides DNS failover
- During an outage, traffic routes directly to origins

**Pros**

- Simplest architecture
- No Application Gateway in the primary path

**Cons**

- Risk of unscreened traffic during failover
- Failover operations can be complex if WAF consistency is required

## Frequently asked questions

**Is Azure Traffic Manager a single point of failure?** No. Traffic Manager operates as a globally distributed service. For extreme resilience requirements, customers can combine Traffic Manager with a backup FQDN hosted in a separate DNS provider.

**Should every workload implement these patterns?** No. These patterns are intended for **mission‑critical workloads** where downtime has material business impact. Non‑critical applications do not require multi‑CDN or alternate routing paths.

**What does Microsoft use internally?** Microsoft uses a combination of **active‑active regions**, **multi‑layered CDN patterns**, and **controlled fail‑away mechanisms**, selected based on service criticality and performance requirements.

## What happened in October 2025 (summary)

Two separate Azure Front Door incidents in October 2025 highlighted the importance of architectural resiliency:

- A control‑plane defect caused erroneous metadata propagation, impacting approximately **26% of global edge sites**
- A later compatibility issue across control‑plane versions resulted in DNS resolution failures

Both incidents were mitigated through automated restarts, manual intervention, and controlled failovers. These events accelerated platform‑level hardening investments.

## How Azure Front Door is being hardened

Microsoft has completed or initiated improvements including:

- Synchronous configuration processing before rollout
- Control‑plane and data‑plane isolation
- Reduced configuration propagation times
- Active‑active fail‑away for major first‑party services
- Microcell segmentation to reduce blast radius

These changes reinforce two goals:

- No single tenant configuration should impact others
- Recovery should be fast and predictable

## Key takeaways

- Global platforms can experience rare outages—architect for them
- Mission‑critical workloads should include **alternate routing paths**
- Multi‑CDN and DNS‑based failover patterns are typically the most robust
- Resiliency is a business decision, not just a technical one

Updated Mar 17, 2026

Version 1.0

[Read the entire article](https://techcommunity.microsoft.com/t5/azure-architecture-blog/resiliency-patterns-for-azure-front-door-field-lessons/ba-p/4501252)

