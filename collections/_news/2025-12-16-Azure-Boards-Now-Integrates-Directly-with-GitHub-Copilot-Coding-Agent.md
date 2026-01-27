---
external_url: https://devblogs.microsoft.com/devops/github-copilot-for-azure-boards/
title: Azure Boards Now Integrates Directly with GitHub Copilot Coding Agent
author: Dan Hellem
feed_name: Microsoft DevOps Blog
date: 2025-12-16 13:17:08 +00:00
tags:
- Agentic AI
- Azure & Cloud
- Azure Boards
- Azure DevOps
- Branch Management
- Copilot Coding Agent
- Custom Agents
- Enterprise Integration
- Feature Rollout
- Kanban Board
- MCP Server
- Model Selection
- Pull Request Workflow
- Repository Integration
- REST API
- Work Item Automation
section_names:
- ai
- azure
- devops
- github-copilot
primary_section: github-copilot
---
Dan Hellem reports on the general availability of Azure Boards and GitHub Copilot integration, which streamlines work item automation and coding workflow for development teams.<!--excerpt_end-->

# Azure Boards Now Integrates Directly with GitHub Copilot Coding Agent

**Author:** Dan Hellem

The Azure Boards integration with GitHub Copilot, previously in private preview, is now generally available. This update allows development teams to send Azure DevOps work items directly to GitHub Copilot, streamlining automated code generation and pull request workflows.

## How It Works

- Write clear instructions in the work item's description or any large text field.
- Click the Copilot button on the work item to "Create a pull request with GitHub Copilot." Select the repository, branch, and provide any extra instructions.
- Copilot coding agent receives the work item, relevant content, and recent comments, then generates a branch and draft pull request linked back for traceability.
- Status updates on the work item and Kanban card reflect progress, with Copilot comments indicating readiness for review.
- Review and interact directly with the pull request in GitHub.

## Requirements

To use this integration:

1. Code repositories must reside in GitHub.
2. Azure Boards must be connected to the GitHub repo through Azure DevOps.
3. Copilot coding agent must be enabled on connected repos — for GitHub Copilot Enterprise/Business, admin enablement is required; for Pro/Pro+ it's enabled by default.
4. Update Azure Boards app OAuth permissions to approve new access requirements.

## New Features

- **Branch Selection:** Now choose a target branch for Copilot-generated changes (not just the default branch).
- **Copilot Status on Kanban Cards:** Kanban cards now display Copilot activity and PR readiness for faster progress tracking.

## Coming Soon

- **Agent and Model Customization:** Organizations will be able to select custom coding agents and AI models for PR generation.
- **REST API Support:** Automate work item creation and Copilot processing via API for advanced workflows (e.g., MCP Server triggered automation).

## Rollout Details

- The GA release is staggered for stability and performance, with wider access expected by mid-January due to slower holiday deployment.

For further details, see the official documentation:

- [How to enable GitHub Copilot coding agent](https://docs.github.com/en/copilot/concepts/coding-agent/enable-coding-agent)
- [Integrate Azure Boards with GitHub](https://learn.microsoft.com/en-us/azure/devops/boards/github)
- [Interact with PR in GitHub](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/coding-agent/make-changes-to-an-existing-pr)

---

*This integration streamlines development task automation and traceability, making Azure Boards and GitHub Copilot a cohesive toolset for modern DevOps.*

This post appeared first on "Microsoft DevOps Blog". [Read the entire article here](https://devblogs.microsoft.com/devops/github-copilot-for-azure-boards/)
