---
tags:
- Aggregations
- Bronze Silver Gold
- Change Data Feed
- Common Table Expressions
- Data Engineering
- Data Lineage
- Data Quality Constraints
- DataFrameWriter
- Dependency Management
- ETL Pipelines
- GROUP BY
- Incremental Refresh
- Lakehouse
- Left Outer Join
- Left Semi Join
- Materialized Lake Views
- Medallion Architecture
- Microsoft Fabric
- ML
- MLVs
- News
- Optimal Refresh
- PySpark
- Scheduled Refresh
- Spark SQL
- User Defined Functions
external_url: https://blog.fabric.microsoft.com/en-US/blog/materialized-lake-views-in-microsoft-fabric-generally-available/
primary_section: ml
section_names:
- ml
title: Materialized Lake Views in Microsoft Fabric (Generally Available)
feed_name: Microsoft Fabric Blog
author: Microsoft Fabric Blog
date: 2026-03-18 16:30:00 +00:00
---

Microsoft Fabric Blog announces Materialized Lake Views (MLVs) are now generally available in Microsoft Fabric, explaining how data engineers can build medallion (bronze/silver/gold) Lakehouse pipelines with Spark SQL or PySpark, get incremental refresh via an optimal refresh engine, add multi-schedule orchestration, and enforce data quality rules at scale.<!--excerpt_end-->

## Overview

Materialized Lake Views (MLVs) are now **generally available** in **Microsoft Fabric**. They are designed to help data engineers replace hand-built ETL pipelines with **declarative** transformations defined in **Spark SQL** or **PySpark**, while Fabric manages orchestration, dependencies, and monitoring.

This GA release focuses on making MLVs “production-ready at scale” by closing key preview gaps: **multi-schedule support**, broader **incremental (optimal) refresh** coverage, **PySpark authoring (preview)**, **in-place updates (Replace)**, and expanded **data quality controls**.

## What are Materialized Lake Views?

A **materialized lake view** in Fabric is:

- A **persisted** view (materialized result)
- **Automatically refreshed**
- Defined via **Spark SQL** or **PySpark**

Fabric also:

- Tracks **dependencies between MLVs**
- Orchestrates refreshes in the **correct order**
- Enforces **data quality constraints** at each stage

This supports multi-stage Lakehouse transformations commonly described as **medallion architecture** (bronze → silver → gold), expressed as declarative statements instead of custom Spark jobs.

![Screenshot of a data workflow diagram within a software interface showing interconnected nodes representing datasets and processes related to materialized lake views. Nodes are labeled with terms like "silverweather_events," "bronze_airline_fares_data," and "gold_flight_performance," connected by arrows indicating data flow, with a sidebar listing data sources and a top menu bar for managing runs and reports. AI-generated content may be incorrect.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-data-workflow-diagram-within-a-sof.gif)

## Broader clause coverage for optimal (incremental) refresh

A core MLV goal is to refresh **only what changed** rather than recompute everything.

MLVs can now refresh incrementally when the definition includes:

- Aggregations such as **COUNT** and **SUM** with **GROUP BY**
- **Left outer joins**, **left semi joins**
- **Common table expressions (CTEs)**

### Automatic decision engine

You don’t manually decide whether a refresh should be incremental:

- An **optimal refresh** decision engine evaluates each run
- It compares the amount of changed data vs the cost of full recompute
- It automatically chooses the **faster** approach

**Change Data Feed** is enabled by default on every new MLV (no additional configuration required).

## PySpark authoring support for MLVs (Preview)

Data engineers can now create, refresh, and replace MLVs from Fabric notebooks using **PySpark** and the **DataFrameWriter API**.

PySpark authoring is aimed at cases where teams need more than Spark SQL alone:

- Custom cleansing using **Python libraries**
- Calling **UDFs** that wrap business rules or ML models
- Multi-step transformations mixing procedural code with DataFrame operations

![Materialized lake views "New" menu in Fabric Lakehouse showing "Create with PySpark (Python)" option along with Spark SQL](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/materialized-lake-views-new-menu-in-fabric-lakeh.png)

### PySpark MLV capabilities

PySpark-authored MLVs support:

- Data quality constraints (including expression-based rules and **session-scoped UDFs**)
- Table properties
- Scheduled refreshes from the same notebook used for preparation/exploration

**Current limitation:** PySpark-authored MLVs perform a **full refresh** on each run today. Optimal refresh for PySpark MLVs is planned.

Documentation: https://learn.microsoft.com/fabric/data-engineering/materialized-lake-views/create-materialized-lake-view-pyspark

## Multi-schedule support

Previously, lakehouse MLV refresh was limited to a **single schedule** for all views, which pushed some teams to notebook-based workarounds.

The post highlights problems with notebook-triggered refreshes:

- Bypasses dependency management
- Centralized error reporting and retry logic aren’t used
- Failures can be hidden in notebook cell output
- Downstream views aren’t aware of upstream failures

### What changes with multi-schedule

You can now define **named schedules** within a lakehouse, each targeting a subset of views, for example:

- Finance gold layer hourly
- Lower-priority analytics every six hours

When a named schedule runs, Fabric:

- Refreshes upstream dependencies in the right order
- Runs independent views in parallel
- Surfaces errors centrally

If a run is already in progress when a schedule fires, the new run is **skipped** and the next window proceeds.

![Materialized lake views management page showing new Schedules panel on the right with multiple schedules and available controls like run, edit and toggle option](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/materialized-lake-views-management-page-showing-ne.png)

## In-place view updates with Replace

Previously, changing an MLV definition meant dropping and recreating the view, which could:

- Lose refresh history
- Force downstream consumers to reconnect

With **Replace**, you can update the definition in place:

- Fabric validates the new logic
- Swaps it in while preserving identity/metadata/lineage
- Downstream dependencies remain intact

Replace works for both SQL and PySpark-authored MLVs.

## Stronger data quality rules

Data quality enforcement is expanded beyond preview.

For PySpark-authored MLVs, constraints can now:

- Use expression-based logic combining multiple columns
- Apply arithmetic and built-in functions in one rule
- Invoke **session-scoped UDFs** for validation logic implemented in Python

Fabric tracks constraint outcomes across refreshes and provides a **data quality report** so teams can see:

- Which rules fail most often
- Which views are affected
- Trend changes over time

## What’s ahead

The post calls this release a milestone and lists ongoing work:

- Optimal refresh for PySpark-authored MLVs
- Expanded optimal refresh coverage for additional SQL operators
- Deeper integration with other Fabric workloads

Feedback channels:

- Fabric Ideas portal: https://ideas.fabric.microsoft.com/

## Get started

MLVs are available in every Microsoft Fabric workspace.

- Docs (quick starts + API reference): https://learn.microsoft.com/fabric/data-engineering/materialized-lake-views/get-started-with-materialized-lake-views
- End-to-end medallion tutorial (MLVs + Shortcut Transformations): https://learn.microsoft.com/fabric/data-engineering/materialized-lake-views/tutorial
- Submit ideas/requests: https://ideas.fabric.microsoft.com/
- Event: FabCon & SQLCon 2026 (Atlanta, March 16–20): https://aka.ms/FabCon

Related announcement roundup mentioned in the post:

- FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform: https://aka.ms/FabCon-SQLCon-2026-news


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/materialized-lake-views-in-microsoft-fabric-generally-available/)

