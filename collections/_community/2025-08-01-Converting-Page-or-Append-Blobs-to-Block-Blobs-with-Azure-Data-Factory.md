---
layout: "post"
title: "Converting Page or Append Blobs to Block Blobs with Azure Data Factory"
description: "SaikumarMandepudi details how to convert Azure page or append blobs into block blobs using Azure Data Factory (ADF) to enable cost optimization strategies, such as moving the data to Archive tier. The guide walks through creating datasets, linked services, pipelines, and options for changing blob access tiers after conversion."
author: "SaikumarMandepudi"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-paas-blog/converting-page-or-append-blobs-to-block-blobs-with-adf/ba-p/4433723"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-08-01 10:57:37 +00:00
permalink: "/community/2025-08-01-Converting-Page-or-Append-Blobs-to-Block-Blobs-with-Azure-Data-Factory.html"
categories: ["Azure", "ML"]
tags: ["Access Tiers", "Append Blobs", "Archive Tier", "Azure", "Azure Blob Storage", "Azure Data Factory", "Block Blobs", "Community", "Data Pipelines", "Lifecycle Management", "ML", "Page Blobs", "Storage Cost Optimization"]
tags_normalized: ["access tiers", "append blobs", "archive tier", "azure", "azure blob storage", "azure data factory", "block blobs", "community", "data pipelines", "lifecycle management", "ml", "page blobs", "storage cost optimization"]
---

In this article, SaikumarMandepudi explains how to use Azure Data Factory to convert page or append blobs into block blobs, enabling access tier changes and storage cost optimization.<!--excerpt_end-->

## Introduction

Converting page or append blobs to block blobs can be necessary when optimizing storage costs in Azure. Certain blob types, like page or append blobs, cannot be directly moved to the archive access tier—only block blobs support access tier functionality. This article outlines how to convert page or append blobs into block blobs using Azure Data Factory (ADF), after which any standard method can be used to transition them to the archive tier.

## Problem Context

- Some storage accounts have many infrequently accessed page blobs in the hot tier—often kept solely for backup purposes.
- Only block blobs can have their access tier changed in Azure Blob Storage ([see documentation](https://learn.microsoft.com/en-us/azure/storage/blobs/access-tiers-overview)).

## Azure Data Factory Solution

- The Azure Blob Storage connector in ADF supports copying data **from** block, append, or page blobs, and copying **to** block blobs ([see ADF connector docs](https://learn.microsoft.com/en-us/azure/data-factory/connector-azure-blob-storage?tabs=data-factory#supported-capabilities)).
- No special configuration is required—the ADF copy activity will create destination blobs as block blobs by default.

## Step-by-Step Guide

### Step 1: Create Azure Data Factory (ADF) Instance

1. In the Azure Portal, create a new Azure Data Factory resource using the [quickstart guide](https://learn.microsoft.com/en-us/azure/data-factory/quickstart-create-data-factory).
2. After creation, launch the ADF Studio UI.

### Step 2: Create Datasets

1. Navigate to **Author > Datasets > New dataset**.
2. Select **Azure Blob Storage**, then set the format to "binary."
3. Create one dataset for the source (the storage account containing page or append blobs) and another for the destination.

### Step 3: Create Linked Services

1. Create a new linked service in ADF, referencing the storage account with the source blobs.
2. Set the file path for the page (or append) blobs to convert.
3. Create a separate dataset and corresponding linked service for the destination storage account (can be the same or different account, as required).

### Step 4: Configure the Copy Data Pipeline

1. Create a new pipeline in ADF.
2. From **Move and Transform**, drag and drop the **Copy data** activity.
3. Assign the previously created source and destination datasets.
4. Select the "Recursively" option if you want to include subfolders and publish your changes.
5. Adjust filters and copy behavior as required to suit your scenario.

### Step 5: Debug and Validate

1. Run the pipeline in debug mode.
2. If successful, the output will display a "succeeded" status.
3. Verify in the destination storage account that the blob type is now "block blob," and the access tier defaults to Hot.

## Next Steps: Changing Access Tier

Once blobs are converted to block blobs, you can change their access tier to Archive using methods such as:

- Azure Blob Lifecycle Management (LCM) policies
- Azure Storage actions
- Azure CLI or PowerShell scripts

See [lifecycle management docs](https://learn.microsoft.com/en-us/azure/storage/blobs/lifecycle-management-overview) or [bulk archive docs](https://learn.microsoft.com/en-us/azure/storage/blobs/archive-blob?tabs=azure-powershell#bulk-archive) for references.

## Conclusion

Using Azure Data Factory provides a streamlined approach to convert page or append blobs into block blobs, after which standard tools and policies can be used to transition the access tier and optimize storage costs. This approach is more efficient than developing custom scripts or utilities.

## References

- [Access tiers for blob data](https://learn.microsoft.com/en-us/azure/storage/blobs/access-tiers-overview)
- [ADF Azure Blob Storage connector](https://learn.microsoft.com/en-us/azure/data-factory/connector-azure-blob-storage?tabs=data-factory#supported-capabilities)
- [ADF Copy Activity Overview](https://learn.microsoft.com/en-us/azure/data-factory/copy-activity-overview)
- [Azure Storage task quickstart](https://learn.microsoft.com/en-us/azure/storage-actions/storage-tasks/storage-task-quickstart-portal)
- [Lifecycle management overview](https://learn.microsoft.com/en-us/azure/storage/blobs/lifecycle-management-overview)
- [Bulk change to archive tier](https://learn.microsoft.com/en-us/azure/storage/blobs/archive-blob?tabs=azure-powershell#bulk-archive)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-paas-blog/converting-page-or-append-blobs-to-block-blobs-with-adf/ba-p/4433723)
