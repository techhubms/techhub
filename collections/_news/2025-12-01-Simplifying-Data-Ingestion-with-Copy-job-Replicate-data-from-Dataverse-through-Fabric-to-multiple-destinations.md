---
layout: "post"
title: "Simplifying Data Ingestion with Copy job – Replicate data from Dataverse through Fabric to multiple destinations"
description: "This guide explains how to use Microsoft Fabric Data Factory's Copy job feature to efficiently move and replicate data from Dataverse to multiple destinations, including across different tenants and regions. The article covers configuration of CDC (Change Data Capture), integration of Dataverse with Fabric Lakehouse, and setup of Copy jobs to support bulk and incremental data movement."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-replicate-data-from-dataverse-through-fabric-to-multiple-destinations/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-12-01 09:00:00 +00:00
permalink: "/news/2025-12-01-Simplifying-Data-Ingestion-with-Copy-job-Replicate-data-from-Dataverse-through-Fabric-to-multiple-destinations.html"
categories: ["Azure", "ML"]
tags: ["Azure", "Azure SQL Database", "Bulk Copy", "Change Data Capture", "Copy Job", "Data Factory", "Data Ingestion", "Data Replication", "Data Synchronization", "Dataverse", "Dynamics 365", "Fabric Lakehouse", "Fabric Link", "Incremental Copy", "Microsoft Fabric", "ML", "News", "Power Platform"]
tags_normalized: ["azure", "azure sql database", "bulk copy", "change data capture", "copy job", "data factory", "data ingestion", "data replication", "data synchronization", "dataverse", "dynamics 365", "fabric lakehouse", "fabric link", "incremental copy", "microsoft fabric", "ml", "news", "power platform"]
---

Microsoft Fabric Blog details how to leverage the Data Factory Copy job to move and replicate data from Dataverse to multiple destinations, including setup for CDC and Fabric Lakehouse integration.<!--excerpt_end-->

# Simplifying Data Ingestion with Copy job – Replicate data from Dataverse through Fabric to multiple destinations

Microsoft Fabric Data Factory’s Copy job streamlines the movement of data from varied sources—including Dataverse—across clouds, on-premises environments, and services. It supports delivery patterns like bulk copy, incremental copy, and change data capture (CDC) replication, making it suitable for a range of ingestion and replication scenarios.

## Key Features

- **Flexible Data Movement**: Handles copying data from any source to any destination, including cross-region or cross-tenant transfers.
- **CDC Support**: Natively supports CDC for automatic capture and replication of inserts, updates, and deletions.
- **Fabric Link Integration**: For sources not natively supported for CDC (e.g., Dataverse), Fabric Link enables CDC replication in conjunction with Copy job.

## Step-by-Step Setup

### 1. Prerequisites

- Ensure your Dynamics Finance & Operations (F&O) ERP environment is linked to a Dataverse environment. This can be verified in the Power Platform Admin Center.

### 2. Linking Dataverse to Fabric

- Visit the Power Apps maker portal and select “Link to Microsoft Fabric” to start replicating data into a Fabric Lakehouse.
- Provide the required connection details and select or create a workspace in Fabric. A Lakehouse is provisioned automatically.

### 3. Selecting Data for Replication

- Choose which tables from Dataverse and F&O to synchronize to Fabric within the workspace.
- After provisioning, access the new Fabric Lakehouse directly.

### 4. Creating the Copy Job

- In Microsoft Fabric, use the Copy job creation wizard.
- Set the new Lakehouse as the data source.
- Choose the tables from the Lakehouse to replicate.
- Select one or multiple CDC-supported destinations (such as Azure SQL Database) as targets.
- Enable incremental copy and select an update/merge method appropriate to your needs.
- On execution, the job performs a full initial copy, followed by ongoing syncs for all data changes (inserts, updates, deletions).

## Supported Scenarios

- Movement across clouds or data centers
- Synchronization between Azure tenants
- Replication to analytics stores, operational systems, or other destinations supported by CDC

## Additional Resources

- [What is Copy job – Microsoft Fabric documentation](https://learn.microsoft.com/fabric/data-factory/what-is-copy-job)
- [CDC in Copy Job documentation](https://learn.microsoft.com/en-us/fabric/data-factory/cdc-copy-job)
- [Fabric Community and Ideas](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas/label-name/data%20factory%20%7C%20copy%20job)

For comments or further feedback, engage in the Fabric Community forums.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-replicate-data-from-dataverse-through-fabric-to-multiple-destinations/)
