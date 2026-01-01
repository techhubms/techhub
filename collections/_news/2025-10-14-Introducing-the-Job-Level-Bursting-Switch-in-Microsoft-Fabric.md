---
layout: "post"
title: "Introducing the Job-Level Bursting Switch in Microsoft Fabric"
description: "This announcement from the Microsoft Fabric Blog introduces the Job-Level Bursting Switch, a new feature allowing administrators to control how Spark jobs utilize burst capacity in Microsoft Fabric. The switch helps balance peak job performance and high concurrency, adapting to various data engineering and data science workloads. It outlines configuration scenarios, billing considerations, and provides direct links to documentation for effective Spark resource management."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/introducing-the-job-level-bursting-switch-in-microsoft-fabric/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-10-14 07:00:00 +00:00
permalink: "/2025-10-14-Introducing-the-Job-Level-Bursting-Switch-in-Microsoft-Fabric.html"
categories: ["Azure", "ML"]
tags: ["Autoscale Billing", "Azure", "Burst Capacity", "Capacity Administrator", "Compute Units", "Concurrency", "Data Engineering", "Data Science", "ETL Workloads", "Fabric Capacity", "Interactive Notebooks", "Job Level Bursting", "Microsoft Fabric", "ML", "News", "Performance Optimization", "Resource Management", "Spark"]
tags_normalized: ["autoscale billing", "azure", "burst capacity", "capacity administrator", "compute units", "concurrency", "data engineering", "data science", "etl workloads", "fabric capacity", "interactive notebooks", "job level bursting", "microsoft fabric", "ml", "news", "performance optimization", "resource management", "spark"]
---

The Microsoft Fabric Blog team announces a new Job-Level Bursting Switch for Spark compute in Fabric, granting administrators enhanced control over burst capacity and concurrency optimization.<!--excerpt_end-->

# Introducing the Job-Level Bursting Switch in Microsoft Fabric

Microsoft Fabric Blog introduces a new feature designed to give administrators more granular control over Spark compute resources: the **Job-Level Bursting Switch**. This feature enables precise tuning of how Spark jobs use burst capacity, promoting either peak job performance or maximizing concurrency to suit a wide range of workloads for data engineering and science.

## What Is Job-Level Bursting?

- **Compute Units (CUs) in Fabric** offer a 3× bursting capability, allowing a Spark job to temporarily utilize more compute power than the base capacity.
- Bursting accelerates jobs during intensive operations, ensuring better use of available resources.

## Using the 'Disable job-level bursting' Switch

- **Where to Find:**
  - Admin Portal → Capacity Settings → [Select Capacity] → Data Engineering/Science Settings → Job Management

- **Enabled (default):**
  - A single Spark job can leverage up to three times the base compute units, ideal for large, compute-hungry tasks such as ETL processes.
  - Example: In F64 capacity, jobs can use 192 CUs instead of being limited to 64.
- **Disabled:**
  - Each Spark job is limited to base capacity, improving fair usage across multiple users and supporting better concurrency for shared environments.

## Scenarios and Examples

| Scenario                        | Setting             | Behavior                                                                                  |
|----------------------------------|---------------------|-------------------------------------------------------------------------------------------|
| Heavy ETL Workload               | Bursting enabled    | Job utilizes full burst (e.g., 192 CUs in F64), accelerating execution.                   |
| Multi-user Interactive Notebooks | Bursting disabled   | Job usage capped (e.g., 64 CUs in F64), facilitating high concurrency and responsiveness. |
| Autoscale Billing enabled        | Bursting unavailable| All Spark usage billed on demand (pay-as-you-go); no reserved bursting.                   |

### Important Billing Consideration

- The switch is only available for Spark jobs on **Fabric Capacity**.
- If **Autoscale Billing** is enabled, this control is unavailable because all Spark usage is charged on demand.

## Choosing the Right Setting

- **Enable bursting** when you have large workloads or need to maximize throughput for critical pipelines.
- **Disable bursting** for interactive sessions or collaborative environments, ensuring resources are shared across users.

## Resources and Documentation

- [Job bursting control in Capacity Settings](https://learn.microsoft.com/fabric/data-engineering/capacity-settings-management#admin-control-job-level-bursting-switch)
- [Concurrency limits and queueing in Microsoft Fabric Spark](https://learn.microsoft.com/fabric/data-engineering/spark-job-concurrency-and-queueing)
- [Job queueing for Fabric Spark](https://learn.microsoft.com/fabric/data-engineering/job-admission-management)

This feature, combined with tools like Optimistic Job Admission, offers enhanced flexibility for building efficient and responsive data solutions in Microsoft Fabric.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/introducing-the-job-level-bursting-switch-in-microsoft-fabric/)
