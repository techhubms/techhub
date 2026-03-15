---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/get-started-with-pagerduty-mcp-server-in-azure-sre-agent/ba-p/4497124
title: Integrate PagerDuty MCP Server with Azure SRE Agent for Automated Incident Management
author: dbandaru
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-02-25 16:57:08 +00:00
tags:
- API Authentication
- Audit Logging
- Automation
- Azure
- Azure SRE Agent
- Change Events
- Community
- Connector Configuration
- DevOps
- Escalation Policies
- Event Orchestration
- Incident Management
- Incident Workflows
- Monitoring
- On Call Scheduling
- PagerDuty MCP Server
- Security
- Service Integration
- Streamable HTTP
- User API Token
section_names:
- azure
- devops
- security
---
dbandaru provides a comprehensive guide to integrating PagerDuty MCP server with Azure SRE Agent, outlining secure incident management, configuration steps, and automation best practices for operational teams.<!--excerpt_end-->

# Integrate PagerDuty MCP Server with Azure SRE Agent for Automated Incident Management

Connect Azure SRE Agent to PagerDuty's incident management platform using the official MCP server for seamless management of incidents, on-call schedules, services, escalation policies, and more.

## Overview

The PagerDuty MCP server is a cloud-hosted bridge connecting your PagerDuty account to Azure SRE Agent. It enables real-time, secure interaction with incidents, services, teams, and workflows, using natural language and modern API protocols. The server employs Streamable HTTP transport with a single `Authorization` custom header and does not require additional plugins, proxies, or container setups.

## Key Capabilities

- **Incidents:** Create, list, manage, add notes/responders, find alerts, related or past incidents
- **Services:** CRUD operations on services
- **On-Call & Schedules:** List/manage schedules, on-calls, overrides, users
- **Escalation Policies:** List/get policy details
- **Teams & Users:** Manage teams and users
- **Event Orchestration & Workflows:** Route events, manage execution rules, start incident workflows
- **Alert Grouping, Change Events, Log Entries, Status Pages**

## Prerequisites

- Azure SRE Agent deployed in your Azure environment
- Active PagerDuty account with relevant permissions
- PagerDuty User API Token (created from User Settings > API Access)

## Step 1: Create a PagerDuty API Token

1. Log into PagerDuty ([US](https://app.pagerduty.com/) / [EU](https://app.eu.pagerduty.com/))
2. Go to **My Profile** > **User Settings** > **API Access**
3. Create API User Token, name it descriptively, and **copy it immediately** (displayed once)
4. Recommended: Use a dedicated service account for production, granting only required permissions (Responder/Observer)

## Step 2: Add MCP Connector in Azure Portal

1. Navigate to your SRE Agent resource
2. Select **Builder > Connectors** > **Add connector**
3. Choose **MCP server** (user-provided)
4. Configure:
    - **Name:** `pagerduty-mcp`
    - **Connection Type:** Streamable-HTTP
    - **URL:** `https://mcp.pagerduty.com/mcp` (or EU endpoint)
    - **Authentication:** Custom headers
    - **Authorization:** `Token <your-pagerduty-api-token>` (must use `Token` prefix, not `Bearer`)
5. Add the connector and verify it shows **Connected**

## Step 3: (Optional) Create a PagerDuty Subagent

You can create a specialized subagent (e.g., `PagerDutyIncidentExpert`) by defining its configuration in YAML. This subagent will have focused knowledge and tool access for incident response and PagerDuty workflows. Example capabilities include listing, creating, and managing incidents, services, schedules, and more.

## Step 4: (Optional) Add a PagerDuty Skill

Skills provide contextual and operational knowledge for handling incidents, on-call schedules, event orchestration, and related tasks. Attach the skill to your subagent for more effective workflows.

## Step 5: Test the Integration

Open a chat session with your SRE Agent and try prompts such as:

- "Show me all currently triggered incidents."
- "Who is currently on-call for the escalation policy X?"
- "Create a new high-urgency incident for service Y."
- "List all teams and their members."

## Tools and Workflow Automation

The MCP server exposes 60+ tools, both read and write. Example operations:

- `get_incident`, `list_incidents`, `create_incident`, `manage_incidents`
- `list_oncalls`, `get_schedule`, `create_schedule_override`
- `list_services`, `get_service`, `update_service`
- `list_event_orchestrations`, `append_event_orchestration_router_rule`

## Troubleshooting

- **401 Unauthorized:** Check token validity, format (`Token <value>`), and permissions
- **403 Forbidden:** User account lacks required PagerDuty role
- **Connection issues:** Allow outbound HTTPS to `mcp.pagerduty.com` (or EU endpoint)
- **Region errors:** Ensure correct regional endpoint
- **Permissions/data:** Make sure token's user account has proper asset access

**Test your connection:**

```bash
curl -I "https://mcp.pagerduty.com/mcp" -H "Authorization: Token <your-api-token>"
```

Expect `200 OK` for successful authentication.

## Limitations

- API token permissions are user-scoped and must match required action levels
- Only US and EU endpoints supported
- No dedicated portal connector; use generic MCP connector
- Rate limits enforced as per PagerDuty plan
- Manual token rotation is recommended

## Security Considerations

- Token-based authentication; use minimal required access
- All traffic is encrypted via TLS
- Review audit logs and practice regular key rotation
- Use service accounts for production, avoid over-permissioning

## References

- [PagerDuty MCP Server documentation](https://developer.pagerduty.com/docs/pagerduty-mcp-server/)
- [Azure SRE Agent docs](https://techcommunity.microsoft.com/mcp-integration.md)
- [PagerDuty Audit Records](https://support.pagerduty.com/docs/audit-records)

---
**By dbandaru**

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/get-started-with-pagerduty-mcp-server-in-azure-sre-agent/ba-p/4497124)
