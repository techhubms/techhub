---
external_url: https://blog.fabric.microsoft.com/en-US/blog/enhanced-monitoring-for-spark-high-concurrency-workloads-in-microsoft-fabric/
title: Enhanced Monitoring for Spark High Concurrency Workloads in Microsoft Fabric
author: Microsoft Fabric Blog
viewing_mode: external
feed_name: Microsoft Fabric Blog
date: 2025-08-26 12:04:04 +00:00
tags:
- Big Data
- Data Engineering
- Debugging
- Developer Productivity
- Distributed Computing
- Execution Model
- High Concurrency
- Job Monitoring
- Logs
- Microsoft Fabric
- Notebook Monitoring
- Notebook Snapshots
- Observability
- Performance Tuning
- Pipeline Integration
- Spark
section_names:
- azure
- ml
---
Microsoft Fabric Blog announces enhanced monitoring for Spark workloads running in high concurrency mode. Discover new features for job insights, notebook-aware logging, and hierarchical views to boost developer productivity.<!--excerpt_end-->

# Enhanced Monitoring for Spark High Concurrency Workloads in Microsoft Fabric

Microsoft Fabric introduces a suite of monitoring improvements for Notebooks operating in high concurrency mode. These new features give developers and data engineers better visibility, easier debugging, and improved performance management for Spark across collaborative workloads.

## Key Enhancements in the Spark Application Monitoring Detail Page

### Jobs Tab: Detailed Job-Level Insights

- See Notebook names associated with each Spark job, aiding multi-notebook contexts.
- Access and copy job-related code using the new code snippet viewer.
- Filter Spark jobs by Notebook to focus on relevant workloads within a session.

### Logs Tab: Notebook-Aware Logging

- All log entries are now prefixed with the Notebook ID, making it easier to trace activity.
- Filter logs by Notebook to inspect outputs and errors for individual or collaborative runs.

### Item Snapshots Tab: Hierarchical Notebook View

- Use the hierarchical tree to browse all participating Notebooks within a Spark session.
- View detailed snapshots of completed and in-progress Notebook runs, including:
  - Code at submission time
  - Cell-level execution status
  - Output for each cell
  - Notebook input parameters
- If running as part of a pipeline, see related pipeline and Spark activity for robust traceability.

## Start Exploring

Visit the Monitoring Hub in Microsoft Fabric and open a Spark application running in high concurrency mode to try out the improved Jobs, Logs, and Item Snapshots. These features support multi-notebook workflows and aim to streamline performance tuning, debugging, and collaborative analytics.

For feedback and future improvements, reach out through the official channels.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/enhanced-monitoring-for-spark-high-concurrency-workloads-in-microsoft-fabric/)
