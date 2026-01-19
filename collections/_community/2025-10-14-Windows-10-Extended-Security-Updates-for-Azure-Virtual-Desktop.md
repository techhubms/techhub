---
external_url: https://techcommunity.microsoft.com/t5/azure-virtual-desktop-blog/windows-10-extended-security-updates-for-azure-virtual-desktop/ba-p/4459715
title: Windows 10 Extended Security Updates for Azure Virtual Desktop
author: ivaylo_ivanov
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-10-14 22:00:00 +00:00
tags:
- Autopatch
- Azure Marketplace
- Azure Virtual Desktop
- Cybersecurity
- ESU
- Extended Security Updates
- Microsoft 365 Apps
- Operating System Support
- Session Hosts
- Windows 10
- Windows 11 Migration
- Windows Lifecycle
- Windows Update
section_names:
- azure
- security
---
ivaylo_ivanov explains how Azure Virtual Desktop customers can handle Windows 10's end of support and leverage Extended Security Updates (ESU) for ongoing security and compliance in session host environments.<!--excerpt_end-->

# Windows 10 Extended Security Updates for Azure Virtual Desktop

As Windows 10 reaches end of support on October 14, 2025, organizations using Azure Virtual Desktop (AVD) must plan for ongoing security and operational continuity. This article summarizes ESU options and lifecycle management for Windows 10 session hosts in AVD, including image availability and Microsoft 365 Apps support.

## Existing Session Hosts Running Windows 10

- All Azure Virtual Desktop session hosts running Windows 10 22H2 are automatically entitled to Windows 10 Extended Security Updates (ESU) at no additional cost.
- No manual admin action is required: ESUs are applied when Windows Update or Autopatch scans run, following each session host's configuration.
- The ESU program provides critical and important security updates beyond end of support.
- More details: [Microsoft ESU documentation](https://learn.microsoft.com/en-us/windows/whats-new/extended-security-updates)

## Creating New Session Hosts with Windows 10

- Azure Marketplace offers two Windows 10 22H2 client images for AVD: one with Microsoft 365 Apps and one without.
  - The image with Microsoft 365 Apps retires April 14, 2026.
  - The image without Microsoft 365 Apps remains available until 2028.
- The image without Microsoft 365 Apps will receive its final service update in October 2025; after this, newly created session hosts require ESU installation to remain secure.
- Microsoft recommends organizations plan for a transition to Windows 11 for improved security and long-term support.

## Microsoft 365 Apps Support for Windows 10

- Support for Microsoft 365 Apps on Windows 10 aligns with Windows 10’s lifecycle.
- Reference: [Microsoft 365 Apps and Windows 10 end of support](https://learn.microsoft.com/en-us/microsoft-365-apps/end-of-support/windows-10-support)

## Windows 10 ESU Support Process

- For issues involving Windows 365 or Windows 10 on AVD, Microsoft support will differentiate where the diagnostic responsibility lies.
  - If an issue pertains to Windows 365, resolution is provided as expected.
  - If the issue is in the Windows 10 OS, Microsoft may request reproduction on Windows 11. Only if the issue appears on Windows 11 will full support proceeds. Otherwise, upgrading to Windows 11 is recommended.

## Key Takeaways

- Leverage ESUs for continued protection of existing Windows 10 session hosts on Azure Virtual Desktop after end of support.
- Plan accordingly for session host provisioning and image retirement timelines, particularly if Microsoft 365 Apps are required.
- Transitioning to Windows 11 is highly recommended for sustained support, better security, and compliance.
- Stay informed via the [Azure Virtual Desktop Tech Community](https://techcommunity.microsoft.com/t5/azure-virtual-desktop/bd-p/AzureVirtualDesktopForum) and related Microsoft lifecycle resources.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-virtual-desktop-blog/windows-10-extended-security-updates-for-azure-virtual-desktop/ba-p/4459715)
