---
layout: "post"
title: "GitHub Copilot Coding Agent Now Supports .instructions.md Custom Instructions"
description: "GitHub Copilot coding agent introduces support for multiple .instructions.md files, allowing for fine-grained, context-sensitive custom instructions. This enhances automation and code quality, providing developers with better ways to guide Copilot’s behavior at the repository or directory level."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-07-23-github-copilot-coding-agent-now-supports-instructions-md-custom-instructions"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/label/copilot/feed/"
date: 2025-07-23 16:11:38 +00:00
permalink: "/news/2025-07-23-GitHub-Copilot-Coding-Agent-Now-Supports-instructionsmd-Custom-Instructions.html"
categories: ["AI", "GitHub Copilot"]
tags: [".instructions.md", "AI", "Automation", "Copilot Coding Agent", "Custom Instructions", "Enterprise", "GitHub CLI", "GitHub Copilot", "IDE Integration", "News", "Pro+", "Pull Requests", "Repository Automation", "Task Delegation", "YAML Frontmatter"]
tags_normalized: ["dotinstructionsdotmd", "ai", "automation", "copilot coding agent", "custom instructions", "enterprise", "github cli", "github copilot", "ide integration", "news", "proplus", "pull requests", "repository automation", "task delegation", "yaml frontmatter"]
---

Authored by Allison, this article introduces new capabilities for GitHub Copilot coding agent with support for multiple .instructions.md files, enabling tailored, context-aware custom instructions for improved automation and code quality.<!--excerpt_end-->

## GitHub Copilot Coding Agent: Now Supports .instructions.md Custom Instructions

**Author:** Allison

### Overview

GitHub Copilot coding agent allows developers to offload coding tasks that can be processed in the background, thereby enabling them to focus on other work. The latest update brings support for new ways to provide repository instructions to Copilot coding agent.

### New Custom Instructions Support

Previously, developers could use a `.github/copilot-instructions.md` file to supply Copilot coding agent with information about repository structure, build steps, automated testing, and linters, aiming to produce higher-quality pull requests with less manual oversight.

**What’s New:**

- The Copilot coding agent now supports `.instructions.md` files located under `.github/instructions`.
- Multiple `.instructions.md` files are supported, each capable of applying to different files or directories using YAML frontmatter.

### Benefits

- **Granular Customization:** You can provide different instruction files for various directories or types of files in your repository, allowing Copilot to receive tailored context.
- **Higher-Quality Pull Requests:** Detailed, context-specific instructions help Copilot produce better, more relevant code and pull requests.
- **Improved Automation:** Task delegation is enhanced as Copilot becomes more aware of nuanced or varied build, test, and lint requirements across the repository.

### How It Works

- Place one or more `.instructions.md` files under `.github/instructions`.
- Use YAML frontmatter at the start of each file to designate which directories or files the instructions apply to.
- Copilot coding agent will read these instructions and adjust its automated work on your behalf.

For further guidance on adding repository custom instructions, see [GitHub documentation on custom instructions](https://docs.github.com/enterprise-cloud@latest/copilot/how-tos/custom-instructions/adding-repository-custom-instructions-for-github-copilot).

### Availability

- **Public Preview:** Copilot coding agent is available for users with Copilot Pro, Copilot Pro+, Copilot Business, or Copilot Enterprise (with admin-enabled policy).

### Broad Platform Integration

- Copilot coding agent can be used from github.com, in many IDEs, via GitHub Mobile, the GitHub CLI, and the GitHub MCP Server.
- Enables seamless delegation and task management from wherever you interact with your codebase.

### Learn More

- To explore best practices and deeper usage patterns for Copilot coding agent, refer to the [documentation](https://docs.github.com/enterprise-cloud@latest/copilot/how-tos/agents/copilot-coding-agent/best-practices-for-using-copilot-to-work-on-tasks).
- More details about enabling Copilot coding agent for your organization or repositories are also provided in the official [Copilot coding agent documentation](https://docs.github.com/enterprise-cloud@latest/copilot/how-tos/agents/copilot-coding-agent).

---

*This update provides developers with increased flexibility and precision when using GitHub Copilot’s background automation, helping deliver better code quality, improved CI/CD outcomes, and a more productive development workflow.*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-07-23-github-copilot-coding-agent-now-supports-instructions-md-custom-instructions)
