---
layout: "post"
title: "Copy Job Activity Now in Preview for Microsoft Fabric Data Factory Pipelines"
description: "This news article introduces the Copy job Activity now in Preview within Microsoft Fabric Data Factory pipelines. It details how the new orchestration activity simplifies data movement and ingestion, integrates monitoring features, supports flexible data orchestration with other pipeline activities, and enhances data workflow automation for data engineers and architects working on the Microsoft Fabric platform."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/now-in-public-preview-copy-job-activity-in-pipelines/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-08-25 08:00:00 +00:00
permalink: "/news/2025-08-25-Copy-Job-Activity-Now-in-Preview-for-Microsoft-Fabric-Data-Factory-Pipelines.html"
categories: ["Azure", "ML"]
tags: ["Azure", "Copy Job Activity", "Data Factory", "Data Ingestion", "Data Orchestration", "Data Pipelines", "Incremental Copy", "Medallion Architecture", "Microsoft Fabric", "ML", "Monitoring", "News", "No Code Data Engineering", "Pipeline Automation"]
tags_normalized: ["azure", "copy job activity", "data factory", "data ingestion", "data orchestration", "data pipelines", "incremental copy", "medallion architecture", "microsoft fabric", "ml", "monitoring", "news", "no code data engineering", "pipeline automation"]
---

Microsoft Fabric Blog announces the Preview release of Copy job Activity, a new orchestration feature in Data Factory pipelines, enabling streamlined data movement and workflow automation for engineers working on Microsoft Fabric.<!--excerpt_end-->

# Copy Job Activity Now in Preview for Microsoft Fabric Data Factory Pipelines

Microsoft has announced the Preview release of the Copy job Activity in Microsoft Fabric Data Factory pipelines. This new feature brings the simplicity of the Copy job item directly into Data Factory pipelines, allowing users to manage and orchestrate data movement operations alongside transformations, notifications, and other activities—all in a unified environment.

## Key Features

- **No-Code Simplicity**: Integrate existing or new Copy jobs easily from the pipeline canvas without writing code.
- **Flexible Orchestration**: Chain Copy jobs with other pipeline activities, such as notebooks, dataflows, conditional logic, and even Power BI refresh events. This supports complex architectures, including medallion patterns.
- **Comprehensive Monitoring**: Real-time monitoring links offer visibility into job progress, performance, and execution outcomes, making troubleshooting more efficient.
- **Batch and Incremental Copy Support**: Utilize batch copy, incremental copy, and Change Data Capture (CDC) within a single activity, addressing various data delivery requirements.
- **Integrated Email Notifications**: Combine Copy job activity with Outlook activity to send pipeline notifications and status updates.

## Copy Activity vs. Copy Job Activity

While both activities facilitate data movement, the new Copy job Activity is a significant enhancement:

- Faster and simpler to get started with data movement operations.
- Supports advanced features requested by users such as incremental copy and CDC replication.
- Enables reuse of business logic with conditional execution, retries, and chaining in complex workflows.

## Automation and Orchestration Benefits

This activity is ideal for quick prototypes or for orchestrating complex, end-to-end data workflows in Microsoft Fabric. It brings together data movement, integration, and monitoring capabilities making it easier for data engineers to manage and observe their pipelines.

## Additional Resources

- [Copy job Activity in Data Factory pipelines](https://learn.microsoft.com/fabric/data-factory/copy-job-activity)
- [What is Copy job in Data Factory](https://learn.microsoft.com/fabric/data-factory/what-is-copy-job)
- [Blog: Simplifying Data Ingestion with Copy job](https://blog.fabric.microsoft.com/blog/simplifying-data-ingestion-with-copy-job-incremental-copy-ga-lakehouse-upserts-and-new-connectors)

For more details, visit the official Microsoft documentation or the Microsoft Fabric Blog.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/now-in-public-preview-copy-job-activity-in-pipelines/)
