---
title: Announcing Public Preview for Essential Machine Management
author: Meagan McCrory
feed_name: Microsoft Tech Community
section_names:
- azure
- security
tags:
- Azure
- Azure Arc Enabled Servers
- Azure Monitor VM Insights
- Azure Policy
- Azure Update Manager
- Azure Virtual Machines
- Change Tracking And Inventory
- Community
- Compliance
- Compute Infrastructure Hub
- Essential Machine Management
- Guest Configuration
- Hybrid Management
- Machine Configuration
- Monitoring And Operations
- Multi Cloud Management
- Recommended Alerts
- Security
- Security Baseline Policy
- Subscription Onboarding
external_url: https://techcommunity.microsoft.com/t5/azure-governance-and-management/announcing-public-preview-for-essential-machine-management/ba-p/4502721
primary_section: azure
date: 2026-04-06 18:54:19 +00:00
---

Meagan McCrory announces a public preview “Essential Machine Management” experience in Azure’s Compute Infrastructure Hub, aimed at onboarding Azure VMs and Azure Arc-enabled servers at subscription scope for monitoring, updates, inventory, configuration, and security baselines.<!--excerpt_end-->

## Enable basic management capabilities for all your Azure virtual machines and Arc-enabled servers in your subscription

Managing servers and VMs across Azure, on-premises, and multi-cloud environments often means turning on core capabilities—monitoring, updates, inventory, and configuration—through separate setup experiences. Feedback indicates this makes it harder to get visibility into machine state and take actions.

Microsoft has announced the **public preview of the Essential Machine Management experience within Compute Infrastructure Hub**. It provides a streamlined entry point in Azure to onboard machines at scale and enable basic management capabilities at **subscription scope**.

## What is Essential Machine Management?

Essential Machine Management is a **centralized onboarding experience** to enroll machines into selected Azure cloud-native management services in a scalable way.

Instead of enabling monitoring, updates, inventory, and configuration per machine, you can enroll **entire subscriptions** at once, covering:

- **Azure Virtual Machines**
- **Azure Arc–enabled servers**

Once enrolled, **current and future machines** in the selected subscriptions are automatically onboarded to the enabled services to help ensure consistent visibility and operational coverage.

## What management capabilities are enabled?

Essential Machine Management can onboard machines to multiple Azure management capabilities:

- **Monitoring insights and recommended alerts** for machine health and performance
  - https://learn.microsoft.com/azure/azure-monitor/vm/vminsights-overview
- **Azure Update Manager** to help keep machines secure and compliant
  - https://learn.microsoft.com/azure/update-manager/overview
- **Change tracking and inventory** for visibility and auditability
  - https://learn.microsoft.com/azure/azure-change-tracking-inventory/overview-monitoring-agent
- **Machine configuration** for managing in-machine configuration, compliance, and security
  - https://learn.microsoft.com/azure/governance/machine-configuration/overview/01-overview-concepts
- **Azure Security baseline policy** (tailored rules to assess security posture)
  - https://learn.microsoft.com/azure/governance/machine-configuration/how-to/assign-security-baselines/overview-page

These services are positioned as pre-configured with best practices to provide out-of-the-box value.

## How much does it cost?

- **Azure VMs**: capabilities enabled by Essential Machine Management are provided **at no additional charge**.

- **Azure Arc-enabled servers**:
  - **No additional charge** for Arc-enabled servers with:
    - Windows Server Software Assurance
    - Windows Server PayGo
    - Windows Server Extended Security Updates
  - For **all other Arc-enabled servers**, Essential Machine Management will be priced at **$9 per server per month** once billing is enabled.
    - Pricing details: https://learn.microsoft.com/en-us/azure/operations/configuration-enrollment?toc=%2Fazure%2Fvirtual-machines%2Ftoc.json#essentials-tier

## Getting started

The feature is available in **public preview**.

- Azure portal entry point: https://aka.ms/emm-portal
- Navigation path: **Compute infrastructure → Monitoring + Operations → Essential Machine Management (preview)**

For feedback or support:

- Email: machineenrollmentsupport@microsoft.com
- Learn more: https://aka.ms/EssentialMachineManagement

## Publication info

- Updated: Apr 06, 2026
- Version: 1.0

[Read the entire article](https://techcommunity.microsoft.com/t5/azure-governance-and-management/announcing-public-preview-for-essential-machine-management/ba-p/4502721)

