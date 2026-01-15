---
layout: post
title: 'What’s New with Fabric Activator: Enhanced Automation and Event Detection'
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/whats-new-with-fabric-activator/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-11-20 11:30:00 +00:00
permalink: /ml/news/Whats-New-with-Fabric-Activator-Enhanced-Automation-and-Event-Detection
tags:
- Automation
- Azure
- Business Process Automation
- Data Engineering
- Event Detection
- Fabric Activator
- KQL Queryset
- Microsoft Fabric
- ML
- News
- No Code
- Power Automate
- Real Time Hub
- Real Time Intelligence
- Spark Jobs
- Teams Integration
- User Data Functions
- Workflows
section_names:
- azure
- ml
---
Microsoft Fabric Blog outlines recent enhancements in Fabric Activator, demonstrating how users can automate data-driven business processes, trigger advanced workflows, and integrate actions using no-code solutions within Microsoft Fabric.<!--excerpt_end-->

# What’s New with Fabric Activator: Enhanced Automation and Event Detection

Fabric Activator is a feature within Microsoft Fabric designed to enable rapid, no-code automation of event detection and response across diverse data sources. This latest update introduces several powerful capabilities to streamline operational workflows and enhance responsiveness:

## Key New Features

1. **User Data Functions and Spark Job Definitions as Actions**
   - You can now automate business logic by executing [User data functions](https://learn.microsoft.com/fabric/data-engineering/user-data-functions/user-data-functions-overview) (Preview) and [Spark job definitions](https://learn.microsoft.com/fabric/data-engineering/spark-job-definition) as automated actions when data changes are detected.

2. **Advanced Automation in Real-Time Hub**
   - Fabric Activator integrates with [Real-Time Hub](https://learn.microsoft.com/en-us/fabric/real-time-hub/set-alerts-data-streams?context=%2Ffabric%2Fcontext%2Fcontext-rti&pivots=fabric) (with upcoming support for Real-Time dashboard, Eventhouse, and [KQL Queryset](https://learn.microsoft.com/fabric/real-time-intelligence/data-activator/activator-alert-queryset?tabs=visualization)).
   - Enhanced automation actions include:
     - Passing [parameter values](https://learn.microsoft.com/en-us/fabric/real-time-intelligence/data-activator/activator-trigger-fabric-items#pass-parameter-values-to-fabric-items-preview) to functions, Spark definitions, pipelines, and notebooks.
     - Creating reusable [custom actions](https://learn.microsoft.com/en-us/fabric/real-time-intelligence/data-activator/activator-trigger-power-automate-flows) with Power Automate flows.
     - Sending Teams messages to [group chats and channels](https://learn.microsoft.com/en-us/fabric/real-time-intelligence/data-activator/activator-limitations#allowed-chats-and-channel-for-teams-notifications).
     - Customizing email and Teams notifications to suit operational needs.
   - *Lifecycle management for User data function integration is planned for release by mid-December.*

## Accessing and Using Fabric Activator

- You can begin using these new features by creating an Activator item directly in your Fabric workspace or through embedded experiences such as the Real-Time Hub.
- For example, when you browse data sources such as [Azure events](https://learn.microsoft.com/fabric/real-time-hub/set-alerts-azure-blob-storage-events?context=%2Ffabric%2Fcontext%2Fcontext-rti&pivots=fabric), [Fabric events](https://learn.microsoft.com/fabric/real-time-hub/set-alerts-fabric-workspace-item-events?context=%2Ffabric%2Fcontext%2Fcontext-rti&pivots=fabric), or [Eventstream](https://learn.microsoft.com/fabric/real-time-hub/set-alerts-data-streams?context=%2Ffabric%2Fcontext%2Fcontext-rti&pivots=fabric), the ‘Set alert’ button activates panel-driven configuration for conditions and responses.
- Actions available include standard notifications, advanced activations (like running Fabric activities), and [custom Power Automate workflows](https://learn.microsoft.com/fabric/real-time-intelligence/data-activator/activator-trigger-power-automate-flows).

## Example Workflow

1. **Monitor Data Source:** Choose to monitor Azure, Fabric, or Eventstream data sources.
2. **Set Alert:** Click 'Set alert' to configure condition(s) for which you want to trigger automation.
3. **Define Actions:** Select from notifications, user data function execution, Spark jobs, or Teams/email communications.
4. **Automate Process:** Save your rule to automatically respond to real-time data events, increasing operational efficiency.

![Actions available for automations in Fabric Activator](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/11/image-9.png)

## Get Started and Share Feedback

To explore these capabilities, visit the [Microsoft Fabric portal](https://app.fabric.microsoft.com/). The team welcomes feedback and new ideas in the [Activator community](http://aka.ms/ActivatorCommunity).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/whats-new-with-fabric-activator/)
