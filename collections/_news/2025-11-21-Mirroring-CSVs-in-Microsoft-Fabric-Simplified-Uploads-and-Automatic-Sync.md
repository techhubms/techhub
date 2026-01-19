---
layout: post
title: 'Mirroring CSVs in Microsoft Fabric: Simplified Uploads and Automatic Sync'
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/mirroring-uploading-your-csvs-is-now-simpler-than-ever-before/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-11-21 11:30:00 +00:00
permalink: /ml/news/Mirroring-CSVs-in-Microsoft-Fabric-Simplified-Uploads-and-Automatic-Sync
tags:
- AI
- AI Workflows
- Automatic Synchronization
- BI
- Change Tracking
- CSV Upload
- Data Integration
- Data Pipeline
- ETL
- Free Storage
- Microsoft Fabric
- Mirroring
- OneLake
- Parquet Files
- Primary Key
- SQL Analytics Endpoint
section_names:
- azure
- ml
---
The Microsoft Fabric Blog shares improvements to open mirroring, allowing CSV and parquet files to be uploaded without primary keys. This update streamlines data integration and empowers advanced analytics. Authored by the Microsoft Fabric Blog team.<!--excerpt_end-->

# Mirroring CSVs in Microsoft Fabric: Simplified Uploads and Automatic Sync

Microsoft Fabric has introduced enhancements to open mirroring that simplify the process of uploading and managing CSV and parquet files in OneLake, making analytics workflows quicker and easier.

## Key Updates

- **Primary Keys No Longer Required**: You can now mirror parquet and delimited text files—including CSVs—without specifying a primary key. This removes a common friction point and accelerates setup.
- **Automatic Table Sync**: Updates to source files are seamlessly inserted into existing mirrored tables in OneLake, maintaining up-to-date data without manual intervention.
- **Advanced Control**: For scenarios where tracking changes is important, options remain to define primary keys and a `__rowMarker__` column, enabling updates, deletes, and granular change tracking.
- **Drag-and-Drop Workflow**: Easily create a Mirror Database within Fabric and upload files by simply dragging and dropping. You can name the table, choose whether or not to specify a primary key, and preview the structure before finalizing.
- **Flexible Updates**: To add new rows, drag additional CSVs into the same mirror database and select ‘Update existing’. The system automatically integrates new data with your existing tables.
- **Optimized for AI and BI**: Mirrored tables are available for downstream analytics and business intelligence, and can be queried from the SQL analytics endpoint.
- **No ETL Overhead**: The new approach eliminates complex ETL pipelines, making data movement lightweight and analytics faster.
- **Free Tier**: Mirrored data includes free compute and storage (up to specified limits).

## Step-by-Step Workflow

1. **Create a Mirror Database**
   - Navigate to ‘New Item’, select the Mirrored Database tile.
2. **Upload Files**
   - Drag and drop your file, optionally specify a table name and primary key. Advanced options and a preview are available.
   - Primary Keys are optional; you can leave this field empty.
3. **Data Optimization**
   - Your table is mirrored into OneLake, ready for analytics and BI use cases.
4. **Incremental Updates**
   - Upload new CSVs with additional records. Choose ‘Update existing’ to append new rows into the relevant table.
   - Example: Inventory table row count increases from 20 to 50 after an update.

## Why These Changes Matter

- **Setup is faster:** No compulsory primary key for most flat files.
- **Data stays fresh:** Automatic synchronization keeps tables current.
- **Advanced needs supported:** Optional keys/row markers for change tracking and advanced scenarios.
- **Streamlined analytics:** Eliminates heavy ETL, ideal for quick AI and BI projects.
- **Cost-effective:** Free compute/storage up to defined limits.

## Resources

- [Open Mirroring in Microsoft Fabric](https://learn.microsoft.com/fabric/mirroring/open-mirroring)
- [Original Blog Post](https://blog.fabric.microsoft.com/en-us/blog/mirroring-uploading-your-csvs-is-now-simpler-than-ever-before/)

For deeper guidance on automating analytics workflows and leveraging mirrored data for AI/ML and BI, refer to official documentation and the Microsoft Fabric Blog.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/mirroring-uploading-your-csvs-is-now-simpler-than-ever-before/)
