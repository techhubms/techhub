---
external_url: https://blog.fabric.microsoft.com/en-US/blog/cross-cloud-data-movement-with-best-in-class-connectivity-whats-new-and-whats-next/
title: Cross-Cloud Data Movement with Best-in-Class Connectivity in Fabric Data Factory
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-09-29 09:00:00 +00:00
tags:
- Change Data Capture
- Cloud Data Migration
- Connectors
- Copy Job
- Cross Cloud Integration
- Data Governance
- Data Integration
- Data Lakehouse
- Data Movement
- Data Pipelines
- Enterprise Connectivity
- ETL
- Fabric Data Factory
- Incremental Copy
- Microsoft Fabric
- On Premises Integration
- OneLake
- SQL Database
- VNet Gateway
section_names:
- azure
- ml
primary_section: ml
---
The Microsoft Fabric Blog introduces major updates to Fabric Data Factory, highlighting new cross-cloud data movement and integration features. Learn from Microsoft's team how these advancements strengthen enterprise data strategies.<!--excerpt_end-->

# Cross-Cloud Data Movement with Best-in-Class Connectivity in Fabric Data Factory

Microsoft Fabric is reshaping the landscape of AI-ready data estates by consolidating data, governance, and compute on a unified SaaS platform designed for mission-critical analytics. This blog details new enhancements for Fabric Data Factory, targeting seamless, secure, and scalable data integration across clouds and environments.

## Key Innovations

### Copy Job for Petabyte-Scale Cross-Cloud Data Movement

- **Bulk and Incremental Copy**: Move massive datasets between Azure, AWS, Snowflake, or on-premises sources with managed execution.
- **Change Data Capture (CDC)**: Keep destinations like SQL Database and Snowflake updated in real time with inserts, updates, and deletes.
- **Observability and Error Handling**: Built-in tools improve reliability and reduce operational overhead.
- **Secure, Network-Compliant Access**: Connect to protected data sources behind firewalls using VNet Gateway support.

### Enhanced Integration and Automation

- **Copy Job Activity in Pipelines**: Orchestrate data movement end-to-end directly within Fabric Data Factory pipelines.
- **Connection Parameterization**: Use variable libraries for smooth CI/CD deployments across dev, test, and production.
- **Multiple Schedules and Formats**: Run jobs on custom timelines and use new file formats like Iceberg and JSON.
- **Database Views as Sources**: Enable complex ingestion from database views for both full and incremental loads.

### Enterprise-Grade Connectivity

- **Over 170 Connectors**: Integrate with a rapidly growing set of data sources and destinations.
- **VNet and On-Premises Data Gateways**: Ensure secure transfer across private networks and cloud environments.
- **Dataflow Gen2 and Pipelines Support**: Power reliable integrations and advanced workflows for modern data estates.

### Resources & Learn More

- [Copy Job Activity](https://learn.microsoft.com/fabric/data-factory/copy-job-activity)
- [Connection Parameterization via Variable Library](https://aka.ms/CopyJobFabConEU25)
- [Fabric Lakehouse Change Data Feeds (CDF)](https://aka.ms/CopyJobFabConEU25)
- [CDC Merge into SQL Database & Snowflake](https://aka.ms/CopyJobFabConEU25)
- [Copy Assistant in Pipeline Powered by Copy Job](https://aka.ms/CopyJobFabConEU25)
- [VNet Gateway Support in Copy Job & Copy Activity](https://aka.ms/CopyJobGWFabConEU25)
- [Incremental Copy Reset](https://learn.microsoft.com/fabric/data-factory/what-is-copy-job#reset-incremental-copy)
- [Iceberg & JSON File Format Support](https://blog.fabric.microsoft.com/blog/simplifying-data-ingestion-with-copy-job-reset-incremental-copy-auto-table-creation-and-json-format-support)
- [Multiple Schedules Support](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-multiple-scheduler-support/)
- [Database Views Support](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-copy-data-from-database-view-sample-dataset-and-new-connectors/)

Explore further:

- [Fabric Data Factory Overview](https://learn.microsoft.com/fabric/data-factory/data-factory-overview)
- [Fabric Roadmap](https://aka.ms/Fabric-Roadmap)
- [Fabric Community](https://aka.ms/fabric-community)

## Conclusion

With these latest updates, Fabric Data Factory greatly advances secure, large-scale data integration and movement across clouds and on-premises, empowering organizations to modernize data architecture, accelerate insight, and remain future-ready.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/cross-cloud-data-movement-with-best-in-class-connectivity-whats-new-and-whats-next/)
