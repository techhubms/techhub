---
section_names:
- ml
primary_section: ml
author: Microsoft Fabric Blog
date: 2026-03-19 14:15:00 +00:00
external_url: https://blog.fabric.microsoft.com/en-US/blog/33772/
tags:
- Custom Live Pool
- Data Engineering
- Dependency Management
- Environment Publishing
- Fabric Environments
- Full Mode
- Inline Installation
- JAR
- Library Management
- Microsoft Fabric
- ML
- News
- Notebook Startup Time
- Notebooks
- Production Workloads
- Quick Mode
- Reproducible Environments
- Resources Folder
- Scheduled Jobs
- Spark
- Wheel
title: Best Practices for Library Management with Fabric Environments
feed_name: Microsoft Fabric Blog
---

Microsoft Fabric Blog explains how to manage Spark libraries in Microsoft Fabric Environments, comparing Quick mode, Full mode, custom live pools, the Resources folder, and inline installation to balance fast iteration with stable, reproducible production runs.<!--excerpt_end-->

## Overview

Microsoft Fabric provides multiple ways to add and manage libraries when developing with Spark. With the release of **Quick mode** in **Environments**, you can choose a more flexible and performance-oriented approach to library management.

The main goal is to pick the right strategy to:

- Reduce environment publish time
- Speed up notebook startup
- Improve production stability

> If you haven’t already, check out Arun Ulag’s blog “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for a complete look at FabCon and SQLCon announcements across Fabric and database offerings.

![Animated GIF of the Environment interface showing libraries being added through quick mode and full mode](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/animated-gif-of-the-environment-interface-showing.gif)

## Adding libraries in Microsoft Fabric Environments

Fabric Environments support several approaches for adding/managing libraries, optimized for different scenarios (fast iteration, production stability, or custom package control).

### Summary of options

| Option | Typical use case | Environment publish | Starting session | Key benefit |
| --- | --- | --- | --- | --- |
| Full mode | Production workloads; scheduled jobs; shared, stable environments | 3–6 minutes (depends on library size/complexity) | 1–3 minutes waiting time to deploy the library snapshot (depends on library size/complexity) | Downloads and resolves dependencies; ensures a stable, reproducible library snapshot |
| Full mode with custom live pool | Production workloads that require fast session start | 3–6 minutes (includes custom live pool creation) | ~5 seconds | Stable snapshot with a fast, live session experience |
| Quick mode | Notebook users; lightweight dependencies; development/experimentation | ~5 seconds | Extra time to install libraries when the notebook session starts (depends on library size/complexity) | Instant publishing for rapid iteration; Quick mode libraries override Full mode libraries when duplicates exist |
| Resources folder | Quick validation of a custom package; small resources (for example, a .py module); per notebook or shared folder | Not affected by environment publishing | Installed manually from the Resources folder | Direct access to files/libraries in notebooks; easy management of small/custom assets |
| Inline installation | One-off testing; debugging; temporary experiments | Not affected by environment publishing | Installed manually using inline commands | Immediate feedback within the notebook session |

## Choosing a method by development stage

The recommended approach is to iterate quickly early on, then stabilize and make runs reproducible for production.

## Scenario: Starting a new project in Fabric

When requirements are uncertain and dependencies change frequently:

- Use **inline installation** to quickly test packages directly in notebooks.
- Use the **Resources folder** to store custom wheels or JARs you want to reuse during early exploration.
- Once dependencies become clearer:
  - Use **Quick mode** for lightweight or frequently changing libraries.
  - Use **Full mode** for common or heavier dependencies you expect to share across notebooks.

This lets you validate ideas quickly without paying environment publishing cost for every change, then gradually stabilize as the project matures.

## Scenario: Iterating on an existing environment

When you need to update library versions or introduce new dependencies while keeping the environment usable:

- Keep existing libraries in **Full mode** unchanged to preserve the current snapshot.
- Add new libraries or updated versions to **Quick mode**.
- **Quick mode libraries install at notebook session start** and **override Full mode libraries** for that session.

This supports fast publishing and testing while still benefiting from the stable Full mode snapshot. After validation, you can promote changes into Full mode.

## Scenario: Production workloads

For production, consistency and reproducibility matter most:

- Move validated libraries from **Quick mode** to **Full mode**.
- **Publish** the environment to generate a stable, validated library snapshot.
- Decide whether to use a **custom live pool** based on performance and isolation needs.

This helps ensure all notebooks/jobs run against the same library set, reducing runtime surprises.

## Key takeaways

- Default to **Quick mode for development** and **Full mode for production**.
- Use the **Resources folder** for custom packages or small resources.
- Treat **inline installation** as temporary.
- Combining modes is the recommended and supported best practice.

## Reference

- Library management documentation: https://learn.microsoft.com/fabric/data-engineering/environment-manage-library


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/33772/)

