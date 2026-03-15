---
external_url: https://techcommunity.microsoft.com/t5/azure-customer-innovation-blog/staying-in-the-flow-sleekflow-and-azure-turn-customer/ba-p/4467945
title: How SleekFlow Uses Azure and AI to Orchestrate Enterprise Customer Conversations
author: mtoiba
feed_name: Microsoft Tech Community
date: 2025-11-10 17:54:01 +00:00
tags:
- AgentFlow
- Azure AI Foundry
- Azure Container Apps
- Azure Cosmos DB
- Azure OpenAI
- Cloud Architecture
- Compliance
- Conversational AI
- DiskANN
- KEDA Autoscaling
- Microservices
- Microsoft Phi
- Retrieval Augmented Generation
- Semantic Kernel
- Vector Search
- AI
- Azure
- Community
section_names:
- ai
- azure
primary_section: ai
---
Leo Choi, Director of Engineering at SleekFlow, explains how their AgentFlow platform leverages Microsoft Azure AI technologies and modern cloud architecture to enable responsive, scalable, and secure enterprise customer conversations.<!--excerpt_end-->

# How SleekFlow Uses Azure and AI to Orchestrate Enterprise Customer Conversations

## Introduction

SleekFlow, founded in 2019, supports enterprises in transforming almost-lost customer moments into opportunities for connection and growth. Their AgentFlow conversational AI platform orchestrates over 600,000 daily interactions across channels like WhatsApp, Instagram, web chat, and email.

## Addressing the Challenge of Conversational AI at Scale

Enterprises demand intelligent, always-available conversations. Initial challenges included poor scalability, vector search delays, and high operating costs, especially during peak and multi-tenant workloads. These challenges hindered both development and customer onboarding, signaling the need for an enterprise-grade technical foundation.

## Azure as the Core Platform

SleekFlow chose Microsoft Azure to power AgentFlow, moving beyond basic compute to a full cloud ecosystem able to coordinate specialized AI agents and scale globally. Key Azure components include:

### Azure Cosmos DB

- Serves as memory/context backbone, handling short- and long-term interactions, chat histories, with vector embeddings.
- Enables low-latency (15–20 ms) responses essential for real-time conversations.

### Azure AI Foundry + OpenAI Models

- Drives multilingual natural language generation and understanding.
- Allows conversation agents to operate natively in languages like English, Chinese, and Portuguese.

### Semantic Kernel

- Orchestrates interactions among specialized agents pulling knowledge from context, transactional data, and vector embeddings.
- Example: Pricing agent retrieves latest data, summarizer agent prepares content, coordination agent handles human handoff.

### Azure Container Apps & KEDA Autoscaling

- Ensures elastic scaling from a few idle containers to 160+ on demand.
- Utilizes the Pay-As-You-Go model for cost efficiency.
- Supports event-driven spikes (e.g., promotional campaigns) with stable performance.

### DiskANN and Microsoft Orleans

- DiskANN powers high-speed vector search (semantic lookups in ~15-20 ms).
- Orleans manages in-memory clustering for seamless conversational flow.

## Operational Benefits Achieved

- Retrieval-augmented generation recall improved from 50% to 70%.
- End-to-end execution speed is 50% faster.
- Real-time qualification of leads, cart recovery, and accelerated support resolution.

## Security and Compliance First

- Azure Cosmos DB enforces tenant isolation, RBAC, encryption, and auditable data partitions.
- All AI models (Azure OpenAI, Microsoft Phi) are deployed in SleekFlow's private Azure environment, guaranteeing strict data privacy and compliance.
- Azure certifications (ISO 27001, SOC 2, GDPR) and features like regional residency support global enterprise trust requirements.
- Human override is always possible, with workflows orchestrated via Azure Container Apps and App Service.

## Impact on Development Velocity

- Migration from monolith to microservices on Azure Container Apps cut release cycles from 5 weeks to near-daily, with blue-green deployments enabling zero-downtime updates.
- Reduced latency allows engineers to focus on feature development instead of constant debugging.

## Conclusion & Future

SleekFlow's journey illustrates how tightly-coupled Azure AI, cloud-native, and orchestration tools can drive both business outcomes and engineering agility. Their experience provides a model for building enterprise-ready agentic AI solutions focused equally on speed, scalability, compliance, and human-like customer engagement.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-customer-innovation-blog/staying-in-the-flow-sleekflow-and-azure-turn-customer/ba-p/4467945)
