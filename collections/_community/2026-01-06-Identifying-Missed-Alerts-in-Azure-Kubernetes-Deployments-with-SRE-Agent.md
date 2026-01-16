---
layout: "post"
title: "Identifying Missed Alerts in Azure Kubernetes Deployments with SRE Agent"
description: "This post shares a practical developer’s experience of discovering alerting blind spots in an Azure Kubernetes Service (AKS) setup with Azure Monitor, Kubernetes, and Azure Cache for Redis. It demonstrates how Azure SRE Agent and GitHub integration can surface undetected failure modes, automate diagnostics, and provide concrete remediation for monitoring gaps."
author: "dchelupati"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/find-the-alerts-you-didn-t-know-you-were-missing-with-azure-sre/ba-p/4483494"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-01-06 17:39:38 +00:00
permalink: "/2026-01-06-Identifying-Missed-Alerts-in-Azure-Kubernetes-Deployments-with-SRE-Agent.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["AKS", "Alerting", "Azure", "Azure Cache For Redis", "Azure DevOps", "Azure Monitor", "Azure SRE Agent", "Bicep", "Cloud Operations", "Community", "Container Orchestration", "Credential Rotation", "DevOps", "GitHub MCP", "Incident Response", "KQL", "Kubernetes", "Liveness Probe", "Log Analytics", "Monitoring", "Security", "Subagent", "Synthetic Tests"]
tags_normalized: ["aks", "alerting", "azure", "azure cache for redis", "azure devops", "azure monitor", "azure sre agent", "bicep", "cloud operations", "community", "container orchestration", "credential rotation", "devops", "github mcp", "incident response", "kql", "kubernetes", "liveness probe", "log analytics", "monitoring", "security", "subagent", "synthetic tests"]
---

dchelupati describes how gaps in Azure Monitor alert coverage became apparent during a Redis credential rotation in an AKS application, and how Azure SRE Agent and GitHub MCP integration accelerated incident diagnosis and alert remediation.<!--excerpt_end-->

# Identifying Missed Alerts in Azure Kubernetes Deployments with SRE Agent

Author: dchelupati  
Published: January 6, 2026  

## Introduction

As cloud-native development grows, reliable alerting is crucial for uptime and customer trust. In this post, dchelupati walks through a real-world issue in an Azure Kubernetes Service (AKS) environment using Azure Monitor and Azure Cache for Redis, highlighting how seemingly “complete” alerting can still leave critical gaps.

## The Realization: When Alerts Don’t Fire

Despite configuring six alerts (CPU, memory, pod restarts, container errors, OOMKilled, job failures), a production incident slipped by unnoticed after a Redis password rotation caused failed connections and read/write timeouts, but didn’t trigger the existing rules. This common DevOps scenario proved that alerts which look thorough in theory may not catch real-world failures.

## Demo Application Context

- Web API hosted on AKS
- Azure Cache for Redis for data storage
- Azure Monitor for alerting on core resource health
- GitHub Copilot Agent mode and Claude Opus 4.5 as developer tools

## The Failure Mode: Redis Credential Rotation

- Security teams rotated Redis passwords (a standard compliance action)
- App pods failed to connect after restart, failing readiness probes
- LoadBalancer had no healthy endpoints, leading to timeouts
- No Azure Monitor alerts triggered—none addressed this class of failure

## Using Azure SRE Agent to Surface Alerting Gaps

Instead of manual alert reviews, Azure SRE Agent was used to:

1. Analyze why no alert triggered when the endpoint timed out
2. Enumerate active alert rules versus actual failure types
3. Identify alert rule blind spots

### Key Blind Spots Detected

- No synthetic URL availability tests
- No readiness/liveness probe or "pods not ready" alerts for application namespace
- No explicit detection for Redis connectivity issues
- No alerts for 5xx/timeout spikes at ingress
- Per-pod issues not evident at the node level

SRE Agent used GitHub MCP to review code, create diagnostic KQL/Bicep proposals, and open a GitHub issue recommending remediation in specific repo files. [See issue](https://github.com/dm-chelupati/aksjournalapp/issues/3).

## How Was SRE Agent Set Up?

1. **Agent Creation**: Added from the Azure portal, granted Reader permissions at the subscription level for broad diagnostics
2. **GitHub Connection via MCP**: Provided SRE Agent access to source control for code context and tracking
3. **Alert Management Subagent**: Dedicated subagent with explicit Azure Monitor authority and GitHub integration
4. **Diagnostic Workflow**: Prompted SRE Agent with endpoint context, received summarized alert coverage analysis and actionable remediation

### Tools Leveraged

- Azure CLI for resources and alert rules
- Log Analytics for query-based diagnostics
- GitHub MCP (Model Context Protocol) for repo/file access and automated issue creation

## Key Takeaways

- **Alerting assumptions are often incomplete**. Real failures, like credential changes or probe rejections, need explicit alert coverage.
- **Automation bridges the gap**—rely on tools like SRE Agent to systematically catch what’s missing.
- **Prevention beats detection**: Proactive analysis prevents user-impacting outages.

## Tips for Effective Alert Coverage

- Grant broad enough agent permissions to survey all resources
- Create focused, job-specific subagents
- Test third-party integrations before running live workflows

## Discussion

What alert scenarios have surprised you, and what measures have you adopted to close those gaps? Credential rotations, DNS losses, certificate expirations—all are prime candidates for expanded alerting. Share your experiences below.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/find-the-alerts-you-didn-t-know-you-were-missing-with-azure-sre/ba-p/4483494)
