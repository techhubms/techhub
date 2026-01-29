---
external_url: https://devblogs.microsoft.com/all-things-azure/codex-azure-openai-integration-fast-secure-code-development/
title: Integrating Codex with Azure OpenAI for Secure Automated Coding
author: Govind Kamtamneni
feed_name: Microsoft All Things Azure Blog
date: 2025-06-17 22:54:11 +00:00
tags:
- Agents
- AGENTS.md
- AI Agents
- All Things Azure
- API Integration
- App Development
- Azure AI Foundry
- Azure OpenAI
- CI/CD
- CLI Tools
- Code Automation
- Codex
- Codex Mini
- Coding Agent
- Developer Productivity
- Enterprise Security
- Entra ID
- GitHub Actions
- OpenAI
- Python
- Token Based Authentication
- AI
- Azure
- Coding
- DevOps
- News
section_names:
- ai
- azure
- coding
- devops
primary_section: ai
---
Govind Kamtamneni details how to run OpenAI’s Codex securely on Azure infrastructure, showing developers how to set up Codex CLI with Azure OpenAI, configure advanced agent instructions, and automate coding workflows within enterprise boundaries.<!--excerpt_end-->

# Integrating Codex with Azure OpenAI for Secure Automated Coding

This guide by Govind Kamtamneni shows how to enhance your development workflow by running OpenAI’s Codex coding agent on Azure OpenAI infrastructure. This approach ensures enterprise security, compliance, and private networking for AI-driven code automation.

## Overview

- **Codex on Azure** provides the same experience as ChatGPT’s Codex but securely contained within your Azure tenant.
- Multiple pull requests expanded Codex support for Azure OpenAI, including Entra ID (Azure AD) token-based authentication.
- Further collaboration with OpenAI is ongoing to add more integrations and features.

## Key Features

- **Enterprise-grade Security:** Keep code and data within compliance boundaries using Azure OpenAI.
- **Flexible Integration:** Deploy Codex as a CLI tool or within CI pipelines using GitHub Actions.
- **Customizable Agent Behavior:** Guide Codex with AGENTS.md for personalized and project-specific instructions.
- **Compatible Models:** Use reasoning models like codex-mini, gpt-5, or gpt-5-mini via Azure AI Foundry.

## Prerequisites

- Active Azure subscription with OpenAI access
- Contributor permissions in Azure AI Foundry
- Homebrew or npm for CLI installation

## Step-by-Step Integration

### 1. Deploy a Codex Model in Azure AI Foundry

- Visit [ai.azure.com](https://ai.azure.com)
- Start a new project and select a reasoning model (e.g., codex-mini, gpt-5)
- Deploy and copy the endpoint URL and API key

### 2. Install and Run Codex CLI

Install using Homebrew or npm:

```sh
brew install codex
codex --version # Verify installation
```

Main commands:

- `codex` : Starts the interactive TUI
- `codex exec ...` : Automates tasks non-interactively

### 3. Configure Codex with Azure

Edit `~/.codex/config.toml` with your Azure credentials:

```toml
model = "gpt-5"
model_provider = "azure"
[model_providers.azure]
name = "Azure"
base_url = "https://YOUR_PROJECT_NAME.openai.azure.com/openai"
env_key = "AZURE_OPENAI_API_KEY"
query_params = { api-version = "2025-04-01-preview" }
wire_api = "responses"
```

Export your API key:

```sh
export AZURE_OPENAI_API_KEY="<your-api-key>"
```

### 4. Enhance Codex with AGENTS.md

Codex merges AGENTS.md instructions from various levels (global, project, folder) to tailor agent behavior. For Azure AI Foundry agents, guidance might include:

- Initializing clients with `AIProjectClient` and `DefaultAzureCredential`
- Creating agents, specifying models and tools (like file search, code interpreter)
- Example code for agent creation using the Azure SDK for Python

### 5. Use Codex in Your Development Workflow

- Run commands interactively or automate tasks like generating tests, refactoring code, or integrating with CI pipelines
- Example: `codex "write a python script to create an Azure AI Agent with file search capabilities"`
- Codex can also update change logs automatically during CI/CD

### 6. Run Codex in GitHub Actions

Integrate Codex jobs directly into your CI pipeline, e.g., updating CHANGELOG before release:

```yaml
jobs:
  update_changelog:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Update changelog via Codex
        run: |
          npm install -g @openai/codex
          export AZURE_OPENAI_API_KEY="${{ secrets.AZURE_OPENAI_KEY }}"
          codex -p azure exec --full-auto "update CHANGELOG for next release"
```

### 7. Explore Additional Agents

- Visit [AsyncLoom](https://asyncloom.com) for more Azure-hosted AI agents (GitHub Copilot Coding Agent, Cognition Devin, SRE Agent, etc.)
- Access more guides and agent catalogs for broader automations

## Troubleshooting

| Symptom                                    | Fix                                                              |
|--------------------------------------------|------------------------------------------------------------------|
| 401/403 errors                             | Correctly export `AZURE_OPENAI_API_KEY`. Check access permissions.|
| ENOTFOUND/DNS/404 errors                   | Ensure `base_url` in config is accurate.                        |
| CLI ignores Azure settings                 | Double-check `model_provider`, `[model_providers.azure]`, and env vars. |
| Codex-mini not available for Azure         | Known issue, ignore warning and continue with tasks.            |

## Conclusion

By integrating Codex with Azure OpenAI and configuring secure workflows, you can accelerate coding, automate repetitive tasks, and keep intellectual property secure within your organization’s boundaries. Extensible agent configurations and easy CI integration make this a powerful setup for enterprise AI development.

This post appeared first on "Microsoft All Things Azure Blog". [Read the entire article here](https://devblogs.microsoft.com/all-things-azure/codex-azure-openai-integration-fast-secure-code-development/)
