---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/building-agents-on-azure-container-apps-with-goose-ai-agent/ba-p/4460215
title: Building Agents on Azure Container Apps with Goose AI Agent, Ollama, and gpt-oss
author: simonjj
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-10-14 18:16:13 +00:00
tags:
- Agentic Frameworks
- AI Agents
- Automation
- Azure Container Apps
- Cloud Security
- Containerization
- Custom Models
- Data Governance
- Goose AI Agent
- Gpt Oss
- Microservices
- Model Inference
- Ollama
- Open Source
- RBAC
- Self Hosted AI
- Serverless GPU
section_names:
- ai
- azure
---
simonjj explores how developers can deploy and manage AI agents like Goose on Azure Container Apps, taking advantage of serverless GPU resources and secure, scalable infrastructure for rapid innovation.<!--excerpt_end-->

# Building Agents on Azure Container Apps with Goose AI Agent, Ollama, and gpt-oss

Author: simonjj

## Overview

Azure Container Apps (ACA) provides a new approach for developers to build and operate intelligent agents. Leveraging serverless scalability, GPU-on-demand, and robust enterprise isolation, ACA delivers a strong foundation for securely and cost-effectively running AI agents and inference workloads.

## Challenges in Hosting AI Agents

- **Security**: AI agents often require access to sensitive data and internal APIs, making robust security and data governance essential.
- **Flexibility**: Developers want the ability to try different agentic frameworks without increasing operational burden or compromising isolation.
- **Simplicity and Performance**: Managing scale, networking, and infrastructure should not impede iteration. Separating agent reasoning from inference backends can add complexity and latency.

## ACA and Serverless GPUs for AI Agents

Azure Container Apps streamlines the experience of deploying both agentic logic and inference models inside one environment. Key features include:

- **Security & Data Governance**: Run agents in isolated, private environments with control over identity and networking. Data stays inside your container boundary.
- **Serverless Cost Model**: Scale down to zero when idle, pay only for what you use.
- **Simple Developer Experience**: One-command deployments, integrated Azure identity, and networking. No manual setup or infrastructure management.
- **Inference with Serverless GPUs**: Easily run open-source, community, or custom models (such as gpt-oss via Ollama) alongside your agent framework, avoiding third-party API costs and improving data privacy.

## Deploying Goose AI Agent on ACA

The [Goose AI Agent](https://block.github.io/goose/) is an open-source, general-purpose agent framework from Block. It supports email integration, GitHub operations, CLI access, and system tool extensions. By deploying Goose on ACA, you get:

- **Serverless scaling and secure isolation**
- **GPU-on-demand for inferencing**
- **Rapid customization and iteration**

**Getting started:** Use the [Goose-on-ACA starter template](http://github.com/simonjj/goose-on-aca) to quickly set up your environment—includes model server, web UI, and CLI endpoints.

## Benefits of ACA for Agentic Workloads

- **Always-On Availability**: Run Goose agents continuously for asynchronous and long-lived tasks.
- **Cost Efficiency**: Pay-per-use GPU eliminates expensive, hosted inference APIs.
- **Seamless Developer Experience**: Minimal configuration; deploy full agentic systems in minutes.

## Expanding Ecosystem

ACA is becoming a hub for both open-source and commercial agent frameworks, including n8n and Goose. With features like serverless scaling, GPU-on-demand, and complete network isolation, ACA empowers developers to innovate rapidly and securely, whether prototyping single agents or building complex automated solutions.

## Key Takeaways

- ACA enables secure, flexible, and efficient deployment of AI agents, reducing operational burden.
- Goose, as an open-source agent framework, is optimized for quick deployment and extensive integration.
- Serverless GPUs and isolated container environments on Azure simplify running custom and open-source models, maximizing privacy and cost-effectiveness.

## Resources

- [Azure Container Apps Documentation](http://aka.ms/aca/docs)
- [Goose AI Agent](https://block.github.io/goose/)
- [Goose-on-ACA Starter Template](http://github.com/simonjj/goose-on-aca)
- [ACA Serverless GPUs Overview](https://learn.microsoft.com/azure/container-apps/gpu-serverless-overview)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/building-agents-on-azure-container-apps-with-goose-ai-agent/ba-p/4460215)
