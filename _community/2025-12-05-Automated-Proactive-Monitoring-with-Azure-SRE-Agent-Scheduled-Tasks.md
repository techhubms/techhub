---
layout: "post"
title: "Automated Proactive Monitoring with Azure SRE Agent Scheduled Tasks"
description: "This community article explains how SRE teams can shift to proactive monitoring using Azure SRE Agent's scheduled tasks. It covers how natural language prompts and built-in automation allow incident detection, compliance checks, and health monitoring without code, as well as integrating with tools like Azure CLI and Log Analytics for notifications and escalations."
author: "dchelupati"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/proactive-monitoring-made-simple-with-azure-sre-agent/ba-p/4471205"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-12-05 06:00:35 +00:00
permalink: "/2025-12-05-Automated-Proactive-Monitoring-with-Azure-SRE-Agent-Scheduled-Tasks.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["Application Insights", "Azure", "Azure CLI", "Azure SRE Agent", "Best Practices", "Cloud Operations", "Community", "Compliance", "DevOps", "DevOps Automation", "ICM", "Incident Response", "Kubernetes", "Log Analytics", "MCP Integration", "Microsoft Azure", "Proactive Monitoring", "Scheduled Tasks", "Security", "Security Posture"]
tags_normalized: ["application insights", "azure", "azure cli", "azure sre agent", "best practices", "cloud operations", "community", "compliance", "devops", "devops automation", "icm", "incident response", "kubernetes", "log analytics", "mcp integration", "microsoft azure", "proactive monitoring", "scheduled tasks", "security", "security posture"]
---

dchelupati details how Azure SRE Agent's scheduled tasks enable SRE teams to automate proactive monitoring, incident response, and compliance checks using natural language prompts and Azure's automation ecosystem.<!--excerpt_end-->

# Automated Proactive Monitoring with Azure SRE Agent Scheduled Tasks

SRE teams often struggle to stay ahead of incidents and compliance drift in cloud environments. Azure SRE Agent offers a solution: Scheduled Tasks, which let you describe monitoring and automation intents in natural language and automate them without pipelines or code.

## Key Features

- **No scripting required:** Express monitoring needs with plain language prompts (e.g., “Scan all resources for security best practices”).
- **Service integration:** Leverages Azure CLI, Log Analytics Workspace, AppInsights, and can extend to third-party data via MCP server integration.
- **Automated results analysis:** Produces actionable summaries, not raw alerts.
- **Flexible notification:** Sends updates to Teams channels, creates incidents, or emails on findings.

## How Scheduled Tasks Work

### 1. Prompt Interpretation

Your prompt is interpreted by SRE Agent to create a plan (e.g., scanning for public endpoints across subscriptions).

### 2. Execution Using Built-in Tools

The agent uses Azure-native tools and connects to external systems if needed.

### 3. Result Summarization

After running the check, findings are summarized with context (e.g., listing non-compliant storage accounts).

### 4. Notification & Escalation

Notifications can be sent to Teams, via email, or incidents can be updated automatically.

## Typical Use Cases

| Use Case                  | Prompt Example                                                                      | Schedule     |
|--------------------------|------------------------------------------------------------------------------------|--------------|
| Security Posture Check   | “Scan all subscriptions for resources with public endpoints and flag any that shouldn’t be exposed” | Daily        |
| Cost Anomaly Detection   | “Alert if any service exceeds 20% growth this week over last”                       | Weekly       |
| Compliance Drift         | “Report storage accounts lacking proper encryption”                                 | Daily        |
| Health Summaries         | “Summarize health for all production VMs, highlight degraded ones”                  | 4 hours      |
| Incident Trend Analysis  | “Analyze ICM incidents for weekly patterns”                                         | Weekly       |

## Set Up in Three Steps

1. **Define Intent:** Write a clear prompt specifying what resources, conditions, and actions (e.g., “Check for pods in CrashLoopBackOff, post summary to #sre-alerts”).
2. **Set Schedule:** Use cron expressions or simple intervals.
3. **Choose Notifications:** Decide where to send results—Teams, email, or automatic incident creation.

## Benefits vs Traditional Monitoring

- **Plain language over scripting:** Lower entry barrier.
- **Contextual, AI-driven analysis:** Smarter results, reduce alert fatigue.
- **Unified actions:** Detection and response within one workflow.
- **Accessible for all SREs:** No DevOps pipeline expertise required.

## Best Practices

- **Start with high-value checks, iterate.**
- **Be specific in prompts.**
- **Match schedule to risk.**
- **Review and refine based on results.**

## Example Health Check Sub-Agent

- **Sample:** [Scheduled Health Check Sample](https://github.com/microsoft/sre-agent/blob/main/samples/automation/samples/02-scheduled-health-check-sample.md)
- Uses built-in tools (Azure CLI, Log Analytics Workspace).
- Schedules daily container app health checks at 7 AM.
- Sends email alerts on anomalies.

## Additional Resources

- [More Samples](https://github.com/microsoft/sre-agent/tree/main/samples)
- [Ignite 2025 Announcements](https://aka.ms/ignite25/blog/sreagent)
- [Documentation](https://aka.ms/sreagent/docs)
- [Support & Feature Requests](https://github.com/microsoft/sre-agent/issues)

Azure SRE Agent's scheduled tasks help SRE teams automate checks and stay proactive, reducing the need for hand-built infrastructure while boosting reliability and compliance.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/proactive-monitoring-made-simple-with-azure-sre-agent/ba-p/4471205)
