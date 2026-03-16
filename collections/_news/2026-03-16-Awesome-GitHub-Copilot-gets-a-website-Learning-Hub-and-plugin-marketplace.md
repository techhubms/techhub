---
primary_section: github-copilot
feed_name: Microsoft Blog
tags:
- Agentic Workflows
- Agents
- AI
- Awesome Copilot
- Community Contributions
- Copilot Customizations
- DevOps
- Full Text Search
- GitHub Actions
- GitHub Copilot
- GitHub Copilot CLI
- GitHub Pages
- Hooks
- Instructions
- Microsoft For Developers
- News
- Plugin Marketplace
- Plugins
- Pull Requests
- Skills
- VS Code
- VS Code Insiders
date: 2026-03-16 17:00:33 +00:00
external_url: https://devblogs.microsoft.com/blog/awesome-github-copilot-just-got-a-website-and-a-learning-hub-and-plugins
author: Matt Soucoup
section_names:
- ai
- devops
- github-copilot
title: Awesome GitHub Copilot gets a website, Learning Hub, and plugin marketplace
---

Matt Soucoup announces a major update to the Awesome GitHub Copilot Customizations project: a new website, a Learning Hub, and a plugin system to make Copilot agents, skills, instructions, and automation easier to find, install, and contribute to.<!--excerpt_end-->

## Overview

Back in July, the **Awesome GitHub Copilot Customizations** repo launched as a place for the community to share ways to customize GitHub Copilot (custom instructions, prompts, and chat modes). The project quickly outgrew a README-first format.

The repo now includes (community-contributed):

- **175+ agents**
- **208+ skills**
- **176+ instructions**
- **48+ plugins**
- **7 agentic workflows**
- **3 hooks**

As Copilot customization formats evolved (for example, prompts and chat modes changing over time), the project shifted to make discovery and usage easier.

## What’s new

### The new website

The **Awesome GitHub Copilot website** wraps the repo in a navigable site with search and category pages:

- Website: https://awesome-copilot.github.com
- Built for easier navigation and deployed on **GitHub Pages**

Key features called out:

- **Full-text search** across all resources (agents, skills, instructions, hooks, workflows, plugins), with filtering by category
- **Resource pages** per category with:
  - Live search
  - Modal previews (preview a resource before using it)
  - Direct links back to the source
  - **One-click install** into **VS Code** or **VS Code Insiders**

The GitHub repo remains the source of truth:

- Repo: https://github.com/github/awesome-copilot
- Contributions still happen through PRs; merged PRs flow through to the website.

### The Learning Hub

The **Learning Hub** aims to explain the concepts behind Copilot customization and how the different components fit together:

- Learning Hub: https://awesome-copilot.github.com/learning-hub/

It focuses on fundamentals like:

- What agents, skills, and instructions are
- How a plugin differs from a hook
- How to tailor existing examples for your own needs or write customizations from scratch

### Plugins

Plugins bundle related customization files (agents, skills, commands) into an installable package. The article describes them as themed collections for domains like frontend development, Python, Azure cloud, or team workflows.

Notable announcement:

- **Awesome GitHub Copilot** is a **default plugin marketplace** for:
  - **GitHub Copilot CLI**
  - **VS Code**

There are **48+ plugins** in the repo, and the website includes a plugin page with search and tag filters.

Install example:

```bash
copilot plugin install <plugin-name>@awesome-copilot
```

## Other changes mentioned

- **Agentic Workflows**: described as natural-language **GitHub Actions** that run AI coding agents autonomously. Examples include daily issue reports, updating CODEOWNERS files, and stale repo detection.
- **Hooks**: event-triggered automations during Copilot coding agent sessions (examples given: session logging, governance auditing, custom post-processing).
- **Skills migration**: a consolidation from the original 8 resource types down to a simpler model; **skills** are positioned as the standard unit for bundling reusable knowledge.

## How to get involved

1. Browse the site: https://awesome-copilot.github.com
2. Try installing a plugin from the plugin page
3. Work through the Learning Hub: https://awesome-copilot.github.com/learning-hub
4. Contribute via PRs (Contributing guide): https://github.com/github/awesome-copilot/blob/main/CONTRIBUTING.md
5. Star the repo: https://github.com/github/awesome-copilot


[Read the entire article](https://devblogs.microsoft.com/blog/awesome-github-copilot-just-got-a-website-and-a-learning-hub-and-plugins)

