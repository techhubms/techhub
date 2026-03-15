---
external_url: https://www.microsoft.com/en-us/security/blog/2026/01/23/runtime-risk-realtime-defense-securing-ai-agents/
title: Securing Microsoft Copilot Studio AI Agents with Defender Runtime Protection
author: Microsoft Defender Security Research Team
feed_name: Microsoft Security Blog
date: 2026-01-23 20:57:14 +00:00
tags:
- Agent Builder
- Agent Security
- AI Agents
- Cloud Apps Security
- Data Exfiltration
- Generative Orchestration
- Malicious Attack Prevention
- Microsoft Copilot Studio
- Microsoft Defender
- Microsoft Security
- Prompt Injection
- Real Time Protection
- Runtime Security
- Threat Detection
- Webhook Integration
- AI
- Security
- News
section_names:
- ai
- security
primary_section: ai
---
The Microsoft Defender Security Research Team shares in-depth guidance on securing Microsoft Copilot Studio AI agents at runtime, demonstrating how Defender’s real-time protection thwarts malicious prompt injections and data exfiltration attempts.<!--excerpt_end-->

# Securing Microsoft Copilot Studio AI Agents with Defender Runtime Protection

*By the Microsoft Defender Security Research Team*

AI agents built with Microsoft Copilot Studio—or similar platforms—are transforming organizational productivity by integrating internal data and systems via generative orchestration. However, as capabilities grow, so do the risks: once deployed, AI agents may be vulnerable to adversarial input that can manipulate their decision-making and execution.

## Why Runtime Security for AI Agents is Essential

Traditional controls often miss subtle manipulations. Threat actors can, through crafted input, push agents to operate within their permissions but in unexpected, potentially unsafe ways. Runtime verification and control—like that offered by Microsoft Defender—become critical to:

- Inspect agent behavior as it happens
- Evaluate planned actions against policies
- Block unauthorized operations in real time

Copilot Studio agents benefit from Defender’s integration, where tooling such as real-time protection webhooks allow for per-operation security checks before any action executes. This setup ensures oversight without restricting legitimate flexibility.

## Key Elements of Copilot Studio Agents and Attack Surfaces

Copilot Studio agents are built on:

- **Topics:** Conversation flows triggered by user input
- **Tools:** Capabilities the agent invokes (connector actions, model calls)
- **Knowledge Sources:** Enterprise data integrations (Power Platform, Dynamics 365, websites)

The interaction of these elements in generative orchestration multiplies the potential for novel attack vectors—in particular, via crafted prompts or documents aiming to chain together unintended behaviors.

## How Attackers Exploit Generative Orchestration

### Scenario 1: Malicious Instruction Injection in Event-Triggered Workflows

A financial agent processes emails via event trigger. If a malicious sender injects hidden prompts, the agent could—absent protection—combine reading sensitive knowledge and emailing it externally. Defender’s runtime checks intercept webhook calls including context, intent, and action, and can block such knowledge searches, logging the event and alerting security teams.

### Scenario 2: Prompt Injection in Shared Documents

Agents connected to SharePoint can be manipulated if an insider embeds crafted instructions into a file. When processed, the agent may be tricked into exfiltrating sensitive content via email. Defender’s integration blocks the sending operation based on detected context and policy violations, stopping the attack at execution time.

### Scenario 3: Capability Reconnaissance via Public Chatbot

A public-facing chatbot may be probed for functionality via advanced prompt sequences. Once attackers identify accessible data or tools, they may try to trigger data exfiltration. Defender detects the enumeration attempt and preemptively blocks subsequent actions tied to the reconnaissance.

## Defender’s Runtime Protection Model

Microsoft Defender leverages webhook-based runtime inspection to:

- Analyze each planned action in real time
- Evaluate both intent and context
- Allow or block operations before execution
- Log and alert on suspicious attempts

This strategy applies the same rigor used for code execution security to the dynamic world of agent tool invocation, allowing safe scaling of Copilot Studio agent deployments.

## Best Practices and Next Steps

- *Incorporate runtime webhooks* in all Copilot Studio agent deployments.
- Leverage Defender integration to monitor and respond to suspicious activity.
- Review [Microsoft documentation](https://learn.microsoft.com/en-us/defender-cloud-apps/ai-agent-protection) for enabling these protections.
- Stay prepared for evolving attack strategies as AI orchestration platforms mature.

By adopting proactive real-time defense, organizations can confidently build and operate AI-powered agents while maintaining robust security and compliance.

---
*Research by Microsoft Defender Security Research with contributions from Dor Edry, Uri Oren.*

**Further Reading:**

- [Securing Copilot Studio agents with Microsoft Defender](https://learn.microsoft.com/en-us/defender-cloud-apps/ai-agent-protection)
- [Real-time agent protection (Preview) – Microsoft Defender for Cloud Apps](https://learn.microsoft.com/en-us/defender-cloud-apps/real-time-agent-protection-during-runtime)
- [Copilot Studio Agent Builder](https://learn.microsoft.com/en-us/microsoft-365-copilot/extensibility/copilot-studio-agent-builder)

This post appeared first on "Microsoft Security Blog". [Read the entire article here](https://www.microsoft.com/en-us/security/blog/2026/01/23/runtime-risk-realtime-defense-securing-ai-agents/)
