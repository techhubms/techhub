---
layout: "post"
title: "Zero-Copy Access to OneLake Data in Azure Databricks (Preview)"
description: "This article explains the introduction of OneLake catalog federation (Beta) in Azure Databricks Lakehouse Federation, enabling Unity Catalog in Azure Databricks to query Microsoft Fabric tables stored in OneLake directly and without data duplication. The integration streamlines multi-engine analytics, centralizes truth, reduces complexity, and allows for frictionless data sharing and querying between Microsoft Fabric and Azure Databricks."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/zero-copy-access-to-onelake-data-in-azure-databricks-preview/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2026-02-23 12:00:00 +00:00
permalink: "/2026-02-23-Zero-Copy-Access-to-OneLake-Data-in-Azure-Databricks-Preview.html"
categories: ["Azure", "ML"]
tags: ["Azure", "Azure Databricks", "Beta Features", "Catalog Federation", "Data Governance", "Data Integration", "Data Lakehouse", "Fabric Tables", "Lakehouse Federation", "Metadata Sync", "Microsoft Fabric", "ML", "News", "OneLake", "SQL Analytics", "Unity Catalog", "Zero Copy Query"]
tags_normalized: ["azure", "azure databricks", "beta features", "catalog federation", "data governance", "data integration", "data lakehouse", "fabric tables", "lakehouse federation", "metadata sync", "microsoft fabric", "ml", "news", "onelake", "sql analytics", "unity catalog", "zero copy query"]
---

Microsoft Fabric Blog presents an overview of OneLake catalog federation (Beta) for Azure Databricks, showing how teams can query Fabric tables in OneLake without making extra data copies.<!--excerpt_end-->

# Zero-Copy Access to OneLake Data in Azure Databricks (Preview)

Managing data across many platforms can result in data duplication, complex pipelines, and confusion about which dataset is accurate. To address this, Microsoft is introducing **OneLake catalog federation (Beta)** within Azure Databricks Lakehouse Federation. This feature enables Unity Catalog in Azure Databricks to directly query data stored in OneLake—Microsoft Fabric's core data lake—without making extra copies.

![Integration diagram showing metadata syncing and zero-copy query access from OneLake to Databricks](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/02/a-microsoft-fabric-and-azure-databricks-integratio-1024x682.png)

## Why It Matters

- **Unified data foundation**: OneLake centralizes data, so multiple teams can use the same data products without redundant storage.
- **Reduced complexity**: No need for extra pipelines or refresh jobs—Fabric data in OneLake is immediately consumable and governed.
- **Flexible analytics**: Azure Databricks can access and analyze Fabric tables directly via Unity Catalog, supporting broad analytical scenarios.

## What You Can Do

- **Discover Fabric tables from Unity Catalog**: Once Unity Catalog is connected, your Fabric schemas and tables automatically appear through a synced foreign catalog.
- **Query OneLake data from Databricks compute**: Continue using familiar Databricks SQL and notebooks, referencing tables as `catalog.schema.table`.
- **Maintain OneLake as the source of truth**: All analysis and processing reference the original data without duplication.

## Conceptual Overview

1. Create a OneLake connection within Unity Catalog.
2. Set up a foreign catalog that points to the relevant Fabric item.
3. Databricks synchronizes metadata, so tables and schemas are reflected in Unity Catalog.
4. Execute queries in Databricks, with data being read directly from OneLake.

For a full walkthrough, see [Enable OneLake catalog federation – Azure Databricks | Microsoft Learn](https://learn.microsoft.com/azure/databricks/query-federation/onelake).

## Beta Limitations

This feature is in Beta, and there are configuration requirements and limitations. Review the details in the official [documentation](https://learn.microsoft.com/azure/databricks/admin/workspace-settings/manage-previews).

## Conclusion

OneLake catalog federation reduces friction for data teams, allowing for a simpler architecture and wider reuse of data products in Microsoft Fabric. With a unified foundation, organizations can build and share analytics solutions more effectively.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/zero-copy-access-to-onelake-data-in-azure-databricks-preview/)
