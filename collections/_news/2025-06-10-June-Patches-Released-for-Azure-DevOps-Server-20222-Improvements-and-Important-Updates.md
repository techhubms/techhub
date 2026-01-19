---
external_url: https://devblogs.microsoft.com/devops/june-patches-for-azure-devops-server-4/
title: 'June Patches Released for Azure DevOps Server 2022.2: Improvements & Important Updates'
author: Gloridel Morales
viewing_mode: external
feed_name: Microsoft DevBlog
date: 2025-06-10 19:13:51 +00:00
tags:
- Azure DevOps Server
- Custom Columns
- Installation
- Patches
- Plan Id
- Release Notes
- Server Upgrade
- Suite Id
- Suite Import
- Test Plans
- XLSX Export
section_names:
- azure
- devops
- security
---
Gloridel Morales announces the June patches for Azure DevOps Server 2022.2, highlighting Test Plans enhancements and addressing an ongoing issue with Patch 6, along with installation recommendations and verification steps.<!--excerpt_end-->

## June Patches for Azure DevOps Server 2022.2

**Author:** Gloridel Morales

**Update (July 25):**
We are currently investigating an issue with Patch 6 for Azure DevOps Server 2022.2. Our team is actively working to identify the root cause and implement a resolution as quickly as possible. Updates and further details will be provided in this blog as they become available. Thank you for your patience and understanding.

---

### Patch Release Overview

Today, Microsoft has released new patches that impact the latest version of the self-hosted product, [Azure DevOps Server](https://azure.microsoft.com/services/devops/server/).

- **Recommendation:** All customers are strongly encouraged to use the latest, most secure release of Azure DevOps Server.
- The current version is **Azure DevOps Server 2022.2**, available from the [Azure DevOps Server download page](https://learn.microsoft.com/azure/devops/server/download/azuredevopsserver?view=azure-devops).

> **Note:**
> - Azure DevOps Server 2022.2 is the latest version.
> - If you have previously installed Azure DevOps Server 2022 or Azure DevOps Server 2022.1, you should upgrade to Azure DevOps Server 2022.2 and then install Patch 6 for best security and latest features.

---

### Details of Azure DevOps Server 2022.2 Patch 6

- **Patch Location:** [Patch 6 Download Link](https://aka.ms/devops2022.2patch6)
- **Release Notes:** [Azure DevOps Server 2022 Update 2 Patch 6 Release Notes – June 10, 2025](https://learn.microsoft.com/azure/devops/server/release-notes/azuredevops2022u2?view=azure-devops#azure-devops-server-2022-update-2-patch-6-release-datejune-10-2025)

#### Test Plans Enhancements in Patch 6

- **Export Test Cases with Custom Columns to XLSX:**
  - Test Plans now supports exporting test cases with custom columns, providing greater flexibility and control over the data you share and analyze. This allows users to tailor exports to their needs, ensuring relevance and actionability of exported information.
- **Import Suites & Copy Test Cases with Plan Id and Suite Id (in search only):**
  - Enhanced capabilities for importing test suites and copying test cases, supporting Plan Id and Suite Id for improved management and searchability.

#### Installation Instructions

- **To verify installation:**
  - Run the command: `devops2022.2patch6.exe CheckInstall` (where `devops2022.2patch6.exe` is the downloaded patch executable).
  - The output will confirm whether the patch is installed or not.

### Ongoing Issue with Patch 6

As of July 25, the Microsoft team is actively investigating an issue related to Patch 6 for Azure DevOps Server 2022.2. Updates, fixes, and further information will be provided in the same blog as they become available.

---

**For optimal security and the latest features, Microsoft recommends staying on current supported versions and applying all security patches as they are released.**

For more information and future updates regarding this patch, refer to the [Azure DevOps Blog](https://devblogs.microsoft.com/devops/june-patches-for-azure-devops-server-4/).

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/devops/june-patches-for-azure-devops-server-4/)
