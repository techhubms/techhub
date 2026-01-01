---
layout: "post"
title: "IDENTITY Columns in Fabric Data Warehouse (Preview)"
description: "This article presents an in-depth look at the new IDENTITY column feature in Microsoft Fabric Data Warehouse, explaining how it automatically generates surrogate keys for new table rows at cloud scale. It details the technical underpinnings, including its distributed design, differences from traditional SQL Server implementations, and the practical implications for modern analytics workloads. Readers learn how IDENTITY columns enhance data integrity, support parallel ingestion, and streamline ETL and data modeling tasks."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/identity-columns-in-fabric-data-warehouse-preview/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-11-20 02:30:00 +00:00
permalink: "/2025-11-20-IDENTITY-Columns-in-Fabric-Data-Warehouse-Preview.html"
categories: ["Azure", "ML"]
tags: ["Azure", "BIGINT", "Cloud Analytics", "Data Integrity", "Data Modeling", "Data Warehouse", "Distributed Architecture", "ETL", "Fabric Data Warehouse", "IDENTITY Column", "Ingestion Pipeline", "Microsoft Fabric", "ML", "News", "Parallel Ingestion", "Preview Features", "SQL Server", "Surrogate Key", "T SQL"]
tags_normalized: ["azure", "bigint", "cloud analytics", "data integrity", "data modeling", "data warehouse", "distributed architecture", "etl", "fabric data warehouse", "identity column", "ingestion pipeline", "microsoft fabric", "ml", "news", "parallel ingestion", "preview features", "sql server", "surrogate key", "t sql"]
---

Microsoft Fabric Blog explains the technical implementation and benefits of IDENTITY columns in Fabric Data Warehouse, focusing on automatic surrogate key generation, scalability, and distributed data integrity for analytics workloads.<!--excerpt_end-->

# IDENTITY Columns in Fabric Data Warehouse (Preview)

IDENTITY columns in Fabric Data Warehouse have been introduced based on direct feedback from the data community, addressing the need for simple and reliable automatic key generation. These columns generate unique numeric surrogate keys for each new row in a table, streamlining the data ingestion and analytics workflow.

## What Are IDENTITY Columns?

IDENTITY columns are system-managed fields that automatically assign a unique numeric value, typically used as surrogate keys when natural keys aren't available. This removes the necessity for manual key assignment or custom logic (such as using MAX(ID)+1, ROW_NUMBER(), hashing, or GUID generation) during data ingestion, simplifying ETL processes and ensuring data integrity.

## Advantages of IDENTITY Columns in Fabric Data Warehouse

- Eliminate the need for custom key-generation logic in ETL scripts or applications
- Reduce operational overhead by letting the warehouse engine manage keys
- Guarantee uniqueness at scale, even with parallel ingestion and distributed workloads
- Prevent key duplication errors during concurrent loads

## Distributed Implementation Details

Unlike SQL Server's single-instance model, Fabric Data Warehouse implements IDENTITY columns in a massively parallel, distributed architecture. Backend compute nodes are assigned ranges of unique values, which they use to process rows in parallel. This can result in gaps in value sequences but always preserves uniqueness. Ordered sequences are not guaranteed, reflecting the distributed design priorities of scalability and ingestion throughput.

## Example: Adding an IDENTITY Column

### Table Creation

```sql
CREATE TABLE dbo.TripData (
  tripID BIGINT IDENTITY,
  vendorID VARCHAR(50),
  tpepPickupDateTime DATETIME2(6),
  tpepDropoffDateTime DATETIME2(6),
  passengerCount INT,
  tripDistance FLOAT
);
```

### Ingesting Data

Use `COPY INTO` to ingest data, omitting the IDENTITY column from source:

```sql
COPY INTO dbo.TripData (
  vendorID 1, tpepPickupDateTime 2, tpepDropoffDateTime 3,
  passengerCount 4, tripDistance 5
) FROM 'https://azureopendatastorage.blob.core.windows.net/nyctlc/yellow/puYear=2013/'
WITH ( FILE_TYPE = 'PARQUET' )
```

### Querying Results

```sql
SELECT TOP 10 * FROM TripData ORDER BY tripID
```

Here, 'tripID' is automatically populated with a unique, system-generated BIGINT value for each row, ensuring no duplicates even with distributed ingestion.

## Technical Considerations

- IDENTITY columns use BIGINT data type, supporting values from 1 to 9,223,372,036,854,775,807 (positive only), which is sufficient for nearly any modern dataset.
- Once assigned, values are never reused for the same table.
- The distributed allocation model means ordered, gapless sequences are not guaranteed, but data integrity is preserved.

## Limitations and Future Directions

- `IDENTITY_INSERT` is not supported in Preview but future releases may allow user insertion of negative values for specialized placeholders in dimension tables.
- For scenarios requiring strictly ordered keys or reseeding, alternative strategies may be necessary.

## Getting Started

Define IDENTITY columns in your Fabric Data Warehouse tables to simplify key generation and speed up analytics workloads. See the [official tutorial](https://aka.ms/identitydwdocs) and [documentation](https://blog.fabric.microsoft.com/en-us/blog/identity-columns-in-fabric-data-warehouse-preview/) for implementation guides.

Feedback on the feature is welcome as it evolves in Preview—share your experiences and suggestions on [Ideas](https://ideas.fabric.microsoft.com/).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/identity-columns-in-fabric-data-warehouse-preview/)
