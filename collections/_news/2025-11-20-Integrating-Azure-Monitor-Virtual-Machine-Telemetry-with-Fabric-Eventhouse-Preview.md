---
layout: post
title: Integrating Azure Monitor Virtual Machine Telemetry with Fabric Eventhouse (Preview)
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/azure-monitor-to-fabric-eventhouse-preview/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-11-20 12:00:00 +00:00
permalink: /ml/news/Integrating-Azure-Monitor-Virtual-Machine-Telemetry-with-Fabric-Eventhouse-Preview
tags:
- AD Hoc Query
- Azure Data Explorer
- Azure Monitor
- Azure Monitor Agent
- Data Collection Rules
- Data Visualization
- Event Logs
- Fabric Eventhouse
- Grafana
- Infrastructure Monitoring
- OneLake
- Operational Insights
- Power BI Integration
- Prometheus
- Python Analytics
- Real Time Analytics
- Schema Management
- Tableau
- Telemetry Aggregation
- Virtual Machine Telemetry
section_names:
- azure
- ml
---
Microsoft Fabric Blog explains how to combine VM telemetry from Azure Monitor with Fabric Eventhouse for integrated operational analytics and business insight.<!--excerpt_end-->

# Integrating Azure Monitor Virtual Machine Telemetry with Fabric Eventhouse (Preview)

Managing virtual machines across cloud and hybrid environments introduces complexity for enterprise analytics. This guide outlines how Azure Monitor and Fabric Eventhouse together enable scalable telemetry aggregation and advanced analytics.

## Scenario: Enterprise Telemetry and Operational Insight

Organizations running numerous virtual machines (VMs)—spanning Azure, on-premise, and other clouds—benefit from centralizing telemetry. When operational data from infrastructure, applications, and business systems can be correlated and analyzed together, teams gain deeper insight for troubleshooting and optimization.

**Example Use Case**:

- At an airport, analyzing flight turnaround SLAs, telemetry from gate operations apps, and VM/server logs enables diagnosing root causes for delays and poor user experience.

## Core Components of the Solution

1. **[Azure Monitor Agent (AME)](https://learn.microsoft.com/azure/azure-monitor/agents/azure-monitor-agent-overview)**
   - Collects telemetry from virtual machines, including:
     - Performance Counters
     - Windows Event Logs
     - Linux Syslog
     - IIS logs
     - Firewall logs
     - Custom Text/JSON logs
     - Prometheus Metrics

2. **[Azure Monitor Data Collection Rules (DCR)](https://learn.microsoft.com/azure/azure-monitor/data-collection/data-collection-rule-overview)**
   - Configures which telemetry is gathered and routes it to desired destinations.

3. **[Fabric Eventhouse](https://learn.microsoft.com/fabric/real-time-intelligence/eventhouse)** (Fabric RTI Analytical Store)
   - Ingests and automatically schemas telemetry data
   - Also integrates with [Azure Data Explorer](https://learn.microsoft.com/azure/data-explorer/data-explorer-overview)

## Key Features of the Integration

- **Schema Management**: Tables are created automatically; schema evolves with telemetry.
- **Supported Data Sources**: VM telemetry from Azure, on-premises, or multi-cloud environments.
- **Querying and Analytics**:
  - Ad hoc queries
  - Graph queries & time series analytics
  - [In-query Python](https://learn.microsoft.com/fabric/real-time-intelligence/eventhouse-python)
  - Visualizations via [Real Time Dashboards](https://learn.microsoft.com/fabric/real-time-intelligence/dashboard-real-time-create?tabs=create-manual%2Ckql-database), Power BI, Grafana, Tableau
  - [Fabric Activator](https://learn.microsoft.com/fabric/real-time-intelligence/data-activator/activator-introduction) for automation and alerting
- **Integration with Fabric OneLake**: Facilitates broader analytics and data engineering across organizational data lakes
- **Advanced Scenarios**: Run Spark jobs, join operational telemetry with business metrics for cross-domain insights

## Getting Started

- Refer to [Send virtual machine client data to Fabric and Azure Data Explorer (Preview)](https://aka.ms/AzMontoADX) to set up the integration

## Value Proposition

- Aggregate telemetry from all VMs regardless of where they're hosted
- Enable detailed analysis, root cause diagnostics, and operational improvement
- Correlate infrastructure health and business operations for actionable insight

## Feedback

Try out the preview integration and share your feedback via Microsoft documentation and support channels.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/azure-monitor-to-fabric-eventhouse-preview/)
