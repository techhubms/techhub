---
layout: "post"
title: "What’s New in FinOps Toolkit 12 – July 2025"
description: "This update from Michael Flanakin reviews FinOps Toolkit 12, an open-source resource suite for optimizing cloud costs on Microsoft Azure. The release covers support for FOCUS 1.2, new schema versions, Power BI and PowerShell enhancements, improved network compatibility, Data Explorer cost controls, security permission changes, and executive-friendly reporting for better cost analysis and management."
author: "Michael_Flanakin"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/finops-blog/what-s-new-in-finops-toolkit-12-july-2025/ba-p/4438562"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-08-07 17:00:00 +00:00
permalink: "/community/2025-08-07-Whats-New-in-FinOps-Toolkit-12-July-2025.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["Azure", "Azure Data Explorer", "Azure Optimization Engine", "CIDR Support", "Community", "Cost Export", "Cost Management", "Data Ingestion", "Data Pipelines", "DevOps", "DevOps Automation", "Executive Reporting", "FinOps Toolkit", "FOCUS 1.2", "KQL", "Microsoft Azure", "Network Configuration", "Power BI", "PowerShell", "Private Endpoints", "Resource Management", "Role Based Access Control", "Schema Versioning", "Security", "Security Permissions"]
tags_normalized: ["azure", "azure data explorer", "azure optimization engine", "cidr support", "community", "cost export", "cost management", "data ingestion", "data pipelines", "devops", "devops automation", "executive reporting", "finops toolkit", "focus 1dot2", "kql", "microsoft azure", "network configuration", "power bi", "powershell", "private endpoints", "resource management", "role based access control", "schema versioning", "security", "security permissions"]
---

Michael Flanakin explores the major updates in FinOps Toolkit 12 for Azure, focusing on cost management, new data schemas, Power BI reporting, network and security improvements, and automation tools to streamline financial operations in the cloud.<!--excerpt_end-->

# What’s New in FinOps Toolkit 12 – July 2025

**Author:** Michael Flanakin  
**Published:** August 6, 2025

FinOps Toolkit 12 delivers a comprehensive set of enhancements for practitioners aiming to optimize their cloud investments in the Microsoft ecosystem. This release brings updates spanning data schema support, analytics, automation, network configuration, and security. Here are the highlights:

---

## Overview: The FinOps Toolkit and Its Role

The FinOps Toolkit is an open-source suite for learning, implementing, and automating cloud financial operations (FinOps) using Microsoft Azure. It enables teams to maximize cloud value, analyze spending, automate data pipelines, and reduce operational friction.

Learn more and access documentation: [FinOps Toolkit Documentation](https://aka.ms/ftk/docs?WT.mc_id=finops-250604-micflan)

---

## Key Updates in Version 12

### 1. FOCUS 1.2 Support and Schema Enhancements

- The toolkit now supports the **FinOps Open Cost and Usage Specification (FOCUS) 1.2**, enabling improved multi-cloud and multi-currency reporting via Microsoft Cost Management.
- Updates include new metadata, column additions/renames (such as ServiceSubcategory, InvoiceId, PricingCurrency, and SkuMeter), and provisions for future columns (e.g., CapacityReservationId).
- Guides link to transition, validate, and map cost data to FOCUS, minimizing friction in the migration process:  
  - [Update reports to use FOCUS columns](https://learn.microsoft.com/cloud-computing/finops/focus/mapping?WT.mc_id=finops-250528-micflan)  
  - [Convert cost and usage data to FOCUS](https://learn.microsoft.com/cloud-computing/finops/focus/convert?WT.mc_id=finops-250528-micflan)  
  - [Validate FOCUS data](https://learn.microsoft.com/cloud-computing/finops/focus/validate?WT.mc_id=finops-250528-micflan)

### 2. FinOps Hubs Schema v1_2

- A new schema version, **v1_2**, debuts in FinOps hubs, aligning data ingestion and reporting to FOCUS 1.2.
- Backward compatibility is maintained via versioned functions. Users are guided to adopt these for stable reporting and automation.

More details: [Introducing non-breaking “breaking” changes in FinOps hubs 12](https://techcommunity.microsoft.com/blog/finopsblog/introducing-non-breaking-%E2%80%9Cbreaking%E2%80%9D-changes-in-finops-hubs-12/4438554)

### 3. Flexible CIDR Support for VNets

- Network deployment is now more adaptable, allowing custom CIDR block ranges (/8 to /26) for private endpoints and Power BI data gateways. This increases compatibility and helps avoid deployment blockers for diverse organizational needs.

### 4. Azure Data Explorer Auto-Start

- Cost optimization improves via pipeline automation: when new data arrives, the pipeline checks and starts Data Explorer clusters if they’re paused, minimizing manual intervention and unnecessary cloud costs.
- Detailed operational status and errors are surfaced in Azure Data Factory’s monitoring interface for transparency.
- Reference: [Automatic stop of inactive Azure Data Explorer clusters](https://learn.microsoft.com/azure/data-explorer/auto-stop-clusters)

### 5. Improved Security and Permissions Handling

- Managed exports now default to disabled, simplifying deployments in secure environments by removing the need for elevated (User Access Administrator) permissions unless explicitly required.
- Users can enable managed exports per deployment scenario.

### 6. Power BI Reporting Enhancements

- A new **Executive Summary** page in the Cost summary report aggregates key metrics for decision-makers: top subscriptions, resource groups, and services by spend, with visual analytics for actionable insights.
- The original summary is renamed to **Running total**, maintaining ongoing spend tracking.
- KQL and Power BI reports updated to leverage new data schemas for deeper insights.

### 7. PowerShell and Automation Updates

- PowerShell commands for Cost Management exports now default to the 2025-03-01 API version, ensuring alignment with the latest Azure Cost Management capabilities.
- FOCUS export version updated to 1.2-preview, making it easier to access the newest industry data features.
- Override dataset version quickly by setting `-DatasetVersion` as needed.

### 8. Minor Improvements and Documentation

- Expanded guidance on savings calculations and troubleshooting.
- More resource types and service mappings supported in open data modules.
- Direct links to the latest MS Learn documentation for deployment and optimization.

---

## What’s Next for FinOps Toolkit

Upcoming updates focus on greater automation, expanded reporting, continuous data model improvement, and integration of Azure Optimization Engine capabilities into the toolkit. Expect recurring updates to Power BI workbooks and community-driven feature enhancements.

If you need expert help in deploying or customizing the FinOps toolkit, or want to participate in the community, connect with Michael Flanakin via [LinkedIn](https://linkedin.com/in/flanakin) or Slack.

---

## Additional Resources

- [FinOps Toolkit Guide](http://aka.ms/finops/guide)
- [Dataset Metadata for FOCUS](https://github.com/microsoft/finops-toolkit/releases/latest/download/dataset-metadata.zip)

Browse the [FinOps Blog](https://techcommunity.microsoft.com/category/azure/blog/finopsblog) for more news and updates.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/finops-blog/what-s-new-in-finops-toolkit-12-july-2025/ba-p/4438562)
