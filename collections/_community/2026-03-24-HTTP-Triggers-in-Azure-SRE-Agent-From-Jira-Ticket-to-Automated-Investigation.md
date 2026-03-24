---
primary_section: ai
feed_name: Microsoft Tech Community
tags:
- AI
- API Management
- Autonomous Agents
- Azure
- Azure AD Authentication
- Azure CLI
- Azure Functions
- Azure Logic Apps
- Azure PostgreSQL Flexible Server
- Azure SRE Agent
- Community
- Data Plane RBAC
- DevOps
- HTTP Triggers
- Incident Management
- Incident Response
- Jira
- Kusto
- Log Analytics
- Managed Identity
- MCP
- Microsoft Entra ID
- Observability
- RBAC
- Security
- SRE
- Webhook
section_names:
- ai
- azure
- devops
- security
author: Vineela-Suri
title: 'HTTP Triggers in Azure SRE Agent: From Jira Ticket to Automated Investigation'
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/http-triggers-in-azure-sre-agent-from-jira-ticket-to-automated/ba-p/4504960
date: 2026-03-24 18:13:32 +00:00
---

Vineela-Suri walks through wiring Jira incidents into Azure SRE Agent using HTTP Triggers and an Azure Logic App with Managed Identity, so each new ticket can kick off an automated investigation and post findings back to Jira.<!--excerpt_end-->

# HTTP Triggers in Azure SRE Agent: From Jira Ticket to Automated Investigation

## Introduction

Many teams run observability, incident management, ticketing, and deployment on platforms outside of Azure—Jira, Opsgenie, Grafana, Zendesk, GitLab, Jenkins, Harness, or internal tools. These are the systems where alerts fire, tickets get filed, deployments happen, and operational decisions are made every day.

**HTTP Triggers** in Azure SRE Agent let you connect those systems to SRE Agent by sending a simple HTTP POST. That turns events from external platforms into automated agent actions without manual copy/paste or context switching.

This walkthrough demonstrates connecting **Jira** to SRE Agent so every new incident ticket automatically triggers an investigation, and the agent posts findings back to the Jira ticket.

## The Scenario: Jira Incident → Automated Investigation

- Production apps are backed by **Azure PostgreSQL Flexible Server**.
- Jira is used for incident tracking.
- Today, P1/P2 incidents require an on-call engineer to manually triage: read the ticket, check dashboards, query logs, and correlate deployments.

With HTTP Triggers, SRE Agent can start the investigation automatically and meet engineers in Jira:

> **Jira ticket created → SRE Agent automatically investigates → Agent writes findings back to Jira**

## Architecture Overview

End-to-end flow:

1. **Jira** — A new issue is created in your project.
2. **Azure Logic App** — Jira connector detects the new issue; the Logic App calls the SRE Agent HTTP Trigger using **Managed Identity**.
3. **HTTP Trigger** — The agent prompt is rendered with Jira ticket details via payload placeholders.
4. **Agent Investigation** — The agent uses Jira MCP tools to read/search Jira, queries Azure logs/metrics/recent deployments, and posts findings back to the Jira ticket as a comment.

## How HTTP Triggers Work

Each HTTP Trigger exposes a unique webhook URL:

```text
https://<your-agent>.<instance>.azuresre.ai/api/v1/httptriggers/trigger/<trigger-id>
```

When an external system sends a POST request with a JSON payload, SRE Agent:

1. Validates the trigger exists and is enabled.
2. Renders the agent prompt by injecting payload values into `{payload.X}` placeholders.
3. Creates a new investigation thread (or reuses an existing one).
4. Executes the agent with the rendered prompt (autonomously or in review mode).
5. Records the execution in the trigger history (auditing/observability).

### Payload placeholders

You can write prompts using `{payload.X}` tokens. Example:

```text
Investigate Jira incident {payload.key}: {payload.summary} (Priority: {payload.priority})
```

If the prompt uses no placeholders, the raw JSON payload is appended automatically so the agent still gets context.

### Thread modes

- **New Thread** (recommended for incidents): each invocation creates a new investigation thread.
- **Same Thread**: all invocations share a thread, building a continuous conversation.

## Authenticating External Platforms

The HTTP Trigger endpoint is secured with **Azure AD authentication** (Microsoft Entra ID). Calls require a bearer token scoped to the SRE Agent **data plane**.

Since platforms like Jira can’t natively acquire Entra ID tokens for this endpoint, the pattern is to put an Azure service with **Managed Identity** in the middle:

| Approach | Best for |
| --- | --- |
| **Azure Logic Apps** | Many native connectors, no code, visual workflow |
| **Azure Functions** | Lightweight relay (~15 lines of code), clean webhook endpoint |
| **API Management (APIM)** | Enterprise controls (rate limiting, IP filtering, API keys) |

This walkthrough uses **Azure Logic Apps** with the Jira connector.

## Step-by-step: Connecting Jira to SRE Agent

### Prerequisites

- Azure SRE Agent resource deployed
- Jira Cloud project with API token access
- Azure subscription for the Logic App

### Step 1: Set up the Jira MCP connector

In the agent’s **MCP Tool settings**, add the Jira connector:

| Setting | Value |
| --- | --- |
| Package | `mcp-atlassian` (npm, version `2.0.0`) |
| Transport | `STDIO` |

Environment variables:

| Variable | Value |
| --- | --- |
| `ATLASSIAN_BASE_URL` | `https://your-site.atlassian.net` |
| `ATLASSIAN_EMAIL` | Your Jira account email |
| `ATLASSIAN_API_TOKEN` | Your Jira API token |

Select which Jira MCP tools the agent can use (key ones for this workflow):

- `jira-mcp_read_jira_issue` — read issue details
- `jira-mcp_search_jira_issues` — search issues using JQL
- `jira-mcp_add_jira_comment` — post findings back to the issue
- `jira-mcp_list_jira_projects` — list projects
- `jira-mcp_create_jira_issue` — create issues

This enables bidirectional Jira interaction, including posting investigation findings back to the original incident.

### Step 2: Create the HTTP Trigger

In SRE Agent UI: **Builder → HTTP Triggers → Create**.

| Setting | Value |
| --- | --- |
| Name | `jira-incident-handler` |
| Agent Mode | Autonomous |
| Thread Mode | New Thread |
| Sub-Agent | Optional specialized incident-response agent |

Agent prompt (example):

```text
A new Jira incident has been filed that requires investigation:

- Jira Ticket: {payload.key}
- Summary: {payload.summary}
- Priority: {payload.priority}
- Reporter: {payload.reporter}
- Description: {payload.description}
- Jira URL: {payload.ticketUrl}

Investigate this incident by:

1. Identifying the affected Azure resources mentioned in the description
2. Querying recent metrics and logs for anomalies
3. Checking for recent deployments or configuration changes
4. Providing a structured analysis with Root Cause, Evidence, and Recommended Actions

Once your investigation is complete, use the Jira MCP tools to post a summary of your findings as a comment on the original ticket ({payload.key}).
```

After saving:

- Enable the trigger.
- Copy the **Trigger URL** (needed for the Logic App).

### Step 3: Create the Azure Logic App

In Azure portal:

| Setting | Value |
| --- | --- |
| Type | Consumption (Multi-tenant, Stateful) |
| Name | `jira-sre-agent-bridge` |
| Region | Same region as SRE Agent (example: East US 2) |
| Resource Group | Same as SRE Agent (recommended) |

### Step 4: Enable Managed Identity

Logic App → **Identity** → **System assigned**:

1. Set Status to **On**
2. Click **Save**

### Step 5: Assign the SRE Agent Admin role

SRE Agent resource → **Access control (IAM)** → **Add role assignment**:

| Setting | Value |
| --- | --- |
| Role | SRE Agent Admin |
| Assign to | Managed Identity → select your Logic App |

> **Important**: The **Contributor** role alone isn’t sufficient. Contributor is control plane; SRE Agent HTTP Triggers use a separate **data plane** with its own RBAC. **SRE Agent Admin** provides the data-plane permissions needed.

### Step 6: Create the Jira connection

In the Logic App designer when adding the Jira trigger:

| Setting | Value |
| --- | --- |
| Connection name | `jira-connection` |
| Jira instance | `https://your-site.atlassian.net` |
| Email | Your Jira email |
| API Token | Your Jira API token |

### Step 7: Configure the Logic App workflow

In Logic App **Code view**, paste the workflow definition (example below). It polls Jira for new issues and then calls the SRE Agent HTTP Trigger.

```json
{
  "definition": {
    "$schema": "https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#",
    "contentVersion": "1.0.0.0",
    "triggers": {
      "When_a_new_issue_is_created_(V2)": {
        "recurrence": {
          "interval": 3,
          "frequency": "Minute"
        },
        "splitOn": "@triggerBody()",
        "type": "ApiConnection",
        "inputs": {
          "host": {
            "connection": {
              "name": "@parameters('$connections')['jira']['connectionId']"
            }
          },
          "method": "get",
          "path": "/v2/new_issue_trigger/search",
          "queries": {
            "X-Request-Jirainstance": "https://YOUR-SITE.atlassian.net",
            "projectKey": "YOUR_PROJECT_ID"
          }
        }
      }
    },
    "actions": {
      "Call_SRE_Agent_HTTP_Trigger": {
        "runAfter": {},
        "type": "Http",
        "inputs": {
          "uri": "https://YOUR-AGENT.azuresre.ai/api/v1/httptriggers/trigger/YOUR-TRIGGER-ID",
          "method": "POST",
          "headers": {
            "Content-Type": "application/json"
          },
          "body": {
            "key": "@{triggerBody()?['key']}",
            "summary": "@{triggerBody()?['fields']?['summary']}",
            "priority": "@{triggerBody()?['fields']?['priority']?['name']}",
            "reporter": "@{triggerBody()?['fields']?['reporter']?['displayName']}",
            "description": "@{triggerBody()?['fields']?['description']}",
            "ticketUrl": "@{concat('https://YOUR-SITE.atlassian.net/browse/', triggerBody()?['key'])}"
          },
          "authentication": {
            "type": "ManagedServiceIdentity",
            "audience": "https://azuresre.dev"
          }
        }
      }
    },
    "outputs": {},
    "parameters": {
      "$connections": {
        "type": "Object",
        "defaultValue": {}
      }
    }
  },
  "parameters": {
    "$connections": {
      "type": "Object",
      "value": {
        "jira": {
          "id": "/subscriptions/YOUR-SUB/providers/Microsoft.Web/locations/YOUR-REGION/managedApis/jira",
          "connectionId": "/subscriptions/YOUR-SUB/resourceGroups/YOUR-RG/providers/Microsoft.Web/connections/jira",
          "connectionName": "jira"
        }
      }
    }
  }
}
```

Replace the `YOUR-*` placeholders.

To find your **Jira project ID**, visit:

```text
https://your-site.atlassian.net/rest/api/3/project/YOUR-PROJECT-KEY
```

…and read the `id` field in the response JSON.

The key piece is Managed Identity authentication:

```json
"authentication": {
  "type": "ManagedServiceIdentity",
  "audience": "https://azuresre.dev"
}
```

After saving, Designer view should show a two-step workflow:

- Jira trigger: “When a new issue is created (V2)”
- HTTP action: “Call SRE Agent HTTP Trigger”

## What happens inside the agent

When the trigger fires, the agent receives a prompt with the Jira incident details injected. Example:

> A new Jira incident has been filed that requires investigation:
> - Jira Ticket: KAN-16
> - Summary: Elevated API Response Times — PostgreSQL Table Lock Causing Request Blocking on Listings Service
> - Priority: High
> - Reporter: Vineela Suri
> - Description: Severity: P2 — High. Affected Service: Production API (octopets-prod-postgres). Impact: End users experience slow or unresponsive listing pages.
> - Jira URL: https://your-site.atlassian.net/browse/KAN-16
>
> Investigate this incident by:
> 1. Identifying the affected Azure resources mentioned in the description
> 2. Querying recent metrics and logs for anomalies
> 3. ...

The agent can then investigate using configured tools (examples mentioned):

- Azure CLI (metrics queries)
- Kusto (log analysis)
- Jira MCP connector (read/search/comment)

Each execution is recorded in trigger history (timestamp, thread ID, status, duration, AI-generated summary).

## Extending to other platforms

The same pattern works for platforms not natively supported by SRE Agent:

> **External Platform → Auth Bridge (Managed Identity) → SRE Agent HTTP Trigger**

Examples of inbound swaps:

| External platform | Auth bridge configuration |
| --- | --- |
| Jira | Logic App with Jira V2 connector (polling) |
| OpsGenie | Logic App connector or Azure Function relay |
| Datadog | Azure Function relay or APIM policy |
| Grafana | Azure Function relay or APIM |
| Splunk | APIM webhook endpoint + Managed Identity forwarding |
| Custom/internal tools | Logic App HTTP trigger, Azure Function relay, or APIM |

## Key takeaways

1. **Connect what you use**: HTTP Triggers integrate external incident platforms with SRE Agent.
2. **Secure by design**: Entra ID (Azure AD) authentication + Managed Identity keeps the data plane protected without storing secrets in Jira.
3. **Bidirectional with MCP**: use inbound HTTP Triggers + outbound MCP connectors to close the loop (post findings back).
4. **Full observability**: every run is tracked in trigger history.
5. **Flexible context injection**: `{payload.X}` placeholders allow structured prompts; raw payload passthrough keeps full context available.

## Learn more

- [HTTP Triggers Documentation](https://sre.azure.com/docs/http-triggers)
- Agent Hooks Blog Post — Governance controls for automated investigations (relative link in source; URL not provided)
- [YAML Schema Reference](https://sre.azure.com/docs/yaml-schema-reference)
- [SRE Agent Getting Started Guide](https://sre.azure.com/docs/getting-started)


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/http-triggers-in-azure-sre-agent-from-jira-ticket-to-automated/ba-p/4504960)

