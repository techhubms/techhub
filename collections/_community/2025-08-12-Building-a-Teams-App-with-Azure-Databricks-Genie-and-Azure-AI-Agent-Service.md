---
external_url: https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/supercharge-data-intelligence-build-teams-app-with-azure/ba-p/4442653
title: Building a Teams App with Azure Databricks Genie and Azure AI Agent Service
author: GISV-PSA-MSFT
feed_name: Microsoft Tech Community
date: 2025-08-12 21:10:22 +00:00
tags:
- AI Agents
- Azure AI Agent Service
- Azure AI Foundry
- Azure App Service
- Azure Databricks
- Bot Service
- Conversational AI
- Data Intelligence
- Data Visualization
- DevTunnel
- Enterprise AI Integration
- Genie API
- GitHub
- Kubernetes
- LLM Integration
- Microsoft Teams App
- Natural Language Analytics
- On Behalf Of Authentication
- Python
- Sample Code
- Secure Data Access
section_names:
- ai
- azure
- coding
- ml
---
GISV-PSA-MSFT walks through integrating Azure Databricks Genie APIs with Azure AI Foundry to build a secure Teams app in Python, offering actionable examples for developers and data engineers.<!--excerpt_end-->

# Building a Teams App with Azure Databricks Genie and Azure AI Agent Service

## Introduction

Unlock the full potential of your Azure Databricks investments by connecting them to the powerful AI capabilities of Azure AI Foundry—all within Microsoft Teams. This guide, authored by GISV-PSA-MSFT, is a follow-up to the Azure Databricks connector announcement at Microsoft Build 2025 and demonstrates how to build a Python-based Teams app that interacts with Genie APIs using secure On-Behalf-Of (OBO) authentication.

## Key Components

### AI/BI Genie

Genie is an intelligent agent feature within Azure Databricks that offers natural language APIs. It allows users to query, analyze, and visualize data conversationally, making advanced analytics accessible inside Teams.

### Azure AI Foundry

Azure AI Foundry acts as a unified platform for building, managing, and deploying AI solutions. Its features include orchestrating AI agents, connecting to data sources like Databricks, and managing models, workflows, and governance.

### Azure AI Agent Service

These modular, reusable components interact with data and AI services, enabling developers to build multi-agent workflows, integrate enterprise systems, and deliver contextual insights to users.

## Solution Overview

The provided sample app demonstrates:

- Connecting Teams to Azure Databricks Genie via Azure AI Foundry using OBO auth
- Querying/visualizing data with Genie APIs and LLMs securely
- Running the app locally with DevTunnel and considerations for production deployment

**Purpose:** To provide developers with a starting point for integrating Databricks and Azure AI in Teams for enterprise-grade, AI-powered data workflows.

## Architecture

The architecture involves Teams acting as a chat interface, which passes requests through a registered Azure Bot, leverages secure OBO authentication, and interacts with Azure AI Foundry, which connects and orchestrates Genie queries on Databricks. Returned results (charts, answers, analytics) are rendered in Teams via dynamic storage blobs for images if needed.

## Step-by-Step Setup

1. **Prerequisites:**
    - Knowledge of Azure Databricks, Azure Bot Service, DevTunnel, Teams app registration
    - An Azure AI Foundry project with Databricks and LLM connections

2. **Configuration:**
    - Set up DevTunnel for local development
    - Register the Teams bot and app within Azure
    - Connect Azure AI Foundry to the Databricks Genie space
    - Provision storage with public blob access for chart/image rendering
    - Fill the `.env` file with your app IDs, secrets, and endpoints

3. **Teams App Manifest:**
    - Create and configure the manifest (endpoints, domains, permissions, SSO)

4. **Run and Test:**
    - Activate the Python virtual environment, run the app locally
    - Upload the Teams app manifest for testing conversational queries in the Teams client

## Sample Genie Queries

Try the following examples in your Teams app:

- "Show the top sales reps by pipeline in a pie chart."
- "What is the average size of active opportunities?"
- "How many opportunities were won or lost in the current fiscal year?"

## Resources & Next Steps

- [Code Sample on GitHub](https://github.com/Azure-Samples/AI-Foundry-Connections/tree/main/src/samples/adb_aifoundry_teams) – Includes a step-by-step README
- Consider deploying to Azure App Service or Kubernetes, or experimenting with other tech stacks and the M365 Agents Toolkit
- Explore [M365 Agents SDK](https://learn.microsoft.com/en-us/microsoft-365/ai/agents/) and [Agents Toolkit](https://github.com/microsoft/agents-toolkit) for advanced AI solutions

## Conclusion

By connecting Azure Databricks, AI Foundry, and Genie APIs inside Microsoft Teams, this sample equips developers to deliver secure, scalable, and interactive data intelligence experiences. Experiment with the provided tools, extend their capabilities, and create your own AI-powered enterprise apps.

*Authored by GISV-PSA-MSFT, August 2025.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/supercharge-data-intelligence-build-teams-app-with-azure/ba-p/4442653)
