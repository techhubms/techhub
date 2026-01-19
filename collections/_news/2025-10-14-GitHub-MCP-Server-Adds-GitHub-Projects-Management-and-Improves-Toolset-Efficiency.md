---
external_url: https://github.blog/changelog/2025-10-14-github-mcp-server-now-supports-github-projects-and-more
title: GitHub MCP Server Adds GitHub Projects Management and Improves Toolset Efficiency
author: Allison
viewing_mode: external
feed_name: The GitHub Blog
date: 2025-10-14 15:22:40 +00:00
tags:
- Copilot
- Default Toolset
- DevOps Automation
- GitHub MCP Server
- GitHub Projects
- Label Toolset
- Project Management
- Projects & Issues
- Pull Request Tools
- Server Configuration
- Tool Consolidation
- Workflow Automation
section_names:
- devops
---
Allison introduces enhancements to the GitHub MCP Server, including support for GitHub Projects management and a simplified, multifunctional toolset, making DevOps workflows more manageable for development teams.<!--excerpt_end-->

# GitHub MCP Server Adds GitHub Projects Management and Toolset Improvements

The GitHub MCP Server has rolled out a series of updates focused on giving teams more control over project automation and simplifying the available tools.

## Key Highlights

- **GitHub Projects Integration:**
  - You can now manage and automate GitHub Projects directly from the MCP Server.
  - New options allow you to list and retrieve projects, issues and pull requests, update project item fields (such as moving issues between states), and add or remove items in your project boards.
  - Note: You'll need to explicitly include the `projects` toolset in your server configuration if you want to use these features locally.

- **Footprint Reduction and Performance:**
  - The default configuration now includes a more focused set of the most-used toolsets: `context`, `repos`, `issues`, `pull_requests`, and `users`.
  - Additional or less commonly used tools can still be made available by customizing your configuration.

- **Tool Consolidation:**
  - Pull request tools have been merged into a single, unified tool: `pull_request_read`.
    - Supports multiple operations via a `method` parameter (get, get_files, get_status, get_diff, get_reviews, get_review_comments).
  - The `label` toolset lets you list, create, update, or delete labels, again using a unified tool and method argument.
  - The move toward consolidated, multifunctional tools is intended to reduce configuration bloat, clarify usage, and improve both performance and maintainability.

## How to Get Started

- For the new Projects features, update your MCP Server configuration or tool registry to include the `projects` toolset, then rebuild and restart the server.
- Detailed documentation, ongoing discussions, and feedback can be found in the [GitHub MCP Server repository](https://github.com/github/github-mcp-server) and related discussion threads.

## Additional Resources

- [Discuss using the Projects tools](https://github.com/github/github-mcp-server/discussions/1199)
- [Details about the default configuration changes](https://github.com/github/github-mcp-server/discussions/1182)

These improvements provide a more streamlined, powerful experience for teams automating and managing projects on GitHub through MCP Server.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-10-14-github-mcp-server-now-supports-github-projects-and-more)
