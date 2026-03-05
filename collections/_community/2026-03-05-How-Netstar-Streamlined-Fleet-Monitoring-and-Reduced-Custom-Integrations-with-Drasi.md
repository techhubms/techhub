---
layout: "post"
title: "How Netstar Streamlined Fleet Monitoring and Reduced Custom Integrations with Drasi"
description: "This case study details how Netstar leveraged Drasi’s change-driven architecture to unify fleet monitoring, automate alerts, and reduce engineering overhead. By integrating Azure SQL and EventHub with Drasi, Netstar eliminated fragmented pipelines, enabling real-time visibility and faster incident response. The implementation led to significant reductions in custom development and improved coordination across operations."
author: "CollinBrian"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/how-netstar-streamlined-fleet-monitoring-and-reduced-custom/ba-p/4499592"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-03-05 19:23:46 +00:00
permalink: "/2026-03-05-How-Netstar-Streamlined-Fleet-Monitoring-and-Reduced-Custom-Integrations-with-Drasi.html"
categories: ["Azure"]
tags: ["Azure", "Azure EventHub", "Azure SQL", "Blazor", "Change Driven Architecture", "Community", "Continuous Monitoring", "Continuous Queries", "Drasi", "Fleet Monitoring", "Grafana", "Incident Response", "IoT Telemetry", "Logistics", "Operational Data Integration", "Real Time Monitoring", "Supply Chain Visibility"]
tags_normalized: ["azure", "azure eventhub", "azure sql", "blazor", "change driven architecture", "community", "continuous monitoring", "continuous queries", "drasi", "fleet monitoring", "grafana", "incident response", "iot telemetry", "logistics", "operational data integration", "real time monitoring", "supply chain visibility"]
---

CollinBrian outlines how Netstar improved fleet operations by integrating Drasi with Azure SQL and EventHub, moving from fragmented data pipelines to a unified, real-time monitoring platform.<!--excerpt_end-->

# How Netstar Streamlined Fleet Monitoring and Reduced Custom Integrations with Drasi

**Authored by: CollinBrian**

## Introduction

When high-value containers go off the grid, logistics teams lose visibility, risking cascading delays and congestion. Netstar, a fleet solutions provider supporting customers like Maersk, needed to address growing integration complexity and improve real-time responsiveness as its operations scaled.

## The Challenge of Data Fragmentation

Netstar’s development teams faced repeated integration work as operational needs evolved, building overlapping dashboards and workflows for each new requirement. Batch-based processing further delayed detection of key events, such as missed health signals, constraining timely action.

## Enter Drasi: Change-driven Data Architecture

Netstar adopted [Drasi](https://drasi.io/) as the core of its new real-time architecture. Drasi’s strengths are:

- **Continuous Queries**: Join and update data in real time from Azure SQL (vehicle data) and Azure EventHub (GPS, IoT)
- **Automated Reactions**: Trigger notifications and actions on critical events, such as missed signals or delays
- **Unified Monitoring**: Integrate with Grafana for a consolidated operational dashboard

## Unified, Continuously Updated Fleet Dashboard

By integrating Drasi with Azure SQL and EventHub, Netstar eliminated custom pipelines, providing a single, up-to-date picture for fleet operations. Actions configured in Drasi allow for immediate response to events without human intervention, reducing the risk of missed notifications and improving operational agility.

## Operational and Business Impact

- **Faster incident response**: Immediate alerts for missing health signals enable timely action
- **Improved coordination**: Real-time data supports logistics teams and partners like Maersk
- **Lower development overhead**: One Drasi instance supports many use cases with less custom code (e.g., tracking, health monitoring, route optimization)
- **Streamlined workflow**: Operators use a single Grafana interface with all relevant fleet data
- **Expanded capability**: The architecture extends to billing and contracts by adding data sources and queries, all without new infrastructure

## Broader Architectural Shift

Netstar’s move to a change-driven platform reflects a larger industry trend: the shift from batch processing and custom point integrations to event-driven, real-time operational systems. Drasi’s reusable integration patterns free engineering teams from infrastructure work, so they can focus on business value and rapid innovation.

## Looking Forward

Netstar’s team is now exploring predictive maintenance—spotting emerging issues earlier by analyzing health data trends. This same architecture could inform broader supply chain coordination.

## Conclusion

By adopting Drasi and connecting it with Azure data services, Netstar unified and improved their fleet operations, reduced redundant engineering, and adapted their business to the demands of real-time logistics. This case demonstrates how open source, change-driven platforms can deliver actionable insight and operational efficiency in complex, distributed environments.

Learn more at [Drasi.io](https://drasi.io/).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/how-netstar-streamlined-fleet-monitoring-and-reduced-custom/ba-p/4499592)
