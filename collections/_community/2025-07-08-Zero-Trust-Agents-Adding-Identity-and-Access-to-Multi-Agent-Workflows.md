---
external_url: https://techcommunity.microsoft.com/t5/ai-azure-ai-services-blog/zero-trust-agents-adding-identity-and-access-to-multi-agent/ba-p/4427790
title: 'Zero-Trust Agents: Adding Identity and Access to Multi-Agent Workflows'
author: Thia
feed_name: Microsoft Tech Community
date: 2025-07-08 02:00:00 +00:00
tags:
- Access Management
- Agent Orchestration
- Auditability
- AutoGen
- Azure AI Foundry
- Azure OpenAI
- Enterprise Security
- Entra Agent ID
- Identity Management
- JWT
- OAuth2
- Security Best Practices
- WSO2 Asgardeo
- Zero Trust
section_names:
- ai
- azure
- security
---
Thia details how zero-trust security and identity management can empower secure, autonomous AI agents by utilizing Azure OpenAI, AutoGen, and WSO2 Asgardeo.<!--excerpt_end-->

## Overview

In this article, Thia presents a working reference architecture for introducing zero-trust security to autonomous, agent-driven AI workflows. The focus is a hotel booking scenario showcasing how each AI agent receives its own digital identity and access token, thus enabling secure, auditable operations.

### Key Concepts and Contributions

- **Zero-Trust for AI Agents:** The approach enforces continuous authentication and authorization for every agent action, eliminating implicit trust within agentic workflows.
- **Identity & Access Integration:** WSO2 Asgardeo serves as the identity and access management engine, issuing OAuth2 tokens for agents, while Microsoft's Azure OpenAI Service (via Azure AI Foundry) powers natural language capabilities and intelligence.
- **Security Architecture:** A specially designed SecureFunctionTool intercepts sensitive agent actions in the workflow, validating access privileges and ensuring all API calls are properly authorized—a true zero-trust design.

### How the System Works

1. **Architecture Components:**
   - **AutoGen** orchestrates multiple AI agents and tools.
   - **Hotel Booking Agent** handles booking tasks, calling external resources securely.
   - **WSO2 Asgardeo** acts as the OAuth2 authorization server, issuing identity tokens.
   - **SecureFunctionTool** demands a valid token for sensitive operations before execution.
   - **Azure OpenAI Service (GPT-4o)** provides intelligence and reasoning for agents while safeguarding enterprise data privacy.
2. **Code Flow:**
   - Agents authenticate and obtain tokens for protected actions.
   - SecureFunctionTool validates token authenticity, ensuring proper claims and permissions via local checks or introspection endpoints.
   - Every action is logged for accountability and auditability, ensuring all agent activities are traceable to specific credentials.

### Alignment with Microsoft Entra Agent ID

- The design is closely aligned with Microsoft's Entra Agent ID initiative, which will natively provide unique Azure directory identities to AI agents for robust security, governance, and audit. The solution remains standards-based and adaptable regardless of the underlying identity provider.
- Future scenarios include federating Asgardeo and Entra Agent ID and employing Microsoft security monitoring tools for advanced anomaly detection.

### Practical Advice

- The open-source pattern is reusable for architects and developers keen on secure, multi-agent AI systems.
- OAuth2 and open standards underpin the design, allowing integration with identity providers of choice.
- The approach future-proofs solutions as agentic identity features mature in platforms like Microsoft Entra.

### Conclusion

The article emphasizes the necessity of identity and zero-trust controls as AI agents gain autonomy. By leveraging proven IAM and AI services, organizations can realize secure, governable, and scalable AI-driven automation while maintaining complete oversight and auditability.

---
**References and Further Reading:**

1. [Zero Trust Architecture](https://features.csis.org/zero-trust-architecture/)
2. [Microsoft Extends Zero Trust to Agentic Workforce](https://www.microsoft.com/en-us/security/blog/2025/05/19/microsoft-extends-zero-trust-to-secure-the-agentic-workforce/)
3. [Azure Agentic AI Patterns](https://techcommunity.microsoft.com/blog/educatordeveloperblog/using-azure-ai-agent-service-with-autogen--semantic-kernel-to-build-a-multi-agen/4363121)
4. [WSO2 Identity Server](https://wso2.com/identity-server/)
5. [WSO2 Hotel Booking Sample Code](https://github.com/wso2/iam-ai-samples/tree/hotel_agent_imporvements/hotel-booking-agent-autogen)
6. [Entra Agent ID Announcement](https://techcommunity.microsoft.com/blog/microsoft-entra-blog/announcing-microsoft-entra-agent-id-secure-and-manage-your-ai-agents/3827392?utm_source=chatgpt.com)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/ai-azure-ai-services-blog/zero-trust-agents-adding-identity-and-access-to-multi-agent/ba-p/4427790)
