---
layout: "post"
title: "Preparing for Azure Relay IP Address and DNS Changes"
description: "This community post explains the upcoming changes to Azure Relay's IP addresses and DNS naming conventions. It details the steps organizations need to take to update their network configurations and firewall rules, how to use a PowerShell script to automate namespace information retrieval, and what changes to expect throughout the transition."
author: "ashish-chhabria"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/upcoming-changes-to-azure-relay-ip-addresses-and-dns-support/ba-p/4463597"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-10-22 20:06:09 +00:00
permalink: "/2025-10-22-Preparing-for-Azure-Relay-IP-Address-and-DNS-Changes.html"
categories: ["Azure"]
tags: ["Automation", "Azure", "Azure Relay", "Community", "DNS", "Firewall", "Hybrid Cloud", "Hybrid Connections", "IP Address Management", "Microsoft Azure", "Namespace", "Network Configuration", "PowerShell", "Service Bus", "WCF Relay"]
tags_normalized: ["automation", "azure", "azure relay", "community", "dns", "firewall", "hybrid cloud", "hybrid connections", "ip address management", "microsoft azure", "namespace", "network configuration", "powershell", "service bus", "wcf relay"]
---

ashish-chhabria provides a practical overview of essential updates to Azure Relay's IP addressing and DNS naming. The post walks through required user actions—including updating allow lists and leveraging a provided PowerShell script—to ensure continued connectivity and security.<!--excerpt_end-->

# Preparing for Azure Relay IP Address and DNS Changes

Azure Relay is a crucial service for connecting on-premises infrastructure with the Azure cloud. Microsoft has announced important updates to the IP addresses and DNS naming standards for Azure Relay, which will impact all organizations using WCF Relay and Hybrid Connections.

## What's Changing?

- **IP and DNS Name Transitions:**
  - The IP addresses and DNS names for Azure Relay endpoints will change during the transition.
  - Example: `g0-prod-bn-vaz0001-sb.servicebus.windows.net` may become `gv0-prod-bn-vaz0001-sb.servicebus.windows.net`.
- **Enhanced DNS Support:**
  - Updates are being introduced to provide more reliable DNS resolution for both WCF Relay and Hybrid Connections.

Links to official details:

- [IP-addresses for Azure Relay](https://techcommunity.microsoft.com/blog/messagingonazureblog/upcoming-changes-to-ip-addresses-for-azure-relay/3285254)
- [Azure Relay WCF and Hybrid Connections DNS Support](https://techcommunity.microsoft.com/blog/messagingonazureblog/azure-relay-wcf-and-hybrid-connections-dns-support/370775)

## What Should Customers Do?

- **Update Allow Lists:**
  - Review your network firewalls and security group configurations to permit the new Azure Relay IP ranges and DNS endpoints as indicated by Microsoft documentation.
- **Monitor the Transition:**
  - Expect two rounds of changes. You should track both initial and final transitions and update your configurations accordingly.

## Automating Namespace Information Retrieval

To make the update process simpler, Microsoft has released an updated PowerShell script for retrieving namespace information that reflects the upcoming changes.

- GitHub Link: [GetNamespaceInfo.ps1 (azure-relay-dotnet/tools)](https://github.com/Azure/azure-relay-dotnet/blob/dev/tools/GetNamespaceInfo.ps1)
- Usage instructions are in the [README](https://github.com/Azure/azure-relay-dotnet/blob/dev/tools/Readme.md)

This script allows you to:

- Quickly look up your Azure Relay namespace configuration
- Validate access against current and future endpoints and IP address ranges

### Sample Script Output

```
PS D:\AzureVMSSEssentials\Tools\GetNamespaceInfoWithIpRanges> .\GetNamespaceInfo.ps1 <your-relay-namespace>.servicebus.windows.net
Namespace : <your-relay-namespace>.servicebus.windows.net
Deployment : PROD-BN-VAZ0001
ClusterDNS : ns-prod-bn-vaz0001.eastus2.cloudapp.azure.com
ClusterRegion : eastus2
ClusterVIP : 40.84.75.3
GatewayDnsFormat : g{0}-bn-vaz0001-sb.servicebus.windows.net or gv{0}-bn-vaz0001-sb.servicebus.windows.net
Notes : Entries with 'FUTURE' IPAddress may be added at a later time as needed
Current IP Ranges
Name                             IPAddress
----                             ---------
g0-bn-vaz0001-sb.servicebus.windows.net 20.36.144.8
g1-bn-vaz0001-sb.servicebus.windows.net 20.36.144.1
... (truncated for clarity)
Future IP Ranges for Region:eastus2
addressPrefixes
---------------
1. 18.130.0/23
2. 18.132.0/26
3. 18.132.64/27
```

## Key Takeaways

- Make sure your organization's firewall and allow lists are updated in a timely manner to prevent any service disruption.
- Use the provided PowerShell script to automate validation and reduce the risk of configuration drift.
- Both current and future IP ranges should be reviewed as part of your change management process.

---

*Author: ashish-chhabria – Microsoft, [Messaging on Azure Blog](https://techcommunity.microsoft.com/category/azure/blog/messagingonazureblog)*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/upcoming-changes-to-azure-relay-ip-addresses-and-dns-support/ba-p/4463597)
