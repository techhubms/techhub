---
layout: post
title: 'Extending Layer-2 Networks Over Layer-3 IP with VXLAN: MTU, Overlay, and BFD Best Practices'
author: SaravSubramanian
canonical_url: https://techcommunity.microsoft.com/t5/azure-networking-blog/extending-layer-2-vxlan-networks-over-layer-3-ip-network/ba-p/4466406
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-08 17:44:18 +00:00
permalink: /azure/community/Extending-Layer-2-Networks-Over-Layer-3-IP-with-VXLAN-MTU-Overlay-and-BFD-Best-Practices
tags:
- Azure
- Azure Networking
- Bidirectional Forwarding Detection
- Cloud Networking
- Community
- IP Routing
- Jumbo Frames
- L2 Extension
- Layer 2 Overlay
- Layer 3 IP
- MTU
- Network Virtualization
- Resilience
- UDP Encapsulation
- Virtual Extensible LAN
- VLAN
- VTEP
- VXLAN
- WAN
section_names:
- azure
---
SaravSubramanian presents a vendor-neutral, in-depth exploration of how VXLAN enables scalable Layer-2 overlays on Layer-3 IP networks, focusing on key aspects like MTU sizing, VLAN preservation, and BFD for resilience. This guide provides actionable insights for enterprise and Azure network designs.<!--excerpt_end-->

# Extending Layer-2 Networks Over Layer-3 IP with VXLAN

**Author:** SaravSubramanian  

## Introduction

Virtual Extensible LAN (VXLAN) transforms how enterprises and cloud providers extend Layer-2 networks across geographically distributed sites, enabling scalable, flexible overlays on top of standard Layer-3 IP. VXLAN encapsulates Ethernet frames inside UDP/IP, meaning VLAN segments (traditionally limited by the 12-bit VLAN ID) can now be stretched and multiplied up to around 16 million (thanks to a 24-bit VNI).

**Key Advantages:**

- **Massive Scale:** Support for millions of isolated segments, ideal for multitenancy and large data centers.
- **Flexibility:** Decoupling logical L2 networks from rigid underlay requirements; stretch VLANs across sites without changing existing configurations.
- **Mobility:** Workloads or virtual machines can move between sites (even data centers) without IP changes, thanks to the unified overlay.
- **Operational Simplicity:** Logical L2 overlays can span over any routed IP network and utilize capabilities like ECMP.

## VXLAN Overlay Use Cases and Benefits

- **Transparent VLAN Transport:** VLAN tags are preserved throughout the VXLAN tunnel, so endpoints retain their L2 identities and broadcast domains.
- **Scalable Network Segmentation:** Each VLAN or service is mapped to a VXLAN Network Identifier (VNI), offering granular isolation—key for cloud and enterprise scenarios.
- **Isolation and Underlay Simplicity:** Underlay routers/switches only forward IP packets; no need to manage VLAN trunks or extended STP domains in the WAN or core. This enhances both scalability and stability.
- **Enhanced Resilience:** Overlay traffic can leverage multipathing (ECMP), resulting in better bandwidth use and rapid recovery from failures without L2 convergence delays.

### Example Scenario

If Site A and Site B both run VLAN 100 on their local switches, a VXLAN overlay allows hosts in both sites to behave as if they’re in the same VLAN/subnet (e.g., 10.1.100.x/24), with ARP and broadcasts functioning normally—even if the underlay is a routed WAN.

## MTU and Overhead Considerations

- **VXLAN Overhead:** VXLAN adds ~50 bytes (IPv4) or ~70 bytes (IPv6) to each packet from encapsulation.
- **MTU Planning:** Ensure the underlay IP network MTU is large enough (typically 1550B+) to accommodate the encapsulated frames. If not possible, reduce the overlay MTU accordingly (e.g., to 1450B for a standard 1500B underlay).
- **Why It Matters:** Fragmentation leads to packet drops and tricky troubleshooting. Many devices will drop packets if they're too large, rather than fragment them. Always validate your MTU end-to-end, e.g., with pings using the Don't Fragment (DF) bit.

## Monitoring VXLAN Tunnel Health with BFD

VXLAN overlays do not natively signal link failure. Bidirectional Forwarding Detection (BFD) solves this by running lightweight, rapid keepalives between tunnel endpoints (VTEPs):

- **BFD Mechanism:** Sends hello packets at high frequency (as low as 50ms). If missed, the peer is quickly declared down—potentially sub-second, far faster than routing protocol or ARP timeouts.
- **Operational Benefit:** Enables fast failover or rerouting in the event of underlay/tunnel failure, critical for maintaining resilient overlays.
- **Integration:** Most vendors support BFD for VXLAN tunnels or VTEP-to-VTEP monitoring. Detection triggers can automate route withdrawal or logical interface down events, speeding up network convergence.

## Deployment Recommendations

- Always verify and plan the MTU across the overlay and underlay. Use jumbo frames or set the overlay MTU lower if needed.
- Use BFD for all critical VXLAN tunnels, especially when spanning WANs or unreliable links.
- Map each VLAN/service to a unique VNI for maximum segmentation and scalability.
- Leverage standard routing (OSPF, BGP, etc.) in the underlay, as the overlay is IP-native.
- Validate failover scenarios by forcibly breaking and restoring tunnels, observing BFD response.

## Conclusion

VXLAN overlays are foundational for modern data centers and hybrid cloud architectures—including Microsoft Azure Virtual Networking and similar platforms. By planning for encapsulation overhead and employing robust tunnel monitoring (BFD), engineers can safely extend VLANs across routed IP infrastructure, overcoming the limitations of traditional L2 and MPLS solutions and offering scalable, resilient multicampus/enterprise networks.

**Further Reading:**

- IETF RFC 7348 (VXLAN Specification)
- Vendor documentation on VXLAN, MTU planning, and BFD configuration
- Azure and cloud provider networking best practices

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-networking-blog/extending-layer-2-vxlan-networks-over-layer-3-ip-network/ba-p/4466406)
