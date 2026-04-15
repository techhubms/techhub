---
author: Marc de Droog
primary_section: azure
feed_name: Microsoft Tech Community
external_url: https://techcommunity.microsoft.com/t5/azure-networking-blog/a-demonstration-of-virtual-network-tap/ba-p/4479136
section_names:
- azure
- security
title: A demonstration of Virtual Network TAP
tags:
- Azure
- Azure Marketplace
- Azure Virtual Network
- Azure Virtual Network TAP
- Community
- Network Detection And Response
- Network Virtual Appliance
- NVA
- Packet Capture
- Security
- Traffic Mirroring
- UDP 4789
- VNet Flow Logs
- VNet Peering
- VTAP
- VXLAN
- Windows Virtual Machines
- Wireshark
date: 2026-04-15 10:30:01 +00:00
---

Marc de Droog demonstrates Azure Virtual Network TAP (VTAP) by mirroring full VM NIC traffic (including payload) to a destination VM and inspecting the VXLAN-encapsulated packets in Wireshark.<!--excerpt_end-->

# A demonstration of Virtual Network TAP

Azure [Virtual Network Terminal Access Point (VTAP)](https://learn.microsoft.com/en-us/azure/virtual-network/virtual-network-tap-overview) (public preview in select regions as of April 2026) copies network traffic from source Virtual Machines to a collector or traffic analytics tool running as a Network Virtual Appliance (NVA).

Unlike VNet Flow Logs (which collect traffic metadata), VTAP creates a full copy of all traffic sent and received by the source VM Network Interface Card(s) (NICs), including packet payload content.

## What VTAP does (and how)

- **Agentless and cloud-native**: works at the Azure network infrastructure level.
- **Out-of-band**: no impact on the source VM’s network performance; the source VM is unaware of the tap.
- **Delivery**:
  - Tapped traffic is **VXLAN-encapsulated**.
  - Delivered to a collector NVA in:
    - the **same VNet** as the source VMs, or
    - a **peered VNet**.

Traffic collectors and analytics tools are typically **3rd-party partner products** available in the Azure Marketplace; these include major Network Detection and Response (NDR) solutions.

## Demo setup

This post demonstrates basic VTAP functionality by copying traffic in and out of a source VM to a destination VM.

### Lab topology

- **VNet 1**: three Windows VMs (`vm1`, `vm2`, `vm3`), each running a basic web server that responds with the VM’s name.
- **VNet 2**: one Windows VM used as the **capture/inspection target**, with **Wireshark** installed.
- This demo **does not** use 3rd-party Marketplace VTAP partner solutions.

Lab code: [Virtual Network TAP](https://github.com/mddazure/virtual-network-tap-lab)

## VTAP configuration details

- The **destination** is configured as the target VM’s **NIC**.
- All captured traffic from VTAP sources is:
  - **VXLAN-encapsulated** and
  - sent to the destination using **UDP port 4789** (**not configurable**).

To simplify analysis, the demo uses a **single VTAP source** so it’s easier to inspect flows in Wireshark. In real environments, multiple (or all) VMs could be configured as VTAP sources.

## Generating and observing traffic

### Source traffic generation

The source VM (`vm1`) generates traffic using a script that continuously polls:

- `http://10.0.2.5` (`vm2`)
- `http://10.0.2.6` (`vm3`)
- [https://ipconfig.io](http://ipconfig.io)

### Wireshark capture

On the destination VM:

- Apply a filter on **UDP port 4789** to capture only **VXLAN-encapsulated** traffic forwarded by VTAP.
- Wireshark automatically decodes VXLAN and shows the **decapsulated** traffic to/from `vm1` (the only VTAP source), including:
  - TCP handshakes
  - TCP/HTTP exchanges
  - traffic to `vm2`, `vm3`, and `https://ipconfig.io/`

### What the encapsulation looks like

Inspecting packet details shows VXLAN encapsulation metadata:

- Outer IP packets (carrying UDP/VXLAN) originate from the **source VM IP** `10.0.2.4`.
- Outer destination is the **target VM IP** `10.1.1.4`.
- VXLAN frames contain the original Ethernet frames (and the inner IP packets), allowing you to see the full exchange between `vm1` and its peers.

## Notes on partner solutions

This demo uses Wireshark to visualize VTAP operation. The Marketplace [partner solutions](https://learn.microsoft.com/en-us/azure/virtual-network/virtual-network-tap-overview#virtual-network-tap-partner-solutions) operate on the captured traffic to implement their own functionality.


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-networking-blog/a-demonstration-of-virtual-network-tap/ba-p/4479136)

