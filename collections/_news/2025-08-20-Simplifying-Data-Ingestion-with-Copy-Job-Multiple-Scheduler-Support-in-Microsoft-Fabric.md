---
external_url: https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-multiple-scheduler-support/
title: 'Simplifying Data Ingestion with Copy Job: Multiple Scheduler Support in Microsoft Fabric'
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-08-20 09:00:00 +00:00
tags:
- Bulk Copy
- CI/CD
- Copy Job
- Data Engineering
- Data Factory
- Data Ingestion
- Deployment Pipelines
- Git Integration
- Incremental Copy
- Microsoft Fabric
- Multiple Scheduler
- Orchestration
- Public APIs
- Azure
- ML
- News
section_names:
- azure
- ml
primary_section: ml
---
The Microsoft Fabric Blog outlines recent improvements to Copy Job in Data Factory, with multiple scheduler support enabling advanced data ingestion scheduling and orchestration by the platform team.<!--excerpt_end-->

# Simplifying Data Ingestion with Copy Job: Multiple Scheduler Support in Microsoft Fabric

Copy Job serves as a central solution in Microsoft Fabric Data Factory for seamless data movement. It supports multiple delivery styles, including:

- **Bulk copy**
- **Incremental copy**
- **Change data capture (CDC) replication**

The latest update introduces robust **multiple scheduler support**, enabling:

- Triggering a single Copy job on different schedules simultaneously (e.g., daily at 6 AM and weekly on Sundays)
- Consolidating previously separate jobs into unified configurations
- More efficient, simplified orchestration of recurring data ingestion tasks

Previously, orchestrating varied schedules required creating separate Copy jobs for each trigger. With the new feature, you now configure all desired schedules directly within a single Copy job.

**Integration and Deployment:**

Multiple schedulers can be deployed with Copy job using:

- **Git integration**
- **Deployment Pipelines**
- **Public APIs**

This ensures streamlined CI/CD and reproducible data engineering workflows.

**Additional Resources:**

- [Unlocking Flexibility in Fabric: Introducing Multiple Scheduler and CI/CD Support](https://blog.fabric.microsoft.com/en-us/blog/unlocking-flexibility-in-fabric-introducing-multiple-scheduler-and-ci-cd-support?ft=All)
- [Microsoft Fabric Copy Job Documentation](https://learn.microsoft.com/fabric/data-factory/what-is-copy-job)
- [Fabric Ideas portal](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas/label-name/data%20factory%20%7C%20copy%20job)
- [Microsoft Fabric Community](https://community.fabric.microsoft.com/t5/Copy-job/bd-p/db_copyjob)
- [Fabric documentation](https://aka.ms/FabricBlog/docs)

**Feedback:**

Have questions or ideas? Share feedback via the [Fabric Ideas portal](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas/label-name/data%20factory%20%7C%20copy%20job) or join the conversation in the Fabric Community.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-multiple-scheduler-support/)
