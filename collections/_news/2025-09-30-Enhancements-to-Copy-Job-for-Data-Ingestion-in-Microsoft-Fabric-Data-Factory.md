---
external_url: https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-connection-parameterization-expanded-cdc-and-connectors/
title: Enhancements to Copy Job for Data Ingestion in Microsoft Fabric Data Factory
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-09-30 17:00:00 +00:00
tags:
- Amazon RDS
- Amazon S3
- Azure SQL Database
- Change Data Capture
- CI/CD Automation
- Connection Parameterization
- Copy Job
- Data Factory
- Data Ingestion
- Data Replication
- Delta Change Data Feed
- Fabric Lakehouse Table
- Greenplum
- Incremental Copy
- Lakehouse
- MariaDB
- Microsoft Fabric
- New Connectors
- Oracle Cloud Storage
- Pipeline Automation
- SAP
- SFTP
- Snowflake Integration
- SQL Server
- Storage Integration
- Variables Library
section_names:
- azure
- ml
---
Microsoft Fabric Blog introduces major updates to Copy job in Data Factory, enabling connection parameterization, expanded CDC support, and enhanced connector options for flexible data integration workflows.<!--excerpt_end-->

# Enhancements to Copy Job for Data Ingestion in Microsoft Fabric Data Factory

The Copy job feature in Microsoft Fabric Data Factory provides an intuitive solution for moving data between sources and destinations. Recent updates introduce powerful capabilities:

## Connection Parameterization with Variables Library (Preview)

- Externalize connection details using a centralized Variables Library
- Seamlessly promote Copy jobs across development, testing, and production environments
- Streamline CI/CD pipeline configuration for data integration workflows

More details: [CI/CD for Copy Job documentation](https://learn.microsoft.com/fabric/data-factory/cicd-copy-job)

## Expanded Change Data Capture (CDC) Support

- Copy job now natively supports CDC for Fabric Lakehouse Table connector
- Automate replication of inserts, updates, and deletes using Delta Change Data Feed (CDF)
- Keep downstream systems updated efficiently, eliminating manual refreshes
- Distribute data from OneLake to destinations such as SQL and Snowflake

Reference: [CDC for Copy Job documentation](https://learn.microsoft.com/fabric/data-factory/cdc-copy-job)

## Merge Data into Snowflake

- Updated connector allows CDC-driven data merge into Snowflake
- Supports inserts, updates, and deletions sourced from Azure SQL DB, SQL Server, SQL MI, and Fabric Lakehouse
- Enhanced security via Storage Integration and Snowflake-assigned roles
- Securely connect to Azure Blob Storage without exposing credentials

Read more: [CREATE STORAGE INTEGRATION](https://docs.snowflake.com/en/sql-reference/sql/create-storage-integration)

## Expanded Connector List

The Copy job now supports additional data sources and destinations:

**Newly Supported Connectors:**

- Folder
- REST API
- SAP Table
- SAP BW Open Hub
- Amazon RDS for Oracle
- Cassandra
- Greenplum
- Informix
- Microsoft Access
- Presto

**Incremental Copy Support:**

- SAP HANA
- MariaDB
- MySQL
- SFTP
- FTP
- Oracle cloud storage
- Amazon S3 Compatible

For the complete list: [Supported connectors](https://learn.microsoft.com/en-us/fabric/data-factory/what-is-copy-job#supported-connectors)

## Copy Assistant Improvements

- Simplified workflow through Copy Assistant for pipeline integration
- Eliminates need for parameterized foreach loops for basic data movement
- Enables all Copy job features: incremental copy, CDC, and more

## Additional Resources

- [What is Copy job in Data Factory?](https://learn.microsoft.com/fabric/data-factory/what-is-copy-job)
- [Fabric Ideas Community](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas/label-name/data%20factory%20%7C%20copy%20job)
- [Fabric technical documentation](https://aka.ms/FabricBlog/docs)

For questions or feedback, visit the Fabric Community or documentation links above.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-connection-parameterization-expanded-cdc-and-connectors/)
