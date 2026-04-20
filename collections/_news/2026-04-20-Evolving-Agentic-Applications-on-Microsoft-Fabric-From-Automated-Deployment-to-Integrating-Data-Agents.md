---
date: 2026-04-20 10:00:00 +00:00
section_names:
- ai
- azure
- devops
- ml
tags:
- Access Control
- Agentic Applications
- AI
- Azure
- Configuration Drift
- Data Governance
- DAX
- DevOps
- Environment Variables
- Fabric Data Agent
- Fabric REST APIs
- Infrastructure Automation
- KQL
- Lakehouse
- Lineage View
- Microsoft Fabric
- ML
- News
- Observability
- OneLake
- Read Only Agents
- Reference Architecture
- Scripted Deployment
- Semantic Models
- Solution Accelerator
- SQL
- Workspace Provisioning
primary_section: ai
title: 'Evolving Agentic Applications on Microsoft Fabric: From Automated Deployment to Integrating Data Agents'
external_url: https://blog.fabric.microsoft.com/en-US/blog/evolving-agentic-applications-on-microsoft-fabric-from-automated-deployment-to-integrating-data-agents/
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
---

Microsoft Fabric Blog explains two pragmatic updates to a Fabric-based agentic app reference implementation: script-driven deployment using Microsoft Fabric REST APIs and an optional read-only Fabric Data Agent for governed data Q&A, aimed at safer, more repeatable production operations.<!--excerpt_end-->

## Overview

This update to the Microsoft Fabric agentic app reference implementation focuses on operational maturity after an agentic application moves beyond proof of concept—specifically how to make agent behavior observable, governed, evaluable, and easier to run at scale.

It builds on the prior post: [Operationalizing Agentic Applications with Microsoft Fabric](https://blog.fabric.microsoft.com/blog/operationalizing-agentic-applications-with-microsoft-fabric?ft=All).

Two changes are highlighted:

- Script-driven deployment using Microsoft Fabric REST APIs
- Introducing a Fabric **Data Agent** as an optional, read-only agent focused on governed data retrieval

![Architecture diagram of the evolved agentic banking app showing a data agent enabling data access in the app](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/04/this-is-the-newest-architecture-of-the-agentic-ban.png)

## Scripted deployment with Fabric REST APIs

Microsoft Fabric provides REST APIs for item and capacity management. In the updated solution accelerator, those APIs are used for “day-zero” provisioning tasks that previously required manual setup.

Examples of what teams can automate include creating:

- Lakehouses
- SQL databases
- Other Fabric items within a workspace

The operational goal is repeatability rather than speed:

- Consistent environment setup across dev/test/prod
- Reduced configuration drift
- Clearer ownership, since provisioning choices can be encoded in scripts and version control

## Introducing a Data Agent (complement, not replacement)

The update adds a **Fabric Data Agent** as an optional agent in the agentic team.

- It can be enabled via an environment variable.
- A configuration reference is provided: [Data Agent configuration guideline](https://github.com/Azure-Samples/agentic-app-with-fabric/blob/main/workshop/Data_Agent/data_agent_configuration_reference.md)

The Data Agent is deliberately scoped:

- **Read-only**: it does not create/update/delete data.
- Focused on answering questions over **curated, structured data**.

### Why split “task agents” from “data agents”?

The original approach had task-oriented agents doing both reasoning and data retrieval. Over time, that can blur responsibilities and make safety and evaluation harder—especially when users ask exploratory/analytical questions.

With the revised split:

- **Task agents** reason, plan, and orchestrate actions.
- **Data agents** answer questions about existing data, grounded in governed models.

This is positioned as simplifying security review and improving user/operator mental models.

![Updated lineage view of all Fabric workloads in the workspace](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/04/this-figure-demonstrates-the-updated-lineage-view.png)

## Why consider Fabric’s Data Agent for agentic apps?

The post argues the main value is alignment with **existing Fabric governance**:

- Uses the same permissions and access controls already present in Fabric
- Works with OneLake-backed data foundations
- Operates over governed Fabric data sources like Lakehouses and semantic models

### Read-only by design

Data Agents can answer questions using query languages under the hood:

- SQL
- DAX
- KQL

But they cannot mutate data. The constraint is intentional to reduce the risk of unintended side effects.

### Grounded in curated models

Instead of exposing raw tables, teams configure which models/tables are queryable in natural language. This encourages reuse of semantic definitions already used for analytics/reporting.

### Consistent governance across analytics and AI

Because the Data Agent runs inside Fabric, it inherits workspace-level governance features such as:

- Access controls
- Auditing

This reduces the need to build a parallel security model just for AI interactions.

## Operational benefits without architectural overreach

The design principle presented is that operational maturity comes from narrowing responsibilities rather than expanding them.

- Scripted deployment makes infrastructure predictable and reviewable.
- A dedicated Data Agent makes data access controlled, explainable, and easier to evaluate independently from task execution.

## Closing thoughts and links

The post frames these changes as safeguards for production: automation, clear agent role separation, and integration with governed data platforms.

Explore the repo and contribute:

- Repo landing page: [aka.ms/AgenticAppFabric](https://aka.ms/AgenticAppFabric)
- Issues: [GitHub issues](https://github.com/Azure-Samples/agentic-app-with-fabric/issues)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/evolving-agentic-applications-on-microsoft-fabric-from-automated-deployment-to-integrating-data-agents/)

