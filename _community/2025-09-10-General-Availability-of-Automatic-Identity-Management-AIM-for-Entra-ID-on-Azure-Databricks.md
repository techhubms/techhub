---
layout: "post"
title: "General Availability of Automatic Identity Management (AIM) for Entra ID on Azure Databricks"
description: "This announcement details the general availability of Automatic Identity Management (AIM) for integrating Microsoft Entra ID with Azure Databricks. It highlights the move from manual and script-based identity management to seamless, automated provisioning for users, groups, and service principals, with additional dashboard sharing, automation via APIs, and streamlined admin workflows."
author: "AnaviNahar"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/general-availability-automatic-identity-management-aim-for-entra/ba-p/4452206"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-09-10 16:00:00 +00:00
permalink: "/2025-09-10-General-Availability-of-Automatic-Identity-Management-AIM-for-Entra-ID-on-Azure-Databricks.html"
categories: ["Azure", "Security"]
tags: ["Access Control", "Admin Console", "AIM", "API Automation", "Automation", "Azure", "Azure Databricks", "Community", "Dashboards", "Entra ID", "Identity Management", "Microsoft Entra", "Nested Groups", "Security", "User Provisioning"]
tags_normalized: ["access control", "admin console", "aim", "api automation", "automation", "azure", "azure databricks", "community", "dashboards", "entra id", "identity management", "microsoft entra", "nested groups", "security", "user provisioning"]
---

AnaviNahar announces the general availability of Automatic Identity Management (AIM) for seamless integration between Entra ID and Azure Databricks, eliminating manual user management and enabling enterprise-scale automation.<!--excerpt_end-->

# General Availability of Automatic Identity Management (AIM) for Entra ID on Azure Databricks

**Author:** AnaviNahar  
**Published:** September 10, 2025

Automatic Identity Management (AIM) for Microsoft Entra ID on Azure Databricks is now generally available, streamlining identity and access management for organizations using Azure Databricks.

## Key Improvements

- **No Manual Setup Needed:** Users, groups, and service principals from Microsoft Entra ID are automatically available within Azure Databricks workspaces. This eliminates manual administrative steps such as user import/export, group assignments, or custom SCIM configuration.
- **Seamless Dashboard Sharing:** Share AI/BI dashboards effortlessly with any user, group, or service principal in Microsoft Entra ID. Members who do not have direct workspace access can view dashboards with embedded credentials, simplifying collaboration, even beyond organizational boundaries.
- **Default Enablement:** AIM is enabled by default for all new Azure Databricks accounts. Existing accounts can enable it with a single click in the Account Admin Console, and soon, all will have it enabled by default.
- **Automation at Scale:** Register users, groups, and service principals via APIs, supporting automation and script-based processes for large enterprises.

## Benefits

- **Reduced Admin Overhead:** Eliminate ongoing maintenance scripts and manual group management.
- **Support for Nested Groups:** Integrated handling of complex organizational structures, including nested group memberships.
- **Enterprise-Grade Access Control:** Leverage Microsoft's trusted Entra ID identity platform for robust security and compliance.
- **Instant Access:** Users added to Entra ID are immediately available in Azure Databricks, improving onboarding and collaboration speed.

## Getting Started

- [Databricks blog post: AIM for Entra ID](https://www.databricks.com/blog/automatic-identity-management-entra-id-now-generally-available-azure-databricks)
- [Documentation: Automatic Identity Management in Azure Databricks](https://learn.microsoft.com/en-us/azure/databricks/admin/users-groups/automatic-identity-management)
- [Share a dashboard in Azure Databricks](https://learn.microsoft.com/en-us/azure/databricks/dashboards/share)

## Summary

With the general availability of AIM, organizations can centrally manage identities, automate provisioning, and improve collaboration securely in Azure Databricks environments—all without manual intervention or custom setup.

---
*Written by AnaviNahar – Microsoft, September 2025.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/general-availability-automatic-identity-management-aim-for-entra/ba-p/4452206)
