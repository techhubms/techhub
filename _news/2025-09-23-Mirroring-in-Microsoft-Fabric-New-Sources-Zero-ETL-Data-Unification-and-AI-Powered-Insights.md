---
layout: "post"
title: "Mirroring in Microsoft Fabric: New Sources, Zero-ETL Data Unification, and AI-Powered Insights"
description: "This news update from the Microsoft Fabric Blog details recent enhancements to the Mirroring feature in Microsoft Fabric. It covers new generally available and preview data sources, zero-ETL data access, integration with Power BI, secure connectivity improvements, chat-driven analytics, and expanded developer extensibility, all aimed at simplifying analytics and AI/BI workflows."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/whats-new-to-mirroring-new-sources-and-capabilities-for-all-your-zero-etl-needs/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-09-23 10:00:00 +00:00
permalink: "/2025-09-23-Mirroring-in-Microsoft-Fabric-New-Sources-Zero-ETL-Data-Unification-and-AI-Powered-Insights.html"
categories: ["AI", "Azure", "ML"]
tags: ["AI", "Analytics", "Azure", "Azure SQL Managed Instance", "Data Agent", "Data Governance", "Data Integration", "Data Unification", "Delta Lake", "Enterprise Data", "Google BigQuery", "Microsoft Fabric", "Mirroring", "ML", "News", "On Premises Gateway", "OneLake", "Open Mirroring", "Oracle", "Power BI", "Preview Features", "Semantic Models", "VNet", "Zero ETL"]
tags_normalized: ["ai", "analytics", "azure", "azure sql managed instance", "data agent", "data governance", "data integration", "data unification", "delta lake", "enterprise data", "google bigquery", "microsoft fabric", "mirroring", "ml", "news", "on premises gateway", "onelake", "open mirroring", "oracle", "power bi", "preview features", "semantic models", "vnet", "zero etl"]
---

Microsoft Fabric Blog discusses major updates to Mirroring, enabling seamless zero-ETL data unification for analytics and AI. This update, authored by the Microsoft Fabric Blog team, highlights new source support, secure connectivity, and chat-driven analytics with mirrored data.<!--excerpt_end-->

# Mirroring in Microsoft Fabric: New Sources, Zero-ETL Data Unification, and AI-Powered Insights

Microsoft Fabric has significantly evolved its Mirroring functionality, making data unification across analytics and AI workloads easier by enabling zero-ETL access to a growing set of sources.

## Key Updates

- **New Source Support (Preview & GA):**  
  - Google BigQuery and Oracle are now available in preview as mirrored data sources.
  - Azure SQL Managed Instance is generally available (GA) for mirroring.
  - Secure support for Snowflake, Azure SQL Database, and Azure SQL Managed Instance behind firewalls via VNet and on-premises gateways.

- **Business Impact:**  
  Organizations like James Hall & Co. Ltd. are reporting reduced infrastructure consumption, rapid report refreshes, and the ability to deliver broader actionable insights. With Open Mirroring, onboarding new datasets is frictionless, empowering better business outcomes.

> “We are currently mirroring nearly half a billion rows across 50 tables. This data services over 30 reports for 400+ users, giving the business insight into Sales, Stock, and Wastage... getting data into Fabric is no longer a concern and we can focus fully on delivering value through the data itself.”  
> — Steve Pritchard, James Hall & Co. Ltd.

- **Developer Extensibility:**
  - The Open Mirroring platform allows developers and ISVs to create custom mirroring solutions, including UI support for delimited files (.csv, .txt), with ongoing partner ecosystem expansion.

- **Enabling Advanced Analytics and AI:**
   - Users can create semantic models directly from the Mirrored Database UI or in Power BI Desktop.
   - Chat-driven analytics now in Data Agent (preview): users can interact with mirrored data conversationally for rapid insights, similar to querying Lakehouse data.

- **Upcoming Features:**
   - Delta Change Data Feed (CDF) will simplify historical change tracking.
   - Snowflake Mirroring will expand to additional asset types (starting with views).
   - Enhanced security/governance with OneLake Security and improved partition handling in Open Mirroring.

## Getting Started

- **Connect New Sources:** Preview support is available for Oracle (including OCI and Exadata) and Google BigQuery. Straightforward configuration—no ETL required.
- **Secure and Manage:** Use workspace identity authentication for improved control. Leverage on-premises gateway and VNet support for secure connections.
- **Build and Extend:** Developers can integrate custom mirroring solutions using Open Mirroring, with support for a broader range of partners.
- **Analytics and AI Enablement:**  
   - Build semantic models from mirrored databases for use in Power BI.
   - Leverage chat capabilities within Data Agent for interactive insights.

## Roadmap Highlights

- Delta Change Data Feed support for advanced change tracking
- Expanded Snowflake asset mirroring (views and more)
- Enhanced data governance and security
- Extended partner and integration capabilities in Open Mirroring

## Feedback

Share your experience or feedback through the Microsoft Fabric community channels to shape the future of data mirroring and analytics in Microsoft Fabric.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/whats-new-to-mirroring-new-sources-and-capabilities-for-all-your-zero-etl-needs/)
