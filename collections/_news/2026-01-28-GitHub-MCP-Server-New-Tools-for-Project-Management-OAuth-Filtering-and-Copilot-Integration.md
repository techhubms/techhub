---
layout: "post"
title: "GitHub MCP Server: New Tools for Project Management, OAuth Filtering, and Copilot Integration"
description: "This article presents an in-depth look at the latest GitHub MCP Server improvements, including the consolidated Projects toolset for improved project management, automatic OAuth-based tool filtering, Insiders mode for experimental features, HTTP server deployment with OAuth support, and new Copilot coding agent capabilities. Developers and enterprise teams will learn how these updates streamline GitHub Projects workflows and enhance Copilot automation."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-01-28-github-mcp-server-new-projects-tools-oauth-scope-filtering-and-new-features"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-01-28 10:23:45 +00:00
permalink: "/2026-01-28-GitHub-MCP-Server-New-Tools-for-Project-Management-OAuth-Filtering-and-Copilot-Integration.html"
categories: ["AI", "DevOps", "GitHub Copilot"]
tags: ["AI", "API Security", "Automation", "Copilot", "Copilot Coding Agent", "Copilot Integration", "DevOps", "Enterprise Server", "GitHub Copilot", "GitHub MCP Server", "GitHub Projects", "HTTP Server Mode", "Insiders Mode", "News", "OAuth", "Personal Access Token", "Project Management", "Token Scopes", "Toolset Consolidation"]
tags_normalized: ["ai", "api security", "automation", "copilot", "copilot coding agent", "copilot integration", "devops", "enterprise server", "github copilot", "github mcp server", "github projects", "http server mode", "insiders mode", "news", "oauth", "personal access token", "project management", "token scopes", "toolset consolidation"]
---

Allison explains the newest features of the GitHub MCP Server, highlighting updated project management tools, OAuth-based filtering, HTTP server support for enterprise, and advanced Copilot coding agent integration.<!--excerpt_end-->

# GitHub MCP Server: New Projects Tools, OAuth Scope Filtering, and New Features

**Author: Allison**

The GitHub MCP Server receives a significant update, introducing streamlined project management tools, enhanced security and usability through OAuth scope filtering, support for experimental features with Insiders mode, improved enterprise deployment options, and expanded Copilot agent capabilities.

## Key Features and Improvements

### 1. Consolidated GitHub Projects Toolset

- Unified `projects` toolset with commands:
  - `projects_list`: List user, organization, or repository projects.
  - `projects_get`: Retrieve detailed project info, including fields and items.
  - `projects_write`: Create, update, and manage project items fully.
- Owner type (user/org) detection is automated.
- **Significant reduction in context window use**, saving up to 23,000 tokens (approx. 50%) per operation.

### 2. OAuth Scope-Based Tool Filtering

- Tool availability is now dynamically filtered based on your token’s OAuth scopes.
  - Classic PATs (`ghp_`): Tools are hidden if your token lacks required scope.
  - Fine-grained PATs / OAuth: All tools are shown; API enforces permission.
- Prevents accidental unauthorized operations and reduces interface clutter.

#### Example Table

| Token Type        | Behavior                                 |
|------------------|------------------------------------------|
| Classic PAT      | Tools filtered by token scopes           |
| Fine-grained PAT | All tools shown; API restricts as needed |
| OAuth            | Dynamic scope challenges on demand       |

### 3. Insiders Mode for Experimental Features

- Enable experimental/preview features by opting in via a configuration header or `/insiders` URL.
- Easily toggle between stable and experimental features.
- Available for both local and remote deployments.

### 4. HTTP Server Mode with OAuth Support

- Run MCP Server as a standalone HTTP server.
- Supports per-request OAuth tokens via the Authorization header.
- Fallback to `GITHUB_PERSONAL_ACCESS_TOKEN` when necessary.
- Full support for **GitHub Enterprise Server**.
- Facilitates shared deployment for enterprise teams; no need for personal tokens per user.

### 5. Copilot Coding Agent Tools

- Integrate with GitHub Copilot coding agent using the updated MCP Server.
- New tool: `get_copilot_job_status` to monitor Copilot jobs by job ID or PR number.
- `assign_copilot_to_issue` and `create_pull_request_with_copilot`:
  - Now support `base_ref` for feature branches and stacked PRs.
  - Enable sequential Copilot automation tasks with job tracking.
  - Support for custom instructions to Copilot.

#### Getting Started with Copilot Tools

- Enable the `copilot` toolset in the MCP Server.
- Track progress, automate assignments, and streamline PR workflows with new tools.

## Resources

- [GitHub MCP Server repository](https://github.com/github/github-mcp-server)
- [Managing Personal Access Tokens](https://docs.github.com/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens)
- [GitHub Blog Post](https://github.blog/changelog/2026-01-28-github-mcp-server-new-projects-tools-oauth-scope-filtering-and-new-features)

## Feedback

Open issues or contribute via the MCP Server repository if you have suggestions or bugs to report.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-01-28-github-mcp-server-new-projects-tools-oauth-scope-filtering-and-new-features)
