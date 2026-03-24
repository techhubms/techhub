---
external_url: https://blog.fabric.microsoft.com/en-US/blog/whats-new-with-fabric-activator-more-connected-and-capabilities/
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
tags:
- Alerting
- Apache Spark
- Automation Rules
- Azure Blob Storage Events
- Business Events
- Data Warehouse
- Dataflows Gen2
- Event Data
- Event Driven Architecture
- Eventstream
- Fabric Activator
- Microsoft Fabric
- ML
- News
- Ontology
- Operational Monitoring
- Power BI Alerts
- Real Time Intelligence
- Spark Jobs
- SQL Queries
- Streaming Data
- Table Visual Alerts
- User Data Functions (udf)
primary_section: ml
section_names:
- ml
date: 2026-03-24 11:30:00 +00:00
title: 'What’s new with Fabric Activator: more connected and capabilities'
---

Microsoft Fabric Blog (co-authored with James Hutton) summarizes new Fabric Activator capabilities, focusing on expanded rule actions (Teams notifications, Spark jobs, UDFs, Dataflows) and tighter integration across Eventstream, Warehouse SQL, Business Events, Ontology, and Power BI for near-real-time alerting and automation.<!--excerpt_end-->

# What’s new with Fabric Activator: more connected and capabilities

*If you haven’t already, check out Arun Ulag’s hero blog “[FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform](https://aka.ms/FabCon-SQLCon-2026-news)” for a complete look at all of the FabCon and SQLCon announcements across both Fabric and Microsoft’s database offerings.*

Co-author: James Hutton

In a data-centric environment where immediate responses matter, **Fabric Activator** for **Microsoft Fabric** is positioned as a way to move from passive monitoring to **proactive action**. It continuously monitors **streaming and event data** and can **execute actions or send alerts** when specific conditions are met—without requiring complex code or custom infrastructure.

## What’s new

### More actions for Activator rules

Activator rules can now trigger additional actions, including:

- Send Teams message to **Group chat**
- Send Teams message to **Teams channel**
- **Run Spark job**
- **Run User Data Function (UDF)**
- **Run Dataflow**

![Screenshot showing Fabric Activator actions (notifications, activities, actions)](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/this-image-shows-all-available-actions-in-activato.png)

*Figure 1: Actions available for automations in Fabric Activator.*

### Trigger User Data Functions (UDFs) from Activator rules

Running **User Data Functions (UDFs)** from Activator rules enables fast, programmatic responses when issues occur.

Example scenarios mentioned:

- Automatically create a **support ticket**, **incident**, or **work item** when a threshold is exceeded
- Pass context such as:
  - entity IDs
  - measured values
  - timestamps
- Trigger predefined operational actions such as:
  - restarting a service
  - adjusting configuration
  - invoking internal automation/runbooks

Learn more: [Running User Data Functions (UDFs)](https://learn.microsoft.com/fabric/data-engineering/user-data-functions/user-data-functions-overview)

### Trigger Spark jobs and Dataflows for fresher data

Activator can trigger **Spark jobs** and **Dataflows** in response to Fabric events and **Azure Blob Storage events** emitted when data changes. The goal is to reduce reliance on scheduled refreshes and enable more responsive, event-driven data pipelines.

Learn more:

- [Spark jobs](https://learn.microsoft.com/fabric/data-engineering/spark-job-definition)
- [Dataflows Gen2](https://learn.microsoft.com/fabric/data-factory/dataflows-gen2-overview)

## New rule scenarios across Fabric experiences

### Rules for monitoring Data Warehouse signals via SQL queries

You can create configurable rules (Preview) to monitor **Data Warehouse** health and business-critical signals in real time.

Capabilities described:

- Create rules on **ad-hoc or saved SQL queries**
- Define evaluation frequency
- Set rule conditions based on query results

Learn more: [Create an alert rule on a Fabric Warehouse SQL query](https://aka.ms/DWrules)

![Screenshot showing rule configuration inside Data Warehouse](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/this-image-shows-how-you-can-configure-rules-insid.gif)

*Figure 2: Configure rules in Data Warehouse.*

### Business Events “set alert”

**Business Events** are described as capturing critical business moments and enabling immediate actions. With **Set alert** in Business Events, you can alert and automate business processes and actions.

Learn more: [Consume business events from Activator](https://aka.ms/BusinessEventsSetAlert)

![Screenshot showing “set alert” in Business Events (Real-Time hub)](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/this-image-shows-how-you-can-set-alert-in-business.png)

*Figure 3: Alert and automate actions in Business Events.*

### Rules on business entities in Ontology (preview)

You can define **conditions and actions** for **business entities** in **Ontology**. The post describes this as moving ontology from static models to operationalized ones, capable of initiating business processes via alerts (preview) and automated actions.

Learn more:

- [Ontology overview](https://learn.microsoft.com/fabric/iq/ontology/overview)
- [Rules in ontology (preview) (with Fabric Activator)](https://aka.ms/OntologyAddRule)

![Screenshot showing rule configuration and management in Ontology](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/this-image-shows-how-you-can-configure-and-manage.png)

*Figure 4: Configure rules on entities in Ontology.*

### Add/manage rules inside an Eventstream

Alert creation is now embedded directly into **Eventstream**, removing the need to switch between Eventstream and Activator to complete the workflow.

Learn more: [Add a Fabric Activator destination to an eventstream](https://aka.ms/EventstreamAddRule)

![Screenshot showing setting alerts and managing them inside Eventstream](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/this-image-shows-how-you-can-set-alert-and-manage.png)

*Figure 5: Set alert inside Eventstream.*

![Screenshot showing managing rules inside Eventstream](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/this-image-shows-how-to-manage-rules-inside-events.png)

*Figure 6: Manage rules inside Eventstream.*

### Improved Power BI integration: alerts on new table rows

Activator’s integration with **Power BI** now supports a commonly requested scenario: **notify when a new row appears in a table visual**.

The post describes examples like tracking:

- customer complaints
- support tickets
- software bugs

How to try it:

- In a **published Power BI report**, select **“Set Alert”** on a **table visual**.

Learn more: [Create an alert in a Power BI report](https://learn.microsoft.com/fabric/real-time-intelligence/data-activator/activator-get-data-power-bi)

![Screenshot showing new alerting scenario in Power BI](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/this-image-shows-new-alerting-on-power-bi.png)

*Figure 7: New alerting scenario in Power BI.*

## Share your feedback

- Try the features: [Fabric](https://aka.ms/EventstreamAddRule)
- Join the discussion: [Activator community](https://learn.microsoft.com/fabric/data-factory/dataflows-gen2-overview)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/whats-new-with-fabric-activator-more-connected-and-capabilities/)

