---
layout: "post"
title: "Automating On-Call Runbooks with Azure SRE Agent for Incident Response"
description: "This detailed community post by dchelupati explains how to leverage Azure SRE Agent and AI-driven runbook automation to streamline and enhance incident response for DevOps engineers and SREs. Learn how to set up an automated workflow, from creating the agent to integrating it with your incident management tools, and experience reduced MTTR, consistent execution, and better evidence collection for postmortems."
author: "dchelupati"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/stop-running-runbooks-at-3-am-let-azure-sre-agent-do-your-on/ba-p/4479811"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-12-20 15:52:24 +00:00
permalink: "/community/2025-12-20-Automating-On-Call-Runbooks-with-Azure-SRE-Agent-for-Incident-Response.html"
categories: ["AI", "Azure", "DevOps"]
tags: ["AI", "Application Insights", "Automated Diagnostics", "Az CLI", "Azure", "Azure App Service", "Azure Container Apps", "Azure Monitor", "Azure SRE Agent", "Cloud Operations", "Community", "DevOps", "Email Integration", "Incident Response", "KQL", "Log Analytics", "PagerDuty", "Runbook Automation", "Runbooks", "ServiceNow", "SRE"]
tags_normalized: ["ai", "application insights", "automated diagnostics", "az cli", "azure", "azure app service", "azure container apps", "azure monitor", "azure sre agent", "cloud operations", "community", "devops", "email integration", "incident response", "kql", "log analytics", "pagerduty", "runbook automation", "runbooks", "servicenow", "sre"]
---

In this post, dchelupati details how Azure SRE Agent can automate the tedious on-call runbook execution process for DevOps and SRE teams, enabling faster, more reliable incident diagnostics.<!--excerpt_end-->

# Automating On-Call Runbooks with Azure SRE Agent for Incident Response

**Author:** dchelupati

When production throws 500 errors at 3am, the repetitive, manual runbook tasks involved in incident response can be error-prone and exhausting. dchelupati shares a solution using **Azure SRE Agent** to automate these diagnostics, reducing time to discovery and letting engineers focus on decisive action.

## The Problem with Manual Runbooks

- Incident response often involves following documented steps: querying metrics, logs, requests, and errors.
- This manual process is tedious, repetitive, and prone to human error, especially during late-night on-call emergencies.

## Azure SRE Agent + Runbook Automation Overview

- The Azure SRE Agent can read markdown runbooks, execute diagnostic steps (such as `az monitor metrics`, Log Analytics and App Insights queries), and compile a summary email for responders.
- Reduces terminal context switching and the chance of missing key troubleshooting steps.

### Runbook Components Used

- **az monitor metrics** — Checking resource health and usage
- **Log Analytics queries** — Investigating error and exception patterns
- **App Insights** — Reviewing failed requests, stack traces, and correlation IDs
- **az containerapp logs** — Accessing app revision logs and configuration
- All steps are written in standard markdown, including CLI and KQL queries.

## Automation Process Steps

1. **Create SRE Agent:** Use the [Azure portal](https://aka.ms/sreagent/portal) to spin up an agent (no resource group needed for most scenarios).
2. **Assign Roles:** (Optional) Provide Reader role for az command access if runbooks target Azure resources.
3. **Load Runbooks:** Add markdown runbook files to the agent's knowledge base.
4. **Connect Communication:** Integrate with Outlook to get emailed findings.
5. **Configure Subagent:** Create a subagent with instructions to find and execute appropriate runbooks, collect evidence, and send summaries.
6. **Set Up Incident Trigger:** Connect incident management tools like PagerDuty, ServiceNow, or Azure Monitor alerts to trigger the workflow.

## Flexibility and Platform Agnosticism

- Works with on-prem, hybrid, or other cloud environments - simply include the right diagnostic steps in your runbook.
- The agent executes whatever is in your markdown file, regardless of platform.

## Benefits Highlighted

- **Reduction in MTTR:** Get concise analysis before even starting your investigation.
- **Consistent Execution:** Automated process means no missed or forgotten steps.
- **Documented Evidence:** All queries and results are preserved for future postmortems.
- **Empowered Decision-Making:** Spend your mental energy on solutions, not data collection.

## Conclusion

Automating runbook execution with Azure SRE Agent transforms incident management, making it faster and reducing on-call stress for engineers. If you maintain runbooks for diagnostics, consider connecting them to an SRE Agent and letting automation handle your next 3am alert.

**Try it:** Convert your runbooks to markdown, add them to SRE Agent, and let automation do the grunt work.

---

**Published:** Dec 20, 2025  
**Author:** dchelupati

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/stop-running-runbooks-at-3-am-let-azure-sre-agent-do-your-on/ba-p/4479811)
