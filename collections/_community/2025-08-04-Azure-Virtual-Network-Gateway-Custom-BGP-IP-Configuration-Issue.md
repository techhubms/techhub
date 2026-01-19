---
external_url: https://www.reddit.com/r/AZURE/comments/1mh967w/azure_virtual_network_gateway_with_custom_bgp/
title: 'Azure Virtual Network Gateway: Custom BGP IP Configuration Issue'
author: FewActivity9721
viewing_mode: external
feed_name: Reddit Azure
date: 2025-08-04 09:52:09 +00:00
tags:
- Azure Virtual Network Gateway
- BGP
- BGP Peer IP
- Custom APIPA IP
- GatewaySubnet
- Get AzVirtualNetworkGateway
- IPSEC Tunnel
- Network Configuration
- PowerShell
- Troubleshooting
section_names:
- azure
---
Author FewActivity9721 describes a problem configuring custom BGP IP addresses on an Azure Virtual Network Gateway, impacting IPSEC tunnel setup.<!--excerpt_end-->

## Azure Virtual Network Gateway and Custom BGP IP Configuration Issue

Author: FewActivity9721

### Background

The author describes creating an Azure Virtual Network Gateway through the Azure portal (GUI) and enabling Border Gateway Protocol (BGP) as part of the setup. During this process, they set custom APIPA BGP IP addresses within the standard 169.x.x.x range, which is used for BGP peering across VPN connections.

### Problem Statement

After the gateway configuration, running the following PowerShell command:

```powershell
Get-AzVirtualNetworkGateway -Name XXXX -ResourceGroupName "XXXX" | Select-Object -ExpandProperty BgpSettings
```

returns the default BGP Peer IP address from the GatewaySubnet, instead of the user-specified custom BGP IP address.

### Impact

This behavior is causing difficulties when attempting to establish an IPSEC tunnel with a third-party organization. BGP peering is not successfully established, and the third party has identified the unexpected BGP peer IP address as a likely issue.

### Key Points

- **Azure Virtual Network Gateway**: Configured via the portal, with BGP enabled and custom BGP APIPA IP addresses assigned.
- **PowerShell Query**: Use of `Get-AzVirtualNetworkGateway` and `Select-Object -ExpandProperty BgpSettings` only shows the default, not custom, BGP peer IP.
- **Operational Effect**: BGP peering over the IPSEC tunnel isn’t working as expected, possibly due to a mismatch in expected BGP peer IP addresses.
- **Third-party Feedback**: The external partner suspects that the default BGP Peer IP being used is the root cause of the peering problem.

### Request for Guidance

The author is seeking practical insights or confirmation whether seeing only the default BGP Peer IP (instead of a custom one) is the expected behavior in Azure, and, if not, how this configuration issue can be resolved to enable BGP peering over the IPSEC tunnel.

This post appeared first on "Reddit Azure". [Read the entire article here](https://www.reddit.com/r/AZURE/comments/1mh967w/azure_virtual_network_gateway_with_custom_bgp/)
