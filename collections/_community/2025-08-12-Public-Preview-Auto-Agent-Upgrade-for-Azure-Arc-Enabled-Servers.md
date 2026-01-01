---
layout: "post"
title: "Public Preview: Auto Agent Upgrade for Azure Arc-Enabled Servers"
description: "This announcement introduces Auto Agent Upgrade for Azure Arc-enabled servers, a feature aimed at automating the lifecycle management of the Connected Machine agent across hybrid environments. The feature allows administrators to enable automatic upgrades for agents using Azure CLI or PowerShell, ensuring servers are kept current with minimal manual effort. Available in public preview for version 1.48 or above, this upgrade mechanism reduces operational overhead, maintains consistency, and minimizes the risk of running outdated agents in Azure, on-premises, or edge scenarios. Sample CLI details and guidance for adoption are provided."
author: "Aurnov_Chattopadhyay"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-arc-blog/public-preview-auto-agent-upgrade-for-azure-arc-enabled-servers/ba-p/4442556"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-12 15:30:41 +00:00
permalink: "/2025-08-12-Public-Preview-Auto-Agent-Upgrade-for-Azure-Arc-Enabled-Servers.html"
categories: ["Azure", "DevOps"]
tags: ["Agent Lifecycle Management", "Auto Agent Upgrade", "Automatic Upgrades", "Azure", "Azure Arc", "Azure CLI", "Azure Portal", "Community", "Connected Machine Agent", "DevOps", "Hybrid Cloud", "Hybrid Servers", "Infrastructure Automation", "Operational Consistency", "PowerShell", "Server Management"]
tags_normalized: ["agent lifecycle management", "auto agent upgrade", "automatic upgrades", "azure", "azure arc", "azure cli", "azure portal", "community", "connected machine agent", "devops", "hybrid cloud", "hybrid servers", "infrastructure automation", "operational consistency", "powershell", "server management"]
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
