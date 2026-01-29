---
external_url: https://techcommunity.microsoft.com/t5/azure-architecture-blog/selecting-the-right-agentic-solution-on-azure-part-2-security/ba-p/4461215
title: Selecting the Right Agentic Solution on Azure – Security Deep Dive
author: pranabpaul
feed_name: Microsoft Tech Community
date: 2025-10-22 18:10:53 +00:00
tags:
- Agent Framework
- Agent Orchestration
- API Management
- AutoGen
- Azure AI Foundry Agent Service
- Azure Key Vault
- Azure Logic Apps
- Data Encryption
- Microsoft Entra ID
- OAuth2
- PII Protection
- Prompt Injection
- RBAC
- Security Best Practices
- Semantic Kernel
- Web Application Firewall
- AI
- Azure
- Security
- Community
section_names:
- ai
- azure
- security
primary_section: ai
---
pranabpaul explores the security landscape for agentic solutions on Azure, guiding readers through best practices around authentication, data protection, access control, secrets management, and safe orchestration of AI agents.<!--excerpt_end-->

# Selecting the Right Agentic Solution on Azure – Security Deep Dive

_Author: pranabpaul_

In this follow-up to Part 1, we tackle the security considerations for implementing agentic solutions on Azure. Building secure AI-driven workflows demands a clear understanding of the available Azure services, how agentic solutions are hosted, and the key threats that must be addressed through layered defenses.

## Overview

We build on the decision tree for agentic solutions and dive into the specifics of securing those solutions using Azure Logic Apps, Azure AI Foundry Agent Service, and the new Agent Framework (which succeeds both AutoGen and Semantic Kernel).

---

## Security in Azure Logic Apps Agent Workflows

- **Agent Workflows**: Agent loops as Azure Logic Apps actions, connecting to models via Azure OpenAI or Azure AI Foundry.
- **Authentication & Authorization**: Use Easy Auth (App Service Auth Preview) to integrate with Microsoft Entra ID for workflow authentication and authorization ([Learn More](https://learn.microsoft.com/en-us/azure/logic-apps/set-up-authentication-agent-workflows)).
- **Data at Rest**: Data is stored in Azure Storage with Microsoft-managed encryption keys by default; enhance by restricting access with Azure RBAC and controlling access to run history and workflow parameters. Secure inputs and outputs, especially for webhook triggers. ([Read more](https://learn.microsoft.com/en-us/azure/logic-apps/logic-apps-securing-a-logic-app?tabs=azure-portal))
- **Data in Transit**: Expose workflows via HTTPS. Leverage Azure API Management for applying policies, or Azure Application Gateway/Azure Front Door for Web Application Firewall (WAF) protection.
- **Resources**: Hands-on labs are available ([Logic Apps Labs](https://azure.github.io/logicapps-labs/docs/intro)).

---

## Azure AI Foundry Agent Service Security

- **Abstraction**: Infrastructure is fully managed by Microsoft. No direct compute, networking, or storage overhead for the customer, though bring-your-own-storage is possible.
- **Data at Rest**: Prompts, outputs, and workflow data are encrypted with AES-256 and remain in their deployment region. Customer-Managed Keys (CMK) are supported for enhanced compliance ([Details](https://learn.microsoft.com/en-us/azure/ai-foundry/responsible-ai/agents/data-privacy-security)).
- **Privacy**: Microsoft guarantees user data and outputs are never shared with other customers or upstream AI providers.
- **Network Security**: Support for private endpoints and integration with virtual networks. Notable limitations include subnet IP restrictions and regional constraints ([More info](https://learn.microsoft.com/en-gb/azure/ai-foundry/agents/how-to/virtual-networks)).
- **Upcoming Enhancements**: API Management for AI/Model/Tool APIs is coming soon.
- **APIM Use Case**: Defend AI API endpoints from misuse, inject access policies, and expose only approved workflows ([Gateway Guide](https://learn.microsoft.com/en-us/azure/architecture/ai-ml/guide/azure-openai-gateway-guide)).

---

## Security Practices for Agent Orchestrators (Agent Framework, Semantic Kernel, AutoGen, etc.)

- **Secret Management**: Never hard-code API keys for Foundry, OpenAI, or other tools. Use Azure Key Vault or environment variables. Always encrypt secrets at rest; minimize access scope and lifetime.
- **Access Control & Least Privilege**:
  - Adopt Role-Based Access Control (RBAC) everywhere.
  - Employ strong authentication (Microsoft Entra ID / OAuth2) for both agents and admin tools.
  - Rotate external service credentials regularly, and assign only necessary permissions.
- **Isolation and Sandboxing**:
  - Isolate plugins and enforce sandboxing for user-supplied code.
  - Implement resource limits to prevent denial-of-service or abuse scenarios.
- **Sensitive Data Protection**:
  - Encrypt all data at rest and in transit.
  - Remove or mask PII before using models.
  - Avoid unnecessary persistence of sensitive context.
  - Scrub logs and agent memory for inadvertent leaks.
- **Prompt & Query Security**:
  - Always sanitize or escape user inputs to prevent prompt injection.
  - Implement output filters, profanity checks, and regex validations on model outputs.
  - Limit context length to prevent resource exhaustion.
- **Logging, Observability, and Auditing**:
  - Maintain clear logs for all agent activity, tool invocations, and decision points.
  - Proactively monitor for anomalies and suspicious behavior.

---

## Conclusion

A rigorous, multi-layered approach to securing agentic solutions is essential, given the complexity of AI-driven workflows and the sensitivity of the data involved. Azure provides built-in controls and frameworks, but vigilance in applying best practices around authentication, authorization, encryption, and monitoring is crucial for every deployment.

For further learning, follow the links embedded above for platform documentation, hands-on labs, and security guides.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-architecture-blog/selecting-the-right-agentic-solution-on-azure-part-2-security/ba-p/4461215)
