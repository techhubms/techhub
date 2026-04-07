---
title: Workspace Customer-Managed Keys for BYOK in Microsoft Fabric (Preview)
primary_section: ml
author: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/workspace-customer-managed-keys-for-byok-in-microsoft-fabric-preview/
section_names:
- azure
- ml
- security
date: 2026-04-03 10:00:00 +00:00
tags:
- Azure
- Azure Key Vault
- Azure Key Vault Managed HSM
- Bring Your Own Key (byok)
- Compliance
- Cryptographic Keys
- Customer Managed Keys (cmk)
- Data Encryption
- Defense in Depth
- Encryption
- Fabric Admin Portal
- Fabric Capacity
- Fabric Workspace
- FIPS Validation
- Key Management
- Managed HSM (mhsm)
- Microsoft Fabric
- ML
- News
- Power BI
- Power BI Semantic Models
- Regulatory Requirements
- Security
- Tenant Isolation
feed_name: Microsoft Fabric Blog
---

Microsoft Fabric Blog explains a Microsoft Fabric (Preview) update that lets you enable workspace-level customer-managed keys (CMK) even when the underlying Fabric capacity is configured for Bring Your Own Key (BYOK), simplifying enterprise encryption and compliance setups while using Azure Key Vault or Managed HSM.<!--excerpt_end-->

## Overview
Enterprise analytics platforms often need more than “encryption is on” — they need clear **ownership and control of cryptographic keys** to satisfy security, compliance, and regulatory requirements.

Microsoft Fabric supports two complementary key-management options:

- **Bring Your Own Key (BYOK)**: customer-managed encryption for **Power BI semantic models** at the **capacity level**
- **Customer-Managed Keys (CMK)**: customer-managed encryption for **other Fabric items** at the **workspace level**

Because workspace-level CMK does **not** support Power BI semantic models, using **both** features is how you apply customer-managed encryption across Fabric while still covering semantic models.

## What’s new
Previously, customers could not enable **workspace CMK** on workspaces that used Fabric capacities already configured with **BYOK**. That created a trade-off:

- To protect Fabric data with custom keys, organizations often had to provision **separate capacities**:
  - one for Power BI semantic models (BYOK)
  - another for other Fabric items (CMK)
- This increased **cost** and **operational complexity**.

Now, **workspace-level CMK is supported in all Fabric capacities, including BYOK-enabled capacities**.

### What this enables
- CMK can be enabled on workspaces hosted in **BYOK-enabled capacities**.
- You can use:
  - the **same key** for both BYOK and CMK, or
  - **separate keys** for isolation or compliance requirements.
- You no longer need dedicated capacities solely to adopt CMK.

## How BYOK and CMK work together
At a high level:

- **BYOK** protects **Power BI semantic models** at the **capacity** layer.
- **CMK** protects **Fabric workspace data** at the **workspace** layer.

Used together, they provide **layered encryption controls** aligned to operational and regulatory requirements.

![Comparing previous and current Fabric encryption models, showing workspace-level CMK now supported within BYOK-enabled capacities.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/04/comparing-previous-and-current-fabric-encryption-m.png)

*Figure: Interoperability between workspace-level CMK and capacity level-BYOK.*

## Why this matters
Common scenarios this change supports:

- **Compliance-driven organizations**
  - Meet regulations that require customer ownership of encryption keys and granular cryptographic boundaries.
- **Shared capacities with isolated workspaces**
  - Protect sensitive workspaces within shared Fabric capacities without adding more infrastructure.
- **Defense-in-depth strategies**
  - Encrypt at both capacity and workspace layers to reduce blast radius and strengthen security posture.

## Configuring BYOK and CMK together (high level)
These are the high-level steps to use BYOK and CMK in the same Fabric environment.

### Store keys in Azure Key Vault
Create encryption keys in:

- **Azure Key Vault**, or
- **Azure Key Vault Managed HSM (MHSM)**

Notes:

- Use **MHSM** when you need hardware-backed, single-tenant, **FIPS-validated** key storage.
- Assign the required permissions for the **Power BI service** and **Fabric** to access the keys.

### Configure BYOK on a Fabric capacity
In the Fabric admin portal:

- Select the Fabric capacity.
- Configure **BYOK** using a key stored in Azure Key Vault or MHSM.
- This key encrypts **Power BI semantic models** hosted on the capacity.

### Enable CMK on a workspace within that capacity
In workspace settings:

- Enable encryption using **customer-managed keys**.
- Select a key from Azure Key Vault or MHSM for workspace data encryption.
- Choose whether to reuse the BYOK key or use a separate key for CMK.

## Resources
- [Bring your own encryption keys for Power BI](https://learn.microsoft.com/fabric/enterprise/powerbi/service-encryption-byok)
- [Customer-managed keys for Fabric workspaces](https://learn.microsoft.com/fabric/security/workspace-customer-managed-keys#configure-encryption-with-customer-managed-keys-for-your-workspace)

## Conclusion
Workspace-level **CMK** is now supported in **BYOK-enabled** Microsoft Fabric capacities (Preview), enabling a more unified encryption approach for enterprise analytics. Organizations can improve control and auditability of encryption keys and meet compliance needs without the extra operational burden of provisioning separate capacities.

[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/workspace-customer-managed-keys-for-byok-in-microsoft-fabric-preview/)

