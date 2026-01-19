---
layout: post
title: 'Azure Boards Integration with GitHub Copilot: Private Preview Announced'
author: Dan Hellem
canonical_url: https://devblogs.microsoft.com/devops/azure-boards-integration-with-github-copilot-private-preview/
viewing_mode: external
feed_name: Microsoft DevOps Blog
feed_url: https://devblogs.microsoft.com/devops/feed/
date: 2025-09-16 11:45:27 +00:00
permalink: /github-copilot/news/Azure-Boards-Integration-with-GitHub-Copilot-Private-Preview-Announced
tags:
- Agile
- Azure Boards
- Azure Boards App
- Azure DevOps
- Code Review
- Copilot Coding Agent
- DevOps Workflow
- Feature Preview
- GitHub Integration
- Integration
- Private Preview
- Pull Request Automation
- Requirements Management
- Task Automation
- Work Item Automation
section_names:
- ai
- azure
- devops
- github-copilot
---
Dan Hellem shares an update on the new integration allowing Azure Boards work items to be handled by GitHub Copilot's coding agent, automating code changes and pull requests for Azure DevOps users.<!--excerpt_end-->

# Azure Boards Integration with GitHub Copilot (Private Preview)

**Author:** Dan Hellem

## Overview

Azure DevOps users can now leverage GitHub Copilot's coding agent from within Azure Boards. This feature lets you assign work items to Copilot, which then independently works on tasks, generates code changes, opens pull requests, and notifies you when ready for review. This expands on GitHub's previous Copilot coding agent integration for Issues, making the workflow available to Azure DevOps teams through tight Boards and GitHub repository integration.

## How the Integration Works

- Create or update a work item in Azure Boards with clear instructions in the description field.
- Click the Copilot button to initiate a pull request using GitHub Copilot.
- Select your GitHub repository and add supplemental instructions if needed.
- The related fields, including descriptions and recent comments, are sent to Copilot.
- Copilot works in the background, creates a new branch, drafts a pull request, and links it back to the work item for traceability.
- When completed, the work item displays the updated status, and a notification is posted in the discussion with the pull request ready for review.
- Review and iterate on the pull request as you would with any regular teammate.

## Key Use Cases

Copilot can help with:

- Fixing bugs
- Adding incremental features
- Improving test coverage
- Updating documentation
- Reducing technical debt

## Requirements

To access this integration, you’ll need the following:

1. **Azure Boards and GitHub Integration enabled.** Learn how at [Microsoft Docs](https://learn.microsoft.com/en-us/azure/devops/boards/github).
2. **Copilot coding agent enabled on the target GitHub repo.**
   - Copilot Business/Enterprise: Admin must enable it.
   - Copilot Pro/Pro+: Enabled by default.
   - [How to enable agent](https://docs.github.com/en/copilot/concepts/coding-agent/enable-coding-agent)
3. **Private preview enrollment.** Access must be requested by emailing the DevOps team.
4. **Update permissions.** The Azure Boards app within GitHub must be updated to approve new permissions to enable communication with Copilot.

## Signing Up for the Private Preview

- Get approval from your org admin.
- Email the feature team your organization name.
- Await confirmation that the feature is enabled for your org.

## Known Limitations and Next Steps

- Currently available for most work item types, but future updates may limit it to requirement/task backlogs.
- Integration for GitHub Enterprise Cloud with Data Residency is not yet finalized.
- The feature is under active development, with more enhancements planned over upcoming sprints.

## Feedback and Support

Report issues or send feedback via the [Developer Community site](https://developercommunity.visualstudio.com/AzureDevOps/report), referencing “Azure Boards Coding Agent.”

---

Azure Boards integration with GitHub Copilot coding agent streamlines developer workflows, automates code changes, and brings AI-driven automation into DevOps and project management processes for Azure DevOps teams. Organizations interested in participating should follow the private preview signup steps for early access.

This post appeared first on "Microsoft DevOps Blog". [Read the entire article here](https://devblogs.microsoft.com/devops/azure-boards-integration-with-github-copilot-private-preview/)
