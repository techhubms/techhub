---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/what-it-takes-to-give-sre-agent-a-useful-starting-point/ba-p/4500343
title: What It Takes to Give SRE Agent a Useful Starting Point
author: Dalibor_Kovacevic
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-03-10 14:15:28 +00:00
tags:
- App Security
- Application Insights
- Authentication
- Azure
- Azure Container Apps
- Azure Monitor
- Azure SRE Agent
- Community
- Deployment
- DevOps
- GitHub Integration
- Incident Management
- Incident Response
- Kusto
- Log Analytics
- Managed Identity
- Monitoring
- Observability
- Onboarding
- Operational Readiness
- RBAC
- Security
- Site Reliability Engineering
section_names:
- azure
- devops
- security
---
Dalibor_Kovacevic demonstrates how to effectively onboard the Azure SRE Agent for real-world apps, showing the practical steps needed to deliver actionable insights and better incident response in cloud environments.<!--excerpt_end-->

# What It Takes to Give SRE Agent a Useful Starting Point

*Author: Dalibor_Kovacevic*

## Introduction

The onboarding process for Azure SRE Agent has been revamped to focus on practical value: after setup, can the agent actually help with your real applications? This post walks through every phase of onboarding, emphasizing why rich context and integration with your operational data are crucial.

## Why Context Matters for SRE Agent

In previous experiments, the team discovered that SRE Agent’s effectiveness depends on direct access to a customer’s application resources: code, logs, deployment history, and workspace data. When fully connected, the agent investigates issues with real understanding—pinpointing root causes and recommending actionable fixes.

## Setting a New Onboarding Standard

Real onboarding goes beyond simply provisioning resources. Unless the agent is configured with what it needs—a real app, live data feeds, codebases, and operational knowledge—it cannot deliver the value expected on day one. This walkthrough demonstrates a complete onboarding scenario:

- **Connecting a real sample app**
- **Wiring up Azure Monitor alerts**
- **Attaching code repositories and logs**
- **Uploading a knowledge file**
- **Pushing the agent through realistic engineering tasks**

## Step-by-Step Onboarding

### 1. Create the SRE Agent

- Select subscription, resource group, agent name, and region.
- Azure provisions the runtime, managed identity, Application Insights, and Log Analytics workspace in about two minutes.
- This creates the execution environment for the agent, enabling it to inspect files, run commands, and query services.

### 2. Add Contextual Data

- The setup page organizes onboarding around connecting:
  - **Code**: For real system insights
  - **Logs**: For accurate table/schema recognition
  - **Incidents**: For visibility into real operational pain
  - **Azure Resources**: For proper scoping
  - **Knowledge Files**: For team-specific contextual info

- If nothing is connected, the system warns you that the agent lacks enough to answer useful questions.

### 3. Connect Logs

- Supported platforms include Azure Data Explorer (Kusto), Datadog, Elasticsearch, Dynatrace, New Relic, Splunk, and Hawkeye.
- Explicit connections prevent the agent from ‘guessing’ and ensure that real operational data is used.

### 4. Connect Incident Platform

- Link Azure Monitor (or PagerDuty/ServiceNow) so live alerts map to real app situations.

### 5. Connect Code Repository

- Example: Microsoft-foundry/foundry-agent-webapp (React and ASP.NET Core running on Azure Container Apps).
- The agent gains insight into auth flows, health probe configuration, commit history, and more.

### 6. Scope Azure Resources

- Define exactly which resource group or subscription the agent should observe.
- This limits ‘wandering’ and makes investigations more focused.

### 7. Upload a Knowledge File

- Markdown documents containing architectural overviews, endpoint definitions, expected failure modes, etc., help the agent reason like a team engineer.

## Post-Onboarding: Operational Insights

### Configuration Transparency

- The setup panel displays connected and missing components, making partial setups less risky while clearly showing what else will improve agent performance.

### Real-World Troubleshooting Examples

- **Cluster and Resource Appraisal**: The agent used Azure CLI and logs to summarize deployment details, highlighting real failures (like cold starts and probe noise) over generic advice.
- **Authentication Issue Analysis**: By inspecting connected code, the agent provided actionable checks for 401 errors specific to the app's implementation (client ID, URI issues, scope, logging config, etc.).
- **Incident Monitoring**: Azure Monitor alerts for server errors, container restarts, and high response latency were set up, producing real events for the agent to triage.
- **Incident Response Plans**: The team created targeted plans tied to application specifics and real alert data, not generic templates.
- **RBAC and Log Analytics Debugging**: The agent identified missing permissions and insufficient scope errors, prescribed RBAC fixes, and explained operational data gaps (such as incorrect OpenTelemetry exporter configs).
- **GitHub Issue Triaging**: With repository context, the agent reviewed and properly classified existing GitHub issues, distinguishing between documentation questions and confirmed bugs, and suggesting next steps.

## Lessons Learned

True onboarding means removing the guesswork for AI-driven operational tools:

- **Code**: Stops the agent from speculating on system structure
- **Logs**: Ensures awareness of genuine application state
- **Incidents**: Grounds the agent in operational reality
- **Resource Scope**: Keeps investigations relevant
- **Knowledge Files**: Transfers tribal knowledge for richer reasoning

## Conclusion

Onboarding isn’t done when a resource is provisioned—it’s done when the agent is ready to help with an actual operational issue. The Azure SRE Agent’s new onboarding flow, when used as shown, achieves this, streamlining incident response and operational troubleshooting from day one.

[Create your SRE Agent →](https://sre.azure.com) (Generally available March 10, 2026)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/what-it-takes-to-give-sre-agent-a-useful-starting-point/ba-p/4500343)
