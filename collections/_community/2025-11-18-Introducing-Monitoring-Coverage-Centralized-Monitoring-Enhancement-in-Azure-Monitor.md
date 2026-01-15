---
layout: post
title: 'Introducing Monitoring Coverage: Centralized Monitoring Enhancement in Azure Monitor'
author: Nathan_Mangum
canonical_url: https://techcommunity.microsoft.com/t5/azure-observability-blog/introducing-monitoring-coverage-assess-and-improve-your/ba-p/4470752
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-18 16:00:00 +00:00
permalink: /azure/community/Introducing-Monitoring-Coverage-Centralized-Monitoring-Enhancement-in-Azure-Monitor
tags:
- AKS
- Azure
- Azure Advisor
- Azure Monitor
- Centralized Monitoring
- Community
- Configuration Management
- DevOps
- Log Analytics
- Monitoring Coverage
- Monitoring Recommendations
- Observability
- Preview Features
- Resource Monitoring
- Virtual Machines
section_names:
- azure
- devops
---
Nathan Mangum details the preview release of Monitoring Coverage in Azure Monitor, outlining how organizations can assess and enhance monitoring across Azure resources using a unified, scalable approach.<!--excerpt_end-->

# Introducing Monitoring Coverage: Centralized Monitoring Enhancement in Azure Monitor

As organizations increase their use of Azure, consistent and comprehensive monitoring becomes critical. Microsoft has unveiled Monitoring Coverage (preview), a new feature within Azure Monitor designed to provide a centralized experience for assessing, configuring, and strengthening monitoring across your Azure environment.

## A Unified View of Monitoring Health

Monitoring Coverage integrates with Azure Advisor to highlight areas where monitoring needs improvement. The feature:

- Consolidates monitoring insights for all your Azure resources.
- Distinguishes between resources with basic telemetry and those needing enhanced recommended configurations.
- Helps close gaps in observability strategies efficiently.

## Key Capabilities

- **Comprehensive visibility:** Visualizes monitoring coverage for common Azure resource types.
- **Actionable recommendations:** Surfaces Azure Advisor recommendations that can be applied at scale.
- **Centralized configuration:** Enables settings for multiple resources in one place.
- **Detailed resource insights:** Lets users review active configurations and recommendations per resource.

## Accessing Monitoring Coverage

To get started:

1. Open the Azure portal and go to Azure Monitor.
2. Under Settings, select Monitoring Coverage (preview).
3. Scope your view using filters like Subscriptions, Resource Groups, Tags, Locations, and Resource Types.

## Supported Resource Types

During the preview, Monitoring Coverage supports Virtual Machines (VMs) and Azure Kubernetes Service (AKS) clusters. Microsoft plans to expand support in future updates.

## Overview Tab

The Overview provides a snapshot of monitoring health:

- **Basic monitoring**: Default metrics/logs active on resource creation.
- **Enhanced monitoring**: Microsoft-recommended settings for deeper insights.

Users can quickly spot and remediate coverage gaps; note that enabling enhanced monitoring may incur additional Azure costs depending on configuration.

## Streamlined Enablement Experience

Enabling monitoring is simplified:

1. View a list of affected resources—deselect any you don't want included.
2. Configure settings for each resource type (e.g., choose a Log Analytics workspace).
3. Review all changes before enabling.
4. After enablement, data will typically flow to the workspace within 30–60 minutes.
5. During preview, up to 100 resources can be enabled per operation; an existing Log Analytics or Azure Monitor Workspace is required.

## Monitoring Details Page

For granular management:

- View or group resources by recommendation.
- Filter using standard Azure controls.
- Review the Monitoring coverage column for enabled recommendations and data collection rules.
- Directly enable individual settings when managing resources one by one.

## Feedback and Roadmap

Microsoft invites users to provide feedback through the Azure portal, helping guide future improvements for Monitoring Coverage.

---

Try Monitoring Coverage (preview) in the Azure portal today to elevate your observability coverage and achieve proactive, scalable monitoring for your Azure resources.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-observability-blog/introducing-monitoring-coverage-assess-and-improve-your/ba-p/4470752)
