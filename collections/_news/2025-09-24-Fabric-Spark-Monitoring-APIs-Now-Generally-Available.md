---
external_url: https://blog.fabric.microsoft.com/en-US/blog/general-availability-announcement-fabric-spark-monitoring-apis/
title: Fabric Spark Monitoring APIs Now Generally Available
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-09-24 09:00:00 +00:00
tags:
- APIs
- Application Diagnostics
- Automation
- Data Engineering
- Dynamic Allocation
- Microsoft Fabric
- Monitoring
- Observability
- Performance Tuning
- Resource Usage API
- Scalable Workloads
- Spark
- Spark Advisor API
- Spark Monitoring APIs
- Vcore Usage
section_names:
- azure
- ml
primary_section: ml
---
Microsoft Fabric Blog introduces the Generally Available Fabric Spark Monitoring APIs, enabling granular insight, resource optimization, and advanced observability for Spark workloads on Microsoft Fabric.<!--excerpt_end-->

# Fabric Spark Monitoring APIs Now Generally Available

The Fabric Spark Monitoring APIs deliver enhanced observability and automation for Spark workloads running within Microsoft Fabric. This release marks the APIs' move to general availability, meaning they are ready for production use and equipped with new features based on user feedback.

## What's New

- **Spark Advisor API**: Offers actionable recommendations and skew diagnostics to help identify bottlenecks and improve performance.
- **Resource Usage API**: Provides detailed metrics on vCore allocation and utilization for each executor in a Spark application.
- **Advanced Filtering**: The workspace-level API now supports filtering applications by time range, submitter, and state (Succeeded, Failed, Running), among others, helping teams conduct efficient and targeted analyses.

## Enhanced Application-Level Properties

The Spark Monitoring APIs now expose deeper configuration and usage details for each application:

- Driver Cores & Memory
- Executor Cores & Memory
- Number of Executors
- Dynamic Allocation Enabled
- Dynamic Allocation Max Executors

These additions empower engineering teams and data scientists to better plan resources, monitor allocations, and optimize Spark workload performance at scale.

## Observability and Automation in Practice

This update establishes the Spark Monitoring APIs as a comprehensive solution for monitoring, diagnosing, and optimizing Spark jobs in large Microsoft Fabric environments. Enhanced automation, targeted analysis, and detailed metrics all support improved reliability and operational insight.

> For further documentation and implementation details, visit [Monitor Spark applications using Spark monitoring APIs](https://learn.microsoft.com/fabric/data-engineering/spark-monitoring-api-overview).

## Continuous Improvement Commitment

Microsoft thanks the community for its feedback in shaping these APIs and encourages users to try out the new features available in this GA release. Expect ongoing updates as Microsoft continues to enhance Spark observability capabilities within Fabric.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/general-availability-announcement-fabric-spark-monitoring-apis/)
