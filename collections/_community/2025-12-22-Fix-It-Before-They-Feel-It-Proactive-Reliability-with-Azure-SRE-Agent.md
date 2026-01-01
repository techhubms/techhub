---
layout: "post"
title: "Fix It Before They Feel It: Proactive Reliability with Azure SRE Agent"
description: "This guide presents a technical deep dive into using Azure SRE Agent for autonomous performance issue detection and remediation on production infrastructure. It covers agent architecture, deployment steps, real-world demo, and full setup using Azure services, .NET 9 Web APIs, and GitHub/Teams integration, enabling developers and SREs to implement proactive incident response."
author: "saziz_msft"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/fix-it-before-they-feel-it-higher-reliability-with-proactive/ba-p/4480444"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-12-22 23:54:37 +00:00
permalink: "/2025-12-22-Fix-It-Before-They-Feel-It-Proactive-Reliability-with-Azure-SRE-Agent.html"
categories: ["AI", "Azure", "Coding", "DevOps"]
tags: ["AI", "Application Insights", "ASP.NET Core 9.0", "Autonomous Remediation", "Azure", "Azure App Service", "Azure Bicep", "Azure Monitor", "Azure SRE Agent", "Coding", "Community", "Deployment Health", "DevOps", "DevOps Automation", "GitHub Integration", "Incident Detection", "Knowledge Store", "Microsoft Teams", "MTTD", "MTTR", "Performance Monitoring", "PowerShell", "Slot Swap"]
tags_normalized: ["ai", "application insights", "aspdotnet core 9dot0", "autonomous remediation", "azure", "azure app service", "azure bicep", "azure monitor", "azure sre agent", "coding", "community", "deployment health", "devops", "devops automation", "github integration", "incident detection", "knowledge store", "microsoft teams", "mttd", "mttr", "performance monitoring", "powershell", "slot swap"]
---

saziz_msft introduces a hands-on guide to building self-healing production infrastructure with Azure SRE Agent, focusing on autonomous detection, remediation, and reporting of performance issues.<!--excerpt_end-->

# Fix It Before They Feel It: Proactive Reliability with Azure SRE Agent

## Overview

Discover how to achieve higher reliability and autonomous remediation for your cloud solutions using the Azure SRE Agent. This agent leverages AI techniques to detect, diagnose, and fix production issues with no human intervention, ensuring minimal user impact and fast recovery.

## Demo and Key Features

Presented at **.NET Day 2025**, the blog features a live demo where intentionally degraded code is deployed in production. Azure SRE Agent detects the anomaly, rolls back to a healthy state, communicates the incident, and generates insightful reports—all automatically.

### Capabilities

- **Proactive Baseline Learning**: Learns application response norms and stores historical metrics
- **Real-time Anomaly Detection**: Monitors production for deviations from baselines
- **Autonomous Remediation**: Performs Azure CLI operations (slot swaps) upon alert triggers
- **Cross-platform Notification**: Updates Microsoft Teams and GitHub Issues during incidents
- **Incident Reporting**: Generates daily summaries of incident response and resolution metrics

### Demo Architecture

- **Application Layer**:
  - .NET 9 Web API hosted on Azure App Service
  - Application Insights for monitoring
  - Azure Monitor Alerts for trigger events
- **Azure SRE Agent**:
  - 3 sub-agents (AvgResponseTime, DeploymentHealthCheck, DeploymentReporter)
  - Knowledge Store for metric baselines
- **External Integrations**:
  - GitHub for code tracking and issue automation
  - Microsoft Teams for real-time comms
  - Outlook for report delivery

### Walkthrough: Demo Flow

1. **Deploy infrastructure and applications** using provided PowerShell scripts
2. **Configure sub-agents** in Azure SRE Agents Portal (set up triggers, schedules)
3. **Run the demo**:
   - Simulate bad code in production
   - Agent evaluates response times via Application Insights
   - If >20% degradation, triggers health check, Azure slot swap to rollback
   - Posts incident updates to Teams, creates GitHub issue, sends email summary

[Detailed setup and demo instructions](https://github.com/microsoft/sre-agent/tree/main/samples/proactive-reliability#demo-flow)

### Setting Up

**Prerequisites:**

- Azure subscription with Contributor access
- Azure CLI (az login)
- .NET 9.0 SDK
- PowerShell 7.0+

**Key Scripts:**

- `1-setup-demo.ps1` — Deploys base infrastructure, App Service, Insights, baseline app versions
- `2-run-demo.ps1` — Runs the anomaly + remediation demo

**Performance Toggle Example (ProductsController.cs):**

```csharp
private const bool EnableSlowEndpoints = false; // false = fast, true = slow
```

- Production: `EnableSlowEndpoints = false` (~50ms response)
- Staging: `EnableSlowEndpoints = true` (~1500ms response)

### Technology Stack

- .NET 9.0 Web API
- Azure App Service
- Application Insights and Log Analytics
- Azure DevOps/Bicep Infrastructure
- Azure SRE Agent + PowerShell

### Resources

- [Source Code & Setup](https://github.com/microsoft/sre-agent/samples/proactive-reliability)
- [Azure SRE Agent Documentation](https://learn.microsoft.com/en-us/azure/sre-agent/)

## Conclusion

Implementing autonomous, proactive remediation with Azure SRE Agent reduces MTTD and MTTR, ensuring high availability and robust user experience.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/fix-it-before-they-feel-it-higher-reliability-with-proactive/ba-p/4480444)
