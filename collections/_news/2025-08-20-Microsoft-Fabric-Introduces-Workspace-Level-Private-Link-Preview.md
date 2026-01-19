---
external_url: https://blog.fabric.microsoft.com/en-US/blog/fabric-workspace-level-private-link-preview/
title: Microsoft Fabric Introduces Workspace-Level Private Link (Preview)
author: Microsoft Fabric Blog
viewing_mode: external
feed_name: Microsoft Fabric Blog
date: 2025-08-20 16:11:57 +00:00
tags:
- Azure Portal
- Azure Private Link
- Cloud Security
- Data Privacy
- Enterprise Security
- Fabric Administration
- Fabric Workspace
- Granular Access Control
- Microsoft Fabric
- Network Security
- Private Endpoint
- Regulatory Compliance
- Virtual Networks
section_names:
- azure
- security
---
Microsoft Fabric Blog announces the preview of workspace-level Private Link, a new security feature enabling more granular private network access to individual Fabric workspaces. This post by the Microsoft Fabric Blog team guides IT professionals through setup and highlights key security benefits.<!--excerpt_end-->

# Fabric workspace-level Private Link (Preview)

Microsoft Fabric has announced the preview release of **workspace-level Private Link** support, delivering increased network security and granular access control to organizations using Microsoft Fabric.

## Overview

Following last year’s general availability of Private Link for Fabric Tenants, Microsoft responded to customer feedback by enabling Private Link at the workspace level. This allows organizations to protect individual Fabric workspaces with private endpoints, ensuring traffic stays within the virtual network and does not traverse the public internet.

**Key scenarios:**

- Financial institutions needing strict data privacy
- Healthcare providers securing sensitive patient data
- Any enterprise segmenting network access across business units, environments, or domains

## What is Workspace-Level Private Link?

**Azure Private Link** enables private connectivity from Azure services to virtual networks. Now, Microsoft Fabric supports configuring these connections at the workspace scope.

![Workspace-level Private Link](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/07/Picture2.png)

## Key Benefits

- **Enhanced Enterprise Security:** All traffic to the Fabric workspace remains within the organization's Azure virtual network, minimizing exposure to the public internet.
- **Granular Access Controls:** Organizations can control private network access **per workspace** instead of globally, making it ideal for enforcing differentiated security policies for dev/test/prod or separate business units.
- **Regulatory Compliance:** Supports compliance efforts in regulated industries via network isolation.

## Limitations

- Fabric portal access to workspaces via workspace-level Private Link is not supported in this preview.
- See [current limitations](https://learn.microsoft.com/fabric/security/security-workspace-level-private-links-support).

## Implementation Guide

To set up workspace-level Private Link for a Fabric workspace:

1. **Enable Inbound Network Rules:**
   - A Fabric tenant admin should enable “[Configure workspace-level inbound network rules](https://learn.microsoft.com/en-us/fabric/security/security-workspace-enable-inbound-access-protection)” in the Fabric admin portal.

2. **Configure Private Link and Endpoints:**
   - As a workspace admin, use the Azure portal to:
     - Create an Azure Private Link service referencing the target Fabric workspace.
     - Set up private endpoints for this service within your virtual network.
     - Test connectivity via a VM in the same network to confirm private access.
     - Restrict inbound public access for the workspace.

3. **Further Details:**
   - Step-by-step configuration is available [here](https://learn.microsoft.com/fabric/security/security-workspace-level-private-links-set-up).

## Looking Ahead

Microsoft plans to enhance and refine workspace-level Private Link based on user feedback and evolving cloud security needs. Organizations are encouraged to trial the feature and provide feedback to improve Microsoft Fabric’s security capabilities.

**Learn More:**

- [Feature overview](https://learn.microsoft.com/fabric/security/security-workspace-level-private-links-overview)
- [Support and limitations](https://learn.microsoft.com/fabric/security/security-workspace-level-private-links-support)

---

*This announcement was published by the Microsoft Fabric Blog team.*

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/fabric-workspace-level-private-link-preview/)
