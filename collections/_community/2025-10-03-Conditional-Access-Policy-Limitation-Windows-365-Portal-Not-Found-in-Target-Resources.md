---
layout: "post"
title: "Conditional Access Policy Limitation: Windows 365 Portal Not Found in Target Resources"
description: "This post explains a challenge encountered when configuring Conditional Access (CA) policies for external users in Microsoft Entra ID, specifically the inability to add 'Windows 365 Portal' as an exception in CA policy resources. The author documents tests with both the Windows App client and web interface when trying to allow access to Azure Virtual Desktop while blocking other Microsoft 365 resources. Insights include resource IDs, application IDs, and the limitations faced when the relevant application does not appear in the exception list. The post seeks solutions from the technical community."
author: "WF-PHG"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-virtual-desktop/ca-policy-application-not-found-in-target-resources/m-p/4458834#M13916"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-10-03 14:06:51 +00:00
permalink: "/community/2025-10-03-Conditional-Access-Policy-Limitation-Windows-365-Portal-Not-Found-in-Target-Resources.html"
categories: ["Azure", "Security"]
tags: ["Access Control", "Application ID", "Azure", "Azure Active Directory", "Azure Virtual Desktop", "Community", "Conditional Access", "Entra ID", "External Users", "MFA", "Microsoft 365", "Resource ID", "Security", "Windows 365", "Windows App"]
tags_normalized: ["access control", "application id", "azure", "azure active directory", "azure virtual desktop", "community", "conditional access", "entra id", "external users", "mfa", "microsoft 365", "resource id", "security", "windows 365", "windows app"]
---

WF-PHG highlights a Conditional Access policy issue in Microsoft Entra ID: the inability to configure access exceptions for the 'Windows 365 Portal' when only Azure Virtual Desktop and Security Info should be accessible for external users.<!--excerpt_end-->

# Conditional Access Policy Application Not Found in Target Resources

**Author:** WF-PHG

## Scenario Overview

WF-PHG describes a scenario where Conditional Access (CA) policies are set for external users (created in AD and synchronized to Entra ID). The policy blocks access to all Microsoft 365 resources except:

- **Azure Virtual Desktop** (Resource ID: 9cdead84-a844-4324-93f2-b2e6bb768d07)
- **Security Info** (for MFA setup)

## Issue Encountered

With Microsoft rolling out the new Windows App (intended to replace the old Remote Desktop app and web interface), the author attempted to use:

- **Windows App installed on a PC**: Accessed Azure Virtual Desktop successfully using `Windows 365 Client` (Application ID: 4fb5cc57-dbbc-4cdc-9595-748adff5f414, Resource ID matches Azure Virtual Desktop).
- **Windows App web interface** (https://windows.cloud.microsoft/): Attempted sign-in results in access denied. The sign-in logs show the attempted resource is:
  - Application: Windows 365 Portal
  - Application ID: 3b511579-5e00-46e1-a89e-a6f0870e2f5a
  - Resource: Windows 365 Portal
  - Resource ID: 3b511579-5e00-46e1-a89e-a6f0870e2f5a

## Policy Configuration Limitation

- The author cannot find `Windows 365 Portal` in the list of applications that can be set as an exception in the CA policy.
- The closest match, `Windows 365`, does **not** resolve the access problem, as it uses a different resource ID.

## Questions Raised

- How can administrators allow access to `Windows 365 Portal` via the CA policy if it cannot be found in the target resource list?
- Is there a workaround or update required from Microsoft?

## Key Technical Details

- Synchronized users between Active Directory and Entra ID
- Resource IDs and Application IDs explicitly listed for troubleshooting
- Differentiation between desktop client and web interface access flows

## Summary

This scenario highlights a technical limitation in CA policy resource targeting, particularly with new service endpoints (such as the Windows App web interface) that may not yet appear in admin configuration tools. Resolution likely requires Microsoft support escalation, new documentation, or product updates affecting resource visibility in Conditional Access.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-virtual-desktop/ca-policy-application-not-found-in-target-resources/m-p/4458834#M13916)
