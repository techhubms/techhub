---
external_url: https://techcommunity.microsoft.com/t5/azure-architecture-blog/cross-region-zero-trust-connecting-power-platform-to-azure-paas/ba-p/4484995
title: 'Cross-Region Zero Trust: Secure Power Platform Connectivity to Azure PaaS Without Public Exposure'
author: Idit_Bnaya
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2026-01-13 19:09:12 +00:00
tags:
- Automation
- Azure Firewall
- Azure PaaS
- Customer Managed Key
- Dataverse
- DNS Proxy
- Enterprise Policy
- High Availability
- Key Vault
- Log Analytics
- Power Apps
- Power Automate
- Power Platform
- Private Endpoint
- RBAC
- Regional Pair
- SQL Database
- Storage Account
- User Assigned Managed Identity
- Virtual Network
- VNet Injection
- Zero Trust
section_names:
- azure
- security
---
Idit_Bnaya presents a hands-on walkthrough for connecting Power Platform to Azure PaaS services across regions with strict Zero Trust principles, focusing on private network links, managed identity, custom orchestration, and verifiable security.<!--excerpt_end-->

# Cross-Region Zero Trust: Connecting Power Platform to Azure PaaS Across Regions

## Introduction

Modern cloud environments require data to move securely between services in different regions. Power Platform environments (including Dynamics 365, Power Apps, or Power Automate) may be hosted in one region, while sensitive Azure PaaS resources such as SQL Databases and Storage Accounts reside in another—often with strict public access controls.

This guide demonstrates a practical Zero Trust architecture that enables private communication between these services, eliminating exposure to the public internet. It focuses on VNet integration, private endpoints, firewall-managed DNS, managed identities, and automated policy deployments to ensure high availability and robust security.

## Key Challenges Addressed

- **Connecting Power Platform in Region A to isolated Azure PaaS in Region B** without public internet access
- **Maintaining high availability** through regional pair topology
- **Automating complex network and security policy deployment**
- **Proving end-to-end private connectivity via audit logs**

## Solution Overview

1. **VNet Injection:** The Power Platform environment uses outbound VNet integration to inject traffic into a dedicated subnet within a Spoke VNet (Region A).
2. **Centralized Hub:** An Azure Firewall acts as both security checkpoint and DNS proxy in a centralized Hub VNet.
3. **Global Backbone:** Global VNet Peering connects Spoke VNets across regions, ensuring all data stays on the Microsoft backbone.
4. **Private Endpoints:** Destination Azure PaaS resources (Region B) are restricted to private endpoint access only.

## High Availability with Regional Pairs

- Always deploy mirrored VNets and delegated subnets in both regions of a regional pair.
- Each region requires its own Enterprise Policy and delegated subnet for failover resilience.
- Use the Microsoft-provided [SetupSubscriptionForPowerPlatform.ps1](https://github.com/microsoft/PowerPlatform-EnterprisePolicies/blob/main/README.md#how-to-run-setup-scripts) to register your Azure subscription for required resource providers.

## Step-by-Step Implementation

### 1. **Networking Foundation**

- Create Spoke VNets and delegated subnets in each region.
- Deploy a central Hub VNet with Azure Firewall (with DNS Proxy enabled).
- Set up global VNet Peering between Spokes and Hub.
- Link Private DNS Zones (e.g., `privatelink.blob.core.windows.net`) to the Hub VNet.

### 2. **Identity: Secretless Security**

- Create a User-Assigned Managed Identity (UAMI) in Azure.
- Assign RBAC roles such as Storage Blob Data Contributor to the UAMI on the destination PaaS resource (Storage Account, SQL, etc).
- Register the UAMI as an Application User in the Power Platform admin center for your environment.

### 3. **Enterprise Policies and Automation**

- Enterprise Policies (Network Injection, Encryption) must be deployed via PowerShell/CLI — not the portal UI.
- Use the [official Microsoft Enterprise Policy repository](https://github.com/microsoft/PowerPlatform-EnterprisePolicies) for templates, or the [Simplified Scripts Repository](https://github.com/Iditbnaya/Power-Platform-Enterprise-Policies-Simplified-scripts) for orchestrating multi-region/high-availability setups.
- Scripts handle dual-VNet injection, automate regional deployments, and streamline linking steps for rapid, compliant rollout.

### 4. **Encryption: Customer-Managed Keys (CMK)**

- Configure Azure Key Vault for CMK, ensuring Purge Protection and Soft Delete are enabled.
- Assign the Managed Identity to the Key Vault Crypto Service Encryption User role for wrap/unwrap permissions.
- Link CMK policies back to Power Platform environments using the admin interface.

### 5. **Final Linking and Verification**

- In Power Platform Admin Center:
  - Link environments to both Network and Encryption Enterprise Policies.
  - Enable Customer-managed keys; ensure the Managed Identity has “Get”, “List”, “Wrap”, and “Unwrap” permissions on your encryption key.
- **Verification:** Use Azure Log Analytics and KQL queries to confirm all access is routed privately (presence of NetworkPerimeter ID in logs)

  ```kusto
  AzureDiagnostics
  | where ResourceProvider == "MICROSOFT.KEYVAULT"
  | where OperationName == "KeyGet" or OperationName == "KeyUnwrap"
  | where ResultType == "Success"
  | project TimeGenerated, OperationName, VaultName = Resource, ResultType, CallerIP = CallerIPAddress, EnterprisePolicy = identity_claim_xms_mirid_s, NetworkPerimeter = identity_claim_xms_az_nwperimid_s
  | sort by TimeGenerated desc
  ```

## Summary Workflow

**PHASE 1: Infrastructure**

- Deploy VNets/subnets in both regional pairs
- Setup Hub VNet + Firewall + DNS
- Peering

**PHASE 2: Identity**

- UAMI creation
- RBAC assignment
- Key Vault prep

**PHASE 3: Policy Deployment**

- Network Injection & Encryption policies via PowerShell/CLI

**PHASE 4: Final Power Platform Linking**

- Enable policies in Admin Center
- Register Managed Identity

**PHASE 5: Verification**

- Run real workload
- Check Log Analytics for private bridge confirmation

## References and Further Resources

- [Power Platform VNet Integration Documentation](https://learn.microsoft.com/en-us/power-platform/admin/vnet-support-overview#supported-services)
- [Power Platform Enterprise Policies (Official)](https://github.com/microsoft/PowerPlatform-EnterprisePolicies)
- [Simplified Scripts Repository](https://github.com/Iditbnaya/Power-Platform-Enterprise-Policies-Simplified-scripts)
- [Manage your customer-managed encryption key - Power Platform | Microsoft Learn](https://learn.microsoft.com/en-us/power-platform/admin/customer-managed-key)

By moving Power Platform workloads under enterprise-grade network controls and encryption policies, organizations can confidently leverage low-code capabilities without compromising on security or regulatory requirements.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-architecture-blog/cross-region-zero-trust-connecting-power-platform-to-azure-paas/ba-p/4484995)
