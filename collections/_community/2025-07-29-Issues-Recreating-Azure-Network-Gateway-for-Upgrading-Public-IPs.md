---
layout: "post"
title: "Issues Recreating Azure Network Gateway for Upgrading Public IPs"
description: "Dave_PW describes challenges faced while recreating an Azure network gateway to upgrade a public IP used in an IPsec tunnel. The post covers attempts to migrate resources, connectivity failures after deploying a new gateway in a separate virtual network, and considerations about downtime and fallback options."
author: "Dave_PW"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/AZURE/comments/1mcaooo/azure_network_gateway_issue_recreating/"
viewing_mode: "external"
feed_name: "Reddit Azure"
feed_url: "https://www.reddit.com/r/azure/.rss"
date: 2025-07-29 12:55:30 +00:00
permalink: "/2025-07-29-Issues-Recreating-Azure-Network-Gateway-for-Upgrading-Public-IPs.html"
categories: ["Azure"]
tags: ["Azure", "Azure Network Gateway", "Azure Networking", "Community", "Connectivity Issues", "Downtime", "Firewall Rule", "FortiGate", "IPsec Tunnel", "NSG", "Public IP Upgrade", "Virtual Network"]
tags_normalized: ["azure", "azure network gateway", "azure networking", "community", "connectivity issues", "downtime", "firewall rule", "fortigate", "ipsec tunnel", "nsg", "public ip upgrade", "virtual network"]
---

Authored by Dave_PW, this post discusses the challenges with recreating an Azure network gateway for a public IP upgrade, analyzing connectivity and maintenance implications.<!--excerpt_end-->

## Recreating Azure Network Gateway: Upgrade and Connectivity Challenges

**Author: Dave_PW**

### Background

Dave_PW received a notification to upgrade some public IPs to 'standard' in Azure, including one assigned to the network gateway managing the IPsec tunnel between on-premises infrastructure and Azure.

### Upgrade Limitation

- It is not possible to temporarily disassociate the existing public IP from the Azure gateway for the upgrade.
- Research revealed the only viable approach was to create a new gateway with a new public IP.

### Attempted Solution

- A new gateway was created with a new public IP.
- The Azure portal did not allow using the original virtual network (VNet) since it was already linked to the existing gateway.
- A new VNet was created, matching the original address range and subnet.
- The cloned gateway and connection were provisioned and connected to the on-premises FortiGate device.

### Resulting Issue

- Directing traffic through the new tunnel fails to provide connectivity to Azure resources.
- Testing included an 'allow any' firewall rule in the Network Security Group (NSG) for one VM—still no access.

### Analysis & Constraints

- The likely cause: For successful connectivity, the gateway must reside in the same VNet as the resources.
- Unfortunately, Azure does not support changing the VNet for an existing gateway.
- The only known solution: Remove the old gateway (incurring downtime), then create a new gateway attached to the existing VNet.
- Downtime is estimated at around 30 minutes or more, depending on Azure’s provisioning speed.
- This approach removes fallback capability if issues arise during migration.

### Call for Advice

Dave_PW asks if there are any overlooked options or better migration strategies before proceeding with the downtime-prone migration.

---

> *Discussion and advice on minimizing downtime or alternative migration methods are welcomed by the author, especially from those with experience in similar Azure scenarios.*

This post appeared first on "Reddit Azure". [Read the entire article here](https://www.reddit.com/r/AZURE/comments/1mcaooo/azure_network_gateway_issue_recreating/)
