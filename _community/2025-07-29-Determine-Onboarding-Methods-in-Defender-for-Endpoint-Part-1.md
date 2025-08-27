---
layout: "post"
title: "Determine Onboarding Methods in Defender for Endpoint - Part 1"
description: "This article by edgarus71 explores the various methods to onboard devices into Microsoft Defender for Endpoint, the significance of tracking how each device was onboarded, and practical troubleshooting tips based on registry keys and log files. It also addresses co-management scenarios and provides insights for large-scale device management."
author: "edgarus71"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/core-infrastructure-and-security/determine-onboarding-methods-in-defender-for-endpoint-part-1/ba-p/4437782"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-07-29 14:00:39 +00:00
permalink: "/2025-07-29-Determine-Onboarding-Methods-in-Defender-for-Endpoint-Part-1.html"
categories: ["Azure", "Security"]
tags: ["Azure", "BKMK EPLog", "Co Management", "Community", "Defender For Endpoint", "Device Onboarding", "EndpointProtectionAgent.log", "GPO", "Group Policy", "Local Script", "MCM", "Microsoft Configuration Manager", "Microsoft Intune", "Onboard", "Registry Keys", "Security", "Troubleshoot", "Troubleshooting", "Understanding", "Windows Endpoint"]
tags_normalized: ["azure", "bkmk eplog", "co management", "community", "defender for endpoint", "device onboarding", "endpointprotectionagentdotlog", "gpo", "group policy", "local script", "mcm", "microsoft configuration manager", "microsoft intune", "onboard", "registry keys", "security", "troubleshoot", "troubleshooting", "understanding", "windows endpoint"]
---

edgarus71 details approaches for onboarding devices to Defender for Endpoint, distinguishing between methods like Intune, GPO, and MCM, and providing troubleshooting advice.<!--excerpt_end-->

## Summary

In this article, edgarus71 provides a comprehensive overview of the methods available for onboarding Windows devices into Microsoft Defender for Endpoint. The author discusses the practical distinctions between onboarding via Local Script, Group Policy Objects (GPO), Microsoft Configuration Manager (MCM), and Mobile Device Management (MDM) tools such as Microsoft Intune. The importance of determining and tracking which method was used—primarily for troubleshooting and streamlining large deployments—is emphasized throughout the article.

## Onboarding Methods

- **Local Script**: Useful for small-scale or PoC deployments. Presents a license prompt on the device, making it unsuitable for large-scale automation.
- **Group Policy (GPO)**: Designed for on-premises environments; avoids the license prompt and is more scalable for domain-joined devices.
- **Microsoft Configuration Manager (MCM)**: Preferred in organizations with established device management via MCM, especially for servers and complex, automated environments.
- **MDM/Intune**: Ideal for modern device management, enabling simple, policy-driven, bulk onboarding for client devices, but not Windows Server.
- **VDI Scripts**: Mentioned but not discussed in this part.

## Why Track the Onboarding Method?

Tracking the onboarding method is valuable for:

- Easing future deployments and migrations.
- Gaining visibility into the types of managed devices and associated platforms.
- Improving troubleshooting by narrowing focus on the platform and potential conflicting policies.

## Where to Find Method Indicators

The article provides practical guidance on identifying onboarding methods:

- **Intune**: Presence of the `MdmSubscriberIds` registry key at `HKLM\SOFTWARE\Microsoft\Windows Defender`.
- **Local Script**: Look for the `latency` key (value 'demo') at `HKLM\SOFTWARE\Policies\Microsoft\Windows Advanced Threat Protection`.
- **MCM**: Checks for the existence of `C:\Windows\CCM\Logs\EndpointProtectionAgent.log`.
- **GPO**: OnboardingState at `HKLM\SOFTWARE\Microsoft\Windows Advanced Threat Protection\Status` should be 1, latency key absent at the relevant location.

## Co-management Scenarios

For Windows clients managed by both Intune and MCM (co-management), both indicators may appear: the Intune registry key, and the presence of MCM log files. Conflicting or overlapping indicators are detailed, stressing the need to carefully evaluate which platform handled onboarding versus ongoing management.

## Troubleshooting Considerations

- Each method leaves specific traces, such as registry keys or log files, but overlaps can occur—especially if management responsibilities are split or have shifted over time.
- PowerShell and automation can be used for discovery at scale, which will be explored in a follow-up article.
- The article also lists tips and official resources for further troubleshooting onboarding issues.

## Key Takeaways

- Distinguishing onboarding methods is crucial for diagnosing device registration issues within Defender for Endpoint.
- Each onboarding route is recommended for certain environments or scenarios.
- Careful tracking, leveraging system indicators, simplifies troubleshooting and supports effective device management.

## References & Further Reading

- [Log file reference for MCM](https://learn.microsoft.com/en-us/intune/configmgr/core/plan-design/hierarchy/log-files#BKMK_EPLog)
- [MDMDiagReport_RegistryDump.reg review](https://learn.microsoft.com/en-us/windows/client-management/mdm-collect-logs#understanding-zip-structure)
- [Onboard devices in Defender for Endpoint using a GPO](https://learn.microsoft.com/en-us/defender-endpoint/configure-endpoints-gp)
- [Onboard devices using Microsoft Intune](https://learn.microsoft.com/en-us/intune/intune-service/protect/advanced-threat-protection-configure#onboard-windows-devices)
- [Troubleshooting Defender for Endpoint onboarding](https://learn.microsoft.com/en-us/defender-endpoint/troubleshoot-onboarding)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/core-infrastructure-and-security/determine-onboarding-methods-in-defender-for-endpoint-part-1/ba-p/4437782)
