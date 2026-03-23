---
tags:
- Agent Endpoint
- AI
- AI Agents
- Azd
- Azd AI Agent
- Azd Extension
- Azure
- Azure AI Foundry
- Azure Developer CLI
- Azure SDK
- Bicep
- CI/CD
- DevOps
- Environment Variables
- GitHub Actions
- GPT 4o
- IaC
- Log Streaming
- Managed Identity
- Microsoft Foundry
- Monitoring
- News
- Python
- RBAC
- VS Code
external_url: https://devblogs.microsoft.com/azure-sdk/azd-ai-agent-end-to-end/
author: PuiChee (PC) Chan
date: 2026-03-23 16:54:40 +00:00
title: 'From code to cloud: Deploy an AI agent to Microsoft Foundry in minutes with azd'
feed_name: Microsoft Azure SDK Blog
primary_section: ai
section_names:
- ai
- azure
- devops
---

PuiChee (PC) Chan walks through an end-to-end workflow for deploying an AI agent to Microsoft Foundry using the Azure Developer CLI (azd), including Bicep-based infrastructure scaffolding, one-command deployment, local dev, invocation, and real-time log streaming from VS Code.<!--excerpt_end-->

## Overview

You built an AI agent and it works locally. The missing step is getting it to a production endpoint with provisioning, model deployment, identity, wiring, and monitoring.

This tutorial shows how to go from a repo to a live, invokable Microsoft Foundry agent using `azd ai agent`, including remote invocation, local development, and real-time monitoring—all from Visual Studio Code.

## Prerequisites

- [VS Code](https://code.visualstudio.com/) installed
- [Azure Developer CLI (`azd`)](https://learn.microsoft.com/azure/developer/azure-developer-cli/install-azd) installed
- [Git](https://git-scm.com/) installed
- An Azure subscription with access to [Microsoft Foundry](https://ai.azure.com/)
  - Ensure you have quota for the model you plan to deploy (for example, GPT-4o) in your target region

## Step 1: Clone a sample agent project

This walkthrough uses a sample Python-based hotel concierge agent.

```bash
git clone https://github.com/puicchan/seattle-hotel-agent
cd seattle-hotel-agent
code .
```

## Step 2: Authenticate and deploy

Sign in to Azure:

```bash
azd auth login
```

Initialize and deploy:

```bash
azd ai agent init
azd up
```

These two commands scaffold the infrastructure and deploy your agent. After deployment, `azd` prints a direct link to your agent in the Microsoft Foundry portal.

## Under the hood

### What `azd ai agent init` generates

`azd ai agent init` generates infrastructure-as-code (IaC) assets in your repo, including:

- `infra/main.bicep`
  - Entry point wiring together all resources
- A Foundry Resource (top-level container for AI resources)
- A Foundry Project under the hub (where your agent lives)
- Model deployment configuration (for example, GPT-4o)
- Managed identity with RBAC role assignments
  - Lets your agent securely access the model and any connected data sources
- `azure.yaml`
  - `azd` service map tying your agent code to the Foundry host
- `agent.yaml`
  - Agent definition file with metadata and environment variables

You own these artifacts: the generated Bicep is in your repo, so you can inspect, customize, and version-control it.

### What `azd up` does

`azd up` provisions the resources in Azure and publishes the agent to Foundry. Behind the scenes it:

- Runs the Bicep deployment
- Uploads your agent definition
- Registers the agent endpoint

The Foundry link in the output points to the agent’s playground.

## Step 3: Try it in the Foundry playground

Open the agent playground using the Foundry link from `azd up`, then try a prompt like:

- “What suites are available at the downtown Seattle hotel?”

![Agent playground in Microsoft Foundry](https://devblogs.microsoft.com/azure-sdk/wp-content/uploads/sites/58/2026/03/03-27-azd-ai-agent-end-to-end-foundry-playground-scaled.webp)

## Step 4: Invoke your agent from the terminal

Invoke the remote agent endpoint from VS Code’s terminal:

```bash
azd ai agent invoke
```

This preserves conversation context across turns for multi-turn chat.

> **Tip:** By default, `azd ai agent invoke` targets the remote endpoint. If a local agent is running (see next step), it automatically routes to the local instance.

## Step 5: Run locally for development

To iterate faster on your agent logic, run it locally:

```bash
azd ai agent run
```

Pair this with `azd ai agent invoke` to test prompts against the local instance. Edit code, restart, and invoke again—no redeploy required.

## Step 6: Monitor in real time

Stream logs from the published agent:

```bash
azd ai agent monitor
```

Notes:

- By default, it prints ~50 recent log entries and exits.
- Add `--follow` to stream continuously as requests arrive.

This is useful when you have a frontend app or any client consuming the endpoint and you want to watch request/response flow for production troubleshooting.

## Step 7: Check agent health

Get a quick status check and deployment metadata:

```bash
azd ai agent show
```

## Step 8: Clean up resources

Delete the resource group and all provisioned resources to avoid ongoing charges:

```bash
azd down
```

## Bonus: Wire up a frontend chat app

A separate lightweight chat app is available that consumes the agent deployed earlier.

Clone it:

```bash
git clone https://github.com/puicchan/chat-app-foundry
cd chat-app-foundry
```

Set environment variables for the published agent. You can find these in the `azd up` output from the agent deployment, or by running `azd env get-values` in the `seattle-hotel-agent` directory.

```bash
azd env set AZURE_AI_AGENT_NAME "seattle-hotel-agent"
azd env set AZURE_AI_AGENT_VERSION "<version-number>"
azd env set AI_ACCOUNT_NAME "<your-ai-account-name>"
azd env set AI_ACCOUNT_RESOURCE_GROUP "<your-resource-group>"
azd env set AZURE_AI_FOUNDRY_ENDPOINT "<your-foundry-endpoint>"
```

Provision and deploy the chat app:

```bash
azd up
```

Then stream logs in another terminal:

```bash
azd ai agent monitor --follow
```

Ask a question in the chat UI and watch the request/response appear in the log stream.

![Chat app UI with agent monitor running side-by-side](https://devblogs.microsoft.com/azure-sdk/wp-content/uploads/sites/58/2026/03/03-27-azd-ai-agent-end-to-end-monitor.webp)

## The full command set at a glance

| Command | What it does |
| --- | --- |
| `azd ai agent init` | Scaffold a Foundry agent project with IaC |
| `azd up` | Provision Azure resources and deploy the agent |
| `azd ai agent invoke` | Send prompts to the remote or locally run agent |
| `azd ai agent run` | Run the agent locally for development |
| `azd ai agent monitor` | Stream real-time logs from the published agent |
| `azd ai agent show` | Check the health and status of the published agent |
| `azd down` | Delete all provisioned Azure resources |

## What comes next

This workflow is positioned as an “inner loop” for agent development: build, deploy, test, monitor—from the terminal.

It can also plug into CI/CD:

- To deploy on every push to `main`, add `azd up` to a GitHub Actions workflow.
- To manage dev/staging/prod with the same commands, use `azd env`.

The `azd ai agent` commands are powered by an `azd` extension built by the Foundry team and are described as actively evolving.

## Get started and resources

To try it:

- [Install `azd`](https://learn.microsoft.com/azure/developer/azure-developer-cli/install-azd)
- Run `azd ai agent init` in an agent project (it automatically installs the required extension)

Feedback:

- File bugs/feature requests in the [`azure-dev` GitHub repo](https://github.com/Azure/azure-dev/issues) and tag them with `ai-agent`.

Resources:

- [Azure Developer CLI documentation](https://learn.microsoft.com/azure/developer/azure-developer-cli/) — docs, install guides, reference
- [Sample agent repo](https://github.com/puicchan/seattle-hotel-agent) — hotel concierge agent used in the walkthrough
- [Sample chat app repo](https://github.com/puicchan/chat-app-foundry) — lightweight frontend consuming your published agent
- [Microsoft Foundry](https://ai.azure.com/) — manage agents in the portal


[Read the entire article](https://devblogs.microsoft.com/azure-sdk/azd-ai-agent-end-to-end/)

