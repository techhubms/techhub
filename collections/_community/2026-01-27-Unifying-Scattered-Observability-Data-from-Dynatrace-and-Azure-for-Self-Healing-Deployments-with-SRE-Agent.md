---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/unifying-scattered-observability-data-from-dynatrace-azure-for/ba-p/4489547
title: Unifying Scattered Observability Data from Dynatrace and Azure for Self-Healing Deployments with SRE Agent
author: Vineela-Suri
feed_name: Microsoft Tech Community
date: 2026-01-27 05:34:14 +00:00
tags:
- Azure Container Apps
- Azure Monitor
- Azure SRE Agent
- Cloud Native
- Deployment Automation
- DevOps Workflow
- DQL
- Dynatrace
- Log Analysis
- MCP
- Observability
- Remediation
- Rollback
- Scheduled Tasks
- Self Healing
- Subagents
- Azure
- DevOps
- Community
section_names:
- azure
- devops
primary_section: azure
---
Vineela-Suri shows how to build a self-healing deployment pipeline on Azure by integrating Dynatrace observability with Azure SRE Agent. The walkthrough demonstrates step-by-step configuration of connectors, subagents, and scheduled tasks for automatic log analysis and rollout/rollback in live cloud environments.<!--excerpt_end-->

# Unifying Scattered Observability Data from Dynatrace and Azure for Self-Healing Deployments with SRE Agent

## The Deployment Remediation Challenge

Modern operations teams often face delays identifying and remediating faulty deployments due to disconnected data across observability and deployment platforms. Error logs in Dynatrace, deployment history in Azure Container Apps, and resource metrics in Azure Monitor all exist separately, requiring manual intervention for rollback and remediation.

## Solution Overview: SRE Agent, MCP, and Subagents

**Azure SRE Agent**—with support for the Model Context Protocol (MCP)—bridges these silos. By connecting external observability tools like Dynatrace via MCP and building specialized subagents for log analysis and remediation, teams can automate fault detection and recovery workflows for Azure Container Apps.

### Core Components Used

- **Dynatrace MCP Connector:** Enables Azure SRE Agent to query Dynatrace APIs via MCP.
- **Log Analysis Subagent (DynatraceSubagent):** Runs DQL queries, analyzes error spikes, and identifies root causes.
- **Remediation Subagent:** Correlates log data with Azure deployments, generates visual evidence, and autonomously rolls back failed deployments.
- **Scheduled Tasks:** Weekly, automated checks trigger the workflow for continuous reliability.

## Implementation Guide

### 1. Connect Dynatrace via MCP

Configure SRE Agent to connect to Dynatrace using the Model Context Protocol. Dynatrace MCP exposes an API gateway for direct integration.

```json
{
  "name": "dynatrace-mcp-connector",
  "dataConnectorType": "Mcp",
  "dataSource": "Endpoint=https://<your-tenant>.live.dynatrace.com/platform-reserved/mcp-gateway/v0.1/servers/dynatrace-mcp/mcp;AuthType=BearerToken;BearerToken=<your-api-token>"
}
```

💡 **Tip:** Dynatrace API tokens should have `entities.read`, `events.read`, and `metrics.read` scopes.

### 2. Build Specialized Subagents

#### DynatraceSubagent

- Executes DQL queries (`create-dql`, `execute-dql`, `explain-dql`) to analyze logs and detect error spikes.
- Fetches 5xx error counts, evaluates trends, and finds root causes of incidents.
- Example configuration: [DynatraceLogAnalysisSubagent.yaml](https://github.com/microsoft/sre-agent/blob/main/samples/proactive-reliability/SubAgents/DynatraceLogAnalysisSubagent.yaml)

#### RemediationSubagent

- Retrieves Azure Container Apps revision history (`GetDeploymentTimes`, `ListRevisions`).
- Generates correlation charts of errors vs. deployments (`PlotTimeSeriesData`, etc.).
- Computes causation confidence and executes rollbacks when above threshold.
- Example configuration: [DeploymentRemediationSubagent.yaml](https://github.com/microsoft/sre-agent/blob/main/samples/proactive-reliability/SubAgents/DeploymentRemediationSubagent.yaml)

### 3. Orchestrate with Scheduled Tasks

- Configure a scheduled task (e.g., every Monday at 9:30 AM) to initiate the workflow automatically.
- Task triggers RemediationSubagent, which delegates log analysis to DynatraceSubagent and acts on findings.

**Example Settings:**

| Setting | Value            |
|---------|------------------|
| Task Name | OctopetsScheduledTask |
| Frequency | Weekly         |
| Day       | Monday         |
| Time      | 9:30 AM        |
| Response Subagent | RemediationSubagent |

### 4. Workflow Execution

1. Scheduled task triggers.
2. RemediationSubagent pulls recent Azure deployment history.
3. Delegates log query/analysis to DynatraceSubagent.
4. DynatraceSubagent diagnoses error spike, returns analysis.
5. RemediationSubagent correlates errors, determines confidence, and—if threshold met—executes autonomous rollback.
6. Traffic is routed to last known good revision within minutes.

### Why It Matters

This automation reduces manual toil, speeds incident response from >75 minutes to under 5 minutes, and coordinates disparate data for reliable self-healing deployments.

| Before                         | After                             |
|--------------------------------|-----------------------------------|
| Manual log checks              | Automated DQL queries via MCP      |
| Manual data correlation        | Subagent-driven analysis           |
| Manual rollback                | Confidence-based auto-remediation  |
| Slow response, human in loop   | Proactive checks and automation    |

### Try It Yourself: Getting Started Checklist

1. Connect your observability tool (Dynatrace, Datadog, Prometheus) via MCP.
2. Create a log analysis subagent for error pattern detection.
3. Build a remediation subagent for deployment correlation and rollback.
4. Set up scheduled tasks to automate workflow runs.
5. Use subagent handoffs to delegate specialized tasks.

### Further Reading

- [Azure SRE Agent Documentation](https://learn.microsoft.com/en-us/azure/sre-agent/)
- [Model Context Protocol (MCP) Guide](https://learn.microsoft.com/en-us/azure/sre-agent/custom-mcp-server)
- [Subagent Builder](https://learn.microsoft.com/en-us/azure/sre-agent/subagent-builder-overview)
- [Scheduled Tasks](https://learn.microsoft.com/en-us/azure/sre-agent/scheduled-tasks?tabs=health-check)
- [Azure SRE Agent Blogs](https://techcommunity.microsoft.com/tag/azure%20sre%20agent?nodeId=board%3AAppsonAzureBlog)

---

**Author:** Vineela-Suri

*Published on Apps on Azure Blog; member since November 2024.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/unifying-scattered-observability-data-from-dynatrace-azure-for/ba-p/4489547)
