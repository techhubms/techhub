---
feed_name: The GitHub Blog
section_names:
- ai
- azure
- devops
- github-copilot
primary_section: github-copilot
tags:
- Agentic CLI
- AI
- Air Gapped Environments
- Anthropic
- Azure
- Azure OpenAI Service
- Bring Your Own Key (byok)
- Client Apps
- Context Window (128k)
- Copilot
- DevOps
- Environment Variables
- Foundry Local
- GitHub Authentication
- GitHub Code Search
- GitHub Copilot
- GitHub Copilot CLI
- Local Models
- Model Providers
- News
- Offline Mode
- Ollama
- OpenAI Compatible Endpoints
- Streaming
- Sub Agents
- Telemetry Disabled
- Tool Calling
- Vllm
external_url: https://github.blog/changelog/2026-04-07-copilot-cli-now-supports-byok-and-local-models
date: 2026-04-07 14:53:34 +00:00
author: Allison
title: GitHub Copilot CLI now supports BYOK and local models
---

Allison announces an update to GitHub Copilot CLI that adds BYOK provider support (including Azure OpenAI) and a fully offline mode for local models, enabling air-gapped terminal workflows while keeping the same agentic CLI experience.<!--excerpt_end-->

# GitHub Copilot CLI now supports BYOK and local models

GitHub Copilot CLI now lets you connect your own model provider or run fully local models instead of using GitHub-hosted model routing. This enables you to:

- Use models/providers you already pay for
- Operate in air-gapped environments
- Maintain direct control over LLM spend
- Keep the same agentic terminal experience

## Connect any model provider

You can configure Copilot CLI to use a different provider by setting a few environment variables before launching the CLI.

Supported options mentioned include:

- Remote services:
  - Azure OpenAI
  - Anthropic
  - Any OpenAI-compatible endpoint
- Local model runtimes:
  - Ollama
  - vLLM
  - Foundry Local

Setup guide: Using your own LLM models in GitHub Copilot CLI

- https://docs.github.com/copilot/how-tos/copilot-cli/customize-copilot/use-byok-models

## Offline mode for air-gapped environments

To prevent Copilot CLI from contacting GitHub servers, set:

```bash
COPILOT_OFFLINE=true
```

In offline mode:

- All telemetry is disabled
- The CLI only communicates with your configured provider

Combined with a local model, this supports fully air-gapped development workflows.

Docs: Running in offline mode

- https://docs.github.com/copilot/how-tos/copilot-cli/customize-copilot/use-byok-models#running-in-offline-mode

## GitHub authentication is now optional

When using your own model provider:

- GitHub authentication is not required
- You can start using Copilot CLI immediately using only your provider credentials

If you also sign in to GitHub, you can use your preferred model for AI responses while also accessing GitHub-backed features such as:

- `/delegate`
- GitHub Code Search
- GitHub MCP server

Docs: Unauthenticated use

- https://docs.github.com/copilot/how-tos/copilot-cli/set-up-copilot-cli/authenticate-copilot-cli#unauthenticated-use

## What you need to know

- Your model must support:
  - Tool calling
  - Streaming
- For best results, use a model with at least a **128k token context window**.
- Built-in sub-agents (explore, task, code-review) automatically inherit your provider configuration.
- If your provider configuration is invalid, Copilot CLI shows actionable error messages and **does not silently fall back** to GitHub-hosted models.
- Run this for quick setup instructions:

```bash
copilot help providers
```

## Discussion

Join the discussion in GitHub Community:

- https://github.com/orgs/community/discussions/categories/copilot


[Read the entire article](https://github.blog/changelog/2026-04-07-copilot-cli-now-supports-byok-and-local-models)

