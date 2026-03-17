---
date: 2026-03-17 21:00:44 +00:00
feed_name: The GitHub Blog
external_url: https://github.blog/changelog/2026-03-17-secret-scanning-in-ai-coding-agents-via-the-github-mcp-server
title: Secret scanning in AI coding agents via the GitHub MCP Server
tags:
- Agent Plugins
- AI
- AI Coding Agents
- Application Security
- Copilot Chat
- Copilot Plugins
- Credential Leaks
- DevOps
- Exposed Secrets
- GitHub Advanced Security
- GitHub Copilot
- GitHub Copilot CLI
- GitHub MCP Server
- GitHub Secret Protection
- GitHub Secret Scanning
- MCP Compatible IDEs
- Model Context Protocol (mcp)
- News
- Pre Commit Scanning
- Public Preview
- Pull Requests
- Secure Development Workflow
- Security
- VS Code
section_names:
- ai
- devops
- github-copilot
- security
primary_section: github-copilot
author: Allison
---

Allison announces a public preview feature where AI coding agents can invoke GitHub secret scanning via the GitHub MCP Server, helping developers catch exposed credentials in code changes before committing or opening a pull request.<!--excerpt_end-->

# Secret scanning in AI coding agents via the GitHub MCP Server

The GitHub MCP Server can now scan your code changes for exposed secrets before you commit or open a pull request. This is designed to prevent credential leaks by detecting secrets while you write code with MCP-compatible IDEs and AI coding agents.

This feature is available in **public preview** for repositories with **GitHub Secret Protection** enabled.

## How it works

In **MCP-enabled environments**, an AI coding agent can run secret scanning based on your prompts and instructions.

- When you ask your AI coding agent to check for secrets, it invokes the secret scanning tools on the **GitHub MCP Server**.
- Your AI coding agents send the code to the **GitHub secret scanning engine**.
- The response includes **structured results**, including the **locations** and **details** for any secrets found.

## Get started

1. Set up the **GitHub MCP Server** in your developer environment.
2. (Optional) Install the **GitHub Advanced Security** plugin for a more tailored secret scanning experience.

   Examples:

   - **GitHub Copilot CLI**: run

     ```text
     /plugin install advanced-security@copilot-plugins
     ```

   - **Visual Studio Code**:
     - Install the `advanced-security` agent plugin
     - Use `/secret-scanning` in **Copilot Chat** to start your prompt

3. Ask your agent to scan your current changes for exposed secrets before you commit.

   Notes:

   - **GitHub Copilot CLI**: enable the new tool with

     ```text
     copilot --add-github-mcp-tool run_secret_scanning
     ```

   - **Visual Studio Code**: in Copilot Chat, use `/secret-scanning` followed by your prompt

### Example prompt

```text
Scan my current changes for exposed secrets and show me the files and lines I should update before I commit.
```

## Learn more

- [GitHub secret scanning](https://github.com/features/security#secret-scanning)
- [GitHub MCP Server](https://github.com/github/github-mcp-server)
- [Set up the GitHub MCP Server](https://docs.github.com/copilot/how-tos/provide-context/use-mcp/set-up-the-github-mcp-server)
- [Find and install plugins in GitHub Copilot CLI](https://docs.github.com/copilot/how-tos/copilot-cli/customize-copilot/plugins-finding-installing)
- [Install agent plugins in Visual Studio Code](https://code.visualstudio.com/docs/copilot/customization/agent-plugins)


[Read the entire article](https://github.blog/changelog/2026-03-17-secret-scanning-in-ai-coding-agents-via-the-github-mcp-server)

