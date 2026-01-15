---
layout: post
title: 'The Complete MCP Experience: Full Specification Support in VS Code'
author: Harald Kirschner
canonical_url: https://code.visualstudio.com/blogs/2025/06/12/full-mcp-spec-support
viewing_mode: external
feed_name: Visual Studio Code Releases
feed_url: https://code.visualstudio.com/feed.xml
date: 2025-06-12 00:00:00 +00:00
permalink: /ai/news/The-Complete-MCP-Experience-Full-Specification-Support-in-VS-Code
tags:
- AI
- AI Agent Integration
- Authorization
- Coding
- Developer Tools
- DevOps
- GitHub MCP Server
- Identity Providers
- MCP
- Microsoft
- News
- OAuth
- Prompts
- Remote Servers
- Resources
- Sampling
- VS Code
- Workspace Automation
section_names:
- ai
- coding
- devops
---
Harald Kirschner and collaborators present the complete integration of the Model Context Protocol specification in VS Code, detailing new authorization, prompts, and resource management features that empower AI agent workflows for developers.<!--excerpt_end-->

# The Complete MCP Experience: Full Specification Support in VS Code

By Harald Kirschner, Connor Peet, and Tyler Leonhardt

Published: June 12, 2025

Visual Studio Code now fully supports the [Model Context Protocol (MCP) specification](https://modelcontextprotocol.io/), enabling developers to leverage advanced AI agent workflows directly within their code editor. This release marks a significant milestone in delivering a standard foundation for building and integrating AI agents in modern development environments.

## Key Features of the MCP Integration

- **Authorization**: The MCP authorization specification represents a major leap, letting developers use existing identity providers (such as Microsoft, Anthropic, Okta/Auth0, Stytch, Descope) for authentication. This standard allows developers to delegate authentication, rather than implementing bespoke OAuth flows, strengthening security and streamlining user experience. Learn more from [Den Delimarsky's overview](https://den.dev/blog/new-mcp-authorization-spec/).
- **Remote Server Support**: With streamable HTTP transport and authorization, you can now connect to remote MCP servers that are secure and scalable. For example, the [GitHub MCP Server](https://github.blog/changelog/2025-06-12-remote-github-mcp-server-is-now-available-in-public-preview/) integrates with VS Code's authentication and enables seamless repository and issue management.
- **Prompts**: Beyond traditional tool integrations, MCP now supports context-aware, dynamic prompts. Servers can tailor automated workflows to individual projects and workspaces, and developers can invoke these via slash commands (e.g., `/mcp.servername.promptname`) within VS Code.
- **Resources**: MCP resources represent interactable semantic data such as images, logs, or files. Developers can manipulate these directly in their workspace — for example, annotating screenshots or handling streaming logs.
- **Sampling**: With the sampling capability, MCP servers can make controlled language model requests using the developer's existing model subscriptions. This approach simplifies multi-agent orchestration and advanced reasoning without separate AI SDKs or credentials.

## Security-First Design

The new specification isolates MCP servers as resource providers and delegates authorization to standard identity infrastructure, increasing both enterprise sophistication and developer productivity.

## Getting Started

- Explore the [VS Code MCP documentation](https://code.visualstudio.com/docs/copilot/chat/mcp-servers).
- Try the [GitHub MCP server](https://github.blog/changelog/2025-06-12-remote-github-mcp-server-is-now-available-in-public-preview/) for a production example of remote server integration and secure authentication.
- Browse the [official MCP server repository](https://github.com/modelcontextprotocol/servers) or review the [MCP specification](https://modelcontextprotocol.io/) for technical details.

This milestone expands what's possible for AI agent development in editor environments and sets a new foundation for building secure, scalable, and sophisticated AI-driven tools.

*Happy Coding!*

---

**References**

- [Den Delimarsky — New MCP Authorization Spec Overview](https://den.dev/blog/new-mcp-authorization-spec/)
- [Model Context Protocol Documentation](https://modelcontextprotocol.io/docs/)
- [VS Code MCP Servers Guide](https://code.visualstudio.com/docs/copilot/chat/mcp-servers)

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/blogs/2025/06/12/full-mcp-spec-support)
