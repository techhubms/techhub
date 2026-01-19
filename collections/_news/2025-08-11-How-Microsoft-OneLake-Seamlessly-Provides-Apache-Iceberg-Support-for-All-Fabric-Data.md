---
external_url: https://blog.fabric.microsoft.com/en-US/blog/how-to-access-your-microsoft-fabric-tables-in-apache-iceberg-format/
title: How Microsoft OneLake Seamlessly Provides Apache Iceberg Support for All Fabric Data
author: Microsoft Fabric Blog
viewing_mode: external
feed_name: Microsoft Fabric Blog
date: 2025-08-11 09:00:00 +00:00
tags:
- Apache Iceberg
- Apache XTable
- Cloud Data Platforms
- Data Engineering
- Data Governance
- Data Lake
- Delta Lake
- Dremio
- ETL
- Fabric Lakehouse
- Interoperability
- Lakehouse
- Metadata Conversion
- Microsoft Fabric
- OneLake
- Open Table Formats
- Snowflake
- Table Format Virtualization
- Table Formats
- Trino
- Virtualization
section_names:
- ml
---
Microsoft Fabric Blog, with Kevin Liu, presents how Microsoft OneLake now seamlessly virtualizes Delta Lake tables as Apache Iceberg tables, enabling true multi-engine analytics without data duplication.<!--excerpt_end-->

# How Microsoft OneLake Seamlessly Provides Apache Iceberg Support for All Fabric Data

Co-authored with [Kevin Liu](https://www.linkedin.com/in/kevinjqliu/), Apache Iceberg Committer and Principal Engineer at Microsoft.

Microsoft Fabric is a unified SaaS data and analytics platform for the AI era. Traditionally, all workloads in Microsoft Fabric use Delta Lake as the open-source table format. With OneLake—Fabric’s single data lake—organizations unify their data estate across multiple clouds and on-premises systems.

## New: Delta Lake to Apache Iceberg Interop

Microsoft recently announced that OneLake can now transparently serve Delta Lake tables as Apache Iceberg tables. This capability means organizations can use engines and tools that support either Delta or Iceberg formats, promoting true openness and avoiding data duplication or migration efforts.

### How To Get Started

1. **Create or identify** a Delta Lake table in OneLake.
2. **Use any Iceberg-compatible engine** (such as Spark, Trino, Snowflake) to query the table.
3. **No manual conversion required**—OneLake automatically serves the table as Iceberg.

## Why Is This Important?

- Many analytics engines support different open table formats, such as Delta Lake (for Spark-based pipelines), and Iceberg (used by Trino, Dremio, and Snowflake).
- OneLake’s metadata translation empowers seamless interoperability—your tables are accessible in both ecosystems with no duplication or workflow changes.
- Easily utilize Iceberg-native tools on top of existing Delta Lake datasets, and simplify governance with a single source of truth.

## Step-by-Step Example

- Examine a table stored in the Delta format in the Fabric Lakehouse.
- View metadata directories: both *_delta_log* and *metadata* folders should exist.
- Under *metadata*, Iceberg files are present, making the table accessible in Iceberg format.

### Using in Snowflake

- In Snowflake, create an external volume pointing to the Fabric Lakehouse.
- Create an Iceberg catalog.
- Reference the Iceberg metadata file path from OneLake to create a table.
- Query the table as any native Iceberg table in Snowflake.

## Technical Details: Table Format Virtualization

OneLake uses a process called **table format virtualization**. When an Iceberg-compatible engine accesses a table natively stored as Delta, OneLake dynamically generates the necessary Iceberg metadata files on demand.

This architecture uses [Apache XTable](https://xtable.apache.org/) for metadata conversion. Enhancements include translating Delta deletion vectors into Iceberg positional delete files, with plans to contribute improvements back to the open source community.

### Workflow Overview

- The Delta table lives in OneLake or external storage (e.g. ADLS, S3).
- When queried by an Iceberg engine, OneLake intercepts the request and produces Iceberg-compatible metadata in real-time.
- The source Delta data and its *\_delta_log* structure remain unmodified.
- The process only serves the necessary Iceberg files to the requesting engine, ensuring performance and consistency.

## Future Directions

- Expanding support for additional data types and advanced features.
- Ensuring compatibility with is upcoming Iceberg V3 specification.
- Continued enhancements for easier analytics across engines and avoiding storage silos.

## Learn More

- [Official announcement: Access your Delta Lake tables as Apache Iceberg automatically](https://blog.fabric.microsoft.com/blog/new-in-onelake-access-your-delta-lake-tables-as-iceberg-automatically/)
- [Store and use your Iceberg data in OneLake](https://support.fabric.microsoft.com/blog/store-and-use-your-snowflake-iceberg-data-in-onelake/)
- [Microsoft Fabric documentation](https://learn.microsoft.com/fabric)

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/how-to-access-your-microsoft-fabric-tables-in-apache-iceberg-format/)
