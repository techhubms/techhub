---
external_url: https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-movement-across-multiple-clouds-with-copy-job-enhancements-on-incremental-copy-and-change-data-capture/
title: 'Enhancements to Microsoft Fabric Data Factory Copy Job: Incremental Copy and Change Data Capture'
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2026-01-20 09:00:00 +00:00
tags:
- Amazon S3
- Azure Data Explorer
- Change Data Capture
- Cloud Data Integration
- Column Mapping
- Copy Job
- Data Engineering
- Data Factory
- Data Movement
- Fabric Lakehouse
- Google Cloud Storage
- Incremental Copy
- Microsoft Fabric
- Multi Cloud
- SAP Datasphere
- Azure
- ML
- News
- Machine Learning
section_names:
- azure
- ml
primary_section: ml
---
The Microsoft Fabric Blog team announces enhancements to Copy job in Microsoft Fabric Data Factory, adding multi-cloud incremental copy, CDC support, and advanced column mapping for streamlined data movement.<!--excerpt_end-->

# Enhancements to Microsoft Fabric Data Factory Copy Job: Incremental Copy and Change Data Capture

Microsoft Fabric Data Factory's Copy job provides an intuitive solution for moving data across clouds, on-premises systems, and between services. This update introduces major improvements to incremental copy and change data capture (CDC) capabilities, empowering data engineers and architects with greater flexibility and connectivity.

## Expanded Source Store Support for Incremental Copy

Copy job now supports additional connectors for incremental copy, enabling robust multi-cloud integration. New supported sources include:

- Google Big Query
- Google Cloud Storage
- DB2
- ODBC
- Fabric Lakehouse table
- Folder
- Azure files
- SharePoint List
- Amazon RDS for SQL Server
- Amazon RDS for Oracle
- Azure Data Explorer

## Incremental Copy from Fabric Lakehouse

Copy job supports both Delta Change Data Feed (CDF) and watermark-based methods when performing incremental copy from a Fabric Lakehouse table. Users can choose:

- **CDF**: Captures inserts, updates, and deletions for comprehensive replication.
- **Watermark-based**: Tracks changes using a selected column, offering flexibility when CDF is not enabled.

Configuration is accessible via Advanced Settings after creating a Copy job.

## Expanded CDC Support for SAP Datasphere Outbound

In addition to existing support for ADLS Gen2, Copy job now allows CDC replication from SAP Datasphere Outbound to Amazon S3 and Google Cloud Storage. This expansion increases compatibility and staging options for enterprises with hybrid and multi-cloud architectures.

Refer to [the official tutorial](https://learn.microsoft.com/en-us/fabric/data-factory/copy-job-tutorial-sap-datasphere) for detailed guidance.

## Column Mapping in CDC

Copy job introduces comprehensive column mapping for CDC replication, allowing users to:

- Rename columns
- Change data types
- Customize destination schemas

Column mapping is now available for full copy, watermark-based incremental copy, and CDC scenarios.

## Additional Resources

- [Microsoft Fabric Copy Job Documentation](https://learn.microsoft.com/fabric/data-factory/what-is-copy-job)
- [Fabric Ideas – Copy Job](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas/label-name/data%20factory%20%7C%20copy%20job)
- [Fabric Community – Copy Job discussions](https://community.fabric.microsoft.com/t5/Copy-job/bd-p/db_copyjob)

For questions or feedback, join the conversation in the Fabric Community or leave a comment on the original blog post.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-movement-across-multiple-clouds-with-copy-job-enhancements-on-incremental-copy-and-change-data-capture/)
