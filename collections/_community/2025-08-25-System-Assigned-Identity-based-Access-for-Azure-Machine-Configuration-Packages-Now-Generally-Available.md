---
external_url: https://techcommunity.microsoft.com/t5/azure-governance-and-management/system-assigned-identity-based-access-for-machine-configuration/ba-p/4446603
title: System-Assigned Identity-based Access for Azure Machine Configuration Packages Now Generally Available
author: mutemwamasheke
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-08-25 14:30:00 +00:00
tags:
- Automation
- Azure Arc
- Azure Machine Configuration
- Azure Policy
- Azure Storage
- Blob Storage
- Compliance
- Guest Configuration
- Policy Enforcement
- PowerShell
- Role Based Access Control
- Server Management
- System Assigned Identity
- User Assigned Identity
- Virtual Machines
section_names:
- azure
- devops
- security
---
mutemwamasheke details the new support for System Assigned Identities to enable secure, private access to configuration packages in Azure Machine Configuration, making it easier to manage and secure VMs and Arc-enabled servers.<!--excerpt_end-->

# System-Assigned Identity-based Access for Azure Machine Configuration Packages – GA on both Azure and Arc!

Azure Machine Configuration now supports System Assigned Identities for secure, private access to configuration packages stored in Azure Storage. This simplifies access management and enhances security for both Azure and Arc-enabled servers, superseding the previous reliance on Shared Access Signature (SAS) Tokens.

## Background

Azure Machine Configuration (formerly Azure Policy Guest Configuration) allows at-scale auditing and configuration of OS, application, and workload settings across Azure and hybrid environments. This update improves the security model by letting VMs or Arc-enabled machines use their own system-assigned identities to fetch configuration packages from Azure Storage securely.

## What’s New?

- **System Assigned Identity Support:**
  - Custom policy definitions can now use the system-assigned identity of a VM or Arc-enabled server.
  - Machines with proper identity and Azure Storage Blob Data Reader permissions can access configuration packages privately.
  - No more requirement for SAS tokens—enabling tighter storage account access controls.
  - Feature is available across both Azure and Arc machines.
- **User Assigned Identity:** Remains supported as an option for managed access.

## Getting Started

To enable this feature:

1. **Deploy the Machine Configuration Extension:**
   - Use the policy initiative: [Deploy prerequisites to enable machine configuration policies on virtual machines](https://learn.microsoft.com/en-us/azure/virtual-machines/extensions/guest-configuration#:~:text=Azure%20Policy.%20To,on%20virtual%20machines)
2. **Enable System Assigned Identity:**
   - All VMs/Arc-enabled servers in your policy scope need [system-assigned identity enabled](https://learn.microsoft.com/en-us/entra/identity/managed-identities-azure-resources/how-to-configure-managed-identities?pivots=qs-configure-portal-windows-vm#enable-system-assigned-managed-identity-during-creation-of-a-vm).
   - Grant [Storage Blob Data Reader](https://learn.microsoft.com/en-us/azure/role-based-access-control/built-in-roles#storage) (or equivalent) permissions to the machine.
3. **Use the Latest Guest Configuration PowerShell Module:**
   - Install at least version 4.10.0: [Download module](https://www.powershellgallery.com/packages/GuestConfiguration/4.10.0)
   - Author policies with managed identities.
4. **Develop and Upload Your Custom Package:**
   - [Follow the documentation](https://learn.microsoft.com/en-us/azure/governance/machine-configuration/how-to/develop-custom-package/overview) to create and upload a policy package.
5. **Author Policy Definitions:**
   - Use `New-GuestConfigurationPolicy @PolicyConfig -UseSystemAssignedIdentity` for system identity support.
   - Example PowerShell configuration:

     ```powershell
     $PolicyConfig = @{
       PolicyId = '_My GUID_'
       ContentUri = 'https://yourstorageaccount.blob.core.windows.net/yourcontainer/package.zip'
       DisplayName = 'My deployment policy'
       Description = 'My deployment policy'
       Path = './policies/deployIfNotExists.json'
       Platform = 'Windows'
       PolicyVersion = 1.0.0
       Mode = 'ApplyAndAutoCorrect'
       LocalContentPath = "C:\Local\Path\To\Package"
     }
     New-GuestConfigurationPolicy @PolicyConfig -UseSystemAssignedIdentity
     ```

   - Example policy definition metadata:

     ```json
     "metadata": {
        "category": "Guest Configuration",
        "version": "1.0.0",
        "requiredProviders": [ "Microsoft.GuestConfiguration" ],
        "guestConfiguration": {
          "name": "TimeZone",
          "version": "1.0.0",
          "contentType": "Custom",
          "contentUri": "https://yourstorageaccount.blob.core.windows.net/yourcontainer/package.zip",
          "contentHash": "HASHVALUE",
          "contentManagedIdentity": "system"
        }
      }
     ```

   - Upload using `New-AzPolicyDefinition` in PowerShell.

## Feature Limitations

- **Guest Configuration Agent:** Requires version 1.29.98.0+ (Windows), 1.26.93.0+ (Linux)
- **API Version:** Policy definition must use Azure Policy API version 2024-04-05 or later
- **Billing:** Azure Arc servers incur a $6/server/month charge (waived under certain conditions—see documentation)

## Useful Links

- [Azure Machine Configuration Documentation](https://docs.microsoft.com/en-us/azure/governance/machine-configuration/)
- [Managed Identities Overview](https://learn.microsoft.com/en-us/entra/identity/managed-identities-azure-resources/overview)
- [User Assigned Identity Access for Packages](https://techcommunity.microsoft.com/blog/azuregovernanceandmanagementblog/user-assigned-identity-based-access-for-machine-configuration-packages-%E2%80%93-general/4305594)

*Updated Aug 21, 2025 | Author: mutemwamasheke*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-governance-and-management/system-assigned-identity-based-access-for-machine-configuration/ba-p/4446603)
