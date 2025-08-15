---
layout: "post"
title: "Azure DevOps Access Level Persisting After Visual Studio License Removal"
description: "A detailed community case examining issues with reverting a user's Azure DevOps access level from Visual Studio Enterprise subscription to Stakeholder. The discussion covers user license assignment through on-prem AD and Entra ID synchronization, observed behaviors following license removal, and attempts to manually reset permissions and access levels."
author: "dbenson"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure/unable-to-revert-azure-devops-user-access-level/m-p/4442871#M22102"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-08-13 12:05:29 +00:00
permalink: "/2025-08-13-Azure-DevOps-Access-Level-Persisting-After-Visual-Studio-License-Removal.html"
categories: ["Azure", "DevOps"]
tags: ["Access Level", "Azure", "Azure Active Directory", "Azure DevOps", "Community", "DevOps", "Entra ID", "Identity Management", "License Synchronization", "On Premises AD", "Permissions", "Stakeholder Access", "Subscription Management", "User Licensing", "Visual Studio Enterprise"]
tags_normalized: ["access level", "azure", "azure active directory", "azure devops", "community", "devops", "entra id", "identity management", "license synchronization", "on premises ad", "permissions", "stakeholder access", "subscription management", "user licensing", "visual studio enterprise"]
---

dbenson shares a technical scenario involving Azure DevOps user access levels, highlighting the challenges of reverting a user's access from Visual Studio Enterprise subscription to Stakeholder after license group removal.<!--excerpt_end-->

# Azure DevOps Access Level Persisting After Visual Studio License Removal

dbenson describes a scenario where a user's Visual Studio Enterprise subscription access in Azure DevOps persists, even after removing the license through group changes in on-premises Active Directory and synchronization to Entra ID (formerly Azure AD).

## Scenario Overview

- **Initial Setup**: User assigned to an on-prem AD group granting Visual Studio Enterprise subscription. This assignment is replicated to Entra ID, and Azure DevOps recognizes the license.
- **License Removal**: User is removed from the on-prem AD group; this replicates to Entra ID, and the Visual Studio license is removed as confirmed on the license page.
- **Access Level Behavior**: Despite the removal, Azure DevOps still shows the user's access level as Visual Studio Enterprise subscription.
- **Manual Changes Attempted**:
  - Manually changed user's access level to Stakeholder.
  - On login, access level reverts to Visual Studio Enterprise subscription.
  - Removed user from Azure DevOps organization and re-added with Stakeholder level and project permissions.
  - On next login, user still defaults to Visual Studio Enterprise subscription, bypassing expected defaults.

## Observed Issue

- Attempting to revert access fails, as Azure DevOps appears to recognize a license that is no longer present in Entra ID.
- The problem persists even after removing and re-adding the user.

## Questions Raised

- Why does Azure DevOps maintain the higher access even after the license group and assignment have been revoked?
- Is there additional entitlements or synchronization logic causing Azure DevOps to override manual settings?
- What steps are needed to ensure the user's access is properly set to Stakeholder after license removal?

## Key Takeaways

- Licensing and access level synchronization between on-prem AD, Entra ID, and Azure DevOps can lead to persistent access issues.
- Manual intervention in Azure DevOps may not be sufficient when automated licensing processes are in place.
- Further investigation may be required in license synchronization flows, cached entitlements, or Azure DevOps organization settings.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure/unable-to-revert-azure-devops-user-access-level/m-p/4442871#M22102)
