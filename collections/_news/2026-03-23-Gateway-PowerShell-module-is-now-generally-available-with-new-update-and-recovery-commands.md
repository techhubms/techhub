---
tags:
- Automation
- Cmdlets
- Data Integration
- DevOps
- DevOps Workflows
- Disaster Recovery
- Enterprise Administration
- Gateway PowerShell Module
- Lifecycle Management
- Microsoft Fabric
- Microsoft Learn
- ML
- News
- On Premises Data Gateway
- PowerShell
- Scripting
- Update Management
- Version Management
- VNet Data Gateway
date: 2026-03-23 09:00:00 +00:00
primary_section: ml
external_url: https://blog.fabric.microsoft.com/en-US/blog/powershell-module-for-gateways-with-expanded-automation-capabilities-generally-available/
title: Gateway PowerShell module is now generally available, with new update and recovery commands
author: Microsoft Fabric Blog
section_names:
- devops
- ml
feed_name: Microsoft Fabric Blog
---

Microsoft Fabric Blog announces that the PowerShell module for managing on-premises and VNet data gateways is now generally available, adding production support plus new cmdlets for updates, version visibility, and restore scenarios to enable reliable automation in enterprise environments.<!--excerpt_end-->

# Gateway PowerShell module is now generally available, with new update and recovery commands

*If you haven’t already, check out Arun Ulag’s hero blog “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for broader announcements across Fabric and database offerings: https://aka.ms/FabCon-SQLCon-2026-news*

The **PowerShell module for gateways** is now out of preview and into **production-grade (generally available) support**. It provides a supported command surface for managing:

- **On-premises data gateways**
- **VNet data gateways**

…with a focus on **automation and scripting** for large-scale and regulated environments.

## What it means for administrators and enterprises

This GA milestone is positioned as “ready for broad production use”, emphasizing:

- Full production support
- Stable and validated command behaviors
- Broader lifecycle coverage
- Improved error handling and reliability
- Better alignment with enterprise automation and DevOps workflows

It enables IT teams to standardize gateway operations with scripts/automation pipelines instead of manual UI-driven steps.

![Screenshot of commands from the Gateway PowerShell module used to manage and monitor gateway operations.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-commands-from-the-gateway-powershell.png)

*Figure: Using the Gateway PowerShell module to manage gateway operations from the command line.*

## New PowerShell commands

This release adds cmdlets for update control, version visibility, and recovery.

### Update-DataGatewayClusterMember

Triggers gateway member updates when a newer version is available.

New parameters:

- **TargetVersion** — update to a specified gateway version
- **CheckStatus** — retrieve latest update status to track progress

### Get-DataGatewayAvailableUpdates

Retrieves the **latest six available gateway versions** to support upgrade planning.

New parameters:

- **GatewayClusterId** — check versions available for a cluster
- **MemberGatewayId** — check versions for a specific gateway member

If no versions are returned, the gateway is already up to date.

### Restore-DataGatewayClusterMember

Restores an existing gateway member to accelerate recovery and replacement scenarios.

## Command improvements

### Install-DataGateway

- Added **TargetDirectory** to support custom installation locations (helps align with enterprise server standards)

### Get-DataGatewayDatasourceStatus

- Fixed failures when datasources are offline
- Improved reliability for troubleshooting and monitoring scripts

### Add-DataGatewayClusterDatasourceUser

- Deprecated email-address-based user specification in favor of stronger identity-based approaches

### Module quality improvements

- Fixed **module version display** issues for accurate reporting and environment validation across automation workflows

## What’s ahead

Microsoft says it will keep expanding gateway automation capabilities, command coverage, and admin experiences across UI, API, and scripting surfaces.

Documentation:

- Gateway PowerShell documentation and cmdlet reference (Microsoft Learn): https://learn.microsoft.com/data-integration/gateway/service-gateway-powershell-support?toc=%2Ffabric%2Fdata-factory%2Ftoc.json&bc=fabric%2Fdata-factory%2Fbreadcrumb%2Ftoc.json


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/powershell-module-for-gateways-with-expanded-automation-capabilities-generally-available/)

