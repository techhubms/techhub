---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/host-declarative-markdown-based-agents-on-azure-functions/ba-p/4496038
title: Hosting Declarative Markdown-Based Agents on Azure Functions with GitHub Copilot SDK
author: AnthonyChu
primary_section: github-copilot
feed_name: Microsoft Tech Community
date: 2026-02-20 00:42:26 +00:00
tags:
- AGENTS.md
- AI
- Autonomous Agents
- Azure
- Azure Developer CLI
- Azure Functions
- Cloud Agents
- Community
- Copilot CLI
- Event Triggers
- Function App
- GitHub Copilot
- GitHub Copilot SDK
- HTTP API
- MCP
- Python Tools
- REST Endpoints
- Serverless
- VS Code
- .NET
section_names:
- ai
- azure
- dotnet
- github-copilot
---
AnthonyChu introduces an experimental method for developers to deploy markdown-based Copilot agents on Azure Functions using the GitHub Copilot SDK, offering end-to-end cloud workflow and seamless agent collaboration.<!--excerpt_end-->

# Hosting Declarative Markdown-Based Agents on Azure Functions with GitHub Copilot SDK

**Author:** AnthonyChu

## Overview

Developers at Microsoft and the Azure Functions team now have an experimental way to host local, markdown-configured agents as cloud services using Azure Functions. This approach leverages the GitHub Copilot SDK and Model Context Protocol (MCP) for scalable and collaborative AI-driven workflows.

## From Local Development to Cloud Deployment

- **Agent Structure:**
    - Agents are defined using files like `AGENTS.md`, custom skills in `.github/skills/`, and MCP configurations in `.vscode/mcp.json`.
    - Tools can be added as simple Python files in `src/tools/` without requiring knowledge of the Azure Functions programming model.

- **Local Experience:**
    - Agents can be run using VS Code or the Copilot CLI with the same folder structure required for deployment.

## Fast Deployment with Azure Functions

- **Seamless Cloud Hosting:**
    - Deployment mirrors the local experience.
    - Use the Azure Developer CLI (`azd up`) from the project root to package and deploy your agent as a Function App.

- **Automatic Resource Management:**
    - Azure sets up necessary resources such as a file share for agent chat history, ensuring session persistence even with function scaling.

- **Built-in Test UI:**
    - Once deployed, the Function App provides a browser-based chat UI at the root URL for direct testing.

## Powered by GitHub Copilot SDK and MCP

- **Project Structure Awareness:**
    - The GitHub Copilot SDK natively recognizes agent project structures and exposes them as HTTP APIs and MCP servers.

- **Endpoints:**
    - `/agent/chat` - Main REST endpoint for agent communication (supports streaming SSE).
    - `/runtime/webhooks/mcp` - MCP server endpoint, supporting integration back into local VS Code for hybrid workflows.

## Extensibility with Python Tools

- **No Framework Knowledge Required:**
    - Developers can add custom functionalities by writing standard async Python functions (e.g., cost estimators) and placing them in the `src/tools/` directory.

- **Dependency Management:**
    - External dependencies are managed via `src/requirements.txt`.

## Event-Driven Agents

- **Beyond Request/Response:**
    - Schedule agent tasks (like status checks or summaries) using timer triggers defined in AGENTS.md frontmatter, turning agents into autonomous background workers.

    ```yaml
    ---
    functions:
      - name: morningBriefing
        trigger: timer
        schedule: "0 0 8 * * *"
        prompt: "Check the latest build status and summarize any new critical issues."
    ---
    ```

## Try It Yourself

- **Get Started:**
    - Explore the demo repository: [copilot-custom-agents-on-azure-functions](https://github.com/vrdmr/copilot-custom-agents-on-azure-functions) for full code samples and step-by-step instructions.
    - Supports models from GitHub and Microsoft Foundry.

- **Feedback:**
    - Community input is encouraged to refine and extend this experimental workflow.

## Key Takeaways

- Declarative agent definition with GitHub Copilot SDK and Markdown.
- Zero-friction transition from local testing to scalable, cloud-based hosting.
- Secure endpoints and event-driven design open new possibilities for team collaboration and automation.

For more details and to contribute feedback, visit the [project repository](https://github.com/vrdmr/copilot-custom-agents-on-azure-functions).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/host-declarative-markdown-based-agents-on-azure-functions/ba-p/4496038)
