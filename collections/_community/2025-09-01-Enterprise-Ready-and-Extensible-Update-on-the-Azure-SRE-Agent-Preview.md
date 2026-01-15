---
layout: post
title: 'Enterprise-Ready and Extensible: Update on the Azure SRE Agent Preview'
author: Mayunk_Jain
canonical_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/enterprise-ready-and-extensible-update-on-the-azure-sre-agent/ba-p/4444299
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-09-01 19:17:04 +00:00
permalink: /ai/community/Enterprise-Ready-and-Extensible-Update-on-the-Azure-SRE-Agent-Preview
tags:
- AAU
- AI
- AI Automation
- App Service
- Azure
- Azure CLI
- Azure DevOps
- Azure Functions
- Azure Monitor
- Azure SRE Agent
- Cloud Governance
- Community
- DevOps
- Enterprise Integration
- GitHub Integration
- Incident Management
- Incident Response
- Kubernetes
- Operational Excellence
- PagerDuty
- Root Cause Analysis
- Security
- ServiceNow
- Source Code Analysis
section_names:
- ai
- azure
- devops
- security
---
Mayunk Jain introduces updates to the Azure SRE Agent, an enterprise-grade AI assistant for automated incident response on Azure, highlighting new integrations and enhanced operational features.<!--excerpt_end-->

# Enterprise-Ready and Extensible: Update on the Azure SRE Agent Preview

_Author: Mayunk Jain_

## Overview

The Azure SRE Agent is an AI-assisted automation solution designed to streamline incident response, diagnostics, and mitigation across Azure services. Since its preview at Microsoft Build, the agent has rapidly evolved, gaining advanced incident management features, deeper service integrations, and capabilities that help organizations improve uptime, reduce operational toil, and safely automate remediation in cloud environments.

## Key Highlights

- **AI-Powered Automation**: The Agent automates detection, diagnostics, mitigation, and handoffs to development teams, reducing manual triage.
- **Secure by Design**: Operates with read-only permissions; escalates write actions for explicit user approval, ensuring safe and auditable workflows.
- **Expansive Service Support**: Now integrates with Azure CLI, kubectl, psql, PostgreSQL, Azure API Management, Functions, App Service, AKS, ACA, and more—expanding diagnostics and operational coverage.
- **Incident Management Integrations**: Connects natively with Azure Monitor, PagerDuty, ServiceNow for incident intake, updates, and synchronization. Alerts are captured and handled in near-real time.
- **DevOps Loop Closure**: Incident reports can now be created as Azure DevOps work items or GitHub issues, directly triggering follow-up automation (e.g., PR creation, merge after validation).
- **Source Code Aware RCA**: The agent’s root cause analysis traces incidents back to specific files and methods in source repositories, drawing connections through Azure DevOps or GitHub integrations for pinpoint accuracy.
- **Customizable Incident Handling**: Through user-supplied Runbooks and machine learning from past incidents, the agent supports both fully automated and human-in-the-loop workflows.

## What's New Since Build

### 1. Granular Permissions with Governance

Configure the agent with strict read-only access; write operations require explicit user approval. This provides security and operational governance by default.

### 2. Expanded Service Skills

- **CLI Tooling**: Azure CLI, kubectl, and psql support for subscriptions, Kubernetes clusters, databases.
- **Resource Coverage**: Support for PostgreSQL diagnostics, API Management policy inspection, and deeper insights for Azure Functions, App Service, AKS, ACA.
- **Global Reasoning**: Azure CLI support enables generic assistance for most Azure services.

### 3. Enhanced Incident Management

- Native integration with Azure Monitor alerts, PagerDuty, and ServiceNow for automatic incident capture and lifecycle management.

### 4. Improved DevOps Workflows

- Automatically generates actionable reports and assignments in Azure DevOps or GitHub to close the loop between detection and code remediation.

### 5. Extensible Handling

- Learns from previous incidents and applies user-defined instructions via Runbooks, promoting consistent outcomes across organizations.

### 6. Root Cause Analysis with Source Code Context

- Correlates incident data (logs, metrics, exceptions) with relevant source code, marking responsible files and methods for rapid remediation.

## Billing and Operational Model

- **Billing Starts September 1, 2025**: Dual flow model (always-on monitoring and incident mitigation tasks) using AAU (Azure Agent Units) for predictable costs. Pricing and estimation tools are available via the Azure pricing calculator.

## Benefits for Teams

- **Reduced Toil**: Less repetitive manual work.
- **Improved Uptime and MTTR**: Faster detection and mitigation.
- **Enterprise-Grade Security**: Delivers safe operations with auditability and permission controls.

## Get Started

- [Signup for Preview](https://aka.ms/sreagent)
- [Azure SRE Agent Overview](https://learn.microsoft.com/en-us/azure/sre-agent/overview?tabs=explore)
- [Pricing Calculator](https://aka.ms/sreagent/pricing/calc)
- [Integration and DevOps Resources](https://aka.ms/Build25/HeroBlog/agenticDevOps)

_For more technical resources, visit the [Azure SRE Agent home page](https://www.azure.com/sreagent)._

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/enterprise-ready-and-extensible-update-on-the-azure-sre-agent/ba-p/4444299)
