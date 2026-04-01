---
primary_section: github-copilot
feed_name: The GitHub Blog
external_url: https://github.blog/changelog/2026-03-25-github-copilot-for-jira-public-preview-enhancements
author: Allison
tags:
- AI
- Atlassian Marketplace
- Atlassian MCP Server
- Branch Naming
- Collaboration Tools
- Configuration Troubleshooting
- Confluence
- Copilot
- DevOps
- Documentation
- Error Messages
- GitHub Copilot
- GitHub Copilot Coding Agent
- GitHub Copilot For Jira
- Improvement
- Jira Integration
- Jira Ticket References
- MCP
- Model Selection
- News
- Onboarding
- Personal Access Token
- Public Preview
- Pull Requests
- Setup Guidance
- Supported AI Models
title: GitHub Copilot for Jira — Public preview enhancements
section_names:
- ai
- devops
- github-copilot
date: 2026-03-25 21:09:35 +00:00
---

Allison shares the latest public preview improvements to the GitHub Copilot coding agent integration for Jira, including better setup guidance, in-Jira model selection, Jira ticket references in PRs, and adding Confluence context via MCP.<!--excerpt_end-->

# GitHub Copilot for Jira — Public preview enhancements

Since launching the [public preview](https://github.blog/changelog/2026-03-05-github-copilot-coding-agent-for-jira-is-now-in-public-preview/) of **GitHub Copilot coding agent for Jira**, GitHub has been incorporating customer feedback and shipping updates.

## What’s new

## Improved onboarding and setup guidance

The integration now includes improved guidance for early adopters:

- Clearer error messages
- Steps to resolve common configuration issues
- Expanded documentation for prerequisites and setup

See: [Integrate Copilot coding agent with Jira](https://docs.github.com/copilot/how-tos/use-copilot-agents/coding-agent/integrate-coding-agent-with-jira)

## Model selection (from Jira)

You can now choose which AI model Copilot coding agent uses for a task **directly from Jira**.

- Mention `@GitHub Copilot` in a Jira comment
- Indicate the model you want the agent to use in that comment

More details: [Changing the AI model](https://docs.github.com/copilot/how-tos/use-copilot-agents/coding-agent/changing-the-ai-model)

## Jira ticket references in pull requests

Copilot coding agent is now instructed to include your Jira ticket number in:

- The pull request title
- The branch name

Additionally, the pull request will include:

- A link back to the originating Jira ticket
- The Jira context provided to the agent

This is intended to improve traceability from Jira → PR → codebase.

## Confluence context via MCP

You can provide Copilot coding agent access to Confluence pages by configuring the **Atlassian MCP server** with a **personal access token (PAT)**. This enables the agent to reference Confluence content (for example, design docs and specs) when working on Jira issues.

Setup: [Extending Copilot coding agent with MCP](https://docs.github.com/copilot/customizing-copilot/extending-copilot-coding-agent-with-mcp)

## Getting started

To receive these updates, update to the latest version of the **GitHub Copilot for Jira** app in your Atlassian environment:

- Atlassian support guidance: [Update an installed app](https://support.atlassian.com/organization-administration/docs/managing-an-installed-app/#:~:text=is%20installed%20on.-,Update%20an%20app,-App%20developers%20may)
- [Atlassian Marketplace listing](https://marketplace.atlassian.com/apps/1582455624)
- [Documentation: integrating Copilot coding agent with Jira](https://docs.github.com/copilot/how-tos/use-copilot-agents/coding-agent/integrate-coding-agent-with-jira)
- [Share feedback / access support](https://www.surveymonkey.com/r/CCAforJira)


[Read the entire article](https://github.blog/changelog/2026-03-25-github-copilot-for-jira-public-preview-enhancements)

