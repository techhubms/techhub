---
external_url: https://www.reddit.com/r/AZURE/comments/1mkt2tk/azure_vpn_p2s_and_expressroute_coexistence/
title: Architecting Azure VPN P2S and ExpressRoute Coexistence in a Hub & Spoke Topology
author: tampasmix
feed_name: Reddit Azure
date: 2025-08-08 11:50:57 +00:00
tags:
- Azure Firewall
- Azure Route Server
- Azure VPN
- Cost Optimization
- ExpressRoute
- Hub And Spoke
- Network Architecture
- Network Security
- On Premises Connectivity
- Point To Site VPN
- RDP
- Remote Access
- Transitive Routing
- Virtual Network Gateway
- Virtual WAN
- Azure
- Security
- Community
section_names:
- azure
- security
primary_section: azure
---
tampasmix explores approaches for connecting Azure VPN P2S remote users to on-premises networks via ExpressRoute in a hub-and-spoke architecture, discussing routing, gateway setup, and practical options for solving connectivity challenges.<!--excerpt_end-->

# Azure VPN P2S and ExpressRoute Coexistence in Hub & Spoke Architecture

**Author:** tampasmix

## Scenario Overview

The project aims to allow remote workforce users, connecting through Azure Point-to-Site (P2S) VPN, to access on-premises resources routed via Azure ExpressRoute, all within a hub-and-spoke network topology. The setup includes:

- **Hub & Spoke architecture** (1 hub, 3 spokes)
- **ExpressRoute Gateway** deployed in the hub's `gatewaySubnet`
- **Azure Firewall** inspecting traffic between spokes and from/on-premises
- Separate **Virtual Network Gateways** for ExpressRoute and P2S VPN in the hub

## Problem Statement

While P2S VPN clients can access Azure assets on spoke VNets, getting traffic to flow from remote users (VPN P2S) to on-premises resources (over ExpressRoute) is problematic. Existing Microsoft documentation lacks specific guidance for P2S with ExpressRoute, focusing more on S2S (Site-to-Site) scenarios.

## Discussed Solutions and Observations

- **Route Management**: To direct some traffic over VPN instead of ExpressRoute, on-premises must not advertise the relevant address spaces for ExpressRoute, and include them in VPN Phase 2 settings.
  - "All missed routes in the ER should be in the VPN to work."
  - Failover/routing policies are crucial for traffic separation.
- **Azure Route Server** can help manage complex transitive routing, but it incurs significant additional cost.
- **Remote Access Limitation**: The configuration aims specifically for remote users (RDP), and trying to avoid deploying a route server due to cost implications.
- **Current Setup Limitation**:
  - Two gateways in the hub (one for ExpressRoute, one for P2S)
  - P2S users access Azure spokes, but not on-prem via ExpressRoute
  - Previous customer projects required Azure Route Server to solve this

## Alternative Strategies Mentioned

- **Azure Virtual WAN (vWAN)**: Simplifies transitive routing, can enable "branch to branch" (spoke to spoke/on-prem via VPN/ExpressRoute)
- **Azure Route Server**: Most robust solution but expensive; enables branch-to-branch routing

## Community Request

The author requests references or articles from anyone who has solved this scenario or implemented successful alternatives.

---

### Key Considerations for Similar Designs

- Ensure on-premises advertised route tables are configured to align with remote access objectives.
- When feasible, evaluate infrastructure cost/complexity tradeoffs between deploying Azure Route Server, Virtual WAN, or custom route tables.
- Consider that most reference architectures/blended scenarios involving both P2S and ExpressRoute will require advanced routing to bridge connectivity gaps, especially when using Azure Firewall for east-west or north-south inspection.

## References and Further Reading

- [Azure VPN Gateway: About Point-to-Site VPN](https://learn.microsoft.com/en-us/azure/vpn-gateway/point-to-site-about)
- [ExpressRoute and Virtual Network Gateways](https://learn.microsoft.com/en-us/azure/expressroute/expressroute-virtual-network-gateways)
- [Azure Route Server Documentation](https://learn.microsoft.com/en-us/azure/route-server/overview)

## Summary

- Typical hub & spoke topology with P2S and ExpressRoute has routing challenges for enabling P2S clients to reach on-prem assets
- Azure Route Server and Virtual WAN are considered solutions, with cost as a primary concern
- Custom route management and split routing may be necessary but can increase complexity

_If you have implemented a similar design or have a reference, sharing your experience would be helpful to the community._

This post appeared first on "Reddit Azure". [Read the entire article here](https://www.reddit.com/r/AZURE/comments/1mkt2tk/azure_vpn_p2s_and_expressroute_coexistence/)
