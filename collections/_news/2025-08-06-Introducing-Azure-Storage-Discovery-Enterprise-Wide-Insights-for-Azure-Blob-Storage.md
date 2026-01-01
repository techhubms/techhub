---
layout: "post"
title: "Introducing Azure Storage Discovery: Enterprise-Wide Insights for Azure Blob Storage"
description: "Announcing the public preview of Azure Storage Discovery, a fully managed Azure service that provides comprehensive visibility and actionable insights across your Azure Blob Storage data estate. The service offers integration with Azure Copilot, interactive reporting, cost optimization features, and advanced security analysis."
author: "Aung Oo"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://azure.microsoft.com/en-us/blog/introducing-azure-storage-discovery-transform-data-management-with-storage-insights/"
viewing_mode: "external"
feed_name: "The Azure Blog"
feed_url: "https://azure.microsoft.com/en-us/blog/feed/"
date: 2025-08-06 15:00:00 +00:00
permalink: "/2025-08-06-Introducing-Azure-Storage-Discovery-Enterprise-Wide-Insights-for-Azure-Blob-Storage.html"
categories: ["AI", "Azure"]
tags: ["AI", "Azure", "Azure Blob Storage", "Azure Copilot", "Azure Storage Discovery", "Cloud Storage", "Cost Optimization", "Data Governance", "Data Insights", "Enterprise Data Estate", "Historical Data Analytics", "Interactive Reports", "Microsoft Entra", "News", "Operational Efficiency", "Security Compliance", "Storage", "Storage Management"]
tags_normalized: ["ai", "azure", "azure blob storage", "azure copilot", "azure storage discovery", "cloud storage", "cost optimization", "data governance", "data insights", "enterprise data estate", "historical data analytics", "interactive reports", "microsoft entra", "news", "operational efficiency", "security compliance", "storage", "storage management"]
---

Authored by Aung Oo, this blog post announces Azure Storage Discovery, a new Azure service for managing and analyzing enterprise Blob Storage. Learn about its features, Copilot integration, and user experiences.<!--excerpt_end-->

# Introducing Azure Storage Discovery: Enterprise-Wide Insights for Azure Blob Storage

**Author:** Aung Oo

We are excited to announce the public preview of [Azure Storage Discovery](https://aka.ms/StorageDiscovery), a fully managed service providing enterprise-wide visibility into your Azure Blob Storage data estate. This service empowers organizations to analyze how their data estate has evolved over time, optimize costs, enhance security, and drive operational efficiency—all from a unified interface. Azure Storage Discovery integrates with Azure Copilot, enabling natural language-based insights and decision acceleration without the need for a query language.

## Addressing the Complexity of Cloud Data Management

As organizations expand their digital footprints, managing vast and globally distributed datasets across business units and workloads becomes challenging. Azure Storage Discovery aggregates insights across all Azure Blob Storage accounts, helping you:

- Detect outliers and unusual usage trends
- Analyze long-term data trends
- Dive into specific resources with filters and pivots

Traditionally, this information required multiple tools or custom scripts, involving substantial manual effort and infrastructure management. Azure Storage Discovery automates and centralizes the process, aggregating insights across all your Microsoft Entra tenant subscriptions and presenting them in the Azure portal.

[Learn more about Azure Storage Discovery](https://learn.microsoft.com/en-us/azure/storage-discovery/overview)

## Key Questions Answered

With Azure Storage Discovery, you can quickly get answers on:

- Total data stored across all storage accounts
- Regions with the highest data growth
- Opportunities for cost reduction by identifying infrequent data usage
- Storage configurations' alignment with security and compliance best practices

Explore these and many more insights with a few clicks, aided by Azure Copilot.

## Core Features and Capabilities

### 1. Azure Copilot Integration

Use natural language to query storage insights, going beyond pre-built reports to synthesize information across capacity, activity, errors, and configurations.

### 2. Advanced Storage Insights

Gain analytics on:

- Data estate growth
- Cost optimization opportunities
- Under-utilized data
- Workload bottlenecks
- Security posture improvements

Insights are powered by storage metrics such as object size and count, transaction volumes, ingress/egress data, and configuration details.

### 3. Interactive Reports

In the Azure Portal, analyze trends over time, drill into top accounts, and navigate directly to specific resources. Reports can be filtered on configuration parameters such as region, redundancy, performance type, and encryption.

### 4. Organization-Wide Visibility

Analyze up to 1 million storage accounts across subscriptions, resource groups, and regions within a single workspace. Flexible filtering and scoping allow actionable, targeted insights.

### 5. Fully Managed Service

Azure Storage Discovery operates entirely within the Azure Portal, requiring no extra infrastructure or impact on live workloads.

### 6. Historical Data Analytics

The service backfills up to 30 days of historical data within hours of deployment, with retention of all insights for up to 18 months.

## Real-World Customer Stories

### Tesco: Gaining a 360-degree View

Tesco, a global retailer, utilized Azure Storage Discovery for an “effortless 360 View” of their Blob Storage estate. The Cloud Platform Engineering team implemented it to centralize cost analytics, security, and operational reporting, reducing manual reporting time and allowing informed discussions with application teams on data usage and trends.

> As our data estate in Azure Storage continues to grow, it has become time consuming to gather the insights required to drive decisions... Anything which reduces the time it takes me to gather valuable insights is super valuable. The data presented is compelling for conversations with application teams, allowing us to focus on what really matters...
>
> —Rhyan Waine, Lead Engineer, Cloud Platform Engineering, Tesco

### Willis Towers Watson (WTW): Managing Growth and Costs

WTW used Storage Discovery to identify storage accounts with rapid data growth and escalating costs. The team could quickly pinpoint focus areas, detect rarely accessed data, and trigger automated cleanups using Blob Lifecycle Management.

> As soon as my team started using Storage Discovery, they were immediately impressed by the insights it provided. Their reaction: ‘Great—let’s dive in and see what we can uncover.’... We identified several storage accounts growing at an unusual rate and also found data that hadn’t been accessed in a long time, leading to automatic cleanups.
>
> —Darren Gipson, Lead DevOps Engineer, Willis Towers Watson

## How to Start with Azure Storage Discovery

### 1. Configure a Discovery Workspace

Set up a workspace defining the scope of your Azure Blob Storage analysis. Select relevant subscriptions and resource groups within your Microsoft Entra tenant.

![Workspace Setup Screenshot](https://azure.microsoft.com/en-us/blog/wp-content/uploads/2025/08/Discovery_PubPrev_BlogPost_2.webp)

### 2. Define Scopes

Create up to five scopes to group storage accounts by business group, workload, or other logical units—using ARM resource tags.

![Scope Filtering Screenshot](https://azure.microsoft.com/en-us/blog/wp-content/uploads/2025/08/Discovery_PubPrev_BlogPost_3.webp)

Upon deployment, Azure Storage Discovery provides generated reports within the Azure portal automatically.

![Reports Screenshot](https://azure.microsoft.com/en-us/blog/wp-content/uploads/2025/08/Discovery_PubPrev_BlogPost_4.webp)

## Pricing and Availability

Azure Storage Discovery is available in [select Azure regions](https://aka.ms/StorageDiscovery/deploy) as a public preview. The **Free** plan provides insights on capacity and configuration (retained for up to 15 days); the **Standard** plan includes advanced analytics with historical retention up to 18 months. Both plans are offered free until September 30th, 2025. More information on [pricing](https://aka.ms/StorageDiscovery/pricing) is available in the documentation.

## Getting Started

Unlock the full potential of your storage estate in minutes:

- Visit the [quick start guide](https://aka.ms/StorageDiscovery/quickstart) to configure your first workspace.
- Read the [documentation](https://aka.ms/StorageDiscovery).
- Provide feedback: [StorageDiscoveryFeedback@service.microsoft.com](mailto:StorageDiscovery@microsoft.com)

For full details, see [the official documentation](https://learn.microsoft.com/en-us/azure/storage-discovery/overview).

This post appeared first on "The Azure Blog". [Read the entire article here](https://azure.microsoft.com/en-us/blog/introducing-azure-storage-discovery-transform-data-management-with-storage-insights/)
