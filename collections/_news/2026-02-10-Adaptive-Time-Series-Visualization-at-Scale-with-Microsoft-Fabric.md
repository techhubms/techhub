---
external_url: https://blog.fabric.microsoft.com/en-US/blog/adaptive-time-series-visualization-at-scale-with-microsoft-fabric/
title: Adaptive Time Series Visualization at Scale with Microsoft Fabric
author: Microsoft Fabric Blog
primary_section: ml
feed_name: Microsoft Fabric Blog
date: 2026-02-10 10:00:00 +00:00
tags:
- Aggregation
- Anomaly Detection
- Azure
- Data Engineering
- Data Visualization
- DirectQuery
- Dynamic M Query Parameters
- Industrial Data
- IoT Analytics
- KQL Database
- Microsoft Fabric
- ML
- News
- Operational Analytics
- Power BI
- Real Time Analytics
- Semantic Model
- Solution Accelerator
- Time Series Analysis
section_names:
- azure
- ml
---
Microsoft Fabric Blog, with coauthor Slava Trofimov, presents a comprehensive approach to scalable time series visualization by leveraging Microsoft Fabric, KQL databases, and Power BI for interactive, real-time analytics on massive datasets.<!--excerpt_end-->

# Adaptive Time Series Visualization at Scale with Microsoft Fabric

Coauthor: Slava Trofimov

Industrial operations generate massive volumes of time series data, creating challenges for real-time, interactive analysis by users of all technical backgrounds. This guide explains how to use an innovative, Microsoft Fabric-native design pattern that overcomes data volume and complexity hurdles through advanced visualization and database techniques.

## Challenges

- Billions of sensor readings per month from large plants or IoT operations
- High data volumes, query latency, performance bottlenecks
- Complex tools requiring deep technical skills

## Key Solution: Fabric-Native Pattern Combining KQL Databases & Power BI

- **KQL Databases:** Store and manage vast event streams, aggregate data, perform anomaly detection, and execute fast queries.
- **Power BI Integration:** Delivers fast, flexible visualizations for diverse users (engineers, data scientists, business users).

### Main Features

- **Intelligent Time Binning:** Automatically selects optimal intervals based on date range to efficiently handle billions of data points.
- **Time Brushing:** Interactive zoom-in capabilities using custom visuals (Time Series Brush Slicer).
- **Multi-metric Comparison:** Analyze multiple time series in parallel.
- **Flexible Aggregation:** Toggle between average, min, max, and sum metrics easily.
- **Anomaly Detection:** KQL database queries surface unusual patterns without requiring ML expertise.
- **Statistical Insights:** Gain descriptive statistics, correlations, and asset hierarchy context directly within Power BI reports.

## Component Architecture

- **Semantic Model in DirectQuery Mode:** Delegates query processing to KQL database, handling real-time, high-volume data.
- **Dynamic M Query Parameters:** Slicers/filters feed user input into Power Query, tailoring queries to user selections.
- **Power Query Functions:** Compute optimal time bins dynamically as users zoom in or out.
- **Field Parameters:** Let users customize chart layouts.
- **Time Brush Custom Visual:** Offers intuitive, interactive selection of time windows on reports.

## Process Overview

1. **Initial Selections:** User sets assets/tags, time range, and aggregations in Power BI.
2. **Parameter Binding:** User settings are mapped to dynamic M query parameters.
3. **Query Generation:** Power Query builds customized KQL queries with suitable binning and filters.
4. **Data Processing:** KQL Database executes queries using functions like `make-series` and `series_decompose_anomalies`.
5. **Result Transfer:** Only relevant, summarized data is returned to Power BI for display.
6. **Interactive Exploration:** Users refine analyses iteratively using the brush slicer and report controls.

Diagrams in the original blog illustrate how user actions propagate through Power BI, Power Query, and KQL database layers, enabling detailed and responsive visualizations over massive time ranges.

## Getting Started

- **Solution Accelerator:** Rapidly try the approach using a pre-built report and public sample data ([basic solution](https://slavatrofimov.github.io/Time-Series-Visualization-with-Microsoft-Fabric/getting-started/basic-solution/)).
- **Advanced Sample:** A more flexible, customizable sample is also available for deeper implementation ([advanced solution](https://slavatrofimov.github.io/Time-Series-Visualization-with-Microsoft-Fabric/getting-started/advanced-solution/)).

## Resources

- [Documentation for the solution accelerator](https://slavatrofimov.github.io/Time-Series-Visualization-with-Microsoft-Fabric/) on GitHub Pages
- [Source code of the solution accelerator](https://github.com/slavatrofimov/Time-Series-Visualization-with-Microsoft-Fabric) on GitHub
- [Time brushing custom visual](https://github.com/slavatrofimov/Time-Series-Brush-Slicer) on GitHub

---
This pattern lets organizations democratize operational analytics, combining the scalability of KQL with the accessibility of Power BI and Microsoft Fabric.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/adaptive-time-series-visualization-at-scale-with-microsoft-fabric/)
