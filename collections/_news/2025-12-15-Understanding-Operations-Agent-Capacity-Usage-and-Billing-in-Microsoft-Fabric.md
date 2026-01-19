---
layout: post
title: Understanding Operations Agent Capacity Usage and Billing in Microsoft Fabric
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/understanding-operations-agent-capacity-consumption-usage-reporting-and-billing/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-12-15 11:59:00 +00:00
permalink: /ai/news/Understanding-Operations-Agent-Capacity-Usage-and-Billing-in-Microsoft-Fabric
tags:
- Autonomous Agents
- Autonomous Reasoning
- Azure Metrics
- Background Usage
- Billing
- Capacity Metrics App
- Capacity Units
- Compute Meter
- Consumption Rates
- Copilot in Fabric
- Data Monitoring
- Eventhouse
- Fabric Preview
- Microsoft Fabric
- OneLake Storage
- Operations Agent
- Power Automate
- Real Time Intelligence
- Usage Reporting
section_names:
- ai
- azure
- ml
---
Microsoft Fabric Blog explains how operations agents leverage Copilot and autonomous AI reasoning to monitor and act on data, with guidance on usage metering, billing, and technical metrics for architects, engineers, and admins.<!--excerpt_end-->

# Understanding Operations Agent Capacity Usage and Billing in Microsoft Fabric

Microsoft Fabric's operations agents, announced at Ignite, introduce autonomous capabilities for data monitoring, goal inference, and actionable recommendations. As Preview continues, billing for these agents will be enabled, aligned with Fabric Capacity Unit (CU) consumption, and reported in the Capacity Metrics App.

## Key Operations Agent Activities

- **Copilot in Fabric Usage:** Charges accrue when Large Language Model (LLM) features are used for agent configuration or direct interaction.
- **Compute:** Covers background querying and monitoring, with the agent evaluating rules and conditions. This compute is tracked in the metrics app, with potential additional costs from data sources.
- **Autonomous Reasoning:** When monitored criteria are met, the agent leverages LLMs to summarize data, recommend actions, and communicate with users.
- **Storage:** Fabric retains monitored data and agent configurations for 30 days, incurring storage costs based on OneLake rates.

## Capacity Metrics Tracking

Configuration, query, rule tracking, and data retention all generate usage, tracked in the [Capacity Metrics App](https://learn.microsoft.com/fabric/enterprise/metrics-app-compute-page):

- Long-lived queries against Eventhouse result in both compute and storage costs.
- Autonomous reasoning events charge by token usage, reflecting deep AI processing for recommendations.

### Flow Diagram (Overview)

1. **Configure agent:** Copilot creates playbooks, Eventhouse queries identify data fields; storage tracks configuration.
2. **Activate agent:** Agents run background queries, apply rules; compute and storage meters track usage.
3. **Trigger recommendations:** LLM summarizes matching data; autonomous reasoning CUs calculated; user can approve Power Automate flows (consult latest [Power Automate pricing](https://www.microsoft.com/power-platform/products/power-automate/pricing)).

## Usage Categories and Rates

All operations agent activity in Fabric is considered *Background usage* (including direct interaction). Copilot and AI operations are billed under background, detailed in the [Copilot Fabric Consumption docs](https://learn.microsoft.com/fabric/fundamentals/copilot-fabric-consumption#capacity-utilization-type).

| Azure Metric Name                                      | Fabric Operation Name             | CU Rate                                        |
|-------------------------------------------------------|-----------------------------------|------------------------------------------------|
| Operations Agents Compute Capacity Usage CU            | Operations agent compute          | 0.46 CUs per vCore hour                        |
| Copilot and AI Capacity Usage CU                      | Copilot in Fabric                 | 100 CUs/1000 input tokens; 400 CUs/1000 output |
| Operations Agents Autonomous Reasoning Capacity Usage  | Autonomous reasoning              | 400 CUs/1000 input; 1600 CUs/1000 output       |
| n/a                                                   | OneLake Storage                   | per GB per hour ([link](https://learn.microsoft.com/fabric/onelake/onelake-consumption)) |

## Timeline

- Usage reporting visible in Capacity Metrics App from December.
- Billing for Copilot in Fabric starts December; other features billed after January 8, 2026.

## Further Reading

- [Operations agent billing documentation](https://learn.microsoft.com/fabric/real-time-intelligence/operations-agent-billing)
- [Capacity Metrics App documentation](https://learn.microsoft.com/fabric/enterprise/metrics-app-compute-page)
- [Power Automate pricing](https://www.microsoft.com/power-platform/products/power-automate/pricing)

---

Microsoft Fabric Blog provides robust background on operational agent monitoring and billing, with technical details for capacity planning, reporting, and AI-powered automation.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/understanding-operations-agent-capacity-consumption-usage-reporting-and-billing/)
