---
layout: post
title: 'Microsoft Fabric Update: Workspace Admins Gain Direct Workload Assignment'
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/new-in-microsoft-fabric-empowering-workspace-admins-with-direct-workload-assignment/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-09-25 13:00:00 +00:00
permalink: /ml/news/Microsoft-Fabric-Update-Workspace-Admins-Gain-Direct-Workload-Assignment
tags:
- Analytics
- Azure
- Data Governance
- Data Ingestion
- Data Management
- Data Quality
- Entra ID
- Microsoft Fabric
- ML
- News
- No Code Data Integration
- Osmos
- Power BI
- Profisee
- Tenant Settings
- Workloads Hub
- Workspace Admin
- Workspace Level Assignment
section_names:
- azure
- ml
---
Microsoft Fabric Blog announces a major update empowering workspace admins to directly assign additional workloads like Osmos and Profisee, improving agility and governance in data and analytics operations.<!--excerpt_end-->

# Microsoft Fabric: Direct Workload Assignment for Workspace Admins

## Overview

Microsoft Fabric users now benefit from more flexible and efficient management of data workloads. The new workspace-level assignment feature enables workspace admins to add analytical and data management workloads directly to their own workspaces, bypassing previous capacity-level or tenant setup bottlenecks.

## Key Benefits

- **Faster Onboarding:** Teams can use workloads immediately, minimizing delays and dependency on broader tenant setup.
- **Autonomy for Admins:** Workspace admins independently manage which workloads are active in their workspace.
- **Improved Governance:** Workload assignments are customizable to match policy requirements, keeping enterprise governance intact.

## How It Works

Admins can now access the [Workloads Hub](https://app.powerbi.com/workloadhub/) and assign additional workloads directly to the workspace, where their data and teams operate. This flexibility allows specific teams to innovate and build custom solutions without impacting the configuration of the entire tenant.

![Assigning workloads in Fabric](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/09/Recording-2025-09-11-aadasdasdas.gif)

## Available Additional Workloads

- **Osmos:** Ingests messy and fragmented data from diverse sources (OneLake, spreadsheets, external systems). It leverages AI Agents to unify schemas using Osmos Data Wrangler—offering a no-code and pipeline-free integration experience.
- **Profisee:** Cleans, enriches, and deduplicates data. It builds Golden Records, applies data quality rules, and guides business users to resolve missing values for trustworthy analytics and AI readiness.

These tools enable teams to process and govern their data more effectively, making Fabric suitable for advanced reporting, analytics, and machine learning.

![Workload assignment UI](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/09/screencapture-msit-powerbi-workloadhub-my-2025-09-11-22_10_43.png)

## Governance and Security

- New [Tenant Settings](https://app.powerbi.com/admin-portal/tenantSettings) under Additional Workloads allow admins to enable or disable workspace-level workload assignments. This setting is on by default but can be switched off as needed.
- Entra ID consent policies still apply. Users and admins must comply with consent requirements before accessing or using workloads, ensuring ongoing compliance and security.

## Learn More

- [Add a workload in the workload hub](http://aka.ms/addworkload)
- [Authentication overview with Entra ID](https://aka.ms/EntraIDConsent)

These changes make Microsoft Fabric more agile, secure, and responsive—allowing controlled yet flexible adoption of new workloads tailored for analytics, governance, and AI scenarios.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/new-in-microsoft-fabric-empowering-workspace-admins-with-direct-workload-assignment/)
