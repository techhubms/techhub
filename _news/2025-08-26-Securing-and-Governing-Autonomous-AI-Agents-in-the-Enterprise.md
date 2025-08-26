---
layout: "post"
title: "Securing and Governing Autonomous AI Agents in the Enterprise"
description: "This article by Igor Sakhnov, Corporate Vice President and Deputy CISO for Identity at Microsoft, explores strategies to secure and govern autonomous AI agents in enterprises as their adoption surpasses human users. The post outlines risk profiles unique to AI agents, practical measures for identity management, access control, detection of emerging threats, and how Microsoft’s security products extend zero trust and governance into the agentic era."
author: "Igor Sakhnov"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.microsoft.com/en-us/security/blog/2025/08/26/securing-and-governing-the-rise-of-autonomous-agents/"
viewing_mode: "external"
feed_name: "Microsoft Security Blog"
feed_url: "https://www.microsoft.com/en-us/security/blog/feed/"
date: 2025-08-26 16:00:00 +00:00
permalink: "/2025-08-26-Securing-and-Governing-Autonomous-AI-Agents-in-the-Enterprise.html"
categories: ["AI", "Security"]
tags: ["Access Control", "Agent Identity", "Agent Registry", "Agentic Systems", "AI", "AI Governance", "AI Security", "Autonomous Agents", "Compliance", "Data Protection", "Enterprise Security", "Entra Agent ID", "MCP", "Microsoft Defender", "Microsoft Entra", "Microsoft Purview", "News", "Role Based Access Control", "Security", "Security Posture", "Threat Detection", "XPIA", "Zero Trust Security"]
tags_normalized: ["access control", "agent identity", "agent registry", "agentic systems", "ai", "ai governance", "ai security", "autonomous agents", "compliance", "data protection", "enterprise security", "entra agent id", "mcp", "microsoft defender", "microsoft entra", "microsoft purview", "news", "role based access control", "security", "security posture", "threat detection", "xpia", "zero trust security"]
---

Igor Sakhnov, Corporate Vice President and Deputy CISO for Identity at Microsoft, shares practical advice on securing and governing autonomous AI agents, highlighting new risks and governance strategies in large-scale enterprise deployments.<!--excerpt_end-->

# Securing and Governing Autonomous AI Agents in the Enterprise

**Author:** Igor Sakhnov, Corporate Vice President and Deputy Chief Information Security Officer (CISO) for Identity, Microsoft

As enterprises increasingly deploy autonomous agents—digital actors capable of reasoning, collaborating, and taking action—the responsibility to secure and govern these entities grows. The number of AI agents in enterprise environments is projected to exceed human users by 2026, fundamentally shifting the risk landscape.

## Understanding Autonomous Agents and Their Rise

Autonomous agents originated from the convergence of generative AI and self-directed systems. Modern agents span SaaS, PaaS, and IaaS environments, including:

- Low-code business automations via platforms like Copilot Studio
- Flexible integrations with Azure AI Foundry
- Custom models/services deployed in enterprise infrastructure

Growth is driven by frameworks such as Model Context Protocol (MCP) and Agent-to-Agent (A2A) interactions. These agents now tackle complex workflows across operations and development, but they bring unique risks.

## Unique Risk Profile of Enterprise Agents

AI agents differ from conventional workloads in several ways:

- **Self-initiating:** Capable of autonomous actions
- **Persistent:** Operate continuously, risk lifecycle drift and misuse
- **Opaque:** May be difficult to audit, troubleshoot, or explain (especially those using LLMs)
- **Prolific:** Easily created, even by non-technical users, increasing risk of sprawl
- **Interconnected:** Orchestrate processes by calling other agents/services, creating complex dependencies and attack surfaces

Governance must address these risks as agents become a new primary workload.

## Common Failure Points & Threats

Agents can err during long-running tasks ("task drift") or via malicious inputs like Cross Prompt Injection Attacks (XPIA). Best practices for mitigating these risks include:

- Setting clear operational guardrails
- Monitoring behaviors
- Prompt shields for XPIA
- Robust authentication to prevent deepfakes
- Rigorous prompt engineering and employee training

## Model Context Protocol (MCP) and Agent Governance

MCP enables agents to connect securely with external data, tools, and services. Its flexibility requires robust governance:

- **Risks:** Poorly managed MCP setups can lead to data exfiltration, prompt injection, and unauthorized access
- **Role-Based Access Control (RBAC):** Essential for dynamic, context-driven agent permissions
- **Agent Registry:** Centralizes agent metadata, attributes, and relationships for better visibility and control

## Securing Agents: A Layered Approach

Building agent security centers on seven key capabilities:

1. **Identity Management:** Unique, auditable agent identities governed like user/service identities
2. **Access Control:** Least-privilege, time-bound, revocable permissions
3. **Data Security:** DLP, sensitivity-aware, adaptive controls
4. **Posture Management:** Continuous misconfiguration and vulnerability assessments
5. **Threat Protection:** Early detection of injections, misuse, and anomalies; integration with XDR
6. **Network Security:** Secure access controls and traffic inspection
7. **Compliance:** Policy-aligned logs, retention, and reporting

## Microsoft’s Agent-Governance Offering

Microsoft is extending its security portfolio—including Entra, Purview, and Defender—to cover autonomous agent governance using a zero trust approach. New offerings like Entra Agent ID provide managed, auditable agent identities with no default permissions and just-in-time access.

The agent registry concept is evolving to become an authoritative store alongside directory services, enabling observability, risk management, and scalable governance for agent-specific workloads.

## Call to Action

The "agentic era" requires visibility, layered security, and proactive governance. Organizations must treat autonomous agents as first-class workloads, embedding trust into every layer—from identity management to compliance. Building a secure, governed foundation will allow these powerful agents to deliver value without introducing unnecessary risk.

For further information, see [Build a strong security posture for AI](https://learn.microsoft.com/en-us/security/security-for-ai/) and follow the [Microsoft Security Blog](https://www.microsoft.com/en-us/security/blog/).

This post appeared first on "Microsoft Security Blog". [Read the entire article here](https://www.microsoft.com/en-us/security/blog/2025/08/26/securing-and-governing-the-rise-of-autonomous-agents/)
