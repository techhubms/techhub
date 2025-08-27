---
layout: "post"
title: "Gain Deeper Insights into Spark Jobs with JobInsight in Microsoft Fabric"
description: "This article introduces JobInsight, a Java-based diagnostic library for Microsoft Fabric, enabling developers and data engineers to deeply analyze Spark jobs. It explains how to interactively access Spark queries, jobs, stages, and logs from Fabric Notebooks, automate post-run diagnostics, and store metrics in a Lakehouse or ADLS Gen2 for advanced troubleshooting and analytics."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/gain-deeper-insights-into-spark-jobs-with-jobinsight-in-microsoft-fabric/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-08-26 12:20:07 +00:00
permalink: "/2025-08-26-Gain-Deeper-Insights-into-Spark-Jobs-with-JobInsight-in-Microsoft-Fabric.html"
categories: ["Azure", "ML"]
tags: ["ADLS Gen2", "Automation", "Azure", "Big Data", "Data Analysis", "Data Engineering", "Event Logs", "Java", "JobInsight", "Lakehouse", "Microsoft Fabric", "ML", "News", "Notebook", "Performance Monitoring", "Resource Tuning", "Spark", "Spark Diagnostics"]
tags_normalized: ["adls gen2", "automation", "azure", "big data", "data analysis", "data engineering", "event logs", "java", "jobinsight", "lakehouse", "microsoft fabric", "ml", "news", "notebook", "performance monitoring", "resource tuning", "spark", "spark diagnostics"]
---

Microsoft Fabric Blog showcases how JobInsight empowers developers and data engineers to analyze Spark applications in Microsoft Fabric, offering metrics extraction, log diagnostics, and advanced troubleshooting directly within Fabric Notebooks.<!--excerpt_end-->

# Gain Deeper Insights into Spark Jobs with JobInsight in Microsoft Fabric

JobInsight is a powerful Java-based diagnostic library that streamlines the analysis of completed Spark jobs within Microsoft Fabric. Designed for both developers and data engineers, JobInsight provides direct programmatic access to Spark execution metrics and logs straight from a Fabric Notebook environment.

## Key Capabilities

### Interactive Spark Job Analysis

- Access in-depth Spark execution data, including queries, jobs, stages, tasks, and executors.
- Structured APIs return these objects as Spark Datasets for investigation, visualization, and custom analysis.

### Spark Event Log Access

- Easily copy Spark event logs to a OneLake or ADLS Gen2 directory.
- Ideal for long-term storage, custom diagnostics, or offline inspection.

## How to Use JobInsight

**Analyzing Completed Spark Applications:**

```scala
import com.microsoft.jobinsight.diagnostic.SparkDiagnostic

val jobInsight = SparkDiagnostic.analyze(
  workspaceId,
  artifactId,
  livyId,
  jobType,         // e.g., "sessions" or "batches"
  stateStorePath,  // Output path to store analysis results
  attemptId        // Optional; defaults to 1
)

val queries = jobInsight.queries
val jobs = jobInsight.jobs
val stages = jobInsight.stages
val tasks = jobInsight.tasks
val executors = jobInsight.executors
```

This enables direct, code-driven performance analysis, anomaly detection, and debugging inside Fabric.

**Reloading Previous Analyses:**

```scala
val jobInsight = SparkDiagnostic.loadJobInsight(stateStorePath)

val queries = jobInsight.queries
val jobs = jobInsight.jobs
```

This feature makes iterative and historical investigations efficient.

**Saving Metrics and Logs to a Lakehouse:**

```scala
val df = jobInsight.queries

df.write
  .format("delta")
  .mode("overwrite")
  .saveAsTable("sparkdiagnostic_lh.Queries")
```

Repeat for additional DataFrames (jobs, stages, etc.) as required.

## Copying Event Logs for In-Depth Analysis

Move raw Spark event logs to OneLake or ADLS Gen2 for long-term retention or advanced offline debugging:

```scala
import com.microsoft.jobinsight.diagnostic.LogUtils

val contentLength = LogUtils.copyEventLog(
  workspaceId,
  artifactId,
  livyId,
  jobType,
  targetDirectory,
  asyncMode = true, // Use async mode for better performance
  attemptId = 1
)
```

**Example Usage:**

```scala
val lakehouseBaseDir = "abfss://<workspace>@<onelake>/Files/eventlog/0513"
val jobType = "sessions"

copyEventLogs(workspaceId, artifactId, livyId, jobType, attemptId = 1, asyncMode = true, s"$lakehouseBaseDir/$jobType/async")
copyEventLogs(workspaceId, artifactId, livyId, jobType, attemptId = 1, asyncMode = false, s"$lakehouseBaseDir/$jobType/sync")
```

## Why Use JobInsight?

- Visualize Spark execution breakdowns by job, stage, or executor
- Monitor and tune resource utilization
- Identify and resolve Spark performance bottlenecks
- Save, reuse, and automate diagnostics workflows
- Seamless integration with Delta Lake and Lakehouse paradigms

For more details and documentation, refer to the [JobInsight diagnostics library (Preview)](https://review.learn.microsoft.com/fabric/data-engineering/user-guide?branch=pr-en-us-9703).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/gain-deeper-insights-into-spark-jobs-with-jobinsight-in-microsoft-fabric/)
