---
layout: "post"
title: "Advanced SAP Data Integration with Microsoft Fabric: Mirroring and Copy Job"
description: "This article presents a comprehensive overview of new SAP data integration capabilities within Microsoft Fabric, focusing on Mirroring and Copy Job enhancements. It outlines how Microsoft Fabric enables zero-ETL, real-time replication and petabyte-scale data movement for SAP sources, supporting unified analytics and AI-ready data integration via OneLake, Data Factory, and Data Agents. The piece is aimed at technical practitioners and architects seeking to maximize SAP and non-SAP data value using Fabric’s suite of connectors, multi-cloud support, and engineering features."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/announcing-sap-connectivity-with-mirroring-and-copy-job-in-microsoft-fabric/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-11-19 08:30:00 +00:00
permalink: "/2025-11-19-Advanced-SAP-Data-Integration-with-Microsoft-Fabric-Mirroring-and-Copy-Job.html"
categories: ["AI", "Azure", "ML"]
tags: ["AI", "AI Ready Data", "Azure", "BW OpenHub", "Copy Job", "Data Agents", "Data Engineering", "Data Factory", "Data Integration", "Microsoft Fabric", "Mirroring", "ML", "Multi Cloud", "News", "OData", "ODBC", "OneLake", "Petabyte Scale", "Real Time Analytics", "SAP Business Data Cloud Connect", "SAP Connectors", "SAP Datasphere", "Zero ETL"]
tags_normalized: ["ai", "ai ready data", "azure", "bw openhub", "copy job", "data agents", "data engineering", "data factory", "data integration", "microsoft fabric", "mirroring", "ml", "multi cloud", "news", "odata", "odbc", "onelake", "petabyte scale", "real time analytics", "sap business data cloud connect", "sap connectors", "sap datasphere", "zero etl"]
---

Ulrich Christ, Wilson Lee, Linda Wang, and Ye Xu introduce new SAP data connectivity features in Microsoft Fabric, detailing Mirroring and Copy Job for zero-ETL and scalable data integration for unified analytics and AI scenarios.<!--excerpt_end-->

# Advanced SAP Data Integration with Microsoft Fabric: Mirroring and Copy Job

*By Ulrich Christ, Wilson Lee, Linda Wang, and Ye Xu*

Microsoft Fabric now offers cutting-edge SAP data integration capabilities designed to unify enterprise data in the cloud for advanced analytics and AI applications. With newly announced Mirroring for SAP Datasphere and Copy Job support, organizations gain:

## Key Capabilities

- **Mirroring for SAP Datasphere**: Achieve zero-ETL, near real-time replication of SAP data directly into OneLake, maintaining business logic, security, and data lineage. Supports major SAP applications (SAP S/4HANA, ECC, BW, SuccessFactors, Ariba, Concur) via native extraction technologies.
- **Copy Job for SAP Datasphere**: Move SAP data (bulk, incremental, CDC) at petabyte scale to destinations across Azure, AWS, Snowflake, and more. Enables managed, secure, cross-cloud data movement that powers holistic analytics and innovation.

## Fabric Data Factory Integration

Fabric Data Factory provides:

- A wide suite of built-in SAP connectors (HANA, BW, ECC, S/4HANA, OData, ODBC, OpenHub) for both citizen and professional data engineers.
- Features for mapping tables, managing schema, and orchestrating full-scale integration pipelines in a multi-cloud context.
- Dataflow Gen2 and Pipeline support for easy access to analytic layers, SQL artifacts, and native SAP business objects.

#### Example Connector Table

| Connector               | SAP Sources                | Use Case                                       | Fabric Artefact    |
|------------------------|----------------------------|------------------------------------------------|-------------------|
| SAP BW                 | SAP BW, BW/4HANA, S/4HANA | Multidimensional analytics layer               | Dataflow Gen2     |
| SAP HANA               | HANA, HANA Cloud, Datasphere| SQL tables, views, Calculation Views           | Dataflow Gen2, Pipeline, Copy Job |
| SAP Table              | S/4HANA, ECC               | DDIC tables, ABAP CDS Views                    | Pipeline, Copy Job|
| SAP BW OpenHub         | BW                         | Outbound integration with InfoProviders        | Pipeline, Copy Job|
| OData                  | SuccessFactors, C4C, S/4HANA, ECC | OData services                     | Dataflow Gen2, Pipeline, Copy Job|
| ODBC                   | HANA, HANA Cloud, Datasphere| Access SQL tables, views                       | Dataflow Gen2, Pipeline, Copy Job|

## AI-Ready Data and Data Agents

Unified data in OneLake enables Data Agents in Fabric to deliver advanced analytics and AI-powered automation. Integration points create cross-domain reporting (e.g., finance and customer metrics), all available in a single, secure environment—enhancing decision-making and business intelligence.

## Upcoming: SAP Business Data Cloud Connect

SAP BDC Connect for Microsoft Fabric will soon allow zero-copy data sharing, securely integrating SAP data products into OneLake and making them accessible from both Microsoft 365 and SAP applications. Advanced data engineering, warehousing, and Power BI features will support enterprise-grade AI and analytics use cases for SAP and non-SAP estates.

## Resources and Next Steps

- [Mirrored database from SAP Datasphere documentation](https://aka.ms/sap-mirroring-doc)
- [Copy Job with SAP Data documentation](https://aka.ms/sap-copyjob-doc)
- [Data Agents in Microsoft Fabric](https://learn.microsoft.com/fabric/data-agents)
- [Fabric roadmap](https://aka.ms/Fabric-Roadmap)
- [Fabric blog](https://blog.fabric.microsoft.com/blog)
- [Fabric forums](https://aka.ms/fabric-community)

Organizations can now instantly replicate and unify SAP data, activate holistic analytics, and prepare their data estates for a new generation of AI-driven insights using Fabric's expanding platform.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/announcing-sap-connectivity-with-mirroring-and-copy-job-in-microsoft-fabric/)
