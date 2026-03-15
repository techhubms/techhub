---
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/mcp-vs-mcp-cli-dynamic-tool-discovery-for-token-efficient-ai/ba-p/4494272
title: 'MCP vs mcp-cli: Dynamic Tool Discovery for Token-Efficient AI Agents'
author: anbonagi
primary_section: ai
feed_name: Microsoft Tech Community
date: 2026-03-06 08:00:00 +00:00
tags:
- Agent Skills
- AI
- AI Agents
- Automation
- Bun
- CLI Tools
- Community
- Context Window
- Dev Tools
- Dynamic Context Discovery
- GitHub
- Integration
- MCP
- Mcp CLI
- Protocol
- Schema
- Server Integration
- Token Efficiency
- Tool Discovery
section_names:
- ai
---
anbonagi presents a comprehensive analysis of the Model Context Protocol (MCP) and mcp-cli tool, highlighting dynamic tool discovery approaches for AI agents and strategies to optimize token usage in real-world integrations.<!--excerpt_end-->

# MCP vs mcp-cli: Dynamic Tool Discovery for Token-Efficient AI Agents

## Introduction

The AI agent development landscape is facing new scaling challenges, particularly with context window bloat when integrating multiple Model Context Protocol (MCP) servers. Traditional static tool loading requires developers to consume extensive tokens just describing tools. This post introduces **mcp-cli**, a lightweight CLI tool for dynamic discovery, and explains the core system design differences that underpin efficient AI agent integrations.

## Part 1: Understanding MCP (Model Context Protocol)

### What is MCP?

The **Model Context Protocol (MCP)** serves as an open standard for connecting AI agents and applications to external tools, APIs, and data sources. It enables:

- **AI Agents** (such as Claude and Gemini) to discover and use tools
- **Tool Providers** to expose their capabilities in a standardized way
- **Seamless integration** across diverse environments

Learn more here: [MCP for Beginners](https://aka.ms/mcp-for-beginners)

### How MCP Works

- **Tool providers (servers)** publish tool definitions and JSON schemas
- **Clients (AI agents)** discover available tools, invoke them, and receive results

Example (GitHub MCP Server):

- `search_repositories`: Search GitHub repositories
- `create_issue`: Create a GitHub issue
- `list_pull_requests`: List open PRs

Each tool comes with a JSON schema describing parameters and requirements.

### The Static Integration Problem

**Traditional MCP workflow:**

1. Load all tool definitions from all servers at startup
2. Send every tool schema to the AI model
3. Invoke tools upon request

**Scaling Issue:**

- Loading 6 MCP servers with 60 tools can consume ~47,000 tokens
- With dynamic discovery, the same is achieved in ~400 tokens (a 99% reduction)

**Problems with Static Loading:**

- Reduced effective context for real work
- Limits on number of MCP servers that can be used
- Higher context compaction and API costs

## Part 2: Enter mcp-cli – Dynamic Context Discovery

### What is mcp-cli?

**mcp-cli** is a single-binary CLI tool (written in Bun) that performs dynamic discovery for MCP servers, fetching only the needed information on demand.

### Static vs. Dynamic: Paradigm Shift

- **Traditional MCP (Static):** Loads all tool schemas at once, overwhelming the model's context window
- **mcp-cli (Dynamic):** Queries only for necessary servers, tools, and schemas as required, greatly reducing token usage

### Core Commands

1. **Discover:** List available MCP servers and tools
2. **Inspect:** Show a tool's full JSON schema
3. **Execute:** Run a tool and return results

### Key Features

- Supports both Stdio and HTTP
- Connection pooling (reduces repeated start-up overhead)
- Tool filtering (permit/deny lists)
- Glob searching (e.g., `mcp-cli grep "*mail*"`)
- Designed for agent integration (system instructions/skills)
- Lightweight (minimal dependencies)

## Part 3: Detailed Comparison Table

| Aspect              | Traditional MCP         | mcp-cli              |
|---------------------|------------------------|----------------------|
| Protocol            | HTTP/REST or Stdio     | Stdio/HTTP (CLI)     |
| Context Loading     | Static (upfront)       | Dynamic (on-demand)  |
| Tool Discovery      | All at once            | Lazy enumeration     |
| Schema Inspection   | Pre-loaded             | On-request           |
| Token Usage         | High (~47k for 60)     | Low (~400 for 60)    |
| Best For            | Direct integration     | AI agent use         |
| Implementation      | Server-side            | CLI-side             |
| Complexity          | Medium                 | Low                  |
| Startup Time        | One call               | Multiple (optimized) |
| Scaling             | Limited by context     | Unlimited            |
| Integration         | Custom                 | Pre-built CLI        |

## Part 4: When to Use Each Approach

**Use Traditional MCP:**

- Building direct server integrations
- Manageable group of tools
- Must have HTTP-level request/response control
- Need for synchronous, real-time calls

**Use mcp-cli:**

- Integrations with AI agents (e.g., Claude, Gemini)
- Multiple MCP servers
- Token efficiency is a key requirement
- CLI-based automation or battle-tested dynamic discovery
- Building agent skills and system instructions

## Conclusion

MCP standardizes tool discovery for AI systems, but static contexts quickly hit scale and cost barriers. **mcp-cli** offers a pragmatic, scalable way to integrate tools dynamically, ensuring token-efficient usage and easier AI agent development. Think of MCP as the protocol 'language' and mcp-cli as the 'interpreter.' For modern agent workflows, dynamic approaches with tools like mcp-cli are increasingly preferred.

## Resources

- [MCP Specification](https://modelcontextprotocol.io/)
- [mcp-cli GitHub](https://github.com/philschmid/mcp-cli)
- [MCP for Beginners](https://aka.ms/mcp-for-beginners)
- [Practical demo: AnveshMS/mcp-cli-example](https://github.com/AnveshMS/mcp-cli-example)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/mcp-vs-mcp-cli-dynamic-tool-discovery-for-token-efficient-ai/ba-p/4494272)
