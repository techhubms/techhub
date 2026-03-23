---
title: How to get total blob count and total capacity with Azure Blob Inventory
tags:
- Access Tier
- Apache Parquet
- Azure
- Azure Blob Storage
- Azure Portal
- Azure Storage
- Blob Inventory
- Blob Versions
- Community
- Containers
- CSV
- Data Management
- Deleted Blobs
- Directories
- Encryption Status
- Inventory Rules
- Legal Hold
- Manifest.json
- Object Count
- Prefix Match
- Retention
- Snapshots
- Storage Account
- Total Object Size
primary_section: azure
section_names:
- azure
feed_name: Microsoft Tech Community
external_url: https://techcommunity.microsoft.com/t5/azure-paas-blog/how-to-get-blob-total-blob-count-and-total-capacity-with-blob/ba-p/4485643
date: 2026-03-23 13:46:08 +00:00
author: ruineiva
---

ruineiva explains how to use Azure Storage Blob Inventory to retrieve total blob count and total capacity at the storage account, container, or directory level by configuring inventory rules (especially the prefixMatch filter) and reading summary values from the generated *-manifest.json file.<!--excerpt_end-->

## Overview

This article shows how to use **Azure Storage Blob Inventory** to get:

- The **total number of blobs**
- The **total capacity** (bytes)

…scoped to:

- A whole **storage account**
- A specific **container**
- A specific **directory path** (prefix)

The key idea is to configure the inventory rule so you can read the totals from the inventory run’s **manifest** file, without processing the full CSV/Parquet inventory output.

## What Blob Inventory is

**Azure Storage blob inventory** generates a report listing containers/blobs (and optionally versions/snapshots/deleted blobs) plus selected properties.

- Output formats: **CSV** or **Apache Parquet**
- Frequency: **daily** or **weekly**

Microsoft documentation: https://learn.microsoft.com/en-us/azure/storage/blobs/blob-inventory

## Configure an inventory rule (Azure portal)

1. Sign in to the **Azure portal**: https://portal.azure.com/
2. Open your **storage account**.
3. Under **Data management**, select **Blob inventory**.
4. Select **Add your first inventory rule** (or **Add a rule**).
5. Fill in the rule fields:

   - **Rule name**: name your rule.
   - **Container**: destination container where the inventory output will be written.
   - **Object type to inventory**: select **blob**.
   - **Blob types**:
     - Blob Storage: select **Block blobs**, **Page blobs**, **Append blobs**.
     - Data Lake Storage: select **Block blobs**, **Append blobs**.
   - **Subtypes**:
     - Blob Storage: select **Include blob versions**, **Include snapshots**, **Include deleted blobs**.
     - Data Lake Storage: select **Include snapshots**, **Include deleted blobs**.
   - **Blob inventory fields**: choose at least the fields needed for this scenario (Microsoft list of supported fields: https://learn.microsoft.com/en-us/azure/storage/blobs/blob-inventory#custom-schema-fields-supported-for-blob-inventory). Examples given:
     - Blob Storage: *Name*, *Creation-Time*, *ETag*, *Content-Length*, *Snapshot*, *VersionId*, *IsCurrentVersion*, *Deleted*, *RemainingRetentionDays*
     - Data Lake Storage: *Name*, *Creation-Time*, *ETag*, *Content-Length*, *Snapshot*, *DeletionId*, *Deleted*, *DeletedTime*, *RemainingRetentionDays*
   - **Inventory frequency**:
     - **Daily** schedules an inventory run every day
     - **Weekly** runs on Sundays (UTC)
   - **Export format**: **csv** or **parquet**
   - **Prefix match** (main focus of this article): filter by blob name prefix.

## Using `prefixMatch` to scope totals

The **Prefix match** field determines where inventory starts collecting blob information.

Example scenario:

- Container: `work`
- Directory under that container: `items`

Use these settings:

- **Storage-account level totals**: leave **Prefix match** empty
- **Container level totals**: set `prefix match = work/`
- **Directory level totals**: set `prefix match = work/items/`

## Where to read blob count and capacity (manifest summary)

After a run, Blob Inventory creates a manifest file named:

- `<ruleName>-manifest.json`

That file contains a `summary` section with the totals you want:

- `objectCount` = total blob count
- `totalObjectSize` = total size in bytes

Example manifest excerpt:

```json
{
  "destinationContainer": "inventory-destination-container",
  "endpoint": "https://testaccount.blob.core.windows.net",
  "files": [
    {
      "blob": "2021/05/26/13-25-36/Rule_1/Rule_1.csv",
      "size": 12710092
    }
  ],
  "inventoryCompletionTime": "2021-05-26T13:35:56Z",
  "inventoryStartTime": "2021-05-26T13:25:36Z",
  "ruleDefinition": {
    "filters": {
      "blobTypes": ["blockBlob"],
      "includeBlobVersions": false,
      "includeSnapshots": false,
      "prefixMatch": ["penner-test-container-100003"]
    },
    "format": "csv",
    "objectType": "blob",
    "schedule": "daily",
    "schemaFields": [
      "Name",
      "Creation-Time",
      "BlobType",
      "Content-Length",
      "LastAccessTime",
      "Last-Modified",
      "Metadata",
      "AccessTier"
    ]
  },
  "ruleName": "Rule_1",
  "status": "Succeeded",
  "summary": {
    "objectCount": 110000,
    "totalObjectSize": 23789775
  },
  "version": "1.0"
}
```

## Special notes

- You must define **one rule per path** (per container or directory prefix) to get totals at those different scopes.
- Inventory runs also generate CSV/Parquet output files. If you only need the totals from the manifest summary, you may want to **delete** those output files to avoid unnecessary storage.

## Support documentation links

- Enable inventory reports: https://learn.microsoft.com/en-us/azure/storage/blobs/blob-inventory-how-to?tabs=azure-portal
- Inventory run scheduling and timing: https://learn.microsoft.com/en-us/azure/storage/blobs/blob-inventory#inventory-run
- Inventory output path format: https://learn.microsoft.com/en-us/azure/storage/blobs/blob-inventory#inventory-output
- Inventory files (inventory, checksum, manifest): https://learn.microsoft.com/en-us/azure/storage/blobs/blob-inventory#inventory-files
- Pricing and billing: https://learn.microsoft.com/en-us/azure/storage/blobs/blob-inventory#pricing-and-billing
- Known issues and limitations: https://learn.microsoft.com/en-us/azure/storage/blobs/blob-inventory#known-issues-and-limitations

## Disclaimer (as provided)

- Steps are for illustration only.
- Provided “as is” without warranties.
- Permission granted to use/modify/reproduce/distribute with conditions:
  - Don’t use the author/company name/logo/trademarks to market your software product embedding the steps.
  - Include a valid copyright notice.
  - Indemnify and hold harmless the providers from claims/lawsuits arising from use/distribution.

Updated Mar 23, 2026

Version 1.0


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-paas-blog/how-to-get-blob-total-blob-count-and-total-capacity-with-blob/ba-p/4485643)

