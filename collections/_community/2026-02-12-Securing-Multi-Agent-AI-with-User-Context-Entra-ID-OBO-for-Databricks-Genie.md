---
external_url: https://techcommunity.microsoft.com/t5/azure-architecture-blog/securing-multi-agent-ai-with-user-context-entra-id-obo-for/ba-p/4493308
title: 'Securing Multi-Agent AI with User Context: Entra ID OBO for Databricks Genie'
author: Charles_Chukwudozie
primary_section: ai
feed_name: Microsoft Tech Community
date: 2026-02-12 06:27:56 +00:00
tags:
- AI
- AI Governance
- Azure
- Azure App Service
- Azure Cosmos DB
- Azure Databricks
- Chainlit
- Community
- Human in The Loop
- Identity Delegation
- LangGraph
- Microsoft Entra ID
- MSAL
- OAuth 2.0
- On Behalf Of Flow
- RBAC
- Security
- Unity Catalog
- Zero Trust
section_names:
- ai
- azure
- security
---
Charles Chukwudozie explains how his team implemented Microsoft Entra ID OBO authentication to maintain secure user identity and RBAC inside an enterprise AI multi-agent system. The post emphasizes Databricks Genie, LangGraph, and lessons learned in managing secure, zero-trust data access.<!--excerpt_end-->

# Securing Multi-Agent AI with User Context: Entra ID OBO for Databricks Genie

## Introduction

When building AI-powered enterprise applications, a recurring challenge is preserving user identity and access controls as AI agents interact with backend services on the user's behalf. Standard solutions often use shared credentials or PAT tokens, bypassing critical row-level security (RLS) and other data governance, leading to potential data exposure.

This article details how we solved this for an enterprise customer by employing Microsoft Entra ID On-Behalf-Of (OBO) authentication in a custom multi-agent solution powered by [LangGraph](https://docs.langchain.com/oss/python/langgraph/overview), [Chainlit](https://docs.chainlit.io/authentication/oauth), [Databricks Genie](https://learn.microsoft.com/en-us/azure/databricks/genie/), and [Azure Cosmos DB](https://learn.microsoft.com/en-us/azure/cosmos-db/overview). Our approach enables each agent (such as Databricks Genie and a custom data agent) to operate with the authenticating user's permissions, ensuring RBAC compliance–and keeping a secure audit trail for every operation.

## Architecture Overview

Key system components:

- **Chainlit**: A Python-based conversational interface framework, OAuth 2.0-compliant, customizable to customer UI and reducing frontend maintenance.
- **Azure App Service**: Used for managed hosting, with built-in auth and scaling.
- **LangGraph**: Open-source agent orchestration, powering multi-agent workflows.
- **Azure Databricks Genie**: Natural language-to-SQL agent for querying corporate data.
- **Azure Cosmos DB**: Storing conversation history and checkpoints.
- **Microsoft Entra ID**: Identity provider with On-Behalf-Of flow support.

Agents are separated by function and access:

- **Genie**: Executes read-only NL-to-SQL using per-user OBO tokens.
- **Task Agent**: Handles sensitive SQL (modifications/deletes), requires human-in-the-loop (HITL) approval.
- **Memory Agent**: Supports long-term memory, without per-user authentication.

## Chainlit OAuth and OBO Challenges

Out-of-the-box, Chainlit’s OAuth with Microsoft Entra ID issues tokens scoped for [Microsoft Graph API](https://learn.microsoft.com/en-us/graph/overview), not for custom backend or OBO flows. This restricts token usage exclusively to Graph, making downstream services like Databricks inaccessible through delegated user context.

Key problems:

- Token audience is graph.microsoft.com (not your app or resource)
- Cannot be used for OBO flow with Databricks or other custom APIs

**Solution:** Replace the default Chainlit OAuth with a custom provider that requests the scope `api://{client_id}/access_as_user`. This returns a token valid for OBO exchange.

- **Extract user details** from the ID token (since Graph API is inaccessible with this token).

## The OBO Exchange Process

With the correct user access token, utilize the [MSAL Python library](https://learn.microsoft.com/en-us/entra/msal/python/) to exchange for a Databricks-scoped token:

- **Audience:** Databricks resource ID
- **Claims:** User UPN, OID — critical for RBAC
- **Usage:** Databricks SDK/API, respects all Unity Catalog policies for the user

## Agent Instance Isolation

To maintain proper user isolation and prevent privilege leaks, never globally cache agent instances. Each user must have their own Genie (and other) agent running solely under their identity and session context.

## OBO Token Integration with Databricks Genie

During Genie agent initialization, inject the OBO-acquired user token into the Databricks SDK (WorkspaceClient). This ensures:

- All agent queries execute as the authenticated user
- No agent ever exceeds user-authorized permissions

## Human-in-the-Loop for Destructive Operations

Read-only (SELECT) natural language queries are allowed by Genie. For mutating operations (DELETE/UPDATE), the system uses LangGraph’s interrupt triggers and LLM-based intent detection to require explicit user (or HITL) approval before proceeding. The OBO token guarantees that custom scripts execute solely under the user’s actual permissions.

## Entra ID App Registration Checklist

To enable this flow, configure the following in your Entra ID app registration:

1. **API Permissions:** Azure Databricks → `user_impersonation`, with admin consent
2. **Expose API:** Scope `access_as_user` via URI `api://{client_id}`
3. **Redirect URI:** `{your-app-url}/auth/oauth/azure-ad/callback`

## Lessons Learned

- Token audience matters: always ensure correct audience when acquiring tokens
- Never cache user-specific clients globally
- Use ID token claims to extract user info if Graph API is unreachable
- Enforce HITL for all destructive SQL; RBAC alone is not enough

## Conclusion and Outlook

By integrating Microsoft Entra ID OBO throughout the multi-agent system, the solution achieves:

- Persistent user identity and RBAC enforcement for all agent actions
- Secure, auditable, and zero-trust permissions architecture
- Applicability beyond Databricks (any OAuth 2.0-supporting Azure service)
- Compliance with Microsoft SFI (Secure Future Initiative) and enterprise governance

Future enterprise AI platforms will require centralized, standardized user delegation and identity controls across agents and services. Solutions are emerging (e.g., Entra Agent ID, Azure AI Foundry) to answer these challenges at scale.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-architecture-blog/securing-multi-agent-ai-with-user-context-entra-id-obo-for/ba-p/4493308)
