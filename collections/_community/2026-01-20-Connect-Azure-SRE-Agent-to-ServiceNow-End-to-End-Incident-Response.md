---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/connect-azure-sre-agent-to-servicenow-end-to-end-incident/ba-p/4487824
title: 'Connect Azure SRE Agent to ServiceNow: End-to-End Incident Response'
author: dbandaru
feed_name: Microsoft Tech Community
date: 2026-01-20 20:07:59 +00:00
tags:
- AI Agent
- AKS
- Azure Monitor
- Azure SRE Agent
- Cloud Automation
- Incident Automation
- Incident Management
- Incident Response
- Kubernetes
- Microsoft Azure
- Operations
- Root Cause Analysis
- ServiceNow Integration
- SRE
- Triage Automation
- Work Notes Automation
- AI
- Azure
- DevOps
- Community
section_names:
- ai
- azure
- devops
primary_section: ai
---
dbandaru demonstrates how to connect Azure SRE Agent to ServiceNow, automating incident detection and resolution through AI-driven workflows and seamless integration, with instructions for configuration and hands-on testing.<!--excerpt_end-->

# Connect Azure SRE Agent to ServiceNow: End-to-End Incident Response

In this comprehensive guide, dbandaru shows how to integrate the Azure SRE Agent with ServiceNow for streamlined, automated incident management in cloud-native environments. The process covers initial configuration, testing the full incident lifecycle, and understanding how AI helps resolve issues in real time.

## 🎯 What You'll Learn

- Integrate Azure SRE Agent with ServiceNow as your incident platform
- Securely configure ServiceNow credentials and connect both systems
- Create and submit a test incident in ServiceNow
- Watch as the AI-powered agent automatically investigates and resolves issues
- Review triage notes and root cause documentation written back to ServiceNow

## Prerequisites

- ServiceNow instance (Developer/PDI/Enterprise) with admin access
- Azure SRE Agent deployed in your Azure subscription

## Step 1: Gather ServiceNow Credentials

You'll need:

- **Endpoint URL**: Format `https://your-instance.service-now.com`
- **Username**: Found in profile on ServiceNow
- **Password**: Your ServiceNow login password

## Step 2: Configure SRE Agent in Azure Portal

1. Open [Azure Portal](https://portal.azure.com)
2. Search and select "Azure SRE Agent (Preview)"
3. Navigate to **Settings → Incident platform**, select **ServiceNow**
4. Input your ServiceNow endpoint, username, and password
5. Enable **Quickstart response plan** for automatic investigation
6. Save and verify connection: look for "ServiceNow is connected."

## Step 3: Create a Test Incident in ServiceNow

1. In ServiceNow, browse **Incident → Create New**
2. Fill details:
   - **Short Description**: `[SRE Agent Test] AKS Cluster memory pressure detected in production environment`
   - **Caller**: Any user
   - **Impact**: 2 - Medium
3. Submit and note the incident number

## Step 4: Watch SRE Agent in Action

1. Return to Azure Portal → SRE Agent → Activities → Incidents
2. The new ServiceNow incident appears automatically
3. Click incident to view autonomous investigation:
   - Acknowledged incident
   - Created triage plan
   - Identified affected AKS clusters
   - Validated memory metrics
   - Resolved and recorded root cause with resolution notes

## Step 5: Review Automated Resolution in ServiceNow

- Open the same incident in ServiceNow
- Confirm **State** is **Resolved**
- Review agent’s work notes and detailed resolution notes including:
  - Timestamps
  - Root cause analysis
  - Validation steps
  - Fixes applied

## Next Steps

- Customize SRE Agent response plans for different incidents
- Route Azure Monitor alerts for specific scenarios
- Explore advanced Azure SRE Agent documentation for additional automation features

---
*Questions? Join the discussion on the [Microsoft Tech Community](https://techcommunity.microsoft.com).*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/connect-azure-sre-agent-to-servicenow-end-to-end-incident/ba-p/4487824)
