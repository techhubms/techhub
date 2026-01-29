---
external_url: https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-expanded-cdc-support-for-more-sources-destinations/
title: Expanded CDC Support and Data Ingestion Enhancements in Microsoft Fabric Copy Job
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-11-19 12:30:00 +00:00
tags:
- Bulk Copy
- Change Data Capture
- Cloud Data Integration
- Copy Job
- Data Engineering
- Data Factory
- Data Ingestion
- Data Movement
- Data Pipeline
- Google BigQuery
- Hybrid Cloud
- Incremental Copy
- Lakehouse
- Microsoft Fabric
- Monitoring
- Observability
- Oracle
- SAP
- Snowflake
- Azure
- Machine Learning
- News
section_names:
- azure
- ml
primary_section: ml
---
Microsoft Fabric Blog introduces enhancements to Data Factory's Copy job—now supporting more CDC sources including SAP, Snowflake, and BigQuery—with improved monitoring, expanded destinations like Fabric Lakehouse, and detailed job analytics for data engineers.<!--excerpt_end-->

# Expanded CDC Support and Data Ingestion Enhancements in Microsoft Fabric Copy Job

**Author: Microsoft Fabric Blog**

Microsoft Fabric Data Factory's Copy job continues to evolve as a robust solution for data movement within and across cloud environments. This news outlines significant improvements—chiefly, the expansion of Change Data Capture (CDC) capabilities and enhanced monitoring features, aimed at data engineers and architects streamlining ingestion and integration tasks.

## Key Copy Job Enhancements

- **Wider CDC Source Support:**
  - Copy job natively supports CDC for additional sources such as SAP via Datasphere, Snowflake, and Google BigQuery.
  - Automatically captures inserts, updates, and deletions—no watermark columns or manual refreshes needed.
  - For early adopters, Oracle CDC and more sources are available under Private Preview ([Copy Job Sign Up Form](http://aka.ms/CopyJobCDCSignUp)).

- **Expanded CDC Destinations:**
  - Fabric Lakehouse tables can now serve as CDC destinations.
  - Facilitates direct merging of CDC-tracked changes (inserts, updates, deletions) into Lakehouse, ensuring data is kept current.

- **Enhanced Observability and Monitoring:**
  - The updated monitoring experience provides:
    - Detailed statistics per run, including watermark values, load types
    - Row counts for inserts, updates, deletions
    - Improved job tracking and visibility

## Delivery Styles Supported

- Bulk copy
- Incremental copy
- Change Data Capture replication

## How This Benefits Data Engineering Teams

- Greater flexibility for hybrid and cross-cloud data movement
- Less manual management—automation around CDC reduces overhead
- More comprehensive monitoring capabilities yield insight and faster troubleshooting
- Supports modern analytics architectures based on Fabric Lakehouse

## Resources

- [Microsoft Fabric Copy Job Documentation](https://learn.microsoft.com/fabric/data-factory/what-is-copy-job)
- Share feedback on [Fabric Ideas](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas/label-name/data%20factory%20%7C%20copy%20job)
- Join discussions in the [Fabric Community](https://community.fabric.microsoft.com/t5/Copy-job/bd-p/db_copyjob)

## Feedback

Have questions or want to share your experience? Leave a comment on the original Microsoft Fabric Blog post and join the engagement in the Fabric community forums.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-expanded-cdc-support-for-more-sources-destinations/)
