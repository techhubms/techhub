---
layout: "post"
title: "How to Access a Shared OneDrive Folder in Azure Logic Apps"
description: "This guide addresses a common enterprise scenario where files from a shared OneDrive folder must be ingested and processed within Azure Logic Apps, then connected to services like SharePoint or Azure Blob Storage. It explains the technical challenge with OneDrive's user scope, and offers two detailed, practical solutions: using the Microsoft Graph API, or leveraging Graph Explorer to manually obtain the necessary folder information. Troubleshooting and fallback methods for edge cases are included."
author: "Arpit_MSFT"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-integration-services-blog/how-to-access-a-shared-onedrive-folder-in-azure-logic-apps/ba-p/4484962"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-02-26 04:31:43 +00:00
permalink: "/2026-02-26-How-to-Access-a-Shared-OneDrive-Folder-in-Azure-Logic-Apps.html"
categories: ["Azure"]
tags: ["Automation", "Azure", "Azure Blob Storage", "Azure Logic Apps", "Community", "Connectors", "Delegated Permissions", "DriveId", "Enterprise Automation", "FolderId", "Graph Explorer", "Integration", "Microsoft Graph", "OneDrive For Business", "SharePoint", "User Context Scoped", "Workflow"]
tags_normalized: ["automation", "azure", "azure blob storage", "azure logic apps", "community", "connectors", "delegated permissions", "driveid", "enterprise automation", "folderid", "graph explorer", "integration", "microsoft graph", "onedrive for business", "sharepoint", "user context scoped", "workflow"]
---

Arpit_MSFT demonstrates how to reliably access shared OneDrive folders inside Azure Logic Apps, covering technical constraints and practical solutions leveraging both Graph API and Graph Explorer.<!--excerpt_end-->

# How to Access a Shared OneDrive Folder in Azure Logic Apps

Author: **Arpit_MSFT**

## Overview

A common enterprise automation task is to copy files from a **OneDrive folder shared by a colleague** into another storage destination—like **SharePoint** or **Azure Blob Storage**—via **Azure Logic Apps**. However, configuring this connection isn't straightforward due to how OneDrive handles folder visibility for connected accounts.

## The Problem: Shared OneDrive Folders Don't Appear by Default

When setting up the [OneDrive for Business – “List files in folder”](https://learn.microsoft.com/en-us/connectors/onedriveforbusiness/#list-files-in-folder) action in a Logic App, you'll notice these constraints:

- The folder picker displays only your root directory and your own subfolders
- **Folders shared with you** are missing, even if you see them in the OneDrive UI

**Why?**

- The OneDrive connector works in the context of the signed-in user's drive
- Shared folders reside in a **separate drive** with a unique **driveId**

## Solutions: How to Access Shared Folders

**There are two main supported approaches:**

### 1. Use Microsoft Graph APIs (Delegated Permissions)

- **Invoke Graph APIs** using the [HTTP with Microsoft Entra ID (preauthorized)](https://learn.microsoft.com/en-us/connectors/webcontents/#invoke-an-http-request) connector
- Requires delegated permissions and (potentially) admin consent, plus Azure Entra configuration
- Allows direct file/folder access with more flexibility, but introduces additional setup complexity

  - [Bulk authorization reference](https://learn.microsoft.com/en-us/connectors/webcontents/#authorize-the-connector-to-act-on-behalf-of-a-signed-in-user)

### 2. Use Graph Explorer to Find Metadata and Configure by Hand

- Use [Microsoft Graph Explorer](https://developer.microsoft.com/en-us/graph/graph-explorer) to enumerate folders shared with you
- Extract **driveId** and **id** for the target folder
- In the Logic App, add the "List files in folder" action and, instead of the picker, manually enter the folder value using the format:
  
      {driveId}/{folderId}

- Complete the workflow to process/copy files as needed

## Step-by-Step: Accessing a Shared Folder

1. **Obtain Metadata via Graph Explorer**
   - Issue:

     ```http
     GET https://graph.microsoft.com/v1.0/{shared-folder-owner-username}/drive/root/children
     ```

   - Review results for the shared folder's `parentReference.driveId` and `id`

2. **Add Action in Logic App**
   - Use OneDrive for Business → List files in folder
   - Bypass folder picker; instead, input `{driveId}/{folderId}` manually

3. **Continue Workflow**
   - Iterate over files as normal
   - Copy to SharePoint, Azure Blob Storage, or apply further automation

## Troubleshooting: Edge Cases

If the required IDs can’t be found with Graph Explorer:

- Open the folder in OneDrive Web
- Use **browser developer tools** to inspect network traffic
- Look for `CurrentFolderUniqueId` (folder id) and the `driveId`
- This approach is robust for uncommon or filtered folder structures

---

**Takeaway:**

Azure Logic Apps can interact with shared OneDrive folders once you understand and supply the needed drive and folder identifiers. Both the API method and the manual Graph Explorer method are effective for working around the "missing shared folder" scenario in enterprise integration workflows.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/how-to-access-a-shared-onedrive-folder-in-azure-logic-apps/ba-p/4484962)
