---
layout: post
title: 'Public Preview: Auto Agent Upgrade for Azure Arc-Enabled Servers'
author: Aurnov_Chattopadhyay
canonical_url: https://techcommunity.microsoft.com/t5/azure-arc-blog/public-preview-auto-agent-upgrade-for-azure-arc-enabled-servers/ba-p/4442556
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community
date: 2025-08-12 15:30:41 +00:00
permalink: /azure/community/Public-Preview-Auto-Agent-Upgrade-for-Azure-Arc-Enabled-Servers
tags:
- Agent Lifecycle Management
- Auto Agent Upgrade
- Automatic Upgrades
- Azure
- Azure Arc
- Azure CLI
- Azure Portal
- Community
- Connected Machine Agent
- DevOps
- Hybrid Cloud
- Hybrid Servers
- Infrastructure Automation
- Operational Consistency
- PowerShell
- Server Management
section_names:
- azure
- devops
---
Aurnov Chattopadhyay introduces the Auto Agent Upgrade feature for Azure Arc-enabled servers, designed to automate and simplify agent updates across hybrid environments.<!--excerpt_end-->

# Public Preview: Auto Agent Upgrade for Azure Arc-Enabled Servers

**Author:** Aurnov Chattopadhyay

## Overview

The Auto Agent Upgrade feature for Azure Arc-enabled servers is now available in public preview. This addition streamlines agent lifecycle management by automating the upgrade process for the Connected Machine agent, ensuring that servers—whether in Azure, on-premises, or on the edge—are consistently kept up to date with the latest supported version.

## Key Features

- **Automatic Upgrade:** Agents can automatically upgrade without manual intervention or scripting.
- **Easy Enablement:** Activate via Azure CLI or PowerShell by setting `enableAutomaticUpgrade` to `true`.
- **Managed Rollout:** Upgrades occur in controlled batches, maintaining stability across regions.
- **Upgrade Tracking:** Monitor upgrade status through the Azure Portal under the `agentUpgrade` property.
- **Version Requirements:** Available for agents running version 1.48 or newer.

## Example: Enabling Auto Agent Upgrade via PowerShell

```powershell
Set-AzContext -Subscription "YOUR SUBSCRIPTION"

$params = @{
    ResourceGroupName = "YOUR RESOURCE GROUP"
    ResourceProviderName = "Microsoft.HybridCompute"
    ResourceType = "Machines"
    ApiVersion = "2024-05-20-preview"
    Name = "YOUR MACHINE NAME"
    Method = "PATCH"
    Payload = '{"properties":{"agentUpgrade":{ "enableAutomaticUpgrade":true}}}'
}

Invoke-AzRestMethod @params
```

## Benefits

- **Reduces operational risk** by minimizing manual patching and related errors.
- **Improves consistency** across hybrid estates by keeping agent versions aligned.
- **Minimizes disruption**, ensuring upgrades are seamless and workloads remain unaffected.

## Getting Started

To learn more and begin using Auto Agent Upgrade, visit [https://aka.ms/AzureConnectedMachineAgent](https://aka.ms/AzureConnectedMachineAgent).

*This feature is only available on agents running version 1.48 or greater.*

---

Published Aug 12, 2025  
Version 1.0

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-arc-blog/public-preview-auto-agent-upgrade-for-azure-arc-enabled-servers/ba-p/4442556)
