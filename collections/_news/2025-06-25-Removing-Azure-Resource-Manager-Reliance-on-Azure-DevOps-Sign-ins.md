---
layout: "post"
title: "Removing Azure Resource Manager Reliance on Azure DevOps Sign-ins"
description: "Azure DevOps will no longer require Azure Resource Manager (ARM) resources for Microsoft Entra sign-ins and token refreshes. Organizations should update their Conditional Access policies to specifically target Azure DevOps. These changes, impacting user authentication and access, take effect from July 28, 2025, and September 2, 2025."
author: "Angel Wong"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/devops/removing-azure-resource-manager-reliance-on-azure-devops-sign-ins/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/devops/feed/"
date: 2025-06-25 18:24:26 +00:00
permalink: "/news/2025-06-25-Removing-Azure-Resource-Manager-Reliance-on-Azure-DevOps-Sign-ins.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["Access Control", "ARM", "Authentication", "Azure", "Azure & Cloud", "Azure DevOps", "Azure Resource Manager", "Cloud Security", "Conditional Access", "DevOps", "Identity Management", "Microsoft Entra", "Microsoft Visual Studio Team Services", "News", "Policy Management", "Security", "Token Management", "User Sign in"]
tags_normalized: ["access control", "arm", "authentication", "azure", "azure and cloud", "azure devops", "azure resource manager", "cloud security", "conditional access", "devops", "identity management", "microsoft entra", "microsoft visual studio team services", "news", "policy management", "security", "token management", "user sign in"]
---

Angel Wong announces important authentication updates for Azure DevOps, moving away from Azure Resource Manager dependence for Entra sign-ins. This guide helps administrators prepare for the coming changes in Conditional Access requirements.<!--excerpt_end-->

## Removing Azure Resource Manager Reliance on Azure DevOps Sign-ins

**Author:** Angel Wong

Azure DevOps will no longer depend on the Azure Resource Manager (ARM) resource (`https://management.azure.com`) when users sign in or refresh Microsoft Entra access tokens. Previously, ARM acted as a required audience for these processes, forcing administrators to ensure that all Azure DevOps users met ARM-based Conditional Access policies to maintain access.

### What’s Changing?

- **ARM audience is no longer required for Azure DevOps tokens.**
- Administrators can now create Conditional Access policies that specifically target Azure DevOps, rather than depending on ARM-based enforcement to manage Azure DevOps access.
- **Timeline:**
  - These changes come into effect on **July 28, 2025** and **September 2, 2025**.

### Does This Change Affect Me?

If you previously set up a Conditional Access policy specifically for the [Windows Azure Service Management API application](https://learn.microsoft.com/en-us/entra/identity/conditional-access/concept-conditional-access-cloud-apps#windows-azure-service-management-api), be aware:

- **This policy will no longer cover Azure DevOps sign-ins.**
- You must create a new Azure DevOps-exclusive Conditional Access policy to continue enforcing access for ADO users.

### Creating a Conditional Access Policy for Azure DevOps

As a tenant admin, use [Conditional Access policies](https://learn.microsoft.com/en-us/entra/identity/conditional-access/overview) to grant or block user access to Azure resources based on conditions such as:

- Accepted IP addresses
- Membership in specific Entra groups
- Use of recognized devices
- Completion of multifactor authentication

**To create a policy for Azure DevOps:**

1. Open the [Azure Portal](https://portal.azure.com) and navigate to **Microsoft Entra Conditional Access**.
2. Select **Policies** in the sidebar.
3. Click **+ New policy**.
4. Name your policy and configure other desired settings.
5. Under **Target resources**, choose **Select resources**, and add **“Microsoft Visual Studio Team Services”** or **“Azure DevOps”** (resource id: `499b84ac-1321-427f-aa17-267ca6975798`) as targets.
6. Click **Save** to enforce the new policy.

![Setup a new conditional access policy for Azure DevOps](https://devblogs.microsoft.com/devops/wp-content/uploads/sites/6/2025/06/ADO-CAP-1.png)

Explore further Conditional Access policy options and recommendations in the [Microsoft Entra documentation](https://learn.microsoft.com/en-us/entra/identity/conditional-access/).

### Notable Exceptions

Some users will continue to require access to ARM:

- **Billing administrators:** Need ARM to set up billing and access subscriptions.
- **Service Connection creators:** Require ARM for role assignments and updates to managed service identities (MSIs).

**Summary:**

- Azure DevOps is removing its dependency on ARM resources for sign-in and token refresh.
- Administrators should proactively update Conditional Access policies to explicitly secure Azure DevOps usage.
- Plan these changes ahead of the enforcement dates to avoid access issues for your users.

For a detailed guide and the official announcement, visit the [Azure DevOps Blog](https://devblogs.microsoft.com/devops/removing-azure-resource-manager-reliance-on-azure-devops-sign-ins/).

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/devops/removing-azure-resource-manager-reliance-on-azure-devops-sign-ins/)
