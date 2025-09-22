---
layout: "post"
title: "Microsoft Fabric Spark Applications Comparison Feature (Preview)"
description: "Explore the preview release of the Spark Applications Comparison feature in Microsoft Fabric. This update enables developers and data engineers to visually compare multiple Spark application runs, analyze performance metrics, debug regressions and improvements, and optimize resource usage. The feature integrates with Spark L2 Monitoring, providing a rich toolset for deep analysis and performance troubleshooting within modern data engineering workflows."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/public-preview-announcement-fabric-spark-applications-comparison/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-09-22 09:30:00 +00:00
permalink: "/2025-09-22-Microsoft-Fabric-Spark-Applications-Comparison-Feature-Preview.html"
categories: ["Azure", "ML"]
tags: ["Apache Spark", "Application Comparison", "Azure", "Data Engineering", "Debugging", "L2 Monitoring", "Microsoft Fabric", "ML", "Monitoring Tools", "News", "Performance Monitoring", "Regression Analysis", "Resource Optimization", "Run Metrics", "Spark Applications"]
tags_normalized: ["apache spark", "application comparison", "azure", "data engineering", "debugging", "l2 monitoring", "microsoft fabric", "ml", "monitoring tools", "news", "performance monitoring", "regression analysis", "resource optimization", "run metrics", "spark applications"]
---

Microsoft Fabric Blog introduces the Spark Applications Comparison feature, allowing developers and data engineers to analyze, debug, and optimize Spark application runs by comparing key performance metrics side by side.<!--excerpt_end-->

# Microsoft Fabric Spark Applications Comparison (Preview)

The Spark Applications Comparison feature is now available in preview on Microsoft Fabric. This tool is designed to empower developers and data engineers to:

- Analyze, debug, and optimize Apache Spark performance across multiple application runs
- Track changes and improvements due to code updates or data variations

## What Is Spark Applications Comparison?

This feature allows users to select up to four Spark application runs and compare them side by side. By presenting key execution and resource metrics for each run, it becomes easier to:

- Identify performance regressions or improvements
- Spot anomalies and root causes
- Visualize metric deltas compared to a baseline run

### Key Capabilities

- Compare runs from the same artifact (Notebook or Spark Job Definition)
- View differences in execution time, data trends, and resource usage
- Debug problematic runs by inspecting metric changes

## The Compare Panel

The Compare Panel presents a comprehensive breakdown of relevant performance and resource metrics, such as:

- Start time
- Running duration
- Queued/total duration
- Executor CPU time and run time
- Input/output data size and record counts
- Disk and memory spill
- Shuffle write volume and record count

For outlier or anomalous runs, root causes and key resource bottlenecks are highlighted to help focus performance investigations.

## Deep Dive: Spark Monitoring L2 Integration

Each run in the comparison view links to the Spark L2 Monitoring page, which provides:

- Detailed job/stage breakdowns
- Logs and configuration history
- Fine-grained control for root cause analysis and performance optimization

## How To Use It

1. Go to the Monitor Run Series page in Microsoft Fabric
2. Select 'Compare Runs' tab
3. Choose up to four completed Spark runs. One serves as the base run, while the rest are compared against it (the base run can be reassigned)
4. Review the highlighted changes and metrics in the Compare Panel

For further analysis, investigate individual runs using the linked L2 Monitoring tool.

## Getting Started

To explore this feature, visit the Monitor Run Series page in Microsoft Fabric, select multiple completed runs, and access the Compare Panel. This enables you to track application evolution and optimize workflows.

For more details, see the [official documentation](https://learn.microsoft.com/fabric/data-engineering/spark-comparison-runs).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/public-preview-announcement-fabric-spark-applications-comparison/)
