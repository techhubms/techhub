---
external_url: https://code.visualstudio.com/blogs/2025/07/17/copilot-coding-agent
title: Command GitHub's Coding Agent from VS Code
author: Burke Holland
viewing_mode: external
feed_name: Visual Studio Code Releases
date: 2025-07-17 00:00:00 +00:00
tags:
- AI Developer Agent
- Autonomous Development
- Code Review Automation
- Codebase Integration
- Coding Agent
- Dev Environment
- GitHub
- GitHub Pull Requests Extension
- MCP
- MCP Servers
- PR Workflow
- Session Management
- VS Code
section_names:
- ai
- coding
- github-copilot
---
Burke Holland explains how the GitHub Copilot Coding Agent is now integrated in VS Code, letting developers assign issues, review PRs, and automate code tasks directly from the editor.<!--excerpt_end-->

# Command GitHub's Coding Agent from VS Code

*By Burke Holland, July 17, 2025*

The GitHub Copilot Coding Agent now brings AI-driven automation directly into Visual Studio Code, offering developers the ability to delegate complex workflow tasks right inside their editor. Rather than just one synchronous agent, you can run multiple agents concurrently to tackle code reviews, implement features, open pull requests, and more—all as if you had a squad of AI teammates at hand.

## What Is the Copilot Coding Agent?

The GitHub Copilot Coding Agent is an autonomous developer bot. You can assign issues to it; the agent explores the codebase in an isolated environment, makes changes, builds and tests code, and iteratively updates PRs based on your feedback. It acts as a contributor to your repository and supports integrations with Model Context Protocol (MCP) tools for database and cloud service work.

**How it works:**

1. Enable the agent in GitHub settings.
2. Assign issues to the Copilot Coding Agent directly from VS Code using the [GitHub Pull Requests extension](https://marketplace.visualstudio.com/items?itemName=GitHub.vscode-pull-request-github).
3. The agent performs the coding work, opens a pull request, and updates you with its status.
4. You review the PR inside VS Code, provide feedback, and the agent iterates until completion.

## VS Code Integration Features

- **Assign Issues From the Sidebar:** Select and assign an issue to Copilot with a few clicks, all within VS Code.
- **Track Agent Progress:** The 'Copilot on My Behalf' view tracks what the agent's working on and provides real-time updates. You can terminate agent sessions if needed.
- **PR Live Preview:** Integrated support with services like Azure Static Web Apps, Vercel, and Netlify lets you preview changes live.
- **Agent Limitations and Configuration:** If the agent cannot perform some actions (like database changes), it will propose scripts for manual review. If you set up an appropriate MCP server for your database, the agent can be given broader access.
- **Copilot Chat Integration:** Users can hand off tasks directly from the Copilot Chat panel. The agent receives all context from the current chat and documents its steps as a to-do list in the PR description.

## Configuration Example

To enable the Coding Agent UI integration, add the following setting in VS Code:

```json
"githubPullRequests.codingAgent.uiIntegration": true,
```

## Current Limitations and Roadmap

- PR performance and rendering improvements
- Integrated chat view for agent sessions
- A central command center for Copilot agents in VS Code
- Enhanced documentation and the ability to share custom instructions between agent and VS Code
- Ongoing progress tracked in the [Coding Agent iteration plan](https://github.com/microsoft/vscode/issues/255483)

## Feedback and Next Steps

Try this integration today by installing the GitHub Pull Requests extension and configuring the above setting. The VS Code team welcomes feedback in [issues on the VS Code repo](https://github.com/microsoft/vscode/issues).

Stay tuned to [release notes](https://code.visualstudio.com/updates) and the [Copilot documentation](https://docs.github.com/en/copilot/concepts/about-copilot-coding-agent) for updates.

---
*Author: Burke Holland*

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/blogs/2025/07/17/copilot-coding-agent)
