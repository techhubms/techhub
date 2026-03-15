---
external_url: https://techcommunity.microsoft.com/t5/azure-arc-blog/announcing-general-availability-of-software-defined-networking/ba-p/4467579
title: General Availability of Software Defined Networking (SDN) on Azure Local with Azure Arc
author: varunlakshmanan
feed_name: Microsoft Tech Community
date: 2025-11-06 16:24:37 +00:00
tags:
- Azure Arc
- Azure CLI
- Azure Local
- Azure Portal
- Azure Resource Manager
- Disaster Recovery
- Failover Cluster
- Hybrid Cloud
- Network Controller
- Network Policy
- Network Security Groups
- NSG
- SDN
- Software Defined Networking
- Virtual Networks
- VLAN
- Azure
- Security
- Community
section_names:
- azure
- security
primary_section: azure
---
varunlakshmanan announces the general availability of Software Defined Networking (SDN) on Azure Local, enabled by Azure Arc, detailing the new network management, security, and operational features for hybrid environments.<!--excerpt_end-->

# General Availability of Software Defined Networking (SDN) on Azure Local with Azure Arc

**Author:** varunlakshmanan

## Overview

With the release of Azure Local version 2510, Software Defined Networking (SDN) is now generally available on Azure Local, enabled by Azure Arc. This capability brings cloud-native networking capabilities to on-premises environments, allowing users to manage logical networks, network interfaces, and Network Security Groups (NSGs) through the familiar Azure control plane (including Azure Portal, Azure CLI, and ARM templates).

## Key Highlights

- **Centralized Network Management:** Logical networks, network interfaces, and NSGs can be centrally managed using Azure tools.
- **Fine-grained Traffic Control:** Policy-driven inbound and outbound rules through NSGs, protecting edge workloads just like in public Azure.
- **Seamless Hybrid Consistency:** Operations staff can leverage the same tools and constructs across Azure public cloud and Azure Local, minimizing training and onboarding friction.

## Feature Capabilities

- **SDN Control Plane:** Runs as a Failover Cluster service on Azure Local physical hosts (no VMs required).
- **Logical Network Deployment:** Support for VLAN-backed networks in datacenter environments, integrated with SDN via Azure Arc.
- **VM Network Interfaces:** Assignment of static or DHCP IPs to VMs from logical networks.
- **NSG Application:** Create, attach, and manage NSGs directly from Azure, applying either generic VLAN rules or fine-grained rules to individual VM interfaces using full 5-tuple control (source/destination IP, port, protocol).
- **Default Network Policies:** Apply baseline security policies at VM network interface creation (e.g., open inbound HTTP while blocking others, allowing outbound traffic, or referencing existing NSGs).
- **Disaster Recovery:** Azure Arc Resource Bridge (ARB) allows NSGs and rules to be recovered alongside VMs and associated resources if cluster recovery is required.

## Management Modes

There are currently two ways to manage SDN on Azure Local:

- **SDN Enabled by Azure Arc:** Brings Azure-style management and control but currently does not support all SDN features (vNETs, SLBs, Gateways).
- **On-Premises SDN Tools:** For workloads requiring missing features, use SDN managed by on-premises tools like [SDN Express (PowerShell)](https://learn.microsoft.com/en-us/azure/azure-local/deploy/sdn-express-23h2?view=azloc-2510) or [Windows Admin Center (WAC)](https://learn.microsoft.com/en-us/azure/azure-local/deploy/sdn-wizard-23h2?view=azloc-2510). These provide full-stack SDN (including SLBs, Gateways, VNET peering).

**Note:** Only one mode of SDN management can be active at a time—hybrid management is not supported. For important considerations, review the [official documentation](https://learn.microsoft.com/en-us/azure/azure-local/concepts/sdn-overview?view=azloc-2510#important-considerations).

## Community and Next Steps

The release of SDN on Azure Local was made possible by feedback and innovation from the community. Microsoft encourages users to try the feature and share experiences.

Learn more in the [Azure Local documentation](https://learn.microsoft.com/en-us/azure/azure-local/concepts/sdn-overview?view=azloc-2510).

---

*Published Nov 06, 2025*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-arc-blog/announcing-general-availability-of-software-defined-networking/ba-p/4467579)
