---
layout: "post"
title: "Automate Large-Scale VM Power Management with Azure Scheduled Actions"
description: "This guide explains how Azure Scheduled Actions enables managing the power state (start, stop, hibernate) of thousands of virtual machines at scale. Learn how this resource provider automates scheduling, handles throttling, manages retries for transient errors, and supports both immediate and future actions with operational transparency."
author: "TravisCragg_MSFT"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-compute-blog/handle-scale-for-power-state-operations-using-scheduled-actions/ba-p/4470131"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-17 21:36:26 +00:00
permalink: "/community/2025-11-17-Automate-Large-Scale-VM-Power-Management-with-Azure-Scheduled-Actions.html"
categories: ["Azure"]
tags: ["API Automation", "Azure", "Batch Operations", "Cloud Infrastructure", "Community", "Compute.schedule", "Error Handling", "High Scale", "Microsoft Azure", "Power Management", "Resource Provider", "Scheduled Actions", "Throttling", "Virtual Machines", "VM Operations"]
tags_normalized: ["api automation", "azure", "batch operations", "cloud infrastructure", "community", "computedotschedule", "error handling", "high scale", "microsoft azure", "power management", "resource provider", "scheduled actions", "throttling", "virtual machines", "vm operations"]
---

TravisCragg_MSFT introduces Azure Scheduled Actions, a tool for automating periodic power state operations across thousands of Azure VMs, highlighting automatic error handling and large-scale batch management.<!--excerpt_end-->

# Automate Large-Scale VM Power Management with Azure Scheduled Actions

Azure Scheduled Actions is a dedicated resource provider (compute.schedule) that streamlines bulk operations on Virtual Machines (VMs). It sits above the standard REST API, allowing you to schedule, automate, and manage large numbers of VM state changes (stop, start, hibernate) on a periodic basis, without having to handle complexity around subscription throttling or transient errors.

## What Are Scheduled Actions?

- **Resource Provider:** `compute.schedule` operates over Azure's REST API to execute scheduled power state changes on your behalf.
- **Supported Operations:** Stop, start, and hibernate VMs.
- **Scale:** Handles up to 5000 concurrent VM operations, using batch calls (up to 100 VMs per call), suitable for environments that require wide-reaching, timed infrastructure changes.

## Key Benefits

- **Automated Throttling and Retry:** Scheduled Actions abstracts the need to code around subscription-level throttling and transient error handling. Retries are managed for you, ensuring higher operational success rates.
- **High-Scale Operations:** Enable mass scheduling for power operations—no need to individually orchestrate calls or spread out volume.
- **Flexible Timing:** Execute actions immediately or schedule them for a future time. Scheduled actions can also be cancelled if needed.
- **Status Tracking:** Each VM operation generates an operation ID, making it possible to programmatically check the status of every action.

## How It Works

1. **Batch Submission:** Submit up to 5000 VMs in one operation using batch calls (max 100 VMs per call).
2. **Scheduling:** Select immediate or a specified future execution time for each action.
3. **Automatic Retries:** Transient and intermittent errors are detected and retried by the system.
4. **Operation Management:** Track progress or cancel future actions using generated operation IDs.

## Example Use Cases

- Automating nightly shutdowns or weekend de-allocations to save costs at scale.
- Coordinating mass startups before business hours or for compute-intensive batch jobs.
- Managing environments where manual orchestration of cloud infrastructure is infeasible due to scale.

## Getting Started

- Read the [Scheduled Actions documentation](https://aka.ms/ScheduledActionsDocs) for implementation steps, API references, and advanced scenarios.

---

**Author:** TravisCragg_MSFT

*Updated Nov 14, 2025. For more resources, visit the [Azure Compute Blog](https://techcommunity.microsoft.com/category/azure/blog/azurecompute).*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-compute-blog/handle-scale-for-power-state-operations-using-scheduled-actions/ba-p/4470131)
