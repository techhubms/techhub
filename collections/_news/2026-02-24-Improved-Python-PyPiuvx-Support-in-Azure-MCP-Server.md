---
external_url: https://devblogs.microsoft.com/azure-sdk/azure-mcp-server-better-python-support/
title: Improved Python (PyPi/uvx) Support in Azure MCP Server
author: Sandeep Sen
primary_section: github-copilot
feed_name: Microsoft Azure SDK Blog
date: 2026-02-24 16:00:21 +00:00
tags:
- Agentic Workflows
- Agents
- AI
- AI Development
- Automation
- Azure
- Azure AI Search
- Azure Integration
- Azure MCP Server
- Azure SDK
- CI/CD
- CLI
- Cloud Development
- Cosmos DB
- GitHub Copilot
- GitHub Copilot SDK
- IDE Integration
- Key Vault
- MCP
- Microsoft Azure
- News
- PyPI
- Python
- Uvx
- VS Code
- .NET
section_names:
- ai
- azure
- dotnet
- github-copilot
---
Sandeep Sen explains how Python developers can now install and run Azure MCP Server with PyPI and uvx, unlocking seamless Azure integration and support for agentic workflows and GitHub Copilot SDK.<!--excerpt_end-->

# Improved Python (PyPi/uvx) Support in Azure MCP Server

Azure MCP Server now offers first-class Python support, making Azure integration more accessible to Python developers building agentic workflows or AI-powered automation. This update introduces a PyPI package (`msmcp-azure`) and uvx-based on-demand execution for greater flexibility and ease of use.

## Why Python Support Matters

Python plays a pivotal role in AI and data science. The new integration allows developers to leverage over 40 Azure services from within the Python ecosystem, removing the prior runtime barriers (Node.js or .NET) and improving developer productivity.

## Getting Started

### Run Azure MCP Server On-Demand with uvx

To quickly launch Azure MCP Server without permanent installation:

```bash
uvx --from msmcp-azure azmcp server start
```

### Install via pip

To add Azure MCP Server to a Python project or virtual environment:

```bash
pip install msmcp-azure
```

To install a specific version:

```bash
pip install msmcp-azure==<version>
```

#### Prerequisites

- Install [uv](https://docs.astral.sh/uv/getting-started/installation/) for `uvx`
- Python 3.10+ for `pip` installation

## Configuring Your MCP Client for Python

After installation, update your IDE or client configuration:

```json
{
  "mcpServers": {
    "azure-mcp-server": {
      "command": "uvx",
      "args": [ "--from", "msmcp-azure", "azmcp", "server", "start" ]
    }
  }
}
```

Supported environments include VS Code, Visual Studio, IntelliJ, Eclipse, and other MCP-compatible clients. For more, visit the [Azure MCP Server documentation](https://aka.ms/azmcp/docs).

> **Note:** Authenticate to Azure before running the server. See the [Authentication guide](https://github.com/microsoft/mcp/blob/main/docs/Authentication.md).

## When Should You Use uvx or pip?

| Method         | Best For                                                      |
|---------------|---------------------------------------------------------------|
| **uvx**       | Ad-hoc use, always up-to-date. Best for direct MCP server use.|
| **pip**       | Project or virtual environment integration.                   |

## GitHub Copilot SDK Integration

The new Python package integrates with the [GitHub Copilot SDK](https://github.com/github/copilot-sdk). Example configuration for agentic sessions:

```python
import asyncio
from copilot import CopilotClient

async def main():
    client = CopilotClient()
    await client.start()
    session = await client.create_session(
        session_config={
            "mcp_servers": {
                "azure-mcp": {
                    "command": "uvx",
                    "args": ["--from", "msmcp-azure", "azmcp", "server", "start"],
                    "tools": ["*"]
                }
            }
        }
    )
    response = await session.send_message("List my Azure Storage containers")
    print(response.text)
    await client.stop()

if __name__ == "__main__":
    asyncio.run(main())
```

## Capabilities Enabled by Azure MCP Server + Python

- Access to 40+ Azure services (Foundry, AI Search, Cosmos DB, Storage, Key Vault, etc.)
- Seamless IDE integration (VS Code, IntelliJ, Eclipse)
- Multiple server modes (namespace, all, single-tool)
- Read-only mode for safer exploration
- Docker support for CI/CD scenarios

Example prompts:

- "List my Azure Storage containers"
- "Show me my Cosmos DB databases"
- "What indexes do I have in my Azure AI Search service?"
- "Generate Azure CLI commands to create a new resource group"

## Resources

- **PyPI Package:** [msmcp-azure](https://pypi.org/project/msmcp-azure/)
- **GitHub Repo:** [aka.ms/azmcp](https://aka.ms/azmcp)
- **Documentation:** [aka.ms/azmcp/docs](https://aka.ms/azmcp/docs)
- **VS Code Extension:** [aka.ms/azmcp/download/vscode](https://aka.ms/azmcp/download/vscode)
- **Report Issues:** [aka.ms/azmcp/issues](https://aka.ms/azmcp/issues)

## Summary

The Python package for Azure MCP Server, detailed by Sandeep Sen, provides a fast, native, and flexible route for Python developers to leverage Azure’s extensive agentic capabilities and integrate with tools like the GitHub Copilot SDK.

This post appeared first on "Microsoft Azure SDK Blog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-mcp-server-better-python-support/)
