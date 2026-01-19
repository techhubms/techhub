---
external_url: https://devblogs.microsoft.com/all-things-azure/claude-code-microsoft-foundry-enterprise-ai-coding-agent-setup/
title: 'Enterprise AI Coding Agent Setup: Claude Code, Microsoft Foundry, Spec Kit, and GitHub Actions'
author: Govind Kamtamneni
viewing_mode: external
feed_name: Microsoft All Things Azure Blog
date: 2025-12-02 01:37:40 +00:00
tags:
- Agent Framework
- All Things Azure
- Azure AI Foundry
- Azure CLI
- Claude Code
- Enterprise AI
- Entra ID
- Fabric Connector
- GitHub Actions
- MCP Tools
- Microsoft Foundry
- Multi Agent System
- Node.js
- Project Context
- Python
- RAG Grounding
- Security Authentication
- Spec Kit
- Structured Development
- Unit Testing
- VS Code Extension
section_names:
- ai
- azure
- coding
- devops
- security
---
Govind Kamtamneni provides an in-depth walkthrough on configuring Claude Code with Microsoft Foundry, integrating Spec Kit for requirements, securing with Entra ID, and automating agent workflows in enterprise-scale AI development.<!--excerpt_end-->

# Enterprise AI Coding Agent Setup: Claude Code, Microsoft Foundry, Spec Kit, and GitHub Actions

Govind Kamtamneni outlines a comprehensive workflow for enterprise AI agent development and management, combining Claude Code CLI, Microsoft Foundry, structured project context, and CI/CD automation.

## Prerequisites

- Azure subscription with Foundry access
- Node.js 18+ (required for Claude Code CLI)
- (Optional) Azure CLI installed and authenticated (`az login`)

---

## 1. Deploying Claude Models in Microsoft Foundry

- Access the Microsoft Foundry portal.
- Navigate to Discover → Models and search for "Claude" models (Opus, Sonnet, Haiku versions).
- Deploy the selected model using default settings.
- Retrieve Target URI and Key for integration.

[More details](https://learn.microsoft.com/en-us/azure/ai-foundry/quickstarts/get-started-code?view=foundry&preserve-view=true&tabs=python%2Cpython2)

---

## 2. Installing Claude Code CLI

```sh
npm install -g @anthropic-ai/claude-code
claude --version
```

Refer to [Claude Code Docs](https://code.claude.com/docs/en/setup) for advanced install options.

---

## 3. Configuring for Foundry

Set environment variables for integration:

- For Bash or WSL:

  ```sh
  export CLAUDE_CODE_USE_FOUNDRY=1
  export ANTHROPIC_FOUNDRY_RESOURCE={resource}
  # (Optionally provide base URL)
  export ANTHROPIC_FOUNDRY_BASE_URL=https://{resource}.services.ai.azure.com
  # Specify model versions
  export ANTHROPIC_DEFAULT_SONNET_MODEL="claude-sonnet-4-5"
  export ANTHROPIC_DEFAULT_HAIKU_MODEL="claude-haiku-4-5"
  export ANTHROPIC_DEFAULT_OPUS_MODEL="claude-opus-4-5"
  ```

- For PowerShell, use `$env:VAR = "..."` syntax.

### Authentication Options

- **Entra ID:**

  ```sh
  az login
  ```

  Uses Azure CLI default credentials.

- **API Key:**
  Set `ANTHROPIC_FOUNDRY_API_KEY` in your environment.

---

## 4. VS Code Extension Integration

- Install Claude Code extension for VS Code.
- Configure environment variables in your workspace settings.

---

## 5. Validation and Testing

- Run `/status` in Claude CLI to confirm connection to Microsoft Foundry and the chosen model deployment.

---

## 6. Project Context with CLAUDE.md

Claude Code loads `CLAUDE.md` files for deeper context:

- Use hierarchical file locations (global, repo, directory).
- Example for Microsoft Agent Framework involves defining overview, tech stack, architecture, commands (local, test, cloud deployment), and current sprint milestones (e.g., RAG grounding, connector integration).
- Demonstrate use of AzureAIAgentClient and AzureCliCredential for secure access.

---

## 7. Exploring Your Project

- Start Claude Code inside your project directory for contextual agent orchestration.
- Use direct commands to interact and query project details.

---

## 8. Spec Kit for Structured Development

- Integrate Spec Kit for translating requirements into architectural plans and actionable tasks, automating the full development flow with CLI commands:
  - `/speckit.constitution`: Coding standards and principles
  - `/speckit.specify`: Requirements gathering
  - `/speckit.plan`: Technical design and dependencies
  - `/speckit.tasks`: Task breakdown
  - `/speckit.implement`: Automated implementation
- Example: Adding a SharePoint MCP tool for document retrieval.

---

## 9. GitHub Actions Automated Workflows

- Generate unit tests for agent code with Claude Code in CI using async fixtures and AzureAIAgentClient mocking.
- PR review automation with Claude Code for context-driven feedback and review.

---

## 10. Agent HQ: Unified Management

- Manage all deployed agents (Foundry, Copilot, custom) through the Agent HQ dashboard in GitHub or Foundry portal.
- Centralized observability, policy management, and deployments.

---

## 11. Monitoring and Security in Foundry

- Operate tab for usage monitoring (token consumption, latency, error rates).
- Control access via Entra ID role assignments and secure deployment credentials.

---

## Troubleshooting Tips

- Resolve common issues: authentication errors, model deployment mismatches, regional constraints, and rate limits.

---

## Resources

- [Claude Code Documentation](https://docs.anthropic.com/en/docs/claude-code)
- [Microsoft Foundry Docs](https://learn.microsoft.com/en-us/azure/ai-foundry/)
- [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/)
- [Spec Kit on GitHub](https://github.com/github/spec-kit)

---

## Summary

This guide enables enterprise teams to set up secure, scalable coding agents using Claude Code, integrated with Microsoft Foundry and CI/CD pipelines, leveraging Spec Kit for structured development and GitHub Actions for automation—all supported by strong authentication and access management via Entra ID.

This post appeared first on "Microsoft All Things Azure Blog". [Read the entire article here](https://devblogs.microsoft.com/all-things-azure/claude-code-microsoft-foundry-enterprise-ai-coding-agent-setup/)
