---
layout: post
title: How to Find, Install, and Manage MCP Servers with the GitHub MCP Registry
author: Andrea Griffiths
canonical_url: https://github.blog/ai-and-ml/generative-ai/how-to-find-install-and-manage-mcp-servers-with-the-github-mcp-registry/
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/feed/
date: 2025-10-24 16:00:00 +00:00
permalink: /ai/news/How-to-Find-Install-and-Manage-MCP-Servers-with-the-GitHub-MCP-Registry
tags:
- Agentic Workflows
- AI
- AI & ML
- AI Tools
- Authentication
- Automation
- Coding
- Developer Productivity
- DevOps
- Generative AI
- GitHub
- GitHub Actions
- Governance
- MCP
- MCP Registry
- News
- OAuth
- Playwright
- Registry Allow Lists
- Server Publishing
- VS Code
section_names:
- ai
- coding
- devops
---
Andrea Griffiths explains how developers and teams can discover, install, and manage Model Context Protocol (MCP) servers using the GitHub MCP Registry, including publishing your own server and setting up governance for enterprise environments.<!--excerpt_end-->

# How to Find, Install, and Manage MCP Servers with the GitHub MCP Registry

*By Andrea Griffiths*

The GitHub MCP Registry is a centralized hub for discovering, installing, and managing Model Context Protocol (MCP) servers—a key part of modern AI-driven developer workflows. MCP servers connect tools, APIs, and workflows to your AI stack, letting coding agents like GitHub Copilot perform advanced tasks.

## What Is the MCP Registry?

Think of the MCP Registry as an app store for AI tools that your coding agent (like Copilot) can use. Some examples of MCP servers include:

- **Playwright** for browser automation
- **GitHub MCP server** (access to 100+ GitHub API tools)
- Partner servers from **Notion**, **Unity**, **MarkItDown** (Microsoft), and more

As of today, there are 44 MCP servers in the registry, and more are added regularly.

## Installing an MCP Server

Installing an MCP server is now streamlined with GitHub's registry:

1. Browse available servers at [GitHub MCP Registry](https://github.com/mcp).
2. Select your server (e.g., Playwright) and click **Install in VS Code**.
3. VS Code launches, pre-configured for the server.
4. Set optional parameters, if needed.
5. Authenticate with OAuth for seamless and secure access.

**Pro Tip:** For remote servers, OAuth-based authentication replaces manual token management. Simply approve access during setup.

## Publishing Your Own MCP Server

To add your server to the registry:

1. **Install the MCP Publisher CLI:**
   - `brew install mcp-publisher` or download the appropriate binary
2. **Initialize your server:**
   - Navigate to your project and run `mcp-publisher init` to generate a `server.json` config file.
3. **Prove Ownership:** Add MCP metadata to your package:
   - NPM: Add `"mcpName"` to `package.json`
   - PyPI/NuGet: Add `mcp-name:` to your README
   - Docker: Add a label to your Dockerfile
4. **Authenticate:**
   - GitHub namespaces use `mcp-publisher login github`; custom domains require DNS verification.
5. **Publish:**
   - Run `mcp-publisher publish`. On success, your server appears in the registry.
   - To increase visibility, email partnerships@github.com to nominate your server for listing.

### Automation with GitHub Actions

Automate publishing using a `.github/workflows/publish-mcp.yml` CI workflow:

- On each new release tag, build, test, and publish to both the package registry and MCP registry.
- MCP Publisher CLI supports GitHub authentication for secure automated publishing.

## Enterprise Management and Governance

Organizations can establish **allow lists** for MCP servers, ensuring only vetted tools are accessible in developer environments. Steps include:

1. Hosting an internal registry following the MCP API spec
2. Adding approved MCP servers
3. Connecting GitHub Enterprise settings to the registry endpoint
4. Enabling enforcement in IDEs such as VS Code

**Security Tip:** Integrate your CI/CD or security pipeline to vet servers before adding them to allow lists.

## Tips and Tricks for Power Users

- Use GitHub stars and verified orgs to assess server credibility
- Test servers locally with [MCP Inspector](https://github.com/modelcontextprotocol/inspector)
- Take advantage of agentic workflows—Copilot agent combines Playwright and GitHub MCP servers for automated pull requests and UI validation
- Semantic tool lookups in VS Code will surface only the most relevant tools in complex environments

## Roadmap: What's Next for the MCP Registry?

- **Self-publication:** Enabling even broader community contributions
- **More IDE integrations:** Expanding beyond VS Code
- **Enterprise features:** Advanced governance for regulated industries
- **Agentic workflows:** Pre-bundled flows for common tasks

## Conclusion

The GitHub MCP Registry aims to be the canonical source for managing all MCP servers. By centralizing installation, publishing, and governance, the registry streamlines the configuration of AI-driven coding environments—empowering developers and organizations to build more productive, secure, and compliant AI ecosystems.

[Explore the MCP Registry on GitHub](https://github.com/mcp?utm_source=blog-source&utm_campaign=mcp-registry-server-launch-2025)

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/ai-and-ml/generative-ai/how-to-find-install-and-manage-mcp-servers-with-the-github-mcp-registry/)
