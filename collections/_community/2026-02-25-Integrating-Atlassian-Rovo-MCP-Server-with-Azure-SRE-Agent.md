---
layout: "post"
title: "Integrating Atlassian Rovo MCP Server with Azure SRE Agent"
description: "This guide shows how to connect Azure SRE Agent to Atlassian Cloud products (Jira, Confluence, Compass, Jira Service Management) via the official Rovo MCP server. You'll learn how to configure secure authentication, set up connectors in the Azure portal, create custom subagents, and enable best practices through skills. The article covers permissions, available tools, troubleshooting, and security considerations for SRE and DevOps scenarios."
author: "dbandaru"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/get-started-with-atlassian-rovo-mcp-server-in-azure-sre-agent/ba-p/4497122"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-02-25 16:54:52 +00:00
permalink: "/2026-02-25-Integrating-Atlassian-Rovo-MCP-Server-with-Azure-SRE-Agent.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["API Token Authentication", "Atlassian Rovo MCP", "Audit Logging", "Automation", "Azure", "Azure SRE Agent", "Cloud Security", "Community", "Compass Integration", "Confluence Integration", "Connector Configuration", "CQL", "DevOps", "Incident Management", "Jira Integration", "Jira Service Management", "JQL", "OAuth 2.1", "Role Based Access Control", "Security", "Service Component Management", "Streamable HTTP"]
tags_normalized: ["api token authentication", "atlassian rovo mcp", "audit logging", "automation", "azure", "azure sre agent", "cloud security", "community", "compass integration", "confluence integration", "connector configuration", "cql", "devops", "incident management", "jira integration", "jira service management", "jql", "oauth 2dot1", "role based access control", "security", "service component management", "streamable http"]
---

dbandaru details how to integrate Atlassian Rovo MCP server with Azure SRE Agent for seamless, secure, and automated DevOps workflows across Jira, Confluence, Compass, and Jira Service Management.<!--excerpt_end-->

# Integrating Atlassian Rovo MCP Server with Azure SRE Agent

This comprehensive guide explains connecting Azure SRE Agent with key Atlassian Cloud products by leveraging the Atlassian Rovo MCP server. You will learn configuration steps, supported tools, permission models, and how to enhance your DevOps workflows.

## Overview

The Atlassian Rovo MCP server acts as a cloud-hosted bridge between your Atlassian Cloud site and Azure SRE Agent. Once set up, it enables:

- Real-time interaction with Jira, Confluence, Compass, and Jira Service Management via natural language commands.
- All actions are governed by your Atlassian user permissions for secure operations.
- Two authentication modes: OAuth 2.1 (for interactive consent) and API token (for automation).
- Direct Streamable-HTTP connectivity—no need for local proxies or containers.

## Key Capabilities by Product

| Product                    | Capabilities                                                                 |
|----------------------------|-----------------------------------------------------------------------------|
| **Jira**                   | Search issues (JQL), create/update tickets, add comments, transition issues  |
| **Confluence**             | Search (CQL), create/update pages, manage comments                           |
| **Compass**                | Manage services/components, define relationships, manage field definitions   |
| **Jira Service Management**| Query ops alerts, on-call schedules, team info, escalate alerts             |
| **Rovo Search**            | Natural language search across Jira and Confluence, fetch by ARI             |

## Prerequisites

- **Azure SRE Agent** deployed in your Azure subscription
- **Atlassian Cloud site** with access to Jira, Confluence, Compass, or JSM
- Appropriate user permissions on Atlassian products
- For API token: Org admin must enable API token auth in Rovo MCP server settings

## Step 1: Gather Atlassian Credentials

**Option A: API Token (Recommended for Automation)**

- Generate token via Atlassian account ([API token creation page](https://id.atlassian.com/manage-profile/security/api-tokens?autofillToken&expiryDays=max&appId=mcp&selectedScopes=all))
- Base64 encode credentials (`email:api_token`)
- Use in `Authorization: Basic <value>` header

**Option B: OAuth 2.1 (Browser-based Consent)**

- The SRE Agent prompts you to complete consent in browser
- Token remains valid until expired or revoked
- Site admin may need to grant 3LO consent on first use

## Step 2: Add MCP Connector in Azure Portal

1. Open the Azure portal, select your SRE Agent resource
2. Navigate to **Builder > Connectors** and select **Add connector**
3. Choose **MCP server** as connector type
4. Enter connection details:
    - **Connection type**: Streamable-HTTP
    - **URL**: `https://mcp.atlassian.com/v1/mcp`
    - **Authentication**: Custom headers (API token) or OAuth 2.0 (OAuth 2.1)
    - **Header Key**: `Authorization` (for API token or Bearer token)
5. Complete form and verify the **Connected** status

## Step 3: Create an Atlassian Subagent (Optional, for AI Expertise)

- Go to **Builder > Subagents**, select **Add subagent**
- Paste the provided YAML configuration to grant the subagent Atlassian expertise
- Reference the MCP connector you just created for tool access

## Step 4: Add an Atlassian Skill (Optional)

- Add skill to provide deep knowledge of JQL, CQL, and Atlassian workflows
- Paste config, then reference skill in subagent configuration

## Step 5: Test Your Integration

Try sample prompts in SRE Agent chat:

- Find all open bugs assigned to me in the PAYMENTS project
- Create a new Confluence page about “Architecture Review” in Engineering
- List all components in Compass
- Show me active ops alerts from last 24h

## Available Tools Summary

### Jira Tools (14)

- JQL search, issue creation, transition, commenting, etc.

### Confluence Tools (11)

- CQL search, page creation/update, comment management

### Compass Tools (13)

- Component and relationship management, activity event queries

### JSM Tools (4)

- Alert querying, on-call schedules, team info, escalation actions

### Rovo/Shared Tools (4)

- Natural language cross-search, fetch by ARI, user info, accessible site listing

## Troubleshooting & Security

- **Authentication issues**: Regenerate token or re-authorize in Atlassian UI
- **Permissions:** Map product permission requirements to user/scenario
- **Audit logging:** All activity is recorded for compliance
- **Data security:** Encrypted with HTTPS (TLS 1.2+)
- **Admin controls:** Enable/disable features, manage allowlists, monitor logs

## Limitations

- Some tools require specific auth (e.g. API token for JSM)
- Bitbucket integration listed in scopes, but not yet supported
- Certain Compass tools may require OAuth 2.1

## Related Resources

- [Official Documentation](https://support.atlassian.com/rovo/docs/getting-started-with-the-atlassian-remote-mcp-server/)
- [Supported Tools](https://support.atlassian.com/atlassian-rovo-mcp-server/docs/supported-tools/)
- [Security Risks](https://www.atlassian.com/blog/artificial-intelligence/mcp-risk-awareness)
- [Azure SRE Agent Docs](https://techcommunity.microsoft.com/category/azure/blog/appsonazureblog)

---

**Author:** dbandaru

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/get-started-with-atlassian-rovo-mcp-server-in-azure-sre-agent/ba-p/4497122)
