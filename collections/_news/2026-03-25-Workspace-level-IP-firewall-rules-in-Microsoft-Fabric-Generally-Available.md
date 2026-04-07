---
primary_section: ml
feed_name: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/workspace-level-ip-firewall-rules-in-microsoft-fabric-generally-available/
author: Microsoft Fabric Blog
tags:
- Inbound Access Control
- IP Allowlist
- Lakehouse
- Microsoft Entra Conditional Access
- Microsoft Fabric
- ML
- Network Security
- News
- OneLake
- Outbound Access Protection
- Role Based Access Control
- Security
- Spark
- Tenant Private Link
- Tenant Settings
- Warehouse
- Workspace IP Firewall Rules
- Workspace Private Link
- Workspace Settings
title: Workspace level IP firewall rules in Microsoft Fabric (Generally Available)
section_names:
- ml
- security
date: 2026-03-25 10:00:00 +00:00
---

Microsoft Fabric Blog announces general availability of workspace-level IP firewall rules in Microsoft Fabric, explaining how workspace admins can allowlist trusted public IPs to reduce public internet exposure and how tenant admins control feature availability alongside Private Link and Microsoft Entra Conditional Access.<!--excerpt_end-->

# Workspace level IP firewall rules in Microsoft Fabric (Generally Available)

*Source: Microsoft Fabric Blog*

Microsoft Fabric now supports **workspace-level IP firewall rules** (generally available) to provide a lightweight, network-based control for restricting **inbound access** to Fabric workspaces using **trusted public IP addresses**.

The post also points readers to Arun Ulag’s overview of broader FabCon and SQLCon 2026 announcements: [FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform](https://aka.ms/FabCon-SQLCon-2026-news).

## Why this feature exists

In an increasingly connected data environment, limiting access to sensitive data is critical. Fabric already provides multiple security controls for inbound and outbound connectivity, including:

- [Workspace and Tenant Private Links](https://learn.microsoft.com/fabric/security/security-private-links-overview)
- [Microsoft Entra Conditional Access](https://learn.microsoft.com/fabric/security/security-conditional-access)
- [Outbound Access Protection](https://learn.microsoft.com/fabric/security/workspace-outbound-access-protection-overview)

**Workspace-level IP firewall rules** add another option when **private connectivity isn’t feasible**, allowing customers to reduce exposure to the public internet while keeping workspace-level flexibility.

## What workspace-level IP firewall rules do

With this capability, **workspace administrators** can define an **IP allowlist** for a specific workspace so that:

- Only requests originating from **approved public IP addresses** can access workspace data
- Requests from non-approved IPs are blocked

![Diagram illustrating network access control for Fabric Tenant Workspace A, showing two user connections from different public networks. One connection from IP 198.51.100.25 is blocked (red X), while the other from IP 203.0.113.10 is allowed (green check), with allowed IP highlighted in green and workspace components like Lakehouse, Warehouse, Notebook, OneLake, and Spark Job Definition displayed inside a gray box.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/diagram-illustrating-network-access-control-for-fa.png)

*Figure: Workspace IP Firewall feature restricting Fabric workspace access to approved public IP address.*

## How it fits with other Fabric security controls

The post describes the feature as complementary to existing **network and identity security** capabilities, including:

- Workspace-level Private Link
- Tenant-level Private Link
- Microsoft Entra Conditional Access
- Role-based access controls

This lets customers choose protections based on workload and access requirements.

## Tenant and workspace administration model

Key operational points:

- The feature is **opt-in** and has **no impact unless explicitly configured**.
- **Fabric tenant administrators** control whether the feature is available via a **tenant-level setting**.
- After tenant admins enable availability, **workspace admins** manage **IP address allowlists** in **workspace settings**.

## Documentation

For setup steps, limitations, and supported artifacts, see:

- [Setup and use Workspace-level IP Firewall rules](https://learn.microsoft.com/fabric/security/security-workspace-level-firewall-overview)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/workspace-level-ip-firewall-rules-in-microsoft-fabric-generally-available/)

