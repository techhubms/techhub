---
layout: post
title: 'Azure CLI and Azure PowerShell: Quality, Security, and AI Updates Announced at Ignite 2025'
author: Alex-wdy
canonical_url: https://techcommunity.microsoft.com/t5/azure-tools-blog/azure-cli-and-azure-powershell-ignite-2025-announcement/ba-p/4471182
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-19 19:13:15 +00:00
permalink: /ai/community/Azure-CLI-and-Azure-PowerShell-Quality-Security-and-AI-Updates-Announced-at-Ignite-2025
tags:
- ArizeAI Extension
- ARM API
- Automation
- Azure CLI
- Azure PowerShell
- Azure Services
- Bicep Templates
- Claims Challenge
- Cloud Endpoint Discovery
- Fabric Module
- IaC
- Identity Management
- Invoke AzRestMethod
- MFA Enforcement
- Microsoft Ignite
- Paginate Parameter
- Python 3.13
- Release Notes
- Security Vulnerabilities
- What If Analysis
section_names:
- ai
- azure
- coding
- devops
- security
---
Alex-wdy shares comprehensive updates from Microsoft Ignite 2025, focusing on Azure CLI, Azure PowerShell, and new AI-driven features, including security and quality improvements.<!--excerpt_end-->

# Azure CLI and Azure PowerShell: Ignite 2025 Update

Microsoft announced major improvements to Azure CLI and Azure PowerShell for 2025, emphasizing quality, security, and AI-driven features. This update brings new capabilities, feature enhancements, and security standards compliance for cloud automation practitioners.

## Key Investment Areas

- **Quality and Security:** Focused upgrades, including enforcement of Multi-Factor Authentication (MFA) and mitigation of CVEs, underpin the commitment to secure automation.
- **User Experience:** Features and upgrades designed to simplify usage, improve error handling, and bolster reliability.
- **AI Integration:** New 'What-If' and 'Export Bicep' parameters leverage AI to assist with previewing infrastructure changes and auto-generating Bicep templates, streamlining infrastructure-as-code workflows.

## Feature Highlights

### Security Enhancements

- **MFA Enforcement:** Azure Resource Manager (ARM) now requires MFA for sensitive operations, with improved claims challenge mechanisms and troubleshooting guidance.
- **CVE Mitigations:** Azure CLI upgraded from version 2.76 to 2.77 to close remote code execution and certificate validation vulnerabilities, in full alignment with security best practices.

### Python 3.13 Support

- Azure CLI now requires Python 3.13 for strict SSL verification, improving security but potentially causing proxy compatibility issues. Users should update proxy certificates accordingly.

### New Modules, Extensions, and Service Coverage

- **Services Updated:** ACR, ACS, AKS, App Config, App Service, ARM, Backup, Batch, Compute, Cosmos DB, Cognitive Services, IoT, Key Vault, SQL, Storage, and more.
- **Extensions Added:** arize-ai, connectedmachine, containerapp, lambda-test, migrate, neon, pscloud, sftp, site, storage-blob-preview.
- **GA Modules:** DeviceRegistry, DataMigration, FirmwareAnalysis, LoadTesting, StorageDiscovery, DataTransfer, ArizeAI, Fabric, StorageAction, Oracle.

### ARM API and Endpoint Discovery

- Azure CLI now leverages ARM API version 2022-09-01 for more accurate cloud registration and endpoint management, supporting better service compatibility and forward-compatibility.

### Pagination Support in PowerShell

- **Invoke-AzRestMethod** gains a new '-Paginate' parameter to improve efficiency with large datasets, supporting server-driven pagination while maintaining backward compatibility.

### Intelligent Change Preview and Template Export

- **What-If Analysis:** Preview infrastructure modifications before deployment, identifying resources impacted by commands.
- **Export Bicep:** Automatically generate Bicep templates to speed up infrastructure-as-code adoption, both features utilizing AI-assisted logic.

## Handling Claims Challenges for MFA

- When performing create, update, or delete operations, users must address claims challenges issued by ARM if MFA is not configured, with well-documented guidance for both Azure CLI and PowerShell workflows.

**Azure CLI Example:**

```bash
az login --tenant "aaaabbbb-0000-cccc-1111-dddd2222eeee" --scope "https://management.core.windows.net//.default" --claims-challenge "<claims-challenge-token>"
```

**Azure PowerShell Example:**

```powershell
Connect-AzAccount -Tenant yyyyyyyy-yyyy-yyyy-yyyy-yyyyyyy -Subscription zzzzzzzz-zzzz-zzzz-zzzz-zzzzzzzz -ClaimsChallenge <claims-challenge-token>
```

- [Troubleshooting Azure CLI MFA](https://learn.microsoft.com/cli/azure/use-azure-cli-successfully-troubleshooting#troubleshooting-multifactor-authentication-mfa)
- [Troubleshooting Az PowerShell MFA](https://learn.microsoft.com/powershell/azure/troubleshooting#troubleshooting-multifactor-authentication-mfa)

## Breaking Changes and Migration Guidance

- Guidance and migration documents for the latest breaking changes:
  - [Azure CLI Release Notes](https://learn.microsoft.com/cli/azure/release-notes-azure-cli)
  - [Azure PowerShell Migration Guide](https://learn.microsoft.com/powershell/azure/migrate-az-15.0.0)
  - [Azure CLI Milestones](https://github.com/Azure/azure-cli/milestones)
  - [Azure PowerShell Milestones](https://github.com/Azure/azure-powershell/milestones)

## Get Involved

- Feedback and updates are welcome on GitHub:
  - [Azure CLI GitHub](https://github.com/Azure/azure-cli)
  - [Azure PowerShell GitHub](https://github.com/Azure/azure-powershell)
- Follow updates on X (Twitter): [@azureposh](https://twitter.com/azureposh), [@AzureCli](https://twitter.com/azurecli)

## Additional Resources

- [Azure CLI Release Notes](https://learn.microsoft.com/cli/azure/release-notes-azure-cli)
- [Azure PowerShell Release Notes](https://learn.microsoft.com/powershell/azure/release-notes-azureps)
- [Azure cloud management with Azure CLI](https://learn.microsoft.com/cli/azure/manage-clouds-azure-cli)
- [Invoke-AzRestMethod documentation](https://learn.microsoft.com/powershell/module/az.accounts/invoke-azrestmethod)
- [Preview Signup for AI Features](https://aka.ms/PreviewSignupPSCLI)

---

This update was prepared by Alex-wdy for the Tech Community. Stay tuned for further features and improvements in the Azure command-line ecosystem.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-tools-blog/azure-cli-and-azure-powershell-ignite-2025-announcement/ba-p/4471182)
