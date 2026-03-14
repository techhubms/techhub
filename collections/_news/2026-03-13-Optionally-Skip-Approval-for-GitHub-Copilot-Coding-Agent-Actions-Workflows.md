---
external_url: https://github.blog/changelog/2026-03-13-optionally-skip-approval-for-copilot-coding-agent-actions-workflows
title: Optionally Skip Approval for GitHub Copilot Coding Agent Actions Workflows
author: Allison
primary_section: github-copilot
feed_name: The GitHub Blog
date: 2026-03-13 18:14:48 +00:00
tags:
- AI
- Approval Process
- Continuous Integration
- Copilot
- Copilot Coding Agent
- DevOps
- GitHub Actions
- GitHub Copilot
- Improvement
- News
- Open Source
- Pull Requests
- Repository Settings
- Workflow Automation
- Workflow Permissions
section_names:
- ai
- devops
- github-copilot
---
Allison announces a new option in GitHub that lets repository administrators skip manual approval for GitHub Actions workflows triggered by Copilot coding agent, balancing workflow speed and security.<!--excerpt_end-->

# Optionally Skip Approval for GitHub Copilot Coding Agent Actions Workflows

When the [Copilot coding agent](https://docs.github.com/copilot/concepts/agents/coding-agent/about-coding-agent) initiates a pull request or pushes changes, it's treated like an outside contributor in open source projects. By default, GitHub Actions workflows don't run on these events until manually approved by someone with the **Approve and run workflows** button. This requirement exists to minimize security risks, as workflows can access secrets and repository permissions depending on setup.

However, manual approval introduces friction to the development workflow, delaying test results and deployments when Copilot is actively contributing code.

## What's New

**A new repository setting lets administrators optionally skip the human approval step, allowing workflows started by Copilot to run immediately.** This offers flexibility for teams who prefer fast iteration and are comfortable with the risk/reward trade-off. For most projects, the default—requiring manual approval—remains unchanged to prioritize security.

For details about configuration and risk assessment, see [Configuring settings for GitHub Copilot coding agent](https://docs.github.com/copilot/how-tos/use-copilot-agents/coding-agent/configuring-agent-settings).

### Key Points

- **By Default:** Human approval is required for Copilot-triggered workflows.
- **New Option:** Administrators can enable automatic workflow runs for Copilot events.
- **Trade-off:** Faster feedback and automation, but possibly increased security exposure.
- **Documentation:** [Read more in GitHub Docs](https://docs.github.com/copilot/how-tos/use-copilot-agents/coding-agent/configuring-agent-settings).

## Security Considerations

While the option boosts developer velocity, it is critical to understand the security implications. Automated workflows may access repository secrets and perform sensitive actions, so enabling this feature should be carefully evaluated based on risk tolerance.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-03-13-optionally-skip-approval-for-copilot-coding-agent-actions-workflows)
