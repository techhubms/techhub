---
layout: "post"
title: "Integrating OneDrive and SharePoint Documents into OneLake Analytics with Shortcuts"
description: "This article from the Microsoft Fabric Blog explains how organizations can seamlessly integrate everyday files like Word documents, Excel spreadsheets, PowerPoint presentations, and PDFs stored in OneDrive and SharePoint into Microsoft OneLake for unified analytics. By using shortcuts, businesses can avoid data duplication and silos and apply governance, AI-powered transforms, and analytics workflows directly on business-critical documents without moving or copying files. The workflow enables advanced analytics, BI, and AI scenarios within Fabric, leveraging both structured and unstructured data."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/turning-everyday-documents-from-sharepoint-and-onedrive-into-analytics-ready-data-with-onelake-shortcuts/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-12-03 12:00:00 +00:00
permalink: "/2025-12-03-Integrating-OneDrive-and-SharePoint-Documents-into-OneLake-Analytics-with-Shortcuts.html"
categories: ["Azure", "ML"]
tags: ["AI", "AI Transforms", "Analytics", "Azure", "BI Integration", "Data Governance", "Data Virtualization", "Document Indexing", "Excel", "Fabric", "Lakehouse", "Microsoft OneLake", "ML", "News", "OneDrive", "Power BI", "SharePoint", "Shortcut Workflow", "Structured Data", "Unstructured Data"]
tags_normalized: ["ai", "ai transforms", "analytics", "azure", "bi integration", "data governance", "data virtualization", "document indexing", "excel", "fabric", "lakehouse", "microsoft onelake", "ml", "news", "onedrive", "power bi", "sharepoint", "shortcut workflow", "structured data", "unstructured data"]
---

Microsoft Fabric Blog details how organizations can use OneDrive and SharePoint shortcuts in OneLake to integrate everyday files into analytics workflows, with AI-powered transforms and unified governance.<!--excerpt_end-->

# Integrating OneDrive and SharePoint Documents into OneLake Analytics with Shortcuts

Microsoft Fabric Blog introduces a streamlined approach for businesses to integrate commonly used files—such as Word documents, Excel spreadsheets, PowerPoint presentations, and PDFs—from OneDrive and SharePoint directly into Microsoft OneLake for analytics.

## Seamless Integration Without Data Duplication

By creating shortcuts, users can reference files stored in OneDrive and SharePoint within OneLake storage, eliminating the need to copy or export data. This supports governance and management through Fabric's unified security and management layer.

### Key Benefits

- **Files remain in OneDrive and SharePoint** while being accessible for analytics.
- **Unified view** of structured and unstructured data in OneLake.
- **Simplified governance** under Fabric's management layer.

## Unlocking New Analytics and AI Scenarios

- **BI teams** can join Excel-based forecasts with transactional lakehouse data.
- **Documents** like contract PDFs and project specs are indexed for use in AI-powered knowledge bases with Microsoft Foundry.
- **Shortcuts enable rapid discovery** and integration of business context documents alongside operational analytics tables.

## Transforming Files into Analytics-Ready Data

The shortcut workflow includes an optional transformation step:

- Project shortcut data into structured tables for analysis.
- Keep transformed data in sync as new files arrive.
- Combine transformed and shortcut data with Fabric workloads, including warehouses, notebooks, and Power BI.
- AI-powered transforms allow rich analysis without manual data movement.

## Getting Started with OneLake Shortcuts

1. Open a lakehouse in your Fabric workspace.
2. Right-click a folder and select "New shortcut."
3. Connect to OneDrive or SharePoint and select relevant folders (sales decks, finance trackers, project docs).
4. Use shortcuts to power reports, notebooks, AI agents, and transforms.
5. Files remain safely in their original locations while supporting advanced analytics and AI functionality.

Refer to the official [Create a OneDrive or SharePoint shortcut](https://review.learn.microsoft.com/fabric/onelake/create-onedrive-sharepoint-shortcut?branch=pr-en-us-11352) documentation for step-by-step details.

---

By leveraging OneDrive and SharePoint shortcuts in OneLake, organizations can virtualize key business documents and unlock powerful analytics and AI capabilities—all governed under a single platform.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/turning-everyday-documents-from-sharepoint-and-onedrive-into-analytics-ready-data-with-onelake-shortcuts/)
