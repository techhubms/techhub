---
author: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/give-your-ai-agent-the-keys-to-onelake-onelake-mcp-generally-available/
primary_section: ai
date: 2026-04-16 10:00:00 +00:00
section_names:
- ai
- azure
- dotnet
- ml
tags:
- .NET
- .NET App
- AI
- AI Agents
- Az Login
- Azure
- Azure Authentication
- Blob Storage API
- Data Catalog
- Delta Lake
- Fabric MCP Server
- File System APIs
- KQL Database
- Microsoft Fabric
- Mirrored Database
- ML
- Model Context Protocol (mcp)
- News
- OneLake
- OneLake Access API
- OneLake Table APIs
- OneLake Tools
- Open Mirror
- Parquet
- Replication Health
- Schema Discovery
- Semantic Models
- Tenant Data Plane
- VS Code Extension
- Workspace Discovery
title: 'Give your AI agent the keys to OneLake: OneLake MCP (Generally Available)'
feed_name: Microsoft Fabric Blog
---

Microsoft Fabric Blog introduces the generally available OneLake MCP tools, showing how an AI agent can discover Fabric items, inspect schemas, map OneLake storage, and assess mirrored-database health through a single natural-language conversation (no code or portal clicks).<!--excerpt_end-->

## Give your AI agent the keys to OneLake: OneLake MCP (Generally Available)

Have you ever tried to understand what’s stored in your Fabric items? Would you even know where to begin?

The post demonstrates an example: 92,000 UK property transactions were stored in an open mirrored database. Instead of reading documentation or clicking through the portal, the author prompted an AI agent:

- “Document what’s in the House Price Open Mirror in my UK Property Data workspace.”

With one prompt, the agent:

- Found the workspace
- Discovered the table schema
- Mapped the storage structure
- Assessed database health

![Screenshot of an AI agent response documenting a mirrored database in OneLake, showing record count, compression format, table schema, and storage layout.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/04/screenshot-of-an-ai-agent-response-documenting-a-m.png)

*Figure: An AI agent documents an open mirrored database from a single natural-language prompt — no code, no clicks.*

The agent moved through the hierarchy and APIs:

- Workspace → item → table schema → physical files
- Switching between:
  - Table APIs
  - File APIs

It also:

- Read monitoring files to check replication health
- Measured file sizes to assess optimization

This scenario is enabled by **OneLake tools** in the **Fabric MCP server**:

- OneLake tools: https://learn.microsoft.com/fabric/onelake/onelake-local-mcp
- Fabric MCP server mention (March 2026 feature summary): https://blog.fabric.microsoft.com/blog/fabric-march-2026-feature-summary/#post-34196-_Toc224559595

## How does it work?

[MCP](https://modelcontextprotocol.io/) is described as the language AI uses to talk to the tools it needs.

[OneLake](https://learn.microsoft.com/fabric/onelake/) is positioned as “all your data, in one place”, including:

- Shortcuts
- Mirroring
- Catalog-connected data across your estate into a single logical lake

Fabric items store data in OneLake in open formats, including:

- Lakehouses
- Mirrored databases
- KQL databases
- Semantic models (when OneLake availability is enabled)

The OneLake MCP tools connect to your tenant’s data plane and use:

- Your existing Azure identity
- Your Fabric permissions

Meaning:

- The AI agent can only access what you can access

The tool coverage includes (19 commands total):

- File system APIs (browse/read/write):
  - https://learn.microsoft.com/fabric/onelake/onelake-access-api
- Table APIs (schema and metadata discovery):
  - https://learn.microsoft.com/fabric/onelake/table-apis/table-apis-overview
- Workspace and item discovery (to help agents orient themselves)

References:

- Getting started guide: https://learn.microsoft.com/fabric/onelake/onelake-local-mcp
- Full command reference (README): https://github.com/microsoft/mcp/tree/main/tools/Fabric.Mcp.Tools.OneLake

The post calls out potential scenarios:

- Admin: inventory every item in a workspace
- Data engineer: check table optimization across lakehouses
- Analyst: explore an unfamiliar dataset without writing a query
- Developer: automate these tasks

## How the example works

The database in the example was built using the OneLake API deep dive:

- https://learn.microsoft.com/fabric/onelake/onelake-apis-in-action

Implementation notes from the post:

- A **.NET app** streams UK house price data into an **open mirrored database** via the **Blob Storage API**.
- Fabric converts raw **Parquet** uploads into managed **Delta Lake tables** automatically.

How the agent inspects data:

- Table APIs: understand schema and table metadata
- File APIs: map physical storage, including:
  - The landing zone where data arrives
  - The optimized tables Fabric produces

Because OneLake stores items in open formats, the post emphasizes that an agent can:

- Read table metadata without running queries
- Browse files without opening a notebook

The suggested scale-up scenario:

- Ask an agent to scan every item across one or more workspaces
- Document what exists, size, and health
n
## Get started

The OneLake tools are included in the Fabric MCP server VS Code extension:

- https://marketplace.visualstudio.com/items?itemName=fabric.vscode-fabric-mcp-server

Auth and permissions:

- Uses Azure authentication
- Sign in with `az login`
- Ensure you have access to the Fabric workspaces you want to explore
- No additional roles required beyond normal workspace access

For manual configuration or using MCP outside VS Code:

- Fabric MCP Server README: https://github.com/microsoft/mcp/blob/main/servers/Fabric.Mcp.Server/README.md

## What’s next

The OneLake tools are part of the broader Microsoft MCP initiative:

- https://github.com/microsoft/mcp

Planned/future directions mentioned:

- Tools for setting up and managing shortcuts
- Configuring OneLake security policies
- Enabling OneLake diagnostics for monitoring and troubleshooting

The post asks for feedback and examples, suggesting sharing:

- Automated documentation pipelines
- Governance agents
- Migration workflows

via issues/discussions on the GitHub repo:

- https://github.com/microsoft/mcp

## Learn more

- Use AI agents with OneLake through MCP: https://learn.microsoft.com/fabric/onelake/onelake-local-mcp
- Fabric MCP server VS Code extension: https://marketplace.visualstudio.com/items?itemName=fabric.vscode-fabric-mcp-server
- March 2026 Feature Summary: https://blog.fabric.microsoft.com/blog/fabric-march-2026-feature-summary/#post-34196-_Toc224559595
- Fabric MCP Server README: https://github.com/microsoft/mcp/blob/main/servers/Fabric.Mcp.Server/README.md
- OneLake tools (full command reference): https://github.com/microsoft/mcp/tree/main/tools/Fabric.Mcp.Tools.OneLake
- Open Mirror deep dive: https://learn.microsoft.com/fabric/onelake/onelake-apis-in-action
- OneLake table APIs overview: https://learn.microsoft.com/fabric/onelake/table-apis/table-apis-overview
- Connecting to Microsoft OneLake (Storage API reference): https://learn.microsoft.com/fabric/onelake/onelake-access-api


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/give-your-ai-agent-the-keys-to-onelake-onelake-mcp-generally-available/)

