---
layout: "post"
title: "Windows 365 Reserve: Secure On-Demand Cloud PCs for Business Continuity"
description: "This article provides an in-depth introduction to Windows 365 Reserve, a new Microsoft offering currently in limited public preview. It enables organizations to provide employees with secure, temporary Cloud PCs on demand when their primary devices are unavailable. The guide details admin setup in Intune, provisioning policies, license structure, security features, and end-user experiences, alongside practical guidance for responding to device outages. It also highlights integration with Microsoft Intune and Zero Trust security, and outlines steps for joining the public preview."
author: "Logan_Silliman"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/windows-it-pro-blog/enhancing-business-continuity-windows-365-reserve-is-now-in/ba-p/4441669"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-11 16:00:00 +00:00
permalink: "/2025-08-11-Windows-365-Reserve-Secure-On-Demand-Cloud-PCs-for-Business-Continuity.html"
categories: ["Azure", "Security"]
tags: ["Azure", "Azure Virtual Desktop", "Business Continuity", "Cloud PC", "Community", "Device Management", "Device Outage", "Endpoint Security", "Enterprise IT", "IT Administration", "Microsoft Entra", "Microsoft Intune", "Provisioning Policy", "Security", "Temporary Cloud Access", "Windows 365 Reserve", "Zero Trust"]
tags_normalized: ["azure", "azure virtual desktop", "business continuity", "cloud pc", "community", "device management", "device outage", "endpoint security", "enterprise it", "it administration", "microsoft entra", "microsoft intune", "provisioning policy", "security", "temporary cloud access", "windows 365 reserve", "zero trust"]
---

Logan Silliman explains Windows 365 Reserve, Microsoft’s new solution for providing secure, on-demand Cloud PCs during device outages. The post covers admin management in Intune, security design, and end-user experience.<!--excerpt_end-->

# Windows 365 Reserve: Secure On-Demand Cloud PCs for Business Continuity

**Author:** Logan Silliman  
**Updated:** Aug 08, 2025

## Overview

Windows 365 Reserve is a new standalone Windows 365 feature providing organizations with the ability to grant employees secure, temporary Cloud PC access (up to 10 days/year) when primary devices are unavailable. Designed for fast recovery from device failures or incidents like malware and theft, this solution ensures workforce productivity and operational resiliency.

---

## Key Features and Benefits

- **Rapid Cloud PC Deployment:** Admins can quickly provision pre-configured Cloud PCs with all the necessary apps and security for users whose physical devices are lost or under repair.
- **Centralized IT Management:** Provision and manage these temporary Cloud PCs within Microsoft Intune using organization-wide policies.
- **Secure by Design:** Integrates Zero Trust principles—access is managed, can be revoked at any time, and inherits organization security baselines.
- **Flexible Access:** Employees use any secondary device to connect—managed or personal—via web or the Windows App, supporting remote and hybrid work needs.
- **Operational Control:** Licenses allow up to 10 days of Cloud PC use per user/year, consumable in multiple sessions. Timely notifications help admins and users manage and preserve allotted time efficiently.

---

## Admin Experience

- **Simple Setup:** After purchasing licenses, setup is done within Intune. Admins create provisioning policies specifying:
  - Cloud PC geography
  - Microsoft Entra user groups for coverage
  - (Optionally) Gallery image version, language, and scope tags
- **Default Selections:** To simplify rapid deployment, some configuration (Cloud PC size, region, and network) is managed automatically.
- **Intune Integration:** Deployment, access, and security control are unified in Microsoft Intune, minimizing IT overhead during disruptions.
- **Provisioning Delay:** Policies and group assignments must be in place seven days before distributing Cloud PCs.
- **Usage and Deprovisioning:** When issues are resolved, Cloud PCs can be deprovisioned to save unused access days. Deprovisioning is handled through Intune.

---

## End-User Experience

- **Accessible from Anywhere:** Users connect to their temporary Cloud PC from any device using the web or the Windows App.
- **Clear Guidance:** Users are informed of expiration dates and receive prompts to deprovision when their primary device is restored.
- **Security Maintained:** All access occurs within organization's compliance and security policies.

---

## Security and Compliance

- **Zero Trust Principles:** All provisioning and deprovisioning actions are under admin control. Access can be revoked at any time.
- **Network and Capacity:** Requires network connectivity and may face Azure regional capacity constraints.
- **Audit and Reporting:** Use Intune reporting for access management and incident tracking.

---

## How to Join the Preview

Windows 365 Reserve is in a gated public preview. To participate:

- [Complete this form](https://forms.office.com/pages/responsepage.aspx?id=v4j5cvGGr0GRqy180BHbR35z8y2oNjBOmsgL_HrnYKlUNDJKREdZSDBPOVdWSktZQ0VKNzYxR05UViQlQCNjPTEu&route=shorturl)  
- Or reach out to your Microsoft account team

---

## Additional Resources

- [Original disclosure](https://blogs.windows.com/windowsexperience/2025/06/18/strengthen-business-resilience-with-windows-365-and-azure-virtual-desktop/)
- [Windows Tech Community](http://aka.ms/community/Windows)
- [Windows App on all major platforms](https://techcommunity.microsoft.com/blog/windows-itpro-blog/windows-app-now-available-on-all-major-platforms/4246939)
- Support: [Windows on Microsoft Q&A](https://docs.microsoft.com/answers/products/windows#windows-client-for-it-pros)

---

*Note: Windows 365 Reserve is a preview feature. Details and features may change before general availability.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/windows-it-pro-blog/enhancing-business-continuity-windows-365-reserve-is-now-in/ba-p/4441669)
