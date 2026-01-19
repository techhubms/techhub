---
layout: post
title: Best Practices for Migrating COTS Applications to Microsoft Azure
author: srhulsus
canonical_url: https://techcommunity.microsoft.com/t5/azure-migration-and/best-practices-for-migrating-cots-applications-to-microsoft/ba-p/4473323
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-26 20:43:14 +00:00
permalink: /azure/community/Best-Practices-for-Migrating-COTS-Applications-to-Microsoft-Azure
tags:
- Automation
- Availability Zones
- Azure Advisor
- Azure API Management
- Azure Governance
- Azure Landing Zone
- Azure Migrate
- Azure Monitor
- Azure Site Recovery
- Azure SQL Managed Instance
- Azure Virtual Machines
- Azure VMware Solution
- Backup
- Cloud Adoption Framework
- Cloud Security
- COTS Migration
- Disaster Recovery
- Identity Integration
- Licensing
- Logic Apps
- Microsoft Azure
- Microsoft Entra ID
- Microsoft Sentinel
- Monitoring
- Operational Optimization
- Runbooks
section_names:
- azure
- security
---
srhulsus explains essential best practices for planning and executing COTS application migrations to Microsoft Azure, covering operational, security, and modernization steps.<!--excerpt_end-->

# Best Practices for Migrating COTS Applications to Microsoft Azure

Migrating Commercial Off-The-Shelf (COTS) applications to Microsoft Azure requires a more deliberate process than standard application migrations. COTS applications typically have strict vendor requirements concerning supported Azure services, operating systems, databases, licensing models, and upgrade cycles. Here is a step-by-step guide for a successful migration:

## 1. Vendor Engagement and Initial Assessment

- Consult the COTS software vendor to verify supported Azure resources, platforms, and licensing requirements.
- Review available Azure-ready architectures or marketplace templates provided by vendors.
- Follow vendor-supported designs to maintain certification and future support in the cloud.
- Reference: [Microsoft Cloud Adoption Framework](https://learn.microsoft.com/azure/cloud-adoption-framework)

## 2. Assessing Application Changes for Migration

- Decide between "lift-and-shift" versus modernization:
  - Some COTS can be run on Azure Virtual Machines with no changes.
  - Modernization topics include moving to Azure SQL Managed Instance or updating architectures for cloud resiliency.
- Reference: [Azure Migration Strategies](https://learn.microsoft.com/azure/cloud-adoption-framework/strategies/migrate)

## 3. Establishing a Secure Azure Landing Zone

- Set up a compliant landing zone with:
  - Microsoft Entra ID for identity integration.
  - Segmented virtual networking.
  - Governance controls and monitoring systems.
  - Built-in disaster recovery planning.
- Reference: [Landing Zone Design Principles](https://learn.microsoft.com/azure/cloud-adoption-framework/ready/landing-zone)

## 4. Infrastructure Alignment with Vendor Requirements

- Identify infrastructure needs such as CPU architecture, memory size, OS versions, and storage performance.
- Use Azure Virtual Machines or Azure VMware Solution for vendor-specific configurations or legacy applications.
- Reference: [Azure VMware Solution](https://learn.microsoft.com/azure/azure-vmware/introduction)

## 5. Licensing and Cost Implications

- Map existing licensing approaches to cloud resources (physical hosts, vCPUs, user counts).
- Clarify changes with vendors and review Microsoft’s licensing policies for Azure workloads.
- Reference: [Core-Based Licensing Models Guidance](https://www.microsoft.com/licensing/guidance/Core-based-licensing-models?msockid=192747e30f11612820fd52ff0ea8603a)

## 6. Modernizing the Surrounding Ecosystem

- Leverage services such as:
  - Azure API Management for secure app interfaces.
  - Logic Apps for backend automation.
  - Azure Monitor and Microsoft Sentinel for operational monitoring and security.
- References:
  - [Azure API Management](https://learn.microsoft.com/azure/api-management)
  - [Logic Apps](https://learn.microsoft.com/azure/logic-apps)
  - [Azure Monitor](https://learn.microsoft.com/azure/azure-monitor)
  - [Microsoft Sentinel](https://learn.microsoft.com/azure/sentinel)

## 7. High-Availability and Disaster Recovery Planning

- Implement resilience strategies:
  - Availability Zones
  - Azure Site Recovery
  - Paired Regions and disk replication
- Consult both vendors and Microsoft for compliance and technical fit.
- Reference: [Azure Site Recovery Overview](https://learn.microsoft.com/azure/site-recovery/site-recovery-overview)

## 8. Pilot Migration and Validation

- Conduct pilot runs for performance, integration, authentication, and operational validation.
- Use Azure Migrate to evaluate compatibility and prep for full migration.
- Reference: [Azure Migrate](https://learn.microsoft.com/azure/migrate)

## 9. Post-Migration Operational Realignment

- Define cloud-centric monitoring thresholds, incident response practices, and backup routines.
- Develop or adapt runbooks for common operational procedures.
- Reference: [Well-Architected Framework](https://learn.microsoft.com/azure/well-architected)

## 10. Continuous Optimization

- Adopt continuous improvement practices to optimize cost, reliability, and security.
- Use Azure Advisor for recommendations based on real usage data.
- Reference: [Azure Advisor](https://learn.microsoft.com/azure/advisor/advisor-overview)

---

**Summary:** Migrating COTS workloads to Azure demands thorough planning, vendor collaboration, careful infrastructure and security provisioning, and ongoing operational refinement. By leveraging Microsoft’s frameworks and services, organizations can build a robust and future-ready cloud foundation for their COTS applications.

*Author: srhulsus*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-migration-and/best-practices-for-migrating-cots-applications-to-microsoft/ba-p/4473323)
