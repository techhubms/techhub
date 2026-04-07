---
feed_name: Microsoft Security Blog
primary_section: ai
external_url: https://www.microsoft.com/en-us/security/blog/2026/03/30/addressing-the-owasp-top-10-risks-in-agentic-ai-with-microsoft-copilot-studio/
title: Addressing the OWASP Top 10 Risks in Agentic AI with Microsoft Copilot Studio
section_names:
- ai
- security
tags:
- Actions
- Agentic AI
- AI
- AI Red Teaming
- AI Security
- Compliance
- Connectors
- Data Loss Prevention
- Delegated Identity
- Governance
- Memory Poisoning
- Microsoft Agent 365
- Microsoft Copilot Studio
- News
- Observability
- OWASP Top 10
- OWASP Top 10 For Agentic Applications (2026)
- Policy Enforcement
- Privilege Escalation
- Prompt Injection
- Rogue Agents
- Security
- Supply Chain Security
- Threat Protection
- Tool Invocation
author: Efim Hudis
date: 2026-03-30 16:00:00 +00:00
---

Efim Hudis explains how the OWASP Top 10 Risks for Agentic Applications (2026) show up in real agent deployments, and maps those failure modes to practical mitigations and guardrails in Microsoft Copilot Studio and Microsoft Agent 365.<!--excerpt_end-->

## Overview

Agentic AI is shifting from pilots to production, and that changes the security model: agents can retrieve sensitive data, invoke tools, and take actions using real identities and permissions. Failures are often “bad outcomes” (a chain of actions across systems), not just “bad output.”

This article does two things:

- Summarizes the key failure modes described in the **OWASP Top 10 for Agentic Applications (2026)**.
- Maps those risks to example mitigations using **Microsoft Copilot Studio** and the (preview at the time of writing) **Microsoft Agent 365** governance capabilities.

Key reference:

- OWASP Top 10 for Agentic Applications (2026): https://genai.owasp.org/resource/owasp-top-10-for-agentic-applications-for-2026/

## OWASP and the agentic security problem

OWASP (Open Worldwide Application Security Project) publishes widely used application security guidance. As LLMs and agent-like systems became integrated into apps and workflows, OWASP identified gaps in traditional guidance and created the **OWASP Top 10 for Agentic Applications** to provide practical, actionable guidance.

Microsoft contributed review input via members of the **Microsoft AI Red Team**:

- Microsoft AI Red Team article: https://www.microsoft.com/en-us/security/blog/2023/08/07/microsoft-ai-red-team-building-future-of-safer-ai/

## The 10 failure modes OWASP highlights in agentic systems

The OWASP list frames common ways autonomous systems fail when they can interpret untrusted content as instruction, chain tools, act with delegated identity, and persist across sessions.

1. **Agent goal hijack (ASI01)**
   - Redirecting an agent’s goals/plans through injected instructions or poisoned content.
2. **Tool misuse and exploitation (ASI02)**
   - Misusing tools via unsafe chaining, ambiguous instructions, or manipulated tool outputs.
3. **Identity and privilege abuse (ASI03)**
   - Exploiting delegated trust, inherited credentials, or role chains to gain unauthorized access/actions.
4. **Agentic supply chain vulnerabilities (ASI04)**
   - Compromised third-party agents, tools, plugins, registries, or update channels.
5. **Unexpected code execution (ASI05)**
   - Agent-generated or agent-invoked code leading to unintended execution, compromise, or escape.
6. **Memory and context poisoning (ASI06)**
   - Corrupting stored context (memory, embeddings, RAG stores) to bias future reasoning/actions.
7. **Insecure inter-agent communication (ASI07)**
   - Spoofing/intercepting/manipulating agent-to-agent messages due to weak auth/integrity checks.
8. **Cascading failures (ASI08)**
   - A single fault propagates across agents, tools, and workflows into wider impact.
9. **Human–agent trust exploitation (ASI09)**
   - Abusing user trust/authority bias to get unsafe approvals or extract sensitive info.
10. **Rogue agents (ASI10)**
   - Agents drifting or being compromised beyond intended scope.

## Building observable, governed, and secure agents with Microsoft Copilot Studio

The article’s main idea: security needs to cover both:

- **Development time**: define intent, permissions, and constraints.
- **Operational time**: continuously monitor and control real behavior after deployment.

Copilot Studio is positioned as a platform foundation for building agents with guardrails and governance.

### Development-time guardrails in Copilot Studio

The post describes the following Copilot Studio mechanisms as mitigating multiple OWASP risks:

- **Predefined actions, connectors, and capabilities**
  - Purpose: limit exposure to arbitrary behaviors.
  - Helps mitigate:
    - Unexpected code execution (**ASI05**)
    - Unsafe tool invocation (**ASI02**)
    - Uncontrolled external dependencies / supply chain concerns (**ASI04**)
  - Admin/DLP reference: https://learn.microsoft.com/en-us/microsoft-copilot-studio/admin-data-loss-prevention?tabs=webapp

- **Isolation and change control for deployed agents**
  - Agents run in isolated environments, and can’t modify their own logic without republishing.
  - Helps mitigate:
    - Rogue agent behavior / self-modification (**ASI10**)
  - References:
    - Environments: https://learn.microsoft.com/en-us/microsoft-copilot-studio/environments-first-run-experience
    - Publication fundamentals: https://learn.microsoft.com/en-us/microsoft-copilot-studio/publication-fundamentals-publish-channels?tabs=web

- **Containment and recoverability controls**
  - Agents can be disabled or restricted.
  - Mapped in the post to limiting blast radius and helping prevent propagation.
  - Helps mitigate:
    - Insecure inter-agent communication (containment controls mentioned in context) (**ASI07**)
    - Cascading failures (**ASI08**)
  - Stop sharing / restriction reference: https://learn.microsoft.com/en-us/microsoft-copilot-studio/admin-share-bots?tabs=web#stop-sharing-an-agent

Example scenario used in the article:

- If a deployed support agent is coaxed into “adding a new action that forwards logs to an external endpoint,” the agent can’t quietly expand its toolset; changes require republishing, and teams can disable or restrict the agent quickly.

### Operational governance with Microsoft Agent 365

The post states **Microsoft Agent 365** (preview at the time of the article) will be generally available **May 1**, and describes it as a centralized control plane for observing, governing, and securing agents across their lifecycle.

- Microsoft Agent 365: https://www.microsoft.com/en-us/microsoft-agent-365

Operational capabilities described:

- **Central visibility** into agent usage, performance, risks, and connections to enterprise data/tools.
- **Policy enforcement and protection** across the environment.

Example scenario:

- If an agent accesses a sensitive document, teams can detect it in Agent 365, investigate, and restrict access or disable the agent.

The post maps capabilities to OWASP risks as follows:

- **Access and identity controls + policy enforcement**
  - Apply guardrails like access packages and usage restrictions.
  - Mitigates identity and privilege abuse (**ASI03**).

- **Data security and compliance controls**
  - Prevent leakage and detect risky or non-compliant interactions.
  - Mitigates human–agent trust exploitation / sensitive info extraction (**ASI09**).

- **Threat protection**
  - Identify vulnerabilities (**ASI04**) and detect incidents such as:
    - Prompt injection (**ASI01**)
    - Tool misuse (**ASI02**)
    - Compromised or rogue agents (**ASI10**)

## Additional resources

- OWASP Top 10 for Agentic Applications (2026): https://genai.owasp.org/resource/owasp-top-10-for-agentic-applications-for-2026/
- Microsoft AI Red Team (learn page): https://learn.microsoft.com/security/ai-red-team/
- Microsoft Security for AI: https://www.microsoft.com/security/business/solutions/security-for-ai
- Microsoft Agent 365: https://www.microsoft.com/microsoft-agent-365
- Microsoft AI Agents Hub: https://adoption.microsoft.com/ai-agents/copilot-studio/

## Source

- Microsoft Security Blog post: https://www.microsoft.com/en-us/security/blog/2026/03/30/addressing-the-owasp-top-10-risks-in-agentic-ai-with-microsoft-copilot-studio/

[Read the entire article](https://www.microsoft.com/en-us/security/blog/2026/03/30/addressing-the-owasp-top-10-risks-in-agentic-ai-with-microsoft-copilot-studio/)

