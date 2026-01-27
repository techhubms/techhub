---
external_url: https://azure.microsoft.com/en-us/blog/agent-factory-creating-a-blueprint-for-safe-and-secure-ai-agents/
title: 'Agent Factory: Blueprint for Safe and Secure Enterprise AI Agents Using Azure AI Foundry'
author: Yina Arenas
feed_name: The Azure Blog
date: 2025-09-17 21:30:00 +00:00
tags:
- Agent Factory
- AI + Machine Learning
- AI Agent Blueprint
- Analytics
- Azure AI Foundry
- Compliance
- Data Protection
- DLP
- Enterprise AI
- Entra Agent ID
- EU AI Act
- Governance
- Management And Governance
- Microsoft Defender
- Microsoft Purview
- NIST AI RMF
- Observability
- Prompt Injection
- PyRIT
- Red Teaming Agent
- Risk Evaluation
- Shift Left Security
section_names:
- ai
- azure
- security
primary_section: ai
---
Yina Arenas discusses how enterprises can use Azure AI Foundry to implement a secure and trustworthy AI agent blueprint, addressing identity, governance, and real-time security controls for agentic AI in production environments.<!--excerpt_end-->

# Agent Factory: Blueprint for Safe and Secure Enterprise AI Agents Using Azure AI Foundry

**Author:** Yina Arenas  
**Source:** [Microsoft Azure Blog](https://azure.microsoft.com/en-us/blog/agent-factory-creating-a-blueprint-for-safe-and-secure-ai-agents/)

## Overview

Azure AI Foundry delivers a comprehensive blueprint for enterprises to create safe and secure AI agents. By combining security, safety, and governance in a layered process, Foundry helps organizations embed trust into every stage of AI agent development—from proof of concept to production deployment.

## Key Areas of Trust

- **Identity Management:** Every agent receives a unique Entra Agent ID, providing full lifecycle visibility and reducing the risk of untracked “shadow agents.”
- **Data Protection by Design:** Sensitive information is classified and governed with built-in safeguards, honoring Microsoft Purview sensitivity labels and DLP (Data Loss Prevention) policies across agent operations.
- **Built-In Security Controls:** Harm and risk filters, prompt injection classifiers, groundedness checks, and protected material detection are implemented as primary defense mechanisms.
- **Continuous Risk Evaluation:** Automated safety evaluations and adversarial testing with tools like the Red Teaming Agent and PyRIT probe agents for vulnerabilities both pre- and post-deployment.
- **Enterprise Monitoring:** Telemetry streams directly into Microsoft Defender XDR and existing compliance/security workflows, allowing for real-time response and oversight.

## Azure AI Foundry Capabilities

- **Entra Agent ID:** Full audit trail and ownership tracking for every agent.
- **Agent Controls:** Cross-prompt injection scanning (including prompt documents, tool responses, and external triggers), misalignment and high-risk action prevention, and advanced risk/harm filtering.
- **Lifecycle Evaluations:** Harm/risk checks, groundedness scoring, and protected material scans, plus adversarial testing at scale.
- **Custom Data Control:** Enterprise integration with existing Azure resources for secure storage and processing.
- **Network Isolation:** Supports private network isolation through custom virtual networks and subnet delegation.
- **Microsoft Purview Integration:** Sensitivity labeling/retention policies flow through agents to output; frameworks such as the EU AI Act and NIST AI RMF are supported for compliance tracking.
- **Microsoft Defender Integration:** Automated surfacing of security alerts, incident telemetry, and risk evaluations, enabling holistic response via the Defender XDR platform.
- **Governance Collaborators:** Credo AI and Saidot partnerships allow mapping of results to regulatory frameworks and responsible AI practices.

## Blueprint in Action

1. **Start with identity.** Assign Entra Agent IDs for agent discovery and ownership.
2. **Integrated controls.** Leverage prompt shields, risk filters, and threat mitigations.
3. **Continuous evaluation.** Run safety/risk checks and adversarial exercises throughout the agent lifecycle.
4. **Protect sensitive data.** Apply Purview/DLP labels and enforce data boundaries by design.
5. **Monitor and respond.** Stream real-time metrics into Defender XDR and leverage Foundry’s observability tools.
6. **Regulatory mapping.** Use governance partners to align with legal and ethical frameworks.

## Example Use Cases

- **EY:** Utilizes Foundry for model evaluations comparing quality, safety, and cost at scale.
- **Accenture:** Tests Red Teaming Agent to simulate attacks and validate agent resilience before production roll-out.

## Further Learning and Resources

- [Azure AI Foundry](https://ai.azure.com/)
- [Responsible AI Training](https://learn.microsoft.com/training/modules/responsible-ai-studio/)
- [Microsoft Secure Event](https://register.secure.microsoft.com/)

For technical best practices, see the complete Agent Factory blog series covering use cases, agent development, observability, and integration patterns.

This post appeared first on "The Azure Blog". [Read the entire article here](https://azure.microsoft.com/en-us/blog/agent-factory-creating-a-blueprint-for-safe-and-secure-ai-agents/)
