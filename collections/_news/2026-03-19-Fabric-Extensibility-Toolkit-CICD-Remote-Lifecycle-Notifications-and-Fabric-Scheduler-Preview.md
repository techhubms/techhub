---
section_names:
- azure
- devops
- ml
- security
primary_section: ml
author: Microsoft Fabric Blog
date: 2026-03-19 14:10:00 +00:00
external_url: https://blog.fabric.microsoft.com/en-US/blog/fabric-extensibility-toolkit-ci-cd-remote-lifecycle-notifications-and-fabric-scheduler-preview/
tags:
- Azure
- Azure Pipelines
- CI/CD
- Delegated User Token
- Deployment Pipelines
- DevOps
- Environment Promotion
- Fabric Extensibility Toolkit
- Fabric Scheduler
- Git Integration
- GitHub Actions
- Item Definition
- Lakehouse
- Microsoft Entra ID
- Microsoft Fabric
- ML
- News
- On Behalf Of Token
- OneLake
- Remote Jobs
- Remote Lifecycle Notification API
- REST API
- Security
- Variable Library
- Webhook
- Workload Development
- Workload Manifest
- Workspace
title: 'Fabric Extensibility Toolkit: CI/CD, Remote Lifecycle Notifications, and Fabric Scheduler (Preview)'
feed_name: Microsoft Fabric Blog
---

Microsoft Fabric Blog announces new preview capabilities for the Fabric Extensibility Toolkit, covering Git and Deployment Pipelines support for custom workload items, Variable Library configuration per environment, webhook-based lifecycle notifications to backends, and scheduled “Remote Jobs” that run with delegated user tokens.<!--excerpt_end-->

## Overview

Alongside the **Microsoft Fabric Extensibility Toolkit** becoming **Generally Available (GA)**, this announcement introduces three new **preview** capabilities aimed at making custom Fabric workload items behave like native items across the platform lifecycle:

- **CI/CD support** for workload items in **Git integration** and **Deployment Pipelines**
- **Remote Lifecycle Notification API** (webhooks to your backend)
- **Fabric Scheduler (Remote Jobs)** (scheduled calls from Fabric to your backend)

The goal is to support two common workload-developer requests:

1. Native lifecycle behavior across the platform (CI/CD, Git, Deployment Pipelines)
2. Tight integration with backend services and operational scheduling

## CI/CD support: Items in Deployment Pipelines and Git (Preview)

Fabric’s CI/CD approach is built around:

- **Git integration** (version control)
- **Deployment Pipelines** (promoting changes across environments)

With this preview, **custom workload items participate in both without custom logic required**.

### Git integration for workload items

When a Fabric workspace is connected to a Git repo, **Fabric extensibility workload items are included automatically**.

Each workload item’s definition is serialized into the repository when committed, including:

- metadata
- configuration
- state

Example structure shown:

```text
workspace-git-repo/

MyItem.MyWorkloadItem/

.platform      # Fabric item metadata

definition.json # Example of your item’s persisted definition
```

What this enables:

- Reviewing **item state changes in pull requests** alongside code changes
- Using **branch strategies** to isolate development from production item data
- **Audited and version-controlled** configuration changes

### Deployment Pipelines for workload items

Workload items can now be promoted through **Deployment Pipelines** across **dev/test/prod workspaces**.

When an item is promoted:

- The item’s **definition content is carried forward** to the target workspace
- Fabric handles **workspace-specific adjustments** (examples given: connection rebinding, capacity differences)
- Partners can optionally implement a **deployment hook** to apply stage-specific transformations before the definition lands in the next stage

### Automated deployment in your pipelines

If you’re already using:

- **GitHub Actions**, or
- **Azure Pipelines**

…no additional tooling is required. Because workload items are now Git-native, existing commit + promotion automation “just works” for these items.

For REST automation, the standard **Deployment Pipelines REST API endpoints** now support custom workload items without special handling.

## Variable Library support

CI/CD promotion typically needs **different configuration per environment**, which is what **Variable Library** support targets.

A **Variable Library** is described as a **workspace-level store of named values** that can vary per deployment stage.

### The problem it addresses

If your workload item definition contains a hard-coded environment reference (example: a specific **Lakehouse ID**), then promoting dev → prod may still point to the dev Lakehouse.

### The approach

Instead of storing the Lakehouse ID directly in the item definition:

- Store a **variable name** in the definition
- Use the **Variable Library** to map that variable name to the correct Lakehouse ID per workspace (dev/test/prod)

Result:

- No hard-coded IDs
- No manual reconfiguration
- No custom hook logic required (for this specific case)

## Remote Lifecycle Notification API (Preview)

The **Remote Lifecycle Notification API** turns workload items into active participants by notifying your backend when item lifecycle events occur.

Key points:

- It is **opt-in** (if you don’t register an endpoint, nothing changes)
- You register a **notification endpoint** in your workload manifest
- Fabric calls your backend via webhook when an item is:
  - **Created**
  - **Updated**
  - **Deleted**

### What gets notified

The payload includes:

- event type (Created/Updated/Deleted)
- the item involved
- workspace context

Notifications happen in near real time regardless of the trigger path, including:

- Fabric UI actions
- Fabric REST API calls
- CI/CD and Deployment Pipeline promotions
- Admin cleanup/deletions

### Key use cases described

- **Licensing and entitlement checks**: verify entitlement, enforce seat limits, trigger activation flows before the item is opened
- **Infrastructure provisioning**: provision per-item resources (examples listed: database schema, container, API key, Azure resource group) on create; tear down on delete
- **Synchronization with external systems**: keep an external catalog/governance system in sync without polling

### Why it matters with CI/CD

Combined with CI/CD, lifecycle notifications ensure your backend stays aware as items move across stages. An item promoted through three pipeline stages triggers three notifications (one per workspace), enabling automated per-environment provisioning/reconfiguration.

## Fabric Scheduler: Remote Jobs (Preview)

The **Fabric Scheduler** (also called **Remote Jobs**) allows workload items to expose job types that users can schedule directly in Fabric.

Key points:

- It is **opt-in**
- Users schedule jobs using the standard Fabric scheduling UI (same experience as for **Notebooks** and **Dataflows**)
- When a schedule fires, Fabric calls a registered backend endpoint and includes:
  - **Item context** (item ID, workspace, and any relevant item definition data)
  - A delegated **user token**: an **On-Behalf-Of (OBO)** token scoped to the schedule owner, usable to call Fabric APIs, OneLake, or other Entra-protected services

Your backend runs the job, reports status back to Fabric, and results appear in Fabric’s standard job history UI.

### What Remote Jobs enable

Because the job runs with the user’s delegated identity, the post lists examples of what the job can do (subject to authorization):

- Read/write **OneLake** folders associated with the item
- Call the **Fabric REST API** to read/update item definitions
- Query external APIs/databases using the user’s identity
- Chain into other Fabric capabilities (examples: trigger Pipelines, read Lakehouses, write to Warehouses)

## Try it out

Both CI/CD support and Remote Jobs are included as **ready-to-use samples** in the **Starter Kit**.

- Opt-in, activated with a single setup flag
- Samples wire up manifest entries, backend endpoints, and frontend UI to demonstrate the end-to-end flow

Resources:

- Docs: https://aka.ms/fabric-extensibility-toolkit-docs
- Feedback: https://aka.ms/FabricBlog/ideas
- Community gallery: https://community.fabric.microsoft.com/t5/Extensibility-Toolkit-Gallery/bd-p/ac_extensibilitytoolkit_gallery


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/fabric-extensibility-toolkit-ci-cd-remote-lifecycle-notifications-and-fabric-scheduler-preview/)

