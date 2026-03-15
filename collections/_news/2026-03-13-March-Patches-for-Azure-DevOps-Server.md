---
external_url: https://devblogs.microsoft.com/devops/march-patches-for-azure-devops-server-4/
title: March Patches for Azure DevOps Server
author: Gloridel Morales
primary_section: azure
feed_name: Microsoft DevOps Blog
date: 2026-03-13 21:41:24 +00:00
tags:
- Azure
- Azure DevOps Server
- DevOps
- DevOps Tools
- Download Instructions
- Group Membership
- Installation Verification
- Microsoft Azure
- News
- On Premises DevOps
- Patch 2
- Patches
- Release Notes
- Security Patch
section_names:
- azure
- devops
---
Gloridel Morales details the March patch release for Azure DevOps Server, providing guidance on installation, verification, and who needs the update for secure DevOps operations.<!--excerpt_end-->

# March Patches for Azure DevOps Server

**Author:** Gloridel Morales

## Overview

Microsoft has released a new security patch (Patch 2) for the self-hosted product Azure DevOps Server. This patch is intended to address an issue in the original Azure DevOps Server release that, under certain conditions, could lead to group memberships being deactivated. Customers are strongly encouraged to keep their installations up to date to maintain security and functionality.

## Who Should Install This Patch

- **Applies to:** Customers who installed Azure DevOps Server prior to the re-published release on March 13, 2026.
  - If you previously used the earlier mitigation script, installing Patch 2 completes the remediation.
  - If you have **not** run the mitigation script, simply installing Patch 2 is sufficient.
- **Does NOT apply to:** Customers performing new installations or upgrades using the re-published release available as of March 13, 2026.

## How to Download and Install Patch 2

- Access the patch download from the [official Azure DevOps Server page](https://learn.microsoft.com/azure/devops/server/download/azuredevopsserver?view=azure-devops).
- Direct link: [Download Patch 2](https://aka.ms/devopsserverpatch2)
- Review detailed [release notes](https://learn.microsoft.com/azure/devops/server/release-notes/azuredevopsserver?view=azure-devops#azure-devops-server-patch-2-release-date-march-13-2026) for information on patch contents and changes.

## Verifying Patch Installation

To ensure Patch 2 has been successfully installed, run the following command on your Azure DevOps Server machine using the patch installer:

```sh
<patch-installer>.exe CheckInstall
```

Replace `<patch-installer>` with the actual file name of the downloaded patch installer. The command will confirm if the patch is installed correctly.

## Additional Resources

- [Azure DevOps Server Product Page](https://azure.microsoft.com/services/devops/server/)
- [Azure DevOps Server Patch Download](https://aka.ms/devopsserverpatch2)
- [Patch Release Notes](https://learn.microsoft.com/azure/devops/server/release-notes/azuredevopsserver?view=azure-devops#azure-devops-server-patch-2-release-date-march-13-2026)

## Summary

Keeping Azure DevOps Server updated with the latest patches is vital for operational stability and security, particularly for on-premises deployments. Follow the above steps to ensure your environment is protected against the latest identified issue.

This post appeared first on "Microsoft DevOps Blog". [Read the entire article here](https://devblogs.microsoft.com/devops/march-patches-for-azure-devops-server-4/)
