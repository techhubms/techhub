---
layout: "post"
title: "Unlocking Real-Time Operational Intelligence: Azure Monitor Logs Integration in Fabric via Eventstream"
description: "This article introduces a new integration between Azure Monitor Diagnostic Logs and Microsoft Fabric via Eventstream. It explains how organizations can now directly ingest Azure resource logs into Fabric-native analytics workflows, reducing setup complexity and enabling real-time, actionable insights. The guide covers usage scenarios, configuration steps, and the benefits of unified operational monitoring."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/unlocking-real-time-operational-intelligence-azure-monitor-logs-integration-in-fabric-via-eventstream/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-09-22 17:00:00 +00:00
permalink: "/2025-09-22-Unlocking-Real-Time-Operational-Intelligence-Azure-Monitor-Logs-Integration-in-Fabric-via-Eventstream.html"
categories: ["Azure", "ML"]
tags: ["Analytics Workflow", "Azure", "Azure Integration", "Azure Monitor", "Business Intelligence", "Cloud Monitoring", "Data Ingestion", "Data Transformation", "Diagnostic Logs", "Eventhouse", "Eventstream", "Microsoft Fabric", "ML", "Monitoring Logs", "News", "Operational Monitoring", "Real Time Intelligence", "Streaming Analytics"]
tags_normalized: ["analytics workflow", "azure", "azure integration", "azure monitor", "business intelligence", "cloud monitoring", "data ingestion", "data transformation", "diagnostic logs", "eventhouse", "eventstream", "microsoft fabric", "ml", "monitoring logs", "news", "operational monitoring", "real time intelligence", "streaming analytics"]
---

Microsoft Fabric Blog details how the new Azure Monitor Logs to Fabric Eventstream integration helps organizations unify operational intelligence by enabling real-time, code-free data ingestion and analytics.<!--excerpt_end-->

# Unlocking Real-Time Operational Intelligence: Azure Monitor Logs Integration in Fabric via Eventstream

Operational agility relies on accessing insights as they happen. Microsoft Fabric's Real-Time Intelligence (RTI) introduces new capabilities for ingesting, transforming, and analyzing data in real time, empowering organizations to act promptly and make better decisions.

## Integration Overview

The latest integration allows Azure Monitor Diagnostic Logs to be ingested directly into Microsoft Fabric via Eventstream. This automates and simplifies the process of routing operational data from Azure resources, providing:

- **Direct log routing:** Connect Azure Monitor logs to Fabric’s analytics workflows without complex configuration.
- **No-code transformation:** Users can transform logs for analytics and operational use without writing code.
- **Unified visibility:** Combine Azure diagnostics data with other business data already in Fabric for cross-departmental insights.
- **Real-time actions:** Ingest, visualize, and act on monitoring data instantly, all within Fabric’s unified platform.

## How the Integration Works

1. **Navigate to Real-Time Hub:** Within Fabric, access the Real-Time Hub and select the Azure Diagnostics data source card.
2. **Select Azure Resources:** Filter and search for specific Azure resources by type, subscription, group, or region.
3. **Configure Ingestion:** Initiate the end-to-end workflow directly from the chosen Azure resource, selecting logs and metrics for import.
4. **Transform and Route Data:** Use Fabric’s Eventstream to adjust and transform logs, then route them to destinations such as Eventhouse, Activator, or custom endpoints.

## Key Benefits

- Reduce setup time and friction—no Event Hubs or extra authentication steps needed.
- Empower teams to monitor, diagnose, and analyze Azure resources alongside business data in a seamless workflow.
- Achieve a holistic, real-time view for proactive management and rapid operational response.

## Further Reading

For implementation details, follow the official [Stream Azure Diagnostics to Fabric documentation](https://review.learn.microsoft.com/fabric/real-time-hub/add-source-azure-diagnostic-logs-metrics?branch=release-fabcon-real-time-intelligence).

---

By integrating Azure Monitor with Microsoft Fabric via Eventstream, organizations unlock strategic value and operational insights that drive intelligent decision-making.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/unlocking-real-time-operational-intelligence-azure-monitor-logs-integration-in-fabric-via-eventstream/)
