---
external_url: https://techcommunity.microsoft.com/t5/azure-observability-blog/azure-copilot-observability-agent-intelligent-investigations/ba-p/4469719
title: Intelligent Troubleshooting with Azure Copilot Observability Agent
author: EfratNauerman
feed_name: Microsoft Tech Community
date: 2025-11-18 16:02:34 +00:00
tags:
- AI Driven Investigations
- AKS
- Anomaly Detection
- Application Services
- Azure Copilot Observability Agent
- Azure Monitor
- Azure Portal
- Cloud Operations
- Copilot Integration
- Deep Preview
- Diagnostics
- Investigation Workflow
- LLMs
- ML
- Metric Analysis
- Observability
- Resource Health
- Root Cause Analysis
- Virtual Machines
- AI
- Azure
- Community
section_names:
- ai
- azure
primary_section: ai
---
EfratNauerman shares an overview of the Azure Copilot observability agent, explaining its AI-powered approach to diagnostic investigations and how it enhances troubleshooting, visibility, and recovery across the Azure stack.<!--excerpt_end-->

# Intelligent Troubleshooting with Azure Copilot Observability Agent

The Azure Copilot observability agent is a new tool designed to make cloud operations and troubleshooting on Azure smarter and more efficient. Building on top of Azure Monitor’s investigation capabilities, it uses advanced AI and machine learning to automatically identify, analyze, and correlate issues across applications and infrastructure components.

## Key Features

- **Automatic Problem Isolation:** Quickly pinpoints problems in complex applications spanning multiple components.
- **Anomaly Detection and Correlation:** Uses ML models and observability signals (metrics, logs) to identify root causes.
- **Multi-source Data Correlation:** Combines data from various sources for full diagnostic context.
- **Human-readable Findings:** Provides actionable insights and next steps in plain language.
- **Collaboration Support:** Results can be preserved and shared with the team for tracking and resolution.

## How It Works

When an alert is triggered in Azure, users can click ‘Investigate’ to launch the observability agent. The agent then surfaces AI-generated findings, each offering potential causes and troubleshooting starting points. Supporting data is easily available for deeper analysis.

AI, machine learning models, and large language models (LLMs) enable:

- Automated anomaly detection in metrics/logs
- Correlation of events and health signals
- Generation of actionable recommendations

## Expanded Intelligence Across Azure Environments

The agent is built for full-stack coverage, supporting:

- **Application Services**
- **Virtual Machines (VMs)**
- **Azure Kubernetes Service (AKS) Clusters**
- **Underlying infrastructure**

### Investigation Types

1. **Metric Analysis:** Detect abnormal CPU, memory, or network usage in VMs and AKS nodes.
2. **Alert Correlation:** Surface cascading issues like pod restarts in AKS clusters.
3. **Resource Health Checks:** Validate infrastructure stability alongside application performance.
4. **Diagnostics Tool Integration:** Link findings directly to Azure diagnostics tools for remediation steps.
5. **Log-based Metric Analysis:** Enrich root cause analysis by linking anomalies to log-derived metadata.

## Regional and Workflow Integration

- Supported in most Azure regions ([documentation](https://learn.microsoft.com/en-us/azure/azure-monitor/aiops/aiops-issue-and-investigation-overview)).
- Integrated with Azure portal, alerting infrastructure, and Copilot (gated preview).
- Developers can interact with the agent in natural language via Copilot: e.g., "Show me my critical alerts", "Which alerts need my attention?"
- Use deep agentic reasoning for interactive, dialog-based investigation.

## Roadmap and Future Development

Planned updates will expand the agent’s intelligence, enable advanced scenario coverage, and allow streamlined monitoring configuration. New "Deep Preview" features aim to offer even richer investigation capabilities and more natural interaction.

## Getting Started

- Access the agent through Azure alerts, Copilot preview, or the Azure portal.
- For full documentation and setup details, visit [documentation](https://aka.ms/investigationmanuals).
- Join webinars, Ignite sessions, and previews for early feature access ([Ignite Session link](https://ignite.microsoft.com/en-US/sessions/BRK149?source=sessions), [December Webinar Registration](https://forms.office.com/r/XYAarZvFte), [Deep Preview Signup](https://forms.office.com/r/m5K2DcLkdF)).

## Feedback and Support

Microsoft encourages user feedback via <azmoninvestigation@microsoft.com> and built-in feedback forms. The team is committed to evolving capabilities based on practitioner needs.

---

**Author:** EfratNauerman

For more updates, follow the Azure Observability Blog.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-observability-blog/azure-copilot-observability-agent-intelligent-investigations/ba-p/4469719)
