---
external_url: https://techcommunity.microsoft.com/t5/azure-networking/spoke-hub-hub-traffic-with-vpn-gateway-bgp-and-firewall-issue/m-p/4471878#M750
title: Troubleshooting Azure Firewall and VPN Gateway Routing in Hub-Spoke Topology
author: CUrti300
feed_name: Microsoft Tech Community
date: 2025-11-20 18:04:09 +00:00
tags:
- Azure Firewall
- Azure Hybrid Connectivity
- Azure Networking
- BGP
- Connectivity Issues
- Hub Spoke Architecture
- Inbound Private Resolver
- Network Security
- Network Segmentation
- Route Propagation
- Routing Troubleshooter
- UDR
- User Defined Routing
- Virtual Appliance
- Virtual Network
- VPN Gateway
- Azure
- Security
- Community
section_names:
- azure
- security
primary_section: azure
---
CUrti300 shares real-world troubleshooting experience with Azure VPN Gateway, BGP, and Firewall in a hub-spoke environment, highlighting obstacles with custom routing and inspection.<!--excerpt_end-->

# Troubleshooting Azure Firewall and VPN Gateway Routing in Hub-Spoke Topology

**Author:** CUrti300

## Scenario Overview

This post describes challenges in implementing Azure Firewall inspection for VPN gateway connectivity between virtual networks using a dual hub-spoke architecture with BGP-enabled VPN Gateways.

## Issue Encountered

- Azure Firewall was introduced for inspection across VNET-VNET traffic.
- Traffic path works as expected from **SpokeA → HubA Firewall → HubA VPN → HubB VPN → SpokeB**.
- Routing fails when targeting destinations like **HubB VM** or **Inbound Resolver**; according to the Azure Connectivity Troubleshooter, traffic reaches **HubA VPN** but encounters a **Local Error: RouteMissing**.

## Observations & Troubleshooting

- Issue likely caused by UDR changes meant to force inspection through Azure Firewall.
- Previous configuration (without UDRs) functioned correctly.
- Multiple UDR variations tested on Gateway Subnet with no resolution.
- The problem is reproducible in other hub-spoke hybrid environments, pointing to a pattern tied to firewall and routing design.

## Current Network Configuration

### Hub-Spoke-A

**Subnets and Resources:**

- VPN Gateway (GatewaySubnet)
- Azure Firewall (AzureFirewallSubnet)
- Inbound Private Resolver (PrivateResolverSubnet)
- Virtual Machine (VM Subnet)

**GatewaySubnet UDRs:**

- Prefix: Hub-B → Next Hop: Virtual Appliance (**Hub-A Firewall**)
- Prefix: Spoke-B → Next Hop: Virtual Appliance (**Hub-A Firewall**)
- Route propagation enabled

### Hub-Spoke-B

**Subnets and Resources:**

- VPN Gateway (GatewaySubnet)
- Azure Firewall (AzureFirewallSubnet)
- Inbound Private Resolver (PrivateResolverSubnet)
- Virtual Machine (VM Subnet)

**GatewaySubnet UDRs:**

- Prefix: Hub-A → Next Hop: Virtual Appliance (**Hub-B Firewall**)
- Prefix: Spoke-A → Next Hop: Virtual Appliance (**Hub-B Firewall**)
- Route propagation enabled

### Spoke Subnets UDRs

- Default: 0.0.0.0/0 → Next Hop: Virtual Appliance (respective Hub Firewall)
- Route propagation enabled

## Technical Highlights

- BGP is enabled between VPN gateways for dynamic route management.
- Route troubleshooting points to UDR conflicts or Azure limitations when mixing forced firewall inspection with BGP-learned VPN transit routes.
- Custom UDRs on gateway subnets may override BGP routes, affecting connectivity to resources behind VPN gateways.

## Summary

This setup illustrates the complexity of Azure networking when combining hub-spoke topology, VPN Gateway (with BGP), and Azure Firewall. Introducing UDRs for forced inspection can unintentionally break previously working routes, especially for inter-hub-to-VM or resolver scenarios. Careful review of UDR precedence, route propagation, and Azure constraints is needed.

**Discussion is open for suggested routing strategies, design patterns, or troubleshooting steps to resolve Azure Firewall and VPN Gateway routing issues in similar architectures.**

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-networking/spoke-hub-hub-traffic-with-vpn-gateway-bgp-and-firewall-issue/m-p/4471878#M750)
