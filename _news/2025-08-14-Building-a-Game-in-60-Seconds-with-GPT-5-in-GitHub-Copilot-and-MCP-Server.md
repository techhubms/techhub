---
layout: "post"
title: "Building a Game in 60 Seconds with GPT-5 in GitHub Copilot and MCP Server"
description: "This article by Kedasha Kerr explores how the release of GPT-5 in GitHub Copilot and the new Model Context Protocol (MCP) server are streamlining software development. It includes hands-on examples: building a real game from a natural language prompt, automating GitHub repository creation, and issue management—all within VS Code using natural language and advanced AI. Ideal for developers looking to leverage the latest AI-driven tools, the post provides practical, step-by-step advice for enabling GPT-5 in Copilot and integrating natural language automation into everyday workflows."
author: "Kedasha Kerr"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/ai-and-ml/generative-ai/gpt-5-in-github-copilot-how-i-built-a-game-in-60-seconds/"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/feed/"
date: 2025-08-14 16:30:00 +00:00
permalink: "/2025-08-14-Building-a-Game-in-60-Seconds-with-GPT-5-in-GitHub-Copilot-and-MCP-Server.html"
categories: ["AI", "Coding", "DevOps", "GitHub Copilot"]
tags: ["AI", "AI & ML", "AI Development", "Automation", "Coding", "Copilot Integration", "Developer Workflow", "DevOps", "Game Development", "Generative AI", "GitHub Actions", "GitHub Copilot", "GPT 5", "HTML5", "Issue Automation", "JavaScript", "MCP", "MCP Server", "Natural Language Automation", "News", "OpenAI", "Programming Productivity", "Repository Management", "Spec Driven Development", "VS Code"]
tags_normalized: ["ai", "ai and ml", "ai development", "automation", "coding", "copilot integration", "developer workflow", "devops", "game development", "generative ai", "github actions", "github copilot", "gpt 5", "html5", "issue automation", "javascript", "mcp", "mcp server", "natural language automation", "news", "openai", "programming productivity", "repository management", "spec driven development", "vs code"]
---

Kedasha Kerr demonstrates how developers can rapidly build applications using GPT-5 in GitHub Copilot and automate GitHub workflows via the new MCP server, revolutionizing AI-powered coding and DevOps.<!--excerpt_end-->

# Building a Game in 60 Seconds with GPT-5 in GitHub Copilot and MCP Server

**Author:** Kedasha Kerr

## Introduction

Discover how GPT-5 and the GitHub Model Context Protocol (MCP) server are transforming the way developers build software. This post walks through live build examples that showcase AI-powered coding and natural language automation, all from within popular tools like VS Code and GitHub Copilot.

## Key Highlights

- **GPT-5 Integration:** Use OpenAI's latest model directly in GitHub Copilot across ask, edit, and agent modes in VS Code.
- **Spec-Driven Development:** Start projects by describing requirements to the AI, allowing GPT-5 to generate specs and code.
- **Lightning Fast Prototyping:** Build a Magic Tiles style game in under 60 seconds using a natural language prompt.
- **MCP Server Automation:** Automate GitHub repository management and issue creation from your IDE through conversational commands.

## Using GPT-5 in GitHub Copilot

### Enabling GPT-5

1. Open the model picker in GitHub Copilot (VS Code or your chosen IDE)
2. Select **GPT-5 (Preview)** from the options
3. Start working in ask/edit/agent modes

*Note: Enterprise and business users may need administrative approval to access GPT-5.*

### Live Example: Magic Tiles Game

- **Prompt:** Ask GPT-5 to describe Magic Tiles in MVP terms—receiving a comprehensive breakdown, data model, and checklist.
- **Build:** Use the simple prompt "Build this." GPT-5 produces a working HTML/CSS/JavaScript prototype in under a minute.
- **Iterate:** Add user instructions or modify features by conversing naturally. GPT-5 immediately updates and suggests next steps.

## Automating GitHub with MCP Server

### What is the Model Context Protocol (MCP)?

MCP is an open standard connecting AI assistants to external tools—empowering LLMs to interact with GitHub, email, databases, and more, closing the gap between development and automation.

### Setting Up MCP Server

1. Add an `.vscode/mcp.json` file:

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

2. Click "Start" in your MCP configuration.
3. Authenticate via GitHub's OAuth (supports passkeys).
4. Use Copilot's interface to access new MCP-powered tools.

### Real-World Automation Examples

- **Create a Repository:** Ask Copilot in plain English to make a new repo (e.g., "Can you create a repository for this project called teenyhost?"). Copilot handles setup, code push, and remote configuration automatically.
- **Bulk Issue Creation:** Generate and categorize issues from brainstorming, then have Copilot create them as formatted GitHub issues with titles, descriptions, and labels.

## Developer Workflow Revolutionized

- **Speed:** Real-time, context-rich AI makes rapid iteration and feature prototype possible.
- **Frictionless Automation:** Skip manual tasks like context-switching or formatting—handle more from inside your editor.
- **Human-in-the-Loop:** Developers retain control over actions (e.g., cancel pushes) while the AI manages repetitive tasks.

## Getting Started

1. Switch Copilot to GPT-5 in your IDE.
2. Try spec-driven prompts: "Describe the core requirements for [your project]."
3. Set up your workspace with `.vscode/mcp.json` and authenticate MCP.
4. Experiment with MCP-based automation:
   - Create repositories
   - Generate and triage issues
   - Explore automation for branching, PRs, and more

## What's Next

Kedasha plans to stream building a custom MCP server from scratch—showing how accessible and extensible this new automation model can be. Install MCP, give GPT-5 a try, and join the community in exploring AI-enhanced workflows.

## Resources

- [GitHub Copilot](https://github.com/features/copilot)
- [GPT-5 in GitHub Copilot](https://github.blog/ai-and-ml/generative-ai/gpt-5-in-github-copilot-how-i-built-a-game-in-60-seconds/)
- [GitHub MCP Server](https://github.com/github/github-mcp-server)
- [Model Context Protocol Explained](https://github.blog/ai-and-ml/llms/what-the-heck-is-mcp-and-why-is-everyone-talking-about-it/)

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/ai-and-ml/generative-ai/gpt-5-in-github-copilot-how-i-built-a-game-in-60-seconds/)
