---
layout: post
title: Interactive Write-back from Power BI to Azure Databricks with Power Platform Connector
author: AnaviNahar
canonical_url: https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/closing-the-loop-interactive-write-back-from-power-bi-to-azure/ba-p/4442999
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-08-15 00:12:37 +00:00
permalink: /ml/community/Interactive-Write-back-from-Power-BI-to-Azure-Databricks-with-Power-Platform-Connector
tags:
- Analytics
- Azure Databricks
- Connector Integration
- Copilot Studio
- Data Governance
- Data Warehousing
- Data Write Back
- Delta Lake
- Embedded Power Apps
- Microsoft
- Operational Analytics
- Power Apps
- Power Automate
- Power BI
- Power Platform
- Real Time Data
- Spark
- Workflows
section_names:
- azure
- ml
---
Anavi Nahar shares a detailed walkthrough of how the new Azure Databricks connector for Power Platform allows Power BI users to write data back to Azure Databricks in real time, creating actionable analytics workflows.<!--excerpt_end-->

# Interactive Write-back from Power BI to Azure Databricks with Power Platform Connector

*Collaborative post by Anavi Nahar, Microsoft, and Databricks.*

## Introduction

The Azure Databricks connector for Power Platform is now Generally Available. This integration enables organizations to seamlessly combine the analytics capabilities of Power BI with the data warehousing and processing power of Azure Databricks.

## Key Highlights

- **Interactive Write-back**: Power BI users can now "close the loop" by writing data back into Azure Databricks using Power Apps directly within their reports. This enables real-time operational analytics and facilitates faster actions based on insights.
- **No Data Duplication**: Data remains secure and governed without the need for duplicated sources or custom connectors.
- **Unified Workflows**: Users can trigger Power Automate flows or build Copilot Studio agents, uniting reporting and action in one place.

## How It Works: Enabling Writebacks

1. **Connect Power Apps to Azure Databricks**
   - In Power Apps, establish a connection to Azure Databricks. [Documentation](https://learn.microsoft.com/en-us/azure/databricks/integrations/msft-power-platform)
2. **Add Power Apps Visual in Power BI**
   - In Power BI (desktop or service), insert a Power Apps visual to your report using the purple icon.
3. **Bind Data to the Power App**
   - Use the visualization pane to link your data from Power BI to the Power App.
4. **Create or Embed Power Apps**
   - Either build a new Power App within Power BI or embed an existing one.
5. **Perform Write-back Operations**
   - Directly within Power BI, users can adjust records that are immediately written back to Azure Databricks for real-time operational impact.

## Use Case Examples

- **Warehouse Management**: Managers can flag issues or update inventory directly while monitoring metrics.
- **Inventory Control**: Store owners view analytics and instantly update records in the central Databricks warehouse.

## Getting Started

The connector is now generally available:

- [Official Blog Announcement](https://www.databricks.com/blog/introducing-azure-databricks-power-platform-connector-unleashing-governed-real-time-data-power)
- [Technical Documentation](https://learn.microsoft.com/en-us/azure/databricks/integrations/msft-power-platform)

Upcoming features include executing existing Azure Databricks Jobs via Power Automate for greater workflow automation.

If your organization requires advanced custom solutions, explore [Databricks Apps](https://www.databricks.com/product/databricks-apps) on Azure Databricks with no extra services or licenses required.

## About the Author

**Anavi Nahar** – Microsoft community member and advocate for actionable analytics with Azure platforms.

---

*Version 2.0, Updated Aug 15, 2025*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/closing-the-loop-interactive-write-back-from-power-bi-to-azure/ba-p/4442999)
