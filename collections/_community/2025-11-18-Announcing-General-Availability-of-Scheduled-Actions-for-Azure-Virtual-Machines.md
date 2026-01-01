---
layout: "post"
title: "Announcing General Availability of Scheduled Actions for Azure Virtual Machines"
description: "This article introduces Scheduled Actions, a resource provider for Azure Virtual Machines designed to simplify large-scale VM management. It explains how Scheduled Actions automates scheduling, scaling, and retrying power state operations across thousands of VMs, reducing manual overhead and improving operational reliability in Azure. Example scenarios and core benefits are detailed for cloud administrators."
author: "TravisCragg_MSFT"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-compute-blog/announcing-general-availability-of-scheduled-actions-for-azure/ba-p/4470797"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-18 17:35:21 +00:00
permalink: "/2025-11-18-Announcing-General-Availability-of-Scheduled-Actions-for-Azure-Virtual-Machines.html"
categories: ["Azure"]
tags: ["API Automation", "Azure", "Azure Compute", "Azure REST API", "Azure Virtual Machines", "Batch Operations", "Community", "Concurrent Operations", "Contoso Use Case", "Scheduled Actions", "Subscription Throttling", "VM Management"]
tags_normalized: ["api automation", "azure", "azure compute", "azure rest api", "azure virtual machines", "batch operations", "community", "concurrent operations", "contoso use case", "scheduled actions", "subscription throttling", "vm management"]
---

TravisCragg_MSFT presents Scheduled Actions for Azure Virtual Machines, outlining how this feature allows administrators to automate and scale VM power operations with simplified scheduling and retry logic.<!--excerpt_end-->

# Announcing General Availability of Scheduled Actions for Azure Virtual Machines

Managing virtual machines (VMs) at scale in Azure often requires handling subscription throttling, managing retries for transient failures, and maintaining operational efficiency. Scheduled Actions is a new resource provider designed to automate and simplify these processes for cloud administrators.

## What is Scheduled Actions?

Scheduled Actions acts as an orchestration layer above Azure's REST API, making API calls on your subscription's behalf at the time you specify. With this feature, you can manage the lifecycle of hundreds or thousands of VMs—start, stop, or hibernate—on recurring or ad hoc schedules. It streamlines the process by:

- Handling subscription-level throttling
- Managing retries for transient or intermittent errors
- Enabling scheduled or immediate execution of bulk VM operations

## Key Benefits

- **Scheduling**: Execute actions on VMs immediately or for a future time, supporting reliable automation.
- **Scale**: Perform up to 5,000 concurrent operations in a single action, ideal for enterprise environments.
- **Intelligent Throttling**: Automatically aligns API calls with your subscription's throttling limits to maximize throughput without exceeding quotas.
- **Automated Retries**: Tracks and retries transient failures automatically, increasing the success rate and reducing manual intervention.

## How Scheduled Actions Works

Scheduled Actions processes batch calls of up to 100 VMs at a time, supporting up to 5,000 VMs for a single action. For each VM, an operation ID is generated, which you can use to check status or audit execution. Actions can be immediately executed or scheduled for a future trigger time. Scheduled actions for future execution may also be cancelled as needed.

## Example Use Case: Contoso's VM Rollout

Contoso, preparing for a major weekend product launch, needs 3,000 Azure VMs started by 8:00 am and hibernated by 6:30 pm. With Scheduled Actions, their cloud team:

1. Submits 30 batches of 100 VM IDs to start at 7:45 am (ensuring all VMs are ready by 8).
2. Schedules another 30 batches to hibernate at 6:15 pm (so all VMs are safely off by 6:30).

This eliminates the need for custom automation scripts or manual monitoring, improving operational efficiency and allowing the team to focus on event delivery rather than infrastructure management.

## Getting Started

For setup instructions, detailed scenarios, and API documentation, visit the [Scheduled Actions documentation page](https://aka.ms/ScheduledActionsDocs).

---
*Article by TravisCragg_MSFT. Originally posted on the Azure Compute Blog.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-compute-blog/announcing-general-availability-of-scheduled-actions-for-azure/ba-p/4470797)
