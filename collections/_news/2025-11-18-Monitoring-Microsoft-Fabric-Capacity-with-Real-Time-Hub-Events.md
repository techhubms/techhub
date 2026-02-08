---
external_url: https://blog.fabric.microsoft.com/en-US/blog/fabric-capacity-events-in-real-time-hub-preview/
title: Monitoring Microsoft Fabric Capacity with Real-Time Hub Events
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-11-18 08:00:00 +00:00
tags:
- Activator
- Advanced Analysis
- Automation
- Capacity Administration
- Capacity Events
- Capacity Monitoring
- Capacity State
- Capacity Summary
- Dashboard
- Data Retention
- Event Processing
- Eventhouse
- Eventstream
- Microsoft Fabric
- Process Automation
- Real Time Hub
- Schema Reference
- Teams Notification
- Azure
- ML
- News
section_names:
- azure
- ml
primary_section: ml
---
Microsoft Fabric Blog introduces real-time event monitoring in Fabric’s Real-Time Hub, guiding capacity administrators on using new event types and automation tooling for operational visibility and control.<!--excerpt_end-->

# Monitoring Microsoft Fabric Capacity with Real-Time Hub Events

Microsoft Fabric introduces new real-time event capabilities in the Real-Time Hub, enabling organizations to gain visibility and control over capacity resources. Two event types are now available:

- **Capacity Summary**: Provides high-level insights about capacity health and utilization. Emitted every 30 seconds.
- **Capacity State**: Tracks immediate capacity status (e.g., paused or overloaded due to throttling). Emitted as events occur.

To accelerate adoption, the community has created an [event monitoring accelerator](https://aka.ms/fabric-capacity-events-accelerator) featuring ready-made templates and configurations.

![Real-Time Dashboard](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/11/image-39.png)

## Example Automation Scenarios

Organizations can use these events to:

- **Monitor and React**: Configure Activator in Real-Time Hub to trigger actions (e.g., notify Teams when interactive delay approaches 95%).
- **Advanced Data Operations**: Direct event streams to Eventhouse for storage and analysis, maintaining comprehensive event history.

### Scenario 1: Automated Notification

> *As a Capacity Administrator, I would like to receive a notification in Teams if the interactive delay capacity percentage approaches 95%.*

Using Real-Time Hub, set up an Activator rule to monitor `interactiveDelayThresholdPercentage`, sending automated alerts to designated Teams channels.

![Activator Rule Panel](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/11/image-54.png)

### Scenario 2: Event Storage and Analysis

> *As a Capacity Administrator, I want to extract, perform complex operations on, and store all events for my capacity.*

Configure Eventstream to collect data, forwarding it into an Eventhouse for deeper analysis or historical retention.

![Eventstream Flow](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/11/image-42.png)

## Getting Started

Refer to the [Introduction to Azure and Fabric events](https://aka.ms/tutorial-capacity-overview-events) for step-by-step instructions. For details on event schema, see [Fabric capacity overview events documentation](https://aka.ms/explore-capacity-overview-events).

## Community and Next Steps

Expect more granular event features coming soon. Share your feedback at [Fabric Ideas](https://ideas.fabric.microsoft.com/) and tap into the [Fabric Community](https://aka.ms/FabricBlog/Community) for collaboration.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/fabric-capacity-events-in-real-time-hub-preview/)
