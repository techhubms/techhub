---
tags:
- AI
- AI Ready Datasets
- Auditability
- Azure AI Foundry
- Azure Marketplace
- Canonical Data Model
- Capital Markets
- Data Governance
- Data Reconciliation
- Data Residency
- Entitlements
- Eventhouse
- Fabric Extensibility Toolkit
- Fabric REST APIs
- Fabric Workload Hub
- Fabric Workloads
- Financial Services
- Hedge Funds
- Lakehouse
- Microsoft Fabric
- ML
- News
- Notebooks
- OneLake
- PII
- Power BI
- Zero Trust AI
section_names:
- ai
- ml
external_url: https://blog.fabric.microsoft.com/en-US/blog/unlocking-financial-insights-with-capital-markets-datahub-workload-a-partner-led-innovation-in-microsoft-fabric-preview/
author: Microsoft Fabric Blog
primary_section: ai
feed_name: Microsoft Fabric Blog
title: 'Capital Markets DataHub workload (Preview) in Microsoft Fabric: connectors, canonical model, reconciliation, and Azure AI Foundry integration'
date: 2026-03-26 10:30:00 +00:00
---

Microsoft Fabric Blog announces the preview of the Capital Markets DataHub workload for Microsoft Fabric, a partner-built solution that delivers reconciled, governed financial datasets into Fabric (Lakehouse/Eventhouse) and integrates with Azure AI Foundry to support analytics and AI-enabled use cases in regulated capital markets environments.<!--excerpt_end-->

## Overview

Capital Markets DataHub (Preview) is a **third-party Microsoft Fabric workload** designed for **capital markets and hedge fund** scenarios where financial data is difficult to reconcile and govern. The workload is built using the **Fabric Extensibility Toolkit (FET)** and integrates with **Azure AI Foundry**, with the goal of producing **reconciled, governed, analytics- and AI-ready** financial data that can be delivered directly into a customer’s **Lakehouse and Eventhouse**.

The post positions the workload as a way to modernize analytics stacks and enable AI usage in a highly regulated domain where data consistency, identifiers, and entitlements are major blockers.

## Why capital markets data is hard to reconcile

The article calls out several drivers of complexity:

- Hundreds of internal and external sources (prime brokers, custodians, market data providers)
- Multiple identifiers per entity across systems (portfolios, instruments, entities)
- Continuous lifecycle events (corporate actions, accruals, position changes)
- Real-time expectations (intraday relevance; end-of-day can be stale)
- Strict governance needs:
  - entitlements
  - PII handling
  - cross-jurisdiction data residency
  - auditability

A key point: ingesting “raw data” isn’t sufficient without **taxonomy, semantics, reconciliation logic, and entitlement controls**.

## What the Capital Markets DataHub workload provides

The workload is described as a **financial data product** (not a generic warehouse) that embeds industry-specific intelligence into the data layer. High-level capabilities listed include:

- **200+ capital markets connectors** (banks, custodians, prime brokers, administrators, market data providers)
- A **canonical capital markets data model** to normalize data (instruments, positions, trades, and more)
- **Built-in reconciliation logic** (claimed **>99% accuracy**) to resolve breaks/inconsistencies
- **Financial services-specific entitlements** layered over Fabric governance
- **AI-ready datasets** intended to be safe for Copilot/models to consume

It’s also presented as an integrated experience with Fabric components such as **Lakehouse**, **Eventhouse**, **Power BI**, and **notebooks**, and extends to **Azure AI Foundry** and **Microsoft Excel** integration.

## How it works on Microsoft Fabric

Implementation details called out:

- Built using **Fabric Public REST APIs**, tools, and client SDKs from the **Fabric Extensibility Toolkit (FET)**
- The end-to-end flow is described as moving through **data preparation**, **data delivery**, and **data consumption** steps

![The workflow steps from Data Preparation to Data Consumption](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-workflow-steps-from-data-preparation-to-data-c.png)

The post also emphasizes **“zero trust AI”** concepts in a regulated setting via **fine-grained security** and business entitlements.

### Ask DataHub natural language interface

On top of datasets stored in **OneLake**, the workload provides a natural language interface named **Ask DataHub**, with example questions like:

- “Explain today’s exposure changes by strategy and sector.”
- “What is the latest Month- to- Date P&L by asset type?”
- “Summarize risk drivers impacting the portfolio this week.”

The article claims that because data is **reconciled, governed, and permissioned**, Copilot and “agentic workflows” can operate more safely.

## Claimed customer value

The post lists the following outcomes:

- Reduced manual wrangling and reconciliation
- Unified portfolio/risk/operations/investor reporting on one foundation
- Faster time-to-value for analytics and AI initiatives
- Improved transparency and auditability (regulators/clients)
- Enable AI adoption without compromising security/governance

## Get started and resources

- Availability: as a **Microsoft Fabric workload** in the **Fabric Workload Hub**
- Tenant setup: add **Capital Markets DataHub** to your tenant via the Workload Hub
- Demo video: https://www.youtube.com/watch?v=a_HAxV1Kt9s

![Workload Hub on Fabric with Capital Markets Datahub highlighted](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/workload-hub-on-fabric-with-capital-markets-datahu.png)

Additional links:

- Azure Marketplace listing: https://aka.ms/AA1061cv
- Product documentation: https://aka.ms/AA105f5g

## Related announcement

The post references:

- Arun Ulag’s blog: “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform”
  - https://aka.ms/FabCon-SQLCon-2026-news


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/unlocking-financial-insights-with-capital-markets-datahub-workload-a-partner-led-innovation-in-microsoft-fabric-preview/)

