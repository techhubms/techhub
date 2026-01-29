---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/compose-for-agents-on-azure-container-apps-and-serverless-gpu/ba-p/4471061
title: 'Compose for Agents on Azure Container Apps: Serverless GPU and Agentic AI Workloads'
author: simonjj
feed_name: Microsoft Tech Community
date: 2025-11-18 17:27:09 +00:00
tags:
- Agentic Applications
- AI Frameworks
- Azure Container Apps
- Cloud Deployment
- Containerization
- CrewAI
- DevOps Automation
- Docker Compose
- Dynamic Scaling
- LangGraph
- LLM Orchestration
- MCP
- MCP Gateway
- Microservices
- Model Runner
- Retrieval Augmented Generation
- Serverless GPU
- Spring AI
- Vector Database
- AI
- Azure
- DevOps
- Community
section_names:
- ai
- azure
- devops
primary_section: ai
---
Simonjj explores the public preview of Compose for Agents on Azure Container Apps, showing how developers can leverage serverless GPUs, Docker Compose, and a wide selection of AI frameworks to build and scale agentic applications on Azure.<!--excerpt_end-->

# Compose for Agents on Azure Container Apps and Serverless GPU (Public Preview)

*Author: simonjj*

## Overview

Azure Container Apps (ACA) now offers first-class support for agentic AI applications via Docker’s Compose for Agents, currently in public preview. This empowers developers to easily define, deploy, and operate complex agent-based workloads—such as virtual assistants and workflow agents—using familiar Docker Compose tooling and Azure’s fully managed, serverless infrastructure.

## Key Challenges in Agentic Application Development

Modern intelligent applications require weaving together LLMs, vector databases, MCP (Model Context Protocol) tools, and orchestration logic. Traditionally, this brings several challenges:

- **Tooling sprawl**: Multiple SDKs and frameworks, tedious dependency management
- **Specialized hardware**: GPUs are required for performant model inference and orchestration, which are costly to manage
- **Operational complexity**: Scaling and securing multi-service applications is traditionally a full-time effort

## How Azure Container Apps Simplifies Agentic AI Solutions

Azure Container Apps addresses these pain points with:

- **Serverless GPUs with per-second billing**: Run workloads on demand, paying only for actual GPU usage—ideal for sporadic or highly variable compute needs
- **Sandboxed dynamic sessions**: Secure, short-lived environments for running user-provided or third-party code, separating evaluation scripts or plugins from critical services
- **Automatic scaling and managed operations**: Container Apps handle service discovery, ingress, scaling, rolling updates, and revision management—no need to maintain your own orchestrator
- **First-class Docker Compose integration**: Define your stack in a standard `compose.yaml`, inclusive of models, agents, open tools, and microservices, for seamless deployment to both local and cloud environments
- **Integrated Model Runner & MCP Gateway**: Model Runner lets users pull and expose open-weight language models via OpenAI-compatible endpoints. MCP Gateway connects agents to a curated suite of tools, enabling retrieval-augmented generation (RAG), vector search, and specialized tool invocation directly within the Compose stack

## Developer Workflow and Supported Frameworks

With Compose for Agents in ACA, developers can:

- **Describe agent stacks declaratively**: Use a single YAML file to orchestrate models (e.g., via Model Runner and MCP Gateway), agents, and services using frameworks like LangGraph, Embabel, Vercel AI SDK, Spring AI, CrewAI, and Google ADK
- **Maintain consistency across environments**: The same compose file can launch locally or in production, enabling fast iteration and cloud readiness
- **Scale seamlessly**: Virtually unlimited compute elasticity with serverless GPU integration and scaling

### Supported Architectures and Tools

- **Framework agnosticism**: Choose from LangGraph, CrewAI, Spring AI, or other preferred stacks
- **Tool integration**: Leverage over a hundred ready-made tools and services from Docker's MCP catalog for retrieval, summarization, data access, and more

## Getting Started

To try Compose for Agents preview on ACA:

1. Install the latest Azure Container Apps Extension
2. Define your agentic application and dependencies in `compose.yaml`
3. Deploy with `az containerapp compose up`—GPU allocation, dynamic sessions, and scaling are handled automatically
4. Iterate locally with `docker compose up`, then use the same configuration file to run in Azure

For detailed instructions, visit [https://aka.ms/aca/compose-for-agents](https://aka.ms/aca/compose-for-agents)

## Collaboration with Docker

This innovation is the result of joint work between Microsoft and Docker, bringing together Docker Compose's developer-favorite workflow with Azure's robust, scalable serverless infrastructure for modern AI workloads.

---

*Published by simonjj, November 18, 2025. For more content like this, follow the Apps on Azure Blog.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/compose-for-agents-on-azure-container-apps-and-serverless-gpu/ba-p/4471061)
