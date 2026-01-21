---
external_url: https://github.blog/ai-and-ml/github-copilot-cli-101-how-to-use-github-copilot-from-the-command-line/
title: 'GitHub Copilot CLI 101: How to use GitHub Copilot from the command line'
author: Alexandra Lietzke
feed_name: The GitHub Blog
date: 2025-11-06 20:30:00 +00:00
tags:
- Agentic Workflows
- AI & ML
- AI Powered Development
- Code Generation
- Command Line Interface
- Developer Tools
- Generative AI
- GitHub Copilot CLI
- Issue Management
- LLMs
- MCP Integration
- Node.js
- npm
- Project Setup
- Pull Requests
- Script Refactoring
- Security Best Practices
- Terminal Productivity
- Workflow Automation
section_names:
- ai
- coding
- github-copilot
---
Alexandra Lietzke provides an in-depth walkthrough of GitHub Copilot CLI, detailing how developers can leverage AI assistance directly in the terminal for common coding tasks, with practical installation steps and usage scenarios.<!--excerpt_end-->

# GitHub Copilot CLI 101: How to use GitHub Copilot from the command line

## Introduction

GitHub Copilot CLI brings Copilot's AI coding capabilities straight into the terminal, allowing developers to generate scripts, refactor code, ask questions, and automate workflows without switching contexts. If your development flow centers around your terminal, this tool can support tasks ranging from code queries to project management—all from the command line.

## What Is GitHub Copilot CLI?

The Copilot CLI is a command-line interface for interacting with Copilot using natural language. It extends traditional terminal workflows, letting developers communicate with Copilot for suggestions, explanations, and commands. Instead of manual script-writing or command searching, Copilot CLI automates tasks and offers advice within your coding environment.

## Key Features

- **Interactive and programmatic modes:** Use Copilot in a session for conversational, iterative task refinement, or pass one-off prompts for quick assistance.
- **Command approval workflow:** Before modifying or executing files, Copilot CLI prompts users for confirmation. Approval can be granted for a session or specific tasks for safe environment control.
- **Security controls:** You must designate trusted folders; Copilot CLI will not interact with your codebase unless you confirm access. Tool approvals are granular to avoid accidental destructive actions.

## Installation and Setup

To get started, you need:

- An active GitHub Copilot subscription (Pro, Pro+, Business, or Enterprise)
- Node.js version 22+
- npm version 10+
- (Optional) Organizational Copilot CLI policy enabled by your admin, if applicable

**Installation command:**

```bash
npm install -g @github/copilot
```

After installing, choose your project directory, trust the folder when prompted, and log in to GitHub with `/login` if requested.

## Usage Modes

- **Interactive mode:** Engage in a chat-like session with Copilot, iterating on tasks directly in the terminal.
- **Programmatic mode:** Pass a prompt via flags like `-p` or `--prompt` for immediate responses inline.

Tool actions affecting files or the environment require approval each time, unless you grant persistent trust. Security implications for commands like `rm` are highlighted to avoid unintended file deletions.

## Example Prompts & Use Cases

- **Debugging & refactoring:**
  - "Create a bash script to check for uncommitted changes and push if clean"
  - "Explain each of these scripts and offer improvements"
  - "Upgrade all npm dependencies to their latest safe versions"
- **Issue and PR management:**
  - "Create an issue for adding GitHub Copilot instructions"
  - "Create a pull request with the changes we have made"
  - "Show the content of issue #4"
  - "What pull requests are attached to this issue?"
- **MCP server integration:**
  - "Using the Microsoft Learn MCP server, list all GitHub Copilot Microsoft Learn modules"
  - "Create a README with all the Microsoft Learn Copilot modules and skills"
- **Project prototyping:**
  - "Use create-next-app with Tailwind CSS to build a Next.js dashboard"
  - "Provide commands to set up a Python virtual environment and install requirements.txt"
- **Documentation generation:**
  - "Review the project README to make it easier for newcomers"
- **Environment setup & maintenance:**
  - "Generate a bash script to clone a GitHub repo, install dependencies, and start the dev server"
  - "Suggest commands to build and run a Docker container"
- **Test coverage:**
  - "Generate a command to scaffold new Jest test suites for uncovered components"
  - "Suggest steps to add integration tests for API endpoints using Supertest"

## Security Considerations

Copilot CLI enforces session-based and folder-based trust before interacting with your codebase. Always ensure you approve actions thoughtfully, especially for commands with side effects like deleting or modifying files.

## Resources

- [Docs: Getting started with Copilot CLI](https://docs.github.com/en/copilot/concepts/agents/about-copilot-cli)
- [Responsible use guidelines](https://docs.github.com/en/enterprise-cloud@latest/copilot/responsible-use/copilot-cli)
- [CLI demo from GitHub Universe](https://www.youtube.com/watch?v=82jciNezDMY)

## Conclusion

GitHub Copilot CLI is designed to empower developers through instant AI assistance within their natural workflow. Explore the CLI for improved automation, faster troubleshooting, and efficient project management—all while maintaining security through controlled approvals.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/ai-and-ml/github-copilot-cli-101-how-to-use-github-copilot-from-the-command-line/)
