---
layout: post
title: Customer-Managed Keys for Microsoft Fabric Workspaces Now Generally Available
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/customer-managed-keys-for-fabric-workspaces-generally-available/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-10-15 10:00:00 +00:00
permalink: /azure/news/Customer-Managed-Keys-for-Microsoft-Fabric-Workspaces-Now-Generally-Available
tags:
- Azure
- Azure Key Vault
- CMK
- Compliance
- Customer Managed Keys
- Data Protection
- Data Security
- Encryption
- Fabric Warehouse
- Governance
- Managed HSM
- Microsoft Fabric
- News
- Security
- SQL Analytics Endpoint
- TLS 1.2
- Workspace Administration
section_names:
- azure
- security
---
Microsoft Fabric Blog announces the general availability of customer-managed keys for Fabric workspaces, giving administrators enhanced encryption control. This update allows organizations to secure their data at rest using Azure Key Vault keys, addressing compliance and security needs.<!--excerpt_end-->

# Customer-Managed Keys for Microsoft Fabric Workspaces Now Generally Available

Protecting sensitive data is crucial, especially for organizations with strict compliance and advanced security needs. Microsoft Fabric now supports customer-managed keys (CMK) for workspaces, allowing you to protect data at rest with encryption keys that you create, own, and maintain in your Azure Key Vault (AKV).

## Enhanced Data Protection and Control

- **Default Fabric encryption**: All data at rest is encrypted by default using Microsoft-managed keys. Data in transit is protected by TLS 1.2 or higher.
- **Customer-managed keys (CMK)**: Administrators can now use their own keys stored in Azure Key Vault or Managed HSM to control encryption, enabling the ability to:
  - Oversee the lifecycle, access, and utilization of encryption keys
  - Rotate and revoke key access to meet compliance or governance requirements
  - Add another layer of security beyond Microsoft's standard encryption

This capability is particularly relevant for industries handling sensitive data or requiring strong data governance.

## What’s New in GA

- CMK support has expanded from preview to general availability.
- Encryption support now includes Fabric Warehouses, Notebooks, and the SQL Analytics Endpoint in CMK-enabled workspaces.
- Ongoing work aims to introduce API support, Key Vaults behind firewalls, and additional Fabric item coverage.

For more information, see the [customer-managed keys documentation](https://learn.microsoft.com/en-us/fabric/security/workspace-customer-managed-keys) and the [Warehouse’s CMK launch blog](https://blog.fabric.microsoft.com/blog/bringing-customer-managed-keys-to-fabric-warehouse-and-sql-analytics-endpoint/).

## How to Get Started

Workspace administrators can set up CMK in the Fabric portal:

1. Navigate to Workspace Settings.
2. Enable encryption using your customer-managed key stored in Azure Key Vault.
3. Follow the step-by-step process described in the [encryption documentation](https://learn.microsoft.com/fabric/security/workspace-customer-managed-keys).

## Feedback and Community

Microsoft welcomes feedback on security and flexibility features. You can share suggestions on the [Fabric Ideas – Microsoft Fabric Community](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas/label-name/fabric%20platform%20%7C%20security).

---

**References:**

- [Customer-managed keys documentation](https://learn.microsoft.com/en-us/fabric/security/workspace-customer-managed-keys)
- [Warehouse’s CMK launch blog](https://blog.fabric.microsoft.com/blog/bringing-customer-managed-keys-to-fabric-warehouse-and-sql-analytics-endpoint/)
- [Fabric Ideas – Microsoft Fabric Community](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas/label-name/fabric%20platform%20%7C%20security)

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/customer-managed-keys-for-fabric-workspaces-generally-available/)
