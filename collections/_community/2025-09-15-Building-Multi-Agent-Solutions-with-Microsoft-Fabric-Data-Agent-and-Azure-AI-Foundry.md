---
layout: post
title: Building Multi-Agent Solutions with Microsoft Fabric Data Agent and Azure AI Foundry
author: GISV-PSA-MSFT
canonical_url: https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/explore-microsoft-fabric-data-agent-azure-ai-foundry-for-agentic/ba-p/4453709
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-09-15 14:49:18 +00:00
permalink: /ai/community/Building-Multi-Agent-Solutions-with-Microsoft-Fabric-Data-Agent-and-Azure-AI-Foundry
tags:
- Agentic AI
- Azure AI Foundry
- Azure AI Services
- Copilot Studio
- Data Engineering
- Data Lakehouse
- Fabric Data Agent
- Fabric Notebook
- Financial Services
- Generative AI
- Industry Solutions
- Insurance Analytics
- Low Code Development
- M365 Agents Toolkit
- Microsoft Fabric
- Microsoft Teams Integration
- Multi Agent Architecture
- Power BI
- Pro Code Development
- Responsible AI
section_names:
- ai
- azure
- ml
---
GISV-PSA-MSFT, with contributors Jeet J and Ritaja S, presents a step-by-step guide to implementing agentic AI applications, combining Microsoft Fabric Data Agent, Azure AI Foundry, and Copilot Studio to solve real-world industry problems such as insurance analytics.<!--excerpt_end-->

# Building Multi-Agent Solutions with Microsoft Fabric Data Agent and Azure AI Foundry

**Contributors:** Jeet J & Ritaja S

## Context & Objective

Agentic AI applications are rapidly expanding within enterprises. This post demonstrates how both business analysts and developers can leverage the Microsoft stack—specifically Fabric Data Agent, Azure AI Foundry, and Copilot Studio—to build production-ready, reusable multi-agent apps.

Professionals can now use Microsoft Fabric and Azure AI Foundry together, enabling robust, industry-specific AI solutions. This walkthrough covers setting up agents for insurance and financial services, combining low-code and pro-code development, and integrating solutions into Microsoft Teams via the M365 Agents Toolkit.

**Disclaimer:** This guide is for educational purposes. Production deployment requires following engineering best practices.

## Key Topics Covered

- Agentic patterns in Microsoft ecosystem
- Setting up and connecting Fabric Data Agent & Azure AI Foundry Agent
- Creating sample industry datasets with Fabric Notebooks
- Building and configuring multi-agent workflows (insurance use case)
- Integrating and deploying with Copilot Studio and Teams
- Responsible engineering tips

## Prerequisites

- Access to Azure Tenant and Subscription
- Fabric compute capacity (F2 or higher)
- Admin access to Fabric Portal/Power BI for tenant settings
- At least one data source (Fabric Data Warehouse, Lakehouse, semantic model, or KQL DB)
- Power BI semantic models via XMLA endpoints enabled
- (For Copilot Studio) License or trial sign-up and Teams integration capability

## Pro-Code Agentic Path: Fabric & Azure AI Foundry

### 1. Setup

- **Workspace:** Create Fabric Workspace on Power BI, link appropriate compute capacity.
- **Lakehouse:** Add InsuranceLakehouse; associate with Fabric Notebook.
- **Data Preparation:** Use Python (PySpark) with the Faker library to generate fake insurance datasets.
- **Data Agent:** Create a Fabric Data Agent, link data source, add sample instructions (e.g., churn calculations), and publish.

### 2. Azure AI Foundry Agent

- Launch Azure AI Foundry.
- Deploy a language model (e.g., gpt-4o-mini).
- Create a Foundry Agent, using Fabric Data Agent as the knowledge source (connect securely with Workspace and Artifact IDs).
- Add tools like Code Interpreter for enhanced responses.
- Test via sample analytics questions (e.g., churn rate, claims trends, forecasting).

### 3. Integration and Publishing

- Expose Fabric Data Agent for use in Copilot Studio.
- Combine multi-agent capabilities for advanced analytics workflows.

## Low-Code Agentic Path: Fabric & Copilot Studio

- Setup Copilot Studio (trial/license/pay-as-you-go).
- Use Fabric Lakehouse and Data Agent as before.
- Create new agent in Copilot Studio, configure connection to Fabric Data Agent.
- Define business-relevant prompt instructions.
- Enable Teams/M365 Copilot channel, follow policies for Teams app deployment.
- Test question routing to Fabric Data Agent, analyze activity map.

## End-to-End Integration

- Demonstrates how pro-code and low-code developers jointly build multi-agent solutions.
- Shows Copilot Studio and Azure AI Foundry agent integration for connected scenarios.
- Emphasizes responsible engineering principles (monitoring—Azure Monitor, governance—Purview).

## Key Takeaways

- Real industry use-case (insurance/financial) for agentic AI
- Stepwise creation of sample data, agents, and workflow
- Integration of analytics, AI, and user channels in the Microsoft cloud
- Links to official docs, tutorials, and extended scenarios (see resource section below)

## Further Resources

- Build AI Agents: [Microsoft Developer](https://developer.microsoft.com/en-us/agents)
- Microsoft Fabric overview: [Learn](https://learn.microsoft.com/en-us/fabric/fundamentals/microsoft-fabric-overview)
- [Create a Fabric data agent](https://learn.microsoft.com/en-us/fabric/data-science/how-to-create-data-agent)
- [Azure AI Foundry documentation](https://learn.microsoft.com/en-us/azure/ai-foundry/)
- [M365 Agents Toolkit](https://learn.microsoft.com/en-us/microsoft-365/developer/overview-m365-agents-toolkit?toc=%2Fmicrosoftteams%2Fplatform%2Ftoc.json)
- [GitHub Toolkit](https://github.com/OfficeDev/microsoft-365-agents-toolkit)
- [Full Teams and Copilot deployment tutorial (YouTube)](https://www.youtube.com/watch?v=U9Yv2vjKYbI)

**Happy Learning!**

---

## Authors and Acknowledgments

- **Contributors:** Jeet J & Ritaja S  
- **Reviewed by:** Joanne W, Amir J & Noah A  
- **Posted by:** GISV-PSA-MSFT, Microsoft Tech Community  
- **Last Updated:** Sep 15, 2025 (Version 1.0)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/explore-microsoft-fabric-data-agent-azure-ai-foundry-for-agentic/ba-p/4453709)
