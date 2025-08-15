---
layout: "post"
title: "Expanding Global Reach and Enhanced Observability with Oracle Database@Azure"
description: "This blog post provides an overview of the expanded regional availability of Oracle Database@Azure, now including Germany North, and introduces enhanced observability features for Oracle Exadata VM Clusters and Infrastructure managed through Azure Monitor and Microsoft Sentinel. It details how organizations can centralize log monitoring for Oracle and Azure services, utilize advanced analytics, and strengthen security and compliance within hybrid cloud environments."
author: "bhbandam"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/oracle-on-azure-blog/expanding-global-reach-and-enhancing-observability-with-oracle/ba-p/4443650"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-14 19:40:40 +00:00
permalink: "/2025-08-14-Expanding-Global-Reach-and-Enhanced-Observability-with-Oracle-DatabaseAzure.html"
categories: ["Azure", "Security"]
tags: ["Azure", "Azure Monitor", "Azure Security Center", "Centralized Monitoring", "Community", "Compliance Monitoring", "Exadata Infrastructure", "Exadata VM Clusters", "Germany North", "Hybrid Cloud", "Kusto Query Language", "Log Analytics", "Microsoft Sentinel", "Operational Visibility", "Oracle Database@Azure", "Regional Availability", "Security", "SEIM", "SOAR", "Threat Detection"]
tags_normalized: ["azure", "azure monitor", "azure security center", "centralized monitoring", "community", "compliance monitoring", "exadata infrastructure", "exadata vm clusters", "germany north", "hybrid cloud", "kusto query language", "log analytics", "microsoft sentinel", "operational visibility", "oracle database at azure", "regional availability", "security", "seim", "soar", "threat detection"]
---

bhbandam, together with Jeni Mattson, shares details about the new regional expansion of Oracle Database@Azure to Germany North and highlights how organizations can leverage enhanced observability features with Azure Monitor and Microsoft Sentinel for streamlined monitoring and improved security.<!--excerpt_end-->

# Expanding Global Reach and Enhanced Observability with Oracle Database@Azure

**Authors:** bhbandam (Microsoft) and Jeni Mattson, Principal Product Manager, Exadata and Database Cloud Product Management

## Overview

Microsoft and Oracle have announced the expansion of Oracle Database@Azure to the Germany North region, increasing global availability to 23 Azure regions. This expansion supports customer needs for data residency, reduces latency, and increases availability. Oracle Database@Azure users can now deploy workloads closer to their operations across a growing list of regions, with plans to expand to 10 more regions by the end of 2025.

### Current Regions Supported

- Australia East
- Australia Southeast
- Brazil South
- Canada Central
- Central India
- Central US
- East US
- East US 2
- France Central
- Germany North
- Germany West Central
- Italy North
- Japan East
- Japan West
- North Europe
- Southeast Asia
- South Central US
- Sweden Central
- UK South
- UK West
- West US
- West US 2
- West US 3

## Enhanced Observability for Exadata Environments

Oracle Exadata customers using Oracle Database@Azure have new observability capabilities via Azure Monitor and Microsoft Sentinel. Users can now:

- Centralize real-time monitoring of logs from both Azure services and Oracle databases.
- Correlate logs across these services for proactive troubleshooting and streamlined root cause analysis.
- Query logs using Azure Monitor and Log Analytics, set up proactive alerts for critical system events, and automate incident response.
- Customize dashboards and advanced reports using Kusto Query Language.
- Seamlessly integrate monitoring with Azure Security Center for threat detection and use Microsoft Sentinel for Security Information and Event Management (SEIM) and Security Orchestration, Automation, and Response (SOAR).
- Support compliance and auditing by retaining logs and tracking changes and access using Azure policy.

### Supported Log Types

- Exadata VM cluster Life Cycle Management Logs
- Exadata Database logs
- Exadata Infrastructure Logs
- Exadata Data Guard logs

## Getting Started

To enable enhanced observability:

1. Enable the observability feature in Azure Portal.
2. Configure relevant settings, including integration with partner solutions.
3. Set up monitoring and alerting, and onboard data to Microsoft Sentinel for advanced analysis.
4. Refer to the [detailed setup guide](https://review.learn.microsoft.com/en-us/azure/oracle/oracle-db/oracle-exadata-database-on-dedicated-infrastructure-logs?branch=pr-en-us-304189#step-1-create-and-configure-a-diagnostic-setting) for step-by-step instructions.

## Learn More

- [Azure Monitor Logs - Azure Monitor | Microsoft Learn](https://learn.microsoft.com/en-us/azure/azure-monitor/logs/data-platform-logs)
- [Log Analytics workspace overview - Azure Monitor | Microsoft Learn](https://learn.microsoft.com/en-us/azure/azure-monitor/logs/log-analytics-workspace-overview)
- [Onboard to Microsoft Sentinel | Microsoft Learn](https://learn.microsoft.com/en-us/azure/sentinel/quickstart-onboard?tabs=defender-portal)

---
*Updated: August 14, 2025*

For additional resources and to follow further updates, visit the [Oracle on Azure Blog](https://techcommunity.microsoft.com/t5/azure/ct-p/Azure).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/oracle-on-azure-blog/expanding-global-reach-and-enhancing-observability-with-oracle/ba-p/4443650)
