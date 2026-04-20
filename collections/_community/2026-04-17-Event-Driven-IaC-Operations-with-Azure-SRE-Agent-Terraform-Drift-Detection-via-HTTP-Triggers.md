---
date: 2026-04-17 02:13:18 +00:00
author: Vineela-Suri
section_names:
- azure
- devops
- security
feed_name: Microsoft Tech Community
tags:
- App Service Plan
- Application Insights
- Autonomous Investigation
- Azure
- Azure Activity Log
- Azure App Service
- Azure Logic Apps
- Azure SRE Agent
- Community
- DevOps
- DevSecOps
- Drift Detection
- GitHub
- HTTP Triggers
- IaC
- Incident Correlation
- Managed Identity
- Microsoft Entra ID
- Microsoft Teams
- Pull Requests
- Remediation Recommendations
- Security
- Terraform
- Terraform Cloud
- TLS Configuration
title: 'Event-Driven IaC Operations with Azure SRE Agent: Terraform Drift Detection via HTTP Triggers'
primary_section: azure
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/event-driven-iac-operations-with-azure-sre-agent-terraform-drift/ba-p/4512233
---

Vineela-Suri walks through an event-driven pipeline where Terraform drift alerts trigger Azure SRE Agent via an authenticated HTTP endpoint, so the agent can correlate drift with Azure telemetry, classify severity, recommend safe remediation, notify Microsoft Teams, and even open a GitHub PR.<!--excerpt_end-->

## Overview

Drift detection answers **what changed** (for example, from a scheduled `terraform plan`). This post focuses on what happens *after* drift is found: **who changed it, why it changed, and whether reverting it is safe**.

The core idea is to connect drift detection outputs (Terraform Cloud webhooks, plan summaries) to **Azure SRE Agent HTTP Triggers**, so one webhook can start an autonomous investigation that:

- Classifies drift as **Benign**, **Risky**, or **Critical**
- Correlates drift with **Azure Activity Log** and **Application Insights**
- Produces context-aware remediation guidance (including cases where you should **not** revert)
- Posts a full summary to **Microsoft Teams**
- Updates its own operational “skill” file based on an execution review
- Optionally creates a **GitHub pull request** to fix root cause

## Why “just revert the drift” can take an app down

A common failure mode:

1. A nightly `terraform plan` finds drift
2. A notification lands in Slack/Teams
3. Someone files a ticket
4. An engineer manually investigates across Terraform state, Azure Portal, Activity Log, and Application Insights
5. They find the drift happened during an incident response
6. They run `terraform apply` to revert
7. The app goes down because the drift was actually mitigating an ongoing issue

The post argues drift tooling is good at surfacing diffs, but not at explaining **why** the drift happened or **whether it’s safe** to remediate.

## Architecture: detection to resolution in one webhook

High-level flow:

- **Terraform Cloud** (or another drift detector) sends a webhook when drift is detected.
- An **Azure Logic App** acts as an auth bridge, using **Managed Identity** to acquire an Azure AD token.
- The Logic App forwards the authenticated request to an **Azure SRE Agent HTTP Trigger**.
- The agent runs an investigation workflow and notifies the team.

Related link in the post: https://techcommunity.microsoft.com/blog/appsonazureblog/http-triggers-in-azure-sre-agent-from-jira-ticket-to-automated-investigation/4504960?previewMessage=true

## Setup steps

### Step 1: Deploy demo infrastructure with Terraform (Azure App Service)

The demo uses an Azure App Service running Node.js, deployed via Terraform:

- **App Service Plan**: B1 (Basic), single vCPU, about **$13/mo**
- **App Service**: Node 20 LTS with **TLS 1.2**
- **Tags**: `environment=demo`, `managed_by=terraform`, `project=sre-agent-iac-blog`

Example Terraform snippet from the post:

```hcl
resource "azurerm_service_plan" "demo" {
  name                = "iacdemo-plan"
  resource_group_name = azurerm_resource_group.demo.name
  location            = azurerm_resource_group.demo.location
  os_type             = "Linux"
  sku_name            = "B1"
}
```

A **Logic App** is deployed to handle auth between Terraform Cloud webhooks and the agent endpoint.

### Step 2: Create a drift analysis skill (agent runbook)

The post models “skills” as domain knowledge files that encode the investigation approach. The `terraform-drift-analysis` skill uses this workflow:

1. Identify scope (resource group/resources)
2. Detect drift (Terraform config vs Azure reality)
3. Correlate with incidents (Activity Log + App Insights)
4. Classify severity (Benign/Risky/Critical)
5. Investigate root cause (read source code)
6. Generate drift report (structured summary)
7. Recommend smart remediation (don’t blindly revert)
8. Notify team (Teams)

Key rule encoded:

> NEVER revert critical drift that is actively mitigating an incident.

### Step 3: Create an Azure SRE Agent HTTP Trigger

In the SRE Agent UI, the post creates an HTTP Trigger named `tfc-drift-handler` with a prompt that includes Terraform Cloud payload fields:

```text
A Terraform Cloud run has completed and detected infrastructure drift.

Workspace: {payload.workspace_name}
Organization: {payload.organization_name}
Run ID: {payload.run_id}
Run Message: {payload.run_message}

STEP 1 — DETECT DRIFT...
STEP 2 — CORRELATE WITH INCIDENTS...
STEP 3 — CLASSIFY SEVERITY...
STEP 4 — INVESTIGATE ROOT CAUSE...
STEP 5 — GENERATE DRIFT REPORT...
STEP 6 — RECOMMEND SMART REMEDIATION...
STEP 7 — NOTIFY TEAM...
```

### Step 4: Connect GitHub and Teams

Two connectors are configured so the agent can investigate and notify:

- **GitHub** (read source code during investigation)
- **Microsoft Teams** (post drift/investigation summaries)

## Demo incident: drift caused by incident response

### Act 1: The latency bug

The demo app’s `/api/data` endpoint calls a synchronous function `processLargeDatasetSync()` that repeatedly sorts an array, producing an **O(n² log n)** blocking operation.

On a B1 plan (single vCPU), this blocks the Node.js event loop. Under load:

- Latency spikes to **25–58 seconds**
- Users see **502 Bad Gateway** from the Azure load balancer

### Act 2: On-call changes infrastructure directly

An on-call engineer responds using Azure Portal/CLI instead of Terraform, causing drift:

1. Add diagnostic tags: `manual_update=True`, `changed_by=portal_user` (benign)
2. Downgrade TLS from **1.2 to 1.0** (risky, security regression)
3. Scale App Service Plan from **B1 to S1** (critical, cost from ~$13/mo to ~$73/mo)

The post notes scaling partially mitigates latency (more compute), but doesn’t fix application-level blocking patterns.

### Act 3: Nightly drift detection fires

A scheduled Terraform plan detects the drift and triggers the webhook → Logic App auth bridge → SRE Agent HTTP Trigger.

## What the agent found

### Layer 1: Drift detection + severity

Three drift items (with severity and impact):

- **Critical**: App Service Plan SKU B1 → S1 (**+462%** cost; ~$13/mo → ~$73/mo)
- **Risky**: Minimum TLS version **1.2 → 1.0** (security regression; mentions BEAST/POODLE exposure)
- **Benign**: Extra tags (`changed_by`, `manual_update`) with no functional impact

### Layer 2: Incident correlation (App Insights + Activity Log)

The agent queries Application Insights and correlates drift with a performance incident:

- **97.6% of requests** (40/41) affected
- `GET /api/data` averages **25,919 ms**, P95 **57,697 ms**
- `/api/data` exists in production but **not in the repository source code**, implying code/environment divergence

### Layer 3: Smart remediation recommendations

Instead of “revert everything,” remediation differs by drift item:

1. **Tags (Benign)**: safe to revert anytime (example given: `terraform apply -target`)
2. **TLS 1.0 (Risky)**: **revert immediately** (security issue unrelated to the incident mitigation)
3. **SKU S1 (Critical)**: **do NOT revert** until the performance root cause is fixed

Reasoning: reverting S1 → B1 while blocking code is still deployed would likely turn a partially mitigated incident into an outage.

### Layer 4: Investigation summary

The summary includes:

- **Actor** identified from Activity Log (email shown in the post)
- The App Insights performance incident data
- The code vs deployed state mismatch
- The conclusion that SKU scaling was emergency mitigation, not “unauthorized drift”

### Layer 5: Teams notification

The agent posts the severity-coded drift table, incident context, and recommended action sequence to a Microsoft Teams channel so engineers don’t have to piece it together across dashboards.

## Self-improving behavior (execution review + skill update)

After the run, the agent performs an execution review and identifies gaps in its own `terraform-drift-analysis.md` guidance, then updates the skill file. Gaps called out:

1. Missing incident correlation guidance (App Insights)
2. No code–infrastructure mismatch detection guidance
3. No smart remediation guidance about not reverting incident-mitigating drift
4. Report template missing an incident correlation column
5. No explicit Activity Log integration guidance

The post frames this as a learning loop: each investigation improves the runbook the agent uses next time.

## Agent-created PR

The agent also creates a GitHub PR that includes:

- App safety changes such as `MAX_DELAY_MS` and `SERVER_TIMEOUT_MS` to prevent unbounded latency
- Updates to the drift-analysis skill file based on the execution review

Repository link in the post:

- https://github.com/microsoft/sre-agent/tree/main/samples/terraform-drift-detection

## Key takeaways

- Drift detection output alone (diffs) isn’t enough; you need context and safety assessment.
- “Revert all drift” can cause outages when drift was part of incident mitigation.
- A Logic App + Managed Identity pattern can authenticate webhook events into Azure.
- Azure SRE Agent HTTP Triggers can turn drift events into end-to-end investigations and actions (reporting, remediation guidance, notifications, and PRs).


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/event-driven-iac-operations-with-azure-sre-agent-terraform-drift/ba-p/4512233)

