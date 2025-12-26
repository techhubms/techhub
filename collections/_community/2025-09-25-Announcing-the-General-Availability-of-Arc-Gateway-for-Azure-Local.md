---
layout: "post"
title: "Announcing the General Availability of Arc Gateway for Azure Local"
description: "This article introduces the General Availability of the Arc Gateway for Azure Local, detailing how it simplifies and secures connectivity between on-premises or edge environments and Azure. It explains reduced firewall complexity, secure traffic management, integration with Azure Local VMs and AKS clusters, enterprise proxy support, and provides documentation for deployment and configuration."
author: "Cristian Edwards Sabathe"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-arc-blog/announcing-the-general-availability-of-arc-gateway-for-azure/ba-p/4456256"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-09-25 13:15:23 +00:00
permalink: "/community/2025-09-25-Announcing-the-General-Availability-of-Arc-Gateway-for-Azure-Local.html"
categories: ["Azure", "Security"]
tags: ["AKS", "Arc Gateway", "Azure", "Azure Arc", "Azure Local", "Cloud Integration", "Community", "Connectivity", "Edge Computing", "Enterprise Proxy", "Firewall Management", "General Availability", "HTTPS Traffic", "Hybrid Cloud", "Microsoft Azure", "Network Security", "Security", "Virtual Machines"]
tags_normalized: ["aks", "arc gateway", "azure", "azure arc", "azure local", "cloud integration", "community", "connectivity", "edge computing", "enterprise proxy", "firewall management", "general availability", "https traffic", "hybrid cloud", "microsoft azure", "network security", "security", "virtual machines"]
---

Cristian Edwards Sabathe announces the general availability of Arc Gateway for Azure Local, explaining its impact on secure and unified connectivity for on-premises and edge environments connecting to Azure.<!--excerpt_end-->

# Announcing the General Availability of Arc Gateway for Azure Local

**Author:** Cristian Edwards Sabathe, Azure Local Principal Product Manager

## Overview

Microsoft has announced the General Availability (GA) of the Arc Gateway for Azure Local. This milestone improves how organizations connect their on-premises and edge infrastructure with Azure, delivering simplified, secure, and efficient traffic management.

## Key Features

- **Unified Secure Traffic:** All outbound HTTPS traffic from Azure Local instances and workloads routes through a single, centralized gateway, removing the need for complex and numerous firewall rules.
- **Fewer Endpoints:** The number of required outbound endpoints is reduced from over 100 to fewer than 28, making firewall configuration easier and improving security posture.
- **Comprehensive Workload Integration:** Arc Gateway supports not just infrastructure endpoints but also Azure Local VMs and AKS clusters (currently in preview for AKS), allowing broad hybrid estate connectivity.
- **Enterprise Proxy Support:** Arc Gateway integrates with existing enterprise proxies, ensuring that outbound Azure-bound traffic can utilize established proxy solutions before reaching Azure.

## Technical Deep Dive

Organizations previously faced challenges with complex firewall requirements and sprawling outbound rules to enable hybrid Azure connectivity. Arc Gateway addresses this by consolidating outbound points of egress, thus minimizing potential security risks and simplifying overall configuration.

To learn more about the technical aspects, see the [detailed deep dive](https://github.com/Azure/AzureLocal-Supportability/blob/main/TSG/Networking/Arc-Gateway-Outbound-Connectivity/DeepDive-ArcGateway-Outbound-Traffic.md).

## Frequently Asked Questions

- **Is it possible to enable Arc Gateway on existing Azure Local clusters?**
  - Microsoft is working towards supporting this in a future Azure Local release.
- **Can Arc Gateway be used for Azure Local VMs if not enabled at deployment?**
  - Yes, if you have a working Arc Gateway resource, you can deploy or attach Azure Local VMs, provided guest management is enabled.
- **Can AKS clusters on Azure Local use the Arc Gateway?**
  - Yes, if enabled during infrastructure deployment, AKS clusters will use the Arc Gateway (currently in Public Preview until future GA).

## Getting Started

Explore documentation to set up and manage Arc Gateway for Azure Local:

- [Overview of Azure Arc gateway for Azure Local - Microsoft Learn](https://learn.microsoft.com/en-us/azure/azure-local/deploy/deployment-azure-arc-gateway-overview?view=azloc-2508&tabs=portal)
- [Register Azure Local using Arc gateway - Microsoft Learn](https://learn.microsoft.com/en-us/azure/azure-local/deploy/deployment-with-azure-arc-gateway?view=azloc-2508&tabs=script&pivots=register-proxy)
- [Create Azure Local virtual machines using Arc gateway - Microsoft Learn](https://learn.microsoft.com/en-us/azure/azure-local/manage/create-arc-virtual-machines?view=azloc-2508&tabs=azurecli#create-a-vm-with-arc-gateway-configured)
- [Create AKS cluster in Azure Local with Arc gateway - Microsoft Learn](https://learn.microsoft.com/en-us/azure/aks/aksarc/arc-gateway-aks-arc)

## Conclusion

The Arc Gateway for Azure Local is a significant advancement for hybrid and edge connectivity, offering organizations a secure, manageable, and scalable way to connect their resources to Azure. For more technical details and deployment instructions, refer to the provided links.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-arc-blog/announcing-the-general-availability-of-arc-gateway-for-azure/ba-p/4456256)
