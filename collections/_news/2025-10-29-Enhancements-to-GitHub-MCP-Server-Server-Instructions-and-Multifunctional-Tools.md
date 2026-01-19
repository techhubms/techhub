---
external_url: https://github.blog/changelog/2025-10-29-github-mcp-server-now-comes-with-server-instructions-better-tools-and-more
title: 'Enhancements to GitHub MCP Server: Server Instructions and Multifunctional Tools'
author: Allison
viewing_mode: external
feed_name: The GitHub Blog
date: 2025-10-29 09:07:43 +00:00
tags:
- AI Assisted Development
- Code Security
- Copilot
- Default Toolset
- DevOps Automation
- GitHub MCP Server
- Issue Management
- MCP
- Multitool Workflows
- Pull Request Review
- Repository Configuration
- Server Instructions
- Tool Consolidation
section_names:
- ai
- devops
---
Allison reports on the latest GitHub MCP Server update, detailing new server instructions for guiding models and the consolidation of tools to support more efficient developer workflows.<!--excerpt_end-->

# Enhancements to GitHub MCP Server: Server Instructions and Multifunctional Tools

The GitHub MCP Server now features server instructions designed to guide AI models when interacting with the server. These instructions allow for precise multitool workflows, such as ensuring specific tools are used in a particular order (e.g., reviewing a pull request with a designated tool sequence), improving how models manage tasks like reviews, issue triage, and discussions.

## Key Improvements

### 1. Server Instructions for Models

- **What it is:** Follows the Model Context Protocol, letting you define a system prompt that guides an AI model's use of the MCP server.
- **Benefits:** Models can now follow workflow conventions, tool dependencies, and best practices (e.g., always paginate results, or sequence tool usage).

### 2. Tool Consolidation

- **Why:** Reduces footprint and simplifies both server configuration and AI model reasoning.
- **How:** Multiple related tools have been merged into unified, more powerful multifunctional tools. Each now uses a `method` parameter to support various actions, such as:
  - For pull requests: `pull_request_review_write` (covering creation, submission, deletion of reviews)
  - For issues: `issue_read` (fetching issues, comments, labels, or sub-issues) and `issue_write` (creating or updating issues)
  - For sub-issues: `sub_issue_write` (adding, reprioritizing, or removing sub-issues)

### 3. Easier MCP Server Configuration

- Toolsets can now be configured using the `default` keyword, streamlining the process compared to manual listing.
- Example: Add a toolset like `code_security` by simply specifying `default,code_security` in your server configuration.
- The `default` toolset contains essential toolsets, including context, repos, issues, pull requests, and user information.

## Impact on Workflows

- AI models will reliably follow specified development and review processes.
- DevOps automation via the MCP Server is simplified, leading to leaner configurations, faster operations, and more robust developer tooling.
- The system is poised for continued improvement, with user feedback guiding further refinements.

## Learn More

- Visit the [GitHub MCP Server repository](https://github.com/github/github-mcp-server) for documentation and updates.
- Feedback is encouraged to help GitHub evolve these capabilities for developer needs.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-10-29-github-mcp-server-now-comes-with-server-instructions-better-tools-and-more)
