---
layout: "post"
title: "Closing the loop: Interactive write-back from Power BI to Azure Databricks"
description: "This collaborative post by Microsoft and Databricks introduces the new Azure Databricks connector for Power Platform, now generally available. It allows organizations to build Power Apps, automate flows, and create Copilot Studio agents, while enabling real-time write-back of data from Power BI to Azure Databricks. The connector integrates Power Platform tools with governed data, supporting streamlined operational workflows and eliminating data duplication. The post provides step-by-step instructions for enabling writebacks and explains how users can act on business data instantly using familiar Power BI and Power Apps interfaces."
author: "AnaviNahar"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/closing-the-loop-interactive-write-back-from-power-bi-to-azure/ba-p/4442999"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-14 16:00:00 +00:00
permalink: "/2025-08-14-Closing-the-loop-Interactive-write-back-from-Power-BI-to-Azure-Databricks.html"
categories: ["Azure", "ML"]
tags: ["Analytics", "Azure", "Azure Databricks", "Community", "Connector", "Copilot Studio", "Data Governance", "Data Integration", "Data Warehousing", "Databricks", "Delta Lake", "Microsoft", "ML", "Operational Workflows", "Power Apps", "Power BI", "Power Platform", "Real Time Data", "Spark", "Writeback"]
tags_normalized: ["analytics", "azure", "azure databricks", "community", "connector", "copilot studio", "data governance", "data integration", "data warehousing", "databricks", "delta lake", "microsoft", "ml", "operational workflows", "power apps", "power bi", "power platform", "real time data", "spark", "writeback"]
---

AnaviNahar presents an in-depth look at the newly released Azure Databricks connector for Power Platform, explaining how organizations can now write data back to Databricks from Power BI and streamline analytics workflows in real time.<!--excerpt_end-->

# Closing the loop: Interactive write-back from Power BI to Azure Databricks

*Collaborative post by Microsoft and Databricks, with contributions from Toussaint Webb (Product Manager, Databricks).*

## Introduction

The Azure Databricks connector for Power Platform is now generally available, enabling seamless integration between Power Apps, Power Automate, Copilot Studio, and Azure Databricks data—with robust governance and no data duplication required.

## Key Capabilities

- **Real-time Data Flow:** Organizations can read from and write to Azure Databricks data warehouses in real time from Power Platform tools.
- **Operational Efficiency:** Power BI users can initiate writebacks directly to Azure Databricks, enabling fast, data-driven actions.
- **End-to-End Integration:** No custom connectors or additional services/licenses required; the connector operates entirely within Power Platform environments.

## Step-by-Step Guide: Writebacks from Power BI to Azure Databricks

1. **Create a Connector in Power Apps**
     - Open Power Apps and follow [official documentation](https://learn.microsoft.com/en-us/azure/databricks/integrations/msft-power-platform) to link with Azure Databricks.
2. **Embed Power Apps Visual in Power BI**
     - In Power BI (desktop or online), insert a Power Apps visual (purple icon) into your report.
3. **Connect Data to Your App**
     - Use the Power BI visualization pane to provide data for the embedded Power App.
4. **Build or Select Power App**
     - Either create a new app directly from Power BI or select an existing app to embed and enable real-time updates to Azure Databricks.

With these steps, users can update and manage data in Azure Databricks directly from within Power BI, unlocking new workflows such as real-time inventory adjustments or performance monitoring.

## Workflow Scenarios

- **Warehouse Management:** Supervisors can flag operational issues in real time and update records on the spot.
- **Retail Inventory:** Store managers can review and instantly adjust inventory levels using the integrated platform.

## Availability and Further Exploration

- The Azure Databricks Power Platform Connector is now generally available for all Azure Databricks customers.
- Deep-dive blog: [Introducing Azure Databricks Power Platform Connector](https://www.databricks.com/blog/introducing-azure-databricks-power-platform-connector-unleashing-governed-real-time-data-power)
- [Technical documentation](https://learn.microsoft.com/en-us/azure/databricks/integrations/msft-power-platform) for setup guidance.
- Future updates include support for executing Azure Databricks Jobs via Power Automate.
- For customizable solutions, see [Databricks Apps](https://www.databricks.com/product/databricks-apps).

---
**Author**: AnaviNahar (Microsoft)

*Originally published on the Analytics on Azure Blog, Aug 13, 2025*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/closing-the-loop-interactive-write-back-from-power-bi-to-azure/ba-p/4442999)
