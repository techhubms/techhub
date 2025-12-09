---
layout: "post"
title: "Nasdaq Boardvantage: AI-Driven Governance Architecture with Azure PostgreSQL and Microsoft Foundry"
description: "This session from Microsoft Ignite 2025 delivers an in-depth look at how Nasdaq Boardvantage leverages Azure Database for PostgreSQL and MySQL, Microsoft Foundry, Azure Kubernetes Service (AKS), and Azure API Management to build a secure, agentic AI-driven governance solution. It covers architectural patterns for data security, smart document summarization, AI Meeting Minutes, hybrid search using PG Vector, and advanced microservices orchestration with Azure Functions and Prompt Flow."
author: "Microsoft Events"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.youtube.com/watch?v=BkOcPQntsk4"
viewing_mode: "internal"
feed_name: "Microsoft Events YouTube"
feed_url: "https://www.youtube.com/feeds/videos.xml?channel_id=UCrhJmfAGQ5K81XQ8_od1iTg"
date: 2025-12-09 18:08:55 +00:00
permalink: "/2025-12-09-Nasdaq-Boardvantage-AI-Driven-Governance-Architecture-with-Azure-PostgreSQL-and-Microsoft-Foundry.html"
categories: ["AI", "Azure", "ML", "Security"]
tags: ["AI", "AI Governance", "AI Meeting Minutes", "AKS", "API Management", "Azure", "Azure Database For MySQL", "Azure Database For PostgreSQL", "Azure Functions", "BRK137", "Cloud Architecture", "Data Security", "F5n1", "Hybrid Search", "Ignite", "Innovate With Azure AI Apps And Agents:azure PostgreSQL", "Innovate With Azure AI Apps And Agents:microsoft Foundry", "Microservices Orchestration", "Microsoft", "Microsoft Foundry", "Microsoft Ignite", "ML", "Ms Ignite", "Msft Ignite", "Nasdaq Boardvantage", "Nasdaq Boardvantage: AI Driven Governance On PostgreSQL And Microsoft Foundry | BRK137", "PG Vector", "Prompt Flow", "Regulatory Compliance", "Security", "Smart Document Summarization", "Unify Your Data Platform", "Videos"]
tags_normalized: ["ai", "ai governance", "ai meeting minutes", "aks", "api management", "azure", "azure database for mysql", "azure database for postgresql", "azure functions", "brk137", "cloud architecture", "data security", "f5n1", "hybrid search", "ignite", "innovate with azure ai apps and agentsazure postgresql", "innovate with azure ai apps and agentsmicrosoft foundry", "microservices orchestration", "microsoft", "microsoft foundry", "microsoft ignite", "ml", "ms ignite", "msft ignite", "nasdaq boardvantage", "nasdaq boardvantage ai driven governance on postgresql and microsoft foundry pipe brk137", "pg vector", "prompt flow", "regulatory compliance", "security", "smart document summarization", "unify your data platform", "videos"]
---

Microsoft Events presents an advanced session with Charles Feddersen and Mohsin Shafqat, exploring how Nasdaq Boardvantage utilizes Azure open-source databases and Microsoft Foundry to achieve secure, AI-powered board governance.<!--excerpt_end-->

{% youtube BkOcPQntsk4 %}

# Nasdaq Boardvantage: AI-Driven Governance on PostgreSQL and Microsoft Foundry

*Session: BRK137 | Microsoft Ignite 2025*

### Speakers

- Charles Feddersen
- Mohsin Shafqat

## Overview

Nasdaq Boardvantage is trusted by nearly half of Fortune 100 companies for secure and intelligent board operations. In this advanced session, the presenters detail the cloud architecture that combines Azure Database for PostgreSQL and MySQL, Microsoft Foundry, Azure Kubernetes Service (AKS), and Azure API Management to create resilient, observation-driven governance solutions for highly confidential financial data.

## Key Topics

### 1. Secure Cloud Architecture

- Adoption of Azure open-source databases (PostgreSQL, MySQL) at the core of the solution.
- Use of Azure Kubernetes Service (AKS) for scalable microservices and orchestration.
- Azure API Management secures entry points and manages traffic.
- Security emphasis: Regulatory compliance and protection of sensitive board data.

### 2. Intelligent Governance and Agentic AI

- Integration with Microsoft Foundry and Azure AI Foundry for AI workloads.
- Implementation of agentic AI features for automating board operations:
  - Smart document summarization.
  - AI-generated meeting minutes based on agenda-contextualized data extraction.
- Prompt Flow in Azure AI Foundry tackles challenges like prompt determinism and hallucination.

### 3. Advanced Data Management

- PostgreSQL with PG Vector supports efficient hybrid search for AI features.
- Storage design accommodates structured and unstructured board documents.
- AI extracts and matches relevant information for meeting documentation through advanced querying and summarization techniques.

### 4. Microservices and Orchestration

- Existing microservices refactored using a fan-in, fan-out orchestration pattern.
- Azure Functions facilitate the architecture for parallelized processing and AI assistant interactions.

### 5. Benefits of Azure Adoption

- Improved scalability, reduced infrastructure management burden.
- Native integration of security, regulatory compliance, and advanced AI capabilities.
- Accelerated innovation for board functionalities and agentic automation.

## Solution Architecture Diagram (Session Reference)

*(Refer to the official session video for visual diagrams and real-world deployment examples.)*

- **AKS** runs microservices, provides scale and resilience.
- **Azure PostgreSQL (with PG Vector)** enables hybrid AI search and stores extracted data for efficient retrieval.
- **Microsoft Foundry / Azure AI Foundry** powers AI feature development including document summarization and chat assistants.
- **Azure API Management** secures and standardizes API gateway for all external and internal API consumption.

## AI-Driven Meeting Minutes and Document Intelligence Workflow

1. Board meeting agendas and documents are ingested.
2. AI models summarize documents and extract critical information.
3. Meeting minutes are generated by matching agenda sections with summarized data.
4. All data stored securely in Azure PostgreSQL and accessible via advanced hybrid search.

## Overcoming Challenges

- Addressed issues like prompt nondeterminism and model hallucination with Prompt Flow refinement.
- Migrated to a fan-in, fan-out pattern for complex AI orchestration and parallel processing.
- Ensured compliance and security remain uncompromised at every architectural layer.

## References and Next Steps

- [Microsoft Ignite 2025](https://ignite.microsoft.com) for on-demand sessions and documentation.
- [Microsoft Fabric AI Data Solutions Plan](https://aka.ms/ignite25-plans-MicrosoftFabricAIDataSolutionsPlan)

---
*This session is part of Microsoft Ignite 2025’s “Innovate with Azure AI apps and agents” track. For more details, search for session code BRK137.*
