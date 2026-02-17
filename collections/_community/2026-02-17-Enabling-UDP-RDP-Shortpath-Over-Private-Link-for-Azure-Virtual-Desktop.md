---
layout: "post"
title: "Enabling UDP RDP Shortpath Over Private Link for Azure Virtual Desktop"
description: "This guide explains the general availability of UDP support over Private Link in Azure Virtual Desktop (AVD). It details how administrators can enable high-performance, secure UDP-based RDP connections with RDP Shortpath for managed networks, outlines practical steps for configuration in the Azure portal, and discusses use cases for regulated network environments."
author: "Rinku_Dalwani"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-virtual-desktop-blog/rdp-shortpath-udp-over-private-link-is-now-generally-available/ba-p/4494644"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-02-17 17:00:00 +00:00
permalink: "/2026-02-17-Enabling-UDP-RDP-Shortpath-Over-Private-Link-for-Azure-Virtual-Desktop.html"
categories: ["Azure", "Security"]
tags: ["Azure", "Azure Configuration", "Azure Networking", "Azure Portal", "Azure Virtual Desktop", "Community", "Direct Network Path", "Endpoint Security", "Managed Networks", "Network Security", "Private Link", "RDP Shortpath", "Remote Desktop Protocol", "Secure Connectivity", "Security", "Session Hosts", "TCP", "UDP"]
tags_normalized: ["azure", "azure configuration", "azure networking", "azure portal", "azure virtual desktop", "community", "direct network path", "endpoint security", "managed networks", "network security", "private link", "rdp shortpath", "remote desktop protocol", "secure connectivity", "security", "session hosts", "tcp", "udp"]
---

Rinku_Dalwani details the availability and setup process for UDP-based RDP Shortpath over Private Link in Azure Virtual Desktop, empowering administrators to optimize secure and efficient connectivity in managed Azure environments.<!--excerpt_end-->

# Enabling UDP RDP Shortpath Over Private Link for Azure Virtual Desktop

Azure Virtual Desktop (AVD) now supports UDP transport over Private Link for Remote Desktop Protocol (RDP) connections, with general availability announced for RDP Shortpath in managed networks. This addition offers direct, high-performance, and secure RDP sessions over Azure Private Link by leveraging UDP, which complements the pre-existing TCP-based solutions.

## Key Benefits and Use Cases

- **Direct UDP Transport:** Facilitates low-latency, high-performance connections between AVD session hosts and clients via Private Link, enhancing the user experience in private and regulated environments.
- **Network Policy Control:** Enables organizations to tightly control network boundaries by specifying private IPs for RDP traffic, crucial for customers with strict security or compliance requirements.
- **Resiliency and Security:** UDP support is added in a way that maintains strong security, reduces operational complexity, and lowers the risk of misconfiguration. TCP-based connectivity continues to operate as a fallback or default for continuity.

## How to Enable UDP-Based RDP Shortpath Over Private Link

To enable UDP transport for RDP Shortpath in your Azure Virtual Desktop deployment:

1. **Access Azure Portal:** Go to your relevant Azure Virtual Desktop Host pools or Workspaces resource.
2. **Configure Networking:**
    - Navigate to **Networking** → **Public access** in the Azure portal.
    - Choose either to enable public access for end users (while keeping private access for session hosts) or disable public access and use only private access. Either of these actions will reveal an opt-in checkbox.
3. **Enable UDP:**
    - Turn on the **Allow Direct UDP network path over Private Link** option.
4. **Adjust RDP Shortpath Settings:**
    - Go to the **RDP Shortpath** tab.
    - Ensure the following are disabled:
        - RDP Shortpath for public networks (via STUN)
        - RDP Shortpath for public networks (via TURN)
    - The portal will prevent saving and display a configuration error if public options are not disabled.
5. **Save Configuration:**
    - Select **Save** to apply changes.

**Important:**

- The **UDP opt-in checkbox is mandatory**—if not selected, RDP Shortpath stays blocked on Private Link, defaulting to WebSocket-based TCP transport.
- Enabling UDP transport is just one part; you must also complete end-to-end Shortpath setup on session hosts and ensure network settings align. [Full documentation here](https://learn.microsoft.com/en-us/azure/virtual-desktop/rdp-shortpath?tabs=managed-networks).

## Additional Resources

- [Private Link for Azure Virtual Desktop – Overview](https://learn.microsoft.com/en-us/azure/virtual-desktop/private-link-overview): Understand how Private Link secures AVD connections.
- [Step-by-step Private Link Setup](https://learn.microsoft.com/en-us/azure/virtual-desktop/private-link-setup?tabs=azure%2Cportal%2Cportal-2): Detailed configuration guidance.
- [RDP Shortpath for AVD Documentation](https://learn.microsoft.com/en-us/azure/virtual-desktop/rdp-shortpath?tabs=managed-networks): Requirements, configuration, and troubleshooting.
- [Azure Virtual Desktop Tech Community](https://techcommunity.microsoft.com/t5/azure-virtual-desktop/bd-p/AzureVirtualDesktopForum): Stay updated with the latest from the AVD community.

---

*Authored by Rinku_Dalwani. For operational announcements and further technical updates, continue to follow the Azure Virtual Desktop blog and community forums.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-virtual-desktop-blog/rdp-shortpath-udp-over-private-link-is-now-generally-available/ba-p/4494644)
