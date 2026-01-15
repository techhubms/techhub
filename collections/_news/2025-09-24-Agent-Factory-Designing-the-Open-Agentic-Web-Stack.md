---
layout: post
title: 'Agent Factory: Designing the Open Agentic Web Stack'
author: Yina Arenas and Ulrich Homann
canonical_url: https://azure.microsoft.com/en-us/blog/agent-factory-designing-the-open-agentic-web-stack/
viewing_mode: external
feed_name: The Azure Blog
feed_url: https://azure.microsoft.com/en-us/blog/feed/
date: 2025-09-24 15:00:00 +00:00
permalink: /ai/news/Agent-Factory-Designing-the-Open-Agentic-Web-Stack
tags:
- A2A Protocol
- Agent Factory
- Agent Orchestration
- Agentic Web Stack
- AI
- AI + Machine Learning
- AI Agents
- Azure
- Azure AI Foundry
- Developer Tools
- Enterprise Automation
- Governance
- Identity Management
- MCP
- Microsoft Entra ID
- Multi Agent Systems
- News
- Observability
- Open Standards
- OpenTelemetry
- Security
- Zero Trust Security
section_names:
- ai
- azure
- security
---
Yina Arenas and Ulrich Homann conclude their Agent Factory series with a deep dive into the open agentic web stack, describing how Azure AI Foundry and open standards enable secure, scalable, and interoperable AI agent ecosystems for the enterprise.<!--excerpt_end-->

# Agent Factory: Designing the Open Agentic Web Stack

Unlock enterprise value with an open, secure, and interoperable AI agent ecosystem.

*This blog wraps up the Agent Factory series, sharing best practices, design patterns, and tools for building agentic AI systems.*

## The Rise of Enterprise-Grade AI Agents

AI agents—autonomous software entities acting on behalf of users and organizations—require more than just code. Architecting scalable, secure, and open agent ecosystems needs comprehensive blueprints covering use case design, developer tooling, observability, integrations, and governance.

## The Agentic Web Stack: 8 Core Components

A robust agentic web stack is a composition of services that deliver a foundation for secure, interoperable, and enterprise-grade multi-agent systems. Azure AI Foundry operationalizes these components:

1. **Communication protocol service**
    - Provides a shared language for agent communications (e.g., MCP, Agent-to-Agent/A2A protocols)
    - Enables interoperable workflows both within and across organizations
2. **Discovery registry service**
    - Catalog exposes available agents and tools
    - Registry tracks deployed, active agent instances for orchestration
3. **Identity and trust management service**
    - Enforces verifiable identity using standards (OIDC, JWT) and enterprise identity systems (Microsoft Entra ID)
    - Role-based access and zero-trust security for all agent actions
4. **Tool invocation and integration service**
    - Provides vendor-neutral tool exposition (e.g., Model Context Protocol)
    - Simplifies registering and reusing APIs across heterogeneous agents and frameworks
5. **Orchestration service**
    - Multi-agent workflows manage complex, distributed tasks and dependencies
    - Unifies frameworks (Semantic Kernel, AutoGen) inside Azure AI Foundry
6. **Telemetry and observability service**
    - Leverages OpenTelemetry for agent-aware instrumentation
    - Enables explainable, auditable, and continuously improvable agent systems
7. **Memory service**
    - Supports both short- and long-term memory integrations
    - Empowers adaptive, context-rich and human-like agent experiences
8. **Evaluation and governance service**
    - Continuous evaluation, policy enforcement, and ethical safeguards
    - Native compliance and auditability via integrated governance hooks

## Strategic Enterprise Use Cases

- **Business process automation:** Seamless, multi-agent workflows drive efficiency and reduce manual intervention
- **Cross-organization supply chain:** Secure, observable agent-to-agent interactions break down data silos
- **Knowledge worker augmentation:** Agents handle repetitive tasks; memory integration preserves continuity and compliance
- **Complex IT operations:** Distributed agentic workflows enable rapid discovery, mitigation, and policy-adherent corrective actions
- **Customer journey optimization:** Collaborative, memory-driven agents build trust and improve interactions

## Preparing for the Agentic Era

Leaders should:

- Adopt open standards (MCP, A2A) early
- Invest in foundational services: identity, observability, memory
- Operationalize governance in agent workflows
- Engage with open-source and standards communities
- Train teams in collaborating with, supervising, and improving agentic systems

## Looking Forward: Ignite 2025

The Agent Factory series provides a roadmap for organizations to build open, scalable, and secure AI agent systems. Azure AI Foundry unifies standards, frameworks, and enterprise capabilities, empowering organizations to shape the agent-driven future of business. Join Ignite 2025 to see the latest advancements in multi-agent orchestration and integrations.

### Related Posts From the Series

- [The new era of agentic AI—common use cases and design patterns](https://azure.microsoft.com/en-us/blog/agent-factory-the-new-era-of-agentic-ai-common-use-cases-and-design-patterns/)
- [Building your first AI agent with the tools to deliver real-world outcomes](https://azure.microsoft.com/en-us/blog/agent-factory-building-your-first-ai-agent-with-the-tools-to-deliver-real-world-outcomes/)
- [Top 5 agent observability best practices for reliable AI](https://azure.microsoft.com/en-us/blog/agent-factory-top-5-agent-observability-best-practices-for-reliable-ai/)
- [From prototype to production—developer tools and rapid agent development](https://azure.microsoft.com/en-us/blog/agent-factory-from-prototype-to-production-developer-tools-and-rapid-agent-development/)
- [Connecting agents, apps, and data with new open standards like MCP and A2A](https://azure.microsoft.com/en-us/blog/agent-factory-connecting-agents-apps-and-data-with-new-open-standards-like-mcp-and-a2a/)
- [Creating a blueprint for safe and secure AI agents](https://azure.microsoft.com/en-us/blog/agent-factory-creating-a-blueprint-for-safe-and-secure-ai-agents/)

This post appeared first on "The Azure Blog". [Read the entire article here](https://azure.microsoft.com/en-us/blog/agent-factory-designing-the-open-agentic-web-stack/)
