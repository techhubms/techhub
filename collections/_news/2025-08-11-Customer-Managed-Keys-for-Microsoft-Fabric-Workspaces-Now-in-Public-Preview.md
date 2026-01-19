---
external_url: https://blog.fabric.microsoft.com/en-US/blog/customer-managed-keys-for-fabric-workspaces-available-in-all-public-regions-now-preview/
title: Customer-Managed Keys for Microsoft Fabric Workspaces Now in Public Preview
author: Microsoft Fabric Blog
viewing_mode: external
feed_name: Microsoft Fabric Blog
date: 2025-08-11 11:57:18 +00:00
tags:
- Azure Key Vault
- Azure Managed HSM
- Cloud Security
- CMK
- Compliance
- Customer Managed Keys
- Data Encryption
- Data Protection
- Key Management
- Microsoft Fabric
- Microsoft Managed Keys
- Public Preview
- Regulatory Requirements
- Workspace Encryption
section_names:
- azure
- ml
- security
---
Microsoft Fabric Blog announces the public preview of customer-managed keys (CMK) for Fabric workspaces, allowing teams to implement stronger data protection and compliance measures. Authored by the Microsoft Fabric Blog team, this update introduces Azure Managed HSM support and enhanced encryption options.<!--excerpt_end-->

# Customer-Managed Keys for Microsoft Fabric Workspaces Now in Public Preview

The Microsoft Fabric Blog team has announced that customer-managed keys (CMK) for Microsoft Fabric workspaces are now available in public preview across all public regions.

## Key Highlights

- **Global Availability**: CMK is now accessible in all public regions, following an earlier, more limited regional preview.
- **Enhanced Compliance**: Organizations can now use their own encryption keys instead of relying solely on Microsoft-managed keys (MMK), simplifying the process of meeting compliance and regulatory requirements.
- **Azure Managed HSM Support**: The release also adds compatibility with [Azure Managed Hardware Security Module (MHSM)](https://learn.microsoft.com/en-us/azure/key-vault/managed-hsm/overview), ensuring higher security standards and enabling dedicated hardware-level protection for stringent scenarios.
- **Comprehensive Guidance**: A [step-by-step guide](https://learn.microsoft.com/en-us/fabric/security/workspace-customer-managed-keys) is available for getting started with CMK in Fabric workspaces.

## What’s New?

- **Flexible Encryption**: While Fabric encrypts all data at rest by default with MMK, CMK provides the option to use organization-controlled keys, supporting varying compliance needs.
- **Stronger Data Protection**: By leveraging Azure Managed HSM, organizations are able to achieve advanced data security and meet the requirements of strict regulatory environments.

## Getting Started

- For setup instructions, refer to the official [Microsoft documentation](https://learn.microsoft.com/en-us/fabric/security/workspace-customer-managed-keys).
- Learn more about the initial preview [here](https://blog.fabric.microsoft.com/en-us/blog/encrypt-data-at-rest-in-your-fabric-workspaces-using-customer-managed-keys/).

## Feedback and Community

The Microsoft Fabric team encourages users to try out CMK and provide feedback or share suggestions via the [Fabric Ideas community](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas).

## Summary

With the public preview of customer-managed keys, Microsoft Fabric workspaces now offer customers the ability to fully control encryption keys for their data, supporting both compliance and high-security scenarios in the modern data analytics and ML landscape.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/customer-managed-keys-for-fabric-workspaces-available-in-all-public-regions-now-preview/)
