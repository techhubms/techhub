---
external_url: https://www.reddit.com/r/AZURE/comments/1mgm8gy/nva_and_vnet_routing/
title: Implementing NVA for Internet Egress and Inter-Subnet Routing in Azure Hub-Spoke Topology
author: HDClown
viewing_mode: external
feed_name: Reddit Azure
date: 2025-08-03 15:34:00 +00:00
tags:
- Azure Load Balancer
- Cato Networks
- Hub Spoke Topology
- Inter Subnet Routing
- Internet Egress
- LAN/WAN
- Network Virtual Appliance
- Public IP
- Route Tables
- SASE
- UDR
- Vnet Peering
- VPN Gateway
section_names:
- azure
---
HDClown discusses the integration of a Network Virtual Appliance into an Azure hub-spoke architecture and seeks advice on achieving internet egress and inter-subnet routing through the NVA.<!--excerpt_end-->

## Scenario Overview

**Current Network Topology:**

- 2 vNets: one hub, one spoke.
- Hub vNet: Contains subnet for VPN gateway, now also two subnets for NVA (WAN/LAN), Public IP on WAN NIC.
- Spoke vNet: Three subnets, VMs in each.
- vNet peering configured (detailed peering settings).
- VPN gateway provides on-prem S2S, Azure Load Balancer previously served internet egress for spoke VMs.
- Initially, no User Defined Routes (UDRs).

**Goal:**

1. All Azure VMs in the spoke vNet should egress to internet via the NVA.
2. All inter-subnet routing in the spoke vNet should flow via the NVA.
3. NVA replaces the VPN gateway for on-prem connections (S2S).

**NVA Details:**

- Cato Networks SASE solution deployed in the hub vNet, two subnets (WAN and LAN), LAN subnet used for routing.
- UDRs with 0.0.0.0/0 → NVA LAN IP for LAN subnet and spoke subnets.

## Steps Already Taken

- Created and associated a route table (UDR 0.0.0.0/0 → NVA LAN IP) to all spoke subnet(s).
- Removed the spoke VMs from Azure Load Balancer’s backend pool.
- Verified that internet egress for spoke VMs now flows through NVA.
- Inter-subnet traffic within the spoke is still NOT routed via the NVA — Azure platform routing prevails for intra-vNet traffic due to default system routes.

## Technical Analysis

### Why Inter-Subnet Routing is Bypassing Your NVA

- **Azure’s System Routes:** For traffic within the same vNet (including between subnets), Azure uses default system routes, bypassing UDRs unless an explicit, more specific route is configured.
- Your default UDR (0.0.0.0/0) is less specific than the system routes (which cover each subnet’s address range).

### Solutions

**1. Add Explicit UDRs for Each Spoke Subnet:**  

- Create a UDR in each spoke subnet's route table targeting every other subnet's address range, setting the next hop to the NVA's LAN IP.
- Example: If spoke subnets are 10.1.1.0/24, 10.1.2.0/24, and 10.1.3.0/24, in 10.1.1.0/24’s route table, add:
  - Destination: 10.1.2.0/24 → Next hop: NVA LAN IP
  - Destination: 10.1.3.0/24 → Next hop: NVA LAN IP
  - Repeat similarly for the other subnets.
- This will force east-west traffic through the NVA.

**2. Considerations for VNet Peering:**

- Typically, keep peering. If you route all spoke traffic via NVA, ensure:
  - Peering allows forwarded traffic.
  - UDRs do not create routing conflicts or black holes (test S2S/on-prem/vNet-to-vNet flows).
- You may not need to change peering checkboxes if you maintain required traffic flows (especially for any planned future cross-vNet or service integration).

**3. Flexibility for Future Changes:**

- By using UDRs, you retain the ability to selectively bypass the NVA in future — simply add/removing specific routes as necessary for new resources or flows.
- You can even use route tables with subnet-level scope for granular control.

### Other Notes

- Be cautious with Azure platform managed services (such as PaaS) as some may not work as expected with forced tunneling.
- For on-prem S2S, update on-prem routes to point to new NVA public IP, and confirm IPsec/BGP setup with Cato NVA.
- Make sure NSGs (Network Security Groups) are updated to permit flows between subnets/NVA/internet as intended.

## Summary of Recommendations

1. Add subnet-specific UDRs in each spoke subnet route table with the NVA’s LAN IP as the next hop for all other subnet ranges.
2. Confirm and test the resulting flows — both east-west (spoke inter-subnet) and north-south (internet/on-prem traffic).
3. Maintain your hub-spoke peering (with current settings) unless you find specific reasons to change after testing.
4. Document your routing for future flexibility, especially if new resources or services will be provisioned.

---

**By HDClown**

Azure networking can be nuanced. Explicit UDRs will give you the controlled traffic flows you seek via your NVA without sacrificing topology flexibility.

This post appeared first on "Reddit Azure". [Read the entire article here](https://www.reddit.com/r/AZURE/comments/1mgm8gy/nva_and_vnet_routing/)
