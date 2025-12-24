---
layout: "post"
title: "Experimentation Analytics with Statsig in Microsoft Fabric"
description: "This article introduces Statsig Experimentation Analytics, a new feature available as a third-party workload in Microsoft Fabric. It explains how product and analytics teams can design experiments, connect exposure data, define custom metrics, and conduct statistical tests directly on their Fabric data without data movement. The post provides a step-by-step overview, highlighting integration with Power BI, real-time incremental analysis, and secure data governance through OneLake. Readers will learn how to leverage experimentation analytics for data-driven product decisions inside the Microsoft Fabric ecosystem."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/27219/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-10-02 13:00:00 +00:00
permalink: "/news/2025-10-02-Experimentation-Analytics-with-Statsig-in-Microsoft-Fabric.html"
categories: ["Azure", "ML"]
tags: ["A/B Testing", "Azure", "Control Group", "Data Science", "Experimentation Analytics", "Feature Rollouts", "Frequentist Testing", "KPI", "Microsoft Fabric", "ML", "News", "OneLake", "Power BI", "Product Analytics", "Sequential Testing", "Statistical Analysis", "Statsig", "Variant Group"]
tags_normalized: ["aslashb testing", "azure", "control group", "data science", "experimentation analytics", "feature rollouts", "frequentist testing", "kpi", "microsoft fabric", "ml", "news", "onelake", "power bi", "product analytics", "sequential testing", "statistical analysis", "statsig", "variant group"]
---

Microsoft Fabric Blog presents an overview of Statsig Experimentation Analytics, explaining how product and analytics teams can run experiments, analyze data, and drive product decisions securely within the Fabric platform.<!--excerpt_end-->

# Experimentation Analytics with Statsig in Microsoft Fabric

Statsig Experimentation Analytics, now available in Microsoft Fabric, enables product and analytics teams to design, execute, and analyze experiments without leaving the Fabric ecosystem. This integration empowers users to:

- **Unify experimentation, feature rollout, and impact analysis** through a single experience
- **Analyze product and behavioral data** stored in OneLake with no data movement
- **Design and analyze experiments** with custom metrics such as user engagement, retention, and funnel analysis
- **Connect exposure data seamlessly** within Fabric, maintaining data governance and security
- **Run rigorous statistical tests**, including A/B/n testing, Frequentist analysis, confidence intervals, sequential testing, and CURE for variance control
- **Monitor experiments in real time**, with automatic daily data refresh and an up-to-date experimentation scorecard
- **Act on insights** by greenlighting winning variants and using Power BI dashboards for product rollout decisions

## How It Works

1. **Accessing Statsig Analytics**
   - Find Statsig Analytics in the Fabric Workload Hub, among third-party workloads.
   - Native integration with OneLake ensures your data remains secure and governed.

2. **Setting Up Metrics and Connections**
   - Connect to your Fabric Data Warehouse in your workspace.
   - Define metrics sources using Statsig's Metrics Explorer (e.g., user engagement, revenue, checkout CTR).

3. **Experiment Assignment and Data Pull**
   - Connect assignment sources, importing exposure data for both control and variant groups.
   - Track and analyze exposures to various experiments over time.

4. **Statistical Testing and Analysis**
   - Leverage advanced statistical tools to validate experiment outcomes.
   - Drill into deep analysis, funnel analytics, and hypothesis testing.
   - Automatic and incremental collection updates results daily for real-time monitoring.

5. **From Insights to Actions**
   - Use Power BI dashboards to visualize metrics and quickly identify winning variants.
   - Make well-informed, data-driven product decisions and stage feature rollouts.

## Who Benefits?

- **Product Managers**: Design and validate new features more confidently and quickly.
- **Data Scientists / Analytics Leaders**: Run complex experiments and analyses natively on Fabric data.

## Getting Started

- Access the Workload Hub in Microsoft Fabric, select Statsig Analytics, and follow the steps to integrate with your data warehouse and Power BI.

For licensing and more details, visit [Statsig’s Product page](https://ms.portal.azure.com/#view/Microsoft_Azure_Marketplace/MarketplaceOffersBlade/selectedMenuItemId/home) on Azure Marketplace.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/27219/)
