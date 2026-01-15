---
layout: post
title: Calculating Expiry and Retention Days for Azure Blob Storage Using Inventory and Synapse
author: Harshi_mrinal
canonical_url: https://techcommunity.microsoft.com/t5/azure-paas-blog/deriving-expiry-days-and-remaining-retention-days-for-blobs/ba-p/4466586
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-11 05:35:25 +00:00
permalink: /azure/community/Calculating-Expiry-and-Retention-Days-for-Azure-Blob-Storage-Using-Inventory-and-Synapse
tags:
- Azure
- Azure Blob Storage
- Azure Data Lake Gen2
- Azure Storage Actions
- Azure Synapse Analytics
- Blob Expiry
- Blob Inventory
- Community
- Data Compliance
- Data Management
- Data Retention
- REST API
- Soft Delete
- SQL Query
- Storage Lifecycle
section_names:
- azure
---
Harshi_mrinal provides a detailed walkthrough for monitoring and calculating blob expiry and retention periods in Azure Blob Storage, using inventory reports and Azure Synapse to ensure compliance with storage policies.<!--excerpt_end-->

# Calculating Expiry and Retention Days for Azure Blob Storage Using Inventory and Synapse

Managing large volumes of data in Azure Blob Storage and Azure Data Lake Gen2 involves ensuring data compliance and optimizing cloud storage costs. One critical aspect is identifying blobs that have been soft deleted or are set to expire in the future and calculating their remaining retention periods.

This guide outlines a step-by-step solution:

## 1. Understanding Blob Expiry and Soft Delete

- **Blob soft delete** allows you to recover blobs after deletion within a specified retention period. After this period, data is permanently deleted.
- **Blob expiry** can be configured through the Set Blob Expiry operation for hierarchical namespace (HNS) enabled storage accounts, scheduling automatic deletions.

## 2. Setting Blob Expiry

- Use either [Azure Storage Actions](https://learn.microsoft.com/en-us/azure/storage-actions/overview#supported-regions) or the [Set Blob Expiry REST API](https://learn.microsoft.com/en-us/rest/api/storageservices/set-blob-expiry?tabs=microsoft-entra-id) to set blob expiry times. These allow automated lifecycle management.

## 3. Generating and Accessing the Blob Inventory Report

- Configure a **Blob Inventory rule** in your storage account.
- Inventory jobs output a CSV file listing metadata for each blob, such as name, deletion status, expiry time, etc.
- To download the report:
    - Go to the dedicated inventory container.
    - Navigate to the latest date folder.
    - Obtain the CSV file URL.

## 4. Querying Blob Inventory with Azure Synapse Analytics

- [Create an Azure Synapse workspace](https://learn.microsoft.com/en-us/azure/synapse-analytics/get-started-create-workspace) if not done already.
- Open [https://web.azuresynapse.net](https://web.azuresynapse.net/), go to the **Develop** tab, use the plus icon, and select **SQL script**.
- Example SQL to list blobs, their containers and expiry information:

```sql
SELECT
  LEFT([Name], CHARINDEX('/', [Name]) - 1) AS Container,
  RIGHT([Name], LEN([Name]) - CHARINDEX('/', [Name])) AS Blob,
  [Expiry-time]
FROM OPENROWSET(
  BULK '<URL to your inventory CSV file>',
  FORMAT = 'csv', PARSER_VERSION = '2.0', HEADER_ROW = TRUE
) AS Source;
```

- To also retrieve retention days for soft-deleted blobs:

```sql
SELECT
  LEFT([Name], CHARINDEX('/', [Name]) - 1) AS Container,
  RIGHT([Name], LEN([Name]) - CHARINDEX('/', [Name])) AS Blob,
  [Expiry-time],
  RemainingRetentionDays
FROM OPENROWSET(
  BULK '<URL to your inventory CSV file>',
  FORMAT = 'csv', PARSER_VERSION = '2.0', HEADER_ROW = TRUE
) AS Source;
```

- Null expiry values mean either the blob is not deleted or no expiry time is set.

## 5. Alternative Approaches

- For organizations requiring automation, similar results can be achieved with PowerShell or Azure CLI scripts.

## 6. References

- [Set Blob Expiry (REST API) - Azure Storage](https://learn.microsoft.com/en-us/rest/api/storageservices/set-blob-expiry?tabs=microsoft-entra-id)
- [Create a storage task - Azure Storage Actions](https://learn.microsoft.com/en-us/azure/storage-actions/storage-tasks/storage-task-create?tabs=azure-portal)
- [Azure Storage blob inventory](https://learn.microsoft.com/en-us/azure/storage/blobs/blob-inventory)
- [Calculate blob count and size using Azure Storage inventory](https://learn.microsoft.com/en-us/azure/storage/blobs/calculate-blob-count-size)

## Summary

By leveraging Azure Blob Inventory and Synapse Analytics, you can efficiently manage your data's lifecycle within Azure, ensuring policies and compliance requirements are met without manual overhead.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-paas-blog/deriving-expiry-days-and-remaining-retention-days-for-blobs/ba-p/4466586)
