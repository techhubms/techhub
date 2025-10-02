---
layout: "post"
title: "Azure SRE Agent Public Preview Expands: New Features for DevOps and Automation"
description: "This post details the public preview expansion of the Azure SRE Agent, an automation tool for site reliability engineering within Azure environments. It covers improvements around governance, security, diagnostics, extensibility, new incident management integrations, automation with runbooks, and the capability to bridge DevOps workflows across Azure, GitHub, and Azure DevOps."
author: "Mayunk_Jain"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/expanding-the-public-preview-of-the-azure-sre-agent/ba-p/4458514"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-10-02 02:24:40 +00:00
permalink: "/2025-10-02-Azure-SRE-Agent-Public-Preview-Expands-New-Features-for-DevOps-and-Automation.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["AKS", "AZ CLI", "Azure", "Azure App Service", "Azure DevOps", "Azure Functions", "Azure Monitor", "Azure Portal", "Azure SRE Agent", "Community", "DevOps", "DevOps Automation", "Diagnostics", "Enterprise Governance", "GitHub Integration", "Incident Management", "Kubectl", "PagerDuty", "RBAC", "Root Cause Analysis", "Runbooks", "Security", "ServiceNow", "Site Reliability Engineering"]
tags_normalized: ["aks", "az cli", "azure", "azure app service", "azure devops", "azure functions", "azure monitor", "azure portal", "azure sre agent", "community", "devops", "devops automation", "diagnostics", "enterprise governance", "github integration", "incident management", "kubectl", "pagerduty", "rbac", "root cause analysis", "runbooks", "security", "servicenow", "site reliability engineering"]
---

Mayunk_Jain introduces the expanded public preview for Azure SRE Agent, focusing on automation, governance, diagnostics, and incident management for DevOps and SRE teams operating in Azure.<!--excerpt_end-->

# Azure SRE Agent Public Preview Expands: New Features for DevOps and Automation

**Author:** Mayunk_Jain

The Azure SRE Agent is now in public preview, instantly available to all users without the need for sign-up. This release incorporates feedback from early adopters and delivers a suite of new capabilities for DevOps, site reliability engineering (SRE), and enterprise teams managing services on Azure.

## What's New in Azure SRE Agent

### Secure-by-Default Governance

- Operates with least-privilege access by default
- Never executes write actions on Azure resources without explicit human approval
- Leverages role-based access control (RBAC) for read-only or approver roles
- Provides oversight, traceability, and flexibility—from insights to full automation

### Deep Diagnostics and Extensible Automation

- Supports all Azure services via AZ CLI and kubectl
- Enhanced diagnostics for PostgreSQL, API Management, Azure Functions, AKS, Azure Container Apps, and Azure App Service
- Consistent automation and insights for diverse cloud environments, from monoliths to microservices

### Automated Incident Management

- Native integrations with Azure Monitor, PagerDuty, and ServiceNow
- Ingests alerts and triggers workflows compatible with existing DevOps toolchains
- Enables streamlined incident detection and automated or human-in-the-loop response

### Root Cause Analysis and Developer Integration

- Integrates with GitHub and Azure DevOps for code-aware root cause analysis (RCA)
- Traces incidents directly to source code and recent changes
- Accelerates resolution by connecting operational data to engineering workflows

### Closing the DevOps Loop

- Automatically generates incident summary reports in GitHub and Azure DevOps, complete with diagnostic context
- Optionally assigns incidents to GitHub Copilot coding agents for automated pull requests and merge workflows, contributing to permanent code fixes

### Getting Started

- [Create a new SRE Agent in the Azure portal](https://aka.ms/sreagent-portal) *(Azure login required)*
- Learn more: [SRE Agent documentation](https://aka.ms/sreagent/docs)
- Billing and reporting: Managed via Azure Agent Units (AAUs) with [predictable billing](https://aka.ms/sreagent/pricing/blog)
- Community: Collaborate or request features through the [official GitHub repo](https://github.com/microsoft/sre-agent)

## Key Features by Area

- **Governance & Security:** Least-privilege operations, explicit approval for write actions, RBAC-enabled roles.
- **Automation:** Pluggable into existing Azure, GitHub, ServiceNow, and PagerDuty workflows.
- **Diagnostics:** Advanced support for cloud-native and database platforms (AKS, App Service, PostgreSQL, etc.).
- **DevOps Integration:** Seamless feedback loops between incident management and code repositories; enables continuous improvement.
- **Extensibility:** Reuse of existing runbooks and customizable workflows.

## Community & Support

- For support, feature requests, or feedback, engage through the [GitHub repository](https://github.com/microsoft/sre-agent).

## Additional Resources

- [Official product home page](http://www.azure.com/sreagent)
- [Enterprise-ready and extensible – Update on the Azure SRE Agent preview](https://techcommunity.microsoft.com/blog/appsonazureblog/enterprise-ready-and-extensible-update-on-the-azure-sre-agent-preview/4444299)

*Updated October 2, 2025 — Version 2.0*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/expanding-the-public-preview-of-the-azure-sre-agent/ba-p/4458514)
