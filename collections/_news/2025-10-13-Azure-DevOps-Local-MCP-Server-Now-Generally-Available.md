---
external_url: https://devblogs.microsoft.com/devops/azure-devops-local-mcp-server-generally-available/
title: Azure DevOps Local MCP Server Now Generally Available
author: Dan Hellem
feed_name: Microsoft DevOps Blog
date: 2025-10-13 11:00:46 +00:00
tags:
- Agile
- Authorization
- Azure DevOps
- Community Feedback
- Developer Tools
- GitHub Copilot
- GitHub Copilot Integration
- LLM Integration
- Local Deployment
- MCP Server
- Model Context Provider
- Open Source
- Prompt Engineering
- Tooling
- VS Code
- AI
- Azure
- DevOps
- News
section_names:
- ai
- azure
- devops
primary_section: ai
---
Dan Hellem introduces the general availability of the Azure DevOps local MCP Server, highlighting its features, local privacy benefits, and integration steps for developers.<!--excerpt_end-->

# Azure DevOps Local MCP Server Now Generally Available

**Author: Dan Hellem**

The local MCP (Model Context Provider) Server for Azure DevOps has reached general availability. This server bridges your AI assistant—such as GitHub Copilot—and your Azure DevOps organization, injecting real-time context into the prompts processed by large language models (LLMs).

Key improvements since preview include:

- Enhanced login and authorization
- Improved and expanded tooling
- Introduction of domains, enabling scoped tool usage to stay under client limits

## What Is a Local MCP Server?

A local MCP Server operates within your internal network or development environment, enabling secure, private access to Azure DevOps data such as work items, wikis, repos, searches, and test plans. Unlike remote or hosted MCP Servers, this local solution keeps your data on-premises for improved security.

The server is deeply integrated with tools like GitHub Copilot, providing tailored, context-rich suggestions and results for developers within Azure DevOps environments.

## General Availability — What Does It Mean?

For this open-source project, general availability means more stable releases and careful update management. Community engagement, issue tracking, and feature contributions will remain high priorities.

## Getting Started

1. Visit the [Azure DevOps MCP Server repository](https://github.com/microsoft/azure-devops-mcp).
2. Open Visual Studio Code.
3. [Copy and paste the configuration](https://github.com/microsoft/azure-devops-mcp/blob/main/README.md#steps) into your `.vscode\mcp.json` file.
4. Start the MCP Server and use the available tools.

For full guidance, refer to the [installation and setup instructions](https://github.com/microsoft/azure-devops-mcp/blob/main/README.md), as well as the [Getting Started](https://github.com/microsoft/azure-devops-mcp/blob/main/docs/GETTINGSTARTED.md) and [Troubleshooting](https://github.com/microsoft/azure-devops-mcp/blob/main/docs/TROUBLESHOOTING.md) guides. You can also review [example scenarios](https://github.com/microsoft/azure-devops-mcp/blob/main/docs/EXAMPLES.md) for practical use cases.

## What's Next?

The team will continue to collect feedback, welcome community contributions, and develop new tools to extend the server's capabilities. Developers are encouraged to report issues or suggest features via the project's [GitHub Issues](https://github.com/microsoft/azure-devops-mcp/issues) page.

---

For more information, ongoing updates, and full documentation, visit the [Azure DevOps Blog](https://devblogs.microsoft.com/devops) and the [project repository](https://github.com/microsoft/azure-devops-mcp).

This post appeared first on "Microsoft DevOps Blog". [Read the entire article here](https://devblogs.microsoft.com/devops/azure-devops-local-mcp-server-generally-available/)
