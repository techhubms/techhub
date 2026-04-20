---
title: April Patches for Azure DevOps Server
external_url: https://devblogs.microsoft.com/devops/april-patches-for-azure-devops-server/
author: Gloridel Morales
feed_name: Microsoft DevOps Blog
date: 2026-04-14 19:18:22 +00:00
section_names:
- devops
- security
primary_section: devops
tags:
- Azure DevOps
- Azure DevOps Server
- CheckInstall
- Connectivity
- DevOps
- GitHub Enterprise Server
- Installation Verification
- Malicious Redirects
- News
- Null Reference Exception
- Open Redirect
- PAT
- Patch Installer
- Patch Management
- Patches
- Personal Access Token
- Pull Requests
- Release Notes
- Security
- Self Hosted
- Sign Out Validation
- Work Item Auto Completion
---

Gloridel Morales announces April patches for Azure DevOps Server, summarizing key fixes (pull request completion reliability, safer sign-out redirect validation, and GitHub Enterprise Server PAT connection) and showing how to verify the patch is installed.<!--excerpt_end-->

# April Patches for Azure DevOps Server

We are releasing patches for our self-hosted product, [Azure DevOps Server](https://azure.microsoft.com/services/devops/server/). We strongly recommend that all customers remain on the latest, most secure version to ensure optimal protection and reliability. The latest release of Azure DevOps Server is available from the [download page](https://learn.microsoft.com/azure/devops/server/download/azuredevopsserver?view=azure-devops).

## What this patch includes

This patch applies to the most recent version of Azure DevOps Server and includes the following updates:

- Fixed an issue where completing a pull request could fail due to a null reference exception during work item auto-completion.
- Improved validation during sign out to prevent potential malicious redirects.
- Fixed creating PAT connection to GitHub Enterprise Server.

## Download

| Version | Patch download | Release notes |
| --- | --- | --- |
| Azure DevOps Server | [Download Patch 3](https://aka.ms/devopsserverpatch3) | [Release notes](https://learn.microsoft.com/azure/devops/server/release-notes/azuredevopsserver?view=azure-devops#azure-devops-server-patch-3-release-date-april-14-2026) |

## Verifying installation

To verify that the patch is installed, run the following command on the Azure DevOps Server machine using the patch installer you downloaded:

```bash
<patch-installer>.exe CheckInstall
```

Replace `<patch-installer>` with the name of the patch file you downloaded. The command output will indicate whether the patch is installed.


[Read the entire article](https://devblogs.microsoft.com/devops/april-patches-for-azure-devops-server/)

