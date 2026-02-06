---
external_url: https://devblogs.microsoft.com/visualstudio/mcp-is-now-generally-available-in-visual-studio/
title: Model Context Protocol (MCP) Is Now Generally Available in Visual Studio
author: Allie Barry
feed_name: Microsoft DevBlog
date: 2025-08-19 18:23:21 +00:00
tags:
- Agent Mode
- Agents
- AI Agents
- Authentication
- Code
- Copilot
- Copilot Chat
- Developer Workflow Automation
- Enterprise Integration
- External Tools
- GitHub
- GitHub Policy
- IDE
- LLM
- MCP
- Microsoft
- Secure Endpoints
- Server Integration
- Servers
- VS
- AI
- DevOps
- GitHub Copilot
- Security
- News
- .NET
section_names:
- ai
- dotnet
- devops
- github-copilot
- security
primary_section: github-copilot
---
Allie Barry introduces the general availability of Model Context Protocol (MCP) in Visual Studio, highlighting how developers can integrate AI agents, like Copilot, for richer automation and secure connections to tools and services.<!--excerpt_end-->

# Model Context Protocol (MCP) Is Now Generally Available in Visual Studio

**Author:** Allie Barry

Visual Studio has added general availability support for Model Context Protocol (MCP), an open standard for integrating AI agents with external tools, databases, APIs, and services. This enables developers to unlock richer, real-time context inside their coding environments and to leverage AI tools like Copilot Chat for deeper automation and smarter assistance.

## What Is MCP?

MCP (Model Context Protocol) acts like HTTP for AI-enabled developer workflows. Rather than building separate custom integrations for each tool, MCP allows connections to robust server endpoints for databases, deployment pipelines, code search, and other resources using a consistent protocol. It makes it easier for AI agents to interact with your complete development stack.

## Key Features of MCP in Visual Studio

### Custom Workflows

- Use custom or community MCP servers to automate repetitive tasks, query metrics, interact with databases, or call internal APIs directly from Copilot Chat.

### Enterprise Integration

- Integrate AI agents with your company’s internal tools and maintain data security with first-class authentication and access control.

### Smarter Copilot Interactions

- Provide Copilot with structured access to project-specific knowledge and workflows for richer responses and more relevant help.

### Full Client Integration

- Connect Visual Studio to local or remote MCP servers via `.mcp.json` configs.
- Refer to [official documentation](https://learn.microsoft.com/en-us/visualstudio/ide/mcp-servers?view=vs-2022) for configuration instructions.

### Authentication Support

- Visual Studio now manages sign-in to protected endpoints, supporting the full MCP authentication spec, so you can connect to protected resources regardless of authentication provider.

### One-Click Server Installation

- New UI flows in Visual Studio let you install and add servers with just one click—watch for “Install in VS” buttons in repositories for faster setup.
- The new Add Server flow eliminates manual steps by letting users add MCP servers via the tooling interface, including Copilot Chat's server picker window.

### Enterprise Governance

- Integration with GitHub policy provides IT admins fine-grained control over which users can access MCP features in their organizations.

## Getting Started

- [Try MCP in Visual Studio](https://visualstudio.microsoft.com/)
- Visit the [documentation page](https://learn.microsoft.com/en-us/visualstudio/ide/mcp-servers?view=vs-2022) for server setup details.
- Provide feedback through the [developer community](https://developercommunity.visualstudio.com/home).

## Summary

MCP general availability in Visual Studio makes it easier for developers and enterprises to take advantage of intelligent, AI-powered workflows through open, secure protocols and deep Copilot integration in the IDE.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/visualstudio/mcp-is-now-generally-available-in-visual-studio/)
