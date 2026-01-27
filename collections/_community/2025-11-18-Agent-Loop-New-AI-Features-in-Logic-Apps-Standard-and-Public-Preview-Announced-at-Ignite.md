---
external_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/agent-loop-ignite-update-new-set-of-ai-features-arrive-in-public/ba-p/4470764
title: 'Agent Loop: New AI Features in Logic Apps Standard and Public Preview Announced at Ignite'
author: DivSwa
feed_name: Microsoft Tech Community
date: 2025-11-18 23:47:06 +00:00
tags:
- Agent Loop
- AI Automation
- AI Gateway
- Azure AI Search
- Azure API Management
- Azure Logic Apps
- BYOM
- Connector Ecosystem
- Document Level Security
- Identity And Access Management
- MCP
- Microsoft Entra ID
- Microsoft Teams Integration
- Okta Integration
- RAG
- Serverless
- Workflow Designer
section_names:
- ai
- azure
- devops
- security
primary_section: ai
---
DivSwa announces the release of Agent Loop General Availability in Logic Apps Standard, alongside a suite of new AI capabilities in public preview, offering developers advanced options for secure, scalable agent-based workflows.<!--excerpt_end-->

# Agent Loop Ignite Update: AI Features Now in Public Preview

**Author: DivSwa**

## Overview

At Microsoft Ignite, the General Availability (GA) of Agent Loop in Azure Logic Apps Standard was announced, along with a wave of new AI-driven capabilities in Public Preview. This set of enhancements expands the horizons for developers building secure, intelligent, agentic workflows at scale.

---

## Key Announcements

### 1. Agent Loop General Availability & Public Preview Features

- **Production-ready agentic automation** available to all Logic Apps Standard customers.
- **Public Preview** adds powerful features: run agents in Consumption SKU, bring your own AI models (BYOM), call any tool via MCP, deploy agents in Teams, secure RAG with document-level permissions, Okta onboarding, and a redesigned workflow designer.

### 2. Agentic Workflows in Consumption SKU

- Agent Loop is now on **Azure Logic Apps Consumption** (serverless, pay-as-you-go).
- Developers can make any workflow intelligent using agent loop actions, with no infrastructure provisioning required.
- **Easy onboarding** and simple authentication process.
- Connects with 1,400+ Logic Apps connectors for broad tool integration.
- Frictionless prototyping and easy scaling to Logic Apps Standard with more controls.

**Preview limitations:** Initial rollout has limited regions, no VS Code local development, and no nested agents or MCP tools yet.

### 3. Bring Your Own Model (BYOM) with AI Gateway

- Use any AI model within Agent Loop—Foundry, on-prem, or private cloud supported.
- **Deep integration with Azure API Management (APIM) AI Gateway:**
  - Central control plane for agent-model connections.
  - Unified, governed API endpoint for authentication, rate limits, monitoring, and observability.
  - Allows model upgrades or config updates without changing app workflows.
- Direct external and local/VNET model integration supported, enabling high flexibility and data residency compliance.

### 4. Model Context Protocol (MCP) Tool Integration

- New support for **MCP** in Logic Apps Standard lets agents discover and invoke external tools through standardized interfaces.
- _Three integration modes:_
  - Bring Your Own MCP connector (custom external server)
  - Managed MCP connector (Azure-hosted, managed experience)
  - Custom MCP connector (OpenAPI-based, tenant/private use)
- Managed and custom MCP connectors support on-behalf-of (OBO) authentication for user-context-aware tool use across workflows.

### 5. Deploy Conversational Agents to Teams and Microsoft 365

- Logic Apps agentic workflows now deploy directly to Teams.
- Agents act as native conversation participants in Teams (standups, approvals, queries, knowledge lookups, etc.).
- Integrated identity and security via Azure Bot Service and Logic Apps.
- Seamless experience across Microsoft 365 surfaces supporting Bots/web endpoints.

### 6. Secure Retrieval Augmented Generation (RAG) with Azure AI Search ACLs

- Agent Loop now integrates with **Azure AI Search ACLs** for document-level security.
- Only information the user is permitted to see will be shown by the AI agent.
- Indices can be configured with user/group permissions; results filtered accordingly.
- Eases development effort for compliance and personalized knowledge assistants.

### 7. Okta Identity Provider Support

- Agent Loop agents now support **Okta** alongside Microsoft Entra ID.
- Okta integration passes user and group context to agents for user-aware decisions and actions.
- Simplifies agent adoption in non-Microsoft IdP environments while maintaining strong security and policy enforcement.

### 8. Redesigned Logic Apps Workflow Designer (Public Preview)

- Major UI and UX update for the Logic Apps designer is now available.
- Enhancements include:
  - Unified interface for visual canvas, code view, settings, and run history
  - Draft Mode with auto-save and safe testing
  - Sticky notes and markdown support for inline documentation
  - Improved search (backend indexed)
  - Monitoring and debugging features, including hierarchical timeline view

---

## Resources

- [Agent Loop GA Announcement](https://aka.ms/agentloop/ga)
- [Agent Loop for Consumption](https://aka.ms/agentloop/consumption)
- [BYOM Documentation](http://aka.ms/agentloop/byom)
- [MCP Integration Guide](https://aka.ms/AgentLoop-MCP)
- [Teams Deployment Instructions](https://aka.ms/agentloop/teams)
- [Secure Knowledge Retrieval](https://techcommunity.microsoft.com/blog/integrationsonazureblog/%F0%9F%94%90secure-ai-agent-knowledge-retrieval---introducing-security-filters-in-agent-lo/4467561)
- [Okta Integration Steps](https://aka.ms/agentloop/okta)
- [Workflow Designer Overview](https://aka.ms/ladesignerv3)

---

## Feedback and Next Steps

DivSwa encourages user feedback on which capabilities to prioritize for maximum impact in the Agent Loop and Logic Apps roadmap.

---

**Version**: 5.0  
**Updated**: Nov 18, 2025

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/agent-loop-ignite-update-new-set-of-ai-features-arrive-in-public/ba-p/4470764)
