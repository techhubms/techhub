---
layout: "post"
title: "Unlocking Flexibility in Fabric: Introducing Multiple Scheduler and CI/CD Support"
description: "Microsoft Fabric now supports Multiple Scheduler per item—up to 20 per item—empowering users with flexible scheduling and robust CI/CD integration. The update simplifies complex deployment scenarios, enhances monitoring, and paves the way for broader CRUD operations and UI enhancements."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/unlocking-flexibility-in-fabric-introducing-multiple-scheduler-and-ci-cd-support/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-08-05 09:00:00 +00:00
permalink: "/news/2025-08-05-Unlocking-Flexibility-in-Fabric-Introducing-Multiple-Scheduler-and-CICD-Support.html"
categories: ["Azure", "ML", "DevOps"]
tags: ["Automation", "Azure", "CI/CD", "Data Engineering", "Data Pipelines", "Deployment Pipelines", "DevOps", "Git Integration", "Job Types", "Microsoft Fabric", "ML", "Monitoring", "Multiple Scheduler", "News", "Scheduler API", "Version Control"]
tags_normalized: ["automation", "azure", "cislashcd", "data engineering", "data pipelines", "deployment pipelines", "devops", "git integration", "job types", "microsoft fabric", "ml", "monitoring", "multiple scheduler", "news", "scheduler api", "version control"]
---

This article by the Microsoft Fabric Blog details the new Multiple Scheduler and CI/CD support in Fabric, highlighting key enhancements for scheduling and deployment workflows.<!--excerpt_end-->

## Multiple Scheduler: A Game-Changer for Complex Scheduling Needs

In modern data environments, flexibility is essential. Until recently, Microsoft Fabric only allowed one scheduler per item. This limitation forced users to duplicate pipelines, manually configure jobs, or create fragile workarounds to meet real-life scheduling requirements.

**This limitation has now been removed.**

### Introducing Multiple Scheduler Support

Microsoft Fabric now supports Multiple Scheduler, allowing users to configure up to 20 schedulers per item, each with its own timetable.

![Multiple Scheduler Interface](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/08/image-4-835x1024.png)

### Why Multiple Scheduler Matters

This was a top ask (P0) from enterprise customers and repeatedly surfaced in Customer Advisory Team (CAT) surveys. It eliminates a longstanding flexibility gap that blocked migrations from other platforms like Azure Data Factory (ADF) and Synapse Analytics.

**Real-World Scenarios Now Addressed:**

- **Multiple Schedules for the Same Item**: For example, a pipeline used by multiple dashboards can now be triggered daily at 6 AM and weekly on Sundays—no duplication needed.
- **Different Job Types on the Same Item**: Items supporting several job types (such as refresh and publish) can now have separate schedulers for each, managed through API (with UI support forthcoming).

---

## Scheduler CI/CD: Deploy with Confidence

With added flexibility comes operational complexity—multiple schedulers mean more configuration and potential for deployment errors. To address this, Microsoft introduces comprehensive Scheduler CI/CD support:

### New Capabilities

- Schedulers are deployed alongside each item through Git, Deployment Pipelines, or public APIs.
- Configuration changes are tracked, versioned, and validated, just like any part of your item.

**Deployment Scenarios Now Supported:**

- **Git Integration**: Scheduler configurations live as `.json` files in the item directory and are reflected in Git status/diffs.

  ![Scheduler JSON Example](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/08/schedulerJSON.png)

- **Deployment Pipeline Support**: Changes are visible within deployment history and comparison tooling.
- **Public API**: Scheduler definitions are embedded, enabling full create/read/update/delete (CRUD) operations and version control.

### What's Available Now

- **Multiple Scheduler API Support**: Create, read, update, or delete up to 20 schedulers per item.
- **UI Support for Default Job Types**: Users can manage multiple schedulers directly from the Fabric UI.
- **CI/CD Integration**: Scheduler configurations are deployed, tracked, and validated with Git and pipelines.
- **Monitoring**: Job histories are separated per scheduler and error reporting is clear.

## Roadmap: What’s Coming Next

Microsoft Fabric plans to extend these capabilities further by:

- Adding full UI CRUD support for all job types
- More granular job history separation in the Monitor hub
- Enabling scheduler reuse across items
- Enhanced validation and better naming support

---

For in-depth guidance, consult the [Choose the best Fabric CI/CD workflow option for you](https://learn.microsoft.com/fabric/cicd/manage-deployment) in the Microsoft documentation.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/unlocking-flexibility-in-fabric-introducing-multiple-scheduler-and-ci-cd-support/)
