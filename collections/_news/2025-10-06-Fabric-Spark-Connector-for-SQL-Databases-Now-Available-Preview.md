---
external_url: https://blog.fabric.microsoft.com/en-US/blog/spark-connector-for-sql-databases-preview/
title: Fabric Spark Connector for SQL Databases Now Available (Preview)
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-10-06 10:12:54 +00:00
tags:
- Authentication
- Azure SQL Database
- Azure SQL Managed Instance
- Big Data
- Data Engineering
- Data Science
- Fabric Runtime
- Fabric SQL Database
- Microsoft Fabric
- PySpark
- Scala
- Spark Connector
- SQL Security
- SQL Server On Azure VM
section_names:
- azure
- ml
primary_section: ml
---
Microsoft Fabric Blog details the new Fabric Spark connector for SQL databases, explaining how Spark developers and data scientists can natively access and secure data across multiple Microsoft SQL platforms.<!--excerpt_end-->

# Fabric Spark Connector for SQL Databases (Preview)

The Fabric Spark connector for SQL databases (Azure SQL Database, Azure SQL Managed Instance, Fabric SQL databases, and SQL Server in Azure VM) is now available in preview. This update simplifies data integration for Spark developers and data scientists by providing streamlined access to SQL database engines through a unified Spark API.

## Key Features

- **Native Integration and Deployment**: The connector comes preinstalled as a default library within the Fabric Runtime, removing the need for manual installation.
- **Wide Product Support**: Compatible with Azure SQL Database, Azure SQL Managed Instance, SQL Server on Azure VM, and Fabric SQL databases.
- **Performance and Scale**: Enables high-performance, large-scale read and write operations from Spark to Microsoft SQL products.
- **Advanced Security**: Enforces the SQL engine-level security models, including:
  - Object-Level Security (OLS)
  - Row-Level Security (RLS)
  - Column-Level Security (CLS)
- **Multi-Language Support**: Supports both PySpark and Scala. PySpark users can now utilize the connector natively, eliminating workarounds.
- **Flexible Authentication**: Offers several authentication methods to fit different enterprise and security environments.

## Why is this important?

- Simplifies the data engineering workflow for teams building on Microsoft Fabric and SQL databases
- Enhances security and compliance by enforcing robust SQL-level security, even when accessing data via Spark
- Supports modern data science stacks (PySpark and Scala), improving accessibility for data scientists
- Immediate integration (preinstalled) means engineers can start using the connector without additional setup

## Learn More

Official documentation: [Spark connector for SQL databases](https://learn.microsoft.com/fabric/data-engineering/spark-sql-connector)

## Conclusion

The Fabric Spark connector for SQL databases increases efficiency and security for Spark-based data engineering and analytics within Microsoft’s unified data platform. Data engineers and data scientists can now perform scalable data operations securely and natively across Microsoft Fabric’s supported SQL platforms.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/spark-connector-for-sql-databases-preview/)
