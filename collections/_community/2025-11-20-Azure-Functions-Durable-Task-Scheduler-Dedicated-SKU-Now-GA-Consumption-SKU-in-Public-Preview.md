---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-azure-functions-durable-task-scheduler-dedicated-sku/ba-p/4465328
title: Azure Functions Durable Task Scheduler Dedicated SKU Now GA, Consumption SKU in Public Preview
author: greenie-msft
feed_name: Microsoft Tech Community
date: 2025-11-20 16:00:00 +00:00
tags:
- Autoscaling
- Azure Functions
- Azure Resource Management
- Capacity Units
- Cloud Native
- Consumption SKU
- Dedicated SKU
- Durable Functions
- Durable Task Scheduler
- Fault Tolerance
- Feature Roadmap
- Orchestration
- Stateful Applications
- Workflow Automation
section_names:
- azure
- devops
---
Authored by greenie-msft, this post introduces the general availability of Azure Functions Durable Task Scheduler Dedicated SKU and the public preview of the Consumption SKU, offering advanced orchestration for scalable, resilient workloads on Azure.<!--excerpt_end-->

# Azure Functions Durable Task Scheduler Dedicated SKU Now GA, Consumption SKU in Public Preview

**Author:** greenie-msft

## Overview

Azure's Durable Task Scheduler is a managed orchestration engine built for complex, stateful workflows and intelligent agents. This scheduler tracks progress, provides automatic state persistence, and streamlines recovery from failures, significantly reducing operational overhead for developers.

## Announcements

- **Dedicated SKU (General Availability):** Predictable, high-throughput orchestration for steady workloads, supporting dedicated infrastructure, custom scaling with capacity units, high availability (multi-CU deployments), and up to 90 days of data retention.
- **Consumption SKU (Public Preview):** Flexible, pay-as-you-go pricing for variable or development/test workloads, offering up to 500 actions/sec and 30 days of data retention without upfront costs.

## Key Features

- **Durable Execution Backend:** Can be used with Azure Durable Functions, Container Apps, Kubernetes Services, and App Service.
- **Orchestration Management:** Built-in automatic monitoring, recovery, and dashboard visibility for workflows.
- **High Scalability:** Supports millions of orchestrations; Dedicated SKU handles up to 2,000 actions/sec per capacity unit and 50GB of orchestration data.

## Customer Experiences

> “The Durable Task Scheduler has been a game-changer for our projects. It keeps our workflows running reliably with minimal code, even as they grow in complexity... scales to handle millions of orchestrations...” — Pedram, VP of Engineering for Copilot

> “We are observing up to 10 times faster speed as compared to the blob storage backend. The dashboard view gives us great visibility...” — Roney Varghese, Software Engineer, Pinnacle Tech

## Dedicated SKU Highlights

- **General Availability**: Available across all Function App SKUs.
- **Custom Scaling**: Purchase additional capacity units (CUs) for higher performance.
- **High Availability**: Multi-CU deployments (at least 3 CUs recommended for mission-critical workloads).
- **Predictable Pricing**: Ideal for organizations preferring consistent costs.

[Learn more about the Dedicated SKU](https://aka.ms/dts-dedicated-sku)

## Consumption SKU Highlights

- **Public Preview**: Tailored for dynamic workloads.
- **Pay-per-Use**: Pay only for dispatched actions.
- **No Upfront Cost**: Cost-effective for development and test.

[Learn more about the Consumption SKU](https://aka.ms/dts-consumption-sku)

## Upcoming Roadmap

- **Private Endpoints** support
- **Zone Redundancy** for resilience (Dedicated SKU)
- **Export API**: Retain orchestration data beyond platform limits
- **Dynamic Scaling**: Allow auto-resizing of capacity units
- **Larger Payload Support**

## Getting Started

- [Documentation](https://aka.ms/dts-documentation)
- [Samples](https://aka.ms/dts-samples)
- [Quickstart Guide](https://aka.ms/dts-getting-started)

Durable Task Scheduler is poised to simplify and scale out orchestration for complex Azure-based workflows, reducing setup and maintenance friction and improving reliability for both mission-critical and fluctuating workloads.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-azure-functions-durable-task-scheduler-dedicated-sku/ba-p/4465328)
