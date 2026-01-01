---
layout: "post"
title: "Azure Migrate Expands Capabilities to Accelerate Migration to Azure Local"
description: "This article offers an in-depth look at the new General Availability of Azure Migrate support for migrating VMware VMs to Azure Local. It details three primary Azure-based modernization strategies—Modernize and Move, Lift and Optimize, and Edge-Optimized Deployment—explaining how Azure Migrate, Azure VMware Solution, and Azure Arc empower organizations to manage, govern, and secure workloads across hybrid and distributed environments. Key features announced include agentless migrations, support for PowerShell automation, and advanced customization options, all aimed at providing flexibility and compliance for organizations with varying cloud and edge needs."
author: "trumanbrown"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-arc-blog/azure-migrate-expands-capabilities-to-accelerate-migration-to/ba-p/4464789"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-10-28 20:23:03 +00:00
permalink: "/2025-10-28-Azure-Migrate-Expands-Capabilities-to-Accelerate-Migration-to-Azure-Local.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["Agentless Migration", "Azure", "Azure Arc", "Azure Local", "Azure Migrate", "Azure VMware Solution", "Cloud Migration", "Community", "Compliance", "Data Residency", "DevOps", "Edge Computing", "Governance", "Hybrid Cloud", "Infrastructure Modernization", "PowerShell Automation", "Security", "Unified Management", "VMware"]
tags_normalized: ["agentless migration", "azure", "azure arc", "azure local", "azure migrate", "azure vmware solution", "cloud migration", "community", "compliance", "data residency", "devops", "edge computing", "governance", "hybrid cloud", "infrastructure modernization", "powershell automation", "security", "unified management", "vmware"]
---

trumanbrown explains the new General Availability of Azure Migrate support for VMware VM migrations to Azure Local, highlighting modernization paths, unified management, and advanced migration features for secure hybrid cloud environments.<!--excerpt_end-->

# Azure Migrate Expands Capabilities to Accelerate Migration to Azure Local

As organizations step up digital transformation, Microsoft is offering enhanced migration and modernization strategies tailored to varied infrastructure needs. This article outlines new capabilities for migrating VMware virtual machines to Azure Local with Azure Migrate.

## Three Paths to Modernization

1. **Modernize and Move**
   - Migrate applications ready for evolution to Azure’s IaaS and PaaS platforms for a secure, scalable, and cost-effective foundation.
   - Azure Migrate helps with readiness assessments, estimating costs, building business cases, and ensuring secure, centralized governance.

2. **Lift and Optimize**
   - For VMware users, Azure VMware Solution (AVS) enables quick migration of VMware workloads to Azure with no code changes.
   - AVS provides a private VMware VCF cloud in Azure, enabling use of existing licenses and direct connections to more than 200 Azure services.
   - Integration with Azure Migrate, VMware HCX, and Azure Arc ensures simplified migration, unified governance, and robust security across cloud and hybrid scenarios.

3. **Edge-Optimized Deployment**
   - For workloads requiring proximity to data generation or consumption—addressing latency, compliance, or sovereignty needs—Azure Local harnesses Azure Arc to deliver Azure services locally.
   - Azure Local, managed through Azure Arc, offers a sovereign, cloud-managed platform that supports hardware partners like Dell, Lenovo, and HPE, facilitating regulatory compliance and operational flexibility.

## Unified Management, Governance, and Security

- Across all approaches, organizations benefit from centralized management and governance via the Azure control plane, gaining cloud capabilities irrespective of workload location.

## General Availability: Azure Migrate for VMware VMs to Azure Local

**Azure Migrate** now supports GA for migrating VMware virtual machines to Azure Local.

### Key Features

- **Azure Portal Orchestration:** Manage and monitor migrations directly from the Azure portal, including replication progress and migration operations.
- **Agentless Architecture:** No agents required on source VMs—deployment is simplified, especially in larger VMware environments.
- **Zero-Downtime Replication:** Background data synchronization with no impact to running workloads.
- **Sovereign Control:** All migration traffic and data remain entirely on-premises, ensuring full operational sovereignty and data residency.
- **Minimized Downtime Cutover:** Optimized migration methods minimize disruption to business operations.

### Advanced Features in GA

- **Static IP retention** for Windows and Linux VMs.
- **PowerShell migration support** for automation and scripting.
- **Custom compute and disk configurations** allowed during migration.

## Getting Started

- Review product updates, requirements, and detailed tutorials in the [Azure Migrate documentation](https://aka.ms/azlocal-vmware-migrate-docs).
- Access FAQs and troubleshooting resources for assistance.

## Community Acknowledgement

Microsoft thanks preview participants and community members for their input, which directly informed this release.

*Published by trumanbrown, October 28, 2025*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-arc-blog/azure-migrate-expands-capabilities-to-accelerate-migration-to/ba-p/4464789)
