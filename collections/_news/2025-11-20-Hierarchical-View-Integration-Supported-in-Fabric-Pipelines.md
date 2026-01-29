---
external_url: https://blog.fabric.microsoft.com/en-US/blog/hierarchical-view-integration-supported-in-pipelines/
title: Hierarchical View Integration Supported in Fabric Pipelines
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-11-20 10:00:00 +00:00
tags:
- Analytics Integration
- Data Engineering
- Data Orchestration
- Downstream Jobs
- Hierarchical View
- Job Dependencies
- Microsoft Fabric
- Monitoring Hub
- Operational Visibility
- Orchestration
- Pipeline Runs
- Pipelines
- Troubleshooting
- Upstream Jobs
- Workflow Management
- Azure
- Machine Learning
- News
section_names:
- azure
- ml
primary_section: ml
---
Microsoft Fabric Blog explains how Hierarchical View in Monitoring Hub empowers users to monitor and troubleshoot complex pipeline workflows by visualizing upstream and downstream job dependencies.<!--excerpt_end-->

# Hierarchical View Integration Supported in Fabric Pipelines

Managing orchestration workflows with multiple automated jobs and complex dependencies can be difficult. The Hierarchical View in Microsoft Fabric's Monitoring Hub is designed to give users complete visibility and control over pipeline runs.

## Why Hierarchical View Matters

Pipelines frequently involve several interdependent jobs. When issues arise, it's vital to understand both upstream and downstream effects to efficiently resolve problems. Without a clear layered view, troubleshooting can become unnecessarily time-consuming.

The Hierarchical View offers a structured representation of pipeline runs, allowing users to:

- **Navigate job layers:** Seamlessly move between upstream and downstream jobs within a single pipeline.
- **Trace dependencies:** Quickly locate related jobs and understand their connections, enabling root cause analysis and preventing cascading problems.

## How It Works

1. **Access Monitoring Hub:** Open the Monitoring Hub page in Microsoft Fabric.
2. **Enable Hierarchical Columns:** Use the column options to toggle on the Upstream and Downstream run columns.
3. **Explore Pipeline Runs:** The page displays a clear hierarchy showing which jobs were triggered, their outcomes, and relationships.

You can drill into details of each job run:

- View upstream run details to see preceding actions.
- View downstream run details to track dependencies and triggered jobs.
- Move smoothly across related jobs and pages, improving operational oversight.

## Practical Benefits

For example, if a pipeline refreshes a dataset and then triggers a dashboard update:

- If the dashboard update fails, Hierarchical View reveals the specific downstream job responsible.
- Users can immediately verify the upstream dataset refresh completion.
- Navigation between run details is direct, so troubleshooting is faster and more accurate.

### Key Advantages

- **Reduce operations risk:** Identify issues and their root causes in seconds.
- **Complete workflow visibility:** Understand intricate orchestration at a glance.
- **Efficient monitoring:** No more guesswork or page hopping—confidence in operation management is improved.

## Learn More

To try Hierarchical View:

- Go to Monitoring Hub in Microsoft Fabric.
- Turn on Upstream and Downstream run columns in the options.
- Explore your pipeline runs using the hierarchical layout.

For further guidance, see [How to monitor pipeline runs in Monitoring hub](https://learn.microsoft.com/fabric/data-factory/monitoring-hub-pipeline-runs).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/hierarchical-view-integration-supported-in-pipelines/)
