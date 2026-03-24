---
external_url: https://blog.fabric.microsoft.com/en-US/blog/configure-and-manage-activator-rules-directly-in-eventstream-preview/
section_names:
- ml
tags:
- Aggregations
- Event Driven Automation
- Eventstream
- Fabric Activator
- Microsoft Fabric
- ML
- News
- Pattern Detection
- Preview Feature
- Real Time Intelligence
- Rule Conditions
- Streaming Data
- Threshold Monitoring
- Workflows
feed_name: Microsoft Fabric Blog
title: Configure and manage Activator rules directly in Eventstream (Preview)
author: Microsoft Fabric Blog
primary_section: ml
date: 2026-03-24 15:00:00 +00:00
---

Microsoft Fabric Blog announces a preview that lets you create and manage Fabric Activator rules directly inside Eventstream, reducing context switching when turning streaming events into alerts and automated actions.<!--excerpt_end-->

# Configure and manage Activator rules directly in Eventstream (Preview)

Customers use **Eventstream** in **Microsoft Fabric** to ingest and process streaming data at scale, and **Fabric Activator** to monitor conditions and trigger actions when thresholds or patterns are detected. This update brings those experiences closer together by letting you **create and manage Activator rules directly within Eventstream**.

Previously, creating an alert required switching from Eventstream to Activator. Now, alert creation is embedded in Eventstream to simplify the workflow.

Related docs:

- Eventstream overview: https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/create-manage-an-eventstream
- Fabric Activator intro: https://learn.microsoft.com/fabric/real-time-intelligence/data-activator/activator-introduction
- Set alerts on an eventstream: https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/set-alerts-event-stream

## What’s new

- Create **Activator rules** directly inside **Eventstream**.
- Define **rule conditions** for values you want to monitor in events.
- Configure **actions** such as notifications or workflows.
- View and manage **all rules** associated with an Eventstream.

## Why this matters

- **Faster setup and reduced context switching**: create alerts while you configure your Eventstream.
- **Full visibility**: see all rules tied to a specific Eventstream for better operational clarity.

## How it works

When building or editing an Eventstream, you can:

1. Select the stream you want to monitor.
2. Choose **Set Alert**.
3. Define your condition (thresholds, aggregations, patterns).
4. Configure the action.
5. Create the rule.

![Screenshot showing Set Alert directly inside Eventstream](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/this-image-shows-how-you-can-set-alert-directly-in.png)

*Figure: Set alert inside Eventstream*

### If you already have an Activator destination on the Eventstream

1. Select the **Activator** node.
2. Select the **Rule** icon.
3. Create the rule.

![Screenshot showing creating an alert inside Eventstream from the Activator destination](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/this-image-shows-how-you-can-create-an-alert-insid.png)

*Figure: Add rule inside Eventstream in Activator destination*

## Managing rules

After creating rule(s) on an Eventstream, you can manage them by editing, deleting, or opening them in Activator.

![Screenshot showing managing rules for an Eventstream](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/this-image-shows-how-you-can-manage-rules-for-even.png)

*Figure: Manage rules inside Eventstream*

### By opening a rule in Activator, you can

- **Manage the rule**: view and manage rules and actions created on your data, build additional business objects to validate hypotheses, and modify/delete them.
- **Edit the rule**: tweak settings and get an instant preview of how edits affect activations.
- **Monitor and analyze the rule**: view step-by-step computation and review activation history.
- **Test/activate the rule**: send a test alert and check chat messages in Teams.

## Share your feedback

- Try the features in Fabric: https://app.fabric.microsoft.com/
- Join the discussion: http://aka.ms/ActivatorCommunity

For broader conference announcements referenced in the post, see: https://aka.ms/FabCon-SQLCon-2026-news


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/configure-and-manage-activator-rules-directly-in-eventstream-preview/)

