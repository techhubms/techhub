---
external_url: https://blog.fabric.microsoft.com/en-US/blog/introducing-the-http-and-mongodb-cdc-connectors-for-eventstream-inspired-by-you/
title: Introducing HTTP and MongoDB CDC Connectors for Fabric Eventstream
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-11-21 12:00:00 +00:00
tags:
- Change Data Capture
- Community Feedback
- Dashboard
- Data Ingestion
- Data Pipeline
- Eventstream
- Fabric Ideas
- HTTP Connector
- Microsoft Fabric
- MongoDB Atlas
- MongoDB CDC
- No Code Integration
- Public APIs
- Real Time Analytics
- Real Time Intelligence
- Snapshot Mode
- Streaming Data
- Azure
- Machine Learning
- News
section_names:
- azure
- ml
primary_section: ml
---
Microsoft Fabric Blog announces new HTTP and MongoDB CDC connectors for Eventstream, enabling users to easily integrate and analyze real-time data within Fabric RTI.<!--excerpt_end-->

# Introducing HTTP and MongoDB CDC Connectors for Fabric Eventstream

Microsoft Fabric Real-Time Intelligence (RTI) expands its Eventstream platform with two powerful data connectors: HTTP and MongoDB Change Data Capture (CDC).

## HTTP Connector

- Provides a no-code, configurable solution to stream data from external platforms into Eventstream using standard HTTP GET or POST requests.
- Supports direct streaming of real-time data (such as financial stock prices from providers like Yahoo Finance) for dashboards and anomaly detection in Fabric RTI.
- Allows users to select from predefined public data feeds. After entering an API key, Eventstream handles HTTP headers and parameters automatically.
- Reduces development overhead, supporting scalable and reliable real-time data ingestion.
- [Learn more in the Microsoft documentation](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/add-source-http).

## MongoDB CDC Connector

- Streams real-time change events from any MongoDB source, including MongoDB Atlas and self-managed databases, directly into Eventstream.
- Ensures dashboards and analytics update as soon as data changes, keeping business insights timely.
- Supports Snapshot mode, letting users capture an initial snapshot of data collections before initiating real-time streaming for a complete and consistent view from day one.
- [Setup guide available here](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/add-source-mongodb-change-data-capture).

## Community-Driven Development

- Both connectors were developed based on feedback from users through Fabric Ideas and surveys.
- Community input shapes the Eventstream roadmap, with direct feature development influenced by suggestions, voting, and shared insights.

### Get Involved

- Share new ideas, vote on existing features, and provide feedback using [Fabric Ideas on Microsoft Fabric Community](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas/label-name/real-time%20intelligence%20%7C%20eventstream).
- [User survey available here](https://aka.ms/EventStreamSources) for submitting top connector requests.

Microsoft Fabric Eventstream continues to evolve to meet real-time analytics needs thanks to ongoing community input and feedback.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/introducing-the-http-and-mongodb-cdc-connectors-for-eventstream-inspired-by-you/)
