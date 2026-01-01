---
layout: "post"
title: "How to Identify Who Created an Azure Resource Using SystemData"
description: "This guide by Bruno Borges details the process of discovering who created any resource within Azure by leveraging metadata available through Azure Resource Manager. It explains how to access and interpret the 'systemData' object found in the resource's JSON view, covering key attributes like 'createdBy', 'createdByType', 'createdAt', 'lastModifiedBy', and their relevance for governance, troubleshooting, and compliance efforts. Readers will understand how to use Azure's built-in tracking features for effective cloud resource management."
author: "Bruno Borges"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/who-created-this-azure-resource-here-s-how-to-find-out/ba-p/4458470"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-10-01 20:52:54 +00:00
permalink: "/2025-10-01-How-to-Identify-Who-Created-an-Azure-Resource-Using-SystemData.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["Azure", "Azure Resource Manager", "Cloud Auditing", "Community", "Compliance", "Createdby", "DevOps", "Governance", "JSON View", "Metadata", "Resource Group", "Resource Lifecycle", "Resource Ownership", "Resource Provisioning", "Security", "Subscription Management", "Systemdata", "Troubleshooting", "User Identity"]
tags_normalized: ["azure", "azure resource manager", "cloud auditing", "community", "compliance", "createdby", "devops", "governance", "json view", "metadata", "resource group", "resource lifecycle", "resource ownership", "resource provisioning", "security", "subscription management", "systemdata", "troubleshooting", "user identity"]
---

Bruno Borges walks Azure administrators through finding out who created a resource by using the systemData metadata, helping improve governance, compliance, and troubleshooting.<!--excerpt_end-->

# How to Identify Who Created an Azure Resource Using SystemData

## Introduction

Managing large Azure subscriptions often means handling dozens or thousands of resources, and understanding resource ownership is critical for governance, compliance, and troubleshooting. Bruno Borges demonstrates a reliable method to identify who created a specific Azure resource using native Azure metadata.

## Step-by-Step Guide

1. **Open Resource Overview**  
   Navigate to your resource's Overview page. Here you'll see basic metadata including:
   - Resource group
   - Subscription
   - Location
   - Login server
   - Provisioning state

   The creator information does *not* appear here.

2. **Switch to JSON View**  
   At the top right of the Overview page, click the **JSON View** link. This opens the complete resource definition in JSON format.

3. **Locate the systemData Section**  
   Scroll through the JSON until you find the `systemData` object. Example:

   ```json
   "systemData": {
     "createdBy": "someuser@domain.com",
     "createdByType": "User",
     "createdAt": "2025–05–20T19:50:33.1511397Z",
     "lastModifiedBy": "someuser@domain.com",
     "lastModifiedByType": "User",
     "lastModifiedAt": "2025–05–20T19:50:33.1511397Z"
   }
   ```

## What systemData Reveals

- **createdBy**: The specific user or service principal responsible for creating the resource.
- **createdByType**: Whether the creator is a human user, managed identity, or Azure service.
- **createdAt**: The UTC timestamp of creation.
- **lastModifiedBy / lastModifiedByType / lastModifiedAt**: Helpful for understanding subsequent changes or updates to the resource.

## Practical Benefits

- **Governance**: Clarifies which team or individual owns a resource.
- **Troubleshooting**: Identifies who made changes affecting the resource.
- **Compliance & Auditing**: Documents accountability necessary for many organizations.

## Recommendations

Make including the **systemData** check part of your standard operational procedure for resource review. Doing so streamlines investigations and ensures accountability within your Azure environment.

## About the Author

Bruno Borges is a Microsoft expert who specializes in cloud solutions and governance best practices.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/who-created-this-azure-resource-here-s-how-to-find-out/ba-p/4458470)
