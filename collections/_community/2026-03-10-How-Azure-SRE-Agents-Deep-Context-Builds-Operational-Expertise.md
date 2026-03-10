---
layout: "post"
title: "How Azure SRE Agent’s Deep Context Builds Operational Expertise"
description: "This article introduces the Deep Context enhancement to Azure SRE Agent, showing how it leverages continuous code access, persistent memory, and background intelligence to deliver expert-level incident response and automation for Azure-based infrastructure. Readers will learn how Deep Context improves root cause analysis, remediation, and institutional knowledge building across SRE and DevOps teams."
author: "dchelupati"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-sre-agent-now-builds-expertise-like-your-best-engineer/ba-p/4500754"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-03-10 14:08:23 +00:00
permalink: "/2026-03-10-How-Azure-SRE-Agents-Deep-Context-Builds-Operational-Expertise.html"
categories: ["AI", "Azure", "DevOps"]
tags: ["AI", "Automation", "Azure", "Azure Monitor", "Azure SRE Agent", "CI/CD", "Cloud Operations", "Community", "Container Apps", "Deep Context", "DevOps", "GitHub", "IaC", "Incident Response", "Kusto Logs", "Operational Intelligence", "PagerDuty", "Persistent Memory", "Root Cause Analysis", "Runbooks", "ServiceNow", "SRE"]
tags_normalized: ["ai", "automation", "azure", "azure monitor", "azure sre agent", "cislashcd", "cloud operations", "community", "container apps", "deep context", "devops", "github", "iac", "incident response", "kusto logs", "operational intelligence", "pagerduty", "persistent memory", "root cause analysis", "runbooks", "servicenow", "sre"]
---

dchelupati explores how Deep Context transforms the Azure SRE Agent, enabling it to learn and act like your best SRE by leveraging continuous code access, persistent memory, and automation for incident response.<!--excerpt_end-->

# How Azure SRE Agent’s Deep Context Builds Operational Expertise

**Author: dchelupati**

## Overview

Azure SRE Agent, enhanced with the new Deep Context feature, is designed to accelerate the journey from incident detection to resolution by mimicking the expertise-building process of experienced site reliability engineers (SREs). Rather than simply querying information on demand, the agent continuously absorbs codebases, operational data, logs, and past incident knowledge, evolving its intelligence with every interaction.

## Core Capabilities of Deep Context

### 1. Continuous Access

- The agent maintains ongoing access to source code, infrastructure configs, the Python runtime, and Azure environment details.
- Connected repositories are cloned automatically, giving the agent early visibility into code structure and configuration.

### 2. Persistent Memory

- Knowledge from previous incidents, architecture reviews, and team conversations is retained across sessions.
- The agent leverages this memory to shortcut investigations and builds up institutional knowledge that persists even if team members change.

### 3. Background Intelligence

- Even outside of active use, the agent scans logs and code, discovers new data tables via Kusto, and documents schemas.
- It generates reusable query templates and aggregates insights across all investigations, enabling proactive pattern recognition and operational guidance.

## Real-World Impact: From Alert to Remediation

- When an incident (e.g., HTTP 5xx spike on a container app) triggers, the agent is already familiar with the environment, recent code changes, and past alerts.
- It immediately navigates to relevant code, correlates configuration and deployment changes, and recognizes incident patterns from persistent memory.
- In autonomous mode, the agent can propose and implement fixes: edit code, restart containers, create branches and pull requests, open GitHub Issues, and verify service recovery—all with minimal human intervention.

## Collaboration and Knowledge Capture

- Teams can use chat commands (e.g., `#remember`) to preserve critical facts and operational preferences.
- After an investigation, the agent can turn remediation steps into runbooks, indexing them for future reference.
- Continuous learning is guided by active feedback—Microsoft’s SREs routinely engage with the agent to reinforce learning or adjust persistent memory.

## Best Practices for Maximum Value

- Granting the agent full access to infrastructure-as-code (IaC), deployment scripts, Helm charts, pipeline definitions, and runbooks dramatically improves its effectiveness.
- Security is maintained via sandboxed code access; the agent works with repository copies and only proposes changes through standard review and CI/CD gates.

## Compound Benefits

- Deep Context supercharges incident management, scheduled tasks, compliance checks, and drift detection.
- Integration with Azure Monitor, Kusto, PagerDuty, ServiceNow, and MCP connectors (like Datadog, Splunk) provides unified operational insight.
- Uploaded architecture documents and postmortems further enrich contextual understanding, turning logs into action and code into solutions.

## Getting Started

- Deep Context is available for all Azure SRE Agents as of the latest general availability (GA) release.
- Reference links:
  - [SRE Agent GA Announcement](https://aka.ms/sreagent/ga)
  - [What's New in SRE Agent GA](https://aka.ms/sreagent/blog/whatsnewGA)
  - [SRE Agent Documentation](https://aka.ms/sreagent/newdocs)
  - [Onboarding Guide](https://aka.ms/sreagent/blogs/onboardingtosrea)

## Conclusion

Deep Context enables SRE teams to capture, retain, and share operational expertise across the organization. By fusing continuous learning with secure automation, Azure SRE Agent shifts from reactive troubleshooting to proactive, expert-driven operations.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-sre-agent-now-builds-expertise-like-your-best-engineer/ba-p/4500754)
