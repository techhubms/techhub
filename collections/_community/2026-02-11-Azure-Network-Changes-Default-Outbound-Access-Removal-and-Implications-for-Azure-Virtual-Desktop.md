---
layout: "post"
title: "Azure Network Changes: Default Outbound Access Removal and Implications for Azure Virtual Desktop"
description: "This post by Kathryn_Jakubek covers the upcoming removal of default outbound internet access (DOA) for new Azure Virtual Networks after March 31, 2026. It details how Azure Virtual Desktop deployments will need explicit outbound connectivity configurations, outlines recommended practices, supported methods such as NAT Gateway and Azure Standard Load Balancer, possible limitations, and provides technical resources for network and security practitioners."
author: "Kathryn_Jakubek"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-virtual-desktop/azure-s-default-outbound-access-changes-guidance-for-azure/m-p/4494462#M14000"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-02-11 22:31:27 +00:00
permalink: "/2026-02-11-Azure-Network-Changes-Default-Outbound-Access-Removal-and-Implications-for-Azure-Virtual-Desktop.html"
categories: ["Azure", "Security"]
tags: ["Azure", "Azure Firewall", "Azure Load Balancer", "Azure Virtual Desktop", "Azure Virtual Network", "Community", "Default Outbound Access", "Egress Control", "Explicit Outbound Connectivity", "Microsoft Azure", "NAT Gateway", "Network Security", "Networking", "Outbound Internet Access", "Private Subnet", "Public IP", "Security", "Session Hosts", "VNet"]
tags_normalized: ["azure", "azure firewall", "azure load balancer", "azure virtual desktop", "azure virtual network", "community", "default outbound access", "egress control", "explicit outbound connectivity", "microsoft azure", "nat gateway", "network security", "networking", "outbound internet access", "private subnet", "public ip", "security", "session hosts", "vnet"]
---

Kathryn_Jakubek discusses critical Azure networking changes affecting Azure Virtual Desktop customers, focusing on explicit outbound connectivity requirements and security considerations for new VNets.<!--excerpt_end-->

# Azure’s Default Outbound Access Changes: Guidance for Azure Virtual Desktop Customers

## Overview

After March 31, 2026, Azure will no longer enable Default Outbound Access (DOA) for newly created Virtual Networks (VNets). Azure Virtual Desktop customers must now configure outbound connectivity explicitly, which impacts deployment and connectivity of session hosts.

## What is Default Outbound Access (DOA)?

Historically, Azure allowed resources in VNets to access the public internet without explicit egress configuration. This supported telemetry, Windows activation, updates, and service dependencies by default.

## What’s Changing?

- **DOA Disabled by Default**: New VNets created after March 31, 2026, will not have DOA. Instead, they’ll default to the Private Subnet option, restricting internet access for enhanced isolation and compliance.
- **Restoring DOA**: Admins may restore DOA manually by disabling the Private Subnet option, but Microsoft recommends using a NAT Gateway for outbound internet access.

## Who is Impacted?

Azure Virtual Desktop (AVD) customers deploying new VNets after the effective date. Existing VNets keep current outbound access settings.

## Required Actions

- **Update Deployment Plans**: Plan for explicit NAT (e.g., NAT Gateway), or restore DOA (not recommended) by disabling Private Subnet.
- **Test Connectivity**: Ensure all service dependencies are reachable via new outbound methods.

## Supported Outbound Methods

1. **NAT Gateway** (Recommended): Provides reliable, scalable outbound internet connectivity but does not support direct UDP peer-to-peer via STUN.
2. **Azure Standard Load Balancer**: Supports UDP connectivity over STUN.
3. **Public IP Address on VM**: Assigns public IP for direct external access.
4. **Azure Firewall or Third-party NVA**: Advanced security, but not recommended for RDP or long-lived connections due to possible disconnects during scale-in.

**Note:** For more on specific pros/cons, see the [Default Outbound Access documentation](https://learn.microsoft.com/en-us/azure/virtual-network/ip-services/default-outbound-access#how-can-i-disable-default-outbound-access).

## Security Considerations

- **Private Subnet**: Enables isolation and compliance by default; explicit configuration improves intentional security posture.
- **Session Host Requirements**: Outbound internet is required for AVD host pools to function and connect to Microsoft services.
- **STUN/TURN Behavior**: NAT Gateway uses symmetric NAT, which can block peer-to-peer connections over UDP (STUN). Azure Standard Load Balancer supports these scenarios.
- **Firewall Recommendations**: Avoid routing RDP/long-lived connections through Azure Firewall or NVAs; use NAT Gateway or similar direct methods.

## FAQ

- **Does this affect existing VNets?** No, only new VNets after March 31, 2026.
- **What if outbound is not set up?** Host pool will not deploy; connectivity will fail. Explicit outbound (e.g., NAT Gateway) is required.
- **Why is outbound internet needed?** AVD session hosts must contact Microsoft endpoints for proper operation. [Required endpoints](https://learn.microsoft.com/azure/virtual-desktop/required-fqdn-endpoint?tabs=azure)
- **Resources:**
  - [Azure updates](https://azure.microsoft.com/en-us/updates?id=492953)
  - [Default Outbound Access in Azure](https://learn.microsoft.com/en-us/azure/virtual-network/ip-services/default-outbound-access#how-can-i-disable-default-outbound-access)
  - [Configure Azure Firewall for AVD](https://learn.microsoft.com/en-us/azure/firewall/protect-azure-virtual-desktop?context=/azure/virtual-desktop/context/context)
  - [Quickstart: Create a NAT Gateway](https://learn.microsoft.com/en-us/azure/nat-gateway/quickstart-create-nat-gateway)

## Wrap-up

Azure’s networking policy change moves towards intentional, secure network architecture. Azure Virtual Desktop customers should proactively implement and test explicit outbound connectivity to maintain reliable operations and security compliance for new deployments.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-virtual-desktop/azure-s-default-outbound-access-changes-guidance-for-azure/m-p/4494462#M14000)
