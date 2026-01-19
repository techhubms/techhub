---
layout: post
title: Ephemeral OS Disk Support Now in Public Preview on Azure Virtual Desktop
author: Ron_Coleman
canonical_url: https://techcommunity.microsoft.com/t5/azure-virtual-desktop-blog/now-in-public-preview-ephemeral-os-disk-support-on-azure-virtual/ba-p/4460172
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-10-15 19:00:00 +00:00
permalink: /azure/community/Ephemeral-OS-Disk-Support-Now-in-Public-Preview-on-Azure-Virtual-Desktop
tags:
- ARM Templates
- Azure Infrastructure
- Azure Portal
- Azure Virtual Desktop
- CLI
- Cloud Deployment
- Dynamic Autoscaling
- Ephemeral OS Disk
- Performance Optimization
- Pooled Host Pools
- PowerShell
- Session Hosts
- Stateless Workloads
- Virtual Machines
section_names:
- azure
---
Ron Coleman announces the public preview of Ephemeral OS disk support for Azure Virtual Desktop, outlining setup guidance, performance benefits, and deployment strategies for stateless workloads.<!--excerpt_end-->

# Ephemeral OS Disk Support Now in Public Preview on Azure Virtual Desktop

Azure Virtual Desktop is rolling out support for Ephemeral OS disks, now available in public preview. This update streamlines the deployment and management of session hosts, particularly for customers needing fast provisioning and stateless environments.

## Key Benefits

- **Faster provisioning and reimaging:** Session hosts can be created or reset in seconds, reducing downtime and improving operational efficiency.
- **Improved performance:** Local disk operations eliminate network latency, accelerating read/write speeds.
- **Stateless optimization:** Ephemeral OS disks are ideal for scenarios where session hosts do not need to retain state between uses.
- **Wide availability:** Supported across all Azure regions and on any VM size with suitable local storage capacity.

## What is an Ephemeral OS Disk?

An Ephemeral OS disk stores the VM's operating system locally rather than using remote Azure Storage. This approach is well-suited to scenarios valuing speed, scalability, and simple reimaging over persistent data retention because the disks are non-persistent.

## Getting Started

To start using ephemeral OS disks:

1. **Ensure prerequisites:** Use a pooled host pool with session host configuration enabled. Ephemeral OS disks are not available for personal host pools or those without this configuration.
2. **Provision in Azure Portal (or via PowerShell, CLI, ARM, SDKs, APIs):**
   - Create a new pooled host pool.
   - On the session host tab, choose an appropriate image and a VM size with enough local disk space.
   - Check the Ephemeral OS disk option.
   - Choose placement: OS cache or Temp disk.
   - Complete the pool configuration to finalize provisioning.

For more, see [Microsoft’s documentation](https://learn.microsoft.com/en-us/azure/virtual-machines/ephemeral-os-disks) and the [host pool configuration guide](https://learn.microsoft.com/en-us/azure/virtual-desktop/add-session-hosts-host-pool?tabs=portal%2Cgui&pivots=host-pool-session-host-configuration).

## Dynamic Autoscaling Support

Ephemeral OS disks are designed for stateless session hosts and do not support traditional start or deallocate operations. Dynamic Autoscaling plans should be used for managing host pool scaling (creation and deletion), ensuring efficient resource utilization aligned with stateless design.

Learn more about [Dynamic Autoscaling plans](https://learn.microsoft.com/en-us/azure/virtual-desktop/autoscale-create-assign-scaling-plan?tabs=portal%2Cintune&pivots=dynamic).

## Next Steps

Explore the public preview and start deploying Ephemeral OS disks for faster, more efficient Azure Virtual Desktop experiences. Read Microsoft’s setup documentation for full prerequisites, examples, and more tips.

For ongoing updates, follow the [Azure Virtual Desktop Tech Community](https://techcommunity.microsoft.com/t5/azure-virtual-desktop/bd-p/AzureVirtualDesktopForum).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-virtual-desktop-blog/now-in-public-preview-ephemeral-os-disk-support-on-azure-virtual/ba-p/4460172)
