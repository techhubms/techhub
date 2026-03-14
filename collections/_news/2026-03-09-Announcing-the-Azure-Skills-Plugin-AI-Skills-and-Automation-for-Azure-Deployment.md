---
external_url: https://devblogs.microsoft.com/all-things-azure/announcing-the-azure-skills-plugin/
title: 'Announcing the Azure Skills Plugin: AI Skills and Automation for Azure Deployment'
author: Chris Harris
primary_section: ai
feed_name: Microsoft All Things Azure Blog
date: 2026-03-09 20:02:13 +00:00
tags:
- AI
- AI Agents
- All Things Azure
- Azure
- Azure CLI
- Azure Developer CLI
- Azure MCP
- Azure Skills Plugin
- Claude Code
- Cloud Deployment
- Cost Optimization
- DevOps
- DevOps Automation
- Diagnostics
- Foundry
- Foundry MCP
- IaC
- Migration
- Model Deployment
- News
- Plugins
- RBAC
- Resource Provisioning
- Skills
- VS Code
section_names:
- ai
- azure
- devops
---
Chris Harris introduces the Azure Skills Plugin, showcasing how agents like GitHub Copilot can automate Azure deployments with curated skills, MCP tooling, and Foundry integration.<!--excerpt_end-->

# Announcing the Azure Skills Plugin: AI Skills and Automation for Azure Deployment

*Author: Chris Harris*

## Overview

The Azure Skills Plugin is a new toolkit designed to give AI coding agents—like GitHub Copilot and Claude Code—concrete knowledge and automation power for real Azure deployments. It bridges the gap between code suggestions and actual cloud execution by providing structured workflows, Azure automation tooling, and integration with Microsoft Foundry for model and agent operations.

## What's Included

- **19+ Azure Skills**: Encapsulated workflows that guide agents through Azure-specific decisions, infrastructure generation, validation, deployment, cost optimization, diagnostics, and more.
- **Azure MCP Server**: Over 200 tools spanning 40+ Azure services, enabling agents to list resources, check prices, query logs, perform diagnostics, and automate deployments.
- **Foundry MCP Server**: Connects AI agents to Microsoft Foundry for direct model management, deployment, and catalog workflows.
- **Plugin Package**: Bundles skills and MCP servers, providing seamless agent access in environments like GitHub Copilot (VS Code), Copilot CLI, Claude Code, and any tool supporting skills/plugins.

## Key Features

- *Portability*: Use the same skills and tools across supported hosts—no matter whether you work in the editor, terminal, or AI-powered agent toolkits.
- *Real Azure Operations*: When the plugin is installed, prompts such as "Deploy my Python Flask API to Azure" are executed as orchestrated actions (e.g., generating Dockerfiles, running validations, deploying infrastructure), moving beyond static tutorials.
- *Concrete Expert Knowledge*: Skills are curated by Azure experts, translating best practices, decision trees, guardrails, and step-by-step workflows into reusable packages.
- *AI and DevOps Integration*: Automates infrastructure as code, diagnostics, cost tracking, and model workflows via skills-driven actions and managed contexts.

## Workflow Breakdown

1. **Skill Guidance**: Agents leverage skills to decide which Azure resources and services are appropriate for the project.
2. **Tool Execution**: MCP Server enables agents to carry out actions like provisioning, deployment, diagnostics, and cost checks directly on Azure.
3. **Foundry Integration**: For AI scenarios, agents interact with Foundry for model cataloging, deployment, and agent orchestration.

### Example Skills

- `azure-prepare`: Analyzes a project and generates infrastructure code plus recommended configurations.
- `azure-validate`: Performs checks to avoid deployment errors upfront.
- `azure-deploy`: Automates the deployment pipeline using `azd`.
- `azure-cost-optimization`: Finds resource waste and offers savings tips.
- `azure-diagnostics`: Troubleshoots Azure issues via logs and KQL.

## How to Get Started

### Prerequisites

- Compatible AI agent host: GitHub Copilot (VS Code), Copilot CLI, Claude Code
- Node.js 18+
- Azure CLI (`az`) and Developer CLI (`azd`), installed and authenticated
- An Azure account for real operations

### Installation Steps

1. Visit [aka.ms/azure-plugin](https://aka.ms/azure-plugin)
2. Follow the install process for your environment
3. Ensure the Azure skills and MCP configuration are present in your workspace
4. Verify skills and MCP servers are active (look for `.mcp.json` and available skills)

### Test Your Setup

- Prompt: "What Azure services would I need to deploy this project?"
- Tool-backed Prompt: "List my Azure resource groups"

If the agent responds with structured guidance (first prompt) and real resource data (second prompt), the setup is functioning.

## Who Benefits?

- Startups and AI app founders seeking faster cloud delivery
- Agent builders needing end-to-end Azure + AI workflows
- Developers streamlining deployment from IDE to Azure
- DevOps teams automating infrastructure as code
- Anyone overwhelmed by Azure service choices

## Core Takeaways

- The plugin tightly couples agent skills with actionable tools, making cloud deployment, diagnostics, and optimization reliable and automated.
- By using skills rather than prompts, developers benefit from codified expertise, guardrails, and direct automation.
- Real Azure operations are executed, minimizing manual errors and "hallucinated" guidance from generic AI responses.

## Upcoming Series Parts

1. Install Guide
2. Skills-MCP Architecture Deep Dive
3. Core Workflow (Prepare → Validate → Deploy)
4. Highlighted Skills (e.g., cost optimization)
5. The Structure of a SKILL.md
6. Real-world End-to-End Demo
7. Future of Skills, MCP, and Azure Automation

## References & Further Reading

- [Azure Skills Plugin README](https://github.com/microsoft/azure-skills)
- [aka.ms/azure-plugin](https://aka.ms/azure-plugin)

---

Ready to streamline your Azure deployments with AI-powered agents? Try the Azure Skills Plugin today and experience context-aware automation, built-in best practices, and DevOps acceleration.

This post appeared first on "Microsoft All Things Azure Blog". [Read the entire article here](https://devblogs.microsoft.com/all-things-azure/announcing-the-azure-skills-plugin/)
