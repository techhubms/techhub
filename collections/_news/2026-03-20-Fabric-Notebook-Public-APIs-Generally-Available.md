---
tags:
- Authentication
- Automation
- CI/CD
- CRUD
- Data Engineering
- Data Science
- DevOps
- Execution Monitoring
- Exit Values
- External Orchestrators
- Fabric Notebooks
- Job Scheduler API
- Lakehouse
- Microsoft Fabric
- ML
- News
- Notebook Orchestration
- Parameterized Runs
- Public APIs
- REST API
- Run Notebook API
- Service Principal
- Workspace
section_names:
- devops
- ml
title: Fabric Notebook Public APIs (Generally Available)
external_url: https://blog.fabric.microsoft.com/en-US/blog/fabric-notebook-public-apis-generally-available/
primary_section: ml
feed_name: Microsoft Fabric Blog
date: 2026-03-20 11:40:00 +00:00
author: Microsoft Fabric Blog
---

Microsoft Fabric Blog announces general availability of Fabric Notebook Public APIs, covering CRUD management and on-demand notebook execution via the Job Scheduler API, including exit values and service principal authentication for CI/CD and orchestration scenarios.<!--excerpt_end-->

# Fabric Notebook Public APIs (Generally Available)

*If you haven’t already, check out Arun Ulag’s hero blog “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for a complete look at all of our FabCon and SQLCon announcements across both Fabric and our database offerings.*

Fabric Notebook Public APIs are now **generally available (GA)**, enabling data engineers and data scientists to **programmatically manage and execute notebooks** as part of modern, automated data workflows.

With this release, Fabric notebooks become fully addressable through public APIs—from **CRUD operations** to **on-demand execution**—unlocking scenarios across **CI/CD**, orchestration, and system integration. The **Run Notebook API** now supports **returning an exit value**, making it easier to build robust conditional pipelines around notebook execution.

## Why notebook public APIs matter

Notebooks are central to how teams explore data, build transformations, and develop analytics logic. Previously, integrating notebooks into automated systems often required workarounds or platform-specific tooling.

With Fabric Notebook Public APIs, you can:

- Treat notebooks as first-class programmable assets
- Automate notebook lifecycle management at scale
- Execute notebooks on demand and use their results to drive downstream decisions
- Integrate Fabric notebooks with external orchestrators, CI/CD systems, and enterprise platforms

## Full lifecycle management with CRUD APIs

Fabric Notebook Public APIs provide full **Create, Read, Update, and Delete (CRUD)** support for notebook items in a workspace.

Supported actions include:

- Creating notebooks
- Updating notebook content
- Retrieving notebook definitions
- Listing notebooks in a workspace
- Deleting notebooks

These capabilities make it easier to:

- Build CI/CD workflows
- Sync notebooks across environments
- Manage notebooks programmatically at scale

## Run notebooks on demand with the Job Scheduler API

Beyond management, the public APIs enable running notebooks on demand using the **Job Scheduler API**, together with **Cancel job instance** and **Get job instance**.

You can:

- Parameterize notebook runs
- Customize session configuration
- Specify environments and lakehouses
- Monitor execution status
- Cancel runs if needed

This allows notebooks to be invoked as part of larger workflows—whether from Fabric pipelines, external schedulers, or custom automation services.

## New in GA: return exit values for better orchestration

A key enhancement in this GA release is support for returning an [exit value](https://learn.microsoft.com/fabric/data-engineering/notebook-utilities#exit-a-notebook) from notebook runs.

This enables notebooks to communicate structured outcomes back to the caller, making it easier to implement:

- Conditional branching
- Success or failure signaling
- Downstream orchestration logic

With exit values, notebooks can act as decision-making units within complex data pipelines.

## Secure automation with Service Principal support

Notebook Public APIs support **service principal authentication** for both CRUD and execution APIs, enabling secure, unattended automation and enterprise-grade CI/CD integrations.

## Get started

To explore the API surface and usage examples, see:

- [Items – REST API (Core) | Microsoft Learn](https://learn.microsoft.com/rest/api/fabric/core/items)
- [Job Scheduler – REST API (Core) | Microsoft Learn](https://learn.microsoft.com/rest/api/fabric/core/job-scheduler)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/fabric-notebook-public-apis-generally-available/)

