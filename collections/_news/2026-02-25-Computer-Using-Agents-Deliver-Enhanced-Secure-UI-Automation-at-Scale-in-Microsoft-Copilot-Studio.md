---
external_url: https://www.microsoft.com/en-us/microsoft-copilot/blog/copilot-studio/computer-using-agents-now-deliver-more-secure-ui-automation-at-scale/
title: Computer-Using Agents Deliver Enhanced Secure UI Automation at Scale in Microsoft Copilot Studio
author: stclarke
primary_section: ai
feed_name: Microsoft News
date: 2026-02-25 21:37:10 +00:00
tags:
- AI
- Audit Logs
- Authentication
- Azure
- Azure Key Vault
- Cloud PC
- Company News
- Computer Using Agents
- Copilot Studio
- Governance
- Intune
- Microsoft Entra ID
- Microsoft Power Platform
- News
- Power Automate
- RPA
- Security
- Session Replay
- UI Automation
- Windows 365 For Agents
section_names:
- ai
- azure
- security
---
stclarke explores recent advancements in Microsoft Copilot Studio’s computer-using agents, focusing on secure, scalable UI automation through AI, Azure integration, and enterprise-grade security features.<!--excerpt_end-->

# Computer-Using Agents Deliver Enhanced Secure UI Automation at Scale in Microsoft Copilot Studio

## Overview

This post discusses major improvements to computer-using agents (CUAs) in Microsoft Copilot Studio, addressing real customer needs around security, scale, and monitoring for UI automation in both web and desktop environments.

## Key Updates

### Multi-Model Flexibility

- **Model Choice:** CUAs now support multiple foundation models, including Anthropic’s Claude Sonnet 4.5 and OpenAI’s Computer-Using Agent.
- **Benefits:** Choose the model best suited for your application—use OpenAI for orchestrating complex flows or Anthropic’s model for nuanced, dynamic UIs.

### Secure Authentication and Credential Management

- **Built-in Credentials:** Agents can securely log into web and desktop apps using built-in credentials.
- **Storage Options:** Credentials are stored either in encrypted internal storage (Power Platform) or with Azure Key Vault for enterprise-grade secret management.
- **Isolation:** Credentials remain inaccessible to AI models, ensuring that only authorized agents can use them.

### Advanced Monitoring and Observability

- **Session Replay:** Step-by-step playback with screenshots enables teams to understand every action taken by an agent.
- **Audit Logs:** Comprehensive logs detail action types, coordinates, resources accessed, timestamps, and more.
- **Compliance Integration:** Send logs to Microsoft Purview; configure Dataverse logging and retention periods to align with governance requirements.

### Scalable Infrastructure

- **Managed Cloud PCs:** New Cloud PC pools powered by Windows 365 provide scalable, cloud-hosted run environments that support spikes in demand without hardware management overhead.
- **Enterprise Integration:** Cloud PCs are Microsoft Entra joined and Intune enrolled, aligning with modern enterprise management practices.

### Extending Existing Automations

- **CUA and Power Automate Synergy:** CUAs complement existing RPA/Power Automate flows, offering adaptive reasoning where traditional selectors or APIs aren’t robust.
- **Reduced Maintenance:** Classic RPAs remain for deterministic tasks; CUAs handle dynamic, changing interfaces, reducing the burden of script maintenance.

## Getting Started

To try computer-using agents in a US-based Microsoft Copilot Studio environment:

1. Create or open an agent in Copilot Studio.
2. Navigate: **Tools → Add tool → New tool → computer use**.
3. Describe the automation task in natural language.
4. (Optional) Select a model, set up credentials, and configure a Cloud PC pool.

For detailed steps and best practices, see the [official documentation](https://learn.microsoft.com/en-us/microsoft-copilot-studio/computer-use).

## Feedback and Community

Microsoft continues to invest in CUAs, with customer feedback directly informing new features. To provide input or connect with the community:

- **Email:** [computeruse-feedback@microsoft.com](mailto:computeruse-feedback@microsoft.com)
- **Join:** [Copilot Studio community](https://community.powerplatform.com/forums/thread/?groupid=db8f53c2-767d-47d6-a1ae-fe4c828a6553)

## References

- [Computer-using agents product page](https://www.microsoft.com/en-us/microsoft-copilot/blog/copilot-studio/computer-using-agents-now-deliver-more-secure-ui-automation-at-scale/)
- [Microsoft Copilot Studio](https://www.microsoft.com/en-us/microsoft-365-copilot/microsoft-copilot-studio/)
- [Azure Key Vault](https://azure.microsoft.com/en-us/products/key-vault/)
- [Cloud PC pool for agents](https://learn.microsoft.com/en-us/windows-365/agents/introduction-windows-365-for-agents)

This post appeared first on "Microsoft News". [Read the entire article here](https://www.microsoft.com/en-us/microsoft-copilot/blog/copilot-studio/computer-using-agents-now-deliver-more-secure-ui-automation-at-scale/)
