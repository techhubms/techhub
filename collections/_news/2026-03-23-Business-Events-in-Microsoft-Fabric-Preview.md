---
section_names:
- ai
- ml
date: 2026-03-23 13:30:00 +00:00
tags:
- Activator
- AI
- Anomaly Detection
- Automation
- Business Events
- CriticalVibrationDetected
- Dataflows
- Event Driven Architecture
- Event Schema Versioning
- Manufacturing Case Study
- Microsoft Fabric
- ML
- News
- Notebooks
- Power Automate
- Real Time Alerts
- Real Time Hub
- Real Time Intelligence
- Schema Governance
- Schema Registry
- Schema Sets
- Spark
- Spark Jobs
- Telemetry
- User Data Functions
author: Microsoft Fabric Blog
title: Business Events in Microsoft Fabric (Preview)
feed_name: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/business-events-in-microsoft-fabric-preview/
primary_section: ai
---

Microsoft Fabric Blog introduces Business Events in Microsoft Fabric (Preview), a Real-Time Hub capability for defining governed, reusable business event schemas and triggering parallel downstream actions (alerts, automation, notebooks, Spark, and AI/ML enrichment) from those events.<!--excerpt_end-->

# Business Events in Microsoft Fabric (Preview)

*If you haven’t already, check out Arun Ulag’s hero blog* “[FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform](https://aka.ms/FabCon-SQLCon-2026-news)” *for a complete look at FabCon and SQLCon announcements across Fabric and Microsoft’s database offerings.*

Modern organizations generate countless signals as customers interact with applications; operations evolve, and market conditions change—but many of these signals stay locked in data and are discovered too late for action. **Business Events** in **Microsoft Fabric (Preview)** aims to help teams move from observing what happened to acting on what matters in real time.

Learn more: [Business Events overview](https://aka.ms/overview-businessevents)

Business Events is positioned as a way to capture critical business moments and enable immediate response and decision-making across analytics, automation, and AI.

## Turning business signals into intelligent action

A **Business Event** is a significant occurrence or change-in-state that matters to the business. The article contrasts this with raw telemetry or diagnostic data: Business Events are intentionally modeled around business outcomes and decisions.

Within Fabric, Business Events provides a unified way to:

- **Define** business-critical event types
- **Generate/emit** events from compute and logic
- **Consume** events to trigger multiple downstream actions

With this release, customers can emit Business Events while running:

- Custom logic via **User Data Functions**
- Compute-intensive analytics via **Notebooks**

Then, they can immediately use those events to:

- Trigger alerts
- Automate business processes
- Run analytics
- Enrich AI experiences

Business Events integrates natively across Fabric, and a single event can drive multiple consumers in parallel, including:

- **Alert and automate** downstream processes using **Activator**
- **Execute custom business logic** through **User Data Functions**
- **Run analytical workflows** in **Notebooks**
- **Provide real time context** to **AI and ML models**
- **Run distributed processing** with **Spark jobs**
- **Prepare and move data** using **Dataflows**
- **Automate business processes** with **Power Automate**

![Screenshot of a software dashboard showing a "Business events" setup page within Real-Time Hub experience. The page includes options for real-time notifications, trigger alerts, and automated downstream workflows, with a prominent button labeled "New Business event" for creating events.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-software-dashboard-showing-a-busi-3.gif)

*Figure: Business Event Creation Experience.*

## Decoupled, scalable event driven architecture

The article describes Business Events as enabling a **fully decoupled architecture**:

- **Publishers** emit events independently
- **Consumers** can be added later without changing publisher code

This is meant to let teams introduce new consumers (analytics pipelines, automation, external integrations) without modifying existing publisher implementations, keeping the system flexible as it grows.

## Consistent modeling with a shared business language

To address consistency and reusability, Business Events are governed through the **Schema Registry** in **Real Time hub**:

- Docs: [Schema Registry overview](https://learn.microsoft.com/fabric/real-time-intelligence/schema-sets/schema-registry-overview)

The Schema Registry acts as a centralized repository for defining and managing event schemas. The article highlights these goals:

- Single source of truth for field names, data types, and semantics
- Reduce/eliminate common integration problems like **schema drift**
- Make schemas centrally discoverable and versioned

The intent is a shared “business language” that is real-time and also reliable and interpretable across teams.

## Comprehensive overview: A manufacturing case study

Example scenario: a manufacturing solution monitors vibration levels across critical equipment.

1. A **Notebook** analyzes incoming telemetry and detects abnormal vibration patterns.
2. When detected, it publishes a **CriticalVibrationDetected** Business Event.
3. **Activator** triggers multiple actions, such as:
   - Shift equipment to operate in safe mode
   - Create a service ticket
   - Perform root cause analysis for future decisions

![Diagram illustrating an automated workflow for handling business events, starting with data analysis in a Notebook and triggering actions via an Activator. Key components include Power Automate running ticket creation workflows linked to ServiceNow for support tickets and a second Notebook performing root cause analysis, generating reports in Power BI, with a control service managing auto mitigation actions.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/diagram-illustrating-an-automated-workflow-for-han.png)

*Figure: Sample architecture using Business Events.*

The point of the example is reacting instantly to critical conditions without stitching together separate systems or adding operational complexity.

## Try Business Events (Preview)

- Submit feedback via [Fabric Ideas](https://ideas.fabric.microsoft.com/) for the Real-Time Hub.
- More details: [Business Events overview](https://aka.ms/overview-businessevents)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/business-events-in-microsoft-fabric-preview/)

