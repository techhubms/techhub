---
external_url: https://devblogs.microsoft.com/azure-sdk/introducing-the-azure-mcp-server/
title: 'Introducing the Azure MCP Server: Empowering AI Agents with Azure Services'
author: Rohit Ganguly
feed_name: Microsoft DevBlog
date: 2025-04-17 19:59:42 +00:00
tags:
- Agents
- AI Agents
- App Configuration
- Azure CLI
- Azure MCP Server
- Azure SDK
- Azure Services
- Azure Storage
- Cosmos DB
- Log Analytics
- MCP
- MCP SDK
- Open Source
- Public Preview
- Semantic Kernel
- VS Code
section_names:
- ai
- azure
- coding
- github-copilot
primary_section: github-copilot
---
Rohit Ganguly introduces the Azure MCP Server, an open-source platform in public preview that connects AI agents like GitHub Copilot Agent Mode to Azure services. This post details supported capabilities, practical integrations, and what developers can expect next.<!--excerpt_end-->

## Introducing the Azure MCP Server: Empowering AI Agents with Azure Services

**Author:** Rohit Ganguly  
**Source:** [Azure SDK Blog](https://devblogs.microsoft.com/azure-sdk/introducing-the-azure-mcp-server/)

We’re thrilled to announce the Public Preview of the Azure MCP Server, bringing the power of Azure to your AI agents. The Azure MCP Server allows AI agents to make use of Azure resources for essential workflows including file storage, database and logs querying, and executing CLI commands.

### What is MCP, and Why an Azure MCP Server?

MCP (Model Context Protocol) is an open protocol facilitating communication between agents (clients) and external resources (servers), enabling seamless integration between AI systems and data sources, tools, and more, using a “write once” approach. Azure MCP Server acts as a bridge to expose Azure’s cloud services for agentic workloads, allowing context-aware operations on Azure resources.

For comprehensive MCP details, visit the [MCP website](https://modelcontextprotocol.io/introduction) and [MCP GitHub organization](https://modelcontextprotocol.io/introduction).

#### Key Example Integrations

- Querying Azure Cosmos DB using natural language
- Reading and managing files in Azure Storage
- Searching logs in Azure Log Analytics

### Public Preview Release Highlights

The Azure MCP Server supports popular Azure services and tools:

#### Supported Azure Services

- **Azure Cosmos DB:**
  - List accounts, databases, manage containers/items
  - Execute SQL queries
- **Azure Storage:**
  - List accounts, manage blob containers and blobs
  - Query storage tables, retrieve metadata
- **Azure Monitor (Log Analytics):**
  - List workspaces, query logs (KQL), configure monitoring
- **Azure App Configuration:**
  - Manage stores, key-value pairs, labeled configs
- **Azure Resource Groups:**
  - List, manage resource groups

#### Supported Azure Tools

- **Azure CLI:**
  - Execute all CLI commands, with JSON output
- **Azure Developer CLI (azd):**
  - Run azd commands for template discovery, initialization, provisioning, deployment

This broad functionality empowers agents to operate on Azure cloud resources, manage environments, and automate deployments.

### Using the Azure MCP Server

Any agent supporting MCP can interact with Azure MCP Server, including **GitHub Copilot Agent Mode** and custom-built MCP clients.

#### GitHub Copilot Agent Mode Integration

- GitHub Copilot now includes [Agent Mode with MCP support](https://github.blog/news-insights/product-news/github-copilot-agent-mode-activated/) in Visual Studio Code.
- Quick install via the [Azure MCP Server GitHub](https://github.com/Azure/azure-mcp) repository.
- Developers can instruct Copilot to list Cosmos DB or Storage accounts within VS Code Agent Mode.
- For the best Azure development experience:
  - Combine Azure MCP Server data plane features with the [GitHub Copilot for Azure VS Code extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azure-github-copilot) for documentation lookup, resource graph queries, and best practices.

#### Custom MCP Clients and Agents

- Custom agents adhering to the [MCP client specification](https://modelcontextprotocol.io/specification/latest) can use Azure MCP Server.
- Examples:
  - [MCP SDK in Python](https://modelcontextprotocol.io/quickstart/client#python)
  - [MCP SDK in .NET](https://modelcontextprotocol.io/quickstart/client#c)
  - [Semantic Kernel .NET SDK](https://devblogs.microsoft.com/semantic-kernel/integrating-model-context-protocol-tools-with-semantic-kernel-a-step-by-step-guide/)
  - [OpenAI Agents SDK for Python](https://openai.github.io/openai-agents-python/mcp/)
- Basic installation and execution command:

  ```bash
  npx -y @azure/mcp@latest server start
  ```

- The MCP specification allows any conformant client to leverage the Azure MCP Server, lowering integration overhead.

### What’s Next for Azure MCP Server?

Future development plans include:

1. More in-depth agent samples
2. Expanded and improved documentation
3. Additional Microsoft product integrations
4. More Azure service integrations

Feedback, bug reports, and feature requests are welcomed via the [GitHub repository](https://github.com/Azure/azure-mcp/issues).

### Summary

The Azure MCP Server enables agents—such as GitHub Copilot Agent Mode and custom solutions using SDKs or frameworks like Semantic Kernel—to unlock powerful Azure resources through a standardized protocol. The project is open-source, in public preview, and actively evolving based on community input.

Learn more at the [Azure MCP Server GitHub page](https://github.com/Azure/azure-mcp/).

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/introducing-the-azure-mcp-server/)
