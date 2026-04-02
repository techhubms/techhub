---
date: 2026-04-02 22:23:15 +00:00
section_names:
- ai
- azure
- devops
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-sre-agent-now-supports-multiple-model-providers-including/ba-p/4508111
title: Azure SRE Agent now supports multiple model providers, including Anthropic Claude
primary_section: ai
author: dchelupati
feed_name: Microsoft Tech Community
tags:
- AI
- Anthropic
- Azure
- Azure OpenAI Service
- Azure SRE Agent
- Claude Opus 4.6
- Community
- Deployment History
- DevOps
- Incident Response
- Investigation Thread
- Log Correlation
- Metrics Anomaly Detection
- Mitigation Runbooks
- Model Provider Abstraction
- Model Provider Selection
- Observability
- Operations Workflows
- Root Cause Analysis
- Site Reliability Engineering
---

dchelupati announces that Azure SRE Agent now lets you choose a model provider (Azure OpenAI or Anthropic Claude), aimed at improving complex incident investigations that correlate logs, deployments, and metrics into a single workflow.<!--excerpt_end-->

# Azure SRE Agent now supports multiple model providers, including Anthropic Claude

Azure SRE Agent now supports **model provider selection**, letting you choose between **Azure OpenAI** and **Anthropic** to better fit different incident and reliability workflows.

The post claims SRE Agent has saved **20,000+ engineering hours** by pulling together **logs, deployments, and signals** into a single **investigation thread**, and that some customers (example: **Ecolab**) have reduced daily alerts by **up to 75%**.

## Choose your model provider: Azure OpenAI or Anthropic

- Azure SRE Agent previously used **Azure OpenAI** only.
- **Anthropic** is now available as an additional provider.
- The baseline model for Anthropic is **Claude Opus 4.6**.

The core idea is that different reliability tasks may require different reasoning capabilities:

- Quick checks (simple health verification)
- Longer investigations (multi-hour root cause analysis spanning many log streams, deployment histories, and correlated metrics)

When Anthropic is selected, Azure SRE Agent routes tasks to the model it deems appropriate, with **Claude Opus 4.6** positioned for:

- **Large context window** use cases
- **Extended reasoning** across multi-step investigations
- Retaining and connecting information across many signals before proposing next steps

## Why this matters for operations teams

The post frames provider choice as most important during complex incidents, where the agent may need to:

- Correlate logs across services
- Review deployment history
- Analyze a metrics anomaly
- Propose a mitigation runbook

It also positions “model provider abstraction” as a foundation for future expansion, so new providers could be added without requiring users to rework how the agent operates, and existing configuration should carry over.

## Get started

- Create a new agent and select Anthropic as the provider during setup: https://sre.azure.com
- Getting Started guide: https://sre.azure.com/docs/get-started/create-agent

## Additional resources

- Product documentation: https://aka.ms/sreagent/docs
- Self-paced hands-on labs: https://aka.ms/sreagent/lab
- Technical videos and demos: https://aka.ms/sreagent/youtube
- Azure SRE Agent home page: https://www.azure.com/sreagent
- Azure SRE Agent on X: https://x.com/azuresreagent

## Notes

- Updated: **Apr 02, 2026**
- Version: **1.0**


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-sre-agent-now-supports-multiple-model-providers-including/ba-p/4508111)

