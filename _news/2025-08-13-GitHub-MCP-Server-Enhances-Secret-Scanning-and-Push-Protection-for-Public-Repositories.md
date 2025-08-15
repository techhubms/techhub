---
layout: "post"
title: "GitHub MCP Server Enhances Secret Scanning and Push Protection for Public Repositories"
description: "This announcement details new security features in the GitHub MCP server, focusing on secret scanning and push protection for tool calls in public repositories. The update blocks tool calls containing exposed secrets by default, offers clear user feedback with bypass options, and introduces enhanced workflow, search, and administration tools for both local and remote MCP servers. It aims to address prompt injection threats and strengthens best practice adherence without requiring special licenses."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-08-13-github-mcp-server-secret-scanning-push-protection-and-more"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-08-13 17:31:10 +00:00
permalink: "/2025-08-13-GitHub-MCP-Server-Enhances-Secret-Scanning-and-Push-Protection-for-Public-Repositories.html"
categories: ["DevOps", "Security"]
tags: ["Agentic Workflows", "CI/CD", "Credential Protection", "DevOps", "Gist Toolset", "GitHub Actions", "GitHub MCP Server", "GraphQL", "News", "Prompt Injection", "Public Repositories", "Push Protection", "Repository Security", "Secret Scanning", "Security", "Tool Call Security", "Workflow Automation"]
tags_normalized: ["agentic workflows", "ci slash cd", "credential protection", "devops", "gist toolset", "github actions", "github mcp server", "graphql", "news", "prompt injection", "public repositories", "push protection", "repository security", "secret scanning", "security", "tool call security", "workflow automation"]
---

Allison introduces major enhancements in secret scanning and push protection for the GitHub MCP server, explaining how these features help secure public repository workflows against credential leaks and prompt injection risks.<!--excerpt_end-->

# GitHub MCP Server: Secret Scanning, Push Protection, and More

**Author:** Allison

## Overview

The GitHub MCP (Machine-Centric Platform) server now provides real-time secret scanning and push protection for all tool call inputs and outputs in public repositories. If an exposed secret is detected during a tool call, the server blocks the operation by default, providing clear details to the user and explicit bypass links for intentional overrides (except for users who have opted out of push protection, for whom bypass is unavailable).

## Included Features

- **Blocking and Bypass:** Users can block or bypass secret-related tool call blocks in MCP, with clear responses and actionable options. Accounts opted out of push protection cannot bypass the block.
- **Agent-Friendly Feedback:** The server returns clear, machine-usable responses for integrating with agent workflows.

## Security Impact: Defending Against Prompt Injection

Prompt injection attacks have exploited public content (e.g., READMEs, issues, PR comments) to trick agents into revealing credentials via tool calls. The new secret scanning mechanism inspects data flowing to and from public repositories, helping to cut off a frequent credential exfiltration path before secrets escape your control.

### Benefits

- **Addresses Major Secret-Leak Vector:** Prevents tool-call payloads with exposed secrets from being processed in public workflows (both reading and writing).
- **Reduces Prompt Injection Exposure:** Prevents prompt-injected attempts to exfiltrate tokens via tool arguments or responses.

### Availability & Licensing

- **Scope:** Only for public repository tool calls.
- **Free:** No Copilot or GitHub Secret Protection license required.
- **Private Repo Support:** Upcoming for users with GitHub Secret Protection licenses.

## Limitations

- The system cannot prevent all types of data leaks (e.g., non-secret information, model-only behaviors, or unscanned channels).
- Users should maintain strong security hygiene: use least-privilege tokens and rotate credentials regularly.

[Official documentation](https://docs.github.com/code-security/secret-scanning/working-with-secret-scanning-and-push-protection/working-with-push-protection-and-the-github-mcp-server) provides further details.

### Community

- Join community discussion and feedback in [Copilot Conversations](https://github.com/orgs/community/discussions/categories/copilot-conversations).

---

# Additional MCP Server Improvements (Remote & Local)

- **GitHub Actions Toolset:** Agents can discover, dispatch, monitor, and debug GitHub Actions workflows more effectively.
- **Gist Toolset:** Create and share snippets and artifacts without modifying repositories.
- **Sub-issues Tools:** Commands such as `add_sub_issue`, `list_sub_issues`, `remove_sub_issue`, and `reprioritize_sub_issue` streamline issue management.
- **Pull Request Workflow Upgrades:** Features like `update_pull_request` to toggle drafts and manage reviewers.
- **Org-wide Discussions:** Enhanced discussion management with richer data fields and sorting.
- **GraphQL & Pagination:** Issue listing upgraded from REST to GraphQL; robust pagination across tools.
- **Improved File Retrieval:** Better path matching, default directory handling, and SHA support in content retrieval.
- **Search Enhancements:** Separate search tools for issues, pull requests, organizations, and users; improved search parameter clarity.

Find more on all changes and improvements in the [GitHub MCP Server releases page](https://github.com/github/github-mcp-server/releases).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-13-github-mcp-server-secret-scanning-push-protection-and-more)
