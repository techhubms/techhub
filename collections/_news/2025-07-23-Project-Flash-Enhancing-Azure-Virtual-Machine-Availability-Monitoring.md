---
external_url: https://azure.microsoft.com/en-us/blog/project-flash-update-advancing-azure-virtual-machine-availability-monitoring-2/
title: 'Project Flash: Enhancing Azure Virtual Machine Availability Monitoring'
author: Mark Russinovich
feed_name: The Azure Blog
date: 2025-07-23 16:00:00 +00:00
tags:
- Advancing Reliability
- Availability Monitoring
- Azure Monitor
- Azure Resource Graph
- Azure Virtual Machines
- Cloud Observability
- Compute
- Event Grid
- Infrastructure Monitoring
- Infrastructure Reliability
- Internet Of Things
- Management And Governance
- Project Flash
- Real Time Alerts
- Resource Health
- Root Cause Analysis
- Service Health
section_names:
- azure
- devops
primary_section: azure
---
Written by Mark Russinovich, this blog offers an in-depth look at Project Flash—a cross-division Azure initiative—outlining its latest advancements in virtual machine availability monitoring, new integrations, and real-world benefits for teams managing Azure infrastructure.<!--excerpt_end-->

# Project Flash: Enhancing Azure Virtual Machine Availability Monitoring

*By Mark Russinovich*

Flash is Microsoft's latest initiative enabling rapid detection of issues originating from the Azure platform, empowering teams to respond quickly to infrastructure-related disruptions. In this update, Mark Russinovich, CTO, Deputy CISO, and Technical Fellow at Microsoft Azure, introduces new advancements in Project Flash, with insights contributed by Yingqi (Halley) Ding, Technical Program Manager, Azure Core Compute team.

---

## Overview of Project Flash

[Project Flash](https://learn.microsoft.com/en-us/azure/virtual-machines/flash-overview) is a cross-division effort at Microsoft to provide precise telemetry, real-time alerts, and scalable monitoring within a unified experience, specifically targeting the diverse observability needs for virtual machine (VM) availability on Azure.

Flash addresses both Azure platform-level and user-level monitoring challenges by:

- Detecting issues such as VM reboots, restarts, application freezes (e.g., due to network driver updates), or planned/unexpected host OS updates.
- Offering trend analysis, customizable alerts for debugging and tracking availability.
- Monitoring VM and resource health at scale with custom dashboards.
- Delivering automated root cause analyses (RCAs) that specify affected VMs, issue causation, durations, and resolutions.
- Proactively alerting on critical events (e.g., degraded nodes, platform-initiated healing, hardware-triggered reboots) to enable timely responses and reduced user impact.
- Allowing dynamic recovery policy adjustments for evolving business priorities.

The solutions stemming from Project Flash support Service-Level Agreement adherence and have been widely adopted in sectors such as e-commerce, gaming, finance, and hedge funds.

#### Customer Perspective

> *At BlackRock, VM reliability is critical ... With Project Flash, we receive a resource health event integrated into our alerting processes the moment an underlying node in Azure infrastructure is marked unallocatable ... This ability to predictively avoid abrupt VM failures has reduced our VM interruption rate and improved the overall reliability of our investment platform.*
>
> — Eli Hamburger, Head of Infrastructure Hosting, BlackRock

## The Flash Monitoring Suite

Project Flash has matured into a robust framework supporting organizations from small deployments to massive cloud estates. Key monitoring solutions include:

| Solution | Description |
|---|---|
| [Azure Resource Graph](https://learn.microsoft.com/en-us/azure/virtual-machines/flash-azure-resource-graph) | Aggregate telemetry and conduct historical investigations across all VMs at scale. |
| [Event Grid system topic](https://learn.microsoft.com/en-us/azure/virtual-machines/flash-event-grid-system-topic) | Trigger fast mitigations with alerts delivered within seconds of resource availability changes. |
| [Azure Monitor – Metrics](https://learn.microsoft.com/en-us/azure/virtual-machines/flash-azure-monitor) | Track trends and configure metric-based alerts with an out-of-the-box VM availability metric. |
| [Resource Health](https://learn.microsoft.com/en-us/azure/virtual-machines/flash-azure-resource-health) | Quickly access per-resource health status and 30-day health history in the Azure Portal. |

![Flash endpoints flowchart.](https://azure.microsoft.com/en-us/blog/wp-content/uploads/2025/07/1-flash-endpoints-1.webp)
*Figure 1: Flash endpoints*

## What’s New in Project Flash

### 1. User vs Platform Activity Dimension for VM Availability Metrics (Public Preview)

A top customer request—user-friendly, near real-time monitoring of compute availability—resulted in the introduction of a **Context dimension** to the [VM availability metric](https://learn.microsoft.com/en-us/azure/virtual-machines/flash-azure-monitor) within Azure Monitor. This dimension enables teams to distinguish if a VM disruption was caused by Azure-initiated (Platform), user-orchestrated (Customer), or unknown activity, supporting:

- Alert rules filtering by disruption context
- Faster, more accurate diagnosis and mitigation

![Monitoring overview with the VM availability metric.](https://azure.microsoft.com/en-us/blog/wp-content/uploads/2025/07/2-vma.webp)
*Figure 2: VM availability metric*

![Graph showing context dimension.](https://azure.microsoft.com/en-us/blog/wp-content/uploads/2025/07/3-context.webp)
*Figure 3: Context dimension*

![UI for alert rule creation with dimension filtering.](https://azure.microsoft.com/en-us/blog/wp-content/uploads/2025/07/4-alert-rule.webp)
*Figure 4: Azure Monitor alert rule*

### 2. Azure Monitor Alerts Integration with Event Grid (Public Preview)

The integration of [Azure Monitor](https://learn.microsoft.com/en-us/azure/azure-monitor/) alerts as event handlers to [Azure Event Grid](https://learn.microsoft.com/en-us/azure/event-grid/overview) enables:

- Low-latency notifications for VM availability and health changes
- Multiple communication channels: SMS, email, push notifications, etc.
- Event-driven automation combining near real-time Event Grid delivery with Azure Monitor's alerting

![List of event grid system topics.](https://azure.microsoft.com/en-us/blog/wp-content/uploads/2025/07/5-eg.webp)
*Figure 5: Event Grid system topics*

![UI for Event Grid subscription creation.](https://azure.microsoft.com/en-us/blog/wp-content/uploads/2025/07/6-eg-subscription.webp)
*Figure 6: Event Grid subscription*

**Getting started** involves following [step-by-step instructions](https://learn.microsoft.com/en-us/azure/event-grid/handle-health-resources-events-using-azure-monitor-alerts) to receive real-time VM health alerts via the Flash platform.

## Looking Ahead

Future enhancements planned for Project Flash include:

- Monitoring scenarios such as inoperable top-of-rack switches, accelerated networking failures, and sophisticated hardware failure prediction
- Improvements to data quality and consistency across all Flash endpoints for more accurate downtime attribution and visibility

**Recommended Approach:**

- Use both Flash Health events (real-time insights and historical disruption logging) and [Scheduled Events](https://learn.microsoft.com/en-us/azure/virtual-machines/windows/scheduled-event-service) (for proactive, prioritized responses to planned maintenance and migrations).
  - Flash Health: Real-time and historical VM disruption analysis, root cause, and downtime management
  - Scheduled Events: Advance notice (up to 15 minutes) for upcoming maintenance or actions

## Learn More

For details, visit the [Project Flash documentation](https://learn.microsoft.com/en-us/azure/virtual-machines/flash-overview) and follow updates on the [Advancing Reliability blog series](https://azure.microsoft.com/en-us/blog/tag/advancing-reliability/).

This post appeared first on "The Azure Blog". [Read the entire article here](https://azure.microsoft.com/en-us/blog/project-flash-update-advancing-azure-virtual-machine-availability-monitoring-2/)
