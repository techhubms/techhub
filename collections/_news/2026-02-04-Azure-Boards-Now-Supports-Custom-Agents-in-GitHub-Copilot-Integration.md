---
external_url: https://devblogs.microsoft.com/devops/azure-boards-integration-with-github-copilot-includes-custom-agent-support/
title: Azure Boards Now Supports Custom Agents in GitHub Copilot Integration
author: Dan Hellem
primary_section: github-copilot
feed_name: Microsoft DevOps Blog
date: 2026-02-04 18:33:03 +00:00
tags:
- .agent.md
- Agent Profiles
- AI
- Azure
- Azure & Cloud
- Azure Boards
- Azure DevOps
- Copilot Coding Agent
- Custom Agents
- DevOps
- DevOps Workflow
- Documentation Automation
- GitHub
- GitHub Copilot
- News
- Pull Request Automation
- Repository Management
- VS Code
section_names:
- ai
- azure
- devops
- github-copilot
---
Dan Hellem announces the launch of custom agent support in the GitHub Copilot Coding Agent for Azure Boards, detailing how developers can standardize workflows and automate coding tasks within DevOps processes.<!--excerpt_end-->

# Azure Boards Now Supports Custom Agents in GitHub Copilot Integration

**Author: Dan Hellem**

The GitHub Copilot Coding Agent for Azure Boards now supports custom agents, enabling teams to automate and standardize their coding workflows directly within Azure DevOps environments.

## What Are Custom Agents?

Custom agents are specialized configurations of the Copilot Coding Agent tailored to your team's workflows, coding conventions, or project needs. Defined using Markdown-based agent profiles, these agents:

- Follow specific prompts, tools, and coding behaviors
- Consistently apply your team's standards
- Can be reused across projects or entire organizations

**Example:**
A `readme-creator` agent specializes in updating and structuring README files according to best practices (project overviews, installation, usage, contributing sections, descriptive links, etc.).

More details: [About Custom Agents](https://docs.github.com/en/copilot/concepts/agents/coding-agent/about-custom-agents)

## Creating a Custom Agent

To set up a custom agent:

1. Create a `.agent.md` profile file in your repository (commonly within `.github/agents/`).
2. Specify prompts, tool access, and desired behaviors in Markdown.
3. Commit the file and merge it into your repo. For organization-wide agents, use a `.github-private` repository.

Organizational owners can standardize agent profiles across multiple repositories, streamlining common workflows and reducing configuration duplication.

- [How to create custom agents](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/coding-agent/create-custom-agents)
- [Custom agents in VS Code](https://code.visualstudio.com/docs/copilot/customization/custom-agents)

## Using Custom Agents in Azure DevOps

- Once set up, custom agents automatically become available in Azure DevOps.
- When creating a pull request from a work item, an agent dropdown appears.
- Select your custom agent and click **Create**. The agent then generates code changes and opens a pull request in the chosen repository.

![Agent selection screenshot](https://devblogs.microsoft.com/devops/wp-content/uploads/sites/6/2026/02/boards-custom-agent-2.webp)

**Rollout Notice:** It may take 3-4 weeks for this feature to be available to all Azure DevOps organizations.

This post appeared first on "Microsoft DevOps Blog". [Read the entire article here](https://devblogs.microsoft.com/devops/azure-boards-integration-with-github-copilot-includes-custom-agent-support/)
