---
external_url: https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-reset-incremental-copy-auto-table-creation-and-json-format-support/
title: 'Enhancements to Microsoft Fabric Copy Job: Reset Incremental Copy, Auto Table Creation, and JSON Support'
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-08-13 09:00:00 +00:00
tags:
- Auto Table Creation
- Azure SQL Database
- Change Data Capture
- Cloud Data Integration
- Copy Job
- Data Automation
- Data Engineering
- Data Ingestion
- Data Movement
- Data Pipeline
- Fabric Data Factory
- Fabric Lakehouse
- Incremental Copy
- JSON Format
- Microsoft Fabric
- SQL Server
- Azure
- ML
- News
- Machine Learning
section_names:
- azure
- ml
primary_section: ml
---
The Microsoft Fabric Blog team introduces new Copy Job features—resettable incremental copy, auto table creation, and JSON support—to improve data ingestion workflows for technical users.<!--excerpt_end-->

# Enhancements to Microsoft Fabric Copy Job: Reset Incremental Copy, Auto Table Creation, and JSON Support

## Introduction

The Microsoft Fabric Blog has announced a set of powerful new features for Copy Job in Fabric Data Factory. These enhancements are designed to streamline data movement and ingestion across a variety of platforms—cloud, on-premises, and hybrid environments.

## What's New in Copy Job

### 1. Reset Incremental Copy

- **Greater Flexibility:** Incremental copy allows for efficient data synchronization by transferring only new or updated records after the initial full load.
- **On-Demand Full Refresh:** Users can now easily reset the incremental state for any table, triggering a full data copy on the next run. This is particularly useful for resolving data discrepancies between source and destination without affecting other tables.
- **Per-Table Control:** Reset operations can occur on individual tables, enabling targeted troubleshooting and minimizing disruption.

### 2. Auto Table Creation on Destination

- **Frictionless Data Movement:** If a destination table does not exist, Copy Job will automatically generate the table schema and create it for you—no manual pre-configuration required.
- **Supported Destinations:**
  - SQL Server
  - Azure SQL Database
  - Fabric Lakehouse Table
  - Snowflake
  - Azure SQL Managed Instance
  - Fabric SQL Database

### 3. JSON Format Support

- **Expanded File Format Compatibility:** In addition to existing support for CSV and Parquet, Copy Job now enables high-throughput movement of JSON files to and from table destinations, giving data engineers more flexibility with modern data formats.

### 4. Quick Access to Connection Details

- **Improved User Experience:** Hover over any data connection in the Copy Job panel to quickly view service and workspace details. This streamlines configuration and helps troubleshoot connection issues.

## How These Changes Help

These improvements are aimed at developers and data professionals seeking to minimize manual intervention, automate common tasks, and handle diverse data sources and targets efficiently. They address challenges in resetting pipelines, automating schema creation, and working with structured or semi-structured data formats.

## Additional Resources

- [Microsoft Fabric Copy Job Documentation](https://learn.microsoft.com/fabric/data-factory/what-is-copy-job)
- [Fabric Ideas Community](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas/label-name/data%20factory%20%7C%20copy%20job)
- [Fabric Community Copy Job Discussions](https://community.fabric.microsoft.com/t5/Copy-job/bd-p/db_copyjob)
- [General Fabric Documentation](https://aka.ms/FabricBlog/docs)

## Conclusion

The latest update to Microsoft Fabric's Copy Job can lead to greater productivity and ease of use for data engineers and technical teams. Features like incremental copy reset, automatic table creation, expanded JSON support, and quicker connection diagnostics make managing large-scale data pipelines more seamless.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-reset-incremental-copy-auto-table-creation-and-json-format-support/)
