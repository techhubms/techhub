---
primary_section: github-copilot
author: Arnaud Lheureux, davidwright, sdaniels
date: 2026-04-10 01:45:08 +00:00
section_names:
- ai
- azure
- devops
- github-copilot
external_url: https://devblogs.microsoft.com/all-things-azure/putting-agentic-platform-engineering-to-the-test/
feed_name: Microsoft All Things Azure Blog
tags:
- Agentic AI
- Agents
- AI
- All Things Azure
- Automation
- Azure
- Azure CLI
- Azure Functions
- Azure MCP Server
- Azure Monitor
- Azure Resource Deployment
- Azure SQL
- Azure Storage
- Copilot Agents
- DevOps
- Git Ape
- GitHub Copilot
- Governance
- Guardrails
- IaC
- Model Context Protocol (mcp)
- News
- Platform Engineering
- Repository Onboarding
- Self Service Platforms
- VS Code
title: Putting Agentic Platform Engineering to the test
---

Arnaud Lheureux, davidwright, and sdaniels walk through a hands-on “agentic platform engineering” demo using Git-ape inside VS Code, where GitHub Copilot agents (via Azure MCP) can deploy and manage Azure infrastructure through validated, policy-aware actions.<!--excerpt_end-->

# Putting Agentic Platform Engineering to the test

In **Part 1** of this series, the authors explain why platform engineering is being reshaped by **agentic AI**:

- Instead of humans translating intent through layers of CLIs, SDKs, and bespoke workflows, capable agents can interpret **natural-language goals**.
- Those agents can then turn intent into **safe, validated platform actions** using well-described APIs and control schemas.
- This raises the bar for internal platforms: stronger **guardrails**, **policy**, and **self-service interfaces** so teams can move faster without sacrificing **safety**, **reliability**, or **governance**.

In **Part 2** (this article), the authors build on those principles with a practical example showing how you can design and operate a platform for this new era.

## 1. Getting started

To start with APE’s **Git-ape**, clone the repository:

```bash
git clone https://github.com/Azure/git-ape
cd git-ape
```

Git-ape provides a ready-to-use demo so you can explore how it generates and manages cloud infrastructure using **Infrastructure-as-Code**.

## 2. Prerequisites

Before you begin, you’ll need:

- **Visual Studio Code** with the **GitHub Copilot** extension enabled
- A supported shell environment (the authors note it was only tested with **Bash shells** on **Ubuntu** and **Git-Bash on Windows**)
- **Azure CLI** installed and authenticated

Authenticate with Azure CLI:

```bash
az login
```

- **Azure MCP Server** configured (example below)

## 3. Installation (Copilot agent discovery)

This repository contains agent configuration files that **GitHub Copilot automatically discovers**.

Steps:

- Open the workspace in **VS Code**
- Verify agents are available by typing:
  - `@git-ape` for deployments
  - `@Git-ape Onboarding` for repository setup

If those agents appear, they’re installed and ready.

## MCP server configuration (Azure MCP)

To enable Git-ape’s Azure capabilities, configure **Azure MCP** in VS Code.

Edit your VS Code `settings.json` and add:

```json
{
  "azureMcp.serverMode": "namespace",
  "azureMcp.enabledServices": [
    "deploy",
    "bestpractices",
    "group",
    "subscription",
    "functionapp",
    "storage",
    "sql",
    "monitor"
  ],
  "azureMcp.readOnly": false
}
```

This enables Git-ape to:

- Deploy Azure resources
- Apply best practices
- Interact with Azure services directly through **Copilot**

For advanced scenarios and troubleshooting:

- Azure MCP Setup Guide: https://github.com/Azure/autocloud/blob/main/docs/AZURE_MCP_SETUP.md

## 4. Destruction (cleanup)

To remove your deployed environment, use the `@git-ape` agent to clean up:

```text
@git-ape destroy [your mess]
```

## Next steps

Once the repo is cloned, agents are discovered, and Azure MCP is configured, you can use **Git-ape through GitHub Copilot** to deploy and onboard Azure resources from within VS Code.

- Readme: https://github.com/Azure/autocloud/blob/main/README.md

The post also references a video walkthrough of the examples (URL not included in the provided content).


[Read the entire article](https://devblogs.microsoft.com/all-things-azure/putting-agentic-platform-engineering-to-the-test/)

