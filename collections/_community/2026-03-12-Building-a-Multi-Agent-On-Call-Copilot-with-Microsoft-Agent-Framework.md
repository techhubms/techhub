---
layout: "post"
title: "Building a Multi-Agent On-Call Copilot with Microsoft Agent Framework"
description: "This article demonstrates the creation of On-Call Copilot, an AI-powered incident response system built using Microsoft Agent Framework and deployed as a Foundry Hosted Agent. It covers the design and architecture of multi-agent orchestration for automating incident triage, communication, and reporting, highlighting key engineering decisions and architectural patterns that enable scalable, low-latency, production-grade solutions for SRE teams on Azure infrastructure."
author: "Lee_Stott"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-a-multi-agent-on-call-copilot-with-microsoft-agent/ba-p/4499962"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-03-12 07:00:00 +00:00
permalink: "/2026-03-12-Building-a-Multi-Agent-On-Call-Copilot-with-Microsoft-Agent-Framework.html"
categories: ["AI", "Azure", "Coding", "DevOps"]
tags: ["AI", "Asyncio", "Azure", "Azure Container Registry", "Azure OpenAI", "Cloud Infrastructure", "Coding", "Community", "ConcurrentBuilder", "DefaultAzureCredential", "DevOps", "DevOps Automation", "Foundry Hosted Agents", "Incident Response", "JSON Automation", "Microsoft Agent Framework", "Model Router", "Multi Agent Systems", "Post Incident Reporting", "Prompt Engineering", "Python", "Site Reliability Engineering", "Zero Dependency UI"]
tags_normalized: ["ai", "asyncio", "azure", "azure container registry", "azure openai", "cloud infrastructure", "coding", "community", "concurrentbuilder", "defaultazurecredential", "devops", "devops automation", "foundry hosted agents", "incident response", "json automation", "microsoft agent framework", "model router", "multi agent systems", "post incident reporting", "prompt engineering", "python", "site reliability engineering", "zero dependency ui"]
---

Lee_Stott explores how to build On-Call Copilot, an AI-driven, multi-agent incident triage and reporting solution using Microsoft Agent Framework, Foundry Hosted Agents, Model Router, and Azure OpenAI—detailing the technical patterns, code, and deployment steps for practical DevOps engineers.<!--excerpt_end-->

# Building a Multi-Agent On-Call Copilot with Microsoft Agent Framework

When every second counts during incidents, automating root cause analysis, triage, communication, and reporting can dramatically improve resolution speed. This guide explains how On-Call Copilot was built using the Microsoft Agent Framework and deployed as a Foundry Hosted Agent to provide instant, scalable, and structured incident triaging powered by Azure OpenAI.

## Overview

On-Call Copilot leverages the Microsoft Agent Framework SDK to orchestrate four specialized agents (Triage, Summary, Comms, PIR) in parallel, using containerized Python services hosted on Microsoft Foundry. Each agent processes the full incident payload (alerts, logs, metrics), returning focused output—structured as JSON—for easy downstream consumption. The overall system reduces incident response bottlenecks and provides actionable reports for engineering teams and stakeholders.

## Why Multi-Agent? The Problem with Single-Prompt Triage

Early AI incident assistants used a single, massive prompt to handle triage, communications, and reporting. This resulted in poor output quality due to context overload and conflicting task requirements. By decomposing the workflow into specialized agents (each with its own system prompt), On-Call Copilot avoids token limits and task interference, yielding higher-quality, reliable results for each sub-task.

## Architecture

- **Foundry Hosted Agent:** Containerized Python orchestrator runs on Microsoft Foundry's managed cloud infrastructure.
- **Agent Orchestration:** Uses `ConcurrentBuilder` from Microsoft Agent Framework, leveraging `asyncio.gather()` to run all four agents in parallel for minimal latency.
- **Model Router:** A single Azure OpenAI deployment (`model-router`) automatically selects the best model (e.g., `gpt-4o`, `gpt-4o-mini`) for each agent's request based on complexity—no hardcoded model switching needed.
- **Secure Auth:** `DefaultAzureCredential` ensures no API keys are ever exposed, using managed identity in production and `az login` for local development.

## The Four Agents

- **Triage Agent:** Performs root cause analysis, identifies immediate remediation steps, surfaces missing information, and aligns suggestions with existing runbooks.
- **Summary Agent:** Crafts a concise, situational narrative (status, what happened).
- **Comms Agent:** Provides audience-specific Slack updates (with emoji severity) and executive summaries for stakeholders.
- **PIR Agent:** Produces chronological post-incident reports, quantifies customer impact, and suggests preventative actions.

## Code Example: Orchestrator

```python
from agent_framework import ConcurrentBuilder
from agent_framework.azure import AzureOpenAIChatClient
from azure.identity import DefaultAzureCredential, get_bearer_token_provider
from app.agents.triage import TRIAGE_INSTRUCTIONS
from app.agents.summary import SUMMARY_INSTRUCTIONS
from app.agents.comms import COMMS_INSTRUCTIONS
from app.agents.pir import PIR_INSTRUCTIONS

_credential = DefaultAzureCredential()
_token_provider = get_bearer_token_provider(_credential, "https://cognitiveservices.azure.com/.default")

def create_workflow_builder():
    triage = AzureOpenAIChatClient(ad_token_provider=_token_provider).create_agent(
        instructions=TRIAGE_INSTRUCTIONS, name="triage-agent")
    summary = AzureOpenAIChatClient(ad_token_provider=_token_provider).create_agent(
        instructions=SUMMARY_INSTRUCTIONS, name="summary-agent")
    comms = AzureOpenAIChatClient(ad_token_provider=_token_provider).create_agent(
        instructions=COMMS_INSTRUCTIONS, name="comms-agent")
    pir = AzureOpenAIChatClient(ad_token_provider=_token_provider).create_agent(
        instructions=PIR_INSTRUCTIONS, name="pir-agent")
    return ConcurrentBuilder().participants([triage, summary, comms, pir])

if __name__ == "__main__":
    builder = create_workflow_builder()
    # Start agent
    from_agent_framework(builder.build).run()
```

## Agent Instructions: Prompt-as-Config

Each agent has a tightly-scoped, plain-text configuration string defining its schema, output guardrails, and expected fields. This enables rapid iteration (even by non-developers) and minimizes strong-coupling between workflow steps.

## Example Incident Envelope

Agents accept a single JSON payload, compatible with inputs from webhooks like PagerDuty, DataDog, or monitoring scripts:

```json
{
    "incident_id": "INC-20260217-002",
    "title": "DB connection pool exhausted — checkout-api degraded",
    "severity": "SEV1",
    "alerts": [{ "name": "DatabaseConnectionPoolNearLimit", "description": "Connection pool at 99.7% on orders-db-primary" }],
    "logs": [{ "source": "order-worker", "lines": ["ERROR: connection timeout after 30s", "WARN: pool exhausted, queueing request"] }],
    "metrics": [{ "name": "db_connection_pool_utilization_pct", "window": "5m", "values_summary": "Jumped from 22% to 99.7%" }],
    "runbook_excerpt": "Step 1: Check DB connection dashboard..."
}
```

## Deployment and Invocation

- **Agent Registration:** The agent is described declaratively with `agent.yaml` for automatic service discovery and request routing.
- **Deployment:** One-command setup via `azd up` (provisions infra, builds Docker image, deploys container, and Model Router).
- **Zero-Dependency UI:** Includes a browser-based live demo (HTML/CSS/JS with Python backend) for hands-on review of agent results.
- **API Invocation:** Use Python scripts, curl, or local validation scripts for automated or interactive testing.

## Performance & Engineering Considerations

- **Parallelism:** Running four agents concurrently reduces incident response latency by up to 4x compared to sequential execution.
- **Security:** Managed identity and default credentials eliminate manual API key rotation.
- **Strict JSON Output:** Reliable, post-processable, and safe for downstream automation.
- **Extensibility:** New agents (e.g., for ticket auto-creation, payload normalization) can be wired in with minimal changes.

## Key Takeaways

- The multi-agent pattern, enabled by Microsoft Agent Framework, is powerful for all decomposable automation workflows—not just chatbots.
- Prompt-as-config accelerates iteration and involves non-developers in workflow evolution.
- Azure Foundry Hosted Agents provide production-grade infra (scaling, monitoring, telemetry) without DevOps burden.
- Model Router ensures models are always cost-efficient and up-to-date with no changes to user code.

## Resources

- [Microsoft Agent Framework SDK](https://learn.microsoft.com/agent-framework/)
- [Model Router Docs](https://learn.microsoft.com/azure/ai-foundry/openai/how-to/model-router)
- [Foundry Hosted Agents Guide](https://learn.microsoft.com/azure/ai-foundry/agents/how-to/deploy-hosted-agent)
- [ConcurrentBuilder Samples](https://github.com/microsoft-foundry/foundry-samples)
- [DefaultAzureCredential Reference](https://learn.microsoft.com/python/api/azure-identity/azure.identity.defaultazurecredential)

---

This solution brings real-world SRE best practices, AI orchestration, and Azure-first deployment patterns together for actionable, scalable incident response in modern cloud environments.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-a-multi-agent-on-call-copilot-with-microsoft-agent/ba-p/4499962)
