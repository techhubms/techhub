---
external_url: https://github.blog/changelog/2025-12-10-the-github-mcp-server-adds-support-for-tool-specific-configuration-and-more
title: GitHub MCP Server Adds Tool-Specific Configuration and Security Features
author: Allison
feed_name: The GitHub Blog
date: 2025-12-10 11:01:02 +00:00
tags:
- AI Workflows
- Content Sanitization
- Context Window
- Copilot
- GitHub
- GitHub MCP Server
- Go SDK
- Granular Permissions
- Lockdown Mode
- MCP
- Prompt Injection Mitigation
- Public Repositories
- Security Hardening
- Server Configuration
- Tool Configuration
- X MCP Tools
- AI
- DevOps
- Security
- News
section_names:
- ai
- devops
- security
primary_section: ai
---
Allison introduces new features in the GitHub MCP Server, including tool-specific configuration, improved context reduction, enhanced security via Lockdown mode, and migration to the official Go SDK for the Model Context Protocol.<!--excerpt_end-->

# GitHub MCP Server: Tool-Specific Configuration, Security, and Protocol Upgrades

**Author:** Allison

## Overview

The latest GitHub MCP Server update introduces:

- Tool-specific configuration via the `X-MCP-Tools` header
- Enhanced security with Lockdown mode and content sanitization
- Full migration to the official MCP Go SDK for improved protocol support

## Tool-Specific Configuration

Previously, toolsets in the GitHub MCP Server enabled broad groups of tools, but fine-grained customization wasn't possible. Now, using the new `X-MCP-Tools` header on the remote (and a corresponding flag for the local server), users can enable only the tools they need. This:

- Reduces context window usage
- Minimizes unnecessary tool loading
- Enables better cost efficiency and performance

**Example (Remote):**

```json
"X-MCP-Tools": "get_file_contents,pull_request_read"
```

**Example (Local):**

```bash
--tools=get_file_contents,pull_request_read
```

Mix and match tool-specific and toolset-specific configuration modes for further customization. Example use cases and further documentation are available in the [server configuration docs](https://github.com/github/github-mcp-server/blob/main/docs/server-configuration.md).

### Context Reduction Benefits

Loading just the most-used tools (3-10) in context can deliver a 60-90% reduction in context window usage vs. the default toolsets, leading to:

- Faster response times
- Lower operational costs
- More effective use of AI-powered agents

## Migration to the Official MCP Go SDK

Both local and remote GitHub MCP Servers have migrated fully to the [official Go SDK](https://github.com/modelcontextprotocol/go-sdk), keeping the server up to date with new MCP spec features. This enables benefits like resource auto-completions for owners, repo names, and file paths.

A special thanks to the [mark3labs/mcp-go](https://github.com/mark3labs/mcp-go) community, which previously powered the MCP Server.

## Security: Lockdown Mode & Content Sanitization

### Lockdown Mode

For public repositories, the new `Lockdown` mode filters content from untrusted contributors, exposing only content from those with push access. It is enabled with:

```json
"X-MCP-Lockdown": "true"
```

Certain tools will return errors or filter content when invoked by untrusted authors, bolstering protection against supply chain risks.

### Content Sanitization Against Prompt Injection

To defend against prompt injection attacks, content is now automatically sanitized:

- Invisible Unicode character filtering
- HTML sanitization (strip unsafe tags/attributes)
- Removal of unrendered markdown text in code fences

*Example:*
A malicious markdown block containing instructions embedded in code will be sanitized so only safe content (e.g., actual code) is passed to the AI model.

## Resources and Feedback

Explore the documentation and repository for more guidance:

- [GitHub MCP Server repository](https://github.com/github/github-mcp-server)
- [Server configuration docs](https://github.com/github/github-mcp-server/blob/main/docs/server-configuration.md)

If you encounter issues or have feedback, open an issue in the [GitHub MCP Server repository](https://github.com/github/github-mcp-server).

---
*These updates help developers, DevOps engineers, and AI builders to work more securely and efficiently with GitHub-integrated AI tools.*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-10-the-github-mcp-server-adds-support-for-tool-specific-configuration-and-more)
