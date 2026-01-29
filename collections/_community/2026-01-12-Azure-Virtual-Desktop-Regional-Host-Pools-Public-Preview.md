---
external_url: https://techcommunity.microsoft.com/t5/azure-virtual-desktop-blog/now-in-public-preview-azure-virtual-desktop-regional-host-pools/ba-p/4474598
title: Azure Virtual Desktop Regional Host Pools Public Preview
author: TomHickling
feed_name: Microsoft Tech Community
date: 2026-01-12 20:30:42 +00:00
tags:
- Admin Preview
- Azure Portal
- Azure Regions
- Azure Virtual Desktop
- Cloud Infrastructure
- Cloud Services
- Data Sovereignty
- Database Architecture
- Deployment
- Host Pools
- Migration
- PowerShell
- Regional Host Pools
- Resiliency
- Azure
- Community
section_names:
- azure
primary_section: azure
---
TomHickling presents the public preview of regional host pools for Azure Virtual Desktop, highlighting the infrastructure's resiliency improvements and deployment guidance for administrators.<!--excerpt_end-->

# Azure Virtual Desktop Regional Host Pools Public Preview

**Author: TomHickling**

## Overview

Microsoft has announced the public preview of **regional host pools** for Azure Virtual Desktop (AVD), focusing on greater resiliency and data sovereignty. This new architecture deploys host pool metadata on separate databases within each Azure region, reducing cross-region dependencies and improving local recovery.

## Key Concepts

### What Are Regional Host Pools?

- Traditional AVD host pools use a geographical model, with a single database per geography serving multiple regions.
- Regional host pools introduce dedicated databases and infrastructure per Azure region supported by AVD.
- This design limits the blast radius of failures to only the affected region and supports strict data residency.

### Benefits

- **Enhanced resilience**: Issues are isolated to individual regions due to localized databases and infrastructure.
- **Data sovereignty**: Host pool metadata stays within the user's chosen region, supporting compliance requirements.
- **Future-proofing**: Microsoft intends for all new pools to use the regional model. Migration tools will be released.

## Deployment & Transition

### Enabling Regional Host Pools

- The public preview is opt-in.
- Currently, only PowerShell is supported for creating regional host pools. Azure Portal support is coming soon.

**To enable via PowerShell:**

1. Register the preview feature:

   ```powershell
   Register-AzProviderFeature -ProviderNamespace Microsoft.DesktopVirtualization -FeatureName AVDRegionalResourcesPublicPreview
   ```

2. Use Az.DesktopVirtualization module version 5.4.5-preview or later.
3. When creating a pool, use the `-DeploymentScope Regional` parameter:

   ```powershell
   New-AzWvdHostPool -DeploymentScope Regional ...
   ```

- New application groups inherit the deployment scope of their parent host pool.
- When portal support is added, select "Regional" in the new Deployment Scope dropdown after selecting a supported location.

### Preview Limitations

- Supported only in selected regions (initially East US 2 and Central US).
- No support yet for:
  - Automated host pool configuration
  - Private Link
  - Dynamic autoscaling
  - App Attach
  - Log Analytics for regional session hosts

### Transition Guidance

- Both geographical and regional host pools will co-exist during rollout.
- Plan to migrate existing geographical host pools to regional for improved reliability.
- Only objects of the same deployment scope can be associated (e.g., regional application groups with regional workspaces).
- Microsoft will release migration tooling in the future.

## Global Rollout

- Additional Azure regions will support regional host pools over time.
- Stay updated via the [Azure Virtual Desktop Tech Community](https://techcommunity.microsoft.com/t5/azure-virtual-desktop/bd-p/AzureVirtualDesktopForum).
- Further documentation: [Regional Host Pools](https://aka.ms/regionalhostpools)

## Summary

Regional host pools make Azure Virtual Desktop deployments more resilient and compliant. Administrators are encouraged to use the new model for all future pools and monitor for region coverage expansion and future migration tools.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-virtual-desktop-blog/now-in-public-preview-azure-virtual-desktop-regional-host-pools/ba-p/4474598)
