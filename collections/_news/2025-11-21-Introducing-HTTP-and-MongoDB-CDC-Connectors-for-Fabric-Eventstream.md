---
layout: "post"
title: "Introducing HTTP and MongoDB CDC Connectors for Fabric Eventstream"
description: "This news update introduces two new connectors—HTTP and MongoDB CDC—for Microsoft Fabric Eventstream in Real-Time Intelligence (RTI). The HTTP connector allows no-code streaming from public APIs and diverse platforms using GET or POST requests, while the MongoDB CDC connector enables real-time data ingestion and snapshotting from MongoDB databases. Both features are shaped by direct user feedback and community engagement, further enhancing Fabric RTI’s capabilities for streaming, real-time analytics, and up-to-date dashboards."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/introducing-the-http-and-mongodb-cdc-connectors-for-eventstream-inspired-by-you/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-11-21 12:00:00 +00:00
permalink: "/news/2025-11-21-Introducing-HTTP-and-MongoDB-CDC-Connectors-for-Fabric-Eventstream.html"
categories: ["Azure", "ML"]
tags: ["Azure", "Change Data Capture", "Community Feedback", "Dashboard", "Data Ingestion", "Data Pipeline", "Eventstream", "Fabric Ideas", "HTTP Connector", "Microsoft Fabric", "ML", "MongoDB Atlas", "MongoDB CDC", "News", "No Code Integration", "Public APIs", "Real Time Analytics", "Real Time Intelligence", "Snapshot Mode", "Streaming Data"]
tags_normalized: ["azure", "change data capture", "community feedback", "dashboard", "data ingestion", "data pipeline", "eventstream", "fabric ideas", "http connector", "microsoft fabric", "ml", "mongodb atlas", "mongodb cdc", "news", "no code integration", "public apis", "real time analytics", "real time intelligence", "snapshot mode", "streaming data"]
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
