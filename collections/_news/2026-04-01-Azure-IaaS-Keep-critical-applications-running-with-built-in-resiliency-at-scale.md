---
section_names:
- azure
- devops
author: Igal Figlin
feed_name: The Azure Blog
external_url: https://azure.microsoft.com/en-us/blog/azure-iaas-keep-critical-applications-running-with-built-in-resiliency-at-scale/
title: 'Azure IaaS: Keep critical applications running with built-in resiliency at scale'
date: 2026-04-01 16:00:00 +00:00
tags:
- Application Gateway
- Availability Zones
- Azure
- Azure Backup
- Azure Data Box
- Azure Front Door
- Azure IaaS
- Azure Load Balancer
- Azure Migrate
- Azure Site Recovery
- Azure Storage
- Azure Storage Mover
- Azure Traffic Manager
- Azure Verified Modules
- Azure Virtual Machines
- CI/CD
- Compute
- Configuration Drift
- Deployment Automation
- DevOps
- Disaster Recovery
- Fault Domains
- Fault Simulation
- GRS
- High Availability
- IaC
- Landing Zones
- LRS
- Managed Disks
- Management And Governance
- Migration
- Mission Critical Workloads
- Networking
- News
- Observability
- RA GRS
- Resiliency
- Resiliency Testing
- Security
- Snapshots
- Storage
- Storage Redundancy
- Terraform
- Virtual Machine Scale Sets
- ZRS
primary_section: azure
---

Igal Figlin outlines how to design and operate resilient, mission-critical workloads on Azure IaaS, covering compute isolation (VM Scale Sets and availability zones), storage redundancy and recovery options (LRS/ZRS/GRS, Backup, Site Recovery), and networking patterns (Load Balancer, Application Gateway, Traffic Manager, Front Door), with migration and IaC/CI-CD as key opportunities to bake resiliency in early.<!--excerpt_end-->

# Azure IaaS: Keep critical applications running with built-in resiliency at scale

Azure disruptions (hardware issues, planned maintenance, zonal outages, and even regional incidents) are not edge cases. This piece explains how Azure IaaS capabilities across **compute, storage, and networking** can be combined into practical resiliency architectures—and why outcomes still depend on how you design and operate your workloads.

Key idea: **resiliency is a shared responsibility**. Azure provides platform capabilities for availability and recovery, but customers must design, configure, and validate their architectures.

Relevant resources:

- Azure IaaS: <https://azure.microsoft.com/en-us/solutions/azure-iaas>
- Azure IaaS Resource Center: <https://azure.microsoft.com/solutions/azure-iaas/>

## Resiliency for mission-critical applications

For mission-critical workloads, downtime can impact transactions, operations, financial outcomes, and reputation. Resilient design starts by planning for *how the system behaves during disruption*, not by assuming disruption won’t happen.

Azure IaaS resiliency building blocks are described as supporting:

- Isolation and redundancy
- Failover and recovery
- Reduced “blast radius” during incidents

## Keep applications available with resilient compute design

Compute resiliency starts with **placement and isolation**. If your VMs are too closely coupled from an infrastructure perspective, a localized issue can impact more of the workload than expected.

### Virtual Machine Scale Sets

For scale plus availability, **Virtual Machine Scale Sets** can automate deployment/management while distributing instances across:

- Availability zones
- Fault domains

This is positioned as especially useful for front-end tiers and application tiers where maintaining enough healthy instances is essential.

Link: <https://azure.microsoft.com/en-us/products/virtual-machine-scale-sets>

### Availability zones

Availability zones provide datacenter-level isolation inside a region (independent power, cooling, and networking). Architecting across zones enables workloads to continue running even if one zone is impacted.

![3 D resilient apps flowchart including Azure Portal, Azure Copilot, and Powershell C L I](https://azure.microsoft.com/en-us/blog/wp-content/uploads/2026/04/BRK148-Architect-Resilient-Apps-Breakout-1024x576.webp)

## Build continuity and recovery on a resilient storage foundation

When disruptions occur, data durability and recoverability become central. Azure storage redundancy models mentioned include:

- **LRS (Locally redundant storage)**: multiple copies in a single datacenter
- **ZRS (Zone-redundant storage)**: synchronous replication across zones in a region
- **GRS (Geo-redundant storage)**: replication to a secondary region
- **RA-GRS (Read-access geo-redundant storage)**: GRS plus read access to the secondary

For VM-based workloads and **managed disks**, recovery also depends on:

- Snapshots
- **Azure Backup**: <https://azure.microsoft.com/en-us/products/backup>
- **Azure Site Recovery**: <https://azure.microsoft.com/en-us/products/site-recovery>

The article ties storage choices directly to:

- **RPO (Recovery Point Objective)**
- **RTO (Recovery Time Objective)**

## Keep network traffic moving when conditions change

Even if compute and storage are healthy, users still experience an outage if they can’t reach the workload. Networking resiliency focuses on distributing traffic to healthy resources and rerouting around failures.

Services called out:

- **Azure Load Balancer**: <https://azure.microsoft.com/en-us/products/load-balancer>
- **Application Gateway** (Layer 7 routing): <https://azure.microsoft.com/en-us/products/application-gateway>
- **Traffic Manager** (DNS routing across endpoints): <https://azure.microsoft.com/en-us/products/traffic-manager>
- **Azure Front Door** (global traffic failover): <https://azure.microsoft.com/en-us/products/frontdoor/>

## Tailor resiliency to workload requirements

Different workload types push you toward different resiliency patterns:

- Stateless tiers: autoscaling, zone distribution, rapid instance replacement
- Stateful workloads: stronger replication, backup, and failover planning

Mission-critical systems typically demand tighter recovery targets and more rigorously tested recovery paths, but the article notes that higher redundancy also means more cost/complexity trade-offs.

## Make migrations an opportunity to improve resiliency

Migration (or deploying net-new workloads) is described as a prime time to remove inherited single points of failure and build stronger continuity into the architecture.

An example given: Carne Group combining **Azure Site Recovery** with **Terraform-based landing zones** to streamline cutover while improving resilience.

Customer story link: <https://www.microsoft.com/en/customers/story/26204-carne-group-azure-site-recovery>

### Site Recovery for regional resilience

**Azure Site Recovery** is presented as a foundational capability for **regional resilience**, enabling replication and restart of workloads in another Azure region on demand. The customer controls where/when workloads move, aligned with capacity, compliance, and regional needs.

### Migration tooling

Services listed for different migration scenarios:

- **Azure Migrate**: <https://azure.microsoft.com/en-us/products/azure-migrate>
- **Azure Storage Mover**: <https://azure.microsoft.com/en-us/products/storage-mover/>
- **Azure Data Box**: <https://azure.microsoft.com/en-us/products/databox>

The article also mentions using GitHub and pipeline-based deployment practices to operationalize resiliency over time, pointing to **Azure Verified Modules**:

- <https://azure.github.io/Azure-Verified-Modules/>

## Maintain resiliency after deployment as workloads evolve

Resiliency can degrade over time due to:

- Configuration drift
- New dependencies
- Changing recovery expectations

Suggested ongoing practices include:

- Testing and drills
- Fault simulations
- Observability practices to identify issues early and understand root cause

A GitHub project, **Resiliency in Azure**, is mentioned as previewed at Ignite (with a public preview planned for Microsoft Build 2026):

- <https://github.com/Azure/ResiliencyInAzure>

Sign-up link:

- <https://github.com/Azure/ResiliencyInAzure>

## Summary

Azure IaaS provides resiliency capabilities across compute, storage, and networking, but resilient outcomes come from how you design, configure, automate, and continuously validate your architecture.

To explore more tutorials and best practices, see the Azure IaaS Resource Center:

- <https://azure.microsoft.com/en-us/solutions/azure-iaas>


[Read the entire article](https://azure.microsoft.com/en-us/blog/azure-iaas-keep-critical-applications-running-with-built-in-resiliency-at-scale/)

