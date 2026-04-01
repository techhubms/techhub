---
title: The Durable Task Scheduler Consumption SKU is now Generally Available
author: greenie-msft
section_names:
- ai
- azure
- security
feed_name: Microsoft Tech Community
date: 2026-03-31 12:00:00 +00:00
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/the-durable-task-scheduler-consumption-sku-is-now-generally/ba-p/4506682
primary_section: ai
tags:
- Actions Dispatched
- AI
- Azure
- Azure App Service
- Azure Container Apps
- Azure Functions
- Azure Kubernetes Service (aks)
- Community
- Consumption SKU
- Data Retention
- Durable Execution
- Durable Functions
- Durable Task Framework
- Durable Task Scheduler
- Event Driven Architecture
- External Event Correlation
- Fault Tolerance
- General Availability
- Idempotency
- Microsoft Entra ID
- Monitoring Dashboard
- Pay Per Use Pricing
- RBAC
- Security
- State Persistence
- Workflow Orchestration
---

greenie-msft announces the General Availability of the Durable Task Scheduler Consumption SKU, a pay-per-use orchestration backend on Azure for running durable workflows and agent sessions across Azure Functions, Azure Container Apps, and other compute environments.<!--excerpt_end-->

## Build resilient AI agents and workflows on Azure, now with pay-as-you-go pricing

The **Durable Task Scheduler Consumption SKU** is now **Generally Available (GA)**.

- Announcement: [Durable Task Scheduler Consumption SKU (billing)](https://learn.microsoft.com/en-us/azure/azure-functions/durable/durable-task-scheduler/durable-task-scheduler-billing#consumption-sku)
- Background: [What is Durable Task?](https://learn.microsoft.com/en-us/azure/azure-functions/durable/what-is-durable-task)
- Samples: [Durable Task Scheduler samples](https://github.com/Azure-Samples/Durable-Task-Scheduler/tree/main/samples)

With the Consumption SKU, you can run **durable workflows and agent sessions on Azure** with:

- **Pay-per-use pricing**
- **No storage to manage**
- **No capacity planning**
- **No idle costs**

## What is the Durable Task Scheduler?

The **Durable Task Scheduler** is a **fully managed orchestration backend** for **durable execution on Azure**. It’s designed so that workflows and agent sessions can reliably **resume and run to completion**, even across:

- Process failures
- Restarts
- Scaling events

It provides:

- Task scheduling
- State persistence
- Fault tolerance
- Built-in monitoring

This removes the operational overhead of managing your own execution engine and storage backend.

### Related background posts

- [Announcing Limited Early Access of the Durable Task Scheduler](https://aka.ms/dts-early-access)
- [Announcing Workflow in Azure Container Apps with the Durable Task Scheduler](https://aka.ms/workflow-in-aca)
- [Announcing Dedicated SKU GA & Consumption SKU Public Preview](https://aka.ms/dts-dedicated-ga)

> “The Durable Task Scheduler has become a foundational piece of what we call ‘workflows’... The combination of durable execution, external event correlation, deterministic idempotency, and the local emulator experience has made it a natural fit for our event-driven architecture.”
> — Emily Lewis, CarMax

## Where it runs

The scheduler works across Azure compute environments (see: [choose orchestration framework](https://learn.microsoft.com/en-us/azure/azure-functions/durable/choose-orchestration-framework)):

- **Azure Functions**: via the Durable Functions extension across Function App SKUs (including Flex Consumption)
- **Azure Container Apps**: using the Durable Functions or Durable Task SDKs, with built-in workflow support and autoscaling
- **Any compute**: AKS, Azure App Service, or anywhere you can run the Durable Task SDKs (\*.NET, Python, Java, JavaScript)

## Why choose the Consumption SKU?

The Consumption SKU charges based on **actions dispatched** (definition: [What is an action?](https://learn.microsoft.com/en-us/azure/azure-functions/durable/durable-task-scheduler/durable-task-scheduler-billing#what-is-an-action)):

- No minimum commitments
- No idle costs
- No capacity/throughput reservations

It’s positioned for bursty or unpredictable workloads such as:

- **AI agent orchestration**: multi-step agent workflows calling LLMs, retrieving data, and taking actions
- **Event-driven pipelines**: queues/webhooks/streams with checkpointing and reliable orchestration
- **API-triggered workflows**: signups, form submissions, payment flows
- **Distributed transactions**: retries and compensation logic (durable sagas)

## What’s included in the Consumption SKU at GA

### Performance

- **Up to 500 actions per second** (with the option to move to Dedicated SKU for higher scale)
- **Up to 30 days of data retention** for orchestration history (debugging, auditing)

### Built-in monitoring dashboard

A dashboard to:

- Filter orchestrations by status
- Drill into execution history
- View Gantt/sequence charts
- Manage orchestrations (pause, resume, terminate, raise events)

Access is secured with **Role-Based Access Control (RBAC)**.

### Identity-based security

Authentication and authorization are handled via:

- **Microsoft Entra ID** for authentication
- **RBAC** for authorization

The post explicitly calls out that this avoids managing SAS tokens or access keys.

## Get started

- [Documentation](https://aka.ms/dts-documentation)
- [Getting started](https://aka.ms/dts-getting-started)
- [Samples](https://aka.ms/dts-samples)
- [Pricing](https://learn.microsoft.com/en-us/azure/azure-functions/durable/durable-task-scheduler/durable-task-scheduler-billing#durable-task-scheduler-pricing)
- [Consumption SKU docs](https://aka.ms/dts-consumption-sku)

Feedback/bugs: file an issue on the [GitHub repository](https://github.com/Azure-Samples/Durable-Task-Scheduler/).

[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/the-durable-task-scheduler-consumption-sku-is-now-generally/ba-p/4506682)

