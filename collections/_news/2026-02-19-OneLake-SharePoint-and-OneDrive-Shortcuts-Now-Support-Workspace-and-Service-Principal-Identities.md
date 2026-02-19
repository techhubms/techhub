---
layout: "post"
title: "OneLake SharePoint and OneDrive Shortcuts Now Support Workspace and Service Principal Identities"
description: "This news post details the general availability of workspace and service principal identity support for SharePoint and OneDrive shortcuts in OneLake. This advancement enables organizations to securely use Microsoft 365 files directly in OneLake for analytics, BI, and AI, using centralized authentication methods (Workspace Identity and Service Principal). The update improves governance, API performance, and cross-tenant access while reducing reliance on user credentials—streamlining large-scale analytics and AI scenarios involving structured and unstructured data."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/onelake-sharepoint-and-onedrive-shortcuts-now-support-workspace-and-service-principal-identities-generally-available/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2026-02-19 12:00:00 +00:00
permalink: "/2026-02-19-OneLake-SharePoint-and-OneDrive-Shortcuts-Now-Support-Workspace-and-Service-Principal-Identities.html"
categories: ["AI", "Azure", "ML"]
tags: ["AI", "Analytics", "Authentication", "Azure", "BI", "Cross Tenant Access", "Data Engineering", "Governance", "Lakehouse", "Microsoft 365", "Microsoft Entra ID", "ML", "News", "OneDrive", "OneLake", "Service Principal", "SharePoint", "Shortcuts", "Structured Data", "Unstructured Data", "Workspace Identity"]
tags_normalized: ["ai", "analytics", "authentication", "azure", "bi", "cross tenant access", "data engineering", "governance", "lakehouse", "microsoft 365", "microsoft entra id", "ml", "news", "onedrive", "onelake", "service principal", "sharepoint", "shortcuts", "structured data", "unstructured data", "workspace identity"]
---

Microsoft Fabric Blog introduces support for workspace and service principal identities when creating OneLake SharePoint and OneDrive shortcuts, streamlining analytics, BI, and AI workloads with improved security and governance.<!--excerpt_end-->

# OneLake SharePoint and OneDrive Shortcuts Now Support Workspace and Service Principal Identities

The Microsoft Fabric Blog announces the general availability of workspace identity (WI) and service principal (SPN) authentication types for SharePoint and OneDrive shortcuts in OneLake. This enhancement allows organizations to securely use Microsoft 365 files directly in OneLake, eliminating the need to copy or move data. By leveraging these authentication methods, analytics, BI, and AI workloads gain a unified and governed view of both structured data and documents.

## Key Benefits

- **Centralized Authentication**: Moves away from individual user credentials, using Workspace Identity or Service Principal in Microsoft Entra ID. This ensures continuity when users exit the organization.
- **Improved Governance**: Utilizes Entra ID for authentication management, providing better oversight and control.
- **Higher API Limits**: WI and SPN authentication increase API call limits, minimizing throttling during heavy operations.
- **Cross-Tenant Access**: Service principals allow pipelines and analytic workloads to access data across different tenants securely.

## Real-world Scenarios

- Data engineering teams can convert pipelines to service principal-based shortcuts, reducing workflow disruptions caused by credential issues.
- Organizations can now share Excel spreadsheets and documents from SharePoint and OneDrive with external teams, unlocking analytics, BI, and AI possibilities—such as combining Lakehouse data with documents, AI indexing, and transforming files into analytics-ready datasets.

## Getting Started

1. **Create a Service Principal or Workspace Identity**: Register a service principal (SPN) in Microsoft Entra ID or enable Workspace Identity as required.
2. **Assign Permissions**: Grant SharePoint or OneDrive permissions as outlined in the [workspace identity authentication documentation](https://learn.microsoft.com/fabric/onelake/create-onedrive-sharepoint-shortcut#configure-workspace-identity-authentication).
3. **Create Shortcuts**: Add SharePoint or OneDrive shortcuts to your Lakehouse using SPN or WI following the [instructions](https://learn.microsoft.com/fabric/onelake/create-onedrive-sharepoint-shortcut#create-a-shortcut).

For further guidance, see the [official documentation](https://learn.microsoft.com/fabric/onelake/create-onedrive-sharepoint-shortcut) and provide feedback to help improve future capabilities.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/onelake-sharepoint-and-onedrive-shortcuts-now-support-workspace-and-service-principal-identities-generally-available/)
