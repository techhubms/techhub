---
feed_name: Microsoft Tech Community
section_names:
- azure
- devops
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-monitor-in-azure-sre-agent-autonomous-alert-investigation/ba-p/4509069
date: 2026-04-07 22:20:53 +00:00
tags:
- AKS
- Alert Deduplication
- Alert Merging
- Alert Rules
- Alerts Management REST API
- Auto Resolve
- Azure
- Azure Cache For Redis
- Azure Monitor
- Azure SRE Agent
- Community
- CrashLoopBackOff
- DevOps
- Incident Management
- Incident Response Plans
- Kubernetes Secrets
- Log Analytics
- Log Search Alerts
- Managed Identity
- Metric Alerts
- Monitoring Reader
- Observability
- Pod Rollout
- Prometheus Alerts
- Runbooks
- Service Health
- Smart Detection
- SRE
author: Vineela-Suri
title: 'Azure Monitor in Azure SRE Agent: Autonomous Alert Investigation and Intelligent Merging'
primary_section: azure
---

Vineela-Suri walks through how Azure SRE Agent integrates with Azure Monitor to pick up fired alerts, investigate them autonomously, and merge repeated firings into a single incident thread, using an AKS + Azure Cache for Redis failure scenario and practical guidance on when to enable or disable auto-resolve.<!--excerpt_end-->

## Overview

Azure Monitor can tell you when something is wrong, but triaging alerts and digging through logs takes time—and duplicate firings add noise while someone is already investigating. This post explains how **Azure SRE Agent** integrates with **Azure Monitor** to:

- Pick up alerts as they fire
- Investigate autonomously
- Remediate when it can
- **Merge repeat firings** of the same alert rule into a single investigation thread

The demo scenario uses **AKS + Azure Cache for Redis** and highlights how the **auto-resolve** setting on Azure Monitor alert rules changes the agent’s merge behavior.

## Key takeaways

1. **Use Incident Response Plans to scope which alerts the agent handles**
   - Filter by severity, title patterns, and resource type
   - Start in **review mode**, then promote to **autonomous** once you trust the agent for that failure pattern
2. **Recurring alerts merge into one thread automatically**
   - Repeated firings of the same alert rule get merged into the existing investigation
3. **Tune auto-resolve based on failure type**
   - **Auto-resolve OFF** for persistent failures (bad credentials, misconfigurations, resource exhaustion)
   - **Auto-resolve ON** for transient issues (traffic spikes, brief timeouts)
4. **Design alert rules around failure categories, not components**
   - One alert rule maps to one investigation thread
   - Structure by symptom (Redis errors, HTTP errors, pod health) to avoid overlap
5. **Attach custom response plans for specialized handling**
   - Route specific patterns to custom agents with tailored instructions, tools, and runbooks

## It starts with any Azure Monitor alert

Azure SRE Agent watches for alerts by querying the **Azure Alerts Management REST API**:

- [Azure Alerts Management REST API – Get all alerts](https://learn.microsoft.com/rest/api/monitor/alertsmanagement/alerts/get-all)

Because everything comes through the same API, the agent can process all alert signal types consistently:

- Log search alerts
- Metric alerts
- Activity log alerts
- Smart detection
- Service health
- Prometheus

No per-alert-type connectors/webhooks are required. The key configuration is **which alerts the agent should care about**, handled through **Incident Response Plans**.

## Setting up: Incident Response Plans and alert rules

In the SRE Agent UI:

- Go to **Settings > Incident Platform > Azure Monitor**
- Create an **Incident Response Plan**
- Scope by:
  - Severity
  - Alert title/name patterns
  - Target resource types
  - Agent mode (**autonomous** vs **review**)

> Action: Match agent mode to confidence in the remediation, not just severity.
> - Use **autonomous** for predictable/safe remediations (e.g., rollback config, restart pod).
> - Use **review** when you want human validation—especially Sev0/Sev1 impacting critical systems.

On the Azure Monitor side (demo setup):

- Three **log-based alert rules** against an AKS cluster’s **Log Analytics workspace**
- Main rule: Redis connection error query looking for signatures like:
  - `WRONGPASS`
  - `ECONNREFUSED`
  - Other Redis failure patterns in `ContainerLog`

Alert evaluation configuration:

- Evaluates every **5 minutes**
- Uses a **15-minute aggregation window**
- If the query returns results, the alert fires

## Breaking Redis (on purpose)

Demo app:

- Node.js journal app on **AKS**
- Backed by **Azure Cache for Redis**

Failure injection:

- Update the Redis password in a Kubernetes secret to an incorrect value
- Pods pick up the bad credential
- Redis connections fail
- Errors flow into logs
- The Redis connection error alert fires

## What happened next (agent behavior)

- The agent scanner polls the Azure Monitor Alerts API every **60 seconds**
- When it sees a new alert (state: "New", condition: "Fired"):
  - Matches it against the Sev1 Incident Response Plan
  - **Acknowledges** the alert in Azure Monitor (state becomes "Acknowledged")
  - Creates an investigation thread containing:
    - Alert ID
    - Rule name
    - Severity
    - Description
    - Affected resource
    - Subscription and resource group
    - Azure Portal deep link

Autonomous remediation performed in the demo:

- Query container logs
- Identify Redis `WRONGPASS` errors
- Trace to the bad Kubernetes secret
- Retrieve the correct access key from Azure Cache for Redis
- Update the Kubernetes secret
- Trigger a pod rollout
- Mark the thread as "Completed"

## Alert merging: reducing duplicate investigations

Because the alert evaluates every 5 minutes, it can fire multiple times while remediation is underway (seven firings over 35 minutes in the demo).

**Alert merging** prevents duplicates:

- When a subsequent firing arrives for the same alert rule, the agent checks:
  - Is there already an active thread for this rule created within the last 7 days?
  - Is it still not resolved/closed?
- If yes, the new firing is **silently merged**:
  - Alert count increments
  - "Last fired" timestamp updates
  - No new thread and no extra notification

### Merge decision logic

| Condition | Result |
| --- | --- |
| Same alert rule, existing thread still active | **Merged** — alert count increments, no new thread |
| Same alert rule, existing thread resolved/closed | **New thread** — fresh investigation starts |
| Different alert rule | **New thread** — always separate |

## The auto-resolve twist

Azure Monitor has an alert rule setting: **"Automatically resolve alerts"**.

- When enabled, Azure Monitor transitions an alert to **"Resolved"** once the underlying condition clears.

### Scenario A: Auto-resolve OFF

- Alert stays in "Fired" across evaluation cycles
- Repeat firings merge cleanly
- Result in the demo: **one thread, seven merged alerts**

### Scenario B: Auto-resolve ON

When repeated break/fix cycles occur:

1. Redis breaks → alert fires → agent creates a thread
2. Agent fixes the issue
3. Condition clears and alerts get closed/resolved
4. Redis breaks again → merge check finds no active thread → **a new thread is created**

For persistent misconfiguration-style issues, this results in more threads and more noise.

## Should you turn auto-resolve off?

It depends on whether a repeat firing represents the **same ongoing problem** or a **new occurrence**.

### Quick reference: auto-resolve decision guide

| | Auto-resolve OFF | Auto-resolve ON |
| --- | --- | --- |
| Use when | Problem persists until fixed | Problem is transient and self-correcting |
| Examples | Bad credentials, misconfigurations, CrashLoopBackOff, connection pool exhaustion, IOPS limits | OOM kills during traffic spikes, brief latency from neighboring deployments, one-off job timeouts |
| Merge behavior | All repeat firings merge into one thread | Each break-fix cycle creates a new thread |
| Best for | Agent manages alert lifecycle continuously | Each occurrence may have a different root cause |
| Tradeoff | Alerts stay "Fired/Acknowledged" until explicitly closed | More threads, but each has a clean investigation |

> Note: These behaviors describe how SRE Agent groups alert investigations and may differ from how Azure Monitor documents native alert state behavior.

## Best practices checklist

## Alert rule design

| Do | Don't |
| --- | --- |
| Design rules around **failure categories** (root cause, blast radius, infra health) | Create one alert per component (overlapping threads) |
| Set **evaluation frequency** and **aggregation window** to match the failure pattern | Use one cadence for everything |

Example structure used in the test:

- Root cause signal — Redis `WRONGPASS`/`ECONNREFUSED` → **Sev1**
- Blast radius signal — HTTP 5xx response codes → **Sev2**
- Infrastructure signal — `KubeEvents` `Reason="Unhealthy"` → **Sev2**

## Incident Response Plan setup

| Do | Don't |
| --- | --- |
| Create separate response plans per severity tier | Use one catch-all filter for everything |
| Start with **review mode** (especially Sev0/Sev1) | Jump straight to autonomous mode on critical alerts |
| Promote to **autonomous** once validated for a failure pattern | Assume severity alone determines the right mode |

## Response plans

| Do | Don't |
| --- | --- |
| Attach **custom response plans** to specific patterns | Leave everything to the agent’s general knowledge |
| Include relevant tools/runbooks/instructions | Write generic instructions |
| Route Redis alerts to Redis-focused agents; K8s alerts to kubectl-focused agents | Assume one agent fits all |

## Getting started

1. Open your agent at [sre.azure.com](https://sre.azure.com)
2. Ensure the agent’s managed identity has **Monitoring Reader** on target subscriptions
3. Go to **Settings > Incident Platform > Azure Monitor** and create Incident Response Plans
4. Review each alert rule’s **auto-resolve** setting
   - OFF for persistent issues
   - ON for transient issues
5. Start with a test response plan using **Title Contains** to target a single alert rule
6. Watch the **Incidents** page and review investigation threads before expanding scope

## Learn more

- [Azure SRE Agent Documentation](https://sre.azure.com/docs)
- [Incident Response Guide](https://sre.azure.com/docs/incident-response)
- [Azure Monitor Alert Rules overview](https://learn.microsoft.com/azure/azure-monitor/alerts/alerts-overview)


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-monitor-in-azure-sre-agent-autonomous-alert-investigation/ba-p/4509069)

