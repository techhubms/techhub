---
layout: "post"
title: "ACP Protocol Support Now Available in GitHub Copilot CLI"
description: "GitHub Copilot CLI introduces support for the Agent Client Protocol (ACP), enabling standardized integration with third-party tools, IDEs, and automation pipelines. This update empowers developers to orchestrate Copilot’s agentic capabilities across custom workflows, editors, and automated systems using an extensible industry protocol."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-01-28-acp-support-in-copilot-cli-is-now-in-public-preview"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-01-28 15:00:30 +00:00
permalink: "/2026-01-28-ACP-Protocol-Support-Now-Available-in-GitHub-Copilot-CLI.html"
categories: ["AI", "GitHub Copilot"]
tags: ["ACP", "Agent Client Protocol", "AI", "AI Agent Integration", "API Protocols", "CI/CD Automation", "Command Line", "Copilot", "Developer Tools", "GitHub Copilot", "GitHub Copilot CLI", "IDE Integration", "Improvement", "News", "Permission Handling", "Session Management"]
tags_normalized: ["acp", "agent client protocol", "ai", "ai agent integration", "api protocols", "cislashcd automation", "command line", "copilot", "developer tools", "github copilot", "github copilot cli", "ide integration", "improvement", "news", "permission handling", "session management"]
---

Allison shares details about the public preview of ACP support in GitHub Copilot CLI, showcasing new possibilities for integrating Copilot’s AI agent across development tools and automation workflows.<!--excerpt_end-->

# ACP Protocol Support Now Available in GitHub Copilot CLI

GitHub Copilot CLI now implements the [Agent Client Protocol (ACP)](https://zed.dev/acp), allowing standardized communication between AI agents and clients. This industry protocol paves the way for deeper integration of Copilot's agentic abilities within various developer tools and workflows.

## How It Works

- **ACP Mode Startup:**
  - Start Copilot in ACP mode over stdio: `copilot --acp`
  - Or via TCP on a given port: `copilot --acp --port 8080`

- **ACP Client Capabilities:**
  - Initialize connections and discover what the Copilot agent can do
  - Create isolated sessions with custom working directories
  - Send prompts containing text, images, and additional context
  - Receive streaming agent updates
  - Respond to permission prompts for tool execution
  - Manage operation cancellation and session lifecycle

## Use Cases

- **IDE Integrations:**
  - Bring Copilot’s capabilities to any editor or development environment supporting ACP
- **CI/CD Pipelines:**
  - Automate agentic coding tasks as part of continuous integration or deployment workflows
- **Custom Frontends:**
  - Develop tailored user experiences or specialized workflows
- **Multi-Agent Systems:**
  - Coordinate Copilot with other AI agents using a shared standard

## Resources

- [ACP documentation](https://docs.github.com/copilot)
- [GitHub Community Announcements](https://github.com/orgs/community/discussions/categories/announcements)

This public preview allows the DevOps and development community to extend, orchestrate, and deeply integrate Copilot's agentic workflows wherever they are most valuable.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-01-28-acp-support-in-copilot-cli-is-now-in-public-preview)
