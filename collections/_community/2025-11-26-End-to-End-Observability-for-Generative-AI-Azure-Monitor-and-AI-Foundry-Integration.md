---
layout: post
title: 'End-to-End Observability for Generative AI: Azure Monitor and AI Foundry Integration'
author: Hong Gao
canonical_url: https://techcommunity.microsoft.com/t5/azure-observability-blog/observability-for-the-age-of-generative-ai/ba-p/4473307
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-26 18:35:59 +00:00
permalink: /ai/community/End-to-End-Observability-for-Generative-AI-Azure-Monitor-and-AI-Foundry-Integration
tags:
- Agent Monitoring
- AI
- Application Insights
- Azure
- Azure AI Foundry
- Azure Monitor
- Azure Monitor Workbooks
- Cloud Monitoring
- Community
- Compliance
- Cost Governance
- DevOps
- Generative AI
- Grafana
- LLM
- Multi Agent Orchestration
- Observability
- OpenTelemetry
- Telemetry
section_names:
- ai
- azure
- devops
---
Hong Gao introduces new Azure Monitor and AI Foundry integration, providing enterprise-grade observability for generative AI systems. The post highlights unified dashboards, AI telemetry, and OpenTelemetry-powered insights.<!--excerpt_end-->

# End-to-End Observability for Generative AI: Azure Monitor and AI Foundry Integration

**Author:** Hong Gao

## Introduction

Monitoring and trusting your systems requires new strategies in the age of Generative AI. Unlike traditional applications, GenAI apps are dynamic, making choices, planning, and integrating tool invocations. Conventional observability tools focused on servers and microservices fall short for these new workloads.

## What’s New: Azure Monitor and AI Foundry Integration

At Microsoft Ignite, the next phase of integrating Azure Monitor with AI Foundry was announced—features designed specifically to tackle observability challenges in GenAI and LLM-based applications and agents.

### Key Capabilities

- **Agent Overview Dashboard:** Unified dashboards in Grafana and Azure show multiple GenAI agents’ metrics, including:
  - Success rate
  - Grounding quality
  - Safety violations
  - Latency
  - Cost per outcome
  - Ability to track regressions after model/prompt changes

- **AI-Tailored Trace View:** Every AI agent decision is readable as a "story" (plan → reasoning → tool calls → guardrail checks). This makes it possible to identify performance or safety issues rapidly.

- **AI-Aware Trace Search:** Search, filter, and sort millions of runs using GenAI-specific attributes such as model IDs, grounding scores, or cost—pinpoint critical events efficiently.

- **Low-Code Agent Monitoring:** Foundry’s visual interface enables automatic observability of agents. No coding needed—track reliability, safety, and cost from day one.

- **Full-Stack Visibility:** All evaluations, traces, and red-teaming results are visible in Azure Monitor, allowing agent signals to be correlated with infrastructure KPIs and telemetry from other services.

## Video Demonstration

A demonstration video showing these features in action is available: [2025_IgniteAct3Video.mp4](https://microsoft.sharepoint-df.com/teams/AIObservability/_layouts/15/stream.aspx?id=%2Fteams%2FAIObservability%2FShared%20Documents%2FEvents%2F2025%5FIgniteAct3Video%2Emp4&referrer=StreamWebApp%2EWeb&referrerScenario=AddressBarCopied%2Eview%2E04c6b2e7%2Dc876%2D461d%2D9900%2D43434361afe9)

## Learn More

- [Get Started Documentation](https://learn.microsoft.com/en-us/azure/azure-monitor/app/agents-view)

## OpenTelemetry Innovation

These new features leverage the latest OpenTelemetry extensions, described [in this Azure AI Foundry blog post](https://techcommunity.microsoft.com/blog/azure-ai-foundry-blog/azure-ai-foundry-advancing-opentelemetry-and-delivering-unified-multi-agent-obse/4456039). Microsoft is actively contributing to OpenTelemetry agent standards, making it possible to capture multi-agent orchestration traces, LLM reasoning context, and custom evaluation signals. This enables interoperability across Azure Monitor, Foundry, and tools like Datadog, Arize, and Weights & Biases, providing customers with consistent monitoring across cloud and hybrid AI scenarios.

## Built for Enterprise Scale

By building on open standards and deep Azure integrations, organizations can apply robust governance, compliance, and quality assurance disciplines to AI-powered workloads—just as they do for traditional applications.

## Conclusion

Generative AI transforms what it means to operate and monitor software. With these innovations, Microsoft aims to give customers reliable, transparent, and compliant ways to run AI solutions at enterprise scale.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-observability-blog/observability-for-the-age-of-generative-ai/ba-p/4473307)
