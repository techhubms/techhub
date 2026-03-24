---
primary_section: ml
feed_name: Microsoft Fabric Blog
tags:
- Bulk Copy
- Change Data Capture
- Checkpointing
- Copy Job
- Data Movement
- Datetime Parsing
- Delayed Extraction
- ELT
- ETL
- Exactly Once Semantics
- Fabric Data Factory
- Incremental Copy
- Incremental Ingestion
- Microsoft Fabric
- ML
- Multi Cloud Data Integration
- News
- ROWVERSION
- SQL Systems
- State Management
- Watermark Column
section_names:
- ml
author: Microsoft Fabric Blog
title: Incremental copy gets more flexible—New watermark column types in Copy job in Fabric Data Factory (Generally Available)
external_url: https://blog.fabric.microsoft.com/en-US/blog/incremental-copy-gets-more-flexible-new-watermark-column-types-in-copy-job-in-fabric-data-factory-generally-available/
date: 2026-03-24 11:00:00 +00:00
---

Microsoft Fabric Blog announces Generally Available expanded watermark column type support in Fabric Data Factory Copy job, enabling incremental copy using ROWVERSION, Date, or String-as-datetime to make incremental ingestion work across more real-world source schemas.<!--excerpt_end-->

# Incremental copy gets more flexible—New watermark column types in Copy job in Fabric Data Factory (Generally Available)

*If you haven’t already, check out Arun Ulag’s hero blog “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for a complete look at all of our FabCon and SQLCon announcements across both Fabric and our database offerings:* https://aka.ms/FabCon-SQLCon-2026-news

## Introduction

[Copy job](https://learn.microsoft.com/fabric/data-factory/what-is-copy-job) is a Microsoft Fabric Data Factory feature for simplified data movement across multiple clouds. It supports:

- Bulk copy
- Incremental copy
- Change data capture (CDC) replication

Incremental copy is a common pattern for keeping analytics systems up to date, but source systems vary in how they expose changes. Many databases don’t have a single, clean datetime or INT column for tracking updates.

This update introduces **expanded watermark column type support in Copy job**, making incremental copy usable across more source schemas.

## What’s new: More watermark column types

Copy job now supports **three additional watermark column types** for incremental copy:

- **ROWVERSION**
- **Date**
- **String (interpreted as datetime)**

Copy job continues to handle state and progress management (for example: state tracking, checkpoints, and incremental windows) automatically.

## Why these new watermark column types matter

### ROWVERSION: Precise change tracking in SQL systems

Many SQL-based systems have a **ROWVERSION** column that automatically changes whenever a row is modified.

Using ROWVERSION as a watermark:

- Captures every insert or update reliably
- Avoids reliance on application-managed timestamps
- Fits high-throughput transactional systems

Copy job uses the monotonic nature of ROWVERSION values to track progress safely across runs.

### Date: Simple and explicit time-based incremental copy

Date/datetime columns are a common approach for incremental ingestion (for example: `LastUpdatedDate`, `ModifiedAt`).

With native Date watermark support:

- You can use existing columns without special workarounds
- Copy job applies **delayed extraction** to avoid data loss or overlap between runs
- Copy job safely manages incremental windows

### String (interpreted as datetime): When datetime values aren’t stored as datetime

Some systems store timestamps as strings instead of a true datetime type.

Copy job now supports **string-based watermark columns** if values can be interpreted as datetime:

- No need to cast/transform columns via custom queries
- No schema changes required in the source
- Incremental copy can still work when timestamps are stored as strings

## Getting started

No new setting is required.

When configuring incremental copy in Copy job:

- Select your source table
- Choose an incremental (watermark) column
- Pick **ROWVERSION**, **Date**, or **String (datetime)** based on your schema

Copy job handles the rest (progress tracking, window management, and **exactly-once semantics**).

## Learn more

- [What is Copy job?](https://learn.microsoft.com/fabric/data-factory/what-is-copy-job)
- Submit feedback on [Fabric Ideas](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas/label-name/data%20factory%20%7C%20copy%20job)
- Join the conversation in [the Fabric Community (Copy job)](https://community.fabric.microsoft.com/t5/Copy-job/bd-p/db_copyjob)

Questions or feedback? Leave your thoughts in the comment section.

[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/incremental-copy-gets-more-flexible-new-watermark-column-types-in-copy-job-in-fabric-data-factory-generally-available/)

