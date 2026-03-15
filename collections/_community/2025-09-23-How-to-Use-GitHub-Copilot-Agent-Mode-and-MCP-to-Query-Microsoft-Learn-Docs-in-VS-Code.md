---
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/use-copilot-and-mcp-to-query-microsoft-learn-docs/ba-p/4455835
title: How to Use GitHub Copilot Agent Mode and MCP to Query Microsoft Learn Docs in VS Code
author: carlottacaste
feed_name: Microsoft Tech Community
date: 2025-09-23 07:00:00 +00:00
tags:
- Agent Mode
- AI Assistant
- API Integration
- Copilot Studio
- Developer Productivity
- Extension Installation
- Grounded Answers
- MCP
- Microsoft Documentation
- Microsoft Learn Docs
- Prompt Engineering
- Python
- VS Code
- AI
- Azure
- GitHub Copilot
- Community
- .NET
section_names:
- ai
- azure
- dotnet
- github-copilot
primary_section: github-copilot
---
carlottacaste explains how to leverage GitHub Copilot in Agent Mode with MCP servers to query Microsoft Learn Docs right inside VS Code. Learn practical setup steps, prompt file configuration, and how to create your own Python MCP client.<!--excerpt_end-->

# How to Use GitHub Copilot Agent Mode and MCP to Query Microsoft Learn Docs in VS Code

In this guide, carlottacaste walks you through integrating GitHub Copilot in Agent Mode with MCP (Model Context Protocol) servers to obtain reliable, grounded answers from Microsoft Learn Docs—directly within your coding environment.

## Why Use Copilot with MCP Servers?

- Get trustworthy answers based only on up-to-date Microsoft documentation.
- Avoid context-switching and manual searching by bringing the docs into your workspace.
- Ensure your AI assistant’s responses are grounded and reputable, reducing the risk of outdated or inaccurate information.

## What is MCP (Model Context Protocol)?

[MCP](https://modelcontextprotocol.io/docs/getting-started/intro) is a standardized protocol enabling AI tools to connect with external tools, data, and resources in a unified way. The architecture consists of:

- **Hosts:** AI environments (VS Code, Copilot Studio) initiating data access.
- **Clients:** Protocol clients like GitHub Copilot handling connections.
- **Servers:** Lightweight programs exposing tools, resources, or prompts (like the Microsoft Docs MCP Server).

Discover available MCP servers, including the official Microsoft Docs MCP Server: [https://code.visualstudio.com/mcp](https://code.visualstudio.com/mcp)

## Installing and Using the Microsoft Docs MCP Server in VS Code

1. **Locate the Server:** Visit the [MCP server list](https://code.visualstudio.com/mcp) and find the [Microsoft Docs MCP Server](https://github.com/microsoftdocs/mcp).
2. **Install:** Click 'Install' to add it to your VS Code instance.
3. **Activate Agent Mode:** Open GitHub Copilot chat, switch to Agent Mode, and select your preferred model (such as GPT-5).
4. **Enable the Server:** Ensure the MCP server is listed and activated under tools.
5. **Query Microsoft Learn Docs:** Enter a question—e.g., “Does Azure AI Foundry offer a Python SDK?”—and Copilot will surface grounded results from Microsoft Learn Docs. When prompted, allow Copilot to use the 'microsoft_docs_search' tool offered by the MCP server.

## Ground Azure Queries Using Copilot Prompt Files

- Use Copilot Agent Mode's 'Prompt Files' feature to ensure your Microsoft questions use the Microsoft Docs MCP server.
- Create or edit a markdown prompt file (like `msdocs-assistant.prompt.md`) with clear instructions (e.g., "Always use Microsoft Docs MCP Server for Microsoft queries").
- Set prompt files at user or workspace scope for flexible configuration.

## Advanced: Building a Custom Python MCP Client

- For advanced scenarios, craft a custom Python client to interact directly with the Microsoft Docs MCP server over HTTP.
- Steps include:
  1. Initiate a Copilot chat and configure the model.
  2. Have Copilot or yourself draft code to query the MCP server and perform a test call.
  3. Set up a Python virtual environment.
  4. Install dependencies with `pip install "mcp[cli]"`.
  5. Run and test your client.
  6. Optionally, commit your code to GitHub for source control.

## Further Learning and Resources

- [Video tutorial series on using Copilot Agent Mode and MCP](#)
- [Instruction files and example MCP client code on GitHub](https://github.com/carlotta94c/ms-docs-mcp-client) (All files generated with GitHub Copilot.)
- [MCP for Beginners course](https://aka.ms/mc-for-beginners)

---

**Author**: carlottacaste  ([Microsoft Developer Community](https://techcommunity.microsoft.com/t5/user/viewprofilepage/user-id/1310161))

## Key Takeaways

- Integrate Copilot Agent Mode with MCP servers for on-demand, grounded Microsoft documentation.
- Automate best practices for trusted answers with prompt files.
- Build custom MCP clients for advanced queries and flexibility.

Use these strategies to streamline your Azure and Microsoft tech development workflow and increase confidence in AI-powered coding assistance.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/use-copilot-and-mcp-to-query-microsoft-learn-docs/ba-p/4455835)
