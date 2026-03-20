---
title: 'Azure Developer CLI (azd): Run and test AI agents locally with azd'
feed_name: Microsoft Azure SDK Blog
tags:
- Agent Invocation
- AI
- AI Agents
- Azd
- Azure
- Azure AI Foundry
- Azure Developer CLI
- Azure SDK
- Azure.ai.agents Extension
- CLI Tooling
- Dependency Detection
- DevOps
- GitHub Issues
- Inner Development Loop
- Local Development
- News
- Preview Release
- Streaming Responses
- Terminal Workflow
- V0.1.14 Preview
external_url: https://devblogs.microsoft.com/azure-sdk/azd-ai-agent-run-invoke/
date: 2026-03-18 17:49:36 +00:00
section_names:
- ai
- azure
- devops
primary_section: ai
author: PuiChee (PC) Chan, Travis Angevine
---

PuiChee (PC) Chan and Travis Angevine announce new Azure Developer CLI (azd) commands to run AI agents locally and invoke them from the terminal, supporting both local sessions and Azure AI Foundry endpoints for faster iteration.<!--excerpt_end-->

## Overview

You can now run an AI agent on your local machine and send it messages directly from the Azure Developer CLI (`azd`)—without using the Azure portal.

This capability is provided by the `azure.ai.agents` extension.

## What’s new

The [`azure.ai.agents` extension](https://learn.microsoft.com/azure/developer/azure-developer-cli/extensions/azure-ai-foundry-extension) adds two commands:

- `azd ai agent run`
  - Starts your agent locally.
  - Automatically detects your project type (Python, Node.js, etc.).
  - Detects and installs dependencies.

- `azd ai agent invoke`
  - Sends a message to a running agent.
  - Works against:
    - A locally running agent, or
    - An agent deployed to Azure AI Foundry.

## Why it matters

AI agent development often involves switching between an editor, terminal, and cloud portal to test changes. The new `run` and `invoke` commands keep the inner loop in the terminal:

- Start the agent
- Send a prompt
- See the streamed response
- Iterate

## How to use it

### Start an agent locally

Use `azd ai agent run` to launch the agent process locally. If your project contains multiple agents, you can specify one by name.

### Invoke an agent (local or remote)

Use `azd ai agent invoke` to send a message.

- By default, `invoke` targets the remote Azure AI Foundry endpoint.
- Add `--local` to talk to a locally running agent.

```bash
azd ai agent run                 # Start the agent locally
azd ai agent run my-agent         # Start a specific agent by name
azd ai agent invoke "Summarize this doc"  # Send a message to the remote endpoint
azd ai agent invoke "Hello" --local       # Send a message to a locally running agent
```

Session and conversation identifiers persist across invocations, allowing multi-turn conversations without extra flags.

## Try it out

The commands are available in the `azure.ai.agents` extension **v0.1.14-preview**.

Upgrade an existing installation:

```bash
azd extension upgrade azure.ai.agents
```

If you’re new to `azd`, install it and initialize an agent project:

- Install: https://learn.microsoft.com/azure/developer/azure-developer-cli/install-azd
- Initialize: `azd ai agent init`

## Feedback and references

- Report issues or start discussions: https://github.com/Azure/azure-dev
- User research signup: https://aka.ms/azd-user-research-signup
- Feature PR: https://github.com/Azure/azure-dev/pull/7026
- Contributor: Travis Angevine (https://github.com/trangevi)


[Read the entire article](https://devblogs.microsoft.com/azure-sdk/azd-ai-agent-run-invoke/)

