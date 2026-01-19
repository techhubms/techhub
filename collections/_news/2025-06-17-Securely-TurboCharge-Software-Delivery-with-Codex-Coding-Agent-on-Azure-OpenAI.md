---
external_url: https://devblogs.microsoft.com/all-things-azure/securely-turbo%e2%80%91charge-your-software-delivery-with-the-codex-coding-agent-on-azure-openai/
title: Securely Turbo‑Charge Software Delivery with Codex Coding Agent on Azure OpenAI
author: Govind Kamtamneni
viewing_mode: external
feed_name: Microsoft DevBlog
date: 2025-06-17 22:54:11 +00:00
tags:
- Agents
- All Things Azure
- App Development
- Automation
- Azure AI Foundry
- Azure OpenAI
- CI/CD
- CLI
- Codex
- Codex Mini
- Coding Agent
- Config.json
- Developer Productivity
- Entra ID
- GitHub Actions
- Node.js
- OpenAI
- Private Networking
- Refactoring
section_names:
- ai
- azure
- coding
- devops
- security
---
In this detailed guide, Govind Kamtamneni explains how to run the Codex Coding Agent securely on Azure OpenAI. He covers setup, enterprise security benefits, automation via GitHub Actions, and troubleshooting tips.<!--excerpt_end-->

# Securely Turbo‑Charge Software Delivery with Codex Coding Agent on Azure OpenAI

*By Govind Kamtamneni*

## Introduction

We have contributed several pull requests to add Azure OpenAI support to **Codex**, enabling users to enjoy a similar Codex experience to ChatGPT while running securely within the Azure environment:

- [PR #769](https://github.com/openai/codex/pull/769)
- [PR #1324](https://github.com/openai/codex/pull/1324)
- [PR #1321](https://github.com/openai/codex/pull/1321)
- [PR #92](https://github.com/openai/codex/pull/92) (work-in-progress for Entra ID token-based authentication)

We continue to collaborate with OpenAI on future updates and integrations.

Until OpenAI publishes a new [NPM release](https://www.npmjs.com/package/@openai/codex?activeTab=versions), you should [build Codex from source](https://github.com/openai/codex?tab=readme-ov-file#installation).

## What is Codex CLI?

The [Codex CLI](https://github.com/openai/codex) is a coding agent that powers ChatGPT’s Codex features. You can now run Codex *entirely on Azure infrastructure*, offering these advantages:

- Enterprise-grade security
- Private networking
- Role-based access control
- Predictable cost management
- Data retention within compliance boundaries

Codex is more than a chat assistant — it's an asynchronous coding agent that can be triggered from your terminal or CI pipeline (e.g., GitHub Actions). It automates PR creation, refactoring, and test writing with the credentials of your Azure OpenAI deployment. The tool supports use cases such as language translation, data-to-code, and legacy migration ([more info](https://openai.com/index/introducing-codex/)).

To further expand possibilities, [gitagu.com](https://gitagu.com) offers tools to configure repositories for Codex and browse other Azure-hosted coding agents like GitHub Copilot Coding Agent, Cognition Devin, and SRE Agent.

## Prerequisites

- **Azure subscription** with access to Azure OpenAI
- Contributor permissions in [Azure AI Foundry](https://ai.azure.com)
- Compatible OS: macOS, Linux, or Windows 11 with WSL 2
- Node.js 18+ and npm

## Step 1: Deploy a Codex Model in Azure AI Foundry

1. Visit Azure AI Foundry ([ai.azure.com](https://ai.azure.com)) and create a new project.
2. Select a reasoning model (e.g., [`codex-mini`](https://devblogs.microsoft.com/foundry/codex-mini-fast-scalable-code-generation-for-the-cli-era/), `o4-mini`, or `o3`).
3. Deploy the model, naming it as desired (deployment takes ~2 minutes).
4. Copy the **Endpoint URL** and generate an **API key**.

## Step 2: Install the Codex CLI

```bash
npm install -g @openai/codex
export NODE_NO_WARNINGS=1 # Optional: reduce verbosity
codex --version # Verify installation
```

## Step 3: Configure Codex for Azure

Edit your `~/.codex/config.json`:

```json
{
  "model": "codex-mini",
  "provider": "azure",
  "providers": {
    "azure": {
      "name": "AzureOpenAI",
      "baseURL": "https://<your-resource>.cognitiveservices.azure.com/openai",
      "envKey": "AZURE_OPENAI_API_KEY"
    }
  },
  "history": {
    "maxSize": 1000,
    "saveHistory": true,
    "sensitivePatterns": []
  }
}
```

Export credentials (Unix, macOS, or WSL 2):

```bash
export AZURE_OPENAI_API_KEY="<your-api-key>"

# Work-around for Codex bug – also set

export OPENAI_API_KEY="$AZURE_OPENAI_API_KEY"
```

## Step 4: Use the Coding Agent

You can invoke Codex as an agent for Azure tasks:

```bash
codex -p azure
```

Examples:

- Generate a unit test: `# generate a unit test for src/utils/date.ts`
- Code translation: `# convert this Java class to Python`

## Step 5: Integrate Codex with GitHub Actions

Codex can execute as part of your CI/CD pipeline. For example, to automate code refactoring with GitHub Actions:

Store your API key as `AZURE_OPENAI_API_KEY` in GitHub Secrets. Add a job like:

```yaml
jobs:
  codex_refactor:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Run Codex agent
        run: |
          npm install -g @openai/codex
          export AZURE_OPENAI_API_KEY=${{ secrets.AZURE_OPENAI_API_KEY }}
          export OPENAI_API_KEY=${{ secrets.AZURE_OPENAI_API_KEY }}
          codex -p azure "# refactor the authentication module for clarity"
```

## Step 6: Explore More Agents with Gitagu

- Browse documentation and benchmarks for Azure-hosted agents
- One-click repo-ready configuration guides
- Experiment with agents: GitHub Copilot Coding Agent, Cognition Devin, SRE Agent, and others ([gitagu.com](https://gitagu.com))

## Troubleshooting

| Symptom | Fix |
| --- | --- |
| `401 Unauthorized` / `403 Forbidden` | Ensure both `AZURE_OPENAI_API_KEY` and `OPENAI_API_KEY` are exported with the same value; verify API key has deployment access. |
| `ENOTFOUND`, DNS error, or `404 Not Found` | Verify `baseURL` in config matches your resource and domain. Some tenants require different Azure domains, e.g. `cognitiveservices.azure.com`. |
| CLI says "no provider found" or Azure settings ignored | Ensure `~/.codex/config.json` is configured correctly with proper provider, envKey, and model/baseURL. |
| "codex-mini" model warning for Azure | This is a known issue under active PR; you can ignore and proceed. |

## Conclusion

Within minutes, you can connect a robust AI coding agent to your Azure tenant, keep your intellectual property secure, and accelerate software delivery. Combining Codex CLI, GitHub Actions, and the Gitagu agent catalog allows for a flexible, AI-powered engineering workflow.

Share your feedback or questions in the comments.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/all-things-azure/securely-turbo%e2%80%91charge-your-software-delivery-with-the-codex-coding-agent-on-azure-openai/)
