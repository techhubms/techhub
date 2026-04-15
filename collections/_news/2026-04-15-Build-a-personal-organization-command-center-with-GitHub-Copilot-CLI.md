---
date: 2026-04-15 17:00:00 +00:00
feed_name: The GitHub Blog
tags:
- Agent Mode
- AI
- AI & ML
- Asynchronous Coding Agents
- Calendar Sync
- Command Line Tools
- Copilot Cloud Agent
- Developer Productivity
- Electron
- GitHub Copilot
- GitHub Copilot CLI
- MCP
- Microsoft 365 Data
- News
- Node.js
- Planning Workflow
- React
- Tailwind CSS
- Vite
- VS Code
- WorkIQ MCP
section_names:
- ai
- github-copilot
primary_section: github-copilot
external_url: https://github.blog/ai-and-ml/github-copilot/build-a-personal-organization-command-center-with-github-copilot-cli/
title: Build a personal organization command center with GitHub Copilot CLI
author: Cassidy Williams
---

Cassidy Williams interviews GitHub Staff Software Engineer Brittany Ellich about building a personal “command center” app, focusing on how GitHub Copilot CLI and agent-based workflows supported the process from planning through implementation.<!--excerpt_end-->

# Build a personal organization command center with GitHub Copilot CLI

What if you could remove the struggle of context switching across several apps, bringing them together into one place?

This post introduces GitHub Staff Software Engineer Brittany Ellich and the personal productivity tool she built to streamline her work. The project was inspired by the GitHub Copilot CLI and uses AI throughout the workflow—from ideation and planning through implementation.

## Q & A

### What is your role at GitHub?

Brittany Ellich is a staff software engineer on GitHub’s billing team. Her day-to-day work includes metered billing—tracking things like Actions minutes, storage amounts, and Copilot usage.

She also:

- “Dogfoods” tools coming out of the Copilot organization
- Contributes to ATProto projects
- Built Open Social for applications built on the AT Protocol

### What did you build?

She built a personal organization “command center” to address **digital fragmentation**—bringing information scattered across many apps into one calm, central space.

### How long did v1 take to make?

She uses a **plan-then-implement** workflow:

- Uses AI to help plan the system first
- Uses Copilot to implement based on that plan

For v1, this approach let her go from idea to a working tool in **a single day**, alongside regular work.

A planning technique she describes is having Copilot “interview” her—asking questions about how the system should work until the plan is clear enough to reduce guesswork during implementation.

### What’s your favorite tool stack to build with?

She describes a workflow combining synchronous and asynchronous agent work:

- **VS Code agent mode** for synchronous development
  - Typically runs up to **two non-competing agent workflows** at a time
- **Copilot Cloud Agent** for asynchronous development
  - Keeps a pipeline of tasks going (bug fixes, tech-debt changes) when well-scoped
  - Focuses her attention in VS Code on work that needs more oversight

### Do you care what tech stack your apps use now?

Not much. This was her first Electron app, and she notes it was “almost completely built by Agent Mode.”

She later simplified the repository to make it publicly accessible, which required more hands-on work. She also notes a practical limitation: agents tend to add code more readily than they remove it.

Project repo:

- [command-center-lite](https://github.com/brittanyellich/command-center-lite)

### One-line takeaway

“Go build something! Building solutions from scratch has never been easier, and it’s helpful for learning how to work with new AI tools.”

### How does she keep up with industry changes?

She keeps up via:

- Articles, podcasts, and social media
- Internally shared resources via GitHub Slack
- The GitHub blog

## Try Brittany’s approach

The post positions this project as a reminder that useful projects often start as small fixes for everyday problems.

If you want to try something similar, the tools called out include:

- [Electron](https://www.electronjs.org/): cross-platform desktop apps
- [React](https://react.dev/): UI components and state management
- [Vite](https://vite.dev/): build tooling with hot module replacement
- [Tailwind](https://tailwindcss.com/): utility-first CSS
- [WorkIQ MCP](https://github.com/microsoft/work-iq): an MCP server and CLI for accessing Microsoft 365 data
  - Background on MCP: [What the heck is MCP and why is everyone talking about it?](https://github.blog/ai-and-ml/llms/what-the-heck-is-mcp-and-why-is-everyone-talking-about-it/)

All of these are open source, and GitHub Copilot is positioned as a way to get started faster.

### Prerequisites mentioned

To run her exact solution by cloning the repo, she lists:

- Node.js (v18 or higher)
- GitHub Copilot CLI (noted as used for WorkIQ setup)
  - Docs: [Using GitHub Copilot in the command line](https://docs.github.com/copilot/using-github-copilot/using-github-copilot-in-the-command-line?utm_source=blog-command-center-cta&utm_medium=blog&utm_campaign=dev-pod-copilot-cli-2026)
- A Microsoft 365 account (for calendar sync)
- An ElevenLabs account (for voice assistant setup)

More details are in the repository README:

- [command-center-lite README](https://github.com/brittanyellich/command-center-lite/blob/main/README.md)

Related link:

- [Get started with GitHub Copilot CLI](https://github.com/features/copilot/cli?utm_source=blog-command-center-cta&utm_medium=blog&utm_campaign=dev-pod-copilot-cli-2026)


[Read the entire article](https://github.blog/ai-and-ml/github-copilot/build-a-personal-organization-command-center-with-github-copilot-cli/)

