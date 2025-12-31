---
layout: "post"
title: "Entity Diagram in Eventhouse KQL Database (Preview)"
description: "This post introduces the Entity Diagram feature for Eventhouse KQL Database in Microsoft Fabric, offering a visual map of your database. It explains how the diagram displays connections between tables, functions, materialized views, update policies, and event streams, and highlights practical benefits like spotting schema violations and understanding ingestion flows."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/entity-diagram-in-eventhouse-kql-database-preview/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-11-03 09:00:00 +00:00
permalink: "/news/2025-11-03-Entity-Diagram-in-Eventhouse-KQL-Database-Preview.html"
categories: ["Azure", "ML"]
tags: ["Azure", "Cross Database Relationships", "Data Engineering", "Data Flow", "Database Development", "Entity Diagram", "Eventhouse", "Eventstreams", "Ingestion Monitoring", "KQL Database", "Materialized Views", "Microsoft Fabric", "ML", "News", "Real Time Analytics", "Schema Validation", "Update Policies", "Visualization Tools"]
tags_normalized: ["azure", "cross database relationships", "data engineering", "data flow", "database development", "entity diagram", "eventhouse", "eventstreams", "ingestion monitoring", "kql database", "materialized views", "microsoft fabric", "ml", "news", "real time analytics", "schema validation", "update policies", "visualization tools"]
---

Microsoft Fabric Blog introduces the Entity Diagram for Eventhouse KQL Database, providing developers and data engineers with a visual way to explore data flows, entity connections, and schema integrity within their KQL databases.<!--excerpt_end-->

# Entity Diagram in Eventhouse KQL Database (Preview)

As KQL databases grow in complexity, managing and understanding relationships between various entities—such as tables, functions, materialized views, update policies, and event streams—can become difficult. The new **Entity Diagram** feature in Microsoft Fabric's Eventhouse KQL Database aims to resolve this by offering a comprehensive visual overview of your database structure and data flow.

## What is the Entity Diagram?

The Entity Diagram shows how your database's components interact:

- **Tables**: Visualize tables and understand which sources (event streams, update policies) populate them.
- **Functions**: See how functions relate to tables and other entities.
- **Materialized Views & Update Policies**: Track how data is aggregated and transformed.
- **Shortcuts & Continuous Exports**: Visualize cross-database relationships and data exports.

You can instantly view which tables are connected to which event sources, and how data moves through update policies, aggregations, and exports.

## Key Features

- **Visual Relationships**: Get a map of all database entities and their dependencies.
- **Ingestion Details**: Check the number of records ingested by each table or materialized view, and jump to event streams.
- **Schema Violation Detection**: The diagram flags broken references (e.g., missing tables/functions or outdated update policies), making it easy to identify and address inconsistencies in your ETL flows.
- **Exploration and Troubleshooting**: Click on nodes for more detail, follow dependencies, and get the full lineage of data from source to destination.

## Who is it for?

The Entity Diagram is useful for developers, data engineers, and analysts who want a clear picture of their KQL database's architecture, as well as insight into operational issues (such as schema or reference violations).

## Resources

- [View an entity diagram in KQL database (preview) - Microsoft Docs](https://learn.microsoft.com/fabric/real-time-intelligence/database-entity-diagram)
- [Entity Diagram in Eventhouse KQL Database (Preview) - Fabric Blog](https://blog.fabric.microsoft.com/en-us/blog/entity-diagram-in-eventhouse-kql-database-preview/)

By offering a comprehensive visual interface, the Entity Diagram in Eventhouse KQL Database enables teams to build, debug, and evolve their data solutions with greater confidence and efficiency.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/entity-diagram-in-eventhouse-kql-database-preview/)
