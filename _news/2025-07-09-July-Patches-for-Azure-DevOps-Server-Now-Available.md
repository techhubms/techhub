---
layout: "post"
title: "July Patches for Azure DevOps Server Now Available"
description: "Microsoft has released new patches for Azure DevOps Server, addressing a critical issue in multi-repo trigger functionality. The post urges all users to upgrade to the latest, most secure release and provides details on downloading and verifying patch installation for Azure DevOps Server 2020.1.2 Patch 17."
author: "Gloridel Morales"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/devops/july-patches-for-azure-devops-server-2/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/devops/feed/"
date: 2025-07-09 15:37:13 +00:00
permalink: "/2025-07-09-July-Patches-for-Azure-DevOps-Server-Now-Available.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["Azure", "Azure DevOps Server", "DevOps", "Microsoft", "Multi Repo Trigger", "News", "Null Reference Exception", "On Premises DevOps", "Patch Installation", "Patches", "Release Notes", "Security", "Security Update", "YAML Pipelines"]
tags_normalized: ["azure", "azure devops server", "devops", "microsoft", "multi repo trigger", "news", "null reference exception", "on premises devops", "patch installation", "patches", "release notes", "security", "security update", "yaml pipelines"]
---

Authored by Gloridel Morales, this post announces July patches for Azure DevOps Server, highlighting key fixes and steps for users to ensure secure and updated deployments.<!--excerpt_end-->

## July Patches for Azure DevOps Server

**Author:** Gloridel Morales

Today, Microsoft has released important patches affecting the latest version of its self-hosted DevOps product, [Azure DevOps Server](https://azure.microsoft.com/services/devops/server/). Microsoft strongly encourages all customers to use the newest, most secure release of Azure DevOps Server. The latest available version is Azure DevOps Server 2022.2, which can be downloaded from the [Azure DevOps Server download page](https://learn.microsoft.com/azure/devops/server/download/azuredevopsserver?view=azure-devops).

### Details for Azure DevOps Server 2020.1.2 Patch 17

- **Release Notes:** For specifics, see the official [release notes](https://learn.microsoft.com/azure/devops/server/release-notes/azuredevops2020u1?view=azure-devops#azure-devops-server-2020-update-12-patch-17-release-date-july-9-2025).

#### Update Recommendation

If you are running Azure DevOps Server 2020.1.2, install [Patch 17](https://aka.ms/devops2020.1.2patch17) to benefit from the latest security and product experience enhancements.

#### Issues Addressed

This patch addresses a null reference exception within the multi-repo trigger functionality. The issue occurs when no resource repositories are defined in YAML pipelines, potentially causing disruptions or unexpected behavior.

#### Patch Verification Steps

To verify that the patch has been correctly installed:

1. Download `devops2020.1.2patch17.exe` from the provided link.
2. Run the following command:

   ```
   devops2020.1.2patch17.exe CheckInstall
   ```

3. The output will confirm whether the patch is installed or not.

For those maintaining on-premises Azure DevOps Server deployments, timely patching is essential for maintaining security and system stability.

---

For more information, visit the [Azure DevOps Blog](https://devblogs.microsoft.com/devops).

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/devops/july-patches-for-azure-devops-server-2/)
