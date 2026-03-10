---
layout: "post"
title: "What's New in Azure SRE Agent: GA Release Highlights"
description: "This update details the general availability of Azure SRE Agent, focusing on new onboarding, deep environment integration, and automated investigation capabilities for Microsoft Azure operations teams. Learn about key features like workflow automation, extensibility, and integration with incident management platforms."
author: "dchelupati"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/what-s-new-in-azure-sre-agent-in-the-ga-release/ba-p/4500779"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-03-10 15:03:50 +00:00
permalink: "/2026-03-10-Whats-New-in-Azure-SRE-Agent-GA-Release-Highlights.html"
categories: ["Azure", "DevOps"]
tags: ["Azure", "Azure DevOps", "Azure Monitoring", "Azure SRE Agent", "Community", "Continuous Learning", "DevOps", "ICM Integration", "Incident Management", "MCP Connectors", "MTTR Reduction", "Operational Automation", "PagerDuty", "Plugin Marketplace", "Python Integration", "Root Cause Analysis", "ServiceNow", "Site Reliability Engineering"]
tags_normalized: ["azure", "azure devops", "azure monitoring", "azure sre agent", "community", "continuous learning", "devops", "icm integration", "incident management", "mcp connectors", "mttr reduction", "operational automation", "pagerduty", "plugin marketplace", "python integration", "root cause analysis", "servicenow", "site reliability engineering"]
---

dchelupati shares an in-depth overview of Azure SRE Agent's general availability, covering new operational, onboarding, and automation features for SRE and DevOps practitioners using Microsoft Azure.<!--excerpt_end-->

# What's New in Azure SRE Agent: GA Release Highlights

**Author:** dchelupati

Azure SRE Agent is now generally available ([GA announcement](https://aka.ms/sreagent/ga)). After significant preview use by Microsoft teams and early customers, the GA release brings several new capabilities shaped by real-world operational needs.

## Key Features in the GA Release

- **Redesigned onboarding:** A guided setup allows teams to start benefiting from SRE Agent on day one. Connect code, logs, incidents, Azure resources, and knowledge files through a unified workflow.
  - [Learn more about onboarding](https://aka.ms/sreagent/blogs/onboardingtosrea)

- **Deep context and persistent memory:** The agent continuously explores your environment, accessing logs, code, and configuration data. It tracks operational history to surface actionable insights—even without explicit prompts. This continuous learning helps the agent build expertise about routes, handlers, and deployments.
  - [Read about continuous learning](https://aka.ms/sreagent/blogs/deepcontextblog)

- **Automated investigation:** The agent proactively and reactively investigates environment issues. Integrate with ICM, PagerDuty, and ServiceNow for automated incident response and triage.

- **Root cause analysis:** By connecting errors to the relevant code and learning from past incidents, the agent reduces mean time to recovery (MTTR) for teams.

- **End-to-end workflow automation:** Via MCP connectors and HTTP integrations, orchestrate Azure, monitoring, and service workflows from a single control point. Extend capabilities using custom Python scripts or by adding skills and plugins from the Plugin Marketplace.

## Getting Started and Resources

- [Create your agent](https://aka.ms/sreagent-portal)
- [Documentation](https://aka.ms/sreagent/newdocs)
- [Quick start guide](https://aka.ms/sreagent/newdocs/get-started)
- [Pricing](https://azure.microsoft.com/en-us/pricing/details/sre-agent/)
- [Sample repository](https://github.com/microsoft/sre-agent/tree/main/samples)
- [Feedback & issues](https://github.com/microsoft/sre-agent/issues)
- [Videos](https://www.youtube.com/@AzureSREAgent)

## Why Use Azure SRE Agent?

- **Automate investigations and root cause analysis** for faster incident response.
- **Orchestrate complex workflows** spanning Azure, monitoring, and ticketing tools.
- **Customize with skills and plugins** for domain-specific operations.
- **Single pane of glass** for operational oversight and knowledge reuse.

Try Azure SRE Agent today to streamline site reliability operations in your Azure environment. More features are expected soon.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/what-s-new-in-azure-sre-agent-in-the-ga-release/ba-p/4500779)
