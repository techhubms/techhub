---
tags:
- Dataflows Gen2
- Email Notifications
- Failure Notifications
- Job Scheduler
- Job Scheduler API
- Microsoft Fabric
- ML
- Monitoring Hub
- News
- Notebooks
- Operational Monitoring
- Pipelines
- REST API
- Run History
- Scheduled Jobs
- Scheduling
author: Microsoft Fabric Blog
section_names:
- ml
external_url: https://blog.fabric.microsoft.com/en-US/blog/get-notified-when-scheduled-jobs-fail-in-fabric-generally-available/
primary_section: ml
title: Get notified when scheduled jobs fail in Fabric (Generally Available)
date: 2026-03-31 14:00:00 +00:00
feed_name: Microsoft Fabric Blog
---

Microsoft Fabric Blog announces a generally available feature in Microsoft Fabric: email failure notifications for scheduled jobs, configurable per item schedule and supported across Pipelines, Notebooks, Dataflows Gen2, and other schedulable Fabric items.<!--excerpt_end-->

## Overview

Microsoft Fabric now supports **email notifications for failed scheduled jobs**. The goal is to help teams detect failures earlier and respond before they cause stale data, broken downstream processes, or other issues.

## Why this matters

Scheduled jobs usually run in the background, so failures can go unnoticed unless someone manually checks run history or discovers problems downstream.

Failure notifications can help you:

- Spot failed runs sooner without manually checking run history
- Reduce downtime when a job fails
- Keep the right people informed when attention is needed

## Set up failure notifications

Failure notifications are configured alongside an item’s schedules:

1. Open your item in Microsoft Fabric.
2. Select **Schedule** to open the schedule configuration.
3. Under **Failure notifications**, add the users or groups who should be notified when a scheduled run fails.

These notification settings apply to **all schedules for that item**, so you only need to configure them once.

![Screenshot of the Schedule tab in Item Settings for a Pipeline in Fabric showing Failure notifications recipients](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-the-schedule-tab-in-item-settings-fo.png)

*Figure: Failure notifications for a Pipeline schedule.*

## How failure notifications work

- Works for **all Fabric items that support scheduling**, including:
  - Pipelines
  - Notebooks
  - Dataflows Gen2
  - (and other schedulable Fabric items)
- Schedules created either:
  - In the **Fabric portal**, or
  - Via the **Job Scheduler API**

If a **scheduled run** fails, a notification is sent. **Manually triggered runs** do **not** send failure notifications.

Email notifications include error details and a link to the failed run in the **Monitoring Hub** to support investigation and next steps.

## Learn more

- Job Scheduler API: https://learn.microsoft.com/rest/api/fabric/core/job-scheduler/create-item-schedule?tabs=HTTP
- Job Scheduler documentation: https://aka.ms/ScheduledJobNotifications


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/get-notified-when-scheduled-jobs-fail-in-fabric-generally-available/)

