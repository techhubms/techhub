---
layout: "post"
title: "Fabric Spark Run Series Analysis: Enterprise-Scale Observability for Microsoft Fabric Spark Jobs"
description: "This post introduces Fabric Spark Run Series Analysis, now generally available in Microsoft Fabric. The feature provides enterprise-scale observability for recurring Spark job executions, offering outlier detection, performance analysis, and optimization with integrations like Autotune. Readers will learn about key capabilities, access points, and how Run Series Analysis supports more proactive, data-driven tuning of Spark workloads."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/fabric-spark-run-series-analysis-generally-available/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-09-23 09:00:00 +00:00
permalink: "/2025-09-23-Fabric-Spark-Run-Series-Analysis-Enterprise-Scale-Observability-for-Microsoft-Fabric-Spark-Jobs.html"
categories: ["Azure", "ML"]
tags: ["Anomaly Detection", "Autotune", "Azure", "Data Engineering", "Enterprise Data", "Microsoft Fabric", "ML", "Monitoring", "News", "Notebook Executions", "Observability", "Performance Tuning", "Pipeline Monitoring", "Run Series Analysis", "Spark", "Spark Job Definitions", "Spark SQL"]
tags_normalized: ["anomaly detection", "autotune", "azure", "data engineering", "enterprise data", "microsoft fabric", "ml", "monitoring", "news", "notebook executions", "observability", "performance tuning", "pipeline monitoring", "run series analysis", "spark", "spark job definitions", "spark sql"]
---

Microsoft Fabric Blog presents Fabric Spark Run Series Analysis, a feature designed to enhance recurring Spark job observability and performance optimization within Microsoft Fabric's data engineering platform.<!--excerpt_end-->

# Fabric Spark Run Series Analysis: Enterprise-Scale Observability for Microsoft Fabric Spark Jobs

Fabric Spark Run Series Analysis is a Microsoft Fabric feature created to provide robust observability for recurring Apache Spark job executions. Building upon previous announcements such as the Autotune feature, this tool is generally available and aims to help users understand, compare, and optimize Spark performance at scale.

## What Is Spark Run Series Analysis?

Spark Run Series Analysis groups Spark application runs—whether from recurring pipeline activities, Notebook executions, Spark Job Definitions (SJDs), or Autotune-enabled runs—into automatically identified run series. This process assists users in:

- Detecting outliers and anomalies across recurring runs
- Understanding execution time trends and variations in data input/output
- Evaluating the impact of Autotune recommendations
- Reviewing Spark SQL configurations and performance breakdowns over time

This holistic view is crucial for data engineers seeking to troubleshoot issues, identify inefficiencies, and optimize execution performance over time.

## Key Capabilities

- **Run Series Comparison**: Compare execution durations of Spark runs within the same series, drilling down to identify causes for variation in performance—such as differences in input/output data.
- **Outlier Detection and Analysis**: Detect anomalous runs automatically and surface factors like resource constraints or configuration changes as potential contributors.
- **Detailed Run Instance View**: For each run, access detailed metrics on execution phases and view applied configuration values (including Autotune recommendations) to spot optimization opportunities.
- **Support for Running Applications**: Gain insights on Spark jobs even while they are still in progress.

## Access Points in Microsoft Fabric

- **Monitoring Hub’s Historical View:** Gives an overview of recurring jobs and performance trends.
- **Recent Runs Panel:** Accessed through Notebooks or Spark Job Definitions.
- **Spark Application Monitoring Detail Page:** For deep-dive, single-application analytics.

## Driving Smarter Performance Tuning

The GA release of Spark Run Series Analysis now empowers Microsoft Fabric users to be proactive and data-driven in Spark workload tuning. It enables anomaly investigation, runtime trend comparison, and assessment of Autotune's impact—driving efficiency for recurring production-scale Spark jobs.

**Further Reading:** [Monitor Apache Spark run series (Microsoft Learn)](https://learn.microsoft.com/fabric/data-engineering/apache-spark-monitor-run-series)

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/fabric-spark-run-series-analysis-generally-available/)
