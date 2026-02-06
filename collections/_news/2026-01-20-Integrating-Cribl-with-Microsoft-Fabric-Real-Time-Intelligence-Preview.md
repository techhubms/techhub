---
external_url: https://blog.fabric.microsoft.com/en-US/blog/expanding-real-time-intelligence-data-sources-with-cribl-source-preview/
title: Integrating Cribl with Microsoft Fabric Real-Time Intelligence (Preview)
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2026-01-20 13:00:00 +00:00
tags:
- Cloud Analytics
- Cribl
- Data Activator
- Data Ingestion
- Data Integration
- Event Driven Architecture
- Eventhouse
- Eventstream
- Kafka Integration
- Kusto Query Language
- Microsoft Fabric
- Real Time Dashboard
- Real Time Intelligence
- Stream Analytics
- Telemetry Data
- Azure
- ML
- News
- Machine Learning
section_names:
- azure
- ml
primary_section: ml
---
This news post from the Microsoft Fabric Blog explains how organizations can use the new Cribl source integration (Preview) to easily collect and analyze real-time data in Microsoft Fabric Real-Time Intelligence.<!--excerpt_end-->

# Integrating Cribl with Microsoft Fabric Real-Time Intelligence (Preview)

Microsoft Fabric has announced the preview of Cribl source integration in Real-Time Intelligence (RTI). This enhancement allows organizations to ingest a broad range of telemetry data—including logs, metrics, and traces—from cloud services, on-premises, and edge environments directly into Fabric RTI Eventstream via Cribl Stream.

## Overview

The collaboration between Microsoft and Cribl streamlines real-time data integration, empowering users to:

- Configure a dedicated Cribl source type in the Fabric Real-Time Hub
- Leverage Kafka endpoints in Eventstream to receive data from Cribl
- Add Fabric RTI as a destination in Cribl for seamless data pushes

Cribl supports both push and pull ingestion models, allowing connections to various data sources such as Syslog, Splunk, OpenTelemetry, AWS S3, and more. Organizations can now efficiently route real-time data to Microsoft Fabric for processing, analytics, and visualization.

## How It Works

1. **Setup in Fabric**: In the Real-Time Hub, create a new data source, selecting 'Cribl.' This process generates the required Kafka endpoint within Eventstream, complete with details like server address and topic name.

2. **Configure Cribl**: In Cribl Stream, add 'Fabric Real-Time Intelligence' as a destination. Enter the Kafka connection details from Eventstream. QuickConnect in Cribl allows you to link incoming sources (e.g., Syslog) to this destination, enabling data flow directly into Microsoft Fabric.

3. **Eventstream Preview**: Once data is routed, it becomes available in your Fabric Eventstream for further processing and visualization.

## Analyzing Real-Time Data

With Cribl-sourced data in Fabric, users can:

- Run Kusto Query Language (KQL) queries using Eventhouse
- Export queries as visuals to Real-Time Dashboards
- Set up real-time notifications and alerts with Activator
- Transform data with Derived Stream for further use
- Connect custom endpoints for event-driven apps

## Getting Started

- Cribl source for Fabric RTI is in preview and available for all Fabric users.
- New users can start with a [Microsoft 365 or Power BI trial](https://learn.microsoft.com/power-bi/enterprise/service-admin-signing-up-for-power-bi-with-a-new-office-365-trial) and a free [Fabric trial capacity](https://learn.microsoft.com/fabric/fundamentals/fabric-trial).
- Additional documentation and community resources are available ([Cribl source documentation](https://aka.ms/criblsrcdoc)).

## Feedback

Feedback is encouraged through the [community forum](https://aka.ms/realtimecommunity), [idea submissions](https://aka.ms/realtimeideas), or direct [email contact](mailto:askeventstreams@microsoft.com).

---

By leveraging the new Cribl integration, organizations gain broader coverage of data sources and benefit from Microsoft Fabric's robust real-time analytics platform for improved decision-making and event-driven architectures.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/expanding-real-time-intelligence-data-sources-with-cribl-source-preview/)
