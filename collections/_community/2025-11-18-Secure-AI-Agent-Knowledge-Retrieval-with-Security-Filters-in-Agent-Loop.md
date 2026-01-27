---
external_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/secure-ai-agent-knowledge-retrieval-introducing-security-filters/ba-p/4467561
title: Secure AI Agent Knowledge Retrieval with Security Filters in Agent Loop
author: harimehta
feed_name: Microsoft Tech Community
date: 2025-11-18 17:25:53 +00:00
tags:
- Access Control
- ACL Support
- ADLS Gen2
- Agent Loop
- AI Agent
- Azure AI Search
- Azure Logic Apps
- Document Level Authorization
- Group Based Filtering
- Indexing
- Microsoft Entra
- RAG Workflows
- REST API
- Retrieval Augmented Generation
- Security Filters
- User Permissions
- Vector Search
section_names:
- ai
- azure
- security
primary_section: ai
---
harimehta explains how to create secure, permission-aware AI agents within Azure Logic Apps by leveraging Agent Loop and Azure AI Search to enforce document-level access controls for Retrieval-Augmented Generation workflows.<!--excerpt_end-->

# Secure AI Agent Knowledge Retrieval with Security Filters in Agent Loop

## Overview

This guide presents a new feature in Azure Logic Apps Agent Loop, allowing developers to implement document-level authorization in Retrieval-Augmented Generation (RAG) workflows. By integrating Azure AI Search's access control capabilities, developers can ensure their AI agents only retrieve and respond with data the user is authorized to access.

## Why Security Trimming Matters

- Ensures AI responses are tailored to user permissions
- Protects sensitive and confidential information
- Maintains compliance and organizational security policies

## Security Challenges for AI Agents

AI agents accessing organization-wide data pose significant security risks—without proper controls, they can leak confidential documents. Traditional solutions required manual, error-prone security implementations and parallel permission systems, which are hard to scale and prone to vulnerabilities.

## Solution: Agent Loop + Azure AI Search Native ACL Support

Azure Logic Apps Agent Loop now offers out-of-the-box integration with Azure AI Search's document-level access controls, resulting in:

- Secure-by-default AI knowledge retrieval
- Simple configuration in Logic Apps workflows (no custom code required)
- Confidence that AI agents respect user boundaries

## Two-Phase Security Architecture Explained

### Phase I: Permission-Aware Indexing

- During ingestion, index documents with a custom `UserIds` or `GroupIds` field in Azure AI Search linking documents to user or group Object IDs.
- **ADLS Gen2 Indexer** (Pull model): Automatically pulls ACL assignments from Azure Data Lake Storage.
- **Push API** (Push model): Developers can push documents and permission metadata via REST API or Azure SDKs.
- *Tip*: Use group IDs for easier permission management—just update group memberships instead of reindexing documents.

### Phase II: Filtered Retrieval via Agent Loop

- Use Azure AI Search actions in Logic Apps to automatically apply permission filters during vector search.

#### User-Based Filtering Example

```powershell
UserIds/any(u: u eq '@{currentRequest()['headers']['X-MS-CLIENT-PRINCIPAL-ID']}')
```

This extracts the authenticated user's principal ID from request headers and filters search results accordingly.

#### Group-Based Filtering Example

- Retrieve user's principal ID from headers
- Query Microsoft Entra for group memberships
- Apply filter:

```powershell
GroupIds/any(g: g in ('@{variables('userGroups')}'))
```

**Advantages:**

- Simplified permission maintenance
- Supports nested groups and RBAC
- Updates handled through group membership

## The Complete Agent Loop Flow

1. User sends a query to the AI agent
2. Agent Loop receives the query with authentication
3. Security filter is applied (user or group based)
4. Azure AI Search retrieves only authorized documents
5. LLM generates a response grounded in permitted data
6. Secure, compliant answer is returned

## Example: HR Knowledge Assistant

- Executives: Access to confidential compensation and merger data
- Managers: Team-specific policy info
- All employees: General benefits and policies

The same AI agent serves all users securely by automatically applying permission-based filters—no code duplication or risk of information leakage.

## Summary

Agent Loop with Azure AI Search ACL transforms secure AI agent development. Developers get a streamlined, configuration-driven experience while security teams gain enforceable and auditable access controls. Users can confidently interact with AI agents, assured their permissions are respected.

---
**Further Learning**

- [Step-by-step tutorial for configuring security filters](https://azure.github.io/logicapps-labs/docs/logicapps-ai-course/agent_functionality/add-security-filters-for-agent-knowledge-trimming)
- [Azure Search document-level access overview](https://learn.microsoft.com/en-us/azure/search/search-document-level-access-overview)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/secure-ai-agent-knowledge-retrieval-introducing-security-filters/ba-p/4467561)
