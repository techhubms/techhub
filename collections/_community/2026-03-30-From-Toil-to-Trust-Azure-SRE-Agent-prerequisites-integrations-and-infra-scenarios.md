---
tags:
- AI
- Application Insights
- Automation
- Azure
- Azure CLI
- Azure Data Explorer
- Azure DevOps Connector
- Azure Monitor
- Azure RBAC
- Azure Resource Manager
- Azure SRE Agent
- Community
- DevOps
- Firewall Allow List
- GitHub Connector
- Governance
- Grafana
- Incident Response
- KQL
- Least Privilege
- Log Analytics
- Managed Identity
- MCP
- MCP Connectors
- Network Egress
- PagerDuty
- Preview
- Reliability Engineering
- REST API
- Root Cause Analysis
- Runbooks
- Security
- ServiceNow
- SRE
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/from-toil-to-trust-how-azure-sre-agent-is-redefining-cloud/ba-p/4505875
section_names:
- ai
- azure
- devops
- security
date: 2026-03-30 07:59:46 +00:00
primary_section: ai
title: 'From Toil to Trust: Azure SRE Agent prerequisites, integrations, and infra scenarios'
feed_name: Microsoft Tech Community
author: siddhigupta
---

siddhigupta walks infrastructure and SRE teams through Azure SRE Agent in preview—what it is, the incident/RCA and remediation scenarios it targets, and the practical onboarding checklist (regions, RBAC/managed identity, monitoring, networking, connectors, and governance) needed to use it safely in Azure.<!--excerpt_end-->

## Overview

As Azure environments scale, outages often come from complex dependencies across compute, networking, storage, and platform services—not a single misconfigured VM. This post explains how **Azure SRE Agent** aims to help infrastructure and SRE teams operate reliably at scale with **AI-assisted diagnostics and controlled remediation**.

Azure SRE Agent documentation: [Azure SRE Agent](https://learn.microsoft.com/en-us/azure/sre-agent/)

> Azure SRE Agent is currently in **preview**. Features, capabilities, and [regional availability](https://learn.microsoft.com/en-us/azure/sre-agent/supported-regions) may change before GA.

## What Azure SRE Agent is (infrastructure lens)

Azure SRE Agent is an **AI-powered reliability assistant** integrated into Azure. It continuously observes telemetry from:

- Azure Monitor
- Log Analytics
- Service APIs

It helps engineers **diagnose, investigate, and remediate** production issues.

From an infra standpoint, the agent is described as understanding:

- Azure resource topology and dependencies
- Common failure patterns across Azure services
- Safe operational actions using **Azure CLI** and **REST APIs**

It can:

- **Recommend actions**, or
- **Execute remediation steps** with guardrails/approvals

### Automation mechanisms

- **Built-in Azure knowledge**: Preconfigured operational patterns for Azure services
- **Custom runbooks**: Execute Azure CLI commands and REST API calls
- **Subagent extensibility**: Specialized agents for areas like VMs, databases, networking
- **External integrations**: Monitoring, incident management, and source control systems

## Infrastructure scenarios where it helps

### 1. Incident investigation & production outages

Instead of pivoting manually between alerts, metrics, logs, and dashboards, the agent can correlate telemetry and summarize issues in natural language.

Typical infrastructure issues mentioned:

- VMs becoming unresponsive
- App Service or Container App failures
- Network connectivity or NSG misconfigurations
- Storage throttling or capacity exhaustion

### 2. Log/metric-driven root cause analysis

Because it consumes Azure Monitor + Log Analytics data directly, it can do context-aware RCA without requiring engineers to write KQL for common scenarios.

Example question from the post:

> “Why did my App Service start returning HTTP 500 errors after the last deployment?”

The described approach is to correlate deployment activity, configuration changes, and telemetry to identify the likely root cause.

### 3. Safe, controlled remediation

Two modes are described:

- **Review mode**: actions proposed and require explicit approval
- **Autonomous mode**: pre-approved subagents execute actions automatically

Examples of repeatable infra tasks:

- Restarting unhealthy services
- Scaling compute resources
- Rolling back failed deployments
- Correcting known configuration drift

### 4. Guardrails & operational hygiene

Continuous evaluation of infra posture and operational risks, such as:

- Detecting insecure network exposure
- Flagging unsupported SKUs/configurations
- Identifying operational anti-patterns

### 5. Extending automation with subagents

Subagents documentation: [subagents](https://learn.microsoft.com/en-us/azure/sre-agent/sub-agents)

Using the Subagent Builder, teams can:

- Attach custom runbooks
- Integrate external observability tools
- Trigger actions on alerts or schedules

## Getting started: prerequisites and ownership model

Onboarding spans infrastructure, platform, security, and network teams. The post explicitly tags ownership.

### 1. Subscription & region

**Owner:** Platform / Subscription Admin

- Use a dedicated subscription or resource group for evaluation/PoC
- During preview, the **agent control plane** must be created in one of:
  - Sweden Central
  - Australia East
  - US East 2
- Monitored workloads can be in any Azure region
- Subscription may need to be **allow-listed** for preview access

### 2. Identity & access (critical)

**Owner:** Platform + Security; **Consumer:** Infra / SRE

- Ability to create **managed identities** (system- or user-assigned)
- Elevated permissions needed during onboarding, including:
  - `Microsoft.Authorization/roleAssignments/write` at subscription scope
  - Roles such as **Owner**, **User Access Administrator**, or **RBAC Administrator**
- After onboarding: grant **least-privilege RBAC** to the agent identity:
  - Read-only for investigation
  - Scoped write access only for approved remediation

### 3. Resource provider registration

**Owner:** Platform

- Required Azure resource providers must be registered (one-time task)

### 4. Monitoring & telemetry baseline (hard dependency)

**Owner:** Infra / Platform (shared)

- Azure Monitor enabled for target workloads
- Diagnostic settings send logs/metrics to:
  - Log Analytics
  - Application Insights (where applicable)
- During agent creation, supporting resources may be deployed:
  - Log Analytics workspace
  - Application Insights
  - Smart detector alert rules

### 5. Network & connectivity

**Owner:** Network / Security

- Outbound HTTPS connectivity to Azure management endpoints (ARM, Monitor, etc.)
- Outbound to external systems when integrations are enabled (e.g., ServiceNow or MCP servers)
- Custom MCP endpoints must be **remote and HTTPS-reachable** (local endpoints not supported)
- IP allow-listing must be validated; static egress IPs are not guaranteed

Network controls reference: [network controls](https://learn.microsoft.com/en-us/azure/sre-agent/network-requirements)

### 6. Connector-specific prerequisites (conditional)

**Owner:** Security + Platform; **Consumer:** Infra / SRE

- Microsoft Teams / Outlook connectors:
  - OAuth consent for Microsoft 365 APIs
  - User-assigned managed identity required for connector auth
- Custom MCP connectors:
  - MCP base URL
  - Auth material (API key, token, or OAuth)
  - RBAC permissions to configure/manage connectors

### 7. Automation readiness

**Owner:** Infra / SRE + Security

- Decide recommendation-only vs automated remediation
- Define an approval model:
  - Human-in-the-loop
  - Scoped autonomy for well-understood actions
- Grant write permissions only where automation is explicitly approved

### 8. Governance & data handling

**Owner:** Security / Governance

- Prompts, responses, and analysis data are stored in the agent’s region
- Align with policies for:
  - Logging and retention
  - Auditability
  - Responsible AI usage and approvals

> The post highlights that, because Azure SRE Agent is layered on Azure Resource Manager, Azure Monitor, Log Analytics, and managed identities, identity/telemetry/governance foundations need to be in place before infra teams get full value.

## Supported integrations

The post lists integrations across:

- **Monitoring/observability**: Azure Monitor, Application Insights, Log Analytics, Grafana
- **Incident management**: Azure Monitor Alerts, PagerDuty, ServiceNow
- **Source control/CI/CD**: GitHub, Azure DevOps
- **Data sources**: Azure Data Explorer (Kusto) clusters, **Model Context Protocol (MCP) servers**

## Connectivity matrix (summary)

### Azure control plane connectivity

Outbound-only from the agent to:

- **Azure Resource Manager (ARM)** over HTTPS/443 using Managed Identity (OAuth 2.0)
- **Azure Monitor** over HTTPS/443
- **Log Analytics Workspace** over HTTPS/443 (KQL queries for RCA)
- **Application Insights** over HTTPS/443

No inbound connectivity to customer VNets is required.

### Incident management integrations

- Azure Monitor Alerts: inbound to agent (native)
- ServiceNow and PagerDuty: outbound & inbound via connector (webhook/API), using OAuth/API token

### Collaboration/notifications

- Microsoft Teams: outbound HTTPS/443, OAuth via user-assigned managed identity
- Outlook (Email): outbound HTTPS/443, OAuth via user-assigned managed identity

Note: only user-assigned managed identities are supported for Office 365 connectors.

### External/custom integrations

- Custom MCP servers: outbound HTTPS/443, OAuth or API key
- Python execution tool: outbound HTTPS/443

### Firewall/network considerations

- Allow-list `*.azuresre.ai`
- Allow outbound HTTPS (443) to:
  - Azure control-plane endpoints
  - `*.azuresre.ai`
  - Configured third-party endpoints (ServiceNow, PagerDuty, MCP servers)
- No inbound firewall rules or private endpoint exposure required

## How to access Azure SRE Agent

- Via the Azure portal
- Via the SRE portal: [https://sre.azure.com/](https://sre.azure.com/)

Screenshot link included in the post (portal view):

- [Create and set up](https://learn.microsoft.com/en-us/azure/sre-agent/create-and-set-up)

## Further reading (links from the post)

1. Usage: https://learn.microsoft.com/en-us/azure/sre-agent/usage
2. Automate workflows: https://learn.microsoft.com/en-us/azure/sre-agent/automate-workflows
3. Diagnose Azure observability: https://learn.microsoft.com/en-us/azure/sre-agent/diagnose-azure-observability
4. Connectors: https://learn.microsoft.com/en-us/azure/sre-agent/connectors
5. First investigation: https://learn.microsoft.com/en-us/azure/sre-agent/first-investigation
6. Pricing/billing: https://learn.microsoft.com/en-us/azure/sre-agent/pricing-billing
7. Manage permissions: https://learn.microsoft.com/en-us/azure/sre-agent/manage-permissions
8. MCP connectors: https://learn.microsoft.com/en-us/azure/sre-agent/mcp-connectors
9. Anthropic as a sub-processor: https://learn.microsoft.com/en-us/azure/sre-agent/anthropic-sub-processor

## Why it matters (as framed in the post)

The post argues Azure SRE Agent can:

- Reduce MTTR via Azure Monitor + Log Analytics correlation
- Reduce context switching (“dashboard hopping”)
- Provide explainable deep investigation and RCA
- Lower operational toil by handling repetitive triage
- Improve incident response traceability with ServiceNow/PagerDuty integration
- Enable scheduled proactive checks (health validations, drift checks, post-deploy verifications)
- Encode domain expertise via skills/subagents/workflows
- Correlate infra issues with code and delivery systems via GitHub/Azure DevOps connectors
- Keep automation governed with human-in-the-loop controls

## Closing thoughts

Azure SRE Agent is positioned as a shift from manual diagnostics to **intent-driven, agent-assisted operations** for infrastructure teams managing large Azure estates—provided identity, telemetry, network connectivity, and governance foundations are in place.

[Read the entire article](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/from-toil-to-trust-how-azure-sre-agent-is-redefining-cloud/ba-p/4505875)

