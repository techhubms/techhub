---
layout: "post"
title: "What's New in Fabric Eventstream: July–December 2025 Updates"
description: "This news post from the Microsoft Fabric Blog highlights the latest advancements in Fabric Real-Time Intelligence and Eventstream between July and December 2025. Key updates include new data connectors, improved real-time stream processing, enhanced schema management, and robust security features. The article showcases both technical features and practical application scenarios for real-time operations analytics in Microsoft Fabric."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/whats-new-in-fabric-eventstream-july-december-2025-updates/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2026-02-04 10:00:00 +00:00
permalink: "/2026-02-04-Whats-New-in-Fabric-Eventstream-JulyDecember-2025-Updates.html"
categories: ["Azure", "ML"]
tags: ["Azure", "Azure Monitor", "Cribl Integration", "Data Activator", "Data Engineering", "Data Pipelines", "Eventhouse", "Eventstream", "Microsoft Fabric", "ML", "MongoDB CDC", "News", "Operational Analytics", "Private Link", "Real Time Analytics", "Real Time Intelligence", "Schema Registry", "SQL Operator", "Streaming Data"]
tags_normalized: ["azure", "azure monitor", "cribl integration", "data activator", "data engineering", "data pipelines", "eventhouse", "eventstream", "microsoft fabric", "ml", "mongodb cdc", "news", "operational analytics", "private link", "real time analytics", "real time intelligence", "schema registry", "sql operator", "streaming data"]
---

Microsoft Fabric Blog details the newest advancements in Fabric Eventstream for July–December 2025, covering technical innovations in real-time analytics, robust stream data ingestion, processing, and schema management.<!--excerpt_end-->

# What's New in Fabric Eventstream: July–December 2025 Updates

**Source: Microsoft Fabric Blog**

Fabric Real-Time Intelligence and Eventstream have seen a significant series of updates aimed at making streaming data ingestion, processing, and operationalization easier and more scalable.

## Key Innovations Delivered July–December 2025

### Expanded Data Ingestion: More Connectors

- **HTTP Connector** (Preview): No-code streaming of REST/HTTP data feeds with built-in support for public data sources.
  - [Eventstream HTTP connector documentation](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/add-source-http)
  - [Introducing the HTTP and MongoDB CDC Connectors Blog](https://blog.fabric.microsoft.com/blog/introducing-the-http-and-mongodb-cdc-connectors-for-eventstream-inspired-by-you/)
- **Real-Time Weather Enhancements**: Assign friendly location names for easier analytics.
  - [Add Real-time weather source documentation](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/add-source-real-time-weather)
- **Capacity Overview Events** (Preview): Route Fabric usage and health metrics into Eventstream for real-time and predictive monitoring.
  - [Capacity Overview Events documentation](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/add-source-fabric-capacity-overview-events)
  - [Fabric Capacity Events Blog](https://blog.fabric.microsoft.com/blog/fabric-capacity-events-in-real-time-hub-preview/)
- **MongoDB CDC Connector with Snapshot Support** (Preview): Enables full streaming pipelines from MongoDB, including snapshot and CDC data.
  - [MongoDB CDC documentation](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/add-source-mongodb-change-data-capture)
- **Cribl Integration** (Preview): Direct onboarding from Cribl event feeds.
  - [Cribl Integration documentation](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/add-source-cribl?tabs=oauthbearer)
- **Azure Monitor Diagnostic Logs**: Stream Azure monitoring data into Fabric for analytics/alerting.
  - [Azure Diagnostics documentation](https://learn.microsoft.com/fabric/real-time-hub/add-source-azure-diagnostic-logs-metrics)

### Advanced Stream Processing and Schema Management

- **SQL Operator** (Preview): Code-first data transformation using SQL in Eventstream pipelines, with IntelliSense and query previews.
  - [Process Events Using SQL Operator](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/process-events-using-sql-code-editor)
- **Eventstream Activator Destination**: Improved ability to trigger automated actions based on real-time streaming insights.
  - [Activator Destination documentation](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/add-destination-activator)
- **Schema Registry** (Preview): Centralized schema discovery, governance, and enforcement for event data pipelines. Helps mitigate schema drift and ensures type-safe operations.
  - [Schema Registry Documentation](https://learn.microsoft.com/fabric/real-time-intelligence/schema-sets/schema-registry-overview)
- **Multiple Schema Inferencing**: Support for multiple schema versions within a single Eventstream, enabling ingestion of evolving event data from multiple producers.
  - [Multiple Schema Inferencing documentation](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/process-events-with-multiple-schemas)
- **Confluent Schema Registry Support**: Stream and deserialize Apache Kafka data with Confluent schemas for consistency between systems.
  - [Confluent Kafka documentation](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/add-source-confluent-kafka)

### Security, Control, and Operational Enhancements

- **Workspace Private Link (Preview):** Secure data routing within private networks, reducing exposure to the public internet.
  - [Workspace Private Link documentation](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/set-up-tenant-workspace-private-links)
- **Pause/Resume Derived Streams:** Provides operational flexibility for cost optimization and safer development workflows.
  - [Pause and Resume Data Streams documentation](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/pause-resume-data-streams)

## Real-Time Operations Scenario

A practical example illustrates streaming Azure Monitor diagnostic logs and Capacity Overview events, processing and joining multiple sources (MongoDB CDC, REST APIs, Cribl), employing SQL operators, schema registries, and enforcing control/security with Private Links and stream management tools, all in a unified analytics and alerting solution within Microsoft Fabric.

## Call to Action

- Try Microsoft Fabric with a [free trial](https://aka.ms/try-fabric)
- Ask questions via [askeventstreams@microsoft.com](mailto:askeventstreams@microsoft.com)
- Share feedback on [Fabric Ideas](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas) or join the [Fabric Community](https://aka.ms/FabricBlog/Community)

These advancements continue to streamline real-time analytics, providing data engineers and business analysts with robust, enterprise-ready tools for modern operational intelligence.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/whats-new-in-fabric-eventstream-july-december-2025-updates/)
