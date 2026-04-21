---
primary_section: github-copilot
external_url: https://blog.fabric.microsoft.com/en-US/blog/agentic-fabric-how-mcp-is-turning-your-data-platform-into-an-ai-native-operating-system/
feed_name: Microsoft Fabric Blog
date: 2026-04-21 14:00:00 +00:00
title: 'Agentic Fabric: How MCP is turning your data platform into an AI-native operating system'
section_names:
- ai
- devops
- github-copilot
- ml
- security
author: Microsoft Fabric Blog
tags:
- Agentic Automation
- AI
- Audit Logs
- CI/CD
- Copilot Studio
- Data Warehouse
- DevOps
- Fabric CLI
- Fabric Local MCP
- Fabric Remote MCP
- Fabric REST API
- Git Integration
- GitHub Copilot
- Lakehouse
- MCP
- MCP Server
- Microsoft Entra ID
- Microsoft Fabric
- Microsoft Teams
- ML
- News
- Npx
- OAuth2
- OneLake
- OpenAPI Specification
- RBAC
- Security
- Service Principal Authentication
- VS Code
- VS Code Extension
- Workspace Management
---

Microsoft Fabric Blog (co-authored with Jeremy Hoover) introduces Fabric’s new MCP servers—Local (GA) and Remote (Preview)—so AI assistants like GitHub Copilot and Copilot Studio can discover Fabric APIs, operate on OneLake, and run authenticated workspace/item actions with Entra ID, RBAC, and audit logging.<!--excerpt_end-->

# Agentic Fabric: How MCP is turning your data platform into an AI-native operating system

Co-author: Jeremy Hoover

Developers are starting to interact with data platforms through AI agents—not just portals, menus, or bespoke REST API integrations. The post introduces the **Model Context Protocol (MCP)** and explains how Microsoft is bringing MCP to **Microsoft Fabric** via two MCP server offerings.

## What MCP is

The **Model Context Protocol (MCP)** is an open standard (created by Anthropic) that gives AI agents a universal way to discover and operate external systems through one protocol, instead of building custom integrations (auth flows, API wrappers, tooling glue) for every tool and service.

- MCP spec: [Model Context Protocol (MCP)](https://modelcontextprotocol.io/)

![Figure: The Microsoft MCP Server extension in VS Code is now available and ready to install.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/04/figure-the-microsoft-mcp-server-extension-in-vs-c.png)

## What Microsoft is shipping for Fabric

Microsoft describes two milestones for Fabric:

- **Fabric Local MCP (Generally Available)**: an open-source server that gives AI assistants deep knowledge of Fabric APIs, enables local-to-cloud data operations, and can act as an execution layer through the **Fabric CLI**.
- **Fabric Remote MCP (Preview)**: a cloud-hosted server that lets AI agents perform authenticated operations in your Fabric environment without local setup.

Different usage contexts:

- A developer pair-programming with **GitHub Copilot** can use **Local MCP** to generate grounded code and move data between their machine and **OneLake**.
- An autonomous agent built in **Copilot Studio** can use **Remote MCP** to manage workspaces and permissions for a team.
- A CI/CD pipeline can use the **Fabric CLI**, wrapped as MCP tools, to deploy changes.

## Why MCP now

The post frames MCP as a “universal connector” (USB analogy) for AI tools:

- Instead of unique integrations per AI client, expose your platform as an MCP server.
- Any MCP-compatible client (e.g., GitHub Copilot, Claude, Cursor) can connect.
- It reduces integration plumbing such as OAuth2/token management, rate limiting logic, and API versioning work for each agent.

## Summary: Local vs Remote MCP

| Area | Fabric Local MCP (GA) | Fabric Remote MCP (Preview) |
| --- | --- | --- |
| What it is | Open-source server running on your machine | Cloud-hosted server |
| Purpose | API knowledge, code generation, OneLake file operations, CLI execution | Real operations: workspaces, items, permissions, connections |
| Best for | Developers pair-programming with AI assistants | Agents/automation tools, Copilot Studio bots |
| Install | VS Code extension or `npx @microsoft/fabric-mcp` | Add URL in VS Code (no install) |
| Auth | Local credentials | **Entra ID** |
| Works with | VS Code, Claude, Cursor, any MCP client | VS Code, Copilot Studio, Claude, Cursor, any MCP client |

- Local overview: https://learn.microsoft.com/rest/api/fabric/articles/mcp-servers/pro-dev-local/overview-local-mcp-server
- Remote overview: https://learn.microsoft.com/rest/api/fabric/articles/mcp-servers/core-remote/overview-core-mcp-server

## Fabric Local MCP (Generally Available)

Local MCP runs on your machine and targets two main goals:

1. Help you **build on top of Fabric** by making Fabric’s API surface and best practices available to an AI assistant.
2. Enable **local-to-cloud data operations** with OneLake.

### API documentation and best practices tools

These tools let an agent access Fabric’s API surface *without connecting to your environment*:

| Tool | Functions |
| --- | --- |
| `docs_workloads` | Lists Fabric workload types that have public APIs |
| `docs_api_spec` | Retrieves full OpenAPI spec for a workload |
| `docs_platform_api_spec` | Retrieves platform API spec (workspaces, items, etc.) |
| `docs_item_definition` | Retrieves JSON schema definitions for item types |
| `docs_best_practices` | Guidance for pagination, error handling, retry logic |
| `docs_examples` | Example API requests and responses |

The intended outcome is **grounded code generation**: the agent generates code from the current API spec rather than relying on model training data snapshots.

### OneLake: local-to-cloud data management tools

These tools connect directly to **OneLake**:

| Tool | Functions |
| --- | --- |
| `onelake_list_workspaces` | Lists available Fabric workspaces |
| `onelake_list_items` | Lists workspace items + high-level metadata |
| `onelake_list_files` | Lists files via hierarchical file-list endpoint |
| `onelake_upload_file` | Upload a file to OneLake |
| `onelake_download_file` | Download a OneLake file |
| `onelake_create_directory` / `onelake_delete_directory` | Manage OneLake directories |
| `onelake_list_tables` | Lists tables published within a namespace |
| `onelake_get_table` | Retrieves definition for a table |

The post also mentions additional tooling for DFS-level item listing, table namespace discovery, and table configuration.

### Core Fabric item operations

| Tool | Functions |
| --- | --- |
| `core_create_item` | Creates Fabric items (Lakehouses, Notebooks, etc.) |

### What’s new (Local MCP)

- Integrated authentication (no manual token management)
- Error handling + auto-retry for transient failures
- Production support under standard Microsoft support policies
- Telemetry + diagnostics for tool usage/performance visibility

### Get started (Local MCP)

**Recommended (VS Code extension)**

- Marketplace: [Fabric MCP extension for VS Code](https://marketplace.visualstudio.com/items?itemName=fabric.vscode-fabric-mcp-server)

**Manual configuration (any MCP client)**

```json
{
  "mcpServers": {
    "fabric": {
      "command": "npx",
      "args": ["-y", "@microsoft/fabric-mcp"]
    }
  }
}
```

- GitHub repo: [Fabric Local MCP on GitHub](https://github.com/microsoft/mcp/tree/main/servers/Fabric.Mcp.Server)

## Fabric Remote MCP (Preview)

Remote MCP is a cloud-hosted server that enables AI agents to perform real, authenticated operations in your Fabric environment with no local install.

- Endpoint: `https://api.fabric.microsoft.com/v1/mcp/core`

### How it works (security model)

Requests flow through:

- **Entra ID authentication**
- **RBAC** enforcement (agent operates with the signed-in user identity and permissions)
- **Fabric Audit Logs** for action tracking and admin visibility

### Agent capabilities (Remote MCP)

| Category | Capabilities | Status |
| --- | --- | --- |
| Workspaces | CRUD + listing with filtering | Available |
| Items | Full CRUD; get definitions (notebooks, reports, semantic models) | Available |
| Search | Search items across workspaces | Available |
| Permissions | Manage workspace/item role assignments | Available |
| Connections | Create/view/update connections; discover gateways | Available |
| OneLake | Schema/table/column listing; file operations | Rolling out during preview |
| Safety controls | `is_consequential` flags, dependency warnings | Rolling out during preview |

### Pro tip: compose multiple MCP servers

Examples in the post:

- Fabric MCP + Microsoft Graph MCP server for identity resolution: [Microsoft MCP Server for Enterprise](https://learn.microsoft.com/graph/mcp-server/get-started)
- Fabric MCP + GitHub MCP server for Git-to-Fabric automation: [GitHub MCP](https://github.com/github/github-mcp-server)
- Fabric MCP in Copilot Studio to operate Fabric from Teams

### Quick setup (Remote MCP)

1. VS Code: **Cmd+Shift+P** → “MCP: Add Server” → choose HTTP
2. Enter URL: `https://api.fabric.microsoft.com/v1/mcp/core`
3. Sign in via browser
4. Try: “List all my Fabric workspaces”

## What’s possible (example scenarios)

### Developer: generate correct code

Prompt example:

> “I need a Python script that reads from my lakehouse, transforms the data, and loads it into a warehouse. Show me the right APIs.”

Local MCP is positioned as the way to:

- query OpenAPI specs
- check best practices (pagination/error handling)
- generate code grounded in those specs
- upload test data via OneLake tools

### Team lead: bootstrap a project

Prompt example:

> “Create a new workspace called ‘Q2-Analytics’, add a lakehouse, upload these CSV files, and give read access to the analytics team.”

Remote MCP enables:

- workspace and lakehouse creation
- file upload to OneLake
- role assignment
- all within identity/permissions and audited

### Platform engineer: automate deployments with Fabric CLI

Example commands shown:

```bash
fab create Q2-Analytics.Workspace -P capacityname=$CAPACITY
fab create Q2-Analytics.Workspace/raw-data.Lakehouse
fab deploy –config deployment-config.yaml
```

### Organization: operate Fabric from Teams

Prompt example:

> “@FabricBot create a new workspace for the Q3 marketing campaign and add the marketing team as contributors.”

The pattern described:

- build a custom agent in **Copilot Studio**
- connect it to **Fabric Remote MCP**
- deploy it to **Microsoft Teams**

## Security by design

Security controls called out:

- RBAC enforcement (no elevation beyond the user)
- Full audit trail via Fabric Audit Logs
- `is_consequential` flags and confirmation on destructive actions
- No bulk exfiltration via Remote MCP (metadata/schema only); bulk data via OneLake boundaries
- Admin controls via existing tenant settings

## Future directions and objectives

Features being explored:

- Service principal auth for headless automation
- Deployment pipelines + Git integration via MCP
- Job scheduling/monitoring
- Domain/folder management
- OneLake shortcuts + data access security
- Dry run/simulation mode
- Multi-workspace operations

Questions under consideration include what a fully agentic data platform looks like and what safety controls are required for production.

## Links

- Local GA: [Get started with Fabric Pro-Dev MCP Server](https://learn.microsoft.com/rest/api/fabric/articles/mcp-servers/pro-dev-local/get-started-local)
- Remote Preview: [Get started with Fabric Core MCP Server](https://learn.microsoft.com/rest/api/fabric/articles/mcp-servers/core-remote/get-started-core?tabs=vscode)
- [Fabric Local MCP on GitHub](https://github.com/microsoft/mcp/tree/main/servers/Fabric.Mcp.Server)
- [MCP Protocol Specification](https://modelcontextprotocol.io/)
- [Anthropic: Code Execution with MCP](https://www.anthropic.com/engineering/code-execution-with-mcp)
- [Cloudflare: Code Mode MCP](https://blog.cloudflare.com/code-mode-mcp/)
- [Introduction blog post: Introducing Fabric MCP](https://blog.fabric.microsoft.com/blog/introducing-fabric-mcp-public-preview)
- Feedback form: [Let us know](https://forms.office.com/r/awULEDYwxk)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/agentic-fabric-how-mcp-is-turning-your-data-platform-into-an-ai-native-operating-system/)

