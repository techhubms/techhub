---
external_url: https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/secure-delta-sharing-between-databricks-workspaces-using-ncc-and/ba-p/4462428
title: Secure Delta Sharing Between Databricks Workspaces Using NCC and Private Endpoints
author: Rafia_Aqil
feed_name: Microsoft Tech Community
date: 2025-10-18 00:44:26 +00:00
tags:
- Azure Blob Storage
- Azure Databricks
- Azure DFS
- Azure Storage Account
- Cloud Networking
- Data Engineering
- Delta Lake
- Delta Sharing
- Network Connectivity Configuration
- Partitioning Data
- Private Endpoint
- Secure Data Sharing
- Serverless Warehouse
- SQL
- Unity Catalog
- Workspace Metastore
- Azure
- ML
- Security
- Community
section_names:
- azure
- ml
- security
primary_section: ml
---
Rafia_Aqil demonstrates secure Delta table sharing between Azure Databricks workspaces, covering NCC setup and private endpoint approval for robust data connectivity and protection.<!--excerpt_end-->

# Secure Delta Sharing Between Databricks Workspaces Using NCC and Private Endpoints

Author: Rafia_Aqil

This guide walks through securely sharing Delta tables across two Azure Databricks workspaces—NorthCentral and SouthCentral—by configuring Delta Sharing, Network Connectivity Configuration (NCC), and setting up private endpoints for a Serverless Warehouse.

## Part 1: Delta Sharing Between Workspaces

### Access Delta Shares

1. In your NorthCentral Workspace:
    - Go to **Catalog**.
    - Hover over **Delta Shares Received** and click when the icon appears to open the Delta Sharing page.

### Create a New Recipient

1. On the Delta Sharing page:
    - Click **Shared by me**.
    - Click **New Recipient**.
    - Enter recipient details:
        - **Recipient Name**: (Specify the recipient)
        - **Recipient Type**: Select "Databricks"
        - **Sharing Identifier**: Example – `azure:southcentralus:3035j6je88e8-91-434a-9aca-e6da87c1e882`
            - Retrieve using a notebook or SQL: `SELECT CURRENT_METASTORE();`
    - Click **Create**.

### Share Data

1. Initiate sharing:
    - Click **Share Data**.
    - Provide a **Share Name** and select desired data assets.
    - **Note**: Disable **History** for these data assets to simplify access and avoid unwanted historical data exposure. Consider [data partitioning](https://learn.microsoft.com/en-us/azure/databricks/delta-sharing/create-share#partitions) if appropriate.
    - Add the previously created recipient.
    - Click **Share Data**.

### Add Recipient and Complete Setup

1. In the newly created share:
    - Click **Add Recipient** and select your SouthCentral workspace Metastore ID.

### SouthCentral Workspace Operations

1. In your SouthCentral Workspace:
    - Access Delta Sharing page.
    - Under **Shared with me**, locate and open the newly created share.
    - Add the share to a catalog in Unity Catalog for managed access.

## Part 2: Enable NCC for Serverless Warehouse

### Add Network Connectivity Configuration (NCC)

1. In the Databricks Account Console ([link](https://accounts.azuredatabricks.net/)):
    - Go to **Cloud resources** and click **Add Network Connectivity Configuration**.
    - Fill in fields and associate the NCC with SouthCentral.

### Associate NCC with Workspace

1. In Account Console:
    - Go to **Workspaces**, select SouthCentral, and click **Update Workspace**.
    - Under Network Connectivity dropdown, select your newly created NCC.

### Add Private Endpoint Rule

1. In Cloud resources:
    - Select your NCC.
    - Open **Private Endpoint Rules** and select **Add Private Endpoint Rule**.
    - Provide:
        - **Resource ID**: Enter the Storage Account Resource ID for NorthCentral (find it via the “JSON View” in Azure Storage).
        - **Azure Subresource Type**: Specify `dfs` & `blob` as needed.

### Approve Pending Connection

1. In NorthCentral Storage Account settings:
    - Go to **Networking > Private Endpoints**, view pending connections from Databricks, and approve.
    - Check for status update—should show as **ESTABLISHED** in the Account Console.

Upon completion, your shared Delta tables should be listed under **Delta Shares Received** in SouthCentral. If not visible, apply permissions with:

```sql
GRANT USE_PROVIDER ON METASTORE TO `username@xxxx.com`;
```

## Additional Recommendations

- Always review partitioning options for large datasets to optimize access.
- Regularly check NCC and private endpoint statuses to maintain secure connectivity.
- Disable unnecessary history sharing to minimize exposure.

## References

- [Databricks Delta Sharing documentation](https://learn.microsoft.com/en-us/azure/databricks/delta-sharing/)

_Last updated: Oct 18, 2025 – Version 1.0_

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/secure-delta-sharing-between-databricks-workspaces-using-ncc-and/ba-p/4462428)
