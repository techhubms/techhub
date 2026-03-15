---
external_url: https://blog.fabric.microsoft.com/en-US/blog/introducing-workspace-level-ip-firewall-rules-in-microsoft-fabric-preview/
title: Introducing Workspace-Level IP Firewall Rules in Microsoft Fabric (Preview)
author: Microsoft Fabric Blog
primary_section: azure
feed_name: Microsoft Fabric Blog
date: 2026-01-29 09:00:00 +00:00
tags:
- Access Control
- Admin Portal
- Azure
- Cloud Security
- Compliance
- Entra Conditional Access
- Firewall Rules
- Governance
- IP Firewall
- Microsoft Fabric
- Network Security
- News
- Private Links
- Public Endpoint Protection
- Security
- Workspace Level Security
section_names:
- azure
- security
---
Microsoft Fabric Blog showcases the preview of workspace-level IP firewall rules, letting admins enforce IP-based inbound restrictions for enhanced and granular workspace security.<!--excerpt_end-->

# Introducing Workspace-Level IP Firewall Rules in Microsoft Fabric (Preview)

In today’s interconnected environment, protecting sensitive data in the cloud is essential. Microsoft Fabric, known for its robust security capabilities, introduces workspace-level IP firewall rules—now in Preview.

## Overview of Workspace-Level IP Firewall Rules

Workspace-level IP firewall rules allow Fabric workspace administrators to restrict inbound public network access at individual workspace scope. This is achieved by defining an allowlist of trusted public IP addresses, IP ranges, or CIDR blocks. Only traffic originating from these specified IP ranges is permitted to access the workspace, with all other requests blocked at ingress before reaching authentication or data services.

- **Granular Control**: Unlike tenant-wide restrictions, firewall rules can now be applied per workspace, enabling tailored and differentiated protections for various projects, environments, or business units.
- **Enhanced Security**: This feature complements available security options—such as Private Links and Entra Conditional Access—creating a multi-layered defense framework.

## How to Implement Workspace-Level IP Firewall Rules

1. **Enable at Tenant Level**: Tenant admin enables 'Configure workspace-level inbound network rules' in the Fabric admin portal.
2. **Set Rules as Workspace Admin**:
   - Access the Workspace Settings of the target Fabric workspace.
   - Select 'Allow connections from selected networks and workspace private links.'
   - Edit and add single IPs, IP ranges, or CIDR blocks to the allowlist.
   - Optionally, use 'Add client IP address' to add the current egress IP automatically.

For a detailed setup, limitations, and list of supported artifacts, refer to [Setup and use Workspace-level IP Firewall rules](https://aka.ms/fabric-ip-firewall-blog).

## Key Benefits

- **Enhanced Inbound Security**: Blocks unauthorized public access at the network edge, ensuring only pre-approved IPs reach Fabric workspaces.
- **Granular Workspace-level Control**: Security policies can be differentiated at the workspace level without impacting tenant-wide settings.
- **Reduced Exposure for Public Endpoints**: Ideal for scenarios where Fabric workspaces are accessible via public endpoints by enforcing minimal, trusted IP ranges for critical users, automation, or third-party tools.
- **Compliance and Governance**: Supports regulatory requirements with explicit network boundaries, strengthening the organization’s compliance posture when paired with other Fabric security controls.

## Looking Ahead

As workspace-level IP firewall rules progress through Preview, Microsoft commits to refining the capability based on user feedback and aligning it with broader Fabric security and governance frameworks. This enhancement aims to provide administrators with versatile tools to apply layered inbound protections across different access scenarios.

*Your feedback is encouraged—help shape security enhancements in Fabric by commenting on the original blog post.*

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/introducing-workspace-level-ip-firewall-rules-in-microsoft-fabric-preview/)
