---
tags:
- Agentic Coding
- AI
- AI & ML
- Authentication
- Branching
- Code Generation
- Command Line
- Copilot Agents
- Copilot Cloud Agent
- DevOps
- Draft Pull Request
- Generative AI
- GitHub Copilot
- GitHub Copilot CLI
- GitHub Copilot CLI For Beginners
- GitHub Education
- GitHub MCP Server
- Interactive Mode
- MCP
- News
- Node.js
- Non Interactive Mode
- npm
- Permissions
- Pull Requests
- Slash Commands
- Terminal
title: 'GitHub Copilot CLI for Beginners: Getting started with GitHub Copilot CLI'
external_url: https://github.blog/ai-and-ml/github-copilot/github-copilot-cli-for-beginners-getting-started-with-github-copilot-cli/
date: 2026-04-10 16:00:00 +00:00
author: Christopher Harrison
section_names:
- ai
- devops
- github-copilot
primary_section: github-copilot
feed_name: The GitHub Blog
---

Christopher Harrison introduces GitHub Copilot CLI with a beginner-friendly walkthrough: install via npm, authenticate, grant folder permissions, and start prompting Copilot from your terminal (including delegating work to the Copilot cloud agent).<!--excerpt_end-->

# GitHub Copilot CLI for Beginners: Getting started with GitHub Copilot CLI

Christopher Harrison’s first lesson in the “GitHub Copilot CLI for Beginners” series explains what GitHub Copilot CLI is, how it works, and how to run your first prompts from the terminal.

## What is GitHub Copilot CLI?

GitHub Copilot CLI brings Copilot’s agentic AI capabilities into your command-line interface, so you can work from the terminal with repository context.

Key idea: agents can perform tasks autonomously (for example, build code and run tests), self-correct errors, and let you iterate by reviewing their output.

## Install Copilot CLI

If you already have Node.js installed, the cross-platform installation approach is via npm:

```bash
npm install -g @github/copilot
```

You can also install via package managers like WinGet or Homebrew (check those tools’ docs for the exact commands).

## First-time setup

After installation, launch it by typing `Copilot` in your terminal.

If it’s your first time using it, authenticate:

```text
/login
```

According to the post, `/login` will:

- Tie the client to your Copilot account.
- Connect the readonly GitHub MCP server, granting access to resources on GitHub.

### Grant folder permissions

Copilot needs access to your working folder so it can explore and potentially modify files.

You can grant access either:

- For the current session only, or
- Persistently (so you don’t have to re-approve for the same project later)

## Using Copilot CLI: example prompts and workflows

Once permissions are granted, you can ask questions, request code, and delegate tasks.

### Ask for an overview of a project

```text
Give me an overview of this project
```

Copilot may explore important files and summarize findings.

### Ask for code changes (example: new endpoint)

```text
Let’s add a new endpoint to return all categories
```

Copilot will look for existing documentation/examples in the repo and try to follow established patterns. It may request permission to create files.

### Delegate work to the Copilot cloud agent

For well-defined tasks, you can delegate work from the CLI. The post says Copilot will preserve context, create a new branch, open a draft pull request, and apply changes in the background before asking you to review.

```text
/delegate Let’s deal with issue #14 to add the rest of the CRUD endpoints to games
```

## What’s next in the series

The next lesson will cover two modes:

- **Interactive mode**: have GitHub Copilot run your project locally
- **Non-interactive mode**: use the `-p` flag for quick summaries without leaving your shell context

The series also plans to cover:

- Interactive vs non-interactive modes
- Copilot CLI slash commands
- Using MCP servers with Copilot CLI

## Resources

- GitHub Copilot CLI docs: https://docs.github.com/copilot/concepts/agents/about-copilot-cli
- GitHub Copilot CLI for Beginners video series: https://www.youtube.com/playlist?list=PL0lo9MOBetEHvO-spzKBAITkkTqv4RvNl
- GitHub Copilot CLI 101: https://github.blog/ai-and-ml/github-copilot-cli-101-how-to-use-github-copilot-from-the-command-line/
- Best practices for GitHub Copilot CLI: https://docs.github.com/en/copilot/how-tos/copilot-cli/cli-best-practices
- Feature page: https://github.com/features/copilot/cli


[Read the entire article](https://github.blog/ai-and-ml/github-copilot/github-copilot-cli-for-beginners-getting-started-with-github-copilot-cli/)

