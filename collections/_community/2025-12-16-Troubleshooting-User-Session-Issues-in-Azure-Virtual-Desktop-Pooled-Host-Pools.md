---
layout: post
title: Troubleshooting User Session Issues in Azure Virtual Desktop (Pooled Host Pools)
author: StormShadow007
canonical_url: https://techcommunity.microsoft.com/t5/azure-virtual-desktop/azure-virtual-desktop-pooled-sessions-ending-unexpectedly-and/m-p/4478548#M13967
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-12-16 22:47:32 +00:00
permalink: /azure/community/Troubleshooting-User-Session-Issues-in-Azure-Virtual-Desktop-Pooled-Host-Pools
tags:
- AVD
- Azure
- Azure Portal
- Azure Virtual Desktop
- Community
- Disconnection Issue
- FSLogix
- Pooled Host Pool
- PowerShell
- Profile Lock
- Remote Desktop
- Session Host
- Session Management
- Troubleshooting
- User Sessions
- Windows 10 Enterprise Multi Session
- Windows 11 Enterprise Multi Session
section_names:
- azure
---
StormShadow007 describes challenges with user session disconnects in Azure Virtual Desktop pooled environments, seeking advice on session tracking and cleanup—especially with FSLogix-enabled hosts.<!--excerpt_end-->

# Troubleshooting User Session Issues in Azure Virtual Desktop (Pooled Host Pools)

**Author:** StormShadow007

## Scenario

An organization is encountering intermittent user disconnections and failed session reconnections in an Azure Virtual Desktop (AVD) environment. The setup uses a pooled host pool with Windows 10/11 Enterprise multi-session OS and FSLogix profile management, accessed via the Remote Desktop client (Windows App).

### The Issue

- Users are unexpectedly disconnected during sign-in or cannot reconnect after disconnects.
- Typical user error: _"Your Remote Desktop Services session has ended. The administrator has ended the session, an error occurred while the connection was being established, or a network problem occurred."_
- Affected session hosts appear healthy, and administrators did not initiate session terminations.
- Rebooting a host may temporarily help, but the problem recurs.

### Observations & Actions Taken

- Restarted AVD agent services on session hosts.
- Placed problem hosts into drain mode (preventing new logins).
- Fully rebooted virtual machines.

### Suspected Cause

- Users may retain active or disconnected sessions on previous hosts.
- FSLogix profile locks could be blocking new sessions from starting properly.

## Questions Posed

1. What is the best way to identify which users are connected to which hosts within a pooled host pool?
2. Are there recommended practices (using Azure Portal or PowerShell) to detect and clear stuck/disconnected sessions?
3. Has anyone experienced similar problems with pooled AVD, Windows 10/11, and FSLogix?

## Community Solicitation

The author requests practical advice, PowerShell scripts, or Azure Portal best practices to:

- Query and track user sessions across hosts
- Remove or clear stale or locked sessions
- Resolve FSLogix profile lock issues

---

### Additional Context

- _Environment_: Azure Virtual Desktop (AVD) Pooled Host Pool
- _Operating Systems_: Windows 10/11 Enterprise multi-session
- _Profile Management_: FSLogix
- _Client_: Windows App (Remote Desktop)

This is a recurring troubleshooting scenario in Azure-based remote desktop deployments involving multi-session hosts and user profile containers.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-virtual-desktop/azure-virtual-desktop-pooled-sessions-ending-unexpectedly-and/m-p/4478548#M13967)
