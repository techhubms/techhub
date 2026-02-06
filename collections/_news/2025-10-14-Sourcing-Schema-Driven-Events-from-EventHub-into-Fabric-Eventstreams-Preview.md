---
external_url: https://blog.fabric.microsoft.com/en-US/blog/sourcing-schema-driven-events-from-eventhub-into-fabric-eventstreams-preview/
title: Sourcing Schema-Driven Events from EventHub into Fabric Eventstreams (Preview)
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-10-14 10:00:00 +00:00
tags:
- Analytics
- Avro Schema
- Azure EventHub
- Data Governance
- Data Quality
- Event Processing
- Eventhouse
- Eventstream
- KQL
- Microsoft Fabric
- Payload Modeling
- Pipeline Validation
- Real Time Intelligence
- Schema Registry
- Type Safety
- Azure
- ML
- News
- Machine Learning
section_names:
- azure
- ml
primary_section: ml
---
Microsoft Fabric Blog demonstrates how schema-driven pipelines, using Azure EventHub's integration with Schema Registry, boost event data reliability and analytics quality. Learn from practical examples and guidance provided by the Fabric platform team.<!--excerpt_end-->

# Sourcing Schema-Driven Events from EventHub into Fabric Eventstreams (Preview)

**Author: Microsoft Fabric Blog**

## Overview

This article explores the preview integration of Azure EventHub with Schema Registry in Microsoft Fabric Real-Time Intelligence. It covers how schema-driven event processing delivers type safety, data quality, and reliability for real-time analytics.

## Why EventHub + Schema Registry?

Azure EventHub is critical for high-throughput event streaming from sources like IoT devices and microservices. Managing data consistency and quality becomes difficult due to varying event structures and potential schema drift. By integrating with Schema Registry, Fabric's Eventstreams now provide:

- **Event Contracts** ensuring clear data agreements
- **Early Validation** of event payloads
- **Self-Documenting Pipelines** and type safety
- **Centralized Governance** via schema management

## Example: The Power of Payload Modeling

### Before Schema Registry

- No enforced structure; inconsistent or malformed events (e.g., missing fields, wrong data types) enter pipelines
- Failures in downstream KQL queries and analytics tables
- Debugging and maintaining pipelines is difficult

### After Schema Registry

- Define Avro schemas that specify valid structure and types
- Only events conforming to schema are accepted (others are rejected early)
- Fabric Diagnostics logs malformed events; analytics remain clean and predictable

#### Sample Avro Schema

```json
{
  "type": "record",
  "name": "BaggageCheckinEventData",
  "namespace": "Airport.Baggage",
  "fields": [
    {"name": "bagId", "type": "string"},
    {"name": "weightKg", "type": "double"},
    {"name": "flightId", "type": "string"},
    {"name": "paxId", "type": "string"}
  ]
}
```

## Key Benefits

- **Data Quality**: All ingested events match defined schemas
- **Development Speed**: No ambiguity about fields or types
- **Production Confidence**: Schema validation prevents breaking changes
- **Governance & Reliability**: Malformed data is filtered before entering analytics

## Roadmap: What's Coming Next?

Fabric team is working on:

- Failed event retry and reprocessing
- Schema inference from sample data
- Complex multi-header routing for advanced mapping scenarios

Community input is welcome, and readers are invited to contribute feedback via links provided.

## Getting Started

To begin mapping schemas to your EventHub sources, reference these resources:

- [Event Schema Registry Documentation](https://learn.microsoft.com/fabric/real-time-intelligence/schema-sets/schema-registry-overview)
- [Add EventHub as a source](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/add-source-azure-event-hubs?branch=pr-en-us-10586&pivots=extended-features)

## Conclusion

Schema-driven pipelines in Fabric Real-Time Intelligence, powered by Azure EventHub and Schema Registry, provide scalable, reliable, and managed real-time data streaming. Organizations achieve better analytics performance and governance by enforcing data contracts at ingestion.

*Share feedback, questions, and scenarios with the Fabric Real-Time Intelligence team to help shape future developments.*

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/sourcing-schema-driven-events-from-eventhub-into-fabric-eventstreams-preview/)
