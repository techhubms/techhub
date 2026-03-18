---
section_names:
- azure
date: 2026-03-18 19:41:45 +00:00
feed_name: Microsoft Tech Community
primary_section: azure
external_url: https://techcommunity.microsoft.com/t5/azure-networking/my-first-techcommunity-post-azure-vpn-gateway-bgp-timer/m-p/4503580#M776
tags:
- Active Active VPN Gateway
- Azure
- Azure VPN Gateway
- BGP
- BGP Hold Timer
- BGP Keepalive
- BGP Session Resets
- BGP Timer Negotiation
- Bowtie Configuration
- Community
- CPE
- ExpressRoute
- Hybrid Connectivity
- Microsoft Learn
- Networking
- Route Flapping
- SD WAN
- Site To Site VPN
- VPN Gateway SKUs
title: 'Azure VPN Gateway BGP timer mismatches: why aggressive CPE timers cause session flaps'
author: joclemen
---

joclemen shares a real-world lesson from large SD-WAN customer deployments: Azure VPN Gateway uses fixed BGP timers (60s keepalive / 180s hold) and doesn’t negotiate down, so aggressive customer-premises timers (like 10/30) can trigger repeated BGP session flaps and route instability.<!--excerpt_end-->

# Azure VPN Gateway BGP timer mismatches: why aggressive CPE timers cause session flaps

This is a real-world lesson learned from customer engagements supporting a large global SD-WAN deployment with hundreds of site-to-site VPN connections into **Azure VPN Gateway**. The issue looks simple—**BGP timers**—but mismatched expectations can cause serious instability when connecting on-premises environments to Azure.

If you see seemingly random **BGP session resets**, intermittent route loss, or confusing failover behavior, a **timer mismatch** between Azure and your **customer premises equipment (CPE)** may be the cause.

## Customer expectation: BGP timer negotiation

Many enterprise routers and firewalls support **aggressive BGP timers** and expect these to be *negotiated* during session establishment.

A common customer configuration:

- **Keepalive**: 10 seconds
- **Hold time**: 30 seconds

This can be a valid approach in conventional networking environments where faster failure detection and convergence are desired.

## Azure VPN Gateway reality: fixed BGP timers

**Azure VPN Gateway supports BGP but uses fixed timers (60/180) and won’t negotiate down.**

> The BGP keepalive timer is 60 seconds, and the hold timer is 180 seconds. Azure VPN Gateways use fixed timer values and do not support configurable keepalive or hold timers.

This behavior is consistent across supported VPN Gateway SKUs that offer BGP support.

## What happens during a timer mismatch

If a CPE is configured with a **30-second hold timer**, it expects BGP keepalives within that timeframe. Azure sends keepalives every **60 seconds**.

From the CPE’s point of view:

1. No keepalive is received within 30 seconds
2. The BGP hold timer expires
3. The session is declared dead and torn down

Azure may not declare the peer down on the same timeline, which can lead to repeated session flaps.

## Hidden side effect: BGP state and stability controls

During rapid teardown/re-establishment cycles, some CPE platforms rebuild their BGP tables and may increment internal **routing metadata**. Repeatedly doing this can lead to:

- Unexpected and rapid route updates observed by Azure
- Continual BGP finite state machine resets and re-convergence
- Compromised BGP session stability
- CPE logs generating alerts and support tickets

This often gets described as “Azure randomly drops routes” or “BGP is unstable,” but the instability can originate from mismatched timer expectations.

## Why it’s more noticeable on VPN Gateway than ExpressRoute

The issue is more common with **VPN Gateway** than **ExpressRoute**:

- **ExpressRoute** supports **BFD** (faster failure detection without relying on aggressive BGP timers).
- **VPN Gateway** does **not** support BFD, so some customers compensate by lowering BGP timers on the CPE—creating a mismatch with Azure’s fixed timers.

VPN paths are also more **Internet/WAN-like** (delay/loss/jitter is normal), so conservative timer choices tend to be more stability-focused.

## Updated Azure documentation

Microsoft’s official documentation has been updated to clearly state VPN Gateway’s fixed BGP timer values:

- **Keepalive**: 60 seconds
- **Hold time**: 180 seconds
- **Timer negotiation**: Azure uses fixed timers

Reference:

- [Azure VPN Gateway FAQ | Microsoft Learn](https://learn.microsoft.com/en-us/azure/vpn-gateway/vpn-gateway-vpn-faq#what-are-the-bgp-timer-settings-for-site-to-site-s2s-vpn-connections)

## Practical guidance

If you connect a CPE to **Azure VPN Gateway** using BGP:

- Do **not** configure BGP timers lower than Azure’s defaults
- Align CPE timers to **60 / 180** or higher
- Avoid using aggressive timers as a substitute for **BFD**

For additional resiliency:

- Consider **Active-Active VPN Gateways**
- Use **4 tunnels**, commonly implemented in a **bowtie configuration**, for improved resiliency and traffic stability

## Closing thoughts

Cloud networking can behave *correctly*, but differently than conventional on-prem networking. Understanding these differences—and documenting them clearly—can prevent long troubleshooting cycles.

> Note from the author: They used **Microsoft 365 Copilot** to help with formatting and to validate technical accuracy.


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-networking/my-first-techcommunity-post-azure-vpn-gateway-bgp-timer/m-p/4503580#M776)

