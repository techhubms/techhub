---
layout: "post"
title: "GPT-5 in GitHub Copilot: Building a Game and Automating Workflows in Minutes"
description: "This post explores how the integration of GPT-5 into GitHub Copilot and the introduction of the GitHub Model Context Protocol (MCP) server are transforming developer workflows. The author demonstrates, with real code examples, how developers can leverage these tools for rapid prototyping, natural language automation, and streamlined project management directly within their IDE."
author: "Kedasha Kerr"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/ai-and-ml/generative-ai/gpt-5-in-github-copilot-how-i-built-a-game-in-60-seconds/"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/feed/"
date: 2025-08-14 16:30:00 +00:00
permalink: "/2025-08-14-GPT-5-in-GitHub-Copilot-Building-a-Game-and-Automating-Workflows-in-Minutes.html"
categories: ["AI", "Coding", "DevOps", "GitHub Copilot"]
tags: ["AI", "AI & ML", "AI Pair Programming", "Coding", "Developer Productivity", "DevOps", "Game Development", "Generative AI", "GitHub Automation", "GitHub Copilot", "GPT 5", "Issue Management", "Live Coding", "MCP Server", "Model Context Protocol", "Natural Language Automation", "News", "OpenAI", "Repository Creation", "Spec Driven Development", "Visual Studio Code"]
tags_normalized: ["ai", "ai ml", "ai pair programming", "coding", "developer productivity", "devops", "game development", "generative ai", "github automation", "github copilot", "gpt 5", "issue management", "live coding", "mcp server", "model context protocol", "natural language automation", "news", "openai", "repository creation", "spec driven development", "visual studio code"]
---

Kedasha Kerr demonstrates how developers can use GPT-5 in GitHub Copilot, along with the new MCP server, to rapidly prototype apps and automate GitHub project workflows using natural language right in the IDE.<!--excerpt_end-->

# GPT-5 in GitHub Copilot: Building a Game and Automating Workflows in Minutes

**Author:** Kedasha Kerr

## Overview

With the rollout of GPT-5 and the new GitHub MCP (Model Context Protocol) server, developers can now boost their productivity with AI-powered assistance and seamless automation, all from within their coding environment.

This article walks through real-world examples of using GPT-5 in GitHub Copilot, from generating game code live to automating repository and issue management using natural language commands.

---

## GPT-5 in GitHub Copilot: A New Frontier

GPT-5, OpenAI's latest large language model, is now available in GitHub Copilot. Developers can access it directly from their IDE (such as VS Code) in multiple interaction modes (ask, edit, agent) for enhanced code generation, reasoning, and rapid development cycles. Key benefits include:

- **Faster and more context-aware responses** in code completion and chat
- **Broad access across Copilot interaction modes**
- **Improved quality** in generated code and documentation

**How to enable GPT-5 in Copilot:**

1. Open the model picker in your Copilot interface
2. Select "GPT-5" from the list
3. Start creating with advanced AI capabilities

> _Note: Enterprise users may need admin approval to access GPT-5._

---

## Live Example: Building a Magic Tiles Game in Under 60 Seconds

The demonstration focused on building a simple "Magic Tiles" game end-to-end:

1. **Spec-driven Approach**: Instead of beginning with code, ask Copilot (with GPT-5) for a minimal product spec:
   - _Prompt_: "Do you know the game Magic Tiles? Can you describe the game in simple MVP terms? No auth, just core functionality."
2. **Rapid Prototyping**: Use another simple prompt:
   - _Prompt_: "Build this."
3. **Result:**
   - Copilot generated HTML/CSS/JavaScript code for a working prototype within a minute.
   - Further improvements (like user instructions) were implemented via follow-up prompts.

---

## GitHub MCP Server: Connecting AI to Your Developer Tools

The GitHub MCP server introduces a standardized protocol for letting AI tools perform actions across your toolchain, such as:

- Creating and managing repositories
- Pushing code
- Creating/managing issues
- Integrating with services like Gmail or SQL servers

**Setting it up:**

1. Create a `.vscode/mcp.json` config in your workspace:

   ```json
   {
     "servers": {
       "github": {
         "command": "npx",
         "args": ["-y", "@github/mcp-server-github"]
       }
     }
   }
   ```

2. Authenticate with GitHub using standard OAuth (with passkey support)
3. Access new Copilot options for natural language workflow automation

---

## Automating Real Development Workflows

### Example 1: Creating a Repository with Natural Language

- _Prompt_: "Can you create a repository for this project called teenyhost?"
- Copilot (via MCP) handles repo creation, remote setup, and code push.

### Example 2: Generating Issues from Conversational Prompts

- _Prompt_: "What additional features and improvements can I implement in this app?"
- Copilot provides categorized suggestions.
- _Prompt_: "Can you create issues for all the low effort improvements in this repo?"
- Result: Five well-formatted GitHub issues generated automatically.

### Why It Matters

- **Speed and Flow**: Immediate feedback and project changes without leaving your IDE
- **Frictionless Management**: No more context switching for common repository tasks
- **Human-in-the-Loop**: AI suggests/initiates actions, but you stay in control

---

## How to Get Started

1. **Experiment with GPT-5:**
    - Open Copilot, switch to GPT-5, and try agent mode for complex builds
    - Use the "spec-driven" workflow for structured results
2. **Set Up the MCP Server:**
    - Add `.vscode/mcp.json` as shown above
    - Authenticate and explore new Copilot actions
    - Automate repo creation, issue management, and more with natural language

**Tip:** Start small—try automating one manual workflow, like pushing new side projects or generating issues from a brainstorm.

---

## Looking Ahead

The combination of GPT-5 and the GitHub MCP server marks a major evolution in how developers interact with their tools—moving towards conversational, intent-driven development and workflow automation.

Next steps include building custom MCP servers and exploring deeper integrations across the development stack. The tools are already available; it’s just a matter of exploring and sharing your own workflows with the community.

> Ready to supercharge your development? [Get started with GitHub Copilot](https://github.com/features/copilot) and the [MCP server](https://github.com/github/github-mcp-server) today.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/ai-and-ml/generative-ai/gpt-5-in-github-copilot-how-i-built-a-game-in-60-seconds/)
