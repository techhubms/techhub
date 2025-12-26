---
layout: "post"
title: "Enhanced Query Diagnostics in Azure Log Analytics"
description: "This announcement details new features in Azure Log Analytics designed to make query diagnostics and troubleshooting more effective. The enhancements include improved query notifications, actionable recommendations, a revamped Query Details pane with granular performance metrics, raw error payloads for debugging, and direct integration in the Azure Portal interface. These updates help users resolve query issues efficiently, optimize performance, and analyze log data more thoroughly within Azure Monitor."
author: "Ron Frenkel"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-observability-blog/introducing-the-enhanced-query-diagnostics-in-azure-log/ba-p/4466993"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-04 14:35:59 +00:00
permalink: "/community/2025-11-04-Enhanced-Query-Diagnostics-in-Azure-Log-Analytics.html"
categories: ["Azure"]
tags: ["Azure", "Azure Log Analytics", "Azure Monitor", "Azure Portal", "Community", "Error Handling", "Kusto Query Language", "Log Analytics Service", "Monitoring", "Performance Optimization", "Query Diagnostics", "Query Execution", "Query Performance", "Troubleshooting"]
tags_normalized: ["azure", "azure log analytics", "azure monitor", "azure portal", "community", "error handling", "kusto query language", "log analytics service", "monitoring", "performance optimization", "query diagnostics", "query execution", "query performance", "troubleshooting"]
---

Ron Frenkel introduces significant improvements to query diagnostics in Azure Log Analytics, offering actionable recommendations and deeper performance insights for users seeking to optimize and troubleshoot log queries within the Azure Portal.<!--excerpt_end-->

# Enhanced Query Diagnostics in Azure Log Analytics

Azure Log Analytics now features a streamlined and more powerful query diagnostics experience to help users quickly identify and resolve issues with their log queries.

## What's New?

### Improved Query Notification Experience

- Clear, concise query status notifications
- Direct explanations of errors and root causes
- Actionable recommendations and suggestions to address query issues
- Integrated links to advanced diagnostics via the Query Details pane

### Revamped 'Query Details' Pane

- **Overview Tab**: Presents key performance indicators, including:
  - Execution time breakdown (Engine, Log Analytics Service, Queue times)
  - Metrics such as Total CPU usage, Memory peak, and Response size
- **Raw Statistics Tab**: Contains complete execution details, referenced workspaces, and technical metadata
- **Errors Tab**: Gives access to raw error payloads for advanced debugging

## Benefits

- Enhanced visibility into every aspect of query execution
- Faster and more precise troubleshooting
- Optimization guidance directly built into the Azure Log Analytics workflow
- All features are accessible through the familiar Azure Portal interface

## Resources

- [Optimize log queries in Azure Monitor – Microsoft Learn](https://learn.microsoft.com/en-us/azure/azure-monitor/logs/query-optimization)

## About the Author

Ron Frenkel shares these updates on behalf of the Azure Observability team. For more content and updates, follow the [Azure Observability Blog](https://techcommunity.microsoft.com/t5/azure-observability-blog/bg-p/AzureObservabilityBlog).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-observability-blog/introducing-the-enhanced-query-diagnostics-in-azure-log/ba-p/4466993)
