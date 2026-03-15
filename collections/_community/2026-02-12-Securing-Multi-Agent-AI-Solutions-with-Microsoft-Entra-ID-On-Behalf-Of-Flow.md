---
external_url: https://techcommunity.microsoft.com/t5/azure-architecture-blog/securing-a-multi-agent-ai-solution-focused-on-user-context-the/ba-p/4493308
title: Securing Multi-Agent AI Solutions with Microsoft Entra ID On-Behalf-Of Flow
author: Charles_Chukwudozie
primary_section: ai
feed_name: Microsoft Tech Community
date: 2026-02-12 07:47:13 +00:00
tags:
- AI
- AI Governance
- Azure
- Azure App Service
- Azure Databricks
- Chainlit
- Community
- Enterprise Security
- Human in The Loop
- Identity Provider
- LangGraph
- Microsoft Entra ID
- OAuth 2.0
- On Behalf Of Flow
- Python
- RBAC
- Security
- Unity Catalog
- Zero Trust
section_names:
- ai
- azure
- security
---
Charles_Chukwudozie explains how to implement secure user identity and access controls in enterprise AI multi-agent architectures using Microsoft Entra ID OBO flow, Databricks, and Azure services.<!--excerpt_end-->

# Securing a Multi-Agent AI Solution Focused on User Context & the Complexities of On-Behalf-Of

**Author: Charles_Chukwudozie**

## Introduction

A recurring challenge in enterprise AI-powered applications is preserving user identity and access controls when AI agents interact with backend services. Common patterns—like using shared service accounts or PAT tokens—often bypass finely-tuned security controls, exposing organizations to unauthorized data access.

This article describes a solution that employs [Microsoft Entra ID](https://learn.microsoft.com/en-us/entra/fundamentals/what-is-entra) On-Behalf-Of (OBO) flow with a custom multi-agent architecture orchestrated by LangGraph and driven by AI-powered experiences in Databricks Genie and Chainlit, all hosted on Azure.

## Architecture Overview

**Key Components:**

- **Chainlit**: Python web interface for LLM applications using OAuth 2.0 authentication.
- **Azure App Service**: Managed hosting with built-in authentication and autoscaling.
- **LangGraph**: Python-based open-source multi-agent orchestration framework.
- **Azure Databricks Genie**: Natural language to SQL agent, enforcing RBAC.
- **Azure Cosmos DB**: Memory and checkpoint storage.
- **Microsoft Entra ID**: Identity provider with OBO support.

**Agent Types:**

- **Genie Agent**: Handles read-only, natural language-to-SQL queries on behalf of users with their identity.
- **Task Agent**: Performs sensitive (write/delete) SQL operations, always requiring human-in-the-loop (HITL) approval.
- **Memory Agent**: Handles shared memory; doesn't require per-user authentication.

## Problem: Default OAuth Token Limitations

Chainlit's default Microsoft Entra ID OAuth integration expects Graph API scopes, which cannot be used for the OBO flow targeting downstream services like Databricks. The returned token's audience is for Graph API rather than custom applications, impeding correct delegation.

**Solution:** Implement a custom OAuth provider to:

- Request `api://{client_id}/access_as_user` as OAuth scope
- Ensure tokens have the correct audience (your app, not Graph API)
- Extract user info from ID token claims instead of calling Graph API

## Token Exchange and Integration Flow

Once the user authenticates, the solution:

1. Acquires an access token with your app's client ID as audience
2. Exchanges the token for a Databricks-scoped token using [MSAL for Python](https://learn.microsoft.com/en-us/entra/msal/python/)
3. Injects the Databricks-scoped token into the Genie agent
4. Ensures that each query/operation in Databricks is performed under the real user's identity, enforcing all [Unity Catalog](https://learn.microsoft.com/en-us/azure/databricks/data-governance/unity-catalog/) RBAC permissions

## Per-User Agent Isolation

For security, the system **never caches user-specific agents globally**. Each user session provisions a dedicated Genie agent instance to ensure RBAC and privacy boundaries are always enforced.

## Human-in-the-Loop Safeguards for Destructive Operations

Sensitive SQL commands (e.g., `DELETE` or `UPDATE`) require explicit user approval via LangGraph's interrupt mechanism—providing oversight beyond code-level RBAC. Intent analysis with LLMs powers the detection of potentially destructive actions.

## Entra ID App Registration Requirements

To support this secure flow:

- Add Azure Databricks `user_impersonation` to API permissions (requires admin consent)
- Expose your API scope as `access_as_user` (URI: `api://{client-id}`)
- Configure redirect URI for OAuth callback

## Lessons Learned

- **Token audience must match your app**: OBO will fail otherwise.
- **Never cache clients per user globally**: User isolation is critical.
- **Leverage the ID token for user info**: When Graph API cannot be called.
- **Combine RBAC with HITL**: Even with the right access controls, certain actions require explicit user oversight.

## Outcomes and Conclusion

By applying Microsoft Entra ID OBO flow, the architecture delivers:

- Preserved user context/identity across all AI interactions
- Full RBAC and audit trails at the Databricks/Unity Catalog level
- Zero-trust: AI agents never hold broader permissions than intended
- Secure human-in-the-loop controls for critical data modifications

This approach sets a strong foundation for enterprise AI governance and aligns with Microsoft's Secure Future Initiative (SFI) and Zero Trust principles. As multi-agent AI becomes standard, such identity-focused architectures will be essential for scalability and trust.

Stay tuned for the following post, which addresses these topics from a CXO perspective and explores organization-wide implications of identity-first AI governance.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-architecture-blog/securing-a-multi-agent-ai-solution-focused-on-user-context-the/ba-p/4493308)
