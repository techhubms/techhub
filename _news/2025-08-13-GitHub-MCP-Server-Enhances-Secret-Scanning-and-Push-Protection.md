---
layout: "post"
title: "GitHub MCP Server Enhances Secret Scanning and Push Protection"
description: "This news update covers the latest improvements to the GitHub MCP server, focusing on enhanced secret scanning capabilities, push protection against secret leaks in public repositories, and new toolsets for workflow automation and code security. The update details key benefits for developers, upcoming features, and best practices for secure use of GitHub's agent-powered CI/CD tools."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-08-13-github-mcp-server-secret-scanning-push-protection-and-more"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-08-13 17:31:10 +00:00
permalink: "/2025-08-13-GitHub-MCP-Server-Enhances-Secret-Scanning-and-Push-Protection.html"
categories: ["DevOps", "Security"]
tags: ["Agentic Workflows", "Best Practices", "CI/CD", "Credentials Management", "DevOps", "DevOps Automation", "File Retrieval", "Gist Toolset", "GitHub Actions", "GitHub MCP Server", "GraphQL", "News", "Prompt Injection", "Public Repository Security", "Pull Requests", "Push Protection", "Remote Tool Calls", "Secret Scanning", "Security", "Token Security"]
tags_normalized: ["agentic workflows", "best practices", "ci slash cd", "credentials management", "devops", "devops automation", "file retrieval", "gist toolset", "github actions", "github mcp server", "graphql", "news", "prompt injection", "public repository security", "pull requests", "push protection", "remote tool calls", "secret scanning", "security", "token security"]
---

Allison highlights significant enhancements to the GitHub MCP server, focusing on secret scanning, push protection, and new workflow automation features—all aimed at improving security and developer experience in public repositories.<!--excerpt_end-->

# GitHub MCP Server Enhances Secret Scanning and Push Protection

**Author:** Allison  

The GitHub MCP server introduces major improvements that strengthen security and productivity for developers working with public repositories. These updates are integral to safer remote tool call workflows and automation.

## Key Features

- **Comprehensive Secret Scanning**: All remote GitHub MCP tool call inputs in public repositories are now scanned for secrets. If a secret is detected, the call is blocked by default with clear details. Users may bypass the block if permitted unless they’ve opted out of push protection, in which case bypass is unavailable.
- **Prompt Injection Protection**: By inspecting tool call data moving to and from public repositories, the MCP server preemptively blocks attempts to leak secrets—such as via prompt-injected instructions in READMEs, issues, or PRs—before credentials leave your control.
- **Agent-Friendly Feedback**: The server delivers clear, actionable responses for automation agents, facilitating robust CI/CD pipelines.

## Workflow Automation and Toolset Enhancements

- **GitHub Actions Toolset**: Allows agent-driven workflow management. Agents can discover and dispatch workflow runs, monitor status, and tail logs for faster CI/CD feedback and effective debugging.
- **Gist Toolset**: Offers streamlined sharing of code snippets and artifacts without interacting with a repository.
- **Sub-Issue Tools**: Manage sub-issues with new capabilities like `add_sub_issue`, `list_sub_issues`, `remove_sub_issue`, and `reprioritize_sub_issue`.
- **Pull Request Upgrades**: Enable toggling draft status and requesting reviewers using `update_pull_request`.
- **Rich Discussion Fields**: Organization-scope discussions now support advanced sorting and richer metadata.
- **Improved Search Tools**: Clearly separates search tools for issues, pull requests, organizations, users, and code, with improved clarity and parameter tuning.
- **File Retrieval**: Enhanced path matching, directory defaults, and the ability to retrieve files by SHA provide more reliable file access for automation scenarios.

## Security and Availability

- **Scope**: The new secret scanning and push protection features apply to all public repository tool calls. A GitHub Secret Protection license will soon extend these safeguards to private repositories.
- **Licensing**: Free for all users; no Copilot or extra licensing is required at this time.
- **Limitations**: While secret scanning blocks a major leak vector, it does not prevent all data exfiltration routes (e.g., model-only behaviors, non-secret leaks, unscanned channels). Continued adherence to security practices such as using least-privileged tokens and rotating credentials is recommended.

## Additional Resources

- [Feature documentation](https://docs.github.com/code-security/secret-scanning/working-with-secret-scanning-and-push-protection/working-with-push-protection-and-the-github-mcp-server)
- [GitHub MCP Server releases](https://github.com/github/github-mcp-server/releases)
- [Join the GitHub community discussion](https://github.com/orgs/community/discussions/categories/copilot-conversations)

## Summary

These updates reinforce GitHub's commitment to secure workflows and practical automation for developers. The MCP server now offers stronger protections, smarter tooling, and more transparent feedback—enhancing both security posture and developer productivity.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-13-github-mcp-server-secret-scanning-push-protection-and-more)
