---
layout: "post"
title: "Proactive Cloud Ops with SRE Agent: Scheduled Checks for Azure Optimization"
description: "This article by dchelupati details a hands-on approach for continuously optimizing Azure cloud environments using the Azure SRE Agent. It covers integrating SRE Agent with Azure services, automating cost and security checks via GitHub and Teams, and importing custom organizational standards for proactive cloud governance."
author: "dchelupati"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/proactive-cloud-ops-with-sre-agent-scheduled-checks-for-cloud/ba-p/4487261"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-01-19 22:26:20 +00:00
permalink: "/2026-01-19-Proactive-Cloud-Ops-with-SRE-Agent-Scheduled-Checks-for-Azure-Optimization.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["Automation", "Azure", "Azure Monitor", "Azure SRE Agent", "Cloud Governance", "Cloud Optimization", "Community", "Continuous Compliance", "Cost Management", "DevOps", "GitHub Integration", "Key Vault", "Microsoft Teams", "Observability", "Org Practices", "Resource Groups", "Resource Management", "Scheduled Triggers", "Security", "Security Best Practices"]
tags_normalized: ["automation", "azure", "azure monitor", "azure sre agent", "cloud governance", "cloud optimization", "community", "continuous compliance", "cost management", "devops", "github integration", "key vault", "microsoft teams", "observability", "org practices", "resource groups", "resource management", "scheduled triggers", "security", "security best practices"]
---

dchelupati walks through setting up the Azure SRE Agent for proactive cloud operations, demonstrating how to automate cloud optimization across cost, security, and performance using continuous checks and integrations with GitHub and Teams.<!--excerpt_end-->

# Proactive Cloud Ops with SRE Agent: Scheduled Checks for Azure Optimization

Cloud operations aren't just about keeping things running—they're about making them run better. As Azure environments evolve, staying ahead of resource sprawl, creeping costs, and configuration drift is essential.

## The Cloud Optimization Challenge

Azure environments constantly change:

- New features and services roll out frequently
- Traffic and load patterns shift seasonally
- Costs increase if not monitored
- Security best practices evolve
- Teams may spin up resources and overlook cleanup

You might log into the Azure portal and see that everything is "fine." But is it truly optimized?

> **The question isn't "is something broken?"—it's "could this be better?"**

## Four Pillars of Cloud Optimization

| Pillar        | What Teams Want                   | The Challenge                                                    |
|--------------|-----------------------------------|------------------------------------------------------------------|
| **Security** | Stay compliant, reduce risk       | Configuration drift, legacy settings, expiring credentials       |
| **Cost**     | Spend efficiently, justify budget | Spotting waste among hundreds of resources is difficult          |
| **Performance**| Meet SLOs, handle growth         | Knowing when to scale before demand spikes                       |
| **Availability**| Maximize uptime                 | Hidden dependencies, single points of failure                    |

Many teams check these only occasionally. SRE Agent checks them continuously.

## Introducing SRE Agent + Scheduled Tasks

Azure SRE Agent pulls data from Azure Monitor, configurations, metrics, logs, cost data, and more. It can aggregate insights and run scheduled analyses. External tools such as Datadog, PagerDuty, and Splunk can also be integrated using MCP servers, extending its observability.

For this setup, the focus is on Azure-native data sources.

## How I Set It Up: Step by Step

### 1. Create SRE Agent with Subscription Access

Gave the agent Reader access at the subscription level—enabling holistic visibility across all resource groups, from VMs and Key Vaults to NSGs and Web Apps. No need to configure individual resource groups.

### 2. Upload Organization Practices

Defined "what good looks like" in an org-practices.md file and uploaded this to the SRE Agent's knowledge base. This customizes checks to match the team's specific requirements—not just generic Azure defaults.

👉 [See the full org-practices.md](https://github.com/dm-chelupati/costoptimizationapp/blob/main/knowledge/org-practices.md)

**Demo source repos:**

- [security-demoapp](https://github.com/dm-chelupati/security-demoapp) — with intentional security misconfigurations
- [costoptimizationapp](https://github.com/dm-chelupati/costoptimizationapp/tree/main) — with cost optimization scenarios

### 3. Connect to Microsoft Teams Channel

Integrated SRE Agent with Teams, delivering critical findings as instant notifications and daily digests for warnings. Teams stay informed without leaving their workspace.

### 4. Link Resource Groups to GitHub Repos

Each resource group connects to its respective GitHub repo for traceability:

| Resource Group           | GitHub Repository          |
|-------------------------|---------------------------|
| rg-security-opt-demo    | security-demoapp          |
| rg-cost-opt-sreademo    | costoptimizationapp       |

Findings trigger GitHub Issues, linking violations directly to the source code/infrastructure.

### 5. Test with Prompts

Manually triggered checks (e.g., "scan resource group for violations against org-practices.md") verified that the agent flagged issues correctly. These were surfaced in Teams and created linked issues in GitHub.

**Examples:**

- [Security findings issue](https://github.com/dm-chelupati/security-demoapp/issues/1)
- [Cost findings issue](https://github.com/dm-chelupati/costoptimizationapp/issues/2)

### 6. Set Up Scheduled Triggers

Recurring schedules were configured:

- **Weekly Security Check:** Wednesdays at 8 AM UTC
- **Weekly Cost Review:** Mondays at 8 AM UTC

Admins receive regular optimization reports in both Teams and GitHub. Automation ensures no findings slip through the cracks.

## Why Context Matters

While SRE Agent understands Azure and general best practices out-of-the-box, true value comes from context:

- Your SLOs and acceptable thresholds
- How/when secrets should be rotated
- Which resources are production-critical
- Your tagging and deployment policies

Uploading org-specific standards allows the agent to validate against actual operational requirements, not just generic guidance.

**Enterprise knowledge can be imported from sources like Confluence or SharePoint via MCP servers, so teams leverage existing documentation.**

## The Payoff: Continuous Optimization

What used to be a periodic, reactive task becomes a continuous practice:

- Daily security posture checks
- Weekly cost savings and performance reviews
- Early warnings for expiring credentials or scaling needs

Optimization shifts from a quarterly scramble to a steady, automated improvement cycle.

## Try It Yourself

1. Deploy SRE Agent and grant it subscription access.
2. Upload your policies, standards, and thresholds.
3. Schedule a daily or weekly check (start with security or cost).
4. Watch the results show up in Teams and GitHub.

## Learn More

- [Azure SRE Agent documentation](https://aka.ms/sreagent/docs)
- [Azure SRE Agent blogs](http://aka.ms/sreagent/blogs)
- [Azure SRE Agent community](https://aka.ms/sreagent/discussions)
- [Azure SRE Agent home page](http://www.azure.com/sreagent)
- [Azure SRE Agent pricing](http://aka.ms/sreagent/pricing)

*Azure SRE Agent is currently in preview.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/proactive-cloud-ops-with-sre-agent-scheduled-checks-for-cloud/ba-p/4487261)
