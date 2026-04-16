---
primary_section: azure
author: Michelle_Moya
external_url: https://techcommunity.microsoft.com/t5/azure-virtual-desktop-blog/app-attach-in-azure-virtual-desktop-now-supports-windows-server/ba-p/4511729
feed_name: Microsoft Tech Community
title: App attach in Azure Virtual Desktop now supports Windows Server 2025 and Windows Server 2022
tags:
- App Attach
- App V
- Application Onboarding
- Application Virtualization
- AppX
- Azure
- Azure Virtual Desktop
- Community
- Dynamic Application Delivery
- Golden Image
- Hybrid
- Image Sprawl
- MSIX
- Multi Session
- Pooled Host Pools
- Session Hosts
- Windows Server
date: 2026-04-16 16:00:12 +00:00
section_names:
- azure
---

Michelle_Moya announces expanded App attach support in Azure Virtual Desktop, adding Windows Server 2025 and Windows Server 2022 session hosts to enable dynamic delivery of packaged apps without baking them into base images.<!--excerpt_end-->

## Overview

Managing applications in virtualized desktop and server environments often means baking apps into base images, which can lead to image sprawl, slower updates, and higher operational overhead. **App attach in Azure Virtual Desktop (AVD)** changes this approach by delivering applications dynamically to user sessions **without installing them on the session host**.

This update is especially relevant for organizations impacted by the **end of support for Microsoft Application Virtualization (App-V) Server components in April 2026**.

## What’s new

**App attach now supports Windows Server 2025 and Windows Server 2022 session hosts in Azure Virtual Desktop.**

## Benefits of App attach support for Windows Server

With App attach supported on the latest Windows Server versions, organizations can dynamically deliver **MSIX, AppX, and App‑V packaged applications** to **server-based multi-session environments** using existing App attach workflows. Key outcomes include:

- **Deploy applications to Windows Server 2025 and Windows Server 2022 session hosts**
  - Applications no longer need to be baked into the base image.

- **Maintain a single golden image**
  - Reduce image sprawl by managing apps independently from the OS image.

- **Bring existing App‑V packages to Azure Virtual Desktop**
  - Continue deploying previously packaged apps using App attach workflows **without repackaging** and **without App‑V Server infrastructure** (which reaches end of support in April 2026).

- **Accelerate application onboarding for pooled host pools**
  - Assign and update apps independently of infrastructure lifecycle events such as scaling, reimaging, or host replacement.

- **Support Azure Virtual Desktop Hybrid scenarios**
  - Hybrid customers can use App attach to deliver apps dynamically to session hosts without a complicated application migration.

## Get started

- App attach documentation: https://learn.microsoft.com/en-us/azure/virtual-desktop/app-attach-overview?pivots=app-attach
- Azure Virtual Desktop Tech Community board: https://techcommunity.microsoft.com/t5/azure-virtual-desktop/bd-p/AzureVirtualDesktopForum


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-virtual-desktop-blog/app-attach-in-azure-virtual-desktop-now-supports-windows-server/ba-p/4511729)

