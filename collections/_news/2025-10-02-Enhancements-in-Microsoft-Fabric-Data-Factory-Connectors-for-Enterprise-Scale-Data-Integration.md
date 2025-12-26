---
layout: "post"
title: "Enhancements in Microsoft Fabric Data Factory Connectors for Enterprise-Scale Data Integration"
description: "This article details the newest innovations in Microsoft Fabric Data Factory connectors, focusing on expanding connectivity, improving performance, and enhancing enterprise readiness. Coverage includes new connectors for diverse data sources, upsert support, advanced security with Microsoft Entra ID, improved governance, and operational enhancements for integration pipelines."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/unlocking-seamless-data-integration-with-the-latest-fabric-data-factory-connector-innovations/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-10-02 09:00:00 +00:00
permalink: "/news/2025-10-02-Enhancements-in-Microsoft-Fabric-Data-Factory-Connectors-for-Enterprise-Scale-Data-Integration.html"
categories: ["Azure", "ML", "Security"]
tags: ["Azure", "Azure Database For PostgreSQL", "Azure Databricks", "Connectors", "Data Factory", "Data Integration", "Dataflow Gen2", "Delta Lake", "Enterprise Security", "Governance", "Lakehouse", "Microsoft Entra ID", "Microsoft Fabric", "ML", "MongoDB", "News", "Pipeline Automation", "Security", "Snowflake", "TLS 1.3", "Upsert"]
tags_normalized: ["azure", "azure database for postgresql", "azure databricks", "connectors", "data factory", "data integration", "dataflow gen2", "delta lake", "enterprise security", "governance", "lakehouse", "microsoft entra id", "microsoft fabric", "ml", "mongodb", "news", "pipeline automation", "security", "snowflake", "tls 1dot3", "upsert"]
---

Microsoft Fabric Blog presents an in-depth overview of the latest Fabric Data Factory connector updates, highlighting advancements in data integration, performance, governance, and security for enterprise analytics teams.<!--excerpt_end-->

# Enhancements in Microsoft Fabric Data Factory Connectors for Enterprise-Scale Data Integration

## Introduction

In this article, the Microsoft Fabric Blog highlights major updates to Fabric Data Factory connectors aimed at improving data integration capabilities across enterprise analytics environments. The new features touch on connectivity, throughput, governance, and security.

## 1. Expanded Connector Coverage

Recent updates introduce new connectors for a wide range of enterprise data sources and platforms:

- **Copy Job & Pipeline:** AWS RDS for Oracle, Azure Database for PostgreSQL 2.0, Azure Databricks Delta Lake, Cassandra, Greenplum, HDFS, Informix, Microsoft Access, Presto, SAP BW Open Hub, SAP Table, Teradata
- **Dataflow Gen2:** Snowflake 2.0, Databricks 2.0, Google BigQuery 2.0, Impala 2.0, Netezza, Vertica, Oracle (OPDG only)

These connectors enable bringing varied, structured and semi-structured data sources into Fabric without external ETL tools, helping break down data silos.

## 2. Performance Improvements

- **Salesforce & Salesforce Service Cloud Connectors** now support partitioned data reading, enabling parallel, multi-threaded data pulls for higher throughput without intricate configurations.

## 3. Enterprise Readiness Enhancements

- **Delta Lakehouse Upsert:** Supports incremental, merge-style data updates.
- **Delta Column Mapping & Deletion Vector:** Helps manage evolving schemas and deleted records for resilience and reliability.
- **varchar(max) Support in Data Warehouse:** Facilitates storage of large, unstructured text fields.
- **Temporal Data Fidelity:** MongoDB & MongoDB Atlas connectors now preserve timestamp and date types.
- **Azure Databricks Connector Governance:** Users can target specific Unity Catalogs for precision in organizational governance.
- **DB2 Package Collection Configuration:** Enterprises can specify DB2 package collection at pipeline level, supporting database standards and governance.
- **Snowflake Role Specification:** Assigns custom access roles to ensure security-compliant data movement.
- **Azure Database for PostgreSQL Upsert and Script Activity:** Supports incremental writes and in-pipeline custom SQL execution.

## 4. Security Enhancements

- **Microsoft Entra ID Authentication:** PostgreSQL connector supports central, managed authentication (formerly Azure AD).
- **TLS 1.3 Support for PostgreSQL:** Latest encryption standards ensure data in transit is secure and compliant.
- **Workspace Identity Authentication:** Enables connectors to use managed identities, reducing the need for hardcoded credentials across pipelines.

## Resources and Next Steps

- [Connector overview documentation](https://learn.microsoft.com/fabric/data-factory/connector-overview)
- [Fabric free trial](https://aka.ms/try-fabric)
- [Fabric Roadmap](https://aka.ms/Fabric-Roadmap)
- [Microsoft Fabric Updates Blog](https://blog.fabric.microsoft.com/blog)

## Conclusion

These enhancements strengthen Microsoft Fabric as a platform for secure, scalable, and flexible data integration in the enterprise. Updates to connector support, performance, and governance give organizations fine-grained control and enable compliance with modern security standards, while simplifying integration of vast and diverse datasets across analytics workloads.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/unlocking-seamless-data-integration-with-the-latest-fabric-data-factory-connector-innovations/)
