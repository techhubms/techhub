---
layout: "post"
title: "Remote GitHub MCP Server Now Generally Available"
description: "This announcement covers the general availability of the remote GitHub MCP Server, introducing OAuth-based authentication, expanded AI-powered tooling like the Copilot Coding Agent, upgraded security features such as secret and code scanning, and enhanced workflow management for developers and teams using GitHub. The update focuses on improving integration, security, and usability for customers leveraging Copilot IDEs and MCP workflows."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-09-04-remote-github-mcp-server-is-now-generally-available"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-09-04 15:00:49 +00:00
permalink: "/2025-09-04-Remote-GitHub-MCP-Server-Now-Generally-Available.html"
categories: ["AI", "DevOps", "GitHub Copilot", "Security"]
tags: ["AI", "AI Workflow", "Authentication", "Code Scanning", "Copilot Coding Agent", "Cursor", "DevOps", "DevOps Tools", "Eclipse", "GHAS", "GitHub Copilot", "GitHub MCP Server", "JetBrains", "News", "OAuth 2.1", "PKCE", "Pull Request Workflow", "Secret Scanning", "Security", "Security Advisories", "Security Guardrails", "Sub Issue Management", "VS", "VS Code", "Xcode"]
tags_normalized: ["ai", "ai workflow", "authentication", "code scanning", "copilot coding agent", "cursor", "devops", "devops tools", "eclipse", "ghas", "github copilot", "github mcp server", "jetbrains", "news", "oauth 2dot1", "pkce", "pull request workflow", "secret scanning", "security", "security advisories", "security guardrails", "sub issue management", "vs", "vs code", "xcode"]
---

Allison details the general availability of the remote GitHub MCP Server, highlighting powerful integrations with Copilot, AI-driven automation, advanced authentication, and enhanced security features for developers and teams.<!--excerpt_end-->

# Remote GitHub MCP Server Now Generally Available

The GitHub MCP (Machine-Centric Programming) Server has reached general availability, bringing a host of new features designed to strengthen developer workflows, boost automation, and enhance security in GitHub environments.

## Key Improvements & Updates

### 🔐 OAuth-Based Production Authentication

- **OAuth 2.1 + PKCE Support:** Integrated with major IDEs (VS Code, Visual Studio, JetBrains, Eclipse, Xcode, Cursor).
- **More Secure Alternative to PATs:** Offers automatic token refresh and short-lived credentials.
- **Centralized MCP Access Policy:** One policy manages MCP access across all Copilot environments. The previous "Copilot editor preview policy" no longer controls this setting.

### 🚀 Premium, AI-Powered Tools

- **Copilot Coding Agent:** Enables autonomous development tasks for MCP workflows—automatically fixing bugs, implementing features, creating branches, running tests, and opening pull requests without manual intervention.
- **Secret Scanning with Push Protection:** Now standard for all public repositories to detect and block secrets in tool call inputs. Soon, this will extend to private GHAS-enabled repos.
- **Integrated Code Scanning Alerts:** Seamless access to security findings and early vulnerability detection in development pipelines.

### 🛠️ Enhanced Workflow & Collaboration Tools

- **Security Advisories:** Granular support at organization, repository, and global levels.
- **Sub-Issue Management:** Add, list, remove, and reprioritize tasks within issues.
- **Pull Request Workflow:** Features like draft toggling and improved reviewer requests.
- **Gists & Discussions:** Improved support for collaborative and knowledge-sharing features.
- **Session & Pagination:** Advanced controls for managing sessions and large data sets.

## Why It Matters

The MCP Server empowers AI-powered assistants (like GitHub Copilot, Claude Code, or Cursor) to interact directly with developers' GitHub data and automate key workflows. By reducing context switching, it allows agents to search repositories, manage issues, review pull requests, and more—driving productivity for developer teams.

## Quick Start

- [Install the GitHub MCP Server](https://github.com/github/github-mcp-server/)
- [Official Documentation](https://docs.github.com/copilot/how-tos/provide-context/use-mcp/use-the-github-mcp-server)
- [Join Community Discussion](https://github.com/github/github-mcp-server/discussions)

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-04-remote-github-mcp-server-is-now-generally-available)
