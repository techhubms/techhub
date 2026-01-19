---
external_url: https://blog.fabric.microsoft.com/en-US/blog/understanding-fabric-eventstream-pricing/
title: Understanding Fabric Eventstream Pricing
author: Microsoft Fabric Blog
viewing_mode: external
feed_name: Microsoft Fabric Blog
date: 2026-01-12 14:00:00 +00:00
tags:
- Azure Data Services
- Capacity Metrics
- Cost Estimation
- CU Consumption
- Data Ingestion
- Destinations
- Event Processing
- Eventstream
- Fabric Capacity Estimator
- Microsoft Fabric
- Operators
- Pricing
- Real Time Analytics
- Streams
section_names:
- azure
- ml
---
Microsoft Fabric Blog provides an in-depth guide to understanding Eventstream pricing in Microsoft Fabric, helping technical users estimate costs and optimize architecture.<!--excerpt_end-->

# Understanding Fabric Eventstream Pricing

This guide from the Microsoft Fabric Blog breaks down the pricing structure for Eventstream, part of the Microsoft Fabric real-time analytics platform. It aims to give practitioners clarity and confidence about capacity-unit (CU) consumption and cost planning.

## Key Learning Objectives

- Understand how Eventstream components influence cost
- Map end-to-end data flow to operation-based billing meters
- Calculate potential costs for typical usage scenarios

## Eventstream Components & Billing Meters

Eventstream pricing is determined by several core components:

1. **Input Sources**: Data sources ingested into the event stream. Example sources:
    - Azure SQL DB, Cosmos DB, Service Bus (as connectors, charged per vCore hour)
    - Azure Event Hubs, Azure IoT Hub (charged as processors per hour)
2. **Streams**: Data flows including default and derived streams. These are metered per gigabyte (GB) for both ingress and egress data volume.
3. **Operators**: Processing logic applied through no-code or SQL. Metered per hour based on compute used for each processing route.
4. **Destinations**: Output endpoints like Lakehouse or Eventhouse, with costs varying for push (processing charged) and pull (no processor charge) destinations.

### Billing Operation Types

Each component maps to one or more of four billing operation types:

| Component                  | Operation Type                           | Unit        | Rate Example           |
|--------------------------- |----------------------------------------- |------------ |----------------------- |
| Eventstream Service        | Eventstream Per Hour                     | Per hour    | 0.222 CU hours        |
| Streams (default/derived)  | Data Traffic Per GB                      | Per GB      | 0.342 CU hours        |
| Operators/processes/source | Processor Per Hour                       | Per hour    | 0.778 CU hours+ autoscale|
| Connector sources          | Connectors Per vCore Hour                | Per hour    | 0.611 CU/vCore hour   |

See [Microsoft documentation](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/monitor-capacity-consumption#operation-types) for further details.

## Illustrative Scenarios

### Scenario 1: Custom endpoint to Eventhouse, no processing

- Source: Custom endpoint (no connector or processing charges)
- Streams: Ingest/egress of 1 GB per day (~0.04 GB/hour)
- Metered Charges: Eventstream Per Hour + Data Traffic Per GB
- **Result:** 0.25 CU/hour (F2 recommended)

### Scenario 2: Connector source, operators, two destinations

- Source: Azure SQL CDC (connector charge)
- Operators: Two processing routes (filters/manipulations)
- Destinations: Eventhouse with pre-processing
- **Result:** 2.42 CU/hour (F4 recommended)

### Scenario 3: Aggregation with scaling traffic

- Common setup, data volume varies
    - 3A: 1 GB/hour → 2.28 CU/hour (F4)
    - 3B: 10 GB/hour → 10.00 CU/hour (F16)
- Processing and data egress drive consumption/cost

## Factors Impacting Cost

- Input source type and #
- Number/sophistication of operators
- Number and type of destinations
- Data volume (both ingress and egress)
- Uptime and chosen throughput

## Practical Cost Management Tips

- Use the [Fabric Capacity Estimator](https://www.microsoft.com/microsoft-fabric/capacity-estimator) for initial planning
- Observe real workloads periodically for accurate baselines
- Consider all concurrent Fabric workloads since capacity is shared

## Feedback and Resources

For detailed documentation, see official [Microsoft Eventstream Pricing Docs](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/monitor-capacity-consumption).

Feedback can be sent to [askeventstreams@microsoft.com](mailto:askeventstreams@microsoft.com), via [Fabric Ideas](https://aka.ms/FabricBlog/ideas), or in the [Fabric Community](https://aka.ms/FabricBlog/Community).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/understanding-fabric-eventstream-pricing/)
