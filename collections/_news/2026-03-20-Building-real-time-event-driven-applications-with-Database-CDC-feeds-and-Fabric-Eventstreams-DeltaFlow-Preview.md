---
author: Microsoft Fabric Blog
title: Building real-time, event-driven applications with Database CDC feeds and Fabric Eventstreams DeltaFlow (Preview)
date: 2026-03-20 10:40:00 +00:00
external_url: https://blog.fabric.microsoft.com/en-US/blog/building-real-time-event-driven-applications-with-database-cdc-feeds-and-fabric-eventstreams-deltaflow-preview/
primary_section: ml
section_names:
- azure
- ml
feed_name: Microsoft Fabric Blog
tags:
- Anomaly Detection
- Azure
- Azure SQL Database
- Change Data Capture
- Debezium
- DeltaFlow
- Event Driven Architecture
- Eventhouse
- Fabric Eventstreams
- KQL
- Microsoft Fabric
- ML
- News
- Operational Data
- PostgreSQL
- Preview
- Real Time Dashboards
- Real Time Intelligence
- Schema Registry
- SQL Managed Instance
- SQL On Azure VMs
- Streaming Data
---

Microsoft Fabric Blog introduces DeltaFlow (Preview) in Fabric Eventstreams, a managed way to ingest CDC from operational databases and produce analytics-ready streaming tables for real-time dashboards, alerts, and event-driven applications.<!--excerpt_end-->

# Building real-time, event-driven applications with Database CDC feeds and Fabric Eventstreams DeltaFlow (Preview)

*If you haven’t already, check out Arun Ulag’s hero blog “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for a complete look at all of our FabCon and SQLCon announcements across both Fabric and our database offerings.*

Coauthored by Alicia Li

## Real-time starts with your operational data

Modern business applications often need to react immediately—serving recommendations as users interact, alerting teams when anomalies occur, or updating dashboards as soon as business events happen. These scenarios start with **operational databases**, where every insert, update, or delete can represent a meaningful event.

Historically, converting operational database changes into real-time events has been complex. Teams typically had to:

- Stitch together **Change Data Capture (CDC)** connectors, message brokers, and stream processors
- Understand low-level **Debezium** semantics
- Write custom code to transform raw change logs into data shapes that analysts can use

## Introducing DeltaFlow (Preview)

**Fabric Eventstreams** with **DeltaFlow (Preview)** provides a managed path from database change feeds to **analytics-ready streaming data** in **Microsoft Fabric**.

DeltaFlow:

- Natively ingests CDC events from operational databases
- Transforms them into a **structured, query-able shape that mirrors the source tables**
- Lets teams focus on real-time apps rather than “CDC plumbing”

With DeltaFlow, Eventstreams handles:

- Raw CDC ingestion
- Schema registration
- Transformation
- Destination table management

This removes the need to reason about Debezium payloads, variable schemas, or manual table lifecycle management—while preserving the fidelity required for real-time analytics and automation.

### Transform raw Debezium events into analytics-ready form

You can connect to operational databases using the connector wizard, including:

- **Azure SQL**
- **SQL Managed Instance (MI)**
- **SQL on VMs**
- **PostgreSQL**

In the wizard, select **“Analytics-ready events & auto-updated schema”** during schema handling, choose where to store table schemas, and proceed.

![Animated screenshot of Eventstreams wizard that enables users to connect to a CDC enabled Azure SQL database, enable DeltaFlow and ingest transformed events into an Eventstream.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/animated-screenshot-of-eventstreams-wizard-that-en.gif)

*Figure 1: Enabling DeltaFlow when connecting to an Azure SQL database.*

### Automatically manage destination tables as source database schema evolves

As applications evolve, new tables get added and existing table schemas change. DeltaFlow (where supported by the source database):

- Detects schema changes
- Registers new schemas
- Creates new destination tables automatically

This reduces interruptions and overhead typically caused by manual schema and destination-table management.

![Animated screenshot of a Eventstream showing the source table schemas automatically fetched and registered in the schema registry. The registered schemas are associated with the default stream and then used when configuring the Eventshouse destination.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/animated-screenshot-of-a-eventstream-showing-the-s.gif)

*Figure 2: View source table schemas automatically fetched and registered in the schema registry.*

### Build real-time applications and dashboards using simple analytics queries

DeltaFlow outputs streaming data that:

- Reflects the shape of the source tables
- Adds essential metadata like change type (**insert**, **update**, **delete**) and timestamps

This allows developers and analysts to write straightforward analytics queries using tools like **KQL**, without needing to parse nested JSON payloads or learn CDC-specific semantics.

![Screenshot of JSON object showing transaction details for a sale event, including TransactionID, Timestamp, StoreID, ProductID, Quantity, and TotalAmount. The object also includes source metadata with connector metadata, source database and table information.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-json-object-showing-transaction-deta.png)

*Figure 3: Raw Debezium CDC event*

The result is an easier path from operational data to real-time intelligence—supporting dashboards, alerts, and event-driven workflows.

![Animated screenshot showing destination Eventhouse tables automatically created as part of DeltaFlow. These tables reflect the original shape of the source database tables along with some CDC metadata.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/animated-screenshot-showing-destination-eventhouse.gif)

*Figure 4: Eventhouse destination tables in the transformed, analytics-ready shape.*

## End-to-end example: Reacting to database changes in real time

In an e-commerce application, each order update, purchase, cancellation, or status change is written to an operational database.

With DeltaFlow:

- Database changes are continuously captured
- Changes become streaming events in Fabric Eventstreams
- Streams mirror the order table schema

Those streams can then be routed to multiple consumers at once, such as:

- A real-time dashboard tracking order volume and fulfillment latency
- Alerting logic for unusual spikes or failures
- Downstream analytics systems that continuously aggregate operational metrics

Because the streaming data mirrors the source tables, teams can evolve their real-time apps with familiar analytics queries and without reworking pipelines whenever schemas change.

## Get started today

DeltaFlow (Preview) is available via Fabric Eventstreams connectors such as:

- Azure SQL
- SQL Managed Instance (MI)
- SQL on VM
- PostgreSQL

Learn more:

- [Microsoft Fabric Eventstreams Overview](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/overview?tabs=enhancedcapabilities)
- [Add Azure SQL Database CDC source to an eventstream](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/add-source-azure-sql-database-change-data-capture?pivots=extended-features)
- [Schema Registry in Fabric Real-Time Intelligence](https://learn.microsoft.com/fabric/real-time-intelligence/schema-sets/schema-registry-overview)
- [Real-Time Intelligence in Microsoft Fabric documentation](https://learn.microsoft.com/fabric/real-time-intelligence/)

## We’d love your feedback!

If you find this blog helpful, please give it a thumbs-up.

Have ideas for what you’d like to see next? Drop a comment or reach out to askeventstreams@microsoft.com.

Do you have new feature ideas? Head over to the [Fabric Ideas](https://aka.ms/fabricideas) to submit them.


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/building-real-time-event-driven-applications-with-database-cdc-feeds-and-fabric-eventstreams-deltaflow-preview/)

